using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Storage
{
    public class StorageService : StorageValidatorBase
    {
        public StorageService(IStorage storage) : base(storage) { }

        public override File MailMerge(MailMerge mailMerge)
        {
            Validate(null, ValidationType.MailMerge);
            return Storage.MailMerge(mailMerge);
        }

        public override IEnumerable<IValidation> GetValidations()
        {
            //hook validations here...
            return Storage.GetValidations();
        }
    }
}
