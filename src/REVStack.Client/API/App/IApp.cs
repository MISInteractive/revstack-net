using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.App
{
    public interface IApp
    {
        JObject Create(JObject json);
        JObject Update(JObject json);
        void Delete(string id);
        //superuser
        JObject Get(string id);
        JObject Get();
        IEnumerable<IValidation> GetValidations();
    }
}
