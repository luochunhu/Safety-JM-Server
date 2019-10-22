using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Month;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_MonthService:IJc_MonthService
    {
		private IJc_MonthRepository _Repository;

		public Jc_MonthService(IJc_MonthRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_MonthInfo> AddJc_Month(Jc_MonthAddRequest jc_Monthrequest)
        {
            var _jc_Month = ObjectConverter.Copy<Jc_MonthInfo, Jc_MonthModel>(jc_Monthrequest.Jc_MonthInfo);
            var resultjc_Month = _Repository.AddJc_Month(_jc_Month);
            var jc_Monthresponse = new BasicResponse<Jc_MonthInfo>();
            jc_Monthresponse.Data = ObjectConverter.Copy<Jc_MonthModel, Jc_MonthInfo>(resultjc_Month);
            return jc_Monthresponse;
        }
				public BasicResponse<Jc_MonthInfo> UpdateJc_Month(Jc_MonthUpdateRequest jc_Monthrequest)
        {
            var _jc_Month = ObjectConverter.Copy<Jc_MonthInfo, Jc_MonthModel>(jc_Monthrequest.Jc_MonthInfo);
            _Repository.UpdateJc_Month(_jc_Month);
            var jc_Monthresponse = new BasicResponse<Jc_MonthInfo>();
            jc_Monthresponse.Data = ObjectConverter.Copy<Jc_MonthModel, Jc_MonthInfo>(_jc_Month);  
            return jc_Monthresponse;
        }
				public BasicResponse DeleteJc_Month(Jc_MonthDeleteRequest jc_Monthrequest)
        {
            _Repository.DeleteJc_Month(jc_Monthrequest.Id);
            var jc_Monthresponse = new BasicResponse();            
            return jc_Monthresponse;
        }
				public BasicResponse<List<Jc_MonthInfo>> GetJc_MonthList(Jc_MonthGetListRequest jc_Monthrequest)
        {
            var jc_Monthresponse = new BasicResponse<List<Jc_MonthInfo>>();
            jc_Monthrequest.PagerInfo.PageIndex = jc_Monthrequest.PagerInfo.PageIndex - 1;
            if (jc_Monthrequest.PagerInfo.PageIndex < 0)
            {
                jc_Monthrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_MonthModelLists = _Repository.GetJc_MonthList(jc_Monthrequest.PagerInfo.PageIndex, jc_Monthrequest.PagerInfo.PageSize, out rowcount);
            var jc_MonthInfoLists = new List<Jc_MonthInfo>();
            foreach (var item in jc_MonthModelLists)
            {
                var Jc_MonthInfo = ObjectConverter.Copy<Jc_MonthModel, Jc_MonthInfo>(item);
                jc_MonthInfoLists.Add(Jc_MonthInfo);
            }
            jc_Monthresponse.Data = jc_MonthInfoLists;
            return jc_Monthresponse;
        }
				public BasicResponse<Jc_MonthInfo> GetJc_MonthById(Jc_MonthGetRequest jc_Monthrequest)
        {
            var result = _Repository.GetJc_MonthById(jc_Monthrequest.Id);
            var jc_MonthInfo = ObjectConverter.Copy<Jc_MonthModel, Jc_MonthInfo>(result);
            var jc_Monthresponse = new BasicResponse<Jc_MonthInfo>();
            jc_Monthresponse.Data = jc_MonthInfo;
            return jc_Monthresponse;
        }
	}
}


