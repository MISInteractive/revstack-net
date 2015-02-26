using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public interface IEntity
    {
        string @rid { get; set; }
        string @version { get; set; }
        string @created { get; set; }
        string @modified { get; set; }
        string id { get; set; }
    }
}
