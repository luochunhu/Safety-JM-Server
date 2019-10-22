using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Common;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;

namespace Sys.Safety.WebApiAgent
{
    public class DeviceDefineControllerProxy : BaseProxy, IDeviceDefineService
    {
        /// <summary>
        /// 添加设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse AddDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineAddRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/Add?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse AddDeviceDefines(Sys.Safety.Request.DeviceDefine.DeviceDefinesRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/AddDeviceDefines?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="DeviceDefinerequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineUpdateRequest DeviceDefinerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/Update?token=" + Token, JSONHelper.ToJSONString(DeviceDefinerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateDeviceDefines(Sys.Safety.Request.DeviceDefine.DeviceDefinesRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/UpdateDeviceDefines?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="DeviceDefinerequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteDeviceDefine(Sys.Safety.Request.DeviceDefine.DeviceDefineDeleteRequest DeviceDefinerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/Delete?token=" + Token, JSONHelper.ToJSONString(DeviceDefinerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList(Sys.Safety.Request.DeviceDefine.DeviceDefineGetListRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetPageList?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }

        public BasicResponse<Jc_DevInfo> GetDeviceDefineById(Sys.Safety.Request.DeviceDefine.DeviceDefineGetRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/Get?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DevInfo>>(responseStr);
        }
        /// <summary>
        /// 获取所有设备类型缓存信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DevInfo>> GetAllDeviceDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetAllDeviceDefineCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }
        /// <summary>
        /// 根据DevId获取设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DevInfo> GetDeviceDefineCacheByDevId(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevIdRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetDeviceDefineCacheByDevId?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DevInfo>>(responseStr);
        }
       
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetAllDeviceTypeCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetAllDeviceClassCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetAllDevicePropertyCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取所有解析驱动信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetAllDriverInf()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetAllDriverInf?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取当前最大的DevId
        /// </summary>
        /// <returns></returns>
        public BasicResponse<long> GetMaxDeviceDefineId()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetMaxDeviceDefineId?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<long>>(responseStr);
        }


        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetDeviceDefineCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertIDDevModelID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevpropertIDDevModelIDRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetDeviceDefineCacheByDevpropertIDDevModelID?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevClassID(Sys.Safety.Request.DeviceDefine.DeviceDefineGetByDevClassIDRequest DeviceDefineRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetDeviceDefineCacheByDevClassID?token=" + Token, JSONHelper.ToJSONString(DeviceDefineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }


        public BasicResponse<List<Jc_DevInfo>> GetNotMonitoringAllDeviceDefineCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/DeviceDefine/GetNotMonitoringAllDeviceDefineCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DevInfo>>>(responseStr);
        }
    }
}
