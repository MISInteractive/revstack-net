using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;

namespace Tests.API.Datastore
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void GetClass()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;
        }

        [TestMethod]
        public void IfClassExistsThrowError()
        {

        }

        [TestMethod]
        public void CreateAndDeleteClass()
        {

        }
    }
}
