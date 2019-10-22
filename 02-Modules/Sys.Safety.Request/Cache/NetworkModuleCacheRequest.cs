using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    /// <summary>
    /// 加载网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    /// 添加网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块
        /// </summary>
        public Jc_MacInfo NetworkModuleInfo { get; set; }
    }

    /// <summary>
    /// 批量添加网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheBatchAddRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块集合
        /// </summary>
        public List<Jc_MacInfo> NetworkModuleInfos { get; set; }
    }

    /// <summary>
    /// 更新网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块
        /// </summary>
        public Jc_MacInfo NetworkModuleInfo { get; set; }
    }

    /// <summary>
    /// 批量更新网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheBatchUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块集合
        /// </summary>
        public List<Jc_MacInfo> NetworkModuleInfos { get; set; }
    }

    /// <summary>
    /// 删除网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块
        /// </summary>
        public Jc_MacInfo NetworkModuleInfo { get; set; }
    }

    /// <summary>
    /// 获取所有网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    /// <summary>
    ///根据Key获取网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 网络模块Key
        /// </summary>
        public string Mac { get; set; }
    }

    /// <summary>
    /// 根据条件获取网络模块缓存RPC
    /// </summary>
    public partial class NetworkModuleCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public Func<Jc_MacInfo, bool> Predicate { get; set; }
    }

    public partial class NetworkModuleCacheUpdateNCommandRequest: BasicRequest
    {
        public Jc_MacInfo NetWorkModuleInfo { get; set; }
    }

    public partial class NetworkModuleCacheUpdateFdStateRequest : BasicRequest
    {
        public Jc_MacInfo NetWorkModuleInfo { get; set; }
    }
}
