using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Ll;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_LlService:IJc_LlService
    {
		private IJc_LlRepository _Repository;

		public Jc_LlService(IJc_LlRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_LlInfo> AddJc_Ll(Jc_LlAddRequest jc_Llrequest)
        {
            var _jc_Ll = ObjectConverter.Copy<Jc_LlInfo, Jc_LlModel>(jc_Llrequest.Jc_LlInfo);
            var resultjc_Ll = _Repository.AddJc_Ll(_jc_Ll);
            var jc_Llresponse = new BasicResponse<Jc_LlInfo>();
            jc_Llresponse.Data = ObjectConverter.Copy<Jc_LlModel, Jc_LlInfo>(resultjc_Ll);
            return jc_Llresponse;
        }
				public BasicResponse<Jc_LlInfo> UpdateJc_Ll(Jc_LlUpdateRequest jc_Llrequest)
        {
            var _jc_Ll = ObjectConverter.Copy<Jc_LlInfo, Jc_LlModel>(jc_Llrequest.Jc_LlInfo);
            _Repository.UpdateJc_Ll(_jc_Ll);
            var jc_Llresponse = new BasicResponse<Jc_LlInfo>();
            jc_Llresponse.Data = ObjectConverter.Copy<Jc_LlModel, Jc_LlInfo>(_jc_Ll);  
            return jc_Llresponse;
        }
				public BasicResponse DeleteJc_Ll(Jc_LlDeleteRequest jc_Llrequest)
        {
            _Repository.DeleteJc_Ll(jc_Llrequest.Id);
            var jc_Llresponse = new BasicResponse();            
            return jc_Llresponse;
        }
				public BasicResponse<List<Jc_LlInfo>> GetJc_LlList(Jc_LlGetListRequest jc_Llrequest)
        {
            var jc_Llresponse = new BasicResponse<List<Jc_LlInfo>>();
            jc_Llrequest.PagerInfo.PageIndex = jc_Llrequest.PagerInfo.PageIndex - 1;
            if (jc_Llrequest.PagerInfo.PageIndex < 0)
            {
                jc_Llrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_LlModelLists = _Repository.GetJc_LlList(jc_Llrequest.PagerInfo.PageIndex, jc_Llrequest.PagerInfo.PageSize, out rowcount);
            var jc_LlInfoLists = new List<Jc_LlInfo>();
            foreach (var item in jc_LlModelLists)
            {
                var Jc_LlInfo = ObjectConverter.Copy<Jc_LlModel, Jc_LlInfo>(item);
                jc_LlInfoLists.Add(Jc_LlInfo);
            }
            jc_Llresponse.Data = jc_LlInfoLists;
            return jc_Llresponse;
        }
				public BasicResponse<Jc_LlInfo> GetJc_LlById(Jc_LlGetRequest jc_Llrequest)
        {
            var result = _Repository.GetJc_LlById(jc_Llrequest.Id);
            var jc_LlInfo = ObjectConverter.Copy<Jc_LlModel, Jc_LlInfo>(result);
            var jc_Llresponse = new BasicResponse<Jc_LlInfo>();
            jc_Llresponse.Data = jc_LlInfo;
            return jc_Llresponse;
        }
	}
}


