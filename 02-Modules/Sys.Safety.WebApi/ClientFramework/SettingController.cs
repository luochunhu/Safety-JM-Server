using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Setting;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// Setting配置管理WebApi接口
    /// </summary>
    public class SettingController : Basic.Framework.Web.WebApi.BasicApiController, ISettingService
    {
        static SettingController()
        {

        }
        ISettingService _settingService = ServiceFactory.Create<ISettingService>();
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Setting/SaveSetting")]
        public BasicResponse<SettingInfo> SaveSetting(SettingAddRequest settingrequest)
        {
            return _settingService.SaveSetting(settingrequest);
        }
        [HttpPost]
        [Route("v1/Setting/Add")]
        public BasicResponse<SettingInfo> AddSetting(SettingAddRequest settingrequest)
        {
            return _settingService.AddSetting(settingrequest);
        }
        [HttpPost]
        [Route("v1/Setting/Update")]
        public BasicResponse<SettingInfo> UpdateSetting(SettingUpdateRequest settingrequest)
        {
            return _settingService.UpdateSetting(settingrequest);
        }
        [HttpPost]
        [Route("v1/Setting/Delete")]
        public BasicResponse DeleteSetting(SettingDeleteRequest settingrequest)
        {
            return _settingService.DeleteSetting(settingrequest);
        }
        [HttpPost]
        [Route("v1/Setting/GetPageList")]
        public BasicResponse<List<SettingInfo>> GetSettingList(SettingGetListRequest settingrequest)
        {
            return _settingService.GetSettingList(settingrequest);
        }
        [HttpPost]
        [Route("v1/Setting/GetAllList")]
        public BasicResponse<List<SettingInfo>> GetSettingList()
        {
            return _settingService.GetSettingList();
        }
        [HttpPost]
        [Route("v1/Setting/Get")]
        public BasicResponse<SettingInfo> GetSettingById(SettingGetRequest settingrequest)
        {
            return _settingService.GetSettingById(settingrequest);
        }

        /// <summary>
        /// 根据strKey获取settinginfo
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Setting/GetSettingByKey")]
        public BasicResponse<SettingInfo> GetSettingByKey(GetSettingByKeyRequest settingrequest)
        {
            return _settingService.GetSettingByKey(settingrequest);
        }

        /// <summary>
        /// 根据条件保存配置
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Setting/SaveSettingForCondition")]

        public BasicResponse SaveSettingForCondition(SaveSettingForConditionRequest settingrequest)
        {
            return _settingService.SaveSettingForCondition(settingrequest);
        }

        /// <summary>
        /// 根据条件保存配置
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Setting/GetSettingListForCreator")]
        public BasicResponse<List<SettingInfo>> GetSettingListForCreator()
        {
            return _settingService.GetSettingListForCreator();
        }

        [HttpPost]
        [Route("v1/Setting/GetSettingCacheByKey")]
        public BasicResponse<SettingInfo> GetSettingCacheByKey(GetSettingCacheByKeyRequest request)
        {
            return _settingService.GetSettingCacheByKey(request);
        }
    }
}
