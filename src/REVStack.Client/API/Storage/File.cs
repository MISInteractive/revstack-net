using System.Runtime.Serialization;

namespace RevStack.Client.API.Storage
{
    public class File : Entity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "size")]
        public long Size { get; set; }
    }
}
