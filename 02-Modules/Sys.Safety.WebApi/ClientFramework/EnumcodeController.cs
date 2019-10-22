using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Enumcode;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 枚举管理WebApi接口
    /// </summary>
    public class EnumcodeController : Basic.Framework.Web.WebApi.BasicApiController, IEnumcodeService
    {
        static EnumcodeController()
        {

        }
        IEnumcodeService _enumcodeService = ServiceFactory.Create<IEnumcodeService>();
        /// <summary>
        /// 保存枚举
        /// </summary>
        /// <param name="enumcoderequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Enumcode/SaveEnumCode")]
        public BasicResponse<EnumcodeInfo> SaveEnumCode(EnumcodeAddRequest enumcoderequest)
        {
            return _enumcodeService.SaveEnumCode(enumcoderequest);
        }
        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>
        [HttpPost]
        [Route("v1/Enumcode/UpdateCache")]
        public BasicResponse UpdateCache()
        {
            return _enumcodeService.UpdateCache();
        }
        [HttpPost]
        [Route("v1/Enumcode/Add")]
        public BasicResponse<EnumcodeInfo> AddEnumcode(EnumcodeAddRequest enumcoderequest)
        {
            return _enumcodeService.AddEnumcode(enumcoderequest);
        }
        [HttpPost]
        [Route("v1/Enumcode/Update")]
        public BasicResponse<EnumcodeInfo> UpdateEnumcode(EnumcodeUpdateRequest enumcoderequest)
        {
            return _enumcodeService.UpdateEnumcode(enumcoderequest);
        }
        [HttpPost]
        [Route("v1/Enumcode/Delete")]
        public BasicResponse DeleteEnumcode(EnumcodeDeleteRequest enumcoderequest)
        {
            return _enumcodeService.DeleteEnumcode(enumcoderequest);
        }
        [HttpPost]
        [Route("v1/Enumcode/GetPageList")]
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList(EnumcodeGetListRequest enumcoderequest)
        {
            return _enumcodeService.GetEnumcodeList(enumcoderequest);
        }
        [HttpPost]
        [Route("v1/Enumcode/GetAllList")]
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList()
        {
            return _enumcodeService.GetEnumcodeList();
        }
        [HttpPost]
        [Route("v1/Enumcode/Get")]
        public BasicResponse<EnumcodeInfo> GetEnumcodeById(EnumcodeGetRequest enumcoderequest)
        {
            return _enumcodeService.GetEnumcodeById(enumcoderequest);
        }
        [HttpPost]
        [Route("v1/Enumcode/GetEnumcodeByEnumTypeID")]
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeByEnumTypeID(EnumcodeGetByEnumTypeIDRequest enumcoderequest)
        {
            return _enumcodeService.GetEnumcodeByEnumTypeID(enumcoderequest);
        }

        [HttpPost]
        [Route("v1/Enumcode/GetAllDevicePropertyCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            return _enumcodeService.GetAllDevicePropertyCache();
        }

        [HttpPost]
        [Route("v1/Enumcode/GetAllDeviceClassCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            return _enumcodeService.GetAllDeviceClassCache();
        }



        [HttpPost]
        [Route("v1/Enumcode/GetAllDeviceModelCache")]
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceModelCache()
        {
            return _enumcodeService.GetAllDeviceModelCache();
        }
    }
}
