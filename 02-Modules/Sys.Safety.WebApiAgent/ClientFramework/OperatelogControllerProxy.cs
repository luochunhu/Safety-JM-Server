using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Operatelog;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class OperatelogControllerProxy : BaseProxy, IOperatelogService
    {
        /// <summary>
        /// 获取系统所有操作日志
        /// </summary>
        /// <returns></returns>        
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/GetOperateLogs?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<OperatelogInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据查询条件获取操作日志记录
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>        
        public BasicResponse<List<OperatelogInfo>> GetOperateLogs(OperatelogGetByConditionsRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/GetOperateLogsByConditions?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<OperatelogInfo>>>(responseStr);
        }        
        public BasicResponse<OperatelogInfo> AddOperatelog(OperatelogAddRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/Add?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<OperatelogInfo>>(responseStr);
        }       
        public BasicResponse<OperatelogInfo> UpdateOperatelog(OperatelogUpdateRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/Update?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<OperatelogInfo>>(responseStr);
        }        
        public BasicResponse DeleteOperatelog(OperatelogDeleteRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/Delete?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<List<OperatelogInfo>> GetOperatelogList(OperatelogGetListRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/GetPageList?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<OperatelogInfo>>>(responseStr);
        }      
        public BasicResponse<OperatelogInfo> GetOperatelogById(OperatelogGetRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/Get?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<OperatelogInfo>>(responseStr);
        }
        /// <summary>
        /// 批量添加接口
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>        
        public BasicResponse AddOperatelogs(OperatelogAddListRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/AddOperatelogs?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>       
        public BasicResponse DeleteOperatelogByEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/DeleteOperatelogByEtime?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>      
        public BasicResponse DeleteOperatelogByStimeEtime(OperatelogDeleteByStimeEtimeRequest operatelogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Operatelog/DeleteOperatelogByStimeEtime?token=" + Token, JSONHelper.ToJSONString(operatelogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
