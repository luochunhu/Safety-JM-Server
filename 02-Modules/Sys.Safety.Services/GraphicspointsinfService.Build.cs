using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Graphicspointsinf;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class GraphicspointsinfService : IGraphicspointsinfService
    {
        private IGraphicspointsinfRepository _Repository;

        public GraphicspointsinfService(IGraphicspointsinfRepository _Repository)
        {
            this._Repository = _Repository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("GraphicspointsinfService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        public BasicResponse<GraphicspointsinfInfo> AddGraphicspointsinf(GraphicspointsinfAddRequest graphicspointsinfrequest)
        {
            var graphicspointsinfresponse = new BasicResponse<GraphicspointsinfInfo>();
            if (graphicspointsinfrequest.GraphicspointsinfInfo == null)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = "参数错误！";
                return graphicspointsinfresponse;
            }
            try
            {
                graphicspointsinfrequest.GraphicspointsinfInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                var _graphicspointsinf = ObjectConverter.Copy<GraphicspointsinfInfo, GraphicspointsinfModel>(graphicspointsinfrequest.GraphicspointsinfInfo);
                var resultgraphicspointsinf = _Repository.AddGraphicspointsinf(_graphicspointsinf);
                graphicspointsinfresponse.Data = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(resultgraphicspointsinf);
            }
            catch (Exception ex)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = ex.Message;
                this.ThrowException("AddGraphicspointsinf-新增图形测点", ex);
            }
            return graphicspointsinfresponse;
        }
        public BasicResponse<GraphicspointsinfInfo> UpdateGraphicspointsinf(GraphicspointsinfUpdateRequest graphicspointsinfrequest)
        {
            var graphicspointsinfresponse = new BasicResponse<GraphicspointsinfInfo>();
            if (graphicspointsinfrequest.GraphicspointsinfInfo == null)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = "参数错误！";
                return graphicspointsinfresponse;
            }
            var graphicspointsinfModel = _Repository.GetGraphicspointsinfById(graphicspointsinfrequest.GraphicspointsinfInfo.ID);
            if (graphicspointsinfModel == null)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = "对象不存在！";
                return graphicspointsinfresponse;
            }
            try
            {
                var _graphicspointsinf = ObjectConverter.Copy<GraphicspointsinfInfo, GraphicspointsinfModel>(graphicspointsinfrequest.GraphicspointsinfInfo);
                _Repository.UpdateGraphicspointsinf(_graphicspointsinf);
                graphicspointsinfresponse.Data = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(_graphicspointsinf);
            }
            catch (Exception ex)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = ex.Message;
                this.ThrowException("UpdateGraphicspointsinf-修改图形测点", ex);
            }
            return graphicspointsinfresponse;
        }
        public BasicResponse DeleteGraphicspointsinf(GraphicspointsinfDeleteRequest graphicspointsinfrequest)
        {
            _Repository.DeleteGraphicspointsinf(graphicspointsinfrequest.Id);
            var graphicspointsinfresponse = new BasicResponse();
            return graphicspointsinfresponse;
        }
        public BasicResponse<List<GraphicspointsinfInfo>> GetGraphicspointsinfList(GraphicspointsinfGetListRequest graphicspointsinfrequest)
        {
            var graphicspointsinfresponse = new BasicResponse<List<GraphicspointsinfInfo>>();
            graphicspointsinfrequest.PagerInfo.PageIndex = graphicspointsinfrequest.PagerInfo.PageIndex - 1;
            if (graphicspointsinfrequest.PagerInfo.PageIndex < 0)
            {
                graphicspointsinfrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var graphicspointsinfModelLists = _Repository.GetGraphicspointsinfList(graphicspointsinfrequest.PagerInfo.PageIndex, graphicspointsinfrequest.PagerInfo.PageSize, out rowcount);
            var graphicspointsinfInfoLists = new List<GraphicspointsinfInfo>();
            foreach (var item in graphicspointsinfModelLists)
            {
                var GraphicspointsinfInfo = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(item);
                graphicspointsinfInfoLists.Add(GraphicspointsinfInfo);
            }
            graphicspointsinfresponse.Data = graphicspointsinfInfoLists;
            return graphicspointsinfresponse;
        }
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfById(GraphicspointsinfGetRequest graphicspointsinfrequest)
        {
            var result = _Repository.GetGraphicspointsinfById(graphicspointsinfrequest.Id);
            var graphicspointsinfInfo = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(result);
            var graphicspointsinfresponse = new BasicResponse<GraphicspointsinfInfo>();
            graphicspointsinfresponse.Data = graphicspointsinfInfo;
            return graphicspointsinfresponse;
        }

        /// <summary>
        /// 根据测点名获取GraphicspointsinfInfo
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByPoint(GetGraphicspointsinfByPointRequest graphicspointsinfrequest)
        {
            var response = new BasicResponse<GraphicspointsinfInfo>();
            if (string.IsNullOrWhiteSpace(graphicspointsinfrequest.PointId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var result = _Repository.GetGraphicspointsinfByPoint(graphicspointsinfrequest.PointId);
                var graphicspointsinfInfo = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(result);
                response.Data = graphicspointsinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetGraphicspointsinfByPoint-根据测点名获取GraphicspointsinfInfo", ex);
            }

            return response;
        }
        /// <summary>
        /// 根据测点号及图形ID获取图形测点信息
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByGraphIdAndPoint(GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest)
        {
            var response = new BasicResponse<GraphicspointsinfInfo>();
            if (string.IsNullOrWhiteSpace(graphicspointsinfrequest.PointId) || string.IsNullOrWhiteSpace(graphicspointsinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var result = _Repository.GetGraphicspointsinfByPoint(graphicspointsinfrequest.PointId, graphicspointsinfrequest.GraphId);
                var graphicspointsinfInfo = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(result);
                response.Data = graphicspointsinfInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetGraphicspointsinfByPoint-根据测点名获取GraphicspointsinfInfo", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据测点名称获取测点的绑定类型
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        public BasicResponse<short> Get1GraphBindType(Get1GraphBindTypeRequest graphicspointsinfrequest)
        {
            var response = new BasicResponse<short>();
            if (string.IsNullOrWhiteSpace(graphicspointsinfrequest.PointId) || string.IsNullOrWhiteSpace(graphicspointsinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var result = _Repository.Get1GraphBindType(graphicspointsinfrequest.PointId, graphicspointsinfrequest.GraphId);
                var graphicspointsinfInfo = ObjectConverter.Copy<GraphicspointsinfModel, GraphicspointsinfInfo>(result);
                if (graphicspointsinfInfo != null)
                {
                    response.Data = graphicspointsinfInfo.GraphBindType;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("Get1GraphBindType-根据测点名称获取测点的绑定类型", ex);
            }

            return response;
        }

        /// <summary>
        /// 删除图形的绑定测点信息
        /// </summary>
        /// <param name="graphicspointsinfrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteGraphicsPointsInfForGraphId(DeleteGraphicsPointsInfForGraphIdRequest graphicspointsinfrequest)
        {
            var response = new BasicResponse();
            if (string.IsNullOrWhiteSpace(graphicspointsinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                _Repository.DeleteGraphicsPointsInfForGraphId(graphicspointsinfrequest.GraphId);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("DeleteGraphicsPointsInfForGraphId-删除图形的绑定测点信息", ex); 
            }
            return response;
        }

        /// <summary>
        /// 获取图形上所有测点
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<GraphicspointsinfInfo>> GetAllGraphicspointsinfInfo()
        {
            var graphicspointsinfresponse = new BasicResponse<List<GraphicspointsinfInfo>>();
            try
            {
                var graphicspointsinfModelLists = _Repository.GetAllGraphicspointsinfInfo();
                var graphicspointsinfInfoLists = ObjectConverter.CopyList<GraphicspointsinfModel, GraphicspointsinfInfo>(graphicspointsinfModelLists);
                graphicspointsinfresponse.Data = graphicspointsinfInfoLists.ToList();
            }
            catch (Exception ex)
            {
                graphicspointsinfresponse.Code = -100;
                graphicspointsinfresponse.Message = ex.Message;
                this.ThrowException("GetAllGraphicspointsinfInfo-获取图形上所有测点", ex);
            }
            return graphicspointsinfresponse;
        }
    }
}

