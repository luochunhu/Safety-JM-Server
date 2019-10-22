using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Analyticalexpression;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class AnalyticalExpressionService : IAnalyticalExpressionService
    {
        private IAnalyticalExpressionRepository _Repository;

        public AnalyticalExpressionService(IAnalyticalExpressionRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_AnalyticalExpressionInfo> AddJC_Analyticalexpression(AnalyticalExpressionAddRequest jC_Analyticalexpressionrequest)
        {
            var _jC_Analyticalexpression = ObjectConverter.Copy<JC_AnalyticalExpressionInfo, JC_AnalyticalexpressionModel>(jC_Analyticalexpressionrequest.JC_AnalyticalExpressionInfo);
            var resultjC_Analyticalexpression = _Repository.AddJC_Analyticalexpression(_jC_Analyticalexpression);
            var jC_Analyticalexpressionresponse = new BasicResponse<JC_AnalyticalExpressionInfo>();
            jC_Analyticalexpressionresponse.Data = ObjectConverter.Copy<JC_AnalyticalexpressionModel, JC_AnalyticalExpressionInfo>(resultjC_Analyticalexpression);
            return jC_Analyticalexpressionresponse;
        }

        /// <summary>
        /// 批量新增表达式
        /// </summary>
        /// <param name="jC_ExpressionconfigModelList"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AnalyticalExpressionInfo>> AddAnalyticalExpressionList(AnalyticalExpressionListAddRequest jC_AnalyticalExpressionrequest)
        {
            var _jC_AnalyticalExpression = ObjectConverter.CopyList<JC_AnalyticalExpressionInfo, JC_AnalyticalexpressionModel>(jC_AnalyticalExpressionrequest.JC_AnalyticalExpressionInfoList);
            var resultjC_AnalyticalExpression = _Repository.AddAnalyticalexpressionList(_jC_AnalyticalExpression);
            var jC_AnalyticalExpressionresponse = new BasicResponse<List<JC_AnalyticalExpressionInfo>>();
            jC_AnalyticalExpressionresponse.Data = ObjectConverter.CopyList<JC_AnalyticalexpressionModel,
                JC_AnalyticalExpressionInfo>(resultjC_AnalyticalExpression).ToList();
            return jC_AnalyticalExpressionresponse;
        }
        /// <summary>
        /// 根据模板ID 查询表达式信息
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns>模型列表</returns>
        public BasicResponse<List<JC_AnalyticalExpressionInfo>> GetAnalysisTemplateListByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            DataTable dataTable = _Repository.QueryTable("global_AnalyticalExpressionService_GetAnalyticalExpressionListByTempleteId", jC_Analysistemplaterequest.TempleteId);

            List<JC_AnalyticalExpressionInfo> listResult = ObjectConverter.Copy<JC_AnalyticalExpressionInfo>(dataTable);

            var jC_AnalyticalExpressionresponse = new BasicResponse<List<JC_AnalyticalExpressionInfo>>();
            jC_AnalyticalExpressionresponse.Data = listResult;


            return jC_AnalyticalExpressionresponse;
        }
        public BasicResponse<JC_AnalyticalExpressionInfo> UpdateJC_Analyticalexpression(AnalyticalExpressionUpdateRequest jC_Analyticalexpressionrequest)
        {
            var _jC_Analyticalexpression = ObjectConverter.Copy<JC_AnalyticalExpressionInfo, JC_AnalyticalexpressionModel>(jC_Analyticalexpressionrequest.JC_AnalyticalExpressionInfo);
            _Repository.UpdateJC_Analyticalexpression(_jC_Analyticalexpression);
            var jC_Analyticalexpressionresponse = new BasicResponse<JC_AnalyticalExpressionInfo>();
            jC_Analyticalexpressionresponse.Data = ObjectConverter.Copy<JC_AnalyticalexpressionModel, JC_AnalyticalExpressionInfo>(_jC_Analyticalexpression);
            return jC_Analyticalexpressionresponse;
        }
        public BasicResponse DeleteJC_Analyticalexpression(AnalyticalExpressionDeleteRequest jC_Analyticalexpressionrequest)
        {
            _Repository.DeleteJC_Analyticalexpression(jC_Analyticalexpressionrequest.Id);
            var jC_Analyticalexpressionresponse = new BasicResponse();
            return jC_Analyticalexpressionresponse;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="jC_Analysistemplaterequest">模型ID</param>
        /// <returns></returns>
        public BasicResponse DeleteJC_AnalyticalexpressionByTempleteId(AnalyticalExpressionGetListRequest jC_Analysistemplaterequest)
        {
            var getAnalysisTemplateListByTempleteIdData = GetAnalysisTemplateListByTempleteId(jC_Analysistemplaterequest).Data;
            var _jC_Analyticalexpression = ObjectConverter.CopyList<JC_AnalyticalExpressionInfo, JC_AnalyticalexpressionModel>(getAnalysisTemplateListByTempleteIdData);
            _Repository.Delete(_jC_Analyticalexpression.ToList());
            var jC_Analyticalexpressionresponse = new BasicResponse();
            return jC_Analyticalexpressionresponse;
        }

        public BasicResponse<List<JC_AnalyticalExpressionInfo>> GetJC_AnalyticalexpressionList(AnalyticalExpressionGetListRequest jC_Analyticalexpressionrequest)
        {
            var jC_Analyticalexpressionresponse = new BasicResponse<List<JC_AnalyticalExpressionInfo>>();
            jC_Analyticalexpressionrequest.PagerInfo.PageIndex = jC_Analyticalexpressionrequest.PagerInfo.PageIndex - 1;
            if (jC_Analyticalexpressionrequest.PagerInfo.PageIndex < 0)
            {
                jC_Analyticalexpressionrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_AnalyticalexpressionModelLists = _Repository.GetJC_AnalyticalexpressionList(jC_Analyticalexpressionrequest.PagerInfo.PageIndex, jC_Analyticalexpressionrequest.PagerInfo.PageSize, out rowcount);
            var jC_AnalyticalexpressionInfoLists = new List<JC_AnalyticalExpressionInfo>();
            foreach (var item in jC_AnalyticalexpressionModelLists)
            {
                var JC_AnalyticalExpressionInfo = ObjectConverter.Copy<JC_AnalyticalexpressionModel, JC_AnalyticalExpressionInfo>(item);
                jC_AnalyticalexpressionInfoLists.Add(JC_AnalyticalExpressionInfo);
            }
            jC_Analyticalexpressionresponse.Data = jC_AnalyticalexpressionInfoLists;
            return jC_Analyticalexpressionresponse;
        }
        public BasicResponse<JC_AnalyticalExpressionInfo> GetJC_AnalyticalexpressionById(AnalyticalExpressionGetRequest jC_Analyticalexpressionrequest)
        {
            var result = _Repository.GetJC_AnalyticalexpressionById(jC_Analyticalexpressionrequest.Id);
            var jC_AnalyticalexpressionInfo = ObjectConverter.Copy<JC_AnalyticalexpressionModel, JC_AnalyticalExpressionInfo>(result);
            var jC_Analyticalexpressionresponse = new BasicResponse<JC_AnalyticalExpressionInfo>();
            jC_Analyticalexpressionresponse.Data = jC_AnalyticalexpressionInfo;
            return jC_Analyticalexpressionresponse;
        }
    }
}


