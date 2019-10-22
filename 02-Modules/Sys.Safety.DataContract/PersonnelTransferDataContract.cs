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
    public partial class PersonnelTransferInfo
    {
        /// <summary>
        /// 测点编号
        /// </summary>
        public string point
        {
            get;
            set;
        }
        /// <summary>
        /// 分站号(正整数)
        /// </summary>
        public string fzh
        {
            get;
            set;
        }
        /// <summary>
        /// 通道号(正整数)
        /// </summary>
        public string kh
        {
            get;
            set;
        }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string wz
        {
            get;
            set;
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string type
        {
            get;
            set;
        }
        /// <summary>
        /// 当前状态(正整数)
        /// </summary>
        public string state
        {
            get;
            set;
        }
        /// <summary>
        /// 当前人数
        /// </summary>
        public string k1
        {
            get;
            set;
        }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string jckz1
        {
            get;
            set;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string jckz2
        {
            get;
            set;
        }
        /// <summary>
        ///采集时间
        /// </summary>
        public string zts
        {
            get;
            set;
        }
        /// <summary>
        ///是否报警（1：报警，0：不报警）
        /// </summary>
        public string alarm
        {
            get;
            set;
        }
    }
}
