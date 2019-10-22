using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class R_DeptInfo
    {
        /// <summary>
        /// 部门负责人职务
        /// </summary>
        public string ManagerTitle { get; set; }
        /// <summary>
        /// 部门负责人姓名
        /// </summary>
        public string ManagerName { get; set; }
    }
}
