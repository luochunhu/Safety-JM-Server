using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.Request.Graphicspointsinf;
using Sys.Safety.DataContract.CommunicateExtend;
using System;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:测点定义缓存业务
    /// 修改记录
    /// 2017-05-24
    /// 2017-05-26
    /// </summary>
    public class PointDefineCacheService : IPointDefineCacheService
    {
        private readonly IAreaService areaServie;
        private readonly IGraphicspointsinfService graphicspointsinfService;
        public PointDefineCacheService()
        {
            areaServie = ServiceFactory.Create<IAreaService>();
            graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        }

        public BasicResponse AddPointDefineCache(PointDefineCacheAddRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.AddItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddPointDefineCache(PointDefineCacheBatchAddRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.AddItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdatePointDefineCache(PointDefineCacheBatchUpdateRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public BasicResponse DeletePointDefineCache(PointDefineCacheDeleteRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.DeleteItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchDeletePointDefineCache(PointDefineCacheBatchDeleteRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.DeleteItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache(PointDefineCacheGetAllRequest request)
        {
            //modified by  20170719
            //默认测点查询从只读缓存查询，只有特殊实时性要求高的才从写缓存查询
            var response = new BasicResponse<List<Jc_DefInfo>>();

            if (request.IsQueryFromWriteCache)
            {
                response.Data = PointWriteCache.GetInstance.Query();
            }
            else
            {
                response.Data = PointReadonlyCache.GetInstance.Query();
            }

            return response;
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest request)
        {
            //modified by  20170719
            //默认测点查询从只读缓存查询，只有特殊实时性要求高的才从写缓存查询
            var response = new BasicResponse<List<Jc_DefInfo>>();
            if (request.IsQueryFromWriteCache)
            {
                response.Data = PointWriteCache.GetInstance.Query(request.Predicate);
            }
            else
            {
                response.Data = PointReadonlyCache.GetInstance.Query(request.Predicate);
            }
            return response;
        }

        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByKey(PointDefineCacheGetByKeyRequest request)
        {
            //查询活动点  20170606

            //modified by  20170719
            //默认测点查询从只读缓存查询，只有特殊实时性要求高的才从写缓存查询
            var response = new BasicResponse<Jc_DefInfo>();
            if (request.IsQueryFromWriteCache)
            {
                response.Data = PointWriteCache.GetInstance.Query(pointDefine => pointDefine.Point == request.Point && pointDefine.Activity == "1").FirstOrDefault();
            }
            else
            {
                response.Data = PointReadonlyCache.GetInstance.Query(pointDefine => pointDefine.Point == request.Point && pointDefine.Activity == "1").FirstOrDefault();
            }
            return response;
        }

        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStation(PointDefineCacheGetByStationRequest request)
        {
            //modified by  20170719
            //默认测点查询从只读缓存查询，只有特殊实时性要求高的才从写缓存查询
            var response = new BasicResponse<List<Jc_DefInfo>>();

            if (request.IsQueryFromWriteCache)
            {
                response.Data = PointWriteCache.GetInstance.Query(pointDefine => pointDefine.Fzh == request.Station && pointDefine.Activity == "1" && pointDefine.InfoState != InfoState.Delete);
            }
            else
            {
                response.Data = PointReadonlyCache.GetInstance.Query(pointDefine => pointDefine.Fzh == request.Station && pointDefine.Activity == "1" && pointDefine.InfoState != InfoState.Delete);
            }
            return response;
        }


        public BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest request)
        {
            //modified by  20170719
            //默认测点查询从只读缓存查询，只有特殊实时性要求高的才从写缓存查询
            var response = new BasicResponse<Jc_DefInfo>();
            if (request.IsQueryFromWriteCache)
            {
                response.Data = PointWriteCache.GetInstance.Query(pointDefine => pointDefine.PointID == request.PointID).FirstOrDefault();
            }
            else
            {
                response.Data = PointReadonlyCache.GetInstance.Query(pointDefine => pointDefine.PointID == request.PointID).FirstOrDefault();
            }
            return response;
        }

        public BasicResponse LoadPointDefineCache(PointDefineCacheLoadRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.Load();

            //added by  20170719 加载测点只读缓存数据
            PointReadonlyCache.GetInstance.Load();

            //加载测点定义基本信息之后，加载测点定义拓展属性
            var pointDefineList = PointWriteCache.GetInstance.Query();
            if (pointDefineList.Any())
            {
                LoadPointDefineExtendProperty(pointDefineList);
                //PointWriteCache.GetInstance.UpdateItems(pointDefineList);//取消缓存深复制，存在引用关系  20170814
            }
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineCahce(PointDefineCacheUpdateRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineCommunicateTimes(PointDefineCacheUpdateCommTimesReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateCommTimesInfo(pointDefineCacheRequest.PointDefineInfo, pointDefineCacheRequest.UpdateType);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineControl(PointDefineCacheUpdateControlReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateControlInfo(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineInitInfo(PointDefineCacheUpdateInitInfoReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateInitInfo(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineStationFlag(PointDefineCacheUpdateStationFlatReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateStationFlagInfo(pointDefineCacheRequest.IsInit);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineUniqueCode(PointDefineCacheUpdateUniqueCodeReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateUniqueCodeInfo(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineErrorCount(PointDefineCacheUpdateErrorCountReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateErrorCount(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineRealValue(PointDefineCacheUpdateRealValueReqest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateRealValueInfo(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdateModifyItems(PointDefineCacheUpdateRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdateModifyItems(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdatePointDefineInfo(DefineCacheUpdatePropertiesRequest pointDefineCacheRequest)
        {
            PointWriteCache.GetInstance.UpdatePointInfo(pointDefineCacheRequest.PointID, pointDefineCacheRequest.UpdateItems);
            return new BasicResponse();
        }

        /// <summary>
        /// 批量更新测点缓存字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            //added by  20170719
            Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
            PointWriteCache.GetInstance.BatchUpdatePointInfo(request.PointItems);

            return new BasicResponse();
        }


        /// <summary>
        /// 加载测点定义拓展属性
        /// </summary>
        /// <param name="pointDefines"></param>
        private void LoadPointDefineExtendProperty(List<Jc_DefInfo> pointDefines)
        {
            //位置信息
            var positionList = PositionCache.PositionCahceInstance.Query();
            //设备种类信息
            var deviceClassList = DeviceClassCache.DeviceClassCahceInstance.Query();
            //设备性质信息
            var devicePropertyList = DevicePropertyCache.DeviceDefineCahceInstance.Query();
            //设备型号信息
            var deviceTypeList = DeviceTypeCache.DeviceTypeCahceInstance.Query();
            //设备定义信息
            var deviceDefineList = DeviceDefineCache.DeviceDefineCahceInstance.Query();
            //区域信息
            List<AreaInfo> areaList = new List<AreaInfo>();
            AreaGetListRequest areaRequest = new AreaGetListRequest();
            var areaResponse = areaServie.GetAreaList(areaRequest);
            if (areaResponse != null && areaResponse.IsSuccess)
            {
                areaList = areaResponse.Data;
            }
            //图形信息,只加载通风系统默认图形对应的测点位置信息  20170829
            var request = new GraphicsbaseinfGetListRequest();
            request.PagerInfo.PageIndex = 1;
            request.PagerInfo.PageSize = int.MaxValue;
            IGraphicsbaseinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();
            var response = graphicsbaseinfService.GetGraphicsbaseinfList(request);
            GraphicsbaseinfInfo defaultGraphicsbaseinf = response.Data.Find(a => a.Bz3 == "1");

            List<GraphicspointsinfInfo> graphicspointsinfList = new List<GraphicspointsinfInfo>();
            //GraphicspointsinfGetListRequest graphicspointsinfRequest = new GraphicspointsinfGetListRequest();
            var graphicsbaseinfResponse = graphicspointsinfService.GetAllGraphicspointsinfInfo();
            if (graphicsbaseinfResponse != null && graphicsbaseinfResponse.IsSuccess && defaultGraphicsbaseinf != null)
            {
                graphicspointsinfList = graphicsbaseinfResponse.Data.FindAll(g => g.GraphId == defaultGraphicsbaseinf.GraphId);
            }

            pointDefines.ForEach(pointDefine =>
            {
                if (positionList.Any())
                {
                    var area = areaList.FirstOrDefault(a => a.Areaid == pointDefine.Areaid);
                    pointDefine.AreaName = area == null ? string.Empty : area.Areaname;
                    pointDefine.AreaLoc = area == null ? string.Empty : area.Loc;
                }
                if (positionList.Any())
                {
                    var position = positionList.FirstOrDefault(p => p.WzID == pointDefine.Wzid);
                    pointDefine.Wz = position == null ? string.Empty : position.Wz;
                }
                if (graphicspointsinfList.Any())
                {
                    //从通风系统默认图形中获取测点的默认坐标信息  20170829
                    var graphicspoint = graphicspointsinfList.FirstOrDefault(g => g.Point.Contains(pointDefine.Point));
                    pointDefine.XCoordinate = graphicspoint == null ? string.Empty : graphicspoint.XCoordinate;
                    pointDefine.YCoordinate = graphicspoint == null ? string.Empty : graphicspoint.YCoordinate;
                }
                if (deviceDefineList.Any())
                {
                    var deviceDefine = deviceDefineList.FirstOrDefault(g => g.Devid == pointDefine.Devid);
                    pointDefine.DevName = deviceDefine == null ? string.Empty : deviceDefine.Name;
                    pointDefine.DevPropertyID = deviceDefine == null ? 0 : deviceDefine.Type;
                    pointDefine.DevClassID = deviceDefine == null ? 0 : deviceDefine.Bz3;
                    pointDefine.DevModelID = deviceDefine == null ? 0 : deviceDefine.Bz4;
                    pointDefine.Unit = deviceDefine == null ? string.Empty : deviceDefine.Xs1;
                    //pointDefine.Sysid = deviceDefine != null ? 0 : deviceDefine.Sysid;//去掉  20171207
                }
                if (deviceClassList.Any())
                {
                    var deviceClass = deviceClassList.FirstOrDefault(devc => devc.LngEnumValue == pointDefine.DevClassID);
                    pointDefine.DevClass = deviceClass == null ? string.Empty : deviceClass.StrEnumDisplay;
                }
                if (devicePropertyList.Any())
                {
                    var deviceProperty = devicePropertyList.FirstOrDefault(devc => devc.LngEnumValue == pointDefine.DevPropertyID);
                    pointDefine.DevProperty = deviceProperty == null ? string.Empty : deviceProperty.StrEnumDisplay;
                }
                if (deviceTypeList.Any())
                {
                    var deviceType = deviceTypeList.FirstOrDefault(devc => devc.LngEnumValue == pointDefine.DevModelID);
                    pointDefine.DevModel = deviceType == null ? string.Empty : deviceType.StrEnumDisplay;
                }
                pointDefine.ClsAlarmObj = new AlarmProperty();
                //加载分站最近正常通讯时间(开始置此时间，用来保证开机时不马上写分站中断记录)  20170722
                pointDefine.DttStateTime = DateTime.Now;
                pointDefine.ClsCommObj = new CommProperty(Convert.ToUInt32(pointDefine.Fzh));
                pointDefine.ClsFiveMinObj = new FiveMinData();
                pointDefine.BCommDevTypeMatching = true;
                pointDefine.Alarm = 0;

                pointDefine.DataState = 46;
                pointDefine.State = 46;

                pointDefine.DeviceControlItems = new List<ControlItem>();
                pointDefine.SoleCodingChanels = new List<ControlItem>();
                if (pointDefine.DevPropertyID == 0)
                {
                    pointDefine.ClsCommObj.BInit = false;
                    pointDefine.sendIniCount = 0;

                    pointDefine.LastAcceptFlag = 0;//默认置0

                    pointDefine.StationFaultCount = 20;//起时将容错次数增大，以免分站误中断 
                }
                pointDefine.GetDeviceSoleCoding = 1;//2018.3.26 by 首次下发F命令必须带上获取分站唯一编码命令（K命令必须，I、J命令无影响）
                pointDefine.GetDeviceSoleCodingTime = DateTime.Now;//2018.3.29
                pointDefine.GradingAlarmItems = new List<GradingAlarmItem>();//2018.3.26 by 此处不加，则默认为null，会自动在开机时添加下发分级报警控制命令标记
                pointDefine.IsSendFCommand = false; // 2018.3.26 by 默认未下发F命令，以保证开机时F命令至少下发一次
               

                if (pointDefine.DevPropertyID != 0)
                {
                    pointDefine.PointEditState = 3; //2018.4.9 by  系统启动后，在未收到初始化确认前，不解析传感器数据
                }
            });
        }

        public void Stop()
        {
            PointReadonlyCache.GetInstance.Stop();
        }
    }
}
