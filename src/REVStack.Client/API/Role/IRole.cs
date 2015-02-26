using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Role
{
    public interface IRole
    {
        T Create<T>(T entity) where T : new();
        T Update<T>(T entity) where T : new();
        void Delete(string roleName);
        T Get<T>(string roleName) where T : new();
        IQueryable<T> GetUsersInRole<T>(string roleName) where T : new();
        IQueryable<T> Get<T>() where T : new();
        IEnumerable<IValidation> GetValidations();
    }
}
