using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Storage
{
    public class MailMerge : Entity
    {
        private readonly NameValueCollection _files = new NameValueCollection();
        private readonly NameValueCollection _variables = new NameValueCollection();

        public NameValueCollection Files { get {return _files; } }
        [DataMember(Name = "path")]
        public string Path { get; set; }
        [DataMember(Name = "variables")]
        public NameValueCollection Variables { get { return _variables; } }
    }
}
