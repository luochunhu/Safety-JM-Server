using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AlarmRecordController : Basic.Framework.Web.WebApi.BasicApiController, IAlarmRecordService
    {
        IAlarmRecordService alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BInfo> AddAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordAddRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BInfo> UpdateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordUpdateRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeleteAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordDeleteRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/AlarmRecord/GetAlarmRecordList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_BInfo>> GetAlarmRecordList(Sys.Safety.Request.Jc_B.AlarmRecordGetListRequest AlarmRecordRequest)
        {
            return alarmRecordService.GetAlarmRecordList(AlarmRecordRequest);
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BInfo> GetAlarmRecordById(Sys.Safety.Request.Jc_B.AlarmRecordGetRequest AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_BInfo>> GetAlarmedDataList()
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse BacthUpdateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/AlarmRecord/GetAlarmRecordListByStime")]
        public Basic.Framework.Web.BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(Sys.Safety.Request.Jc_B.AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            return alarmRecordService.GetAlarmRecordListByStime(AlarmRecordRequest);
        }

        [HttpPost]
        [Route("v1/AlarmRecord/GetDateAlarmRecordById")]
        public Basic.Framework.Web.BasicResponse<Jc_BInfo> GetDateAlarmRecordById(Sys.Safety.Request.Jc_B.AlarmRecordGetDateIdRequest AlarmRecordRequest)
        {
            return alarmRecordService.GetDateAlarmRecordById(AlarmRecordRequest);
        }

        [HttpPost]
        [Route("v1/AlarmRecord/UpdateDateAlarmRecord")]
        public Basic.Framework.Web.BasicResponse<bool> UpdateDateAlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordUpdateDateRequest AlarmRecordRequest)
        {
            return alarmRecordService.UpdateDateAlarmRecord(AlarmRecordRequest);
        }

        [HttpPost]
        [Route("v1/AlarmRecord/UpdateAlarmInfoProperties")]
        public Basic.Framework.Web.BasicResponse<bool> UpdateAlarmInfoProperties(Sys.Safety.Request.Jc_B.AlarmRecordUpdateProperitesRequest AlarmRecordRequest)
        {
            return alarmRecordService.UpdateAlarmInfoProperties(AlarmRecordRequest);
        }


        public Basic.Framework.Web.BasicResponse<List<Jc_BInfo>> GetR_AlarmedDataList()
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse BacthUpdateR_AlarmRecord(Sys.Safety.Request.Jc_B.AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            throw new NotImplementedException();
        }


        public Basic.Framework.Web.BasicResponse<List<AlarmProcessInfo>> GetStaionInterruptRecordListByStime(Request.Jc_B.AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            return alarmRecordService.GetAlarmRecordListByStime(AlarmRecordRequest);
        }
    }
}
