using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.DeviceDefine
{
    public partial class DeviceDefineAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DevInfo Jc_DevInfo { get; set; }      
    }
    public partial class DeviceDefinesRequest : Basic.Framework.Web.BasicRequest
    {
        public List<Jc_DevInfo> Jc_DevsInfo { get; set; }
    }

    public partial class DeviceDefineUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DevInfo Jc_DevInfo { get; set; }      
    }

    public partial class DeviceDefineDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class DeviceDefineGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class DeviceDefineGetByDevIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string DevId { get; set; }
    }
    public partial class DeviceDefineGetByDevpropertIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevpropertID { get; set; }
    }
    public partial class DeviceDefineGetByDevpropertIDDevModelIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevpropertID { get; set; }
        public int DevModelID { get; set; }
    }
    public partial class DeviceDefineGetByDevClassIDRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevClassID { get; set; }
    }

    public partial class DeviceDefineGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
