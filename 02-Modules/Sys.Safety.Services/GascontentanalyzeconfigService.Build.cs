using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services
{
    public class GascontentanalyzeconfigService : IGascontentanalyzeconfigService
    {
        private readonly IGasContentAnalyzeConfigCacheService _gasContentAnalyzeConfigCacheService;

        private readonly IGascontentanalyzeconfigRepository _Repository;

        private IPointDefineCacheService _pointDefineCacheService;

        public GascontentanalyzeconfigService(IGascontentanalyzeconfigRepository _Repository, IPointDefineCacheService pointDefineCacheService, IGasContentAnalyzeConfigCacheService gasContentAnalyzeConfigCacheService)
        {
            this._Repository = _Repository;
            _pointDefineCacheService = pointDefineCacheService;
            _gasContentAnalyzeConfigCacheService = gasContentAnalyzeConfigCacheService;
        }

        public BasicResponse<GascontentanalyzeconfigInfo> AddGascontentanalyzeconfig(
            GascontentanalyzeconfigAddRequest gascontentanalyzeconfigRequest)
        {
            gascontentanalyzeconfigRequest.GascontentanalyzeconfigInfo.Id = IdHelper.CreateLongId().ToString();
            var _gascontentanalyzeconfig =
                ObjectConverter.Copy<GascontentanalyzeconfigInfo, GascontentanalyzeconfigModel>(
                    gascontentanalyzeconfigRequest.GascontentanalyzeconfigInfo);
            var resultgascontentanalyzeconfig = _Repository.AddGascontentanalyzeconfig(_gascontentanalyzeconfig);
            var gascontentanalyzeconfigresponse = new BasicResponse<GascontentanalyzeconfigInfo>();
            gascontentanalyzeconfigresponse.Data =
                ObjectConverter.Copy<GascontentanalyzeconfigModel, GascontentanalyzeconfigInfo>(
                    resultgascontentanalyzeconfig);

            //更新缓存
            var req = new GasContentAnalyzeConfigAddCacheRequest
            {
                Info = gascontentanalyzeconfigRequest.GascontentanalyzeconfigInfo
            };
            _gasContentAnalyzeConfigCacheService.AddCache(req);

            return gascontentanalyzeconfigresponse;
        }

        public BasicResponse<GascontentanalyzeconfigInfo> UpdateGascontentanalyzeconfig(
            GascontentanalyzeconfigUpdateRequest gascontentanalyzeconfigRequest)
        {
            var _gascontentanalyzeconfig =
                ObjectConverter.Copy<GascontentanalyzeconfigInfo, GascontentanalyzeconfigModel>(
                    gascontentanalyzeconfigRequest.GascontentanalyzeconfigInfo);
            _Repository.UpdateGascontentanalyzeconfig(_gascontentanalyzeconfig);
            var gascontentanalyzeconfigresponse = new BasicResponse<GascontentanalyzeconfigInfo>();
            gascontentanalyzeconfigresponse.Data =
                ObjectConverter.Copy<GascontentanalyzeconfigModel, GascontentanalyzeconfigInfo>(
                    _gascontentanalyzeconfig);

            //更新缓存
            var req = new GasContentAnalyzeConfigUpdateCacheRequest
            {
                Info = gascontentanalyzeconfigRequest.GascontentanalyzeconfigInfo
            };
            _gasContentAnalyzeConfigCacheService.UpdateCache(req);

            return gascontentanalyzeconfigresponse;
        }

        public BasicResponse DeleteGascontentanalyzeconfig(
            GascontentanalyzeconfigDeleteRequest gascontentanalyzeconfigRequest)
        {
            _Repository.DeleteGascontentanalyzeconfig(gascontentanalyzeconfigRequest.Id);

            //更新缓存
            var req = new GasContentAnalyzeConfigDeleteCacheRequest
            {
                Info = new GascontentanalyzeconfigInfo()
                {
                    Id = gascontentanalyzeconfigRequest.Id
                }
            };
            _gasContentAnalyzeConfigCacheService.DeleteCache(req);

            var gascontentanalyzeconfigresponse = new BasicResponse();
            return gascontentanalyzeconfigresponse;
        }

        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetGascontentanalyzeconfigList(
            GascontentanalyzeconfigGetListRequest gascontentanalyzeconfigRequest)
        {
            var gascontentanalyzeconfigresponse = new BasicResponse<List<GascontentanalyzeconfigInfo>>();
            gascontentanalyzeconfigRequest.PagerInfo.PageIndex = gascontentanalyzeconfigRequest.PagerInfo.PageIndex - 1;
            if (gascontentanalyzeconfigRequest.PagerInfo.PageIndex < 0)
                gascontentanalyzeconfigRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var gascontentanalyzeconfigModelLists = _Repository.GetGascontentanalyzeconfigList(
                gascontentanalyzeconfigRequest.PagerInfo.PageIndex, gascontentanalyzeconfigRequest.PagerInfo.PageSize,
                out rowcount);
            var gascontentanalyzeconfigInfoLists = new List<GascontentanalyzeconfigInfo>();
            foreach (var item in gascontentanalyzeconfigModelLists)
            {
                var GascontentanalyzeconfigInfo =
                    ObjectConverter.Copy<GascontentanalyzeconfigModel, GascontentanalyzeconfigInfo>(item);
                gascontentanalyzeconfigInfoLists.Add(GascontentanalyzeconfigInfo);
            }

            gascontentanalyzeconfigresponse.Data = gascontentanalyzeconfigInfoLists;
            return gascontentanalyzeconfigresponse;
        }

        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigList()
        {
            var models = _Repository.Datas.ToList();
            var infos = ObjectConverter.CopyList<GascontentanalyzeconfigModel, GascontentanalyzeconfigInfo>(models).ToList();

            var req = new PointDefineCacheGetAllRequest();
            var res = _pointDefineCacheService.GetAllPointDefineCache(req);
            var allPointInfo = res.Data;
            foreach (var item in infos)
            {
                var pointInfo = allPointInfo.FirstOrDefault(a => a.PointID == item.Pointid);
                if (pointInfo != null)
                {
                    item.Point = pointInfo.Point;
                    item.Location = pointInfo.Wz;
                }
            }

            var ret = new BasicResponse<List<GascontentanalyzeconfigInfo>>
            {
                Data = infos
            };
            return ret;
        }

        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigListCache()
        {
            var infos =_gasContentAnalyzeConfigCacheService.GetAllCache().Data;

            var req = new PointDefineCacheGetAllRequest();
            var res = _pointDefineCacheService.GetAllPointDefineCache(req);
            var allPointInfo = res.Data;
            foreach (var item in infos)
            {
                var pointInfo = allPointInfo.FirstOrDefault(a => a.PointID == item.Pointid);
                if (pointInfo != null)
                {
                    item.Point = pointInfo.Point;
                    item.Location = pointInfo.Wz;
                }
            }

            var ret = new BasicResponse<List<GascontentanalyzeconfigInfo>>
            {
                Data = infos
            };
            return ret;
        }

        public BasicResponse<GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigById(
            GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            var result = _Repository.GetGascontentanalyzeconfigById(gascontentanalyzeconfigRequest.Id);
            var gascontentanalyzeconfigInfo =
                ObjectConverter.Copy<GascontentanalyzeconfigModel, GascontentanalyzeconfigInfo>(result);
            var gascontentanalyzeconfigresponse = new BasicResponse<GascontentanalyzeconfigInfo>();
            gascontentanalyzeconfigresponse.Data = gascontentanalyzeconfigInfo;
            return gascontentanalyzeconfigresponse;
        }

        public BasicResponse<GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigCacheById(
            GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            var req = new GasContentAnalyzeConfigGetCacheByConditionRequest
            {
                Condition = a => a.Id == gascontentanalyzeconfigRequest.Id
            };
            var gascontentanalyzeconfigInfo = _gasContentAnalyzeConfigCacheService.GetCacheByCondition(req).Data[0];
            var gascontentanalyzeconfigresponse = new BasicResponse<GascontentanalyzeconfigInfo>();
            gascontentanalyzeconfigresponse.Data = gascontentanalyzeconfigInfo;
            return gascontentanalyzeconfigresponse;
        }
    }
}