using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Define.Model
{
    /// <summary>
    /// 分站图形化定义初始加载数据
    /// </summary>
    public class StaionInitInfo
    {
        /// <summary>
        /// 是否移除之前的图元信息
        /// </summary>
        public bool IsRemoveAllVectorLayerOverlay { get; set; }
        /// <summary>
        /// 加载的测点信息
        /// </summary>
        public string LoadPointString { get; set; }
        /// <summary>
        /// 加载的测点信息
        /// </summary>
        public List<AddStationTopologyDefineLineObject> AddStationTopologyDefineLineList { get; set; }
        /// <summary>
        ///是否加载ToolTips
        /// </summary>
        public bool IsLoadToolTip { get; set; }
    }
}
