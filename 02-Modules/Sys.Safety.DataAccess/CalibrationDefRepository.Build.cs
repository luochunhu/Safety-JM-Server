using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class CalibrationDefRepository : RepositoryBase<Jc_BxModel>, ICalibrationDefRepository
    {

        public Jc_BxModel AddCalibrationDef(Jc_BxModel jc_BxModel)
        {
            return base.Insert(jc_BxModel);
        }
        public void UpdateCalibrationDef(Jc_BxModel jc_BxModel)
        {
            base.Update(jc_BxModel);
        }
        public void DeleteCalibrationDef(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_BxModel> GetCalibrationDefList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_BxModelLists = base.Datas;
            rowCount = jc_BxModelLists.Count();
            return jc_BxModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_BxModel GetCalibrationDefById(string id)
        {
            Jc_BxModel jc_BxModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_BxModel;
        }

        /// <summary>
        /// 根据时间删除标校
        /// </summary>
        /// <param name="time"></param>
        public void DeleteCalibrationDefByTime(DateTime time)
        {
            base.ExecuteNonQuery("global_RealModule_DeleteJc_bx_Bytime", time);
        }
    }
}
