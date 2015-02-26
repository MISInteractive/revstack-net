using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Role
{
    public class RoleService : RoleValidatorBase
    {
        public RoleService(IRole role) : base(role) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return Role.GetValidations();
        }

        public override T Create<T>(T entity)
        {
            Validate(entity, ValidationType.Create);
            return Role.Create<T>(entity);
        }

        public override T Update<T>(T entity)
        {
            Validate(entity, ValidationType.Update);
            return Role.Update<T>(entity);
        }

        public override T Get<T>(string roleName)
        {
            return Role.Get<T>(roleName);
        }

        public override IQueryable<T> GetUsersInRole<T>(string roleName)
        {
            return Role.GetUsersInRole<T>(roleName);
        }

        public override IQueryable<T> Get<T>()
        {
            return Role.Get<T>();
        }
        
        public override void Delete(string roleName)
        {
            Validate(roleName, ValidationType.Delete);
            Role.Delete(roleName);
        }
    }
}
