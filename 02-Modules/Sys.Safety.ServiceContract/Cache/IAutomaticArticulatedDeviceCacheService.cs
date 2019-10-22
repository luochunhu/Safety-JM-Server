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
    /// 自动挂接设备缓存操作接口 luoch 20170614
    /// </summary>
    public interface IAutomaticArticulatedDeviceCacheService
    {
        BasicResponse AddAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheAddRequest AutomaticArticulatedDeviceCacheRequest);
        BasicResponse UpdateAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheUpdateRequest AutomaticArticulatedDeviceCacheRequest);
        BasicResponse DeleteAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheDeleteRequest AutomaticArticulatedDeviceCacheRequest);
        BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest);
        BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetByConditionRequest AutomaticArticulatedDeviceCacheRequest);
        BasicResponse<AutomaticArticulatedDeviceInfo> GetAutomaticArticulatedDeviceCacheByKey(AutomaticArticulatedDeviceCacheGetByKeyRequest AutomaticArticulatedDeviceCacheRequest);
    }
}
