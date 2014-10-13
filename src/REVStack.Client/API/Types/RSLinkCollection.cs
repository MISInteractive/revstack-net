using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Types
{
    internal class RSLinkCollection : List<RSID>
    {
        internal int PageSize { get; set; }
        internal RSID Root { get; set; }
        internal int KeySize { get; set; }
    }
}
