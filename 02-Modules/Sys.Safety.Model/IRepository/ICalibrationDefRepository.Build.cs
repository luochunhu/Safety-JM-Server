using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ICalibrationDefRepository : IRepository<Jc_BxModel>
    {
        Jc_BxModel AddCalibrationDef(Jc_BxModel jc_BxModel);
        void UpdateCalibrationDef(Jc_BxModel jc_BxModel);
        void DeleteCalibrationDef(string id);
        IList<Jc_BxModel> GetCalibrationDefList(int pageIndex, int pageSize, out int rowCount);
        Jc_BxModel GetCalibrationDefById(string id);

        /// <summary>
        /// 根据时间删除jc_bx
        /// </summary>
        /// <param name="time"></param>
        void DeleteCalibrationDefByTime(DateTime time);
    }
}
