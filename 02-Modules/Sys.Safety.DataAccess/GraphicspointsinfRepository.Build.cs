using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public partial class GraphicspointsinfRepository : RepositoryBase<GraphicspointsinfModel>, IGraphicspointsinfRepository
    {

        public GraphicspointsinfModel AddGraphicspointsinf(GraphicspointsinfModel graphicspointsinfModel)
        {
            return base.Insert(graphicspointsinfModel);
        }
        public void UpdateGraphicspointsinf(GraphicspointsinfModel graphicspointsinfModel)
        {
            base.Update(graphicspointsinfModel);
        }
        public void DeleteGraphicspointsinf(string id)
        {
            base.Delete(id);
        }
        public IList<GraphicspointsinfModel> GetGraphicspointsinfList(int pageIndex, int pageSize, out int rowCount)
        {
            var graphicspointsinfModelLists = base.Datas.OrderByDescending(c => c.ID).AsQueryable();
            rowCount = graphicspointsinfModelLists.Count();
            return graphicspointsinfModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public GraphicspointsinfModel GetGraphicspointsinfById(string id)
        {
            GraphicspointsinfModel graphicspointsinfModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return graphicspointsinfModel;
        }

        /// <summary>
        /// 根据测点名获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public GraphicspointsinfModel GetGraphicspointsinfByPoint(string pointId)
        {
            GraphicspointsinfModel graphicspointsinfModel = base.Datas.FirstOrDefault(c => c.Point == pointId);
            return graphicspointsinfModel;
        }

        /// <summary>
        /// 根据测点名及图形ID获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public GraphicspointsinfModel GetGraphicspointsinfByPoint(string point, string graphId)
        {
            GraphicspointsinfModel graphicspointsinfModel = base.Datas.FirstOrDefault(c => c.Point == point && c.GraphId == graphId);
            return graphicspointsinfModel;
        }

        /// <summary>
        /// 根据测点名称获取测点的绑定类型
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="graphId"></param>
        /// <returns></returns>
        public GraphicspointsinfModel Get1GraphBindType(string pointId, string graphId)
        {
            GraphicspointsinfModel graphicspointsinfModel = base.Datas.FirstOrDefault(c => c.Point == pointId && c.GraphId == graphId);
            return graphicspointsinfModel;
        }

        /// <summary>
        /// 删除图形的绑定测点信息
        /// </summary>
        /// <param name="graphId"></param>
        public void DeleteGraphicsPointsInfForGraphId(string graphId)
        {
            base.ExecuteNonQuery("global_GraphicsModule_DeleteGraphicsPointsInf_ForGraphId", graphId);
        }

        /// <summary>
        /// 获取图形上所有测点
        /// </summary>
        /// <returns></returns>
        public IList<GraphicspointsinfModel> GetAllGraphicspointsinfInfo()
        {
            var graphicspointsinfModelLists = base.Datas.AsQueryable();
            return graphicspointsinfModelLists.ToList();
        }
    }
}
