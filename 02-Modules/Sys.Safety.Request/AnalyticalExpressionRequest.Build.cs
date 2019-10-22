using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Analyticalexpression
{
    public partial class AnalyticalExpressionAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AnalyticalExpressionInfo JC_AnalyticalExpressionInfo { get; set; }      
    }
    public partial class AnalyticalExpressionListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<JC_AnalyticalExpressionInfo> JC_AnalyticalExpressionInfoList { get; set; }
    }
    public partial class AnalyticalExpressionUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AnalyticalExpressionInfo JC_AnalyticalExpressionInfo { get; set; }      
    }

	public partial class AnalyticalExpressionDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AnalyticalExpressionGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AnalyticalExpressionGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; } 
        public string TempleteId { get; set; } 
    }
}
