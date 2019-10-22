using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkageMasterPointAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkageMasterPointAssService : IEmergencyLinkageMasterPointAssService
    {
        private readonly IEmergencyLinkageMasterPointAssRepository _Repository;

        public EmergencyLinkageMasterPointAssService(IEmergencyLinkageMasterPointAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkageMasterPointAssInfo> AddEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssAddRequest emergencyLinkageMasterPointAssRequest)
        {
            var _emergencyLinkageMasterPointAss =
                ObjectConverter.Copy<EmergencyLinkageMasterPointAssInfo, EmergencyLinkageMasterPointAssModel>(
                    emergencyLinkageMasterPointAssRequest.EmergencyLinkageMasterPointAssInfo);
            var resultemergencyLinkageMasterPointAss =
                _Repository.AddEmergencyLinkageMasterPointAss(_emergencyLinkageMasterPointAss);
            var emergencyLinkageMasterPointAssresponse = new BasicResponse<EmergencyLinkageMasterPointAssInfo>();
            emergencyLinkageMasterPointAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterPointAssModel, EmergencyLinkageMasterPointAssInfo>(
                    resultemergencyLinkageMasterPointAss);
            return emergencyLinkageMasterPointAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterPointAssInfo> UpdateEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssUpdateRequest emergencyLinkageMasterPointAssRequest)
        {
            var _emergencyLinkageMasterPointAss =
                ObjectConverter.Copy<EmergencyLinkageMasterPointAssInfo, EmergencyLinkageMasterPointAssModel>(
                    emergencyLinkageMasterPointAssRequest.EmergencyLinkageMasterPointAssInfo);
            _Repository.UpdateEmergencyLinkageMasterPointAss(_emergencyLinkageMasterPointAss);
            var emergencyLinkageMasterPointAssresponse = new BasicResponse<EmergencyLinkageMasterPointAssInfo>();
            emergencyLinkageMasterPointAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterPointAssModel, EmergencyLinkageMasterPointAssInfo>(
                    _emergencyLinkageMasterPointAss);
            return emergencyLinkageMasterPointAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkageMasterPointAss(
            EmergencyLinkageMasterPointAssDeleteRequest emergencyLinkageMasterPointAssRequest)
        {
            _Repository.DeleteEmergencyLinkageMasterPointAss(emergencyLinkageMasterPointAssRequest.Id);
            var emergencyLinkageMasterPointAssresponse = new BasicResponse();
            return emergencyLinkageMasterPointAssresponse;
        }

        public BasicResponse<List<EmergencyLinkageMasterPointAssInfo>> GetEmergencyLinkageMasterPointAssList(
            EmergencyLinkageMasterPointAssGetListRequest emergencyLinkageMasterPointAssRequest)
        {
            var emergencyLinkageMasterPointAssresponse = new BasicResponse<List<EmergencyLinkageMasterPointAssInfo>>();
            emergencyLinkageMasterPointAssRequest.PagerInfo.PageIndex =
                emergencyLinkageMasterPointAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkageMasterPointAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkageMasterPointAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkageMasterPointAssModelLists = _Repository.GetEmergencyLinkageMasterPointAssList(
                emergencyLinkageMasterPointAssRequest.PagerInfo.PageIndex,
                emergencyLinkageMasterPointAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkageMasterPointAssInfoLists = new List<EmergencyLinkageMasterPointAssInfo>();
            foreach (var item in emergencyLinkageMasterPointAssModelLists)
            {
                var EmergencyLinkageMasterPointAssInfo = ObjectConverter
                    .Copy<EmergencyLinkageMasterPointAssModel, EmergencyLinkageMasterPointAssInfo>(item);
                emergencyLinkageMasterPointAssInfoLists.Add(EmergencyLinkageMasterPointAssInfo);
            }
            emergencyLinkageMasterPointAssresponse.Data = emergencyLinkageMasterPointAssInfoLists;
            return emergencyLinkageMasterPointAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterPointAssInfo> GetEmergencyLinkageMasterPointAssById(
            EmergencyLinkageMasterPointAssGetRequest emergencyLinkageMasterPointAssRequest)
        {
            var result = _Repository.GetEmergencyLinkageMasterPointAssById(emergencyLinkageMasterPointAssRequest.Id);
            var emergencyLinkageMasterPointAssInfo =
                ObjectConverter.Copy<EmergencyLinkageMasterPointAssModel, EmergencyLinkageMasterPointAssInfo>(result);
            var emergencyLinkageMasterPointAssresponse = new BasicResponse<EmergencyLinkageMasterPointAssInfo>();
            emergencyLinkageMasterPointAssresponse.Data = emergencyLinkageMasterPointAssInfo;
            return emergencyLinkageMasterPointAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkageMasterPointAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkageMasterPointAssModel, EmergencyLinkageMasterPointAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkageMasterPointAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }

    }
}