using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Extensions;
using RevStack.Client.API.Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RevStack.Client.Http.Query;

namespace RevStack.Client.Http.Datastore
{
    internal class Datastore : RequestObject, API.Datastore.IDatastore
    {
        private readonly HttpQueryProvider queryProvider;

        public Datastore(string host, int version, ICredentials credentials)
            : base(host, version, credentials) 
        {
            HttpConnection connection = new HttpConnection();
            connection.Host = host;
            connection.AppId = this.AppId;
            connection.Version = version;
            connection.Credentials = credentials;
            queryProvider = new HttpQueryProvider(connection);
        }

        public virtual T Create<T>(T entity) where T : new()
        {
            return this.Create<T>(entity, false);
        }

        public virtual T Create<T>(T entity, bool fullName) where T : new()
        {
            string collection = entity.GetType().Name;
            if (fullName)
                collection = entity.GetType().FullName;
            string json = JsonConvert.SerializeObject(entity);
            JObject item = JObject.Parse(json);
            item["@class"] = collection;
            json = item.ToString();

            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual T Update<T>(T entity) where T : new()
        {
            return this.Update<T>(entity, false);
        }

        public virtual T Update<T>(T entity, bool fullName) where T : new()
        {
            string collection = entity.GetType().Name;
            if (fullName)
                collection = entity.GetType().FullName;
            string json = JsonConvert.SerializeObject(entity);
            JObject item = JObject.Parse(json);
            item["@class"] = collection;
            json = item.ToString();

            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual void Delete(string id)
        {
            id = id.Replace("#", "");
            string method = "DELETE";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual void Command(string query)
        {
            string method = "PUT";
            query = System.Web.HttpUtility.UrlEncode(query);
            //string query2 = Uri.EscapeUriString(query).Replace("%28", "(").Replace("%29", ")");
            //query = Uri.EscapeDataString(query).Replace("%28", "(").Replace("%29", ")");
             
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/command/" + query);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
        }

        public virtual T Get<T>(string id) where T : new()
        {
            return this.Get<T>(id, false);
        }

        public virtual T Get<T>(string id, bool fullName) where T : new()
        {
            string collection = typeof(T).Name;
            if (fullName)
                collection = typeof(T).FullName;

            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/" + collection + "/" + id);
            //string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return JsonConvert.DeserializeObject<T>(response.GetJson().ToString());
        }

        public virtual RevStack.Client.API.Query.Query<T> CreateQuery<T>(object[] args) where T : new()
        {
            int top = -1;
            string fetch = "*:0";
            string collection = typeof(T).Name;

            if (args != null && args[0] != null)
            {
                if (args.Length > 0) top = int.Parse(args[0].ToString());
                if (args.Length > 1) fetch = args[1].ToString();
            }

            if (string.IsNullOrEmpty(fetch))
                fetch = "*:0";

            return new RevStack.Client.API.Query.Query<T>(queryProvider);
        }

        public virtual IQueryable<T> SqlQuery<T>(string query, object[] args) where T : new()
        {
            int top = -1;
            string fetch = "*:0";
            string collection = typeof(T).Name;

            if (args != null && args[0] != null)
            {
                if (args.Length > 0) top = int.Parse(args[0].ToString());
                if (args.Length > 1) fetch = args[1].ToString();
            }

            if (string.IsNullOrEmpty(fetch))
                fetch = "*:0";

            // Uri.EscapeUriString(query).Replace("%28", "(").Replace("%29", ")"))
            query = System.Web.HttpUtility.UrlEncode(query);

            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/datastore/sql/" + query) + "?top=" + top + "&fetch=" + fetch;
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            IQueryable<T> results = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Body).AsQueryable();
            return results;
        }

        public virtual PagedDataSource<T> Paginate<T>(string query, int page, int limit, object[] args) where T : new()
        {
            if (page < 1)
                page = 1;
            
            IQueryable<T> results = this.SqlQuery<T>(query, args);

            if (limit == -1)
                limit = results.Count();

            IQueryable<T> s = results.Page(page, limit).AsQueryable();//.ToList();
            int count = s.Count();
            int total = results.Count();

            double dbl_page_count = ((double)total / (double)limit);
            int page_count = (int)Math.Ceiling(dbl_page_count);
            if (page_count == 0) page_count = 1;

            Pagination pagination = new Pagination();
            pagination.Page = page;
            pagination.Limit = limit;
            pagination.Count = count;
            pagination.PageCount = page_count;
            pagination.TotalCount = total;

            PagedDataSource<T> datasource = new PagedDataSource<T>();
            datasource.Data = s.AsQueryable<T>();
            datasource.Pagination = pagination;
            return datasource;
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }

}
