using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.App
{
    public class AppService : AppValidatorBase
    {
        public AppService(IApp app) : base(app) { }

        public override IEnumerable<IValidation> GetValidations()
        {
            return App.GetValidations();
        }

        public override JObject Create(JObject json)
        {
            Validate(json, ValidationType.Create);
            return App.Create(json);
        }

        public override JObject Update(JObject json)
        {
            Validate(json, ValidationType.Update);
            return App.Update(json);
        }

        public override JObject Get(string id)
        {
            return App.Get(id);
        }

        public override JObject Get()
        {
            return App.Get();
        }
        
        public override void Delete(string id)
        {
            Validate(id, ValidationType.Delete);
            App.Delete(id);
        }
    }
}
