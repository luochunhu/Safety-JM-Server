using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class SetAnalysisModelPointRecordController : Basic.Framework.Web.WebApi.BasicApiController, ISetAnalysisModelPointRecordService
    {

        ISetAnalysisModelPointRecordService setAnalysisModelPointRecordService = ServiceFactory.Create<ISetAnalysisModelPointRecordService>();

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/AddJC_Setanalysismodelpointrecord")]
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> AddJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordAddRequest jC_Setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.AddJC_Setanalysismodelpointrecord(jC_Setanalysismodelpointrecordrequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/DeleteJC_Setanalysismodelpointrecord")]
        public BasicResponse DeleteJC_Setanalysismodelpointrecord(SetanalysismodelpointrecordDeleteRequest jC_Setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.DeleteJC_Setanalysismodelpointrecord(jC_Setanalysismodelpointrecordrequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId")]
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(SetAnalysisModelPointRecordGetTempleteRequest jc_SetAnalysisModelPointRecordGetTempleteRequest)
        {
            return setAnalysisModelPointRecordService.GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(jc_SetAnalysisModelPointRecordGetTempleteRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetCustomizationAnalysisModelPointRecordInfoByModelId")]
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            return setAnalysisModelPointRecordService.GetCustomizationAnalysisModelPointRecordInfoByModelId(jc_SetAnalysisModelPointRecordByModelIdGetRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetCustomizationEditAnalysisModelPointRecordInfoByModelId")]
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationEditAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            return setAnalysisModelPointRecordService.GetCustomizationEditAnalysisModelPointRecordInfoByModelId(jc_SetAnalysisModelPointRecordByModelIdGetRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetJC_SetanalysismodelpointrecordById")]
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> GetJC_SetanalysismodelpointrecordById(SetAnalysisModelPointRecordGetRequest jC_Setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.GetJC_SetanalysismodelpointrecordById(jC_Setanalysismodelpointrecordrequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetJC_SetanalysismodelpointrecordList")]
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetJC_SetanalysismodelpointrecordList(SetAnalysisModelPointRecordGetListRequest jC_Setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.GetJC_SetanalysismodelpointrecordList(jC_Setanalysismodelpointrecordrequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/UpdateJC_Setanalysismodelpointrecord")]
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> UpdateJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordUpdateRequest jC_Setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.UpdateJC_Setanalysismodelpointrecord(jC_Setanalysismodelpointrecordrequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId")]
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            return setAnalysisModelPointRecordService.GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(jc_SetAnalysisModelPointRecordByModelIdGetRequest);
        }

        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetAnalysisModelPointRecordsByAnalysisModelId")]
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAnalysisModelPointRecordsByAnalysisModelId(SetAnalysisModelPointRecordByModelIdGetRequest setanalysismodelpointrecordrequest)
        {
            return setAnalysisModelPointRecordService.GetAnalysisModelPointRecordsByAnalysisModelId(setanalysismodelpointrecordrequest);
        }
        [HttpPost]
        [Route("v1/AnalysisModelPointRecord/GetAllAnalysisModelPointList")]
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAllAnalysisModelPointList()
        {
            return setAnalysisModelPointRecordService.GetAllAnalysisModelPointList();
        }

    }
}
