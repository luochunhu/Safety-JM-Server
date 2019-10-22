using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Model;
using Sys.Safety.Request.KJ_Addresstype;

namespace Sys.Safety.Services
{
    public partial class KJ_AddresstypeService:IKJ_AddresstypeService
    {
		private IKJ_AddresstypeRepository _Repository;

		public KJ_AddresstypeService(IKJ_AddresstypeRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<KJ_AddresstypeInfo> AddKJ_Addresstype(KJ_AddresstypeAddRequest kJ_AddresstypeRequest)
        {
            var _kJ_Addresstype = ObjectConverter.Copy<KJ_AddresstypeInfo, KJ_AddresstypeModel>(kJ_AddresstypeRequest.KJ_AddresstypeInfo);
            var resultkJ_Addresstype = _Repository.AddKJ_Addresstype(_kJ_Addresstype);
            var kJ_Addresstyperesponse = new BasicResponse<KJ_AddresstypeInfo>();
            kJ_Addresstyperesponse.Data = ObjectConverter.Copy<KJ_AddresstypeModel, KJ_AddresstypeInfo>(resultkJ_Addresstype);
            return kJ_Addresstyperesponse;
        }
				public BasicResponse<KJ_AddresstypeInfo> UpdateKJ_Addresstype(KJ_AddresstypeUpdateRequest kJ_AddresstypeRequest)
        {
            var _kJ_Addresstype = ObjectConverter.Copy<KJ_AddresstypeInfo, KJ_AddresstypeModel>(kJ_AddresstypeRequest.KJ_AddresstypeInfo);
            _Repository.UpdateKJ_Addresstype(_kJ_Addresstype);
            var kJ_Addresstyperesponse = new BasicResponse<KJ_AddresstypeInfo>();
            kJ_Addresstyperesponse.Data = ObjectConverter.Copy<KJ_AddresstypeModel, KJ_AddresstypeInfo>(_kJ_Addresstype);  
            return kJ_Addresstyperesponse;
        }
				public BasicResponse DeleteKJ_Addresstype(KJ_AddresstypeDeleteRequest kJ_AddresstypeRequest)
        {
            _Repository.DeleteKJ_Addresstype(kJ_AddresstypeRequest.Id);
            var kJ_Addresstyperesponse = new BasicResponse();            
            return kJ_Addresstyperesponse;
        }
				public BasicResponse<List<KJ_AddresstypeInfo>> GetKJ_AddresstypeList(KJ_AddresstypeGetListRequest kJ_AddresstypeRequest)
        {
            var kJ_Addresstyperesponse = new BasicResponse<List<KJ_AddresstypeInfo>>();
            kJ_AddresstypeRequest.PagerInfo.PageIndex = kJ_AddresstypeRequest.PagerInfo.PageIndex - 1;
            if (kJ_AddresstypeRequest.PagerInfo.PageIndex < 0)
            {
                kJ_AddresstypeRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var kJ_AddresstypeModelLists = _Repository.GetKJ_AddresstypeList(kJ_AddresstypeRequest.PagerInfo.PageIndex, kJ_AddresstypeRequest.PagerInfo.PageSize, out rowcount);
            var kJ_AddresstypeInfoLists = new List<KJ_AddresstypeInfo>();
            foreach (var item in kJ_AddresstypeModelLists)
            {
                var KJ_AddresstypeInfo = ObjectConverter.Copy<KJ_AddresstypeModel, KJ_AddresstypeInfo>(item);
                kJ_AddresstypeInfoLists.Add(KJ_AddresstypeInfo);
            }
            kJ_Addresstyperesponse.Data = kJ_AddresstypeInfoLists;
            return kJ_Addresstyperesponse;
        }
				public BasicResponse<KJ_AddresstypeInfo> GetKJ_AddresstypeById(KJ_AddresstypeGetRequest kJ_AddresstypeRequest)
        {
            var result = _Repository.GetKJ_AddresstypeById(kJ_AddresstypeRequest.Id);
            var kJ_AddresstypeInfo = ObjectConverter.Copy<KJ_AddresstypeModel, KJ_AddresstypeInfo>(result);
            var kJ_Addresstyperesponse = new BasicResponse<KJ_AddresstypeInfo>();
            kJ_Addresstyperesponse.Data = kJ_AddresstypeInfo;
            return kJ_Addresstyperesponse;
        }
	}
}


