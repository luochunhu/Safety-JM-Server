using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.ManualCrossControl;

namespace Sys.Safety.WebApi
{
    public class ManualCrossControlController : Basic.Framework.Web.WebApi.BasicApiController, IManualCrossControlService
    {
        static ManualCrossControlController()
        {

        }
        IManualCrossControlService _ManualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        /// <summary>
        /// 添加手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/ManualCrossControl/Add")]
        public BasicResponse AddManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlAddRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.AddManualCrossControl(ManualCrossControlRequest);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/ManualCrossControl/AddManualCrossControls")]
        public BasicResponse AddManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.AddManualCrossControls(ManualCrossControlRequest);
        }

        [HttpPost]
        [Route("v1/ManualCrossControl/Update")]
        public BasicResponse UpdateManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlUpdateRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.UpdateManualCrossControl(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/UpdateManualCrossControls")]
        public BasicResponse UpdateManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.UpdateManualCrossControls(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/Delete")]
        public BasicResponse DeleteManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlDeleteRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.DeleteManualCrossControl(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/DeleteManualCrossControls")]
        public BasicResponse DeleteManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.DeleteManualCrossControls(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/BatchOperationManualCrossControls")]
        public BasicResponse BatchOperationManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.BatchOperationManualCrossControls(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetPageList")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetListRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlList(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetList")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList()
        {
            return _ManualCrossControlService.GetManualCrossControlList();
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/Get")]
        public BasicResponse<Jc_JcsdkzInfo> GetManualCrossControlById(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlById(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetAllManualCrossControl")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControl()
        {
            return _ManualCrossControlService.GetAllManualCrossControl();
        }

        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlByStationID")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByStationID(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlByStationID(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlHandCtrlByStationID")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlHandCtrlByStationID(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlHandCtrlByStationID(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlByTypeBkPoint")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByTypeBkPointRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlByTypeBkPoint(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlByTypeZkPointBkPoint")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPointBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByTypeZkPointBkPointRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlByTypeZkPointBkPoint(ManualCrossControlRequest);
        }
        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlByBkPoint")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByBkPointRequest ManualCrossControlRequest)
        {
            return _ManualCrossControlService.GetManualCrossControlByBkPoint(ManualCrossControlRequest);
        }

        [HttpPost]
        [Route("v1/ManualCrossControl/GetManualCrossControlByTypeZkPoint")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPoint(ManualCrossControlGetByTypeZkPointRequest request)
        {
            return _ManualCrossControlService.GetManualCrossControlByTypeZkPoint(request);
        }

        [HttpPost]
        [Route("v1/ManualCrossControl/GetAllManualCrossControlDetail")]
        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlDetail()
        {
            return _ManualCrossControlService.GetAllManualCrossControlDetail();
        }

        [HttpPost]
        [Route("v1/ManualCrossControl/DeleteOtherManualCrossControlFromDB")]
        public BasicResponse DeleteOtherManualCrossControlFromDB()
        {
            throw new NotImplementedException();
        }
    }
}
