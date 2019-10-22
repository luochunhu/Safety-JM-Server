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
    public class CalibrationStatisticsControllerProxy : BaseProxy, ICalibrationStatisticsService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxexInfo> AddCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexAddRequest jc_Bxexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationStatistics/AddCalibrationStatistics?token=" + Token, JSONHelper.ToJSONString(jc_Bxexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxexInfo> UpdateCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexUpdateRequest jc_Bxexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationStatistics/UpdateCalibrationStatistics?token=" + Token, JSONHelper.ToJSONString(jc_Bxexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexDeleteRequest jc_Bxexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationStatistics/DeleteCalibrationStatistics?token=" + Token, JSONHelper.ToJSONString(jc_Bxexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_BxexInfo>> GetCalibrationStatisticsList(Sys.Safety.Request.Jc_Bxex.Jc_BxexGetListRequest jc_Bxexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationStatistics/GetCalibrationStatisticsList?token=" + Token, JSONHelper.ToJSONString(jc_Bxexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.Jc_BxexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_BxexInfo> GetCalibrationStatisticsById(Sys.Safety.Request.Jc_Bxex.Jc_BxexGetRequest jc_Bxexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/CalibrationStatistics/GetCalibrationStatisticsById?token=" + Token, JSONHelper.ToJSONString(jc_Bxexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BxexInfo>>(responseStr);
        }
    }
}
