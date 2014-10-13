using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.User
{
    public class UserService : UserValidatorBase
    {
        public UserService(IUser user) : base(user) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return User.GetValidations();
        }

        public override JObject Update(JObject json)
        {
            Validate(json, ValidationType.Update);
            return User.Update(json);
        }

        public override IAccessToken Login()
        {
            return User.Login();
        }

        public override void ChangePassword(string newPassword)
        {
            Validate(null, ValidationType.ChangePassword);
            User.ChangePassword(newPassword);
        }

        public override void ResetPassword()
        {
            User.ResetPassword();
        }

        public override JObject Profile()
        {
            return User.Profile();
        }

        public override void Delete()
        {
            User.Delete();
        }
    }
}
