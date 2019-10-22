using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class ClassController : Basic.Framework.Web.WebApi.BasicApiController, IClassService
    {
        IClassService _chartService = ServiceFactory.Create<IClassService>();

        [HttpPost]
        [Route("v1/Class/AddClass")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ClassInfo> AddClass(Sys.Safety.Request.Class.ClassAddRequest classrequest)
        {
            return _chartService.AddClass(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/UpdateClass")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ClassInfo> UpdateClass(Sys.Safety.Request.Class.ClassUpdateRequest classrequest)
        {
            return _chartService.UpdateClass(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/DeleteClass")]
        public Basic.Framework.Web.BasicResponse DeleteClass(Sys.Safety.Request.Class.ClassDeleteRequest classrequest)
        {
            return _chartService.DeleteClass(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/GetClassList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ClassInfo>> GetClassList(Sys.Safety.Request.Class.ClassGetListRequest classrequest)
        {
            return _chartService.GetClassList(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/GetClassById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ClassInfo> GetClassById(Sys.Safety.Request.Class.ClassGetRequest classrequest)
        {
            return _chartService.GetClassById(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/SaveClassList")]
        public Basic.Framework.Web.BasicResponse SaveClassList(Sys.Safety.Request.Class.ClassListAddRequest list)
        {
            return _chartService.SaveClassList(list);
        }

        [HttpPost]
        [Route("v1/Class/DeleteClassByCode")]
        public Basic.Framework.Web.BasicResponse DeleteClassByCode(Sys.Safety.Request.Class.ClassCodeRequest code)
        {
            return _chartService.DeleteClassByCode(code);
        }

        [HttpPost]
        [Route("v1/Class/GetClassDtoByCode")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ClassInfo> GetClassDtoByCode(Sys.Safety.Request.Class.ClassCodeRequest code)
        {
            return _chartService.GetClassDtoByCode(code);
        }

        [HttpPost]
        [Route("v1/Class/GetClassDtoByCode")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ClassInfo> GetClassByStrName(Sys.Safety.Request.Class.GetClassByStrNameRequest classrequest)
        {
            return _chartService.GetClassByStrName(classrequest);
        }

        [HttpPost]
        [Route("v1/Class/SaveClassByCondition")]
        public Basic.Framework.Web.BasicResponse SaveClassByCondition(Sys.Safety.Request.Class.SaveClassByConditionRequest classrequest)
        {
            return _chartService.SaveClassByCondition(classrequest);
        }


        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ClassInfo>> GetAllClassList()
        {
            throw new NotImplementedException();
        }
    }
}
