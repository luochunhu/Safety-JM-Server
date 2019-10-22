using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:设置缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface ISettingCacheService
    {
        /// <summary>
        /// 加载设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse LoadSettingCache(SettingCacheLoadRequest settingCacheRequest);

        /// <summary>
        /// 添加设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse AddSettingCache(SettingCacheAddRequest settingCacheRequest);

        /// <summary>
        /// 修改设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdateSettingCache(SettingCacheUpdateRequest settingCacheRequest);

        /// <summary>
        /// 添加或修改设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse AddOrUpdateSettingCache(ConfigCacheAddOrUpdateRequest settingCacheRequest);

        /// <summary>
        /// 查询所有设置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SettingInfo>> GetAllSettingCache(SettingCacheGetAllRequest settingCacheRequest);

        /// <summary>
        /// 根据Key(StrKey)查询配置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<SettingInfo> GetSettingCacheByKey(SettingCacheGetByKeyRequest settingCacheRequest);

        /// <summary>
        /// 根据条件查询配置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SettingInfo>> GetSettingCace(SettingCacheGetByConditonRequest settingCacheRequest);
    }
}
