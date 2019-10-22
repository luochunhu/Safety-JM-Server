using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Setting
{
    public partial class SettingAddRequest : Basic.Framework.Web.BasicRequest
    {
        public SettingInfo SettingInfo { get; set; }
    }

    public partial class SettingUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public SettingInfo SettingInfo { get; set; }
    }

    public partial class SettingDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class SettingGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class SettingGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class GetSettingByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        public string StrKey { get; set; }
    }
    public partial class SaveSettingForConditionRequest : Basic.Framework.Web.BasicRequest
    {
        public int? State { get; set; }
        public SettingInfo SettingInfo { get; set; }
    }

    public partial class GetSettingCacheByKeyRequest: Basic.Framework.Web.BasicRequest
    {
        public string Key { get; set; }
    }
}
