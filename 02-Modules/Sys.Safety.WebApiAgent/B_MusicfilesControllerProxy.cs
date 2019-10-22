using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class B_MusicfilesControllerProxy : BaseProxy, IB_MusicfilesService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.B_MusicfilesInfo> AddB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesAddRequest b_MusicfilesRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Musicfiles/AddB_Musicfiles?token=" + Token, JSONHelper.ToJSONString(b_MusicfilesRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_MusicfilesInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.B_MusicfilesInfo> UpdateB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesUpdateRequest b_MusicfilesRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Musicfiles/UpdateB_Musicfiles?token=" + Token, JSONHelper.ToJSONString(b_MusicfilesRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_MusicfilesInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteB_Musicfiles(Sys.Safety.Request.B_Musicfiles.B_MusicfilesDeleteRequest b_MusicfilesRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Musicfiles/DeleteB_Musicfiles?token=" + Token, JSONHelper.ToJSONString(b_MusicfilesRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_MusicfilesInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_MusicfilesInfo>> GetB_MusicfilesList(Sys.Safety.Request.B_Musicfiles.B_MusicfilesGetListRequest b_MusicfilesRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Musicfiles/GetB_MusicfilesList?token=" + Token, JSONHelper.ToJSONString(b_MusicfilesRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<B_MusicfilesInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.B_MusicfilesInfo> GetB_MusicfilesById(Sys.Safety.Request.B_Musicfiles.B_MusicfilesGetRequest b_MusicfilesRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/B_Musicfiles/GetB_MusicfilesById?token=" + Token, JSONHelper.ToJSONString(b_MusicfilesRequest));
            return JSONHelper.ParseJSONString<BasicResponse<B_MusicfilesInfo>>(responseStr);
        }
    }
}
