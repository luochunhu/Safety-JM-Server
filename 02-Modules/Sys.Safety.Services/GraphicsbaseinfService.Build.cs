using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Graphicsbaseinf;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Enums;

namespace Sys.Safety.Services
{
    public partial class GraphicsbaseinfService : IGraphicsbaseinfService
    {
        /// <summary>
        /// 图形更新标记
        /// </summary>
        private bool GraphSaveFlag = false;
        private IGraphicsbaseinfRepository _Repository;
        private IGraphicspointsinfRepository _GraphicspointsinfRepository;
        private IGraphicsrouteinfRepository _GraphicsrouteinfRepository;

        IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        INetworkModuleCacheService networkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();


        private IV_DefCacheService vdefCacheService = ServiceFactory.Create<IV_DefCacheService>();
        private IB_DefCacheService bdefCacheService = ServiceFactory.Create<IB_DefCacheService>();
        private IRPointDefineCacheService rdefCacheService = ServiceFactory.Create<IRPointDefineCacheService>();

        public GraphicsbaseinfService(IGraphicsbaseinfRepository _Repository, IGraphicspointsinfRepository _GraphicspointsinfRepository, IGraphicsrouteinfRepository _GraphicsrouteinfRepository)
        {
            this._Repository = _Repository;
            this._GraphicspointsinfRepository = _GraphicspointsinfRepository;
            this._GraphicsrouteinfRepository = _GraphicsrouteinfRepository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("GraphicsbaseinfService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }

        public BasicResponse<GraphicsbaseinfInfo> AddGraphicsbaseinf(GraphicsbaseinfAddRequest graphicsbaseinfrequest)
        {
            var graphicsbaseinfresponse = new BasicResponse<GraphicsbaseinfInfo>();
            if (graphicsbaseinfrequest.GraphicsbaseinfInfo == null)
            {
                graphicsbaseinfresponse.Code = -100;
                graphicsbaseinfresponse.Message = "参数错误！";
                return graphicsbaseinfresponse;
            }
            try
            {
                graphicsbaseinfrequest.GraphicsbaseinfInfo.GraphId = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                var _graphicsbaseinf = ObjectConverter.Copy<GraphicsbaseinfInfo, GraphicsbaseinfModel>(graphicsbaseinfrequest.GraphicsbaseinfInfo);
                var resultgraphicsbaseinf = _Repository.AddGraphicsbaseinf(_graphicsbaseinf);
                graphicsbaseinfresponse.Data = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(resultgraphicsbaseinf);
            }
            catch (Exception ex)
            {
                graphicsbaseinfresponse.Code = -100;
                graphicsbaseinfresponse.Message = ex.Message;
                this.ThrowException("AddGraphicsbaseinf-新增图形", ex); ;
            }
            return graphicsbaseinfresponse;
        }
        public BasicResponse<GraphicsbaseinfInfo> UpdateGraphicsbaseinf(GraphicsbaseinfUpdateRequest graphicsbaseinfrequest)
        {
            var graphicsbaseinfresponse = new BasicResponse<GraphicsbaseinfInfo>();
            if (graphicsbaseinfrequest.GraphicsbaseinfInfo == null)
            {
                graphicsbaseinfresponse.Code = -100;
                graphicsbaseinfresponse.Message = "参数错误！";
                return graphicsbaseinfresponse;
            }
            var graphicsbaseinfresmodel = _Repository.GetGraphicsbaseinfById(graphicsbaseinfrequest.GraphicsbaseinfInfo.GraphId);
            if (graphicsbaseinfresmodel == null)
            {
                graphicsbaseinfresponse.Code = -100;
                graphicsbaseinfresponse.Message = "对象不存在！";
                return graphicsbaseinfresponse;
            }
            try
            {
                var _graphicsbaseinf = ObjectConverter.Copy<GraphicsbaseinfInfo, GraphicsbaseinfModel>(graphicsbaseinfrequest.GraphicsbaseinfInfo);
                var systemDefaultGraphicsModel = _Repository.GetSystemtDefaultGraphics(_graphicsbaseinf.Type);
                if (systemDefaultGraphicsModel != null)
                {
                    _Repository.UpdateSystemDefaultGraphics("0", systemDefaultGraphicsModel.GraphId);
                }
                if (_graphicsbaseinf.Bz4 == "1") //修改应急联动默认图形
                {
                    var emergencyLinkageGraphicsModel = _Repository.GetEmergencyLinkageGraphics();
                    if (emergencyLinkageGraphicsModel != null)
                    {
                        _Repository.UpdateEmergencyLinkageGraphics(emergencyLinkageGraphicsModel.GraphId);
                    }
                }
                _Repository.UpdateGraphicsbaseinf(_graphicsbaseinf);
                graphicsbaseinfresponse.Data = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(_graphicsbaseinf);
            }
            catch (Exception ex)
            {
                graphicsbaseinfresponse.Code = -100;
                graphicsbaseinfresponse.Message = ex.Message;
                this.ThrowException("UpdateGraphicsbaseinf-修改图形", ex);
            }
            return graphicsbaseinfresponse;
        }
        public BasicResponse DeleteGraphicsbaseinf(GraphicsbaseinfDeleteRequest graphicsbaseinfrequest)
        {
            _Repository.DeleteGraphicsbaseinf(graphicsbaseinfrequest.Id);
            var graphicsbaseinfresponse = new BasicResponse();
            return graphicsbaseinfresponse;
        }
        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfList(GraphicsbaseinfGetListRequest graphicsbaseinfrequest)
        {
            var graphicsbaseinfresponse = new BasicResponse<List<GraphicsbaseinfInfo>>();
            graphicsbaseinfrequest.PagerInfo.PageIndex = graphicsbaseinfrequest.PagerInfo.PageIndex - 1;
            if (graphicsbaseinfrequest.PagerInfo.PageIndex < 0)
            {
                graphicsbaseinfrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var graphicsbaseinfModelLists = _Repository.GetGraphicsbaseinfList(graphicsbaseinfrequest.PagerInfo.PageIndex, graphicsbaseinfrequest.PagerInfo.PageSize, out rowcount);
            var graphicsbaseinfInfoLists = new List<GraphicsbaseinfInfo>();
            foreach (var item in graphicsbaseinfModelLists)
            {
                var GraphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(item);
                graphicsbaseinfInfoLists.Add(GraphicsbaseinfInfo);
            }
            graphicsbaseinfresponse.Data = graphicsbaseinfInfoLists;
            return graphicsbaseinfresponse;
        }
        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfById(GraphicsbaseinfGetRequest graphicsbaseinfrequest)
        {
            var result = _Repository.GetGraphicsbaseinfById(graphicsbaseinfrequest.Id);
            var graphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(result);
            var graphicsbaseinfresponse = new BasicResponse<GraphicsbaseinfInfo>();
            graphicsbaseinfresponse.Data = graphicsbaseinfInfo;
            return graphicsbaseinfresponse;
        }

        /// <summary>
        /// 设置图形标记
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse SetSaveFlag(SetSaveFlagRequest graphicsbaseinfrequest)
        {
            var respone = new BasicResponse();
            this.GraphSaveFlag = graphicsbaseinfrequest.Flag;
            return respone;
        }

        /// <summary>
        /// 获取图形更新标价
        /// </summary>
        /// <returns></returns>
        public BasicResponse<bool> GetSaveFlag()
        {
            var response = new BasicResponse<bool>();
            response.Data = this.GraphSaveFlag;
            return response;
        }

        /// <summary>
        /// 根据图形名称获取图形信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfByName(GetGraphicsbaseinfByNameRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<GraphicsbaseinfInfo>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphName))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsbaseinfModel = _Repository.GetGraphicsbaseinfByName(graphicsbaseinfrequest.GraphName);
                var graphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(graphicsbaseinfModel);
                response.Data = graphicsbaseinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetGraphicsbaseinfByName-根据图形名称获取图形信息发生异常", ex);
            }
            return response;
        }

        /// <summary>
        /// 根据图形名称获取所有的图形信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfListByName(GetGraphicsbaseinfListByNameRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<List<GraphicsbaseinfInfo>>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphName))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsbaseinfModelLists = _Repository.GetGraphicsbaseinfListByName(graphicsbaseinfrequest.GraphName);
                var graphicsbaseinfInfolLists = ObjectConverter.CopyList<GraphicsbaseinfModel, GraphicsbaseinfInfo>(graphicsbaseinfModelLists);
                response.Data = graphicsbaseinfInfolLists.ToList();
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetGraphicsbaseinfListByName-根据图形名称获取所有的图形信息发生异常", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取图形对应的线路信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMapRoutesInfo(GetMapRoutesInfoRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsrouteinfData = _Repository.GetMapRoutesInfo(graphicsbaseinfrequest.GraphId);
                response.Data = graphicsrouteinfData;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetMapRoutesInfo-获取图形对应的线路信息", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取图形对应测点绑定信息
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMapPointsInfo(GetMapPointsInfoRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsrouteinfData = _Repository.GetMapPointsInfo(graphicsbaseinfrequest.GraphId);

                if (graphicsrouteinfData.Rows.Count>0) 
                {

                    var rdefinfos = rdefCacheService.GetAllRPointDefineCache(new Sys.Safety.Request.PersonCache.RPointDefineCacheGetAllRequest()).Data;
                    var bdefinfos = bdefCacheService.GetAll(new B_DefCacheGetAllRequest()).Data;
                    var vdefinfos = vdefCacheService.GetAll(new V_DefCacheGetAllRequest()).Data;

                    foreach (DataRow row in graphicsrouteinfData.Rows) 
                    {
                        var rdefinfo = rdefinfos.FirstOrDefault(o => o.Point == row["Point"].ToString());
                        if (rdefinfo != null) 
                        {
                            row["wz"] = rdefinfo.Wz;
                            row["name"] = rdefinfo.DevName;
                            continue;
                        }
                        var bdefinfo = bdefinfos.FirstOrDefault(o => o.Point == row["Point"].ToString());
                        if (bdefinfo != null)
                        {
                            row["wz"] = bdefinfo.Wz;
                            row["name"] = bdefinfo.DevName;
                            continue;
                        }
                        var vdefino = vdefinfos.FirstOrDefault(o => o.IPAddress == row["Point"].ToString());
                        if (vdefino != null) 
                        {
                            row["wz"] = vdefino.Devname;
                            row["name"] = "视频";
                            continue;
                        }

                        if (row["SysId"].ToString() == "-1")
                        {
                            row["name"] = "分析模型";
                        }
                    }
                }

                response.Data = graphicsrouteinfData;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetMapPointsInfo-获取图形对应测点绑定信息", ex);
            }

            return response;
        }

        /// <summary>
        ///获取图形更新时间
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetGraphicTimer(GetGraphicTimerRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphName))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsrouteinfData = _Repository.GetGraphicTimer(graphicsbaseinfrequest.GraphName);
                response.Data = graphicsrouteinfData;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetMapPointsInfo-获取图形对应测点绑定信息", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取图形中的所有测点列表
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAllGraphPoint(GetAllGraphPointRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId) || string.IsNullOrWhiteSpace(graphicsbaseinfrequest.Type))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsrouteinfData = _Repository.GetAllGraphPoint(graphicsbaseinfrequest.GraphId);

                DataTable dtRvalue = new DataTable();
                dtRvalue.TableName = "getAllGraphPoint";
                dtRvalue = graphicsrouteinfData.Clone();
                DataRow[] drRval;
                switch (graphicsbaseinfrequest.Type)
                {

                    case "0"://所有测点
                        dtRvalue = graphicsrouteinfData.Copy();
                        break;
                    case "1"://分站
                        drRval = graphicsrouteinfData.Select("type='0'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "2"://所有传感器
                        drRval = graphicsrouteinfData.Select("type>0 and type<10", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "3"://交换机
                        drRval = graphicsrouteinfData.Select("type='10'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            if (dr["Point"].ToString().Contains(".") || dr["Point"].ToString().ToLower().Contains("com"))
                            {
                                dtRvalue.Rows.Add(dr.ItemArray);
                            }
                        }
                        break;
                    case "4"://开关量
                        drRval = graphicsrouteinfData.Select("type='2'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "5"://模拟量
                        drRval = graphicsrouteinfData.Select("type='1'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "6"://控制量
                        drRval = graphicsrouteinfData.Select("type='3'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "7"://开关量及控制量
                        drRval = graphicsrouteinfData.Select("type='3' or type='2'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    default:
                        break;
                }
                response.Data = dtRvalue;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetMapPointsInfo-获取图形对应测点绑定信息", ex);
            }

            return response;
        }

        /// <summary>
        ///  加载所有测点定义信息（含交换机）
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> LoadAllpointDefByType(LoadAllpointDefByTypeRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.Type))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                DataTable dtRvalue = new DataTable();
                DataTable dtPoint = new DataTable();
                DataTable dtIP = new DataTable();
                DataRow[] drRval;
                dtPoint = _Repository.GetAllPoint(); 
                //dtPoint = _Repository.GetAllEmergencyLinkPoint();
                dtIP = _Repository.GetAllIp();
                if (dtIP != null && dtIP.Rows.Count > 0)
                {
                    foreach (DataRow item in dtIP.Rows)
                    {
                        DataRow drtemp = dtPoint.NewRow();
                        drtemp.ItemArray = item.ItemArray;
                        dtPoint.Rows.InsertAt(drtemp, 0);
                    }
                }
                dtRvalue = dtPoint.Clone();
                switch (graphicsbaseinfrequest.Type)
                {
                    case "0"://所有测点
                        dtRvalue = dtPoint.Copy();
                        break;
                    case "1"://分站
                        drRval = dtPoint.Select("type='0'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "2"://所有传感器
                        drRval = dtPoint.Select("type>0 and type<10", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "3"://交换机
                        drRval = dtPoint.Select("type='10'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "4"://开关量
                        drRval = dtPoint.Select("type='2'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "5"://模拟量
                        drRval = dtPoint.Select("type='1'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "6"://控制量
                        drRval = dtPoint.Select("type='3'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                    case "7"://开关量及控制量
                        drRval = dtPoint.Select("type='3' or type='2'", "Point asc");
                        foreach (DataRow dr in drRval)
                        {
                            dtRvalue.Rows.Add(dr.ItemArray);
                        }
                        break;
                }
                response.Data = dtRvalue;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("LoadAllpointDefByType-加载所有测点定义信息（含交换机）", ex);
            }

            return response;
        }

        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteGraphicsbaseinfForGraphId(DeleteGraphicsbaseinfForGraphIdRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                //删除图形
                _Repository.DeleteGraphicsbaseinfForGraphId(graphicsbaseinfrequest.GraphId);
                //删除当前图形的图元路线信息
                _GraphicsrouteinfRepository.DeleteGraphicsrouteinfForGraphId(graphicsbaseinfrequest.GraphId);
                //删除当前图形的测点绑定信息
                _GraphicspointsinfRepository.DeleteGraphicsPointsInfForGraphId(graphicsbaseinfrequest.GraphId);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("DeleteGraphicsbaseinfForGraphId-删除图形", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取交换机信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetSwitchInformation()
        {
            var response = new BasicResponse<DataTable>();
            response.Data = _Repository.GetSwitchInformation();
            return response;
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetDefPointInformation()
        {
            var response = new BasicResponse<DataTable>();
            response.Data = _Repository.GetDefPointInformation();
            return response;
        }

        /// <summary>
        /// 在缓存中获取所有测点信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetDefPointInformationInCache()
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            try
            {
                var pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
                var pointDefineCacheGetAllResponse = pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheGetAllRequest);
                if (pointDefineCacheGetAllResponse.Data != null)
                {
                    response.Data = pointDefineCacheGetAllResponse.Data;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetDefPointInformationInCache-在缓存中获取所有测点信息", ex);
            }
            return response;
        }

        /// <summary>
        /// 在缓存中获取所有交换机
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetSwitchInformationInCache()
        {
            var response = new BasicResponse<List<Jc_MacInfo>>();
            try
            {
                var networkModuleCacheGetAllRequest = new NetworkModuleCacheGetAllRequest();
                var networkModuleCacheGetAllResponse = networkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheGetAllRequest);
                if (networkModuleCacheGetAllResponse.Data != null)
                {
                    response.Data = networkModuleCacheGetAllResponse.Data;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetSwitchInformationInCache-在缓存中获取所有交换机", ex); ;
            }

            return response;
        }

        /// <summary>
        /// 读取所有设备、数据状态枚举类型
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAllDeviceEnumcode()
        {
            var response = new BasicResponse<DataTable>();
            try
            {
                response.Data = _Repository.GetAllDeviceEnumcode();
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetAllDeviceEnumcode-读取所有设备、数据状态枚举类型", ex);
            }
            return response;
        }

        /// <summary>
        /// 加载所有图形信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> LoadGraphicsInfo()
        {
            var response = new BasicResponse<DataTable>();
            response.Data = _Repository.LoadGraphicsInfo();
            return response;
        }

        /// <summary>
        /// 获取图形上的所有测点信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAllPointInMap(GetAllPointInMapRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                response.Data = _Repository.GetAllPointInMap(graphicsbaseinfrequest.GraphId);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetAllPointInMap-获取图形上的所有测点信息", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<GraphicsbaseinfInfo> GetUserDefinedGraphicsByType(GetUserDefinedGraphicsByTypeRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<GraphicsbaseinfInfo>();
            if (!graphicsbaseinfrequest.Type.HasValue)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsbaseinfModel = _Repository.GetUserDefinedGraphicsByType(graphicsbaseinfrequest.Type.Value);
                var graphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(graphicsbaseinfModel);
                response.Data = graphicsbaseinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetUserDefinedGraphicsByType-获取用户自定义图形", ex); ;
            }

            return response;
        }

        /// <summary>
        /// 修改系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateSystemDefaultGraphics(UpdateSystemDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.Bz3) || string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                _Repository.UpdateSystemDefaultGraphics(graphicsbaseinfrequest.Bz3, graphicsbaseinfrequest.GraphId);

            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("UpdateUserDefinedGraphics-修改用户自定义图形", ex);
            }
            return response;
        }

        /// <summary>
        ///获取应急联动图形
        /// </summary>
        /// <returns></returns>
        public BasicResponse<GraphicsbaseinfInfo> GetEmergencyLinkageGraphics()
        {
            var response = new BasicResponse<GraphicsbaseinfInfo>();
            try
            {
                var graphicsbaseinfModel = _Repository.GetEmergencyLinkageGraphics();
                var graphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(graphicsbaseinfModel);
                response.Data = graphicsbaseinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetEmergencyLinkageGraphics-获取应急联动图形", ex);
            }
            return response;
        }


        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateEmergencyLinkageGraphics(UpdateEmergencyLinkageGraphicsRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse();
            if (string.IsNullOrWhiteSpace(graphicsbaseinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                _Repository.UpdateEmergencyLinkageGraphics(graphicsbaseinfrequest.GraphId);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("UpdateEmergencyLinkageGraphics-修改应急联动图形", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <param name="graphicsbaseinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<GraphicsbaseinfInfo> GetSystemtDefaultGraphics(GetSystemtDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            var response = new BasicResponse<GraphicsbaseinfInfo>();
            if (!graphicsbaseinfrequest.Type.HasValue)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var graphicsbaseinfModel = _Repository.GetSystemtDefaultGraphics(graphicsbaseinfrequest.Type.Value);
                var graphicsbaseinfInfo = ObjectConverter.Copy<GraphicsbaseinfModel, GraphicsbaseinfInfo>(graphicsbaseinfModel);
                response.Data = graphicsbaseinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetSystemtDefaultGraphics-获取系统默认图形", ex);
            }

            return response;
        }
    }
}


