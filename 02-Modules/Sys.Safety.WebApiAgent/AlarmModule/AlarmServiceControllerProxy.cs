using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Alarm;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;


namespace Sys.Safety.WebApiAgent
{
    public class AlarmServiceControllerProxy : BaseProxy, IAlarmService
    {
        public BasicResponse<DataTable> GetAlarmTypeDataByProperty(GetAlarmTypeDataByPropertyRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getalarmtypedatabyproperty?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetClassByProperty(GetClassByPropertyRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getclassbyproperty?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetDataDefProperty()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getdatadefproperty?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetEnumcodeByEnumTypeId(GetEnumcodeByEnumTypeIdRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getenumcode?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetListDef()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getlistdef?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responsestr);
        }

        public BasicResponse<List<EnumcodeInfo>> GetListEnumClass()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getlistenumclass?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responsestr);
        }

        public BasicResponse<List<EnumcodeInfo>> GetListEnumPropert()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getlistenumpropert?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responsestr);
        }

        public BasicResponse<long> GetMaxId()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getmaxid?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<long>>(responsestr);
        }

        public BasicResponse<DateTime> GetMaxTimeFromJCR()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getmaxtimefromjcr?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DateTime>>(responsestr);
        }

        public BasicResponse<DataTable> GetPointByClass(GetPointByClassRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "v1/alarm/getpointbyclass?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<List<ShowDataInfo>> GetReleaseAlarmRecords(GetReleaseAlarmRecordsRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/getreleasealarmrecords?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<ShowDataInfo>>>(responsestr);
        }

        public BasicResponse<Dictionary<int, EnumcodeInfo>> QueryDevClassByDevpropertId(QueryDevClassByDevpropertRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/querydevclassbydevpropert?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Dictionary<int, EnumcodeInfo>>>(responsestr);
        }

        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevClassIDCache(QueryPointByDevClassRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/querypointbydevclassidcache?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responsestr);
        }

        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevpropertIDCache(QueryPointByDevpropertRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/querypointbydevpropertidcache?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responsestr);
        }

        public BasicResponse<bool> SaveConfigToDatabase(SaveConfigToDatabaseRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/saveconfigtodatabase?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }


        public BasicResponse<DataTable> GetCalibrationRecord(GetCalibrationRecordRequest alarmRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/GetCalibrationRecord?token=" + Token, JSONHelper.ToJSONString(alarmRequest));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }


        public BasicResponse<int> UpdateCalibrationRecord(UpdateCalibrationRecordRequest request)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/UpdateCalibrationRecord?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responsestr);
        }


        public BasicResponse InsertCalibrationRecord(InsertCalibrationRecordRequest request)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/alarm/InsertCalibrationRecord?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responsestr);
        }
    }
}
