using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Cache.Person;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.Request.R_Restrictedperson;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.KJ237Cache
{
    public class RPointDefineCacheService : IRPointDefineCacheService
    {
        private readonly IAreaService areaServie;
        private readonly IGraphicspointsinfService graphicspointsinfService;

        public RPointDefineCacheService()
        {
            areaServie = ServiceFactory.Create<IAreaService>();
            graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        }

        public Basic.Framework.Web.BasicResponse LoadRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheLoadRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.Load();

            //加载测点定义基本信息之后，加载测点定义拓展属性
            var pointDefineList = RPointDefineCache.GetInstance.Query();
            if (pointDefineList.Any())
            {
                LoadPointDefineExtendProperty(pointDefineList);
            }
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheAddRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.AddItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BacthAddRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheBatchAddRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.AddItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRPointDefineCahce(Sys.Safety.Request.PersonCache.RPointDefineCacheUpdateRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.UpdateItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchRUpdatePointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheBatchUpdateRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.UpdateItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheDeleteRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.DeleteItem(pointDefineCacheRequest.PointDefineInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheBatchDeleteRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.DeleteItems(pointDefineCacheRequest.PointDefineInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> GetAllRPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheGetAllRequest pointDefineCacheRequest)
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            response.Data = RPointDefineCache.GetInstance.Query();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefInfo> PointDefineCacheByPointIdRequeest(Sys.Safety.Request.PersonCache.RPointDefineCacheByPointIdRequeest pointDefineCacheRequest)
        {
            var response = new BasicResponse<Jc_DefInfo>();
            response.Data = RPointDefineCache.GetInstance.Query(o=>o.PointID==pointDefineCacheRequest.PointID).FirstOrDefault();
            return response;
        }
        
        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> GetPointDefineCache(Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest)
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            response.Data = RPointDefineCache.GetInstance.Query(pointDefineCacheRequest.Predicate);
            return response;
        }

        public Basic.Framework.Web.BasicResponse UpdateRPointDefineInfo(Sys.Safety.Request.PersonCache.RDefineCacheUpdatePropertiesRequest pointDefineCacheRequest)
        {
            RPointDefineCache.GetInstance.UpdatePointInfo(pointDefineCacheRequest.PointID, pointDefineCacheRequest.UpdateItems);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdatePointDefineInfo(Sys.Safety.Request.PersonCache.RDefineCacheBatchUpdatePropertiesRequest request)
        {
            Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
            RPointDefineCache.GetInstance.BatchUpdatePointInfo(request.PointItems);

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
            //加载识别器限制进入、禁止进入人员信息  20171122
            List<R_RestrictedpersonInfo> RestrictedpersonInfoList = new List<R_RestrictedpersonInfo>();
            IR_RestrictedpersonService r_RestrictedpersonService = ServiceFactory.Create<IR_RestrictedpersonService>();
            R_RestrictedpersonGetListRequest restrictedpersonRequest=new R_RestrictedpersonGetListRequest();
            RestrictedpersonInfoList = r_RestrictedpersonService.GetRestrictedpersonList(restrictedpersonRequest).Data;

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
                    //pointDefine.Sysid = deviceDefine != null ? 0 : deviceDefine.Sysid; //去掉  20171207
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
                //加载识别器禁止进入、限制进入人员信息  20171122
                if (RestrictedpersonInfoList.Any())
                {
                    var tempRestrictedpersonInfoList = RestrictedpersonInfoList.FindAll(a => a.PointId == pointDefine.PointID);
                    pointDefine.RestrictedpersonInfoList = tempRestrictedpersonInfoList;
                }
                //加载识别器类型信息  20171122
                pointDefine.RecognizerTypeDesc = EnumHelper.GetEnumDescription((Recognizer)pointDefine.Bz1);

                //赋值分站识别器的初始状态
                pointDefine.DataState = 46;  
                pointDefine.State = 46;

                pointDefine.ClsCommObj = new DataContract.CommunicateExtend.CommProperty((uint)pointDefine.Fzh);
                pointDefine.DttStateTime = DateTime.Now;  //2017.11.30 by 

                if (pointDefine.DevPropertyID == 0)
                {
                    pointDefine.ClsCommObj.BInit = false;
                    pointDefine.sendIniCount = 1;
                }
            });
        }
    }
}
