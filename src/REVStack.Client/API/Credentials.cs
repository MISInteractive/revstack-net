using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public class Credentials : ICredentials
    {
        public Credentials() { }

        public Credentials(string appId, IAccessToken accessToken)
        {
            AppId = appId;
            AccessToken = accessToken;
        }

        public Credentials(string appId, string username, string password, IAccessToken accessToken)
        {
            AppId = appId;
            Username = username;
            Password = password;
            AccessToken = accessToken;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the appId.
        /// </summary>
        /// <value>
        /// The AppId.
        /// </value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the AccessToken.
        /// </summary>
        /// <value>
        /// The AccessToken.
        /// </value>
        public IAccessToken AccessToken { get; set; }
    }
}
