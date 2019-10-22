using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class MultiplesettingControllerProxy : BaseProxy, IJC_MultiplesettingService
    {
        
        public BasicResponse<JC_MultiplesettingInfo> AddMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingAddRequest multiplesettingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/AddMultiplesetting?token=" + Token, JSONHelper.ToJSONString(multiplesettingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_MultiplesettingInfo>>(responseStr);
        }

        public BasicResponse<JC_MultiplesettingInfo> UpdateMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingUpdateRequest multiplesettingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/UpdateMultiplesetting?token=" + Token, JSONHelper.ToJSONString(multiplesettingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_MultiplesettingInfo>>(responseStr);
        }

        public BasicResponse DeleteMultiplesetting(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingDeleteRequest multiplesettingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/DeleteMultiplesetting?token=" + Token, JSONHelper.ToJSONString(multiplesettingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_MultiplesettingInfo>>(responseStr);
        }

        public BasicResponse<List<JC_MultiplesettingInfo>> GetMultiplesettingList(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingGetListRequest multiplesettingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/GetMultiplesettingList?token=" + Token, JSONHelper.ToJSONString(multiplesettingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_MultiplesettingInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_MultiplesettingInfo>> GetAllMultiplesettingList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/GetAllMultiplesettingList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_MultiplesettingInfo>>>(responseStr);
        }

        public BasicResponse<JC_MultiplesettingInfo> GetMultiplesettingById(Sys.Safety.Request.JC_Multiplesetting.JC_MultiplesettingGetRequest multiplesettingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Multiplesetting/GetMultiplesettingById?token=" + Token, JSONHelper.ToJSONString(multiplesettingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_MultiplesettingInfo>>(responseStr);
        }
    }
}
