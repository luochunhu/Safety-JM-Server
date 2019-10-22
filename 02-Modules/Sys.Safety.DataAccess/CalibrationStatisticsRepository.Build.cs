using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class CalibrationStatisticsRepository : RepositoryBase<Jc_BxexModel>, ICalibrationStatisticsRepository
    {
        public Jc_BxexModel AddCalibrationStatistics(Jc_BxexModel jc_BxexModel)
        {
            return Insert(jc_BxexModel);
        }

        public void UpdateCalibrationStatistics(Jc_BxexModel jc_BxexModel)
        {
            Update(jc_BxexModel);
        }

        public void DeleteCalibrationStatistics(string id)
        {
            Delete(id);
        }

        public IList<Jc_BxexModel> GetCalibrationStatisticsList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_BxexModelLists = Datas;
            rowCount = jc_BxexModelLists.Count();
            return jc_BxexModelLists.Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public Jc_BxexModel GetCalibrationStatisticsById(string id)
        {
            var jc_BxexModel = Datas.FirstOrDefault(c => c.ID == id);
            return jc_BxexModel;
        }
    }
}