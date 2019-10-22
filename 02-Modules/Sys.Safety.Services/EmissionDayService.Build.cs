using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Ll_D;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class EmissionDayService:IEmissionDayService
    {
		private IEmissionDayRepository _Repository;

		public EmissionDayService(IEmissionDayRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_Ll_DInfo> AddEmissionDay(Jc_Ll_DAddRequest jc_Ll_Drequest)
        {
            var _jc_Ll_D = ObjectConverter.Copy<Jc_Ll_DInfo, Jc_Ll_DModel>(jc_Ll_Drequest.Jc_Ll_DInfo);
            var resultjc_Ll_D = _Repository.AddEmissionDay(_jc_Ll_D);
            var jc_Ll_Dresponse = new BasicResponse<Jc_Ll_DInfo>();
            jc_Ll_Dresponse.Data = ObjectConverter.Copy<Jc_Ll_DModel, Jc_Ll_DInfo>(resultjc_Ll_D);
            return jc_Ll_Dresponse;
        }
				public BasicResponse<Jc_Ll_DInfo> UpdateEmissionDay(Jc_Ll_DUpdateRequest jc_Ll_Drequest)
        {
            var _jc_Ll_D = ObjectConverter.Copy<Jc_Ll_DInfo, Jc_Ll_DModel>(jc_Ll_Drequest.Jc_Ll_DInfo);
            _Repository.UpdateEmissionDay(_jc_Ll_D);
            var jc_Ll_Dresponse = new BasicResponse<Jc_Ll_DInfo>();
            jc_Ll_Dresponse.Data = ObjectConverter.Copy<Jc_Ll_DModel, Jc_Ll_DInfo>(_jc_Ll_D);  
            return jc_Ll_Dresponse;
        }
				public BasicResponse DeleteEmissionDay(Jc_Ll_DDeleteRequest jc_Ll_Drequest)
        {
            _Repository.DeleteEmissionDay(jc_Ll_Drequest.Id);
            var jc_Ll_Dresponse = new BasicResponse();            
            return jc_Ll_Dresponse;
        }
				public BasicResponse<List<Jc_Ll_DInfo>> GetEmissionDayList(Jc_Ll_DGetListRequest jc_Ll_Drequest)
        {
            var jc_Ll_Dresponse = new BasicResponse<List<Jc_Ll_DInfo>>();
            jc_Ll_Drequest.PagerInfo.PageIndex = jc_Ll_Drequest.PagerInfo.PageIndex - 1;
            if (jc_Ll_Drequest.PagerInfo.PageIndex < 0)
            {
                jc_Ll_Drequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_Ll_DModelLists = _Repository.GetEmissionDayList(jc_Ll_Drequest.PagerInfo.PageIndex, jc_Ll_Drequest.PagerInfo.PageSize, out rowcount);
            var jc_Ll_DInfoLists = new List<Jc_Ll_DInfo>();
            foreach (var item in jc_Ll_DModelLists)
            {
                var Jc_Ll_DInfo = ObjectConverter.Copy<Jc_Ll_DModel, Jc_Ll_DInfo>(item);
                jc_Ll_DInfoLists.Add(Jc_Ll_DInfo);
            }
            jc_Ll_Dresponse.Data = jc_Ll_DInfoLists;
            return jc_Ll_Dresponse;
        }
				public BasicResponse<Jc_Ll_DInfo> GetEmissionDayById(Jc_Ll_DGetRequest jc_Ll_Drequest)
        {
            var result = _Repository.GetEmissionDayById(jc_Ll_Drequest.Id);
            var jc_Ll_DInfo = ObjectConverter.Copy<Jc_Ll_DModel, Jc_Ll_DInfo>(result);
            var jc_Ll_Dresponse = new BasicResponse<Jc_Ll_DInfo>();
            jc_Ll_Dresponse.Data = jc_Ll_DInfo;
            return jc_Ll_Dresponse;
        }
	}
}


