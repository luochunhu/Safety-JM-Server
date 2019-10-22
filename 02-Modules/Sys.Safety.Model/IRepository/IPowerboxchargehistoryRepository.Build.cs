using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IPowerboxchargehistoryRepository : IRepository<PowerboxchargehistoryModel>
    {
                PowerboxchargehistoryModel AddPowerboxchargehistory(PowerboxchargehistoryModel powerboxchargehistoryModel);
		        void UpdatePowerboxchargehistory(PowerboxchargehistoryModel powerboxchargehistoryModel);
	            void DeletePowerboxchargehistory(string id);
		        IList<PowerboxchargehistoryModel> GetPowerboxchargehistoryList(int pageIndex, int pageSize, out int rowCount);
				PowerboxchargehistoryModel GetPowerboxchargehistoryById(string id);
    }
}
