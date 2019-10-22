using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Powerboxchargehistory;
using Sys.Safety.DataContract.UserRoleAuthorize;

namespace Sys.Safety.Client.Control.Model
{
    /// <summary>
    /// 电源管理【测点定义及控制相关接口】
    /// </summary>
    public class ChargeMrg
    {
        private static readonly IPointDefineService PointDefineService = ServiceFactory.Create<IPointDefineService>();

        private static readonly INetworkModuleService NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();

        private static readonly IManualCrossControlService ManualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();

        private static readonly IPowerboxchargehistoryService powerboxchargehistoryService = ServiceFactory.Create<IPowerboxchargehistoryService>();

        /// <summary>
        /// 获取电源箱状态
        /// </summary>
        /// <param name="m">设备类型</param>
        /// <param name="fzhormac">分站传分站号，交换机传MAC</param>
        public static void sendD(int m, string fzhormac)
        {
            SendDComReqest sendDComReqest = new SendDComReqest
            {
                queryBatteryRealDataItems = new List<BatteryControlItem>()
                {
                    new BatteryControlItem()
                    {
                        DevProID = m,
                        FzhOrMac = fzhormac,
                        controlType = 0
                    }
                }
            };

            var res = PointDefineService.SendQueryBatteryRealDataRequest(sendDComReqest);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }


        /// <summary>根据测点编号查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByCodeCache(string PointCode)
        {
            //return DEFService.QueryPointByCodeCache(PointCode);
            var req = new PointDefineGetByPointRequest
            {
                Point = PointCode
            };
            var res = PointDefineService.GetPointDefineCacheByPoint(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        public static Jc_MacInfo QueryJcMac(string mac)
        {
            //IJC_MACService DEFService = ServiceFactory.CreateService<IJC_MACService>();
            //return DEFService.QueryMACByCode(PointCode);
            //var req = new NetworkModuleCacheGetByConditonRequest
            //{
            //    Predicate = a => a.MAC == PointCode && a.InfoState != InfoState.Delete
            //};
            //var res = NetworkModuleService.GetNetworkModuleCacheByDynamicCondition(req);
            var req = new NetworkModuleGetByMacRequest
            {
                Mac = mac
            };
            var res = NetworkModuleService.GetNetworkModuleCacheByMac(req);

            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data.FirstOrDefault();
        }

        /// <summary>
        /// 通过被控点和控制类型查询放电信息   //此方法在实时模块也在调用，目前框架不支持多线程同时调用同一服务，修改为不同的接口  20170425
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZbyInf(int Type, string BkPoint)
        {
            //IJC_JCSDKZServiceInThread JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZServiceInThread>();
            //return JCSDKZService.QueryJCSDKZbyInf(Type, BkPoint);
            //var req = new ManualCrossControlCacheGetByConditionRequest
            //{
            //    Predicate = a => a.Bkpoint == BkPoint && a.Type == Type && a.InfoState != InfoState.Delete
            //};
            //var res = ManualCrossControlService.GetManualCrossControlByDynamicCondition(req);
            var req = new ManualCrossControlGetByTypeBkPointRequest
            {
                Type = Type,
                BkPoint = BkPoint
            };
            var res = ManualCrossControlService.GetManualCrossControlByTypeBkPoint(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 添加控制配置缓存对象 包括更新
        /// </summary>
        /// <param name="item"></param>
        /// 
        public static bool AddJC_JCSDKZCache(Jc_JcsdkzInfo item)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.AddJC_JCSDKZCache(item);
            var req = new ManualCrossControlAddRequest
            {
                ManualCrossControlInfo = item
            };
            var res = ManualCrossControlService.AddManualCrossControl(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.IsSuccess;
        }

        /// <summary>
        /// 批量删除控制配置缓存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool DelJC_JCSDKZCache(List<Jc_JcsdkzInfo> items)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.DelJCSKZs(items);
            var req = new ManualCrossControlsRequest
            {
                ManualCrossControlInfos = items
            };
            var res = ManualCrossControlService.DeleteManualCrossControls(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.IsSuccess;
        }

        /// <summary>
        /// 获取分站电源箱信息
        /// </summary>
        /// <param name="add"></param>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static BatteryItem GetSubstationBatteryInfo(string add, string fzh)
        {
            var req = new GetSubstationBatteryInfoRequest()
            {
                Address = add,
                Fzh = fzh
            };
            var res = PointDefineService.GetSubstationBatteryInfo(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 获取交换机电源箱信息
        /// </summary>
        /// <param name="add"></param>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static BatteryItem GetSwitchBatteryInfo(string add, string mac)
        {
            var req = new GetSwitchBatteryInfoRequest()
            {
                Address = add,
                Mac = mac
            };
            var res = NetworkModuleService.GetSwitchBatteryInfo(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 交换机电源箱控制
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="controlType"></param>
        public static void SendSwitchesDControl(string mac, byte controlType)
        {
            var req = new SwitchesDControlRequest
            {
                swichControlItems = new List<SwichControlItem>()
                {
                    new SwichControlItem()
                    {
                        mac = mac,
                        controlType = controlType
                    }
                }
            };
            var res = PointDefineService.SendSwitchesDControl(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 分站电源箱控制   0不进行操作，1取消维护性放电，2维护性放电
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="controlType"></param>
        public static void SendStationDControl(ushort fzh, byte controlType)
        {
            var req = new StationDControlRequest
            {
                controlItems = new List<StationControlItem>()
                {
                    new StationControlItem()
                    {
                        fzh = fzh,
                        controlType = controlType
                    }
                }
            };
            var res = PointDefineService.SendStationDControl(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 下发获取分站基础信息
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="getSoftwareVersions"></param>
        /// <param name="getHardwareVersions"></param>
        /// <param name="getDeviceSoleCoding"></param>
        public static void QueryDeviceInfoRequest(string fzh, byte getSoftwareVersions, byte getHardwareVersions, byte getDeviceSoleCoding)
        {
            var req = new DeviceInfoRequest
            {
                deviceInfoRequestItems = new List<DeviceInfoRequestItem>()
                {
                    new DeviceInfoRequestItem()
                    {
                        Fzh = Convert.ToUInt16(fzh),
                        controlType = 1
                    }
                }
            };
            var res = PointDefineService.QueryDeviceInfoRequest(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 获取分站基础信息
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static GetSubstationBasicInfoResponse GetSubstationBasicInfo(string fzh)
        {
            var req = new GetSubstationBasicInfoRequest
            {
                Fzh = fzh,
            };
            var res = PointDefineService.GetSubstationBasicInfo(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
        /// <summary>
        /// 写放电记录
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="Mac"></param>
        public static void AddPowerboxchargehistory(int fzh, string mac)
        {
            PowerboxchargehistoryAddRequest powerboxchargehistoryRequest = new PowerboxchargehistoryAddRequest();
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo = new PowerboxchargehistoryInfo();
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.Fzh = fzh.ToString();
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.Mac = mac;
            ClientItem clientItem = new ClientItem();
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            }
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.UserName = clientItem.UserName;
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.Stime = DateTime.Now;
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.DischargeStime = DateTime.Parse("1900-01-01 00:00:00");
            //if (!string.IsNullOrEmpty(mac))
            //{//交换机放电，直接更新放电开始时间
            //    powerboxchargehistoryRequest.PowerboxchargehistoryInfo.DischargeStime = DateTime.Now;
            //}
            powerboxchargehistoryRequest.PowerboxchargehistoryInfo.DischargeEtime = DateTime.Parse("1900-01-01 00:00:00");
            powerboxchargehistoryService.AddPowerboxchargehistory(powerboxchargehistoryRequest);
        }
        public static void MacPowerboxchargehistoryUpdate(string mac)
        {
            List<PowerboxchargehistoryInfo> powerboxchargehistoryList = powerboxchargehistoryService.GetPowerboxchargehistoryByFzhOrMac(new PowerboxchargehistoryGetByFzhOrMacRequest()
            {
                Mac = mac
            }).Data;
            if (powerboxchargehistoryList.Count > 0)
            {
                foreach (PowerboxchargehistoryInfo powerboxcharge in powerboxchargehistoryList)
                {
                    if (powerboxcharge.DischargeEtime < DateTime.Parse("2000-01-01"))
                    {
                        powerboxcharge.Etime = DateTime.Now;
                        //powerboxcharge.DischargeEtime = DateTime.Now;
                        PowerboxchargehistoryUpdate(powerboxcharge);
                    }
                }
            }
        }
        public static void PowerboxchargehistoryUpdate(PowerboxchargehistoryInfo powerboxcharge)
        {
            PowerboxchargehistoryUpdateRequest powerboxchargehistoryUpdateRequest = new PowerboxchargehistoryUpdateRequest();
            powerboxchargehistoryUpdateRequest.PowerboxchargehistoryInfo = powerboxcharge;
            powerboxchargehistoryService.UpdatePowerboxchargehistory(powerboxchargehistoryUpdateRequest);
        }
    }
}
