using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.User
{
    public abstract class UserDecorator : IUser
    {
        private readonly IUser _user;

        protected IUser User
        {
            get { return _user; }
        }

        protected UserDecorator(IUser user)
        {
            _user = user;
        }

        public virtual IAccessToken Login()
        {
            return User.Login();
        }

        public virtual JObject Profile()
        {
            return User.Profile();
        }

        public virtual JObject Update(JObject json)
        {
            return User.Update(json);
        }

        public virtual void Delete()
        {
            User.Delete();
        }

        public virtual void ChangePassword(string newPassword)
        {
            User.ChangePassword(newPassword);
        }

        public virtual void ResetPassword()
        {
            User.ResetPassword();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return User.GetValidations();
        }
    }
}
