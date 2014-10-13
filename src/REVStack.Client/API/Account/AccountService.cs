using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Account
{
    public class AccountService : AccountValidatorBase
    {
        public AccountService(IAccount account) : base(account) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return Account.GetValidations();
        }

        public override JObject Create()
        {
            Validate(null, ValidationType.Create);
            return Account.Create();
        }

        public override IAccessToken Login()
        {
            return Account.Login();
        }

        public override void ChangePassword(string newPassword)
        {
            Validate(null, ValidationType.ChangePassword);
            Account.ChangePassword(newPassword);
        }

        public override JObject Get(string id)
        {
            return Account.Get(id);
        }

        public override JObject Profile()
        {
            return Account.Profile();
        }

        public override JArray Get()
        {
            return Account.Get();
        }

        public override void Delete(string id)
        {
            Account.Delete(id);
        }
    }
}
