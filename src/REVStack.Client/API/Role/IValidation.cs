using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Role
{
    public interface IValidation
    {
        void Validate(Object obj);
        bool IsValid { get; }
        Exception Exception { get; }
        ValidationType ValidationType { get; }
    }
}
