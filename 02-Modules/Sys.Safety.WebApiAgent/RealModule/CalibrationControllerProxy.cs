using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Calibration;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class CalibrationControllerProxy : BaseProxy, ICalibrationService
    {
        public BasicResponse<DataTable> GetBxDetail(GetCalibrationDetailRequest calibrationRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/calibration/getbxdetail?token=" + Token, JSONHelper.ToJSONString(calibrationRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetBxStatistics(GetCalibrationStatisticsRequest calibrationRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/calibration/getbxstatistics?token=" + Token, JSONHelper.ToJSONString(calibrationRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }
    }
}
