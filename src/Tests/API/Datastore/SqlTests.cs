using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;

namespace Tests.API.Datastore
{
    [TestClass]
    public class SqlTests
    {
        [TestMethod]
        public void DropCommandTests()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;
            string query = "drop class reviews";
            request.Datastore.Command(query);
        }
    }
}
