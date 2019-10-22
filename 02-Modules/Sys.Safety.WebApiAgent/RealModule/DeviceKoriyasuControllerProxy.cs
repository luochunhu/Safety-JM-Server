using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent.RealModule
{
    public class DeviceKoriyasuControllerProxy : BaseProxy, IDeviceKoriyasuService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefwbInfo> AddDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.DeviceKoriyasuAddRequest jc_Defwbrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/DeviceKoriyasu/AddDeviceKoriyasu?token=" + Token, JSONHelper.ToJSONString(jc_Defwbrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefwbInfo>>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefwbInfo> UpdateDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.Jc_DefwbUpdateRequest jc_Defwbrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/DeviceKoriyasu/UpdateDeviceKoriyasu?token=" + Token, JSONHelper.ToJSONString(jc_Defwbrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefwbInfo>>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse DeleteDeviceKoriyasu(Sys.Safety.Request.Jc_Defwb.Jc_DefwbDeleteRequest jc_Defwbrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/DeviceKoriyasu/DeleteDeviceKoriyasu?token=" + Token, JSONHelper.ToJSONString(jc_Defwbrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefwbInfo>> GetDeviceKoriyasuList(Sys.Safety.Request.Jc_Defwb.Jc_DefwbGetListRequest jc_Defwbrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/DeviceKoriyasu/GetDeviceKoriyasuList?token=" + Token, JSONHelper.ToJSONString(jc_Defwbrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_DefwbInfo>>>(responsestr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefwbInfo> GetDeviceKoriyasuById(Sys.Safety.Request.Jc_Defwb.Jc_DefwbGetRequest jc_Defwbrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/DeviceKoriyasu/GetDeviceKoriyasuById?token=" + Token, JSONHelper.ToJSONString(jc_Defwbrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefwbInfo>>(responsestr);
        }
    }
}
