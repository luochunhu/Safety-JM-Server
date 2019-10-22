using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.UserRoleAuthorize
{
   public class RightItem
    {
        /// <summary>
        /// 请求库编码
        /// </summary>
        public string RightCode { get; set; }
        /// <summary>
        /// 请求库名称
        /// </summary>
        public string RightName { get; set; }
        /// <summary>
        /// 请求库分组编码
        /// </summary>
        public string RightGroupCode { get; set; }
        /// <summary>
        /// 请求库分组名称
        /// </summary>
        public string RightGroupName { get; set; }
    }
}
