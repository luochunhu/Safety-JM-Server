using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Model;
using Sys.Safety.Request.KJ_Addresstyperule;

namespace Sys.Safety.Services
{
    public partial class KJ_AddresstyperuleService : IKJ_AddresstyperuleService
    {
        private IKJ_AddresstyperuleRepository _Repository;

        public KJ_AddresstyperuleService(IKJ_AddresstyperuleRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<KJ_AddresstyperuleInfo> AddKJ_Addresstyperule(KJ_AddresstyperuleAddRequest kJ_AddresstyperuleRequest)
        {
            var _kJ_Addresstyperule = ObjectConverter.Copy<KJ_AddresstyperuleInfo, KJ_AddresstyperuleModel>(kJ_AddresstyperuleRequest.KJ_AddresstyperuleInfo);
            var resultkJ_Addresstyperule = _Repository.AddKJ_Addresstyperule(_kJ_Addresstyperule);
            var kJ_Addresstyperuleresponse = new BasicResponse<KJ_AddresstyperuleInfo>();
            kJ_Addresstyperuleresponse.Data = ObjectConverter.Copy<KJ_AddresstyperuleModel, KJ_AddresstyperuleInfo>(resultkJ_Addresstyperule);
            return kJ_Addresstyperuleresponse;
        }
        public BasicResponse<KJ_AddresstyperuleInfo> UpdateKJ_Addresstyperule(KJ_AddresstyperuleUpdateRequest kJ_AddresstyperuleRequest)
        {
            var _kJ_Addresstyperule = ObjectConverter.Copy<KJ_AddresstyperuleInfo, KJ_AddresstyperuleModel>(kJ_AddresstyperuleRequest.KJ_AddresstyperuleInfo);
            _Repository.UpdateKJ_Addresstyperule(_kJ_Addresstyperule);
            var kJ_Addresstyperuleresponse = new BasicResponse<KJ_AddresstyperuleInfo>();
            kJ_Addresstyperuleresponse.Data = ObjectConverter.Copy<KJ_AddresstyperuleModel, KJ_AddresstyperuleInfo>(_kJ_Addresstyperule);
            return kJ_Addresstyperuleresponse;
        }
        public BasicResponse DeleteKJ_Addresstyperule(KJ_AddresstyperuleDeleteRequest kJ_AddresstyperuleRequest)
        {
            _Repository.DeleteKJ_Addresstyperule(kJ_AddresstyperuleRequest.Id);
            var kJ_Addresstyperuleresponse = new BasicResponse();
            return kJ_Addresstyperuleresponse;
        }
        public BasicResponse DeleteKJ_AddresstyperuleByAddressTypeId(KJ_AddresstyperuleDeleteByAddressTypeIdRequest kJ_AddresstyperuleRequest)
        {
            _Repository.DeleteKJ_AddresstyperuleByAddressTypeId(kJ_AddresstyperuleRequest.AddressTypeId);
            var kJ_Addresstyperuleresponse = new BasicResponse();
            return kJ_Addresstyperuleresponse;
        }
        public BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleList(KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            var kJ_Addresstyperuleresponse = new BasicResponse<List<KJ_AddresstyperuleInfo>>();
            kJ_AddresstyperuleRequest.PagerInfo.PageIndex = kJ_AddresstyperuleRequest.PagerInfo.PageIndex - 1;
            if (kJ_AddresstyperuleRequest.PagerInfo.PageIndex < 0)
            {
                kJ_AddresstyperuleRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var kJ_AddresstyperuleModelLists = _Repository.GetKJ_AddresstyperuleList(kJ_AddresstyperuleRequest.PagerInfo.PageIndex, kJ_AddresstyperuleRequest.PagerInfo.PageSize, out rowcount);
            var kJ_AddresstyperuleInfoLists = new List<KJ_AddresstyperuleInfo>();
            foreach (var item in kJ_AddresstyperuleModelLists)
            {
                var KJ_AddresstyperuleInfo = ObjectConverter.Copy<KJ_AddresstyperuleModel, KJ_AddresstyperuleInfo>(item);
                kJ_AddresstyperuleInfoLists.Add(KJ_AddresstyperuleInfo);
            }
            kJ_Addresstyperuleresponse.Data = kJ_AddresstyperuleInfoLists;
            return kJ_Addresstyperuleresponse;
        }
        public BasicResponse<KJ_AddresstyperuleInfo> GetKJ_AddresstyperuleById(KJ_AddresstyperuleGetRequest kJ_AddresstyperuleRequest)
        {
            var result = _Repository.GetKJ_AddresstyperuleById(kJ_AddresstyperuleRequest.Id);
            var kJ_AddresstyperuleInfo = ObjectConverter.Copy<KJ_AddresstyperuleModel, KJ_AddresstyperuleInfo>(result);
            var kJ_Addresstyperuleresponse = new BasicResponse<KJ_AddresstyperuleInfo>();
            kJ_Addresstyperuleresponse.Data = kJ_AddresstyperuleInfo;
            return kJ_Addresstyperuleresponse;
        }

        public BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleListByAddressTypeId(KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            var kJ_Addresstyperuleresponse = new BasicResponse<List<KJ_AddresstyperuleInfo>>();

            var kJ_AddresstyperuleModelLists = _Repository.GetKJ_AddresstyperuleListByAddressTypeId(kJ_AddresstyperuleRequest.Id);
            var kJ_AddresstyperuleInfoLists = new List<KJ_AddresstyperuleInfo>();
            foreach (var item in kJ_AddresstyperuleModelLists)
            {
                var KJ_AddresstyperuleInfo = ObjectConverter.Copy<KJ_AddresstyperuleModel, KJ_AddresstyperuleInfo>(item);
                kJ_AddresstyperuleInfoLists.Add(KJ_AddresstyperuleInfo);
            }
            kJ_Addresstyperuleresponse.Data = kJ_AddresstyperuleInfoLists;
            return kJ_Addresstyperuleresponse;
        }
    }
}


