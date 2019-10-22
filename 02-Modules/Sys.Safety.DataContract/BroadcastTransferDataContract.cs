using Sys.Safety.DataContract.CommunicateExtend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 人员定位传输对象
    /// </summary>
    public partial class BroadcastTransferInfo
    {
        /// <summary>
        /// 测点编号
        /// </summary>
        public string AddNum
        {
            get;
            set;
        }        
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Wz
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Groupname
        {
            get;
            set;
        }
        /// <summary>
        /// 当前状态(正整数)
        /// </summary>
        public string State
        {
            get;
            set;
        }        
        /// <summary>
        /// MAC地址
        /// </summary>
        public string Mac
        {
            get;
            set;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP
        {
            get;
            set;
        }        
    }
}
