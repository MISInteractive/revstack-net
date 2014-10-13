using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Account
{
    public interface IAccount
    {
        JObject Create();
        IAccessToken Login();
        void ChangePassword(string newPassword);
        JObject Profile();

        //RevStack superuser
        void Delete(string id);
        JObject Get(string id);
        JArray Get();

        IEnumerable<IValidation> GetValidations();
    }
}
