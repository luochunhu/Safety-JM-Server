using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Operatelog;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IOperatelogService
    {
        /// <summary>
        /// 获取系统所有操作日志
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<OperatelogInfo>> GetOperateLogs();
        /// <summary>
        /// 根据查询条件获取操作日志记录
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse<List<OperatelogInfo>> GetOperateLogs(OperatelogGetByConditionsRequest operatelogrequest);
        BasicResponse<OperatelogInfo> AddOperatelog(OperatelogAddRequest operatelogrequest);
        BasicResponse<OperatelogInfo> UpdateOperatelog(OperatelogUpdateRequest operatelogrequest);
        BasicResponse DeleteOperatelog(OperatelogDeleteRequest operatelogrequest);
        BasicResponse<List<OperatelogInfo>> GetOperatelogList(OperatelogGetListRequest operatelogrequest);
        BasicResponse<OperatelogInfo> GetOperatelogById(OperatelogGetRequest operatelogrequest);
        /// <summary>
        /// 批量添加接口
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse AddOperatelogs(OperatelogAddListRequest operatelogrequest);
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteOperatelogByEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest);
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteOperatelogByStimeEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest);
    }
}

