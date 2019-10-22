using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载设备种类缓存RPC
    /// </summary>
    public partial class DeviceClassCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
        
    }

    /// <summary>
    /// 获取所有设备种类缓存RPC
    /// </summary>
    public partial class DeviceClassCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据条件获取设备种类缓存RPC
    /// </summary>
    public partial class DeviceClassCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<EnumcodeInfo,bool> Predicate { get; set; }
    }

    /// <summary>
    /// 根据设备性质获取设备种类缓存RPC
    /// </summary>
    public partial class DeviceClassCacheGetByDevicePropertyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备性质Id
        /// </summary>
        public int DevicePropertyId { get; set; }
    }

    /// <summary>
    /// 根据Key获取设备种类缓存RPC
    /// </summary>
    public partial class DeviceClassCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备种类Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }

    /// <summary>
    /// 设备种类缓存是否存在RPC
    /// </summary>
    public partial class DeviceClassCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备种类Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }
}
