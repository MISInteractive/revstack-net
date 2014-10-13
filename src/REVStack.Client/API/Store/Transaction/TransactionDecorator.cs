using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Store.Transaction
{
    public class TransactionDecorator : ITransaction
    {
        private readonly ITransaction _transaction;

        protected ITransaction Transaction
        {
            get { return _transaction; }
        }

        protected TransactionDecorator(ITransaction transaction)
        {
            _transaction = transaction;
        }

        public virtual JObject Create(JObject json)
        {
            return Transaction.Create(json);
        }

        public virtual JObject Update(JObject json)
        {
            return Transaction.Update(json);
        }

        public virtual JObject Get(string id)
        {
            return Transaction.Get(id);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Transaction.GetValidations();
        }
    }
}
