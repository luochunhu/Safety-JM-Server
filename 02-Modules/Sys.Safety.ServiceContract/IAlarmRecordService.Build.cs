using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.ServiceContract
{
    public interface IAlarmRecordService
    {
        BasicResponse<Jc_BInfo> AddAlarmRecord(AlarmRecordAddRequest AlarmRecordRequest);
        BasicResponse<Jc_BInfo> UpdateAlarmRecord(AlarmRecordUpdateRequest AlarmRecordRequest);
        BasicResponse DeleteAlarmRecord(AlarmRecordDeleteRequest AlarmRecordRequest);
        BasicResponse<List<Jc_BInfo>> GetAlarmRecordList(AlarmRecordGetListRequest AlarmRecordRequest);
        BasicResponse<Jc_BInfo> GetAlarmRecordById(AlarmRecordGetRequest AlarmRecordRequest);
        /// <summary>
        /// 获取当前正在报警的数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_BInfo>> GetAlarmedDataList();

        BasicResponse BacthUpdateAlarmRecord(AlarmRecordBatchUpateRequesst AlarmRecordRequest);

        BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest AlarmRecordRequest);

        BasicResponse<List<AlarmProcessInfo>> GetStaionInterruptRecordListByStime(AlarmRecordGetByStimeRequest AlarmRecordRequest);

        /// <summary>
        /// 根据Id获取日期报警表记录
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_BInfo> GetDateAlarmRecordById(AlarmRecordGetDateIdRequest AlarmRecordRequest);

        /// <summary>
        /// 更新日期报警表记录
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> UpdateDateAlarmRecord(AlarmRecordUpdateDateRequest AlarmRecordRequest);

        /// <summary>
        /// 更新报警信息部分属性
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> UpdateAlarmInfoProperties(AlarmRecordUpdateProperitesRequest AlarmRecordRequest);

        /// <summary>
        /// 获取所有人员未结束设备报警信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_BInfo>> GetR_AlarmedDataList();
        /// <summary>
        /// 更新人员报警信息
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        BasicResponse BacthUpdateR_AlarmRecord(AlarmRecordBatchUpateRequesst AlarmRecordRequest);
    }
}

