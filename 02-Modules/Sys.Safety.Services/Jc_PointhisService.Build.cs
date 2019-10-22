using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Pointhis;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_PointhisService:IJc_PointhisService
    {
		private IJc_PointhisRepository _Repository;

		public Jc_PointhisService(IJc_PointhisRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_PointhisInfo> AddJc_Pointhis(Jc_PointhisAddRequest jc_Pointhisrequest)
        {
            var _jc_Pointhis = ObjectConverter.Copy<Jc_PointhisInfo, Jc_PointhisModel>(jc_Pointhisrequest.Jc_PointhisInfo);
            var resultjc_Pointhis = _Repository.AddJc_Pointhis(_jc_Pointhis);
            var jc_Pointhisresponse = new BasicResponse<Jc_PointhisInfo>();
            jc_Pointhisresponse.Data = ObjectConverter.Copy<Jc_PointhisModel, Jc_PointhisInfo>(resultjc_Pointhis);
            return jc_Pointhisresponse;
        }
				public BasicResponse<Jc_PointhisInfo> UpdateJc_Pointhis(Jc_PointhisUpdateRequest jc_Pointhisrequest)
        {
            var _jc_Pointhis = ObjectConverter.Copy<Jc_PointhisInfo, Jc_PointhisModel>(jc_Pointhisrequest.Jc_PointhisInfo);
            _Repository.UpdateJc_Pointhis(_jc_Pointhis);
            var jc_Pointhisresponse = new BasicResponse<Jc_PointhisInfo>();
            jc_Pointhisresponse.Data = ObjectConverter.Copy<Jc_PointhisModel, Jc_PointhisInfo>(_jc_Pointhis);  
            return jc_Pointhisresponse;
        }
				public BasicResponse DeleteJc_Pointhis(Jc_PointhisDeleteRequest jc_Pointhisrequest)
        {
            _Repository.DeleteJc_Pointhis(jc_Pointhisrequest.Id);
            var jc_Pointhisresponse = new BasicResponse();            
            return jc_Pointhisresponse;
        }
				public BasicResponse<List<Jc_PointhisInfo>> GetJc_PointhisList(Jc_PointhisGetListRequest jc_Pointhisrequest)
        {
            var jc_Pointhisresponse = new BasicResponse<List<Jc_PointhisInfo>>();
            jc_Pointhisrequest.PagerInfo.PageIndex = jc_Pointhisrequest.PagerInfo.PageIndex - 1;
            if (jc_Pointhisrequest.PagerInfo.PageIndex < 0)
            {
                jc_Pointhisrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_PointhisModelLists = _Repository.GetJc_PointhisList(jc_Pointhisrequest.PagerInfo.PageIndex, jc_Pointhisrequest.PagerInfo.PageSize, out rowcount);
            var jc_PointhisInfoLists = new List<Jc_PointhisInfo>();
            foreach (var item in jc_PointhisModelLists)
            {
                var Jc_PointhisInfo = ObjectConverter.Copy<Jc_PointhisModel, Jc_PointhisInfo>(item);
                jc_PointhisInfoLists.Add(Jc_PointhisInfo);
            }
            jc_Pointhisresponse.Data = jc_PointhisInfoLists;
            return jc_Pointhisresponse;
        }
				public BasicResponse<Jc_PointhisInfo> GetJc_PointhisById(Jc_PointhisGetRequest jc_Pointhisrequest)
        {
            var result = _Repository.GetJc_PointhisById(jc_Pointhisrequest.Id);
            var jc_PointhisInfo = ObjectConverter.Copy<Jc_PointhisModel, Jc_PointhisInfo>(result);
            var jc_Pointhisresponse = new BasicResponse<Jc_PointhisInfo>();
            jc_Pointhisresponse.Data = jc_PointhisInfo;
            return jc_Pointhisresponse;
        }
	}
}


