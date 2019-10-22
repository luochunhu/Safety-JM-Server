using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Analysistemplatealarmlevel
{
    public partial class AnalysistemplatealarmlevelAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_AnalysistemplatealarmlevelInfo AnalysistemplatealarmlevelInfo { get; set; }
    }

    public partial class AnalysistemplatealarmlevelUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_AnalysistemplatealarmlevelInfo AnalysistemplatealarmlevelInfo { get; set; }
    }

    public partial class AnalysistemplatealarmlevelDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class AnalysistemplatealarmlevelGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysistemplateId { get; set; }
    }

    public partial class AnalysistemplatealarmlevelGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }
}
