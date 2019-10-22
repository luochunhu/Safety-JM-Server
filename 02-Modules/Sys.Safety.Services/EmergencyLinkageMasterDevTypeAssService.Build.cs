using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkageMasterDevTypeAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkageMasterDevTypeAssService : IEmergencyLinkageMasterDevTypeAssService
    {
        private readonly IEmergencyLinkageMasterDevTypeAssRepository _Repository;

        public EmergencyLinkageMasterDevTypeAssService(IEmergencyLinkageMasterDevTypeAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> AddEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssAddRequest emergencyLinkageMasterDevTypeAssRequest)
        {
            var _emergencyLinkageMasterDevTypeAss =
                ObjectConverter.Copy<EmergencyLinkageMasterDevTypeAssInfo, EmergencyLinkageMasterDevTypeAssModel>(
                    emergencyLinkageMasterDevTypeAssRequest.EmergencyLinkageMasterDevTypeAssInfo);
            var resultemergencyLinkageMasterDevTypeAss =
                _Repository.AddEmergencyLinkageMasterDevTypeAss(_emergencyLinkageMasterDevTypeAss);
            var emergencyLinkageMasterDevTypeAssresponse = new BasicResponse<EmergencyLinkageMasterDevTypeAssInfo>();
            emergencyLinkageMasterDevTypeAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterDevTypeAssModel, EmergencyLinkageMasterDevTypeAssInfo>(
                    resultemergencyLinkageMasterDevTypeAss);
            return emergencyLinkageMasterDevTypeAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> UpdateEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssUpdateRequest emergencyLinkageMasterDevTypeAssRequest)
        {
            var _emergencyLinkageMasterDevTypeAss =
                ObjectConverter.Copy<EmergencyLinkageMasterDevTypeAssInfo, EmergencyLinkageMasterDevTypeAssModel>(
                    emergencyLinkageMasterDevTypeAssRequest.EmergencyLinkageMasterDevTypeAssInfo);
            _Repository.UpdateEmergencyLinkageMasterDevTypeAss(_emergencyLinkageMasterDevTypeAss);
            var emergencyLinkageMasterDevTypeAssresponse = new BasicResponse<EmergencyLinkageMasterDevTypeAssInfo>();
            emergencyLinkageMasterDevTypeAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterDevTypeAssModel, EmergencyLinkageMasterDevTypeAssInfo>(
                    _emergencyLinkageMasterDevTypeAss);
            return emergencyLinkageMasterDevTypeAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkageMasterDevTypeAss(
            EmergencyLinkageMasterDevTypeAssDeleteRequest emergencyLinkageMasterDevTypeAssRequest)
        {
            _Repository.DeleteEmergencyLinkageMasterDevTypeAss(emergencyLinkageMasterDevTypeAssRequest.Id);
            var emergencyLinkageMasterDevTypeAssresponse = new BasicResponse();
            return emergencyLinkageMasterDevTypeAssresponse;
        }

        public BasicResponse<List<EmergencyLinkageMasterDevTypeAssInfo>> GetEmergencyLinkageMasterDevTypeAssList(
            EmergencyLinkageMasterDevTypeAssGetListRequest emergencyLinkageMasterDevTypeAssRequest)
        {
            var emergencyLinkageMasterDevTypeAssresponse =
                new BasicResponse<List<EmergencyLinkageMasterDevTypeAssInfo>>();
            emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageIndex =
                emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkageMasterDevTypeAssModelLists = _Repository.GetEmergencyLinkageMasterDevTypeAssList(
                emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageIndex,
                emergencyLinkageMasterDevTypeAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkageMasterDevTypeAssInfoLists = new List<EmergencyLinkageMasterDevTypeAssInfo>();
            foreach (var item in emergencyLinkageMasterDevTypeAssModelLists)
            {
                var EmergencyLinkageMasterDevTypeAssInfo = ObjectConverter
                    .Copy<EmergencyLinkageMasterDevTypeAssModel, EmergencyLinkageMasterDevTypeAssInfo>(item);
                emergencyLinkageMasterDevTypeAssInfoLists.Add(EmergencyLinkageMasterDevTypeAssInfo);
            }
            emergencyLinkageMasterDevTypeAssresponse.Data = emergencyLinkageMasterDevTypeAssInfoLists;
            return emergencyLinkageMasterDevTypeAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterDevTypeAssInfo> GetEmergencyLinkageMasterDevTypeAssById(
            EmergencyLinkageMasterDevTypeAssGetRequest emergencyLinkageMasterDevTypeAssRequest)
        {
            var result =
                _Repository.GetEmergencyLinkageMasterDevTypeAssById(emergencyLinkageMasterDevTypeAssRequest.Id);
            var emergencyLinkageMasterDevTypeAssInfo = ObjectConverter
                .Copy<EmergencyLinkageMasterDevTypeAssModel, EmergencyLinkageMasterDevTypeAssInfo>(result);
            var emergencyLinkageMasterDevTypeAssresponse = new BasicResponse<EmergencyLinkageMasterDevTypeAssInfo>();
            emergencyLinkageMasterDevTypeAssresponse.Data = emergencyLinkageMasterDevTypeAssInfo;
            return emergencyLinkageMasterDevTypeAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkageMasterDevTypeAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkageMasterDevTypeAssModel, EmergencyLinkageMasterDevTypeAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkageMasterDevTypeAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }
    }
}