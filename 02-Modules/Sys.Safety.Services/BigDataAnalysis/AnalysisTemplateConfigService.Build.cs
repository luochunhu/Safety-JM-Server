using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AnalysisTemplateConfig;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class AnalysisTemplateConfigService : IAnalysisTemplateConfigService
    {
        private IAnalysisTemplateConfigRepository _Repository;

        public AnalysisTemplateConfigService(IAnalysisTemplateConfigRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_AnalysisTemplateConfigInfo> AddJC_Analysistemplateconfig(AnalysisTemplateConfigAddRequest jC_Analysistemplateconfigrequest)
        {
            var _jC_Analysistemplateconfig = ObjectConverter.Copy<JC_AnalysisTemplateConfigInfo, JC_AnalysistemplateconfigModel>(jC_Analysistemplateconfigrequest.JC_AnalysisTemplateConfigInfo);
            var resultjC_Analysistemplateconfig = _Repository.AddJC_Analysistemplateconfig(_jC_Analysistemplateconfig);
            var jC_Analysistemplateconfigresponse = new BasicResponse<JC_AnalysisTemplateConfigInfo>();
            jC_Analysistemplateconfigresponse.Data = ObjectConverter.Copy<JC_AnalysistemplateconfigModel, JC_AnalysisTemplateConfigInfo>(resultjC_Analysistemplateconfig);
            return jC_Analysistemplateconfigresponse;
        }
        /// <summary>
        /// 批量新增分析模型配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> AddAnalysistemplateconfigList(AnalysisTemplateConfigListAddRequest jC_Analysistemplateconfigrequest)
        {
            var _jC_Analysistemplateconfig = ObjectConverter.CopyList<JC_AnalysisTemplateConfigInfo, JC_AnalysistemplateconfigModel>(jC_Analysistemplateconfigrequest.JC_AnalysisTemplateConfigInfoList);
            var resultjC_Analysistemplateconfig = _Repository.AddAnalysistemplateconfigList(_jC_Analysistemplateconfig);
            var jC_Analysistemplateconfigresponse = new BasicResponse<List<JC_AnalysisTemplateConfigInfo>>();
            jC_Analysistemplateconfigresponse.Data = ObjectConverter.CopyList<JC_AnalysistemplateconfigModel,
                JC_AnalysisTemplateConfigInfo>(resultjC_Analysistemplateconfig).ToList();
            return jC_Analysistemplateconfigresponse;
        }

        public BasicResponse<JC_AnalysisTemplateConfigInfo> UpdateJC_Analysistemplateconfig(AnalysisTemplateConfigUpdateRequest jC_Analysistemplateconfigrequest)
        {
            var _jC_Analysistemplateconfig = ObjectConverter.Copy<JC_AnalysisTemplateConfigInfo, JC_AnalysistemplateconfigModel>(jC_Analysistemplateconfigrequest.JC_AnalysisTemplateConfigInfo);
            _Repository.UpdateJC_Analysistemplateconfig(_jC_Analysistemplateconfig);
            var jC_Analysistemplateconfigresponse = new BasicResponse<JC_AnalysisTemplateConfigInfo>();
            jC_Analysistemplateconfigresponse.Data = ObjectConverter.Copy<JC_AnalysistemplateconfigModel, JC_AnalysisTemplateConfigInfo>(_jC_Analysistemplateconfig);
            return jC_Analysistemplateconfigresponse;
        }
        public BasicResponse DeleteJC_Analysistemplateconfig(AnalysisTemplateConfigDeleteRequest jC_Analysistemplateconfigrequest)
        {
            _Repository.DeleteJC_Analysistemplateconfig(jC_Analysistemplateconfigrequest.Id);
            var jC_Analysistemplateconfigresponse = new BasicResponse();
            return jC_Analysistemplateconfigresponse;
        }
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigList(AnalysisTemplateConfigGetListRequest jC_Analysistemplateconfigrequest)
        {
            var jC_Analysistemplateconfigresponse = new BasicResponse<List<JC_AnalysisTemplateConfigInfo>>();
            jC_Analysistemplateconfigrequest.PagerInfo.PageIndex = jC_Analysistemplateconfigrequest.PagerInfo.PageIndex - 1;
            if (jC_Analysistemplateconfigrequest.PagerInfo.PageIndex < 0)
            {
                jC_Analysistemplateconfigrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_AnalysistemplateconfigModelLists = _Repository.GetJC_AnalysistemplateconfigList(jC_Analysistemplateconfigrequest.PagerInfo.PageIndex, jC_Analysistemplateconfigrequest.PagerInfo.PageSize, out rowcount);
            var jC_AnalysistemplateconfigInfoLists = new List<JC_AnalysisTemplateConfigInfo>();
            foreach (var item in jC_AnalysistemplateconfigModelLists)
            {
                var JC_AnalysisTemplateConfigInfo = ObjectConverter.Copy<JC_AnalysistemplateconfigModel, JC_AnalysisTemplateConfigInfo>(item);
                jC_AnalysistemplateconfigInfoLists.Add(JC_AnalysisTemplateConfigInfo);
            }
            jC_Analysistemplateconfigresponse.Data = jC_AnalysistemplateconfigInfoLists;
            return jC_Analysistemplateconfigresponse;
        }
        public BasicResponse<JC_AnalysisTemplateConfigInfo> GetJC_AnalysistemplateconfigById(AnalysisTemplateConfigGetRequest jC_Analysistemplateconfigrequest)
        {
            var result = _Repository.GetJC_AnalysistemplateconfigById(jC_Analysistemplateconfigrequest.Id);
            var jC_AnalysistemplateconfigInfo = ObjectConverter.Copy<JC_AnalysistemplateconfigModel, JC_AnalysisTemplateConfigInfo>(result);
            var jC_Analysistemplateconfigresponse = new BasicResponse<JC_AnalysisTemplateConfigInfo>();
            jC_Analysistemplateconfigresponse.Data = jC_AnalysistemplateconfigInfo;
            return jC_Analysistemplateconfigresponse;
        }

        /// <summary>
        /// 根据模板ID查询模板表达式配置关系
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        public BasicResponse<List<JC_AnalysisTemplateConfigInfo>> GetJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            var result = _Repository.GetJC_AnalysistemplateconfigByTempleteId(jC_AnalysisTemplateConfigGetByTempleteIdRequest.TempleteId);
            var jC_AnalysistemplateconfigInfo = ObjectConverter.CopyList<JC_AnalysistemplateconfigModel, JC_AnalysisTemplateConfigInfo>(result);
            var jC_Analysistemplateconfigresponse = new BasicResponse<List<JC_AnalysisTemplateConfigInfo>>();
            jC_Analysistemplateconfigresponse.Data = jC_AnalysistemplateconfigInfo.ToList();
            return jC_Analysistemplateconfigresponse;
        }
        /// <summary>
        /// 根据模板ID删除模板表达式配置关系
        /// </summary>
        /// <param name="TempleteId">模板ID</param>
        /// <returns></returns>
        public BasicResponse DeleteJC_AnalysistemplateconfigByTempleteId(AnalysisTemplateConfigGetByTempleteIdRequest jC_AnalysisTemplateConfigGetByTempleteIdRequest)
        {
            var result = GetJC_AnalysistemplateconfigByTempleteId(jC_AnalysisTemplateConfigGetByTempleteIdRequest).Data;
            var jC_AnalysistemplateconfigInfo = ObjectConverter.CopyList<JC_AnalysisTemplateConfigInfo, JC_AnalysistemplateconfigModel>(result);

            var jC_Analysistemplateconfigresponse = new BasicResponse();
            _Repository.DeleteJC_AnalysistemplateconfigByTempleteId(jC_AnalysisTemplateConfigGetByTempleteIdRequest.TempleteId);
        
            return jC_Analysistemplateconfigresponse;

        }
    }
}


