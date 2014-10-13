using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Store.Transaction
{
    public class TransactionService : TransactionValidatorBase
    {
        public TransactionService(ITransaction transaction) : base(transaction) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return Transaction.GetValidations();
        }

        public override JObject Create(JObject json)
        {
            Validate(json, ValidationType.Create);
            return Transaction.Create(json);
        }

        public override JObject Update(JObject json)
        {
            Validate(json, ValidationType.Update);
            return Transaction.Update(json);
        }

        public override JObject Get(string id)
        {
            return Transaction.Get(id);
        }
    }
}
