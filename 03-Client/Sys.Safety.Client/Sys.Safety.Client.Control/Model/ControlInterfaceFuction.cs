using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data ;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.StaionControlHistoryData;
using Sys.Safety.Request.StaionHistoryData;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Control;

namespace Sys.Safety.Client.Control.Model
{
    public  class ControlInterfaceFuction
    {
        private static readonly IControlService ControlService = ServiceFactory.Create<IControlService>();
        private static readonly IPointDefineService PointDefineService = ServiceFactory.Create<IPointDefineService>();
        private static readonly INetworkModuleService NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
        private static readonly IStaionHistoryDataService StaionHistoryDataService = ServiceFactory.Create<IStaionHistoryDataService>();
        private static readonly IStaionControlHistoryDataService StaionControlHistoryDataService = ServiceFactory.Create<IStaionControlHistoryDataService>();

        /// <summary>
        /// 获取所有绑定电源箱的分站  20170117
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDyxFz()
        {
            //DataTable   msg = null;
            //try
            //{
            //    if (StaticClass.ServerConet)
            //    {
            //        msg = serverquery.GetDyxFz();
            //    }
            //}
            //catch
            //{
            //    OprFuction.SetServerConct();
            //}
            var res = ControlService.GetDyxFz();
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 获取所有绑定电源箱的分站  20170118
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDyxMac()
        {
            //DataTable msg = null;
            //try
            //{
            //    if (StaticClass.ServerConet)
            //    {
            //        msg = serverquery.GetDyxMac();
            //    }
            //}
            //catch
            //{
            //    OprFuction.SetServerConct();
            //}

            var res = ControlService.GetDyxMac();
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 根据分站号获取电源箱地址号
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static List<string> GetSubstationPowerBoxAddress(string fzh)
        {
            var req = new GetAllPowerBoxAddressRequest
            {
                Fzh = fzh
            };
            var res = PointDefineService.GetAllPowerBoxAddress(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 根据分站号获取电源箱地址号
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static List<string> GetSwitchPowerBoxAddress(string mac)
        {
            var req = new GetAllPowerBoxAddressByMacRequest()
            {
                Mac = mac
            };
            var res = NetworkModuleService.GetAllPowerBoxAddress(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 获取所有分站
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetAllSubstation()
        {
            var req = new PointDefineGetByDevpropertIDRequest
            {
                DevpropertID = 0
            };
            var res = PointDefineService.GetPointDefineCacheByDevpropertID(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> GetAllSwitch()
        {
            var res = NetworkModuleService.GetAllNetworkModuleCache();
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        public static List<Jc_DefInfo> GetAllPointByStation(int fzh)
        {
            PointDefineGetByStationIDRequest pointDefineGetByStationIDRequest = new PointDefineGetByStationIDRequest();
            pointDefineGetByStationIDRequest.StationID = fzh;
            var res = PointDefineService.GetPointDefineCacheByStationID(pointDefineGetByStationIDRequest);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 根据分站号获取所有电源箱信息
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static GetSubstationAllPowerBoxInfoResponse GetSubstationPowerBoxInfo(string fzh)
        {
            var req = new GetSubstationAllPowerBoxInfoRequest
            {
                Fzh = fzh
            };
            var res = PointDefineService.GetSubstationAllPowerBoxInfo(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 根据mac地址获取所有电源箱信息
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static GetSwitchAllPowerBoxInfoResponse GetSwitchPowerBoxInfo(string mac)
        {
            var req = new GetSwitchAllPowerBoxInfoRequest
            {
                Mac = mac
            };
            var res = NetworkModuleService.GetSwitchAllPowerBoxInfo(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 根据分站号和时间获取分站的5分钟历史数据
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<GetSubstationHistoryRealDataByFzhTimeResponse> GetSubstationHistoryRealDataByFzhTime(string fzh, DateTime time)
        {
            var req = new GetSubstationHistoryRealDataByFzhTimeRequest
            {
                Fzh = fzh,
                Time = time.ToString()
            };
            var res = StaionHistoryDataService.GetSubstationHistoryRealDataByFzhTime(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 发送获取或取消获取5分钟历史数据命令
        /// </summary>
        /// <param name="lisSc"></param>
        public static void SendQueryHistoryRealDataRequest(List<StationControlItem> lisSc)
        {
            var req = new HistoryRealDataRequest
            {
                controlItems = lisSc
            };
            PointDefineService.SendQueryHistoryRealDataRequest(req);
        }

        /// <summary>
        /// 根据分站号和时间获取分站的控制历史数据
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<GetStaionControlHistoryDataByByFzhTimeResponse> GetStaionControlHistoryDataByByFzhTime(string fzh, DateTime time)
        {
            var req = new GetStaionControlHistoryDataByByFzhTimeRequest
            {
                Fzh = fzh,
                Time = time.ToString()
            };
            var res = StaionControlHistoryDataService.GetStaionControlHistoryDataByByFzhTime(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 发送获取或取消获取控制历史数据命令
        /// </summary>
        /// <param name="lisSc"></param>
        public static void SendQueryHistoryControlRequest(List<StationControlItem> lisSc)
        {
            var req = new GetHistoryControlRequest
            {
                controlItems = lisSc
            };
            PointDefineService.SendQueryHistoryControlRequest(req);
        }
    }
}
