using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsbaseinf;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class GraphicsbaseinfControllerProxy : BaseProxy, IGraphicsbaseinfService
    {
        public BasicResponse<GraphicsbaseinfInfo> AddGraphicsbaseinf(GraphicsbaseinfAddRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/add?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicsbaseinf(GraphicsbaseinfDeleteRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/delete?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse DeleteGraphicsbaseinfForGraphId(DeleteGraphicsbaseinfForGraphIdRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/deleteforgraphid?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse<DataTable> GetAllDeviceEnumcode()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getalldeviceenumcode?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetAllGraphPoint(GetAllGraphPointRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getallgraphpoint?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetAllPointInMap(GetAllPointInMapRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getallpointinmap?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetDefPointInformation()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getdefpointinformation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<List<Jc_DefInfo>> GetDefPointInformationInCache()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getdefpointinformationincache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responsestr);
        }

        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfById(GraphicsbaseinfGetRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/get?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse<GraphicsbaseinfInfo> GetGraphicsbaseinfByName(GetGraphicsbaseinfByNameRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getbyname?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfList(GraphicsbaseinfGetListRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getlist?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<GraphicsbaseinfInfo>>>(responsestr);
        }

        public BasicResponse<List<GraphicsbaseinfInfo>> GetGraphicsbaseinfListByName(GetGraphicsbaseinfListByNameRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getlistbyname?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<GraphicsbaseinfInfo>>>(responsestr);
        }

        public BasicResponse<DataTable> GetGraphicTimer(GetGraphicTimerRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getgraphictimer?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetMapPointsInfo(GetMapPointsInfoRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getmappointsinfo?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetMapRoutesInfo(GetMapRoutesInfoRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getmaproutesinfo?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<bool> GetSaveFlag()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getsaveflag?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }

        public BasicResponse<DataTable> GetSwitchInformation()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getswitchinformation?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<List<Jc_MacInfo>> GetSwitchInformationInCache()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getswitchinformationincache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_MacInfo>>>(responsestr);
        }

        public BasicResponse<DataTable> LoadAllpointDefByType(LoadAllpointDefByTypeRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/loadallpointdefbytype?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> LoadGraphicsInfo()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/loadgraphicsinfo?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse SetSaveFlag(SetSaveFlagRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/setsaveflag?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<GraphicsbaseinfInfo> UpdateGraphicsbaseinf(GraphicsbaseinfUpdateRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/update?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }
        public BasicResponse<GraphicsbaseinfInfo> GetUserDefinedGraphicsByType(GetUserDefinedGraphicsByTypeRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getuserdefinedgraphicsbytype?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }
        public BasicResponse<GraphicsbaseinfInfo> GetEmergencyLinkageGraphics()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getemergencylinkagegraphics?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }
        public BasicResponse UpdateEmergencyLinkageGraphics(UpdateEmergencyLinkageGraphicsRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/updateemergencylinkagegraphics?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<GraphicsbaseinfInfo> GetSystemtDefaultGraphics(GetSystemtDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/getsystemtdefaultgraphics?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse<GraphicsbaseinfInfo>>(responsestr);
        }

        public BasicResponse UpdateSystemDefaultGraphics(UpdateSystemDefaultGraphicsRequest graphicsbaseinfrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/graphicsbaseinf/updatesystemdefaultgraphics?token=" + Token, JSONHelper.ToJSONString(graphicsbaseinfrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }
    }
}
