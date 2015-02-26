using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RevStack.Client.API
{
    [DataContract]
    public abstract class Entity : IEntity
    {
        [DataMember(Name = "@rid")]
        public string @rid { get; set; }
        [DataMember(Name = "@version")]
        public string @version { get; set; }
        [DataMember(Name = "@created")]
        public string @created { get; set; }
        [DataMember(Name = "@modified")]
        public string @modified { get; set; }
        [DataMember]
        public string id { get; set; }
    }
}
