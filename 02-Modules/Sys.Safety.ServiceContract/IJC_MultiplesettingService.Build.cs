using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Multiplesetting;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJC_MultiplesettingService
    {
        BasicResponse<JC_MultiplesettingInfo> AddMultiplesetting(JC_MultiplesettingAddRequest multiplesettingrequest);
        BasicResponse<JC_MultiplesettingInfo> UpdateMultiplesetting(JC_MultiplesettingUpdateRequest multiplesettingrequest);
        BasicResponse DeleteMultiplesetting(JC_MultiplesettingDeleteRequest multiplesettingrequest);
        BasicResponse<List<JC_MultiplesettingInfo>> GetMultiplesettingList(JC_MultiplesettingGetListRequest multiplesettingrequest);
        BasicResponse<List<JC_MultiplesettingInfo>> GetAllMultiplesettingList();
        BasicResponse<JC_MultiplesettingInfo> GetMultiplesettingById(JC_MultiplesettingGetRequest multiplesettingrequest);	
    }
}

