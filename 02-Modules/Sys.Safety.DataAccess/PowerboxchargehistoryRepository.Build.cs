using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class PowerboxchargehistoryRepository:RepositoryBase<PowerboxchargehistoryModel>,IPowerboxchargehistoryRepository
    {

                public PowerboxchargehistoryModel AddPowerboxchargehistory(PowerboxchargehistoryModel powerboxchargehistoryModel)
		{
		   return base.Insert(powerboxchargehistoryModel);
		}
		        public void UpdatePowerboxchargehistory(PowerboxchargehistoryModel powerboxchargehistoryModel)
		{
		   base.Update(powerboxchargehistoryModel);
		}
	            public void DeletePowerboxchargehistory(string id)
		{
		   base.Delete(id);
		}
		        public IList<PowerboxchargehistoryModel> GetPowerboxchargehistoryList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  powerboxchargehistoryModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return powerboxchargehistoryModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public PowerboxchargehistoryModel GetPowerboxchargehistoryById(string id)
		{
		    PowerboxchargehistoryModel powerboxchargehistoryModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return powerboxchargehistoryModel;
		}
    }
}
