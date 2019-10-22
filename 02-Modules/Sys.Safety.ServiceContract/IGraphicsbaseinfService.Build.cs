using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.ServiceContract
{
    public interface IGraphicsbaseinfService
    {
        BasicResponse<GraphicsbaseinfInfo> AddGraphicsbaseinf(GraphicsbaseinfAddRequest graphicsbaseinfrequest);
        BasicResponse<GraphicsbaseinfInfo> UpdateGraphicsbaseinf(GraphicsbaseinfUpdateRequest graphicsbaseinfrequest);
        BasicResponse DeleteGraphicsbaseinf(GraphicsbaseinfDeleteRequest graphicsbaseinfrequest);
        BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfList(GraphicsbaseinfGetListRequest graphicsbaseinfrequest);
        BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfById(GraphicsbaseinfGetRequest graphicsbaseinfrequest);

        /// <summary>
        /// 设置图形更新标记
        /// </summary>
        /// <returns></returns>
        BasicResponse SetSaveFlag(SetSaveFlagRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取图形更新标记
        /// </summary>
        /// <returns></returns>
        BasicResponse<bool> GetSaveFlag();

        /// <summary>
        /// 根据图形名称获取图形信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfByName(GetGraphicsbaseinfByNameRequest graphicsbaseinfrequest);

        /// <summary>
        /// 根据图形名称获取所有图形信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfListByName(GetGraphicsbaseinfListByNameRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取图形对应的线路信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMapRoutesInfo(GetMapRoutesInfoRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取图形对应测点绑定信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMapPointsInfo(GetMapPointsInfoRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取图形更新时间
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetGraphicTimer(GetGraphicTimerRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetAllGraphPoint(GetAllGraphPointRequest graphicsbaseinfrequest);

        /// <summary>
        /// 加载所有测点定义信息（含交换机）
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> LoadAllpointDefByType(LoadAllpointDefByTypeRequest graphicsbaseinfrequest);

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteGraphicsbaseinfForGraphId(DeleteGraphicsbaseinfForGraphIdRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetSwitchInformation();

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetDefPointInformation();

        /// <summary>
        /// 在缓存中查找所有测点信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetDefPointInformationInCache();

        /// <summary>
        /// 在缓存中查找所有交换机信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetSwitchInformationInCache();

        /// <summary>
        /// 获取所有设备、数据状态枚举类型
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetAllDeviceEnumcode();

        /// <summary>
        /// 加载所有图形信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> LoadGraphicsInfo();

        /// <summary>
        /// 获取图形上的所有测点信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetAllPointInMap(GetAllPointInMapRequest graphicsbaseinfrequest);

        /// <summary>
        /// 获取应急联动图形
        /// </summary>
        /// <returns></returns>
        BasicResponse<GraphicsbaseinfInfo> GetEmergencyLinkageGraphics();

        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<GraphicsbaseinfInfo> GetUserDefinedGraphicsByType(GetUserDefinedGraphicsByTypeRequest graphicsbaseinfrequest);


        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse<GraphicsbaseinfInfo> GetSystemtDefaultGraphics(GetSystemtDefaultGraphicsRequest graphicsbaseinfrequest);

        /// <summary>
        /// 修改系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse UpdateSystemDefaultGraphics(UpdateSystemDefaultGraphicsRequest graphicsbaseinfrequest);

        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        BasicResponse UpdateEmergencyLinkageGraphics(UpdateEmergencyLinkageGraphicsRequest graphicsbaseinfrequest);

    }
}

