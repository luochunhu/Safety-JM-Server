using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Kd;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_KdService:IJc_KdService
    {
		private IJc_KdRepository _Repository;

		public Jc_KdService(IJc_KdRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_KdInfo> AddJc_Kd(Jc_KdAddRequest jc_Kdrequest)
        {
            var _jc_Kd = ObjectConverter.Copy<Jc_KdInfo, Jc_KdModel>(jc_Kdrequest.Jc_KdInfo);
            var resultjc_Kd = _Repository.AddJc_Kd(_jc_Kd);
            var jc_Kdresponse = new BasicResponse<Jc_KdInfo>();
            jc_Kdresponse.Data = ObjectConverter.Copy<Jc_KdModel, Jc_KdInfo>(resultjc_Kd);
            return jc_Kdresponse;
        }
				public BasicResponse<Jc_KdInfo> UpdateJc_Kd(Jc_KdUpdateRequest jc_Kdrequest)
        {
            var _jc_Kd = ObjectConverter.Copy<Jc_KdInfo, Jc_KdModel>(jc_Kdrequest.Jc_KdInfo);
            _Repository.UpdateJc_Kd(_jc_Kd);
            var jc_Kdresponse = new BasicResponse<Jc_KdInfo>();
            jc_Kdresponse.Data = ObjectConverter.Copy<Jc_KdModel, Jc_KdInfo>(_jc_Kd);  
            return jc_Kdresponse;
        }
				public BasicResponse DeleteJc_Kd(Jc_KdDeleteRequest jc_Kdrequest)
        {
            _Repository.DeleteJc_Kd(jc_Kdrequest.Id);
            var jc_Kdresponse = new BasicResponse();            
            return jc_Kdresponse;
        }
				public BasicResponse<List<Jc_KdInfo>> GetJc_KdList(Jc_KdGetListRequest jc_Kdrequest)
        {
            var jc_Kdresponse = new BasicResponse<List<Jc_KdInfo>>();
            jc_Kdrequest.PagerInfo.PageIndex = jc_Kdrequest.PagerInfo.PageIndex - 1;
            if (jc_Kdrequest.PagerInfo.PageIndex < 0)
            {
                jc_Kdrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_KdModelLists = _Repository.GetJc_KdList(jc_Kdrequest.PagerInfo.PageIndex, jc_Kdrequest.PagerInfo.PageSize, out rowcount);
            var jc_KdInfoLists = new List<Jc_KdInfo>();
            foreach (var item in jc_KdModelLists)
            {
                var Jc_KdInfo = ObjectConverter.Copy<Jc_KdModel, Jc_KdInfo>(item);
                jc_KdInfoLists.Add(Jc_KdInfo);
            }
            jc_Kdresponse.Data = jc_KdInfoLists;
            return jc_Kdresponse;
        }
				public BasicResponse<Jc_KdInfo> GetJc_KdById(Jc_KdGetRequest jc_Kdrequest)
        {
            var result = _Repository.GetJc_KdById(jc_Kdrequest.Id);
            var jc_KdInfo = ObjectConverter.Copy<Jc_KdModel, Jc_KdInfo>(result);
            var jc_Kdresponse = new BasicResponse<Jc_KdInfo>();
            jc_Kdresponse.Data = jc_KdInfo;
            return jc_Kdresponse;
        }
	}
}


