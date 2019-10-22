using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_ShowRepository : IRepository<Jc_ShowModel>
    {
        Jc_ShowModel AddJc_Show(Jc_ShowModel jc_ShowModel);
        void UpdateJc_Show(Jc_ShowModel jc_ShowModel);
        void DeleteJc_Show(string id);
        IList<Jc_ShowModel> GetJc_ShowList(int pageIndex, int pageSize, out int rowCount);
        Jc_ShowModel GetJc_ShowById(string id);

        /// <summary>
        /// 根据页面编号删除
        /// </summary>
        /// <param name="page"></param>
        void DeleteJc_ShowModelByPage(int page);
    }
}
