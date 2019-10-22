using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Runlog;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRunlogService
    {
        /// <summary>
        /// 获取系统所有运行日志
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<RunlogInfo>> GetRunlogs();
        /// <summary>
        /// 根据查询条件获取运行日志记录
        /// </summary>
        /// <param name="Runlogrequest"></param>
        /// <returns></returns>
        BasicResponse<List<RunlogInfo>> GetRunlogs(RunlogGetByConditionsRequest runlogrequest);
        BasicResponse<RunlogInfo> AddRunlog(RunlogAddRequest runlogrequest);
        BasicResponse<RunlogInfo> UpdateRunlog(RunlogUpdateRequest runlogrequest);
        BasicResponse DeleteRunlog(RunlogDeleteRequest runlogrequest);
        BasicResponse<List<RunlogInfo>> GetRunlogList(RunlogGetListRequest runlogrequest);
        BasicResponse<RunlogInfo> GetRunlogById(RunlogGetRequest runlogrequest);
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRunlogByEtime(RunlogDeleteByStimeEtimeRequest runlogrequest);
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRunlogByStimeEtime(RunlogDeleteByStimeEtimeRequest runlogrequest);
    }
}

