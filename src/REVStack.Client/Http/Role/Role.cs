using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevStack.Client.API;
using RevStack.Client.API.Role;
using RevStack.Client.Http.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RevStack.Client.Http.Role
{
    internal class Role : RequestObject, API.Role.IRole
    {
        private readonly HttpQueryProvider _queryProvider;

        public Role(string host, int version, ICredentials credentials)
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
            item["@class"] = "ORole";
            json = item.ToString();

            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            string json = JsonConvert.SerializeObject(entity);
            JObject item = JObject.Parse(json);
            item["@class"] = "ORole";
            json = item.ToString();

            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual void Delete(string roleName)
        {
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role/" + roleName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual T Get<T>(string roleName) where T : new()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role/" + roleName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual IQueryable<T> Get<T>() where T : new()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role/");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            IQueryable<T> results = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Body).AsQueryable();
            return results;
        }

        public virtual IQueryable<T> GetUsersInRole<T>(string roleName) where T : new()
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/role/users/" + roleName);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            IQueryable<T> results = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Body).AsQueryable();
            return results;
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
