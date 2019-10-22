using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_AlarmNotificationPersonnelConfigInfo : Basic.Framework.Web.BasicInfo
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
        /// 分析模型名称
        /// </summary>
        public string AnalysisModeName
        {
            get;
            set;
        }
        /// <summary>
        /// 报警方式
        /// </summary>
        public string AlarmType
        {
            get;
            set;
        }
        /// <summary>
        /// 报警颜色
        /// </summary>
        public string AlarmColor
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


