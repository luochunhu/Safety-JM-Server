using Basic.Framework.Data;
using System.Collections.Generic;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IShortCutMenuRepository : IRepository<ShortCutMenuModel>
    {
        ShortCutMenuModel AddShortCutMenu(ShortCutMenuModel shortCutMenuModel);
        void UpdateShortCutMenu(ShortCutMenuModel shortCutMenuModel);
        void DeleteShortCutMenu(string id);
        IList<ShortCutMenuModel> GetShortCutMenuList(int pageIndex, int pageSize, out int rowCount);
        ShortCutMenuModel GetShortCutMenuById(string id);
    }
}
