using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.EmergencyLinkagePassiveAreaAss;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class EmergencyLinkagePassiveAreaAssService : IEmergencyLinkagePassiveAreaAssService
    {
        private readonly IEmergencyLinkagePassiveAreaAssRepository _Repository;

        public EmergencyLinkagePassiveAreaAssService(IEmergencyLinkagePassiveAreaAssRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<EmergencyLinkagePassiveAreaAssInfo> AddEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssAddRequest emergencyLinkagePassiveAreaAssRequest)
        {
            var _emergencyLinkagePassiveAreaAss =
                ObjectConverter.Copy<EmergencyLinkagePassiveAreaAssInfo, EmergencyLinkagePassiveAreaAssModel>(
                    emergencyLinkagePassiveAreaAssRequest.EmergencyLinkagePassiveAreaAssInfo);
            var resultemergencyLinkagePassiveAreaAss =
                _Repository.AddEmergencyLinkagePassiveAreaAss(_emergencyLinkagePassiveAreaAss);
            var emergencyLinkagePassiveAreaAssresponse = new BasicResponse<EmergencyLinkagePassiveAreaAssInfo>();
            emergencyLinkagePassiveAreaAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassiveAreaAssModel, EmergencyLinkagePassiveAreaAssInfo>(
                    resultemergencyLinkagePassiveAreaAss);
            return emergencyLinkagePassiveAreaAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassiveAreaAssInfo> UpdateEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssUpdateRequest emergencyLinkagePassiveAreaAssRequest)
        {
            var _emergencyLinkagePassiveAreaAss =
                ObjectConverter.Copy<EmergencyLinkagePassiveAreaAssInfo, EmergencyLinkagePassiveAreaAssModel>(
                    emergencyLinkagePassiveAreaAssRequest.EmergencyLinkagePassiveAreaAssInfo);
            _Repository.UpdateEmergencyLinkagePassiveAreaAss(_emergencyLinkagePassiveAreaAss);
            var emergencyLinkagePassiveAreaAssresponse = new BasicResponse<EmergencyLinkagePassiveAreaAssInfo>();
            emergencyLinkagePassiveAreaAssresponse.Data =
                ObjectConverter.Copy<EmergencyLinkagePassiveAreaAssModel, EmergencyLinkagePassiveAreaAssInfo>(
                    _emergencyLinkagePassiveAreaAss);
            return emergencyLinkagePassiveAreaAssresponse;
        }

        public BasicResponse DeleteEmergencyLinkagePassiveAreaAss(
            EmergencyLinkagePassiveAreaAssDeleteRequest emergencyLinkagePassiveAreaAssRequest)
        {
            _Repository.DeleteEmergencyLinkagePassiveAreaAss(emergencyLinkagePassiveAreaAssRequest.Id);
            var emergencyLinkagePassiveAreaAssresponse = new BasicResponse();
            return emergencyLinkagePassiveAreaAssresponse;
        }

        public BasicResponse<List<EmergencyLinkagePassiveAreaAssInfo>> GetEmergencyLinkagePassiveAreaAssList(
            EmergencyLinkagePassiveAreaAssGetListRequest emergencyLinkagePassiveAreaAssRequest)
        {
            var emergencyLinkagePassiveAreaAssresponse = new BasicResponse<List<EmergencyLinkagePassiveAreaAssInfo>>();
            emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageIndex =
                emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageIndex < 0)
                emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var emergencyLinkagePassiveAreaAssModelLists = _Repository.GetEmergencyLinkagePassiveAreaAssList(
                emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageIndex,
                emergencyLinkagePassiveAreaAssRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkagePassiveAreaAssInfoLists = new List<EmergencyLinkagePassiveAreaAssInfo>();
            foreach (var item in emergencyLinkagePassiveAreaAssModelLists)
            {
                var EmergencyLinkagePassiveAreaAssInfo = ObjectConverter
                    .Copy<EmergencyLinkagePassiveAreaAssModel, EmergencyLinkagePassiveAreaAssInfo>(item);
                emergencyLinkagePassiveAreaAssInfoLists.Add(EmergencyLinkagePassiveAreaAssInfo);
            }
            emergencyLinkagePassiveAreaAssresponse.Data = emergencyLinkagePassiveAreaAssInfoLists;
            return emergencyLinkagePassiveAreaAssresponse;
        }

        public BasicResponse<EmergencyLinkagePassiveAreaAssInfo> GetEmergencyLinkagePassiveAreaAssById(
            EmergencyLinkagePassiveAreaAssGetRequest emergencyLinkagePassiveAreaAssRequest)
        {
            var result = _Repository.GetEmergencyLinkagePassiveAreaAssById(emergencyLinkagePassiveAreaAssRequest.Id);
            var emergencyLinkagePassiveAreaAssInfo =
                ObjectConverter.Copy<EmergencyLinkagePassiveAreaAssModel, EmergencyLinkagePassiveAreaAssInfo>(result);
            var emergencyLinkagePassiveAreaAssresponse = new BasicResponse<EmergencyLinkagePassiveAreaAssInfo>();
            emergencyLinkagePassiveAreaAssresponse.Data = emergencyLinkagePassiveAreaAssInfo;
            return emergencyLinkagePassiveAreaAssresponse;
        }

        public BasicResponse<IList<EmergencyLinkagePassiveAreaAssInfo>> GetEmergencyLinkageMasterAreaAssListByAssId(LongIdRequest request)
        {
            var models = _Repository.GetEmergencyLinkageMasterAreaAssListByAssId(request.Id.ToString());
            var infos =
                ObjectConverter.CopyList<EmergencyLinkagePassiveAreaAssModel, EmergencyLinkagePassiveAreaAssInfo>(models);
            var ret = new BasicResponse<IList<EmergencyLinkagePassiveAreaAssInfo>>()
            {
                Data = infos
            };
            return ret;
        }

    }
}