using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Common;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.NetworkModule;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract.CommunicateExtend;

namespace Sys.Safety.WebApiAgent
{
    public class NetworkModuleControllerProxy : BaseProxy, INetworkModuleService
    {

        public BasicResponse AddNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleAddRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/Add?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse AddNetworkModules(Sys.Safety.Request.NetworkModule.NetworkModulesRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/AddNetworkModules?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse UpdateNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleUpdateRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/Update?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse UpdateNetworkModules(Sys.Safety.Request.NetworkModule.NetworkModulesRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/UpdateNetworkModules?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse DeleteNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleDeleteByMacRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/Delete?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList(Sys.Safety.Request.NetworkModule.NetworkModuleGetListRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetPageList?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }
        public BasicResponse<Jc_MacInfo> GetNetworkModuleById(Sys.Safety.Request.NetworkModule.NetworkModuleGetRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/Get?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_MacInfo>>(responseStr);
        }
        public BasicResponse<List<string>> GetSwitchsPosition()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetSwitchsPosition?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<string>>>(responseStr);
        }
        public BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetAllNetworkModuleCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByDynamicCondition(NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetNetworkModuleCacheByDynamicCondition?token=" + Token, JSONHelper.ToJSONString(networkModuleCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_MacInfo>> SearchALLNetworkModuleAndAddCache(SearchNetworkModuleRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/SearchALLNetworkModuleAndAddCache?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }
        public BasicResponse SetNetworkModuletParameters(NetworkModuletParametersSetRequest networkModuleCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/SetNetworkModuletParameters?token=" + Token, JSONHelper.ToJSONString(networkModuleCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }
        public BasicResponse SetNetworkModuletParametersComm(NetworkModuletCommParametersSetRequest networkModuleCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/SetNetworkModuletParametersComm?token=" + Token, JSONHelper.ToJSONString(networkModuleCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        } 
        public BasicResponse NetworkModuleSaveData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/NetworkModuleSaveData?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }



        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByWz(NetworkModuleGetByWzRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetNetworkModuleCacheByWz?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheBySwitchesMac(NetworkModuleGetBySwitchesMacRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetNetworkModuleCacheBySwitchesMac?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByMac(NetworkModuleGetByMacRequest NetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetNetworkModuleCacheByMac?token=" + Token, JSONHelper.ToJSONString(NetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }

        public BasicResponse<NetDeviceSettingInfo> GetNetworkModuletParameters(NetworkModuletParametersGetRequest networkModuleCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetNetworkModuletParameters?token=" + Token, JSONHelper.ToJSONString(networkModuleCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<NetDeviceSettingInfo>>(responseStr);
        }


        public BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressByMacRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetAllPowerBoxAddress?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<string>>>(responseStr);
        }

        public BasicResponse<DataContract.CommunicateExtend.BatteryItem> GetSwitchBatteryInfo(GetSwitchBatteryInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetSubstationBatteryInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<BatteryItem>>(responseStr);
        }


        public BasicResponse<GetSwitchAllPowerBoxInfoResponse> GetSwitchAllPowerBoxInfo(GetSwitchAllPowerBoxInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetSwitchAllPowerBoxInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<GetSwitchAllPowerBoxInfoResponse>>(responseStr);
        }


        public BasicResponse SetNetworkModuleSyncTime()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/SetNetworkModuleSyncTime?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse TestAlarm(TestAlarmRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/TestAlarm?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<Jc_MacInfo>> GetAllSwitchsCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/NetworkModule/GetAllSwitchsCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responseStr);
        }
    }
}
