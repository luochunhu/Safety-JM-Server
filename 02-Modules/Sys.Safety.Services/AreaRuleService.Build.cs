using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Area;

namespace Sys.Safety.Services
{
    public partial class AreaRuleService : IAreaRuleService
    {
        private IAreaRuleRepository _Repository;

        public AreaRuleService(IAreaRuleRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<AreaRuleInfo> AddAreaRule(AreaRuleAddRequest areaRuleRequest)
        {
            var _areaRule = ObjectConverter.Copy<AreaRuleInfo, AreaRuleModel>(areaRuleRequest.AreaRuleInfo);
            var resultareaRule = _Repository.AddAreaRule(_areaRule);
            var areaRuleresponse = new BasicResponse<AreaRuleInfo>();
            areaRuleresponse.Data = ObjectConverter.Copy<AreaRuleModel, AreaRuleInfo>(resultareaRule);
            return areaRuleresponse;
        }
        public BasicResponse<AreaRuleInfo> UpdateAreaRule(AreaRuleUpdateRequest areaRuleRequest)
        {
            var _areaRule = ObjectConverter.Copy<AreaRuleInfo, AreaRuleModel>(areaRuleRequest.AreaRuleInfo);
            _Repository.UpdateAreaRule(_areaRule);
            var areaRuleresponse = new BasicResponse<AreaRuleInfo>();
            areaRuleresponse.Data = ObjectConverter.Copy<AreaRuleModel, AreaRuleInfo>(_areaRule);
            return areaRuleresponse;
        }
        public BasicResponse DeleteAreaRule(AreaRuleDeleteRequest areaRuleRequest)
        {
            _Repository.DeleteAreaRule(areaRuleRequest.Id);
            var areaRuleresponse = new BasicResponse();
            return areaRuleresponse;
        }
        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleList(AreaRuleGetListRequest areaRuleRequest)
        {
            var areaRuleresponse = new BasicResponse<List<AreaRuleInfo>>();
            areaRuleRequest.PagerInfo.PageIndex = areaRuleRequest.PagerInfo.PageIndex - 1;
            if (areaRuleRequest.PagerInfo.PageIndex < 0)
            {
                areaRuleRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var areaRuleModelLists = _Repository.GetAreaRuleList(areaRuleRequest.PagerInfo.PageIndex, areaRuleRequest.PagerInfo.PageSize, out rowcount);
            var areaRuleInfoLists = new List<AreaRuleInfo>();
            foreach (var item in areaRuleModelLists)
            {
                var AreaRuleInfo = ObjectConverter.Copy<AreaRuleModel, AreaRuleInfo>(item);
                areaRuleInfoLists.Add(AreaRuleInfo);
            }
            areaRuleresponse.Data = areaRuleInfoLists;
            return areaRuleresponse;
        }
        public BasicResponse<AreaRuleInfo> GetAreaRuleById(AreaRuleGetRequest areaRuleRequest)
        {
            var result = _Repository.GetAreaRuleById(areaRuleRequest.Id);
            var areaRuleInfo = ObjectConverter.Copy<AreaRuleModel, AreaRuleInfo>(result);
            var areaRuleresponse = new BasicResponse<AreaRuleInfo>();
            areaRuleresponse.Data = areaRuleInfo;
            return areaRuleresponse;
        }

        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleListByAreaID(GetAreaRuleListByAreaIDRequest areaRuleRequest)
        {
            var result = _Repository.GetAreaRuleList();
            var areaRuleInfo = ObjectConverter.CopyList<AreaRuleModel, AreaRuleInfo>(result);
            List<AreaRuleInfo> areaRuleItems = areaRuleInfo.Where(a => a.Areaid == areaRuleRequest.areaID).ToList();
            var areaRuleresponse = new BasicResponse<List<AreaRuleInfo>>();
            areaRuleresponse.Data = areaRuleItems;
            return areaRuleresponse;
        }


        public BasicResponse DeleteAreaRuleByAreaID(AreaRuleDeleteRequest areaRuleRequest)
        {
            _Repository.DeleteAreaRuleByAreaID(areaRuleRequest.Id);
            var areaRuleresponse = new BasicResponse();
            return areaRuleresponse;
        }
    }
}


