using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicspointsinf;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class GraphicspointsinfControllerProxy : BaseProxy, IGraphicspointsinfService
    {
        public BasicResponse<GraphicspointsinfInfo> AddGraphicspointsinf(GraphicspointsinfAddRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/add?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicspointsinf(GraphicspointsinfDeleteRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/delete?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicsPointsInfForGraphId(DeleteGraphicsPointsInfForGraphIdRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/deletegraphicspointsinfforgraphid?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse<short> Get1GraphBindType(Get1GraphBindTypeRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/get1graphbindtype?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<short>>(responsestr);
        }

        public BasicResponse<List<GraphicspointsinfInfo>> GetAllGraphicspointsinfInfo()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/GetAllGraphicspointsinfInfo?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<GraphicspointsinfInfo>>>(responsestr);
        }

        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfById(GraphicspointsinfGetRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/get?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByGraphIdAndPoint(GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/GetGraphicspointsinfByGraphIdAndPoint?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse<GraphicspointsinfInfo> GetGraphicspointsinfByPoint(GetGraphicspointsinfByPointRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/getgraphicspointsinfbypoint?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }

        public BasicResponse<List<GraphicspointsinfInfo>> GetGraphicspointsinfList(GraphicspointsinfGetListRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/getlist?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<GraphicspointsinfInfo>>>(responsestr);
        }

        public BasicResponse<GraphicspointsinfInfo> UpdateGraphicspointsinf(GraphicspointsinfUpdateRequest graphicspointsinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicspointsinf/update?token=" + Token, JSONHelper.ToJSONString(graphicspointsinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicspointsinfInfo>>(responsestr);
        }
    }
}
