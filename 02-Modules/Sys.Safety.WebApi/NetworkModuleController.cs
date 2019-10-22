using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.NetworkModule;
using Sys.DataCollection.Common.Protocols;

namespace Sys.Safety.WebApi
{
    public class NetworkModuleController : Basic.Framework.Web.WebApi.BasicApiController, INetworkModuleService
    {
        static NetworkModuleController()
        {

        }
        INetworkModuleService _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();

        [HttpPost]
        [Route("v1/NetworkModule/Add")]
        public BasicResponse AddNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleAddRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.AddNetworkModule(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/AddNetworkModules")]
        public BasicResponse AddNetworkModules(Sys.Safety.Request.NetworkModule.NetworkModulesRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.AddNetworkModules(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/Update")]
        public BasicResponse UpdateNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleUpdateRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.UpdateNetworkModule(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/UpdateNetworkModules")]
        public BasicResponse UpdateNetworkModules(Sys.Safety.Request.NetworkModule.NetworkModulesRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.UpdateNetworkModules(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/Delete")]
        public BasicResponse DeleteNetworkModule(Sys.Safety.Request.NetworkModule.NetworkModuleDeleteByMacRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.DeleteNetworkModule(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetPageList")]
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList(Sys.Safety.Request.NetworkModule.NetworkModuleGetListRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.GetNetworkModuleList(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetList")]
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList()
        {
            return _NetworkModuleService.GetNetworkModuleList();
        }
        [HttpPost]
        [Route("v1/NetworkModule/Get")]
        public BasicResponse<Jc_MacInfo> GetNetworkModuleById(Sys.Safety.Request.NetworkModule.NetworkModuleGetRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.GetNetworkModuleById(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetSwitchsPosition")]
        public BasicResponse<List<string>> GetSwitchsPosition()
        {
            return _NetworkModuleService.GetSwitchsPosition();
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetAllNetworkModuleCache")]
        public BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache()
        {
            return _NetworkModuleService.GetAllNetworkModuleCache();
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetAllSwitchsCache")]
        public BasicResponse<List<Jc_MacInfo>> GetAllSwitchsCache()
        {
            return _NetworkModuleService.GetAllSwitchsCache();
        }    
        [HttpPost]
        [Route("v1/NetworkModule/SearchALLNetworkModuleAndAddCache")]
        public BasicResponse<List<Jc_MacInfo>> SearchALLNetworkModuleAndAddCache(SearchNetworkModuleRequest request)
        {
            return _NetworkModuleService.SearchALLNetworkModuleAndAddCache(request);
        }
        [HttpPost]
        [Route("v1/NetworkModule/SetNetworkModuletParameters")]
        public BasicResponse SetNetworkModuletParameters(NetworkModuletParametersSetRequest networkModuleCacheRequest)
        {
            return _NetworkModuleService.SetNetworkModuletParameters(networkModuleCacheRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/SetNetworkModuletParametersComm")]
        public BasicResponse SetNetworkModuletParametersComm(NetworkModuletCommParametersSetRequest networkModuleCacheRequest)
        {
            return _NetworkModuleService.SetNetworkModuletParametersComm(networkModuleCacheRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetNetworkModuletParameters")]
        public BasicResponse<NetDeviceSettingInfo> GetNetworkModuletParameters(NetworkModuletParametersGetRequest networkModuleCacheRequest)
        {
            return _NetworkModuleService.GetNetworkModuletParameters(networkModuleCacheRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/NetworkModuleSaveData")]
        public BasicResponse NetworkModuleSaveData()
        {
            return _NetworkModuleService.NetworkModuleSaveData();
        }


        [HttpPost]
        [Route("v1/NetworkModule/GetNetworkModuleCacheByWz")]
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByWz(NetworkModuleGetByWzRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.GetNetworkModuleCacheByWz(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetNetworkModuleCacheBySwitchesMac")]
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheBySwitchesMac(NetworkModuleGetBySwitchesMacRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.GetNetworkModuleCacheBySwitchesMac(NetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/NetworkModule/GetNetworkModuleCacheByMac")]
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByMac(NetworkModuleGetByMacRequest NetworkModuleRequest)
        {
            return _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
        }

        [HttpPost]
        [Route("v1/NetworkModule/GetAllPowerBoxAddress")]
        public BasicResponse<List<string>> GetAllPowerBoxAddress(Sys.Safety.Request.NetworkModule.GetAllPowerBoxAddressByMacRequest request)
        {
            return _NetworkModuleService.GetAllPowerBoxAddress(request);
        }

        [HttpPost]
        [Route("v1/NetworkModule/GetSubstationBatteryInfo")]
        public BasicResponse<Sys.Safety.DataContract.CommunicateExtend.BatteryItem> GetSwitchBatteryInfo(GetSwitchBatteryInfoRequest request)
        {
            return _NetworkModuleService.GetSwitchBatteryInfo(request);
        }

        [HttpPost]
        [Route("v1/NetworkModule/GetSwitchAllPowerBoxInfo")]
        public BasicResponse<GetSwitchAllPowerBoxInfoResponse> GetSwitchAllPowerBoxInfo(GetSwitchAllPowerBoxInfoRequest request)
        {
            return _NetworkModuleService.GetSwitchAllPowerBoxInfo(request);
        }

        [HttpPost]
        [Route("v1/NetworkModule/SetNetworkModuleSyncTime")]
        public BasicResponse SetNetworkModuleSyncTime()
        {
            return _NetworkModuleService.SetNetworkModuleSyncTime();
        }

        [HttpPost]
        [Route("v1/NetworkModule/TestAlarm")]
        public BasicResponse TestAlarm(TestAlarmRequest testAlarmRequest)
        {
            return _NetworkModuleService.TestAlarm(testAlarmRequest);
        }
    }
}
