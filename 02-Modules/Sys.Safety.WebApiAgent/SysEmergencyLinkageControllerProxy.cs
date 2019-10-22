using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class SysEmergencyLinkageControllerProxy : BaseProxy, ISysEmergencyLinkageService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.SysEmergencyLinkageInfo> AddSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageAddRequest sysEmergencyLinkageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/AddSysEmergencyLinkage?token=" + Token, JSONHelper.ToJSONString(sysEmergencyLinkageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<SysEmergencyLinkageInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.SysEmergencyLinkageInfo> UpdateSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageUpdateRequest sysEmergencyLinkageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/UpdateSysEmergencyLinkage?token=" + Token, JSONHelper.ToJSONString(sysEmergencyLinkageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<SysEmergencyLinkageInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteSysEmergencyLinkage(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageDeleteRequest sysEmergencyLinkageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/DeleteSysEmergencyLinkage?token=" + Token, JSONHelper.ToJSONString(sysEmergencyLinkageRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.SysEmergencyLinkageInfo>> GetSysEmergencyLinkageList(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetListRequest sysEmergencyLinkageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetSysEmergencyLinkageList?token=" + Token, JSONHelper.ToJSONString(sysEmergencyLinkageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.SysEmergencyLinkageInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.SysEmergencyLinkageInfo> GetSysEmergencyLinkageById(Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest sysEmergencyLinkageRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetSysEmergencyLinkageById?token=" + Token, JSONHelper.ToJSONString(sysEmergencyLinkageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<SysEmergencyLinkageInfo>>(responseStr);
        }

        public BasicResponse AddEmergencylinkageconfigMasterInfoPassiveInfo(
            AddEmergencylinkageconfigMasterInfoPassiveInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/AddEmergencylinkageconfigMasterInfoPassiveInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<GetSysEmergencyLinkageListAndStatisticsResponse>> GetSysEmergencyLinkageListAndStatistics(StringRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetSysEmergencyLinkageListAndStatistics?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<GetSysEmergencyLinkageListAndStatisticsResponse>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetMasterPointInfoByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetMasterPointInfoByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<AreaInfo>> GetMasterAreaInfoByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetMasterAreaInfoByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DevInfo>> GetMasterEquTypeInfoByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetMasterEquTypeInfoByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }


        public BasicResponse<List<EnumcodeInfo>> GetMasterTriDataStateByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetMasterTriDataStateByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }

        public BasicResponse<List<R_PersoninfInfo>> GetPassivePersonByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetPassivePersonByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }

        public BasicResponse<List<IdTextCheck>> GetPassivePointInfoByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetPassivePointInfoByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<IdTextCheck>>>(responseStr);
        }

        public BasicResponse<List<AreaInfo>> GetPassiveAreaInfoByAssId(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetPassiveAreaInfoByAssId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaInfo>>>(responseStr);
        }

        public BasicResponse<List<IdTextCheck>> GetAllPassivePointInfo()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetAllPassivePointInfo?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<IdTextCheck>>>(responseStr);
        }


        public BasicResponse SoftDeleteSysEmergencyLinkageById(LongIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/SoftDeleteSysEmergencyLinkageById?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageList?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<SysEmergencyLinkageInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DefInfo>> GetAllMasterPointsById(SysEmergencyLinkageGetRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetAllMasterPointsById?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse UpdateRealTimeState(UpdateRealTimeStateRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/UpdateRealTimeState?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }


        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageInfo()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageInfo?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<SysEmergencyLinkageInfo>>>(responseStr);
        }


        BasicResponse ISysEmergencyLinkageService.UpdateSysEmergencyLinkage(SysEmergencyLinkageUpdateRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/UpdateSysEmergencyLinkage?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageListDb()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/SysEmergencyLinkage/GetAllSysEmergencyLinkageListDb?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<SysEmergencyLinkageInfo>>>(responseStr);
        }
    }
}
