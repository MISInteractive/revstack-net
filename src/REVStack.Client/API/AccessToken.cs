using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public class AccessToken : IAccessToken
    {
        public AccessToken(string token) 
        {
            this.Token = token;
        }

        public string Token { get; set; }
    }
}
