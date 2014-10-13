using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Store.Transaction
{
    public interface ITransaction
    {
        JObject Create(JObject json);
        JObject Update(JObject json);
        JObject Get(string id);
        IEnumerable<IValidation> GetValidations();
    }
}
