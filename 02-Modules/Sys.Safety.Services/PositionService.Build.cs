using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Position;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Data;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 安装位置管理服务
    /// </summary>
    public partial class PositionService : IPositionService
    {
        private IPositionRepository _Repository;
        private IPositionCacheService _PositionCacheService;

        public PositionService(IPositionRepository _Repository, IPositionCacheService _PositionCacheService)
        {
            this._Repository = _Repository;
            this._PositionCacheService = _PositionCacheService;
        }
        /// <summary>
        ///添加安装位置
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPosition(PositionAddRequest PositionRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_WzInfo item = PositionRequest.PositionInfo;
            Jc_WzInfo olditem = null;

            PositionCacheGetByKeyRequest positionCacheRequest = new PositionCacheGetByKeyRequest();
            positionCacheRequest.PositionId = item.ID;
            var result = _PositionCacheService.GetPositionCacheByKey(positionCacheRequest);
            olditem = result.Data;

            //增加重复判断
            if (result.Data != null)
            { //缓存中存在此测点
                Result.Code = 1;
                Result.Message = "当前添加的安装位置已存在！";
                return Result;
            }
            //保存数据库
            var _jc_Wz = ObjectConverter.Copy<Jc_WzInfo, Jc_WzModel>(PositionRequest.PositionInfo);
            var resultjc_Wz = _Repository.AddPosition(_jc_Wz);

            //保存缓存
            PositionCacheAddRequest AddPositionCacheRequest = new PositionCacheAddRequest();
            AddPositionCacheRequest.PositionInfo = item;
            _PositionCacheService.AddPositionCache(AddPositionCacheRequest);

            return Result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPositions(PositionsRequest PositionRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_WzInfo> items = PositionRequest.PositionsInfo;
            List<Jc_WzInfo> PositionCaches = new List<Jc_WzInfo>();
            Jc_WzInfo olditem = null;

            PositionCacheGetAllRequest positionCacheRequest = new PositionCacheGetAllRequest();
            var result = _PositionCacheService.GetAllPositongCache(positionCacheRequest);
            PositionCaches = result.Data;

            foreach (Jc_WzInfo item in items)
            {
                olditem = PositionCaches.Find(a => a.WzID == item.WzID);
                //增加重复判断
                if (result.Data != null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前添加的安装位置已存在！";
                    return Result;
                }
            }


            TransactionsManager.BeginTransaction(() =>
               {
                   foreach (Jc_WzInfo item in items)
                   {
                       //保存数据库
                       var _jc_Wz = ObjectConverter.Copy<Jc_WzInfo, Jc_WzModel>(item);
                       var resultjc_Wz = _Repository.AddPosition(_jc_Wz);
                   }

                   //保存缓存
                   PositionCacheBatchAddRequest BatchAddPositionCacheRequest = new PositionCacheBatchAddRequest();
                   BatchAddPositionCacheRequest.PositionInfos = items;
                   _PositionCacheService.BatchAddPositionCache(BatchAddPositionCacheRequest);
               });

            return Result;
        }
        /// <summary>
        /// 安装位置更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePosition(PositionUpdateRequest PositionRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_WzInfo item = PositionRequest.PositionInfo;        

            PositionCacheGetByKeyRequest positionCacheRequest = new PositionCacheGetByKeyRequest();
            positionCacheRequest.PositionId = item.WzID;
            var result = _PositionCacheService.GetPositionCacheByKey(positionCacheRequest);           

            //增加判断
            if (result.Data == null)
            { //缓存中存在此测点
                Result.Code = 1;
                Result.Message = "当前更新的安装位置不存在！";
                return Result;
            }
            //保存数据库
            var _jc_Wz = ObjectConverter.Copy<Jc_WzInfo, Jc_WzModel>(PositionRequest.PositionInfo);
            _Repository.UpdatePosition(_jc_Wz);
            //保存缓存
            PositionCacheUpdateRequest UpdatePositionCacheRequest = new PositionCacheUpdateRequest();
            UpdatePositionCacheRequest.PositionInfo = item;
            _PositionCacheService.UpdatePositionCache(UpdatePositionCacheRequest);

            return Result;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePositions(PositionsRequest PositionRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_WzInfo> items = PositionRequest.PositionsInfo;
            List<Jc_WzInfo> PositionCaches = new List<Jc_WzInfo>();
            Jc_WzInfo olditem = null;

            PositionCacheGetAllRequest positionCacheRequest = new PositionCacheGetAllRequest();
            var result = _PositionCacheService.GetAllPositongCache(positionCacheRequest);
            PositionCaches = result.Data;

            foreach (Jc_WzInfo item in items)
            {
                olditem = PositionCaches.Find(a => a.WzID == item.WzID);
                //增加重复判断
                if (result.Data == null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前更新的安装位置不存在！";
                    return Result;
                }
            }


            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_WzInfo item in items)
                {
                    //保存数据库
                    var _jc_Wz = ObjectConverter.Copy<Jc_WzInfo, Jc_WzModel>(item);
                    _Repository.UpdatePosition(_jc_Wz);
                }

                //保存缓存
                PositionCacheBatchUpdateRequest BatchUpdatePositionCacheRequest = new PositionCacheBatchUpdateRequest();
                BatchUpdatePositionCacheRequest.PositionInfos = items;
                _PositionCacheService.BatchUpdatePositionCache(BatchUpdatePositionCacheRequest);
            });

            return Result;
        }
        public BasicResponse DeletePosition(PositionDeleteRequest PositionRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_WzInfo item = new Jc_WzInfo();           

            PositionCacheGetByKeyRequest positionCacheRequest = new PositionCacheGetByKeyRequest();
            positionCacheRequest.PositionId = PositionRequest.Id;
            var result = _PositionCacheService.GetPositionCacheByKey(positionCacheRequest);
            item = result.Data;
            //增加判断
            if (result.Data == null)
            { //缓存中存在此测点
                Result.Code = 1;
                Result.Message = "当前删除的安装位置不存在！";
                return Result;
            }

            //数据库操作
            _Repository.DeletePosition(item.WzID);
           
            //保存缓存
            PositionCacheDeleteRequest positionDeleteCacheRequest = new PositionCacheDeleteRequest();
            positionDeleteCacheRequest.PositionInfo = item;
            _PositionCacheService.DeletePositionCache(positionDeleteCacheRequest);

            return Result;
        }
        public BasicResponse<List<Jc_WzInfo>> GetPositionList(PositionGetListRequest PositionRequest)
        {
            var jc_Wzresponse = new BasicResponse<List<Jc_WzInfo>>();
            PositionRequest.PagerInfo.PageIndex = PositionRequest.PagerInfo.PageIndex - 1;
            if (PositionRequest.PagerInfo.PageIndex < 0)
            {
                PositionRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_WzModelLists = _Repository.GetPositionList(PositionRequest.PagerInfo.PageIndex, PositionRequest.PagerInfo.PageSize, out rowcount);
            var jc_WzInfoLists = new List<Jc_WzInfo>();
            foreach (var item in jc_WzModelLists)
            {
                var Jc_WzInfo = ObjectConverter.Copy<Jc_WzModel, Jc_WzInfo>(item);
                jc_WzInfoLists.Add(Jc_WzInfo);
            }
            jc_Wzresponse.Data = jc_WzInfoLists;
            return jc_Wzresponse;
        }
        public BasicResponse<List<Jc_WzInfo>> GetPositionList()
        {
            var jc_Wzresponse = new BasicResponse<List<Jc_WzInfo>>();           
            var jc_WzModelLists = _Repository.GetPositionList();
            var jc_WzInfoLists = new List<Jc_WzInfo>();
            foreach (var item in jc_WzModelLists)
            {
                var Jc_WzInfo = ObjectConverter.Copy<Jc_WzModel, Jc_WzInfo>(item);
                jc_WzInfoLists.Add(Jc_WzInfo);
            }
            jc_Wzresponse.Data = jc_WzInfoLists;
            return jc_Wzresponse;
        }
        public BasicResponse<Jc_WzInfo> GetPositionById(PositionGetRequest PositionRequest)
        {
            var result = _Repository.GetPositionById(PositionRequest.Id);
            var jc_WzInfo = ObjectConverter.Copy<Jc_WzModel, Jc_WzInfo>(result);
            var jc_Wzresponse = new BasicResponse<Jc_WzInfo>();
            jc_Wzresponse.Data = jc_WzInfo;
            return jc_Wzresponse;
        }
        /// <summary>
        /// 获取所有安装位置缓存
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_WzInfo>> GetAllPositionCache()
        {
            BasicResponse<List<Jc_WzInfo>> Result = new BasicResponse<List<Jc_WzInfo>>();
            PositionCacheGetAllRequest positionCacheRequest = new PositionCacheGetAllRequest();
            var result = _PositionCacheService.GetAllPositongCache(positionCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 根据安装位置ID获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWzID(PositionGetByWzIDRequest PositionRequest)
        {
            BasicResponse<List<Jc_WzInfo>> Result = new BasicResponse<List<Jc_WzInfo>>();
            PositionCacheGetByConditionRequest positionCacheRequest = new PositionCacheGetByConditionRequest();
            positionCacheRequest.Predicate = a => a.WzID == PositionRequest.WzID;
            var result = _PositionCacheService.GetPositionCache(positionCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 根据安装位置获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest)
        {
            BasicResponse<List<Jc_WzInfo>> Result = new BasicResponse<List<Jc_WzInfo>>();
            PositionCacheGetByConditionRequest positionCacheRequest = new PositionCacheGetByConditionRequest();
            positionCacheRequest.Predicate = a => a.Wz == PositionRequest.Wz;
            var result = _PositionCacheService.GetPositionCache(positionCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取缓存中当前最大的位置ID
        /// </summary>
        /// <returns></returns>
        public BasicResponse<long> GetMaxPositionId()
        {
            BasicResponse<long> Result = new BasicResponse<long>();
            long MaxPositionId = 0;
            PositionCacheGetAllRequest positionCacheRequest = new PositionCacheGetAllRequest();
            var result = _PositionCacheService.GetAllPositongCache(positionCacheRequest);
            foreach (Jc_WzInfo item in result.Data)
            {
                if (item.InfoState == InfoState.Delete)
                {
                    continue;
                }
                if (long.Parse(item.WzID) > MaxPositionId)
                {
                    MaxPositionId = long.Parse(item.WzID);
                }
            }
            Result.Data = MaxPositionId;
            return Result;
        }

        public BasicResponse<string> AddPositionBySql(AddPositionBySqlRequest request)
        {
            var id = IdHelper.CreateLongId().ToString();
            var dt = _Repository.QueryTable("global_PointDefineService_AddPosition", id, request.Wz, DateTime.Now);
            return new BasicResponse<string>()
            {
                Data = dt.Rows[0][0].ToString()
            };
        }
    }
}


