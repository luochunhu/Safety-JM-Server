using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class SysinfRepository:RepositoryBase<SysinfModel>,ISysinfRepository
    {

                public SysinfModel AddSysinf(SysinfModel sysinfModel)
		{
		   return base.Insert(sysinfModel);
		}
		        public void UpdateSysinf(SysinfModel sysinfModel)
		{
		   base.Update(sysinfModel);
		}
	            public void DeleteSysinf(string id)
		{
		   base.Delete(id);
		}
		        public IList<SysinfModel> GetSysinfList(int pageIndex, int pageSize, out int rowCount)
		{
            var sysinfModelLists = base.Datas;
		   rowCount = sysinfModelLists.Count();
           return sysinfModelLists.OrderBy(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public SysinfModel GetSysinfById(string id)
		{
		    SysinfModel sysinfModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return sysinfModel;
		}
    }
}
