using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Operatelog
{
    public partial class OperatelogAddRequest : Basic.Framework.Web.BasicRequest
    {
        public OperatelogInfo OperatelogInfo { get; set; }
    }
    /// <summary>
    /// 批量添加
    /// </summary>
    public partial class OperatelogAddListRequest : Basic.Framework.Web.BasicRequest
    {
        public List<OperatelogInfo> OperatelogInfo { get; set; }
    }

    public partial class OperatelogUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public OperatelogInfo OperatelogInfo { get; set; }
    }

    public partial class OperatelogDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class OperatelogDeleteByStimeEtimeRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime Stime { get; set; }
        public DateTime Etime { get; set; }
    }

    public partial class OperatelogGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class OperatelogGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }
    public partial class OperatelogGetByConditionsRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public string type { get; set; }
        public string context { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
