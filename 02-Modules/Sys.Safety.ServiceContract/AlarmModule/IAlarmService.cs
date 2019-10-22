using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Alarm;

namespace Sys.Safety.ServiceContract
{
    public interface IAlarmService
    {
        /// <summary>
        /// 服务端JC_R内存
        /// 服务端返回所有JC_R（经过关联服务端内存的定义后）内存记录给客户端，最多500条。
        /// 然后客户端根据自己的标记ID和报警配置进行报警展示
        /// 内存的def,dev,wz是最新的定义，无需自己再去判断defdatetime
        /// 1.客户端向服务端实时请求（或者是报警展示完后）最新的报警记录（报警记录是服务端已经于内存中关联好的）        
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<List<ShowDataInfo>> GetReleaseAlarmRecords(GetReleaseAlarmRecordsRequest alarmRequest);

        /// <summary>
        /// 获取最大的Id
        /// </summary>
        /// <returns></returns>
        BasicResponse<long> GetMaxId();

        /// <summary>
        /// 获取最大时间
        /// </summary>
        /// <returns></returns>
        BasicResponse<DateTime> GetMaxTimeFromJCR();

        BasicResponse<List<EnumcodeInfo>> GetListEnumPropert();

        /// <summary>
        /// 从服务端内存结构中获取所有设备种类
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetListEnumClass();

        /// <summary>
        /// 从服务端内存获取设备定义列表
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetListDef();

        /// <summary>
        /// 保存配置文件到数据库
        /// </summary>
        /// <returns></returns>
        BasicResponse<bool> SaveConfigToDatabase(SaveConfigToDatabaseRequest alarmRequest);

        /// <summary>
        /// 通过设备性质查找设备种类
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<Dictionary<int, EnumcodeInfo>> QueryDevClassByDevpropertId(QueryDevClassByDevpropertRequest alarmRequest);

        /// <summary>
        /// 通过设备种类查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> QueryPointByDevClassIDCache(QueryPointByDevClassRequest alarmRequest);

        /// <summary>
        /// 通过设备性质查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> QueryPointByDevpropertIDCache(QueryPointByDevpropertRequest alarmRequest);

        /// <summary>
        /// 根据EnumTypeId获取Enumcode
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetEnumcodeByEnumTypeId(GetEnumcodeByEnumTypeIdRequest alarmRequest);

        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetDataDefProperty();

        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetClassByProperty(GetClassByPropertyRequest alarmRequest);

        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetPointByClass(GetPointByClassRequest alarmRequest);

        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetAlarmTypeDataByProperty(GetAlarmTypeDataByPropertyRequest alarmRequest);

        /// <summary>
        /// 获取指定时间范围内的标校记录
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetCalibrationRecord(GetCalibrationRecordRequest alarmRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bxzt"></param>
        /// <param name="pointid"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        BasicResponse<int> UpdateCalibrationRecord(UpdateCalibrationRecordRequest request);

        BasicResponse InsertCalibrationRecord(InsertCalibrationRecordRequest request);
    }
}
