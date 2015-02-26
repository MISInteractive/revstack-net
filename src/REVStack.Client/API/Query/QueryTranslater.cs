using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Query
{
    internal class QueryTranslator : ExpressionVisitorBase
    {
        StringBuilder sb;
        ParameterExpression row;
        ColumnProjection projection;

        internal QueryTranslator()
        {
        }

        //internal string Translate(Expression expression)
        //{
        //    this.sb = new StringBuilder();
        //    this.Visit(expression);
        //    return this.sb.ToString();
        //}

        internal TranslateResult Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.row = Expression.Parameter(typeof(ProjectionRow), "row");
            this.Visit(expression);
            return new TranslateResult
            {
                CommandText = this.sb.ToString(),
                Projector = this.projection != null ? Expression.Lambda(this.projection.Selector, this.row) : null
            };
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
           
            if (m.Method.DeclaringType == typeof(Queryable) || m.Method.DeclaringType == typeof(System.Linq.Enumerable))
            {
                if (m.Method.Name == "Where")
                {
                    //sb.Append("SELECT * FROM (");
                    this.Visit(m.Arguments[0]);
                    sb.Append(" WHERE ");
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    this.Visit(lambda.Body);
                    return m;
                }
                else if (m.Method.Name == "Single" || m.Method.Name == "SingleOrDefault")
                {
                    this.Visit(m.Arguments[0]);
                    sb.Append(" WHERE ");
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    this.Visit(lambda.Body);
                    //this.Visit(m.Arguments[1]);
                    sb.Append(" LIMIT 1 ");
                    return m;
                }
                else if (m.Method.Name == "FirstOrDefault")
                {
                    //this.Visit(m.Arguments[0]);
                    //sb.Append(" WHERE ");
                    //if (m.Arguments.Count > 1)
                    //{
                    //    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    //    this.Visit(lambda.Body);
                    //}
                    sb.Append(" LIMIT 1 ");
                    return m;
                }
                else if (m.Method.Name == "Select")
                {
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    ColumnProjection projection = new ColumnProjector().ProjectColumns(lambda.Body, this.row);
                    sb.Append("SELECT ");
                    sb.Append(projection.Columns);
                    sb.Append(" FROM (");
                    this.Visit(m.Arguments[0]);
                    sb.Append(") ");
                    this.projection = projection;
                    return m;
                }
            }
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            //Expression operand = this.Visit(u.Operand);
            //if (operand != u.Operand)
            //{
            //    return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
            //}

            if (u.NodeType == ExpressionType.ArrayLength)
            {
                Expression expression = this.Visit(u.Operand);
                //translate arraylength into normal member expression
                return Expression.MakeMemberAccess(expression, expression.Type.GetRuntimeProperty("Length"));
            }
            else if (u.NodeType == ExpressionType.Convert)
            {
                return base.Visit(u.Operand);
            }
            else
            {
                return u.Update(this.Visit(u.Operand));
            }

            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
                sb.Append("SELECT * FROM ");
                sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            } //there must be a better way...
            else if (m.Expression != null && m.Expression.NodeType == ExpressionType.Constant) 
            {
                object v = GetValue(m);
                //handle strings...
                if (v.GetType() == typeof(string))
                    v = "\'" + v.ToString() + "\'";
                sb.Append(v);
                return m;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        public static object GetValue(Expression member)
        {
            return Expression.Lambda(member).Compile().DynamicInvoke();
        }
    }

    //public class QueryTranslator : ExpressionVisitor
    //{
    //    StringBuilder sb;

    //    internal QueryTranslator()
    //    {
    //    }

    //    internal string Translate(Expression expression)
    //    {
    //        this.sb = new StringBuilder();
    //        this.Visit(expression);
    //        return this.sb.ToString();
    //    }

    //    //private static Expression StripQuotes(Expression e)
    //    //{
    //    //    while (e.NodeType == ExpressionType.Quote)
    //    //    {
    //    //        e = ((UnaryExpression)e).Operand;
    //    //    }
    //    //    return e;
    //    //}

    //    //protected override Expression VisitMethodCall(MethodCallExpression m)
    //    //{
    //    //    if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
    //    //    {
    //    //        sb.Append("SELECT * FROM (");
    //    //        this.Visit(m.Arguments[0]);
    //    //        sb.Append(") AS T WHERE ");
    //    //        LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
    //    //        this.Visit(lambda.Body);
    //    //        return m;
    //    //    }
    //    //    throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
    //    //}

    //    private static LambdaExpression GetLambda(Expression e)
    //    {
    //        while (e.NodeType == ExpressionType.Quote)
    //        {
    //            e = ((UnaryExpression)e).Operand;
    //        }
    //        if (e.NodeType == ExpressionType.Constant)
    //        {
    //            return ((ConstantExpression)e).Value as LambdaExpression;
    //        }
            
    //        return e as LambdaExpression;
    //    }

    //    private Expression BindWhere(Type resultType, Expression source, LambdaExpression predicate)
    //    {
    //        this.Visit(source);
    //        sb.Append(" WHERE ");
    //        //LambdaExpression lambda = (LambdaExpression)StripQuotes(source);
    //        this.Visit(predicate.Body);
    //        return source;
    //    }

    //    protected override Expression VisitMethodCall(MethodCallExpression m)
    //    {
    //        if (m.Method.DeclaringType == typeof(Queryable) || m.Method.DeclaringType == typeof(Enumerable))
    //        {
    //            sb.Append("SELECT * FROM {0}");

    //            switch (m.Method.Name)
    //            {
    //                case "Where":
    //                    return this.BindWhere(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]));
    //                //case "Select":
    //                //    return this.BindSelect(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]));
    //                //case "SelectMany":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindSelectMany(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]), null);
    //                //    }
    //                //    else if (m.Arguments.Count == 3)
    //                //    {
    //                //        return this.BindSelectMany(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]), GetLambda(m.Arguments[2]));
    //                //    }
    //                //    break;
    //                //case "Join":
    //                //    return this.BindJoin(m.Type, m.Arguments[0], m.Arguments[1], GetLambda(m.Arguments[2]), GetLambda(m.Arguments[3]), GetLambda(m.Arguments[4]));
    //                //case "GroupJoin":
    //                //    if (m.Arguments.Count == 5)
    //                //    {
    //                //        return this.BindGroupJoin(m.Method, m.Arguments[0], m.Arguments[1], GetLambda(m.Arguments[2]), GetLambda(m.Arguments[3]), GetLambda(m.Arguments[4]));
    //                //    }
    //                //    break;
    //                //case "OrderBy":
    //                //    return this.BindOrderBy(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]), OrderType.Ascending);
    //                //case "OrderByDescending":
    //                //    return this.BindOrderBy(m.Type, m.Arguments[0], GetLambda(m.Arguments[1]), OrderType.Descending);
    //                //case "ThenBy":
    //                //    return this.BindThenBy(m.Arguments[0], GetLambda(m.Arguments[1]), OrderType.Ascending);
    //                //case "ThenByDescending":
    //                //    return this.BindThenBy(m.Arguments[0], GetLambda(m.Arguments[1]), OrderType.Descending);
    //                //case "GroupBy":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindGroupBy(m.Arguments[0], GetLambda(m.Arguments[1]), null, null);
    //                //    }
    //                //    else if (m.Arguments.Count == 3)
    //                //    {
    //                //        LambdaExpression lambda1 = GetLambda(m.Arguments[1]);
    //                //        LambdaExpression lambda2 = GetLambda(m.Arguments[2]);
    //                //        if (lambda2.Parameters.Count == 1)
    //                //        {
    //                //            // second lambda is element selector
    //                //            return this.BindGroupBy(m.Arguments[0], lambda1, lambda2, null);
    //                //        }
    //                //        else if (lambda2.Parameters.Count == 2)
    //                //        {
    //                //            // second lambda is result selector
    //                //            return this.BindGroupBy(m.Arguments[0], lambda1, null, lambda2);
    //                //        }
    //                //    }
    //                //    else if (m.Arguments.Count == 4)
    //                //    {
    //                //        return this.BindGroupBy(m.Arguments[0], GetLambda(m.Arguments[1]), GetLambda(m.Arguments[2]), GetLambda(m.Arguments[3]));
    //                //    }
    //                //    break;
    //                //case "Distinct":
    //                //    if (m.Arguments.Count == 1)
    //                //    {
    //                //        return this.BindDistinct(m.Arguments[0]);
    //                //    }
    //                //    break;
    //                //case "Skip":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindSkip(m.Arguments[0], m.Arguments[1]);
    //                //    }
    //                //    break;
    //                //case "Take":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindTake(m.Arguments[0], m.Arguments[1]);
    //                //    }
    //                //    break;
    //                //case "First":
    //                //case "FirstOrDefault":
    //                //case "Single":
    //                //case "SingleOrDefault":
    //                //case "Last":
    //                //case "LastOrDefault":
    //                //    if (m.Arguments.Count == 1)
    //                //    {
    //                //        return this.BindFirst(m.Arguments[0], null, m.Method.Name, m == this.root);
    //                //    }
    //                //    else if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindFirst(m.Arguments[0], GetLambda(m.Arguments[1]), m.Method.Name, m == this.root);
    //                //    }
    //                //    break;
    //                //case "Any":
    //                //    if (m.Arguments.Count == 1)
    //                //    {
    //                //        return this.BindAnyAll(m.Arguments[0], m.Method, null, m == this.root);
    //                //    }
    //                //    else if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindAnyAll(m.Arguments[0], m.Method, GetLambda(m.Arguments[1]), m == this.root);
    //                //    }
    //                //    break;
    //                //case "All":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindAnyAll(m.Arguments[0], m.Method, GetLambda(m.Arguments[1]), m == this.root);
    //                //    }
    //                //    break;
    //                //case "Contains":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindContains(m.Arguments[0], m.Arguments[1], m == this.root);
    //                //    }
    //                //    break;
    //                //case "Cast":
    //                //    if (m.Arguments.Count == 1)
    //                //    {
    //                //        return this.BindCast(m.Arguments[0], m.Method.GetGenericArguments()[0]);
    //                //    }
    //                //    break;
    //                //case "Reverse":
    //                //    return this.BindReverse(m.Arguments[0]);
    //                //case "Intersect":
    //                //case "Except":
    //                //    if (m.Arguments.Count == 2)
    //                //    {
    //                //        return this.BindIntersect(m.Arguments[0], m.Arguments[1], m.Method.Name == "Except");
    //                //    }
    //                //    break;

    //                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
    //            }
    //        }
    //        else if (typeof(Updatable).IsAssignableFrom(m.Method.DeclaringType))
    //        {
    //            //IEntityTable upd = this.batchUpd != null
    //            //    ? this.batchUpd
    //            //    : (IEntityTable)((ConstantExpression)m.Arguments[0]).Value;

    //            //switch (m.Method.Name)
    //            //{
    //            //    case "Insert":
    //            //        return this.BindInsert(
    //            //            upd,
    //            //            m.Arguments[1],
    //            //            m.Arguments.Count > 2 ? GetLambda(m.Arguments[2]) : null
    //            //            );
    //            //    case "Update":
    //            //        return this.BindUpdate(
    //            //            upd,
    //            //            m.Arguments[1],
    //            //            m.Arguments.Count > 2 ? GetLambda(m.Arguments[2]) : null,
    //            //            m.Arguments.Count > 3 ? GetLambda(m.Arguments[3]) : null
    //            //            );
    //            //    case "InsertOrUpdate":
    //            //        return this.BindInsertOrUpdate(
    //            //            upd,
    //            //            m.Arguments[1],
    //            //            m.Arguments.Count > 2 ? GetLambda(m.Arguments[2]) : null,
    //            //            m.Arguments.Count > 3 ? GetLambda(m.Arguments[3]) : null
    //            //            );
    //            //    case "Delete":
    //            //        if (m.Arguments.Count == 2 && GetLambda(m.Arguments[1]) != null)
    //            //        {
    //            //            return this.BindDelete(upd, null, GetLambda(m.Arguments[1]));
    //            //        }
    //            //        return this.BindDelete(
    //            //            upd,
    //            //            m.Arguments[1],
    //            //            m.Arguments.Count > 2 ? GetLambda(m.Arguments[2]) : null
    //            //            );
    //            //    case "Batch":
    //            //        return this.BindBatch(
    //            //            upd,
    //            //            m.Arguments[1],
    //            //            GetLambda(m.Arguments[2]),
    //            //            m.Arguments.Count > 3 ? m.Arguments[3] : Expression.Constant(50),
    //            //            m.Arguments.Count > 4 ? m.Arguments[4] : Expression.Constant(false)
    //            //            );
    //            //}
    //        }
            
    //        return base.VisitMethodCall(m);
    //    }

    //    protected override Expression VisitUnary(UnaryExpression u)
    //    {
    //        switch (u.NodeType)
    //        {
    //            case ExpressionType.Not:
    //                sb.Append(" NOT ");
    //                this.Visit(u.Operand);
    //                break;
    //            default:
    //                throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
    //        }
    //        return u;
    //    }
    //}
}