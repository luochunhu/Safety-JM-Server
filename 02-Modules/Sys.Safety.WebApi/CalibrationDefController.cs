using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class CalibrationDefController : BasicApiController, ICalibrationDefService
    {
        ICalibrationDefService _calibrationDefService = ServiceFactory.Create<ICalibrationDefService>();

        [HttpPost]
        [Route("v1/CalibrationDef/AddCalibrationDef")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxInfo> AddCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxAddRequest jc_Bxrequest)
        {
            return _calibrationDefService.AddCalibrationDef(jc_Bxrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationDef/UpdateCalibrationDef")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxInfo> UpdateCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxUpdateRequest jc_Bxrequest)
        {
            return _calibrationDefService.UpdateCalibrationDef(jc_Bxrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationDef/DeleteCalibrationDef")]
        public Basic.Framework.Web.BasicResponse DeleteCalibrationDef(Sys.Safety.Request.Jc_Bx.Jc_BxDeleteRequest jc_Bxrequest)
        {
            return _calibrationDefService.DeleteCalibrationDef(jc_Bxrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationDef/GetCalibrationDefList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_BxInfo>> GetCalibrationDefList(Sys.Safety.Request.Jc_Bx.Jc_BxGetListRequest jc_Bxrequest)
        {
            return _calibrationDefService.GetCalibrationDefList(jc_Bxrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationDef/GetCalibrationDefById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxInfo> GetCalibrationDefById(Sys.Safety.Request.Jc_Bx.Jc_BxGetRequest jc_Bxrequest)
        {
            return _calibrationDefService.GetCalibrationDefById(jc_Bxrequest);
        }
    }
}
