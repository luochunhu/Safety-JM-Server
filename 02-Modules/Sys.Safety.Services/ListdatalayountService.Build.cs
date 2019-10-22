using System;
using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listdatalayount;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListdatalayountService : IListdatalayountService
    {
        private readonly IListdatalayountRepository _Repository;

        public ListdatalayountService(IListdatalayountRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListdatalayountInfo> AddListdatalayount(ListdatalayountAddRequest listdatalayountrequest)
        {
            var _listdatalayount =
                ObjectConverter.Copy<ListdatalayountInfo, ListdatalayountModel>(
                    listdatalayountrequest.ListdatalayountInfo);
            var resultlistdatalayount = _Repository.AddListdatalayount(_listdatalayount);
            var listdatalayountresponse = new BasicResponse<ListdatalayountInfo>();
            listdatalayountresponse.Data =
                ObjectConverter.Copy<ListdatalayountModel, ListdatalayountInfo>(resultlistdatalayount);
            return listdatalayountresponse;
        }

        public BasicResponse<ListdatalayountInfo> UpdateListdatalayount(
            ListdatalayountUpdateRequest listdatalayountrequest)
        {
            var _listdatalayount =
                ObjectConverter.Copy<ListdatalayountInfo, ListdatalayountModel>(
                    listdatalayountrequest.ListdatalayountInfo);
            _Repository.UpdateListdatalayount(_listdatalayount);
            var listdatalayountresponse = new BasicResponse<ListdatalayountInfo>();
            listdatalayountresponse.Data =
                ObjectConverter.Copy<ListdatalayountModel, ListdatalayountInfo>(_listdatalayount);
            return listdatalayountresponse;
        }

        public BasicResponse DeleteListdatalayount(ListdatalayountDeleteRequest listdatalayountrequest)
        {
            _Repository.DeleteListdatalayount(listdatalayountrequest.Id);
            var listdatalayountresponse = new BasicResponse();
            return listdatalayountresponse;
        }

        public BasicResponse<List<ListdatalayountInfo>> GetListdatalayountList(
            ListdatalayountGetListRequest listdatalayountrequest)
        {
            var listdatalayountresponse = new BasicResponse<List<ListdatalayountInfo>>();
            listdatalayountrequest.PagerInfo.PageIndex = listdatalayountrequest.PagerInfo.PageIndex - 1;
            if (listdatalayountrequest.PagerInfo.PageIndex < 0)
                listdatalayountrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listdatalayountModelLists =
                _Repository.GetListdatalayountList(listdatalayountrequest.PagerInfo.PageIndex,
                    listdatalayountrequest.PagerInfo.PageSize, out rowcount);
            var listdatalayountInfoLists = new List<ListdatalayountInfo>();
            foreach (var item in listdatalayountModelLists)
            {
                var ListdatalayountInfo = ObjectConverter.Copy<ListdatalayountModel, ListdatalayountInfo>(item);
                listdatalayountInfoLists.Add(ListdatalayountInfo);
            }
            listdatalayountresponse.Data = listdatalayountInfoLists;
            return listdatalayountresponse;
        }

        public BasicResponse<ListdatalayountInfo> GetListdatalayountById(ListdatalayountGetRequest listdatalayountrequest)
        {
            var result = _Repository.GetListdatalayountById(listdatalayountrequest.Id);
            var listdatalayountInfo = ObjectConverter.Copy<ListdatalayountModel, ListdatalayountInfo>(result);
            var listdatalayountresponse = new BasicResponse<ListdatalayountInfo>();
            listdatalayountresponse.Data = listdatalayountInfo;
            return listdatalayountresponse;
        }

        public BasicResponse SaveListDataLayountInfo(SaveListDataLayountInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListdatalayountInfo, ListdatalayountModel>(request.Info);
                    _Repository.AddListdatalayount(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListdatalayount(request.Info.ListDataLayoutID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListdatalayountInfo, ListdatalayountModel>(request.Info);
                    _Repository.UpdateListdatalayount(model);
                    break;
            }
            return new BasicResponse();
        }

        public BasicResponse DeleteListdatalayountByTimeListDataId(DeleteListdatalayountByTimeListDataIdRequest request)
        {
            _Repository.DeleteListdatalayountByTimeListDataId(request.Time,
                Convert.ToInt64(request.ListDataId));
            return new BasicResponse();
        }

        public BasicResponse<IList<ListdatalayountInfo>> GetListdatalayountByListDataId(GetListdatalayountByListDataIdRequest request)
        {
            var result = _Repository.GetListdatalayountByListDataId(request.Id);
            var listdatalayountInfo = ObjectConverter.CopyList<ListdatalayountModel, ListdatalayountInfo>(result);
            var ret = new BasicResponse<IList<ListdatalayountInfo>>
            {
                Data = listdatalayountInfo
            };
            return ret;
        }

        public BasicResponse<ListdatalayountInfo> GetListdatalayountByListDataIdArrangeName(GetListdatalayountByListDataIdArrangeTimeRequest request)
        {
            var result = _Repository.GetListdatalayountByListDataIdArrangeName(request.ListDataId, request.ArrangeName);
            var listdatalayountInfo = ObjectConverter.Copy<ListdatalayountModel, ListdatalayountInfo>(result);
            var ret = new BasicResponse<ListdatalayountInfo>
            {
                Data = listdatalayountInfo
            };
            return ret;
        }
    }
}