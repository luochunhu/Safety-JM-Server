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

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// Config配置管理WebApi接口
    /// </summary>
    public class ConfigController : Basic.Framework.Web.WebApi.BasicApiController, IConfigService
    {
        static ConfigController()
        {

        }
        IConfigService _configService = ServiceFactory.Create<IConfigService>();
        [HttpPost]
        [Route("v1/Config/Add")]
        public BasicResponse<ConfigInfo> AddConfig(ConfigAddRequest configrequest)
        {
            return _configService.AddConfig(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/Update")]
        public BasicResponse<ConfigInfo> UpdateConfig(ConfigUpdateRequest configrequest)
        {
            return _configService.UpdateConfig(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/Delete")]
        public BasicResponse DeleteConfig(ConfigDeleteRequest configrequest)
        {
            return _configService.DeleteConfig(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/GetPageList")]
        public BasicResponse<List<ConfigInfo>> GetConfigList(ConfigGetListRequest configrequest)
        {
            return _configService.GetConfigList(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/GetList")]
        public BasicResponse<List<ConfigInfo>> GetConfigList()
        {
            return _configService.GetConfigList();
        }
        [HttpPost]
        [Route("v1/Config/Get")]
        public BasicResponse<ConfigInfo> GetConfigById(ConfigGetRequest configrequest)
        {
            return _configService.GetConfigById(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/GetByName")]
        public BasicResponse<ConfigInfo> GetConfigByName(ConfigGetByNameRequest configrequest)
        {
            return _configService.GetConfigByName(configrequest);
        }
        [HttpPost]
        [Route("v1/Config/SaveInspection")]
        public BasicResponse SaveInspection()
        {
            return _configService.SaveInspection();
        }
        [HttpPost]
        [Route("v1/Config/GetRunningInfo")]
        public BasicResponse<RunningInfo>  GetRunningInfo()
        {
            return _configService.GetRunningInfo();
        }
        [HttpPost]
        [Route("v1/Config/GetDiskInfo")]
        public BasicResponse<HardDiskInfo>  GetDiskInfo(ConfigGetDiskInfoRequest request)
        {
            return _configService.GetDiskInfo(request);
        }
        [HttpPost]
        [Route("v1/Config/GetProcessInfo")]
        public BasicResponse<PorcessInfo> GetProcessInfo(ConfigGetProcessInfoRequest request)
        {
            return _configService.GetProcessInfo(request);
        }

        /// <summary>
        /// 获取数据状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Config/GetDbState")]
        public BasicResponse<bool> GetDbState()
        {
            return _configService.GetDbState();
        }
        /// <summary>
        /// 获取数据库磁盘信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Config/GetDatabaseDiskInfo")]
        public BasicResponse<HardDiskInfo> GetDatabaseDiskInfo()
        {
            return _configService.GetDatabaseDiskInfo();
        }
        /// <summary>
        /// 退出服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Config/ExitServer")]
        public BasicResponse ExitServer()
        {
            return _configService.ExitServer();
        }

        [HttpPost]
        [Route("v1/Config/SaveInspectionIn")]
        public BasicResponse SaveInspectionIn(SaveInspectionInRequest saveInspectionInRequest)
        {
            return _configService.SaveInspectionIn(saveInspectionInRequest);
        }
    }
}
