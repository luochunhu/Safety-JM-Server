using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 添加自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 自动挂接设备缓存
        /// </summary>
        public AutomaticArticulatedDeviceInfo AutomaticArticulatedDeviceInfo { get; set; }
    }

    

    /// <summary>
    /// 更新自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 自动挂接设备缓存
        /// </summary>
        public AutomaticArticulatedDeviceInfo AutomaticArticulatedDeviceInfo { get; set; }
    }
    /// <summary>
    /// 删除自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 自动挂接设备缓存
        /// </summary>
        public AutomaticArticulatedDeviceInfo AutomaticArticulatedDeviceInfo { get; set; }
    }

    
    /// <summary>
    /// 获取所有自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 根据Key获取自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 自动挂接设备Key
        /// </summary>
        public string AutomaticArticulatedDeviceId { get; set; }
    }

    /// <summary>
    /// 根据条件获取自动挂接设备缓存RPC
    /// </summary>
    public partial class AutomaticArticulatedDeviceCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 自动挂接设备缓存
        /// </summary>
        public Func<AutomaticArticulatedDeviceInfo, bool> Pridicate { get; set; }
    }
    
}
