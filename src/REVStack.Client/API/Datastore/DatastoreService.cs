using Newtonsoft.Json;
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

        public override T Create<T>(T entity)
        {
            return this.Create<T>(entity, false);
        }

        public override T Create<T>(T entity, bool fullName)
        {
            Validate(entity, ValidationType.Create);
            return Datastore.Create<T>(entity, fullName);
        }

        public override T Update<T>(T entity)
        {
            return this.Update<T>(entity, false);
        }

        public override T Update<T>(T entity, bool fullName)
        {
            Validate(entity, ValidationType.Update);
            return Datastore.Update<T>(entity, fullName);
        }

        public override T Get<T>(string id)
        {
            return this.Get<T>(id, false);
        }

        public override T Get<T>(string id, bool fullName)
        {
            return Datastore.Get<T>(id, fullName);
        }

        public override RevStack.Client.API.Query.Query<T> CreateQuery<T>(object[] args) 
        {
            return Datastore.CreateQuery<T>(args);
        }

        public override IQueryable<T> SqlQuery<T>(string query, object[] args)
        {
            return Datastore.SqlQuery<T>(query, args);
        }

        public override PagedDataSource<T> Paginate<T>(string query, int page, int limit, object[] args)
        {
            return Datastore.Paginate<T>(query, page, limit, args);
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
