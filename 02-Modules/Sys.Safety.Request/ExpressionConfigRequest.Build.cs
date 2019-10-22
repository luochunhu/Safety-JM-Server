using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Expressionconfig
{
    public partial class ExpressionConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_ExpressionConfigInfo JC_ExpressionConfigInfo { get; set; }
    }
    public partial class ExpressionConfigListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<JC_ExpressionConfigInfo> JC_ExpressionConfigInfoList { get; set; }
    }


    public partial class ExpressionConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_ExpressionConfigInfo JC_ExpressionConfigInfo { get; set; }      
    }

	public partial class ExpressionconfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ExpressionConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class ExpressionConfigGetByExpressionIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string expressionId { get; set; }
    }
    
	public partial class ExpressionConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string TempleteId { get; set; }
        public string SearchName { get; set; }
		public string Id	{ get; set; }
        public List<JC_ExpressionConfigInfo> JC_ExpressionConfigInfoList { get; set; }
    }
}
