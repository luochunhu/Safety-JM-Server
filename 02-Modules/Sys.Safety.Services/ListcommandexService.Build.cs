using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listcommandex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListcommandexService : IListcommandexService
    {
        private readonly IListcommandexRepository _Repository;

        public ListcommandexService(IListcommandexRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListcommandexInfo> AddListcommandex(ListcommandexAddRequest listcommandexrequest)
        {
            var _listcommandex =
                ObjectConverter.Copy<ListcommandexInfo, ListcommandexModel>(listcommandexrequest.ListcommandexInfo);
            var resultlistcommandex = _Repository.AddListcommandex(_listcommandex);
            var listcommandexresponse = new BasicResponse<ListcommandexInfo>();
            listcommandexresponse.Data = ObjectConverter.Copy<ListcommandexModel, ListcommandexInfo>(resultlistcommandex);
            return listcommandexresponse;
        }

        public BasicResponse<ListcommandexInfo> UpdateListcommandex(ListcommandexUpdateRequest listcommandexrequest)
        {
            var _listcommandex =
                ObjectConverter.Copy<ListcommandexInfo, ListcommandexModel>(listcommandexrequest.ListcommandexInfo);
            _Repository.UpdateListcommandex(_listcommandex);
            var listcommandexresponse = new BasicResponse<ListcommandexInfo>();
            listcommandexresponse.Data = ObjectConverter.Copy<ListcommandexModel, ListcommandexInfo>(_listcommandex);
            return listcommandexresponse;
        }

        public BasicResponse DeleteListcommandex(ListcommandexDeleteRequest listcommandexrequest)
        {
            _Repository.DeleteListcommandex(listcommandexrequest.Id);
            var listcommandexresponse = new BasicResponse();
            return listcommandexresponse;
        }

        public BasicResponse<List<ListcommandexInfo>> GetListcommandexList(
            ListcommandexGetListRequest listcommandexrequest)
        {
            var listcommandexresponse = new BasicResponse<List<ListcommandexInfo>>();
            listcommandexrequest.PagerInfo.PageIndex = listcommandexrequest.PagerInfo.PageIndex - 1;
            if (listcommandexrequest.PagerInfo.PageIndex < 0)
                listcommandexrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listcommandexModelLists = _Repository.GetListcommandexList(listcommandexrequest.PagerInfo.PageIndex,
                listcommandexrequest.PagerInfo.PageSize, out rowcount);
            var listcommandexInfoLists = new List<ListcommandexInfo>();
            foreach (var item in listcommandexModelLists)
            {
                var ListcommandexInfo = ObjectConverter.Copy<ListcommandexModel, ListcommandexInfo>(item);
                listcommandexInfoLists.Add(ListcommandexInfo);
            }
            listcommandexresponse.Data = listcommandexInfoLists;
            return listcommandexresponse;
        }

        public BasicResponse<ListcommandexInfo> GetListcommandexById(ListcommandexGetRequest listcommandexrequest)
        {
            var result = _Repository.GetListcommandexById(listcommandexrequest.Id);
            var listcommandexInfo = ObjectConverter.Copy<ListcommandexModel, ListcommandexInfo>(result);
            var listcommandexresponse = new BasicResponse<ListcommandexInfo>();
            listcommandexresponse.Data = listcommandexInfo;
            return listcommandexresponse;
        }

        public BasicResponse SaveListCommandInfo(SaveListCommandInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListcommandexInfo, ListcommandexModel>(request.Info);
                    _Repository.AddListcommandex(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListcommandex(request.Info.ListCommandID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListcommandexInfo, ListcommandexModel>(request.Info);
                    _Repository.UpdateListcommandex(model);
                    break;
            }
            return new BasicResponse();
        }
    }
}