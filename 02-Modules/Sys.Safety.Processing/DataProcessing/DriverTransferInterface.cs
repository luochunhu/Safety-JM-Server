using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Net.Sockets;
using System.Windows.Forms;
using System.Data;
using Sys.Safety.DataContract;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Interface;

namespace Sys.Safety.Processing.DataProcessing
{
    public delegate void DriverEventHandler(MasProtocol masProtocol);
    /// <summary> 
    /// 功能描述：通信动态接口          
    /// </summary>
    public class DriverTransferInterface
    {
        public event DriverEventHandler OnDriverSendEventHandler;

        #region ----驱动属性映射----

        /// <summary>
        /// 获取驱动名称
        /// </summary>
        /// <param name="_dyobj" >实例化对象</param>
        public static string Get_Drv_StrDriverName(dynamic _dyobj)
        {
            return _dyobj.Drv_StrDriverName;
        }
        /// <summary>
        /// 获取驱动版本号
        /// </summary>
        /// <param name="_dyobj" >实例化对象</param>
        public static string Get_Drv_StrDriverVersion(dynamic _dyobj)
        {
            return _dyobj.Drv_StrDriverVersion;
        }
        /// <summary>
        /// 获取驱动文件名称
        /// </summary>
        /// <param name="_dyobj" >实例化对象</param>
        public static string Get_Drv_StrDriverSource(dynamic _dyobj)
        {
            return _dyobj.Drv_StrDriverSource;
        }
        /// <summary>
        /// 获取驱动更新时间
        /// </summary>
        /// <param name="_dyobj" >实例化对象</param>
        public static DateTime Get_Drv_DttDriverVersionTime(dynamic _dyobj)
        {
            return _dyobj.Drv_DttDriverVersionTime;
        }

        /// <summary>
        /// 获取获取驱动索引
        /// </summary>
        /// <param name="_dyobj" >实例化对象</param>
        public static decimal Get_Drv_ID(dynamic _dyobj)
        {
            return _dyobj.Drv_ID;
        }

        #endregion

        /// <summary>
        /// 获取下发数据
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="stationID">分站测点号</param>
        /// <returns></returns>
        public static void GetSendData(IDriver _dyobj, string stationID,Jc_DevInfo devInfo,ref int sendTime)
        {
            _dyobj.GetSendData(stationID, devInfo, ref sendTime);
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="data">分站号</param>
        public static void DataProc(IDriver _dyobj, MasProtocol data)
        {
            _dyobj.DataProc(data);
        }
        /// <summary>
        /// 设备中断处理
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="stationID">分站号</param>
        public static void Drv_InterruptPro(IDriver _dyobj, short stationID)
        {
            _dyobj.Drv_InterruptPro(stationID);
        }
        /// <summary>
        /// 五分钟数据处理
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="stationID">分站号</param>
        /// <param name="time">当前时间</param>
        public static void Drv_FiveMinPro(IDriver _dyobj, short stationID, DateTime time)
        {
            _dyobj.Drv_FiveMinPro(stationID, time);
        }
        /// <summary>
        /// 跨天处理
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="stationID">分站号</param>
        /// <param name="today">今天跨天时间（一般为当天23:59:59）</param>
        /// <param name="tomorrow">跨天生成新记录时间（一般为第二天00:00:00）</param>
        public static void Drv_CrossDayPro(IDriver _dyobj, short stationID, DateTime today, DateTime tomorrow)
        {
            _dyobj.Drv_CrossDayPro(stationID, today, tomorrow);
        }
        /// <summary>
        /// 系统退出处理
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="stationID">分站号</param>
        /// <param name="time">系统退出时间</param>
        public static void Drv_SystemExistPro(IDriver _dyobj, short stationID, DateTime time)
        {
            _dyobj.Drv_SystemExistPro(stationID, time);
        }
        /// <summary>
        /// 驱动预处理
        /// </summary>
        /// <param name="_dyobj">驱动</param>
        /// <param name="defInfo">被处理过的设备链表</param>
        public static void Drv_Pretreatment(IDriver _dyobj, List<Jc_DefInfo> defInfo)
        {
            _dyobj.Drv_Pretreatment(defInfo);
        }
    }
}
