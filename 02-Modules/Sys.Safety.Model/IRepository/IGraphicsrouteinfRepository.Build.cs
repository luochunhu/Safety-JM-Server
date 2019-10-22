using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IGraphicsrouteinfRepository : IRepository<GraphicsrouteinfModel>
    {
        GraphicsrouteinfModel AddGraphicsrouteinf(GraphicsrouteinfModel graphicsrouteinfModel);
        void UpdateGraphicsrouteinf(GraphicsrouteinfModel graphicsrouteinfModel);
        void DeleteGraphicsrouteinf(string id);
        IList<GraphicsrouteinfModel> GetGraphicsrouteinfList(int pageIndex, int pageSize, out int rowCount);
        GraphicsrouteinfModel GetGraphicsrouteinfById(string id);

        /// <summary>
        /// 删除图形绑定线路信息
        /// </summary>
        /// <param name="graphId"></param>
        void DeleteGraphicsrouteinfForGraphId(string graphId);
    }

}
