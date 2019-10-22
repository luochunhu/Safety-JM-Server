using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.CBFCommon
{
    public class EnumcodeControllerProxy : BaseProxy, IEnumcodeService
    {
        /// <summary>
        /// 保存枚举
        /// </summary>
        /// <param name="enumcoderequest"></param>
        /// <returns></returns>        
        public BasicResponse<EnumcodeInfo> SaveEnumCode(EnumcodeAddRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/SaveEnumCode?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumcodeInfo>>(responseStr);
        }
        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>       
        public BasicResponse UpdateCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/UpdateCache?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<EnumcodeInfo> AddEnumcode(EnumcodeAddRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/Add?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumcodeInfo>>(responseStr);
        }        
        public BasicResponse<EnumcodeInfo> UpdateEnumcode(EnumcodeUpdateRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/Update?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumcodeInfo>>(responseStr);
        }        
        public BasicResponse DeleteEnumcode(EnumcodeDeleteRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/Delete?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList(EnumcodeGetListRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetPageList?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
        public BasicResponse<EnumcodeInfo> GetEnumcodeById(EnumcodeGetRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/Get?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<EnumcodeInfo>>(responseStr);
        }
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeByEnumTypeID(EnumcodeGetByEnumTypeIDRequest enumcoderequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetEnumcodeByEnumTypeID?token=" + Token, JSONHelper.ToJSONString(enumcoderequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }

        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetAllDevicePropertyCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }

        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetAllDeviceClassCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }


        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceModelCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Enumcode/GetAllDeviceModelCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<EnumcodeInfo>>>(responseStr);
        }
    }
}
