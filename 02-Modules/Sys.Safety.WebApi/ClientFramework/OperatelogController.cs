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
using Sys.Safety.Request.Operatelog;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 操作日志管理WebApi接口
    /// </summary>
    public class OperatelogController : Basic.Framework.Web.WebApi.BasicApiController, IOperatelogService
    {
        static OperatelogController()
        {

        }
        IOperatelogService _operatelogService = ServiceFactory.Create<IOperatelogService>();
        /// <summary>
        /// 获取系统所有操作日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Operatelog/GetOperateLogs")]
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs()
        {
            return _operatelogService.GetOperateLogs();
        }
        /// <summary>
        /// 根据查询条件获取操作日志记录
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Operatelog/GetOperateLogsByConditions")]
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs(OperatelogGetByConditionsRequest operatelogrequest)
        {
            return _operatelogService.GetOperateLogs(operatelogrequest);
        }
        [HttpPost]
        [Route("v1/Operatelog/Add")]
        public BasicResponse<OperatelogInfo> AddOperatelog(OperatelogAddRequest operatelogrequest)
        {
            return _operatelogService.AddOperatelog(operatelogrequest);
        }
        [HttpPost]
        [Route("v1/Operatelog/Update")]
        public BasicResponse<OperatelogInfo> UpdateOperatelog(OperatelogUpdateRequest operatelogrequest)
        {
            return _operatelogService.UpdateOperatelog(operatelogrequest);
        }
        [HttpPost]
        [Route("v1/Operatelog/Delete")]
        public BasicResponse DeleteOperatelog(OperatelogDeleteRequest operatelogrequest)
        {
            return _operatelogService.DeleteOperatelog(operatelogrequest);
        }
        [HttpPost]
        [Route("v1/Operatelog/GetPageList")]
        public BasicResponse<List<OperatelogInfo>> GetOperatelogList(OperatelogGetListRequest operatelogrequest)
        {
            return _operatelogService.GetOperatelogList(operatelogrequest);
        }
        [HttpPost]
        [Route("v1/Operatelog/Get")]
        public BasicResponse<OperatelogInfo> GetOperatelogById(OperatelogGetRequest operatelogrequest)
        {
            return _operatelogService.GetOperatelogById(operatelogrequest);
        }
        /// <summary>
        /// 批量添加接口
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Operatelog/AddOperatelogs")]
        public BasicResponse AddOperatelogs(OperatelogAddListRequest operatelogrequest)
        {
            return _operatelogService.AddOperatelogs(operatelogrequest);
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Operatelog/DeleteOperatelogByEtime")]
        public BasicResponse DeleteOperatelogByEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            return _operatelogService.DeleteOperatelogByEtime(operatelogrequest);
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Operatelog/DeleteOperatelogByStimeEtime")]
        public BasicResponse DeleteOperatelogByStimeEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            return _operatelogService.DeleteOperatelogByStimeEtime(operatelogrequest);
        }
    }
}
