using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Ll_M;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class EmissionMonthService:IEmissionMonthService
    {
		private IEmissionMonthRepository _Repository;

		public EmissionMonthService(IEmissionMonthRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_Ll_MInfo> AddEmissionMonth(Jc_Ll_MAddRequest jc_Ll_Mrequest)
        {
            var _jc_Ll_M = ObjectConverter.Copy<Jc_Ll_MInfo, Jc_Ll_MModel>(jc_Ll_Mrequest.Jc_Ll_MInfo);
            var resultjc_Ll_M = _Repository.AddEmissionMonth(_jc_Ll_M);
            var jc_Ll_Mresponse = new BasicResponse<Jc_Ll_MInfo>();
            jc_Ll_Mresponse.Data = ObjectConverter.Copy<Jc_Ll_MModel, Jc_Ll_MInfo>(resultjc_Ll_M);
            return jc_Ll_Mresponse;
        }
				public BasicResponse<Jc_Ll_MInfo> UpdateEmissionMonth(Jc_Ll_MUpdateRequest jc_Ll_Mrequest)
        {
            var _jc_Ll_M = ObjectConverter.Copy<Jc_Ll_MInfo, Jc_Ll_MModel>(jc_Ll_Mrequest.Jc_Ll_MInfo);
            _Repository.UpdateEmissionMonth(_jc_Ll_M);
            var jc_Ll_Mresponse = new BasicResponse<Jc_Ll_MInfo>();
            jc_Ll_Mresponse.Data = ObjectConverter.Copy<Jc_Ll_MModel, Jc_Ll_MInfo>(_jc_Ll_M);  
            return jc_Ll_Mresponse;
        }
				public BasicResponse DeleteEmissionMonth(Jc_Ll_MDeleteRequest jc_Ll_Mrequest)
        {
            _Repository.DeleteEmissionMonth(jc_Ll_Mrequest.Id);
            var jc_Ll_Mresponse = new BasicResponse();            
            return jc_Ll_Mresponse;
        }
				public BasicResponse<List<Jc_Ll_MInfo>> GetEmissionMonthList(Jc_Ll_MGetListRequest jc_Ll_Mrequest)
        {
            var jc_Ll_Mresponse = new BasicResponse<List<Jc_Ll_MInfo>>();
            jc_Ll_Mrequest.PagerInfo.PageIndex = jc_Ll_Mrequest.PagerInfo.PageIndex - 1;
            if (jc_Ll_Mrequest.PagerInfo.PageIndex < 0)
            {
                jc_Ll_Mrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_Ll_MModelLists = _Repository.GetEmissionMonthList(jc_Ll_Mrequest.PagerInfo.PageIndex, jc_Ll_Mrequest.PagerInfo.PageSize, out rowcount);
            var jc_Ll_MInfoLists = new List<Jc_Ll_MInfo>();
            foreach (var item in jc_Ll_MModelLists)
            {
                var Jc_Ll_MInfo = ObjectConverter.Copy<Jc_Ll_MModel, Jc_Ll_MInfo>(item);
                jc_Ll_MInfoLists.Add(Jc_Ll_MInfo);
            }
            jc_Ll_Mresponse.Data = jc_Ll_MInfoLists;
            return jc_Ll_Mresponse;
        }
				public BasicResponse<Jc_Ll_MInfo> GetEmissionMonthById(Jc_Ll_MGetRequest jc_Ll_Mrequest)
        {
            var result = _Repository.GetEmissionMonthById(jc_Ll_Mrequest.Id);
            var jc_Ll_MInfo = ObjectConverter.Copy<Jc_Ll_MModel, Jc_Ll_MInfo>(result);
            var jc_Ll_Mresponse = new BasicResponse<Jc_Ll_MInfo>();
            jc_Ll_Mresponse.Data = jc_Ll_MInfo;
            return jc_Ll_Mresponse;
        }
	}
}


