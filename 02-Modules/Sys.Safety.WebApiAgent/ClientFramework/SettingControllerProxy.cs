using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Setting;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class SettingControllerProxy : BaseProxy, ISettingService
    {
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>        
        public BasicResponse<SettingInfo> SaveSetting(SettingAddRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/SaveSetting?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }
        public BasicResponse<SettingInfo> AddSetting(SettingAddRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/Add?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }
        public BasicResponse<SettingInfo> UpdateSetting(SettingUpdateRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/Update?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }
        public BasicResponse DeleteSetting(SettingDeleteRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/Delete?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<SettingInfo>> GetSettingList(SettingGetListRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/GetPageList?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<SettingInfo>>>(responseStr);
        }
        public BasicResponse<List<SettingInfo>> GetSettingList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<SettingInfo>>>(responseStr);
        }
        public BasicResponse<SettingInfo> GetSettingById(SettingGetRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/Get?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }

        public BasicResponse<SettingInfo> GetSettingByKey(GetSettingByKeyRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/GetSettingByKey?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }


        public BasicResponse SaveSettingForCondition(SaveSettingForConditionRequest settingrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/SaveSettingForCondition?token=" + Token, JSONHelper.ToJSONString(settingrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<SettingInfo>> GetSettingListForCreator()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/GetSettingListForCreator?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<SettingInfo>>>(responseStr);
        }

        public BasicResponse<SettingInfo> GetSettingCacheByKey(GetSettingCacheByKeyRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Setting/GetSettingCacheByKey?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<SettingInfo>>(responseStr);
        }
    }
}
