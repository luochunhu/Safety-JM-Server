using System.Collections.Generic;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.Listtemple;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListtempleService : IListtempleService
    {
        private readonly IListtempleRepository _Repository;

        public ListtempleService(IListtempleRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListtempleInfo> AddListtemple(ListtempleAddRequest listtemplerequest)
        {
            var _listtemple = ObjectConverter.Copy<ListtempleInfo, ListtempleModel>(listtemplerequest.ListtempleInfo);
            var resultlisttemple = _Repository.AddListtemple(_listtemple);
            var listtempleresponse = new BasicResponse<ListtempleInfo>();
            listtempleresponse.Data = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(resultlisttemple);
            return listtempleresponse;
        }

        public BasicResponse<ListtempleInfo> UpdateListtemple(ListtempleUpdateRequest listtemplerequest)
        {
            var _listtemple = ObjectConverter.Copy<ListtempleInfo, ListtempleModel>(listtemplerequest.ListtempleInfo);
            _Repository.UpdateListtemple(_listtemple);
            var listtempleresponse = new BasicResponse<ListtempleInfo>();
            listtempleresponse.Data = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(_listtemple);
            return listtempleresponse;
        }

        public BasicResponse DeleteListtemple(ListtempleDeleteRequest listtemplerequest)
        {
            _Repository.DeleteListtemple(listtemplerequest.Id);
            var listtempleresponse = new BasicResponse();
            return listtempleresponse;
        }

        public BasicResponse<List<ListtempleInfo>> GetListtempleList(ListtempleGetListRequest listtemplerequest)
        {
            var listtempleresponse = new BasicResponse<List<ListtempleInfo>>();
            listtemplerequest.PagerInfo.PageIndex = listtemplerequest.PagerInfo.PageIndex - 1;
            if (listtemplerequest.PagerInfo.PageIndex < 0)
                listtemplerequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listtempleModelLists = _Repository.GetListtempleList(listtemplerequest.PagerInfo.PageIndex,
                listtemplerequest.PagerInfo.PageSize, out rowcount);
            var listtempleInfoLists = new List<ListtempleInfo>();
            foreach (var item in listtempleModelLists)
            {
                var ListtempleInfo = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(item);
                listtempleInfoLists.Add(ListtempleInfo);
            }
            listtempleresponse.Data = listtempleInfoLists;
            return listtempleresponse;
        }

        public BasicResponse<ListtempleInfo> GetListtempleById(ListtempleGetRequest listtemplerequest)
        {
            var result = _Repository.GetListtempleById(listtemplerequest.Id);
            var listtempleInfo = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(result);
            var listtempleresponse = new BasicResponse<ListtempleInfo>();
            listtempleresponse.Data = listtempleInfo;
            return listtempleresponse;
        }

        public BasicResponse SaveListTempleInfo(SaveListTempleInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListtempleInfo, ListtempleModel>(request.Info);
                    _Repository.AddListtemple(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListtemple(request.Info.ListTempleID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListtempleInfo, ListtempleModel>(request.Info);
                    _Repository.UpdateListtemple(model);
                    break;
            }
            return new BasicResponse();
        }

        public BasicResponse<ListtempleInfo> GetListtempleByListDataID(IdRequest request)
        {
            var model = _Repository.GetListtempleByListDataID(request.Id.ToString());
            var info = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(model);
            var ret = new BasicResponse<ListtempleInfo>()
            {
                Data = info
            };
            return ret;
        }

        public BasicResponse<DataTable> GetNameFromListDataExListEx(IdRequest request)
        {
            var listDataId = request.Id;
            var dtName = _Repository.QueryTable("global_ListtempleService_GetNameFromListDataExListEx", listDataId);
            var ret = new BasicResponse<DataTable>
            {
                Data = dtName
            };
            return ret;
        }
    }
}