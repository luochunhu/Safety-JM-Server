using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ICalibrationStatisticsRepository : IRepository<Jc_BxexModel>
    {
        Jc_BxexModel AddCalibrationStatistics(Jc_BxexModel jc_BxexModel);
        void UpdateCalibrationStatistics(Jc_BxexModel jc_BxexModel);
        void DeleteCalibrationStatistics(string id);
        IList<Jc_BxexModel> GetCalibrationStatisticsList(int pageIndex, int pageSize, out int rowCount);
        Jc_BxexModel GetCalibrationStatisticsById(string id);
    }
}