using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{

    [Serializable]
    public class StationUpdateItem
    {
        /// <summary>
        /// 分站号
        /// </summary>
        public int fzh;
        /// <summary>
        /// 当前分站需要下发哪一包数据（用于补发数据包用）从1 开始
        /// </summary>
        public int nowNeedSendBuffIndex;
        /// <summary>
        /// 分站当前升级状态
        /// </summary>
        public StationUpdateState updateState;
        /// <summary>
        /// 当前下发的命令类型
        /// </summary>
        public ProtocolType protocolType;
       
        /// <summary>
        /// 当前状态详细描述
        /// </summary>
        public string msg;
        /// <summary>
        /// 分站是否远程升级
        /// </summary>
        public bool isUpdate;
        /// <summary>
        /// 最近一次下发升级数据包时间
        /// </summary>
        public DateTime lastSendTime;
        /// <summary>
        /// 当前需要下发的数据包
        /// </summary>
        public byte[] nowNeedSendBuffer;
        /// <summary>
        /// 是否需要下发Buffer
        /// </summary>
        public bool isSendBuffer;

        public object protocol;
        public uint crc;
        /// <summary>
        /// 分站设备的当前参数
        /// </summary>
        public StationWorkState stationWorkState;

        public string Info;
    }

    [Serializable]
    public class StationWorkState
    {
        /// <summary>
        /// 软件版本
        /// </summary>
        public double softVersion;
        /// <summary>
        /// 远程升级状态 (=0：分站未进入升级状态且不能接受升级请求,=1：分站未进入升级状态但可以接受升级请求,=2：分站已处于升级文件接收阶段)
        /// </summary>
        public int updateState;
        /// <summary>
        /// 设备类型
        /// </summary>
        public int devTypeID;
        /// <summary>
        /// 设备硬件版本号
        /// </summary>
        public double hardVersion;
    }
}
