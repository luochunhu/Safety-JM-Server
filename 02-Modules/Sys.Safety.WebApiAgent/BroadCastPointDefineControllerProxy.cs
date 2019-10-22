using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class BroadCastPointDefineControllerProxy : BaseProxy, IBroadCastPointDefineService
    {
        public Basic.Framework.Web.BasicResponse AddPointDefine(Sys.Safety.Request.PointDefine.PointDefineAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/AddPointDefine?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse AddPointDefines(Sys.Safety.Request.PointDefine.PointDefinesAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/AddPointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse UpdatePointDefine(Sys.Safety.Request.PointDefine.PointDefineUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/UpdatePointDefine?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse UpdatePointDefines(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/UpdatePointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse UpdatePointDefinesCache(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/UpdatePointDefinesCache?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse BatchUpdatePointDefineInfo(Sys.Safety.Request.Cache.DefineCacheBatchUpdatePropertiesRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/BatchUpdatePointDefineInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeletePointDefine(Sys.Safety.Request.PointDefine.PointDefineDeleteRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/DeletePointDefine?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> GetAllPointDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/GetAllPointDefineCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_DefInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse PointDefineSaveData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/PointDefineSaveData?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_DefInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> GetPointDefineCacheByAreaID(Sys.Safety.Request.PointDefine.PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/GetPointDefineCacheByAreaID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_DefInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefInfo> GetPointDefineCacheByPointID(Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/GetPointDefineCacheByPointID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.Jc_DefInfo>>(responseStr);
        }


        public BasicResponse<bool> SynchronousPoint(Request.PointDefine.SynchronousPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/SynchronousPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }



        public BasicResponse BroadcastSysPointSync(Request.PointDefine.BroadcastSysPointSyncRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/BroadCastPointDefine/BroadcastSysPointSync?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }
    }
}
