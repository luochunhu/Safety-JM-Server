using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listdisplayex;

namespace Sys.Safety.ServiceContract
{
    public interface IListdisplayexService
    {
        BasicResponse<ListdisplayexInfo> AddListdisplayex(ListdisplayexAddRequest listdisplayexrequest);
        BasicResponse<ListdisplayexInfo> UpdateListdisplayex(ListdisplayexUpdateRequest listdisplayexrequest);
        BasicResponse DeleteListdisplayex(ListdisplayexDeleteRequest listdisplayexrequest);
        BasicResponse<List<ListdisplayexInfo>> GetListdisplayexList(ListdisplayexGetListRequest listdisplayexrequest);
        BasicResponse<ListdisplayexInfo> GetListdisplayexById(ListdisplayexGetRequest listdisplayexrequest);

        BasicResponse SaveListDisplayExInfo(SaveListDisplayExInfoRequest request);
    }
}