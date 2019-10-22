using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.App;
using Sys.Safety.Enums;
using Sys.Safety.Request.App;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.Request.Login;
using Sys.Safety.Request.User;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.App;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sys.Safety.Model;
using Sys.Safety.Request.Setting;

namespace Sys.Safety.Services.App
{
    public class KJ73NAppService : IKJ73NAppService
    {
        private readonly INetworkModuleCacheService networkCacheService;
        private readonly IAlarmCacheService alarmCacheService;
        private readonly IPointDefineCacheService pointDefineCacheService;
        private readonly ILoginService loginService;
        private readonly IUserService userService;
        private readonly IDeviceClassCacheService deviceClassCacheService;
        private readonly IDevicePropertyCacheService devicePropertyCacheService;
        private readonly IDeviceDefineCacheService deviceDefineCacheService;
        private readonly IEnumcodeService enumCodeService;
        private readonly IChartService chartService;
        private readonly IJc_RRepository repository;
        private readonly IAlarmHandleService alarmHandleService;
        private readonly IAlarmRecordService alarmRecordService;
        private readonly IConfigCacheService configCacheService;
        private readonly IPositionCacheService positionCacheService;
        private readonly ISettingService _settingService;

        public KJ73NAppService()
        {
            networkCacheService = ServiceFactory.Create<INetworkModuleCacheService>();
            alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
            loginService = ServiceFactory.Create<ILoginService>();
            userService = ServiceFactory.Create<IUserService>();

            deviceClassCacheService = ServiceFactory.Create<IDeviceClassCacheService>();
            devicePropertyCacheService = ServiceFactory.Create<IDevicePropertyCacheService>();
            deviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
            enumCodeService = ServiceFactory.Create<IEnumcodeService>();
            chartService = ServiceFactory.Create<IChartService>();
            repository = ServiceFactory.Create<IJc_RRepository>();
            alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();
            alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();
            configCacheService = ServiceFactory.Create<IConfigCacheService>();
            positionCacheService = ServiceFactory.Create<IPositionCacheService>();

            _settingService = ServiceFactory.Create<ISettingService>();
        }

        #region 私有方法

        /// <summary>
        /// 获取交换机实时信息
        /// </summary>
        /// <param name="realDataList"></param>
        private void GetSwitchRealDataInfo(List<RealDataAppDataContract> realDataList)
        {
            try
            {
                var networkModuleList = networkCacheService.GetAllNetworkModuleCache(new Sys.Safety.Request.Cache.NetworkModuleCacheGetAllRequest()).Data;

                List<string> maclist = new List<string>();
                foreach (var network in networkModuleList)
                {
                    if (string.IsNullOrEmpty(network.Bz2))
                        continue;

                    //交换机信息由Bz2判断，一个交换机下可以有多个网络模块。
                    if (maclist.Contains(network.Bz2))
                    {
                        continue;
                    }
                    maclist.Add(network.Bz2);

                    RealDataAppDataContract realData = new RealDataAppDataContract();
                    realData.ID = network.ID;
                    realData.ModelName = "交换机";
                    realData.Place = network.Wz;
                    //判断当前交换机下面的所有网络模块状态
                    var netinfos = networkModuleList.FirstOrDefault(netinfo => netinfo.Bz2 == network.Bz2 && netinfo.NetID < 1 && !string.IsNullOrEmpty(netinfo.Bz1) && netinfo.Bz1 != "0|0|0|0|0|0|0|0");
                    if (netinfos == null)
                    {
                        realData.Value = "正常";
                        realData.DataState = "正常";
                        realData.DeviceState = "正常";
                        realData.Alarm = false;
                        realData.LinkageState = "0";
                    }
                    else
                    {
                        realData.Value = "异常";
                        realData.DataState = "异常";
                        realData.DeviceState = "异常";
                        realData.Alarm = true;
                        realData.LinkageState = "1";
                    }

                    realData.AlarmTime = "";
                    realData.Code = network.Bz2;
                    realDataList.Add(realData);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 获取设备实时数据对象
        /// </summary>
        /// <param name="point">测点信息</param>
        /// <param name="alarmInfoList">所有报警信息</param>
        /// <param name="pointDefineList">所有测点信息</param>
        /// <returns></returns>
        private RealDataAppDataContract GetRealDataInfo(Jc_DefInfo point, List<Jc_BInfo> alarmInfoList, List<Jc_DefInfo> pointDefineList)
        {
            //设备休眠
            if ((point.Bz4 & 0x02) == 0x02)
            {
                point.Ssz = "休眠";
                point.State = (int)DeviceRunState.EquipmentSleep;
                point.DataState = (int)DeviceDataState.EquipmentSleep;
            }

            RealDataAppDataContract realdate = new RealDataAppDataContract();
            realdate.ID = point.PointID;
            realdate.ModelName = point.DevName;
            realdate.Place = point.Wz;
            realdate.Value = point.Ssz;
            realdate.DataState = EnumHelper.GetEnumDescription((DeviceDataState)point.DataState);
            realdate.DeviceState = EnumHelper.GetEnumDescription((DeviceRunState)point.State);
            realdate.DeviceStateCode = point.State.ToString();
            realdate.DeviceProperty = point.DevProperty;
            realdate.DevicePropertyCode = point.DevPropertyID.ToString();
            realdate.Alarm = point.Alarm > 0 ? true : false;
            realdate.Code = point.Point;
            realdate.LinkageState = "-1";
            var alarmList = alarmInfoList.Where(alarm => alarm.PointID == point.PointID).OrderByDescending(alarm => alarm.Stime).ToList();
            if (point.Alarm > 0 && alarmList.Any())
            {
                realdate.AlarmTime = alarmList[0].Stime.ToString("yyyy-MM-dd HH:mm:ss");
                var timespan = DateTime.Now - alarmList[0].Stime;
                realdate.Duration = timespan.Minutes;
            }
            else
            {
                realdate.AlarmTime = string.Empty;
            }

            List<string> LinkagePoint = new List<string>();
            switch (point.DevPropertyID)
            {
                case 0://如果是分站，则查看分站下面的测点是否有异常的
                    List<Jc_DefInfo> sonlist = pointDefineList.Where(a => a.Fzh == point.Fzh && a.Kh > 0).OrderBy(o => o.DevPropertyID).ToList();
                    if (sonlist.Count > 0)
                    {
                        List<Jc_BInfo> sonalarmlist = alarmInfoList.FindAll(a => a.Isalarm > 0 && a.Fzh == point.Fzh && a.Kh > 0);
                        realdate.LinkageState = sonalarmlist.Count > 0 ? "1" : "0";
                        //添加分站下面的测点列表
                        realdate.SonPointDetail = new List<RealDataAppDataContract>();
                        sonlist.ForEach(son =>
                        {
                            RealDataAppDataContract temprealdate = GetRealDataInfo(son, alarmList, pointDefineList);
                            realdate.SonPointDetail.Add(temprealdate);
                        });
                    }
                    break;
                case 1://如果是模拟量，则查看模拟量报警、断电、断线本地及交叉控制是否有馈电异常记录  

                    //模拟量数据正常状态显示单位
                    if (!string.IsNullOrEmpty(point.Ssz) &&
                        point.Ssz != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentDown) &&
                        point.Ssz != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentOverrange) &&
                        point.Ssz != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentUnderrange) &&
                        point.Ssz != "休眠")
                    {
                        var deviceItem = deviceDefineCacheService.GetPointDefineCacheByKey(new DeviceDefineCacheGetByKeyRequest { Devid = point.Devid }).Data;
                        if (deviceItem != null)
                        {
                            realdate.Value += deviceItem.Xs1;
                        }
                    }


                    if (point.K1 > 0 || point.K2 > 0 || point.K3 > 0 || point.K4 > 0 || point.K5 > 0 || point.K6 > 0 || point.K7 > 0
                        || point.Jckz1.Trim().Length > 0 || point.Jckz2.Trim().Length > 0 || point.Jckz3.Trim().Length > 0)
                    {
                        //模拟量实时值需要显示单位

                        List<Jc_BInfo> analogalrmList = alarmInfoList.FindAll(a => a.Kdid != null && a.PointID == point.PointID);
                        realdate.LinkageState = analogalrmList.Count > 0 ? "1" : "0";
                        LinkagePoint = GetControlPoint(point);
                        realdate.LinkPointDetail = new List<LinkPointDetailAppDataContract>();
                        LinkagePoint.ForEach(link =>
                        {
                            int tempStation = int.Parse(link.Substring(0, 3));
                            int tempChannel = int.Parse(link.Substring(4, 2));
                            Jc_DefInfo temppoint = pointDefineList.FirstOrDefault(tp => tp.Fzh == tempStation && tp.Kh == tempChannel && tp.DevPropertyID == 3);
                            if (temppoint != null)
                            {
                                LinkPointDetailAppDataContract linkpointdetail = new LinkPointDetailAppDataContract();
                                linkpointdetail.Code = temppoint.Point;
                                linkpointdetail.Place = temppoint.Wz;
                                linkpointdetail.PowerState = true;
                                linkpointdetail.FeedState = true;
                                //linkpointdetail.PowerStateText = "正常";
                                //linkpointdetail.FeedStateText = "正常";
                                //状态
                                linkpointdetail.PowerStateText = temppoint.Ssz;
                                if ((point.Bz4 & 0x02) == 0x02)
                                {
                                    linkpointdetail.PowerStateText = "休眠";
                                }
                                if (temppoint.NCtrlSate == 32)
                                {//断电失败
                                    linkpointdetail.FeedState = false;
                                    linkpointdetail.FeedStateText = "断电失败";
                                }
                                else if (temppoint.NCtrlSate == 2)
                                {//断电成功
                                    linkpointdetail.FeedState = true;
                                    linkpointdetail.FeedStateText = "正常";
                                }
                                else if (temppoint.NCtrlSate == 0)
                                {//复电成功
                                    linkpointdetail.FeedState = true;
                                    linkpointdetail.FeedStateText = "正常";
                                }
                                else if (temppoint.NCtrlSate == 30)
                                {//复电失败
                                    linkpointdetail.FeedState = false;
                                    linkpointdetail.FeedStateText = "复电失败";
                                }
                                realdate.LinkPointDetail.Add(linkpointdetail);
                            }
                        });
                    }
                    break;
                case 2://如果是开关量，则查看开关量0，1，2态控制是否有异常
                    if (point.K1 > 0 || point.K2 > 0 || point.K3 > 0 || point.Jckz1.Trim().Length > 0 || point.Jckz2.Trim().Length > 0 || point.Jckz3.Trim().Length > 0)
                    {
                        List<Jc_BInfo> analogalrmList = alarmInfoList.FindAll(a => a.Kdid != null && a.PointID == point.PointID);
                        realdate.LinkageState = analogalrmList.Count > 0 ? "1" : "0";
                        LinkagePoint = GetControlPoint(point);
                        realdate.LinkPointDetail = new List<LinkPointDetailAppDataContract>();
                        LinkagePoint.ForEach(link =>
                        {
                            int tempStation = int.Parse(link.Substring(0, 3));
                            int tempChannel = int.Parse(link.Substring(4, 2));
                            Jc_DefInfo temppoint = pointDefineList.FirstOrDefault(tp => tp.Fzh == tempStation && tp.Kh == tempChannel && tp.DevPropertyID == 3);
                            if (temppoint != null)
                            {
                                LinkPointDetailAppDataContract linkpointdetail = new LinkPointDetailAppDataContract();
                                linkpointdetail.Code = temppoint.Point;
                                linkpointdetail.Place = temppoint.Wz;
                                linkpointdetail.PowerState = true;
                                linkpointdetail.FeedState = true;
                                //linkpointdetail.PowerStateText = "正常";
                                //linkpointdetail.FeedStateText = "正常";
                                //关联设备实时值

                                linkpointdetail.PowerStateText = temppoint.Ssz;
                                if ((point.Bz4 & 0x02) == 0x02)
                                {
                                    linkpointdetail.PowerStateText = "休眠";
                                }

                                if (temppoint.NCtrlSate == 32)
                                {//断电失败
                                    linkpointdetail.FeedState = false;
                                    linkpointdetail.FeedStateText = "断电失败";
                                }
                                else if (temppoint.NCtrlSate == 2)
                                {//断电成功
                                    linkpointdetail.FeedState = true;
                                    linkpointdetail.FeedStateText = "正常";
                                }
                                else if (temppoint.NCtrlSate == 0)
                                {//复电成功
                                    linkpointdetail.FeedState = true;
                                    linkpointdetail.FeedStateText = "正常";
                                }
                                else if (temppoint.NCtrlSate == 30)
                                {//复电失败
                                    linkpointdetail.FeedState = false;
                                    linkpointdetail.FeedStateText = "复电失败";
                                }
                                realdate.LinkPointDetail.Add(linkpointdetail);
                            }
                        });
                    }
                    break;
                case 4://累积量正常状态下显示单位值
                    if (!string.IsNullOrEmpty(point.Ssz) && point.Ssz != "休眠")
                    {
                        var deviceItem = deviceDefineCacheService.GetPointDefineCacheByKey(new DeviceDefineCacheGetByKeyRequest { Devid = point.Devid }).Data;
                        if (deviceItem != null)
                        {
                            realdate.Value += deviceItem.Xs1;
                        }
                    }
                    break;
            }
            return realdate;
        }

        /// <summary>
        /// 获取模拟量开关量的所有控制口关联信息
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public static List<string> GetControlPoint(Jc_DefInfo point)
        {
            List<string> LinkagePoint = new List<string>();
            //查找所有关联控制口的状态                            
            List<string> tempK = GetLocalControlPoint(point.Fzh, point.K1);//上限报警控制口，0态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K2);//上限断电控制口，1态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K3);//下限报警控制口，2态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K4);//下限断电控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K5);//上溢控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K6);//负漂控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K7);//断线控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Contains(K))
                {
                    LinkagePoint.Add(K);
                }
            }

            //获取交叉控制信息
            string[] TempJckz = point.Jckz1.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            TempJckz = point.Jckz2.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            TempJckz = point.Jckz3.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Contains(K) && K.Length > 0)
                {
                    LinkagePoint.Add(K);
                }
            }
            return LinkagePoint;
        }

        /// <summary>
        /// 获取本地控制口测点号列表信息
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private static List<string> GetLocalControlPoint(int Fzh, int K)
        {
            List<string> temp = new List<string>();
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {
                            temp.Add(Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0");
                        }
                        else
                        {
                            temp.Add(Fzh.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1");
                        }
                    }
                }
            }
            return temp;
        }

        private DataTable GetJCRData(RunLogGetRequest request, string strTable, string strDateFormat, string strIninWhere)
        {
            DataTable dtreturn = new DataTable();
            DateTime dateTime = TypeConvert.ToDateTime(request.StartTime);
            //string strTableName = strTable + dateTime.ToString(strDateFormat);
            string strwhere = " where 1=1 " + strIninWhere;

            if (TypeConvert.ToString(request.PointID).Length > 0)
                strwhere += " and pointid in(" + request.PointID + ")";
            if (TypeConvert.ToString(request.StateionID).Length > 0)
                strwhere += " and fzh in(" + request.StateionID + ")";
            if (TypeConvert.ToString(request.DevTypeID).Length > 0)//感觉应该查设备类型即devid字段，但是文档里面写的是查询设备种类
                strwhere += " and KJ_DeviceType.bz3 in(" + request.DevTypeID + ")";
            if (dateTime.Year > 2000)
            {
                if (strTable == "KJ_DataRunRecord" || strTable == "KJ_DataDetail")
                    strwhere += " and timer<'" + dateTime + "'";
                if (strTable == "KJ_DataAlarm")
                    strwhere += " and stime<'" + dateTime + "'";
            }
            string strorderby = "";
            if (strTable == "KJ_DataRunRecord" || strTable == "KJ_DataDetail")
                strorderby = " order by timer desc ";
            if (strTable == "KJ_DataAlarm")
                strorderby = " order by stime desc ";

            for (int i = 0; i < 3; i++)
            {//最多只查询三个月数据
                if (dtreturn.Rows.Count >= request.PageSize) break;
                //string strsql = "select " + strTableName + ".*,jc_dev.name,jc_dev.xs1,jc_wz.wz  from " + strTableName + " left join jc_dev on " + strTableName + ".devid=jc_dev.devid left join jc_wz on " + strTableName + ".wzid=jc_wz.wzid " + strwhere.Replace("{TableName}", strTableName) + strorderby + " limit 0," + request.PageSize;

                string strTableName = strTable + dateTime.ToString(strDateFormat);

                try
                {
                    //判断表是否存在
                    var tempdtIsExit = repository.QueryTable("global_ChartService_GetMySqlTableName_ByTableName", strTableName);
                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        DataTable dt = repository.QueryTable("global_AppGetRunInfo", new object[]
                        {
                            strTableName,
                            strTableName,
                            strTableName,
                            strTableName,
                            strwhere.Replace("{TableName}", strTableName) + strorderby,
                            request.PageSize
                        });

                        if (dtreturn.Rows.Count == 0) dtreturn = dt.Clone();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dtreturn.Rows.Count > request.PageSize) break;
                            dtreturn.ImportRow(row);
                        }
                        if (strTable == "KJ_DataRunRecord" || strTable == "KJ_DataAlarm")
                            dateTime = dateTime.AddMonths(-1);
                        if (strTable == "KJ_DataDetail")
                            dateTime = dateTime.AddDays(-1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("App获取报警记录、运行记录报错！" + ex.Message);
                }
            }
            return dtreturn;
        }

        private DataTable GetJCBDDData(RunLogGetRequest request, string strTable, string strDateFormat, string strIninWhere)
        {
            DataTable dtreturn = new DataTable();
            DateTime dateTime = TypeConvert.ToDateTime(request.StartTime);
            //string strTableName = strTable + dateTime.ToString(strDateFormat);

            string strwhere = " where 1=1 " + strIninWhere;

            if (TypeConvert.ToString(request.PointID).Length > 0)
                strwhere += " and pointid in(" + request.PointID + ")";
            if (TypeConvert.ToString(request.StateionID).Length > 0)
                strwhere += " and fzh in(" + request.StateionID + ")";
            if (TypeConvert.ToString(request.DevTypeID).Length > 0)//感觉应该查设备类型即devid字段，但是文档里面写的是查询设备种类
                strwhere += " and KJ_DeviceType.bz3 in(" + request.DevTypeID + ")";
            if (dateTime.Year > 2000)
                strwhere += " and stime<'" + dateTime + "'";
            for (int i = 0; i < 3; i++)
            {//最多只查询三个月数据
                if (dtreturn.Rows.Count >= request.PageSize) break;
                //string strsql = "select " + strTableName + ".*,jc_dev.name,jc_dev.name as xs1,jc_wz.wz  from " + strTableName + " left join jc_dev on " + strTableName + ".devid=jc_dev.devid left join jc_wz on " + strTableName + ".wzid=jc_wz.wzid " + strwhere.Replace("{TableName}", strTableName) + "order by stime desc limit 0," + request.PageSize;

                string strTableName = strTable + dateTime.ToString(strDateFormat);
                try
                {
                    //判断表是否存在
                    var tempdtIsExit = repository.QueryTable("global_ChartService_GetMySqlTableName_ByTableName", strTableName);
                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        DataTable dt = repository.QueryTable("global_AppGetAlarmPowerOffInfo", new object[]
                        {
                            strTableName,
                            strTableName,
                            strTableName,
                            strTableName,
                            strwhere.Replace("{TableName}", strTableName),
                            request.PageSize
                        });
                        if (dtreturn.Rows.Count == 0) dtreturn = dt.Clone();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (dtreturn.Rows.Count > request.PageSize) break;
                            dtreturn.ImportRow(row);
                        }
                        if (strTable == "KJ_DataRunRecord" || strTable == "KJ_DataAlarm")
                            dateTime = dateTime.AddMonths(-1);
                        if (strTable == "KJ_DataDetail")
                            dateTime = dateTime.AddDays(-1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("App 获取断电记录错误！" + ex.Message);
                }
            }
            return dtreturn;
        }

        #endregion

        public BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest)
        {
            BasicResponse<List<RealDataAppDataContract>> result = new BasicResponse<List<RealDataAppDataContract>>();
            try
            {
                List<RealDataAppDataContract> realDataList = new List<RealDataAppDataContract>();
                var pointDefineList = pointDefineCacheService.GetAllPointDefineCache(new Sys.Safety.Request.Cache.PointDefineCacheGetAllRequest()).Data.OrderBy(o => o.Point).ToList();
                //pointDefineList = pointDefineList.OrderBy(o => o.Point).ThenBy(o => o.DevPropertyID).ToList();

                var alarmList = alarmCacheService.GetAllAlarmCache(new Sys.Safety.Request.Cache.AlarmCacheGetAllRequest()).Data;
                //交换机实时信息
                if (realDataRequest.CharacterCode == "-2")
                {
                    GetSwitchRealDataInfo(realDataList);
                    //分站、模拟量、开关量等其它设备
                    pointDefineList.ForEach(temppoint => realDataList.Add(GetRealDataInfo(temppoint, alarmList, pointDefineList)));

                }
                else if (realDataRequest.CharacterCode == "-1")
                {
                    GetSwitchRealDataInfo(realDataList);
                }
                else
                {
                    int tempInt = 0;
                    List<Jc_DefInfo> selDefList = new List<Jc_DefInfo>();
                    int.TryParse(realDataRequest.CharacterCode, out tempInt);
                    selDefList = pointDefineList.FindAll(a => a.DevPropertyID == tempInt);
                    if (realDataRequest.TypeCode != "-1")
                    {
                        tempInt = 0;
                        int.TryParse(realDataRequest.TypeCode, out tempInt);
                        selDefList = selDefList.FindAll(a => a.DevClassID == tempInt);
                    }
                    if (realDataRequest.ModelCode != "-1")
                    {
                        tempInt = 0;
                        int.TryParse(realDataRequest.ModelCode, out tempInt);
                        selDefList = selDefList.FindAll(a => a.Devid == tempInt.ToString());
                    }
                    if (realDataRequest.StateCode != "-1")
                    {
                        tempInt = 0;
                        int.TryParse(realDataRequest.StateCode, out tempInt);
                        selDefList = selDefList.FindAll(a => a.DataState == tempInt);
                    }
                    if (realDataRequest.StateType != "-1")
                    {
                        tempInt = 0;
                        int.TryParse(realDataRequest.StateType, out tempInt);
                        if (tempInt > 0)
                        {
                            selDefList = selDefList.FindAll(a => a.Alarm > 0);
                        }
                        else
                        {
                            selDefList = selDefList.FindAll(a => a.Alarm == 0);
                        }
                    }

                    //分站、模拟量、开关量等其它设备
                    selDefList.ForEach(temppoint => realDataList.Add(GetRealDataInfo(temppoint, alarmList, pointDefineList)));
                }

                var configresponse = configCacheService.GetConfigCacheByKey(new ConfigCacheGetByKeyRequest() { Name = "defdatetime" });

                if (configresponse.IsSuccess && configresponse.Data != null)
                {
                    realDataList.ForEach(o => o.DefDateTime = configresponse.Data.Text);
                }

                result.Data = realDataList;
            }
            catch (Exception ex)
            {
                LogHelper.Info("App获取实时数据失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 实时获取所有模拟量报警、断电的ID列表，用","间隔  20170731
        /// </summary>
        /// <param name="realDataRequest"></param>
        /// <returns></returns>
        public BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest)
        {
            BasicResponse Result = new BasicResponse();

            PositionCacheGetAllRequest positionCacheRequest = new PositionCacheGetAllRequest();
            List<Jc_WzInfo> postionList = positionCacheService.GetAllPositongCache(positionCacheRequest).Data;

            AlarmCacheGetByConditonRequest alarmCacheRequest = new AlarmCacheGetByConditonRequest();
            List<int> alarmTypeList = new List<int>();

            GetSettingByKeyRequest request1 = new GetSettingByKeyRequest();
            request1.StrKey = "AppAlarmType";
            var result1 = _settingService.GetSettingByKey(request1);
            if (result1 != null && result1.Data != null && !string.IsNullOrEmpty(result1.Data.StrValue))
            {
                string[] appAlarmTypeArr = result1.Data.StrValue.Split(',');
                foreach (string type in appAlarmTypeArr)
                {
                    alarmTypeList.Add(int.Parse(type));
                }
            }
            else
            {
                alarmTypeList.Add(10);
                alarmTypeList.Add(12);
                alarmTypeList.Add(16);
                alarmTypeList.Add(18);
                alarmTypeList.Add(20);
            }

            alarmCacheRequest.Predicate = a => (alarmTypeList.Contains(a.Type)) && a.Etime.ToString("yyyy") == "1900";
            var resultAlarm = alarmCacheService.GetAlarmCache(alarmCacheRequest);
            string AlarmIdList = "";

            if (resultAlarm != null && resultAlarm.Data != null && resultAlarm.Data.Count > 0)
            {
                foreach (Jc_BInfo tempAlarm in resultAlarm.Data)
                {
                    Jc_WzInfo tempPostion = postionList.Find(a => a.WzID == tempAlarm.Wzid);
                    string tempPostionStr = "";
                    if (tempPostion != null)
                    {
                        tempPostionStr = tempPostion.Wz;
                    }
                    string tempTypeStr = "";
                    tempTypeStr = EnumHelper.GetEnumDescription((DeviceDataState)tempAlarm.Type);
                    //if (tempAlarm.Type == 10)
                    //{
                    //    tempTypeStr = "上限报警";
                    //}
                    //if (tempAlarm.Type == 12)
                    //{
                    //    tempTypeStr = "上限断电";
                    //}
                    //if (tempAlarm.Type == 16)
                    //{
                    //    tempTypeStr = "下限报警";
                    //}
                    //if (tempAlarm.Type == 18)
                    //{
                    //    tempTypeStr = "下限断电";
                    //}
                    //if (tempAlarm.Type == 20)
                    //{
                    //    tempTypeStr = "断线";
                    //}
                    AlarmIdList += tempAlarm.ID.ToString() + "|" + tempPostionStr + "|" + tempTypeStr + "|" + tempAlarm.Ssz + ",";
                }
            }

            Result.Message = AlarmIdList;

            return Result;
        }


        public BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            BasicResponse<RealDataAppDataContract> result = new BasicResponse<RealDataAppDataContract>();
            RealDataAppDataContract realData = new RealDataAppDataContract();
            try
            {
                var pointDefineList = pointDefineCacheService.GetAllPointDefineCache(new Sys.Safety.Request.Cache.PointDefineCacheGetAllRequest()).Data;
                var point = pointDefineList.FirstOrDefault(p => p.PointID == realDataGetDetialRequest.ID);
                if (point != null)
                {
                    var alarmList = alarmCacheService.GetAllAlarmCache(new Sys.Safety.Request.Cache.AlarmCacheGetAllRequest()).Data;
                    realData = GetRealDataInfo(point, alarmList, pointDefineList);
                    result.Data = realData;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "未找到测点信息！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取测点详细信息失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            BasicResponse<RealDataAppDataContract> result = new BasicResponse<RealDataAppDataContract>();
            RealDataAppDataContract realData = new RealDataAppDataContract();
            try
            {
                var pointDefineList = pointDefineCacheService.GetAllPointDefineCache(new Sys.Safety.Request.Cache.PointDefineCacheGetAllRequest()).Data;
                var point = pointDefineList.FirstOrDefault(p => p.PointID == realDataGetDetialRequest.ID && p.DevPropertyID == (int)DeviceProperty.Substation);
                if (point != null)
                {
                    var alarmList = alarmCacheService.GetAllAlarmCache(new Sys.Safety.Request.Cache.AlarmCacheGetAllRequest()).Data;
                    realData = GetRealDataInfo(point, alarmList, pointDefineList);
                    result.Data = realData;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "未找到分站信息！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取分站详细信息失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest)
        {
            BasicResponse<SwitcheAppDataContract> result = new BasicResponse<SwitcheAppDataContract>();
            SwitcheAppDataContract switcheObj = new SwitcheAppDataContract();
            try
            {
                var networkModuleList = networkCacheService.GetAllNetworkModuleCache(new Sys.Safety.Request.Cache.NetworkModuleCacheGetAllRequest()).Data;
                Jc_MacInfo networkInfo = networkModuleList.FirstOrDefault(a => a.Bz2 == switheRequest.ID);
                if (networkInfo != null && !string.IsNullOrEmpty(networkInfo.Bz2))
                {
                    switcheObj.ID = networkInfo.Bz2.ToString();
                    switcheObj.Place = networkInfo.Wz;
                    switcheObj.TypeName = "交换机";
                    //查找网络模块列表
                    var pointDefineList = pointDefineCacheService.GetAllPointDefineCache(new Sys.Safety.Request.Cache.PointDefineCacheGetAllRequest()).Data;

                    switcheObj.NetworkModuleObjList = new List<NetworkModuleAppDataContract>();
                    List<Jc_MacInfo> NetworkModList = networkModuleList.FindAll(a => a.Bz2 == networkInfo.Bz2);
                    foreach (Jc_MacInfo networkmodule in NetworkModList)
                    {
                        NetworkModuleAppDataContract network = new NetworkModuleAppDataContract();
                        network.ID = networkmodule.ID.ToString();
                        network.IP = networkmodule.IP;
                        network.MAC = networkmodule.MAC;
                        network.NO = networkmodule.NetID.ToString();
                        //根据连接号和State字段来判断交换机的实时值
                        if (!string.IsNullOrEmpty(networkmodule.Bz1) && networkmodule.Bz1 != "0|0|0|0|0|0|0|0")
                        {//如果绑定了分站才判断
                            if (networkmodule.NetID < 1)
                            {
                                network.Value = "通讯中断";
                                network.Alarm = true;
                            }
                            else if (networkInfo.State == 4)
                            {
                                network.Value = "直流正常";
                                network.Alarm = true;
                            }
                            else
                            {
                                network.Value = "交流正常";
                                network.Alarm = false;
                            }
                        }
                        else
                        {
                            network.Value = "未使用";
                            network.Alarm = false;
                        }
                        //查找当前交换机下面的分站设备是否存在异常
                        List<Jc_DefInfo> linkageState = pointDefineList.FindAll(a => a.Jckz1 == networkmodule.MAC && a.DevPropertyID == 0);
                        List<Jc_DefInfo> tempAlarm = linkageState.FindAll(a => a.Alarm > 0);
                        network.DeviceNum = linkageState.Count.ToString();
                        network.AlarmNum = tempAlarm.Count.ToString();
                        switcheObj.NetworkModuleObjList.Add(network);
                    }
                    result.Data = switcheObj;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "未找到交换机信息！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取交换机信息失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }

            return result;
        }

        public BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest)
        {
            BasicResponse<NetworkModuleAppDataContract> result = new BasicResponse<NetworkModuleAppDataContract>();
            NetworkModuleAppDataContract networkdata = new NetworkModuleAppDataContract();

            try
            {
                var networkModuleList = networkCacheService.GetAllNetworkModuleCache(new Sys.Safety.Request.Cache.NetworkModuleCacheGetAllRequest()).Data;
                Jc_MacInfo networkInfo = networkModuleList.FirstOrDefault(a => a.ID == networkModuleRequest.ID);
                if (networkInfo != null)
                {
                    networkdata.ID = networkInfo.ID.ToString();
                    networkdata.IP = networkInfo.IP;
                    networkdata.MAC = networkInfo.MAC;
                    networkdata.NO = networkInfo.NetID.ToString();
                    if (!string.IsNullOrEmpty(networkInfo.Bz1) && networkInfo.Bz1 != "0|0|0|0|0|0|0|0")
                    {//如果绑定了分站才判断
                        //根据连接号和State字段来判断交换机的实时值
                        if (networkInfo.NetID < 1)
                        {
                            networkdata.Value = "通讯中断";
                            networkdata.Alarm = true;
                        }
                        else if (networkInfo.State == 4)
                        {
                            networkdata.Value = "直流正常";
                            networkdata.Alarm = true;
                        }
                        else
                        {
                            networkdata.Value = "交流正常";
                            networkdata.Alarm = false;
                        }
                    }
                    else
                    {
                        networkdata.Value = "未使用";
                        networkdata.Alarm = false;
                    }
                    //查找当前交换机下面的分站设备是否存在异常
                    var pointDefineList = pointDefineCacheService.GetAllPointDefineCache(new Sys.Safety.Request.Cache.PointDefineCacheGetAllRequest()).Data;
                    var alarmList = alarmCacheService.GetAllAlarmCache(new Sys.Safety.Request.Cache.AlarmCacheGetAllRequest()).Data;

                    List<Jc_DefInfo> linkageState = pointDefineList.FindAll(a => a.Jckz1 == networkInfo.MAC && a.DevPropertyID == 0);
                    List<Jc_DefInfo> tempAlarm = linkageState.FindAll(a => a.Alarm > 0);
                    networkdata.DeviceNum = linkageState.Count.ToString();
                    networkdata.AlarmNum = tempAlarm.Count.ToString();
                    networkdata.PointList = new List<RealDataAppDataContract>();
                    linkageState.ForEach(p =>
                    {
                        networkdata.PointList.Add(GetRealDataInfo(p, alarmList, pointDefineList));
                    });

                    result.Data = networkdata;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "未找网络模块信息！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取网络模块信息失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }

            return result;
        }

        public BasicResponse<UserInfo> Logon(LogonRequest logonRequest)
        {
            BasicResponse<UserInfo> result = new BasicResponse<UserInfo>();
            try
            {
                UserLoginRequest ueserRequest = new UserLoginRequest
                {
                    UserName = logonRequest.UserName,
                    Password = logonRequest.Password
                };

                var userresponse = loginService.UserLogin(ueserRequest);
                if (userresponse != null && userresponse.IsSuccess && userresponse.Data != null)
                {
                    result.Data = userresponse.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("用户登陆失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest)
        {
            BasicResponse result = new BasicResponse();
            try
            {
                UserLoginRequest ueserRequest = new UserLoginRequest
                {
                    UserName = modifyPasswordRequest.UserCode,
                    Password = modifyPasswordRequest.OldPassWord
                };

                var userresponse = loginService.UserLogin(ueserRequest);
                if (userresponse != null && userresponse.IsSuccess && userresponse.Data != null)
                {
                    UserInfo user = userresponse.Data;
                    user.Password = MD5Helper.MD5Encrypt(modifyPasswordRequest.NewPassWord);

                    UserUpdateRequest updateRequest = new UserUpdateRequest();
                    updateRequest.UserInfo = user;

                    var updateresponse = userService.UpdateUser(updateRequest);
                    if (updateresponse != null && updateresponse.IsSuccess && updateresponse.Data != null)
                    {
                        result.Message = "修改密码成功！";
                    }
                    else
                    {
                        result.Code = -100;
                        result.Message = "修改密码失败！";
                    }
                }
                else
                {
                    result.Code = -100;
                    result.Message = "用户密码验证失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("修改密码失败：" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest)
        {
            BasicResponse<BaseDataAppDataContract> result = new BasicResponse<BaseDataAppDataContract>();
            try
            {
                BaseDataAppDataContract basedata = new BaseDataAppDataContract();

                //设备性质
                List<EnumcodeInfo> ListsEnumCodeFilter = devicePropertyCacheService.GetAllDevicePropertyCache(new DevicePropertyCacheGetAllRequest()).Data;
                List<DevProperty> DevPropertyDTOs = new List<DevProperty>();
                foreach (EnumcodeInfo enumCode in ListsEnumCodeFilter)
                {
                    DevProperty devproperty = new DevProperty();
                    devproperty.ID = enumCode.LngEnumValue;
                    devproperty.Name = enumCode.StrEnumDisplay;
                    DevPropertyDTOs.Add(devproperty);
                }
                basedata.DevPropertyDTOs = DevPropertyDTOs;

                //设备种类
                ListsEnumCodeFilter = deviceClassCacheService.GetAllDeviceClassCache(new DeviceClassCacheGetAllRequest()).Data;
                List<DevType> DevTypeDTOs = new List<DevType>();
                foreach (EnumcodeInfo enumCode in ListsEnumCodeFilter)
                {
                    DevType devtype = new DevType();
                    devtype.ID = enumCode.LngEnumValue;
                    devtype.Name = enumCode.StrEnumDisplay;
                    devtype.DevPropertyID = TypeConvert.ToInt(enumCode.LngEnumValue3);//代表设备性质
                    DevTypeDTOs.Add(devtype);
                }
                basedata.DevTypeDTOs = DevTypeDTOs;

                //设备类型
                List<Jc_DevInfo> ListsDev = deviceDefineCacheService.GetAllPointDefineCache(new DeviceDefineCacheGetAllRequest()).Data;
                List<DevModel> DevModelDTOs = new List<DevModel>();
                foreach (Jc_DevInfo devdto in ListsDev)
                {
                    DevModel devModel = new DevModel();
                    devModel.ID = long.Parse(devdto.Devid);
                    devModel.Name = devdto.Name;
                    devModel.StrDevSpecification = devdto.DevModel;
                    devModel.DevPropertyID = devdto.Type;
                    devModel.DevTypeID = devdto.Bz3;
                    DevModelDTOs.Add(devModel);
                }
                basedata.DevModelDTOs = DevModelDTOs;

                //设备状态
                ListsEnumCodeFilter = enumCodeService.GetEnumcodeList().Data.Where(en => en.EnumTypeID == ((int)EnumTypeEnum.DeviceState).ToString()).ToList();

                List<DevState> DevStateDTOs = new List<DevState>();
                foreach (EnumcodeInfo enumCode in ListsEnumCodeFilter)
                {
                    //设备状态取消设备检修
                    if (enumCode.StrEnumDisplay != "设备检修")
                    {
                        DevState devState = new DevState();
                        devState.ID = enumCode.LngEnumValue;
                        devState.Name = enumCode.StrEnumDisplay;
                        devState.DevPropertyID = TypeConvert.ToInt(enumCode.LngEnumValue4);
                        devState.RelDevProperty = enumCode.LngEnumValue3;//设备状态关联设备性质
                        DevStateDTOs.Add(devState);
                    }
                }
                basedata.DevStateDTOs = DevStateDTOs;

                //测点
                //PointDefineCacheGetByConditonRequest getbyconditorequest = new PointDefineCacheGetByConditonRequest
                //{
                //    Predicate = point => point.DevPropertyID == (int)DeviceProperty.Substation
                //};
                List<Jc_DefInfo> ListsDEF = pointDefineCacheService.GetAllPointDefineCache(new PointDefineCacheGetAllRequest()).Data;
                List<Point> PointDTOs = new List<Point>();
                foreach (Jc_DefInfo defdto in ListsDEF)
                {
                    Point point = new Point();
                    point.ID = defdto.PointID.ToString();
                    point.StationID = defdto.Fzh.ToString();
                    point.Code = defdto.Point;
                    point.Place = defdto.Wz;
                    point.DevModelID = defdto.Devid.ToString();
                    point.DevTypeID = defdto.DevClassID.ToString();
                    point.DevPropertyID = defdto.DevPropertyID.ToString();
                    PointDTOs.Add(point);
                }
                basedata.PointDTOs = PointDTOs;

                result.Data = basedata;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取基础数据失败:" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest)
        {
            BasicResponse<List<AnalogChartAppDataContract>> result = new BasicResponse<List<AnalogChartAppDataContract>>();
            try
            {
                List<AnalogChartAppDataContract> MLLFiveLineData = new List<AnalogChartAppDataContract>();
                var SzNameS = DateTime.Parse(analoglineRequest.StartTime);
                var SzNameE = DateTime.Parse(analoglineRequest.EndTime);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 7)
                {
                    result.Code = -1;
                    result.Message = "查询日期不能大于7天,请重新选择日期";
                    return result;
                }

                var CurrentPointID = analoglineRequest.ID;
                PointDefineCacheGetByConditonRequest getbypointidrequest = new PointDefineCacheGetByConditonRequest
                {
                    Predicate = point => point.PointID == CurrentPointID
                };
                var pointInfo = pointDefineCacheService.GetPointDefineCache(getbypointidrequest).Data.FirstOrDefault();
                var CurrentDevid = pointInfo == null ? string.Empty : pointInfo.Devid;
                var CurrentWzid = pointInfo == null ? string.Empty : pointInfo.Wzid;

                GetFiveMiniteLineRequest getfivelineRequest = new GetFiveMiniteLineRequest()
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var dt = chartService.GetFiveMiniteLine(getfivelineRequest).Data;

                //转换数据为WEB需要的格式返回 
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    AnalogChartAppDataContract chartData = new AnalogChartAppDataContract();
                    chartData.ID = (i + 1).ToString();
                    chartData.PointID = CurrentPointID;
                    //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
                    chartData.SSZ = dt.Rows[i]["Av"].ToString();
                    chartData.ZDZ = dt.Rows[i]["Bv"].ToString();
                    chartData.ZXZ = dt.Rows[i]["Cv"].ToString();
                    chartData.PJZ = dt.Rows[i]["Dv"].ToString();
                    chartData.YDZ = dt.Rows[i]["Ev"].ToString();
                    chartData.Type = dt.Rows[i]["type"].ToString();
                    chartData.TypeText = dt.Rows[i]["typetext"].ToString();
                    chartData.Timer = dt.Rows[i]["Timer"].ToString();
                    MLLFiveLineData.Add(chartData);
                }
                result.Data = MLLFiveLineData;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取模拟量曲线失败:" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest)
        {
            BasicResponse<List<DerailChartAppDataContract>> result = new BasicResponse<List<DerailChartAppDataContract>>();
            try
            {
                List<DerailChartAppDataContract> KGLFiveLineData = new List<DerailChartAppDataContract>();
                var SzNameS = DateTime.Parse(deraillineRequest.StartTime);
                var SzNameE = DateTime.Parse(deraillineRequest.EndTime);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 3)
                {
                    result.Code = -100;
                    result.Message = "查询日期不能大于3天,请重新选择日期";
                    return result;
                }

                var CurrentPointID = deraillineRequest.ID;
                PointDefineCacheGetByConditonRequest getbypointidrequest = new PointDefineCacheGetByConditonRequest
                {
                    Predicate = point => point.PointID == CurrentPointID
                };
                var pointInfo = pointDefineCacheService.GetPointDefineCache(getbypointidrequest).Data.FirstOrDefault();
                var CurrentDevid = pointInfo == null ? string.Empty : pointInfo.Devid;
                var CurrentWzid = pointInfo == null ? string.Empty : pointInfo.Wzid;


                var dt_line = new DataTable();
                for (var NTime = SzNameS; NTime <= SzNameE; NTime = NTime.AddDays(1))//曲线只支持按天查询 
                {
                    GetStateLineDtRequest getdtRequest = new GetStateLineDtRequest
                    {
                        SzNameT = NTime,
                        CurrentPointID = CurrentPointID,
                        CurrentDevid = CurrentDevid,
                        CurrentWzid = CurrentWzid,
                        kglztjsfs = true
                    };
                    dt_line = chartService.GetStateLineDt(getdtRequest).Data;
                    //转换数据为WEB需要的格式返回 
                    for (var i = 0; i < dt_line.Rows.Count; i++)
                    {
                        DerailChartAppDataContract ChartData = new DerailChartAppDataContract();
                        ChartData.ID = (i + 1).ToString();
                        ChartData.PointID = CurrentPointID;
                        ChartData.Type = dt_line.Rows[i]["C"].ToString();
                        ChartData.TypeText = dt_line.Rows[i]["stateName"].ToString();
                        ChartData.Stime = dt_line.Rows[i]["sTimer"].ToString();
                        ChartData.Etime = dt_line.Rows[i]["eTimer"].ToString();
                        ChartData.KD = dt_line.Rows[i]["D"].ToString();
                        ChartData.CS = dt_line.Rows[i]["E"].ToString();
                        KGLFiveLineData.Add(ChartData);
                    }
                }
                result.Data = KGLFiveLineData;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取开关量曲线失败:" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = ex.Message;
            }

            return result;
        }

        public BasicResponse<GetJCRDataResponse> GetJCRData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetJCRDataResponse> result = new BasicResponse<GetJCRDataResponse>();
            try
            {
                GetJCRDataResponse response = new GetJCRDataResponse();
                List<RunLogAppDataContract> ListJCRDto = new List<RunLogAppDataContract>();
                DataTable dt = GetJCRData(runlogRequest, "KJ_DataRunRecord", "yyyyMM", "");

                var pointList = pointDefineCacheService.GetAllPointDefineCache(new PointDefineCacheGetAllRequest()).Data;

                foreach (DataRow row in dt.Rows)
                {
                    RunLogAppDataContract jcrData = new RunLogAppDataContract();
                    string pointid = TypeConvert.ToString(row["pointID"]);
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.time = TypeConvert.ToDateTime(row["timer"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.DataState = EnumHelper.GetEnumDescription((DeviceDataState)TypeConvert.ToInt(row["type"]));

                    var value = TypeConvert.ToString(row["val"]);
                    jcrData.value = value;
                    //根据测点设备类型取实时值
                    var pointitem = pointList.FirstOrDefault(o => o.PointID == pointid);
                    if (pointitem != null)
                    {
                        //模拟量需要显示单位，但异常时不显示单位
                        if ((DeviceProperty)pointitem.DevPropertyID == DeviceProperty.Analog)
                        {
                            //模拟量数据正常状态显示单位
                            if (!string.IsNullOrEmpty(value) &&
                                value != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentDown) &&
                                value != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentOverrange) &&
                                value != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentUnderrange) &&
                                value != EnumHelper.GetEnumDescription(DeviceDataState.EquipmentStateUnknow))
                            {
                                jcrData.value += TypeConvert.ToString(row["xs1"]);
                            }
                        }
                    }
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取运行记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetJCMCDataResponse> result = new BasicResponse<GetJCMCDataResponse>();
            try
            {
                GetJCMCDataResponse response = new GetJCMCDataResponse();
                DataTable dt = GetJCRData(runlogRequest, "KJ_DataDetail", "yyyyMMdd", "");
                List<RunLogAppDataContract> ListJCRDto = new List<RunLogAppDataContract>();
                foreach (DataRow row in dt.Rows)
                {
                    RunLogAppDataContract jcrData = new RunLogAppDataContract();
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.time = TypeConvert.ToDateTime(row["timer"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.value = TypeConvert.ToString(row["ssz"]) + TypeConvert.ToString(row["xs1"]);
                    jcrData.DataState = EnumHelper.GetEnumDescription((DeviceDataState)TypeConvert.ToInt(row["type"]));
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取密采记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetMLLBJDataResponse> result = new BasicResponse<GetMLLBJDataResponse>();
            try
            {
                //模拟量报警记录取上限报警和下限报警
                GetMLLBJDataResponse response = new GetMLLBJDataResponse();
                DataTable dt = GetJCRData(runlogRequest, "KJ_DataAlarm", "yyyyMM", "  and ({TableName}.type=10 or {TableName}.type=16) ");
                List<RunLogAppDataContract> ListJCRDto = new List<RunLogAppDataContract>();
                foreach (DataRow row in dt.Rows)
                {
                    RunLogAppDataContract jcrData = new RunLogAppDataContract();
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.StartTime = TypeConvert.ToDateTime(row["stime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.EndTime = TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    int tempSsz = 0;
                    int tempZdz = 0;
                    int.TryParse(row["zdz"].ToString(), out tempZdz);
                    int.TryParse(row["ssz"].ToString(), out tempSsz);
                    if (tempZdz > 0)
                    {
                        jcrData.value = TypeConvert.ToString(row["zdz"]) + TypeConvert.ToString(row["xs1"]);
                    }
                    else
                    {
                        jcrData.value = TypeConvert.ToString(row["ssz"]) + TypeConvert.ToString(row["xs1"]);
                    }
                    //报警持续时间                    
                    if (TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss") != "1900-01-01 00:00:00")
                    {
                        var timespan = TypeConvert.ToDateTime(row["etime"]) - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    else
                    {
                        var timespan = DateTime.Now - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取模拟量报警记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetKGLBJDataResponse> result = new BasicResponse<GetKGLBJDataResponse>();
            try
            {
                GetKGLBJDataResponse response = new GetKGLBJDataResponse();
                DataTable dt = GetJCRData(runlogRequest, "KJ_DataAlarm", "yyyyMM", "  and {TableName}.isalarm=1 and {TableName}.type in (25,26,27) ");
                List<RunLogAppDataContract> ListJCRDto = new List<RunLogAppDataContract>();
                foreach (DataRow row in dt.Rows)
                {
                    RunLogAppDataContract jcrData = new RunLogAppDataContract();
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.StartTime = TypeConvert.ToDateTime(row["stime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.EndTime = TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.value = TypeConvert.ToString(row["ssz"]);
                    //报警持续时间                    
                    if (TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss") != "1900-01-01 00:00:00")
                    {
                        var timespan = TypeConvert.ToDateTime(row["etime"]) - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    else
                    {
                        var timespan = DateTime.Now - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取开关量报警记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetMLLDDDataResponse> result = new BasicResponse<GetMLLDDDataResponse>();
            try
            {
                GetMLLDDDataResponse response = new GetMLLDDDataResponse();
                DataTable dt = GetJCBDDData(runlogRequest, "KJ_DataAlarm", "yyyyMM", " and {TableName}.type in (12,18) ");
                List<AlarmPowerOffAppDataContract> ListJCRDto = new List<AlarmPowerOffAppDataContract>();
                foreach (DataRow row in dt.Rows)
                {
                    AlarmPowerOffAppDataContract jcrData = new AlarmPowerOffAppDataContract();
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.StartTime = TypeConvert.ToDateTime(row["stime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.EndTime = TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    int tempSsz = 0;
                    int tempZdz = 0;
                    int.TryParse(row["zdz"].ToString(), out tempZdz);
                    int.TryParse(row["ssz"].ToString(), out tempSsz);
                    if (tempZdz > 0)
                    {
                        jcrData.value = TypeConvert.ToString(row["zdz"]) + TypeConvert.ToString(row["xs1"]);
                    }
                    else
                    {
                        jcrData.value = TypeConvert.ToString(row["ssz"]) + TypeConvert.ToString(row["xs1"]);
                    }
                    //报警持续时间                    
                    if (TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss") != "1900-01-01 00:00:00")
                    {
                        var timespan = TypeConvert.ToDateTime(row["etime"]) - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    else
                    {
                        var timespan = DateTime.Now - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;

            }
            catch (Exception ex)
            {
                LogHelper.Error("获取模拟量断电记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest)
        {
            BasicResponse<GetKGLDDDataResponse> result = new BasicResponse<GetKGLDDDataResponse>();
            try
            {
                GetKGLDDDataResponse response = new GetKGLDDDataResponse();
                DataTable dt = GetJCBDDData(runlogRequest, "KJ_DataAlarm", "yyyyMM", " and {TableName}.type in(25,26,27) and LENGTH(kzk)>0");
                List<AlarmPowerOffAppDataContract> ListJCRDto = new List<AlarmPowerOffAppDataContract>();
                foreach (DataRow row in dt.Rows)
                {
                    AlarmPowerOffAppDataContract jcrData = new AlarmPowerOffAppDataContract();
                    jcrData.ID = TypeConvert.ToString(row["ID"]);
                    jcrData.PointID = TypeConvert.ToString(row["Point"]);
                    jcrData.DevModelName = TypeConvert.ToString(row["name"]);
                    jcrData.place = TypeConvert.ToString(row["wz"]);
                    jcrData.StartTime = TypeConvert.ToDateTime(row["stime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.EndTime = TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    jcrData.value = TypeConvert.ToString(row["ssz"]);
                    //报警持续时间                    
                    if (TypeConvert.ToDateTime(row["etime"]).ToString("yyyy-MM-dd HH:mm:ss") != "1900-01-01 00:00:00")
                    {
                        var timespan = TypeConvert.ToDateTime(row["etime"]) - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    else
                    {
                        var timespan = DateTime.Now - TypeConvert.ToDateTime(row["stime"]);
                        jcrData.Duration = timespan.Minutes + "分钟" + timespan.Seconds + "秒";
                    }
                    ListJCRDto.Add(jcrData);
                }
                response.ListJCRDto = ListJCRDto;
                result.Data = response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取开关量断电记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest alarmRecordRequest)
        {
            BasicResponse<List<AlarmProcessInfo>> result = new BasicResponse<List<AlarmProcessInfo>>();
            try
            {
                DateTime stime = new DateTime();
                DateTime.TryParse(alarmRecordRequest.Stime, out stime);
                DateTime etime = new DateTime();
                DateTime.TryParse(alarmRecordRequest.ETime, out etime); 
                if (stime > etime)
                {
                    throw new Exception("开始时间不应大于结束时间");
                }
                TimeSpan span = etime.Subtract(stime);
                if (span.TotalDays > 2)
                {
                    throw new Exception("查询时间不能大于3天");
                }

                var response = alarmRecordService.GetAlarmRecordListByStime(alarmRecordRequest);
                if (response != null && response.IsSuccess)
                {
                    var alarmrecords = response.Data;
                    alarmrecords.ForEach(o =>
                    {
                        if (o.Cs == null)
                            o.Cs = string.Empty;
                        if (o.Bz1 == null)
                            o.Bz1 = string.Empty;
                    });

                    result.Data = alarmrecords;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "获取逻辑报警记录失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取逻辑报警记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest)
        {
            BasicResponse result = new BasicResponse();
            try
            {
                AlarmRecordGetDateIdRequest alarmgetrequest = new AlarmRecordGetDateIdRequest()
                {
                    AlarmDate = alarmRecordEndRequest.Stime.ToString("yyyyMM"),
                    Id = alarmRecordEndRequest.AlarmId
                };
                var getresponse = alarmRecordService.GetDateAlarmRecordById(alarmgetrequest);
                if (getresponse != null && getresponse.IsSuccess && getresponse.Data != null)
                {
                    Jc_BInfo alarmInfo = getresponse.Data;
                    alarmInfo.Remark = alarmRecordEndRequest.AlarmReason;
                    alarmInfo.Cs = alarmRecordEndRequest.AlarmMeasure;
                    //alarmInfo.Etime = alarmRecordEndRequest.Etime;
                    alarmInfo.Bz1 = alarmRecordEndRequest.AlarmProcessPerson;
                    alarmInfo.Bz2 = alarmRecordEndRequest.Etime.ToString("yyyy-MM-dd HH:mm:ss");

                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("Bz1", alarmInfo.Bz1);
                    updateItems.Add("Bz2", alarmInfo.Bz2);
                    updateItems.Add("Remark", alarmInfo.Remark);
                    updateItems.Add("Cs", alarmInfo.Cs);

                    AlarmRecordUpdateProperitesRequest updateRequest = new AlarmRecordUpdateProperitesRequest()
                    {
                        AlarmInfo = alarmInfo,
                        UpdateItems = updateItems
                    };
                    var updateResponse = alarmRecordService.UpdateAlarmInfoProperties(updateRequest);

                    //AlarmRecordUpdateDateRequest updateRequest = new AlarmRecordUpdateDateRequest
                    //{
                    //    AlarmInfo = alarmInfo
                    //};
                    //var updateResponse = alarmRecordService.UpdateDateAlarmRecord(updateRequest);
                    if (updateResponse != null && updateResponse.IsSuccess)
                    {
                        result.Message = "结束报警成功!";
                    }
                    else
                    {
                        result.Code = -100;
                        result.Message = "修改报警记录失败!";
                    }
                }
                else
                {
                    result.Code = -100;
                    result.Message = "修改报警记录失败,未找到记录!";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("修改报警记录失败:" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = "修改报警记录失败!";
            }
            return result;
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            BasicResponse<List<JC_AlarmHandleInfo>> result = new BasicResponse<List<JC_AlarmHandleInfo>>();
            try
            {
                var stime = alarmHandelRequest.Stime;
                var etime = alarmHandelRequest.Etime;
                if (stime > etime)
                {
                    throw new Exception("开始时间不应大于结束时间");
                }
                TimeSpan span = etime.Subtract(stime);
                if (span.TotalDays > 3)
                {
                    throw new Exception("查询时间不能大于3天");
                }

                var response = alarmHandleService.GetAlarmHandleByStimeAndEtime(alarmHandelRequest);
                if (response != null && response.IsSuccess)
                {
                    var alarmhandles = response.Data;
                    alarmhandles.ForEach(o =>
                    {
                        if (o.Handling == null)
                            o.Handling = string.Empty;
                        if (o.ExceptionReason == null)
                            o.ExceptionReason = null;
                    });
                    result.Data = alarmhandles;
                }
                else
                {
                    result.Code = -100;
                    result.Message = "获取设备报警记录失败！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取设备报警记录失败：" + "\r\n" + ex.ToString());
                result.Code = -100;
                result.Message = ex.Message;
            }
            return result;
        }

        public BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest)
        {
            BasicResponse result = new BasicResponse();
            try
            {
                AlarmHandleGetRequest alarmgetRequest = new AlarmHandleGetRequest
                {
                    Id = alarmHandleEndRequest.AlarmId
                };
                var getresponse = alarmHandleService.GetJC_AlarmHandleById(alarmgetRequest);
                if (getresponse != null && getresponse.IsSuccess && getresponse.Data != null)
                {
                    JC_AlarmHandleInfo alarmInfo = getresponse.Data;
                    alarmInfo.ExceptionReason = alarmHandleEndRequest.AlarmReason;
                    alarmInfo.Handling = alarmHandleEndRequest.AlarmMeasure;
                    //alarmInfo.EndTime = alarmHandleEndRequest.Etime;
                    alarmInfo.HandlingPerson = alarmHandleEndRequest.AlarmProcessPerson;
                    alarmInfo.HandlingTime = alarmHandleEndRequest.Etime;

                    AlarmHandleUpdateRequest updateRequest = new AlarmHandleUpdateRequest
                    {
                        JC_AlarmHandleInfo = alarmInfo
                    };
                    var updateResponse = alarmHandleService.UpdateJC_AlarmHandle(updateRequest);
                    if (updateResponse != null && updateResponse.IsSuccess)
                    {
                        result.Message = "结束报警成功!";
                    }
                    else
                    {
                        result.Code = -100;
                        result.Message = "修改报警记录失败!";
                    }
                }
                else
                {
                    result.Code = -100;
                    result.Message = "修改报警记录失败,未找到记录!";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("修改报警记录失败:" + "\r\n" + ex.Message);
                result.Code = -100;
                result.Message = "修改报警记录失败!";
            }
            return result;
        }
    }
}
