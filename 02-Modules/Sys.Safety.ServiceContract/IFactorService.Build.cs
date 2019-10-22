using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Factor;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IFactorService
    {  
	            BasicResponse<JC_FactorInfo> AddJC_Factor(FactorAddRequest jC_Factorrequest);		
		        BasicResponse<JC_FactorInfo> UpdateJC_Factor(FactorUpdateRequest jC_Factorrequest);	 
		        BasicResponse DeleteJC_Factor(FactorDeleteRequest jC_Factorrequest);
		        BasicResponse<List<JC_FactorInfo>> GetJC_FactorList(FactorGetListRequest jC_Factorrequest);
		         BasicResponse<JC_FactorInfo> GetJC_FactorById(FactorGetRequest jC_Factorrequest);	
    }
}

