using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class ShortCutMenuInfo
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// 菜单父级编码
        /// </summary>
        public string MenuParentCode { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuImage { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        public string RequestCode { get; set; }
    }
}
