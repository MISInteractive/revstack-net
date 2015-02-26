using RevStack.Client.API;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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

        internal static void ForceCanonicalPathAndQuery(Uri uri)
        {
            string paq = uri.PathAndQuery; // need to access PathAndQuery
            FieldInfo flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            ulong flags = (ulong)flagsFieldInfo.GetValue(uri);
            flags &= ~((ulong)0x30); // Flags.PathNotCanonical|Flags.QueryNotCanonical
            flagsFieldInfo.SetValue(uri, flags);
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

            return rv;
        }

        public static HttpRestResponse SendUploadRequest(string url, NameValueCollection values, NameValueCollection files, string username, string password, IAccessToken token, bool requiresAuthentication, bool requiresToken)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            HttpRestResponse rv = new HttpRestResponse
            {
                Headers = new Dictionary<string, string>(),
                Body = string.Empty
            };

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
            byte[] boundaryBytesF = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            
            try 
            { 
                // Create the request and set parameters
                request = (HttpWebRequest)WebRequest.Create(url); 
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                //request.KeepAlive = false;
                request.Method = "POST";

                request.Headers.Add(Constants.API_VERSION_HEADER, Constants.API_VERSION);

                if (requiresAuthentication)
                {
                    byte[] authBytes = Encoding.UTF8.GetBytes((username + ":" + password).ToCharArray());
                    request.Headers["Authorization"] = string.Format(Constants.BASIC_AUTH_FORMAT, Convert.ToBase64String(authBytes));
                }

                if (requiresToken)
                    request.Headers["Authorization"] = string.Format(Constants.SESSION_TOKEN_FORMAT, token.Token);

                
                // Get request stream
                Stream requestStream = request.GetRequestStream();


                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in values.Keys)
                {
                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    string formitem = string.Format(formdataTemplate, key, values[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formitembytes, 0, formitembytes.Length);
                }
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                if (files != null)
                {
                    foreach (string key in files.Keys)
                    {
                        if (File.Exists(files[key]))
                        {
                            int bytesRead = 0;
                            byte[] buffer = new byte[2048];
                            byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                            requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                            using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                            {
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    // Write file content to stream, byte by byte
                                    requestStream.Write(buffer, 0, bytesRead);
                                }

                                fileStream.Close();
                            }
                        }
                    }
                }

                // Write trailer and close stream
                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();

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

            return rv;
        }
    }
}
