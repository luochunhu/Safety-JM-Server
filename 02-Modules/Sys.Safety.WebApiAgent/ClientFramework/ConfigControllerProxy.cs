using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Config;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class ConfigControllerProxy : BaseProxy, IConfigService
    {
        public BasicResponse<ConfigInfo> AddConfig(ConfigAddRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/Add?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ConfigInfo>>(responseStr);
        }
        public BasicResponse<ConfigInfo> UpdateConfig(ConfigUpdateRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/Update?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ConfigInfo>>(responseStr);
        }
        public BasicResponse DeleteConfig(ConfigDeleteRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/Delete?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<ConfigInfo>> GetConfigList(ConfigGetListRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetPageList?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<ConfigInfo>>>(responseStr);
        }
        public BasicResponse<List<ConfigInfo>> GetConfigList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<ConfigInfo>>>(responseStr);
        }
        public BasicResponse<ConfigInfo> GetConfigById(ConfigGetRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetConfigById?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ConfigInfo>>(responseStr);
        }
        public BasicResponse<ConfigInfo> GetConfigByName(ConfigGetByNameRequest configrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetByName?token=" + Token, JSONHelper.ToJSONString(configrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ConfigInfo>>(responseStr);
        }
        public BasicResponse SaveInspection()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/SaveInspection?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<RunningInfo>  GetRunningInfo()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetRunningInfo?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<RunningInfo>>(responseStr);
        }

        public BasicResponse<HardDiskInfo>  GetDiskInfo(ConfigGetDiskInfoRequest request)
        {
           // var responseStr = HttpClientHelper.Post(webapi + "/v1/Config/GetDiskInfo?token=" + token, JSONHelper.ToJSONString(diskName));
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetDiskInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<HardDiskInfo>>(responseStr);
        }

        public BasicResponse<PorcessInfo> GetProcessInfo(ConfigGetProcessInfoRequest request)
        {
            //var responseStr = HttpClientHelper.Post(webapi + "/v1/Config/GetProcessInfo?token=" + token, JSONHelper.ToJSONString(processName));
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetProcessInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<PorcessInfo>>(responseStr);
        }

        public BasicResponse<bool> GetDbState()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetDbState?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse<HardDiskInfo> GetDatabaseDiskInfo()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/GetDatabaseDiskInfo?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<HardDiskInfo>>(responseStr);
        }
        public BasicResponse ExitServer()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/ExitServer?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse SaveInspectionIn(SaveInspectionInRequest saveInspectionInRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Config/SaveInspectionIn?token=" + Token, JSONHelper.ToJSONString(saveInspectionInRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
