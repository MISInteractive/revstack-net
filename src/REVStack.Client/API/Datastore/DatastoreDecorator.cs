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

        public virtual JObject Create(JObject json)
        {
            return Datastore.Create(json);
        }

        public virtual JObject Update(JObject json)
        {
            return Datastore.Update(json);
        }

        public virtual void Delete(string id)
        {
            Datastore.Delete(id);
        }

        public virtual void Command(string query)
        {
            Datastore.Command(query);
        }

        public virtual JObject Get(string id)
        {
            return Datastore.Get(id);
        }

        public virtual JArray Lookup(string query, int page, int limit)
        {
            return Datastore.Lookup(query, page, limit);
        }

        public virtual JObject Paginate(string query, int page, int limit)
        {
            return Datastore.Paginate(query, page, limit);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Datastore.GetValidations();
        }
    }

}
