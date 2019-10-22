using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class R_PrealControllerProxy : BaseProxy, IR_PrealService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.R_PrealInfo> AddPreal(Sys.Safety.Request.R_Preal.R_PrealAddRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/AddPreal?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PrealInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PrealInfo> UpdatePreal(Sys.Safety.Request.R_Preal.R_PrealUpdateRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/UpdatePreal?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PrealInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeletePreal(Sys.Safety.Request.R_Preal.R_PrealDeleteRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/DeletePreal?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PrealInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PrealInfo>> GetPrealList(Sys.Safety.Request.R_Preal.R_PrealGetListRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/GetPrealList?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PrealInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PrealInfo> GetPrealById(Sys.Safety.Request.R_Preal.R_PrealGetRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/GetPrealById?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PrealInfo>>(responseStr);
        }


        public BasicResponse<List<R_PrealInfo>> GetAllPrealCacheList(RPrealCacheGetAllRequest PrealRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/GetAllPrealCacheList?token=" + Token, JSONHelper.ToJSONString(PrealRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PrealInfo>>>(responseStr);
        }
        
        public BasicResponse<List<R_PrealInfo>> GetAllAlarmPrealCacheList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/GetAllAlarmPrealCacheList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PrealInfo>>>(responseStr);
        }

        public BasicResponse OldPlsPersonRealSync(OldPlsPersonRealSyncRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Preal/OldPlsPersonRealSync?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
