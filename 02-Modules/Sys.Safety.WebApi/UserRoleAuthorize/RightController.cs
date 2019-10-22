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
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.Request.Right;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 权限管理WebApi接口
    /// </summary>
    public class RightController : Basic.Framework.Web.WebApi.BasicApiController, IRightService
    {
        static RightController()
        {

        }
        IRightService _rightService = ServiceFactory.Create<IRightService>();        
        [HttpPost]
        [Route("v1/Right/Add")]
        public BasicResponse<RightInfo> AddRight(RightAddRequest rightrequest)
        {
            return _rightService.AddRight(rightrequest);
        }
        [HttpPost]
        [Route("v1/Right/Update")]
        public BasicResponse<RightInfo> UpdateRight(RightUpdateRequest rightrequest)
        {
            return _rightService.UpdateRight(rightrequest);
        }
        [HttpPost]
        [Route("v1/Right/Delete")]
        public BasicResponse DeleteRight(RightDeleteRequest rightrequest)
        {
            return _rightService.DeleteRight(rightrequest);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Right/DeleteRights")]
        public BasicResponse DeleteRights(RightsDeleteRequest rightrequest)
        {
            return _rightService.DeleteRights(rightrequest);
        }
        [HttpPost]
        [Route("v1/Right/GetPageList")]
        public BasicResponse<List<RightInfo>> GetRightList(RightGetListRequest rightrequest)
        {
            return _rightService.GetRightList(rightrequest);
        }
        [HttpPost]
        [Route("v1/Right/GetAllList")]
        public BasicResponse<List<RightInfo>> GetRightList()
        {
            return _rightService.GetRightList();
        }
        [HttpPost]
        [Route("v1/Right/Get")]
        public BasicResponse<RightInfo> GetRightById(RightGetRequest rightrequest)
        {
            return _rightService.GetRightById(rightrequest);
        }
        /// <summary>
        /// 添加一个全新信息到权限表并返回成功后的权限对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Right/AddRightEx")]
        public BasicResponse<RightInfo> AddRightEx(RightAddRequest rightrequest)
        {
            return _rightService.AddRightEx(rightrequest);
        }
    }
}
