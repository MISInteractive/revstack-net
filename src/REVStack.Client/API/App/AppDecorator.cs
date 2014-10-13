using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.App
{
    public abstract class AppDecorator : IApp
    {
        private readonly IApp _app;

        protected IApp App
        {
            get { return _app; }
        }

        protected AppDecorator(IApp app)
        {
            _app = app;
        }

        public virtual JObject Create(JObject json)
        {
            return App.Create(json);
        }

        public virtual JObject Update(JObject json)
        {
            return App.Update(json);
        }

        public virtual void Delete(string id)
        {
            App.Delete(id);
        }

        public virtual JObject Get(string id)
        {
            return App.Get(id);
        }

        public virtual JObject Get()
        {
            return App.Get();
        }

        public virtual IEnumerable<IValidation> GetValidations()
        {
            return App.GetValidations();
        }
    }
}
