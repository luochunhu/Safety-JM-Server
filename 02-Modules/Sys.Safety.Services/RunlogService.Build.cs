using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Runlog;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class RunlogService : IRunlogService
    {
        private IRunlogRepository _Repository;

        public RunlogService(IRunlogRepository _Repository)
        {
            this._Repository = _Repository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RunlogService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }

        /// <summary>
        /// 获取系统所有运行日志
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<RunlogInfo>> GetRunlogs()
        {
            BasicResponse<List<RunlogInfo>> Result = new BasicResponse<List<RunlogInfo>>();
            try
            {
                var RunlogModelLists = _Repository.GetRunlogList();
                var RunlogInfoLists = new List<RunlogInfo>();
                foreach (var item in RunlogModelLists)
                {
                    var RunlogInfo = ObjectConverter.Copy<RunlogModel, RunlogInfo>(item);
                    RunlogInfoLists.Add(RunlogInfo);
                }
                Result.Data = RunlogInfoLists;
            }
            catch (Exception ex)
            {
                ThrowException("GetRunlogs", ex);
            }
            return Result;
        }
        /// <summary>
        /// 根据查询条件获取运行日志记录
        /// </summary>
        /// <param name="Runlogrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<RunlogInfo>> GetRunlogs(RunlogGetByConditionsRequest runlogrequest)
        {
            BasicResponse<List<RunlogInfo>> Result = new BasicResponse<List<RunlogInfo>>();
            List<RunlogInfo> operateList = new List<RunlogInfo>();

            try
            {
                if (runlogrequest.dtStart != new DateTime(1, 1, 1, 0, 0, 0) && runlogrequest.dtEnd != new DateTime(1, 1, 1, 0, 0, 0)
                    && runlogrequest.dtStart != null && runlogrequest.dtEnd != null)
                {
                    if (runlogrequest.dtStart > runlogrequest.dtEnd)
                    {
                        //throw new BusinessException(String.Format("开始日期不能大于结束日期！"));
                        ThrowException("GetRunlogs", new Exception("开始日期不能大于结束日期！"));
                    }
                }

                var RunlogModelLists = _Repository.GetRunlogList();
                var RunlogInfoLists = new List<RunlogInfo>();
                foreach (var item in RunlogModelLists)
                {
                    var RunlogInfo = ObjectConverter.Copy<RunlogModel, RunlogInfo>(item);
                    RunlogInfoLists.Add(RunlogInfo);
                }

                //根据条件筛选
                if (runlogrequest.dtStart != new DateTime(1, 1, 1, 0, 0, 0) && runlogrequest.dtStart != null)
                {
                    RunlogInfoLists = RunlogInfoLists.FindAll(a => a.CreateDate >= runlogrequest.dtStart).ToList();
                }
                if (runlogrequest.dtEnd != new DateTime(1, 1, 1, 0, 0, 0) && runlogrequest.dtEnd != null)
                {
                    RunlogInfoLists = RunlogInfoLists.FindAll(a => a.CreateDate <= runlogrequest.dtEnd).ToList();
                }
                if (!string.IsNullOrEmpty(runlogrequest.loglevel.ToString()))
                {
                    RunlogInfoLists = RunlogInfoLists.FindAll(a => a.LogLevel == runlogrequest.loglevel.ToString()).ToList();
                }
                if (!string.IsNullOrEmpty(runlogrequest.context))
                {
                    RunlogInfoLists = RunlogInfoLists.FindAll(a => a.MessageContent.Contains(runlogrequest.context)).ToList();
                }
                if (runlogrequest.pageNumber > 0 && runlogrequest.pageSize > 0)
                {
                    int pageIndex = runlogrequest.pageNumber - 1;
                    int pageSize = runlogrequest.pageSize;
                    RunlogInfoLists = RunlogInfoLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                ThrowException("GetRunlogs", ex);
            }
            return Result;
        }
        public BasicResponse<RunlogInfo> AddRunlog(RunlogAddRequest runlogrequest)
        {
            var _runlog = ObjectConverter.Copy<RunlogInfo, RunlogModel>(runlogrequest.RunlogInfo);
            var resultrunlog = _Repository.AddRunlog(_runlog);
            var runlogresponse = new BasicResponse<RunlogInfo>();
            runlogresponse.Data = ObjectConverter.Copy<RunlogModel, RunlogInfo>(resultrunlog);
            return runlogresponse;
        }
        public BasicResponse<RunlogInfo> UpdateRunlog(RunlogUpdateRequest runlogrequest)
        {
            var _runlog = ObjectConverter.Copy<RunlogInfo, RunlogModel>(runlogrequest.RunlogInfo);
            _Repository.UpdateRunlog(_runlog);
            var runlogresponse = new BasicResponse<RunlogInfo>();
            runlogresponse.Data = ObjectConverter.Copy<RunlogModel, RunlogInfo>(_runlog);
            return runlogresponse;
        }
        public BasicResponse DeleteRunlog(RunlogDeleteRequest runlogrequest)
        {
            _Repository.DeleteRunlog(runlogrequest.Id);
            var runlogresponse = new BasicResponse();
            return runlogresponse;
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRunlogByEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            _Repository.DeleteRunlogByEtime(runlogrequest.Etime);
            var Runlogresponse = new BasicResponse();
            return Runlogresponse;
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="Runlogrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRunlogByStimeEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            _Repository.DeleteRunlogByStimeEtime(runlogrequest.Etime, runlogrequest.Stime);
            var Runlogresponse = new BasicResponse();
            return Runlogresponse;
        }
        public BasicResponse<List<RunlogInfo>> GetRunlogList(RunlogGetListRequest runlogrequest)
        {
            var runlogresponse = new BasicResponse<List<RunlogInfo>>();
            runlogrequest.PagerInfo.PageIndex = runlogrequest.PagerInfo.PageIndex - 1;
            if (runlogrequest.PagerInfo.PageIndex < 0)
            {
                runlogrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var runlogModelLists = _Repository.GetRunlogList(runlogrequest.PagerInfo.PageIndex, runlogrequest.PagerInfo.PageSize, out rowcount);
            var runlogInfoLists = new List<RunlogInfo>();
            foreach (var item in runlogModelLists)
            {
                var RunlogInfo = ObjectConverter.Copy<RunlogModel, RunlogInfo>(item);
                runlogInfoLists.Add(RunlogInfo);
            }
            runlogresponse.Data = runlogInfoLists;
            return runlogresponse;
        }
        public BasicResponse<RunlogInfo> GetRunlogById(RunlogGetRequest runlogrequest)
        {
            var result = _Repository.GetRunlogById(runlogrequest.Id);
            var runlogInfo = ObjectConverter.Copy<RunlogModel, RunlogInfo>(result);
            var runlogresponse = new BasicResponse<RunlogInfo>();
            runlogresponse.Data = runlogInfo;
            return runlogresponse;
        }
    }
}


