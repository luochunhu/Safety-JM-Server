using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Processing.Statistics
{
    public static class Calibration
    {
        private static DateTime _lastRunTime = new DateTime();

        private static Thread _handleThread;

        private static readonly RepositoryBase<UserModel> UserRepositoryBase =
            ServiceFactory.Create<IUserRepository>() as RepositoryBase<UserModel>;

        private static readonly IPointDefineCacheService PointDefineCacheService =
            ServiceFactory.Create<IPointDefineCacheService>();

        private static bool _isRun;


        public static void Start()
        {
            // 20170704
            LogHelper.Info("【Calibration】标校数据统计线程开启。");

            _isRun = true;
            if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
            {
                _handleThread = new Thread(HandleThreadFun);
                _handleThread.Start();
            }
        }

        /// <summary>
        ///     停止模块  20170413
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【Calibration】标校数据统计线程结束。");
            _isRun = false;
            while (true)
            {
                if (_isRun) break;
                Thread.Sleep(1000);
            }
        }

        private static void HandleThreadFun()
        {
            while (_isRun)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    if ((dtNow - _lastRunTime).TotalSeconds >= 30)
                    {
                        StaBx(dtNow);
                        if ((dtNow.Hour == 0) && (dtNow.Minute < 5))
                        {
                            StaBx(dtNow.AddDays(-1));
                            HandleLastBx(dtNow.AddDays(-1));
                        }
                        _lastRunTime = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

                Thread.Sleep(1000);
            }
            _isRun = true;
            LogHelper.Info("【Calibration】标校数据统计线程结束成功。");
        }

        /// <summary>
        ///     统计标校记录
        /// </summary>
        /// <param name="time">统计时间</param>
        private static void StaBx(DateTime time)
        {
            #region 旧统计逻辑
            //var dtSta = new DateTime(time.Ticks);
            //var sStartDate = dtSta.ToString("yyyy-MM-dd 00:00:00.000");
            //var sStopDate = dtSta.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000");
            //var sMcTable = "KJ_DataDetail" + dtSta.ToString("yyyyMMdd");

            //// 20170915
            ////获取有标校记录的测点
            ////var res = PointDefineCacheService.GetAllPointDefineCache(new PointDefineCacheGetAllRequest());
            ////List<Jc_DefInfo> point = res.Data.Where(a => a.Activity == "1").ToList();
            ////var dt = ObjectConverter.ToDataTable<Jc_DefInfo>(point);
            //var dt = UserRepositoryBase.QueryTable("global_Calibration_GetAllByCondition", "distinct point", sMcTable + " where state=24");

            //foreach (DataRow dr in dt.Rows)
            //{
            //    //获取最后一条标校详细记录
            //    var dt2 = UserRepositoryBase.QueryTable("global_Calibration_GetEtimeFromJcbxexByPointStimeEtime",
            //        dr["point"], sStartDate, sStopDate);

            //    DateTime dtWhi; //循环时间
            //    if (dt2.Rows.Count == 0)
            //        dtWhi = Convert.ToDateTime(dtSta.ToString("yyyy-MM-dd 23:59:59.999")).AddDays(-1);
            //    else
            //        dtWhi = Convert.ToDateTime(dt2.Rows[0]["etime"]);

            //    while (true)
            //    {
            //        //获取dtWhi之后的第一条标校密采记录且该记录的时间没有非标校密采记录的时间
            //        var dt3 = UserRepositoryBase.QueryTable("global_Calibration_GetTimerFromJcmcByLoopTime", sMcTable,
            //            dr["point"], dtWhi.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            //        if (dt3.Rows.Count != 0) //存在标校记录
            //        {
            //            //获取(dtWhi之后的第一条标校密采记录且该记录的时间没有非标校密采记录)之后且在((dtWhi之后的第一条标校密采记录且该记录的时间没有非标校密采记录)之后的第一条非标校记录)之前的最后一条标校记录
            //            var dt4 = UserRepositoryBase.QueryTable("global_Calibration_GetTimerFromJcmcByPointTimer",
            //                sMcTable, dr["point"], Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), sStopDate);

            //            //判断第一条和最后一条标校记录的时间差
            //            var dtFirst = Convert.ToDateTime(dt3.Rows[0]["timer"]);
            //            var dtLast = Convert.ToDateTime(dt4.Rows[0]["timer"]);
            //            var dTotalSec = (dtLast - dtFirst).TotalSeconds;
            //            if (dTotalSec >= 30)        //标校持续时间大于等于30秒
            //            {
            //                //获取最大值和最小值以及它们的时间
            //                var dt6 = UserRepositoryBase.QueryTable(
            //                    "global_Calibration_GetMaxValueFromJcmcByPointTimer", sMcTable, dr["point"].ToString(),
            //                    Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), Convert.ToDateTime(dt4.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss fff"));

            //                var dt7 = UserRepositoryBase.QueryTable(
            //                    "global_Calibration_GetMinValueFromJcmcByPointTimer", sMcTable, dr["point"].ToString(),
            //                    Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), Convert.ToDateTime(dt4.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss fff"));

            //                //获取平均值
            //                var dt9 = UserRepositoryBase.QueryTable(
            //                    "global_Calibration_GetAvgValueFromJcmcByPointTimer", sMcTable, dr["point"].ToString(),
            //                    Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), Convert.ToDateTime(dt4.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss fff"));

            //                //判断标校详细记录是否已存在此次记录
            //                var dt8 = UserRepositoryBase.QueryTable(
            //                    "global_Calibration_GetBxexCountFromJcbxexByPointStime", dr["point"].ToString(),
            //                    Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"));

            //                if (dt8.Rows[0][0].ToString() == "0")
            //                {
            //                    var lId = IdHelper.CreateLongId();
            //                    UserRepositoryBase.ExecuteNonQuery("global_Calibration_InsertIntoJcbxex",
            //                        lId.ToString(), dr["point"].ToString(), Convert.ToDateTime(dt3.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"),
            //                        Convert.ToDateTime(dt4.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), ((long)dTotalSec).ToString(),
            //                        dt6.Rows[0]["ssz"].ToString(), dt7.Rows[0]["ssz"].ToString(),
            //                        dt9.Rows[0]["avgSsz"].ToString(), dt6.Rows[0]["timer"].ToString(),
            //                        dt7.Rows[0]["timer"].ToString(), dt4.Rows[0]["hasFbx"].ToString());
            //                }
            //                else
            //                {
            //                    UserRepositoryBase.ExecuteNonQuery("global_Calibration_UpdateJcbxex",
            //                        Convert.ToDateTime(dt4.Rows[0]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff"), ((long)dTotalSec).ToString(),
            //                        dt6.Rows[0]["ssz"].ToString(), dt7.Rows[0]["ssz"].ToString(),
            //                        dt9.Rows[0]["avgSsz"].ToString(), dt6.Rows[0]["timer"].ToString(),
            //                        dt7.Rows[0]["timer"].ToString(), dt4.Rows[0]["hasFbx"].ToString(),
            //                        dr["point"].ToString(), dtFirst.ToString());
            //                }

            //                if (dt4.Rows[0]["hasFbx"].ToString() == "1")
            //                    break;
            //                dtWhi = dtLast;
            //            }
            //            else        //标校持续时间小于30秒
            //            {
            //                if (dt4.Rows[0]["hasFbx"].ToString() == "1")
            //                    break;
            //                dtWhi = dtLast;
            //            }
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            #endregion

            var dtSta = new DateTime(time.Ticks);
            var sStartDate = dtSta.ToString("yyyy-MM-dd 00:00:00.000");
            var sStopDate = dtSta.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000");
            var sMcTable = "KJ_DataDetail" + dtSta.ToString("yyyyMMdd");

            var dtBxex = UserRepositoryBase.QueryTable("global_Calibration_GetBxexFromJcbxexByStartStopStime", sStartDate, sStopDate);
            var dt = UserRepositoryBase.QueryTable("global_Calibration_GetBxexFromJcmcByTableName", sMcTable);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var pointid = dt.Rows[i]["pointid"];
                var stime = Convert.ToDateTime(dt.Rows[i]["starttime"]);
                var dr = dtBxex.Select("pointid=" + pointid + " and stime='" + stime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                if (dr.Length == 0)     //不存在记录
                {
                    var lId = IdHelper.CreateLongId();
                    UserRepositoryBase.ExecuteNonQuery("global_Calibration_InsertIntoJcbxex", lId.ToString(),
                        dt.Rows[i]["point"].ToString(), dt.Rows[i]["pointid"].ToString(), dt.Rows[i]["starttime"].ToString(),
                        dt.Rows[i]["stoptime"].ToString(), dt.Rows[i]["cx"].ToString(), dt.Rows[i]["maxssz"].ToString(),
                        dt.Rows[i]["minssz"].ToString(), dt.Rows[i]["avgssz"].ToString(), dt.Rows[i]["maxtime"].ToString(),
                        dt.Rows[i]["mintime"].ToString(), dt.Rows[i]["bxzt"].ToString());
                }
                else        //存在记录
                {
                    if (dr[0]["bxzt"].ToString() == "1")
                    {
                        UserRepositoryBase.ExecuteNonQuery("global_Calibration_UpdateJcbxex", dt.Rows[i]["stoptime"].ToString(), dt.Rows[i]["cx"].ToString(), dt.Rows[i]["maxssz"].ToString(), dt.Rows[i]["minssz"].ToString(), dt.Rows[i]["avgssz"].ToString(), dt.Rows[i]["maxtime"].ToString(), dt.Rows[i]["mintime"].ToString(), dt.Rows[i]["bxzt"].ToString(), dt.Rows[i]["pointid"].ToString(), dt.Rows[i]["starttime"].ToString());
                    }
                }
            }
        }

        /// <summary>
        ///     处理最后一次标校
        /// </summary>
        /// <param name="time">处理时间</param>
        private static void HandleLastBx(DateTime time)
        {
            var dtSta = new DateTime(time.Ticks);
            var dt = UserRepositoryBase.QueryTable("global_Calibration_GetBxexFromJcbxexByState", dtSta.ToString());

            if (dt.Rows.Count != 0)
            {
                var sql2 = "update KJ_EffectEx set bxzt='2' where bxzt='1'";
                UserRepositoryBase.ExecuteNonQueryBySql(sql2);
            }
        }
    }
}