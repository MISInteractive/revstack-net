using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Storage
{
    public abstract class StorageValidatorBase : StorageDecorator
    {
        public StorageValidatorBase(IStorage storage) : base(storage) { }

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
