using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class AlarmRecordControllerProxy : BaseProxy, IAlarmRecordService
    {
        public BasicResponse<Jc_BInfo> AddAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordAddRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<Jc_BInfo> UpdateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordUpdateRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse DeleteAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordDeleteRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<Jc_BInfo>> GetAlarmRecordList(Sys.Safety.Request.Jc_B.AlarmRecordGetListRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<Jc_BInfo> GetAlarmRecordById(Sys.Safety.Request.Jc_B.AlarmRecordGetRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<Jc_BInfo>> GetAlarmedDataList()
        {
            throw new NotImplementedException();
        }

        public BasicResponse BacthUpdateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(Sys.Safety.Request.Jc_B.AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmRecord/GetAlarmRecordListByStime?token=" + Token, JSONHelper.ToJSONString(AlarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AlarmProcessInfo>>>(responseStr);
        }


        public BasicResponse<Jc_BInfo> GetDateAlarmRecordById(Sys.Safety.Request.Jc_B.AlarmRecordGetDateIdRequest AlarmRecordRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmRecord/GetDateAlarmRecordById?token=" + Token, JSONHelper.ToJSONString(AlarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_BInfo>>(responseStr);
        }

        public BasicResponse<bool> UpdateDateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordUpdateDateRequest AlarmRecordRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmRecord/UpdateDateAlarmRecord?token=" + Token, JSONHelper.ToJSONString(AlarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse<bool> UpdateAlarmInfoProperties(Sys.Safety.Request.Jc_B.AlarmRecordUpdateProperitesRequest AlarmRecordRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmRecord/UpdateAlarmInfoProperties?token=" + Token, JSONHelper.ToJSONString(AlarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }


        public BasicResponse<List<Jc_BInfo>> GetR_AlarmedDataList()
        {
            throw new NotImplementedException();
        }

        public BasicResponse BacthUpdateR_AlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }


        public BasicResponse<List<AlarmProcessInfo>> GetStaionInterruptRecordListByStime(Request.Jc_B.AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmRecord/GetStaionInterruptRecordListByStime?token=" + Token, JSONHelper.ToJSONString(AlarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AlarmProcessInfo>>>(responseStr);
        }
    }
}
