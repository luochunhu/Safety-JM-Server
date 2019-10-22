using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class KJ_AddresstypeInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 地点类型名称
        /// </summary>
        public string Addresstypename
        {
            get;
            set;
        }
        /// <summary>
        /// 地点类型描述
        /// </summary>
        public string Addresstypedesc
        {
            get;
            set;
        }
        /// <summary>
        /// 创建或者更新时间
        /// </summary>
        public string Createupdatetime
        {
            get;
            set;
        }
        /// <summary>
        /// 备注1
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 备注2
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 备注3
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
    }
}


