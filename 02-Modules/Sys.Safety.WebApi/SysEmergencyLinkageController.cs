using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class SysEmergencyLinkageController :BasicApiController, ISysEmergencyLinkageService
    {
        private ISysEmergencyLinkageService _sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();

        //[HttpPost]
        //[Route("v1/SysEmergencyLinkage/AddSysEmergencyLinkage")]
        //public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.SysEmergencyLinkageInfo> AddSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageAddRequest sysEmergencyLinkageRequest)
        //{
        //    return _sysEmergencyLinkageService.AddSysEmergencyLinkage(sysEmergencyLinkageRequest);
        //}

        //[HttpPost]
        //[Route("v1/SysEmergencyLinkage/UpdateSysEmergencyLinkage")]
        //public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.SysEmergencyLinkageInfo> UpdateSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageUpdateRequest sysEmergencyLinkageRequest)
        //{
        //    return _sysEmergencyLinkageService.UpdateSysEmergencyLinkage(sysEmergencyLinkageRequest);
        //}

        //[HttpPost]
        //[Route("v1/SysEmergencyLinkage/DeleteSysEmergencyLinkage")]
        //public Basic.Framework.Web.BasicResponse DeleteSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageDeleteRequest sysEmergencyLinkageRequest)
        //{
        //    return _sysEmergencyLinkageService.DeleteSysEmergencyLinkage(sysEmergencyLinkageRequest);
        //}

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetSysEmergencyLinkageList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.SysEmergencyLinkageInfo>> GetSysEmergencyLinkageList(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetListRequest sysEmergencyLinkageRequest)
        {
            return _sysEmergencyLinkageService.GetSysEmergencyLinkageList(sysEmergencyLinkageRequest);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetSysEmergencyLinkageById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.SysEmergencyLinkageInfo> GetSysEmergencyLinkageById(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest sysEmergencyLinkageRequest)
        {
            return _sysEmergencyLinkageService.GetSysEmergencyLinkageById(sysEmergencyLinkageRequest);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/AddEmergencylinkageconfigMasterInfoPassiveInfo")]
        public Basic.Framework.Web.BasicResponse AddEmergencylinkageconfigMasterInfoPassiveInfo(Sys.Safety.Request.SysEmergencyLinkage.AddEmergencylinkageconfigMasterInfoPassiveInfoRequest request)
        {
            return _sysEmergencyLinkageService.AddEmergencylinkageconfigMasterInfoPassiveInfo(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetSysEmergencyLinkageListAndStatistics")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.Request.SysEmergencyLinkage.GetSysEmergencyLinkageListAndStatisticsResponse>> GetSysEmergencyLinkageListAndStatistics(Sys.Safety.Request.Listex.StringRequest request)
        {
            return _sysEmergencyLinkageService.GetSysEmergencyLinkageListAndStatistics(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetMasterPointInfoByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefInfo>> GetMasterPointInfoByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetMasterPointInfoByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetMasterAreaInfoByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.AreaInfo>> GetMasterAreaInfoByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetMasterAreaInfoByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetMasterEquTypeInfoByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DevInfo>> GetMasterEquTypeInfoByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetMasterEquTypeInfoByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetMasterTriDataStateByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.EnumcodeInfo>> GetMasterTriDataStateByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetMasterTriDataStateByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetPassivePersonByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PersoninfInfo>> GetPassivePersonByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetPassivePersonByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetPassivePointInfoByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.Request.SysEmergencyLinkage.IdTextCheck>> GetPassivePointInfoByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetPassivePointInfoByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetPassiveAreaInfoByAssId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.AreaInfo>> GetPassiveAreaInfoByAssId(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.GetPassiveAreaInfoByAssId(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetAllPassivePointInfo")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.Request.SysEmergencyLinkage.IdTextCheck>> GetAllPassivePointInfo()
        {
            return _sysEmergencyLinkageService.GetAllPassivePointInfo();
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/SoftDeleteSysEmergencyLinkageById")]
        public Basic.Framework.Web.BasicResponse SoftDeleteSysEmergencyLinkageById(Sys.Safety.Request.Listex.LongIdRequest request)
        {
            return _sysEmergencyLinkageService.SoftDeleteSysEmergencyLinkageById(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageList()
        {
            return _sysEmergencyLinkageService.GetAllSysEmergencyLinkageList();
        }


        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetAllMasterPointsById")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefInfo>> GetAllMasterPointsById(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest request)
        {

            return _sysEmergencyLinkageService.GetAllMasterPointsById(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/UpdateRealTimeState")]
        public Basic.Framework.Web.BasicResponse UpdateRealTimeState(Sys.Safety.Request.SysEmergencyLinkage.UpdateRealTimeStateRequest request)
        {
            return _sysEmergencyLinkageService.UpdateRealTimeState(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageInfo")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageInfo()
        {
            return _sysEmergencyLinkageService.GetAllSysEmergencyLinkageInfo();
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/UpdateSysEmergencyLinkage")]
        public Basic.Framework.Web.BasicResponse UpdateSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageUpdateRequest request)
        {
            return _sysEmergencyLinkageService.UpdateSysEmergencyLinkage(request);
        }

        [HttpPost]
        [Route("v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageListDb")]
        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageListDb()
        {
            return _sysEmergencyLinkageService.GetAllSysEmergencyLinkageListDb();
        }
    }
}
