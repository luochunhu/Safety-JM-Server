using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.StationUpdate
{

    public class StationUpdateRequest : BaseRequest
    {
        public StationUpdateRequest(int requestType)
            : base(requestType)
        {

        }
    }

    public partial class LoadUpdateBufferRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 下发文件Buffer
        /// </summary>
        public byte[] updateBufferItems { get; set; }
        /// <summary>
        /// 设备类型编码
        /// </summary>
        public int updatetTypeid { get; set; }
        /// <summary>
        /// 硬件编码
        /// </summary>
        public int updateHardVersion { get; set; }
        /// <summary>
        /// 升级文件版本号
        /// </summary>
        public int updateFileVersion { get; set; }
        /// <summary>
        /// 版本上限
        /// </summary>
        public int updateMaxVersion { get; set; }
        /// <summary>
        /// 版本下限
        /// </summary>
        public int updateMinVersion { get; set; }
        /// <summary>
        /// 升级文件总片数
        /// </summary>
        public int updateCount { get; set; }
        /// <summary>
        /// CRC值
        /// </summary>
        public uint crcValue { get; set; }
    }

    public partial class UpdateOrderForUserRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 客户端传递过来的命令
        /// </summary>
        public int order;
        /// <summary>
        /// 分站号
        /// </summary>
        public int fzh;
    }
    public partial class UpdateOrderForSysRequest : Basic.Framework.Web.BasicRequest
    {
        public StationUpdateItem item;
    }
    public partial class GetStationItemRequest : Basic.Framework.Web.BasicRequest
    {
        public int fzh;
    }

    public partial class GetAllStationItemsRequest : Basic.Framework.Web.BasicRequest
    {
    }
}
