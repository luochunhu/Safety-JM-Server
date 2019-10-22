using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Mc;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_McService:IJc_McService
    {
		private IJc_McRepository _Repository;

		public Jc_McService(IJc_McRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_McInfo> AddJc_Mc(Jc_McAddRequest jc_Mcrequest)
        {
            var _jc_Mc = ObjectConverter.Copy<Jc_McInfo, Jc_McModel>(jc_Mcrequest.Jc_McInfo);
            var resultjc_Mc = _Repository.AddJc_Mc(_jc_Mc);
            var jc_Mcresponse = new BasicResponse<Jc_McInfo>();
            jc_Mcresponse.Data = ObjectConverter.Copy<Jc_McModel, Jc_McInfo>(resultjc_Mc);
            return jc_Mcresponse;
        }
				public BasicResponse<Jc_McInfo> UpdateJc_Mc(Jc_McUpdateRequest jc_Mcrequest)
        {
            var _jc_Mc = ObjectConverter.Copy<Jc_McInfo, Jc_McModel>(jc_Mcrequest.Jc_McInfo);
            _Repository.UpdateJc_Mc(_jc_Mc);
            var jc_Mcresponse = new BasicResponse<Jc_McInfo>();
            jc_Mcresponse.Data = ObjectConverter.Copy<Jc_McModel, Jc_McInfo>(_jc_Mc);  
            return jc_Mcresponse;
        }
				public BasicResponse DeleteJc_Mc(Jc_McDeleteRequest jc_Mcrequest)
        {
            _Repository.DeleteJc_Mc(jc_Mcrequest.Id);
            var jc_Mcresponse = new BasicResponse();            
            return jc_Mcresponse;
        }
				public BasicResponse<List<Jc_McInfo>> GetJc_McList(Jc_McGetListRequest jc_Mcrequest)
        {
            var jc_Mcresponse = new BasicResponse<List<Jc_McInfo>>();
            jc_Mcrequest.PagerInfo.PageIndex = jc_Mcrequest.PagerInfo.PageIndex - 1;
            if (jc_Mcrequest.PagerInfo.PageIndex < 0)
            {
                jc_Mcrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_McModelLists = _Repository.GetJc_McList(jc_Mcrequest.PagerInfo.PageIndex, jc_Mcrequest.PagerInfo.PageSize, out rowcount);
            var jc_McInfoLists = new List<Jc_McInfo>();
            foreach (var item in jc_McModelLists)
            {
                var Jc_McInfo = ObjectConverter.Copy<Jc_McModel, Jc_McInfo>(item);
                jc_McInfoLists.Add(Jc_McInfo);
            }
            jc_Mcresponse.Data = jc_McInfoLists;
            return jc_Mcresponse;
        }
				public BasicResponse<Jc_McInfo> GetJc_McById(Jc_McGetRequest jc_Mcrequest)
        {
            var result = _Repository.GetJc_McById(jc_Mcrequest.Id);
            var jc_McInfo = ObjectConverter.Copy<Jc_McModel, Jc_McInfo>(result);
            var jc_Mcresponse = new BasicResponse<Jc_McInfo>();
            jc_Mcresponse.Data = jc_McInfo;
            return jc_Mcresponse;
        }
	}
}


