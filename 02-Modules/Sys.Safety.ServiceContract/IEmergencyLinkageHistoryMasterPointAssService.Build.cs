using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.EmergencyLinkageHistoryMasterPointAss;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkageHistoryMasterPointAssService
    {  
	            BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> AddEmergencyLinkageHistoryMasterPointAss(EmergencyLinkageHistoryMasterPointAssAddRequest emergencyLinkageHistoryMasterPointAssRequest);		
		        BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> UpdateEmergencyLinkageHistoryMasterPointAss(EmergencyLinkageHistoryMasterPointAssUpdateRequest emergencyLinkageHistoryMasterPointAssRequest);	 
		        BasicResponse DeleteEmergencyLinkageHistoryMasterPointAss(EmergencyLinkageHistoryMasterPointAssDeleteRequest emergencyLinkageHistoryMasterPointAssRequest);
		        BasicResponse<List<EmergencyLinkageHistoryMasterPointAssInfo>> GetEmergencyLinkageHistoryMasterPointAssList(EmergencyLinkageHistoryMasterPointAssGetListRequest emergencyLinkageHistoryMasterPointAssRequest);
		         BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> GetEmergencyLinkageHistoryMasterPointAssById(EmergencyLinkageHistoryMasterPointAssGetRequest emergencyLinkageHistoryMasterPointAssRequest);	
    }
}

