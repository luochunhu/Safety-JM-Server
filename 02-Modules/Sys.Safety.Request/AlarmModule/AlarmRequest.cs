using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataContract;

namespace Sys.Safety.Request.Alarm
{
    public partial class GetReleaseAlarmRecordsRequest : Basic.Framework.Web.BasicRequest
    {
        public long Id { get; set; }
    }
    public partial class SaveConfigToDatabaseRequest : Basic.Framework.Web.BasicRequest
    {
        public string Config { get; set; }
        public SettingInfo SettingInfo { get; set; }
    }
    public partial class QueryDevClassByDevpropertRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevPropertId { get; set; }
    }

    public partial class QueryPointByDevClassRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevClassId { get; set; }
    }

    public partial class QueryPointByDevpropertRequest : Basic.Framework.Web.BasicRequest
    {
        public int DevPropertId { get; set; }
    }

    public partial class GetEnumcodeByEnumTypeIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string EnumTypeId { get; set; }
    }

    public partial class GetClassByPropertyRequest : Basic.Framework.Web.BasicRequest
    {
        public string Type { get; set; }
    }

    public partial class GetPointByClassRequest : Basic.Framework.Web.BasicRequest
    {
        public string sClass { get; set; }
    }

    public partial class GetAlarmTypeDataByPropertyRequest : Basic.Framework.Web.BasicRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public partial class GetCalibrationRecordRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public partial class UpdateCalibrationRecordRequest : Basic.Framework.Web.BasicRequest
    {
        public string ID { get; set; }
        public string csStr { get; set; }
        //public DateTime StartTime { get; set; }
        //public string  pointid { get; set; }
    }

    public partial class InsertCalibrationRecordRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BxexInfo jc_bx { get; set; }
    }

}
