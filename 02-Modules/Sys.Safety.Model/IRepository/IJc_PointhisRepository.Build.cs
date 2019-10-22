using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_PointhisRepository : IRepository<Jc_PointhisModel>
    {
                Jc_PointhisModel AddJc_Pointhis(Jc_PointhisModel jc_PointhisModel);
		        void UpdateJc_Pointhis(Jc_PointhisModel jc_PointhisModel);
	            void DeleteJc_Pointhis(string id);
		        IList<Jc_PointhisModel> GetJc_PointhisList(int pageIndex, int pageSize, out int rowCount);
				Jc_PointhisModel GetJc_PointhisById(string id);
    }
}
