using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PointDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface ILargeDataAnalysisCacheClientService
    {
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigCache(LargeDataAnalysisCacheClientGetAllRequest getAllRequest);
        /// <summary>
        /// 获取有触发应急联动的实时分析结果.
        /// </summary>
        /// <param name="getAllRequest">没有参数的请求</param>
        /// <returns>触发应急联动的实时分析结果</returns>
        BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigCacheWithEmergencyLinkage(LargeDataAnalysisCacheClientGetAllRequest getAllRequest);

        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <param name="deviceClassCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache(DeviceClassCacheGetAllRequest deviceClassCacheRequest);


        /// <summary>
        ///通过设备种类查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest);

        /// <summary>
        /// 更新大数据分析模型缓存
        /// </summary>
        /// <param name="largeDataAnalysisConfigCache"></param>
        /// <returns></returns>
        BasicResponse UpdateLargeDataAnalysisConfigCahce(LargeDataAnalysisConfigCacheUpdateRequest largeDataAnalysisConfigCache);

        /// <summary>
        /// 根据测点编号获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest pointDefineCacheRequest);
    }
}
