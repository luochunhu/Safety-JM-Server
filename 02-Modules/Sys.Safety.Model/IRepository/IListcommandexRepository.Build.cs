using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IListcommandexRepository : IRepository<ListcommandexModel>
    {
                ListcommandexModel AddListcommandex(ListcommandexModel listcommandexModel);
		        void UpdateListcommandex(ListcommandexModel listcommandexModel);
	            void DeleteListcommandex(string id);
		        IList<ListcommandexModel> GetListcommandexList(int pageIndex, int pageSize, out int rowCount);
				ListcommandexModel GetListcommandexById(string id);
    }
}
