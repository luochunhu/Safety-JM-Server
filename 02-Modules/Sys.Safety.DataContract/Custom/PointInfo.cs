using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.Custom
{
    public class PointInfo : Kvp
    {
        /// <summary>位置
        /// 
        /// </summary>
        public string Location { get; set; }

        /// <summary>类型
        /// 
        /// </summary>
        public string Type { get; set; }
    }
}
