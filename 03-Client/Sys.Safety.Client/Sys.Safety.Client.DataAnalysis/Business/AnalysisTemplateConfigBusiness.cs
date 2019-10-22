using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Services;
using Sys.Safety.Request.AnalysisTemplate;
using Sys.Safety.Request.AnalysisTemplateConfig;
using Sys.Safety.Request.JC_Analyticalexpression;
using Sys.Safety.Request.JC_Expressionconfig;
using Sys.Safety.Request.JC_Factor;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.Request.JC_Parameter;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class AnalysisTemplateConfigBusiness
    {

        //创建业务服务对象（走webapi远程调用）
        //模板配置
        IAnalysisTemplateConfigService analysisTemplateConfigService = ServiceFactory.Create<IAnalysisTemplateConfigService>();

        //模板
        IAnalysisTemplateService analysisTemplateService = ServiceFactory.Create<IAnalysisTemplateService>();

        //表达式
        IAnalyticalExpressionService analyticalExpressionService = ServiceFactory.Create<IAnalyticalExpressionService>();

        //表达式配置
        IExpressionConfigService expressionconfigService = ServiceFactory.Create<IExpressionConfigService>();

        //参数表
        IParameterService parameterService = ServiceFactory.Create<IParameterService>();

        //因子表
        IFactorService factorService = ServiceFactory.Create<IFactorService>();

        /// <summary>
        /// 大数据分析模型配置表
        /// </summary>
        ILargedataAnalysisConfigService largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();

        /// <summary>
        /// 添加分析模板 dataType : add 新增  edit 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddAnalysisTemplateConfig(AnalysisTemplateBusinessModel data, string dataType)
        {
            try
            {
                string error = "";
                if (dataType == "add")
                {
                    //1.模板
                    AnalysisTemplateAddRequest analysisTemplateRequest = new AnalysisTemplateAddRequest();
                    analysisTemplateRequest.JC_AnalysisTemplateInfo = data.AnalysisTemplateInfo;
                    //调用接口
                    Basic.Framework.Web.BasicResponse<JC_AnalysisTemplateInfo> analysisTemplateResult =
                        analysisTemplateService.AddJC_Analysistemplate(analysisTemplateRequest);

                    if (analysisTemplateResult.IsSuccess == false)
                    {
                        error = analysisTemplateResult.Message;
                        return error;
                    }
                    else
                    {
                        if (analysisTemplateResult.Code == 100)
                            error = "100";
                        else
                        {
                            error = analysisTemplateResult.Message;
                        }
                    }
                    if (error != "100")
                        return error;
                }
                else if (dataType == "edit")
                {
                    //1.检查模板是否被使用，如果模板使用，需要将模型停用才可以修改模型
                    //调用接口
                    Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> largedataAnalysisConfigResult =
                        largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByTempleteId(new LargedataAnalysisConfigGetRequest() { TempleteId = data.AnalysisTemplateInfo.Id });

                    if (largedataAnalysisConfigResult.IsSuccess == false)
                    {
                        return largedataAnalysisConfigResult.Message;
                    }

                    if (largedataAnalysisConfigResult.Data != null && largedataAnalysisConfigResult.Data.Count > 0)
                    {
                        return "此模板已被使用，不能修改。";
                    }
                    //更新分析模板
                    //analysisTemplateService.UpdateJC_Analysistemplate(new AnalysisTemplateUpdateRequest() { JC_AnalysisTemplateInfo = data.AnalysisTemplateInfo });
                    //1.删除表达式 
                    //根据模板ID 查询表达式信息
                    var JC_ExpressionConfigInfoList = expressionconfigService.GetExpressionConfigListByTempleteId(
                       new ExpressionConfigGetListRequest() { TempleteId = data.AnalysisTemplateInfo.Id }
                        ).Data;

                    //global_AnalyticalExpressionService_GetAnalyticalExpressionListByTempleteId
                    analyticalExpressionService.DeleteJC_AnalyticalexpressionByTempleteId(
                        new AnalyticalExpressionGetListRequest() { TempleteId = data.AnalysisTemplateInfo.Id }
                        );
                    //2.删除分析模板配置表
                    analysisTemplateConfigService.DeleteJC_AnalysistemplateconfigByTempleteId(
                      new AnalysisTemplateConfigGetByTempleteIdRequest() { TempleteId = data.AnalysisTemplateInfo.Id }
                      );
                    //3.删除表达式配置表
                    expressionconfigService.DeleteJC_ExpressionconfigByTempleteId(
                     new ExpressionConfigGetListRequest() { JC_ExpressionConfigInfoList = JC_ExpressionConfigInfoList }
                      );
                }

                //2.表达式
                AnalyticalExpressionListAddRequest analyticalExpressionAddRequestRequest = new AnalyticalExpressionListAddRequest();
                analyticalExpressionAddRequestRequest.JC_AnalyticalExpressionInfoList = data.AnalysisExpressionInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_AnalyticalExpressionInfo>> analysisExpressionResult = analyticalExpressionService.AddAnalyticalExpressionList(analyticalExpressionAddRequestRequest);

                if (analysisExpressionResult.IsSuccess == false)
                {
                    error = analysisExpressionResult.Message;
                }
                else
                {
                    if (analysisExpressionResult.Code == 100)
                        error = "100";
                    else
                    {
                        error = analysisExpressionResult.Message;
                    }
                }

                //3.分析模板配置表
                AnalysisTemplateConfigListAddRequest analysisTemplateConfigListAddRequest = new AnalysisTemplateConfigListAddRequest();
                analysisTemplateConfigListAddRequest.JC_AnalysisTemplateConfigInfoList = data.JC_AnalysisTemplateConfigInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateConfigInfo>> analysisTemplateConfigResult =
                    analysisTemplateConfigService.AddAnalysistemplateconfigList(analysisTemplateConfigListAddRequest);

                if (analysisTemplateConfigResult.IsSuccess == false)
                {
                    error = analysisTemplateConfigResult.Message;
                }
                else
                {
                    if (analysisTemplateConfigResult.Code == 100)
                        error = "100";
                    else
                    {
                        error = analysisTemplateConfigResult.Message;
                    }
                }
                //4.表达式配置表
                ExpressionConfigListAddRequest expressionConfigListAddRequest = new ExpressionConfigListAddRequest();
                expressionConfigListAddRequest.JC_ExpressionConfigInfoList = data.ExpressionConfigInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_ExpressionConfigInfo>> expressionConfigResult =
                    expressionconfigService.AddExpressionConfigList(expressionConfigListAddRequest);

                if (expressionConfigResult.IsSuccess == false)
                {
                    error = expressionConfigResult.Message;
                }
                else
                {

                    if (expressionConfigResult.Code == 100)
                        error = "100";
                    else
                    {
                        error = expressionConfigResult.Message;
                    }
                }

                return error;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return ex.Message;
            }

        }
        /// <summary>
        /// 初始化分析模板信息
        /// </summary>
        /// <returns></returns>
        public AnalysisTemplateBusinessModel LoadAnalysisTemplate(string templateId)
        {
            AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();

            //初始化参数列表
            analysisTemplateBusinessModel.ParameterInfoList = parameterService.GetJC_ParameterList
                (new ParameterGetListRequest()).Data;

            //初始化因子列表
            analysisTemplateBusinessModel.FactorInfoList = factorService.GetJC_FactorList
              (new FactorGetListRequest()).Data;

            //初始化模板信息
            analysisTemplateBusinessModel.AnalysisTemplateInfo = analysisTemplateService.GetJC_AnalysistemplateByTempleteId(new AnalysisTemplateGetRequest() { TempleteId = templateId }).Data;

            //初始化表达式配置
            analysisTemplateBusinessModel.ExpressionConfigInfoList = expressionconfigService.GetExpressionConfigListByTempleteId(new ExpressionConfigGetListRequest() { TempleteId = templateId }).Data;
            //表达式信息
            analysisTemplateBusinessModel.AnalysisExpressionInfoList = analyticalExpressionService.GetAnalysisTemplateListByTempleteId(new AnalyticalExpressionGetListRequest() { TempleteId = templateId }).Data;

            return analysisTemplateBusinessModel;
        }

        /// <summary>
        /// 删除分析模板
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns></returns>
        public string DeleteAnalysisTemplateConfig( List<delAnalysisTemplateModel> ids)
        {
            try
            {
                //验证模板是否被使用
                //分析模型配置表
                //JC_LargedataAnalysisConfig
                //调用接口
                StringBuilder returnSbError = new StringBuilder();
                foreach (var item in ids)
                {
                    Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> largedataAnalysisConfigResult =
                    largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByTempleteId(new LargedataAnalysisConfigGetRequest() { TempleteId = item.id });

                    if (largedataAnalysisConfigResult.IsSuccess == true)
                    {
                        if (largedataAnalysisConfigResult.Data != null && largedataAnalysisConfigResult.Data.Count > 0)
                        {
                            returnSbError.Append("[");
                            returnSbError.Append(item.name);
                            returnSbError.Append("]");
                        }

                    }
                    else
                    {
                        return largedataAnalysisConfigResult.Message;
                    }
                }
                if (!string.IsNullOrWhiteSpace(returnSbError.ToString()))
                {
                    return "模板" + returnSbError.ToString() + "已被使用，不能删除。";
                }
                List<string> lstID = new List<string>();
                foreach (var item in ids)
                {
                    lstID.Add(item.id);
                }


                //1.模板
                AnalysisTemplateDeleteRequest analysisTemplateDeleteRequest = new AnalysisTemplateDeleteRequest();
                analysisTemplateDeleteRequest.Ids = lstID;
                //调用接口
                Basic.Framework.Web.BasicResponse analysisTemplateResult =
                    analysisTemplateService.DeleteJC_Analysistemplate(analysisTemplateDeleteRequest);

                if (analysisTemplateResult.IsSuccess == false)
                {
                    return analysisTemplateResult.Message;
                }
                else
                {
                    if (analysisTemplateResult.Code != 100)
                        return analysisTemplateResult.Message;
                    else
                        return "100";
                }
               
            }
            catch (Exception ex)
            {
                return "-100";
            }

        }


        /// <summary>
        /// 修改分析模板
        /// </summary>
        /// <param name="id">模板ID</param>
        ///  <param name="data"></param>
        /// <returns></returns>
        public string UpdateAnalysisTemplateConfig(string id, AnalysisTemplateBusinessModel data)
        {
            try
            {
                //1.检查模板是否被使用，如果模板使用，需要将模型停用才可以修改模型
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateConfigInfo>> analysisTemplateResult =
                 analysisTemplateConfigService.GetJC_AnalysistemplateconfigByTempleteId(
                 new AnalysisTemplateConfigGetByTempleteIdRequest() { TempleteId = id });

                if (analysisTemplateResult.IsSuccess == false)
                {
                    return analysisTemplateResult.Message;
                }

                if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
                {
                    return "此模板已被使用,不能对模型进行修改操作。";
                }

                //2.更新模板信息
                //2.1 删除模板关系，重新建立模板关系
                string error = "";

                //1.模板

                //2.表达式
                AnalyticalExpressionListAddRequest analyticalExpressionAddRequestRequest = new AnalyticalExpressionListAddRequest();
                analyticalExpressionAddRequestRequest.JC_AnalyticalExpressionInfoList = data.AnalysisExpressionInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_AnalyticalExpressionInfo>> analysisExpressionResult = analyticalExpressionService.AddAnalyticalExpressionList(analyticalExpressionAddRequestRequest);

                if (analysisTemplateResult.IsSuccess == false)
                {
                    error = analysisTemplateResult.Message;
                }
                else
                {
                    error = "100";
                }


                //3.分析模板配置表
                AnalysisTemplateConfigListAddRequest analysisTemplateConfigListAddRequest = new AnalysisTemplateConfigListAddRequest();
                analysisTemplateConfigListAddRequest.JC_AnalysisTemplateConfigInfoList = data.JC_AnalysisTemplateConfigInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateConfigInfo>> analysisTemplateConfigResult =
                    analysisTemplateConfigService.AddAnalysistemplateconfigList(analysisTemplateConfigListAddRequest);

                if (analysisTemplateConfigResult.IsSuccess == false)
                {
                    error = analysisTemplateResult.Message;
                }
                else
                {
                    error = "100";
                }
                //4.表达式配置表
                ExpressionConfigListAddRequest expressionConfigListAddRequest = new ExpressionConfigListAddRequest();
                expressionConfigListAddRequest.JC_ExpressionConfigInfoList = data.ExpressionConfigInfoList;
                //调用接口
                Basic.Framework.Web.BasicResponse<List<JC_ExpressionConfigInfo>> expressionConfigResult =
                    expressionconfigService.AddExpressionConfigList(expressionConfigListAddRequest);

                if (expressionConfigResult.IsSuccess == false)
                {
                    error = expressionConfigResult.Message;
                }
                else
                {
                    error = "100";
                }

                return error;

            }
            catch (Exception ex)
            {
                return "更新失败";
            }

        }

        /// <summary>
        /// 初始化大数据分析模型名称列表
        /// </summary>
        /// <returns></returns>
        public AnalysisTemplateBusinessModel GetAnalysisTemplateList()
        {
            AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();

            //初始化大数据分析模型名称
            AnalysisTemplateGetListRequest analysisTemplateGetListRequest = new AnalysisTemplateGetListRequest();
            analysisTemplateGetListRequest.PagerInfo = new PagerInfo()
            {
                PageSize = 1,
                PageIndex = 5000
            };
            //调用接口
            Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateInfo>> analysisTemplateResult =
                analysisTemplateService.GetJC_AnalysistemplateList(analysisTemplateGetListRequest);

            if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = analysisTemplateResult.Data.ToList();
            }
            else
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = new List<JC_AnalysisTemplateInfo>();
            }
            if (analysisTemplateBusinessModel.AnalysisTemplateInfoList.Count > 0)
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = analysisTemplateBusinessModel.AnalysisTemplateInfoList.OrderBy(o => o.Name).ToList();

            return analysisTemplateBusinessModel;
        }

        /// <summary>
        /// 模板名称列表
        /// </summary>
        /// <returns></returns>
        public AnalysisTemplateBusinessModel GetAnalysisTemplateListDetail(string tmplateId)
        {
            AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();


            //调用接口
            Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateInfo>> analysisTemplateResult =
                analysisTemplateService.GetAnalysisTemplateListDetail(new AnalysisTemplateGetRequest() { Id = tmplateId });

            if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = analysisTemplateResult.Data.ToList();
            }
            else
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = new List<JC_AnalysisTemplateInfo>();
            }


            return analysisTemplateBusinessModel;
        }
        /// <summary>
        /// 根据模板ID获取表达式信息
        /// </summary>
        /// <returns></returns>
        public AnalysisTemplateBusinessModel GetAnalyticalExpressionInfoListByTempleteId(string tmplateId)
        {
            AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();


            //调用接口
            Basic.Framework.Web.BasicResponse<List<JC_AnalyticalExpressionInfo>> analysisTemplateResult =
                analyticalExpressionService.GetAnalysisTemplateListByTempleteId(new AnalyticalExpressionGetListRequest() { TempleteId = tmplateId });

            if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
            {
                analysisTemplateBusinessModel.AnalysisExpressionInfoList = analysisTemplateResult.Data.ToList();
            }
            else
            {
                analysisTemplateBusinessModel.AnalysisExpressionInfoList = new List<JC_AnalyticalExpressionInfo>();
            }


            return analysisTemplateBusinessModel;
        }


        /// <summary>
        ///根据模板名称模糊查询模板列表 pageSize=0查询所有数据
        /// </summary>
        /// <returns></returns>
        public AnalysisTemplateBusinessModel GetAnalysisTemplateListByTmplateName(string tmplateName, int pageIndex = 1, int pageSize = 0)
        {
            AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();
            AnalysisTemplateGetListByNameRequest analysisTemplateGetListRequest = new AnalysisTemplateGetListByNameRequest();
            analysisTemplateGetListRequest.Name = tmplateName;
            analysisTemplateGetListRequest.PagerInfo = new PagerInfo()
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            //调用接口
            Basic.Framework.Web.BasicResponse<List<JC_AnalysisTemplateInfo>> analysisTemplateResult =
                analysisTemplateService.GetJC_AnalysistemplateListByName(analysisTemplateGetListRequest);

            if (analysisTemplateResult.Data != null && analysisTemplateResult.Data.Count > 0)
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = analysisTemplateResult.Data.ToList();
            }
            else
            {
                analysisTemplateBusinessModel.AnalysisTemplateInfoList = new List<JC_AnalysisTemplateInfo>();
            }
            analysisTemplateBusinessModel.pagerInfo = analysisTemplateResult.PagerInfo;



            return analysisTemplateBusinessModel;
        }
        /// <summary>
        /// 检查模版是否被使用
        /// </summary>
        /// <returns></returns>
        public bool checkTemplateUsed(string id)
        {
            //1.检查模板是否被使用，如果模板使用，需要将模型停用才可以修改模型
            //调用接口
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> largedataAnalysisConfigResult = largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByTempleteId(
                   new LargedataAnalysisConfigGetRequest() { TempleteId = id }
                );


            if (largedataAnalysisConfigResult.IsSuccess == false)
            {
                return false;
            }

            if (largedataAnalysisConfigResult.Data != null && largedataAnalysisConfigResult.Data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
    /// <summary>
    /// 模板删除model
    /// </summary>
    public class delAnalysisTemplateModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
