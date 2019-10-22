using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.DataCollection.Common.Protocols.Devices;

namespace Sys.Safety.ServiceContract
{
    public interface IPointDefineService
    {
        /// <summary>
        /// 添加测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefine(PointDefineAddRequest PointDefineRequest);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefines(PointDefinesAddRequest PointDefineRequest);
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefine(PointDefineUpdateRequest PointDefineRequest);
        ///<summary>
        /// 批量更新
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefines(PointDefinesUpdateRequest PointDefineRequest);
        /// <summary>
        /// 批量更新测点定义缓存
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefinesCache(PointDefinesUpdateRequest PointDefineRequest);
        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request);
        /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
        /// </summary>
        /// <param name="jc_DefAndjc_Macrequest"></param>
        /// <returns></returns>
        BasicResponse AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest);
        BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest);
        BasicResponse<List<Jc_DefInfo>> GetPointDefineList(PointDefineGetListRequest PointDefineRequest);
        BasicResponse<List<Jc_DefInfo>> GetPointDefineList();
        BasicResponse<Jc_DefInfo> GetPointDefineById(PointDefineGetRequest PointDefineRequest);
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache();
        /// <summary>
        /// 根据测点号查找缓存信息
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest);

         /// <summary>
        /// 根据测点名称/位置查询测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(PointDefineGetByWzRequest PointDefineRequest);
        /// <summary>
        ///  根据MAC地址查询MAC地址对应的分站信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(PointDefineGetByMacRequest PointDefineRequest);
        /// <summary>
        ///  查找COM下的所有分站
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(PointDefineGetByCOMRequest PointDefineRequest);
        /// <summary>
        ///通过设备性质查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest);
        /// <summary>
        ///通过设备种类查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest);
        /// <summary>
        ///通过设备型号查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(PointDefineGetByDevModelIDRequest PointDefineRequest);
        /// <summary>
        ///通过设备类型查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(PointDefineGetByDevIDRequest PointDefineRequest);
        /// <summary>
        ///根据分站号查找分站下的所有测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(PointDefineGetByStationIDRequest PointDefineRequest);
        /// <summary>
        ///根据分站号查找分站下的所有测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(PointDefineGetByStationPointRequest PointDefineRequest);
        /// <summary>
        ///通过分站号、口号、设备性质查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest);
        /// <summary>
        ///通过分站号、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest);
         /// <summary>
        ///通过分站号、通道号 地址号、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest);
         /// <summary>
        ///通过分站号、通道号 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(PointDefineGetByStationIDChannelIDRequest PointDefineRequest);
        /// <summary>
        /// 通过区域ID 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(PointDefineGetByAreaIdRequest PointDefineRequest);
        /// <summary>
        /// 根据地点类型获取测点信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAddressTypeId(PointDefineGetByAddressTypeIdRequest PointDefineRequest);
        /// <summary>
        ///通过区域编码 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(PointDefineGetByAreaCodeRequest PointDefineRequest);
        /// <summary>
        ///通过区域编码、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest);
        /// <summary>
        ///通过区域名称、测点号、安装位置等关键字查找分站设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(PointDefineGetByStrKeywordsRequest PointDefineRequest);
        /// <summary>
        ///通过测点ID 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest);
         /// <summary>
        /// 获取所有电池电量过低的传感器（目前只支持无线）
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySensorPowerAlarmValue(PointDefineGetBySensorPowerAlarmValueRequest PointDefineRequest);
        /// <summary>
        /// 获取所有欠压报警设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByUnderVoltageAlarmValue(PointDefineGetByUnderVoltageAlarmValueRequest PointDefineRequest);
        /// <summary>
        /// 获取所有分级报警的设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByGradingAlarmLevel(PointDefineGetByGradingAlarmLevelRequest PointDefineRequest);
       
        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation();
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation();
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation();
        /// <summary>
        /// 测点定义保存巡检操作
        /// </summary>
        /// <returns></returns>
        BasicResponse PointDefineSaveData();

        /// <summary>
        /// 发送D命令
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse SendQueryBatteryRealDataRequest(SendDComReqest request);

        /// <summary>
        /// 下发交换机电源箱控制命令
        /// </summary>
        /// <param name="controlType"></param>
        /// <returns></returns>
        BasicResponse SendSwitchesDControl(SwitchesDControlRequest controlInfo);
        /// <summary>
        /// 根据分站号获取电源箱地址号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressRequest request);

        /// <summary>
        /// 获取电源箱信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<BatteryItem> GetSubstationBatteryInfo(GetSubstationBatteryInfoRequest request);

        /// <summary>
        /// 根据分站号获取所有电源箱信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<GetSubstationAllPowerBoxInfoResponse> GetSubstationAllPowerBoxInfo(GetSubstationAllPowerBoxInfoRequest request);
        /// <summary>
        /// 获取分站历史数据 2017.6.13 by
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        BasicResponse SendQueryHistoryControlRequest(GetHistoryControlRequest request);

        /// <summary>
        /// 获取分站历史5分钟数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse SendQueryHistoryRealDataRequest(HistoryRealDataRequest request);
        /// <summary>
        /// 获取设备唯一编码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse QueryDeviceInfoRequest(DeviceInfoRequest request);
        /// <summary>
        /// 修改设备地址号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse ModificationDeviceAdressRequest(DeviceAddressModificationRequest request);

        /// <summary>
        /// 分站电源箱控制
        /// </summary>
        /// <param name="controlInfo"></param>
        /// <returns></returns>
        BasicResponse SendStationDControl(StationDControlRequest controlInfo);
        /// <summary>
        /// 查找所有自动挂接设备
        /// </summary>
        /// <param name="AutomaticArticulatedDeviceCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest);

        /// <summary>
        /// 获取分站基础信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<GetSubstationBasicInfoResponse> GetSubstationBasicInfo(GetSubstationBasicInfoRequest request);

        /// <summary>
        /// 判断控制口是否被用作甲烷风电闭锁控制口或者模开口是否在风电闭锁中使用(true：表示未使用，false：表示已使用) 
        /// 2017-07-10 , 从Sys.Safety.Client.Define.Model.RelateUpdate.ControlPointLegal 搬到服务端。
        /// </summary>
        /// <param name="request"></param>
        /// <returns>(true：表示未使用，false：表示已使用)</returns>
        BasicResponse<bool> ControlPointLegal(PointDefineGetByPointIDRequest request);
        /// <summary>
        /// 判断控制口是否被用作风电闭锁或者甲烷风电闭锁控制口或者模开口是否在风电闭锁中使用(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>(true：表示未使用，false：表示已使用)</returns>
        BasicResponse<bool> ControlPointLegalAll(PointDefineGetByPointIDRequest request);

        BasicResponse<int> QueryRealLinkageInfoFromMonitor(GetRealLinkageInfoRequest request);
    } 
}

