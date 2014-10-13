using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.User
{
    public interface IUser
    {
        IAccessToken Login();
        JObject Profile();
        JObject Update(JObject json);
        void Delete();
        void ChangePassword(string newPassword);
        void ResetPassword();
        IEnumerable<IValidation> GetValidations();
    }
}
