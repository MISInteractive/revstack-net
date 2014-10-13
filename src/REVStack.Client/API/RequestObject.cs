using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public abstract class RequestObject
    {
        public RequestObject(string host, int version, ICredentials credentials) 
        {
            this.Host = host;
            this.Version = version;
            this.Credentials = credentials;
            this.AppId = credentials.AppId;
        }

        public string Host { get; set; }
        public int Version { get; set; }
        public ICredentials Credentials { get; set; }
        public string AppId { get; set; }
    }
}
