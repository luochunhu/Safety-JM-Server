using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Graphicsrouteinf;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class GraphicsrouteinfService : IGraphicsrouteinfService
    {
        private IGraphicsrouteinfRepository _Repository;

        public GraphicsrouteinfService(IGraphicsrouteinfRepository _Repository)
        {
            this._Repository = _Repository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("GraphicsrouteinfService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        public BasicResponse<GraphicsrouteinfInfo> AddGraphicsrouteinf(GraphicsrouteinfAddRequest graphicsrouteinfrequest)
        {
            var graphicsrouteinfresponse = new BasicResponse<GraphicsrouteinfInfo>();
            if (graphicsrouteinfrequest.GraphicsrouteinfInfo == null)
            {
                graphicsrouteinfresponse.Code = -100;
                graphicsrouteinfresponse.Message = "参数错误！";
                return graphicsrouteinfresponse;
            }
            try
            {
                graphicsrouteinfrequest.GraphicsrouteinfInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                var _graphicsrouteinf = ObjectConverter.Copy<GraphicsrouteinfInfo, GraphicsrouteinfModel>(graphicsrouteinfrequest.GraphicsrouteinfInfo);
                var resultgraphicsrouteinf = _Repository.AddGraphicsrouteinf(_graphicsrouteinf);
                graphicsrouteinfresponse.Data = ObjectConverter.Copy<GraphicsrouteinfModel, GraphicsrouteinfInfo>(resultgraphicsrouteinf);
            }
            catch (Exception ex)
            {
                graphicsrouteinfresponse.Code = -100;
                graphicsrouteinfresponse.Message = ex.Message;
                this.ThrowException("AddGraphicsrouteinf-新增图形绑定路线", ex);
            }

            return graphicsrouteinfresponse;
        }
        public BasicResponse<GraphicsrouteinfInfo> UpdateGraphicsrouteinf(GraphicsrouteinfUpdateRequest graphicsrouteinfrequest)
        {
            var graphicsrouteinfresponse = new BasicResponse<GraphicsrouteinfInfo>();
            if (graphicsrouteinfrequest.GraphicsrouteinfInfo == null)
            {
                graphicsrouteinfresponse.Code = -100;
                graphicsrouteinfresponse.Message = "参数错误！";
                return graphicsrouteinfresponse;
            }
            var graphicsrouteinfModel = _Repository.GetGraphicsrouteinfById(graphicsrouteinfrequest.GraphicsrouteinfInfo.ID);
            if (graphicsrouteinfModel == null)
            {
                graphicsrouteinfresponse.Code = -100;
                graphicsrouteinfresponse.Message = "对象不存在！";
                return graphicsrouteinfresponse;
            }

            try
            {
                var _graphicsrouteinf = ObjectConverter.Copy<GraphicsrouteinfInfo, GraphicsrouteinfModel>(graphicsrouteinfrequest.GraphicsrouteinfInfo);
                _Repository.UpdateGraphicsrouteinf(_graphicsrouteinf);
                graphicsrouteinfresponse.Data = ObjectConverter.Copy<GraphicsrouteinfModel, GraphicsrouteinfInfo>(_graphicsrouteinf);
            }
            catch (Exception ex)
            {
                graphicsrouteinfresponse.Code = -100;
                graphicsrouteinfresponse.Message = ex.Message;
                this.ThrowException("UpdateGraphicsrouteinf-修改图形绑定线路信息", ex);
            }

            return graphicsrouteinfresponse;
        }
        public BasicResponse DeleteGraphicsrouteinf(GraphicsrouteinfDeleteRequest graphicsrouteinfrequest)
        {
            _Repository.DeleteGraphicsrouteinf(graphicsrouteinfrequest.Id);
            var graphicsrouteinfresponse = new BasicResponse();
            return graphicsrouteinfresponse;
        }
        public BasicResponse<List<GraphicsrouteinfInfo>> GetGraphicsrouteinfList(GraphicsrouteinfGetListRequest graphicsrouteinfrequest)
        {
            var graphicsrouteinfresponse = new BasicResponse<List<GraphicsrouteinfInfo>>();
            graphicsrouteinfrequest.PagerInfo.PageIndex = graphicsrouteinfrequest.PagerInfo.PageIndex - 1;
            if (graphicsrouteinfrequest.PagerInfo.PageIndex < 0)
            {
                graphicsrouteinfrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var graphicsrouteinfModelLists = _Repository.GetGraphicsrouteinfList(graphicsrouteinfrequest.PagerInfo.PageIndex, graphicsrouteinfrequest.PagerInfo.PageSize, out rowcount);
            var graphicsrouteinfInfoLists = new List<GraphicsrouteinfInfo>();
            foreach (var item in graphicsrouteinfModelLists)
            {
                var GraphicsrouteinfInfo = ObjectConverter.Copy<GraphicsrouteinfModel, GraphicsrouteinfInfo>(item);
                graphicsrouteinfInfoLists.Add(GraphicsrouteinfInfo);
            }
            graphicsrouteinfresponse.Data = graphicsrouteinfInfoLists;
            return graphicsrouteinfresponse;
        }
        public BasicResponse<GraphicsrouteinfInfo> GetGraphicsrouteinfById(GraphicsrouteinfGetRequest graphicsrouteinfrequest)
        {
            var result = _Repository.GetGraphicsrouteinfById(graphicsrouteinfrequest.Id);
            var graphicsrouteinfInfo = ObjectConverter.Copy<GraphicsrouteinfModel, GraphicsrouteinfInfo>(result);
            var graphicsrouteinfresponse = new BasicResponse<GraphicsrouteinfInfo>();
            graphicsrouteinfresponse.Data = graphicsrouteinfInfo;
            return graphicsrouteinfresponse;
        }


        /// <summary>
        /// 删除图形绑定线路信息
        /// </summary>
        /// <param name="graphicsrouteinfrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteGraphicsrouteinfForGraphId(DeleteGraphicsrouteinfRequest graphicsrouteinfrequest)
        {
            var response = new BasicResponse();
            if (string.IsNullOrWhiteSpace(graphicsrouteinfrequest.GraphId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                _Repository.DeleteGraphicsrouteinfForGraphId(graphicsrouteinfrequest.GraphId);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("DeleteGraphicsrouteinfForGraphId-删除图形绑定线路信息", ex);
            }
            return response;
        }
    }
}


