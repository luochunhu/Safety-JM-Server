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
    public class AllSystemPointDefineController : Basic.Framework.Web.WebApi.BasicApiController, IAllSystemPointDefineService
    {
        static AllSystemPointDefineController()
        {

        }
        IAllSystemPointDefineService _PointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();   
       
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetAllPointDefineCache")]
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
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByPoint")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPoint(PointDefineRequest);
        }
        
        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetNetworkCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation()
        {
            return _PointDefineService.GetNetworkCommunicationStation();
        }
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetCOMCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation()
        {
            return _PointDefineService.GetCOMCommunicationStation();
        }
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetNonCommunicationStation")]
        public BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation()
        {
            return _PointDefineService.GetNonCommunicationStation();
        }
        

        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByWz")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(PointDefineGetByWzRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByWz(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByMac")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(PointDefineGetByMacRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByMac(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByCOM")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(PointDefineGetByCOMRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByCOM(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByDevpropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByDevClassID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByDevModelID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(PointDefineGetByDevModelIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevModelID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByDevID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(PointDefineGetByDevIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(PointDefineGetByStationIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationPoint")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(PointDefineGetByStationPointRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationPoint(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(PointDefineGetByStationIDChannelIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStationIDChannelID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByAreaId")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(PointDefineGetByAreaIdRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaId(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByAreaCode")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(PointDefineGetByAreaCodeRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaCode(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByAreaCodeDevPropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaCodeDevPropertID(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByStrKeywords")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(PointDefineGetByStrKeywordsRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByStrKeywords(PointDefineRequest);
        }
        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheByPointID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest);
        }



        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest pointDefineCacheRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/AllSystemPointDefine/GetPointDefineCacheBySwitch")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySwitch(PointDefineGetByMacRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheBySwitch(PointDefineRequest);
        }
    }
}
