using RevStack.Client.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RevStack.Client.Http
{
    static class HttpClient
    {
        internal static string BuildUrl(string host, int version, string appId, string api) 
        {
            string url = host;
            if (!url.EndsWith("/"))
                url = url + "/";
            if (!string.IsNullOrEmpty(appId))
                url = url + appId;
            if (!api.StartsWith("/"))
                api = "/" + api;
            url = url + api;
            return url;
        }

        public static HttpRestResponse SendRequest(string url, string method, string body, string username, string password, IAccessToken token, bool requiresAuthentication, bool requiresToken)
        {
            HttpRestResponse rv = new HttpRestResponse
            {
                Headers = new Dictionary<string, string>(),
                Body = string.Empty
            };

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.KeepAlive = false;
                
                request.Headers.Add(Constants.API_VERSION_HEADER, Constants.API_VERSION);

                if (requiresAuthentication)
                {
                    byte[] authBytes = Encoding.UTF8.GetBytes((username + ":" + password).ToCharArray());
                    request.Headers["Authorization"] = string.Format(Constants.BASIC_AUTH_FORMAT, Convert.ToBase64String(authBytes));
                }

                if (requiresToken)
                    request.Headers["Authorization"] = string.Format(Constants.SESSION_TOKEN_FORMAT, token.Token);
                
                //var cred = new NetworkCredential(username, password);
                //request.Credentials = cred;

                request.ContentType = Constants.CONTENT_TYPE;

                if (!string.IsNullOrEmpty(body))
                {
                    var bytes = Encoding.UTF8.GetBytes(body);
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else 
                {
                    request.ContentLength = 0;
                }

                response = (HttpWebResponse)request.GetResponse();
             
                rv.StatusCode = (int)((HttpWebResponse)response).StatusCode;
                rv.StatusString = ((HttpWebResponse)response).StatusDescription;
                rv.ContentLength = response.ContentLength;
                rv.ContentType = response.ContentType;
                response.Headers.AllKeys.ToList().ForEach(o => rv.Headers.Add(o, response.Headers[o]));

            }
            catch (WebException ex)
            {
                rv.Exception = ex;
                if (ex.Response != null)
                {
                    rv.StatusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                    rv.StatusString = ((HttpWebResponse)ex.Response).StatusDescription;
                    response = (HttpWebResponse)ex.Response;
                }
            }
            catch (Exception ex)
            {
                rv.Exception = ex;
            }
           
            if (response != null && response.ContentLength > 0)
            {
                string tempString = null;
                int count = 0;
                byte[] buf = new byte[8192];
                StringBuilder sb = new StringBuilder();
                do
                {
                    count = response.GetResponseStream().Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        tempString = Encoding.UTF8.GetString(buf, 0, count);
                        sb.Append(tempString);
                    }
                }
                while (count > 0);
                rv.Body = sb.ToString();
            }

            if (rv.Headers.ContainsKey("Token")) 
            {
                rv.Token = rv.Headers["Token"];
            }

            //if (rv.Headers.ContainsKey("Set-Cookie"))
            //{
            //    if (rv.Headers["Set-Cookie"].Contains("OSESSIONID"))
            //    {
            //        rv.OSessionId = Regex.Match(rv.Headers["Set-Cookie"], "(?<=OSESSIONID=).*?(?=;)", RegexOptions.Compiled).Value;
            //    }
            //}

            return rv;
        }
    }
}
