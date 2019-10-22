using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.FactorValue
{
    /// <summary>
    /// 参数值获取请求
    /// </summary>
    public partial class FactorValueGetRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 测点编号
        /// </summary>
        public string PointId { get; set; }
    }
}
