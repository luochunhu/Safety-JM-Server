using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.UndefinedDef;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class R_KqbcControllerProxy : BaseProxy, IR_KqbcService
    {




        public BasicResponse<R_KqbcInfo> AddKqbc(Sys.Safety.Request.Kqbc.R_KqbcAddRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_KqbcInfo> UpdateKqbc(Sys.Safety.Request.Kqbc.R_KqbcUpdateRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse DeleteKqbc(Sys.Safety.Request.Kqbc.R_KqbcDeleteRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_KqbcInfo>> GetKqbcList(Sys.Safety.Request.Kqbc.R_KqbcGetListRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_KqbcInfo>> GetAllKqbcList()
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_KqbcInfo> GetKqbcById(Sys.Safety.Request.Kqbc.R_KqbcGetRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_KqbcInfo>> GetAllKqbcCacheList(RKqbcCacheGetAllRequest RKqbcCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Kqbc/GetAllKqbcCacheList?token=" + Token, JSONHelper.ToJSONString(RKqbcCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_KqbcInfo>>>(responseStr);
        }


        public BasicResponse<R_KqbcInfo> GetDefaultKqbcCache(RKqbcCacheGetByConditionRequest RKqbcCacheRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Kqbc/GetDefaultKqbcCache?token=" + Token, JSONHelper.ToJSONString(RKqbcCacheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_KqbcInfo>>(responseStr);
        }
    }
}
