using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listmetadata;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListmetadataService : IListmetadataService
    {
        private readonly IListmetadataRepository _Repository;

        public ListmetadataService(IListmetadataRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListmetadataInfo> AddListmetadata(ListmetadataAddRequest listmetadatarequest)
        {
            var _listmetadata =
                ObjectConverter.Copy<ListmetadataInfo, ListmetadataModel>(listmetadatarequest.ListmetadataInfo);
            var resultlistmetadata = _Repository.AddListmetadata(_listmetadata);
            var listmetadataresponse = new BasicResponse<ListmetadataInfo>();
            listmetadataresponse.Data = ObjectConverter.Copy<ListmetadataModel, ListmetadataInfo>(resultlistmetadata);
            return listmetadataresponse;
        }

        public BasicResponse<ListmetadataInfo> UpdateListmetadata(ListmetadataUpdateRequest listmetadatarequest)
        {
            var _listmetadata =
                ObjectConverter.Copy<ListmetadataInfo, ListmetadataModel>(listmetadatarequest.ListmetadataInfo);
            _Repository.UpdateListmetadata(_listmetadata);
            var listmetadataresponse = new BasicResponse<ListmetadataInfo>();
            listmetadataresponse.Data = ObjectConverter.Copy<ListmetadataModel, ListmetadataInfo>(_listmetadata);
            return listmetadataresponse;
        }

        public BasicResponse DeleteListmetadata(ListmetadataDeleteRequest listmetadatarequest)
        {
            _Repository.DeleteListmetadata(listmetadatarequest.Id);
            var listmetadataresponse = new BasicResponse();
            return listmetadataresponse;
        }

        public BasicResponse<List<ListmetadataInfo>> GetListmetadataList(ListmetadataGetListRequest listmetadatarequest)
        {
            var listmetadataresponse = new BasicResponse<List<ListmetadataInfo>>();
            listmetadatarequest.PagerInfo.PageIndex = listmetadatarequest.PagerInfo.PageIndex - 1;
            if (listmetadatarequest.PagerInfo.PageIndex < 0)
                listmetadatarequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listmetadataModelLists = _Repository.GetListmetadataList(listmetadatarequest.PagerInfo.PageIndex,
                listmetadatarequest.PagerInfo.PageSize, out rowcount);
            var listmetadataInfoLists = new List<ListmetadataInfo>();
            foreach (var item in listmetadataModelLists)
            {
                var ListmetadataInfo = ObjectConverter.Copy<ListmetadataModel, ListmetadataInfo>(item);
                listmetadataInfoLists.Add(ListmetadataInfo);
            }
            listmetadataresponse.Data = listmetadataInfoLists;
            return listmetadataresponse;
        }

        public BasicResponse<ListmetadataInfo> GetListmetadataById(ListmetadataGetRequest listmetadatarequest)
        {
            var result = _Repository.GetListmetadataById(listmetadatarequest.Id);
            var listmetadataInfo = ObjectConverter.Copy<ListmetadataModel, ListmetadataInfo>(result);
            var listmetadataresponse = new BasicResponse<ListmetadataInfo>();
            listmetadataresponse.Data = listmetadataInfo;
            return listmetadataresponse;
        }

        public BasicResponse SaveListMetaDataExInfo(SaveListMetaDataExInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListmetadataInfo, ListmetadataModel>(request.Info);
                    _Repository.AddListmetadata(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListmetadata(request.Info.ID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListmetadataInfo, ListmetadataModel>(request.Info);
                    _Repository.UpdateListmetadata(model);
                    break;
            }
            return new BasicResponse();
        }
    }
}