using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Parameter;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IParameterService
    {  
	            BasicResponse<JC_ParameterInfo> AddJC_Parameter(ParameterAddRequest jC_Parameterrequest);		
		        BasicResponse<JC_ParameterInfo> UpdateJC_Parameter(ParameterUpdateRequest jC_Parameterrequest);	 
		        BasicResponse DeleteJC_Parameter(ParameterDeleteRequest jC_Parameterrequest);
		        BasicResponse<List<JC_ParameterInfo>> GetJC_ParameterList(ParameterGetListRequest jC_Parameterrequest);
		         BasicResponse<JC_ParameterInfo> GetJC_ParameterById(ParameterGetRequest jC_Parameterrequest);	
    }
}

