using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Services
{
    public class LargeDataAnalysisCacheClientService : ILargeDataAnalysisCacheClientService
    {
        private ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService;
        private IDeviceClassCacheService deviceClassCacheService;
        private IPointDefineService pointDefineService;
        private IPointDefineCacheService pointDefineCacheService;
        public LargeDataAnalysisCacheClientService(ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService,
            IDeviceClassCacheService deviceClassCacheService,
            IPointDefineService pointDefineService,
            IPointDefineCacheService pointDefineCacheService)
        {
            this.largeDataAnalysisConfigCacheService = largeDataAnalysisConfigCacheService;
            this.deviceClassCacheService = deviceClassCacheService;
            this.pointDefineService = pointDefineService;
            this.pointDefineCacheService = pointDefineCacheService;
        }

        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest)
        {
            return deviceClassCacheService.GetAllDeviceClassCache(deviceClassCacheRequest);
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            return largeDataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(new Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetAllRequest());
        }

        /// <summary>
        /// 获取有触发应急联动的实时分析结果.
        /// </summary>
        /// <param name="getAllRequest">没有参数的请求</param>
        /// <returns>触发应急联动的实时分析结果</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCacheWithEmergencyLinkage(LargeDataAnalysisCacheClientGetAllRequest getAllRequest)
        {
            return largeDataAnalysisConfigCacheService.GetLargeDataAnalysisConfigCache(new Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetByConditonRequest() {
                Predicate = (analysisConfig) => { return analysisConfig.AnalysisResult == 2 && analysisConfig.IsEmergencyLinkage && analysisConfig.EmergencyLinkageConfig != ""; }
            });
        }

        /// <summary>
        /// 通过设备种类查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            return pointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);
        }

        /// <summary>
        /// 更新大数据分析模型缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        public BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache)
        {
            var requestInfo = largeDataAnalysisConfigCache.LargeDataAnalysisConfigInfo;
            //BasicResponse<JC_LargedataAnalysisConfigInfo> findResponse = largeDataAnalysisConfigCacheService.GetLargeDataAnalysisConfigCacheByKey(new LargeDataAnalysisConfigCacheGetByKeyRequest() { Id = requestInfo.Id });
            //if (!findResponse.IsSuccess || findResponse.Data == null || findResponse.Data.UpdatedTime > requestInfo.UpdatedTime)
            //    return new BasicResponse();
            largeDataAnalysisConfigCacheService.UpdateLargeDataAnalysisConfigCahce(largeDataAnalysisConfigCache);
            return new BasicResponse();
        }

        /// <summary>
        /// 根据测点编号获取测点定义缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest request)
        {
            var response = pointDefineCacheService.PointDefineCacheByPointIdRequeest(request);
            return response;
        }
    }
}
