using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class V_DefControllerProxy : BaseProxy, IV_DefService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.V_DefInfo> AddDef(Sys.Safety.Request.Def.DefAddRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/AddDef?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<V_DefInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.V_DefInfo> UpdateDef(Sys.Safety.Request.Def.DefUpdateRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/UpdateDef?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<V_DefInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteDef(Sys.Safety.Request.Def.DefDeleteRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/DeleteDef?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<V_DefInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.V_DefInfo>> GetDefList(Sys.Safety.Request.Def.DefGetListRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/GetDefList?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<V_DefInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.V_DefInfo> GetDefById(Sys.Safety.Request.Def.DefGetRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/GetDefById?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<V_DefInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.V_DefInfo>> GetAllDef(Sys.Safety.Request.Def.DefGetAllRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/GetAllDef?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<V_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<V_DefInfo>> GetAllVideoDefCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/GetAllVideoDefCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<V_DefInfo>>>(responseStr);
        }


        public BasicResponse<V_DefInfo> GetDefByIP(Sys.Safety.Request.Def.DefIPRequest defRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/V_Def/GetDefByIP?token=" + Token, JSONHelper.ToJSONString(defRequest));
            return JSONHelper.ParseJSONString<BasicResponse<V_DefInfo>>(responseStr);
        }
    }
}
