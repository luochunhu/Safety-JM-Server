using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class CalibrationDefControllerProxy : BaseProxy, ICalibrationDefService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxInfo> AddCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxAddRequest jc_Bxrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationDef/AddCalibrationDef?token=" + Token, JSONHelper.ToJSONString(jc_Bxrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxInfo> UpdateCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxUpdateRequest jc_Bxrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationDef/UpdateCalibrationDef?token=" + Token, JSONHelper.ToJSONString(jc_Bxrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxDeleteRequest jc_Bxrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationDef/DeleteCalibrationDef?token=" + Token, JSONHelper.ToJSONString(jc_Bxrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_BxInfo>> GetCalibrationDefList(Sys.Safety.Request.Jc_Bx.Jc_BxGetListRequest jc_Bxrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationDef/GetCalibrationDefList?token=" + Token, JSONHelper.ToJSONString(jc_Bxrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_BxInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxInfo> GetCalibrationDefById(Sys.Safety.Request.Jc_Bx.Jc_BxGetRequest jc_Bxrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationDef/GetCalibrationDefById?token=" + Token, JSONHelper.ToJSONString(jc_Bxrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxInfo>>(responseStr);
        }
    }
}
