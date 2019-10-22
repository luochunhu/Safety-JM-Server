using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Model
{
    public interface IAlarmRepository
    {
        /// <summary>
        /// 根据EnumTypeId获取Enumcode
        /// </summary>
        /// <param name="enumTypeId"></param>
        /// <returns></returns>
        DataTable GetEnumcodeByEnumTypeId(string enumTypeId);

        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        DataTable GetDataDefProperty();

        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetClassByProperty(string type);

        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="sclass"></param>
        /// <returns></returns>
        DataTable GetPointByClass(string sclass);

        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        DataTable GetAlarmTypeDataByProperty(string code, string name);
        /// <summary>
        /// 获取指定时间内的标效记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable GetCalibrationRecord(DateTime startTime, DateTime endTime);
        /// <summary>
        /// 更新标校状态
        /// </summary>
        /// <param name="bxzt"></param>
        /// <param name="pointid"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        int UpdateCalibrationRecord(string id,string csStr);
    }
}
