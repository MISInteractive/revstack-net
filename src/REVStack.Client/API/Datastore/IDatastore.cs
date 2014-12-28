using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Datastore
{
    public interface IDatastore
    {
        T Create<T>(T entity) where T : new();
        T Create<T>(T entity, bool fullName) where T : new();
        T Update<T>(T entity) where T : new();
        T Update<T>(T entity, bool fullName) where T : new();
        void Delete(string id);
        T Get<T>(string id, bool fullName) where T : new();
        T Get<T>(string id) where T : new();
        void Command(string query);
        RevStack.Client.API.Query.Query<T> CreateQuery<T>(object[] args) where T : new();
        IQueryable<T> SqlQuery<T>(string query, object[] args) where T : new(); //top=-1 fetch=*:-1 to get all
        PagedDataSource<T> Paginate<T>(string query, int page, int limit, object[] args) where T : new(); //page=-1 limit=-1 to get all
        IEnumerable<IValidation> GetValidations();
    }

}
