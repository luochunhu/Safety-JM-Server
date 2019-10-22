using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Dataright;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IDatarightService
    {  
	            BasicResponse<DatarightInfo> AddDataright(DatarightAddRequest datarightrequest);		
		        BasicResponse<DatarightInfo> UpdateDataright(DatarightUpdateRequest datarightrequest);	 
		        BasicResponse DeleteDataright(DatarightDeleteRequest datarightrequest);
		        BasicResponse<List<DatarightInfo>> GetDatarightList(DatarightGetListRequest datarightrequest);
		         BasicResponse<DatarightInfo> GetDatarightById(DatarightGetRequest datarightrequest);	
    }
}

