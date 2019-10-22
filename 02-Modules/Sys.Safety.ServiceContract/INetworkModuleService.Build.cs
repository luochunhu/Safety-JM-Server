using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract.CommunicateExtend;

namespace Sys.Safety.ServiceContract
{
    public interface INetworkModuleService
    {
        /// <summary>
        /// 添加网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse AddNetworkModule(NetworkModuleAddRequest NetworkModuleRequest);
         /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse AddNetworkModules(NetworkModulesRequest NetworkModuleRequest);
        /// <summary>
        /// 更新网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkModule(NetworkModuleUpdateRequest NetworkModuleRequest);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkModules(NetworkModulesRequest NetworkModuleRequest);
        /// <summary>
        /// 删除网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteNetworkModule(NetworkModuleDeleteByMacRequest NetworkModuleRequest);     
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList(NetworkModuleGetListRequest NetworkModuleRequest);
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList();       
        BasicResponse<Jc_MacInfo> GetNetworkModuleById(NetworkModuleGetRequest NetworkModuleRequest);	
        /// <summary>
        /// 获取所有交换机的安装位置
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<string>> GetSwitchsPosition();
        /// <summary>
        /// 获取所有网格模块缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache();

        BasicResponse<List<Jc_MacInfo>> GetAllSwitchsCache();

       /// <summary>
        /// 根据安装位置获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByWz(NetworkModuleGetByWzRequest NetworkModuleRequest);
        /// <summary>
        /// 通过交换机的mac获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheBySwitchesMac(NetworkModuleGetBySwitchesMacRequest NetworkModuleRequest);
        /// <summary>
        /// 通过Mac获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByMac(NetworkModuleGetByMacRequest NetworkModuleRequest);

        /// <summary>
        /// 搜索网络模块，并更新缓存信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> SearchALLNetworkModuleAndAddCache(SearchNetworkModuleRequest request);
        /// <summary>
        /// 设置网络模块参数---基础参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse SetNetworkModuletParameters(NetworkModuletParametersSetRequest networkModuleCacheRequest);
        /// <summary>
        /// 设置网络模块参数---串口参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse SetNetworkModuletParametersComm(NetworkModuletCommParametersSetRequest networkModuleCacheRequest);
        /// <summary>
        /// 获取网络模块参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<NetDeviceSettingInfo> GetNetworkModuletParameters(NetworkModuletParametersGetRequest networkModuleCacheRequest);
        /// <summary>
        /// 网络模块保存巡检
        /// </summary>
        /// <returns></returns>
        BasicResponse NetworkModuleSaveData();

        /// <summary>
        /// 获取所有电源箱地址号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressByMacRequest request);

        /// <summary>
        /// 获取电源箱信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<BatteryItem> GetSwitchBatteryInfo(GetSwitchBatteryInfoRequest request);

        /// <summary>
        /// 根据mac地址获取所有电源箱信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<GetSwitchAllPowerBoxInfoResponse> GetSwitchAllPowerBoxInfo(GetSwitchAllPowerBoxInfoRequest request);
        /// <summary>
        /// 下发所有网络模块时间同步命令接口
        /// </summary>
        /// <returns></returns>
        BasicResponse SetNetworkModuleSyncTime();

        BasicResponse TestAlarm(TestAlarmRequest testAlarmRequest);
    }
}

