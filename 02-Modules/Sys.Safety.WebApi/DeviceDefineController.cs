using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.WebApi
{
    public class DeviceDefineController : Basic.Framework.Web.WebApi.BasicApiController, IDeviceDefineService
    {
        static DeviceDefineController()
        {

        }
        IDeviceDefineService _DeviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        /// <summary>
        /// 添加设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/Add")]
        public BasicResponse AddDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineAddRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.AddDeviceDefine(DeviceDefineRequest);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/AddDeviceDefines")]
        public BasicResponse AddDeviceDefines(Sys.Safety.Request.DeviceDefine.DeviceDefinesRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.AddDeviceDefines(DeviceDefineRequest);
        }
        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="DeviceDefinerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/Update")]
        public BasicResponse UpdateDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineUpdateRequest DeviceDefinerequest)
        {
            return _DeviceDefineService.UpdateDeviceDefine(DeviceDefinerequest);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/UpdateDeviceDefines")]
        public BasicResponse UpdateDeviceDefines(Sys.Safety.Request.DeviceDefine.DeviceDefinesRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.UpdateDeviceDefines(DeviceDefineRequest);
        }
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="DeviceDefinerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/Delete")]
        public BasicResponse DeleteDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineDeleteRequest DeviceDefinerequest)
        {
            return _DeviceDefineService.DeleteDeviceDefine(DeviceDefinerequest);
        }
        [HttpPost]
        [Route("v1/DeviceDefine/GetPageList")]
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList(Sys.Safety.Request.DeviceDefine.DeviceDefineGetListRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineList(DeviceDefineRequest);
        }
        [HttpPost]
        [Route("v1/DeviceDefine/GetList")]
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList()
        {
            return _DeviceDefineService.GetDeviceDefineList();
        }
        [HttpPost]
        [Route("v1/DeviceDefine/Get")]
        public BasicResponse<Jc_DevInfo> GetDeviceDefineById(Sys.Safety.Request.DeviceDefine.DeviceDefineGetRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineById(DeviceDefineRequest);
        }
        /// <summary>
        /// 获取所有设备类型缓存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetAllDeviceDefineCache")]
        public BasicResponse<List<Jc_DevInfo>> GetAllDeviceDefineCache()
        {
            return _DeviceDefineService.GetAllDeviceDefineCache();
        }
        /// <summary>
        /// 根据DevId获取设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetDeviceDefineCacheByDevId")]
        public BasicResponse<Jc_DevInfo> GetDeviceDefineCacheByDevId(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevIdRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineCacheByDevId(DeviceDefineRequest);
        }
        
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetAllDeviceTypeCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache()
        {
            return _DeviceDefineService.GetAllDeviceTypeCache();
        }
        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetAllDeviceClassCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            return _DeviceDefineService.GetAllDeviceClassCache();
        }
        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetAllDevicePropertyCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            return _DeviceDefineService.GetAllDevicePropertyCache();
        }
        /// <summary>
        /// 获取所有解析驱动信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetAllDriverInf")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDriverInf()
        {
            return _DeviceDefineService.GetAllDriverInf();
        }
        /// <summary>
        /// 获取当前最大的DevId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/DeviceDefine/GetMaxDeviceDefineId")]
        public BasicResponse<long> GetMaxDeviceDefineId()
        {
            return _DeviceDefineService.GetMaxDeviceDefineId();
        }

        [HttpPost]
        [Route("v1/DeviceDefine/GetDeviceDefineCacheByDevpropertID")]
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineCacheByDevpropertID(DeviceDefineRequest);
        }
        [HttpPost]
        [Route("v1/DeviceDefine/GetDeviceDefineCacheByDevpropertIDDevModelID")]
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertIDDevModelID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevpropertIDDevModelIDRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineCacheByDevpropertIDDevModelID(DeviceDefineRequest);
        }
        [HttpPost]
        [Route("v1/DeviceDefine/GetDeviceDefineCacheByDevClassID")]
        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevClassID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevClassIDRequest DeviceDefineRequest)
        {
            return _DeviceDefineService.GetDeviceDefineCacheByDevClassID(DeviceDefineRequest);
        }

        [HttpPost]
        [Route("v1/DeviceDefine/GetNotMonitoringAllDeviceDefineCache")]
        public BasicResponse<List<Jc_DevInfo>> GetNotMonitoringAllDeviceDefineCache()
        {
            return _DeviceDefineService.GetNotMonitoringAllDeviceDefineCache();
        }
    }
}
