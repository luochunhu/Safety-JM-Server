using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Fklib;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IFklibService
    {  
	            BasicResponse<FklibInfo> AddFklib(FklibAddRequest fklibrequest);		
		        BasicResponse<FklibInfo> UpdateFklib(FklibUpdateRequest fklibrequest);	 
		        BasicResponse DeleteFklib(FklibDeleteRequest fklibrequest);
		        BasicResponse<List<FklibInfo>> GetFklibList(FklibGetListRequest fklibrequest);
		         BasicResponse<FklibInfo> GetFklibById(FklibGetRequest fklibrequest);	
    }
}

