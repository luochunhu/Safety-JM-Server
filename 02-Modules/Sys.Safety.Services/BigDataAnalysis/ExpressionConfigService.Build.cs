using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Expressionconfig;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class ExpressionConfigService : IExpressionConfigService
    {
        private IExpressionConfigRepository _Repository;

        public ExpressionConfigService(IExpressionConfigRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_ExpressionConfigInfo> AddJC_Expressionconfig(ExpressionConfigAddRequest jC_Expressionconfigrequest)
        {
            var _jC_Expressionconfig = ObjectConverter.Copy<JC_ExpressionConfigInfo, JC_ExpressionconfigModel>(jC_Expressionconfigrequest.JC_ExpressionConfigInfo);
            var resultjC_Expressionconfig = _Repository.AddJC_Expressionconfig(_jC_Expressionconfig);
            var jC_Expressionconfigresponse = new BasicResponse<JC_ExpressionConfigInfo>();
            jC_Expressionconfigresponse.Data = ObjectConverter.Copy<JC_ExpressionconfigModel, JC_ExpressionConfigInfo>(resultjC_Expressionconfig);
            return jC_Expressionconfigresponse;
        }

        /// <summary>
        /// 批量新增分析模型配置
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_ExpressionConfigInfo>> AddExpressionConfigList(ExpressionConfigListAddRequest jC_ExpressionConfigListAddRequest)
        {
            var _jC_ExpressionConfig = ObjectConverter.CopyList<JC_ExpressionConfigInfo, JC_ExpressionconfigModel>(jC_ExpressionConfigListAddRequest.JC_ExpressionConfigInfoList);
            var resultjC_ExpressionConfig = _Repository.AddExpressionconfigList(_jC_ExpressionConfig);
            var jC_ExpressionConfigresponse = new BasicResponse<List<JC_ExpressionConfigInfo>>();
            jC_ExpressionConfigresponse.Data = ObjectConverter.CopyList<JC_ExpressionconfigModel,
                JC_ExpressionConfigInfo>(resultjC_ExpressionConfig).ToList();
            return jC_ExpressionConfigresponse;
        }
        /// <summary>
        /// 根据表达式ID 获取表达式配置信息
        /// </summary>
        /// <param name="ExpressionId"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigListByExpressionId(ExpressionConfigGetByExpressionIdRequest expressionId)
        {
            var resultjC_ExpressionConfig = _Repository.GetJC_ExpressionconfigListByExpressionId(expressionId.expressionId);
            var jC_ExpressionConfigresponse = new BasicResponse<List<JC_ExpressionConfigInfo>>();
            jC_ExpressionConfigresponse.Data = ObjectConverter.CopyList<JC_ExpressionconfigModel,
                JC_ExpressionConfigInfo>(resultjC_ExpressionConfig).ToList();
            return jC_ExpressionConfigresponse;
        }
        public BasicResponse<JC_ExpressionConfigInfo> UpdateJC_Expressionconfig(ExpressionConfigUpdateRequest jC_Expressionconfigrequest)
        {
            var _jC_Expressionconfig = ObjectConverter.Copy<JC_ExpressionConfigInfo, JC_ExpressionconfigModel>(jC_Expressionconfigrequest.JC_ExpressionConfigInfo);
            _Repository.UpdateJC_Expressionconfig(_jC_Expressionconfig);
            var jC_Expressionconfigresponse = new BasicResponse<JC_ExpressionConfigInfo>();
            jC_Expressionconfigresponse.Data = ObjectConverter.Copy<JC_ExpressionconfigModel, JC_ExpressionConfigInfo>(_jC_Expressionconfig);
            return jC_Expressionconfigresponse;
        }
        public BasicResponse DeleteJC_Expressionconfig(ExpressionconfigDeleteRequest jC_Expressionconfigrequest)
        {
            _Repository.DeleteJC_Expressionconfig(jC_Expressionconfigrequest.Id);
            var jC_Expressionconfigresponse = new BasicResponse();
            return jC_Expressionconfigresponse;
        }
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetJC_ExpressionconfigList(ExpressionConfigGetListRequest jC_Expressionconfigrequest)
        {
            var jC_Expressionconfigresponse = new BasicResponse<List<JC_ExpressionConfigInfo>>();
            jC_Expressionconfigrequest.PagerInfo.PageIndex = jC_Expressionconfigrequest.PagerInfo.PageIndex - 1;
            if (jC_Expressionconfigrequest.PagerInfo.PageIndex < 0)
            {
                jC_Expressionconfigrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_ExpressionconfigModelLists = _Repository.GetJC_ExpressionconfigList(jC_Expressionconfigrequest.PagerInfo.PageIndex, jC_Expressionconfigrequest.PagerInfo.PageSize, out rowcount);
            var jC_ExpressionconfigInfoLists = new List<JC_ExpressionConfigInfo>();
            foreach (var item in jC_ExpressionconfigModelLists)
            {
                var JC_ExpressionConfigInfo = ObjectConverter.Copy<JC_ExpressionconfigModel, JC_ExpressionConfigInfo>(item);
                jC_ExpressionconfigInfoLists.Add(JC_ExpressionConfigInfo);
            }
            jC_Expressionconfigresponse.Data = jC_ExpressionconfigInfoLists;
            return jC_Expressionconfigresponse;
        }
        public BasicResponse<JC_ExpressionConfigInfo> GetJC_ExpressionconfigById(ExpressionConfigGetRequest jC_Expressionconfigrequest)
        {
            var result = _Repository.GetJC_ExpressionconfigById(jC_Expressionconfigrequest.Id);
            var jC_ExpressionconfigInfo = ObjectConverter.Copy<JC_ExpressionconfigModel, JC_ExpressionConfigInfo>(result);
            var jC_Expressionconfigresponse = new BasicResponse<JC_ExpressionConfigInfo>();
            jC_Expressionconfigresponse.Data = jC_ExpressionconfigInfo;
            return jC_Expressionconfigresponse;
        }

        /// <summary>
        /// 根据模板ID 查询表达式配置信息
        /// </summary>
        /// <param name="jC_ExpressionConfigrequest">模型ID</param>
        /// <returns>模型列表</returns>
        public BasicResponse<List<JC_ExpressionConfigInfo>> GetExpressionConfigListByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest)
        {
            DataTable dataTable = _Repository.QueryTable("global_JC_ExpressionConfigService_GetJC_ExpressionConfigListByTempleteId", jC_Analysistemplaterequest.TempleteId);

            List<JC_ExpressionConfigInfo> listResult = ObjectConverter.Copy<JC_ExpressionConfigInfo>(dataTable);

            var jC_AnalyticalExpressionresponse = new BasicResponse<List<JC_ExpressionConfigInfo>>();
            jC_AnalyticalExpressionresponse.Data = listResult;


            return jC_AnalyticalExpressionresponse;
        }
        /// <summary>
        /// 根据模板ID 删除表达式配置信息
        /// </summary>
        /// <param name="jC_ExpressionConfigrequest">模板ID</param>
        /// <returns></returns>
        public BasicResponse DeleteJC_ExpressionconfigByTempleteId(ExpressionConfigGetListRequest jC_Analysistemplaterequest)
        {
            //DataTable dataTable = _Repository.QueryTable("global_JC_ExpressionConfigService_GetJC_ExpressionConfigListByTempleteId", jC_Analysistemplaterequest.TempleteId);

            //List<JC_ExpressionConfigInfo> listResult = ObjectConverter.Copy<JC_ExpressionConfigInfo>(dataTable);


            if (jC_Analysistemplaterequest.JC_ExpressionConfigInfoList != null && jC_Analysistemplaterequest.JC_ExpressionConfigInfoList.Count > 0)
            {
                var _jC_Analyticalexpression = ObjectConverter.CopyList<JC_ExpressionConfigInfo, JC_ExpressionconfigModel>(jC_Analysistemplaterequest.JC_ExpressionConfigInfoList);
                _Repository.Delete(_jC_Analyticalexpression);
            }
               
            var jC_Analyticalexpressionresponse = new BasicResponse();
            return jC_Analyticalexpressionresponse;
        }
    }
}


