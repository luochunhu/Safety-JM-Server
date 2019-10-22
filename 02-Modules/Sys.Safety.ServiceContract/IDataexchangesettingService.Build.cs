using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Dataexchangesetting;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IDataexchangesettingService
    {  
	            BasicResponse<DataexchangesettingInfo> AddDataexchangesetting(DataexchangesettingAddRequest dataexchangesettingrequest);		
		        BasicResponse<DataexchangesettingInfo> UpdateDataexchangesetting(DataexchangesettingUpdateRequest dataexchangesettingrequest);	 
		        BasicResponse DeleteDataexchangesetting(DataexchangesettingDeleteRequest dataexchangesettingrequest);
		        BasicResponse<List<DataexchangesettingInfo>> GetDataexchangesettingList(DataexchangesettingGetListRequest dataexchangesettingrequest);
		         BasicResponse<DataexchangesettingInfo> GetDataexchangesettingById(DataexchangesettingGetRequest dataexchangesettingrequest);	
    }
}

