using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IManualCrossControlRepository : IRepository<Jc_JcsdkzModel>
    {
        Jc_JcsdkzModel AddManualCrossControl(Jc_JcsdkzModel ManualCrossControlModel);
        void UpdateManualCrossControl(Jc_JcsdkzModel ManualCrossControlModel);
        void DeleteManualCrossControl(string id);
        IList<Jc_JcsdkzModel> GetManualCrossControlList(int pageIndex, int pageSize, out int rowCount);
        List<Jc_JcsdkzModel> GetManualCrossControlList();
        Jc_JcsdkzModel GetManualCrossControlById(string id);

        void DelteManualCrossControlFromDB();
    }
}
