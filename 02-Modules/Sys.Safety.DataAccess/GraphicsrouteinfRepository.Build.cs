using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class GraphicsrouteinfRepository : RepositoryBase<GraphicsrouteinfModel>, IGraphicsrouteinfRepository
    {

        public GraphicsrouteinfModel AddGraphicsrouteinf(GraphicsrouteinfModel graphicsrouteinfModel)
        {
            return base.Insert(graphicsrouteinfModel);
        }
        public void UpdateGraphicsrouteinf(GraphicsrouteinfModel graphicsrouteinfModel)
        {
            base.Update(graphicsrouteinfModel);
        }
        public void DeleteGraphicsrouteinf(string id)
        {
            base.Delete(id);
        }
        public IList<GraphicsrouteinfModel> GetGraphicsrouteinfList(int pageIndex, int pageSize, out int rowCount)
        {
            var graphicsrouteinfModelLists = base.Datas.OrderByDescending(c=>c.ID).AsQueryable();
            rowCount = graphicsrouteinfModelLists.Count();
            return graphicsrouteinfModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public GraphicsrouteinfModel GetGraphicsrouteinfById(string id)
        {
            GraphicsrouteinfModel graphicsrouteinfModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return graphicsrouteinfModel;
        }

        /// <summary>
        /// 删除图形绑定线路信息
        /// </summary>
        /// <param name="graphId"></param>
        public void DeleteGraphicsrouteinfForGraphId(string graphId)
        {
            base.ExecuteNonQuery("global_GraphicsModule_DeleteGraphicsrouteinf_ForGraphId", graphId);
        }
    }
}
