using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_KdRepository : IRepository<Jc_KdModel>
    {
                Jc_KdModel AddJc_Kd(Jc_KdModel jc_KdModel);
		        void UpdateJc_Kd(Jc_KdModel jc_KdModel);
	            void DeleteJc_Kd(string id);
		        IList<Jc_KdModel> GetJc_KdList(int pageIndex, int pageSize, out int rowCount);
				Jc_KdModel GetJc_KdById(string id);
    }
}
