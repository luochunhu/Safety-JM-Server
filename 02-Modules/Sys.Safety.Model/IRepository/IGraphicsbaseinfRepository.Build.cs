using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IGraphicsbaseinfRepository : IRepository<GraphicsbaseinfModel>
    {
        GraphicsbaseinfModel AddGraphicsbaseinf(GraphicsbaseinfModel graphicsbaseinfModel);
        void UpdateGraphicsbaseinf(GraphicsbaseinfModel graphicsbaseinfModel);
        void DeleteGraphicsbaseinf(string id);
        IList<GraphicsbaseinfModel> GetGraphicsbaseinfList(int pageIndex, int pageSize, out int rowCount);
        GraphicsbaseinfModel GetGraphicsbaseinfById(string id);

        /// <summary>
        /// 根据图形名称获取图形信息
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        GraphicsbaseinfModel GetGraphicsbaseinfByName(string graphName);

        /// <summary>
        ///根据图形名称获取所有图形
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        IList<GraphicsbaseinfModel> GetGraphicsbaseinfListByName(string graphName);

        /// <summary>
        /// 获取对应的图形线路信息
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        DataTable GetMapRoutesInfo(string graphId);

        /// <summary>
        /// 获取图形对应测点绑定信息
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        DataTable GetMapPointsInfo(string graphId);

        /// <summary>
        /// 获取图形更新时间
        /// </summary>
        /// <param name="graphName"></param>
        /// <returns></returns>
        DataTable GetGraphicTimer(string graphName);

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphId"></param>
        /// <returns></returns>
        DataTable GetAllGraphPoint(string graphId);

        DataTable GetAllPoint();

        DataTable GetAllEmergencyLinkPoint();

        DataTable GetAllIp();

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphId"></param>
        void DeleteGraphicsbaseinfForGraphId(string graphId);

        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        DataTable GetSwitchInformation();

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDefPointInformation();

        /// <summary>
        /// 读取所有设备、数据状态枚举类型
        /// </summary>
        /// <returns></returns>
        DataTable GetAllDeviceEnumcode();

        /// <summary>
        /// 加载所有图形信息
        /// </summary>
        /// <returns></returns>
        DataTable LoadGraphicsInfo();

        /// <summary>
        /// 获取图形上的所有测点信息
        /// </summary>
        /// <returns></returns>
        DataTable GetAllPointInMap(string graphId);


        /// <summary>
        /// 获取应急联动图形
        /// </summary>
        /// <returns></returns>
        GraphicsbaseinfModel GetEmergencyLinkageGraphics();

        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        GraphicsbaseinfModel GetUserDefinedGraphicsByType(short type);

        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        GraphicsbaseinfModel GetSystemtDefaultGraphics(short? type);

        /// <summary>
        /// 修改系统默认图形
        /// </summary>
        /// <param name="bz3"></param>
        /// <param name="graphId"></param>
        void UpdateSystemDefaultGraphics(string bz3, string graphId);

        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphId"></param>
        void UpdateEmergencyLinkageGraphics(string graphId);

        
    }
}
