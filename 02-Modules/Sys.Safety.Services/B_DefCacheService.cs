using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Cache.Audio;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.Request.R_Restrictedperson;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class B_DefCacheService:IB_DefCacheService
    {
         private readonly IAreaService areaServie;
        private readonly IGraphicspointsinfService graphicspointsinfService;

        public B_DefCacheService()
        {
            areaServie = ServiceFactory.Create<IAreaService>();
            graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        }
        public Basic.Framework.Web.BasicResponse LoadCache(Sys.Safety.Request.Cache.B_DefCacheLoadRequest bDefCacheRequest)
        {
            B_DefCache.Instance.Load();

            //加载测点定义基本信息之后，加载测点定义拓展属性
            var pointDefineList = B_DefCache.Instance.Query();
            if (pointDefineList.Any())
            {
                LoadPointDefineExtendProperty(pointDefineList);
            }
            return new BasicResponse();
        }
        public Basic.Framework.Web.BasicResponse Insert(Sys.Safety.Request.Cache.B_DefCacheInsertRequest bDefCacheRequest)
        {
            B_DefCache.Instance.AddItem(bDefCacheRequest.B_DefInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchInsert(Sys.Safety.Request.Cache.B_DefCacheBatchInsertRequest bDefCacheRequest)
        {
            B_DefCache.Instance.AddItems(bDefCacheRequest.B_DefInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse Update(Sys.Safety.Request.Cache.B_DefCacheUpdateRequest bDefCacheRequest)
        {
            B_DefCache.Instance.UpdateItem(bDefCacheRequest.B_DefInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdate(Sys.Safety.Request.Cache.B_DefCacheBatchUpdateRequest bDefCacheRequest)
        {
            B_DefCache.Instance.UpdateItems(bDefCacheRequest.B_DefInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse Delete(Sys.Safety.Request.Cache.B_DefCacheDeleteRequest bDefCacheRequest)
        {
            B_DefCache.Instance.DeleteItem(bDefCacheRequest.B_DefInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDelete(Sys.Safety.Request.Cache.B_DefCacheBatchDeleteRequest bDefCacheRequest)
        {
            B_DefCache.Instance.DeleteItems(bDefCacheRequest.B_DefInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_DefInfo> GetById(Sys.Safety.Request.Cache.B_DefCacheGetByIdRequest bDefCacheRequest)
        {
            BasicResponse<DataContract.Jc_DefInfo> response = new BasicResponse<DataContract.Jc_DefInfo>();
            response.Data = B_DefCache.Instance.Query(o => o.PointID == bDefCacheRequest.PointId).FirstOrDefault();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> GetAll(Sys.Safety.Request.Cache.B_DefCacheGetAllRequest bDefCacheRequest)
        {
            BasicResponse<List<DataContract.Jc_DefInfo>> response = new BasicResponse<List<DataContract.Jc_DefInfo>>();
            response.Data = B_DefCache.Instance.Query();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_DefInfo>> Get(Sys.Safety.Request.Cache.B_DefCacheGetByConditionRequest bDefCacheRequest)
        {
            BasicResponse<List<DataContract.Jc_DefInfo>> response = new BasicResponse<List<DataContract.Jc_DefInfo>>();
            response.Data = B_DefCache.Instance.Query(bDefCacheRequest.predicate);
            return response;
        }

        public Basic.Framework.Web.BasicResponse UpdateInfo(Sys.Safety.Request.Cache.UpdatePropertiesRequest bDefCacheRequest)
        {
            B_DefCache.Instance.UpdatePointInfo(bDefCacheRequest.PointID, bDefCacheRequest.UpdateItems);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateInfo(Sys.Safety.Request.Cache.BatchUpdatePropertiesRequest bDefCacheRequest)
        {
            Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
            B_DefCache.Instance.BatchUpdatePointInfo(bDefCacheRequest.PointItems);

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
               

                //赋值初始状态
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
