using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class StaionHistoryDataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Fzh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Kh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Dzh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Point
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            get;
            set;
        }
        /// <summary>
        /// 分级报警标记
        /// </summary>
        public int GradingAlarmLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealData
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Voltage
        {
            get;
            set;
        }
        /// <summary>
        /// 设备型号编码
        /// </summary>
        public int DeviceTypeCode { get; set; }      
        /// <summary>
        /// 回控状态（0无电 1无电）;  人员定位：是否呼叫
        /// </summary>
        public string FeedBackState { get; set; }
        /// <summary>
        /// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 复电失败） ；  人员定位：是否为补传
        /// </summary>
        public string FeedState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SaveTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DataTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark5
        {
            get;
            set;
        }
    }
}


