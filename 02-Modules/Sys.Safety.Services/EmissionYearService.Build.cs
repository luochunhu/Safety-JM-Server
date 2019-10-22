using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Ll_Y;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class EmissionYearService:IEmissionYearService
    {
		private IEmissionYearRepository _Repository;

		public EmissionYearService(IEmissionYearRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_Ll_YInfo> AddEmissionYear(Jc_Ll_YAddRequest jc_Ll_Yrequest)
        {
            var _jc_Ll_Y = ObjectConverter.Copy<Jc_Ll_YInfo, Jc_Ll_YModel>(jc_Ll_Yrequest.Jc_Ll_YInfo);
            var resultjc_Ll_Y = _Repository.AddEmissionYear(_jc_Ll_Y);
            var jc_Ll_Yresponse = new BasicResponse<Jc_Ll_YInfo>();
            jc_Ll_Yresponse.Data = ObjectConverter.Copy<Jc_Ll_YModel, Jc_Ll_YInfo>(resultjc_Ll_Y);
            return jc_Ll_Yresponse;
        }
				public BasicResponse<Jc_Ll_YInfo> UpdateEmissionYear(Jc_Ll_YUpdateRequest jc_Ll_Yrequest)
        {
            var _jc_Ll_Y = ObjectConverter.Copy<Jc_Ll_YInfo, Jc_Ll_YModel>(jc_Ll_Yrequest.Jc_Ll_YInfo);
            _Repository.UpdateEmissionYear(_jc_Ll_Y);
            var jc_Ll_Yresponse = new BasicResponse<Jc_Ll_YInfo>();
            jc_Ll_Yresponse.Data = ObjectConverter.Copy<Jc_Ll_YModel, Jc_Ll_YInfo>(_jc_Ll_Y);  
            return jc_Ll_Yresponse;
        }
				public BasicResponse DeleteEmissionYear(Jc_Ll_YDeleteRequest jc_Ll_Yrequest)
        {
            _Repository.DeleteEmissionYear(jc_Ll_Yrequest.Id);
            var jc_Ll_Yresponse = new BasicResponse();            
            return jc_Ll_Yresponse;
        }
				public BasicResponse<List<Jc_Ll_YInfo>> GetEmissionYearList(Jc_Ll_YGetListRequest jc_Ll_Yrequest)
        {
            var jc_Ll_Yresponse = new BasicResponse<List<Jc_Ll_YInfo>>();
            jc_Ll_Yrequest.PagerInfo.PageIndex = jc_Ll_Yrequest.PagerInfo.PageIndex - 1;
            if (jc_Ll_Yrequest.PagerInfo.PageIndex < 0)
            {
                jc_Ll_Yrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_Ll_YModelLists = _Repository.GetEmissionYearList(jc_Ll_Yrequest.PagerInfo.PageIndex, jc_Ll_Yrequest.PagerInfo.PageSize, out rowcount);
            var jc_Ll_YInfoLists = new List<Jc_Ll_YInfo>();
            foreach (var item in jc_Ll_YModelLists)
            {
                var Jc_Ll_YInfo = ObjectConverter.Copy<Jc_Ll_YModel, Jc_Ll_YInfo>(item);
                jc_Ll_YInfoLists.Add(Jc_Ll_YInfo);
            }
            jc_Ll_Yresponse.Data = jc_Ll_YInfoLists;
            return jc_Ll_Yresponse;
        }
				public BasicResponse<Jc_Ll_YInfo> GetEmissionYearById(Jc_Ll_YGetRequest jc_Ll_Yrequest)
        {
            var result = _Repository.GetEmissionYearById(jc_Ll_Yrequest.Id);
            var jc_Ll_YInfo = ObjectConverter.Copy<Jc_Ll_YModel, Jc_Ll_YInfo>(result);
            var jc_Ll_Yresponse = new BasicResponse<Jc_Ll_YInfo>();
            jc_Ll_Yresponse.Data = jc_Ll_YInfo;
            return jc_Ll_Yresponse;
        }
	}
}


