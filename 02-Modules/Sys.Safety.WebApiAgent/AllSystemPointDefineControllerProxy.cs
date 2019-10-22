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
    public class AllSystemPointDefineControllerProxy : BaseProxy, IAllSystemPointDefineService
    {
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetAllPointDefineCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据测点号查找缓存信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(Sys.Safety.Request.PointDefine.PointDefineGetByPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        public BasicResponse<Jc_DefInfo> GetNotMonitoringPointDefineCacheByPoint(StringRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetNotMonitoringPointDefineCacheByPoint?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByDynamicCondition?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetCOMCommunicationStation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetNonCommunicationStation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
        /// <summary>
        /// 测点定义保存巡检操作
        /// </summary>
        /// <returns></returns>
        public BasicResponse PointDefineSaveData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/PointDefineSaveData?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse SendDCom(SendDComReqest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/SendDCom?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(Sys.Safety.Request.PointDefine.PointDefineGetByWzRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByWz?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(Sys.Safety.Request.PointDefine.PointDefineGetByMacRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByMac?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(Sys.Safety.Request.PointDefine.PointDefineGetByCOMRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByCOM?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(Sys.Safety.Request.PointDefine.PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(Sys.Safety.Request.PointDefine.PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByDevClassID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(Sys.Safety.Request.PointDefine.PointDefineGetByDevModelIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByDevModelID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(Sys.Safety.Request.PointDefine.PointDefineGetByDevIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByDevID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(Sys.Safety.Request.PointDefine.PointDefineGetByStationPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(Sys.Safety.Request.PointDefine.PointDefineGetByStationIDChannelIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStationIDChannelID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(Sys.Safety.Request.PointDefine.PointDefineGetByAreaIdRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByAreaId?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(Sys.Safety.Request.PointDefine.PointDefineGetByAreaCodeRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByAreaCode?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(Sys.Safety.Request.PointDefine.PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByAreaCodeDevPropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(Sys.Safety.Request.PointDefine.PointDefineGetByStrKeywordsRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByStrKeywords?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheByPointID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }



        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest pointDefineCacheRequest)
        {
            throw new NotImplementedException();
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySwitch(PointDefineGetByMacRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AllSystemPointDefine/GetPointDefineCacheBySwitch?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }
    }
}
