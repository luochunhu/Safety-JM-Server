using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface ISysEmergencyLinkageCacheService
    {
        /// <summary>
        /// 加载设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse LoadSysEmergencyLinkageCache(EmergencyLinkageConfigCacheLoadRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 添加设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse AddSysEmergencyLinkageCache(EmergencyLinkageConfigCacheAddRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 修改设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdateSysEmergencyLinkageCache(EmergencyLinkageConfigCacheUpdateRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 删除设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse DeleteSysEmergencyLinkageCache(EmergencyLinkageConfigCacheDeleteRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 查询所有设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageCache(EmergencyLinkageConfigCacheGetAllRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 根据Key(StrKey)查询配置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<SysEmergencyLinkageInfo> GetSysEmergencyLinkageCacheByKey(EmergencyLinkageConfigCacheGetByKeyRequest EmergencyLinkageConfigCacheRequest);

        /// <summary>
        /// 根据条件查询配置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SysEmergencyLinkageInfo>> GetSysEmergencyLinkageCache(EmergencyLinkageConfigCacheGetByConditonRequest EmergencyLinkageConfigCacheRequest);    
    }
}
