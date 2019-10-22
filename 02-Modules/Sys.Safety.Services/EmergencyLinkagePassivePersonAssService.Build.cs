using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkagePassivePersonAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkagePassivePersonAssService : IEmergencyLinkagePassivePersonAssService
    {
        private readonly IEmergencyLinkagePassivePersonAssRepository _Repository;

        public EmergencyLinkagePassivePersonAssService(IEmergencyLinkagePassivePersonAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkagePassivePersonAssInfo> AddEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssAddRequest emergencyLinkagePassivePersonAssRequest)
        {
            var _emergencyLinkagePassivePersonAss =
                ObjectConverter.Copy<EmergencyLinkagePassivePersonAssInfo, EmergencyLinkagePassivePersonAssModel>(
                    emergencyLinkagePassivePersonAssRequest.EmergencyLinkagePassivePersonAssInfo);
            var resultemergencyLinkagePassivePersonAss =
                _Repository.AddEmergencyLinkagePassivePersonAss(_emergencyLinkagePassivePersonAss);
            var emergencyLinkagePassivePersonAssresponse = new BasicResponse<EmergencyLinkagePassivePersonAssInfo>();
            emergencyLinkagePassivePersonAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassivePersonAssModel, EmergencyLinkagePassivePersonAssInfo>(
                    resultemergencyLinkagePassivePersonAss);
            return emergencyLinkagePassivePersonAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassivePersonAssInfo> UpdateEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssUpdateRequest emergencyLinkagePassivePersonAssRequest)
        {
            var _emergencyLinkagePassivePersonAss =
                ObjectConverter.Copy<EmergencyLinkagePassivePersonAssInfo, EmergencyLinkagePassivePersonAssModel>(
                    emergencyLinkagePassivePersonAssRequest.EmergencyLinkagePassivePersonAssInfo);
            _Repository.UpdateEmergencyLinkagePassivePersonAss(_emergencyLinkagePassivePersonAss);
            var emergencyLinkagePassivePersonAssresponse = new BasicResponse<EmergencyLinkagePassivePersonAssInfo>();
            emergencyLinkagePassivePersonAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassivePersonAssModel, EmergencyLinkagePassivePersonAssInfo>(
                    _emergencyLinkagePassivePersonAss);
            return emergencyLinkagePassivePersonAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkagePassivePersonAss(
            EmergencyLinkagePassivePersonAssDeleteRequest emergencyLinkagePassivePersonAssRequest)
        {
            _Repository.DeleteEmergencyLinkagePassivePersonAss(emergencyLinkagePassivePersonAssRequest.Id);
            var emergencyLinkagePassivePersonAssresponse = new BasicResponse();
            return emergencyLinkagePassivePersonAssresponse;
        }

        public BasicResponse<List<EmergencyLinkagePassivePersonAssInfo>> GetEmergencyLinkagePassivePersonAssList(
            EmergencyLinkagePassivePersonAssGetListRequest emergencyLinkagePassivePersonAssRequest)
        {
            var emergencyLinkagePassivePersonAssresponse =
                new BasicResponse<List<EmergencyLinkagePassivePersonAssInfo>>();
            emergencyLinkagePassivePersonAssRequest.PagerInfo.PageIndex =
                emergencyLinkagePassivePersonAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkagePassivePersonAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkagePassivePersonAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkagePassivePersonAssModelLists = _Repository.GetEmergencyLinkagePassivePersonAssList(
                emergencyLinkagePassivePersonAssRequest.PagerInfo.PageIndex,
                emergencyLinkagePassivePersonAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkagePassivePersonAssInfoLists = new List<EmergencyLinkagePassivePersonAssInfo>();
            foreach (var item in emergencyLinkagePassivePersonAssModelLists)
            {
                var EmergencyLinkagePassivePersonAssInfo = ObjectConverter
                    .Copy<EmergencyLinkagePassivePersonAssModel, EmergencyLinkagePassivePersonAssInfo>(item);
                emergencyLinkagePassivePersonAssInfoLists.Add(EmergencyLinkagePassivePersonAssInfo);
            }
            emergencyLinkagePassivePersonAssresponse.Data = emergencyLinkagePassivePersonAssInfoLists;
            return emergencyLinkagePassivePersonAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassivePersonAssInfo> GetEmergencyLinkagePassivePersonAssById(
            EmergencyLinkagePassivePersonAssGetRequest emergencyLinkagePassivePersonAssRequest)
        {
            var result =
                _Repository.GetEmergencyLinkagePassivePersonAssById(emergencyLinkagePassivePersonAssRequest.Id);
            var emergencyLinkagePassivePersonAssInfo = ObjectConverter
                .Copy<EmergencyLinkagePassivePersonAssModel, EmergencyLinkagePassivePersonAssInfo>(result);
            var emergencyLinkagePassivePersonAssresponse = new BasicResponse<EmergencyLinkagePassivePersonAssInfo>();
            emergencyLinkagePassivePersonAssresponse.Data = emergencyLinkagePassivePersonAssInfo;
            return emergencyLinkagePassivePersonAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkagePassivePersonAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkagePassivePersonAssModel, EmergencyLinkagePassivePersonAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkagePassivePersonAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }

    }
}