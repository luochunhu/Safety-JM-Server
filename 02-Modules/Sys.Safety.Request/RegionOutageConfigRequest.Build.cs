using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.RegionOutageConfig
{
    public partial class RegionOutageConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_RegionOutageConfigInfo JC_RegionOutageConfigInfo { get; set; }      
    }
    public partial class RegionOutageConfigListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
        public List<JC_RegionOutageConfigInfo> JC_RegionOutageConfigInfoList { get; set; }
    }
    public partial class RegionOutageConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_RegionOutageConfigInfo JC_RegionOutageConfigInfo { get; set; }      
    }

	public partial class RegionoutageconfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class RegionoutageconfigDeleteByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }
    public partial class RegionOutageConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RegionOutageConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }

        public string AnalysisModelId { get; set; }
    }

    /// <summary>
    /// 检查分析模型配置的控制点是否存在解控的请求对象
    /// </summary>
    public partial class ReleaseControlCheckRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
        public string PointId { get; set; }
    }
    public partial class GetByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }

    public partial class GetAllRegionOutageConfigRequest : Basic.Framework.Web.BasicRequest
    {

    }
}
