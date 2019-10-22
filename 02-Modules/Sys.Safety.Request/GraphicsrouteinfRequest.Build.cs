using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Graphicsrouteinf
{
    public partial class GraphicsrouteinfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicsrouteinfInfo GraphicsrouteinfInfo { get; set; }
    }

    public partial class GraphicsrouteinfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicsrouteinfInfo GraphicsrouteinfInfo { get; set; }
    }

    public partial class GraphicsrouteinfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicsrouteinfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicsrouteinfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class DeleteGraphicsrouteinfRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }

    public partial class SetSaveFlagRequest : Basic.Framework.Web.BasicRequest
    {
        public bool SaveFlag { get; set; }
    }

}
