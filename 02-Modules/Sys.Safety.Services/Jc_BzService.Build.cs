using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Bz;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_BzService:IJc_BzService
    {
		private IJc_BzRepository _Repository;

		public Jc_BzService(IJc_BzRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_BzInfo> AddJc_Bz(Jc_BzAddRequest jc_Bzrequest)
        {
            var _jc_Bz = ObjectConverter.Copy<Jc_BzInfo, Jc_BzModel>(jc_Bzrequest.Jc_BzInfo);
            var resultjc_Bz = _Repository.AddJc_Bz(_jc_Bz);
            var jc_Bzresponse = new BasicResponse<Jc_BzInfo>();
            jc_Bzresponse.Data = ObjectConverter.Copy<Jc_BzModel, Jc_BzInfo>(resultjc_Bz);
            return jc_Bzresponse;
        }
				public BasicResponse<Jc_BzInfo> UpdateJc_Bz(Jc_BzUpdateRequest jc_Bzrequest)
        {
            var _jc_Bz = ObjectConverter.Copy<Jc_BzInfo, Jc_BzModel>(jc_Bzrequest.Jc_BzInfo);
            _Repository.UpdateJc_Bz(_jc_Bz);
            var jc_Bzresponse = new BasicResponse<Jc_BzInfo>();
            jc_Bzresponse.Data = ObjectConverter.Copy<Jc_BzModel, Jc_BzInfo>(_jc_Bz);  
            return jc_Bzresponse;
        }
				public BasicResponse DeleteJc_Bz(Jc_BzDeleteRequest jc_Bzrequest)
        {
            _Repository.DeleteJc_Bz(jc_Bzrequest.Id);
            var jc_Bzresponse = new BasicResponse();            
            return jc_Bzresponse;
        }
				public BasicResponse<List<Jc_BzInfo>> GetJc_BzList(Jc_BzGetListRequest jc_Bzrequest)
        {
            var jc_Bzresponse = new BasicResponse<List<Jc_BzInfo>>();
            jc_Bzrequest.PagerInfo.PageIndex = jc_Bzrequest.PagerInfo.PageIndex - 1;
            if (jc_Bzrequest.PagerInfo.PageIndex < 0)
            {
                jc_Bzrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_BzModelLists = _Repository.GetJc_BzList(jc_Bzrequest.PagerInfo.PageIndex, jc_Bzrequest.PagerInfo.PageSize, out rowcount);
            var jc_BzInfoLists = new List<Jc_BzInfo>();
            foreach (var item in jc_BzModelLists)
            {
                var Jc_BzInfo = ObjectConverter.Copy<Jc_BzModel, Jc_BzInfo>(item);
                jc_BzInfoLists.Add(Jc_BzInfo);
            }
            jc_Bzresponse.Data = jc_BzInfoLists;
            return jc_Bzresponse;
        }
				public BasicResponse<Jc_BzInfo> GetJc_BzById(Jc_BzGetRequest jc_Bzrequest)
        {
            var result = _Repository.GetJc_BzById(jc_Bzrequest.Id);
            var jc_BzInfo = ObjectConverter.Copy<Jc_BzModel, Jc_BzInfo>(result);
            var jc_Bzresponse = new BasicResponse<Jc_BzInfo>();
            jc_Bzresponse.Data = jc_BzInfo;
            return jc_Bzresponse;
        }
	}
}


