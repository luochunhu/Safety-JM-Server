using Basic.Framework.Service;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.QueryCacheData
{
    public class QueryCacheDataToGateway
    {
        static IDeviceDefineService _DeviceDefineService;
        /// <summary>
        /// 修改为获取所有缓存  20171205
        /// </summary>
        static IAllSystemPointDefineService _PointDefineService;
        static INetworkModuleService _NetworkModuleService;
        static IManualCrossControlService _ManualCrossControlService;
        static QueryCacheDataToGateway()
        {
            _DeviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
            _PointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();
            _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
            _ManualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        }
        /// <summary>
        /// 查询服务接口端缓存信息，并同步到网关
        /// </summary>
        /// <returns></returns>
        public static QueryCacheDataResponse QueryServiceCacheDataToGateway()
        {
            QueryCacheDataResponse queryCacheDataResponse = new QueryCacheDataResponse();
            //获取定义、设备类型、网络模块、手动/交叉控制缓存信息
            List<Jc_DefInfo> PointDefineCacheList = _PointDefineService.GetAllPointDefineCache().Data.FindAll(a => a.Upflag != "1");//修改，不向网关同步由子系统同步的数据  20180131
            List<Jc_DevInfo> DeviceDefineCacheList = _DeviceDefineService.GetAllDeviceDefineCache().Data;
            List<Jc_MacInfo> NetworkModuleCacheList = _NetworkModuleService.GetAllNetworkModuleCache().Data;
            List<Jc_JcsdkzInfo> ManualCrossControlCacheList = _ManualCrossControlService.GetAllManualCrossControl().Data;
            //数据转换
            List<DeviceInfo> GatewayPointDefineCacheList = Basic.Framework.Common.ObjectConverter.CopyList<Jc_DefInfo, DeviceInfo>(PointDefineCacheList).ToList();
            List<DeviceTypeInfo> GatewayDeviceDefineCacheList = Basic.Framework.Common.ObjectConverter.CopyList<Jc_DevInfo, DeviceTypeInfo>(DeviceDefineCacheList).ToList();
            List<NetworkDeviceInfo> GatewayNetworkModuleCacheList = Basic.Framework.Common.ObjectConverter.CopyList<Jc_MacInfo, NetworkDeviceInfo>(NetworkModuleCacheList).ToList();
            List<DeviceAcrossControlInfo> GatewayManualCrossControlCacheList = Basic.Framework.Common.ObjectConverter.CopyList<Jc_JcsdkzInfo, DeviceAcrossControlInfo>(ManualCrossControlCacheList).ToList();
            //key赋值
            foreach (DeviceInfo PointDefine in GatewayPointDefineCacheList)
            {
                PointDefine.UniqueKey = PointDefine.Point;
            }
            foreach (DeviceTypeInfo DeviceDefine in GatewayDeviceDefineCacheList)
            {
                DeviceDefine.UniqueKey = DeviceDefine.Devid;
            }
            foreach (NetworkDeviceInfo NetworkModule in GatewayNetworkModuleCacheList)
            {
                NetworkModule.UniqueKey = NetworkModule.MAC;
            }
            foreach (DeviceAcrossControlInfo ManualCrossControl in GatewayManualCrossControlCacheList)
            {
                ManualCrossControl.UniqueKey = ManualCrossControl.ID;
            }

            queryCacheDataResponse.DeviceList = GatewayPointDefineCacheList;
            queryCacheDataResponse.DeviceTypeList = GatewayDeviceDefineCacheList;
            queryCacheDataResponse.NetworkDeviceList = GatewayNetworkModuleCacheList;
            queryCacheDataResponse.DeviceAcrossControlList = GatewayManualCrossControlCacheList;

            return queryCacheDataResponse;
        }
    }
}
