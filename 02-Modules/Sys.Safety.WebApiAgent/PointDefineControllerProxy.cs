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
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.WebApiAgent
{
    public class PointDefineControllerProxy : BaseProxy, IPointDefineService
    {

        /// <summary>
        /// 添加测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefine(Sys.Safety.Request.PointDefine.PointDefineAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/Add?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefines(Sys.Safety.Request.PointDefine.PointDefinesAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/AddPointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefine(Sys.Safety.Request.PointDefine.PointDefineUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/Update?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefines(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/UpdatePointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse UpdatePointDefinesCache(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/UpdatePointDefinesCache?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
        /// </summary>
        /// <param name="PointDefineAddNetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(Sys.Safety.Request.PointDefine.PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/AddUpdatePointDefineAndNetworkModuleCache?token=" + Token, JSONHelper.ToJSONString(PointDefineAddNetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeletePointDefine(Sys.Safety.Request.PointDefine.PointDefineDeleteRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/Delete?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList(Sys.Safety.Request.PointDefine.PointDefineGetListRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPageList?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<Jc_DefInfo> GetPointDefineById(Sys.Safety.Request.PointDefine.PointDefineGetRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/Get?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetAllPointDefineCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据测点号查找缓存信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(Sys.Safety.Request.PointDefine.PointDefineGetByPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        public BasicResponse<Jc_DefInfo> GetNotMonitoringPointDefineCacheByPoint(StringRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetNotMonitoringPointDefineCacheByPoint?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByDynamicCondition?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetCOMCommunicationStation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetNonCommunicationStation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 测点定义保存巡检操作
        /// </summary>
        /// <returns></returns>
        public BasicResponse PointDefineSaveData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/PointDefineSaveData?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse SendDCom(SendDComReqest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/SendDCom?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(Sys.Safety.Request.PointDefine.PointDefineGetByWzRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByWz?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(Sys.Safety.Request.PointDefine.PointDefineGetByMacRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByMac?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(Sys.Safety.Request.PointDefine.PointDefineGetByCOMRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByCOM?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(Sys.Safety.Request.PointDefine.PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(Sys.Safety.Request.PointDefine.PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByDevClassID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(Sys.Safety.Request.PointDefine.PointDefineGetByDevModelIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByDevModelID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(Sys.Safety.Request.PointDefine.PointDefineGetByDevIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByDevID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(Sys.Safety.Request.PointDefine.PointDefineGetByStationPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationIDChannelIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStationIDChannelID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(Sys.Safety.Request.PointDefine.PointDefineGetByAreaIdRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByAreaId?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(Sys.Safety.Request.PointDefine.PointDefineGetByAreaCodeRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByAreaCode?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByAreaCodeDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(Sys.Safety.Request.PointDefine.PointDefineGetByStrKeywordsRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByStrKeywords?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByPointID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetAllPowerBoxAddress?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<string>>>(responseStr);
        }

        public BasicResponse<DataContract.CommunicateExtend.BatteryItem> GetSubstationBatteryInfo(GetSubstationBatteryInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetSubstationBatteryInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<BatteryItem>>(responseStr);
        }


        public BasicResponse<GetSubstationAllPowerBoxInfoResponse> GetSubstationAllPowerBoxInfo(GetSubstationAllPowerBoxInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetSubstationAllPowerBoxInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<GetSubstationAllPowerBoxInfoResponse>>(responseStr);
        }


        public BasicResponse SendQueryHistoryControlRequest(GetHistoryControlRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/QueryHistoryControlRequest?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse SendQueryBatteryRealDataRequest(SendDComReqest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/SendQueryBatteryRealDataRequest?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse SendSwitchesDControl(SwitchesDControlRequest controlInfo)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/SendSwitchesDControl?token=" + Token, JSONHelper.ToJSONString(controlInfo));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse SendQueryHistoryRealDataRequest(HistoryRealDataRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/QueryHistoryRealDataRequest?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse QueryDeviceInfoRequest(DeviceInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/QueryDeviceInfoRequest?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse ModificationDeviceAdressRequest(DeviceAddressModificationRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/ModificationDeviceAdressRequest?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse SendStationDControl(StationDControlRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/SendStationDControl?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetAllAutomaticArticulatedDeviceCache?token=" + Token, JSONHelper.ToJSONString(AutomaticArticulatedDeviceCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AutomaticArticulatedDeviceInfo>>>(responseStr);
        }


        public BasicResponse<GetSubstationBasicInfoResponse> GetSubstationBasicInfo(GetSubstationBasicInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetSubstationBasicInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<GetSubstationBasicInfoResponse>>(responseStr);
        }

        public BasicResponse<bool> ControlPointLegal(PointDefineGetByPointIDRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/ControlPointLegal?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse<bool> ControlPointLegalAll(PointDefineGetByPointIDRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/ControlPointLegalAll?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/BatchUpdatePointDefineInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetNotMonitoringPointDefineCacheByDevpropertID(Sys.Safety.Request.Listex.IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetNotMonitoringPointDefineCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetNotMonitoringPointDefineCacheByDevID(Sys.Safety.Request.Listex.IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetNotMonitoringPointDefineCacheByDevID?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetNotMonitoringPointDefineCacheByStationID(Sys.Safety.Request.Listex.IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetNotMonitoringPointDefineCacheByStationID?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySensorPowerAlarmValue(PointDefineGetBySensorPowerAlarmValueRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheBySensorPowerAlarmValue?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByGradingAlarmLevel(PointDefineGetByGradingAlarmLevelRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByGradingAlarmLevel?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<int> QueryRealLinkageInfoFromMonitor(GetRealLinkageInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/QueryRealLinkageInfoFromMonitor?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByUnderVoltageAlarmValue(PointDefineGetByUnderVoltageAlarmValueRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByUnderVoltageAlarmValue?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAddressTypeId(PointDefineGetByAddressTypeIdRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PointDefine/GetPointDefineCacheByAddressTypeId?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
    }
}
