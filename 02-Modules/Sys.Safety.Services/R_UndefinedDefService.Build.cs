using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.UndefinedDef;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.Services
{
    public partial class R_UndefinedDefService : IR_UndefinedDefService
    {
        private IR_UndefinedDefRepository _Repository;
        private IRUndefinedDefCacheService _RUndefinedDefCacheService;

        public R_UndefinedDefService(IR_UndefinedDefRepository _Repository, IRUndefinedDefCacheService _RUndefinedDefCacheService)
        {
            this._Repository = _Repository;
            this._RUndefinedDefCacheService = _RUndefinedDefCacheService;
        }
        public BasicResponse<R_UndefinedDefInfo> AddUndefinedDef(R_UndefinedDefAddRequest undefinedDefRequest)
        {
            var undefinedDefresponse = new BasicResponse<R_UndefinedDefInfo>();
            //判断缓存中是否存在
            R_UndefinedDefInfo oldR_UndefinedDefInfo = null;
            RUndefinedDefCacheGetByKeyRequest RUndefinedDefCacheRequest = new RUndefinedDefCacheGetByKeyRequest();
            RUndefinedDefCacheRequest.Id = undefinedDefRequest.UndefinedDefInfo.Id;
            oldR_UndefinedDefInfo = _RUndefinedDefCacheService.GetByKeyRUndefinedDefCache(RUndefinedDefCacheRequest).Data;
            if (oldR_UndefinedDefInfo != null)
            {
                //缓存中存在此测点
                undefinedDefresponse.Code = 1;
                undefinedDefresponse.Message = "当前添加的人员未定义设备已存在！";               
                return undefinedDefresponse;
            }

            var _undefinedDef = ObjectConverter.Copy<R_UndefinedDefInfo, R_UndefinedDefModel>(undefinedDefRequest.UndefinedDefInfo);
            var resultundefinedDef = _Repository.AddUndefinedDef(_undefinedDef);

            //更新缓存
            RUndefinedDefCacheAddRequest RUndefinedDefCacheUpdateRequest = new RUndefinedDefCacheAddRequest();
            RUndefinedDefCacheUpdateRequest.RUndefinedDefInfo = undefinedDefRequest.UndefinedDefInfo;
            _RUndefinedDefCacheService.AddRUndefinedDefCache(RUndefinedDefCacheUpdateRequest);

            undefinedDefresponse.Data = ObjectConverter.Copy<R_UndefinedDefModel, R_UndefinedDefInfo>(resultundefinedDef);
            return undefinedDefresponse;
        }
        public BasicResponse<R_UndefinedDefInfo> UpdateUndefinedDef(R_UndefinedDefUpdateRequest undefinedDefRequest)
        {
            var undefinedDefresponse = new BasicResponse<R_UndefinedDefInfo>();
            //判断缓存中是否存在
            R_UndefinedDefInfo oldR_UndefinedDefInfo = null;
            RUndefinedDefCacheGetByKeyRequest RUndefinedDefCacheRequest = new RUndefinedDefCacheGetByKeyRequest();
            RUndefinedDefCacheRequest.Id = undefinedDefRequest.UndefinedDefInfo.Id;
            oldR_UndefinedDefInfo = _RUndefinedDefCacheService.GetByKeyRUndefinedDefCache(RUndefinedDefCacheRequest).Data;
            if (oldR_UndefinedDefInfo == null)
            {
                //缓存中存在此测点
                undefinedDefresponse.Code = 1;
                undefinedDefresponse.Message = "当前更新的人员未定义设备不存在！";
                return undefinedDefresponse;
            }

            var _undefinedDef = ObjectConverter.Copy<R_UndefinedDefInfo, R_UndefinedDefModel>(undefinedDefRequest.UndefinedDefInfo);
            _Repository.UpdateUndefinedDef(_undefinedDef);

            //更新缓存
            RUndefinedDefCacheUpdateRequest RUndefinedDefCacheUpdateRequest = new RUndefinedDefCacheUpdateRequest();
            RUndefinedDefCacheUpdateRequest.RUndefinedDefInfo = undefinedDefRequest.UndefinedDefInfo;
            _RUndefinedDefCacheService.UpdateRUndefinedDefCache(RUndefinedDefCacheUpdateRequest);

            undefinedDefresponse.Data = ObjectConverter.Copy<R_UndefinedDefModel, R_UndefinedDefInfo>(_undefinedDef);
            return undefinedDefresponse;
        }
        public BasicResponse DeleteUndefinedDef(R_UndefinedDefDeleteRequest undefinedDefRequest)
        {
            var undefinedDefresponse = new BasicResponse();
            //判断缓存中是否存在
            R_UndefinedDefInfo oldR_UndefinedDefInfo = null;
            RUndefinedDefCacheGetByKeyRequest RUndefinedDefCacheRequest = new RUndefinedDefCacheGetByKeyRequest();
            RUndefinedDefCacheRequest.Id = undefinedDefRequest.Id;
            oldR_UndefinedDefInfo = _RUndefinedDefCacheService.GetByKeyRUndefinedDefCache(RUndefinedDefCacheRequest).Data;
            if (oldR_UndefinedDefInfo == null)
            {
                //缓存中存在此测点
                undefinedDefresponse.Code = 1;
                undefinedDefresponse.Message = "当前删除的人员未定义设备不存在！";
                return undefinedDefresponse;
            }

            _Repository.DeleteUndefinedDef(undefinedDefRequest.Id);

            //更新缓存
            RUndefinedDefCacheDeleteRequest RUndefinedDefCacheDeleteRequest = new Sys.Safety.Request.PersonCache.RUndefinedDefCacheDeleteRequest();
            RUndefinedDefCacheDeleteRequest.RUndefinedDefInfo = oldR_UndefinedDefInfo;
            _RUndefinedDefCacheService.DeleteRUndefinedDefCache(RUndefinedDefCacheDeleteRequest);

            return undefinedDefresponse;
        }
        public BasicResponse<List<R_UndefinedDefInfo>> GetUndefinedDefList(R_UndefinedDefGetListRequest undefinedDefRequest)
        {
            var undefinedDefresponse = new BasicResponse<List<R_UndefinedDefInfo>>();
            undefinedDefRequest.PagerInfo.PageIndex = undefinedDefRequest.PagerInfo.PageIndex - 1;
            if (undefinedDefRequest.PagerInfo.PageIndex < 0)
            {
                undefinedDefRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var undefinedDefModelLists = _Repository.GetUndefinedDefList(undefinedDefRequest.PagerInfo.PageIndex, undefinedDefRequest.PagerInfo.PageSize, out rowcount);
            var undefinedDefInfoLists = new List<R_UndefinedDefInfo>();
            foreach (var item in undefinedDefModelLists)
            {
                var UndefinedDefInfo = ObjectConverter.Copy<R_UndefinedDefModel, R_UndefinedDefInfo>(item);
                undefinedDefInfoLists.Add(UndefinedDefInfo);
            }
            undefinedDefresponse.Data = undefinedDefInfoLists;
            return undefinedDefresponse;
        }
        public BasicResponse<R_UndefinedDefInfo> GetUndefinedDefById(R_UndefinedDefGetRequest undefinedDefRequest)
        {
            var result = _Repository.GetUndefinedDefById(undefinedDefRequest.Id);
            var undefinedDefInfo = ObjectConverter.Copy<R_UndefinedDefModel, R_UndefinedDefInfo>(result);
            var undefinedDefresponse = new BasicResponse<R_UndefinedDefInfo>();
            undefinedDefresponse.Data = undefinedDefInfo;
            return undefinedDefresponse;
        }
        /// <summary>
        /// 获取所有未定义设备  20171127
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        public Basic.Framework.Web.BasicResponse<List<DataContract.R_UndefinedDefInfo>> GetAllRUndefinedDefCache(Sys.Safety.Request.PersonCache.RUndefinedDefCacheGetAllRequest RUndefinedDefCacheRequest)
        {
            return _RUndefinedDefCacheService.GetAllRUndefinedDefCache(RUndefinedDefCacheRequest);
        }
        
    }
}


