using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Login
{
    public partial class LoginRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, object> loginContext { get; set; }      
    }
    public partial class LoginOutRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserName { get; set; }
    }

    public partial class UserLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
