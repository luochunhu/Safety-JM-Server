using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Cs;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_CsService:IJc_CsService
    {
		private IJc_CsRepository _Repository;

		public Jc_CsService(IJc_CsRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_CsInfo> AddJc_Cs(Jc_CsAddRequest jc_Csrequest)
        {
            var _jc_Cs = ObjectConverter.Copy<Jc_CsInfo, Jc_CsModel>(jc_Csrequest.Jc_CsInfo);
            var resultjc_Cs = _Repository.AddJc_Cs(_jc_Cs);
            var jc_Csresponse = new BasicResponse<Jc_CsInfo>();
            jc_Csresponse.Data = ObjectConverter.Copy<Jc_CsModel, Jc_CsInfo>(resultjc_Cs);
            return jc_Csresponse;
        }
				public BasicResponse<Jc_CsInfo> UpdateJc_Cs(Jc_CsUpdateRequest jc_Csrequest)
        {
            var _jc_Cs = ObjectConverter.Copy<Jc_CsInfo, Jc_CsModel>(jc_Csrequest.Jc_CsInfo);
            _Repository.UpdateJc_Cs(_jc_Cs);
            var jc_Csresponse = new BasicResponse<Jc_CsInfo>();
            jc_Csresponse.Data = ObjectConverter.Copy<Jc_CsModel, Jc_CsInfo>(_jc_Cs);  
            return jc_Csresponse;
        }
				public BasicResponse DeleteJc_Cs(Jc_CsDeleteRequest jc_Csrequest)
        {
            _Repository.DeleteJc_Cs(jc_Csrequest.Id);
            var jc_Csresponse = new BasicResponse();            
            return jc_Csresponse;
        }
				public BasicResponse<List<Jc_CsInfo>> GetJc_CsList(Jc_CsGetListRequest jc_Csrequest)
        {
            var jc_Csresponse = new BasicResponse<List<Jc_CsInfo>>();
            jc_Csrequest.PagerInfo.PageIndex = jc_Csrequest.PagerInfo.PageIndex - 1;
            if (jc_Csrequest.PagerInfo.PageIndex < 0)
            {
                jc_Csrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_CsModelLists = _Repository.GetJc_CsList(jc_Csrequest.PagerInfo.PageIndex, jc_Csrequest.PagerInfo.PageSize, out rowcount);
            var jc_CsInfoLists = new List<Jc_CsInfo>();
            foreach (var item in jc_CsModelLists)
            {
                var Jc_CsInfo = ObjectConverter.Copy<Jc_CsModel, Jc_CsInfo>(item);
                jc_CsInfoLists.Add(Jc_CsInfo);
            }
            jc_Csresponse.Data = jc_CsInfoLists;
            return jc_Csresponse;
        }
				public BasicResponse<Jc_CsInfo> GetJc_CsById(Jc_CsGetRequest jc_Csrequest)
        {
            var result = _Repository.GetJc_CsById(jc_Csrequest.Id);
            var jc_CsInfo = ObjectConverter.Copy<Jc_CsModel, Jc_CsInfo>(result);
            var jc_Csresponse = new BasicResponse<Jc_CsInfo>();
            jc_Csresponse.Data = jc_CsInfo;
            return jc_Csresponse;
        }
	}
}


