using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Storage;
using RevStack.Client.Http.Query;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Storage
{
    internal class Storage : RequestObject, API.Storage.IStorage
    {
        private readonly HttpQueryProvider queryProvider;

        public Storage(string host, int version, ICredentials credentials)
            : base(host, version, credentials)
        {
            HttpConnection connection = new HttpConnection();
            connection.Host = host;
            connection.AppId = this.AppId;
            connection.Version = version;
            connection.Credentials = credentials;
            queryProvider = new HttpQueryProvider(connection);
        }

        public virtual File MailMerge(MailMerge mailMerge) 
        {
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/storage/file/mail-merge");
            
            NameValueCollection nvc = new NameValueCollection();
            JObject obj = new JObject();
            JObject variables = new JObject();

            foreach (string key in mailMerge.Variables.Keys)
                variables.Add(key, mailMerge.Variables[key].ToString());

            obj.Add("path", mailMerge.Path);
            obj.Add("variables", variables);
            nvc.Add("data", obj.ToString());

            HttpRestResponse response = HttpClient.SendUploadRequest(url, nvc, mailMerge.Files, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return (File)JsonConvert.DeserializeObject(response.GetJson().ToString(), typeof(File));
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
