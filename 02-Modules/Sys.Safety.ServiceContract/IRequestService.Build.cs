using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.ServiceContract
{
    public interface IRequestService
    {
        BasicResponse<RequestInfo> AddRequest(RequestAddRequest requestrequest);
        BasicResponse<RequestInfo> UpdateRequest(RequestUpdateRequest requestrequest);
        BasicResponse DeleteRequest(RequestDeleteRequest requestrequest);
        BasicResponse<List<RequestInfo>> GetRequestList(RequestGetListRequest requestrequest);
        /// <summary>
        /// 查询所有请求库信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<RequestInfo>> GetRequestList();
        BasicResponse<RequestInfo> GetRequestById(RequestGetRequest requestrequest);
        /// <summary>
        /// 根据Code查询
        /// </summary>
        /// <param name="requestrequest"></param>
        /// <returns></returns>
        BasicResponse<RequestInfo> GetRequestByCode(RequestGetByCodeRequest requestrequest);
        /// <summary>
        /// 根据请求库Code查询请求库对应的菜单信息
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetRequestMenuByCode(RequestGetMenuByCodeRequest operatelogrequest);
        /// <summary>
        /// 添加一个全新信息到请求库表并返回成功后的请求库对象
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        BasicResponse<RequestInfo> AddRequestEx(RequestAddRequest requestrequest);
    }
}

