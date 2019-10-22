using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AnalysisTemplate;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;
using Sys.Safety.DataAccess;
using Basic.Framework.Data;

namespace Sys.Safety.Services
{
    public partial class AnalysisTemplateService : IAnalysisTemplateService
    {
        private IAnalysisTemplateRepository _Repository;
        private IAnalyticalExpressionRepository _analyticalExpressionRepository;
        private IAnalysisTemplateConfigRepository _analysisTemplateConfigRepository;
        private IExpressionConfigRepository _expressionConfigRepository;


        public AnalysisTemplateService(IAnalysisTemplateRepository _Repository,
           IAnalyticalExpressionRepository _analyticalExpressionRepository,
            IAnalysisTemplateConfigRepository _analysisTemplateConfigRepository,
            IExpressionConfigRepository _expressionConfigRepository)
        {
            this._Repository = _Repository;
            this._analyticalExpressionRepository = _analyticalExpressionRepository;
            this._analysisTemplateConfigRepository = _analysisTemplateConfigRepository;
            this._expressionConfigRepository = _expressionConfigRepository;
        }
        public BasicResponse<JC_AnalysisTemplateInfo> AddJC_Analysistemplate(AnalysisTemplateAddRequest jC_Analysistemplaterequest)
        {
            var _jC_Analysistemplate = ObjectConverter.Copy<JC_AnalysisTemplateInfo, JC_AnalysistemplateModel>(jC_Analysistemplaterequest.JC_AnalysisTemplateInfo);
            var jC_Analysistemplateresponse = new BasicResponse<JC_AnalysisTemplateInfo>();

            var resultjC_Analysistemplate = _Repository.GetJC_AnalysistemplateByName(jC_Analysistemplaterequest.JC_AnalysisTemplateInfo.Name);

            if (resultjC_Analysistemplate != null && !string.IsNullOrWhiteSpace(resultjC_Analysistemplate.Name))
            {
                jC_Analysistemplateresponse.Code = -20;
                jC_Analysistemplateresponse.Message = string.Format("模板:{0}, 已存在!", resultjC_Analysistemplate.Name);

                return jC_Analysistemplateresponse;
            }
            //1.模板
            resultjC_Analysistemplate = _Repository.AddJC_Analysistemplate(_jC_Analysistemplate);


            return jC_Analysistemplateresponse;
        }
        public BasicResponse<JC_AnalysisTemplateInfo> UpdateJC_Analysistemplate(AnalysisTemplateUpdateRequest jC_Analysistemplaterequest)
        {
            JC_AnalysisTemplateInfo updateInfo = jC_Analysistemplaterequest.JC_AnalysisTemplateInfo;
            if (updateInfo == null)
                return new BasicResponse<JC_AnalysisTemplateInfo>() { Code = -100, Data = jC_Analysistemplaterequest.JC_AnalysisTemplateInfo, Message = "参数异常" };
            JC_AnalysistemplateModel model = _Repository.GetJC_AnalysistemplateById(updateInfo.Id);
            if (model == null)
                return new BasicResponse<JC_AnalysisTemplateInfo>() { Code = -100, Data = jC_Analysistemplaterequest.JC_AnalysisTemplateInfo, Message = "分析模板不存在." };
            model.IsDeleted = updateInfo.IsDeleted;
            model.Name = updateInfo.Name;
            _Repository.UpdateJC_Analysistemplate(model);
            var jC_Analysistemplateresponse = new BasicResponse<JC_AnalysisTemplateInfo>();
            jC_Analysistemplateresponse.Data = ObjectConverter.Copy<JC_AnalysistemplateModel, JC_AnalysisTemplateInfo>(model);
            return jC_Analysistemplateresponse;
        }
        public BasicResponse DeleteJC_Analysistemplate(AnalysisTemplateDeleteRequest jC_Analysistemplaterequest)
        {
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in jC_Analysistemplaterequest.Ids)
                {
                    _Repository.DeleteJC_Analysistemplate(item);

                    //1.删除表达式 
                    //根据模板ID 查询表达式信息
                    //global_AnalyticalExpressionService_GetAnalyticalExpressionListByTempleteId
                    _analyticalExpressionRepository.DeleteJC_AnalyticalexpressionByTempleteId(item);
                  
                    //2.删除分析模板配置表
                    _analysisTemplateConfigRepository.DeleteJC_AnalysistemplateconfigByTempleteId(item);
                    //3.删除表达式配置表 
                    _expressionConfigRepository.DeleteJC_ExpressionconfigByTempleteId(item);
                }
            });
            var jC_Analysistemplateresponse = new BasicResponse();
            return jC_Analysistemplateresponse;
        }
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateList(AnalysisTemplateGetListRequest jC_Analysistemplaterequest)
        {
            var jC_Analysistemplateresponse = new BasicResponse<List<JC_AnalysisTemplateInfo>>();
            jC_Analysistemplaterequest.PagerInfo.PageIndex = jC_Analysistemplaterequest.PagerInfo.PageIndex - 1;
            if (jC_Analysistemplaterequest.PagerInfo.PageIndex < 0)
            {
                jC_Analysistemplaterequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            jC_Analysistemplaterequest.PagerInfo.PageSize = 0;
            var jC_AnalysistemplateModelLists = _Repository.GetJC_AnalysistemplateList(jC_Analysistemplaterequest.PagerInfo.PageIndex, jC_Analysistemplaterequest.PagerInfo.PageSize, out rowcount);
            var jC_AnalysistemplateInfoLists = new List<JC_AnalysisTemplateInfo>();
            foreach (var item in jC_AnalysistemplateModelLists)
            {
                var JC_AnalysisTemplateInfo = ObjectConverter.Copy<JC_AnalysistemplateModel, JC_AnalysisTemplateInfo>(item);
                jC_AnalysistemplateInfoLists.Add(JC_AnalysisTemplateInfo);
            }
            jC_Analysistemplateresponse.Data = jC_AnalysistemplateInfoLists;
            return jC_Analysistemplateresponse;
        }
        /// <summary>
        /// 根据模板name模糊查询模板列表
        /// </summary>
        /// <param name="jC_Analysistemplaterequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetJC_AnalysistemplateListByName(AnalysisTemplateGetListByNameRequest jC_Analysistemplaterequest)
        {
            var jC_Analysistemplateresponse = new BasicResponse<List<JC_AnalysisTemplateInfo>>();
            jC_Analysistemplaterequest.PagerInfo.PageIndex = jC_Analysistemplaterequest.PagerInfo.PageIndex - 1;
            if (jC_Analysistemplaterequest.PagerInfo.PageIndex < 0)
            {
                jC_Analysistemplaterequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            rowcount = jC_Analysistemplaterequest.PagerInfo.RowCount;
            var jC_AnalysistemplateModelLists = _Repository.GetJC_AnalysistemplateListByName(jC_Analysistemplaterequest.Name.Trim()
                , jC_Analysistemplaterequest.PagerInfo.PageIndex, jC_Analysistemplaterequest.PagerInfo.PageSize, out rowcount);
            var jC_AnalysistemplateInfoLists = new List<JC_AnalysisTemplateInfo>();
            foreach (var item in jC_AnalysistemplateModelLists)
            {
                var JC_AnalysisTemplateInfo = ObjectConverter.Copy<JC_AnalysistemplateModel, JC_AnalysisTemplateInfo>(item);
                jC_AnalysistemplateInfoLists.Add(JC_AnalysisTemplateInfo);
            }
            jC_Analysistemplateresponse.Data = jC_AnalysistemplateInfoLists;
            jC_Analysistemplateresponse.PagerInfo.PageIndex = jC_Analysistemplaterequest.PagerInfo.PageIndex;
            jC_Analysistemplateresponse.PagerInfo.PageSize = jC_Analysistemplaterequest.PagerInfo.PageSize;
            jC_Analysistemplateresponse.PagerInfo.RowCount = rowcount;
            return jC_Analysistemplateresponse;
        }
        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateById(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            var result = _Repository.GetJC_AnalysistemplateById(jC_Analysistemplaterequest.Id);
            var jC_AnalysistemplateInfo = ObjectConverter.Copy<JC_AnalysistemplateModel, JC_AnalysisTemplateInfo>(result);
            var jC_Analysistemplateresponse = new BasicResponse<JC_AnalysisTemplateInfo>();
            jC_Analysistemplateresponse.Data = jC_AnalysistemplateInfo;
            return jC_Analysistemplateresponse;
        }
        public BasicResponse<JC_AnalysisTemplateInfo> GetJC_AnalysistemplateByTempleteId(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            var result = _Repository.GetJC_AnalysistemplateByTempleteId(jC_Analysistemplaterequest.TempleteId);
            var jC_AnalysistemplateInfo = ObjectConverter.Copy<JC_AnalysistemplateModel, JC_AnalysisTemplateInfo>(result);
            var jC_Analysistemplateresponse = new BasicResponse<JC_AnalysisTemplateInfo>();
            jC_Analysistemplateresponse.Data = jC_AnalysistemplateInfo;
            return jC_Analysistemplateresponse;
        }
        /// <summary>
        /// 根据模型ID查询模型列表（模型ID为空时，查询所有）.
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns>模型列表</returns>
        public BasicResponse<List<JC_AnalysisTemplateInfo>> GetAnalysisTemplateListDetail(AnalysisTemplateGetRequest jC_Analysistemplaterequest)
        {
            DataTable dataTable = _Repository.QueryTable("global_AnalysisTemplateService_GetAnalysisTemplateListDetail");

            List<JC_AnalysisTemplateInfo> listResult = ObjectConverter.Copy<JC_AnalysisTemplateInfo>(dataTable);

            var jc_AnalysisTemplateResponse = new BasicResponse<List<JC_AnalysisTemplateInfo>>();
            if (string.IsNullOrWhiteSpace(jC_Analysistemplaterequest.Id))
            {
                jc_AnalysisTemplateResponse.Data = listResult;
            }
            else
            {
                jc_AnalysisTemplateResponse.Data = listResult.Where(t => t.Id == jC_Analysistemplaterequest.Id).ToList();
            }

            return jc_AnalysisTemplateResponse;
        }
    }
}


