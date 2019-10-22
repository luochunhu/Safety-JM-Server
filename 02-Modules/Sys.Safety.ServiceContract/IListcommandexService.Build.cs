using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listcommandex;

namespace Sys.Safety.ServiceContract
{
    public interface IListcommandexService
    {
        BasicResponse<ListcommandexInfo> AddListcommandex(ListcommandexAddRequest listcommandexrequest);
        BasicResponse<ListcommandexInfo> UpdateListcommandex(ListcommandexUpdateRequest listcommandexrequest);
        BasicResponse DeleteListcommandex(ListcommandexDeleteRequest listcommandexrequest);
        BasicResponse<List<ListcommandexInfo>> GetListcommandexList(ListcommandexGetListRequest listcommandexrequest);
        BasicResponse<ListcommandexInfo> GetListcommandexById(ListcommandexGetRequest listcommandexrequest);

        BasicResponse SaveListCommandInfo(SaveListCommandInfoRequest request);
    }
}