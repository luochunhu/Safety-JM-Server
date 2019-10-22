using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_ShowRepository : RepositoryBase<Jc_ShowModel>, IJc_ShowRepository
    {

        public Jc_ShowModel AddJc_Show(Jc_ShowModel jc_ShowModel)
        {
            return base.Insert(jc_ShowModel);
        }
        public void UpdateJc_Show(Jc_ShowModel jc_ShowModel)
        {
            base.Update(jc_ShowModel);
        }
        public void DeleteJc_Show(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_ShowModel> GetJc_ShowList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_ShowModelLists = base.Datas.ToList();
            rowCount = jc_ShowModelLists.Count();
            return jc_ShowModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_ShowModel GetJc_ShowById(string id)
        {
            Jc_ShowModel jc_ShowModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_ShowModel;
        }

        /// <summary>
        /// 根据页面编号删除
        /// </summary>
        /// <param name="page"></param>
        public void DeleteJc_ShowModelByPage(int page)
        {
            base.ExecuteNonQuery("global_RealModule_DeleteJc_show_byPage", page);
        }
    }
}
