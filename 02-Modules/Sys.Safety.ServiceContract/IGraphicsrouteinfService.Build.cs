using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Graphicsrouteinf;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IGraphicsrouteinfService
    {
        BasicResponse<GraphicsrouteinfInfo> AddGraphicsrouteinf(GraphicsrouteinfAddRequest graphicsrouteinfrequest);
        BasicResponse<GraphicsrouteinfInfo> UpdateGraphicsrouteinf(GraphicsrouteinfUpdateRequest graphicsrouteinfrequest);
        BasicResponse DeleteGraphicsrouteinf(GraphicsrouteinfDeleteRequest graphicsrouteinfrequest);
        BasicResponse<List<GraphicsrouteinfInfo>> GetGraphicsrouteinfList(GraphicsrouteinfGetListRequest graphicsrouteinfrequest);
        BasicResponse<GraphicsrouteinfInfo> GetGraphicsrouteinfById(GraphicsrouteinfGetRequest graphicsrouteinfrequest);

        /// <summary>
        /// 删除图形绑定线路信息
        /// </summary>
        /// <param name="graphicsrouteinfrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteGraphicsrouteinfForGraphId(DeleteGraphicsrouteinfRequest graphicsrouteinfrequest);
    }
}

