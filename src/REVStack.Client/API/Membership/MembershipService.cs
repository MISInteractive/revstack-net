using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Membership
{
    public class MembershipService : MembershipValidatorBase
    {
        public MembershipService(IMembership membership) : base(membership) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return Membership.GetValidations();
        }

        public override T Create<T>(T entity)
        {
            Validate(entity, ValidationType.Create);
            return Membership.Create<T>(entity);
        }

        public override T Update<T>(T entity)
        {
            Validate(entity, ValidationType.Update);
            return Membership.Update<T>(entity);
        }

        public override T Get<T>(string userName)
        {
            return Membership.Get<T>(userName);
        }

        public override IQueryable<T> Get<T>()
        {
            return Membership.Get<T>();
        }

        public override void Delete(string userName)
        {
            Validate(userName, ValidationType.Delete);
            Membership.Delete(userName);
        }

        public override void AddUserToRole(string userName, string roleName)
        {
            Membership.AddUserToRole(userName, roleName);
        }

        public override void RemoveUserFromRole(string userName, string roleName)
        {
            Membership.RemoveUserFromRole(userName, roleName);
        }

        public override void ResetPassword(string userName)
        {
            Membership.ResetPassword(userName);
        }
    }
}
