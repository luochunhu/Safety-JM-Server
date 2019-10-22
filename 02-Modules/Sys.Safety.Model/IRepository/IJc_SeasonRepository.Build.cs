using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_SeasonRepository : IRepository<Jc_SeasonModel>
    {
                Jc_SeasonModel AddJc_Season(Jc_SeasonModel jc_SeasonModel);
		        void UpdateJc_Season(Jc_SeasonModel jc_SeasonModel);
	            void DeleteJc_Season(string id);
		        IList<Jc_SeasonModel> GetJc_SeasonList(int pageIndex, int pageSize, out int rowCount);
				Jc_SeasonModel GetJc_SeasonById(string id);
    }
}
