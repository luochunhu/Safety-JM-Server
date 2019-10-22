using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Runlog;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class RunlogControllerProxy : BaseProxy, IRunlogService
    {
        /// <summary>
        /// /// <summary>
        /// 获取系统所有运行日志
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>        
        public BasicResponse<List<RunlogInfo>> GetRunlogs()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/GetRunlogs?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<RunlogInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据查询条件获取运行日志记录
        /// </summary>
        /// <param name="runlogrequest"></param>
        /// <returns></returns>       
        public BasicResponse<List<RunlogInfo>> GetRunlogs(RunlogGetByConditionsRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/GetRunlogsByConditions?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RunlogInfo>>>(responseStr);
        }        
        public BasicResponse<RunlogInfo> AddRunlog(RunlogAddRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/Add?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RunlogInfo>>(responseStr);
        }       
        public BasicResponse<RunlogInfo> UpdateRunlog(RunlogUpdateRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/Update?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RunlogInfo>>(responseStr);
        }        
        public BasicResponse DeleteRunlog(RunlogDeleteRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/Delete?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }       
        public BasicResponse<List<RunlogInfo>> GetRunlogList(RunlogGetListRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/GetPageList?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RunlogInfo>>>(responseStr);
        }        
        public BasicResponse<RunlogInfo> GetRunlogById(RunlogGetRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/Get?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RunlogInfo>>(responseStr);
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>       
        public BasicResponse DeleteRunlogByEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/DeleteRunlogByEtime?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="operatelogrequest"></param>
        /// <returns></returns>       
        public BasicResponse DeleteRunlogByStimeEtime(RunlogDeleteByStimeEtimeRequest runlogrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Runlog/DeleteRunlogByStimeEtime?token=" + Token, JSONHelper.ToJSONString(runlogrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
