using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Enumtype;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class EnumtypeControllerProxy : BaseProxy, IEnumtypeService
    {
        /// <summary>
        /// 保存枚举类型
        /// </summary>
        /// <param name="enumtyperequest"></param>
        /// <returns></returns>        
        public BasicResponse<EnumtypeInfo> SaveEnumType(EnumtypeAddRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/SaveEnumType?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumtypeInfo>>(responseStr);
        }        
        public BasicResponse<EnumtypeInfo> AddEnumtype(EnumtypeAddRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/Add?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumtypeInfo>>(responseStr);
        }        
        public BasicResponse<EnumtypeInfo> UpdateEnumtype(EnumtypeUpdateRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/Update?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumtypeInfo>>(responseStr);
        }        
        public BasicResponse DeleteEnumtype(EnumtypeDeleteRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/Delete?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }       
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList(EnumtypeGetListRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/GetPageList?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumtypeInfo>>>(responseStr);
        }
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumtypeInfo>>>(responseStr);
        }
        public BasicResponse<EnumtypeInfo> GetEnumtypeById(EnumtypeGetRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/Get?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumtypeInfo>>(responseStr);
        }
        public BasicResponse<EnumtypeInfo> GetEnumtypeByStrCode(EnumtypeGetByStrCodeRequest enumtyperequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumtype/GetByStrCode?token=" + Token, JSONHelper.ToJSONString(enumtyperequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumtypeInfo>>(responseStr);
        }
    }
}
