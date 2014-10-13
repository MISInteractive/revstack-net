using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RevStack.Client.API;

namespace Tests.API.App
{
    [TestClass]
    public class UpdateAppTests
    {
        [TestMethod]
        public void UpdateApp()
        {
            ICredentials credentials = new Credentials("", Constants.SUPER_USERNAME, Constants.SUPER_PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.Account.Login();
            credentials.AccessToken = token;

            string appStr = "{ 'id': '" + Constants.APP_ID + "', 'name': 'S-L Designs', 'description': 'S-L Designs', 'authType': 'basic', 'schema': { 'read': ['host', 'admin', 'everyone'], 'create': ['host', 'admin', 'everyone'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] }, 'role': { 'read': ['host', 'admin'], 'create': ['host', 'admin'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] }, 'user': { 'read': ['host', 'admin', 'everyone'], 'create': ['host', 'admin', 'everyone'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] } }";         
            JObject app = JObject.Parse(appStr);
            app = request.App.Update(app);
        }
    }
}
