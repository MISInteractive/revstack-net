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
        JObject Create(JObject json);
        JObject Update(JObject json);
        void Delete(string id);
        JObject Get(string id);
        void Command(string query);
        JArray Lookup(string query, int page, int limit); // page=-1 limit=-1 to get all
        JObject Paginate(string query, int page, int limit);
        IEnumerable<IValidation> GetValidations();
    }

}
