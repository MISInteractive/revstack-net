using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;

namespace Tests.API.App
{
    [TestClass]
    public class CreateAppTests
    {
        [TestMethod]
        public void CreateApp()
        {
            ICredentials credentials = new Credentials("", Constants.SUPER_USERNAME, Constants.SUPER_PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.Account.Login();
            credentials.AccessToken = token;

            string appStr = "{ 'name': 'The Hub LTD', 'description': 'The Hub LTD', 'authType': 'basic', 'schema': { 'read': ['host', 'admin', 'everyone'], 'create': ['host', 'admin', 'everyone'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] }, 'role': { 'read': ['host', 'admin'], 'create': ['host', 'admin'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] }, 'user': { 'read': ['host', 'admin', 'everyone'], 'create': ['host', 'admin', 'everyone'], 'update': ['admin', 'host'], 'delete': ['admin', 'host'] } }";

            JObject app = JObject.Parse(appStr);
            
            app = request.App.Create(app);

            Console.WriteLine("Id=" + app["id"].ToString());
        }
    }
}
