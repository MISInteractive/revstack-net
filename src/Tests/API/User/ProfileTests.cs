using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;

namespace Tests.API.User
{
    [TestClass]
    public class ProfileTests
    {
        [TestMethod]
        public void GetProfile()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            JObject json = request.User.Profile();
            Console.WriteLine("profile=" + json.ToString());
        }
    }
}
