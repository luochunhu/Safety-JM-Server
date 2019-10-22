using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 交换机信息
    /// </summary>
    public class SwitcheAppDataContract
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        public bool Alarm { get; set; }
        /// <summary>
        /// 交换机下网络模块列表
        /// </summary>
        public List<NetworkModuleAppDataContract> NetworkModuleObjList { get; set; }
    }
}
