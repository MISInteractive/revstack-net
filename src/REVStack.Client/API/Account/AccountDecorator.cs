using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Account
{
    public abstract class AccountDecorator : IAccount
    {
        private readonly IAccount _account;

        protected IAccount Account
        {
            get { return _account; }
        }

        protected AccountDecorator(IAccount account)
        {
            _account = account;
        }

        public virtual JObject Create()
        {
            return Account.Create();
        }

        public virtual JObject Profile()
        {
            return Account.Profile();
        }

        public virtual void ChangePassword(string newPassword)
        {
            Account.ChangePassword(newPassword);
        }

        public virtual IAccessToken Login()
        {
            return Account.Login();
        }

        public virtual void Delete(string id)
        {
            Account.Delete(id);
        }

        public virtual JObject Get(string id)
        {
            return Account.Get(id);
        }

        public virtual JArray Get()
        {
            return Account.Get();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Account.GetValidations();
        }
    }
}
