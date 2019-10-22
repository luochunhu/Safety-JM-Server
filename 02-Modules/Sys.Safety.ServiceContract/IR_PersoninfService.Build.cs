using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Personinf;

namespace Sys.Safety.ServiceContract
{
    public interface IR_PersoninfService
    {
        BasicResponse<R_PersoninfInfo> AddPersoninf(R_PersoninfAddRequest personinfRequest);
        BasicResponse<R_PersoninfInfo> UpdatePersoninf(R_PersoninfUpdateRequest personinfRequest);
        BasicResponse DeletePersoninf(R_PersoninfDeleteRequest personinfRequest);
        BasicResponse<List<R_PersoninfInfo>> GetPersoninfList(R_PersoninfGetListRequest personinfRequest);
        BasicResponse<R_PersoninfInfo> GetPersoninfById(R_PersoninfGetRequest personinfRequest);

        BasicResponse<List<R_PersoninfInfo>> GetAllPersonInfo(BasicRequest personinfRequest);
        /// <summary>
        /// 获取所有人员缓存信息
        /// </summary>
        /// <param name="personinfRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PersoninfInfo>> GetAllPersonInfoCache(BasicRequest personinfRequest);

        /// <summary>
        /// 根据ID获取人员信息
        /// </summary>
        /// <param name="personinfRequest"></param>
        /// <returns></returns>
        BasicResponse<R_PersoninfInfo> GetPersoninfCache(R_PersoninfGetRequest personinfRequest);

        /// <summary>
        /// 获取所有已定义人员
        /// </summary>
        /// <param name="personinfRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PersoninfInfo>> GetAllDefinedPersonInfoCache(BasicRequest personinfRequest);

        BasicResponse<List<R_PersoninfInfo>> GetPersoninfCacheByBh(R_PersoninfGetByBhRequest personinfRequest);
    }
}

