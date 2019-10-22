using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class GraphicsbaseinfController : Basic.Framework.Web.WebApi.BasicApiController, IGraphicsbaseinfService
    {
        private IGraphicsbaseinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();

        [HttpPost]
        [Route("v1/graphicsbaseinf/add")]
        public BasicResponse<GraphicsbaseinfInfo> AddGraphicsbaseinf(GraphicsbaseinfAddRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.AddGraphicsbaseinf(graphicsbaseinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsbaseinf/delete")]
        public BasicResponse DeleteGraphicsbaseinf(GraphicsbaseinfDeleteRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.DeleteGraphicsbaseinf(graphicsbaseinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsbaseinf/get")]
        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfById(GraphicsbaseinfGetRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetGraphicsbaseinfById(graphicsbaseinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsbaseinf/getlist")]
        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfList(GraphicsbaseinfGetListRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetGraphicsbaseinfList(graphicsbaseinfrequest);
        }

        [HttpPost]
        [Route("v1/graphicsbaseinf/update")]
        public BasicResponse<GraphicsbaseinfInfo> UpdateGraphicsbaseinf(GraphicsbaseinfUpdateRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.UpdateGraphicsbaseinf(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/deleteforgraphid")]
        public BasicResponse DeleteGraphicsbaseinfForGraphId(DeleteGraphicsbaseinfForGraphIdRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.DeleteGraphicsbaseinfForGraphId(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取所有设备、数据状态枚举类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getalldeviceenumcode")]
        public BasicResponse<DataTable> GetAllDeviceEnumcode()
        {
            return graphicsbaseinfService.GetAllDeviceEnumcode();
        }

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getallgraphpoint")]
        public BasicResponse<DataTable> GetAllGraphPoint(GetAllGraphPointRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetAllGraphPoint(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getdefpointinformation")]
        public BasicResponse<DataTable> GetDefPointInformation()
        {
            return graphicsbaseinfService.GetDefPointInformation();
        }

        /// <summary>
        /// 在缓存中获取测点信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getdefpointinformationincache")]
        public BasicResponse<List<Jc_DefInfo>> GetDefPointInformationInCache()
        {
            return graphicsbaseinfService.GetDefPointInformationInCache();
        }

        /// <summary>
        /// 根据name获取图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getbyname")]
        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfByName(GetGraphicsbaseinfByNameRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetGraphicsbaseinfByName(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 根据图形名称获取图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getlistbyname")]
        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfListByName(GetGraphicsbaseinfListByNameRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetGraphicsbaseinfListByName(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取图形更新时间
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getgraphictimer")]
        public BasicResponse<DataTable> GetGraphicTimer(GetGraphicTimerRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetGraphicTimer(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getmappointsinfo")]
        public BasicResponse<DataTable> GetMapPointsInfo(GetMapPointsInfoRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetMapPointsInfo(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取图形对应的线路信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getmaproutesinfo")]
        public BasicResponse<DataTable> GetMapRoutesInfo(GetMapRoutesInfoRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetMapRoutesInfo(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取图形更新标价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getsaveflag")]
        public BasicResponse<bool> GetSaveFlag()
        {
            return graphicsbaseinfService.GetSaveFlag();
        }

        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getswitchinformation")]
        public BasicResponse<DataTable> GetSwitchInformation()
        {
            return graphicsbaseinfService.GetSwitchInformation();
        }

        /// <summary>
        /// 在缓存中获取交换机信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getswitchinformationincache")]
        public BasicResponse<List<Jc_MacInfo>> GetSwitchInformationInCache()
        {
            return graphicsbaseinfService.GetSwitchInformationInCache();
        }

        /// <summary>
        /// 加载所有测点定义信息（含交换机）
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/loadallpointdefbytype")]
        public BasicResponse<DataTable> LoadAllpointDefByType(LoadAllpointDefByTypeRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.LoadAllpointDefByType(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 设置图形标记
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/setsaveflag")]
        public BasicResponse SetSaveFlag(SetSaveFlagRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.SetSaveFlag(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 加载所有图形信息
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/loadgraphicsinfo")]
        public BasicResponse<DataTable> LoadGraphicsInfo()
        {
            return graphicsbaseinfService.LoadGraphicsInfo();
        }

        /// <summary>
        /// 获取图形上的所有测点信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getallpointinmap")]
        public BasicResponse<DataTable> GetAllPointInMap(GetAllPointInMapRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetAllPointInMap(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getuserdefinedgraphicsbytype")]
        public BasicResponse<GraphicsbaseinfInfo> GetUserDefinedGraphicsByType(GetUserDefinedGraphicsByTypeRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetUserDefinedGraphicsByType(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取应急联动图形
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getemergencylinkagegraphics")]
        public BasicResponse<GraphicsbaseinfInfo> GetEmergencyLinkageGraphics()
        {
            return graphicsbaseinfService.GetEmergencyLinkageGraphics();
        }

        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/updateemergencylinkagegraphics")]
        public BasicResponse UpdateEmergencyLinkageGraphics(UpdateEmergencyLinkageGraphicsRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.UpdateEmergencyLinkageGraphics(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/getsystemtdefaultgraphics")]
        public BasicResponse<GraphicsbaseinfInfo> GetSystemtDefaultGraphics(GetSystemtDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.GetSystemtDefaultGraphics(graphicsbaseinfrequest);
        }

        /// <summary>
        /// 修改系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/graphicsbaseinf/updatesystemdefaultgraphics")]
        public BasicResponse UpdateSystemDefaultGraphics(UpdateSystemDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            return graphicsbaseinfService.UpdateSystemDefaultGraphics(graphicsbaseinfrequest);
        }
    }
}
