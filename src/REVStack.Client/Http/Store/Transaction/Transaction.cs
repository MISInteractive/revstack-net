using Newtonsoft.Json.Linq;
using RevStack.Client.API;
using RevStack.Client.API.Store.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Store.Transaction
{
    internal class Transaction : RequestObject, API.Store.Transaction.ITransaction
    {
        public Transaction(string host, int version, ICredentials credentials)
            :base(host, version, credentials) {}

        public virtual JObject Create(JObject json)
        {
            string method = "POST";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/store/transaction");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Update(JObject json)
        {
            string method = "PUT";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/store/transaction");
            HttpRestResponse response = HttpClient.SendRequest(url, method, json.ToString(), this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual JObject Get(string id)
        {
            string method = "GET";
            string url = HttpClient.BuildUrl(this.Host, this.Version, this.AppId.ToString(), "/store/transaction/" + id);
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, this.Credentials.Username, this.Credentials.Password, this.Credentials.AccessToken, false, true);
            return response.GetJson();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return new List<IValidation>();
        }
    }
}
