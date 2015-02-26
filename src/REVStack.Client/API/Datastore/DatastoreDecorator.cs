using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Datastore
{
    public abstract class DatastoreDecorator : IDatastore
    {
        private readonly IDatastore _datastore;

        protected IDatastore Datastore
        {
            get { return _datastore; }
        }

        protected DatastoreDecorator(IDatastore datastore)
        {
            _datastore = datastore;
        }

        //public virtual object Create(object entity)
        //{
        //    return this.Create(entity, false);
        //}

        //public virtual object Create(object entity, bool fullName)
        //{
        //    return Datastore.Create(entity);
        //}

        public virtual T Create<T>(T entity) where T : new()
        {
            return this.Create<T>(entity, false);
        }

        public virtual T Create<T>(T entity, bool fullName) where T : new()
        {
            return Datastore.Create<T>(entity);
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            return this.Update<T>(entity, false);
        }

        public virtual T Update<T>(T entity, bool fullName) where T : new()
        {
            return Datastore.Update<T>(entity);
        }

        public virtual void Delete(string id)
        {
            Datastore.Delete(id);
        }

        public virtual void Command(string query)
        {
            Datastore.Command(query);
        }

        public virtual T Get<T>(string id, bool fullName) where T : new()
        {
            return Datastore.Get<T>(id, false);
        }

        public virtual T Get<T>(string id) where T : new()
        {
            return Datastore.Get<T>(id);
        }

        public virtual RevStack.Client.API.Query.Query<T> CreateQuery<T>(object[] args) where T : new()
        {
            return Datastore.CreateQuery<T>(args);
        }

        public virtual IQueryable<T> SqlQuery<T>(string query, object[] args) where T : new()
        {
            return Datastore.SqlQuery<T>(query, args);
        }

        public virtual PagedDataSource<T> Paginate<T>(string query, int page, int limit, object[] args) where T : new()
        {
            return Datastore.Paginate<T>(query, page, limit, args);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Datastore.GetValidations();
        }
    }

}
