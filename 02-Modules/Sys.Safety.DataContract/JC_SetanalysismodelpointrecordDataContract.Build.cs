using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_SetAnalysisModelPointRecordInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 分析模型ID
        /// </summary>
        public string AnalysisModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 表达式配置ID
        /// </summary>
        public string ExpressionConfigId { get; set; }
        /// <summary>
        /// 表达式ID
        /// </summary>
        public string ExpressionId
        {
            get;
            set;
        }
        /// <summary>
        /// 参数Id
        /// </summary>
        public string ParameterId
        {
            get;
            set;
        }
        /// <summary>
        /// 因子Id
        /// </summary>
        public string FactorId
        {
            get;
            set;
        }
        /// <summary>
        /// 测点Id
        /// </summary>
        public string PointId
        {
            get;
            set;
        }
    }
}


