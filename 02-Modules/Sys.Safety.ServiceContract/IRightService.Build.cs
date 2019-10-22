using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Right;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRightService
    {
        BasicResponse<RightInfo> AddRight(RightAddRequest rightrequest);
        BasicResponse<RightInfo> UpdateRight(RightUpdateRequest rightrequest);
        BasicResponse DeleteRight(RightDeleteRequest rightrequest);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRights(RightsDeleteRequest rightrequest);
        BasicResponse<List<RightInfo>> GetRightList(RightGetListRequest rightrequest);
        BasicResponse<List<RightInfo>> GetRightList();
        BasicResponse<RightInfo> GetRightById(RightGetRequest rightrequest);
        /// <summary>
        /// 添加一个全新信息到权限表并返回成功后的权限对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        BasicResponse<RightInfo> AddRightEx(RightAddRequest rightrequest);
    }
}

