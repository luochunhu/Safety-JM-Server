using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Show;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_ShowService
    {
        BasicResponse<Jc_ShowInfo> AddJc_Show(Jc_ShowAddRequest jc_Showrequest);
        BasicResponse<Jc_ShowInfo> UpdateJc_Show(Jc_ShowUpdateRequest jc_Showrequest);
        BasicResponse DeleteJc_Show(Jc_ShowDeleteRequest jc_Showrequest);
        BasicResponse<List<Jc_ShowInfo>> GetJc_ShowList(Jc_ShowGetListRequest jc_Showrequest);
        BasicResponse<Jc_ShowInfo> GetJc_ShowById(Jc_ShowGetRequest jc_Showrequest);
        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="jc_Showrequest"></param>
        /// <returns></returns>
        BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest jc_Showrequest);
    }
}

