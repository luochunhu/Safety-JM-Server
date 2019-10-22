using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;

using Sys.Safety.Model;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using Sys.Safety.Request.DataToDb;
using Sys.Safety.ServiceContract.DataToDb;

namespace Sys.Safety.Processing.Statistics
{
    public static class DataStatistics
    {
        private static DateTime _lastRunTime = new DateTime();

        private static readonly RepositoryBase<UserModel> _userRepositoryBase =
            ServiceFactory.Create<IUserRepository>() as RepositoryBase<UserModel>;

        private static readonly IAlarmRecordService alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();

        private static readonly IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();

        public static IInsertToDbService<Jc_McInfo> mcDataInsertToDbService = ServiceFactory.Create<IInsertToDbService<Jc_McInfo>>();

        private static readonly string TabNameDayY = "KJ_Day";

        private static readonly string TabNameFiveT = "KJ_StaFiveMinute";

        private static readonly string TabNameHourT = "KJ_Hour";

        private static readonly string TabNameMC = "KJ_DataDetail";

        private static readonly string TabNameMouth = "KJ_Month";

        private static readonly string TabNameQuarter = "KJ_Season";

        private static readonly string TabNameYear = "KJ_Year";

        /// <summary>
        ///     数据统计线程
        /// </summary>
        public static Thread dataTread;

        public static Thread dataCollectionTread;

        private static DataTable IAQILevel;

        public static Thread lampAlarmThread;

        private static DataTable tablelevel;

        private static bool threadout;

        private static DateTime _lastDataCollectionTime;

        //private static List<AlarmProcessInfo> allStationInterruptProcessList = new List<AlarmProcessInfo>();

        public static void Start()
        {
            LogHelper.Info("【DataStatistics】小时、日、月、年数据统计线程开启。");
            threadout = true;
            if (dataTread == null || (dataTread != null && !dataTread.IsAlive))
            {
                dataTread = new Thread(DataStaticsFunction);
                dataTread.Start();
            }
            if (dataCollectionTread == null || (dataCollectionTread != null && !dataCollectionTread.IsAlive))
            {
                dataCollectionTread = new Thread(dataCollectionFunction);
                dataCollectionTread.Start();
            }

        }

        /// <summary>
        ///     系统退出可调用
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【DataStatistics】小时、日、月、年数据统计线程结束。");
            threadout = false;
            threadout = false;
            while (true)
            {
                if (threadout) break;
                Thread.Sleep(1000);
            }
        }

        private static string TimeToString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static long GetID()
        {
            //return DateTimeUtil.GetDateTimeNowToInt64();
            return IdHelper.CreateLongId();
        }

        private static void ExecuteFiveMinData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            DateTime stime, etime, savetime;
            long id;
            float pjz;
            string level;
            string devid;
            DataRow[] rows;
            selcttablename = TabNameMC;
            savetabelname = TabNameFiveT;
            DataTable dt;

            #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

            stime = ntime.AddMinutes(-5);
            stime = new DateTime(stime.Year, stime.Month, stime.Day, stime.Hour, stime.Minute / 5 * 5, 00);
            etime = stime.AddMinutes(5).AddSeconds(-1);
            savetime = stime;
            selcttablename += stime.ToString("yyyyMMdd");
            savetabelname += stime.ToString("yyyyMMdd");
            //SQL报错，修改了一下语句 
            //            sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
            //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
            //                a.pjz,b.timer zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(ssz ) zdz ,
            //                ROUND(avg(ssz ),2) pjz from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,fzh,kh,dzh,point,devid,wzid)a join (
            //                select PointID,ssz,timer from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,ssz,timer) b on a.PointID=b.PointID 
            //                and a.zdz=b.ssz)c join (select a.PointID,a.zxz,b.timer zxzs from ( SELECT PointID ,min(ssz) zxz  
            //                from {0} where timer >='{1}' and timer <'{2}'GROUP BY PointID)a join (select PointID,ssz,timer 
            //                from {0} where timer >='{1}' and timer <'{2}'GROUP BY PointID,ssz,timer ) b on a.PointID=b.PointID 
            //                and a.zxz=b.ssz)d on c.PointID=d.PointID", selcttablename, TimeToString(stime), TimeToString(etime));
            //            dt = _userRepositoryBase.QueryTableBySql(sql);
            dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetMaxMinAvgFromJcmc", selcttablename,
                TimeToString(stime), TimeToString(etime));

            #endregion

            #region 数据处理

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                #region 删除数据

                try
                {
                    sql = string.Format("delete from {0} where timer='{1}'", savetabelname, TimeToString(savetime));
                    RunSql(sql);
                }
                catch
                {
                }

                #endregion

                for (var i = 0; i < dt.Rows.Count; i++)
                    try
                    {
                        id = GetID();
                        pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                        devid = dt.Rows[i]["devid"].ToString();
                        level = "1";
                        if ((tablelevel != null) && (tablelevel.Rows.Count > 0))
                        {
                            rows = tablelevel.Select("devid=" + devid + " and min<" + pjz + " and max>=" + pjz);
                            if (rows.Length > 0)
                                level = rows[0]["id"].ToString();
                        }
                        sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                        pjz,zdzs,zxz,zxzs,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                            '{11}','{12}','{13}','{14}')",
                            savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                            , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                            , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                            , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                            , TimeToString(savetime)); //删除质量等级id 
                        RunSql(sql);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("5分钟统计出错：" + ex.Message);
                    }
            }

            #endregion
        }

        private static void ExecuteHourData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            long id;
            float pjz;
            string level;
            string devid;
            DataRow[] rows;
            DateTime stime, etime, savetime;
            DataTable dt;
            try
            {
                selcttablename = TabNameFiveT;
                savetabelname = TabNameHourT;

                #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

                stime = ntime;
                stime = new DateTime(stime.Year, stime.Month, stime.Day, stime.Hour, 0, 0);
                savetime = stime;
                etime = stime.AddHours(1).AddSeconds(-1);
                selcttablename += stime.ToString("yyyyMMdd");
                savetabelname += stime.ToString("yyyyMM");
                //                sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
                //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
                //                a.pjz,b.zdzs zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(zdz ) zdz ,
                //                ROUND(avg(pjz ),2) pjz from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID)a
                // join (select PointID,zdz,zdzs from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,zdz  ) b
                // on a.PointID=b.PointID and a.zdz=b.zdz)c join (select a.PointID,a.zxz,b.zxzs zxzs from ( SELECT PointID ,min(zxz) zxz  
                //                from {0} where timer >='{1}' and timer <'{2}'   GROUP BY PointID)a join (select PointID,zxz,zxzs 
                //                from {0} where timer >='{1}' and timer <'{2}'   GROUP BY PointID,zxz ) b on a.PointID=b.PointID 
                //                and a.zxz=b.zxz)d on c.PointID=d.PointID", selcttablename, TimeToString(stime), TimeToString(etime));
                //                dt = _userRepositoryBase.QueryTableBySql(sql);
                dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetMaxMinAvgFromJcm", selcttablename,
                    TimeToString(stime), TimeToString(etime));

                #endregion

                #region 数据处理-旧代码

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    #region 删除数据

                    try
                    {
                        sql = string.Format("delete from {0} where timer='{1}'", savetabelname, TimeToString(savetime));
                        RunSql(sql);
                    }
                    catch
                    {
                    }

                    #endregion

                    for (var i = 0; i < dt.Rows.Count; i++)
                        try
                        {
                            id = GetID();
                            pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                            devid = dt.Rows[i]["devid"].ToString();
                            level = "1";
                            //if (tablelevel != null && tablelevel.Rows.Count > 0)
                            //{
                            //    rows = tablelevel.Select("devid=" + devid + " and min<=" + pjz + " and max>" + pjz);
                            //    if (rows.Length > 0)
                            //    {
                            //        level = rows[0]["id"].ToString();
                            //    }
                            //}

                            sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                                                    pjz,zdzs,zxz,zxzs,envirlevelid,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                                                        '{11}','{12}','{13}','{14}','{15}')",
                                savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                                , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                                , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                                , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                                , level, TimeToString(savetime));
                            RunSql(sql);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("小时统计出错：" + ex.Message);
                        }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("小时统计出错：" + ex.Message);
            }
        }

        private static void ExecuteDayData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            long id;
            float pjz;
            string level;
            string devid;
            DataRow[] rows;
            DateTime stime, etime, savetime;
            DataTable dt;
            try
            {
                selcttablename = TabNameHourT;
                savetabelname = TabNameDayY;

                #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

                stime = ntime;
                stime = new DateTime(stime.Year, stime.Month, stime.Day, 0, 0, 0);
                savetime = stime;
                etime = stime.AddDays(1).AddSeconds(-1);
                selcttablename += stime.ToString("yyyyMM");
                // 20170611
                //savetabelname += stime.ToString("yyyyMM");
                //                sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
                //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
                //                a.pjz,b.zdzs zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(zdz ) zdz ,
                //                ROUND(avg(pjz ),2) pjz from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID)a join (
                //                select PointID,zdz,zdzs from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,zdz ) b on a.PointID=b.PointID 
                //                and a.zdz=b.zdz)c join (select a.PointID,a.zxz,b.zxzs zxzs from ( SELECT PointID ,min(zxz) zxz  
                //                from {0} where timer >='{1}' and timer <'{2}'GROUP BY PointID)a join (select PointID,zxz,zxzs 
                //                from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,zxz  ) b on a.PointID=b.PointID 
                //                and a.zxz=b.zxz)d on c.PointID=d.PointID", selcttablename, TimeToString(stime), TimeToString(etime));
                //                dt = _userRepositoryBase.QueryTableBySql(sql);
                dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetMaxMinAvgFromJchour", selcttablename,
                    TimeToString(stime), TimeToString(etime));

                #endregion

                #region 数据处理

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    #region 删除数据

                    try
                    {
                        sql = string.Format("delete from {0} where timer='{1}'", savetabelname, TimeToString(savetime));
                        RunSql(sql);
                    }
                    catch
                    {
                    }

                    #endregion

                    for (var i = 0; i < dt.Rows.Count; i++)
                        try
                        {
                            id = GetID();
                            pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                            devid = dt.Rows[i]["devid"].ToString();
                            level = "1";
                            sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                        pjz,zdzs,zxz,zxzs,envirlevelid,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                            '{11}','{12}','{13}','{14}','{15}')",
                                savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                                , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                                , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                                , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                                , level, TimeToString(savetime));
                            RunSql(sql);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("天数据统计出错：" + ex.Message);
                        }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("天数据统计出错：" + ex.Message);
            }
        }

        private static void ExecuteMonthData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            long id;
            float pjz;
            string level;
            string devid;
            DataRow[] rows;
            DateTime stime, etime;
            int savetime;
            DataTable dt;
            try
            {
                selcttablename = TabNameDayY;
                savetabelname = TabNameMouth;

                #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

                stime = ntime;
                stime = new DateTime(stime.Year, stime.Month, 1, 0, 0, 0);
                savetime = int.Parse(stime.ToString("yyyyMM"));
                etime = stime.AddMonths(1).AddSeconds(-1);
                // 20170610
                //selcttablename += stime.ToString("yyyyMM");
                //                sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
                //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
                //                a.pjz,b.zdzs zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(zdz ) zdz ,
                //                ROUND(avg(pjz ),2) pjz from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID)a join (
                //                select PointID,zdz,zdzs from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,zdz ) b on a.PointID=b.PointID 
                //                and a.zdz=b.zdz)c join (select a.PointID,a.zxz,b.zxzs zxzs from ( SELECT PointID ,min(zxz) zxz  
                //                from {0} where timer >='{1}' and timer <'{2}'GROUP BY PointID)a join (select PointID,zxz,zxzs 
                //                from {0} where timer >='{1}' and timer <'{2}' GROUP BY PointID,zxz ) b on a.PointID=b.PointID 
                //                and a.zxz=b.zxz)d on c.PointID=d.PointID", selcttablename, TimeToString(stime), TimeToString(etime));
                //                dt = _userRepositoryBase.QueryTableBySql(sql);
                dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetMaxMinAvgFromJcday", selcttablename,
                    TimeToString(stime), TimeToString(etime));

                #endregion

                #region 数据处理

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    #region 删除数据

                    try
                    {
                        sql = string.Format("delete from {0} where timer='{1}'", savetabelname, savetime);
                        RunSql(sql);
                    }
                    catch
                    {
                    }

                    #endregion

                    for (var i = 0; i < dt.Rows.Count; i++)
                        try
                        {
                            id = GetID();
                            pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                            devid = dt.Rows[i]["devid"].ToString();
                            level = "1";
                            sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                        pjz,zdzs,zxz,zxzs,envirlevelid,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                            '{11}','{12}','{13}','{14}','{15}')",
                                savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                                , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                                , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                                , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                                , level, savetime);
                            RunSql(sql);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("月数据统计出错：" + ex.Message);
                        }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("月数据统计出错：" + ex.Message);
            }
        }

        private static void ExecuteQuarterData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            long id;
            float pjz;
            string level;
            string devid;
            int year, season;
            DataRow[] rows;
            int stime, etime;
            int savetime;
            DataTable dt;
            try
            {
                selcttablename = TabNameMouth;
                savetabelname = TabNameQuarter;

                #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

                var count = (ntime.Month - 1) / 3;
                year = ntime.Year;
                season = count + 1;
                savetime = year * 10 + season;
                count = count * 3 + 1;
                stime = ntime.Year * 100 + count;

                etime = ntime.Year * 100 + count + 2;

                //                sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
                //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
                //                a.pjz,b.zdzs zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(zdz ) zdz ,
                //                ROUND(avg(pjz ),2) pjz from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID)a join (
                //                select PointID,zdz,zdzs from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID,zdz ) b on a.PointID=b.PointID 
                //                and a.zdz=b.zdz)c join (select a.PointID,a.zxz,b.zxzs zxzs from ( SELECT PointID ,min(zxz) zxz  
                //                from {0} where timer >='{1}' and timer <='{2}'GROUP BY PointID)a join (select PointID,zxz,zxzs 
                //                from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID,zxz ) b on a.PointID=b.PointID 
                //                and a.zxz=b.zxz)d on c.PointID=d.PointID", selcttablename, stime, etime);
                //                dt = _userRepositoryBase.QueryTableBySql(sql);
                dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetQuarterMaxMinAvgFromJcmonth",
                    selcttablename, stime, etime);

                #endregion

                #region 数据处理

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    #region 删除数据

                    try
                    {
                        sql = string.Format("delete from {0} where timer='{1}'", savetabelname, savetime);
                        RunSql(sql);
                    }
                    catch
                    {
                    }

                    #endregion

                    for (var i = 0; i < dt.Rows.Count; i++)
                        try
                        {
                            id = GetID();
                            pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                            devid = dt.Rows[i]["devid"].ToString();
                            level = "1";
                            sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                        pjz,zdzs,zxz,zxzs,envirlevelid,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                            '{11}','{12}','{13}','{14}','{15}')",
                                savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                                , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                                , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                                , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                                , level, savetime);
                            RunSql(sql);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("季度数据统计出错：" + ex.Message);
                        }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("季度数据统计出错：" + ex.Message);
            }
        }

        private static void ExecuteYearData(DateTime ntime)
        {
            string selcttablename = "", savetabelname = "";
            var sql = "";
            long id;
            float pjz;
            string level;
            string devid;
            DataRow[] rows;
            int stime, etime;
            int savetime;
            DataTable dt;
            try
            {
                selcttablename = TabNameMouth;
                savetabelname = TabNameYear;

                #region 计算要查询的表名、存储的表名、查询的开始时间、结束时间

                stime = ntime.Year * 100 + 1;
                savetime = ntime.Year;
                etime = ntime.Year * 100 + 12;

                //                sql = string.Format(@"select c.PointID,c.fzh,c.kh,c.dzh,c.point,c.devid,c.wzid,c.zdz,
                //                c.pjz,c.zdzs,d.zxz,d.zxzs from (select a.PointID,a.fzh,a.kh,a.dzh,a.point,a.devid,a.wzid,a.zdz ,
                //                a.pjz,b.zdzs zdzs from ( SELECT PointID ,fzh,kh,dzh,point,devid,wzid,max(zdz ) zdz ,
                //                ROUND(avg(pjz ),2) pjz from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID)a join (
                //                select PointID,zdz,zdzs from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID,zdz ) b on a.PointID=b.PointID 
                //                and a.zdz=b.zdz)c join (select a.PointID,a.zxz,b.zxzs zxzs from ( SELECT PointID ,min(zxz) zxz  
                //                from {0} where timer >='{1}' and timer <='{2}'GROUP BY PointID)a join (select PointID,zxz,zxzs 
                //                from {0} where timer >='{1}' and timer <='{2}' GROUP BY PointID,zxz ) b on a.PointID=b.PointID 
                //                and a.zxz=b.zxz)d on c.PointID=d.PointID", selcttablename, stime, etime);
                //                dt = _userRepositoryBase.QueryTableBySql(sql);
                dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetYearMaxMinAvgFromJcmonth",
                    selcttablename, stime, etime);

                #endregion

                #region 数据处理

                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    #region 删除数据

                    try
                    {
                        sql = string.Format("delete from {0} where timer='{1}'", savetabelname, savetime);
                        RunSql(sql);
                    }
                    catch
                    {
                    }

                    #endregion

                    for (var i = 0; i < dt.Rows.Count; i++)
                        try
                        {
                            id = GetID();
                            pjz = float.Parse(dt.Rows[i]["pjz"].ToString());
                            devid = dt.Rows[i]["devid"].ToString();
                            level = "1";
                            sql = string.Format(@"insert into {0}(id,PointID,fzh,kh,dzh,point,devid,wzid,zdz,
                        pjz,zdzs,zxz,zxzs,envirlevelid,timer) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',
                            '{11}','{12}','{13}','{14}','{15}')",
                                savetabelname, id, dt.Rows[i]["PointID"], dt.Rows[i]["fzh"], dt.Rows[i]["kh"]
                                , dt.Rows[i]["dzh"], dt.Rows[i]["point"], dt.Rows[i]["devid"]
                                , dt.Rows[i]["wzid"], dt.Rows[i]["zdz"], dt.Rows[i]["pjz"]
                                , dt.Rows[i]["zdzs"], dt.Rows[i]["zxz"], dt.Rows[i]["zxzs"]
                                , level, savetime);
                            RunSql(sql);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("年数据统计出错：" + ex.Message);
                        }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("年数据统计出错：" + ex.Message);
            }
        }
        /// <summary>
        /// 补传数据处理 
        /// </summary>
        public static void dataCollectionFunction()
        {
            while (threadout)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    if ((dtNow - _lastDataCollectionTime).TotalSeconds >= 10 || (dtNow.Hour == 23 && dtNow.Minute == 59 && dtNow.Second >= 58))
                    {
                        dataCollection();
                        _lastDataCollectionTime = DateTime.Now;
                        //if (dtNow.Hour == 23 && dtNow.Minute == 59 && dtNow.Second >= 58)
                        //{
                        //    allStationInterruptProcessList.Clear();
                        //}
                    }

                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(1000);
            }
        }
        private static void dataCollection()
        {
            AlarmRecordGetByStimeRequest AlarmRecordRequest = new AlarmRecordGetByStimeRequest();
            AlarmRecordRequest.Stime = DateTime.Now.ToString("yyyy-MM-dd");
            AlarmRecordRequest.ETime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<Jc_DefInfo> allPointDefine = pointDefineService.GetAllPointDefineCache().Data;
            List<AlarmProcessInfo> allStationInterrupt = alarmRecordService.GetStaionInterruptRecordListByStime(AlarmRecordRequest).Data;
            foreach (AlarmProcessInfo alarm in allStationInterrupt)
            {
                if (alarm.Point.Length != 7)
                {
                    continue;
                }
                //if (allStationInterruptProcessList.FindAll(a => a.Point == alarm.Point && a.Stime == alarm.Stime).Count < 1)
                //{//未处理过才进行处理，避免重复处理 
                int fzh = int.Parse(alarm.Point.Substring(0, 3));
                DataTable dt = _userRepositoryBase.QueryTable("global_GetStationHistoryRecordByStime", alarm.Stime, alarm.Etime, fzh);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Jc_DefInfo point = allPointDefine.Find(a => a.Point == dt.Rows[i]["Point"].ToString());
                    if (point != null)
                    {
                        //查询密采表是否存在对应记录
                        string tableName = "kj_datadetail" + alarm.Stime.Year + alarm.Stime.Month.ToString("00") + alarm.Stime.Day.ToString("00");
                        DataTable dtMc = _userRepositoryBase.QueryTable("global_GetMcRecordByStime", tableName, dt.Rows[i]["SaveTime"].ToString(), point.Point);
                        if (dtMc.Rows.Count < 1)
                        {
                            Jc_McInfo densityColl = new Jc_McInfo();
                            densityColl.PointID = point.PointID;
                            densityColl.ID = IdHelper.CreateLongId().ToString();
                            densityColl.Point = dt.Rows[i]["Point"].ToString();
                            densityColl.Devid = point.Devid;
                            densityColl.Fzh = point.Fzh;
                            densityColl.Kh = point.Kh;
                            densityColl.Dzh = point.Dzh;
                            densityColl.Timer = DateTime.Parse(dt.Rows[i]["SaveTime"].ToString());
                            short tempShort = 0;
                            short.TryParse(dt.Rows[i]["State"].ToString(), out tempShort);
                            densityColl.Type = tempShort;
                            tempShort = 0;
                            short.TryParse(dt.Rows[i]["State"].ToString(), out tempShort);
                            densityColl.State = tempShort;
                            float tempFloat = 0;
                            float.TryParse(dt.Rows[i]["Voltage"].ToString(), out tempFloat);
                            densityColl.Voltage = tempFloat;
                            double tempDouble = 0;
                            double.TryParse(dt.Rows[i]["RealData"].ToString(), out tempDouble);
                            densityColl.Ssz = tempDouble;
                            densityColl.Upflag = "1";//表示补传数据
                            densityColl.Wzid = point.Wzid;

                            densityColl.InfoState = InfoState.AddNew;
                            //密采记录入库
                            DataToDbAddRequest<Jc_McInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_McInfo>();
                            dataToDbAddRequest.Item = densityColl;
                            mcDataInsertToDbService.AddItem(dataToDbAddRequest);
                        }
                    }
                }
                //}
            }
        }
        //处理线程函数
        public static void DataStaticsFunction()
        {
            DateTime time;
            var temptime = DateTime.Now.AddMonths(-1);
            DateTime nowtime;
            var sysID = "0";
            while (threadout)
            {
                try
                {
                    nowtime = DateTime.Now;
                    if ((nowtime - _lastRunTime).TotalSeconds >= 60)
                    {
                        time = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, nowtime.Hour, nowtime.Minute / 5 * 5, 0);
                        if (temptime != time)
                        {
                            temptime = time;
                            if (nowtime.Minute <= 5)
                            {
                                ExecuteHourData(nowtime.AddHours(-1));
                                UpdateCffzHourKqsc(nowtime.AddHours(-1), true);
                                UpdateCffzHourPjz(nowtime.AddHours(-1), true);
                            }

                            ExecuteHourData(nowtime);
                            UpdateCffzHourKqsc(nowtime);
                            UpdateCffzHourPjz(nowtime);

                            if ((nowtime.Hour == 0) && (nowtime.Minute <= 5))
                            {
                                ExecuteDayData(nowtime.AddDays(-1));
                                UpdateCffzDayKqsc(nowtime.AddDays(-1));
                                UpdateCffzDayPjz(nowtime.AddDays(-1), true);
                            }
                            ExecuteDayData(nowtime);
                            UpdateCffzDayKqsc(nowtime);
                            UpdateCffzDayPjz(nowtime);

                            if ((nowtime.Day == 1) && (nowtime.Hour == 0) && (nowtime.Minute <= 5))
                            {
                                ExecuteMonthData(nowtime.AddMonths(-1));
                                UpdateCffzMonthKqsc(nowtime.AddMonths(-1));
                                UpdateCffzMonthPjz(nowtime.AddMonths(-1), true);
                            }
                            ExecuteMonthData(nowtime);
                            UpdateCffzMonthKqsc(nowtime);
                            UpdateCffzMonthPjz(nowtime);

                            if ((nowtime.Month % 3 == 1) && (nowtime.Day == 1) && (nowtime.Hour == 0) && (nowtime.Minute <= 5))
                                ExecuteQuarterData(nowtime.AddMonths(-1));
                            ExecuteQuarterData(nowtime);

                            if ((nowtime.Month == 1) && (nowtime.Day == 1) && (nowtime.Hour == 0) && (nowtime.Minute <= 5))
                            {
                                ExecuteMonthData(nowtime.AddYears(-1));
                                UpdateCffzYearKqsc(nowtime.AddYears(-1));
                                UpdateCffzYearPjz(nowtime.AddYears(-1), true);
                            }
                            ExecuteYearData(nowtime);
                            UpdateCffzYearKqsc(nowtime);
                            UpdateCffzYearPjz(nowtime);
                        }

                        _lastRunTime = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error("数据统计线程出错：" + e.Message);
                }
                Thread.Sleep(1000);
            }
            threadout = true;

            LogHelper.Info("【DataStatistics】小时、日、月、年数据统计线程结束成功。");
        }

        /// <summary>
        ///     统计抽放分站kh为9-16的小时平均值
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzHourPjz(DateTime time, bool isLast = false)
        {
            try
            {
                var dtTime = new DateTime(time.Ticks);
                var sQueryTable = "KJ_StaFiveMinute" + dtTime.ToString("yyyyMMdd");
                var sStartTime = dtTime.ToString("yyyy-MM-dd HH:00:00");
                var sStopTime = dtTime.AddHours(1).ToString("yyyy-MM-dd HH:00:00");

                #region 查询的sql

                //var sql = "SELECT temp.PointID, temp.fzh, ( SELECT avg(pjz) FROM " + sQueryTable +
                //          @" AS c WHERE c.fzh = temp.fzh AND kh = 9 AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS FY, ( SELECT avg(pjz) FROM " + sQueryTable +
                //          @" AS c WHERE c.fzh = temp.fzh AND kh = 10 AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS WD, ( SELECT avg(pjz) FROM " + sQueryTable +
                //          @" AS c WHERE c.fzh = temp.fzh AND kh = 11 AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS WS, ( SELECT avg(pjz) FROM " + sQueryTable +
                //          @" AS c WHERE c.fzh = temp.fzh AND kh = 12 AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS CO FROM ( SELECT PointID, fzh FROM KJ_DeviceDefInfo AS a INNER JOIN KJ_DeviceType AS b ON a.devid = b.devid AND b.type = 0 AND b.LC2 = 14 WHERE a.kh = 0 AND activity = 1 ) AS temp";

                #endregion

                //var dt = _userRepositoryBase.QueryTableBySql(sql);
                var dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetKhAvgFromJcm", sQueryTable, sStartTime, sStopTime);

                if (dt == null)
                    return;

                //debug:
                StringBuilder sbPointId = new StringBuilder();

                var sbUpdateSql = new StringBuilder(); //更新的sql
                foreach (DataRow row in dt.Rows)
                {
                    var decFy = row["FY"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["FY"]), 2).ToString();
                    var decWd = row["WD"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WD"]), 2).ToString();
                    var decWs = row["WS"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WS"]), 2).ToString();
                    var decCo = row["CO"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["CO"]), 2).ToString();
                    var sbSet = new StringBuilder(); //set sql

                    #region 拼接set sql

                    //计算分钟
                    int totalMin;
                    if (isLast)
                    {
                        totalMin = 60;
                    }
                    else
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:mm:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }

                    var sSetSection =
                        " / CASE yxsc WHEN '0' THEN " + totalMin + " ELSE yxsc / 60 END FROM (SELECT * FROM CF_Hour" +
                        dtTime.ToString("yyyyMM") + ") AS temp WHERE temp.pointid = '" + row["PointID"] +
                        "' AND date_format(temp.timer, '%Y-%m-%d %H') = '" + dtTime.ToString("yyyy-MM-dd HH") +
                        "' LIMIT 1 )";
                    sbSet.Append("bh = ( SELECT bhl " + sSetSection + ", gh = ( SELECT ghl " + sSetSection +
                                 ", bc = ( SELECT bcl " + sSetSection + ", gc = ( SELECT gcl " + sSetSection);

                    if (decFy != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("FY = '" + decFy + "'");
                    }
                    if (decWd != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WD = '" + decWd + "'");
                    }
                    if (decWs != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WS = '" + decWs + "'");
                    }
                    if (decCo != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("CO = '" + decCo + "'");
                    }

                    #endregion

                    sbUpdateSql.Append("update CF_Hour" + dtTime.ToString("yyyyMM") + " set " + sbSet +
                                       " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y-%m-%d %H')='" +
                                       dtTime.ToString("yyyy-MM-dd HH") + "';");

                    //debug:
                    if (sbPointId.Length != 0)
                    {
                        sbPointId.Append(",");
                    }
                    sbPointId.Append(row["PointID"]);
                }

                //debug:
                DataTable debugDt = new DataTable();
                if (sbPointId.Length == 0)
                {
                    LogHelper.Debug("UpdateCffzHourPjz-无测点");
                }
                else
                {
                    string debugSql = "select * from CF_Hour" + dtTime.ToString("yyyyMM") + " where PointID in (" +
                                  sbPointId + ") and date_format(timer,'%Y-%m-%d %H')='" +
                                  dtTime.ToString("yyyy-MM-dd HH") + "';";
                    debugDt = _userRepositoryBase.QueryTableBySql(debugSql);
                }

                if (sbUpdateSql.Length != 0)
                    RunSql(sbUpdateSql.ToString());

                //debug:
                StringBuilder debugLog = new StringBuilder();
                foreach (DataRow item in debugDt.Rows)
                {
                    debugLog.Append("\r\nID=" + item["ID"] + ",pointID=" + item["pointID"] + ",FY=" + item["FY"] + ",WD=" + item["WD"] + ",WS=" + item["WS"] + ",CO=" + item["CO"] + ",BH=" + item["BH"] + ",GH=" + item["GH"] + ",BC=" + item["BC"] + ",GC=" + item["GC"] + ",BHL=" + item["BHL"] + ",BCL=" + item["BCL"] + ",GHL=" + item["GHL"] + ",GCL=" + item["GCL"] + ",timer=" + item["timer"] + ",yxsc=" + item["yxsc"]);
                }
                if (debugDt.Rows.Count != 0)
                    LogHelper.Debug("UpdateCffzHourPjz-" + debugLog);
            }
            catch (Exception e)
            {
                LogHelper.Error("UpdateCffzHourPjz：" + e.Message);
            }
        }

        /// <summary>
        ///     统计抽放分站kh为9-16的日平均值
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzDayPjz(DateTime time, bool isLast = false)
        {
            try
            {
                var dtTime = new DateTime(time.Ticks);
                var sQueryTable = "CF_Hour" + dtTime.ToString("yyyyMM");
                var sStartTime = dtTime.ToString("yyyy-MM-dd 00:00:00");
                var sStopTime = dtTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

                #region 查询的sql

                //var sql = "SELECT temp.PointID, temp.fzh, ( SELECT avg(FY) FROM " + sQueryTable +
                //          @" AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS FY, ( SELECT avg(WD) FROM " + sQueryTable +
                //          @" AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS WD, ( SELECT avg(WS) FROM " + sQueryTable +
                //          @" AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS WS, ( SELECT avg(CO) FROM " + sQueryTable +
                //          @" AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //          sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //          @"')) AS CO FROM ( SELECT PointID, fzh FROM KJ_DeviceDefInfo AS a INNER JOIN KJ_DeviceType AS b ON a.devid = b.devid AND b.type = 0 AND b.LC2 = 14 WHERE a.kh = 0 AND activity = 1 ) AS temp";

                #endregion

                //var dt = DatatableRunSql(sql);
                //var dt = _userRepositoryBase.QueryTableBySql(sql);
                var dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetAvgFromJcllh", sQueryTable, sStartTime, sStopTime);

                if (dt == null)
                    return;

                var sbUpdateSql = new StringBuilder(); //更新的sql
                foreach (DataRow row in dt.Rows)
                {
                    var decFy = row["FY"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["FY"]), 2).ToString();
                    var decWd = row["WD"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WD"]), 2).ToString();
                    var decWs = row["WS"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WS"]), 2).ToString();
                    var decCo = row["CO"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["CO"]), 2).ToString();

                    var sbSet = new StringBuilder(); //set sql

                    #region 拼接set sql

                    //计算分钟
                    int totalMin;
                    if (isLast)
                    {
                        totalMin = 24 * 60;
                    }
                    else
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:mm:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd 00:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }

                    var sSetSection =
                        " / CASE yxsc WHEN '0' THEN " + totalMin +
                        " ELSE yxsc / 60 END FROM (SELECT * FROM CF_Day) AS temp WHERE temp.pointid = '" +
                        row["PointID"] +
                        "' AND date_format(temp.timer, '%Y-%m-%d') = '" + dtTime.ToString("yyyy-MM-dd") +
                        "' LIMIT 1 )";
                    sbSet.Append("bh = ( SELECT bhl " + sSetSection + ", gh = ( SELECT ghl " + sSetSection +
                                 ", bc = ( SELECT bcl " + sSetSection + ", gc = ( SELECT gcl " + sSetSection);

                    if (decFy != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("FY = '" + decFy + "'");
                    }
                    if (decWd != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WD = '" + decWd + "'");
                    }
                    if (decWs != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WS = '" + decWs + "'");
                    }
                    if (decCo != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("CO = '" + decCo + "'");
                    }

                    #endregion

                    sbUpdateSql.Append("update CF_Day set " + sbSet +
                                       " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y-%m-%d')='" +
                                       dtTime.ToString("yyyy-MM-dd") + "';");
                }

                if (sbUpdateSql.Length != 0)
                    RunSql(sbUpdateSql.ToString());
            }
            catch (Exception e)
            {
                LogHelper.Error("UpdateCffzDayPjz：" + e.Message);
            }
        }

        /// <summary>
        ///     统计抽放分站kh为9-16的月平均值
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzMonthPjz(DateTime time, bool isLast = false)
        {
            try
            {
                var dtTime = new DateTime(time.Ticks);
                var sStartTime = dtTime.ToString("yyyy-MM-01 00:00:00");
                var sStopTime = dtTime.AddMonths(1).ToString("yyyy-MM-01 00:00:00");

                #region 查询的sql

                //var sql =
                //    @"SELECT temp.PointID, temp.fzh, ( SELECT avg(FY) FROM CF_Day AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS FY, ( SELECT avg(WD) FROM CF_Day AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS WD, ( SELECT avg(WS) FROM CF_Day AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS WS, ( SELECT avg(CO) FROM CF_Day AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS CO FROM ( SELECT PointID, fzh FROM KJ_DeviceDefInfo AS a INNER JOIN KJ_DeviceType AS b ON a.devid = b.devid AND b.type = 0 AND b.LC2 = 14 WHERE a.kh = 0 AND activity = 1 ) AS temp";

                #endregion

                //var dt = DatatableRunSql(sql);
                //var dt = _userRepositoryBase.QueryTableBySql(sql);
                var dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetAvgFromJclld", sStartTime, sStopTime);

                if (dt == null)
                    return;

                var sbUpdateSql = new StringBuilder(); //更新的sql
                foreach (DataRow row in dt.Rows)
                {
                    var decFy = row["FY"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["FY"]), 2).ToString();
                    var decWd = row["WD"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WD"]), 2).ToString();
                    var decWs = row["WS"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WS"]), 2).ToString();
                    var decCo = row["CO"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["CO"]), 2).ToString();

                    var sbSet = new StringBuilder(); //set sql

                    #region 拼接set sql

                    //计算分钟
                    int totalMin;
                    if (isLast)
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.AddMonths(1).ToString("yyyy-MM-01 00:00:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-01 00:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }
                    else
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:mm:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-01 00:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }

                    var sSetSection =
                        " / CASE yxsc WHEN '0' THEN " + totalMin +
                        " ELSE yxsc / 60 END FROM (SELECT * FROM CF_Month) AS temp WHERE temp.pointid = '" +
                        row["PointID"] +
                        "' AND date_format(temp.timer, '%Y-%m') = '" + dtTime.ToString("yyyy-MM") +
                        "' LIMIT 1 )";
                    sbSet.Append("bh = ( SELECT bhl " + sSetSection + ", gh = ( SELECT ghl " + sSetSection +
                                 ", bc = ( SELECT bcl " + sSetSection + ", gc = ( SELECT gcl " + sSetSection);

                    if (decFy != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("FY = '" + decFy + "'");
                    }
                    if (decWd != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WD = '" + decWd + "'");
                    }
                    if (decWs != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WS = '" + decWs + "'");
                    }
                    if (decCo != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("CO = '" + decCo + "'");
                    }

                    #endregion

                    sbUpdateSql.Append("update CF_Month set " + sbSet +
                                       " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y-%m')='" +
                                       dtTime.ToString("yyyy-MM") + "';");
                }

                if (sbUpdateSql.Length != 0)
                    RunSql(sbUpdateSql.ToString());
            }
            catch (Exception e)
            {
                LogHelper.Error("UpdateCffzMonthPjz：" + e.Message);
            }
        }

        /// <summary>
        ///     统计抽放分站kh为9-16的月平均值
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzYearPjz(DateTime time, bool isLast = false)
        {
            try
            {
                var dtTime = new DateTime(time.Ticks);
                var sStartTime = dtTime.ToString("yyyy-01-01 00:00:00");
                var sStopTime = dtTime.AddYears(1).ToString("yyyy-01-01 00:00:00");

                #region 查询的sql

                //var sql =
                //    @"SELECT temp.PointID, temp.fzh, ( SELECT avg(FY) FROM CF_Month AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS FY, ( SELECT avg(WD) FROM CF_Month AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS WD, ( SELECT avg(WS) FROM CF_Month AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS WS, ( SELECT avg(CO) FROM CF_Month AS c WHERE c.pointID = temp.PointID AND unix_timestamp(timer) >= unix_timestamp('" +
                //    sStartTime + @"') AND unix_timestamp(timer) < unix_timestamp('" + sStopTime +
                //    @"')) AS CO FROM ( SELECT PointID, fzh FROM KJ_DeviceDefInfo AS a INNER JOIN KJ_DeviceType AS b ON a.devid = b.devid AND b.type = 0 AND b.LC2 = 14 WHERE a.kh = 0 AND activity = 1 ) AS temp";

                #endregion

                //var dt = DatatableRunSql(sql);
                //var dt = _userRepositoryBase.QueryTableBySql(sql);
                var dt = _userRepositoryBase.QueryTable("global_DataStatistics_GetAvgFromJcllm", sStartTime, sStopTime);

                if (dt == null)
                    return;

                var sbUpdateSql = new StringBuilder(); //更新的sql
                foreach (DataRow row in dt.Rows)
                {
                    var decFy = row["FY"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["FY"]), 2).ToString();
                    var decWd = row["WD"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WD"]), 2).ToString();
                    var decWs = row["WS"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["WS"]), 2).ToString();
                    var decCo = row["CO"] == DBNull.Value
                        ? null
                        : Math.Round(Convert.ToDecimal(row["CO"]), 2).ToString();

                    var sbSet = new StringBuilder(); //set sql

                    #region 拼接set sql

                    //计算分钟
                    int totalMin;
                    if (isLast)
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.AddYears(1).ToString("yyyy-01-01 00:00:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-01-01 00:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }
                    else
                    {
                        var dtThisTime = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:mm:00"));
                        var dtZeroTime = Convert.ToDateTime(dtTime.ToString("yyyy-01-01 00:00:00"));
                        totalMin = (int)(dtThisTime - dtZeroTime).TotalMinutes;
                    }

                    var sSetSection =
                        " / CASE yxsc WHEN '0' THEN " + totalMin +
                        " ELSE yxsc / 60 END FROM (SELECT * FROM CF_Year) AS temp WHERE temp.pointid = '" +
                        row["PointID"] +
                        "' AND date_format(temp.timer, '%Y') = '" + dtTime.ToString("yyyy") +
                        "' LIMIT 1 )";
                    sbSet.Append("bh = ( SELECT bhl " + sSetSection + ", gh = ( SELECT ghl " + sSetSection +
                                 ", bc = ( SELECT bcl " + sSetSection + ", gc = ( SELECT gcl " + sSetSection);

                    if (decFy != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("FY = '" + decFy + "'");
                    }
                    if (decWd != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WD = '" + decWd + "'");
                    }
                    if (decWs != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("WS = '" + decWs + "'");
                    }
                    if (decCo != null)
                    {
                        if (sbSet.Length != 0)
                            sbSet.Append(",");
                        sbSet.Append("CO = '" + decCo + "'");
                    }

                    #endregion

                    sbUpdateSql.Append("update CF_Year set " + sbSet +
                                       " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y')='" +
                                       dtTime.ToString("yyyy") + "';");
                }

                if (sbUpdateSql.Length != 0)
                    RunSql(sbUpdateSql.ToString());
            }
            catch (Exception e)
            {
                LogHelper.Error("UpdateCffzYearPjz：" + e.Message);
            }
        }

        /// <summary>
        ///     统计抽放分站关联开关量测点小时开启总时长
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzHourKqsc(DateTime time, bool isLast = false)
        {
            try
            {
                var dtTime = new DateTime(time.Ticks);
                var sQueryTable = "KJ_DataAlarm" + dtTime.ToString("yyyyMM");
                var sStartTimeDay = dtTime.ToString("yyyy-MM-dd 00:00:00");
                var sStopTimeDay = dtTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

                //获取所有抽放分站
                //              var sql = @"select PointID,fzh,jckz3
                //from KJ_DeviceDefInfo as a
                //	inner join KJ_DeviceType as b on a.devid=b.devid and b.type=0 and b.LC2=14
                //where a.kh=0 and activity=1";
                //              //var dtCffz = DatatableRunSql(sql);
                //              var dtCffz = _userRepositoryBase.QueryTableBySql(sql);
                var dtCffz = _userRepositoryBase.QueryTable("global_DataStatistics_GetCffzFromjcdef");

                var sbUpdate = new StringBuilder(); //更新的所有sql
                foreach (DataRow dr in dtCffz.Rows)
                {
                    if (dr["jckz3"] == DBNull.Value)
                        continue;


                    var sarrPoint = dr["jckz3"].ToString().Split('|'); //关联的开关量测点
                    var lisAllSJd = new List<KqTimeDto>(); //所有开关量测点开启的时间段并集

                    //debug:
                    //LogHelper.Debug("UpdateCffzHourKqsc-" + "求时间段并集开始.统计时间:" + dtTime);
                    var debugDbSjd = new StringBuilder();       //数据库的测点开启时间段

                    #region 求时间并集

                    for (var i = 0; i < sarrPoint.Length; i++)
                    {
                        //获取抽放分站关联的每个开关量测点的开启记录
                        var sql2 = @"select *
from " + sQueryTable + @" as temp
where temp.type=27 and temp.point='" + sarrPoint[i] + @"' and
	unix_timestamp(stime)>=unix_timestamp('" + sStartTimeDay + @"') and 
	unix_timestamp(stime)<unix_timestamp('" + sStopTimeDay + @"')
order by stime";
                        //var drCffzKqsj = DatatableRunSql(sql2).Rows; //每个开关量测点的开启时间段
                        var drCffzKqsj = _userRepositoryBase.QueryTableBySql(sql2).Rows;

                        for (var j = 0; j < drCffzKqsj.Count; j++)
                        {
                            //debug:
                            debugDbSjd.Append("\r\n测点号:" + sarrPoint[i] + ",开始时间:" + drCffzKqsj[j]["stime"] + ",结束时间:" + drCffzKqsj[j]["etime"] + ".");

                            var startTime = Convert.ToDateTime(drCffzKqsj[j]["stime"]);
                            //var stopTime = Convert.ToDateTime(drCffzKqsj[j]["etime"]).Year <= 1900
                            //    ? dtTime
                            //    : Convert.ToDateTime(drCffzKqsj[j]["etime"]);
                            DateTime stopTime;
                            if (Convert.ToDateTime(drCffzKqsj[j]["etime"]).Year <= 1900)
                            {
                                if (isLast)
                                {
                                    stopTime = Convert.ToDateTime(dtTime.AddHours(1).ToString("yyyy-MM-dd HH:00:00"));
                                }
                                else
                                {
                                    stopTime = dtTime;
                                }
                            }
                            else
                            {
                                stopTime = Convert.ToDateTime(drCffzKqsj[j]["etime"]);
                            }

                            if (i == 0) //第一个开关量测点开启时间集合
                            {
                                var kt = new KqTimeDto
                                {
                                    StartTime = startTime,
                                    StopTime = stopTime
                                };
                                lisAllSJd.Add(kt);
                            }
                            else //非第一个开关量测点开启时间集合
                            {
                                var locStart = GetQj(startTime, lisAllSJd); //起始时间位置
                                if (locStart == null) continue;
                                var locStop = GetQj(stopTime, lisAllSJd); //结束时间位置
                                if (locStop == null) continue;

                                if (!locStart.IsIn) //开启时间段的起始时间位于集合中开启时间段外
                                {
                                    if (locStart.Index == -1) //开启时间段的起始时间位于区域最后
                                    {
                                        var kt = new KqTimeDto
                                        {
                                            StartTime = startTime,
                                            StopTime = stopTime
                                        };
                                        lisAllSJd.Add(kt);
                                    }
                                    else
                                    {
                                        if (!locStop.IsIn) //开启时间段的结束时间位于集合中开启时间段外
                                        {
                                            if (locStop.Index == -1) //开启时间段的结束时间位于区域最后
                                            {
                                                var kt = new KqTimeDto
                                                {
                                                    StartTime = startTime,
                                                    StopTime = stopTime
                                                };
                                                lisAllSJd.RemoveRange(locStart.Index, lisAllSJd.Count - locStart.Index);
                                                lisAllSJd.Insert(locStart.Index, kt);
                                            }
                                            else
                                            {
                                                if (locStart.Index > locStop.Index) //源数据错误
                                                {
                                                }
                                                else if (locStart.Index == locStop.Index) //同一个区间
                                                {
                                                    var kt = new KqTimeDto
                                                    {
                                                        StartTime = startTime,
                                                        StopTime = stopTime
                                                    };
                                                    lisAllSJd.Insert(locStart.Index, kt);
                                                }
                                                else //stop在start后面区间
                                                {
                                                    var kt = new KqTimeDto
                                                    {
                                                        StartTime = startTime,
                                                        StopTime = stopTime
                                                    };
                                                    lisAllSJd.RemoveRange(locStart.Index, locStop.Index - locStart.Index);
                                                    lisAllSJd.Insert(locStart.Index, kt);
                                                }
                                            }
                                        }
                                        else //开启时间段的结束时间位于集合中开启时间段上
                                        {
                                            var kt = new KqTimeDto
                                            {
                                                StartTime = startTime,
                                                StopTime = lisAllSJd[locStop.Index].StopTime
                                            };
                                            lisAllSJd.RemoveRange(locStart.Index, locStop.Index - locStart.Index + 1);
                                            lisAllSJd.Insert(locStart.Index, kt);
                                        }
                                    }
                                }
                                else //开启时间段的起始时间位于集合中开启时间段上
                                {
                                    if (!locStop.IsIn) //开启时间段的结束时间位于集合中开启时间段外
                                    {
                                        if (locStop.Index == -1)
                                        {
                                            var kt = new KqTimeDto
                                            {
                                                StartTime = lisAllSJd[locStart.Index].StartTime,
                                                StopTime = stopTime
                                            };
                                            lisAllSJd.RemoveRange(locStart.Index, lisAllSJd.Count - locStart.Index);
                                            lisAllSJd.Insert(locStart.Index, kt);
                                        }
                                        else
                                        {
                                            if (locStart.Index >= locStop.Index) //源数据错误
                                            {
                                            }
                                            else
                                            {
                                                var kt = new KqTimeDto
                                                {
                                                    StartTime = lisAllSJd[locStart.Index].StartTime,
                                                    StopTime = stopTime
                                                };
                                                lisAllSJd.RemoveRange(locStart.Index, locStop.Index - locStart.Index);
                                                lisAllSJd.Insert(locStart.Index, kt);
                                            }
                                        }
                                    }
                                    else //开启时间段的结束时间位于集合中开启时间段上
                                    {
                                        if (locStart.Index >= locStop.Index) //源数据错误和不需要处理的情况
                                        {
                                        }
                                        else
                                        {
                                            var kt = new KqTimeDto
                                            {
                                                StartTime = lisAllSJd[locStart.Index].StartTime,
                                                StopTime = lisAllSJd[locStop.Index].StopTime
                                            };
                                            lisAllSJd.RemoveRange(locStart.Index, locStop.Index - locStart.Index + 1);
                                            lisAllSJd.Insert(locStart.Index, kt);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    //debug:
                    //LogHelper.Debug("UpdateCffzHourKqsc-数据库时间段" + debugDbSjd);
                    var sbAllSJd = new StringBuilder();
                    foreach (var item in lisAllSJd)
                    {
                        sbAllSJd.Append("\r\n开始时间:" + item.StartTime + ",结束时间:" + item.StopTime + ".");
                    }
                    //LogHelper.Debug("UpdateCffzHourKqsc-所有测点时间段并集" + sbAllSJd);

                    var sStartTimeHour = Convert.ToDateTime(dtTime.ToString("yyyy-MM-dd HH:00:00"));
                    var sStopTimeHour = Convert.ToDateTime(dtTime.AddHours(1).ToString("yyyy-MM-dd HH:00:00"));
                    var locStartHour = GetQj(sStartTimeHour, lisAllSJd);
                    if (locStartHour == null) continue;
                    var locStopHour = GetQj(sStopTimeHour, lisAllSJd);
                    if (locStopHour == null) continue;

                    //JavaScriptSerializer jss = new JavaScriptSerializer();
                    //string sAllSJd = jss.Serialize(lisAllSJd);
                    //List<KqTimeDto> lisValidSJd = jss.Deserialize<List<KqTimeDto>>(sAllSJd);        //开启的有效时间
                    string sAllSJd;
                    List<KqTimeDto> lisValidSJd;
                    using (var ms = new MemoryStream())
                    {
                        new DataContractJsonSerializer(typeof(List<KqTimeDto>)).WriteObject(ms, lisAllSJd);
                        sAllSJd = Encoding.UTF8.GetString(ms.ToArray());
                    }
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(sAllSJd)))
                    {
                        lisValidSJd =
                            (List<KqTimeDto>)new DataContractJsonSerializer(typeof(List<KqTimeDto>)).ReadObject(ms);
                    }

                    #region 截取有效时间

                    if (!locStartHour.IsIn) //计算时间段的开始时间位于集合中开启时间段外
                    {
                        if (locStartHour.Index == -1) //计算时间段的开始时间位于区域最后
                        {
                            lisValidSJd.Clear();
                        }
                        else
                        {
                            if (!locStopHour.IsIn) //计算时间段的结束时间位于集合中开启时间段外
                            {
                                if (locStopHour.Index == -1) //计算时间段的结束时间位于区域最后
                                {
                                    lisValidSJd.RemoveRange(0, locStartHour.Index);
                                }
                                else
                                {
                                    if (locStartHour.Index == locStopHour.Index) //同一个区间
                                    {
                                        lisValidSJd.Clear();
                                    }
                                    else //stop在start后面区间
                                    {
                                        lisValidSJd.RemoveRange(locStopHour.Index, lisValidSJd.Count - locStopHour.Index);
                                        lisValidSJd.RemoveRange(0, locStartHour.Index);
                                    }
                                }
                            }
                            else //计算时间段的结束时间位于集合中开启时间段上
                            {
                                lisValidSJd[locStopHour.Index].StopTime = sStopTimeHour;
                                lisValidSJd.RemoveRange(locStopHour.Index + 1, lisValidSJd.Count - locStopHour.Index - 1);
                                lisValidSJd.RemoveRange(0, locStartHour.Index);
                            }
                        }
                    }
                    else //开启时间段的起始时间位于集合中开启时间段上
                    {
                        if (!locStopHour.IsIn) //开启时间段的结束时间位于集合中开启时间段外
                        {
                            if (locStopHour.Index == -1)
                            {
                                lisValidSJd[locStartHour.Index].StartTime = sStartTimeHour;
                                lisValidSJd.RemoveRange(0, locStartHour.Index);
                            }
                            else
                            {
                                if (locStartHour.Index < locStopHour.Index)
                                {
                                    lisValidSJd[locStartHour.Index].StartTime = sStartTimeHour;
                                    lisValidSJd.RemoveRange(locStopHour.Index, lisValidSJd.Count - locStopHour.Index);
                                    lisValidSJd.RemoveRange(0, locStartHour.Index);
                                }
                            }
                        }
                        else //开启时间段的结束时间位于集合中开启时间段上
                        {
                            if (locStartHour.Index <= locStopHour.Index)
                            {
                                lisValidSJd[locStartHour.Index].StartTime = sStartTimeHour;
                                lisValidSJd[locStopHour.Index].StopTime = sStopTimeHour;
                                lisValidSJd.RemoveRange(locStopHour.Index + 1, lisValidSJd.Count - locStopHour.Index - 1);
                                lisValidSJd.RemoveRange(0, locStartHour.Index);
                            }
                        }
                    }

                    #endregion

                    //debug:
                    var sbValidSJd = new StringBuilder();
                    foreach (var item in lisValidSJd)
                    {
                        sbValidSJd.Append("\r\n开始时间:" + item.StartTime + ",结束时间:" + item.StopTime + ".");
                    }
                    //LogHelper.Debug("UpdateCffzHourKqsc-有效时间段并集" + sbValidSJd);

                    //时间求和
                    long lTotal = 0;
                    foreach (var val in lisValidSJd)
                        lTotal += Convert.ToInt64((val.StopTime - val.StartTime).TotalSeconds);

                    sbUpdate.Append("update CF_Hour" + dtTime.ToString("yyyyMM") + " set yxsc='" + lTotal +
                                    "' where pointID='" + dr["PointID"] + "' and date_format(timer,'%Y-%m-%d %H')='" +
                                    dtTime.ToString("yyyy-MM-dd HH") + "';");

                    //debug:
                    if (lTotal < 0)
                    {
                        LogHelper.Debug("UpdateCffzHourKqsc-异常.统计时间:" + dtTime);
                        LogHelper.Debug("UpdateCffzHourKqsc-数据库时间段" + debugDbSjd);
                        LogHelper.Debug("UpdateCffzHourKqsc-所有测点时间段并集" + sbAllSJd);
                        LogHelper.Debug("UpdateCffzHourKqsc-有效时间段并集" + sbValidSJd);
                    }
                }

                if (sbUpdate.Length != 0)
                    RunSql(sbUpdate.ToString());
            }
            catch (Exception e)
            {
                LogHelper.Error("UpdateCffzHourKqsc：" + e.Message);
            }
        }

        /// <summary>
        ///     统计抽放分站关联开关量测点天开启总时长
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzDayKqsc(DateTime time)
        {
            var dtTime = new DateTime(time.Ticks);
            var sQueryTable = "CF_Hour" + dtTime.ToString("yyyyMM");
            var sStartTime = dtTime.ToString("yyyy-MM-dd 00:00:00");
            var sStopTime = dtTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            #region 查询的sql

            var sql = @"select temp.PointID,temp.fzh,
(
	select sum(yxsc)
	from " + sQueryTable + @" as c
	where c.pointID=temp.PointID and 
		unix_timestamp(timer)>=unix_timestamp('" + sStartTime + @"') and 
		unix_timestamp(timer)<unix_timestamp('" + sStopTime + @"')
) as yxsc
from (
	select PointID,fzh
	from KJ_DeviceDefInfo as a
		inner join KJ_DeviceType as b on a.devid=b.devid and b.type=0 and b.LC2=14
	where a.kh=0 and activity=1
) as temp";

            #endregion

            //var dt = DatatableRunSql(sql);
            var dt = _userRepositoryBase.QueryTableBySql(sql);

            if (dt == null)
                return;

            var sbUpdateSql = new StringBuilder(); //更新的sql
            foreach (DataRow row in dt.Rows)
            {
                var decYxsc = row["yxsc"] == DBNull.Value
                    ? null
                    : Math.Round(Convert.ToDecimal(row["yxsc"]), 2).ToString();

                if (decYxsc == null)
                    continue;

                var sbSet = new StringBuilder(); //set sql

                #region 拼接set sql

                if (sbSet.Length != 0)
                    sbSet.Append(",");
                sbSet.Append("yxsc = '" + decYxsc + "'");

                #endregion

                sbUpdateSql.Append("update CF_Day set " + sbSet +
                                   " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y-%m-%d')='" +
                                   dtTime.ToString("yyyy-MM-dd") + "';");
            }

            if (sbUpdateSql.Length != 0)
                RunSql(sbUpdateSql.ToString());
        }

        /// <summary>
        ///     统计抽放分站关联开关量测点月开启总时长
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzMonthKqsc(DateTime time)
        {
            var dtTime = new DateTime(time.Ticks);
            var sStartTime = dtTime.ToString("yyyy-MM-01 00:00:00");
            var sStopTime = dtTime.AddMonths(1).ToString("yyyy-MM-01 00:00:00");

            #region 查询的sql

            var sql = @"select temp.PointID,temp.fzh,
(
	select sum(yxsc)
	from CF_Day as c
	where c.pointID=temp.PointID and 
		unix_timestamp(timer)>=unix_timestamp('" + sStartTime + @"') and 
		unix_timestamp(timer)<unix_timestamp('" + sStopTime + @"')
) as yxsc
from (
	select PointID,fzh
	from KJ_DeviceDefInfo as a
		inner join KJ_DeviceType as b on a.devid=b.devid and b.type=0 and b.LC2=14
	where a.kh=0 and activity=1
) as temp";

            #endregion

            //var dt = DatatableRunSql(sql);
            var dt = _userRepositoryBase.QueryTableBySql(sql);

            if (dt == null)
                return;

            var sbUpdateSql = new StringBuilder(); //更新的sql
            foreach (DataRow row in dt.Rows)
            {
                var decYxsc = row["yxsc"] == DBNull.Value
                    ? null
                    : Math.Round(Convert.ToDecimal(row["yxsc"]), 2).ToString();

                if (decYxsc == null)
                    continue;

                var sbSet = new StringBuilder(); //set sql

                #region 拼接set sql

                if (sbSet.Length != 0)
                    sbSet.Append(",");
                sbSet.Append("yxsc = '" + decYxsc + "'");

                #endregion

                sbUpdateSql.Append("update CF_Month set " + sbSet +
                                   " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y-%m')='" +
                                   dtTime.ToString("yyyy-MM") + "';");
            }

            if (sbUpdateSql.Length != 0)
                RunSql(sbUpdateSql.ToString());
        }

        /// <summary>
        ///     统计抽放分站关联开关量测点年开启总时长
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void UpdateCffzYearKqsc(DateTime time)
        {
            var dtTime = new DateTime(time.Ticks);
            var sStartTime = dtTime.ToString("yyyy-01-01 00:00:00");
            var sStopTime = dtTime.AddYears(1).ToString("yyyy-01-01 00:00:00");

            #region 查询的sql

            var sql = @"select temp.PointID,temp.fzh,
(
	select sum(yxsc)
	from CF_Month as c
	where c.pointID=temp.PointID and 
		unix_timestamp(timer)>=unix_timestamp('" + sStartTime + @"') and 
		unix_timestamp(timer)<unix_timestamp('" + sStopTime + @"')
) as yxsc
from (
	select PointID,fzh
	from KJ_DeviceDefInfo as a
		inner join KJ_DeviceType as b on a.devid=b.devid and b.type=0 and b.LC2=14
	where a.kh=0 and activity=1
) as temp";

            #endregion

            //var dt = DatatableRunSql(sql);
            var dt = _userRepositoryBase.QueryTableBySql(sql);

            if (dt == null)
                return;

            var sbUpdateSql = new StringBuilder(); //更新的sql
            foreach (DataRow row in dt.Rows)
            {
                var decYxsc = row["yxsc"] == DBNull.Value
                    ? null
                    : Math.Round(Convert.ToDecimal(row["yxsc"]), 2).ToString();

                if (decYxsc == null)
                    continue;

                var sbSet = new StringBuilder(); //set sql

                #region 拼接set sql

                if (sbSet.Length != 0)
                    sbSet.Append(",");
                sbSet.Append("yxsc = '" + decYxsc + "'");

                #endregion

                sbUpdateSql.Append("update CF_Year set " + sbSet +
                                   " where PointID='" + row["PointID"] + "' and date_format(timer,'%Y')='" +
                                   dtTime.ToString("yyyy") + "';");
            }

            if (sbUpdateSql.Length != 0)
                RunSql(sbUpdateSql.ToString());
        }

        /// <summary>
        ///     计算时间点在时间段集合的哪个区间的哪个部分
        /// </summary>
        /// <param name="dtBjs">被计算时间</param>
        /// <param name="kt">时间段集合</param>
        /// <returns>计算结果</returns>
        private static LocationDto GetQj(DateTime dtBjs, List<KqTimeDto> kt)
        {
            if (kt == null)
                return null;

            for (var i = 0; i < kt.Count; i++)
                if (dtBjs <= kt[i].StopTime)
                    return new LocationDto
                    {
                        Index = i,
                        IsIn = dtBjs >= kt[i].StartTime
                    };
                else
                    continue;

            return new LocationDto
            {
                Index = -1,
                IsIn = false
            };
        }

        #region 数据库操作接口

        private static void RunSql(string sql)
        {
            try
            {
                //CreatChannel().ExecuteSQL(sql);

                //调用历史存储模块入库  20170420                
                //HistoricalStorage.OptHistoricalStorage.StoreObj_SQLData.AddItem(sql);
                _userRepositoryBase.ExecuteNonQueryBySql(sql);
            }
            catch (Exception ex)
            {
                LogHelper.Error("统计服务,执行sql语句出错：" + ex.Message + ":" + sql);
            }
        }

        //private DataTable DatatableRunSql(string sql)
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = CreatChannel().GetDataTableBySQL(sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("统计服务,执行sql语句出错：" + ex.Message + ":" + sql);
        //    }
        //    return dt;
        //}

        #endregion

        /// <summary>
        ///     开启时间段
        /// </summary>
        public class KqTimeDto
        {
            public DateTime StartTime { get; set; }

            public DateTime StopTime { get; set; }
        }

        public class LocationDto
        {
            /// <summary>
            ///     索引,-1表示最后
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            ///     部分,true表示在时间段内(右边)
            /// </summary>
            public bool IsIn { get; set; }
        }

        #region 手动统计函数

        public static void DataStaticsSD(DateTime stime, DateTime etime)
        {
            //ExecuteFiveMinData( stime,  etime);
            ExecuteHourData(stime, etime);
            ExecuteDayData(stime, etime);
            ExecuteMonthData(stime, etime);
            ExecuteQuarterData(stime, etime);
            ExecuteYearData(stime, etime);
        }

        private static void ExecuteFiveMinData(DateTime stime, DateTime etime)
        {
            DateTime time;
            DateTime nowtime;
            nowtime = stime.AddMinutes(5);
            for (; ; )
                try
                {
                    time = new DateTime(nowtime.Year, nowtime.Month, nowtime.Day, nowtime.Hour, nowtime.Minute / 5 * 5, 0);
                    ExecuteFiveMinData(time);
                    time = time.AddMinutes(5);
                    if (time > etime)
                        break;
                }
                catch
                {
                }
        }

        private static void ExecuteHourData(DateTime stime, DateTime etime)
        {
            DateTime time;
            time = stime;
            while (time < etime)
                try
                {
                    ExecuteHourData(time);
                    time = time.AddHours(1);
                }
                catch
                {
                }
        }

        private static void ExecuteDayData(DateTime stime, DateTime etime)
        {
            DateTime time;
            time = stime;
            while (time < etime)
                try
                {
                    ExecuteDayData(time);
                    time = time.AddDays(1);
                }
                catch
                {
                }
        }

        private static void ExecuteMonthData(DateTime stime, DateTime etime)
        {
            DateTime time;
            time = stime;
            while (time < etime)
                try
                {
                    ExecuteMonthData(time);
                    time = time.AddMonths(1);
                }
                catch
                {
                }
        }

        private static void ExecuteQuarterData(DateTime stime, DateTime etime)
        {
            DateTime time;
            time = stime;
            while (time < etime)
                try
                {
                    ExecuteQuarterData(time);
                    time = time.AddMonths(3);
                }
                catch
                {
                }
        }

        private static void ExecuteYearData(DateTime stime, DateTime etime)
        {
            DateTime time;
            time = stime;
            while (time < etime)
                try
                {
                    ExecuteYearData(time);
                    time = time.AddYears(1);
                }
                catch
                {
                }
        }

        #endregion
    }
}