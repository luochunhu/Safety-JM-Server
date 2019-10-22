using System.Collections.Generic;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.Listtemple;

namespace Sys.Safety.ServiceContract
{
    public interface IListtempleService
    {
        BasicResponse<ListtempleInfo> AddListtemple(ListtempleAddRequest listtemplerequest);
        BasicResponse<ListtempleInfo> UpdateListtemple(ListtempleUpdateRequest listtemplerequest);
        BasicResponse DeleteListtemple(ListtempleDeleteRequest listtemplerequest);
        BasicResponse<List<ListtempleInfo>> GetListtempleList(ListtempleGetListRequest listtemplerequest);
        BasicResponse<ListtempleInfo> GetListtempleById(ListtempleGetRequest listtemplerequest);

        BasicResponse SaveListTempleInfo(SaveListTempleInfoRequest request);

        BasicResponse<ListtempleInfo> GetListtempleByListDataID(IdRequest request);

        BasicResponse<DataTable> GetNameFromListDataExListEx(IdRequest request);
    }
}