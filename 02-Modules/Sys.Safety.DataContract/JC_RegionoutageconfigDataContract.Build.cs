using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_RegionOutageConfigInfo : Basic.Framework.Web.BasicInfo
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
        /// 分析模型Id
        /// </summary>
        public string AnalysisModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointId
        {
            get;
            set;
        }
        /// <summary>
        /// 解控模型Id
        /// </summary>
        public string RemoveModelId { get; set; }

        /// <summary>
        /// 分析不满足条件时是否解除,0-不解除 1-解除
        /// </summary>
        public int IsRemoveControl { get; set; }


        /// <summary>
        /// 控制状态（1代表控制，0代表不控制）
        /// </summary>
        public int ControlStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedTime
        {
            get;
            set;
        }
    }
}


