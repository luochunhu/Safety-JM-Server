using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Largedataanalysislog
{
    public partial class LargedataAnalysisLogAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_LargedataAnalysisLogInfo JC_LargedataAnalysisLogInfo { get; set; }      
    }

	public partial class LargedataAnalysisLogUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_LargedataAnalysisLogInfo JC_LargedataAnalysisLogInfo { get; set; }      
    }

	public partial class LargedataanalysislogDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class LargedataAnalysisLogGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class LargedataAnalysisLogGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class LargedataAnalysisLogGetByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 分析模型Id
        /// </summary>
        public string AnalysisModelId { get; set; }
    }

    public partial class LargedataAnalysisLogGetListByModelIdAndTimeRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 分析模型Id
        /// </summary>
        public string AnalysisModelId { get; set; }

        /// <summary>
        /// 分析日志日期
        /// </summary>
        public DateTime AnalysisDate { get; set; }

    }
}
