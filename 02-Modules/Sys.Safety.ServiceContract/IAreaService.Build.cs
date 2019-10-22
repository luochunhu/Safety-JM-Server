using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Area;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract
{
    public interface IAreaService
    {
        BasicResponse<AreaInfo> AddArea(AreaAddRequest arearequest);
        BasicResponse<AreaInfo> UpdateArea(AreaUpdateRequest arearequest);
        BasicResponse DeleteArea(AreaDeleteRequest arearequest);
        BasicResponse<List<AreaInfo>> GetAreaList(AreaGetListRequest arearequest);
        BasicResponse<AreaInfo> GetAreaById(AreaGetRequest arearequest);

        BasicResponse<List<AreaInfo>> GetAllAreaList(AreaGetListRequest arearequest);

        /// <summary>
        /// 获取所有区域缓存
        /// </summary>
        /// <param name="arearequest"></param>
        /// <returns></returns>
        BasicResponse<List<AreaInfo>> GetAllAreaCache(AreaCacheGetAllRequest arearequest);
    }
}

