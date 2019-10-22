using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_M;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_MService:IJc_MService
    {
		private IJc_MRepository _Repository;

		public Jc_MService(IJc_MRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_MInfo> AddJc_M(Jc_MAddRequest jc_Mrequest)
        {
            var _jc_M = ObjectConverter.Copy<Jc_MInfo, Jc_MModel>(jc_Mrequest.Jc_MInfo);
            var resultjc_M = _Repository.AddJc_M(_jc_M);
            var jc_Mresponse = new BasicResponse<Jc_MInfo>();
            jc_Mresponse.Data = ObjectConverter.Copy<Jc_MModel, Jc_MInfo>(resultjc_M);
            return jc_Mresponse;
        }
				public BasicResponse<Jc_MInfo> UpdateJc_M(Jc_MUpdateRequest jc_Mrequest)
        {
            var _jc_M = ObjectConverter.Copy<Jc_MInfo, Jc_MModel>(jc_Mrequest.Jc_MInfo);
            _Repository.UpdateJc_M(_jc_M);
            var jc_Mresponse = new BasicResponse<Jc_MInfo>();
            jc_Mresponse.Data = ObjectConverter.Copy<Jc_MModel, Jc_MInfo>(_jc_M);  
            return jc_Mresponse;
        }
				public BasicResponse DeleteJc_M(Jc_MDeleteRequest jc_Mrequest)
        {
            _Repository.DeleteJc_M(jc_Mrequest.Id);
            var jc_Mresponse = new BasicResponse();            
            return jc_Mresponse;
        }
				public BasicResponse<List<Jc_MInfo>> GetJc_MList(Jc_MGetListRequest jc_Mrequest)
        {
            var jc_Mresponse = new BasicResponse<List<Jc_MInfo>>();
            jc_Mrequest.PagerInfo.PageIndex = jc_Mrequest.PagerInfo.PageIndex - 1;
            if (jc_Mrequest.PagerInfo.PageIndex < 0)
            {
                jc_Mrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_MModelLists = _Repository.GetJc_MList(jc_Mrequest.PagerInfo.PageIndex, jc_Mrequest.PagerInfo.PageSize, out rowcount);
            var jc_MInfoLists = new List<Jc_MInfo>();
            foreach (var item in jc_MModelLists)
            {
                var Jc_MInfo = ObjectConverter.Copy<Jc_MModel, Jc_MInfo>(item);
                jc_MInfoLists.Add(Jc_MInfo);
            }
            jc_Mresponse.Data = jc_MInfoLists;
            return jc_Mresponse;
        }
				public BasicResponse<Jc_MInfo> GetJc_MById(Jc_MGetRequest jc_Mrequest)
        {
            var result = _Repository.GetJc_MById(jc_Mrequest.Id);
            var jc_MInfo = ObjectConverter.Copy<Jc_MModel, Jc_MInfo>(result);
            var jc_Mresponse = new BasicResponse<Jc_MInfo>();
            jc_Mresponse.Data = jc_MInfo;
            return jc_Mresponse;
        }
	}
}


