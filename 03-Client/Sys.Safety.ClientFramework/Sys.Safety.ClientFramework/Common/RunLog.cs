using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Runlog;
using Basic.Framework.Common;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.CBFCommon
{
    /// <summary>
    /// 系统运行日志帮助类
    /// </summary>
    public class RunLogHelper
    {
        static IRunlogService runlogService = ServiceFactory.Create<IRunlogService>();
        /// <summary>
        /// 增加运行日志
        /// </summary>
        /// <param name="dto">运行日志对象</param>
        private static void InsertRunLog(RunlogInfo dto)
        {
            if (dto == null)
            {
                throw new Exception("运行日志对象不能为空!");
            }
            RunlogAddRequest runlogrequest = new RunlogAddRequest();
            runlogrequest.RunlogInfo = dto;
            runlogService.AddRunlog(runlogrequest);
        }
        /// <summary>
        /// 写运行日志
        /// </summary>
        /// <param name="ThreadNumber"></param>
        /// <param name="LogLevel"></param>
        /// <param name="Logger"></param>
        /// <param name="MessageContent"></param>
        public static void InsertRunLog(string ThreadNumber, string LogLevel, string Logger, string MessageContent)
        {
            try
            {
                RunlogInfo logdto = new RunlogInfo();
                logdto.ID = IdHelper.CreateLongId().ToString(); 
                logdto.ThreadNumber = ThreadNumber;
                logdto.LogLevel = LogLevel;
                logdto.Logger = Logger;
                logdto.MessageContent = MessageContent;
                InsertRunLog(logdto);
            }
            catch (Exception ex)
            {
                LogHelper.Error("RunLogHelper-InsertRunLog" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 清除数据库中指定日期的运行日志数据
        /// </summary>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        public static void ClearRunLog(DateTime stime, DateTime etime)
        {
            try
            {               
                if (stime == null)
                {

                    RunlogDeleteByStimeEtimeRequest runlogrequest = new RunlogDeleteByStimeEtimeRequest();
                    runlogrequest.Etime = etime;
                    runlogService.DeleteRunlogByEtime(runlogrequest);
                }
                else
                {
                    RunlogDeleteByStimeEtimeRequest runlogrequest = new RunlogDeleteByStimeEtimeRequest();
                    runlogrequest.Etime = etime;
                    runlogrequest.Stime = stime;
                    runlogService.DeleteRunlogByStimeEtime(runlogrequest);
                }
            }
            catch (Exception ex)
            {               
                LogHelper.Error("RunLogHelper-ClearRunLog" + ex.Message + ex.StackTrace);
            }
        }
    }
}
