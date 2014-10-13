using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.User
{
    public abstract class UserValidatorBase : UserDecorator
    {
        public UserValidatorBase(IUser user) : base(user) { }

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
