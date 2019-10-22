using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Graphicspointsinf;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.ServiceContract
{
    public interface IGraphicspointsinfService
    {
        BasicResponse<GraphicspointsinfInfo> AddGraphicspointsinf(GraphicspointsinfAddRequest graphicspointsinfrequest);
        BasicResponse<GraphicspointsinfInfo> UpdateGraphicspointsinf(GraphicspointsinfUpdateRequest graphicspointsinfrequest);
        BasicResponse DeleteGraphicspointsinf(GraphicspointsinfDeleteRequest graphicspointsinfrequest);
        BasicResponse<List<GraphicspointsinfInfo>> GetGraphicspointsinfList(GraphicspointsinfGetListRequest graphicspointsinfrequest);
        BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfById(GraphicspointsinfGetRequest graphicspointsinfrequest);

        /// <summary>
        /// 根据测点名获取GraphicspointsinfInfo
        /// </summary>
        /// <returns></returns>
        BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByPoint(GetGraphicspointsinfByPointRequest graphicspointsinfrequest);
         /// <summary>
        /// 根据测点号及图形ID获取图形测点信息
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByGraphIdAndPoint(GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest);

        /// <summary>
        /// 获取测点的绑定类型
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        BasicResponse<short> Get1GraphBindType(Get1GraphBindTypeRequest graphicspointsinfrequest);

        /// <summary>
        /// 删除图形的绑定测点信息
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteGraphicsPointsInfForGraphId(DeleteGraphicsPointsInfForGraphIdRequest graphicspointsinfrequest);

        /// <summary>
        /// 获取所有图形上所有的测点
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<GraphicspointsinfInfo>> GetAllGraphicspointsinfInfo();
    }
}

