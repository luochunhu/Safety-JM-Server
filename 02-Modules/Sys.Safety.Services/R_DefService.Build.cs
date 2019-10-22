using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.Position;
using Sys.Safety.Request.R_Def;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.KJ237Cache;

namespace Sys.Safety.Services
{
    public partial class R_DefService : IR_DefService
    {
        private IR_DefRepository _Repository;

        //private IAreaService _areaService;

        //private IDeviceDefineCacheService _deviceDefineCacheService;

        //private IPositionService _positionService;

        //private IDevicePropertyCacheService _devicePropertyCacheService;

        //private IDeviceClassCacheService _deviceClassCacheService;

        //private IDeviceTypeCacheService _deviceTypeCacheService;

        //private IRPointDefineCacheService _rPointDefineCacheService;

        public R_DefService(IR_DefRepository _Repository
            //, IAreaService areaService,
            //IDeviceDefineCacheService deviceDefineCacheService, IPositionService positionService,
            //IDevicePropertyCacheService devicePropertyCacheService, IDeviceClassCacheService deviceClassCacheService,
            //IDeviceTypeCacheService deviceTypeCacheService, IRPointDefineCacheService rPointDefineCacheService
            )
        {
            this._Repository = _Repository;
            //_areaService = areaService;
            //_deviceDefineCacheService = deviceDefineCacheService;
            //_positionService = positionService;
            //_devicePropertyCacheService = devicePropertyCacheService;
            //_deviceClassCacheService = deviceClassCacheService;
            //_deviceTypeCacheService = deviceTypeCacheService;
            //_rPointDefineCacheService = rPointDefineCacheService;
        }

        public BasicResponse<Jc_DefInfo> AddDef(R_DefAddRequest defRequest)
        {
            var _def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(defRequest.DefInfo);
            var resultdef = _Repository.AddDef(_def);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = ObjectConverter.Copy<R_DefModel, Jc_DefInfo>(resultdef);
            return defresponse;
        }
        public BasicResponse<Jc_DefInfo> UpdateDef(R_DefUpdateRequest defRequest)
        {
            var _def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(defRequest.DefInfo);
            _Repository.UpdateDef(_def);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = ObjectConverter.Copy<R_DefModel, Jc_DefInfo>(_def);
            return defresponse;
        }
        public BasicResponse DeleteDef(R_DefDeleteRequest defRequest)
        {
            _Repository.DeleteDef(defRequest.Id);
            var defresponse = new BasicResponse();
            return defresponse;
        }
        public BasicResponse<List<Jc_DefInfo>> GetDefList(R_DefGetListRequest defRequest)
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
                var DefInfo = ObjectConverter.Copy<R_DefModel, Jc_DefInfo>(item);
                defInfoLists.Add(DefInfo);
            }
            defresponse.Data = defInfoLists;
            return defresponse;
        }
        public BasicResponse<Jc_DefInfo> GetDefById(R_DefGetRequest defRequest)
        {
            var result = _Repository.GetDefById(defRequest.Id);
            var defInfo = ObjectConverter.Copy<R_DefModel, Jc_DefInfo>(result);
            var defresponse = new BasicResponse<Jc_DefInfo>();
            defresponse.Data = defInfo;
            return defresponse;
        }

        public BasicResponse<List<Jc_DefInfo>> GetAllDefInfo()
        {
            var defresponse = new BasicResponse<List<Jc_DefInfo>>();
            var defModelLists = _Repository.Datas.ToList();
            var defInfoLists = ObjectConverter.CopyList<R_DefModel, Jc_DefInfo>(defModelLists).ToList();
            defresponse.Data = defInfoLists;
            return defresponse;
        }
    }
}


