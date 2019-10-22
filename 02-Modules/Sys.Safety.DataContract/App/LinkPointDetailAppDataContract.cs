using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 测点关联设备详情
    /// </summary>
    public class LinkPointDetailAppDataContract
    {
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 断电状态文本
        /// </summary>
        public string PowerStateText { get; set; }
        /// <summary>
        /// 是否断电成功
        /// </summary>
        public bool PowerState { get; set; }
        /// <summary>
        /// 馈电状态文本
        /// </summary>
        public string FeedStateText { get; set; }
        /// <summary>
        /// 是否馈电成功
        /// </summary>
        public bool FeedState { get; set; }   
    }
}
