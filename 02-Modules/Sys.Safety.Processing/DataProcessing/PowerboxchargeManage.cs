using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.Powerboxchargehistory;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 电源箱充放电统计
    /// </summary>
    public class PowerboxchargeManage
    {
        private readonly IPowerboxchargehistoryService powerboxchargehistoryService = ServiceFactory.Create<IPowerboxchargehistoryService>();
        private readonly IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        private readonly INetworkModuleService networkModuleService = ServiceFactory.Create<INetworkModuleService>();
        private Thread thread = null;

        private static volatile PowerboxchargeManage getInstance;
        protected static readonly object obj = new object();

        public static PowerboxchargeManage Instance
        {
            get
            {
                if (getInstance == null)
                {
                    lock (obj)
                    {
                        if (getInstance == null)
                        {
                            getInstance = new PowerboxchargeManage();
                        }
                    }
                }
                return getInstance;
            }
        }
        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(PowerboxchargeStaticThread);
                thread.IsBackground = true;
            }
            thread.Start();
        }
        public void PowerboxchargeStaticThread()
        {
            DateTime timeNow = DateTime.Now;
            List<PowerboxchargehistoryInfo> PowerboxchargehistoryList = new List<PowerboxchargehistoryInfo>();
            List<Jc_DefInfo> StationList = new List<Jc_DefInfo>();
            while (true)
            {
                try
                {

                    timeNow = DateTime.Now;
                    PowerboxchargehistoryGetByStimeRequest powerboxchargehistoryRequest = new PowerboxchargehistoryGetByStimeRequest();
                    powerboxchargehistoryRequest.Stime = timeNow.AddDays(-1);
                    PowerboxchargehistoryList = powerboxchargehistoryService.GetPowerboxchargehistoryByStime(powerboxchargehistoryRequest).Data;
                    StationList = pointDefineService.GetPointDefineCacheByDevpropertID(new Request.PointDefine.PointDefineGetByDevpropertIDRequest() { DevpropertID = 0 }).Data;
                    List<Jc_MacInfo> macList = networkModuleService.GetAllNetworkModuleCache().Data;
                    foreach (PowerboxchargehistoryInfo powerboxcharge in PowerboxchargehistoryList)
                    {
                        if (powerboxcharge.DischargeStime < DateTime.Parse("2000-01-01"))//放电开始判断
                        {
                            if (string.IsNullOrEmpty(powerboxcharge.Mac))
                            {//分站电源箱
                                Jc_DefInfo station = StationList.Find(a => a.Fzh == short.Parse(powerboxcharge.Fzh));
                                if (station != null)
                                {
                                    TimeSpan ts = station.DttRunStateTime - powerboxcharge.Stime;
                                    if (station.State == 4 && ts.TotalMinutes < 5)//如果操作放电，分站在5分钟内变成直流了，则记录放电的开始时间
                                    {
                                        powerboxcharge.DischargeStime = station.DttRunStateTime;
                                        PowerboxchargehistoryUpdate(powerboxcharge);
                                    }
                                }
                            }
                            else
                            {
                                GetMacPowerboxInfo(powerboxcharge.Mac);
                                //交换机电源箱
                                Jc_MacInfo mac = macList.Find(a => a.MAC == powerboxcharge.Mac);
                                if (mac != null)
                                {
                                    if (mac.BatteryItems != null && mac.BatteryItems.Count > 0)
                                    {
                                        if (mac.BatteryItems[0].BatteryACDC == 2)//如果操作放电，交换机变成直流，则记录放电的开始时间
                                        {
                                            powerboxcharge.DischargeStime = mac.PowerDateTime;
                                            PowerboxchargehistoryUpdate(powerboxcharge);
                                        }
                                    }
                                }
                            }
                        }
                        else if (powerboxcharge.DischargeEtime < DateTime.Parse("2000-01-01"))//放电结束判断
                        {
                            if (string.IsNullOrEmpty(powerboxcharge.Mac))
                            {
                                Jc_DefInfo station = StationList.Find(a => a.Fzh == short.Parse(powerboxcharge.Fzh));
                                if (station != null && station.State == 3)//如果分站变成交流了，则记录放电的结束时间
                                {
                                    powerboxcharge.DischargeEtime = station.DttRunStateTime;
                                    PowerboxchargehistoryUpdate(powerboxcharge);
                                }
                            }
                            else
                            {
                                GetMacPowerboxInfo(powerboxcharge.Mac);
                                //交换机电源箱
                                Jc_MacInfo mac = macList.Find(a => a.MAC == powerboxcharge.Mac);
                                if (mac != null)
                                {
                                    if (mac.BatteryItems != null && mac.BatteryItems.Count > 0)
                                    {
                                        if (mac.BatteryItems[0].BatteryACDC == 1)//如果交换机变成交流了，则记录放电的结束时间
                                        {
                                            powerboxcharge.DischargeEtime = mac.PowerDateTime;
                                            PowerboxchargehistoryUpdate(powerboxcharge);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(60000);
            }
        }
        public void PowerboxchargehistoryUpdate(PowerboxchargehistoryInfo powerboxcharge)
        {
            PowerboxchargehistoryUpdateRequest powerboxchargehistoryUpdateRequest = new PowerboxchargehistoryUpdateRequest();
            powerboxchargehistoryUpdateRequest.PowerboxchargehistoryInfo = powerboxcharge;
            powerboxchargehistoryService.UpdatePowerboxchargehistory(powerboxchargehistoryUpdateRequest);
        }
        /// <summary>
        /// 获取交换机电源箱信息
        /// </summary>
        /// <param name="mac"></param>
        public void GetMacPowerboxInfo(string mac)
        {
            SendDComReqest sendDComReqest = new SendDComReqest
            {
                queryBatteryRealDataItems = new List<BatteryControlItem>()
                {
                    new BatteryControlItem()
                    {
                        DevProID = 16,
                        FzhOrMac = mac,
                        controlType = 0
                    }
                }
            };

            var res = pointDefineService.SendQueryBatteryRealDataRequest(sendDComReqest);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }
    }
}
