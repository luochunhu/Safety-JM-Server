using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Multiplesetting;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class JC_MultiplesettingService : IJC_MultiplesettingService
    {
        private IJC_MultiplesettingRepository _Repository;

        public JC_MultiplesettingService(IJC_MultiplesettingRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_MultiplesettingInfo> AddMultiplesetting(JC_MultiplesettingAddRequest multiplesettingrequest)
        {
            var _multiplesetting = ObjectConverter.Copy<JC_MultiplesettingInfo, JC_MultiplesettingModel>(multiplesettingrequest.MultiplesettingInfo);
            var resultmultiplesetting = _Repository.AddMultiplesetting(_multiplesetting);
            var multiplesettingresponse = new BasicResponse<JC_MultiplesettingInfo>();
            multiplesettingresponse.Data = ObjectConverter.Copy<JC_MultiplesettingModel, JC_MultiplesettingInfo>(resultmultiplesetting);
            return multiplesettingresponse;
        }
        public BasicResponse<JC_MultiplesettingInfo> UpdateMultiplesetting(JC_MultiplesettingUpdateRequest multiplesettingrequest)
        {
            var _multiplesetting = ObjectConverter.Copy<JC_MultiplesettingInfo, JC_MultiplesettingModel>(multiplesettingrequest.MultiplesettingInfo);
            _Repository.UpdateMultiplesetting(_multiplesetting);
            var multiplesettingresponse = new BasicResponse<JC_MultiplesettingInfo>();
            multiplesettingresponse.Data = ObjectConverter.Copy<JC_MultiplesettingModel, JC_MultiplesettingInfo>(_multiplesetting);
            return multiplesettingresponse;
        }
        public BasicResponse DeleteMultiplesetting(JC_MultiplesettingDeleteRequest multiplesettingrequest)
        {
            _Repository.DeleteMultiplesetting(multiplesettingrequest.Id);
            var multiplesettingresponse = new BasicResponse();
            return multiplesettingresponse;
        }
        public BasicResponse<List<JC_MultiplesettingInfo>> GetMultiplesettingList(JC_MultiplesettingGetListRequest multiplesettingrequest)
        {
            var multiplesettingresponse = new BasicResponse<List<JC_MultiplesettingInfo>>();
            multiplesettingrequest.PagerInfo.PageIndex = multiplesettingrequest.PagerInfo.PageIndex - 1;
            if (multiplesettingrequest.PagerInfo.PageIndex < 0)
            {
                multiplesettingrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var multiplesettingModelLists = _Repository.GetMultiplesettingList(multiplesettingrequest.PagerInfo.PageIndex, multiplesettingrequest.PagerInfo.PageSize, out rowcount);
            var multiplesettingInfoLists = new List<JC_MultiplesettingInfo>();
            foreach (var item in multiplesettingModelLists)
            {
                var MultiplesettingInfo = ObjectConverter.Copy<JC_MultiplesettingModel, JC_MultiplesettingInfo>(item);
                multiplesettingInfoLists.Add(MultiplesettingInfo);
            }
            multiplesettingresponse.Data = multiplesettingInfoLists;
            return multiplesettingresponse;
        }
        public BasicResponse<List<JC_MultiplesettingInfo>> GetAllMultiplesettingList()
        {
            var multiplesettingresponse = new BasicResponse<List<JC_MultiplesettingInfo>>();
            var multiplesettingModelLists = _Repository.GetAllMultiplesettingList();
            var multiplesettingInfoLists = new List<JC_MultiplesettingInfo>();
            foreach (var item in multiplesettingModelLists)
            {
                var MultiplesettingInfo = ObjectConverter.Copy<JC_MultiplesettingModel, JC_MultiplesettingInfo>(item);
                multiplesettingInfoLists.Add(MultiplesettingInfo);
            }
            multiplesettingresponse.Data = multiplesettingInfoLists;
            return multiplesettingresponse;
        }
        public BasicResponse<JC_MultiplesettingInfo> GetMultiplesettingById(JC_MultiplesettingGetRequest multiplesettingrequest)
        {
            var result = _Repository.GetMultiplesettingById(multiplesettingrequest.Id);
            var multiplesettingInfo = ObjectConverter.Copy<JC_MultiplesettingModel, JC_MultiplesettingInfo>(result);
            var multiplesettingresponse = new BasicResponse<JC_MultiplesettingInfo>();
            multiplesettingresponse.Data = multiplesettingInfo;
            return multiplesettingresponse;
        }
    }
}


