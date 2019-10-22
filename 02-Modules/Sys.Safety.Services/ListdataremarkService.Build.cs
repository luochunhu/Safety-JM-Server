using System;
using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listdataremark;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListdataremarkService : IListdataremarkService
    {
        private readonly IListdataremarkRepository _Repository;

        public ListdataremarkService(IListdataremarkRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<ListdataremarkInfo> AddListdataremark(ListdataremarkAddRequest listdataremarkRequest)
        {
            var _listdataremark =
                ObjectConverter.Copy<ListdataremarkInfo, ListdataremarkModel>(listdataremarkRequest.ListdataremarkInfo);
            var resultlistdataremark = _Repository.AddListdataremark(_listdataremark);
            var listdataremarkresponse = new BasicResponse<ListdataremarkInfo>();
            listdataremarkresponse.Data =
                ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(resultlistdataremark);
            return listdataremarkresponse;
        }

        public BasicResponse<ListdataremarkInfo> UpdateListdataremark(ListdataremarkUpdateRequest listdataremarkRequest)
        {
            var _listdataremark =
                ObjectConverter.Copy<ListdataremarkInfo, ListdataremarkModel>(listdataremarkRequest.ListdataremarkInfo);
            _Repository.UpdateListdataremark(_listdataremark);
            var listdataremarkresponse = new BasicResponse<ListdataremarkInfo>();
            listdataremarkresponse.Data = ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(_listdataremark);
            return listdataremarkresponse;
        }

        public BasicResponse DeleteListdataremark(ListdataremarkDeleteRequest listdataremarkRequest)
        {
            _Repository.DeleteListdataremark(listdataremarkRequest.Id);
            var listdataremarkresponse = new BasicResponse();
            return listdataremarkresponse;
        }

        public BasicResponse<List<ListdataremarkInfo>> GetListdataremarkList(
            ListdataremarkGetListRequest listdataremarkRequest)
        {
            var listdataremarkresponse = new BasicResponse<List<ListdataremarkInfo>>();
            listdataremarkRequest.PagerInfo.PageIndex = listdataremarkRequest.PagerInfo.PageIndex - 1;
            if (listdataremarkRequest.PagerInfo.PageIndex < 0)
                listdataremarkRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listdataremarkModelLists = _Repository.GetListdataremarkList(listdataremarkRequest.PagerInfo.PageIndex,
                listdataremarkRequest.PagerInfo.PageSize, out rowcount);
            var listdataremarkInfoLists = new List<ListdataremarkInfo>();
            foreach (var item in listdataremarkModelLists)
            {
                var ListdataremarkInfo = ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(item);
                listdataremarkInfoLists.Add(ListdataremarkInfo);
            }
            listdataremarkresponse.Data = listdataremarkInfoLists;
            return listdataremarkresponse;
        }

        public BasicResponse<ListdataremarkInfo> GetListdataremarkById(ListdataremarkGetRequest listdataremarkRequest)
        {
            var result = _Repository.GetListdataremarkById(listdataremarkRequest.Id);
            var listdataremarkInfo = ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(result);
            var listdataremarkresponse = new BasicResponse<ListdataremarkInfo>();
            listdataremarkresponse.Data = listdataremarkInfo;
            return listdataremarkresponse;
        }
        
        public BasicResponse<ListdataremarkInfo> GetListdataremarkByTimeListDataId(
            GetListdataremarkByTimeListDataIdRequest getListdataremarkByTimeListDataIdRequest)
        {
            DateTime time = Convert.ToDateTime(getListdataremarkByTimeListDataIdRequest.Time);
            long listDataId = Convert.ToInt64(getListdataremarkByTimeListDataIdRequest.ListDataId);
            var result = _Repository.GetListdataremarkByTimeListDataId(time, listDataId);
            var listdataremarkInfo = ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(result);
            var listdataremarkresponse = new BasicResponse<ListdataremarkInfo>()
            {
                Data = listdataremarkInfo
            };
            return listdataremarkresponse;
        }

        public BasicResponse<ListdataremarkInfo> UpdateListdataremarkByTimeListDataId(ListdataremarkUpdateRequest listdataremarkRequest)
        {
            var _listdataremark =
                ObjectConverter.Copy<ListdataremarkInfo, ListdataremarkModel>(listdataremarkRequest.ListdataremarkInfo);
            _Repository.UpdateListdataremarkByTimeListDataId(_listdataremark);
            var listdataremarkresponse = new BasicResponse<ListdataremarkInfo>();
            listdataremarkresponse.Data = ObjectConverter.Copy<ListdataremarkModel, ListdataremarkInfo>(_listdataremark);
            return listdataremarkresponse;
        }
    }
}