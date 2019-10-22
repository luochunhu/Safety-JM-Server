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
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    /// <summary>
    /// 测点定义服务(所有系统)
    /// </summary>
    public interface IAllSystemPointDefineService
    {      
       
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache();
         /// <summary>
        /// 根据条件查询缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest pointDefineCacheRequest);

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
        ///  根据交换机MAC地址查询MAC地址对应的分站信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySwitch(PointDefineGetByMacRequest PointDefineRequest);
        /// <summary>
        ///  查找COM下的所有分站
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(PointDefineGetByCOMRequest PointDefineRequest);
        /// <summary>
        ///通过设备性质查找监控系统测点
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
        
    } 
}

