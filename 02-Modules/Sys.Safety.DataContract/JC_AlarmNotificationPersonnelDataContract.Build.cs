using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_AlarmNotificationPersonnelInfo
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
        /// 报警配置Id
        /// </summary>
        public string AlarmConfigId
        {
            get;
            set;
        }
        /// <summary>
        /// 报警通知人员Id
        /// </summary>
        public string PersonId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID 
        /// </summary>
        public string UserID
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
        /// 用户Code
        /// </summary>
        public string UserCode
        {
            get;
            set;
        }
        
        /// <summary>
        /// check 
        /// </summary>
        public bool IsCheck
        {
            get;
            set;
        }
    }
}


