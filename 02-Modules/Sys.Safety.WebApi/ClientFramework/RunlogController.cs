using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Runlog;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 运行日志管理WebApi接口
    /// </summary>
    public class RunlogController : Basic.Framework.Web.WebApi.BasicApiController, IRunlogService
    {
        static RunlogController()
        {

        }
        IRunlogService _runlogService = ServiceFactory.Create<IRunlogService>();
        /// <summary>
        /// /// <summary>
        /// 获取系统所有运行日志
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Runlog/GetRunlogs")]
        public BasicResponse<List<RunlogInfo>> GetRunlogs()
        {
            return _runlogService.GetRunlogs();
        }
        /// <summary>
        /// 根据查询条件获取运行日志记录
        /// </summary>
        /// <param name="runlogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Runlog/GetRunlogsByConditions")]
        public BasicResponse<List<RunlogInfo>> GetRunlogs(RunlogGetByConditionsRequest runlogrequest)
        {
            return _runlogService.GetRunlogs(runlogrequest);
        }
        [HttpPost]
        [Route("v1/Runlog/Add")]
        public BasicResponse<RunlogInfo> AddRunlog(RunlogAddRequest runlogrequest)
        {
            return _runlogService.AddRunlog(runlogrequest);
        }
        [HttpPost]
        [Route("v1/Runlog/Update")]
        public BasicResponse<RunlogInfo> UpdateRunlog(RunlogUpdateRequest runlogrequest)
        {
            return _runlogService.UpdateRunlog(runlogrequest);
        }
        [HttpPost]
        [Route("v1/Runlog/Delete")]
        public BasicResponse DeleteRunlog(RunlogDeleteRequest runlogrequest)
        {
            return _runlogService.DeleteRunlog(runlogrequest);
        }
        [HttpPost]
        [Route("v1/Runlog/GetPageList")]
        public BasicResponse<List<RunlogInfo>> GetRunlogList(RunlogGetListRequest runlogrequest)
        {
            return _runlogService.GetRunlogList(runlogrequest);
        }
        [HttpPost]
        [Route("v1/Runlog/Get")]
        public BasicResponse<RunlogInfo> GetRunlogById(RunlogGetRequest runlogrequest)
        {
            return _runlogService.GetRunlogById(runlogrequest);
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Runlog/DeleteRunlogByEtime")]
        public BasicResponse DeleteRunlogByEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            return _runlogService.DeleteRunlogByEtime(runlogrequest);
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Runlog/DeleteRunlogByStimeEtime")]
        public BasicResponse DeleteRunlogByStimeEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            return _runlogService.DeleteRunlogByStimeEtime(runlogrequest);
        }
    }
}
