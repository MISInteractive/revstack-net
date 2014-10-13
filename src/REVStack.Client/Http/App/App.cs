using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.App
{
    internal class App : RequestObject, API.App.IApp
    {
        public App(string host, int version, ICredentials credentials)
            : base(host, version, credentials) { }

        public virtual JObject Create(JObject json)
        {
            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/app");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Update(JObject json)
        {
            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/app");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual void Delete(string id)
        {
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/app/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual JObject Get(string id)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/app/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Get()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, "", "/app/list");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
