using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Storage
{
    public abstract class StorageDecorator : IStorage
    {
        private readonly IStorage _storage;

        protected IStorage Storage
        {
            get { return _storage; }
        }

        protected StorageDecorator(IStorage storage)
        {
            _storage = storage;
        }

        public virtual File MailMerge(MailMerge mailMerge) 
        {
            return this.MailMerge(mailMerge);
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return Storage.GetValidations();
        }
    }
}
