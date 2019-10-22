using Basic.Framework.Data;
using System.Collections.Generic;
using System.Linq;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public partial class ShortCutMenuRepository : RepositoryBase<ShortCutMenuModel>, IShortCutMenuRepository
    {

        public ShortCutMenuModel AddShortCutMenu(ShortCutMenuModel shortCutMenuModel)
        {
            return base.Insert(shortCutMenuModel);
        }
        public void UpdateShortCutMenu(ShortCutMenuModel shortCutMenuModel)
        {
            base.Update(shortCutMenuModel);
        }
        public void DeleteShortCutMenu(string id)
        {
            base.Delete(id);
        }
        public IList<ShortCutMenuModel> GetShortCutMenuList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = base.Datas.Count();
            return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public ShortCutMenuModel GetShortCutMenuById(string id)
        {
            ShortCutMenuModel shortCutMenuModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return shortCutMenuModel;
        }
    }
}
