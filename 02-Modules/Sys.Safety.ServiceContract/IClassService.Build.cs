using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Class;

namespace Sys.Safety.ServiceContract
{
    public interface IClassService
    {
        BasicResponse<ClassInfo> AddClass(ClassAddRequest classrequest);
        BasicResponse<ClassInfo> UpdateClass(ClassUpdateRequest classrequest);
        BasicResponse DeleteClass(ClassDeleteRequest classrequest);
        BasicResponse<List<ClassInfo>> GetClassList(ClassGetListRequest classrequest);
        BasicResponse<List<ClassInfo>> GetAllClassList();
        BasicResponse<ClassInfo> GetClassById(ClassGetRequest classrequest);

        BasicResponse SaveClassList(ClassListAddRequest list);

        BasicResponse DeleteClassByCode(ClassCodeRequest code);

        BasicResponse<ClassInfo> GetClassDtoByCode(ClassCodeRequest code);


        /// <summary>
        /// 根据条件保存SaveClassByCondition
        /// </summary>
        /// <param name="classrequest"></param>
        /// <returns></returns>
        BasicResponse SaveClassByCondition(SaveClassByConditionRequest classrequest);


        /// <summary>
        /// 根据strName获取classInfo
        /// </summary>
        /// <param name="classrequest"></param>
        /// <returns></returns>
        BasicResponse<ClassInfo> GetClassByStrName(GetClassByStrNameRequest classrequest);

    }
}