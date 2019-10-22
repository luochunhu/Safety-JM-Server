using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listdataex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ListdataexService : IListdataexService
    {
        private readonly IListdataexRepository _Repository;
        private readonly RepositoryBase<ListdataexModel> _RepositoryBase;

        public ListdataexService(IListdataexRepository _Repository)
        {
            this._Repository = _Repository;
            _RepositoryBase = _Repository as RepositoryBase<ListdataexModel>;
        }

        public BasicResponse<ListdataexInfo> AddListdataex(ListdataexAddRequest listdataexrequest)
        {
            var _listdataex = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(listdataexrequest.ListdataexInfo);
            var resultlistdataex = _Repository.AddListdataex(_listdataex);
            var listdataexresponse = new BasicResponse<ListdataexInfo>();
            listdataexresponse.Data = ObjectConverter.Copy<ListdataexModel, ListdataexInfo>(resultlistdataex);
            return listdataexresponse;
        }

        public BasicResponse<ListdataexInfo> UpdateListdataex(ListdataexUpdateRequest listdataexrequest)
        {
            var _listdataex = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(listdataexrequest.ListdataexInfo);
            _Repository.UpdateListdataex(_listdataex);
            var listdataexresponse = new BasicResponse<ListdataexInfo>();
            listdataexresponse.Data = ObjectConverter.Copy<ListdataexModel, ListdataexInfo>(_listdataex);
            return listdataexresponse;
        }

        public BasicResponse DeleteListdataex(ListdataexDeleteRequest listdataexrequest)
        {
            _Repository.DeleteListdataex(listdataexrequest.Id);
            var listdataexresponse = new BasicResponse();
            return listdataexresponse;
        }

        public BasicResponse<List<ListdataexInfo>> GetListdataexList(ListdataexGetListRequest listdataexrequest)
        {
            var listdataexresponse = new BasicResponse<List<ListdataexInfo>>();
            listdataexrequest.PagerInfo.PageIndex = listdataexrequest.PagerInfo.PageIndex - 1;
            if (listdataexrequest.PagerInfo.PageIndex < 0)
                listdataexrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listdataexModelLists = _Repository.GetListdataexList(listdataexrequest.PagerInfo.PageIndex,
                listdataexrequest.PagerInfo.PageSize, out rowcount);
            var listdataexInfoLists = new List<ListdataexInfo>();
            foreach (var item in listdataexModelLists)
            {
                var ListdataexInfo = ObjectConverter.Copy<ListdataexModel, ListdataexInfo>(item);
                listdataexInfoLists.Add(ListdataexInfo);
            }
            listdataexresponse.Data = listdataexInfoLists;
            return listdataexresponse;
        }

        public BasicResponse<ListdataexInfo> GetListdataexById(ListdataexGetRequest listdataexrequest)
        {
            var result = _Repository.GetListdataexById(listdataexrequest.Id);
            var listdataexInfo = ObjectConverter.Copy<ListdataexModel, ListdataexInfo>(result);
            var listdataexresponse = new BasicResponse<ListdataexInfo>();
            listdataexresponse.Data = listdataexInfo;
            return listdataexresponse;
        }

        public BasicResponse<IList<ListdataexInfo>> GetListDataExEntity(ListdataexGetBySqlRequest strHql)
        {
            //return base.DAOManager.GetList(strHql);
            var dt = _RepositoryBase.QueryTableBySql(strHql.Sql);
            IList<ListdataexInfo> listdataex = ObjectConverter.Copy<ListdataexInfo>(dt);
            var ret = new BasicResponse<IList<ListdataexInfo>>
            {
                Data = listdataex
            };
            return ret;
        }

        public BasicResponse SaveListDataExInfo(SaveListDataExInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(request.Info);
                    _Repository.AddListdataex(model2);
                    break;
                case InfoState.Delete:
                    _Repository.DeleteListdataex(request.Info.ListDataID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(request.Info);
                    _Repository.UpdateListdataex(model);
                    break;
            }
            return new BasicResponse();
        }
    }
}