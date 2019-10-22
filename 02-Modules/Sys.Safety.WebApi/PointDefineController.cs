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

namespace Sys.Safety.WebApi
{
    public class PointDefineController : Basic.Framework.Web.WebApi.BasicApiController, IPointDefineService
    {
        static PointDefineController()
        {

        }
        IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>();   
        /// <summary>
        ///  添加测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/Add")]
        public BasicResponse AddPointDefine(PointDefineAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefine(PointDefineRequest);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/AddPointDefines")]
        public BasicResponse AddPointDefines(PointDefinesAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefines(PointDefineRequest);
        }
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/Update")]
        public BasicResponse UpdatePointDefine(PointDefineUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefine(PointDefineRequest);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/UpdatePointDefines")]
        public BasicResponse UpdatePointDefines(PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefines(PointDefineRequest);
        }
        /// <summary>
        /// 批量更新缓存
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/UpdatePointDefinesCache")]
        public BasicResponse UpdatePointDefinesCache(PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefinesCache(PointDefineRequest);
        }
       /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
       /// </summary>
       /// <param name="PointDefineAddNetworkModuleRequest"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/AddUpdatePointDefineAndNetworkModuleCache")]
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            return _PointDefineService.AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/Delete")]
        public BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest)
        {
            return _PointDefineService.DeletePointDefine(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPageList")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList(PointDefineGetListRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineList(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetList")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList()
        {
            return _PointDefineService.GetPointDefineList();
        }
        [HttpPost]
        [Route("v1/PointDefine/Get")]
        public BasicResponse<Jc_DefInfo> GetPointDefineById(PointDefineGetRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineById(PointDefineRequest);
        }
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/GetAllPointDefineCache")]
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            return _PointDefineService.GetAllPointDefineCache();
        }
        /// <summary>
        /// 根据测点号查找缓存信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByPoint")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPoint(PointDefineRequest);
        }
        
        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/GetNetworkCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation()
        {
            return _PointDefineService.GetNetworkCommunicationStation();
        }
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/GetCOMCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation()
        {
            return _PointDefineService.GetCOMCommunicationStation();
        }
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/GetNonCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation()
        {
            return _PointDefineService.GetNonCommunicationStation();
        }
        /// <summary>
        /// 测点定义保存巡检操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PointDefine/PointDefineSaveData")]
        public BasicResponse PointDefineSaveData()
        {
            return _PointDefineService.PointDefineSaveData();
        }

        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByWz")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(PointDefineGetByWzRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByWz(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByMac")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(PointDefineGetByMacRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByMac(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByCOM")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(PointDefineGetByCOMRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByCOM(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByDevpropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByDevClassID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByDevModelID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(PointDefineGetByDevModelIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevModelID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByDevID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(PointDefineGetByDevIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(PointDefineGetByStationIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationPoint")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(PointDefineGetByStationPointRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationPoint(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationIDChannelIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStationIDChannelID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(PointDefineGetByStationIDChannelIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByAreaId")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(PointDefineGetByAreaIdRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaId(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByAreaCode")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(PointDefineGetByAreaCodeRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaCode(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByAreaCodeDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaCodeDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByStrKeywords")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(PointDefineGetByStrKeywordsRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStrKeywords(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByPointID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetAllPowerBoxAddress")]
        public BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressRequest request)
        {
            return _PointDefineService.GetAllPowerBoxAddress(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetSubstationBatteryInfo")]
        public BasicResponse<Sys.Safety.DataContract.CommunicateExtend.BatteryItem> GetSubstationBatteryInfo(GetSubstationBatteryInfoRequest request)
        {
            return _PointDefineService.GetSubstationBatteryInfo(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetSubstationAllPowerBoxInfo")]
        public BasicResponse<GetSubstationAllPowerBoxInfoResponse> GetSubstationAllPowerBoxInfo(GetSubstationAllPowerBoxInfoRequest request)
        {
            return _PointDefineService.GetSubstationAllPowerBoxInfo(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/QueryHistoryControlRequest")]
        public BasicResponse SendQueryHistoryControlRequest(GetHistoryControlRequest request)
        {
            return _PointDefineService.SendQueryHistoryControlRequest(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/SendQueryBatteryRealDataRequest")]
        public BasicResponse SendQueryBatteryRealDataRequest(SendDComReqest request)
        {
            return _PointDefineService.SendQueryBatteryRealDataRequest(request);
        }
        [HttpPost]
        [Route("v1/PointDefine/SendSwitchesDControl")]
        public BasicResponse SendSwitchesDControl(SwitchesDControlRequest controlInfo)
        {
            return _PointDefineService.SendSwitchesDControl(controlInfo);
        }

        [HttpPost]
        [Route("v1/PointDefine/QueryHistoryRealDataRequest")]
        public BasicResponse SendQueryHistoryRealDataRequest(HistoryRealDataRequest request)
        {
            return _PointDefineService.SendQueryHistoryRealDataRequest(request);
        }
        [HttpPost]
        [Route("v1/PointDefine/QueryDeviceInfoRequest")]
        public BasicResponse QueryDeviceInfoRequest(DeviceInfoRequest request)
        {
            return _PointDefineService.QueryDeviceInfoRequest(request);
        }
        [HttpPost]
        [Route("v1/PointDefine/ModificationDeviceAdressRequest")]
        public BasicResponse ModificationDeviceAdressRequest(DeviceAddressModificationRequest request)
        {
            return _PointDefineService.ModificationDeviceAdressRequest(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/SendStationDControl")]
        public BasicResponse SendStationDControl(StationDControlRequest controlInfo)
        {
            return _PointDefineService.SendStationDControl(controlInfo);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetAllAutomaticArticulatedDeviceCache")]
        public BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest)
        {
            return _PointDefineService.GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheRequest);
        }
        
        [HttpPost]
        [Route("v1/PointDefine/GetSubstationBasicInfo")]
        public BasicResponse<GetSubstationBasicInfoResponse> GetSubstationBasicInfo(GetSubstationBasicInfoRequest request)
        {
            return _PointDefineService.GetSubstationBasicInfo(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/ControlPointLegal")]
        public BasicResponse<bool> ControlPointLegal(PointDefineGetByPointIDRequest request)
        {
            return _PointDefineService.ControlPointLegal(request);
        }
        [HttpPost]
        [Route("v1/PointDefine/ControlPointLegalAll")]
        public BasicResponse<bool> ControlPointLegalAll(PointDefineGetByPointIDRequest request)
        {
            return _PointDefineService.ControlPointLegalAll(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/BatchUpdatePointDefineInfo")]
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            return _PointDefineService.BatchUpdatePointDefineInfo(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheBySensorPowerAlarmValue")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySensorPowerAlarmValue(PointDefineGetBySensorPowerAlarmValueRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheBySensorPowerAlarmValue(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByGradingAlarmLevel")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByGradingAlarmLevel(PointDefineGetByGradingAlarmLevelRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByGradingAlarmLevel(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PointDefine/QueryRealLinkageInfoFromMonitor")]
        public BasicResponse<int> QueryRealLinkageInfoFromMonitor(GetRealLinkageInfoRequest request)
        {
            return _PointDefineService.QueryRealLinkageInfoFromMonitor(request);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByUnderVoltageAlarmValue")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByUnderVoltageAlarmValue(PointDefineGetByUnderVoltageAlarmValueRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByUnderVoltageAlarmValue(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PointDefine/GetPointDefineCacheByAddressTypeId")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAddressTypeId(PointDefineGetByAddressTypeIdRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAddressTypeId(PointDefineRequest);
        }
    }
}
