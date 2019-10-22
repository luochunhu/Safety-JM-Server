using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Powerboxchargehistory;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IPowerboxchargehistoryService
    {  
	            BasicResponse<PowerboxchargehistoryInfo> AddPowerboxchargehistory(PowerboxchargehistoryAddRequest powerboxchargehistoryRequest);		
		        BasicResponse<PowerboxchargehistoryInfo> UpdatePowerboxchargehistory(PowerboxchargehistoryUpdateRequest powerboxchargehistoryRequest);	 
		        BasicResponse DeletePowerboxchargehistory(PowerboxchargehistoryDeleteRequest powerboxchargehistoryRequest);
		        BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryList(PowerboxchargehistoryGetListRequest powerboxchargehistoryRequest);
		         BasicResponse<PowerboxchargehistoryInfo> GetPowerboxchargehistoryById(PowerboxchargehistoryGetRequest powerboxchargehistoryRequest);
                 BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByFzhOrMac(PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest);
                 BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByStime(PowerboxchargehistoryGetByStimeRequest powerboxchargehistoryRequest);
    }
}

