using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class SetAnalysisModelPointRecordControllerProxy : BaseProxy, ISetAnalysisModelPointRecordService
    {
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> AddJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordAddRequest jC_Setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/AddJC_Setanalysismodelpointrecord?token=" + Token, JSONHelper.ToJSONString(jC_Setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_SetAnalysisModelPointRecordInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_Setanalysismodelpointrecord(SetanalysismodelpointrecordDeleteRequest jC_Setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/DeleteJC_Setanalysismodelpointrecord?token=" + Token, JSONHelper.ToJSONString(jC_Setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(SetAnalysisModelPointRecordGetTempleteRequest jc_SetAnalysisModelPointRecordGetTempleteRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId?token=" + Token, JSONHelper.ToJSONString(jc_SetAnalysisModelPointRecordGetTempleteRequest));
            return JSONHelper.ParseJSONString<BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }

        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetCustomizationAnalysisModelPointRecordInfoByModelId?token=" + Token, JSONHelper.ToJSONString(jc_SetAnalysisModelPointRecordByModelIdGetRequest));
            return JSONHelper.ParseJSONString<BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }

        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationEditAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetCustomizationEditAnalysisModelPointRecordInfoByModelId?token=" + Token, JSONHelper.ToJSONString(jc_SetAnalysisModelPointRecordByModelIdGetRequest));
            return JSONHelper.ParseJSONString<BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }

        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> GetJC_SetanalysismodelpointrecordById(SetAnalysisModelPointRecordGetRequest jC_Setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetJC_SetanalysismodelpointrecordById?token=" + Token, JSONHelper.ToJSONString(jC_Setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_SetAnalysisModelPointRecordInfo>>(responseStr);
        }

        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetJC_SetanalysismodelpointrecordList(SetAnalysisModelPointRecordGetListRequest jC_Setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetJC_SetanalysismodelpointrecordList?token=" + Token, JSONHelper.ToJSONString(jC_Setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }

        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> UpdateJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordUpdateRequest jC_Setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/UpdateJC_Setanalysismodelpointrecord?token=" + Token, JSONHelper.ToJSONString(jC_Setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_SetAnalysisModelPointRecordInfo>>(responseStr);
        }


        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId?token=" + Token, JSONHelper.ToJSONString(jc_SetAnalysisModelPointRecordByModelIdGetRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }


        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAnalysisModelPointRecordsByAnalysisModelId(SetAnalysisModelPointRecordByModelIdGetRequest setanalysismodelpointrecordrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetAnalysisModelPointRecordsByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(setanalysismodelpointrecordrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }



        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAllAnalysisModelPointList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisModelPointRecord/GetAllAnalysisModelPointList?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>>(responseStr);
        }
    }
}
