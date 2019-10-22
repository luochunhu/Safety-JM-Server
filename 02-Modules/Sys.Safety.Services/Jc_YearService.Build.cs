using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Year;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_YearService:IJc_YearService
    {
		private IJc_YearRepository _Repository;

		public Jc_YearService(IJc_YearRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_YearInfo> AddJc_Year(Jc_YearAddRequest jc_Yearrequest)
        {
            var _jc_Year = ObjectConverter.Copy<Jc_YearInfo, Jc_YearModel>(jc_Yearrequest.Jc_YearInfo);
            var resultjc_Year = _Repository.AddJc_Year(_jc_Year);
            var jc_Yearresponse = new BasicResponse<Jc_YearInfo>();
            jc_Yearresponse.Data = ObjectConverter.Copy<Jc_YearModel, Jc_YearInfo>(resultjc_Year);
            return jc_Yearresponse;
        }
				public BasicResponse<Jc_YearInfo> UpdateJc_Year(Jc_YearUpdateRequest jc_Yearrequest)
        {
            var _jc_Year = ObjectConverter.Copy<Jc_YearInfo, Jc_YearModel>(jc_Yearrequest.Jc_YearInfo);
            _Repository.UpdateJc_Year(_jc_Year);
            var jc_Yearresponse = new BasicResponse<Jc_YearInfo>();
            jc_Yearresponse.Data = ObjectConverter.Copy<Jc_YearModel, Jc_YearInfo>(_jc_Year);  
            return jc_Yearresponse;
        }
				public BasicResponse DeleteJc_Year(Jc_YearDeleteRequest jc_Yearrequest)
        {
            _Repository.DeleteJc_Year(jc_Yearrequest.Id);
            var jc_Yearresponse = new BasicResponse();            
            return jc_Yearresponse;
        }
				public BasicResponse<List<Jc_YearInfo>> GetJc_YearList(Jc_YearGetListRequest jc_Yearrequest)
        {
            var jc_Yearresponse = new BasicResponse<List<Jc_YearInfo>>();
            jc_Yearrequest.PagerInfo.PageIndex = jc_Yearrequest.PagerInfo.PageIndex - 1;
            if (jc_Yearrequest.PagerInfo.PageIndex < 0)
            {
                jc_Yearrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_YearModelLists = _Repository.GetJc_YearList(jc_Yearrequest.PagerInfo.PageIndex, jc_Yearrequest.PagerInfo.PageSize, out rowcount);
            var jc_YearInfoLists = new List<Jc_YearInfo>();
            foreach (var item in jc_YearModelLists)
            {
                var Jc_YearInfo = ObjectConverter.Copy<Jc_YearModel, Jc_YearInfo>(item);
                jc_YearInfoLists.Add(Jc_YearInfo);
            }
            jc_Yearresponse.Data = jc_YearInfoLists;
            return jc_Yearresponse;
        }
				public BasicResponse<Jc_YearInfo> GetJc_YearById(Jc_YearGetRequest jc_Yearrequest)
        {
            var result = _Repository.GetJc_YearById(jc_Yearrequest.Id);
            var jc_YearInfo = ObjectConverter.Copy<Jc_YearModel, Jc_YearInfo>(result);
            var jc_Yearresponse = new BasicResponse<Jc_YearInfo>();
            jc_Yearresponse.Data = jc_YearInfo;
            return jc_Yearresponse;
        }
	}
}


