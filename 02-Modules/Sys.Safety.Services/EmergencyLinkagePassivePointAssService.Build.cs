using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkagePassivePointAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkagePassivePointAssService : IEmergencyLinkagePassivePointAssService
    {
        private readonly IEmergencyLinkagePassivePointAssRepository _Repository;

        public EmergencyLinkagePassivePointAssService(IEmergencyLinkagePassivePointAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkagePassivePointAssInfo> AddEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssAddRequest emergencyLinkagePassivePointAssRequest)
        {
            var _emergencyLinkagePassivePointAss =
                ObjectConverter.Copy<EmergencyLinkagePassivePointAssInfo, EmergencyLinkagePassivePointAssModel>(
                    emergencyLinkagePassivePointAssRequest.EmergencyLinkagePassivePointAssInfo);
            var resultemergencyLinkagePassivePointAss =
                _Repository.AddEmergencyLinkagePassivePointAss(_emergencyLinkagePassivePointAss);
            var emergencyLinkagePassivePointAssresponse = new BasicResponse<EmergencyLinkagePassivePointAssInfo>();
            emergencyLinkagePassivePointAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassivePointAssModel, EmergencyLinkagePassivePointAssInfo>(
                    resultemergencyLinkagePassivePointAss);
            return emergencyLinkagePassivePointAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassivePointAssInfo> UpdateEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssUpdateRequest emergencyLinkagePassivePointAssRequest)
        {
            var _emergencyLinkagePassivePointAss =
                ObjectConverter.Copy<EmergencyLinkagePassivePointAssInfo, EmergencyLinkagePassivePointAssModel>(
                    emergencyLinkagePassivePointAssRequest.EmergencyLinkagePassivePointAssInfo);
            _Repository.UpdateEmergencyLinkagePassivePointAss(_emergencyLinkagePassivePointAss);
            var emergencyLinkagePassivePointAssresponse = new BasicResponse<EmergencyLinkagePassivePointAssInfo>();
            emergencyLinkagePassivePointAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassivePointAssModel, EmergencyLinkagePassivePointAssInfo>(
                    _emergencyLinkagePassivePointAss);
            return emergencyLinkagePassivePointAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkagePassivePointAss(
            EmergencyLinkagePassivePointAssDeleteRequest emergencyLinkagePassivePointAssRequest)
        {
            _Repository.DeleteEmergencyLinkagePassivePointAss(emergencyLinkagePassivePointAssRequest.Id);
            var emergencyLinkagePassivePointAssresponse = new BasicResponse();
            return emergencyLinkagePassivePointAssresponse;
        }

        public BasicResponse<List<EmergencyLinkagePassivePointAssInfo>> GetEmergencyLinkagePassivePointAssList(
            EmergencyLinkagePassivePointAssGetListRequest emergencyLinkagePassivePointAssRequest)
        {
            var emergencyLinkagePassivePointAssresponse = new BasicResponse<List<EmergencyLinkagePassivePointAssInfo>>();
            emergencyLinkagePassivePointAssRequest.PagerInfo.PageIndex =
                emergencyLinkagePassivePointAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkagePassivePointAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkagePassivePointAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkagePassivePointAssModelLists = _Repository.GetEmergencyLinkagePassivePointAssList(
                emergencyLinkagePassivePointAssRequest.PagerInfo.PageIndex,
                emergencyLinkagePassivePointAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkagePassivePointAssInfoLists = new List<EmergencyLinkagePassivePointAssInfo>();
            foreach (var item in emergencyLinkagePassivePointAssModelLists)
            {
                var EmergencyLinkagePassivePointAssInfo = ObjectConverter
                    .Copy<EmergencyLinkagePassivePointAssModel, EmergencyLinkagePassivePointAssInfo>(item);
                emergencyLinkagePassivePointAssInfoLists.Add(EmergencyLinkagePassivePointAssInfo);
            }
            emergencyLinkagePassivePointAssresponse.Data = emergencyLinkagePassivePointAssInfoLists;
            return emergencyLinkagePassivePointAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassivePointAssInfo> GetEmergencyLinkagePassivePointAssById(
            EmergencyLinkagePassivePointAssGetRequest emergencyLinkagePassivePointAssRequest)
        {
            var result = _Repository.GetEmergencyLinkagePassivePointAssById(emergencyLinkagePassivePointAssRequest.Id);
            var emergencyLinkagePassivePointAssInfo =
                ObjectConverter.Copy<EmergencyLinkagePassivePointAssModel, EmergencyLinkagePassivePointAssInfo>(result);
            var emergencyLinkagePassivePointAssresponse = new BasicResponse<EmergencyLinkagePassivePointAssInfo>();
            emergencyLinkagePassivePointAssresponse.Data = emergencyLinkagePassivePointAssInfo;
            return emergencyLinkagePassivePointAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkagePassivePointAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkagePassivePointAssModel, EmergencyLinkagePassivePointAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkagePassivePointAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }

    }
}