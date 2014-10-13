using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;

namespace Tests.API.User
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void LoginTest()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            Console.WriteLine("Token=" + token.Token);
        }
    }
}
