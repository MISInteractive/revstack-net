using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Exceptions
{
    public class RSException : Exception
    {
        public RSExceptionType Type { get; set; }

        public RSException()
        {
        }

        public RSException(RSExceptionType type, string message)
            : base(message)
        {
            Type = type;
        }

        public RSException(RSExceptionType type, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }
    }
}
