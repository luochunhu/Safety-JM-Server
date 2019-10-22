using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.KJ_Addresstype;

namespace Sys.Safety.ServiceContract
{
    public interface IKJ_AddresstypeService
    {  
	            BasicResponse<KJ_AddresstypeInfo> AddKJ_Addresstype(KJ_AddresstypeAddRequest kJ_AddresstypeRequest);		
		        BasicResponse<KJ_AddresstypeInfo> UpdateKJ_Addresstype(KJ_AddresstypeUpdateRequest kJ_AddresstypeRequest);	 
		        BasicResponse DeleteKJ_Addresstype(KJ_AddresstypeDeleteRequest kJ_AddresstypeRequest);
		        BasicResponse<List<KJ_AddresstypeInfo>> GetKJ_AddresstypeList(KJ_AddresstypeGetListRequest kJ_AddresstypeRequest);
		         BasicResponse<KJ_AddresstypeInfo> GetKJ_AddresstypeById(KJ_AddresstypeGetRequest kJ_AddresstypeRequest);	
    }
}

