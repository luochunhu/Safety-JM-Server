using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.App
{
    /// <summary>
    /// 实时数据请求参数
    /// </summary>
    public partial class RealDataRequest
    {
        /// <summary>
        /// 设备性质编码(交换机为-1)
        /// </summary>
        public string CharacterCode { get; set; }
        /// <summary>
        /// 种类编号(全部-1)
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 设备类型编号(全部-1)
        /// </summary>
        public string ModelCode { get; set; }
        /// <summary>
        /// 设备状态编号(全部-1)
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// 报警类型(全部-1 正常0 异常1)
        /// </summary>
        public string StateType { get; set; }
    }

    public partial class RealDataGetDetialRequest 
    {
        public string ID { get; set; }
    }

    public partial class SwitcheGetRequest 
    {
        public string ID { get; set; }
    }

    public partial class NetworkModuleAppGetRequest 
    {
        public string ID { get; set; }
    }

    public partial class LogonRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public partial class ModifyUserPasswordRequest 
    {
        /// <summary>
        ///  用户编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassWord { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassWord { get; set; }
    }

    public partial class ChartGetRequest 
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }      
    }

    public partial class GetDayReportRequest
    {       
        /// <summary>
        /// 查询时间
        /// </summary>
        public string Time { get; set; }
       
    }

    public partial class RunLogGetRequest 
    {
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }

        /// <summary>
        /// 分站ID
        /// </summary>
        public string StateionID { get; set; }

        /// <summary>
        /// 设备种类ID
        /// </summary>
        public string DevTypeID { get; set; }


        /// <summary>
        /// 时间
        /// </summary>
        public string StartTime { get; set; }


        /// <summary>
        /// 返回记录数
        /// </summary>
        public int PageSize { get; set; }
    }

    public partial class PointRunLogGetRequest
    {
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string STime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string ETime { get; set; }

    }

    public partial class AlarmRecordEndRequest 
    {
        public string AlarmId { get; set; }

        public DateTime Stime { get; set; }

        public DateTime Etime { get; set; }

        public string AlarmReason { get; set; }

        public string AlarmMeasure { get; set; }

        public string AlarmProcessPerson { get; set; }
    }

    public partial class AlarmHandleEndRequest 
    {
        public string AlarmId { get; set; }

        public DateTime Etime { get; set; }

        public string AlarmReason { get; set; }

        public string AlarmMeasure { get; set; }

        public string AlarmProcessPerson { get; set; }
    }
}
