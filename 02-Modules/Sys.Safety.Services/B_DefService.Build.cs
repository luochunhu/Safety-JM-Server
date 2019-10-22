using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Def;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services
{
    public partial class B_DefService : IB_DefService
    {
        private IB_DefRepository _Repository;
        private IB_DefCacheService _bdefCacheService;

        public B_DefService(IB_DefRepository _Repository, IB_DefCacheService _bdefCacheService)
        {
            this._Repository = _Repository;
            this._bdefCacheService = _bdefCacheService;
        }
        public BasicResponse<Jc_DefInfo> AddDef(B_DefAddRequest defRequest)
        {
            var _def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(defRequest.DefInfo);
            var resultdef = _Repository.AddDef(_def);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = ObjectConverter.Copy<B_DefModel, Jc_DefInfo>(resultdef);
            return defresponse;
        }
        public BasicResponse<Jc_DefInfo> UpdateDef(B_DefUpdateRequest defRequest)
        {
            var _def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(defRequest.DefInfo);
            _Repository.UpdateDef(_def);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = ObjectConverter.Copy<B_DefModel, Jc_DefInfo>(_def);
            return defresponse;
        }
        public BasicResponse DeleteDef(B_DefDeleteRequest defRequest)
        {
            _Repository.DeleteDef(defRequest.Id);
            var defresponse = new BasicResponse();
            return defresponse;
        }
        public BasicResponse<List<Jc_DefInfo>> GetDefList(B_DefGetListRequest defRequest)
        {
            var defresponse = new BasicResponse<List<Jc_DefInfo>>();
            defRequest.PagerInfo.PageIndex = defRequest.PagerInfo.PageIndex - 1;
            if (defRequest.PagerInfo.PageIndex < 0)
            {
                defRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var defModelLists = _Repository.GetDefList(defRequest.PagerInfo.PageIndex, defRequest.PagerInfo.PageSize, out rowcount);
            var defInfoLists = new List<Jc_DefInfo>();
            foreach (var item in defModelLists)
            {
                var DefInfo = ObjectConverter.Copy<B_DefModel, Jc_DefInfo>(item);
                defInfoLists.Add(DefInfo);
            }
            defresponse.Data = defInfoLists;
            return defresponse;
        }
        public BasicResponse<Jc_DefInfo> GetDefById(B_DefGetRequest defRequest)
        {
            var result = _Repository.GetDefById(defRequest.Id);
            var defInfo = ObjectConverter.Copy<B_DefModel, Jc_DefInfo>(result);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = defInfo;
            return defresponse;
        }


        public BasicResponse<List<Jc_DefInfo>> GetAll(BasicRequest defRequest)
        {
            var defresponse = new BasicResponse<List<Jc_DefInfo>>();
            var defModelLists = _Repository.Datas.ToList();
            var definfolists = ObjectConverter.CopyList<B_DefModel, Jc_DefInfo>(defModelLists).ToList();

            //var definfolists = _bdefCacheService.GetAll(new Sys.Safety.Request.Cache.B_DefCacheGetAllRequest()).Data;
            defresponse.Data=definfolists;
            return defresponse;
        }
    }
}


