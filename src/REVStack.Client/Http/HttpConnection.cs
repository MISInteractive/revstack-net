using RevStack.Client.API;

namespace RevStack.Client.Http
{
    public class HttpConnection
    {
        public string Host { get; set; }
        public string AppId { get; set; }
        public int Version { get; set; }
        public ICredentials Credentials { get; set; }
    }
}
