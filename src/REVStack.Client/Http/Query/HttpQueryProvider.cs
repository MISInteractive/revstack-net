using IQToolkit.Data;
using IQToolkit.Data.Common;
using Newtonsoft.Json;
using RevStack.Client.API.Query;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http.Query
{
    public class HttpQueryProvider : QueryProvider
    {
        HttpConnection connection;

        public HttpQueryProvider(HttpConnection connection)
        {
            this.connection = connection;
        }

        public override object Execute(Expression expression)
        {
            int top = -1;
            string fetch = "*:0";

            string query = this.Translate(expression);
            Type elementType = TypeSystem.GetElementType(expression.Type);
            
          
            string method = "GET";
            string url = HttpClient.BuildUrl(connection.Host, connection.Version, connection.AppId.ToString(), "/datastore/sql/" + Uri.EscapeUriString(query).Replace("%28", "(").Replace("%29", ")")) + "?top=" + top + "&fetch=" + fetch;
            HttpRestResponse response = HttpClient.SendRequest(url, method, null, connection.Credentials.Username, connection.Credentials.Password, connection.Credentials.AccessToken, false, true);
            object results = JsonConvert.DeserializeObject(response.Body, typeof(IEnumerable<>).MakeGenericType(elementType));
            
            //IEnumerable<object> results = JsonConvert.DeserializeObject<IEnumerable<object>>(response.Body).Cast<elementType>;
            return results;
        }
    }
}
