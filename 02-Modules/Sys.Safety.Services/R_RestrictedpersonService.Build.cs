using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Restrictedperson;

namespace Sys.Safety.Services
{
    public partial class RestrictedpersonService : IR_RestrictedpersonService
    {
        private IR_RestrictedpersonRepository _Repository;

        public RestrictedpersonService(IR_RestrictedpersonRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<R_RestrictedpersonInfo> AddRestrictedperson(R_RestrictedpersonAddRequest restrictedpersonRequest)
        {
            var _restrictedperson = ObjectConverter.Copy<R_RestrictedpersonInfo, R_RestrictedpersonModel>(restrictedpersonRequest.RestrictedpersonInfo);
            var resultrestrictedperson = _Repository.AddRestrictedperson(_restrictedperson);
            var restrictedpersonresponse = new BasicResponse<R_RestrictedpersonInfo>();
            restrictedpersonresponse.Data = ObjectConverter.Copy<R_RestrictedpersonModel, R_RestrictedpersonInfo>(resultrestrictedperson);
            return restrictedpersonresponse;
        }
        public BasicResponse<R_RestrictedpersonInfo> UpdateRestrictedperson(R_RestrictedpersonUpdateRequest restrictedpersonRequest)
        {
            var _restrictedperson = ObjectConverter.Copy<R_RestrictedpersonInfo, R_RestrictedpersonModel>(restrictedpersonRequest.RestrictedpersonInfo);
            _Repository.UpdateRestrictedperson(_restrictedperson);
            var restrictedpersonresponse = new BasicResponse<R_RestrictedpersonInfo>();
            restrictedpersonresponse.Data = ObjectConverter.Copy<R_RestrictedpersonModel, R_RestrictedpersonInfo>(_restrictedperson);
            return restrictedpersonresponse;
        }
        public BasicResponse DeleteRestrictedperson(R_RestrictedpersonDeleteRequest restrictedpersonRequest)
        {
            _Repository.DeleteRestrictedperson(restrictedpersonRequest.Id);
            var restrictedpersonresponse = new BasicResponse();
            return restrictedpersonresponse;
        }
        /// <summary>
        /// 根据测点id删除测点的限制进入、禁止进入信息
        /// </summary>
        /// <param name="restrictedpersonRequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRestrictedpersonByPointId(R_RestrictedpersonDeleteByPointIdRequest restrictedpersonRequest)
        {
            _Repository.DeleteRestrictedpersonByPointId(restrictedpersonRequest.PointId);
            var restrictedpersonresponse = new BasicResponse();
            return restrictedpersonresponse;
        }
        public BasicResponse<List<R_RestrictedpersonInfo>> GetRestrictedpersonList(R_RestrictedpersonGetListRequest restrictedpersonRequest)
        {
            var restrictedpersonresponse = new BasicResponse<List<R_RestrictedpersonInfo>>();
            restrictedpersonRequest.PagerInfo.PageIndex = restrictedpersonRequest.PagerInfo.PageIndex - 1;
            if (restrictedpersonRequest.PagerInfo.PageIndex < 0)
            {
                restrictedpersonRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var restrictedpersonModelLists = _Repository.GetRestrictedpersonList(restrictedpersonRequest.PagerInfo.PageIndex, restrictedpersonRequest.PagerInfo.PageSize, out rowcount);
            var restrictedpersonInfoLists = new List<R_RestrictedpersonInfo>();
            foreach (var item in restrictedpersonModelLists)
            {
                var RestrictedpersonInfo = ObjectConverter.Copy<R_RestrictedpersonModel, R_RestrictedpersonInfo>(item);
                restrictedpersonInfoLists.Add(RestrictedpersonInfo);
            }
            restrictedpersonresponse.Data = restrictedpersonInfoLists;
            return restrictedpersonresponse;
        }
        public BasicResponse<R_RestrictedpersonInfo> GetRestrictedpersonById(R_RestrictedpersonGetRequest restrictedpersonRequest)
        {
            var result = _Repository.GetRestrictedpersonById(restrictedpersonRequest.Id);
            var restrictedpersonInfo = ObjectConverter.Copy<R_RestrictedpersonModel, R_RestrictedpersonInfo>(result);
            var restrictedpersonresponse = new BasicResponse<R_RestrictedpersonInfo>();
            restrictedpersonresponse.Data = restrictedpersonInfo;
            return restrictedpersonresponse;
        }
    }
}


