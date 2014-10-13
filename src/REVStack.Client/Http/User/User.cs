using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.User
{
    internal class User : RequestObject, API.User.IUser
    {
        public User(string host, int version, ICredentials credentials)
            : base(host, version, credentials) { }

        public virtual IAccessToken Login()
        {
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user/login");
            string method = "GET";
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, true, false);
            JObject json = response.GetJson();
            string token = json["access_token"].ToString();
            return new AccessToken(token);
        }

        public virtual JObject Profile()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Update(JObject json)
        {
            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual void Delete()
        {
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual void ChangePassword(string newPassword)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user/change-password/" + newPassword);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual void ResetPassword()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/user/reset-password");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
