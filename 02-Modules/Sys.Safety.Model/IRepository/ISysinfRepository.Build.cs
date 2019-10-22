using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ISysinfRepository : IRepository<SysinfModel>
    {
                SysinfModel AddSysinf(SysinfModel sysinfModel);
		        void UpdateSysinf(SysinfModel sysinfModel);
	            void DeleteSysinf(string id);
		        IList<SysinfModel> GetSysinfList(int pageIndex, int pageSize, out int rowCount);
				SysinfModel GetSysinfById(string id);
    }
}
