using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载设备型号缓存RPC
    /// </summary>
    public partial class DeviceTypeCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 获取所有设备型号缓存RPC
    /// </summary>
    public partial class DeviceTypeCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {

    }

    /// <summary>
    /// 根据条件获取设备型号缓存RPC
    /// </summary>
    public partial class DeviceTypeCacheGetByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<EnumcodeInfo, bool> Predicate { get; set; }
    }

    /// <summary>
    /// 根据Key获取设备型号缓存RPC
    /// </summary>
    public partial class DeviceTypeCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备型号Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }

    /// <summary>
    /// 设备型号缓存是否存在RPC
    /// </summary>
    public partial class DeviceTypeCacheIsExistsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备型号Key
        /// </summary>
        public int LngEnumValue { get; set; }
    }
}
