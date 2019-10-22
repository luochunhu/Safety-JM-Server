using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataContract;

namespace Sys.Safety.Request.RealMessage
{
    public partial class RemoteUpgradeCommandRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
        public byte Sjml { get; set; }
        public byte SendD { get; set; }
        public byte SjState { get; set; }
    }
    public partial class RemoteGetShowTbRequest : Basic.Framework.Web.BasicRequest
    {
        public string Fzh { get; set; }
    }
    public partial class RemoteUpdateStrtOrStopRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 升级0、结束升级1
        /// </summary>
        public int? Type { get; set; }
    }
    public partial class GetCustomPagePointRequest : Basic.Framework.Web.BasicRequest
    {
        public int Page { get; set; }
    }
    public partial class ReadConfigRequest : Basic.Framework.Web.BasicRequest
    {
        public string KeyName { get; set; }
    }
    public partial class UpdateAlarmStepRequest : Basic.Framework.Web.BasicRequest
    {
        public string TableName { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
    }
    public partial class GetbxpointRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime Time { get; set; }
    }
    public partial class SavePointRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime? Time { get; set; }
        public string PointStr { get; set; }
    }
    public partial class GetZKPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string Point { get; set; }
    }

    public partial class GetPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string Point { get; set; }
    }

    public partial class GetFZJXControlRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 分站号
        /// </summary>
        public int Fzh { get; set; }
    }

    public partial class SaveConfigRequest : Basic.Framework.Web.BasicRequest
    {
        public List<ConfigInfo> ConfigInfoList { get; set; }
    }

    public partial class SaveCustomPagePointsRequest : Basic.Framework.Web.BasicRequest
    {
        public int? Page { get; set; }
        public DataTable dt { get; set; }
    }

    public partial class GetRunLogsRequest : Basic.Framework.Web.BasicRequest
    {
        public long UserId { get; set; }
    }

    public partial class GetMaintenanceHistoryByPointIdRequst : Basic.Framework.Web.BasicRequest
    {
        public long? PointId { get; set; }
    }

    public partial class GetRealDataRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime LastRefreshRealDataTime { get; set; }
    }
    public partial class GetRunRecordListByCounterRequest : Basic.Framework.Web.BasicRequest
    {
        public long Counter { get; set; }
    }
}
