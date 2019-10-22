using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Graphicsbaseinf
{
    public partial class GraphicsbaseinfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicsbaseinfInfo GraphicsbaseinfInfo { get; set; }
    }

    public partial class GraphicsbaseinfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicsbaseinfInfo GraphicsbaseinfInfo { get; set; }
    }

    public partial class GraphicsbaseinfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicsbaseinfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicsbaseinfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class SetSaveFlagRequest : Basic.Framework.Web.BasicRequest
    {
        public bool Flag { get; set; }
    }

    public partial class GetGraphicsbaseinfByNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphName { get; set; }
    }
    public partial class GetGraphicsbaseinfListByNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphName { get; set; }
    }
    public partial class GetMapRoutesInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }

    public partial class GetMapPointsInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }
    public partial class GetGraphicTimerRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphName { get; set; }
    }
    public partial class GetAllGraphPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string Type { get; set; }
        public string GraphId { get; set; }
    }
    public partial class LoadAllpointDefByTypeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Type { get; set; }
    }

    public partial class DeleteGraphicsbaseinfForGraphIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }

    public partial class GetAllPointInMapRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }

    public partial class GetUserDefinedGraphicsByTypeRequest : Basic.Framework.Web.BasicRequest
    {
        public short? Type { get; set; }
    }

    public partial class UpdateSystemDefaultGraphicsRequest : Basic.Framework.Web.BasicRequest
    {
        public string Bz3 { get; set; }
        public string GraphId { get; set; }
    }
    public partial class UpdateEmergencyLinkageGraphicsRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }

    public partial class GetSystemtDefaultGraphicsRequest : Basic.Framework.Web.BasicRequest
    {
        public short? Type { get; set; }
    }
}
