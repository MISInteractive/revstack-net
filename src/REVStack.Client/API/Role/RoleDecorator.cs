using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Role
{
    public abstract class RoleDecorator : IRole
    {
        private readonly IRole _role;

        protected IRole Role
        {
            get { return _role; }
        }

        protected RoleDecorator(IRole role)
        {
            _role = role;
        }

        public virtual T Create<T>(T entity) where T : new()
        {
            return Role.Create<T>(entity);
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            return Role.Update<T>(entity);
        }

        public virtual void Delete(string roleName)
        {
            Role.Delete(roleName);
        }

        public virtual T Get<T>(string roleName) where T : new()
        {
            return Role.Get<T>(roleName);
        }

        public virtual IQueryable<T> GetUsersInRole<T>(string roleName) where T : new()
        {
            return Role.GetUsersInRole<T>(roleName);
        }

        public virtual IQueryable<T> Get<T>() where T : new()
        {
            return Role.Get<T>();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Role.GetValidations();
        }
    }
}
