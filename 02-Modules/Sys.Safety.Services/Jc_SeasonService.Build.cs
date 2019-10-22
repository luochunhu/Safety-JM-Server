using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Season;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_SeasonService:IJc_SeasonService
    {
		private IJc_SeasonRepository _Repository;

		public Jc_SeasonService(IJc_SeasonRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_SeasonInfo> AddJc_Season(Jc_SeasonAddRequest jc_Seasonrequest)
        {
            var _jc_Season = ObjectConverter.Copy<Jc_SeasonInfo, Jc_SeasonModel>(jc_Seasonrequest.Jc_SeasonInfo);
            var resultjc_Season = _Repository.AddJc_Season(_jc_Season);
            var jc_Seasonresponse = new BasicResponse<Jc_SeasonInfo>();
            jc_Seasonresponse.Data = ObjectConverter.Copy<Jc_SeasonModel, Jc_SeasonInfo>(resultjc_Season);
            return jc_Seasonresponse;
        }
				public BasicResponse<Jc_SeasonInfo> UpdateJc_Season(Jc_SeasonUpdateRequest jc_Seasonrequest)
        {
            var _jc_Season = ObjectConverter.Copy<Jc_SeasonInfo, Jc_SeasonModel>(jc_Seasonrequest.Jc_SeasonInfo);
            _Repository.UpdateJc_Season(_jc_Season);
            var jc_Seasonresponse = new BasicResponse<Jc_SeasonInfo>();
            jc_Seasonresponse.Data = ObjectConverter.Copy<Jc_SeasonModel, Jc_SeasonInfo>(_jc_Season);  
            return jc_Seasonresponse;
        }
				public BasicResponse DeleteJc_Season(Jc_SeasonDeleteRequest jc_Seasonrequest)
        {
            _Repository.DeleteJc_Season(jc_Seasonrequest.Id);
            var jc_Seasonresponse = new BasicResponse();            
            return jc_Seasonresponse;
        }
				public BasicResponse<List<Jc_SeasonInfo>> GetJc_SeasonList(Jc_SeasonGetListRequest jc_Seasonrequest)
        {
            var jc_Seasonresponse = new BasicResponse<List<Jc_SeasonInfo>>();
            jc_Seasonrequest.PagerInfo.PageIndex = jc_Seasonrequest.PagerInfo.PageIndex - 1;
            if (jc_Seasonrequest.PagerInfo.PageIndex < 0)
            {
                jc_Seasonrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_SeasonModelLists = _Repository.GetJc_SeasonList(jc_Seasonrequest.PagerInfo.PageIndex, jc_Seasonrequest.PagerInfo.PageSize, out rowcount);
            var jc_SeasonInfoLists = new List<Jc_SeasonInfo>();
            foreach (var item in jc_SeasonModelLists)
            {
                var Jc_SeasonInfo = ObjectConverter.Copy<Jc_SeasonModel, Jc_SeasonInfo>(item);
                jc_SeasonInfoLists.Add(Jc_SeasonInfo);
            }
            jc_Seasonresponse.Data = jc_SeasonInfoLists;
            return jc_Seasonresponse;
        }
				public BasicResponse<Jc_SeasonInfo> GetJc_SeasonById(Jc_SeasonGetRequest jc_Seasonrequest)
        {
            var result = _Repository.GetJc_SeasonById(jc_Seasonrequest.Id);
            var jc_SeasonInfo = ObjectConverter.Copy<Jc_SeasonModel, Jc_SeasonInfo>(result);
            var jc_Seasonresponse = new BasicResponse<Jc_SeasonInfo>();
            jc_Seasonresponse.Data = jc_SeasonInfo;
            return jc_Seasonresponse;
        }
	}
}


