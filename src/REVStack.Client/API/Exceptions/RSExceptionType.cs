using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Exceptions
{
    public enum RSExceptionType
    {
        Connection,
        Deserialization,
        Document,
        Operation,
        Query,
        Serialization
    }
}
