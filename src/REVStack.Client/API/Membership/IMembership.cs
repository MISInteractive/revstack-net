using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Membership
{
    public interface IMembership
    {
        T Create<T>(T entity) where T : new();
        T Update<T>(T entity) where T : new();
        void Delete(string userName);
        void AddUserToRole(string userName, string roleName);
        void RemoveUserFromRole(string userName, string roleName);
        void ResetPassword(string userName);
        T Get<T>(string userName) where T : new();
        IQueryable<T> Get<T>() where T : new();
        IEnumerable<IValidation> GetValidations();
    }
}
