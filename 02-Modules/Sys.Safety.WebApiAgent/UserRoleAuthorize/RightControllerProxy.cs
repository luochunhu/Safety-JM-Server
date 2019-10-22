using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Right;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class RightControllerProxy : BaseProxy, IRightService
    {
        public BasicResponse<RightInfo> AddRight(RightAddRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/Add?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RightInfo>>(responseStr);
        }        
        public BasicResponse<RightInfo> UpdateRight(RightUpdateRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/Update?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RightInfo>>(responseStr);
        }       
        public BasicResponse DeleteRight(RightDeleteRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/Delete?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteRights(RightsDeleteRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/DeleteRights?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<List<RightInfo>> GetRightList(RightGetListRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/GetPageList?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RightInfo>>>(responseStr);
        }       
        public BasicResponse<List<RightInfo>> GetRightList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<RightInfo>>>(responseStr);
        }        
        public BasicResponse<RightInfo> GetRightById(RightGetRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/Get?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RightInfo>>(responseStr);
        }
        /// <summary>
        /// 添加一个全新信息到权限表并返回成功后的权限对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>       
        public BasicResponse<RightInfo> AddRightEx(RightAddRequest rightrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Right/AddRightEx?token=" + Token, JSONHelper.ToJSONString(rightrequest));
            return JSONHelper.ParseJSONString<BasicResponse<RightInfo>>(responseStr);
        }
    }
}
