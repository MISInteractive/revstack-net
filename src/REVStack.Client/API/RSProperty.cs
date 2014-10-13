using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RSProperty : Attribute
    {
        public string Alias { get; set; }
        public bool Serializable { get; set; }
        public bool Deserializable { get; set; }
        
        public RSProperty()
        {
            Alias = "";
            Serializable = true;
            Deserializable = true;
        }
    }
}
