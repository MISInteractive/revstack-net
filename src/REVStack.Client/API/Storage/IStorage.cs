using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Storage
{
    public interface IStorage
    {
        File MailMerge(MailMerge mailMerge);
        IEnumerable<IValidation> GetValidations();
    }
}
