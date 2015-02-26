using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Membership;
using RevStack.Client.Http.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Membership
{
    internal class Membership : RequestObject, API.Membership.IMembership
    {
        private readonly HttpQueryProvider _queryProvider;

        public Membership(string host, int version, ICredentials credentials)
            : base(host, version, credentials)
        {
            HttpConnection connection = new HttpConnection();
            connection.Host = host;
            connection.AppId = this.AppId;
            connection.Version = version;
            connection.Credentials = credentials;
            _queryProvider = new HttpQueryProvider(connection);
        }

        public virtual T Create<T>(T entity) where T : new()
        {
            string json = JsonConvert.SerializeObject(entity);
            JObject item = JObject.Parse(json);
            item["@class"] = "OUser";
            json = item.ToString();

            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            string json = JsonConvert.SerializeObject(entity);
            JObject item = JObject.Parse(json);
            item["@class"] = "OUser";
            json = item.ToString();

            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual void Delete(string userName)
        {
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/" + userName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual T Get<T>(string userName) where T : new()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/" + userName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual IQueryable<T> Get<T>() where T : new()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            IQueryable<T> results = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Body).AsQueryable();
            return results;
        }

        public void AddUserToRole(string userName, string roleName)
        {
            JObject item = JObject.Parse("{ 'name': '" + userName + "', 'role': '" + roleName + "' }");
            string json = item.ToString();

            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/add-user-to-role");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
            JObject item = JObject.Parse("{ 'name': '" + userName + "', 'role': '" + roleName + "' }");
            string json = item.ToString();

            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/remove-user-from-role");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public void ResetPassword(string userName)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/membership/reset-password/" + userName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
