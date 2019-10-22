using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Ll_H;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class EmissionHourService:IEmissionHourService
    {
		private IEmissionHourRepository _Repository;

		public EmissionHourService(IEmissionHourRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_Ll_HInfo> AddEmissionHour(Jc_Ll_HAddRequest jc_Ll_Hrequest)
        {
            var _jc_Ll_H = ObjectConverter.Copy<Jc_Ll_HInfo, Jc_Ll_HModel>(jc_Ll_Hrequest.Jc_Ll_HInfo);
            var resultjc_Ll_H = _Repository.AddEmissionHour(_jc_Ll_H);
            var jc_Ll_Hresponse = new BasicResponse<Jc_Ll_HInfo>();
            jc_Ll_Hresponse.Data = ObjectConverter.Copy<Jc_Ll_HModel, Jc_Ll_HInfo>(resultjc_Ll_H);
            return jc_Ll_Hresponse;
        }
				public BasicResponse<Jc_Ll_HInfo> UpdateEmissionHour(Jc_Ll_HUpdateRequest jc_Ll_Hrequest)
        {
            var _jc_Ll_H = ObjectConverter.Copy<Jc_Ll_HInfo, Jc_Ll_HModel>(jc_Ll_Hrequest.Jc_Ll_HInfo);
            _Repository.UpdateEmissionHour(_jc_Ll_H);
            var jc_Ll_Hresponse = new BasicResponse<Jc_Ll_HInfo>();
            jc_Ll_Hresponse.Data = ObjectConverter.Copy<Jc_Ll_HModel, Jc_Ll_HInfo>(_jc_Ll_H);  
            return jc_Ll_Hresponse;
        }
				public BasicResponse DeleteEmissionHour(Jc_Ll_HDeleteRequest jc_Ll_Hrequest)
        {
            _Repository.DeleteEmissionHour(jc_Ll_Hrequest.Id);
            var jc_Ll_Hresponse = new BasicResponse();            
            return jc_Ll_Hresponse;
        }
				public BasicResponse<List<Jc_Ll_HInfo>> GetEmissionHourList(Jc_Ll_HGetListRequest jc_Ll_Hrequest)
        {
            var jc_Ll_Hresponse = new BasicResponse<List<Jc_Ll_HInfo>>();
            jc_Ll_Hrequest.PagerInfo.PageIndex = jc_Ll_Hrequest.PagerInfo.PageIndex - 1;
            if (jc_Ll_Hrequest.PagerInfo.PageIndex < 0)
            {
                jc_Ll_Hrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_Ll_HModelLists = _Repository.GetEmissionHourList(jc_Ll_Hrequest.PagerInfo.PageIndex, jc_Ll_Hrequest.PagerInfo.PageSize, out rowcount);
            var jc_Ll_HInfoLists = new List<Jc_Ll_HInfo>();
            foreach (var item in jc_Ll_HModelLists)
            {
                var Jc_Ll_HInfo = ObjectConverter.Copy<Jc_Ll_HModel, Jc_Ll_HInfo>(item);
                jc_Ll_HInfoLists.Add(Jc_Ll_HInfo);
            }
            jc_Ll_Hresponse.Data = jc_Ll_HInfoLists;
            return jc_Ll_Hresponse;
        }
				public BasicResponse<Jc_Ll_HInfo> GetEmissionHourById(Jc_Ll_HGetRequest jc_Ll_Hrequest)
        {
            var result = _Repository.GetEmissionHourById(jc_Ll_Hrequest.Id);
            var jc_Ll_HInfo = ObjectConverter.Copy<Jc_Ll_HModel, Jc_Ll_HInfo>(result);
            var jc_Ll_Hresponse = new BasicResponse<Jc_Ll_HInfo>();
            jc_Ll_Hresponse.Data = jc_Ll_HInfo;
            return jc_Ll_Hresponse;
        }
	}
}


