using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Alarm;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;
using Basic.Framework.Common;

namespace Sys.Safety.WebApi.AlarmModule
{
    public class AlarmServiceController : Basic.Framework.Web.WebApi.BasicApiController, IAlarmService
    {
        private IAlarmService alarmService = ServiceFactory.Create<IAlarmService>();

        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getalarmtypedatabyproperty")]
        public BasicResponse<DataTable> GetAlarmTypeDataByProperty(GetAlarmTypeDataByPropertyRequest alarmRequest)
        {
            return alarmService.GetAlarmTypeDataByProperty(alarmRequest);
        }

        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getclassbyproperty")]
        public BasicResponse<DataTable> GetClassByProperty(GetClassByPropertyRequest alarmRequest)
        {
            return alarmService.GetClassByProperty(alarmRequest);
        }

        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getdatadefproperty")]
        public BasicResponse<DataTable> GetDataDefProperty()
        {
            return alarmService.GetDataDefProperty();
        }

        /// <summary>
        /// 根据EnumTypeId获取Enumcode
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getenumcode")]
        public BasicResponse<DataTable> GetEnumcodeByEnumTypeId(GetEnumcodeByEnumTypeIdRequest alarmRequest)
        {
            return alarmService.GetEnumcodeByEnumTypeId(alarmRequest);
        }

        /// <summary>
        /// 从服务端内存获取设备定义列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getlistdef")]
        public BasicResponse<List<Jc_DefInfo>> GetListDef()
        {
            return alarmService.GetListDef();
        }

        /// <summary>
        /// 从服务端内存结构中获取所有设备种类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getlistenumclass")]
        public BasicResponse<List<EnumcodeInfo>> GetListEnumClass()
        {
            return alarmService.GetListEnumClass();
        }


        [HttpPost]
        [Route("v1/alarm/getlistenumpropert")]
        public BasicResponse<List<EnumcodeInfo>> GetListEnumPropert()
        {
            return alarmService.GetListEnumPropert();
        }

        /// <summary>
        ///获取最大Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getmaxid")]
        public BasicResponse<long> GetMaxId()
        {
            return alarmService.GetMaxId();
        }

        /// <summary>
        /// 获取最大时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getmaxtimefromjcr")]
        public BasicResponse<DateTime> GetMaxTimeFromJCR()
        {
            return alarmService.GetMaxTimeFromJCR();
        }

        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/getpointbyclass")]
        public BasicResponse<DataTable> GetPointByClass(GetPointByClassRequest alarmRequest)
        {
            return alarmService.GetPointByClass(alarmRequest);
        }

        [HttpPost]
        [Route("v1/alarm/getreleasealarmrecords")]
        public BasicResponse<List<ShowDataInfo>> GetReleaseAlarmRecords(GetReleaseAlarmRecordsRequest alarmRequest)
        {
            return alarmService.GetReleaseAlarmRecords(alarmRequest);
        }

        /// <summary>
        /// 通过设备性质查找设备种类
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/querydevclassbydevpropert")]
        public BasicResponse<Dictionary<int, EnumcodeInfo>> QueryDevClassByDevpropertId(QueryDevClassByDevpropertRequest alarmRequest)
        {
            return alarmService.QueryDevClassByDevpropertId(alarmRequest);
        }

        /// <summary>
        /// 通过设备种类查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/querypointbydevclassidcache")]
        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevClassIDCache(QueryPointByDevClassRequest alarmRequest)
        {
            return alarmService.QueryPointByDevClassIDCache(alarmRequest);
        }

        /// <summary>
        /// 通过设备性质查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/querypointbydevpropertidcache")]
        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevpropertIDCache(QueryPointByDevpropertRequest alarmRequest)
        {
            return alarmService.QueryPointByDevpropertIDCache(alarmRequest);
        }

        /// <summary>
        ///保存配置文件到数据库
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/alarm/saveconfigtodatabase")]
        public BasicResponse<bool> SaveConfigToDatabase(SaveConfigToDatabaseRequest alarmRequest)
        {
            return alarmService.SaveConfigToDatabase(alarmRequest);
        }
        
        [HttpPost]
        [Route("v1/alarm/GetCalibrationRecord")]
        public BasicResponse<string> GetCalibrationRecord(GetCalibrationRecordRequest alarmRequest)
        {
            var res= alarmService.GetCalibrationRecord(alarmRequest);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            }; 
        }


        BasicResponse<DataTable> IAlarmService.GetCalibrationRecord(GetCalibrationRecordRequest alarmRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/alarm/UpdateCalibrationRecord")]
        public BasicResponse<int> UpdateCalibrationRecord(UpdateCalibrationRecordRequest request)
        {
            return alarmService.UpdateCalibrationRecord(request);
        }

        [HttpPost]
        [Route("v1/alarm/InsertCalibrationRecord")]
        public BasicResponse InsertCalibrationRecord(InsertCalibrationRecordRequest request)
        {
            return alarmService.InsertCalibrationRecord(request);
        }
    }
}
