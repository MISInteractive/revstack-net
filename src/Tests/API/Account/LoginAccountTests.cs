using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;

namespace Tests.API.Account
{
    [TestClass]
    public class LoginAccountTests
    {
        [TestMethod]
        public void Login()
        {
            ICredentials credentials = new Credentials("", Constants.SUPER_USERNAME, Constants.SUPER_PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.Account.Login();
            Console.WriteLine("Token=" + token.Token);
        }

        [TestMethod]
        public void Profile()
        {
            ICredentials credentials = new Credentials("", Constants.SUPER_USERNAME, Constants.SUPER_PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            
            IAccessToken token = request.Account.Login();
            credentials.AccessToken = token;

            JObject json = request.Account.Profile();

            Console.WriteLine("profile=" + json.ToString());
        }
    }
}
