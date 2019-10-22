using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class AreaControllerProxy : BaseProxy, IAreaService
    {
        public BasicResponse<AreaInfo> AddArea(Sys.Safety.Request.Area.AreaAddRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/AddArea?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaInfo>>(responseStr);
        }

        public BasicResponse<AreaInfo> UpdateArea(Sys.Safety.Request.Area.AreaUpdateRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/UpdateArea?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaInfo>>(responseStr);
        }

        public BasicResponse DeleteArea(Sys.Safety.Request.Area.AreaDeleteRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/DeleteArea?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaInfo>>(responseStr);
        }

        public BasicResponse<List<AreaInfo>> GetAreaList(Sys.Safety.Request.Area.AreaGetListRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/GetAreaList?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaInfo>>>(responseStr);
        }

        public BasicResponse<AreaInfo> GetAreaById(Sys.Safety.Request.Area.AreaGetRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/GetAreaById?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaInfo>>(responseStr);
        }

        public BasicResponse<List<AreaInfo>> GetAllAreaList(Sys.Safety.Request.Area.AreaGetListRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/GetAllAreaList?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaInfo>>>(responseStr);
        }


        public BasicResponse<List<AreaInfo>> GetAllAreaCache(Sys.Safety.Request.PersonCache.AreaCacheGetAllRequest arearequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Area/GetAllAreaCache?token=" + Token, JSONHelper.ToJSONString(arearequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaInfo>>>(responseStr);
        }
    }
}
