using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicspointsinf;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class GraphicspointsinfController : Basic.Framework.Web.WebApi.BasicApiController, IGraphicspointsinfService
    {
        private IGraphicspointsinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicspointsinfService>();

        [HttpPost]
        [Route("v1/graphicspointsinf/add")]
        public BasicResponse<GraphicspointsinfInfo> AddGraphicspointsinf(GraphicspointsinfAddRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.AddGraphicspointsinf(graphicspointsinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicspointsinf/delete")]
        public BasicResponse DeleteGraphicspointsinf(GraphicspointsinfDeleteRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.DeleteGraphicspointsinf(graphicspointsinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicspointsinf/get")]
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfById(GraphicspointsinfGetRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.GetGraphicspointsinfById(graphicspointsinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicspointsinf/getlist")]
        public BasicResponse<List<GraphicspointsinfInfo>> GetGraphicspointsinfList(GraphicspointsinfGetListRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.GetGraphicspointsinfList(graphicspointsinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicspointsinf/update")]
        public BasicResponse<GraphicspointsinfInfo> UpdateGraphicspointsinf(GraphicspointsinfUpdateRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.UpdateGraphicspointsinf(graphicspointsinfrequest);
        }

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicspointsinf/deletegraphicspointsinfforgraphid")]
        public BasicResponse DeleteGraphicsPointsInfForGraphId(DeleteGraphicsPointsInfForGraphIdRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.DeleteGraphicsPointsInfForGraphId(graphicspointsinfrequest);
        }

        /// <summary>
        /// 根据测点名称获取测点的绑定类型
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicspointsinf/get1graphbindtype")]
        public BasicResponse<short> Get1GraphBindType(Get1GraphBindTypeRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.Get1GraphBindType(graphicspointsinfrequest);
        }

        /// <summary>
        /// 根据测点名获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicspointsinf/getgraphicspointsinfbypoint")]
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByPoint(GetGraphicspointsinfByPointRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.GetGraphicspointsinfByPoint(graphicspointsinfrequest);
        }

        /// <summary>
        /// 根据测点号及图形ID获取图形测点信息
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicspointsinf/GetGraphicspointsinfByGraphIdAndPoint")]
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByGraphIdAndPoint(GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest)
        {
            return graphicsbaseinfService.GetGraphicspointsinfByGraphIdAndPoint(graphicspointsinfrequest);
        }

        /// <summary>
        /// 获取图形上所有测点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicspointsinf/getallgraphicspointsinfinfo")]
        public BasicResponse<List<GraphicspointsinfInfo>> GetAllGraphicspointsinfInfo()
        {
            return graphicsbaseinfService.GetAllGraphicspointsinfInfo();
        }
    }
}
