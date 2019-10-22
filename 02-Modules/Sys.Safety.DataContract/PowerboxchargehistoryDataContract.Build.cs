using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class PowerboxchargehistoryInfo
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
        /// 分站号
        /// </summary>
        public string Fzh
        {
            get;
            set;
        }
        /// <summary>
        /// 交换机mac
        /// </summary>
        public string Mac
        {
            get;
            set;
        }
        /// <summary>
        /// 执行放电操作时间
        /// </summary>
        public DateTime Stime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束放电操作时间
        /// </summary>
        public DateTime Etime
        {
            get;
            set;
        }
        /// <summary>
        /// 放电开始时间
        /// </summary>
        public DateTime DischargeStime
        {
            get;
            set;
        }
        /// <summary>
        /// 放电结束时间
        /// </summary>
        public DateTime DischargeEtime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
    }
}


