using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsrouteinf;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class GraphicsrouteinfControllerProxy : BaseProxy, IGraphicsrouteinfService
    {
        public BasicResponse<GraphicsrouteinfInfo> AddGraphicsrouteinf(GraphicsrouteinfAddRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/add?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsrouteinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicsrouteinf(GraphicsrouteinfDeleteRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/delete?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsrouteinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicsrouteinfForGraphId(DeleteGraphicsrouteinfRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/deletegraphicsrouteinfforgraphid?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsrouteinfInfo>>(responsestr);
        }

        public BasicResponse<GraphicsrouteinfInfo> GetGraphicsrouteinfById(GraphicsrouteinfGetRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/get?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsrouteinfInfo>>(responsestr);
        }

        public BasicResponse<List<GraphicsrouteinfInfo>> GetGraphicsrouteinfList(GraphicsrouteinfGetListRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/getlist?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<GraphicsrouteinfInfo>>>(responsestr);
        }

        public BasicResponse<GraphicsrouteinfInfo> UpdateGraphicsrouteinf(GraphicsrouteinfUpdateRequest graphicsrouteinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsrouteinf/update?token=" + Token, JSONHelper.ToJSONString(graphicsrouteinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsrouteinfInfo>>(responsestr);
        }
    }
}
