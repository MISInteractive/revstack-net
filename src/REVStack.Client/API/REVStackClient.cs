using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public sealed class RevStackClient
    {
        public static RevStackRequest CreateRequest(string host, int version, string appId, string username, string password)
        {
            return CreateRequest(host, version, appId, username, password, null, ClientType.Http);
        }

        public static RevStackRequest CreateRequest(string host, int version, string appId, string username, string password, IAccessToken accessToken)
        {
            return CreateRequest(host, version, appId, username, password, accessToken, ClientType.Http);
        }

        public static RevStackRequest CreateRequest(string host, int version, string appId, string username, string password, IAccessToken accessToken, ClientType clientType)
        {
            ICredentials credentials = new Credentials(appId, username, password, accessToken);
            return new RevStackRequest(host, version, credentials, clientType);
        }

        public static RevStackRequest CreateRequest(ICredentials credentials, ClientType clientType)
        {
            return CreateRequest(credentials, clientType);
        }

        public static RevStackRequest CreateRequest(ICredentials credentials)
        {
            return new RevStackRequest(credentials, ClientType.Http);
        }
    }
}
