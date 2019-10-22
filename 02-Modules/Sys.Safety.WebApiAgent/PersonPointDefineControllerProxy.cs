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
    public class PersonPointDefineControllerProxy : BaseProxy, IPersonPointDefineService
    {

        /// <summary>
        /// 添加测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefine(Sys.Safety.Request.PointDefine.PointDefineAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/Add?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefines(Sys.Safety.Request.PointDefine.PointDefinesAddRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/AddPointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefine(Sys.Safety.Request.PointDefine.PointDefineUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/Update?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefines(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/UpdatePointDefines?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse UpdatePointDefinesCache(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/UpdatePointDefinesCache?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
        /// </summary>
        /// <param name="PointDefineAddNetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(Sys.Safety.Request.PointDefine.PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/AddUpdatePointDefineAndNetworkModuleCache?token=" + Token, JSONHelper.ToJSONString(PointDefineAddNetworkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeletePointDefine(Sys.Safety.Request.PointDefine.PointDefineDeleteRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/Delete?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }



        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/BatchUpdatePointDefineInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/GetAllPointDefineCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse PointDefineSaveData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/PointDefineSaveData?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/GetPointDefineCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaID(PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/GetPointDefineCacheByAreaID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/GetPointDefineCacheByPointID?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }

        public BasicResponse OldPlsPointSync(OldPlsPointSyncRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/OldPlsPointSync?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse PlsPointSync(PlsPointSyncRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/PlsPointSync?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }




        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/PersonPointDefine/GetPointDefineCacheByPoint?token=" + Token, JSONHelper.ToJSONString(PointDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responseStr);
        }
    }
}
