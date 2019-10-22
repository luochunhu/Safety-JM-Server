using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Operatelog;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class OperatelogService : IOperatelogService
    {
        private IOperatelogRepository _Repository;

        public OperatelogService(IOperatelogRepository _Repository)
        {
            this._Repository = _Repository;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("OperateLogService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }

        /// <summary>
        /// 获取系统所有操作日志
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs()
        {
            BasicResponse<List<OperatelogInfo>> Result = new BasicResponse<List<OperatelogInfo>>();
            try
            {
                var operatelogModelLists = _Repository.GetOperatelogList();
                var operatelogInfoLists = new List<OperatelogInfo>();
                foreach (var item in operatelogModelLists)
                {
                    var OperatelogInfo = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(item);
                    operatelogInfoLists.Add(OperatelogInfo);
                }
                Result.Data = operatelogInfoLists;
            }
            catch (Exception ex)
            {
                ThrowException("GetOperateLogs", ex);
            }
            return Result;
        }
        /// <summary>
        /// 根据查询条件获取操作日志记录
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs(OperatelogGetByConditionsRequest operatelogrequest)
        {
            BasicResponse<List<OperatelogInfo>> Result = new BasicResponse<List<OperatelogInfo>>();
            List<OperatelogInfo> operateList = new List<OperatelogInfo>();
           
            try
            {
                if (operatelogrequest.dtStart != new DateTime(1, 1, 1, 0, 0, 0) && operatelogrequest.dtEnd != new DateTime(1, 1, 1, 0, 0, 0)
                    && operatelogrequest.dtStart != null && operatelogrequest.dtEnd != null)
                {
                    if (operatelogrequest.dtStart > operatelogrequest.dtEnd)
                    {
                        //throw new BusinessException(String.Format("开始日期不能大于结束日期！"));
                        ThrowException("GetOperateLogs", new Exception("开始日期不能大于结束日期！"));
                    }
                }

                var operatelogModelLists = _Repository.GetOperatelogList();
                var operatelogInfoLists = new List<OperatelogInfo>();
                foreach (var item in operatelogModelLists)
                {
                    var OperatelogInfo = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(item);
                    operatelogInfoLists.Add(OperatelogInfo);
                }

                //根据条件筛选
                if (operatelogrequest.dtStart != new DateTime(1, 1, 1, 0, 0, 0) && operatelogrequest.dtStart != null)
                {
                    operatelogInfoLists = operatelogInfoLists.FindAll(a => a.CreateTime >= operatelogrequest.dtStart).ToList();
                }
                if (operatelogrequest.dtEnd != new DateTime(1, 1, 1, 0, 0, 0) && operatelogrequest.dtEnd != null)
                {
                    operatelogInfoLists = operatelogInfoLists.FindAll(a => a.CreateTime <= operatelogrequest.dtEnd).ToList();
                }
                if (!string.IsNullOrEmpty(operatelogrequest.type))
                {
                    operatelogInfoLists = operatelogInfoLists.FindAll(a => a.Type == int.Parse(operatelogrequest.type)).ToList();
                }
                if (!string.IsNullOrEmpty(operatelogrequest.context))
                {
                    operatelogInfoLists = operatelogInfoLists.FindAll(a => a.OperationContent.Contains(operatelogrequest.context)).ToList();
                }
                if (operatelogrequest.pageNumber > 0 && operatelogrequest.pageSize > 0) {
                    int pageIndex = operatelogrequest.pageNumber - 1;
                    int pageSize = operatelogrequest.pageSize;
                    operatelogInfoLists = operatelogInfoLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                ThrowException("GetOperateLogs", ex);
            }
            return Result;
        }        
        public BasicResponse<OperatelogInfo> AddOperatelog(OperatelogAddRequest operatelogrequest)
        {
            var _operatelog = ObjectConverter.Copy<OperatelogInfo, OperatelogModel>(operatelogrequest.OperatelogInfo);
            var resultoperatelog = _Repository.AddOperatelog(_operatelog);
            var operatelogresponse = new BasicResponse<OperatelogInfo>();
            operatelogresponse.Data = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(resultoperatelog);
            return operatelogresponse;
        }
        /// <summary>
        /// 批量添加接口
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse AddOperatelogs(OperatelogAddListRequest operatelogrequest)
        {
            if (operatelogrequest.OperatelogInfo.Count > 0)
            {
                foreach (OperatelogInfo TempOperatelogInfo in operatelogrequest.OperatelogInfo)
                {
                    var _operatelog = ObjectConverter.Copy<OperatelogInfo, OperatelogModel>(TempOperatelogInfo);
                    var resultoperatelog = _Repository.AddOperatelog(_operatelog);
                }
            }
            var operatelogresponse = new BasicResponse();
            return operatelogresponse;
        }
        public BasicResponse<OperatelogInfo> UpdateOperatelog(OperatelogUpdateRequest operatelogrequest)
        {
            var _operatelog = ObjectConverter.Copy<OperatelogInfo, OperatelogModel>(operatelogrequest.OperatelogInfo);
            _Repository.UpdateOperatelog(_operatelog);
            var operatelogresponse = new BasicResponse<OperatelogInfo>();
            operatelogresponse.Data = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(_operatelog);
            return operatelogresponse;
        }
        public BasicResponse DeleteOperatelog(OperatelogDeleteRequest operatelogrequest)
        {
            _Repository.DeleteOperatelog(operatelogrequest.Id);
            var operatelogresponse = new BasicResponse();
            return operatelogresponse;
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteOperatelogByEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            _Repository.DeleteOperatelogByEtime(operatelogrequest.Etime);
            var operatelogresponse = new BasicResponse();
            return operatelogresponse;
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteOperatelogByStimeEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            _Repository.DeleteOperatelogByStimeEtime( operatelogrequest.Etime, operatelogrequest.Stime);
            var operatelogresponse = new BasicResponse();
            return operatelogresponse;
        }
        public BasicResponse<List<OperatelogInfo>> GetOperatelogList(OperatelogGetListRequest operatelogrequest)
        {
            var operatelogresponse = new BasicResponse<List<OperatelogInfo>>();
            operatelogrequest.PagerInfo.PageIndex = operatelogrequest.PagerInfo.PageIndex - 1;
            if (operatelogrequest.PagerInfo.PageIndex < 0)
            {
                operatelogrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var operatelogModelLists = _Repository.GetOperatelogList(operatelogrequest.PagerInfo.PageIndex, operatelogrequest.PagerInfo.PageSize, out rowcount);
            var operatelogInfoLists = new List<OperatelogInfo>();
            foreach (var item in operatelogModelLists)
            {
                var OperatelogInfo = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(item);
                operatelogInfoLists.Add(OperatelogInfo);
            }
            operatelogresponse.Data = operatelogInfoLists;
            return operatelogresponse;
        }
        public BasicResponse<OperatelogInfo> GetOperatelogById(OperatelogGetRequest operatelogrequest)
        {
            var result = _Repository.GetOperatelogById(operatelogrequest.Id);
            var operatelogInfo = ObjectConverter.Copy<OperatelogModel, OperatelogInfo>(result);
            var operatelogresponse = new BasicResponse<OperatelogInfo>();
            operatelogresponse.Data = operatelogInfo;
            return operatelogresponse;
        }
    }
}


