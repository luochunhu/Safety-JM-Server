using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listdisplayex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListdisplayexService : IListdisplayexService
    {
        private readonly IListdisplayexRepository _Repository;

        public ListdisplayexService(IListdisplayexRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListdisplayexInfo> AddListdisplayex(ListdisplayexAddRequest listdisplayexrequest)
        {
            var _listdisplayex =
                ObjectConverter.Copy<ListdisplayexInfo, ListdisplayexModel>(listdisplayexrequest.ListdisplayexInfo);
            var resultlistdisplayex = _Repository.AddListdisplayex(_listdisplayex);
            var listdisplayexresponse = new BasicResponse<ListdisplayexInfo>();
            listdisplayexresponse.Data = ObjectConverter.Copy<ListdisplayexModel, ListdisplayexInfo>(resultlistdisplayex);
            return listdisplayexresponse;
        }

        public BasicResponse<ListdisplayexInfo> UpdateListdisplayex(ListdisplayexUpdateRequest listdisplayexrequest)
        {
            var _listdisplayex =
                ObjectConverter.Copy<ListdisplayexInfo, ListdisplayexModel>(listdisplayexrequest.ListdisplayexInfo);
            _Repository.UpdateListdisplayex(_listdisplayex);
            var listdisplayexresponse = new BasicResponse<ListdisplayexInfo>();
            listdisplayexresponse.Data = ObjectConverter.Copy<ListdisplayexModel, ListdisplayexInfo>(_listdisplayex);
            return listdisplayexresponse;
        }

        public BasicResponse DeleteListdisplayex(ListdisplayexDeleteRequest listdisplayexrequest)
        {
            _Repository.DeleteListdisplayex(listdisplayexrequest.Id);
            var listdisplayexresponse = new BasicResponse();
            return listdisplayexresponse;
        }

        public BasicResponse<List<ListdisplayexInfo>> GetListdisplayexList(
            ListdisplayexGetListRequest listdisplayexrequest)
        {
            var listdisplayexresponse = new BasicResponse<List<ListdisplayexInfo>>();
            listdisplayexrequest.PagerInfo.PageIndex = listdisplayexrequest.PagerInfo.PageIndex - 1;
            if (listdisplayexrequest.PagerInfo.PageIndex < 0)
                listdisplayexrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listdisplayexModelLists = _Repository.GetListdisplayexList(listdisplayexrequest.PagerInfo.PageIndex,
                listdisplayexrequest.PagerInfo.PageSize, out rowcount);
            var listdisplayexInfoLists = new List<ListdisplayexInfo>();
            foreach (var item in listdisplayexModelLists)
            {
                var ListdisplayexInfo = ObjectConverter.Copy<ListdisplayexModel, ListdisplayexInfo>(item);
                listdisplayexInfoLists.Add(ListdisplayexInfo);
            }
            listdisplayexresponse.Data = listdisplayexInfoLists;
            return listdisplayexresponse;
        }

        public BasicResponse<ListdisplayexInfo> GetListdisplayexById(ListdisplayexGetRequest listdisplayexrequest)
        {
            var result = _Repository.GetListdisplayexById(listdisplayexrequest.Id);
            var listdisplayexInfo = ObjectConverter.Copy<ListdisplayexModel, ListdisplayexInfo>(result);
            var listdisplayexresponse = new BasicResponse<ListdisplayexInfo>();
            listdisplayexresponse.Data = listdisplayexInfo;
            return listdisplayexresponse;
        }

        public BasicResponse SaveListDisplayExInfo(SaveListDisplayExInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListdisplayexInfo, ListdisplayexModel>(request.Info);
                    _Repository.AddListdisplayex(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListdisplayex(request.Info.ListDisplayID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListdisplayexInfo, ListdisplayexModel>(request.Info);
                    _Repository.UpdateListdisplayex(model);
                    break;
            }
            return new BasicResponse();
        }
    }
}