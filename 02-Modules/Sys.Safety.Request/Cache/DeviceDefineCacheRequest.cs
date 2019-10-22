using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义
        /// </summary>
        public Jc_DevInfo DeviceDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量添加设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义集合
        /// </summary>
        public List<Jc_DevInfo> DeviceDefineInfos { get; set; }
    }

    /// <summary>
    /// 更新设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义
        /// </summary>
        public Jc_DevInfo DeviceDefineInfo { get; set; }
    }

    /// <summary>
    /// 批量更新设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义集合
        /// </summary>
        public List<Jc_DevInfo> DeviceDefineInfos { get; set; }
    }

    /// <summary>
    /// 删除设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义
        /// </summary>
        public Jc_DevInfo DeviceDefineInfo { get; set; }
    }

    /// <summary>
    /// 获取所有设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    ///根据Key获取设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 设备定义Key
        /// </summary>
        public string Devid { get; set; }
    }

    /// <summary>
    /// 根据条件获取设备定义缓存RPC
    /// </summary>
    public partial class DeviceDefineCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_DevInfo, bool> Predicate { get; set; }
    }
}
