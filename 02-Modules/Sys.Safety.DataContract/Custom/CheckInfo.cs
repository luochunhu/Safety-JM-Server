using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.DataContract.Custom
{
    public class CheckInfo : BasicInfo
    {
        public CheckInfo()
        {
            Check = false;
        }

        public bool Check { get; set; }
    }
}
