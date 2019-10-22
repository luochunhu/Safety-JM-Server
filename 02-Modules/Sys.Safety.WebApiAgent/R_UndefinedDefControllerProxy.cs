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
    public class R_UndefinedDefControllerProxy : BaseProxy, IR_UndefinedDefService
    {



        public BasicResponse<List<R_UndefinedDefInfo>> GetAllRUndefinedDefCache(RUndefinedDefCacheGetAllRequest UndefinedDefRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_UndefinedDef/GetAllRUndefinedDefCache?token=" + Token, JSONHelper.ToJSONString(UndefinedDefRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_UndefinedDefInfo>>>(responseStr);
        }


        public BasicResponse<R_UndefinedDefInfo> AddUndefinedDef(R_UndefinedDefAddRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_UndefinedDefInfo> UpdateUndefinedDef(R_UndefinedDefUpdateRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse DeleteUndefinedDef(R_UndefinedDefDeleteRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_UndefinedDefInfo>> GetUndefinedDefList(R_UndefinedDefGetListRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_UndefinedDefInfo> GetUndefinedDefById(R_UndefinedDefGetRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }


       
    }
}
