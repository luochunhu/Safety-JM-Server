using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class R_PersoninfInfo
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string deptName
        {
            get;
            set;
        }
        /// <summary>
        /// 职务描述
        /// </summary>
        public string zwDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 工种描述
        /// </summary>
        public string gzDesc
        {
            get;
            set;
        }
    }
}


