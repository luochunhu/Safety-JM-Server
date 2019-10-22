using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Callpointlist;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallpointlistService
    {
        BasicResponse<B_CallpointlistInfo> AddB_Callpointlist(B_CallpointlistAddRequest b_CallpointlistRequest);
        BasicResponse<B_CallpointlistInfo> UpdateB_Callpointlist(B_CallpointlistUpdateRequest b_CallpointlistRequest);
        BasicResponse DeleteB_Callpointlist(B_CallpointlistDeleteRequest b_CallpointlistRequest);
        BasicResponse<List<B_CallpointlistInfo>> GetB_CallpointlistList(B_CallpointlistGetListRequest b_CallpointlistRequest);
        BasicResponse<B_CallpointlistInfo> GetB_CallpointlistById(B_CallpointlistGetRequest b_CallpointlistRequest);

        BasicResponse<List<B_CallpointlistInfo>> GetB_CallByBCallId(B_CallpointlistGetRequest b_CallpointlistRequest);
    }
}

