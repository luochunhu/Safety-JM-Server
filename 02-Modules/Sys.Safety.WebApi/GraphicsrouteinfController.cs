using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsrouteinf;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class GraphicsrouteinfController : Basic.Framework.Web.WebApi.BasicApiController, IGraphicsrouteinfService
    {
        private IGraphicsrouteinfService graphicsrouteinfService = ServiceFactory.Create<IGraphicsrouteinfService>();

        [HttpPost]
        [Route("v1/graphicsrouteinf/add")]
        public BasicResponse<GraphicsrouteinfInfo> AddGraphicsrouteinf(GraphicsrouteinfAddRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.AddGraphicsrouteinf(graphicsrouteinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsrouteinf/delete")]
        public BasicResponse DeleteGraphicsrouteinf(GraphicsrouteinfDeleteRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.DeleteGraphicsrouteinf(graphicsrouteinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsrouteinf/get")]
        public BasicResponse<GraphicsrouteinfInfo> GetGraphicsrouteinfById(GraphicsrouteinfGetRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.GetGraphicsrouteinfById(graphicsrouteinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsrouteinf/getlist")]
        public BasicResponse<List<GraphicsrouteinfInfo>> GetGraphicsrouteinfList(GraphicsrouteinfGetListRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.GetGraphicsrouteinfList(graphicsrouteinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsrouteinf/update")]
        public BasicResponse<GraphicsrouteinfInfo> UpdateGraphicsrouteinf(GraphicsrouteinfUpdateRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.UpdateGraphicsrouteinf(graphicsrouteinfrequest);
        }

        /// <summary>
        /// 删除图形绑定线路信息
        /// </summary>
        /// <param name="graphicsrouteinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsrouteinf/deletegraphicsrouteinfforgraphid")]
        public BasicResponse DeleteGraphicsrouteinfForGraphId(DeleteGraphicsrouteinfRequest graphicsrouteinfrequest)
        {
            return graphicsrouteinfService.DeleteGraphicsrouteinfForGraphId(graphicsrouteinfrequest);
        }
    }
}
