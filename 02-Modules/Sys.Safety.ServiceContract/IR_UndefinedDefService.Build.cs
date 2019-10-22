using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.UndefinedDef;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract
{
    public interface IR_UndefinedDefService
    {
        BasicResponse<R_UndefinedDefInfo> AddUndefinedDef(R_UndefinedDefAddRequest undefinedDefRequest);
        BasicResponse<R_UndefinedDefInfo> UpdateUndefinedDef(R_UndefinedDefUpdateRequest undefinedDefRequest);
        BasicResponse DeleteUndefinedDef(R_UndefinedDefDeleteRequest undefinedDefRequest);
        BasicResponse<List<R_UndefinedDefInfo>> GetUndefinedDefList(R_UndefinedDefGetListRequest undefinedDefRequest);
        BasicResponse<R_UndefinedDefInfo> GetUndefinedDefById(R_UndefinedDefGetRequest undefinedDefRequest);
        /// <summary>
        /// 获取所有未定义设备  20171127
        /// </summary>
        /// <param name="RUndefinedDefCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_UndefinedDefInfo>> GetAllRUndefinedDefCache(RUndefinedDefCacheGetAllRequest RUndefinedDefCacheRequest);
        
    }
}

