using System.Collections.Generic;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Jc_Bx;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class CalibrationDefService : ICalibrationDefService
    {
        private readonly ICalibrationDefRepository _Repository;

        public CalibrationDefService(ICalibrationDefRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<Jc_BxInfo> AddCalibrationDef(Jc_BxAddRequest jc_Bxrequest)
        {
            var _jc_Bx = ObjectConverter.Copy<Jc_BxInfo, Jc_BxModel>(jc_Bxrequest.Jc_BxInfo);
            var resultjc_Bx = _Repository.AddCalibrationDef(_jc_Bx);
            var jc_Bxresponse = new BasicResponse<Jc_BxInfo>();
            jc_Bxresponse.Data = ObjectConverter.Copy<Jc_BxModel, Jc_BxInfo>(resultjc_Bx);
            return jc_Bxresponse;
        }

        public BasicResponse<Jc_BxInfo> UpdateCalibrationDef(Jc_BxUpdateRequest jc_Bxrequest)
        {
            var _jc_Bx = ObjectConverter.Copy<Jc_BxInfo, Jc_BxModel>(jc_Bxrequest.Jc_BxInfo);
            _Repository.UpdateCalibrationDef(_jc_Bx);
            var jc_Bxresponse = new BasicResponse<Jc_BxInfo>();
            jc_Bxresponse.Data = ObjectConverter.Copy<Jc_BxModel, Jc_BxInfo>(_jc_Bx);
            return jc_Bxresponse;
        }

        public BasicResponse DeleteCalibrationDef(Jc_BxDeleteRequest jc_Bxrequest)
        {
            _Repository.DeleteCalibrationDef(jc_Bxrequest.Id);
            var jc_Bxresponse = new BasicResponse();
            return jc_Bxresponse;
        }

        public BasicResponse<List<Jc_BxInfo>> GetCalibrationDefList(Jc_BxGetListRequest jc_Bxrequest)
        {
            var jc_Bxresponse = new BasicResponse<List<Jc_BxInfo>>();
            jc_Bxrequest.PagerInfo.PageIndex = jc_Bxrequest.PagerInfo.PageIndex - 1;
            if (jc_Bxrequest.PagerInfo.PageIndex < 0)
                jc_Bxrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var jc_BxModelLists = _Repository.GetCalibrationDefList(jc_Bxrequest.PagerInfo.PageIndex,
                jc_Bxrequest.PagerInfo.PageSize, out rowcount);
            var jc_BxInfoLists = new List<Jc_BxInfo>();
            foreach (var item in jc_BxModelLists)
            {
                var Jc_BxInfo = ObjectConverter.Copy<Jc_BxModel, Jc_BxInfo>(item);
                jc_BxInfoLists.Add(Jc_BxInfo);
            }
            jc_Bxresponse.Data = jc_BxInfoLists;
            return jc_Bxresponse;
        }

        public BasicResponse<Jc_BxInfo> GetCalibrationDefById(Jc_BxGetRequest jc_Bxrequest)
        {
            var result = _Repository.GetCalibrationDefById(jc_Bxrequest.Id);
            var jc_BxInfo = ObjectConverter.Copy<Jc_BxModel, Jc_BxInfo>(result);
            var jc_Bxresponse = new BasicResponse<Jc_BxInfo>();
            jc_Bxresponse.Data = jc_BxInfo;
            return jc_Bxresponse;
        }
    }
}