using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载设备性质缓存RPC
    /// </summary>
    public partial class DevicePropertyCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 获取所有设备性质缓存RPC
    /// </summary>
    public partial class DevicePropertyCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据条件获取设备性质缓存RPC
    /// </summary>
    public partial class DevicePropertyCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<EnumcodeInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 根据Key获取设备性质缓存RPC
    /// </summary>
    public partial class DevicePropertyCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备性质Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }

    /// <summary>
    /// 设备性质缓存是否存在RPC
    /// </summary>
    public partial class DevicePropertyCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备性质Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }
}
