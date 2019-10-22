using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listdataex;

namespace Sys.Safety.ServiceContract
{
    public interface IListdataexService
    {
        BasicResponse<ListdataexInfo> AddListdataex(ListdataexAddRequest listdataexrequest);
        BasicResponse<ListdataexInfo> UpdateListdataex(ListdataexUpdateRequest listdataexrequest);
        BasicResponse DeleteListdataex(ListdataexDeleteRequest listdataexrequest);
        BasicResponse<List<ListdataexInfo>> GetListdataexList(ListdataexGetListRequest listdataexrequest);
        BasicResponse<ListdataexInfo> GetListdataexById(ListdataexGetRequest listdataexrequest);

        BasicResponse<IList<ListdataexInfo>> GetListDataExEntity(ListdataexGetBySqlRequest strHql);

        BasicResponse SaveListDataExInfo(SaveListDataExInfoRequest request);
    }
}