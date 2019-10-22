using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Graphicspointsinf
{
    public partial class GraphicspointsinfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicspointsinfInfo GraphicspointsinfInfo { get; set; }
    }

    public partial class GraphicspointsinfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public GraphicspointsinfInfo GraphicspointsinfInfo { get; set; }
    }

    public partial class GraphicspointsinfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicspointsinfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GraphicspointsinfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class GetGraphicspointsinfByPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointId { get; set; }
    }
    public partial class GetGraphicspointsinfByGraphIdAndPointRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointId { get; set; }
        public string GraphId { get; set; }
    }

    public partial class Get1GraphBindTypeRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointId { get; set; }
        public string GraphId { get; set; }
    }

    public partial class DeleteGraphicsPointsInfForGraphIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string GraphId { get; set; }
    }
}
