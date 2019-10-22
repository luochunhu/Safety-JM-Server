using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listdatalayount;

namespace Sys.Safety.ServiceContract
{
    public interface IListdatalayountService
    {
        BasicResponse<ListdatalayountInfo> AddListdatalayount(ListdatalayountAddRequest listdatalayountrequest);
        BasicResponse<ListdatalayountInfo> UpdateListdatalayount(ListdatalayountUpdateRequest listdatalayountrequest);
        BasicResponse DeleteListdatalayount(ListdatalayountDeleteRequest listdatalayountrequest);

        BasicResponse<List<ListdatalayountInfo>> GetListdatalayountList(
            ListdatalayountGetListRequest listdatalayountrequest);

        BasicResponse<ListdatalayountInfo> GetListdatalayountById(ListdatalayountGetRequest listdatalayountrequest);

        BasicResponse SaveListDataLayountInfo(SaveListDataLayountInfoRequest request);

        BasicResponse DeleteListdatalayountByTimeListDataId(DeleteListdatalayountByTimeListDataIdRequest request);

        BasicResponse<IList<ListdatalayountInfo>> GetListdatalayountByListDataId(GetListdatalayountByListDataIdRequest request);

        BasicResponse<ListdatalayountInfo> GetListdatalayountByListDataIdArrangeName(
            GetListdatalayountByListDataIdArrangeTimeRequest request);
    }
}