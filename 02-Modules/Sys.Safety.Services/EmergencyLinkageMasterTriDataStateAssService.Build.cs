using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkageMasterTriDataStateAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkageMasterTriDataStateAssService : IEmergencyLinkageMasterTriDataStateAssService
    {
        private readonly IEmergencyLinkageMasterTriDataStateAssRepository _Repository;

        public EmergencyLinkageMasterTriDataStateAssService(
            IEmergencyLinkageMasterTriDataStateAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> AddEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssAddRequest emergencyLinkageMasterTriDataStateAssRequest)
        {
            var _emergencyLinkageMasterTriDataStateAss =
                ObjectConverter
                    .Copy<EmergencyLinkageMasterTriDataStateAssInfo, EmergencyLinkageMasterTriDataStateAssModel>(
                        emergencyLinkageMasterTriDataStateAssRequest.EmergencyLinkageMasterTriDataStateAssInfo);
            var resultemergencyLinkageMasterTriDataStateAss =
                _Repository.AddEmergencyLinkageMasterTriDataStateAss(_emergencyLinkageMasterTriDataStateAss);
            var emergencyLinkageMasterTriDataStateAssresponse =
                new BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo>();
            emergencyLinkageMasterTriDataStateAssresponse.Data =
                ObjectConverter
                    .Copy<EmergencyLinkageMasterTriDataStateAssModel, EmergencyLinkageMasterTriDataStateAssInfo>(
                        resultemergencyLinkageMasterTriDataStateAss);
            return emergencyLinkageMasterTriDataStateAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> UpdateEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssUpdateRequest emergencyLinkageMasterTriDataStateAssRequest)
        {
            var _emergencyLinkageMasterTriDataStateAss =
                ObjectConverter
                    .Copy<EmergencyLinkageMasterTriDataStateAssInfo, EmergencyLinkageMasterTriDataStateAssModel>(
                        emergencyLinkageMasterTriDataStateAssRequest.EmergencyLinkageMasterTriDataStateAssInfo);
            _Repository.UpdateEmergencyLinkageMasterTriDataStateAss(_emergencyLinkageMasterTriDataStateAss);
            var emergencyLinkageMasterTriDataStateAssresponse =
                new BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo>();
            emergencyLinkageMasterTriDataStateAssresponse.Data =
                ObjectConverter
                    .Copy<EmergencyLinkageMasterTriDataStateAssModel, EmergencyLinkageMasterTriDataStateAssInfo>(
                        _emergencyLinkageMasterTriDataStateAss);
            return emergencyLinkageMasterTriDataStateAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkageMasterTriDataStateAss(
            EmergencyLinkageMasterTriDataStateAssDeleteRequest emergencyLinkageMasterTriDataStateAssRequest)
        {
            _Repository.DeleteEmergencyLinkageMasterTriDataStateAss(emergencyLinkageMasterTriDataStateAssRequest.Id);
            var emergencyLinkageMasterTriDataStateAssresponse = new BasicResponse();
            return emergencyLinkageMasterTriDataStateAssresponse;
        }

        public BasicResponse<List<EmergencyLinkageMasterTriDataStateAssInfo>>
            GetEmergencyLinkageMasterTriDataStateAssList(
                EmergencyLinkageMasterTriDataStateAssGetListRequest emergencyLinkageMasterTriDataStateAssRequest)
        {
            var emergencyLinkageMasterTriDataStateAssresponse =
                new BasicResponse<List<EmergencyLinkageMasterTriDataStateAssInfo>>();
            emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageIndex =
                emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkageMasterTriDataStateAssModelLists =
                _Repository.GetEmergencyLinkageMasterTriDataStateAssList(
                    emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageIndex,
                    emergencyLinkageMasterTriDataStateAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkageMasterTriDataStateAssInfoLists = new List<EmergencyLinkageMasterTriDataStateAssInfo>();
            foreach (var item in emergencyLinkageMasterTriDataStateAssModelLists)
            {
                var EmergencyLinkageMasterTriDataStateAssInfo = ObjectConverter
                    .Copy<EmergencyLinkageMasterTriDataStateAssModel, EmergencyLinkageMasterTriDataStateAssInfo>(item);
                emergencyLinkageMasterTriDataStateAssInfoLists.Add(EmergencyLinkageMasterTriDataStateAssInfo);
            }
            emergencyLinkageMasterTriDataStateAssresponse.Data = emergencyLinkageMasterTriDataStateAssInfoLists;
            return emergencyLinkageMasterTriDataStateAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo> GetEmergencyLinkageMasterTriDataStateAssById(
            EmergencyLinkageMasterTriDataStateAssGetRequest emergencyLinkageMasterTriDataStateAssRequest)
        {
            var result =
                _Repository.GetEmergencyLinkageMasterTriDataStateAssById(
                    emergencyLinkageMasterTriDataStateAssRequest.Id);
            var emergencyLinkageMasterTriDataStateAssInfo = ObjectConverter
                .Copy<EmergencyLinkageMasterTriDataStateAssModel, EmergencyLinkageMasterTriDataStateAssInfo>(result);
            var emergencyLinkageMasterTriDataStateAssresponse =
                new BasicResponse<EmergencyLinkageMasterTriDataStateAssInfo>();
            emergencyLinkageMasterTriDataStateAssresponse.Data = emergencyLinkageMasterTriDataStateAssInfo;
            return emergencyLinkageMasterTriDataStateAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkageMasterTriDataStateAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkageMasterTriDataStateAssModel, EmergencyLinkageMasterTriDataStateAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkageMasterTriDataStateAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }

    }
}