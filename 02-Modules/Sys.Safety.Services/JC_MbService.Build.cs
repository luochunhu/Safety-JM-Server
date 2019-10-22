using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Mb;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class JC_MbService:IJC_MbService
    {
		private IJC_MbRepository _Repository;

        public JC_MbService(IJC_MbRepository _Repository)
		{
			this._Repository = _Repository; 
		}
        public BasicResponse<JC_MbInfo> AddMb(JC_MbAddRequest mbrequest)
        {
            var _mb = ObjectConverter.Copy<JC_MbInfo, JC_MbModel>(mbrequest.MbInfo);
            var resultmb = _Repository.AddMb(_mb);
            var mbresponse = new BasicResponse<JC_MbInfo>();
            mbresponse.Data = ObjectConverter.Copy<JC_MbModel, JC_MbInfo>(resultmb);
            return mbresponse;
        }
        public BasicResponse<JC_MbInfo> UpdateMb(JC_MbUpdateRequest mbrequest)
        {
            var _mb = ObjectConverter.Copy<JC_MbInfo, JC_MbModel>(mbrequest.MbInfo);
            _Repository.UpdateMb(_mb);
            var mbresponse = new BasicResponse<JC_MbInfo>();
            mbresponse.Data = ObjectConverter.Copy<JC_MbModel, JC_MbInfo>(_mb);  
            return mbresponse;
        }
        public BasicResponse DeleteMb(JC_MbDeleteRequest mbrequest)
        {
            _Repository.DeleteMb(mbrequest.Id);
            var mbresponse = new BasicResponse();            
            return mbresponse;
        }
        public BasicResponse<List<JC_MbInfo>> GetMbList(JC_MbGetListRequest mbrequest)
        {
            var mbresponse = new BasicResponse<List<JC_MbInfo>>();
            mbrequest.PagerInfo.PageIndex = mbrequest.PagerInfo.PageIndex - 1;
            if (mbrequest.PagerInfo.PageIndex < 0)
            {
                mbrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var mbModelLists = _Repository.GetMbList(mbrequest.PagerInfo.PageIndex, mbrequest.PagerInfo.PageSize, out rowcount);
            var mbInfoLists = new List<JC_MbInfo>();
            foreach (var item in mbModelLists)
            {
                var MbInfo = ObjectConverter.Copy<JC_MbModel, JC_MbInfo>(item);
                mbInfoLists.Add(MbInfo);
            }
            mbresponse.Data = mbInfoLists;
            return mbresponse;
        }
        public BasicResponse<JC_MbInfo> GetMbById(JC_MbGetRequest mbrequest)
        {
            var result = _Repository.GetMbById(mbrequest.Id);
            var mbInfo = ObjectConverter.Copy<JC_MbModel, JC_MbInfo>(result);
            var mbresponse = new BasicResponse<JC_MbInfo>();
            mbresponse.Data = mbInfo;
            return mbresponse;
        }
	}
}


