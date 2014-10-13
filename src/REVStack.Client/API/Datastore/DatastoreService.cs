using Newtonsoft.Json.Linq;
using RevStack.Client.API.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Datastore
{
    public class DatastoreService : DatastoreValidatorBase
    {
        public DatastoreService(IDatastore datastore) : base(datastore) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return Datastore.GetValidations();
        }

        public override JObject Create(JObject json)
        {
            Validate(json, ValidationType.Create);
            return Datastore.Create(json);
        }

        public override JObject Update(JObject json)
        {
            Validate(json, ValidationType.Update);
            return Datastore.Update(json);
        }

        public override JObject Get(string id)
        {
            return Datastore.Get(id);
        }

        public override JArray Lookup(string query, int page, int limit)
        {
            return Datastore.Lookup(query, page, limit);
        }

        public override JObject Paginate(string query, int page, int limit)
        {
            return Datastore.Paginate(query, page, limit);
        }

        public override void Command(string query)
        {
            Datastore.Command(query);
        }

        public override void Delete(string id)
        {
            Validate(id, ValidationType.Delete);
            Datastore.Delete(id);
        }
    }

}
