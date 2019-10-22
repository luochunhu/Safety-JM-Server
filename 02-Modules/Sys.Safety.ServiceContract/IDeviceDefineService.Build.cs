using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.ServiceContract
{
    public interface IDeviceDefineService
    {
        /// <summary>
        /// 添加设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse AddDeviceDefine(DeviceDefineAddRequest DeviceDefineRequest);
         /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse AddDeviceDefines(DeviceDefinesRequest DeviceDefineRequest);
        /// <summary>
        /// 更新设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateDeviceDefine(DeviceDefineUpdateRequest DeviceDefinerequest);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateDeviceDefines(DeviceDefinesRequest DeviceDefineRequest);
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteDeviceDefine(DeviceDefineDeleteRequest DeviceDefinerequest);
        BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList(DeviceDefineGetListRequest DeviceDefineRequest);
        BasicResponse<List<Jc_DevInfo>> GetDeviceDefineList();
        BasicResponse<Jc_DevInfo> GetDeviceDefineById(DeviceDefineGetRequest DeviceDefineRequest);
        /// <summary>
        /// 获取所有设备类型缓存信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetAllDeviceDefineCache();

        /// <summary>
        /// 获取非监控系统的所有设备类型缓存信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetNotMonitoringAllDeviceDefineCache();

        /// <summary>
        /// 根据DevId获取设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DevInfo> GetDeviceDefineCacheByDevId(DeviceDefineGetByDevIdRequest DeviceDefineRequest);
        
        /// <summary>
        /// 通过设备性质查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertID(DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest);
        /// <summary>
        /// 通过设备性质、设备型号查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevpropertIDDevModelID(DeviceDefineGetByDevpropertIDDevModelIDRequest DeviceDefineRequest);
         /// <summary>
        /// 通过设备种类查找设备类型
        /// </summary>
        /// <param name="DeviceDefineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetDeviceDefineCacheByDevClassID(DeviceDefineGetByDevClassIDRequest DeviceDefineRequest);
        
        /// <summary>
        /// 获取所有设备型号
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceTypeCache();
        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache();
        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache();
        /// <summary>
        /// 获取所有解析驱动信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDriverInf();
        /// <summary>
        /// 获取当前最大的DevId
        /// </summary>
        /// <returns></returns>
        BasicResponse<long> GetMaxDeviceDefineId();
    }
}

