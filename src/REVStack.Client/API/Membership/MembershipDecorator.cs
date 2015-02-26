using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Membership
{
    public abstract class MembershipDecorator : IMembership
    {
        private readonly IMembership _membership;

        protected IMembership Membership
        {
            get { return _membership; }
        }

        protected MembershipDecorator(IMembership membership)
        {
            _membership = membership;
        }

        public virtual T Create<T>(T entity) where T : new()
        {
            return Membership.Create<T>(entity);
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            return Membership.Update<T>(entity);
        }

        public virtual void Delete(string userName)
        {
            Membership.Delete(userName);
        }

        public virtual T Get<T>(string userName) where T : new()
        {
            return Membership.Get<T>(userName);
        }

        public virtual IQueryable<T> Get<T>() where T : new()
        {
            return Membership.Get<T>();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Membership.GetValidations();
        }


        public virtual void AddUserToRole(string userName, string roleName)
        {
            Membership.AddUserToRole(userName, roleName);
        }

        public virtual void RemoveUserFromRole(string userName, string roleName)
        {
            Membership.RemoveUserFromRole(userName, roleName);
        }

        public virtual void ResetPassword(string userName)
        {
            Membership.ResetPassword(userName);
        }
    }
}
