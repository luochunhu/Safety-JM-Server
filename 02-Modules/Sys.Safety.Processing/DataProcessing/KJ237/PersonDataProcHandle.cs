using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Pb;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 人员定位数据处理类
    /// </summary>
    public class PersonDataProcHandle
    {
        private static object obj = new object();
        private static volatile PersonDataProcHandle getInstance;
        public static PersonDataProcHandle Instance
        {
            get
            {
                LogHelper.Info("PersonDataProcHandle Instance start");
                if (getInstance == null)
                {
                    lock (obj)
                    {
                        if (getInstance == null)
                        {
                            getInstance = new PersonDataProcHandle();
                        }
                    }
                }
                LogHelper.Info("PersonDataProcHandle Instance end");
                return getInstance;
            }
        }

        //private static List<StationStateItem> stationStateItems = new List<StationStateItem>();
        private static List<PersonStateItem> personStateItems = new List<PersonStateItem>();

        bool isRun = true;

        private Thread DataProcThread = null;

        #region ----对外接口----

        public void Start()
        {
            AddOverTime();    //人员定位补录  20171206

            if (DataProcThread == null)
            {
                DataProcThread = new Thread(DataProc);
                DataProcThread.Start();
            }
        }
        public void Stop()
        {
            isRun = false;
        }

        public void AddOverTime()
        {
            try
            {
                DateTime time = DateTime.Now;
                //读取系统退出时间，做为数据补录的结束时间  20170703
                IConfigService configService = ServiceFactory.Create<IConfigService>();
                ConfigGetByNameRequest configGetByNameRequest = new ConfigGetByNameRequest();
                configGetByNameRequest.Name = "SystemExistTime";
                var resultConfig = configService.GetConfigByName(configGetByNameRequest);
                if (resultConfig != null && resultConfig.Data != null)
                {
                    DateTime tempTime = time;
                    DateTime.TryParse(resultConfig.Data.Text, out tempTime);
                    if (tempTime.ToShortDateString() == time.AddDays(-1).ToShortDateString())//昨天退出，今天才启动的情况下补昨天的23:59:59
                    {
                        tempTime = DateTime.Parse(tempTime.ToString("yyyy-MM-dd") + " 23:59:59");
                    }
                    else//其它情况，补当前启动时间
                    {
                        tempTime = time;
                    }
                    time = tempTime;
                }

                //更新报警记录
                IR_PbService r_PBService = ServiceFactory.Create<IR_PbService>();
                List<R_PbInfo> items = new List<R_PbInfo>();
                var result = r_PBService.GetAlarmedDataList();
                if (result.IsSuccess && result.Data != null)
                {
                    items = result.Data;
                    //批量更新报警记录
                    items.ForEach(a =>
                    {
                        a.Endtime = time;
                    });
                    R_PBBatchUpateRequest upRequest = new R_PBBatchUpateRequest();
                    upRequest.PbInfoList = items;
                    r_PBService.BacthUpdateAlarmRecord(upRequest);
                }
                //重新加载报警记录
                IR_PBCacheService r_PBCacheService = ServiceFactory.Create<IR_PBCacheService>();
                R_PBCacheLoadRequest loadRequest = new R_PBCacheLoadRequest();
                r_PBCacheService.LoadR_PBCache(loadRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 AddOverTime Error:" + ex);
            }
        }

        #endregion

        #region ----人员定位数据处理方法----
        private void DataProc()
        {
            List<R_SyncLocalInfo> r_SyncLocalItems;
            List<Jc_DefInfo> defItems;
            List<R_PersoninfInfo> personItems;
            R_PersoninfInfo personInfo;
            List<R_UndefinedDefInfo> undefineItems;
            List<R_PrealInfo> prealInfoItems;
            List<AreaInfo> areaItems;
            R_PrealInfo prealInfo;
            DateTime timeNow;
            int ryljcount = 0;//计算出井执行计时器
            while (isRun)
            {
                try
                {
                    int index = 0;
                    timeNow = DateTime.Now;
                    System.Diagnostics.Stopwatch sss = new System.Diagnostics.Stopwatch();
                    sss.Start();
                    sss.Stop();
                   
                    defItems = KJ237CacheHelper.GetAllPersonPointInfo();  
                    personItems = KJ237CacheHelper.GetAllPersonInfo();                    
                    undefineItems = KJ237CacheHelper.GetAllUndefineInfo();                   
                    prealInfoItems = KJ237CacheHelper.GetAllPrealInfo();                   
                    areaItems = KJ237CacheHelper.GetAllAreaInfo();
                    

                   

                    if (defItems.FindAll(a => a.Upflag != "1").Count < 1)
                    {
                        Thread.Sleep(1000);
                        continue;//如果没有人员定位定义的数据，则直接返回   20180131
                    }
                    //取 R_SyncLocal、Jc_DefInfo、R_PersoninfInfo、R_UndefinedDefInfo 数据
                    r_SyncLocalItems = KJ237CacheHelper.GetR_SyncLocalIetms();
                    //RsyncLocalDataProc 处理
                    r_SyncLocalItems = r_SyncLocalItems.OrderBy(a => a.Rtime).ToList();
                    foreach (R_SyncLocalInfo item in r_SyncLocalItems)
                    {
                        index++;
                        //一次性取出了多条数据，存在下一条数据与上一条数据是同一识别器下的同一人，会重复处理，所以每次处理需重新获取当前人员的Preal信息  20171213
                        prealInfo = KJ237CacheHelper.GetOnePrealInfo(item.Bh);
                        personInfo = personItems.FirstOrDefault(a => a.Bh == item.Bh);
                        if (personInfo == null)
                        {
                            //档案不存在，自动插入到数据库
                            personInfo = AddNewPerson(item.Bh);
                            if (personInfo == null)
                            {
                                LogHelper.Error("添加人员失败，bh = " + item.Bh);
                                continue;
                            }
                        }
                        RsyncLocalDataProc(item, personInfo, prealInfo, defItems, undefineItems, timeNow);
                        //清除 R_SyncLocal 记录 
                        //KJ237CacheHelper.DeleteR_SyncLocalIetms(item);
                    }

                    sss.Restart();
                    KJ237CacheHelper.BachDeleteR_SyncLocalIetms(r_SyncLocalItems); //todo  中途退出处理
                    sss.Stop();

                    prealInfoItems = KJ237CacheHelper.GetAllPrealInfo(); //经过上面的处理 Preal表里面的数据已经发生了变化，需要重新查询
                    //处理人员报警
                    ExeRyDataProc(defItems, prealInfoItems, areaItems, timeNow);
                    //统计识别器人数
                    prealInfoItems = KJ237CacheHelper.GetAllPrealInfo(); //经过上面的处理 Preal表里面的数据已经发生了变化，需要重新查询
                    StatisticalDataProc(defItems, prealInfoItems, timeNow);

                    //判定人员报警及离开，并刷新preal表,并更新入井时长-并删除昨天入井的记录、并把达到识别器离开时间的人员置为出井(每约30秒1次)  20171206
                    ryljcount++;
                    if (ryljcount >= 60)
                    {
                        ryljcount = 0;
                        PrealNormalProc(defItems, prealInfoItems, timeNow);
                        //入井时长统计
                        TimeStatistics(defItems, prealInfoItems, timeNow);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("KJ237 DataProc Error:" + ex);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 处理临时表数据
        /// </summary>
        /// <param name="syncLocalItem"></param>
        /// <param name="personInfo"></param>
        /// <param name="prealInfo"></param>
        /// <param name="defItems"></param>
        /// <param name="undefinedItem"></param>
        private void RsyncLocalDataProc(R_SyncLocalInfo syncLocalItem, R_PersoninfInfo personInfo, R_PrealInfo prealInfo, List<Jc_DefInfo> defItems, List<R_UndefinedDefInfo> undefinedItem, DateTime timeNow)
        {
            bool isAddNew = false;  //是否新增Preal表数据

            SettingInfo setting;

            bool isWritePreal = true;

            if (prealInfo == null)
            {
                isAddNew = true;
            }

            int jxFlag = 0;    //识别器类型（普通站0，出、入站 1）
            int outOrIn = 0;   //补传出、入井，正常出、入井

            int LessTime = 0;//人员实时数据超时时间 单位：分
            int sysflag = syncLocalItem.Sysflag;  //sysflag：0-人员  1-机车  255-人员第三方考勤
            if (sysflag == 255)
            {
                sysflag = 0;//人员第三方考勤在轨迹表中写入255但是在实时表中任然写入0
            }
            R_UndefinedDefInfo tempPointItem = null;

            Dictionary<string, Dictionary<string, object>> updateItems;
            Dictionary<string, object> updateItem;
            Jc_DefInfo defInfo = defItems.FirstOrDefault(a => a.Fzh == syncLocalItem.Fzh && a.Kh == syncLocalItem.Kh && a.DevPropertyID == (int)DeviceProperty.Recognizer);
            string point = syncLocalItem.Fzh.ToString().PadLeft(3, '0') + "R" + syncLocalItem.Kh.ToString().PadLeft(2, '0') + "0";
            if (defInfo == null)
            {
                #region ----人员识别器未定义处理----
                //从缓存中获取已添加的未定缓存
                tempPointItem = undefinedItem.FirstOrDefault(a => a.Point == point);
                if (tempPointItem == null)
                {
                    //此测点在系统定义和未定义设备缓存中均没有
                    tempPointItem = new R_UndefinedDefInfo();
                    tempPointItem.PointId = IdHelper.CreateLongId().ToString();
                    tempPointItem.Fzh = syncLocalItem.Fzh.ToString();
                    tempPointItem.Kh = syncLocalItem.Kh.ToString();
                    tempPointItem.Dzh = "0";
                    tempPointItem.Point = point;
                    tempPointItem.CreateUpdateTime = DateTime.Now;
                    tempPointItem.Devid = "0";
                    tempPointItem.type = Recognizer.InWellNomalStation;
                    //todo  添加到内存和数据库
                }
                #endregion
            }
            else
            {
                #region ----人员识别器已定义处理----
                tempPointItem = new R_UndefinedDefInfo();
                tempPointItem.PointId = defInfo.PointID;
                tempPointItem.Fzh = defInfo.Fzh.ToString();
                tempPointItem.Kh = defInfo.Kh.ToString();
                tempPointItem.Dzh = defInfo.Dzh.ToString();
                tempPointItem.Point = defInfo.Point;
                tempPointItem.CreateUpdateTime = defInfo.CreateUpdateTime;
                tempPointItem.Devid = defInfo.Devid;
                tempPointItem.type = (Recognizer)defInfo.Bz1;
                #endregion
            }

            #region ----R_Preal处理----
            setting = CacheDataHelper.GetSettingByKeyStr("LessTime");
            if (setting == null)
            {
                LessTime = 12 * 60;
            }
            else
            {
                LessTime = Convert.ToInt32(setting.StrValue);
            }
            if (syncLocalItem.Rtime > timeNow.AddMinutes(-1 * LessTime))//当上传的卡号与计算机的时间相差所指定的小时数时，只更改轨迹表，不更新实时表。（一般设定值为12个小时）。
            {
                if (tempPointItem != null)
                {
                    if (isAddNew)
                    {
                        #region ----新增----

                        //判断识别器不为出口站
                        if (!KJ237CacheHelper.IsOutStation(tempPointItem.type))
                        {
                            //查询该 BH 的最后一条R_P记录， if (tempdt.Rows.Count == 0 || (tempdt.Rows.Count > 0 && (jstime >= Convert.ToDateTime(tempdt.Rows[0]["rtime"]))))
                            R_PhistoryInfo r_History = null;
                            if (syncLocalItem.Passup == 1)
                            {
                                r_History = KJ237CacheHelper.GetLastHistoryRP(personInfo.Yid);
                            }

                            if ((r_History != null && r_History.Rtime < syncLocalItem.Rtime) || syncLocalItem.Passup == 0)
                            {
                                if (syncLocalItem.Passup == 1)
                                {
                                    //补传入井
                                    outOrIn = (int)PresonTrajectoryFlag.DelayInWellData;
                                }
                                else
                                {
                                    //正常入井
                                    outOrIn = (int)PresonTrajectoryFlag.NomalInWellData;
                                }

                                if (tempPointItem.type == Recognizer.InWellNomalStation)// 判断识别器类型为进下普通站
                                {
                                    jxFlag = 1;//普通站
                                }
                                else
                                {
                                    jxFlag = 0;//入口或出入站
                                }
                                //更新R_Preal
                                //str = string.Format("insert into R_Preal(bh,yid,point,onpoint,intime,rtime,ontime,bjtype,bjtime,bc,flag,upflag,szqy,sysflag,kbh,uppoint,uptime,devid,gcbh,wz,jxflag,ptptime,by2) VALUES ('{0:D}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','0','0','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{4}','0')",
                                //                            bh, yid, sbq.point, sbq.point, jstime, jstime, jstime, 0, DateTime.Now, GetXjBc(jstime, bh), qyid, sysflag, StaticObj.kbh, sbq.point, jstime, sbq.type, StaticObj.Gcbh, sbq.wz, jxflag);

                                prealInfo = new R_PrealInfo();
                                prealInfo.Id = IdHelper.CreateLongId().ToString();    //ID	ID编号
                                prealInfo.Bh = syncLocalItem.Bh;  //bh	标志卡号
                                prealInfo.Yid = personInfo.Yid;   //Yid	内部编号[主键]
                                //Devid	当前测点的设备类型ID
                                prealInfo.Pointid = tempPointItem.PointId; //Point  pointid	当前测点id 
                                prealInfo.Uppointid = tempPointItem.PointId;     //uppoint uppointid	经过测点号
                                prealInfo.Onpointid = tempPointItem.PointId;        //Onpoint onpointid	入井(第一)测点号                                       
                                prealInfo.Intime = syncLocalItem.Rtime;         //intime	进入当前测点时间
                                prealInfo.Rtime = syncLocalItem.Rtime;           //rtime	最近采集时间
                                prealInfo.Uptime = syncLocalItem.Rtime;            //uptime	经过测点采集时间
                                prealInfo.Ontime = syncLocalItem.Rtime;          //ontime	入井(第一)测点采集时间
                                //Wz	当前点的位置
                                //rjsc	入井时长
                                //bjtype	报警类型如下所述(枚举)
                                prealInfo.Bjtype = 0;
                                //bjtime	报警时间                                       
                                //bjjstime	报警结束时间
                                //ptptime	 经过井下第一个普通站的时间
                                if (jxFlag == 1)
                                {
                                    prealInfo.Ptptime = syncLocalItem.Rtime;
                                }
                                //bjsc	报警时长
                                //bc	班次
                                prealInfo.Bc = GetXjBc(syncLocalItem.Rtime, syncLocalItem.Bh).ToString();
                                //szqy	所在区域[区域编号]
                                //flag	标志0-正常，1-人员已出井
                                prealInfo.Flag = "0";
                                prealInfo.Sysflag = syncLocalItem.Sysflag.ToString();     //sysflag	系统类型标志:0—人员,1—机车
                                prealInfo.Jxflag = jxFlag.ToString();  //jxflag	地面井下标记
                                //by1	
                                //by2	
                                //by3	
                                //by4	
                                //by5	
                                //upflag	上传标志0-已传1-未传
                                KJ237CacheHelper.InsertPrealData(prealInfo);
                            }
                        }
                        else
                        {
                            Basic.Framework.Logging.LogHelper.Error(syncLocalItem.Bh + " 通过出口站非法入井！");
                        }
                        #endregion
                    }
                    else
                    {
                        if (syncLocalItem.Rtime >= prealInfo.Rtime)
                        {
                            #region ----卡号时间大于上次采集到的时间,当作正常处理----
                            updateItems = new Dictionary<string, Dictionary<string, object>>();
                            updateItem = new Dictionary<string, object>();
                            if (prealInfo.Flag == "1")//人员已离井
                            {
                                #region ----人员已离井---新的入井记录----
                                //如果入井的第一个点是出口站则不更新Preal表。
                                //(sbq.type != 3 && sbq.type != 7 && sbq.type != 10)
                                if (tempPointItem.type != Recognizer.AttendanceOutStation
                                    && tempPointItem.type != Recognizer.GroundOutStation
                                     && tempPointItem.type != Recognizer.InWellOutStation)
                                {
                                    #region 加入时间间隔判断,同一天再次入井时必须要有一个间隔时间  20171207
                                    bool onceAgainInWellFlag = false;
                                    TimeSpan wellSpacing = syncLocalItem.Rtime - prealInfo.Rtime;//当前入井的时间减去上一次出井的时间
                                    if (tempPointItem.type == Recognizer.InWellInOutStation || tempPointItem.type == Recognizer.InWellInStation)
                                    {
                                        //如果是井下入口站，5分钟之后才能再次入井。
                                        if (wellSpacing.TotalMinutes >= 5)
                                        {
                                            onceAgainInWellFlag = true;
                                        }
                                    }
                                    else if (tempPointItem.type == Recognizer.GroundInOutStation || tempPointItem.type == Recognizer.GroundInStation
                                        || tempPointItem.type == Recognizer.AttendanceInOutStation || tempPointItem.type == Recognizer.AttendanceInStation)
                                    {
                                        //如果是地面入口站或者考勤机入口站，20分钟之后才能再次入井。
                                        if (wellSpacing.TotalMinutes >= 20)
                                        {
                                            onceAgainInWellFlag = true;
                                        }
                                    }
                                    else
                                    {
                                        //非出入站，40分钟以后才能再次入井。
                                        if (wellSpacing.TotalMinutes >= 40)
                                        {
                                            onceAgainInWellFlag = true;
                                        }
                                    }
                                    #endregion
                                    if (onceAgainInWellFlag)
                                    {
                                        if (syncLocalItem.Passup == 1)
                                        {
                                            outOrIn = (int)PresonTrajectoryFlag.DelayInWellData;    //如果数据为补传,则标记为补传入井
                                        }
                                        else
                                        {
                                            outOrIn = (int)PresonTrajectoryFlag.NomalInWellData;    //如果数据不为补传,则标记为正常入井
                                        }


                                        // type = 1 井下普通站
                                        if (tempPointItem.type == Recognizer.InWellNomalStation)
                                        {
                                            jxFlag = 1;
                                        }
                                        else
                                        {
                                            jxFlag = 0;
                                        }

                                        //更新Preal
                                        //str = string.Format("
                                        //UPDATE R_Preal SET yid='{0}',bjtype=0,point='{1}',intime='{2}',rtime='{2}',uppoint='{1}',uptime='{2}',onpoint='{1}',ontime='{2}',flag='0',by2='0',bc='{3}',upflag='0'
                                        //,szqy='{4}',sysflag='{5}',devid='{9}',wz='{10}',jxflag='{11}',ptptime='{2}' 
                                        //WHERE kbh='{6}' and gcbh='{7}' and bh='{8:D}'",
                                        //yid, sbq.point, jstime, GetXjBc(jstime, bh), qyid, sysflag, StaticObj.kbh, StaticObj.Gcbh, bh, sbq.type, sbq.wz, jxflag);

                                        updateItem.Add("Yid", personInfo.Yid);
                                        updateItem.Add("Bjtype", 0);
                                        updateItem.Add("Pointid", tempPointItem.PointId);
                                        updateItem.Add("Rtime", syncLocalItem.Rtime);
                                        updateItem.Add("Intime", syncLocalItem.Rtime);
                                        updateItem.Add("Uppointid", tempPointItem.PointId);
                                        updateItem.Add("Uptime", syncLocalItem.Rtime);
                                        updateItem.Add("Onpointid", tempPointItem.PointId);
                                        updateItem.Add("Ontime", syncLocalItem.Rtime);
                                        updateItem.Add("Flag", 0);
                                        updateItem.Add("By2", 0);
                                        // todo  实时表暂时不写班次 updateItem.Add("bc", 0);
                                        updateItem.Add("Upflag", 0);
                                        updateItem.Add("JxFlag", jxFlag);
                                        updateItem.Add("Sysflag", syncLocalItem.Sysflag);
                                        updateItem.Add("Ptptime", syncLocalItem.Rtime);
                                    }
                                    else//如果人员已出井，未到下次入井的间隔时间入井，在出入站之间移动，也应该进行位置及时间更新  20171207
                                    {
                                        if (tempPointItem.PointId == prealInfo.Pointid)
                                        {
                                            if (prealInfo.Rtime != syncLocalItem.Rtime)
                                            {
                                                updateItem.Add("Rtime", syncLocalItem.Rtime);
                                            }
                                        }
                                        else
                                        {
                                            //位置变动,更新点号                                                      
                                            updateItem.Add("Pointid", tempPointItem.PointId);
                                            updateItem.Add("Intime", syncLocalItem.Rtime);
                                            updateItem.Add("Rtime", syncLocalItem.Rtime);
                                            updateItem.Add("Uppointid", prealInfo.Pointid);
                                            updateItem.Add("Uptime", prealInfo.Rtime);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                // 如果当前点为井口或者出入站（判断人员出井）
                                // if (sbq.type == 3 || sbq.type == 4 || sbq.type == 7 || sbq.type == 8 || sbq.type == 10 || sbq.type == 11)
                                if (KJ237CacheHelper.IsAllOutStation(tempPointItem.type))
                                {
                                    #region ----人员在井下---更新实时信息或者判断出井----

                                    if (prealInfo.Jxflag == "1")
                                    {
                                        if (syncLocalItem.Passup == 1)
                                        {
                                            outOrIn = (int)PresonTrajectoryFlag.DelayOutWellData;    //如果数据为补传,则置为补传出井
                                        }
                                        else
                                        {
                                            outOrIn = (int)PresonTrajectoryFlag.NomalOutWellData;    //如果数据不为补传,则置为正常出井
                                        }
                                        //str = string.Format(@"update r_preal set flag='1',bjtype=0,upflag='0',jxflag='0',by1='',devid='{0}',wz='{1}',rtime='{2}',intime='{2}',
                                        //point='{3}',uppoint='{4}',uptime='{5}'  where bh='{6}' and gcbh='{7}' and kbh='{8}'",
                                        //sbq.type, sbq.wz, jstime.ToString("yyyy-MM-dd HH:mm:ss"), sbq.point,
                                        //dtCur.Rows[0]["point"].ToString(), dtCur.Rows[0]["rtime"].ToString(), bh, StaticObj.Gcbh, StaticObj.kbh);

                                        updateItem.Add("Flag", 1);
                                        updateItem.Add("Bjtype", 0);
                                        updateItem.Add("Upflag", 0);
                                        updateItem.Add("Jxflag", 0);
                                        updateItem.Add("By1", "");
                                        updateItem.Add("Pointid", tempPointItem.PointId);
                                        updateItem.Add("Rtime", syncLocalItem.Rtime);
                                        updateItem.Add("Intime", syncLocalItem.Rtime);
                                        updateItem.Add("Uppointid", prealInfo.Pointid);
                                        updateItem.Add("Uptime", prealInfo.Rtime);
                                    }
                                    else
                                    {
                                        //未经过井下普通站，但是被考勤出口站识别到则立刻判断出井

                                        //if (sbq.type == 3 || sbq.type == 7 || sbq.type == 10)
                                        if (tempPointItem.type == Recognizer.InWellOutStation || tempPointItem.type == Recognizer.AttendanceOutStation
                                            || tempPointItem.type == Recognizer.GroundOutStation)
                                        {
                                            //if (passup)
                                            //{
                                            //    outorin = 7;    //如果数据为补传,则置为补传出井.
                                            //}
                                            //else
                                            //{
                                            //    outorin = 6;    //如果数据不为补传,则置为正常出井.
                                            //}
                                            if (syncLocalItem.Passup == 1)
                                            {
                                                outOrIn = (int)PresonTrajectoryFlag.DelayOutWellData;
                                            }
                                            else
                                            {
                                                outOrIn = (int)PresonTrajectoryFlag.NomalOutWellData;
                                            }
                                            //gouxsUP 2013-12-23 人员出井后需要更新以下三项--------------出井是一旦被出井识别器识别到，就会置为出井
                                            //否则出井时intime 进入当前测点时间不正确，上传程序取的出井时间就是intime
                                            //1:intime	datetime		进入当前测点时间
                                            //2:uppoint	nvarchar		经过测点号
                                            //3:uptime	datetime		经过测点采集时间
                                            //str = "UPDATE R_preal SET flag='1',bjtype=0,upflag='0',jxflag='0',devid='" + sbq.type + "',wz='" + sbq.wz + "',by1='',rtime='" + jstime.ToString("yyyy-MM-dd HH:mm:ss") + "',point='" + sbq.point + "'  WHERE bh='" + bh + "' and gcbh='" + StaticObj.Gcbh + "' and kbh='" + StaticObj.kbh + "'";
                                            //                                                    str = string.Format(@"update r_preal set flag='1',bjtype=0,upflag='0',jxflag='0',by1='',devid='{0}',wz='{1}',rtime='{2}',intime='{2}',
                                            //                                                                              point='{3}',uppoint='{4}',uptime='{5}'  where bh='{6}' and gcbh='{7}' and kbh='{8}'",
                                            //                                                                          sbq.type, sbq.wz, jstime.ToString("yyyy-MM-dd HH:mm:ss"), sbq.point,
                                            //                                                                          dtCur.Rows[0]["point"].ToString(), dtCur.Rows[0]["rtime"].ToString(), bh, StaticObj.Gcbh, StaticObj.kbh);

                                            updateItem.Add("Flag", 1);
                                            updateItem.Add("Bjtype", 0);
                                            updateItem.Add("Upflag", 0);
                                            updateItem.Add("Jxflag", 0);
                                            updateItem.Add("By1", "");
                                            updateItem.Add("Rtime", syncLocalItem.Rtime);
                                            updateItem.Add("Intime", syncLocalItem.Rtime);
                                            updateItem.Add("Pointid", tempPointItem.PointId);
                                            updateItem.Add("Uppointid", prealInfo.Pointid);
                                            updateItem.Add("Uptime", prealInfo.Rtime);

                                        }
                                        else//未经过井下普通站，在地面普通识别器之间移动，也应该进行位置及时间更新  20171207
                                        {
                                            if (tempPointItem.PointId == prealInfo.Pointid)
                                            {
                                                if (prealInfo.Rtime != syncLocalItem.Rtime)
                                                {
                                                    updateItem.Add("Rtime", syncLocalItem.Rtime);
                                                }
                                            }
                                            else
                                            {
                                                //位置变动,更新点号                                                 
                                                updateItem.Add("Pointid", tempPointItem.PointId);
                                                updateItem.Add("Intime", syncLocalItem.Rtime);
                                                updateItem.Add("Rtime", syncLocalItem.Rtime);
                                                updateItem.Add("Uppointid", prealInfo.Pointid);
                                                updateItem.Add("Uptime", prealInfo.Rtime);
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region----若是井下普通站则把jxFlag=1,同时根据配置按照井下普通站来计算班次----
                                    //if ((sbq.type == 1) && (dtCur.Rows[0]["jxflag"].ToString() == "0"))
                                    //{
                                    //    if (StaticObj.bcrjflag)
                                    //    {
                                    //        str = string.Format("UPDATE R_Preal SET jxflag='{0}',ptptime='{1}' WHERE gcbh='{3}' and kbh='{4}' and bh='{5}'",
                                    //                            1, jstime, GetXjBc(jstime, bh), StaticObj.Gcbh, StaticObj.kbh, bh);
                                    //        StaticObj.sql.AddSqlRealData(str);
                                    //    }
                                    //    else
                                    //    {
                                    //        str = string.Format("UPDATE R_Preal SET jxflag='{0}',ptptime='{1}',bc='{2}' WHERE gcbh='{3}' and kbh='{4}' and bh='{5}'",
                                    //                            1, jstime, GetXjBc(jstime, bh), StaticObj.Gcbh, StaticObj.kbh, bh);
                                    //        StaticObj.sql.AddSqlRealData(str);
                                    //    }
                                    //}

                                    if (tempPointItem.PointId == prealInfo.Pointid)
                                    {
                                        ////位置未变,不更新点号
                                        //str = string.Format("UPDATE R_Preal SET yid='{0}',rtime='{1}',flag='0',upflag='0',szqy='{2}',sysflag='{3}',wz='{4}',devid='{5}',by1='{9}' WHERE kbh='{6}' and gcbh='{7}' and bh='{8:D}'",
                                        //                yid, jstime, qyid, sysflag, sbq.wz, sbq.type, StaticObj.kbh, StaticObj.Gcbh, bh, tsgzflag ? "1" : "0");
                                        if (prealInfo.Rtime != syncLocalItem.Rtime)
                                        {
                                            updateItem.Add("Rtime", syncLocalItem.Rtime);
                                        }
                                    }
                                    else
                                    {
                                        //todo  位置变动,更新点号,报警标志相关定点超时或区域超时等.
                                        //point='{1}',intime='{2}',rtime='{2}',uppoint='{3}',uptime='{4}',flag='0',upflag='0'                                               

                                        updateItem.Add("Pointid", tempPointItem.PointId);
                                        updateItem.Add("Intime", syncLocalItem.Rtime);
                                        updateItem.Add("Rtime", syncLocalItem.Rtime);
                                        updateItem.Add("Uppointid", prealInfo.Pointid);
                                        updateItem.Add("Uptime", prealInfo.Rtime);
                                        updateItem.Add("Flag", 0);
                                        updateItem.Add("Upflag", 0);

                                        //str = string.Format("UPDATE R_Preal SET bjtype=bjtype&(~7709),yid='{0}',point='{1}',intime='{2}',rtime='{2}',uppoint='{3}',uptime='{4}',flag='0',upflag='0',szqy='{5}',sysflag='{6}',devid='{7}',wz='{11}',by1='{12}' WHERE gcbh='{8}' and kbh='{9}' and bh='{10:D}'",
                                        //     yid, sbq.point, jstime, dtCur.Rows[0]["point"].ToString(), dtCur.Rows[0]["rtime"].ToString(), qyid, sysflag, sbq.type, StaticObj.Gcbh, StaticObj.kbh, bh, sbq.wz, tsgzflag ? "1" : "0");

                                        //todo 若是井下普通站则把jxFlag=1,同时根据配置按照井下普通站来计算班次
                                        if (tempPointItem.type == Recognizer.InWellNomalStation && prealInfo.Jxflag == "0")
                                        {
                                            updateItem.Add("Ptptime", syncLocalItem.Rtime);
                                            updateItem.Add("Jxflag", "1");
                                            //更新人员班次  20171204
                                            updateItem.Add("Bc", GetXjBc(syncLocalItem.Rtime, syncLocalItem.Bh));
                                        }
                                    }
                                    #endregion
                                }
                            }
                            if (updateItem.Count > 0)
                            {
                                updateItems.Add(prealInfo.Id, updateItem);
                                KJ237CacheHelper.UpdatePrealByProperties(updateItems);
                            }
                            #endregion
                        }
                        else
                        {
                            #region ----补传数据----
                            if (prealInfo.Flag == "0")//人员在井下
                            {
                                if (tempPointItem.type != Recognizer.InWellNomalStation)//不修改正普通站，只修正出入井口。
                                {
                                    if (prealInfo.Pointid != tempPointItem.PointId)
                                    {
                                        if (syncLocalItem.Rtime < prealInfo.Ontime)
                                        {
                                            #region ----补传的入井记录----
                                            //1。补传的入井；jstime<ontime  ontime-jstime=8小时内
                                            //2。补传的出井；jstime>ontime  jstime-rtime=8小时内            
                                            if ((prealInfo.Ontime - syncLocalItem.Rtime).TotalMinutes <= 8 * 60)
                                            {
                                                // //todo  入井按照原先处理进行处理
                                                #region ----查看实时表中的“onpoint”是否是入井识别器----
                                                //Jc_DefInfo tempDef = defItems.FirstOrDefault(a => a.PointID == prealInfo.Onpointid);
                                                Jc_DefInfo tempDef = defItems.FirstOrDefault(a => a.PointID == tempPointItem.PointId);
                                                if (tempDef != null)
                                                {
                                                    //if (!KJ237CacheHelper.IsInStation((Recognizer)tempDef.Bz1))
                                                    //{
                                                    #region ----修正入井点----
                                                    if ((tempDef.Bz1 == (int)Recognizer.AttendanceInStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.AttendanceInOutStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.InWellInStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.InWellInOutStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.GroundInStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.GroundInOutStation)
                                                    || (tempDef.Bz1 == (int)Recognizer.GroundNomalStation))
                                                    {
                                                        // 更新preal.onPoint的轨迹记录为正常记录
                                                        //StaticObj.sql.AddSqlRealData(string.Format("update {0} set flag=0,upflag='0' where gcbh='{1}' and kbh='{2}' and bh='{3}' and point='{4}' and rtime='{5}'", 
                                                        //"R_P" + Convert.ToDateTime(dtCur.Rows[0]["ontime"]).ToString("yyyyMMdd"), StaticObj.Gcbh, StaticObj.kbh, bh, dtCur.Rows[0]["onpoint"].ToString(), dtCur.Rows[0]["ontime"].ToString()));
                                                        R_PhistoryInfo r_pInfo = KJ237CacheHelper.GetR_PByPar(prealInfo.Pointid, personInfo.Yid, prealInfo.Ontime);
                                                        if (r_pInfo != null)
                                                        {
                                                            r_pInfo.Flag = "0";
                                                            r_pInfo.Upflag = "0";
                                                            KJ237CacheHelper.UpdateR_P(r_pInfo);
                                                        }
                                                        if (syncLocalItem.Passup == 1)
                                                        {
                                                            outOrIn = (int)PresonTrajectoryFlag.DelayInWellData;
                                                        }
                                                        else
                                                        {
                                                            outOrIn = (int)PresonTrajectoryFlag.NomalInWellData;
                                                        }
                                                        // 修正入井班次及时间
                                                        //str = string.Format("UPDATE R_Preal SET onpoint='{0}',ontime='{1}',upflag='0',sysflag='{2}',bc='{6}' WHERE bh='{3:D}' and gcbh='{4}' and kbh='{5}'",
                                                        //       sbq.point, jstime, sysflag, bh, StaticObj.Gcbh, StaticObj.kbh, GetXjBc(jstime, bh));
                                                        updateItems = new Dictionary<string, Dictionary<string, object>>();
                                                        updateItem = new Dictionary<string, object>();
                                                        updateItem.Add("Onpointid", tempPointItem.PointId);
                                                        updateItem.Add("Ontime", syncLocalItem.Rtime);
                                                        //更新人员班次  20171207
                                                        updateItem.Add("Bc", GetXjBc(syncLocalItem.Rtime, syncLocalItem.Bh));
                                                        updateItem.Add("Upflag", 0);
                                                        updateItem.Add("Sysflag", syncLocalItem.Sysflag);
                                                        updateItems.Add(prealInfo.Id, updateItem);
                                                        KJ237CacheHelper.UpdatePrealByProperties(updateItems);
                                                    }
                                                    #endregion
                                                    //}
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region ----补传的出井记录----
                                            if ((syncLocalItem.Rtime - prealInfo.Ontime).TotalMinutes <= 8 * 60)
                                            {
                                                //todo  出井按照原先处理进行处理
                                                #region ----查看实时表中的“onpoint”是否是入井识别器----
                                                Jc_DefInfo tempDef = defItems.FirstOrDefault(a => a.PointID == tempPointItem.PointId);
                                                if (tempDef != null)
                                                {
                                                    //if (cur.type == 3 || cur.type == 4 || cur.type == 7 || cur.type == 8 || sbq.type == 10 || sbq.type == 11)
                                                    if (KJ237CacheHelper.IsAllOutStation((Recognizer)tempDef.Bz1))
                                                    {
                                                        //更改出井点，同时更新轨迹
                                                        // StaticObj.sql.AddSqlRealData(string.Format("update {0} set flag=0,upflag='0' where gcbh='{1}' and kbh='{2}' and bh='{3}' and point='{4}' and rtime='{5}'", 
                                                        //"R_P" + Convert.ToDateTime(dtCur.Rows[0]["rtime"]), StaticObj.Gcbh, StaticObj.kbh, bh, dtCur.Rows[0]["point"].ToString(), dtCur.Rows[0]["rtime"].ToString()));
                                                        R_PhistoryInfo r_pInfo = KJ237CacheHelper.GetR_PByPar(prealInfo.Pointid, personInfo.Yid, prealInfo.Rtime);
                                                        if (r_pInfo != null)
                                                        {
                                                            r_pInfo.Flag = "0";
                                                            r_pInfo.Upflag = "0";
                                                            KJ237CacheHelper.UpdateR_P(r_pInfo);
                                                        }
                                                        //StaticObj.sql.AddSqlRealData(string.Format("update {0} set flag=0,upflag='0' where gcbh='{1}' and kbh='{2}' and bh='{3}' and point='{4}' and rtime='{5}'", "R_P" + Convert.ToDateTime(dtCur.Rows[0]["rtime"]), StaticObj.Gcbh, StaticObj.kbh, bh, dtCur.Rows[0]["point"].ToString(), dtCur.Rows[0]["rtime"].ToString()));
                                                        if (prealInfo.Jxflag == "1")
                                                        {
                                                            if (syncLocalItem.Passup == 1)
                                                            {
                                                                outOrIn = (int)PresonTrajectoryFlag.DelayOutWellData;
                                                            }
                                                            else
                                                            {
                                                                outOrIn = (int)PresonTrajectoryFlag.NomalOutWellData;
                                                            }
                                                            //str = "UPDATE R_preal SET flag='1',bjtype=0,upflag='0',jxflag='0',devid='" + sbq.type + "',wz='" + sbq.wz + "',by1='',rtime='" + jstime.ToString("yyyy-MM-dd HH:mm:ss") + "',point='" + sbq.point + "'  WHERE bh='" + bh + "' and gcbh='" + StaticObj.Gcbh + "' and kbh='" + StaticObj.kbh + "'";
                                                            //更新Preal表
                                                            updateItems = new Dictionary<string, Dictionary<string, object>>();
                                                            updateItem = new Dictionary<string, object>();
                                                            updateItem.Add("Flag", 1);
                                                            updateItem.Add("Bjtype", 0);
                                                            updateItem.Add("Upflag", 0);
                                                            updateItem.Add("Jxflag", 0);
                                                            updateItem.Add("By1", "");
                                                            updateItem.Add("Rtime", syncLocalItem.Rtime);
                                                            updateItem.Add("Pointid", tempPointItem.PointId);
                                                            updateItems.Add(prealInfo.Id, updateItem);
                                                            KJ237CacheHelper.UpdatePrealByProperties(updateItems);
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    LogHelper.Error("RsyncLocalDataProc Error");
                }
            }
            else
            {
                LogHelper.Error("接收到超时数据,Rtime：" + syncLocalItem.Rtime + ",bh：" + syncLocalItem.Bh);
            }

            #region ----实时表中使用by2字段标识“经过第三方入井考勤”（暂无）----

            //todo 实时表中使用by2字段标识“经过第三方入井考勤”
            //if (attendance == 255 && (sbq.type == 2 || sbq.type == 4 || sbq.type == 6 || sbq.type == 8 || sbq.type == 9 || sbq.type == 11)
            //str = string.Format("update r_preal set by2='1' where flag='0' and bh='{0}'", bh);
            // todo 
            #endregion

            #endregion

            #region 将标识卡欠压标识，写到实时缓存扩展字段中，在后面报警处理中统一进行处理  2017
            if (syncLocalItem.Flag == "1" && prealInfo != null)
            {
                updateItems = new Dictionary<string, Dictionary<string, object>>();
                updateItem = new Dictionary<string, object>();
                updateItem.Add("PowerUnderVoltageFlag", syncLocalItem.Flag);
                updateItems.Add(prealInfo.Id, updateItem);
                KJ237CacheHelper.UpdatePrealByProperties(updateItems);
            }
            #endregion

            #region ----写呼叫表、写轨迹表----
            if (prealInfo == null || isAddNew || prealInfo.Rtime != syncLocalItem.Rtime || prealInfo.Pointid != tempPointItem.PointId)//如果测点一样，且Rtime也一样，不重复写记录  20171207
            {
                //写呼叫表
                if (syncLocalItem.Zt == 1)
                {
                    //向上呼叫，写R_PHJ表
                    KJ237CacheHelper.CreateR_PHJInfo(syncLocalItem, personInfo, tempPointItem.PointId, timeNow);
                }

                //写轨迹表
                AddR_PData(personInfo, tempPointItem, syncLocalItem, outOrIn.ToString());
            }
            #endregion
        }

        /// <summary>
        /// 报警处理
        /// </summary>
        private void ExeRyDataProc(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, List<AreaInfo> areaItems, DateTime timeNow)
        {
            try
            {
                //初始化临时状态结构体，待所有报警判断完成后，再将这里面的状态一并写回JC_Def和R_Preal
                IniState(defItems, prealInfoItems);

                //处理人员标识卡欠压  20171214
                Exe_PowerUnderVoltage(defItems, prealInfoItems, timeNow);
                //识别器超时报警处理
                Exe_PointTimeout(defItems, prealInfoItems, timeNow);
                // 识别器超员报警
                Exe_PointCountout(defItems, prealInfoItems, timeNow);
                // 识别器限制、禁止进入
                Exe_PointIllegalIn(defItems, prealInfoItems, timeNow);
                // 人员入井超时
                Exe_PersonInWellTimeout(prealInfoItems, defItems, timeNow);
                // 矿井入井超员
                Exe_WellCountout(prealInfoItems, timeNow);
                /// 区域超时
                Exe_AreaTimeout(defItems, areaItems, prealInfoItems, timeNow);
                /// 区域超员
                Exe_AreaOvercount(defItems, areaItems, prealInfoItems, timeNow);
                /// 区域限制进入
                Exe_BanAccess(defItems, areaItems, prealInfoItems, timeNow);
                // 区域禁止进入
                Exe_AreaUnAccess(defItems, areaItems, prealInfoItems, timeNow);
                //清除已离井人员报警
                Exe_ClearOutWell(prealInfoItems, timeNow);

                #region ----更新实时状态----

                Dictionary<string, Dictionary<string, object>> updateItems = new Dictionary<string, Dictionary<string, object>>();
                Dictionary<string, object> updateItem;
                //更新识别器报警状态
                //StationStateItem stationState;
                //foreach (Jc_DefInfo item in defItems)
                //{
                //    stationState = stationStateItems.FirstOrDefault(a => a.pointId == item.PointID);
                //    if (stationState != null)
                //    {
                //        if (item.DataState != stationState.datastate)
                //        {
                //            updateItem = new Dictionary<string, object>();
                //            updateItem.Add("DataState", stationState.datastate);
                //            updateItems.Add(item.PointID, updateItem);
                //        }
                //    }
                //}
                //if (updateItems.Count > 0)
                //{
                //    KJ237CacheHelper.UpdateJC_defByProperties(updateItems);
                //}
                //更新人员报警状态
                updateItems = new Dictionary<string, Dictionary<string, object>>();
                PersonStateItem personState;
                foreach (R_PrealInfo item in prealInfoItems)
                {
                    personState = personStateItems.FirstOrDefault(a => a.yid == item.Yid);
                    if (personState != null)
                    {
                        //修改，如果报警类型描述不相等，也更新到人员实时缓存  20171215
                        if (item.Bjtype != personState.datastate || item.BjtypeDesc != PersonAlarmStateControl.GetPersonAlarmDescription(personState.datastate))
                        {
                            updateItem = new Dictionary<string, object>();
                            updateItem.Add("Bjtype", personState.datastate);
                            updateItem.Add("BjtypeDesc", PersonAlarmStateControl.GetPersonAlarmDescription(personState.datastate));
                            updateItems.Add(item.Id, updateItem);
                        }
                    }
                }
                if (updateItems.Count > 0)
                {
                    KJ237CacheHelper.UpdatePrealByProperties(updateItems);
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 ExeRyDataProc Error:" + ex);
            }
        }

        /// <summary>
        /// 统计识别器人数
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        /// <param name="timeNow"></param>
        private void StatisticalDataProc(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                List<Jc_DefInfo> stationItems = defItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Substation).ToList();
                List<Jc_DefInfo> recognizerItems = defItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Recognizer).ToList();
                List<Jc_DefInfo> tempItems;

                Dictionary<string, Dictionary<string, object>> updateItems = new Dictionary<string, Dictionary<string, object>>();
                Dictionary<string, object> updateItem;
                List<R_PrealInfo> tempPrealItems;
                int allCount = 0;
                int abnormalCount = 0;
                //统计识别器下的人数
                foreach (Jc_DefInfo item in recognizerItems)
                {
                    tempPrealItems = prealInfoItems.Where(a => a.Pointid == item.PointID && a.Flag == "0").ToList();
                    allCount = tempPrealItems.Count;
                    abnormalCount = tempPrealItems.Count(a => a.Bjtype > 0);

                    if (item.K1 == allCount && item.K2 == abnormalCount && !string.IsNullOrEmpty(item.Ssz))
                    {
                        continue;  //数据未变化，不更新JC_DEF
                    }
                    item.K1 = allCount;
                    item.K2 = abnormalCount;
                    item.Ssz = allCount + " 人";

                    updateItem = new Dictionary<string, object>();
                    updateItem.Add("K1", allCount);
                    updateItem.Add("ssz", allCount + " 人");
                    updateItem.Add("K2", abnormalCount);
                    updateItems.Add(item.PointID, updateItem);
                }
                //统计人员分站下的人数
                foreach (Jc_DefInfo item in stationItems)
                {
                    allCount = 0;
                    abnormalCount = 0;
                    tempItems = recognizerItems.Where(a => a.Fzh == item.Fzh).ToList();
                    foreach (Jc_DefInfo childDef in tempItems)
                    {
                        allCount += childDef.K1;
                        abnormalCount += childDef.K2;
                    }
                    if (item.K1 == allCount && item.K2 == abnormalCount)
                    {
                        continue;  //数据未变化，不更新JC_DEF
                    }
                    item.K1 = allCount;
                    item.K2 = abnormalCount;
                    item.Ssz = allCount + " 人";

                    updateItem = new Dictionary<string, object>();
                    updateItem.Add("K1", allCount);
                    updateItem.Add("ssz", allCount + " 人");
                    updateItem.Add("K2", abnormalCount);
                    updateItems.Add(item.PointID, updateItem);
                }
                if (updateItems.Count > 0)
                {
                    KJ237CacheHelper.UpdateJC_defByProperties(updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 StatisticalDataProc Error:" + ex);
            }
        }

        /// <summary>
        /// 刷新preal表,并更新入井时长-并删除昨天入井的记录、并把达到识别器离开时间的人员置为出井(每30秒1次)
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        /// <param name="timeNow"></param>
        private void PrealNormalProc(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                #region//清除出井人员（今日之前入井并出井的人员）
                List<R_PrealInfo> PrealOutWellList = prealInfoItems.FindAll(a => a.Flag == "1" && a.Ontime < GetFirstBCTime());
                PrealOutWellList.ForEach(prealInfo =>
                {
                    KJ237CacheHelper.DeletePreal(prealInfo);
                });
                #endregion
                #region//人员出井判断（正常出井、强制出井）
                List<R_PrealInfo> PrealDownholeList = prealInfoItems.FindAll(a => a.Flag != "1");
                PrealDownholeList.ForEach(prealInfo =>
                {
                    //获取识别器的离开时间
                    int LeaveTime = 60 * 8; //默认8小时
                    Jc_DefInfo def = defItems.Find(a => a.PointID == prealInfo.Pointid);
                    if (def != null)
                    {
                        if (def.K5 != 0)
                        {
                            LeaveTime = def.K5;
                        }
                        else//如果未设置离开时间，则根据识别器类型判断
                        {
                            if ((Recognizer)def.Bz1 == Recognizer.GroundNomalStation || (Recognizer)def.Bz1 == Recognizer.InWellNomalStation)
                            {//普通站
                                LeaveTime = 8 * 60;
                            }
                            else //出入站
                            {
                                LeaveTime = 2 * 60;
                            }
                        }
                    }
                    //判断Preal表的记录是否满足出井条件
                    if (prealInfo.Rtime.AddMinutes(LeaveTime) < DateTime.Now)
                    {
                        #region//更新最后一条人员轨迹记录为出井标记
                        R_PhistoryInfo r_pInfo = KJ237CacheHelper.GetR_PByPar(prealInfo.Pointid, prealInfo.Yid, prealInfo.Rtime);
                        if (r_pInfo != null)
                        {
                            //写日志，方便分析
                            Basic.Framework.Logging.LogHelper.Info("人员达到离开时间出井，卡号：" + r_pInfo.Bh + "，原轨迹标记：" + r_pInfo.Flag + ",新轨迹标记：6");

                            r_pInfo.Flag = "6";//出井标记
                            r_pInfo.Upflag = "0";
                            KJ237CacheHelper.UpdateR_P(r_pInfo);
                        }
                        #endregion
                        //更新Preal出井
                        Dictionary<string, Dictionary<string, object>> updateItems = new Dictionary<string, Dictionary<string, object>>();
                        Dictionary<string, object> updateItem = new Dictionary<string, object>();
                        updateItem.Add("Flag", 1);
                        updateItem.Add("Bjtype", 0);
                        updateItem.Add("Upflag", 0);
                        updateItem.Add("Jxflag", 0);
                        updateItem.Add("By1", "");
                        updateItem.Add("Rtime", prealInfo.Rtime);
                        updateItem.Add("Pointid", prealInfo.Pointid);
                        updateItems.Add(prealInfo.Id, updateItem);
                        KJ237CacheHelper.UpdatePrealByProperties(updateItems);
                    }
                });
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 PrealNormalProc Error:" + ex);
            }
        }
        /// <summary>
        /// 统计入井时长等累计时间
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        /// <param name="timeNow"></param>
        private void TimeStatistics(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            Dictionary<string, Dictionary<string, object>> updateItems = new Dictionary<string, Dictionary<string, object>>();
            Dictionary<string, object> updateItem = new Dictionary<string, object>();
            //统计人员入井时长
            updateItems = new Dictionary<string, Dictionary<string, object>>();
            foreach (R_PrealInfo item in prealInfoItems)
            {
                item.Rjsc = ((int)(item.Rtime - item.Ontime).TotalMinutes).ToString();
                updateItem = new Dictionary<string, object>();
                updateItem.Add("Rjsc", item.Rjsc);
                updateItems.Add(item.Id, updateItem);
            }
            if (updateItems.Count > 0)
            {
                KJ237CacheHelper.UpdatePrealByProperties(updateItems);
            }
        }
        #endregion

        #region ----报警处理----

        /// <summary>
        /// 识别器超时报警处理
        /// </summary>
        private void Exe_PointTimeout(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                string pointid = "";
                Jc_DefInfo defItem;
                //从Preal表中获取测点超时人员           
                foreach (R_PrealInfo item in prealInfoItems)
                {
                    if (item.Sysflag == "0" && item.Flag == "0")
                    {
                        pointid = item.Pointid;
                        defItem = defItems.FirstOrDefault(a => a.PointID == pointid);
                        if (defItem != null)
                        {
                            if (defItem.K4 > 0 && (timeNow - item.Intime).TotalMinutes >= defItem.K4)//
                            {
                                //(R_PrealInfo preal, DeviceDataState state, int type, string pointID, string areaId, DateTime timeNow)
                                //定点超时报警
                                //UpdateStationStateItem(defItem, DeviceDataState.PointTimeout, 0, timeNow);
                                UpdatePersonStateItem(item, PersonAlarmState.PointTimeout, 0, defItem.PointID, defItem.Areaid, timeNow);
                            }
                            else
                            {
                                //定点超时报警取消
                                //UpdateStationStateItem(defItem, DeviceDataState.PointTimeout, 1, timeNow);
                                UpdatePersonStateItem(item, PersonAlarmState.PointTimeout, 1, defItem.PointID, defItem.Areaid, timeNow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exw_PointTimeout Error:" + ex);
            }
        }
        /// <summary>
        /// 识别器超员报警
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        private void Exe_PointCountout(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                int overCount = 0;
                List<R_PrealInfo> tempPreal;
                foreach (Jc_DefInfo item in defItems)
                {
                    tempPreal = prealInfoItems.Where(a => a.Pointid == item.PointID && a.Flag == "0" && a.Sysflag == "0").ToList();
                    overCount = tempPreal.Count - item.K3;
                    if (item.K3 > 0 && overCount > 0)
                    {

                        //UpdateStationStateItem(item, DeviceDataState.PointOverCount, 0, timeNow);
                        tempPreal = tempPreal.OrderByDescending(a => a.Rtime).Take(overCount).ToList(); ;
                        foreach (R_PrealInfo pItem in tempPreal)
                        {
                            UpdatePersonStateItem(pItem, PersonAlarmState.PointOverCount, 0, item.PointID, item.Areaid, timeNow);
                        }
                    }
                    else
                    {
                        foreach (R_PrealInfo pItem in tempPreal)
                        {
                            UpdatePersonStateItem(pItem, PersonAlarmState.PointOverCount, 1, item.PointID, item.Areaid, timeNow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_PointCountout Error:" + ex);
            }
        }
        /// <summary>
        /// 识别器限制(名单允许进入)、禁止进入（名单禁止进入）
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        private void Exe_PointIllegalIn(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                bool isHavePerson = false;
                List<R_RestrictedpersonInfo> notInList;
                List<R_PrealInfo> personInMyPoint;
                foreach (Jc_DefInfo item in defItems)
                {
                    personInMyPoint = prealInfoItems.Where(p => p.Pointid == item.PointID && p.Flag == "0" && p.Sysflag == "0").ToList();
                    #region ----限制进入判断----
                    isHavePerson = false;
                    if (item.RestrictedpersonInfoList != null)
                    {
                        notInList = item.RestrictedpersonInfoList.Where(a => a.Type == 0).ToList();
                        personInMyPoint.ForEach(p =>
                        {
                            if (notInList.Count > 0)
                            {//如果设置了限制进入人员  20171205
                                if (notInList.FirstOrDefault(n => n.Yid == p.Yid) != null)
                                {
                                    isHavePerson = true;
                                    //在名单内，解除限制进入
                                    UpdatePersonStateItem(p, PersonAlarmState.PersonBanAccess, 1, item.PointID, item.Areaid, timeNow);
                                }
                                else
                                {
                                    //不在名单内，限制进入报警
                                    UpdatePersonStateItem(p, PersonAlarmState.PersonBanAccess, 0, item.PointID, item.Areaid, timeNow);
                                }
                            }
                            else
                            {//如果没有设置限制进入，则直接解除  20171205
                                UpdatePersonStateItem(p, PersonAlarmState.PersonBanAccess, 1, item.PointID, item.Areaid, timeNow);
                            }
                        });
                    }
                    else
                    {//如果没有设置限制进入，则直接解除  20171205
                        personInMyPoint.ForEach(p =>
                        {
                            UpdatePersonStateItem(p, PersonAlarmState.PersonBanAccess, 1, item.PointID, item.Areaid, timeNow);
                        });
                    }
                    //if (isHavePerson)
                    //{
                    //    //识别器下有限制进入人员
                    //    //UpdateStationStateItem(item, DeviceDataState.PersonBanAccess, 0, timeNow);
                    //}
                    //else
                    //{
                    //    //识别器下没有限制进入人员
                    //    //UpdateStationStateItem(item, DeviceDataState.PersonBanAccess, 1, timeNow);
                    //}

                    #endregion

                    #region ----禁止进入判断----
                    isHavePerson = false;
                    if (item.RestrictedpersonInfoList != null)
                    {
                        notInList = item.RestrictedpersonInfoList.Where(a => a.Type == 1).ToList();
                        personInMyPoint.ForEach(p =>
                        {
                            if (notInList.Count > 0)
                            {//如果设置了禁止进入人员  20171205
                                if (notInList.FirstOrDefault(n => n.Yid == p.Yid) != null)
                                {
                                    isHavePerson = true;
                                    //人员禁止进入报警
                                    UpdatePersonStateItem(p, PersonAlarmState.PersonUnAccess, 0, item.PointID, item.Areaid, timeNow);
                                }
                                else
                                {
                                    //人员取消禁止进入
                                    UpdatePersonStateItem(p, PersonAlarmState.PersonUnAccess, 1, item.PointID, item.Areaid, timeNow);
                                }
                            }
                            else
                            {//如果没有设置禁止进入，则直接解除  20171205
                                //人员取消禁止进入
                                UpdatePersonStateItem(p, PersonAlarmState.PersonUnAccess, 1, item.PointID, item.Areaid, timeNow);
                            }
                        });
                    }
                    else
                    {//如果没有设置禁止进入，则直接解除  20171205
                        personInMyPoint.ForEach(p =>
                       {
                           //人员取消禁止进入
                           UpdatePersonStateItem(p, PersonAlarmState.PersonUnAccess, 1, item.PointID, item.Areaid, timeNow);
                       });
                    }
                    //if (isHavePerson)
                    //{
                    //    //识别器下有禁止进入人员
                    //    //UpdateStationStateItem(item, DeviceDataState.PersonUnAccess, 0, timeNow);
                    //}
                    //else
                    //{
                    //    //识别器下没有禁止进入人员
                    //    //UpdateStationStateItem(item, DeviceDataState.PersonUnAccess, 1, timeNow);
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_PointIllegalIn Error:" + ex);
            }
        }
        /// <summary>
        /// 人员入井超时
        /// </summary>
        /// <param name="prealInfoItems"></param>
        private void Exe_PersonInWellTimeout(List<R_PrealInfo> prealInfoItems, List<Jc_DefInfo> defItems, DateTime timeNow)
        {
            try
            {
                int InWellMaxTime = 8 * 60;
                SettingInfo setInfo = CacheDataHelper.GetSettingByKeyStr("InWellMaxTime");
                if (setInfo == null)
                {
                    LogHelper.Error("未找到配置项人员入井最大时长【InWellMaxTime】，取默认值8*60分！");
                    InWellMaxTime = 8 * 60;
                }
                else
                {
                    InWellMaxTime = Convert.ToInt32(setInfo.StrValue);
                }

                Jc_DefInfo def;
                string pointid, areaid;
                foreach (R_PrealInfo item in prealInfoItems)
                {
                    pointid = "0";
                    areaid = "0";
                    def = defItems.FirstOrDefault(a => a.PointID == item.Pointid);
                    if (def != null)
                    {
                        pointid = def.PointID;
                        areaid = def.Areaid;
                    }
                    if (InWellMaxTime > 0) //配置成0表示不启用  20171205
                    {
                        if ((timeNow - item.Ontime).TotalMinutes > InWellMaxTime
                            && item.Sysflag == "0"
                            && item.Flag == "0"
                            && item.Jxflag == "1")
                        {
                            UpdatePersonStateItem(item, PersonAlarmState.PersonInWellTimeout, 0, pointid, areaid, timeNow);
                        }
                        else
                        {
                            UpdatePersonStateItem(item, PersonAlarmState.PersonInWellTimeout, 1, pointid, areaid, timeNow);
                        }
                    }
                    else
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PersonInWellTimeout, 1, pointid, areaid, timeNow);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_PersonInWellTimeout Error:" + ex);
            }
        }
        /// <summary>
        /// 矿井入井超员
        /// </summary>
        /// <param name="prealInfoItems"></param>
        private void Exe_WellCountout(List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                int inWellMaxCount = 0;
                SettingInfo setInfo = CacheDataHelper.GetSettingByKeyStr("InWellMaxCount");
                if (setInfo == null)
                {
                    LogHelper.Error("未找到配置项人员入井最大人数【InWellMaxCount】，无法分析矿井入井是否超员！");
                }
                else
                {
                    inWellMaxCount = Convert.ToInt32(setInfo.StrValue);
                    List<R_PrealInfo> inWellPersonItems = prealInfoItems.Where(a => a.Sysflag == "0" && a.Flag == "0").OrderByDescending(a => a.Rtime).ToList();
                    int overPersonCount = inWellPersonItems.Count - inWellMaxCount;
                    if (inWellMaxCount > 0)//配置成0表示不启用  20171205
                    {
                        if (overPersonCount > 0)
                        {
                            inWellPersonItems = inWellPersonItems.Take(overPersonCount).ToList();
                            foreach (R_PrealInfo item in inWellPersonItems)
                            {
                                //矿井入井超员
                                UpdatePersonStateItem(item, PersonAlarmState.InWellOverCount, 0, "0", "0", timeNow);
                            }
                        }
                        else
                        {
                            //结束入井超员
                            foreach (R_PrealInfo item in prealInfoItems)
                            {
                                if (IsHaveDataState(item.Bjtype, PersonAlarmState.InWellOverCount))
                                {
                                    //结束入井超员
                                    UpdatePersonStateItem(item, PersonAlarmState.InWellOverCount, 1, "0", "0", timeNow);
                                }
                            }
                        }
                    }
                    else
                    {
                        //结束入井超员
                        foreach (R_PrealInfo item in prealInfoItems)
                        {
                            if (IsHaveDataState(item.Bjtype, PersonAlarmState.InWellOverCount))
                            {
                                //结束入井超员
                                UpdatePersonStateItem(item, PersonAlarmState.InWellOverCount, 1, "0", "0", timeNow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_WellCountout Error:" + ex);
            }
        }

        /// <summary>
        /// 区域超时
        /// </summary>
        private void Exe_AreaTimeout(List<Jc_DefInfo> defItems, List<AreaInfo> areaItems, List<R_PrealInfo> prealItems, DateTime timeNow)
        {
            List<string> inAreaYidItems = new List<string>(); //在区域内的人员信息
            List<R_PrealInfo> myAreaPrealItems = new List<R_PrealInfo>();
            List<Jc_DefInfo> defInMyArea;
            int areaMaxTime = 0;
            foreach (AreaInfo aItem in areaItems)
            {
                defInMyArea = defItems.Where(a => a.Areaid == aItem.Areaid).ToList();
                myAreaPrealItems = prealItems.Where(a => defInMyArea.FindIndex(d => d.PointID == a.Pointid) >= 0 && a.Sysflag == "0" && a.Flag == "0").ToList();

                areaMaxTime = GetMinutes(aItem.Bz1);
                foreach (R_PrealInfo pItem in myAreaPrealItems)
                {
                    inAreaYidItems.Add(pItem.Yid);
                    if (areaMaxTime > 0)
                    {
                        if ((timeNow - pItem.Intime).TotalMinutes > areaMaxTime)
                        {
                            //区域超时报警
                            UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 0, pItem.Pointid, aItem.Areaid, timeNow);
                        }
                        else
                        {
                            //区域超时报警取消
                            UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 1, pItem.Pointid, aItem.Areaid, timeNow);
                        }
                    }
                    else
                    {
                        //区域下的所有人解除区域超时报警
                        UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 1, pItem.Pointid, aItem.Areaid, timeNow);
                    }
                }
            }
            //查找上面没有处理过的人员（即不在任何区域内的人员）
            myAreaPrealItems = prealItems.Where(a => inAreaYidItems.FindIndex(n => n == a.Yid) < 0 && a.Sysflag == "0" && a.Flag == "0").ToList();
            foreach (R_PrealInfo pItem in myAreaPrealItems)
            {
                //未在任何区域内的人员强制取消区域报警
                UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 1, pItem.Pointid, "0", timeNow);
            }
        }
        /// <summary>
        /// 区域超员
        /// </summary>
        private void Exe_AreaOvercount(List<Jc_DefInfo> defItems, List<AreaInfo> areaItems, List<R_PrealInfo> prealItems, DateTime timeNow)
        {
            List<string> inAreaYidItems = new List<string>(); //在区域内的人员信息
            List<R_PrealInfo> myAreaPrealItems = new List<R_PrealInfo>();
            List<Jc_DefInfo> defInMyArea;
            int areaMaxCount = 0;
            int overCount = 0;
            foreach (AreaInfo aItem in areaItems)
            {
                defInMyArea = defItems.Where(a => a.Areaid == aItem.Areaid).ToList();
                myAreaPrealItems = prealItems.Where(a => defInMyArea.FindIndex(d => d.PointID == a.Pointid) >= 0 && a.Sysflag == "0" && a.Flag == "0").OrderByDescending(a => a.Rtime).ToList();
                areaMaxCount = Convert.ToInt32(aItem.Bz2);
                overCount = myAreaPrealItems.Count - areaMaxCount;
                if (overCount > 0)
                {
                    myAreaPrealItems = myAreaPrealItems.Take(overCount).ToList();
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        //超员报警
                        UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 0, pItem.Pointid, aItem.Areaid, timeNow);
                    }
                }
                else
                {
                    //未设置区域超员人员，所有人取消超员报警
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        //超员报警取消
                        UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 1, pItem.Pointid, aItem.Areaid, timeNow);
                    }
                }
            }
            //查找上面没有处理过的人员（即不在任何区域内的人员）
            myAreaPrealItems = prealItems.Where(a => inAreaYidItems.FindIndex(n => n == a.Yid) < 0 && a.Sysflag == "0" && a.Flag == "0").ToList();
            foreach (R_PrealInfo pItem in myAreaPrealItems)
            {
                //未在任何区域内的人员强制取消区域报警
                UpdatePersonStateItem(pItem, PersonAlarmState.AreaTimeout, 1, pItem.Pointid, "0", timeNow);
            }
        }
        /// <summary>
        /// 区域限制进入（名单允许进入）
        /// </summary>
        private void Exe_BanAccess(List<Jc_DefInfo> defItems, List<AreaInfo> areaItems, List<R_PrealInfo> prealItems, DateTime timeNow)
        {
            List<string> inAreaYidItems = new List<string>(); //在区域内的人员信息
            List<R_PrealInfo> myAreaPrealItems = new List<R_PrealInfo>();
            List<Jc_DefInfo> defInMyArea;

            List<R_ArearestrictedpersonInfo> doPersonList;

            foreach (AreaInfo aItem in areaItems)
            {
                defInMyArea = defItems.Where(a => a.Areaid == aItem.Areaid).ToList();
                myAreaPrealItems = prealItems.Where(a => defInMyArea.FindIndex(d => d.PointID == a.Pointid) >= 0 && a.Sysflag == "0" && a.Flag == "0").OrderByDescending(a => a.Rtime).ToList();

                //限制进入判断
                doPersonList = aItem.RestrictedpersonInfoList.Where(a => a.Type == 0).ToList();

                if (doPersonList.Count > 0)
                {
                    //myAreaPrealItems = myAreaPrealItems.Where(a => doPersonList.FindIndex(d => d.Yid == a.Yid) >= 0).ToList();//限制进入人员报警名单
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        if (doPersonList.FindIndex(d => d.Yid == pItem.Yid) >= 0)
                        {
                            //名单允许进入
                            UpdatePersonStateItem(pItem, PersonAlarmState.AreaBanAccess, 1, pItem.Pointid, aItem.Areaid, timeNow);
                        }
                        else
                        {
                            //名单不允许进入
                            if (IsHaveDataState(pItem.Bjtype, PersonAlarmState.AreaBanAccess))
                            {
                                UpdatePersonStateItem(pItem, PersonAlarmState.AreaBanAccess, 0, pItem.Pointid, aItem.Areaid, timeNow);
                            }
                        }
                    }
                }
                else
                {
                    //没有设置限制进入人员，所有人取消限制进入报警
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        //限制进入报警取消
                        UpdatePersonStateItem(pItem, PersonAlarmState.AreaBanAccess, 1, pItem.Pointid, aItem.Areaid, timeNow);
                    }
                }
            }
            //查找上面没有处理过的人员（即不在任何区域内的人员）
            myAreaPrealItems = prealItems.Where(a => inAreaYidItems.FindIndex(n => n == a.Yid) < 0 && a.Sysflag == "0" && a.Flag == "0").ToList();
            foreach (R_PrealInfo pItem in myAreaPrealItems)
            {
                //未在任何区域内的人员强制取消区域报警
                UpdatePersonStateItem(pItem, PersonAlarmState.AreaBanAccess, 1, pItem.Pointid, "0", timeNow);
            }
        }
        /// <summary>
        /// 区域禁止进入
        /// </summary>
        private void Exe_AreaUnAccess(List<Jc_DefInfo> defItems, List<AreaInfo> areaItems, List<R_PrealInfo> prealItems, DateTime timeNow)
        {
            List<string> inAreaYidItems = new List<string>(); //在区域内的人员信息
            List<R_PrealInfo> myAreaPrealItems = new List<R_PrealInfo>();
            List<Jc_DefInfo> defInMyArea;

            List<R_ArearestrictedpersonInfo> doPersonList;

            foreach (AreaInfo aItem in areaItems)
            {
                defInMyArea = defItems.Where(a => a.Areaid == aItem.Areaid).ToList();
                myAreaPrealItems = prealItems.Where(a => defInMyArea.FindIndex(d => d.PointID == a.Pointid) >= 0 && a.Sysflag == "0" && a.Flag == "0").OrderByDescending(a => a.Rtime).ToList();

                //禁止进入判断
                doPersonList = aItem.RestrictedpersonInfoList.Where(a => a.Type == 1).ToList();

                if (doPersonList.Count > 0)
                {
                    //myAreaPrealItems = myAreaPrealItems.Where(a => doPersonList.FindIndex(d => d.Yid == a.Yid) >= 0).ToList();//禁止进入人员报警名单
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        if (doPersonList.FindIndex(d => d.Yid == pItem.Yid) >= 0)
                        {
                            //禁止进入报警
                            UpdatePersonStateItem(pItem, PersonAlarmState.AreaUnAccess, 0, pItem.Pointid, aItem.Areaid, timeNow);
                        }
                        else
                        {
                            //禁止进入报警取消
                            if (IsHaveDataState(pItem.Bjtype, PersonAlarmState.AreaUnAccess))
                            {
                                UpdatePersonStateItem(pItem, PersonAlarmState.AreaUnAccess, 1, pItem.Pointid, aItem.Areaid, timeNow);
                            }
                        }
                    }
                }
                else
                {
                    //没有设置禁止进入人员，所有人取消限制进入报警
                    foreach (R_PrealInfo pItem in myAreaPrealItems)
                    {
                        inAreaYidItems.Add(pItem.Yid);
                        //禁止进入报警取消
                        UpdatePersonStateItem(pItem, PersonAlarmState.AreaUnAccess, 0, pItem.Pointid, aItem.Areaid, timeNow);
                    }
                }
            }
            //查找上面没有处理过的人员（即不在任何区域内的人员）
            myAreaPrealItems = prealItems.Where(a => inAreaYidItems.FindIndex(n => n == a.Yid) < 0 && a.Sysflag == "0" && a.Flag == "0").ToList();
            foreach (R_PrealInfo pItem in myAreaPrealItems)
            {
                //未在任何区域内的人员强制取消区域报警
                UpdatePersonStateItem(pItem, PersonAlarmState.AreaUnAccess, 1, pItem.Pointid, "0", timeNow);
            }
        }
        /// <summary>
        /// 已出井人员清除报警
        /// </summary>
        /// <param name="prealInfoItems"></param>
        private void Exe_ClearOutWell(List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                List<R_PrealInfo> outWellPersonItems = prealInfoItems.Where(a => a.Flag == "1").ToList();
                foreach (R_PrealInfo item in outWellPersonItems)
                {
                    //识别器超时报警-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.PointTimeout))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PointTimeout, 1, null, null, timeNow);
                    }
                    // 识别器超员报警-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.PointOverCount))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PointOverCount, 1, null, null, timeNow);
                    }
                    // 识别器限制-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.PersonBanAccess))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PersonBanAccess, 1, null, null, timeNow);
                    }
                    //禁止进入-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.PersonUnAccess))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PersonUnAccess, 1, null, null, timeNow);
                    }
                    // 人员入井超时-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.PersonInWellTimeout))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.PersonInWellTimeout, 1, null, null, timeNow);
                    }
                    // 矿井入井超员-解除
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.InWellOverCount))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.InWellOverCount, 1, null, null, timeNow);
                    }
                    //区域超时
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.AreaTimeout))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.AreaTimeout, 1, null, null, timeNow);
                    }
                    //区域超员
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.AreaOverCount))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.AreaOverCount, 1, null, null, timeNow);
                    }
                    //区域限制进入
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.AreaBanAccess))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.AreaBanAccess, 1, null, null, timeNow);
                    }
                    //区域禁止进入
                    if (IsHaveDataState(item.Bjtype, PersonAlarmState.AreaUnAccess))
                    {
                        UpdatePersonStateItem(item, PersonAlarmState.AreaUnAccess, 1, null, null, timeNow);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_ClearOutWell Error:" + ex);
            }
        }

        /// <summary>
        /// 处理人员标识卡欠压  20171214
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        /// <param name="timeNow"></param>
        private void Exe_PowerUnderVoltage(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems, DateTime timeNow)
        {
            try
            {
                string pointid = "";
                Jc_DefInfo defItem;
                //从Preal表中获取标识卡欠压标识      
                foreach (R_PrealInfo item in prealInfoItems)
                {
                    if (item.Sysflag == "0" && item.Flag == "0")
                    {
                        pointid = item.Pointid;
                        defItem = defItems.FirstOrDefault(a => a.PointID == pointid);
                        if (defItem != null)
                        {
                            if (item.PowerUnderVoltageFlag == "1")
                            {     //报警开始                           
                                UpdatePersonStateItem(item, PersonAlarmState.PowerUnderVoltage, 0, defItem.PointID, defItem.Areaid, timeNow);
                            }
                            else
                            {
                                //报警取消                                
                                UpdatePersonStateItem(item, PersonAlarmState.PowerUnderVoltage, 1, defItem.PointID, defItem.Areaid, timeNow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 Exe_PowerUnderVoltage Error:" + ex);
            }
        }
        #endregion

        #region ----辅助方法----

        /// <summary>
        /// 判断人员当前的数据状态是否包含某一状态
        /// </summary>
        /// <param name="oldDataState"></param>
        /// <param name="myDataState"></param>
        /// <returns></returns>
        private bool IsHaveDataState(int oldDataState, PersonAlarmState myDataState)
        {
            bool isHave = false;
            int address = (int)myDataState;
            if (((oldDataState >> address) & 0x01) == 0x01)
            {
                isHave = true;
            }
            return isHave;
        }

        private void AddR_PData(R_PersoninfInfo personInfo, R_UndefinedDefInfo defInfo, R_SyncLocalInfo syncLocalItem, string Flag)
        {
            R_PhistoryInfo item = new R_PhistoryInfo();
            item.Id = IdHelper.CreateLongId().ToString();     //ID	编号[主键]
            item.Bh = personInfo.Bh.ToString();     //bh	标志卡号
            item.Yid = personInfo.Yid.ToString();   //Yid	内部编号
            item.Pointid = defInfo.PointId;  //Pointid	识别点号
            item.Rtime = syncLocalItem.Rtime; //Rtime	采集时间
            item.Timer = DateTime.Now;  //timer	写数据库时间
            item.Flag = Flag;   //Flag	记录标志,
            //    0-正常采集，1-补传采集，2-人工编辑；
            //    3-正常入井，4-补传入井，5-人工编辑入井；
            //    6-正常出井，7-补传出井，8-人工编辑出井。
            //Szqy	当前点号所在区域(1——255的区域号)
            item.Sysflag = syncLocalItem.Sysflag.ToString();   //sysflag	系统类型标志:0—人员,1—机车
            item.Cwflag = "1";   //cwflag	标注该轨迹数据是否有用
            //by1	备用
            //by2	备用
            //by3	备用
            //by4	备用
            //by5	备用
            item.Upflag = "0";    //Upflag	上传标志，0—未传，1—已传


            KJ237CacheHelper.InsertR_P(item);
        }

        private R_PersoninfInfo AddNewPerson(string bh)
        {
            R_PersoninfInfo personInfo = new R_PersoninfInfo();
            try
            {
                // Sql.Execution(string.Format("insert into r_personinf(bh,yid,sfyx,sysflag,gcbh,kbh,a0,kqgz,bzcgz,a22)
                // values('{0:D}','{1}','1','{2}','{3}','{4}','0','0','0','0')", bh, Yid[0], sysflag, StaticObj.Gcbh, StaticObj.kbh), "r_personinf", 1);
                personInfo.Id = IdHelper.CreateLongId().ToString();
                personInfo.Yid = personInfo.Id;
                personInfo.Bh = bh;
                personInfo.Sfyx = "1";
                personInfo.A0 = "0";
                personInfo.Kqgz = 0;
                personInfo.Bzcgz = 0;
                personInfo.A22 = "0";
                KJ237CacheHelper.AddPerson(personInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PersonDataProcHandle-AddNewPerson Error:" + ex.Message);
                personInfo = null;
            }
            return personInfo;
        }

        private int GetMinutes(string str)
        {
            return (int)(Convert.ToDateTime("2000-1-1 " + str) - new DateTime(2000, 1, 1)).TotalMinutes;
        }
        /// <summary>
        /// 获取班次  20171204
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bh"></param>
        /// <returns></returns>
        public Byte GetXjBc(DateTime t, string bh)
        {
            #region 判断下井班次
            Byte bc = 0;
            try
            {
                R_KqbcInfo kqbc = KJ237CacheHelper.GetDefaultKqbc();
                if (kqbc == null)
                {
                    return bc;
                }
                ushort[] uBc = new ushort[4];
                byte bBcs = byte.Parse(kqbc.Bcs);
                if (!string.IsNullOrEmpty(kqbc.B1stime) && kqbc.B1stime.Contains(":"))
                {
                    uBc[0] = (ushort)(int.Parse(kqbc.B1stime.Split(':')[0]) * 100 + int.Parse(kqbc.B1stime.Split(':')[1]));
                }
                if (!string.IsNullOrEmpty(kqbc.B2stime) && kqbc.B2stime.Contains(":"))
                {
                    uBc[1] = (ushort)(int.Parse(kqbc.B2stime.Split(':')[0]) * 100 + int.Parse(kqbc.B2stime.Split(':')[1]));
                }
                if (!string.IsNullOrEmpty(kqbc.B3stime) && kqbc.B3stime.Contains(":"))
                {
                    uBc[2] = (ushort)(int.Parse(kqbc.B3stime.Split(':')[0]) * 100 + int.Parse(kqbc.B3stime.Split(':')[1]));
                }
                if (!string.IsNullOrEmpty(kqbc.B4stime) && kqbc.B4stime.Contains(":"))
                {
                    uBc[3] = (ushort)(int.Parse(kqbc.B4stime.Split(':')[0]) * 100 + int.Parse(kqbc.B4stime.Split(':')[1]));
                }
                //从人员考勤规则中获取当前人员的班次信息（未实现）------------------
                //if (StaticFuction.IniReadValue("RY", "WebBcyx", StaticObj.ExePathName + "\\config.ini") == "1")
                //{
                //    bcdr = StaticObj.PersonInf.Select("bh='" + bh + "'");
                //    if (bcdr.Length > 0)
                //    {
                //        if (bcdr[0]["bcs"] != null && bcdr[0]["bcs"].ToString() != "")
                //        {
                //            bBcs = Convert.ToByte(bcdr[0]["bcs"]);
                //        }
                //        if (bcdr[0]["b1stime"].ToString() != "")
                //        {
                //            uBc[0] = Convert.ToUInt16(bcdr[0]["b1stime"]);
                //        }
                //        if (bcdr[0]["b2stime"].ToString() != "")
                //        {
                //            uBc[1] = Convert.ToUInt16(bcdr[0]["b2stime"]);
                //        }
                //        if (bcdr[0]["b3stime"].ToString() != "")
                //        {
                //            uBc[2] = Convert.ToUInt16(bcdr[0]["b3stime"]);
                //        }
                //        if (bcdr[0]["b4stime"].ToString() != "")
                //        {
                //            uBc[3] = Convert.ToUInt16(bcdr[0]["b4stime"]);
                //        }
                //    }
                //}
                //---------------------------
                int tjsj = t.Hour * 100 + t.Minute;
                if (bBcs == 2)
                {
                    if (tjsj >= uBc[0] && tjsj <= uBc[1]) bc = 1;
                    else if (tjsj >= uBc[1] && tjsj <= uBc[2]) bc = 2;
                    else
                    {
                        if ((uBc[1] < uBc[0]) && (tjsj >= uBc[0] || tjsj < uBc[1])) bc = 1;
                        if ((uBc[0] < uBc[1]) && (tjsj >= uBc[1] || tjsj < uBc[0])) bc = 2;
                    }
                }
                else if (bBcs == 4)
                {

                    if (tjsj >= uBc[0] && tjsj < uBc[1]) bc = 1;
                    else if (tjsj >= uBc[1] && tjsj < uBc[2]) bc = 2;
                    else if (tjsj >= uBc[2] && tjsj < uBc[3]) bc = 3;
                    else if (tjsj >= uBc[3] && tjsj < uBc[0]) bc = 4;
                    else
                    {
                        if ((uBc[1] < uBc[0]) && (tjsj >= uBc[0] || tjsj < uBc[1])) bc = 1;
                        if ((uBc[2] < uBc[1]) && (tjsj >= uBc[1] || tjsj < uBc[2])) bc = 2;
                        if ((uBc[3] < uBc[2]) && (tjsj >= uBc[2] || tjsj < uBc[3])) bc = 3;
                        if ((uBc[0] < uBc[3]) && (tjsj >= uBc[3] || tjsj < uBc[0])) bc = 4;
                    }
                }
                else
                {
                    if (tjsj >= uBc[0] && tjsj < uBc[1]) bc = 1;
                    else if (tjsj >= uBc[1] && tjsj < uBc[2]) bc = 2;
                    else if (tjsj >= uBc[2] && tjsj < uBc[0]) bc = 3;
                    else
                    {
                        if ((uBc[1] < uBc[0]) && (tjsj >= uBc[0] || tjsj < uBc[1])) bc = 1;
                        if ((uBc[2] < uBc[1]) && (tjsj >= uBc[1] || tjsj < uBc[2])) bc = 2;
                        if ((uBc[0] < uBc[2]) && (tjsj >= uBc[2] || tjsj < uBc[0])) bc = 3;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return bc;
            #endregion
        }

        public DateTime GetFirstBCTime()
        {
            DateTime t1, t2, t;
            t = DateTime.Now;
            t1 = DateTime.MinValue;
            t2 = DateTime.MinValue;
            ushort[] uBc = new ushort[2];

            R_KqbcInfo kqbc = KJ237CacheHelper.GetDefaultKqbc();
            if (kqbc == null)//没有默认班次,时间往前减1天  20171206
            {
                return t.AddDays(-1.0f);
            }
            byte bBcs = byte.Parse(kqbc.Bcs);
            if (!string.IsNullOrEmpty(kqbc.B1stime) && kqbc.B1stime.Contains(":"))
            {
                uBc[0] = (ushort)(int.Parse(kqbc.B1stime.Split(':')[0]) * 100 + int.Parse(kqbc.B1stime.Split(':')[1]));
            }
            if (!string.IsNullOrEmpty(kqbc.B2stime) && kqbc.B2stime.Contains(":"))
            {
                uBc[1] = (ushort)(int.Parse(kqbc.B2stime.Split(':')[0]) * 100 + int.Parse(kqbc.B2stime.Split(':')[1]));
            }

            t1 = DateTime.Parse(string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00", new object[] { t.Year, t.Month, t.Day, (int)(uBc[0] / 100), uBc[0] % 100 }));
            t2 = DateTime.Parse(string.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:00", new object[] { t.Year, t.Month, t.Day, (int)(uBc[1] / 100), uBc[1] % 100 }));

            if (t1 > t2)
            {
                t1 = t1.AddDays(-1);
            }
            else
            {
                if (t1 > t)
                {
                    t1 = t1.AddDays(-1);
                }
            }

            return t1 > DateTime.MinValue ? t1 : t;
        }

        #endregion

        #region ----临时缓存操作----
        /// <summary>
        /// 初始化临时状态
        /// </summary>
        /// <param name="defItems"></param>
        /// <param name="prealInfoItems"></param>
        /// <param name="stationStateItems"></param>
        /// <param name="personStateItems"></param>
        private void IniState(List<Jc_DefInfo> defItems, List<R_PrealInfo> prealInfoItems)
        {
            //stationStateItems = new List<StationStateItem>();   
            //foreach (Jc_DefInfo item in defItems)
            //{
            //    stationStateItems.Add(new StationStateItem(item.PointID, item.State));
            //}
            personStateItems = new List<PersonStateItem>();
            foreach (R_PrealInfo item in prealInfoItems)
            {
                personStateItems.Add(new PersonStateItem(item.Yid, item.Bjtype));
            }
        }

        ///// <summary>
        ///// 修改识别器临时状态
        ///// </summary>
        ///// <param name="pointid"></param>
        ///// <param name="datastate"></param>
        ///// <param name="type">0 add,1 delete</param>
        //private void UpdateStationStateItem(Jc_DefInfo def, PersonAlarmState datastate, int type, DateTime timeNow)
        //{
        //    try
        //    {
        //        //int index = stationStateItems.FindIndex(a => a.pointId == def.PointID);
        //        //int stateAddress = (1 << (int)datastate );
        //        //if (index >= 0)
        //        //{
        //        //    if (type == 0)
        //        //    {
        //        //        if (((def.DataState - (int)DeviceDataState.nomal) & stateAddress) != stateAddress)
        //        //        {
        //        //            //之前没有此种报警，现在产生了
        //        //            stationStateItems[index].datastate |= stateAddress;
        //        //            //开始JC_B记录
        //        //            KJ237CacheHelper.CreateJC_BInfo(def, timeNow, datastate, (DeviceRunState)def.State);
        //        //        }
        //        //    }
        //        //    else if (type == 1)
        //        //    {
        //        //        if (((def.DataState - (int)DeviceDataState.nomal) & stateAddress) == stateAddress)
        //        //        {
        //        //            //之前有此种报警，现在没有了
        //        //            stationStateItems[index].datastate &= (0xFFFF - stateAddress);
        //        //            //结束JC_B记录
        //        //            KJ237CacheHelper.EndJC_BInfo(def.PointID, datastate, timeNow);
        //        //        }
        //        //        else
        //        //        {
        //        //            LogHelper.Info("需要结束UpdateStationStateItem报警" + EnumHelper.GetEnumDescription(datastate) + "，但未找到报警开始记录");
        //        //        }
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    LogHelper.Info("未找到识别器pointid = " + def.PointID);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("KJ237 UpdateStationStateItem Error:" + ex);
        //    }
        //}
        /// <summary>
        /// 修改人员临时状态
        /// </summary>
        /// <param name="pointid"></param>
        /// <param name="yid"></param>
        /// <param name="type">0 add,1 delete</param>
        private void UpdatePersonStateItem(R_PrealInfo preal, PersonAlarmState state, int type, string pointID, string areaId, DateTime timeNow)
        {
            try
            {
                int index = personStateItems.FindIndex(a => a.yid == preal.Yid);
                int stateAddress = (1 << (int)state);
                if (index >= 0)
                {
                    if (type == 0)
                    {
                        if ((preal.Bjtype & stateAddress) != stateAddress)
                        {
                            //之前没有此种报警，现在产生了
                            personStateItems[index].datastate |= stateAddress;
                            //开始R_PB记录
                            KJ237CacheHelper.CreateR_PBInfo(pointID, areaId, timeNow, state, preal);
                        }
                    }
                    else if (type == 1)
                    {
                        if ((preal.Bjtype & stateAddress) == stateAddress)
                        {
                            //之前有此种报警，现在没有了
                            personStateItems[index].datastate &= (0xFFFF - stateAddress);
                            //结束R_PB记录
                            KJ237CacheHelper.EndR_PBInfo(state, timeNow, preal);
                        }
                    }
                }
                else
                {
                    LogHelper.Info("未找到人员yid = " + preal.Yid);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("KJ237 UpdatePersonStateItem Error:" + ex);
            }
        }
        #endregion
    }

    public class StationStateItem
    {
        /// <summary>
        /// 测点ID
        /// </summary>
        public string pointId;
        /// <summary>
        /// 测点状态
        /// </summary>
        public int datastate;
        public StationStateItem(string _pointId, int _datastate)
        {
            pointId = _pointId;
            datastate = _datastate;
        }
    }

    public class PersonStateItem
    {
        /// <summary>
        /// 人员YID
        /// </summary>
        public string yid;
        /// <summary>
        /// 人员状态
        /// </summary>
        public int datastate;

        public PersonStateItem(string _yid, int _datastate)
        {
            yid = _yid;
            datastate = _datastate;
        }
    }
}
