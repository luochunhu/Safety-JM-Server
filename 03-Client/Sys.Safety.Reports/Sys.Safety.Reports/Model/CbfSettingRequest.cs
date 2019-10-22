using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Setting;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Reports.Model
{
    public static class CbfSettingRequest
    {
        private static readonly ISettingService _settingService = ServiceFactory.Create<ISettingService>();

        public static bool IfMergeRecords()
        {
            var req = new GetSettingCacheByKeyRequest
            {
                Key = "RecordsMerging"
            };
            var res = _settingService.GetSettingCacheByKey(req);
            var data = res.Data;
            if (data == null || data.StrValue == "1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
