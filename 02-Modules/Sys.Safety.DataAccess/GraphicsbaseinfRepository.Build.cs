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
    public partial class GraphicsbaseinfRepository : RepositoryBase<GraphicsbaseinfModel>, IGraphicsbaseinfRepository
    {
        public GraphicsbaseinfModel AddGraphicsbaseinf(GraphicsbaseinfModel graphicsbaseinfModel)
        {
            return base.Insert(graphicsbaseinfModel);
        }
        public void UpdateGraphicsbaseinf(GraphicsbaseinfModel graphicsbaseinfModel)
        {
            base.Update(graphicsbaseinfModel);
        }
        public void DeleteGraphicsbaseinf(string id)
        {
            base.Delete(id);
        }
        public IList<GraphicsbaseinfModel> GetGraphicsbaseinfList(int pageIndex, int pageSize, out int rowCount)
        {
            var graphicsbaseinfModelLists = base.Datas.OrderByDescending(c => c.GraphId).AsQueryable();
            rowCount = graphicsbaseinfModelLists.Count();
            return graphicsbaseinfModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public GraphicsbaseinfModel GetGraphicsbaseinfById(string id)
        {
            GraphicsbaseinfModel graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.GraphId == id);
            return graphicsbaseinfModel;
        }

        /// <summary>
        /// 根据图形名称获取图形
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        public GraphicsbaseinfModel GetGraphicsbaseinfByName(string graphName)
        {
            GraphicsbaseinfModel graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.GraphName == graphName);
            return graphicsbaseinfModel;
        }

        /// <summary>
        ///根据图形名称获取所有图形
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        public IList<GraphicsbaseinfModel> GetGraphicsbaseinfListByName(string graphName)
        {
            var graphicsbaseinfModelLists = base.Datas.AsQueryable();
            if (string.IsNullOrWhiteSpace(graphName))
            {
                graphicsbaseinfModelLists = graphicsbaseinfModelLists.Where(c => c.GraphName == graphName).AsQueryable();
            }
            return graphicsbaseinfModelLists.ToList();
        }

        /// <summary>
        /// 获取图形对应的线路信息
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        public DataTable GetMapRoutesInfo(string graphId)
        {
            return base.QueryTable("global_GraphicsModule_GetMapRoutesInfo_ByGraphicsId", graphId);
        }

        /// <summary>
        /// 获取图形对应测点绑定信息
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        public DataTable GetMapPointsInfo(string graphId)
        {
            return base.QueryTable("global_GraphicsModule_GetMapPointsInfo_ByGraphicsId", graphId);
        }

        /// <summary>
        /// 获取图形更细时间
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        public DataTable GetGraphicTimer(string graphName)
        {
            return base.QueryTable("global_GraphicsModule_GetGraphicTimer", graphName);
        }

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        public DataTable GetAllGraphPoint(string graphId)
        {
            return base.QueryTable("global_GraphicsModule_GetAllGraphPoint", graphId);
        }

        public DataTable GetAllPoint()
        {
            return base.QueryTable("global_GraphicsModule_GetAllPoint");
        }

        public DataTable GetAllIp()
        {
            return base.QueryTable("global_GraphicsModule_GetAllIp");
        }

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphId"></param>
        public void DeleteGraphicsbaseinfForGraphId(string graphId)
        {
            base.QueryTable("global_GraphicsModule_DeleteGraphicsbaseinf_ForGraphId", graphId);
        }

        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSwitchInformation()
        {
            return base.QueryTable("global_GraphicsModule_GetSwitchInformation");
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDefPointInformation()
        {
            return base.QueryTable("global_GraphicsModule_GetDefPointInformation");
        }

        /// <summary>
        /// 读取所有设备、数据状态枚举类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDeviceEnumcode()
        {
            return base.QueryTable("global_GraphicsModule_GetAllDeviceEnumcode");
        }

        /// <summary>
        /// 加载所有图形信息
        /// </summary>
        /// <returns></returns>
        public DataTable LoadGraphicsInfo()
        {
            return base.QueryTable("global_GraphicsModule_LoadGraphicsInfo");
        }

        /// <summary>
        /// 获取图形上的所有测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllPointInMap(string GraphId)
        {
            return base.QueryTable("global_GraphicsModule_GetAllPointInMap", GraphId);
        }

        /// <summary>
        /// 获取应急联动图形
        /// </summary>
        /// <returns></returns>
        public GraphicsbaseinfModel GetEmergencyLinkageGraphics()
        {
            GraphicsbaseinfModel graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.Bz4 == "1");
            return graphicsbaseinfModel;
        }

        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GraphicsbaseinfModel GetUserDefinedGraphicsByType(short type)
        {
            GraphicsbaseinfModel graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.Bz3 == "0" && c.Type == type);
            return graphicsbaseinfModel;
        }


        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GraphicsbaseinfModel GetSystemtDefaultGraphics(short? type)
        {
            var graphicsbaseinfModel = new GraphicsbaseinfModel();
            if (type.HasValue && type.Value == 0)
            {
                graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.Bz3 == "1" && c.Type == type.Value);
            }
            if (type.HasValue && type.Value == 1)
            {
                graphicsbaseinfModel = base.Datas.FirstOrDefault(c => c.Bz3 == "2" && c.Type == type.Value);
            }
            return graphicsbaseinfModel;
        }

        /// <summary>
        /// 修改系统默认图形
        /// </summary>
        /// <param name="bz3"></param>
        /// <param name="graphId"></param>
        public void UpdateSystemDefaultGraphics(string bz3, string graphId)
        {
            base.ExecuteNonQuery("global_GraphicsModule_UpdateSystemDefinedGraphics", bz3, graphId);
        }

        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphId"></param>
        public void UpdateEmergencyLinkageGraphics(string graphId)
        {
            base.ExecuteNonQuery("global_GraphicsModule_UpdateEmergencyLinkageGraphics", graphId);
        }


        public DataTable GetAllEmergencyLinkPoint()
        {
            return base.QueryTable("global_GraphicsModule_GetAllEmergencyLinkPoint");
        }
    }
}
