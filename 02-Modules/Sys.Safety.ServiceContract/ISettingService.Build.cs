using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Setting;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ISettingService
    {
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        BasicResponse<SettingInfo> SaveSetting(SettingAddRequest settingrequest);

        BasicResponse<SettingInfo> AddSetting(SettingAddRequest settingrequest);
        BasicResponse<SettingInfo> UpdateSetting(SettingUpdateRequest settingrequest);
        BasicResponse DeleteSetting(SettingDeleteRequest settingrequest);
        BasicResponse<List<SettingInfo>> GetSettingList(SettingGetListRequest settingrequest);
        BasicResponse<List<SettingInfo>> GetSettingList();
        BasicResponse<SettingInfo> GetSettingById(SettingGetRequest settingrequest);
        /// <summary>
        /// 根据strKey获取settinginfo
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        BasicResponse<SettingInfo> GetSettingByKey(GetSettingByKeyRequest settingrequest);

        /// <summary>
        /// 根据条件保存配置
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        BasicResponse SaveSettingForCondition(SaveSettingForConditionRequest settingrequest);

        /// <summary>
        /// 根据创建者获取GetSettingList
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SettingInfo>> GetSettingListForCreator();

        /// <summary>
        /// 获取setting缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<SettingInfo> GetSettingCacheByKey(GetSettingCacheByKeyRequest request);
    }
}

