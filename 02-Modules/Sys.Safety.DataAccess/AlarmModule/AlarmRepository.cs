using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Model;
using Basic.Framework.Data;

namespace Sys.Safety.DataAccess
{
    public class AlarmRepository : RepositoryBase<AlarmRepository>, IAlarmRepository
    {
        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetAlarmTypeDataByProperty(string code, string name)
        {
            return base.QueryTable("global_RealModule_GetAlarmTypeData_ByProperty", code, name);
        }

        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetClassByProperty(string type)
        {
            return base.QueryTable("global_RealModule_GetClassByProperty_ByType", type);
        }

        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataDefProperty()
        {
            return base.QueryTable("global_RealModule_GetDataDefProperty");
        }

        /// <summary>
        /// 根据EnumTypeId获取Enumcode
        /// </summary>
        /// <param name="enumTypeId"></param>
        /// <returns></returns>
        public DataTable GetEnumcodeByEnumTypeId(string enumTypeId)
        {
            return base.QueryTable("global_RealModule_GetEnumcode_ByPointId", enumTypeId);
        }

        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="sclass"></param>
        /// <returns></returns>
        public DataTable GetPointByClass(string sclass)
        {
            return base.QueryTable("global_RealModule_GetPoint_ByClass", sclass);
        }
        /// <summary>
        /// 获取指定时间内的标效记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetCalibrationRecord(DateTime startTime, DateTime endTime)
        {
            return base.QueryTable("global_GetCalibrationRecordByTimer", startTime, endTime);
        }

        /// <summary>
        /// 更新标校状态
        /// </summary>
        /// <param name="bxzt"></param>
        /// <param name="pointid"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        public int UpdateCalibrationRecord(string id,string csStr)
        {
            return base.ExecuteNonQuery("global_UpdateCalibrationRecordByTimer", 2,csStr, id);
        }
    }
}
