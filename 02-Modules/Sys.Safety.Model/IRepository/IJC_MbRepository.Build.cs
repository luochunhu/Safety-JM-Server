using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJC_MbRepository : IRepository<JC_MbModel>
    {
        JC_MbModel AddMb(JC_MbModel mbModel);
        void UpdateMb(JC_MbModel mbModel);
        void DeleteMb(string id);
        IList<JC_MbModel> GetMbList(int pageIndex, int pageSize, out int rowCount);
        JC_MbModel GetMbById(string id);

        void AddItemBySql(JC_MbModel jcmb);
        void UpdateItemBySql(JC_MbModel jcmb);



        /// <summary>
        /// 清除数据库中未结束的报警
        /// </summary>
        /// <returns></returns>
        void ClearDbNoEndAlarmBySql();
    }
}
