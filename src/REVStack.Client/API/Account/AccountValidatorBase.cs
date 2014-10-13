using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Account
{   
    public abstract class AccountValidatorBase : AccountDecorator
    {
        public AccountValidatorBase(IAccount account) : base(account) { }

        public void Validate(Object obj, ValidationType validationType)
        {
            foreach (var validation in GetValidations())
            {
                if (validation.ValidationType == validationType)
                {
                    validation.Validate(obj);
                    if (!validation.IsValid)
                        throw validation.Exception;
                }
            }
        }
    }
}
