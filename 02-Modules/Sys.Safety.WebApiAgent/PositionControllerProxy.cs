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
using Sys.Safety.Request.Cache;

namespace Sys.Safety.WebApiAgent
{
    public class PositionControllerProxy : BaseProxy, IPositionService
    {
        /// <summary>
        ///添加安装位置
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPosition(PositionAddRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/Add?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPositions(PositionsRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/AddPositions?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 安装位置更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePosition(PositionUpdateRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/Update?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePositions(PositionsRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/UpdatePositions?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse DeletePosition(PositionDeleteRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/Delete?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<Jc_WzInfo> GetPositionById(PositionGetRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/Get?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_WzInfo>>(responseStr);
        }
        
        public BasicResponse<List<Jc_WzInfo>> GetPositionList(PositionGetListRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetPageList?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        }
        public BasicResponse<List<Jc_WzInfo>> GetPositionList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取所有安装位置缓存
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_WzInfo>> GetAllPositionCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetAllPositionCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        }        
        /// <summary>
        /// 获取缓存中当前最大的位置ID
        /// </summary>
        /// <returns></returns>
        public BasicResponse<long> GetMaxPositionId() {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetMaxPositionId?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<long>>(responseStr);
        }

        public BasicResponse<string> AddPositionBySql(AddPositionBySqlRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/AddPositionBySql?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }


        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWzID(PositionGetByWzIDRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetPositionCacheByWzID?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetPositionCacheByWz?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        }
    }
}
