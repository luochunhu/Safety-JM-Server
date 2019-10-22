using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AreaAlarm;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class R_AreaAlarmService:IR_AreaAlarmService
    {
		private IR_AreaAlarmRepository _Repository;

		public R_AreaAlarmService(IR_AreaAlarmRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<R_AreaAlarmInfo> AddAreaAlarm(R_AreaAlarmAddRequest areaAlarmRequest)
        {
            var _areaAlarm = ObjectConverter.Copy<R_AreaAlarmInfo, R_AreaAlarmModel>(areaAlarmRequest.AreaAlarmInfo);
            var resultareaAlarm = _Repository.AddAreaAlarm(_areaAlarm);
            var areaAlarmresponse = new BasicResponse<R_AreaAlarmInfo>();
            areaAlarmresponse.Data = ObjectConverter.Copy<R_AreaAlarmModel, R_AreaAlarmInfo>(resultareaAlarm);
            return areaAlarmresponse;
        }
				public BasicResponse<R_AreaAlarmInfo> UpdateAreaAlarm(R_AreaAlarmUpdateRequest areaAlarmRequest)
        {
            var _areaAlarm = ObjectConverter.Copy<R_AreaAlarmInfo, R_AreaAlarmModel>(areaAlarmRequest.AreaAlarmInfo);
            _Repository.UpdateAreaAlarm(_areaAlarm);
            var areaAlarmresponse = new BasicResponse<R_AreaAlarmInfo>();
            areaAlarmresponse.Data = ObjectConverter.Copy<R_AreaAlarmModel, R_AreaAlarmInfo>(_areaAlarm);  
            return areaAlarmresponse;
        }
				public BasicResponse DeleteAreaAlarm(R_AreaAlarmDeleteRequest areaAlarmRequest)
        {
            _Repository.DeleteAreaAlarm(areaAlarmRequest.Id);
            var areaAlarmresponse = new BasicResponse();            
            return areaAlarmresponse;
        }
				public BasicResponse<List<R_AreaAlarmInfo>> GetAreaAlarmList(R_AreaAlarmGetListRequest areaAlarmRequest)
        {
            var areaAlarmresponse = new BasicResponse<List<R_AreaAlarmInfo>>();
            areaAlarmRequest.PagerInfo.PageIndex = areaAlarmRequest.PagerInfo.PageIndex - 1;
            if (areaAlarmRequest.PagerInfo.PageIndex < 0)
            {
                areaAlarmRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var areaAlarmModelLists = _Repository.GetAreaAlarmList(areaAlarmRequest.PagerInfo.PageIndex, areaAlarmRequest.PagerInfo.PageSize, out rowcount);
            var areaAlarmInfoLists = new List<R_AreaAlarmInfo>();
            foreach (var item in areaAlarmModelLists)
            {
                var AreaAlarmInfo = ObjectConverter.Copy<R_AreaAlarmModel, R_AreaAlarmInfo>(item);
                areaAlarmInfoLists.Add(AreaAlarmInfo);
            }
            areaAlarmresponse.Data = areaAlarmInfoLists;
            return areaAlarmresponse;
        }
				public BasicResponse<R_AreaAlarmInfo> GetAreaAlarmById(R_AreaAlarmGetRequest areaAlarmRequest)
        {
            var result = _Repository.GetAreaAlarmById(areaAlarmRequest.Id);
            var areaAlarmInfo = ObjectConverter.Copy<R_AreaAlarmModel, R_AreaAlarmInfo>(result);
            var areaAlarmresponse = new BasicResponse<R_AreaAlarmInfo>();
            areaAlarmresponse.Data = areaAlarmInfo;
            return areaAlarmresponse;
        }
	}
}


