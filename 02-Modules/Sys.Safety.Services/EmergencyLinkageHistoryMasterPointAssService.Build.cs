using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkageHistoryMasterPointAss;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkageHistoryMasterPointAssService : IEmergencyLinkageHistoryMasterPointAssService
    {
        private readonly IEmergencyLinkageHistoryMasterPointAssRepository _Repository;

        public EmergencyLinkageHistoryMasterPointAssService(
            IEmergencyLinkageHistoryMasterPointAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> AddEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssAddRequest emergencyLinkageHistoryMasterPointAssRequest)
        {
            var _emergencyLinkageHistoryMasterPointAss =
                ObjectConverter
                    .Copy<EmergencyLinkageHistoryMasterPointAssInfo, EmergencyLinkageHistoryMasterPointAssModel>(
                        emergencyLinkageHistoryMasterPointAssRequest.EmergencyLinkageHistoryMasterPointAssInfo);
            var resultemergencyLinkageHistoryMasterPointAss =
                _Repository.AddEmergencyLinkageHistoryMasterPointAss(_emergencyLinkageHistoryMasterPointAss);
            var emergencyLinkageHistoryMasterPointAssresponse =
                new BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo>();
            emergencyLinkageHistoryMasterPointAssresponse.Data =
                ObjectConverter
                    .Copy<EmergencyLinkageHistoryMasterPointAssModel, EmergencyLinkageHistoryMasterPointAssInfo>(
                        resultemergencyLinkageHistoryMasterPointAss);
            return emergencyLinkageHistoryMasterPointAssresponse;
        }

        public BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> UpdateEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssUpdateRequest emergencyLinkageHistoryMasterPointAssRequest)
        {
            var _emergencyLinkageHistoryMasterPointAss =
                ObjectConverter
                    .Copy<EmergencyLinkageHistoryMasterPointAssInfo, EmergencyLinkageHistoryMasterPointAssModel>(
                        emergencyLinkageHistoryMasterPointAssRequest.EmergencyLinkageHistoryMasterPointAssInfo);
            _Repository.UpdateEmergencyLinkageHistoryMasterPointAss(_emergencyLinkageHistoryMasterPointAss);
            var emergencyLinkageHistoryMasterPointAssresponse =
                new BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo>();
            emergencyLinkageHistoryMasterPointAssresponse.Data =
                ObjectConverter
                    .Copy<EmergencyLinkageHistoryMasterPointAssModel, EmergencyLinkageHistoryMasterPointAssInfo>(
                        _emergencyLinkageHistoryMasterPointAss);
            return emergencyLinkageHistoryMasterPointAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkageHistoryMasterPointAss(
            EmergencyLinkageHistoryMasterPointAssDeleteRequest emergencyLinkageHistoryMasterPointAssRequest)
        {
            _Repository.DeleteEmergencyLinkageHistoryMasterPointAss(emergencyLinkageHistoryMasterPointAssRequest.Id);
            var emergencyLinkageHistoryMasterPointAssresponse = new BasicResponse();
            return emergencyLinkageHistoryMasterPointAssresponse;
        }

        public BasicResponse<List<EmergencyLinkageHistoryMasterPointAssInfo>>
            GetEmergencyLinkageHistoryMasterPointAssList(
                EmergencyLinkageHistoryMasterPointAssGetListRequest emergencyLinkageHistoryMasterPointAssRequest)
        {
            var emergencyLinkageHistoryMasterPointAssresponse =
                new BasicResponse<List<EmergencyLinkageHistoryMasterPointAssInfo>>();
            emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageIndex =
                emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkageHistoryMasterPointAssModelLists =
                _Repository.GetEmergencyLinkageHistoryMasterPointAssList(
                    emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageIndex,
                    emergencyLinkageHistoryMasterPointAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkageHistoryMasterPointAssInfoLists = new List<EmergencyLinkageHistoryMasterPointAssInfo>();
            foreach (var item in emergencyLinkageHistoryMasterPointAssModelLists)
            {
                var EmergencyLinkageHistoryMasterPointAssInfo = ObjectConverter
                    .Copy<EmergencyLinkageHistoryMasterPointAssModel, EmergencyLinkageHistoryMasterPointAssInfo>(item);
                emergencyLinkageHistoryMasterPointAssInfoLists.Add(EmergencyLinkageHistoryMasterPointAssInfo);
            }
            emergencyLinkageHistoryMasterPointAssresponse.Data = emergencyLinkageHistoryMasterPointAssInfoLists;
            return emergencyLinkageHistoryMasterPointAssresponse;
        }

        public BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo> GetEmergencyLinkageHistoryMasterPointAssById(
            EmergencyLinkageHistoryMasterPointAssGetRequest emergencyLinkageHistoryMasterPointAssRequest)
        {
            var result =
                _Repository.GetEmergencyLinkageHistoryMasterPointAssById(
                    emergencyLinkageHistoryMasterPointAssRequest.Id);
            var emergencyLinkageHistoryMasterPointAssInfo = ObjectConverter
                .Copy<EmergencyLinkageHistoryMasterPointAssModel, EmergencyLinkageHistoryMasterPointAssInfo>(result);
            var emergencyLinkageHistoryMasterPointAssresponse =
                new BasicResponse<EmergencyLinkageHistoryMasterPointAssInfo>();
            emergencyLinkageHistoryMasterPointAssresponse.Data = emergencyLinkageHistoryMasterPointAssInfo;
            return emergencyLinkageHistoryMasterPointAssresponse;
        }
    }
}