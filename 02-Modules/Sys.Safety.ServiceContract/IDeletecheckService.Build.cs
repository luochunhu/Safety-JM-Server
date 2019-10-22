using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Deletecheck;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IDeletecheckService
    {  
	            BasicResponse<DeletecheckInfo> AddDeletecheck(DeletecheckAddRequest deletecheckrequest);		
		        BasicResponse<DeletecheckInfo> UpdateDeletecheck(DeletecheckUpdateRequest deletecheckrequest);	 
		        BasicResponse DeleteDeletecheck(DeletecheckDeleteRequest deletecheckrequest);
		        BasicResponse<List<DeletecheckInfo>> GetDeletecheckList(DeletecheckGetListRequest deletecheckrequest);
		         BasicResponse<DeletecheckInfo> GetDeletecheckById(DeletecheckGetRequest deletecheckrequest);	
    }
}

