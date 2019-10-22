using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listdataremark;

namespace Sys.Safety.ServiceContract
{
    public interface IListdataremarkService
    {
        BasicResponse<ListdataremarkInfo> AddListdataremark(ListdataremarkAddRequest listdataremarkRequest);
        BasicResponse<ListdataremarkInfo> UpdateListdataremark(ListdataremarkUpdateRequest listdataremarkRequest);
        BasicResponse DeleteListdataremark(ListdataremarkDeleteRequest listdataremarkRequest);
        BasicResponse<List<ListdataremarkInfo>> GetListdataremarkList(ListdataremarkGetListRequest listdataremarkRequest);
        BasicResponse<ListdataremarkInfo> GetListdataremarkById(ListdataremarkGetRequest listdataremarkRequest);
        BasicResponse<ListdataremarkInfo> GetListdataremarkByTimeListDataId(GetListdataremarkByTimeListDataIdRequest getListdataremarkByTimeListDataIdRequest);
        BasicResponse<ListdataremarkInfo> UpdateListdataremarkByTimeListDataId(ListdataremarkUpdateRequest listdataremarkRequest);

    }
}