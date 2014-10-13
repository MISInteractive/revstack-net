using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public interface ICredentials
    {
        string Username { get; set; }
        string Password { get; set; }
        string AppId { get; set; }
        IAccessToken AccessToken { get; set; }
    }
}
