using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Config
{
    public partial class ConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ConfigInfo ConfigInfo { get; set; }      
    }

	public partial class ConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ConfigInfo ConfigInfo { get; set; }      
    }

	public partial class ConfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class ConfigGetByNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string Name { get; set; }
    }

    public partial class ConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class ConfigGetDiskInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string DiskName { get; set; }
    }
    public partial class ConfigGetProcessInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string ProcessName { get; set; }
    }

    public partial class SaveInspectionInRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime SaveTime { get; set; }
    }
}
