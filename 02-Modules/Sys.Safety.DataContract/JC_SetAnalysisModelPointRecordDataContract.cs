using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 分析模型测点关联，关联ID对应名字.
    /// </summary>
    public partial class JC_SetAnalysisModelPointRecordInfo
    {
        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string AnalysisModelName
        {
            get;
            set;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expresstion
        {
            get;
            set;
        }
        /// <summary>
        /// 表达式文本
        /// </summary>
        public string ExpresstionText
        {
            get;
            set;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName
        {
            get;
            set;
        }

        /// <summary>
        /// 设备属性Id 0-分站1-模拟量2-开关量3-控制量4-累计量5-导出量 6-其他 7-路灯
        /// </summary>
        public int DevicePropertyId { get; set; }

        /// <summary>
        /// 因子调用方法名称
        /// </summary>
        public string CallMethodName { get; set; }

        /// <summary>
        /// 表达式持续时间
        /// </summary>
        public int ContinueTime { get; set; }

        /// <summary>
        /// 表达式最大持续时间
        /// </summary>
        public int MaxContinueTime { get; set; }

         /// <summary>
        /// 大数据分析配置ID
        /// </summary>
        public string LargedataAnalysisConfigId { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Wz { get; set; }

        /// <summary>
        /// 测点是否激活
        /// </summary>
        public string PointActivity { get; set; }


        private int _DevTypeId = -1;
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public int DevTypeId {
            get { return _DevTypeId; }
            set { _DevTypeId = value; }
        }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DevTypeName { get; set; }
    }
}
