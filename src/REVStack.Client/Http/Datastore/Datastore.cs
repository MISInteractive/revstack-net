using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Extensions;
using RevStack.Client.API.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Datastore
{
    internal class Datastore : RequestObject, API.Datastore.IDatastore
    {      
        public Datastore(string host, int version, ICredentials credentials)
            : base(host, version, credentials) { }

        public virtual JObject Create(JObject json)
        {
            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Update(JObject json)
        {
            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual void Delete(string id)
        {
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual void Command(string query)
        {
            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/command/" + Uri.EscapeUriString(query).Replace("%28", "(").Replace("%29", ")"));
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual JObject Get(string id)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), this.AppId.ToString() + "/datastore/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JArray Lookup(string query, int page, int limit)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/lookup/" + Uri.EscapeUriString(query).Replace("%28", "(").Replace("%29", ")") + "/-1");
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JArray.Parse(response.Body);
        }

        public virtual JObject Paginate(string query, int page, int limit)
        {
            JArray results = this.Lookup(query, page, limit);
            List<JToken> s = results.Page(page, limit).ToList();
            int count = s.Count;
            int total = results.Count;

            double dbl_page_count = ((double)total / (double)limit);
            int page_count = (int)Math.Ceiling(dbl_page_count);
            if (page_count == 0) page_count = 1;

            JObject paging = new JObject();
            paging.Add("page", page);
            paging.Add("limit", limit);
            paging.Add("count", count);
            paging.Add("page_count", page_count);
            paging.Add("total_count", total);

            JObject response = new JObject();
            response.Add("_data", JToken.FromObject(s));
            response.Add("pagination", paging);
            //{"response":[{"Rating":"Good"}],"count":1,"total":1,"pagination":{"page":2, "limit":5}}
            return response;
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }

}
