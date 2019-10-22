using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class AlarmNotificationPersonnelControllerProxy : BaseProxy, IAlarmNotificationPersonnelService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.JC_AlarmNotificationPersonnelInfo> AddJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/AddJC_AlarmNotificationPersonnel?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_AlarmNotificationPersonnelInfo> UpdateJC_AlarmNotificationPersonnel(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelUpdateRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/UpdateJC_AlarmNotificationPersonnel?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelDeleteRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/DeleteJC_AlarmNotificationPersonnel?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelList(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetListRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelList?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_AlarmNotificationPersonnelInfo> GetJC_AlarmNotificationPersonnelById(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelById?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmNotificationPersonnelInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/GetJC_AlarmNotificationPersonnelListByAlarmConfigId?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>> GetAlarmNotificationPersonnelByAnalysisModelId(Sys.Safety.Request.AlarmNotificationPersonnel.AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/GetAlarmNotificationPersonnelByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> AddJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmNotificationPersonnel/AddJC_AlarmNotificationPersonnelList?token=" + Token, JSONHelper.ToJSONString(jC_AlarmNotificationPersonnelrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_AlarmNotificationPersonnelInfo>>>(responseStr);
        }

    }
}
