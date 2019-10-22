using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;
using Basic.Framework.Logging;
using Sys.Safety.Request;

namespace Sys.Safety.Services
{
    public partial class RequestService : IRequestService
    {
        private IRequestRepository _Repository;

        public RequestService(IRequestRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RequestService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        public BasicResponse<RequestInfo> AddRequest(RequestAddRequest requestrequest)
        {
            var _request = ObjectConverter.Copy<RequestInfo, RequestModel>(requestrequest.RequestInfo);
            var resultrequest = _Repository.AddRequest(_request);
            var requestresponse = new BasicResponse<RequestInfo>();
            requestresponse.Data = ObjectConverter.Copy<RequestModel, RequestInfo>(resultrequest);
            return requestresponse;
        }
        public BasicResponse<RequestInfo> UpdateRequest(RequestUpdateRequest requestrequest)
        {
            var _request = ObjectConverter.Copy<RequestInfo, RequestModel>(requestrequest.RequestInfo);
            _Repository.UpdateRequest(_request);
            var requestresponse = new BasicResponse<RequestInfo>();
            requestresponse.Data = ObjectConverter.Copy<RequestModel, RequestInfo>(_request);
            return requestresponse;
        }
        public BasicResponse DeleteRequest(RequestDeleteRequest requestrequest)
        {
            _Repository.DeleteRequest(requestrequest.Id);
            var requestresponse = new BasicResponse();
            return requestresponse;
        }
        public BasicResponse<List<RequestInfo>> GetRequestList(RequestGetListRequest requestrequest)
        {
            var requestresponse = new BasicResponse<List<RequestInfo>>();
            requestrequest.PagerInfo.PageIndex = requestrequest.PagerInfo.PageIndex - 1;
            if (requestrequest.PagerInfo.PageIndex < 0)
            {
                requestrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var requestModelLists = _Repository.GetRequestList(requestrequest.PagerInfo.PageIndex, requestrequest.PagerInfo.PageSize, out rowcount);
            var requestInfoLists = new List<RequestInfo>();
            foreach (var item in requestModelLists)
            {
                var RequestInfo = ObjectConverter.Copy<RequestModel, RequestInfo>(item);
                requestInfoLists.Add(RequestInfo);
            }
            requestresponse.Data = requestInfoLists;
            return requestresponse;
        }
        /// <summary>
        /// 查询所有请求库信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<RequestInfo>> GetRequestList()
        {
            var requestresponse = new BasicResponse<List<RequestInfo>>();
            var requestModelLists = _Repository.GetRequestList();
            var requestInfoLists = new List<RequestInfo>();
            foreach (var item in requestModelLists)
            {
                var RequestInfo = ObjectConverter.Copy<RequestModel, RequestInfo>(item);
                requestInfoLists.Add(RequestInfo);
            }
            requestresponse.Data = requestInfoLists;
            return requestresponse;
        }
        public BasicResponse<RequestInfo> GetRequestById(RequestGetRequest requestrequest)
        {
            var result = _Repository.GetRequestById(requestrequest.Id);
            var requestInfo = ObjectConverter.Copy<RequestModel, RequestInfo>(result);
            var requestresponse = new BasicResponse<RequestInfo>();
            requestresponse.Data = requestInfo;
            return requestresponse;
        }
        /// <summary>
        /// 根据Code查询
        /// </summary>
        /// <param name="requestrequest"></param>
        /// <returns></returns>
        public BasicResponse<RequestInfo> GetRequestByCode(RequestGetByCodeRequest requestrequest)
        {
            var result = _Repository.GetRequestByCode(requestrequest.Code);
            var requestInfo = ObjectConverter.Copy<RequestModel, RequestInfo>(result);
            var requestresponse = new BasicResponse<RequestInfo>();
            requestresponse.Data = requestInfo;
            return requestresponse;
        }
        /// <summary>
        /// 根据请求库Code查询请求库对应的菜单信息
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetRequestMenuByCode(RequestGetMenuByCodeRequest operatelogrequest)
        {
            DataTable RequestMenu = _Repository.GetRequestMenuByCode(operatelogrequest.Code);
            var operatelogresponse = new BasicResponse<DataTable>();
            operatelogresponse.Data = RequestMenu;
            return operatelogresponse;
        }
        /// <summary>
        /// 添加一个全新信息到请求库表并返回成功后的请求库对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        public BasicResponse<RequestInfo> AddRequestEx(RequestAddRequest requestrequest)
        {
            BasicResponse<RequestInfo> Result = new BasicResponse<RequestInfo>();
            RequestInfo requestDTO = requestrequest.RequestInfo;
            try
            {
                long ID = 0;
                if (requestDTO == null)
                {
                    ThrowException("AddRequestEx", new Exception("请求库对象为空，请检查是否已赋值！"));
                }
                if (requestDTO.InfoState == InfoState.NoChange)
                {
                    ThrowException("AddRequestEx", new Exception("DTO对象未设置状态，请先设置！"));
                }
                if (string.IsNullOrEmpty(requestDTO.RequestCode))
                {
                    ThrowException("AddRequestEx", new Exception("请求库编码不能为空，请重新输入！"));
                }
                if (string.IsNullOrEmpty(requestDTO.RequestName))
                {
                    ThrowException("AddRequestEx", new Exception("请求库名称不能为空，请重新输入！"));
                }
                if (requestDTO.InfoState == InfoState.AddNew)
                {
                    //判断请求库名是否存在，只有不存在才能插入
                    if (!CheckRequestNameExist(requestDTO.RequestName) && !CheckRequestCodeExist(requestDTO.RequestCode))
                    {
                        ID = IdHelper.CreateLongId();
                        requestDTO.RequestID = ID.ToString();
                        var _request = ObjectConverter.Copy<RequestInfo, RequestModel>(requestDTO);
                        var resultmenu = _Repository.AddRequest(_request);
                        Result.Data = ObjectConverter.Copy<RequestModel, RequestInfo>(resultmenu);
                    }
                    else
                    {                     
                        ThrowException("AddRequestEx", new Exception("请求库对象已存在，请重新输入！"));
                    }
                }
                else
                {
                    var _request = ObjectConverter.Copy<RequestInfo, RequestModel>(requestDTO);
                    _Repository.UpdateRequest(_request);                    
                    var resultmenu = _Repository.GetRequestById(requestDTO.RequestID);
                    Result.Data = ObjectConverter.Copy<RequestModel, RequestInfo>(resultmenu);
                }
            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 根据传入的请求库名判断当前请求库是否已存在
        /// </summary>
        /// <param name="requestName">请求库名</param>
        /// <returns>true:存在  false:不存在</returns>
        private bool CheckRequestNameExist(string requestName)
        {
            bool isExist = false;
            string strSql = "";
            RequestModel requestDTO = null;
            try
            {
                List<RequestModel> RequestList = _Repository.GetRequestList();
                requestDTO = RequestList.Find(a => a.RequestName == requestName);
                if (requestDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(requestDTO.RequestID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }
            return isExist;
        }
        /// <summary>
        /// 判断请求库编码是否存在
        /// </summary>
        /// <param name="requestCode">请求库编码</param>
        /// <returns></returns>
        private bool CheckRequestCodeExist(string requestCode)
        {
            bool isExist = false;
            string strSql = "";
            RequestModel requestDTO = null;
            try
            {
                List<RequestModel> RequestList = _Repository.GetRequestList();
                requestDTO = RequestList.Find(a => a.RequestCode == requestCode);
                if (requestDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(requestDTO.RequestID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }
            return isExist;
        }
    }
}


