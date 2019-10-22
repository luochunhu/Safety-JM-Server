using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_AnalyticalExpressionInfo : Basic.Framework.Web.BasicInfo
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
        /// 表达式操作记录json
        /// </summary>
        public string ExpresstionOperationRecord
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
        /// 持续时间
        /// </summary>
        public int ContinueTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最大持续时间
        /// </summary>
        public int MaxContinueTime
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


