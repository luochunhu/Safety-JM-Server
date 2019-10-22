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
using Sys.Safety.Request.Enumtype;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 枚举类型管理WebApi接口
    /// </summary>
    public class EnumtypeController : Basic.Framework.Web.WebApi.BasicApiController, IEnumtypeService
    {
        static EnumtypeController()
        {

        }
        IEnumtypeService _enumtypeService = ServiceFactory.Create<IEnumtypeService>();
        /// <summary>
        /// 保存枚举类型
        /// </summary>
        /// <param name="enumtyperequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Enumtype/SaveEnumType")]
        public BasicResponse<EnumtypeInfo> SaveEnumType(EnumtypeAddRequest enumtyperequest)
        {
            return _enumtypeService.SaveEnumType(enumtyperequest);
        }
        [HttpPost]
        [Route("v1/Enumtype/Add")]
        public BasicResponse<EnumtypeInfo> AddEnumtype(EnumtypeAddRequest enumtyperequest)
        {
            return _enumtypeService.AddEnumtype(enumtyperequest);
        }
        [HttpPost]
        [Route("v1/Enumtype/Update")]
        public BasicResponse<EnumtypeInfo> UpdateEnumtype(EnumtypeUpdateRequest enumtyperequest)
        {
            return _enumtypeService.UpdateEnumtype(enumtyperequest);
        }
        [HttpPost]
        [Route("v1/Enumtype/Delete")]
        public BasicResponse DeleteEnumtype(EnumtypeDeleteRequest enumtyperequest)
        {
            return _enumtypeService.DeleteEnumtype(enumtyperequest);
        }
        [HttpPost]
        [Route("v1/Enumtype/GetPageList")]
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList(EnumtypeGetListRequest enumtyperequest)
        {
            return _enumtypeService.GetEnumtypeList(enumtyperequest);
        }
        [HttpPost]
        [Route("v1/Enumtype/GetAllList")]
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList()
        {
            return _enumtypeService.GetEnumtypeList();
        }
         [HttpPost]
        [Route("v1/Enumtype/Get")]
        public BasicResponse<EnumtypeInfo> GetEnumtypeById(EnumtypeGetRequest enumtyperequest)
        {
            return _enumtypeService.GetEnumtypeById(enumtyperequest);
        }
         [HttpPost]
        [Route("v1/Enumtype/GetByStrCode")]
        public BasicResponse<EnumtypeInfo> GetEnumtypeByStrCode(EnumtypeGetByStrCodeRequest enumtyperequest)
        {
            return _enumtypeService.GetEnumtypeByStrCode(enumtyperequest);
        }
    }
}
