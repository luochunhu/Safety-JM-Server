using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.Enums;
using Sys.Safety.Enums.Enums;
using Sys.Safety.Request.Area;
using System;

namespace Sys.Safety.Services
{
    public partial class V_DefService : IV_DefService
    {
        private readonly IV_DefRepository _Repository;

        private readonly IAreaService _areaService;

        private readonly IV_DefCacheService _vdefCacheService;

        public V_DefService(IV_DefRepository _Repository)
        {
            this._Repository = _Repository;
            _areaService = ServiceFactory.Create<IAreaService>();
            _vdefCacheService = ServiceFactory.Create<IV_DefCacheService>();
        }
        public BasicResponse<V_DefInfo> AddDef(DefAddRequest defRequest)
        {
            var _def = ObjectConverter.Copy<V_DefInfo, V_DefModel>(defRequest.DefInfo);
            var resultdef = _Repository.AddDef(_def);
            var defresponse = new BasicResponse<V_DefInfo>();

            var addinfo = ObjectConverter.Copy<V_DefModel, V_DefInfo>(resultdef);
            _vdefCacheService.Insert(new V_DefCacheInsertRequest { V_DefInfo = addinfo });
           
            defresponse.Data = addinfo;
            return defresponse;
        }
        public BasicResponse<V_DefInfo> UpdateDef(DefUpdateRequest defRequest)
        {
            var _def = ObjectConverter.Copy<V_DefInfo, V_DefModel>(defRequest.DefInfo);
            _Repository.UpdateDef(_def);
            var defresponse = new BasicResponse<V_DefInfo>();

            var updateinfo = ObjectConverter.Copy<V_DefModel, V_DefInfo>(_def);
            _vdefCacheService.Update(new V_DefCacheInsertRequest { V_DefInfo = updateinfo });

            defresponse.Data = updateinfo;

            defresponse.Data = ObjectConverter.Copy<V_DefModel, V_DefInfo>(_def);
            return defresponse;
        }
        public BasicResponse DeleteDef(DefDeleteRequest defRequest)
        {
            var deletemodel=_Repository.Datas.FirstOrDefault(o=>o.Id==defRequest.Id);
             var deleteinfo = ObjectConverter.Copy<V_DefModel, V_DefInfo>(deletemodel);
            _Repository.DeleteDef(defRequest.Id);

            _vdefCacheService.Delete(new V_DefCacheDeleteRequest { V_DefInfo = deleteinfo });
            var defresponse = new BasicResponse();
            return defresponse;
        }
        public BasicResponse<List<V_DefInfo>> GetDefList(DefGetListRequest defRequest)
        {
            var defresponse = new BasicResponse<List<V_DefInfo>>();
            defRequest.PagerInfo.PageIndex = defRequest.PagerInfo.PageIndex - 1;
            if (defRequest.PagerInfo.PageIndex < 0)
            {
                defRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var defModelLists = _Repository.GetDefList(defRequest.PagerInfo.PageIndex, defRequest.PagerInfo.PageSize, out rowcount);
            var defInfoLists = new List<V_DefInfo>();
            foreach (var item in defModelLists)
            {
                var DefInfo = ObjectConverter.Copy<V_DefModel, V_DefInfo>(item);
                defInfoLists.Add(DefInfo);
            }
            defresponse.Data = defInfoLists;
            return defresponse;
        }
        public BasicResponse<V_DefInfo> GetDefById(DefGetRequest defRequest)
        {
            //var result = _Repository.GetDefById(defRequest.Id);
            //var defInfo = ObjectConverter.Copy<V_DefModel, V_DefInfo>(result);
            //var defresponse = new BasicResponse<V_DefInfo>();
            //defresponse.Data = defInfo;
            //return defresponse;

            var defInfo = _vdefCacheService.GetById(new V_DefCacheGetByIdRequest() { Id = defRequest .Id}).Data;
            var defresponse = new BasicResponse<V_DefInfo>();
            defresponse.Data = defInfo;
            return defresponse;
        }


        public BasicResponse<List<V_DefInfo>> GetAllDef(DefGetAllRequest defRequest)
        {
            var defresponse = new BasicResponse<List<V_DefInfo>>();
            var defmodellist = _Repository.Datas.ToList();

            var areas = _areaService.GetAllAreaList(new AreaGetListRequest()).Data;

            var definfolist = ObjectConverter.CopyList<V_DefModel, V_DefInfo>(defmodellist).ToList();
            definfolist.ForEach(o =>
            {
                if (!string.IsNullOrEmpty(o.AreaId))
                {
                    o.AreaId = o.AreaId;
                    var area = areas.FirstOrDefault(a => a.Areaid == o.AreaId);
                    o.By1 = area == null ? string.Empty : area.Areaname;
                }
                o.By2 = EnumHelper.GetEnumDescription((VideoVendorType)o.Vendor);
            });

            defresponse.Data = definfolist;
            return defresponse;
        }


        public BasicResponse<List<V_DefInfo>> GetAllVideoDefCache()
        {
            var definfolist = _vdefCacheService.GetAll(new V_DefCacheGetAllRequest()).Data;
            var response = new BasicResponse<List<V_DefInfo>>();
            response.Data = definfolist;
            return response;
        }


        public BasicResponse<V_DefInfo> GetDefByIP(DefIPRequest defRequest)
        {
            Func<V_DefInfo, bool> predicate=v=>v.IPAddress==defRequest.IPAddress;
            var definfo = _vdefCacheService.Get(new V_DefCacheGetByConditionRequest { predicate = predicate }).Data.FirstOrDefault();
            BasicResponse<V_DefInfo> response = new BasicResponse<V_DefInfo>();
            response.Data = definfo;
            return response;
        }
    }
}


