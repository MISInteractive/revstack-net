using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.Http
{
    internal class Constants
    {
        internal const string API_HOST_URL = "http://baas3.r3vstack.com/api"; 
        internal const string API_VERSION = "1";
        internal const string API_VERSION_HEADER = "X-R3VStack-API-Version";
        internal const string CONTENT_TYPE = "application/json"; 
        internal const string BASIC_AUTH_PREFIX = "Basic ";
        internal const string BASIC_AUTH_FORMAT = BASIC_AUTH_PREFIX + "{0}";
        internal const string SESSION_TOKEN_PREFIX = "Session ";
        internal const string SESSION_TOKEN_FORMAT = SESSION_TOKEN_PREFIX + "{0}";
    }
}
