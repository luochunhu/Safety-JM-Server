using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Arearestrictedperson;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class R_ArearestrictedpersonService : IR_ArearestrictedpersonService
    {
        private IR_ArearestrictedpersonRepository _Repository;

        public R_ArearestrictedpersonService(IR_ArearestrictedpersonRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<R_ArearestrictedpersonInfo> AddArearestrictedperson(R_ArearestrictedpersonAddRequest arearestrictedpersonRequest)
        {
            var _arearestrictedperson = ObjectConverter.Copy<R_ArearestrictedpersonInfo, R_ArearestrictedpersonModel>(arearestrictedpersonRequest.ArearestrictedpersonInfo);
            var resultarearestrictedperson = _Repository.AddArearestrictedperson(_arearestrictedperson);
            var arearestrictedpersonresponse = new BasicResponse<R_ArearestrictedpersonInfo>();
            arearestrictedpersonresponse.Data = ObjectConverter.Copy<R_ArearestrictedpersonModel, R_ArearestrictedpersonInfo>(resultarearestrictedperson);
            return arearestrictedpersonresponse;
        }
        public BasicResponse<R_ArearestrictedpersonInfo> UpdateArearestrictedperson(R_ArearestrictedpersonUpdateRequest arearestrictedpersonRequest)
        {
            var _arearestrictedperson = ObjectConverter.Copy<R_ArearestrictedpersonInfo, R_ArearestrictedpersonModel>(arearestrictedpersonRequest.ArearestrictedpersonInfo);
            _Repository.UpdateArearestrictedperson(_arearestrictedperson);
            var arearestrictedpersonresponse = new BasicResponse<R_ArearestrictedpersonInfo>();
            arearestrictedpersonresponse.Data = ObjectConverter.Copy<R_ArearestrictedpersonModel, R_ArearestrictedpersonInfo>(_arearestrictedperson);
            return arearestrictedpersonresponse;
        }
        public BasicResponse DeleteArearestrictedperson(R_ArearestrictedpersonDeleteRequest arearestrictedpersonRequest)
        {
            _Repository.DeleteArearestrictedperson(arearestrictedpersonRequest.Id);
            var arearestrictedpersonresponse = new BasicResponse();
            return arearestrictedpersonresponse;
        }
        public BasicResponse<List<R_ArearestrictedpersonInfo>> GetArearestrictedpersonList(R_ArearestrictedpersonGetListRequest arearestrictedpersonRequest)
        {
            var arearestrictedpersonresponse = new BasicResponse<List<R_ArearestrictedpersonInfo>>();
            arearestrictedpersonRequest.PagerInfo.PageIndex = arearestrictedpersonRequest.PagerInfo.PageIndex - 1;
            if (arearestrictedpersonRequest.PagerInfo.PageIndex < 0)
            {
                arearestrictedpersonRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var arearestrictedpersonModelLists = _Repository.GetArearestrictedpersonList(arearestrictedpersonRequest.PagerInfo.PageIndex, arearestrictedpersonRequest.PagerInfo.PageSize, out rowcount);
            var arearestrictedpersonInfoLists = new List<R_ArearestrictedpersonInfo>();
            foreach (var item in arearestrictedpersonModelLists)
            {
                var ArearestrictedpersonInfo = ObjectConverter.Copy<R_ArearestrictedpersonModel, R_ArearestrictedpersonInfo>(item);
                arearestrictedpersonInfoLists.Add(ArearestrictedpersonInfo);
            }
            arearestrictedpersonresponse.Data = arearestrictedpersonInfoLists;
            return arearestrictedpersonresponse;
        }
        public BasicResponse<R_ArearestrictedpersonInfo> GetArearestrictedpersonById(R_ArearestrictedpersonGetRequest arearestrictedpersonRequest)
        {
            var result = _Repository.GetArearestrictedpersonById(arearestrictedpersonRequest.Id);
            var arearestrictedpersonInfo = ObjectConverter.Copy<R_ArearestrictedpersonModel, R_ArearestrictedpersonInfo>(result);
            var arearestrictedpersonresponse = new BasicResponse<R_ArearestrictedpersonInfo>();
            arearestrictedpersonresponse.Data = arearestrictedpersonInfo;
            return arearestrictedpersonresponse;
        }
        /// <summary>
        /// 根据区域ID删除区域限制进入、禁止进入人员信息
        /// </summary>
        /// <param name="arearestrictedpersonRequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteArearestrictedpersonByAreaId(R_ArearestrictedpersonDeleteByAreaIdRequest arearestrictedpersonRequest)
        {
            var result = new BasicResponse();
             _Repository.Delete(a=>a.AreaId==arearestrictedpersonRequest.AreaId);
             return result;
        }
    }
}


