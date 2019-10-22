using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IGraphicspointsinfRepository : IRepository<GraphicspointsinfModel>
    {
        GraphicspointsinfModel AddGraphicspointsinf(GraphicspointsinfModel graphicspointsinfModel);
        void UpdateGraphicspointsinf(GraphicspointsinfModel graphicspointsinfModel);
        void DeleteGraphicspointsinf(string id);
        IList<GraphicspointsinfModel> GetGraphicspointsinfList(int pageIndex, int pageSize, out int rowCount);
        GraphicspointsinfModel GetGraphicspointsinfById(string id);

        /// <summary>
        /// 根据测点名获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        GraphicspointsinfModel GetGraphicspointsinfByPoint(string pointId);
        /// <summary>
        /// 根据测点名及图形ID获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="point"></param>
        /// <param name="graphId"></param>
        /// <returns></returns>
        GraphicspointsinfModel GetGraphicspointsinfByPoint(string point, string graphId);

        /// <summary>
        /// 根据测点名称，获取测点的绑定类型
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="graphId"></param>
        /// <returns></returns>
        GraphicspointsinfModel Get1GraphBindType(string pointId, string graphId);

        /// <summary>
        /// 删除图形的绑定测点信息
        /// </summary>
        /// <param name="graphId"></param>
        void DeleteGraphicsPointsInfForGraphId(string graphId);

        /// <summary>
        /// 获取图形上所有测点
        /// </summary>
        /// <returns></returns>
        IList<GraphicspointsinfModel> GetAllGraphicspointsinfInfo();
    }
}
