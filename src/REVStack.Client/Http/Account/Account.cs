using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Account
{
    internal class Account : RequestObject, IAccount
    {
        public Account(string host, int version, ICredentials credentials)
            : base(host, version, credentials) { }

        public virtual JObject Create()
        {
            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "revstack", "/account");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, null, true, false);
            return response.GetJson();
        }

        public virtual JObject Profile()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/account/profile");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual void ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        public virtual IAccessToken Login()
        {
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/account/login");
            string method = "GET";
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, true, false);
            JObject json = response.GetJson();
            string token = json["access_token"].ToString();
            return new AccessToken(token);
        }

        public virtual void Delete(string id)
        {
            throw new NotImplementedException();     
        }

        public virtual JObject Get(string id)
        {
            throw new NotImplementedException();
        }

        public virtual JArray Get()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
