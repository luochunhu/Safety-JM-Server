using System;
using System.Collections.Generic;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;

namespace Sys.Safety.Interface
{
    public delegate void DriverEventHandler(MasProtocol masProtocol);
    /// <summary> 
    /// 功能描述：驱动统一接口          
    /// </summary>
    public interface IDriver
    {
        event DriverEventHandler OnDriverSendDataEventHandler;

        #region ----驱动属性----
        /// <summary>
        /// 驱动描述(如:大分站驱动....)
        /// </summary>
        string Drv_StrDriverName { get; }

        /// <summary>
        /// 驱动源文件
        /// 驱动文件的名称,路径为Driver文件夹下
        /// 如:KJF86N_16.dll
        /// </summary>
        string Drv_StrDriverSource { get; }

        /// <summary>
        /// 驱动版本号
        /// </summary>
        string Drv_StrDriverVersion { get; }

        /// <summary>
        /// 驱动索引ID
        /// </summary>
        decimal Drv_ID { get; }

        /// <summary>
        /// 驱动更新时间
        /// </summary>
        DateTime Drv_DttDriverVersionTime { get; }

        #endregion

        /// <summary>
        /// 获取下发数据
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <returns></returns>
        void GetSendData(string stationID, Jc_DevInfo devInfo,ref int sendTime);

        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="data">分站号</param>
        void DataProc(MasProtocol data);

        /// <summary>
        /// 设备中断处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        void Drv_InterruptPro(short stationID);

        /// <summary>
        /// 五分钟数据处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="time">当前时间</param>
        void Drv_FiveMinPro(short stationID, DateTime time);

        /// <summary>
        /// 跨天处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="today">今天跨天时间（一般为当天23:59:59）</param>
        /// <param name="tomorrow">跨天生成新记录时间（一般为第二天00:00:00）</param>
        void Drv_CrossDayPro(short stationID, DateTime today, DateTime tomorrow);

        /// <summary>
        /// 系统退出处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="time">系统退出时间</param>
        void Drv_SystemExistPro(short stationID, DateTime time);

        /// <summary>
        /// 驱动预处理
        /// </summary>
        /// <param name="defInfo">被处理过的设备链表</param>
        void Drv_Pretreatment(List<Jc_DefInfo> defInfo);
    }
}
