using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Day;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_DayService:IJc_DayService
    {
		private IJc_DayRepository _Repository;

		public Jc_DayService(IJc_DayRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_DayInfo> AddJc_Day(Jc_DayAddRequest jc_Dayrequest)
        {
            var _jc_Day = ObjectConverter.Copy<Jc_DayInfo, Jc_DayModel>(jc_Dayrequest.Jc_DayInfo);
            var resultjc_Day = _Repository.AddJc_Day(_jc_Day);
            var jc_Dayresponse = new BasicResponse<Jc_DayInfo>();
            jc_Dayresponse.Data = ObjectConverter.Copy<Jc_DayModel, Jc_DayInfo>(resultjc_Day);
            return jc_Dayresponse;
        }
				public BasicResponse<Jc_DayInfo> UpdateJc_Day(Jc_DayUpdateRequest jc_Dayrequest)
        {
            var _jc_Day = ObjectConverter.Copy<Jc_DayInfo, Jc_DayModel>(jc_Dayrequest.Jc_DayInfo);
            _Repository.UpdateJc_Day(_jc_Day);
            var jc_Dayresponse = new BasicResponse<Jc_DayInfo>();
            jc_Dayresponse.Data = ObjectConverter.Copy<Jc_DayModel, Jc_DayInfo>(_jc_Day);  
            return jc_Dayresponse;
        }
				public BasicResponse DeleteJc_Day(Jc_DayDeleteRequest jc_Dayrequest)
        {
            _Repository.DeleteJc_Day(jc_Dayrequest.Id);
            var jc_Dayresponse = new BasicResponse();            
            return jc_Dayresponse;
        }
				public BasicResponse<List<Jc_DayInfo>> GetJc_DayList(Jc_DayGetListRequest jc_Dayrequest)
        {
            var jc_Dayresponse = new BasicResponse<List<Jc_DayInfo>>();
            jc_Dayrequest.PagerInfo.PageIndex = jc_Dayrequest.PagerInfo.PageIndex - 1;
            if (jc_Dayrequest.PagerInfo.PageIndex < 0)
            {
                jc_Dayrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_DayModelLists = _Repository.GetJc_DayList(jc_Dayrequest.PagerInfo.PageIndex, jc_Dayrequest.PagerInfo.PageSize, out rowcount);
            var jc_DayInfoLists = new List<Jc_DayInfo>();
            foreach (var item in jc_DayModelLists)
            {
                var Jc_DayInfo = ObjectConverter.Copy<Jc_DayModel, Jc_DayInfo>(item);
                jc_DayInfoLists.Add(Jc_DayInfo);
            }
            jc_Dayresponse.Data = jc_DayInfoLists;
            return jc_Dayresponse;
        }
				public BasicResponse<Jc_DayInfo> GetJc_DayById(Jc_DayGetRequest jc_Dayrequest)
        {
            var result = _Repository.GetJc_DayById(jc_Dayrequest.Id);
            var jc_DayInfo = ObjectConverter.Copy<Jc_DayModel, Jc_DayInfo>(result);
            var jc_Dayresponse = new BasicResponse<Jc_DayInfo>();
            jc_Dayresponse.Data = jc_DayInfo;
            return jc_Dayresponse;
        }
	}
}


