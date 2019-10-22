using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.DataToDb;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Pb;
using Sys.Safety.Request.Phj;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.R_Phistory;
using Sys.Safety.Request.R_Preal;
using Sys.Safety.Request.UndefinedDef;
using Sys.Safety.Processing.DataToDb;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.DataToDb;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing
{
    public class KJ237CacheHelper
    {
        #region ----接口定义----

        public static IRPRealCacheService rPRealCacheService;
        public static IR_PhjService r_PhjService;
        public static IR_PhistoryService r_PhistoryService;
        public static IRsyncLocalCacheService rsyncLocalCacheService;
        public static IPersonPointDefineService personPointDefineService;
        public static IRPersoninfCacheService rPersoninfCacheService;
        public static IRUndefinedDefCacheService rUndefinedDefCacheService;
        public static IR_PersoninfRepository r_PersoninfRepository;
        public static IPointDefineService pointDefineService;
        public static IAlarmCacheService alarmCacheService;
        public static IAlarmRecordRepository alarmRecordRepository;
        public static IInsertToDbService<Jc_BInfo> alarmTodbService;
        public static IInsertToDbService<R_PhistoryInfo> phistoryTodbService;
        public static IInsertToDbService<R_PhjInfo> phjTodbService;
        public static IR_PBCacheService r_PBCacheService;
        public static IAreaCacheService areaCacheService;

        public static IR_CallService r_CallService;
        public static IInsertToDbService<R_PbInfo> r_PbService;

        public static IR_KqbcService r_KqbcService;

        #endregion

        static KJ237CacheHelper()
        {
            rPRealCacheService = ServiceFactory.Create<IRPRealCacheService>();
            r_PhjService = ServiceFactory.Create<IR_PhjService>();
            r_PhistoryService = ServiceFactory.Create<IR_PhistoryService>();
            rsyncLocalCacheService = ServiceFactory.Create<IRsyncLocalCacheService>();
            personPointDefineService = ServiceFactory.Create<IPersonPointDefineService>();
            rPersoninfCacheService = ServiceFactory.Create<IRPersoninfCacheService>();
            r_PersoninfRepository = ServiceFactory.Create<IR_PersoninfRepository>();
            rUndefinedDefCacheService = ServiceFactory.Create<IRUndefinedDefCacheService>();
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
            alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            alarmRecordRepository = ServiceFactory.Create<IAlarmRecordRepository>();
            alarmTodbService = ServiceFactory.Create<IInsertToDbService<Jc_BInfo>>();
            phjTodbService = ServiceFactory.Create<IInsertToDbService<R_PhjInfo>>();
            phistoryTodbService = ServiceFactory.Create<IInsertToDbService<R_PhistoryInfo>>();
            r_PBCacheService = ServiceFactory.Create<IR_PBCacheService>();
            areaCacheService = ServiceFactory.Create<IAreaCacheService>();
            r_PbService = ServiceFactory.Create<IInsertToDbService<R_PbInfo>>();
            r_CallService = ServiceFactory.Create<IR_CallService>();
            r_KqbcService = ServiceFactory.Create<IR_KqbcService>();


        }

        #region ----R_Preal----

        /// <summary>
        /// 添加或修改R_Preal记录
        /// </summary>
        /// <param name="prealInfo"></param>
        public static void InsertPrealData(R_PrealInfo prealInfo)
        {
            RPrealCacheAddRequest request = new RPrealCacheAddRequest();
            request.PrealInfo = prealInfo;
            rPRealCacheService.AddRRealCache(request);
        }
        /// <summary>
        /// 更新Preal部分属性
        /// </summary>
        /// <param name="updateItems"></param>
        public static void UpdatePrealByProperties(Dictionary<string, Dictionary<string, object>> updateItems)
        {
            RPrealCacheBatchUpdatePropertiesRequest request = new RPrealCacheBatchUpdatePropertiesRequest();
            request.UpdateItems = updateItems;
            rPRealCacheService.BatchUpdateRealInfo(request);
        }
        /// <summary>
        /// 获取所有人员实时信息
        /// </summary>
        /// <returns></returns>
        public static List<R_PrealInfo> GetAllPrealInfo()
        {
            List<R_PrealInfo> items = new List<R_PrealInfo>();
            RPrealCacheGetAllRequest request = new RPrealCacheGetAllRequest();
            var result = rPRealCacheService.GetAllRRealCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }
        /// <summary>
        /// 获取单个人员信息
        /// </summary>
        /// <returns></returns>
        public static R_PrealInfo GetOnePrealInfo(string bh)
        {
            R_PrealInfo item = null;
            RPrealCacheGetByConditonRequest request = new RPrealCacheGetByConditonRequest();
            request.Predicate = a => a.Bh == bh;
            var result = rPRealCacheService.GetRealCache(request);
            if (result.IsSuccess && result.Data != null && result.Data.Count > 0)
            {
                item = result.Data[0];
            }
            return item;
        }
        /// <summary>
        /// 清除Preal信息
        /// </summary>
        /// <param name="id"></param>
        public static void DeletePreal(R_PrealInfo preal)
        {
            RPrealCacheDeleteRequest request = new RPrealCacheDeleteRequest();
            request.PrealInfo = preal;
            rPRealCacheService.DeleteRRealCache(request);
        }
        #endregion

        #region ----AreaInfo----

        public static List<AreaInfo> GetAllAreaInfo()
        {
            List<AreaInfo> items = new List<AreaInfo>();
            AreaCacheGetAllRequest request = new AreaCacheGetAllRequest();
            var result = areaCacheService.GetAllAreaCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }
        #endregion

        #region ----R_PersonInfo----
        public static void AddPerson(R_PersoninfInfo personInfo)
        {
            //添加到数据库
            var _personinf = ObjectConverter.Copy<R_PersoninfInfo, R_PersoninfModel>(personInfo);
            r_PersoninfRepository.AddPersoninf(_personinf);
            //更新到内存
            RPersoninfCacheAddRequest personRequest = new RPersoninfCacheAddRequest();
            personRequest.RPersoninfInfo = personInfo;
            rPersoninfCacheService.AddRPersoninfCache(personRequest);
        }
        public static List<R_PersoninfInfo> GetAllPersonInfo()
        {
            List<R_PersoninfInfo> items = new List<R_PersoninfInfo>();
            RPersoninfCacheGetAllRequest request = new RPersoninfCacheGetAllRequest();
            var result = rPersoninfCacheService.GetAllRPersoninfCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }
        #endregion

        #region ----R_RsyncLocal----
        public static List<R_SyncLocalInfo> GetR_SyncLocalIetms()
        {
            List<R_SyncLocalInfo> items = new List<R_SyncLocalInfo>();
            R_SyncLocalCacheGetAllRequest request = new R_SyncLocalCacheGetAllRequest();
            var reslut = rsyncLocalCacheService.GetAll(request);
            if (reslut.IsSuccess && reslut.Data != null)
            {
                items = reslut.Data;
            }
            return items;
        }

        public static void DeleteR_SyncLocalIetms(R_SyncLocalInfo item)
        {
            R_SyncLocalCacheDeleteRequest request = new R_SyncLocalCacheDeleteRequest();
            request.SyncLocal = item;
            rsyncLocalCacheService.Delete(request);
        }

        public static void BachDeleteR_SyncLocalIetms(List<R_SyncLocalInfo> items)
        {
            R_SyncLocalCacheBatchDeleteRequest request = new R_SyncLocalCacheBatchDeleteRequest();
            request.SyncLocals = items;
            rsyncLocalCacheService.BatchDelete(request);
        }
        #endregion

        #region ----UndefinedDef----
        public static List<R_UndefinedDefInfo> GetAllUndefineInfo()
        {
            List<R_UndefinedDefInfo> items = new List<R_UndefinedDefInfo>();

            RUndefinedDefCacheGetAllRequest request = new RUndefinedDefCacheGetAllRequest();
            var result = rUndefinedDefCacheService.GetAllRUndefinedDefCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }
        #endregion

        #region ----R_PhistoryInfo----
        public static R_PhistoryInfo GetLastHistoryRP(string yid)
        {
            R_PhistoryInfo r_p = new R_PhistoryInfo();
            R_PhistoryGetLastByYidRequest request = new R_PhistoryGetLastByYidRequest();
            request.yid = yid;
            r_p = r_PhistoryService.GetPersonLastR_Phistory(request).Data;
            return r_p;
        }

        public static R_PhistoryInfo GetR_PByPar(string pointid, string yid, DateTime rtime)
        {
            R_PhistoryInfo item = null;
            R_PhistoryGetByParRequest request = new R_PhistoryGetByParRequest();
            request.pointid = pointid;
            request.yid = yid;
            request.rtime = rtime;
            var result = r_PhistoryService.GetPhistoryByPar(request);
            if (result.IsSuccess && result.Data != null)
            {
                item = result.Data;
            }
            return item;
        }

        /// <summary>
        /// 添加人员轨迹
        /// </summary>
        /// <param name="item"></param>
        public static void InsertR_P(R_PhistoryInfo item)
        {
            item.InfoState = InfoState.AddNew;
            DataToDbAddRequest<R_PhistoryInfo> dataToDbRequest = new DataToDbAddRequest<R_PhistoryInfo>();
            dataToDbRequest.Item = item;
            phistoryTodbService.AddItem(dataToDbRequest);
        }
        /// <summary>
        /// 修改人员轨迹
        /// </summary>
        /// <param name="item"></param>
        public static void UpdateR_P(R_PhistoryInfo item)
        {
            item.InfoState = InfoState.Modified;
            DataToDbAddRequest<R_PhistoryInfo> dataToDbRequest = new DataToDbAddRequest<R_PhistoryInfo>();
            dataToDbRequest.Item = item;
            phistoryTodbService.AddItem(dataToDbRequest);
        }
        #endregion

        #region ----JC_B----

        /// <summary>
        /// 生成报警记录
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="sTime"></param>
        /// <param name="dataState"></param>
        /// <param name="state"></param>
        public static void CreateJC_BInfo(Jc_DefInfo defInfo, DateTime sTime, DeviceDataState dataState, DeviceRunState state)
        {
            Jc_BInfo alarmInfo = new Jc_BInfo();
            string controlport = string.Empty;

            alarmInfo.Cs = "";
            alarmInfo.PointID = defInfo.PointID;
            alarmInfo.ID = IdHelper.CreateLongId().ToString();
            alarmInfo.Devid = defInfo.Devid;
            alarmInfo.Fzh = defInfo.Fzh;
            alarmInfo.Kh = defInfo.Kh;
            alarmInfo.Dzh = defInfo.Dzh;
            alarmInfo.Kzk = controlport;
            alarmInfo.Point = defInfo.Point;

            alarmInfo.Ssz = EnumHelper.GetEnumDescription(state);
            alarmInfo.Isalarm = (short)defInfo.Alarm;

            alarmInfo.Stime = sTime;
            alarmInfo.Etime = new DateTime(1900, 1, 1, 0, 0, 0);
            alarmInfo.Type = (short)dataState;
            alarmInfo.State = GetState((DeviceRunState)state, defInfo.Bz4);
            alarmInfo.Upflag = "0";
            alarmInfo.Wzid = defInfo.Wzid;

            InsertJC_BInfo(alarmInfo);
        }
        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="alarmInfo"></param>
        private static void InsertJC_BInfo(Jc_BInfo alarmInfo)
        {
            //添加记录入缓存
            alarmInfo.Zdzs = new DateTime(1900, 1, 1, 0, 0, 0);
            AlarmCacheAddRequest addRequest = new AlarmCacheAddRequest();
            addRequest.AlarmInfo = alarmInfo;
            alarmCacheService.AddAlarmCache(addRequest);

            //添加报警信息至数据库
            DataToDbAddRequest<Jc_BInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_BInfo>();
            dataToDbAddRequest.Item = alarmInfo;
            alarmTodbService.AddItem(dataToDbAddRequest);
        }
        /// <summary>
        /// 结束报警记录
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="datastate"></param>
        /// <param name="time"></param>
        public static void EndJC_BInfo(string pointid, DeviceDataState datastate, DateTime time)
        {
            AlarmCacheGetByConditonRequest request = new AlarmCacheGetByConditonRequest();
            request.Predicate = p => p.Type == (short)datastate && p.PointID == pointid && (p.Etime < new DateTime(2000) || p.Etime == null);
            var result = alarmCacheService.GetAlarmCache(request);
            if (result.Data != null && result.IsSuccess)
            {
                List<Jc_BInfo> bItems = result.Data;
                foreach (Jc_BInfo item in bItems)
                {
                    item.Etime = time;
                    UpdateJC_BInfo(item);
                }
            }
        }
        /// <summary>
        /// 更新JC_B
        /// </summary>
        /// <param name="alarmInfo"></param>
        private static void UpdateJC_BInfo(Jc_BInfo alarmInfo)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            //updateItems.Add("State", alarmInfo.State);
            updateItems.Add("Etime", alarmInfo.Etime);
            //updateItems.Add("Zdz", alarmInfo.Zdz);
            //updateItems.Add("Pjz", alarmInfo.Pjz);
            //updateItems.Add("Zdzs", alarmInfo.Zdzs);
            //updateItems.Add("Kzk", alarmInfo.Kzk);
            //updateItems.Add("Isalarm", alarmInfo.Isalarm);

            //更新到缓存
            AlarmCacheUpdatePropertiesRequest alarmCacheUpdatePropertiesRequest = new AlarmCacheUpdatePropertiesRequest();
            alarmCacheUpdatePropertiesRequest.AlarmKey = alarmInfo.ID;
            alarmCacheUpdatePropertiesRequest.UpdateItems = updateItems;
            alarmCacheService.UpdateAlarmInfoProperties(alarmCacheUpdatePropertiesRequest);
            //更新到数据库
            System.Data.DataColumn[] cols = new System.Data.DataColumn[updateItems.Count];
            for (int i = 0; i < updateItems.Count; i++)
            {
                cols[i] = new System.Data.DataColumn(updateItems.Keys.ToList()[i]);
            }
            Jc_BModel alarmModel = ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(alarmInfo);
            List<Jc_BModel> alarmModels = new List<Jc_BModel>();
            alarmModels.Add(alarmModel);
            alarmRecordRepository.BulkUpdate("KJ_DataAlarm" + alarmInfo.Stime.ToString("yyyyMM"), alarmModels, cols, "ID");
        }

        #endregion

        #region ----R_PB----

        public static void CreateR_PBInfo(string pointID, string areaID, DateTime stime, PersonAlarmState datastate, R_PrealInfo prealInfo)
        {
            R_PbInfo pb = new R_PbInfo();
            // 编号
            pb.Id = IdHelper.CreateLongId().ToString();
            // 区域ID
            if (string.IsNullOrEmpty(areaID))
            {
                pb.Areaid = "0";
            }
            else
            {
                pb.Areaid = areaID;
            }
            // 人员Id
            pb.Yid = prealInfo.Yid;
            // 测点Id
            pb.Pointid = pointID;
            // 报警开始时间，等同于写记录时间
            pb.Starttime = stime;
            // 报警结束时间
            pb.Endtime = new DateTime(1900, 1, 1);
            // 报警类型(关联枚举表)
            pb.Type = (int)datastate;
            // 系统类型标志:0—人员,1—机车        
            pb.Z4 = prealInfo.Sysflag;
            pb.Z1 = "0";
            pb.Z2 = "0";
            pb.Z3 = "0";
            pb.Z5 = "0";
            pb.Z6 = "0";
            pb.InfoState = InfoState.AddNew;

            //更新数据库
            DataToDbAddRequest<R_PbInfo> request = new DataToDbAddRequest<R_PbInfo>();
            request.Item = pb;
            r_PbService.AddItem(request);
            //更新缓存
            R_PBCacheAddRequest addRequest = new R_PBCacheAddRequest();
            addRequest.R_PBInfo = pb;
            r_PBCacheService.AddR_PBCache(addRequest);
        }

        /// <summary>
        /// 结束R_PB报警
        /// </summary>
        /// <param name="datastate"></param>
        /// <param name="timeNow"></param>
        /// <param name="prealInfo"></param>
        public static void EndR_PBInfo(PersonAlarmState datastate, DateTime timeNow, R_PrealInfo prealInfo)
        {
            R_PBCacheGetByConditonRequest request = new R_PBCacheGetByConditonRequest();
            request.Predicate = a => a.Yid == prealInfo.Yid && a.Type == (int)datastate && (a.Endtime == null || a.Endtime < DateTime.Parse("2000-01-01"));
            var result = r_PBCacheService.GetR_PBCache(request);
            if (result.Data != null && result.IsSuccess)
            {

                List<R_PbInfo> pbItems = result.Data;
                pbItems.ForEach(a =>
                {
                    a.Endtime = timeNow;
                    a.InfoState = InfoState.Modified;
                });

                //更新数据库
                DataToDbBatchAddRequest<R_PbInfo> request1 = new DataToDbBatchAddRequest<R_PbInfo>();
                request1.Items = pbItems;
                r_PbService.AddItems(request1);
                //更新缓存
                R_PBCacheBatchUpdateRequest request2 = new R_PBCacheBatchUpdateRequest();
                request2.R_PBInfos = pbItems;
                r_PBCacheService.BatchUpdateR_PBCache(request2);
            }
        }

        public static List<R_PbInfo> GetAllR_PBItems()
        {
            List<R_PbInfo> items = new List<R_PbInfo>();
            R_PBCacheGetAllRequest request = new R_PBCacheGetAllRequest();
            var result = r_PBCacheService.GetAllR_PBCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }

        /// <summary>
        /// 人员报警表跨天数据处理(结束当天的报警，写第二天的开始)  20171206
        /// </summary>
        /// <param name="today"></param>
        /// <param name="tomorrow"></param>
        public static void Drv_CrossDayPro(DateTime today, DateTime tomorrow)
        {
            List<R_PbInfo> R_PBList = GetAllR_PBItems().FindAll(a => a.Endtime == null || a.Endtime.ToString("yyyy-MM-dd") == "1900-01-01");
            R_PBList.ForEach(a =>
            {
                a.Endtime = today;
                a.InfoState = InfoState.Modified;
            });
            //更新数据库
            DataToDbBatchAddRequest<R_PbInfo> request1 = new DataToDbBatchAddRequest<R_PbInfo>();
            request1.Items = R_PBList;
            r_PbService.AddItems(request1);
            //更新缓存
            R_PBCacheBatchUpdateRequest request2 = new R_PBCacheBatchUpdateRequest();
            request2.R_PBInfos = R_PBList;
            r_PBCacheService.BatchUpdateR_PBCache(request2);
            //写所有未结束的报警记录的第二天开始记录
            foreach (R_PbInfo tempR_PB in R_PBList)
            {
                R_PbInfo newPB = new R_PbInfo();
                newPB.Id = IdHelper.CreateLongId().ToString();
                newPB.Areaid = tempR_PB.Areaid;
                newPB.Yid = tempR_PB.Yid;
                newPB.Pointid = tempR_PB.Pointid;
                newPB.Zdzs = tempR_PB.Zdzs;
                newPB.Starttime = tomorrow;
                newPB.Endtime = new DateTime(1900, 1, 1, 0, 0, 0);
                newPB.Type = tempR_PB.Type;
                newPB.Z1 = tempR_PB.Z1;
                newPB.Z2 = tempR_PB.Z2;
                newPB.Z3 = tempR_PB.Z3;
                newPB.Z4 = tempR_PB.Z4;
                newPB.Z5 = tempR_PB.Z5;
                newPB.Z6 = tempR_PB.Z6;
                newPB.Upflag = "0";

                //更新数据库
                DataToDbAddRequest<R_PbInfo> request = new DataToDbAddRequest<R_PbInfo>();
                request.Item = newPB;
                r_PbService.AddItem(request);
                //更新缓存
                R_PBCacheAddRequest addRequest = new R_PBCacheAddRequest();
                addRequest.R_PBInfo = newPB;
                r_PBCacheService.AddR_PBCache(addRequest);
            }
        }
        /// <summary>
        /// 系统退出处理（结束数据库中的所有人员报警）
        /// </summary>
        public static void Drv_SystemExistPro(DateTime time)
        {
            List<R_PbInfo> R_PBList = GetAllR_PBItems().FindAll(a => a.Endtime == null || a.Endtime.ToString("yyyy-MM-dd") == "1900-01-01");
            R_PBList.ForEach(a =>
            {
                a.Endtime = time;
                a.InfoState = InfoState.Modified;
            });
            //更新数据库
            DataToDbBatchAddRequest<R_PbInfo> request1 = new DataToDbBatchAddRequest<R_PbInfo>();
            request1.Items = R_PBList;
            r_PbService.AddItems(request1);

        }
        #endregion

        #region ----JC_Def----
        public static void UpdateJC_defByProperties(Dictionary<string, Dictionary<string, object>> updateItems)
        {
            DefineCacheBatchUpdatePropertiesRequest request = new DefineCacheBatchUpdatePropertiesRequest();
            request.PointItems = updateItems;
            pointDefineService.BatchUpdatePointDefineInfo(request);
        }
        public static List<Jc_DefInfo> GetAllPersonPointInfo()
        {
            List<Jc_DefInfo> items = new List<Jc_DefInfo>();
            var result = personPointDefineService.GetAllPointDefineCache();
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data;
            }
            return items;
        }
        #endregion

        #region ----R_PHJ----

        public static void CreateR_PHJInfo(R_SyncLocalInfo syncLocalItem, R_PersoninfInfo personInfo, string pointID, DateTime timeNow)
        {
            R_PhjInfo phj = new R_PhjInfo();
            phj.Id = IdHelper.CreateLongId().ToString();
            phj.Hjlx = 0;
            phj.Bh = syncLocalItem.Bh;
            phj.Yid = personInfo.Yid;
            phj.PointId = pointID;
            phj.CallTime = syncLocalItem.Rtime;
            phj.State = 1;
            phj.Timer = timeNow;
            //井下呼叫地面(标志0-正常1-补传2-修改),地面呼叫井下(标志0-中心站未处理(WEB上设置时写0)1-中心站已处理)
            if (syncLocalItem.Passup == 1)
            {
                phj.Flag = 1;
            }
            else
            {
                phj.Flag = 0;
            }
            phj.SysFlag = syncLocalItem.Sysflag;
            phj.upflag = "1";
            phj.InfoState = InfoState.AddNew;

            DataToDbAddRequest<R_PhjInfo> request = new DataToDbAddRequest<R_PhjInfo>();
            request.Item = phj;
            phjTodbService.AddItem(request);
        }

        #endregion

        #region ----R_Call----

        public static List<R_CallInfo> GetCallItems()
        {
            List<R_CallInfo> callItems = new List<R_CallInfo>();
            RCallCacheGetAllRequest request = new RCallCacheGetAllRequest();
            var result = r_CallService.GetAllRCallCache(request);
            if (result.IsSuccess && result.Data != null)
            {
                callItems = result.Data;
            }
            return callItems;
        }

        public static void UpdateCallItems(Dictionary<string, Dictionary<string, object>> updateItems)
        {
            R_CallUpdateProperitesRequest request = new R_CallUpdateProperitesRequest();
            request.updateItems = updateItems;
            r_CallService.BachUpdateAlarmInfoProperties(request);
        }

        public static void DeleteCallItem(string callInfoID)
        {
            R_CallDeleteRequest request = new R_CallDeleteRequest();
            request.Id = callInfoID;
            r_CallService.DeleteCall(request);
        }
        #endregion

        #region ----R_Kqbc----
        public static R_KqbcInfo GetDefaultKqbc()
        {
            R_KqbcInfo kqbc = new R_KqbcInfo();
            kqbc = r_KqbcService.GetDefaultKqbcCache(new RKqbcCacheGetByConditionRequest()).Data;
            return kqbc;
        }
        #endregion


        #region----辅助方法----
        /// <summary>
        /// 综合用户设置的状态与设备本身的状态，获取要写入数据库的状态(现在bz4暂时不用，值为0)
        /// </summary>
        /// <param name="state">设备本身状态</param>
        /// <param name="bz4">用户定义的状态</param>
        /// <returns></returns>
        public static short GetState(DeviceRunState state, int bz4)
        {
            short myState = (short)state;

            if ((bz4 & 0x02) == 0x02)
            {
                myState = (short)DeviceRunState.EquipmentSleep;
            }
            else if ((bz4 & 0x04) == 0x04)
            {
                myState = (short)DeviceRunState.EquipmentDebugging;
            }
            else if ((bz4 & 0x08) == 0x08)
            {
                myState = (short)DeviceRunState.EquipmentAdjusting;
            }

            return myState;
        }
        /// <summary>
        /// 判断入口站
        /// </summary>
        /// <param name="myRecognizer"></param>
        /// <param name="recognizer"></param>
        /// <returns></returns>
        public static bool IsInStation(Recognizer myRecognizer)
        {
            bool success = false;
            if ((myRecognizer == Recognizer.AttendanceInStation)
                || (myRecognizer == Recognizer.InWellInStation)
                || (myRecognizer == Recognizer.GroundInStation))
            {
                success = true;
            }

            return success;
        }
        /// <summary>
        /// 判断入口站加出入口站
        /// </summary>
        /// <param name="myRecognizer"></param>
        /// <returns></returns>
        public static bool IsAllInStation(Recognizer myRecognizer)
        {
            bool success = false;

            if ((myRecognizer == Recognizer.AttendanceInStation)
                || (myRecognizer == Recognizer.AttendanceInOutStation)
                || (myRecognizer == Recognizer.InWellInStation)
                || (myRecognizer == Recognizer.InWellInOutStation)
                || (myRecognizer == Recognizer.GroundInStation)
                || (myRecognizer == Recognizer.GroundInOutStation))
            {
                success = true;
            }

            return success;
        }
        /// <summary>
        /// 判断出口站加出入口站
        /// </summary>
        /// <param name="myRecognizer"></param>
        /// <param name="recognizer"></param>
        /// <returns></returns>
        public static bool IsAllOutStation(Recognizer myRecognizer)
        {
            bool success = false;

            if ((myRecognizer == Recognizer.AttendanceOutStation)
               || (myRecognizer == Recognizer.AttendanceInOutStation)
               || (myRecognizer == Recognizer.InWellOutStation)
               || (myRecognizer == Recognizer.InWellInOutStation)
               || (myRecognizer == Recognizer.GroundOutStation)
               || (myRecognizer == Recognizer.GroundInOutStation))
            {
                success = true;
            }

            return success;
        }
        /// <summary>
        /// 判断出口站
        /// </summary>
        /// <param name="myRecognizer"></param>
        /// <returns></returns>
        public static bool IsOutStation(Recognizer myRecognizer)
        {
            bool success = false;

            if ((myRecognizer == Recognizer.AttendanceOutStation)
               || (myRecognizer == Recognizer.InWellOutStation)
               || (myRecognizer == Recognizer.GroundOutStation))
            {
                success = true;
            }

            return success;
        }
        #endregion
    }
}
