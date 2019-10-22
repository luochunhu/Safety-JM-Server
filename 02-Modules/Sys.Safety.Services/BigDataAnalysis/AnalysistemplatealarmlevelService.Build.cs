using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Analysistemplatealarmlevel;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Services
{
    public partial class Jc_AnalysistemplatealarmlevelService : IJc_AnalysistemplatealarmlevelService
    {
        private IJc_AnalysistemplatealarmlevelRepository _Repository;
        private IAnalysisTemplateAlarmLevelCacheService _AnalysisTemplateAlarmLevelCacheService;

        public Jc_AnalysistemplatealarmlevelService(IJc_AnalysistemplatealarmlevelRepository _Repository,
            IAnalysisTemplateAlarmLevelCacheService _AnalysisTemplateAlarmLevelCacheService)
        {
            this._Repository = _Repository;
            this._AnalysisTemplateAlarmLevelCacheService = _AnalysisTemplateAlarmLevelCacheService;
        }
        public BasicResponse<Jc_AnalysistemplatealarmlevelInfo> AddAnalysistemplatealarmlevel(AnalysistemplatealarmlevelAddRequest analysistemplatealarmlevelRequest)
        {
            var _analysistemplatealarmlevel = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelInfo, Jc_AnalysistemplatealarmlevelModel>(analysistemplatealarmlevelRequest.AnalysistemplatealarmlevelInfo);
            var resultanalysistemplatealarmlevel = _Repository.AddAnalysistemplatealarmlevel(_analysistemplatealarmlevel);

            var analysistemplatealarmlevelinfo = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelModel, Jc_AnalysistemplatealarmlevelInfo>(resultanalysistemplatealarmlevel);

            //添加缓存
            AnalysisTemplateAlarmLevelCacheAddRequest cachereqeust = new AnalysisTemplateAlarmLevelCacheAddRequest();
            cachereqeust.AnalysistemplatealarmlevelInfo = analysistemplatealarmlevelinfo;
            _AnalysisTemplateAlarmLevelCacheService.AddAnalysisTemplateAlarmLevelCache(cachereqeust);

            var analysistemplatealarmlevelresponse = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            analysistemplatealarmlevelresponse.Data = analysistemplatealarmlevelinfo;
            return analysistemplatealarmlevelresponse;
        }
        public BasicResponse<Jc_AnalysistemplatealarmlevelInfo> UpdateAnalysistemplatealarmlevel(AnalysistemplatealarmlevelUpdateRequest analysistemplatealarmlevelRequest)
        {
            var _analysistemplatealarmlevel = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelInfo, Jc_AnalysistemplatealarmlevelModel>(analysistemplatealarmlevelRequest.AnalysistemplatealarmlevelInfo);
            _Repository.UpdateAnalysistemplatealarmlevel(_analysistemplatealarmlevel);

            var analysistemplatealarmlevelinfo = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelModel, Jc_AnalysistemplatealarmlevelInfo>(_analysistemplatealarmlevel);

            //修改缓存
            AnalysisTemplateAlarmLevelCacheUpdateRequest cachereqeust = new AnalysisTemplateAlarmLevelCacheUpdateRequest();
            cachereqeust.AnalysistemplatealarmlevelInfo = analysistemplatealarmlevelinfo;
            _AnalysisTemplateAlarmLevelCacheService.UpdateAnalysisTemplateAlarmLevelCache(cachereqeust);

            var analysistemplatealarmlevelresponse = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            analysistemplatealarmlevelresponse.Data = analysistemplatealarmlevelinfo;
            return analysistemplatealarmlevelresponse;
        }
        public BasicResponse DeleteAnalysistemplatealarmlevel(AnalysistemplatealarmlevelDeleteRequest analysistemplatealarmlevelRequest)
        {
            var _analysistemplatealarmlevel = _Repository.Datas.FirstOrDefault(o => o.Id == analysistemplatealarmlevelRequest.Id);
            var analysistemplatealarmlevelinfo = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelModel, Jc_AnalysistemplatealarmlevelInfo>(_analysistemplatealarmlevel);

            _Repository.DeleteAnalysistemplatealarmlevel(analysistemplatealarmlevelRequest.Id);

            //删除缓存
            AnalysisTemplateAlarmLevelCacheDeleteRequest cachereqeust = new AnalysisTemplateAlarmLevelCacheDeleteRequest();
            cachereqeust.AnalysistemplatealarmlevelInfo = analysistemplatealarmlevelinfo;
            _AnalysisTemplateAlarmLevelCacheService.DeleteAnalysisTemplateAlarmLevelCache(cachereqeust);

            var analysistemplatealarmlevelresponse = new BasicResponse();
            return analysistemplatealarmlevelresponse;
        }
        public BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAnalysistemplatealarmlevelList(AnalysistemplatealarmlevelGetListRequest analysistemplatealarmlevelRequest)
        {
            var analysistemplatealarmlevelresponse = new BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>();
            analysistemplatealarmlevelRequest.PagerInfo.PageIndex = analysistemplatealarmlevelRequest.PagerInfo.PageIndex - 1;
            if (analysistemplatealarmlevelRequest.PagerInfo.PageIndex < 0)
            {
                analysistemplatealarmlevelRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var analysistemplatealarmlevelModelLists = _Repository.GetAnalysistemplatealarmlevelList(analysistemplatealarmlevelRequest.PagerInfo.PageIndex, analysistemplatealarmlevelRequest.PagerInfo.PageSize, out rowcount);
            var analysistemplatealarmlevelInfoLists = new List<Jc_AnalysistemplatealarmlevelInfo>();
            foreach (var item in analysistemplatealarmlevelModelLists)
            {
                var AnalysistemplatealarmlevelInfo = ObjectConverter.Copy<Jc_AnalysistemplatealarmlevelModel, Jc_AnalysistemplatealarmlevelInfo>(item);
                analysistemplatealarmlevelInfoLists.Add(AnalysistemplatealarmlevelInfo);
            }
            analysistemplatealarmlevelresponse.Data = analysistemplatealarmlevelInfoLists;
            return analysistemplatealarmlevelresponse;
        }
        public BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelById(AnalysistemplatealarmlevelGetRequest analysistemplatealarmlevelRequest)
        {
            AnalysisTemplateAlarmLevelCacheGetByIdRequest cacherequest = new AnalysisTemplateAlarmLevelCacheGetByIdRequest();
            cacherequest.Id = analysistemplatealarmlevelRequest.Id;
            var analysistemplatealarmlevelInfo = _AnalysisTemplateAlarmLevelCacheService.GetByIdAnalysisTemplateAlarmLevelCache(cacherequest).Data;

            var analysistemplatealarmlevelresponse = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            analysistemplatealarmlevelresponse.Data = analysistemplatealarmlevelInfo;
            return analysistemplatealarmlevelresponse;
        }


        public BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelByAnalysistemplateId(AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest analysistemplatealarmlevelRequest)
        {
            AnalysisTemplateAlarmLevelCacheGetByTemplateIdRequest cacherequest = new AnalysisTemplateAlarmLevelCacheGetByTemplateIdRequest();
            cacherequest.TemplateId = analysistemplatealarmlevelRequest.AnalysistemplateId;
            var analysistemplatealarmlevelInfo = _AnalysisTemplateAlarmLevelCacheService.GetByTemplateIdAnalysisTemplateAlarmLevelCache(cacherequest).Data;

            var analysistemplatealarmlevelresponse = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            analysistemplatealarmlevelresponse.Data = analysistemplatealarmlevelInfo;
            return analysistemplatealarmlevelresponse;
        }


        public BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysistemplateAlarmLevelInfos()
        {
            var result = _Repository.Datas.ToList();
            var analysistemplatealarmlevelInfos = ObjectConverter.CopyList<Jc_AnalysistemplatealarmlevelModel, Jc_AnalysistemplatealarmlevelInfo>(result);
            BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> response = new BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>();
            response.Data = analysistemplatealarmlevelInfos.ToList();
            return response;
        }
    }
}


