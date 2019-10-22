using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Jc_Defwb;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class DeviceKoriyasuService : IDeviceKoriyasuService
    {
        private readonly IDeviceKoriyasuRepository _Repository;

        public DeviceKoriyasuService(IDeviceKoriyasuRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<Jc_DefwbInfo> AddDeviceKoriyasu(DeviceKoriyasuAddRequest jc_Defwbrequest)
        {
            var _jc_Defwb = ObjectConverter.Copy<Jc_DefwbInfo, Jc_DefwbModel>(jc_Defwbrequest.Jc_DefwbInfo);
            var resultjc_Defwb = _Repository.AddDeviceKoriyasu(_jc_Defwb);
            var jc_Defwbresponse = new BasicResponse<Jc_DefwbInfo>();
            jc_Defwbresponse.Data = ObjectConverter.Copy<Jc_DefwbModel, Jc_DefwbInfo>(resultjc_Defwb);
            return jc_Defwbresponse;
        }

        public BasicResponse<Jc_DefwbInfo> UpdateDeviceKoriyasu(Jc_DefwbUpdateRequest jc_Defwbrequest)
        {
            var _jc_Defwb = ObjectConverter.Copy<Jc_DefwbInfo, Jc_DefwbModel>(jc_Defwbrequest.Jc_DefwbInfo);
            _Repository.UpdateDeviceKoriyasu(_jc_Defwb);
            var jc_Defwbresponse = new BasicResponse<Jc_DefwbInfo>();
            jc_Defwbresponse.Data = ObjectConverter.Copy<Jc_DefwbModel, Jc_DefwbInfo>(_jc_Defwb);
            return jc_Defwbresponse;
        }

        public BasicResponse DeleteDeviceKoriyasu(Jc_DefwbDeleteRequest jc_Defwbrequest)
        {
            _Repository.DeleteDeviceKoriyasu(jc_Defwbrequest.Id);
            var jc_Defwbresponse = new BasicResponse();
            return jc_Defwbresponse;
        }

        public BasicResponse<List<Jc_DefwbInfo>> GetDeviceKoriyasuList(Jc_DefwbGetListRequest jc_Defwbrequest)
        {
            var jc_Defwbresponse = new BasicResponse<List<Jc_DefwbInfo>>();
            jc_Defwbrequest.PagerInfo.PageIndex = jc_Defwbrequest.PagerInfo.PageIndex - 1;
            if (jc_Defwbrequest.PagerInfo.PageIndex < 0)
                jc_Defwbrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var jc_DefwbModelLists = _Repository.GetDeviceKoriyasuList(jc_Defwbrequest.PagerInfo.PageIndex,
                jc_Defwbrequest.PagerInfo.PageSize, out rowcount);
            var jc_DefwbInfoLists = new List<Jc_DefwbInfo>();
            foreach (var item in jc_DefwbModelLists)
            {
                var Jc_DefwbInfo = ObjectConverter.Copy<Jc_DefwbModel, Jc_DefwbInfo>(item);
                jc_DefwbInfoLists.Add(Jc_DefwbInfo);
            }
            jc_Defwbresponse.Data = jc_DefwbInfoLists;
            return jc_Defwbresponse;
        }

        public BasicResponse<Jc_DefwbInfo> GetDeviceKoriyasuById(Jc_DefwbGetRequest jc_Defwbrequest)
        {
            var result = _Repository.GetDeviceKoriyasuById(jc_Defwbrequest.Id);
            var jc_DefwbInfo = ObjectConverter.Copy<Jc_DefwbModel, Jc_DefwbInfo>(result);
            var jc_Defwbresponse = new BasicResponse<Jc_DefwbInfo>();
            jc_Defwbresponse.Data = jc_DefwbInfo;
            return jc_Defwbresponse;
        }
    }
}