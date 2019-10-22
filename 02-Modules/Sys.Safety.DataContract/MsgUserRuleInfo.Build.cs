using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class MsgUserRuleInfo
    {
        /// <summary>
        /// 唯一编码
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 规则Id
        /// </summary>
        public string RuleId
        {
            get;
            set;
        }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone
        {
            get;
            set;
        }
        /// <summary>
        /// 备用1
        /// </summary>
        public string B1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用2
        /// </summary>
        public string B2
        {
            get;
            set;
        }
    }
}


