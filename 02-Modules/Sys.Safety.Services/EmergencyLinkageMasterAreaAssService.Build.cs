using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkageMasterAreaAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkageMasterAreaAssService : IEmergencyLinkageMasterAreaAssService
    {
        private readonly IEmergencyLinkageMasterAreaAssRepository _Repository;

        public EmergencyLinkageMasterAreaAssService(IEmergencyLinkageMasterAreaAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkageMasterAreaAssInfo> AddEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssAddRequest emergencyLinkageMasterAreaAssRequest)
        {
            var _emergencyLinkageMasterAreaAss =
                ObjectConverter.Copy<EmergencyLinkageMasterAreaAssInfo, EmergencyLinkageMasterAreaAssModel>(
                    emergencyLinkageMasterAreaAssRequest.EmergencyLinkageMasterAreaAssInfo);
            var resultemergencyLinkageMasterAreaAss =
                _Repository.AddEmergencyLinkageMasterAreaAss(_emergencyLinkageMasterAreaAss);
            var emergencyLinkageMasterAreaAssresponse = new BasicResponse<EmergencyLinkageMasterAreaAssInfo>();
            emergencyLinkageMasterAreaAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterAreaAssModel, EmergencyLinkageMasterAreaAssInfo>(
                    resultemergencyLinkageMasterAreaAss);
            return emergencyLinkageMasterAreaAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterAreaAssInfo> UpdateEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssUpdateRequest emergencyLinkageMasterAreaAssRequest)
        {
            var _emergencyLinkageMasterAreaAss =
                ObjectConverter.Copy<EmergencyLinkageMasterAreaAssInfo, EmergencyLinkageMasterAreaAssModel>(
                    emergencyLinkageMasterAreaAssRequest.EmergencyLinkageMasterAreaAssInfo);
            _Repository.UpdateEmergencyLinkageMasterAreaAss(_emergencyLinkageMasterAreaAss);
            var emergencyLinkageMasterAreaAssresponse = new BasicResponse<EmergencyLinkageMasterAreaAssInfo>();
            emergencyLinkageMasterAreaAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkageMasterAreaAssModel, EmergencyLinkageMasterAreaAssInfo>(
                    _emergencyLinkageMasterAreaAss);
            return emergencyLinkageMasterAreaAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkageMasterAreaAss(
            EmergencyLinkageMasterAreaAssDeleteRequest emergencyLinkageMasterAreaAssRequest)
        {
            _Repository.DeleteEmergencyLinkageMasterAreaAss(emergencyLinkageMasterAreaAssRequest.Id);
            var emergencyLinkageMasterAreaAssresponse = new BasicResponse();
            return emergencyLinkageMasterAreaAssresponse;
        }

        public BasicResponse<List<EmergencyLinkageMasterAreaAssInfo>> GetEmergencyLinkageMasterAreaAssList(
            EmergencyLinkageMasterAreaAssGetListRequest emergencyLinkageMasterAreaAssRequest)
        {
            var emergencyLinkageMasterAreaAssresponse = new BasicResponse<List<EmergencyLinkageMasterAreaAssInfo>>();
            emergencyLinkageMasterAreaAssRequest.PagerInfo.PageIndex =
                emergencyLinkageMasterAreaAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkageMasterAreaAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkageMasterAreaAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkageMasterAreaAssModelLists = _Repository.GetEmergencyLinkageMasterAreaAssList(
                emergencyLinkageMasterAreaAssRequest.PagerInfo.PageIndex,
                emergencyLinkageMasterAreaAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkageMasterAreaAssInfoLists = new List<EmergencyLinkageMasterAreaAssInfo>();
            foreach (var item in emergencyLinkageMasterAreaAssModelLists)
            {
                var EmergencyLinkageMasterAreaAssInfo = ObjectConverter
                    .Copy<EmergencyLinkageMasterAreaAssModel, EmergencyLinkageMasterAreaAssInfo>(item);
                emergencyLinkageMasterAreaAssInfoLists.Add(EmergencyLinkageMasterAreaAssInfo);
            }
            emergencyLinkageMasterAreaAssresponse.Data = emergencyLinkageMasterAreaAssInfoLists;
            return emergencyLinkageMasterAreaAssresponse;
        }

        public BasicResponse<EmergencyLinkageMasterAreaAssInfo> GetEmergencyLinkageMasterAreaAssById(
            EmergencyLinkageMasterAreaAssGetRequest emergencyLinkageMasterAreaAssRequest)
        {
            var result = _Repository.GetEmergencyLinkageMasterAreaAssById(emergencyLinkageMasterAreaAssRequest.Id);
            var emergencyLinkageMasterAreaAssInfo =
                ObjectConverter.Copy<EmergencyLinkageMasterAreaAssModel, EmergencyLinkageMasterAreaAssInfo>(result);
            var emergencyLinkageMasterAreaAssresponse = new BasicResponse<EmergencyLinkageMasterAreaAssInfo>();
            emergencyLinkageMasterAreaAssresponse.Data = emergencyLinkageMasterAreaAssInfo;
            return emergencyLinkageMasterAreaAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkageMasterAreaAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkageMasterAreaAssModel, EmergencyLinkageMasterAreaAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkageMasterAreaAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }
    }
}