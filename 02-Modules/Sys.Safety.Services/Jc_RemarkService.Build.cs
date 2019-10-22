using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Remark;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_RemarkService:IJc_RemarkService
    {
		private IJc_RemarkRepository _Repository;

		public Jc_RemarkService(IJc_RemarkRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_RemarkInfo> AddJc_Remark(Jc_RemarkAddRequest jc_Remarkrequest)
        {
            var _jc_Remark = ObjectConverter.Copy<Jc_RemarkInfo, Jc_RemarkModel>(jc_Remarkrequest.Jc_RemarkInfo);
            var resultjc_Remark = _Repository.AddJc_Remark(_jc_Remark);
            var jc_Remarkresponse = new BasicResponse<Jc_RemarkInfo>();
            jc_Remarkresponse.Data = ObjectConverter.Copy<Jc_RemarkModel, Jc_RemarkInfo>(resultjc_Remark);
            return jc_Remarkresponse;
        }
				public BasicResponse<Jc_RemarkInfo> UpdateJc_Remark(Jc_RemarkUpdateRequest jc_Remarkrequest)
        {
            var _jc_Remark = ObjectConverter.Copy<Jc_RemarkInfo, Jc_RemarkModel>(jc_Remarkrequest.Jc_RemarkInfo);
            _Repository.UpdateJc_Remark(_jc_Remark);
            var jc_Remarkresponse = new BasicResponse<Jc_RemarkInfo>();
            jc_Remarkresponse.Data = ObjectConverter.Copy<Jc_RemarkModel, Jc_RemarkInfo>(_jc_Remark);  
            return jc_Remarkresponse;
        }
				public BasicResponse DeleteJc_Remark(Jc_RemarkDeleteRequest jc_Remarkrequest)
        {
            _Repository.DeleteJc_Remark(jc_Remarkrequest.Id);
            var jc_Remarkresponse = new BasicResponse();            
            return jc_Remarkresponse;
        }
				public BasicResponse<List<Jc_RemarkInfo>> GetJc_RemarkList(Jc_RemarkGetListRequest jc_Remarkrequest)
        {
            var jc_Remarkresponse = new BasicResponse<List<Jc_RemarkInfo>>();
            jc_Remarkrequest.PagerInfo.PageIndex = jc_Remarkrequest.PagerInfo.PageIndex - 1;
            if (jc_Remarkrequest.PagerInfo.PageIndex < 0)
            {
                jc_Remarkrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_RemarkModelLists = _Repository.GetJc_RemarkList(jc_Remarkrequest.PagerInfo.PageIndex, jc_Remarkrequest.PagerInfo.PageSize, out rowcount);
            var jc_RemarkInfoLists = new List<Jc_RemarkInfo>();
            foreach (var item in jc_RemarkModelLists)
            {
                var Jc_RemarkInfo = ObjectConverter.Copy<Jc_RemarkModel, Jc_RemarkInfo>(item);
                jc_RemarkInfoLists.Add(Jc_RemarkInfo);
            }
            jc_Remarkresponse.Data = jc_RemarkInfoLists;
            return jc_Remarkresponse;
        }
				public BasicResponse<Jc_RemarkInfo> GetJc_RemarkById(Jc_RemarkGetRequest jc_Remarkrequest)
        {
            var result = _Repository.GetJc_RemarkById(jc_Remarkrequest.Id);
            var jc_RemarkInfo = ObjectConverter.Copy<Jc_RemarkModel, Jc_RemarkInfo>(result);
            var jc_Remarkresponse = new BasicResponse<Jc_RemarkInfo>();
            jc_Remarkresponse.Data = jc_RemarkInfo;
            return jc_Remarkresponse;
        }
	}
}


