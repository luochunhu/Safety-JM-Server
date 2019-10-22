using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Define.Model
{
    /// <summary>
    /// 绘制分站与传感器连线对象
    /// </summary>
    public class AddStationTopologyDefineLineObject
    {
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 分支号
        /// </summary>
        public string BranchNumBer { get; set; }
    }
}
