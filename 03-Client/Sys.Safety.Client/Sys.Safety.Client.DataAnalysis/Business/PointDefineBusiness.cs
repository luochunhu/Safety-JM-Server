using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class PointDefineBusiness
    {
        static IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>();
        /// <summary> 添加DEF 缓存
        /// </summary>
        /// <returns></returns>
        public static bool AddDEFCache(Jc_DefInfo item)
        {
            PointDefineAddRequest PointDefineRequest = new PointDefineAddRequest();
            PointDefineRequest.PointDefineInfo = item;
            var result = _PointDefineService.AddPointDefine(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 更新def
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateDEFCache(Jc_DefInfo item)
        {
            PointDefineUpdateRequest PointDefineRequest = new PointDefineUpdateRequest();
            PointDefineRequest.PointDefineInfo = item;
            var result = _PointDefineService.UpdatePointDefine(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>批量添加DEF 缓存
        /// </summary>
        /// <returns></returns>
        public static bool AddDEFsCache(List<Jc_DefInfo> items)
        {
            PointDefinesAddRequest PointDefineRequest = new PointDefinesAddRequest();
            PointDefineRequest.PointDefinesInfo = items;
            var result = _PointDefineService.AddPointDefines(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool UpdateDEFsCache(List<Jc_DefInfo> items)
        {
            PointDefinesUpdateRequest PointDefineRequest = new PointDefinesUpdateRequest();
            PointDefineRequest.PointDefinesInfo = items;
            var result = _PointDefineService.UpdatePointDefines(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>
        /// 查找所有测点信息
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryAllCache()
        {
            var result = _PointDefineService.GetAllPointDefineCache();
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>根据测点编号查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByPointID(string PointID)
        {
            PointDefineGetByPointIDRequest PointIDDefineRequest = new PointDefineGetByPointIDRequest();
            PointIDDefineRequest.PointID = PointID;
            var result = _PointDefineService.GetPointDefineCacheByPointID(PointIDDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data[0];
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>根据测点名称/位置查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public Jc_DefInfo QueryPointByWzCache(string wz)
        {
            PointDefineGetByWzRequest PointDefineRequest = new PointDefineGetByWzRequest();
            PointDefineRequest.Wz = wz;
            var result = _PointDefineService.GetPointDefineCacheByWz(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>查找MAC地址下的所有分站
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByMACCache(string mac)
        {
            PointDefineGetByMacRequest PointDefineRequest = new PointDefineGetByMacRequest();
            PointDefineRequest.Mac = mac;
            var result = _PointDefineService.GetPointDefineCacheByMac(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>查找COM下的所有分站
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByCOMCache(string com)
        {
            PointDefineGetByCOMRequest PointDefineRequest = new PointDefineGetByCOMRequest();
            PointDefineRequest.COM = com;
            var result = _PointDefineService.GetPointDefineCacheByCOM(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备性质查找测点
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevpropertIDCache(int DevPropertID)
        {
            PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            PointDefineRequest.DevpropertID = DevPropertID;
            var result = _PointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备种类查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevClassIDCache(int DevClassID)
        {
            PointDefineGetByDevClassIDRequest PointDefineRequest = new PointDefineGetByDevClassIDRequest();
            PointDefineRequest.DevClassID = DevClassID;
            var result = _PointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过设备型号查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevModelIDCache(int DevModelID)
        {
            PointDefineGetByDevModelIDRequest PointDefineRequest = new PointDefineGetByDevModelIDRequest();
            PointDefineRequest.DevModelID = DevModelID;
            var result = _PointDefineService.GetPointDefineCacheByDevModelID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过设备类型查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevIDCache(string DevID)
        {
            PointDefineGetByDevIDRequest PointDefineRequest = new PointDefineGetByDevIDRequest();
            PointDefineRequest.DevID = DevID;
            var result = _PointDefineService.GetPointDefineCacheByDevID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查找分站下的所有测点(包括分站)
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByFzhCache(int fzh)
        {
            PointDefineGetByStationIDRequest PointDefineRequest = new PointDefineGetByStationIDRequest();
            PointDefineRequest.StationID = fzh;
            var result = _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查询所有网络通讯的分站
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static IList<Jc_DefInfo> QueryStationOfNet()
        {
            var result = _PointDefineService.GetNetworkCommunicationStation();
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查询所有串口通讯的分站
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryStationOfCOM()
        {
            //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
            //return DEFService.QueryStationOfCOMCache();
            return new List<Jc_DefInfo>();//未实现  20170531
        }
        /// <summary> 按分站测点号查找分站下的所有测点
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByStationCache(string station)
        {
            PointDefineGetByStationPointRequest PointDefineRequest = new PointDefineGetByStationPointRequest();
            PointDefineRequest.StationPoint = station;
            var result = _PointDefineService.GetPointDefineCacheByStationPoint(PointDefineRequest);
            List<Jc_DefInfo> StationList = result.Data.FindAll(a => a.DevPropertyID == 0);

            PointDefineGetByStationIDRequest StationPointDefineRequest = new PointDefineGetByStationIDRequest();
            StationPointDefineRequest.StationID = StationList[0].Fzh;
            var resultStationPoint = _PointDefineService.GetPointDefineCacheByStationID(StationPointDefineRequest);
            if (resultStationPoint.IsSuccess == true)
            {
                return resultStationPoint.Data.FindAll(a => a.DevPropertyID != 0);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过分站号、口号、设备性质
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByInfs(int fzh, int kh, int DevPropertID)
        {
            PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            PointDefineRequest.DevPropertID = DevPropertID;
            var result = _PointDefineService.GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 通过分站号、设备性质 查找设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByInfs(int fzh, int DevPropertID)
        {
            PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.DevPropertID = DevPropertID;
            var result = _PointDefineService.GetPointDefineCacheByStationIDDevPropertID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过分站号、通道号 地址号查找设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByChannelInfs(int fzh, int kh, int dzh, int DevPropertID)
        {
            PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            PointDefineRequest.AddressID = dzh;
            PointDefineRequest.DevPropertID = DevPropertID;
            var result = _PointDefineService.GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                if (result.Data.Count > 0)
                {
                    return result.Data[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过分站号、口号查找设备多参数设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryMulitPramPointByChannel(int fzh, int kh)
        {
            PointDefineGetByStationIDChannelIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            var result = _PointDefineService.GetPointDefineCacheByStationIDChannelID(PointDefineRequest);
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 查找未通讯设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryStationNoComm()
        {
            var result = _PointDefineService.GetNonCommunicationStation();
            if (result.IsSuccess == true)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public static bool ControlPointLegal(string pointId)
        {
            Basic.Framework.Web.BasicResponse<bool> response = _PointDefineService.ControlPointLegal(new PointDefineGetByPointIDRequest() { PointID = pointId });
            if (response.IsSuccess)
                return response.Data;
            return true;
        }
    }
}
