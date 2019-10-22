using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_FactorInfo : Basic.Framework.Web.BasicInfo
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
        /// 1-模拟量2-开关量
        /// </summary>
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 分析因子名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 含义说明
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 调用方法名称
        /// </summary>
        public string CallMethodName
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


