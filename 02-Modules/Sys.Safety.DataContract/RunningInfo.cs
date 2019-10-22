using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
   

    [DataContract]
    [Serializable]
    public class RunningInfo
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        [DataMember]
        public string CustomerInfo { get; set; }
        /// <summary>
        /// 主服务器状态
        /// 0：连接数采程序失败
        /// 1：连接数采程序成功
        /// </summary>
        [DataMember]
        public int MasterServerState { get; set; }

        /// <summary>
        /// 备服务器状态
        /// 0：不正常
        /// 1：正常
        /// </summary>
        [DataMember]
        public int SlaveServerState { get; set; }

        /// <summary>
        /// 主网关（数采）状态
        /// 0：不正常
        /// 1：正常
        /// </summary>
        [DataMember]
        public int MasterDataCollectorState { get; set; }

        /// <summary>
        /// 备网关（数采）状态
        /// 0：不正常
        /// 1：正常
        /// </summary>
        [DataMember]
        public int SlaveDataCollectorState { get; set; }

        /// <summary>
        /// 数采最后接收数据时间
        /// </summary>
        [DataMember]
        public DateTime LastReceiveTime { get; set; }

        /// <summary>
        /// 0.连接数据库失败
        /// 1.连接数据库正常
        /// </summary>
        [DataMember]
        public int DbState { get; set; }

        /// <summary>
        /// 占用磁盘空间（单位M）
        /// </summary>
        [DataMember]
        public decimal DbSize { get; set; }

        /// <summary>
        /// 是否使用双机热备
        /// 0.未使用
        /// 1.使用双机热备
        /// </summary>
        [DataMember]
        public bool IsUseHA { get; set; }

        /// <summary>
        /// 当前是主机还是备机1：主机，2：备机
        /// </summary>
        [DataMember]
        public int IsMasterOrBackup { get; set; }
        /// <summary>
        /// 双机热备工作状态 未知--1，网络恢复-0，网络中断,IP正常-1，连接中断-2，本机监测程序退出-3，远程机未运行，本机程序正常-4，远程机未运行，本机程序未运行-5，远程机程序正常，本机程序未运行-6，远程机程序正常，本机程序也处于运行状态-7，网络中断,IP异常-8  20180123
        /// </summary>
        [DataMember]
        public int BackUpWorkState { get; set; }

        /// <summary>
        /// 是否授权过期
        /// </summary>
        [DataMember]
        public bool AuthorizationExpires { get; set; }
    }

    [DataContract]
    [Serializable]
    public class PorcessInfo
    {
        [DataMember]
        public string ProcessName { get; set; }
        [DataMember]
        public decimal CpuUsageRate { get; set; }
        [DataMember]
        public decimal MemoryUsageSize { get; set; }
    }

    [DataContract]
    [Serializable]
    public class HardDiskInfo
    {
        [DataMember]
        public string DiskName { get; set; }
        [DataMember]
        public long TotalSize { get; set; }
        [DataMember]
        public long TotalFreeSize { get; set; }
        [DataMember]
        public long TotalUsageSize { get; set; }
        [DataMember]
        public int TotalUsageRate { get; set; }
    }
}
