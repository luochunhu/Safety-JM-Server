using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Data;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Processing.DataAnalysis;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using System.Data;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.ManualCrossControl;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 分析配置服务.
    /// </summary>
    public partial class LargedataAnalysisConfigService : ILargedataAnalysisConfigService
    {
        private ILargedataAnalysisConfigRepository _Repository;
        private IAnalysisTemplateRepository _AnalysisTemplateRepository;
        private ISetAnalysisModelPointRecordRepository _AnalysisModelPointRecordRepository;
        private ISetAnalysisModelPointRecordService _AnalysisModelPointRecordService;
        private ILargeDataAnalysisConfigCacheService _LargeDataAnalysisConfigCacheService;

        private IAlarmNotificationPersonnelConfigService _AlarmNotificationPersonnelConfigService;
        private IRegionOutageConfigService _RegionOutageConfigService;
        private IPointDefineCacheService pointDefineCacheService;

        private IAlarmHandleService alarmHandleService;
        /// <summary>
        /// 手动控制服务
        /// </summary>
        private IManualCrossControlService manualCrossControlService;

        public LargedataAnalysisConfigService(ILargedataAnalysisConfigRepository _Repository,
            ISetAnalysisModelPointRecordRepository _AnalysisModelPointRecordRepository,
            IAnalysisTemplateRepository _AnalysisTemplateRepository,
            ISetAnalysisModelPointRecordService _AnalysisModelPointRecordService,
            ILargeDataAnalysisConfigCacheService _LargeDataAnalysisConfigCacheService,
            IAlarmNotificationPersonnelConfigService _AlarmNotificationPersonnelConfigService,
            IRegionOutageConfigService _RegionOutageConfigService,
            IPointDefineCacheService pointDefineCacheService,
            IAlarmHandleService alarmHandleService,
            IManualCrossControlService manualCrossControlService)
        {
            this._Repository = _Repository;
            this._AnalysisModelPointRecordRepository = _AnalysisModelPointRecordRepository;
            this._AnalysisTemplateRepository = _AnalysisTemplateRepository;
            this._AnalysisModelPointRecordService = _AnalysisModelPointRecordService;
            this._LargeDataAnalysisConfigCacheService = _LargeDataAnalysisConfigCacheService;
            this._AlarmNotificationPersonnelConfigService = _AlarmNotificationPersonnelConfigService;
            this._RegionOutageConfigService = _RegionOutageConfigService;
            this.pointDefineCacheService = pointDefineCacheService;
            this.alarmHandleService = alarmHandleService;
            this.manualCrossControlService = manualCrossControlService;
        }


        /// <summary>
        /// 验证分析模型
        /// </summary>
        /// <param name="analysisConfig">分析模型配置信息</param>
        /// <returns>Mas应答对象</returns>
        private BasicResponse largeDataAnalysisConfigValidation(JC_LargedataAnalysisConfigInfo analysisConfig)
        {
            if (analysisConfig == null)
                return new BasicResponse() { Code = 1, Message = "分析模型没有初始化" };
            if (string.IsNullOrEmpty(analysisConfig.TempleteId))
                return new BasicResponse() { Code = 1, Message = "没有指定分析模板Id" };
            List<JC_SetAnalysisModelPointRecordInfo> analysisModelPointList = analysisConfig.AnalysisModelPointRecordInfoList;
            if (analysisModelPointList == null || analysisModelPointList.Count == 0)
                return new BasicResponse() { Code = 1, Message = "没有为表达式配置测点或指定设备类型" };
            if (tmpl == null || validateTemplateId != analysisConfig.TempleteId)
            {
                tmpl = _AnalysisModelPointRecordRepository.GetAnalysisModelPointBindingTemplateByTempleteId(analysisConfig.TempleteId);
                validateTemplateId = analysisConfig.TempleteId;
            }
            if (tmpl == null || tmpl.Count == 0)
                return new BasicResponse() { Code = 1, Message = "模板不存在或未配置模板" };
            if (tmpl.Count > 1 && analysisModelPointList.Count != tmpl.Count)//排除只有一个表达式一个参数的验证.
            {
                return new BasicResponse() { Code = 1, Message = "参数个数不匹配" };
            }

            BasicResponse response = new BasicResponse();
            foreach (var item in analysisModelPointList)
            {
                if (string.IsNullOrEmpty(item.PointId) && item.DevTypeId == -1)
                {
                    response.Code = 1; response.Message = "表达式参数配置不完整.";
                    break;
                }
                JC_SetanalysismodelpointrecordModel findModel = tmpl.FirstOrDefault(x => x.ExpressionId == item.ExpressionId && x.ParameterId == item.ParameterId && x.FactorId == item.FactorId);
                if (findModel == null)
                {
                    response.Code = 1; response.Message = "表达式,参数,因子. 不匹配.";
                    break;
                }
            }

            return response;
        }

        /// <summary>
        /// 分析模板是否是单一表达式单一参数
        /// </summary>
        /// <param name="templeteId">分析模板Id</param>
        /// <returns>分析模板是否是单一表达式单一参数</returns>
        private bool isSingleExpressionAndParameter(string templeteId)
        {
            if (tmpl == null || validateTemplateId != templeteId)
            {
                tmpl = _AnalysisModelPointRecordRepository.GetAnalysisModelPointBindingTemplateByTempleteId(templeteId);
                validateTemplateId = templeteId;
            }
            //bool isSingleExpresstion = tmpl.GroupBy(g => g.ExpressionId).ToList().Count == 1;
            bool isSingleParameter = tmpl.GroupBy(g => g.ParameterId).ToList().Count == 1;
            //if (isSingleExpresstion && isSingleParameter)
            //    return true;
            if (isSingleParameter)
                return true;
            return false;
        }

        IList<JC_SetanalysismodelpointrecordModel> tmpl = null;
        string validateTemplateId = string.Empty;
        /// <summary>
        /// 添加分析模型，及其关联的测点。
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRsequest">分析模型的添加请求</param>
        /// <returns>Mas应答对象包含分析模型</returns>
        public BasicResponse<JC_LargedataAnalysisConfigInfo> AddLargeDataAnalysisConfig(LargedataAnalysisConfigAddRequest jc_LargedataAnalysisConfigRsequest)
        {
            BasicResponse validationResponse = largeDataAnalysisConfigValidation(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);
            if (!validationResponse.IsSuccess)
                return new BasicResponse<JC_LargedataAnalysisConfigInfo>() { Code = validationResponse.Code, Message = validationResponse.Message, Data = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo };
            var jC_Largedataanalysisconfigresponse = new BasicResponse<JC_LargedataAnalysisConfigInfo>();
            var _jC_Largedataanalysisconfig = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo, JC_LargedataanalysisconfigModel>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);
            var _jcSetAnalysisModelPointRecordList = ObjectConverter.CopyList<JC_SetAnalysisModelPointRecordInfo, JC_SetanalysismodelpointrecordModel>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList);

            bool isSingleExpressionAndParameter1 = isSingleExpressionAndParameter(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.TempleteId);
            bool isBindDevType = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList[0].DevTypeId > -1;


            if (isSingleExpressionAndParameter1 && !isBindDevType /*&& jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList.Count > 1*/)
            {
                List<JC_SetAnalysisModelPointRecordInfo> listOfPoint = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList;
                string[] pointIds = listOfPoint[0].PointId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (pointIds.Length > 1)
                {
                    foreach (var pointId in pointIds)
                    {
                        List<JC_SetAnalysisModelPointRecordInfo> toAddPointList = new List<JC_SetAnalysisModelPointRecordInfo>();
                        foreach (var oriPoint in listOfPoint)
                        {
                            JC_SetAnalysisModelPointRecordInfo addPoint = ObjectConverter.DeepCopy(oriPoint);
                            addPoint.PointId = pointId;
                            BasicResponse<Jc_DefInfo> pointDefine = pointDefineCacheService.PointDefineCacheByPointIdRequeest(new Sys.Safety.Request.Cache.PointDefineCacheByPointIdRequeest() { PointID = pointId });
                            if (pointDefine.IsSuccess && pointDefine.Data != null)
                                addPoint.Point = pointDefine.Data.Point;
                            toAddPointList.Add(addPoint);
                        }

                        //JC_LargedataAnalysisConfigInfo largedataAnalysisConfigInfo = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo, JC_LargedataAnalysisConfigInfo>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);
                        JC_LargedataAnalysisConfigInfo largedataAnalysisConfigInfo = ObjectConverter.DeepCopy<JC_LargedataAnalysisConfigInfo>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);

                        largedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList = toAddPointList;
                        jC_Largedataanalysisconfigresponse = AddLargeDataAnalysisConfig(new LargedataAnalysisConfigAddRequest() { JC_LargedataAnalysisConfigInfo = largedataAnalysisConfigInfo });

                    }
                    return jC_Largedataanalysisconfigresponse;
                }
            }

            //给模型赋默认值 Start
            _jC_Largedataanalysisconfig.Id = IdHelper.CreateLongId().ToString();
            StringBuilder points = new StringBuilder();
            foreach (var jcSetAnalysisModelPointRecord in _jcSetAnalysisModelPointRecordList)
            {
                jcSetAnalysisModelPointRecord.Id = IdHelper.CreateLongId().ToString();
                jcSetAnalysisModelPointRecord.AnalysisModelId = _jC_Largedataanalysisconfig.Id;

                JC_SetAnalysisModelPointRecordInfo analysisModelPointRecordInfo = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList.FirstOrDefault(q => q.PointId == jcSetAnalysisModelPointRecord.PointId);
                if (string.IsNullOrEmpty(points.ToString()))
                    points.Append(analysisModelPointRecordInfo.Point);
                //points.Append(analysisModelPointRecordInfo.Point).Append(",");
            }
            if (string.IsNullOrEmpty(_jC_Largedataanalysisconfig.Name))
            {
                _jC_Largedataanalysisconfig.Name = _AnalysisTemplateRepository.GetJC_AnalysistemplateById(_jC_Largedataanalysisconfig.TempleteId).Name;
            }
            if (isSingleExpressionAndParameter1 && !isBindDevType)
            {
                _jC_Largedataanalysisconfig.Name = string.Format("{0}({1})", _jC_Largedataanalysisconfig.Name, points.ToString().Trim(','));
            }
            //给模型赋默认值 End

            JC_LargedataanalysisconfigModel existsModel = _Repository.GetLargeDataAnalysisConfigByName(_jC_Largedataanalysisconfig.Name);
            if (null != existsModel)
                return new BasicResponse<JC_LargedataAnalysisConfigInfo>() { Code = -100, Message = "分析模型名称已存在", Data = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo };

            JC_LargedataanalysisconfigModel resultjC_Largedataanalysisconfig = null;
            TransactionsManager.BeginTransaction(() =>
            {
                //保存分析模型
                resultjC_Largedataanalysisconfig = _Repository.AddJC_Largedataanalysisconfig(_jC_Largedataanalysisconfig);
                //保持分析模型中表达式参数与测点的关联关系.
                _AnalysisModelPointRecordRepository.Insert(_jcSetAnalysisModelPointRecordList);
            });

            if (resultjC_Largedataanalysisconfig != null)
            {
                jC_Largedataanalysisconfigresponse.Data = ObjectConverter.Copy<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(resultjC_Largedataanalysisconfig);
                BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> pointListResponse = _AnalysisModelPointRecordService.GetCustomizationAnalysisModelPointRecordInfoByModelId(new SetAnalysisModelPointRecordByModelIdGetRequest() { AnalysisModelId = resultjC_Largedataanalysisconfig.Id });
                if (pointListResponse != null && pointListResponse.Data != null)
                    jC_Largedataanalysisconfigresponse.Data.AnalysisModelPointRecordInfoList = pointListResponse.Data.ToList();
                try
                {
                    //添加到缓存
                    _LargeDataAnalysisConfigCacheService.AddLargeDataAnalysisConfigCache(new LargeDataAnalysisConfigCacheAddRequest() { LargeDataAnalysisConfigInfo = jC_Largedataanalysisconfigresponse.Data });
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(string.Format("添加分析模型后再添加到缓存出错:{0}", ex.StackTrace));
                }

                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey))
                {
                    Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey] = DateTime.Now;
                }
                else
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey, DateTime.Now);
                }
            }
            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        /// 更新分析模型。
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRsequest">更新模型请求对象</param>
        /// <returns>Mas应答对象包含分析模型</returns>
        public BasicResponse<JC_LargedataAnalysisConfigInfo> UpdateLargeDataAnalysisConfig(LargedataAnalysisConfigUpdateRequest jc_LargedataAnalysisConfigRsequest)
        {
            BasicResponse validationResponse = largeDataAnalysisConfigValidation(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);
            if (!validationResponse.IsSuccess)
                return new BasicResponse<JC_LargedataAnalysisConfigInfo>() { Code = validationResponse.Code, Message = validationResponse.Message, Data = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo };
            if (string.IsNullOrEmpty(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.Id))
            {
                return new BasicResponse<JC_LargedataAnalysisConfigInfo>() { Code = 1, Message = "分析模型ID为空", Data = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo };
            }
            var _jC_Largedataanalysisconfig = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo, JC_LargedataanalysisconfigModel>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo);
            var _jcSetAnalysisModelPointRecordList = ObjectConverter.CopyList<JC_SetAnalysisModelPointRecordInfo, JC_SetanalysismodelpointrecordModel>(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList);

            bool isSingleExpressionAndParameter1 = isSingleExpressionAndParameter(jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.TempleteId);
            bool isBindDevType = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList[0].DevTypeId > -1;
            if (isSingleExpressionAndParameter1 && _jcSetAnalysisModelPointRecordList.Count > 0 && !isBindDevType)
                _jC_Largedataanalysisconfig.Name = System.Text.RegularExpressions.Regex.Replace(_jC_Largedataanalysisconfig.Name, "\\(\\w+\\)", string.Format("({0})", jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList[0].Point));


            JC_LargedataanalysisconfigModel existsModel = _Repository.GetLargeDataAnalysisConfigByName(_jC_Largedataanalysisconfig.Name);
            if (null != existsModel && existsModel.Id != _jC_Largedataanalysisconfig.Id)
                return new BasicResponse<JC_LargedataAnalysisConfigInfo>() { Code = -100, Message = "分析模型名称已存在", Data = jc_LargedataAnalysisConfigRsequest.JC_LargedataAnalysisConfigInfo };


            TransactionsManager.BeginTransaction(() =>
            {
                //更新分析模型
                //_jC_Largedataanalysisconfig.UpdatedTime = DateTime.Now;
                //更新时间不变化，这里将分析模型名称改了再改回来。
                string analysisModelName = _jC_Largedataanalysisconfig.Name;
                _jC_Largedataanalysisconfig.Name = analysisModelName + "1";
                _Repository.UpdateJC_Largedataanalysisconfig(_jC_Largedataanalysisconfig);
                _jC_Largedataanalysisconfig.Name = analysisModelName;
                _Repository.UpdateJC_Largedataanalysisconfig(_jC_Largedataanalysisconfig);
                //先删除再插入
                _AnalysisModelPointRecordRepository.DeleteAnalysisModelPointRecordByAnalysisModelId(_jC_Largedataanalysisconfig.Id);
                foreach (var item in _jcSetAnalysisModelPointRecordList)
                {
                    //item.Id = IdHelper.CreateGuidId();
                    item.Id = IdHelper.CreateLongId().ToString();
                    item.AnalysisModelId = _jC_Largedataanalysisconfig.Id;
                }
                //插入模型测点关联记录
                _AnalysisModelPointRecordRepository.Insert(_jcSetAnalysisModelPointRecordList);
            });
            var jC_Largedataanalysisconfigresponse = new BasicResponse<JC_LargedataAnalysisConfigInfo>();
            jC_Largedataanalysisconfigresponse.Data = ObjectConverter.Copy<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(_jC_Largedataanalysisconfig);
            BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> pointListResponse = _AnalysisModelPointRecordService.GetCustomizationAnalysisModelPointRecordInfoByModelId(new SetAnalysisModelPointRecordByModelIdGetRequest() { AnalysisModelId = _jC_Largedataanalysisconfig.Id });
            if (pointListResponse != null && pointListResponse.Data != null)
                jC_Largedataanalysisconfigresponse.Data.AnalysisModelPointRecordInfoList = pointListResponse.Data.ToList();
            try
            {
                jC_Largedataanalysisconfigresponse.Data.AnalysisResult = 0;
                jC_Largedataanalysisconfigresponse.Data.AnalysisTime = null;

                //更新实时显示到缓存，这样客户端刷新就可以看到。不用等到分析周期. 开始
                jC_Largedataanalysisconfigresponse.Data.ExpressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();
                if (jC_Largedataanalysisconfigresponse.Data.AnalysisModelPointRecordInfoList != null && jC_Largedataanalysisconfigresponse.Data.AnalysisModelPointRecordInfoList.Count > 0)
                {
                    IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> expressionGroup = jC_Largedataanalysisconfigresponse.Data.AnalysisModelPointRecordInfoList.GroupBy(p => p.ExpressionId);
                    foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expressionParameterList in expressionGroup)
                    {
                        if (expressionParameterList.FirstOrDefault().DevTypeId == -1)//绑定测点的情况才更新实时显示的表达式
                        {
                            ExpressionRealTimeResultInfo addInitializeResultInfo = new ExpressionRealTimeResultInfo();
                            addInitializeResultInfo.AnalysisModelId = jC_Largedataanalysisconfigresponse.Data.Id;
                            addInitializeResultInfo.AnalysisModelName = jC_Largedataanalysisconfigresponse.Data.Name;
                            addInitializeResultInfo.ExpressionId = expressionParameterList.FirstOrDefault().ExpressionId;
                            addInitializeResultInfo.Expression = expressionParameterList.FirstOrDefault().Expresstion;
                            addInitializeResultInfo.ExpressionText = expressionParameterList.FirstOrDefault().Expresstion;
                            addInitializeResultInfo.FirstSuccessfulTime = DateTime.MinValue;
                            addInitializeResultInfo.LastAnalysisTime = DateTime.MinValue;
                            addInitializeResultInfo.AnalysisResult = 0;
                            addInitializeResultInfo.AnalysisResultText = "未知";
                            addInitializeResultInfo.ContinueTime = expressionParameterList.FirstOrDefault().ContinueTime;
                            addInitializeResultInfo.MaxContinueTime = expressionParameterList.FirstOrDefault().MaxContinueTime;
                            addInitializeResultInfo.ActualContinueTime = 0;
                            foreach (var pointMapping in expressionParameterList)
                            {
                                addInitializeResultInfo.ExpressionText = addInitializeResultInfo.ExpressionText.Replace(pointMapping.ExpressionConfigId, string.Format("{0}->{1}", pointMapping.PointId, pointMapping.FactorName));
                            }
                            jC_Largedataanalysisconfigresponse.Data.ExpressionRealTimeResultList.Add(addInitializeResultInfo);
                        }
                    }
                }
                //更新实时显示到缓存，这样客户端刷新就可以看到。不用等到分析周期. 结束

                //更新缓存
                _LargeDataAnalysisConfigCacheService.UpdateLargeDataAnalysisConfigCahce(new LargeDataAnalysisConfigCacheUpdateRequest() { LargeDataAnalysisConfigInfo = jC_Largedataanalysisconfigresponse.Data });
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(string.Format("更新分析模型后再更新缓存出错:{0}", ex.StackTrace));
            }

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey, DateTime.Now);
            }
            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        /// 删除分析模型
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRsequest">删除分析模型的请求对象</param>
        /// <returns>Mas应答对象</returns>
        public BasicResponse DeleteLargeDataAnalysisConfig(LargedataAnalysisConfigDeleteRequest jc_LargedataAnalysisConfigRsequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse();
            try
            {
                //检查, 报警，控制，应急联动配置
                BasicResponse<bool> hasAlarmNotificationResponse = _AlarmNotificationPersonnelConfigService.HasAlarmNotificationForAnalysisModel(new Sys.Safety.Request.AlarmNotificationPersonnelConfig.GetAlarmNotificationByAnalysisModelIdRequest() { AnalysisModelId = jc_LargedataAnalysisConfigRsequest.Id });
                if (hasAlarmNotificationResponse.IsSuccess && hasAlarmNotificationResponse.Data)
                {
                    return new BasicResponse() { Code = -100, Message = "此分析模型存在报警配置信息, 请先删除报警配置然后再删除此模型." };
                }
                BasicResponse<bool> hasRegionOutageResponse = _RegionOutageConfigService.HasRegionOutageForAnalysisModel(new Sys.Safety.Request.RegionOutageConfig.GetByAnalysisModelIdRequest() { AnalysisModelId = jc_LargedataAnalysisConfigRsequest.Id });
                if (hasRegionOutageResponse.IsSuccess && hasRegionOutageResponse.Data)
                {
                    return new BasicResponse() { Code = -100, Message = "此分析模型存在区域断电配置信息, 请先删除区域断电配置然后再删除此模型." };
                }

                //删除分析模型
                _Repository.DeleteJC_Largedataanalysisconfig(jc_LargedataAnalysisConfigRsequest.Id);

                BasicResponse<JC_LargedataAnalysisConfigInfo> cachedLargedataAnalysisConfigInfo = _LargeDataAnalysisConfigCacheService.GetLargeDataAnalysisConfigCacheByKey(new Sys.Safety.Request.Cache.LargeDataAnalysisConfigCacheGetByKeyRequest() { Id = jc_LargedataAnalysisConfigRsequest.Id });
                if (cachedLargedataAnalysisConfigInfo != null && cachedLargedataAnalysisConfigInfo.Data != null)
                {
                    try
                    {
                        //删除分析模型时，关闭和模型有关的未关闭的报警处理信息.
                        alarmHandleService.CloseUnclosedAlarmHandleByAnalysisModelId(new AlarmHandleGetByAnalysisModelIdRequest() { AnalysisModelId = cachedLargedataAnalysisConfigInfo.Data.Id });
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(string.Format("删除分析模型时，关闭和模型有关的未关闭的报警处理信息出错. 错误消息:{0}", ex.StackTrace));
                    }
                    try
                    {
                        //删除分析模型时，解除和模型有关的控制
                        BasicResponse<List<Jc_JcsdkzInfo>> analysisZKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPoint(new ManualCrossControlGetByTypeZkPointRequest() { ZkPoint = cachedLargedataAnalysisConfigInfo.Data.Id, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                        if (analysisZKResponse != null && analysisZKResponse.Data != null && analysisZKResponse.Data.Count > 0)
                            manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisZKResponse.Data });
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(string.Format("删除分析模型时，解除和模型有关的控制出错. 错误消息:{0}", ex.StackTrace));
                    }
                    try
                    {
                        //删除缓存
                        _LargeDataAnalysisConfigCacheService.DeleteLargeDataAnalysisConfigCache(new LargeDataAnalysisConfigCacheDeleteRequest() { LargeDataAnalysisConfigInfo = cachedLargedataAnalysisConfigInfo.Data });
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(string.Format("删除分析模型后再删除缓存出错:{0}", ex.StackTrace));
                    }
                }

                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey))
                {
                    Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey] = DateTime.Now;
                }
                else
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AnalysisModelChangedKey, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                jC_Largedataanalysisconfigresponse.Code = 2;
                jC_Largedataanalysisconfigresponse.Message = ex.Message;
            }

            return jC_Largedataanalysisconfigresponse;
        }
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRsequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            jc_LargedataAnalysisConfigRsequest.PagerInfo.PageIndex = jc_LargedataAnalysisConfigRsequest.PagerInfo.PageIndex - 1;
            if (jc_LargedataAnalysisConfigRsequest.PagerInfo.PageIndex < 0)
            {
                jc_LargedataAnalysisConfigRsequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_LargedataanalysisconfigModelLists = _Repository.GetJC_LargedataanalysisconfigList(jc_LargedataAnalysisConfigRsequest.PagerInfo.PageIndex, jc_LargedataAnalysisConfigRsequest.PagerInfo.PageSize, out rowcount);
            var jC_LargedataanalysisconfigInfoLists = new List<JC_LargedataAnalysisConfigInfo>();
            foreach (var item in jC_LargedataanalysisconfigModelLists)
            {
                var JC_LargedataanalysisconfigInfo = ObjectConverter.Copy<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(item);
                jC_LargedataanalysisconfigInfoLists.Add(JC_LargedataanalysisconfigInfo);
            }
            jC_Largedataanalysisconfigresponse.Data = jC_LargedataanalysisconfigInfoLists;
            return jC_Largedataanalysisconfigresponse;
        }

        public BasicResponse<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRsequest)
        {
            var result = _Repository.GetJC_LargedataanalysisconfigById(jc_LargedataAnalysisConfigRsequest.Id);
            var jC_LargedataanalysisconfigInfo = ObjectConverter.Copy<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(result);
            BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> pointListResponse = _AnalysisModelPointRecordService.GetCustomizationEditAnalysisModelPointRecordInfoByModelId(new SetAnalysisModelPointRecordByModelIdGetRequest() { AnalysisModelId = jC_LargedataanalysisconfigInfo.Id });
            if (pointListResponse.Data != null)
                jC_LargedataanalysisconfigInfo.AnalysisModelPointRecordInfoList = pointListResponse.Data.ToList();
            else
                jC_LargedataanalysisconfigInfo.AnalysisModelPointRecordInfoList = new List<JC_SetAnalysisModelPointRecordInfo>();
            var jC_Largedataanalysisconfigresponse = new BasicResponse<JC_LargedataAnalysisConfigInfo>();
            jC_Largedataanalysisconfigresponse.Data = jC_LargedataanalysisconfigInfo;
            return jC_Largedataanalysisconfigresponse;
        }
        /// <summary>
        /// 根据模型ID查询模型详细信息
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRsequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargedataAnalysisConfigDetailById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRsequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();

            try
            {
                DataTable dataTable = _Repository.QueryTable("global_LargedataAnalysisConfigService_GetLargedataAnalysisConfigDetailById", jc_LargedataAnalysisConfigRsequest.Id);

                List<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo>(dataTable);

                List<JC_SetAnalysisModelPointRecordInfo> setAnalysisModelPointRecordInfoList = new List<JC_SetAnalysisModelPointRecordInfo>();

                if (listResult.Count > 0)
                {
                    setAnalysisModelPointRecordInfoList = _AnalysisModelPointRecordService.GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(
                           new SetAnalysisModelPointRecordByModelIdGetRequest() { AnalysisModelId = jc_LargedataAnalysisConfigRsequest.Id }).Data;

                    foreach (var item in listResult)
                    {
                        foreach (var itemData in setAnalysisModelPointRecordInfoList)
                        {
                            if (item.Id == itemData.LargedataAnalysisConfigId)
                            {
                                item.Expresstion = item.Expresstion.Replace(itemData.ExpressionConfigId, itemData.Point + "->" + itemData.FactorName);
                            }
                        }
                    }
                }

                jC_Largedataanalysisconfigresponse.Data = listResult;
            }
            catch
            {

            }
            return jC_Largedataanalysisconfigresponse;
        }


        /// <summary>
        /// 获取所有分析配置列表
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest">分析配置请求对象</param>
        /// <returns>所有分析配置列表</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            var jC_LargedataanalysisconfigModelLists = _Repository.GetAllLargeDataAnalysisConfigList();
            IList<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(jC_LargedataanalysisconfigModelLists);
            if (listResult != null)
                jC_Largedataanalysisconfigresponse.Data = listResult.ToList();
            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        ///根据模型名称模糊查询模型列表
        /// </summary>
        /// <returns>模型列表</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByName(LargedataAnalysisConfigGetListByNameRequest jc_LargedataAnalysisConfigRequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex = jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex - 1;
            if (jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex < 0)
            {
                jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            rowcount = jc_LargedataAnalysisConfigRequest.PagerInfo.RowCount;
            var jC_LargedataanalysisconfigModelLists = _Repository.GetLargeDataAnalysisConfigListByName(jc_LargedataAnalysisConfigRequest.Name, jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex, jc_LargedataAnalysisConfigRequest.PagerInfo.PageSize, out rowcount);
            IList<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(jC_LargedataanalysisconfigModelLists);
            if (listResult != null)
            {

                jC_Largedataanalysisconfigresponse.Data = listResult.ToList();
                jC_Largedataanalysisconfigresponse.PagerInfo.PageIndex = jc_LargedataAnalysisConfigRequest.PagerInfo.PageIndex;
                jC_Largedataanalysisconfigresponse.PagerInfo.PageSize = jc_LargedataAnalysisConfigRequest.PagerInfo.PageSize;
                jC_Largedataanalysisconfigresponse.PagerInfo.RowCount = rowcount;
            }

            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        /// 根据模板ID查询分析模型配置信息
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByTempleteId(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            var jC_LargedataanalysisconfigModelLists = _Repository.GetLargeDataAnalysisConfigListByTempleteId(jc_LargedataAnalysisConfigRequest.TempleteId);
            IList<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(jC_LargedataanalysisconfigModelLists);
            if (listResult != null)
                jC_Largedataanalysisconfigresponse.Data = listResult.ToList();
            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        /// 获取启用并带有关联测点的分析配置列表
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest">分析配置请求对象</param>
        /// <returns>启用并带有关联测点的分析配置列表</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllEnabledLargeDataAnalysisConfigWithDetail(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var jC_Largedataanalysisconfigresponse = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            var jC_LargedataanalysisconfigModelLists = _Repository.GetAllEnabledLargeDataAnalysisConfigList();
            IList<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(jC_LargedataanalysisconfigModelLists);
            if (listResult != null)
            {
                foreach (var item in listResult)
                {
                    BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> pointMappingResponse = _AnalysisModelPointRecordService.GetCustomizationAnalysisModelPointRecordInfoByModelId(new Sys.Safety.Request.JC_Setanalysismodelpointrecord.SetAnalysisModelPointRecordByModelIdGetRequest() { AnalysisModelId = item.Id });
                    if (pointMappingResponse.Data != null)
                    {
                        item.AnalysisModelPointRecordInfoList = pointMappingResponse.Data.ToList();
                    }
                    item.ExpressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();
                }
                jC_Largedataanalysisconfigresponse.Data = listResult.ToList();
            }
            else
            {
                jC_Largedataanalysisconfigresponse.Data = new List<JC_LargedataAnalysisConfigInfo>();
            }
            return jC_Largedataanalysisconfigresponse;
        }

        /// <summary>
        /// 获取没有关联报警通知的分析模型
        /// </summary>
        /// <param name="jc_LargedataAnalysisConfigRequest"></param>
        /// <returns>获取没有关联报警通知的分析模型</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutAlarmConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            var response = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            var modelList = _Repository.GetLargeDataAnalysisConfigWithoutAlarmConfigList();
            if (modelList != null && modelList.Count > 0)
                response.Data = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(modelList).ToList();
            else
                response.Data = new List<JC_LargedataAnalysisConfigInfo>();
            return response;
        }

        /// <summary>
        /// 获取关联应急联动的分析模型
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListWithRegionOutageRequest">关联应急联动的请求对象</param>
        /// <returns>获取关联应急联动的分析模型</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            string sqlParameter = string.Empty;
            if (!string.IsNullOrEmpty(largedataAnalysisConfigGetListWithRegionOutageRequest.AnalysisModelName))
                sqlParameter = string.Format(" and ac.Name like '%{0}%' ", largedataAnalysisConfigGetListWithRegionOutageRequest.AnalysisModelName.Replace("'", "''"));
            DataTable dtResult = _Repository.QueryTable("GetLargeDataAnalysisConfigWithRegionOutage", sqlParameter);
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            if (dtResult != null && dtResult.Rows.Count > 0)
                response.Data = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo>(dtResult);
            else
                response.Data = new List<JC_LargedataAnalysisConfigInfo>();

            return response;
        }
        /// <summary>
        /// 获取关联应急联动的分析模型
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListWithRegionOutageRequest">关联应急联动的请求对象</param>
        /// <returns>获取关联应急联动的分析模型</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutagePage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageIndex = largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageIndex - 1;
            if (largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageIndex < 0)
            {
                largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            int pageIndex = largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageIndex;
            int pageSize = largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.PageSize;
            rowcount = largedataAnalysisConfigGetListWithRegionOutageRequest.PagerInfo.RowCount;
            string sqlParameter = string.Empty;
            if (!string.IsNullOrEmpty(largedataAnalysisConfigGetListWithRegionOutageRequest.AnalysisModelName))
                sqlParameter = string.Format(" and ac.Name like '%{0}%' ", largedataAnalysisConfigGetListWithRegionOutageRequest.AnalysisModelName.Replace("'", "''"));
            DataTable dtResult = _Repository.QueryTable("GetLargeDataAnalysisConfigWithRegionOutage", sqlParameter);
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<JC_LargedataAnalysisConfigInfo> listResult = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo>(dtResult);

                rowcount = listResult.Count();
                if (pageSize == 0)
                {//查询所有数据
                    response.Data = listResult.OrderByDescending(t => t.UpdatedTime).ToList();
                }
                else
                {
                    response.Data = listResult.OrderByDescending(t => t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                if (response.Data != null && response.Data.Count > 0)
                {

                    response.PagerInfo.PageIndex = pageIndex;
                    response.PagerInfo.PageSize = pageSize;
                    response.PagerInfo.RowCount = rowcount;
                }
            }
            else
                response.Data = new List<JC_LargedataAnalysisConfigInfo>();

            return response;
        }

        /// <summary>
        /// 获取没有关联应急联动的分析模型
        /// </summary>
        /// <param name="largedataAnalysisConfigGetListRequest">分析配置请求对象</param>
        /// <returns>没有关联应急联动的分析模型</returns>
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutRegionOutage(LargedataAnalysisConfigGetListRequest largedataAnalysisConfigGetListRequest)
        {
            var response = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            var modelList = _Repository.GetLargeDataAnalysisConfigWithoutRegionOutage();
            if (modelList != null && modelList.Count > 0)
                response.Data = ObjectConverter.CopyList<JC_LargedataanalysisconfigModel, JC_LargedataAnalysisConfigInfo>(modelList).ToList();
            else
                response.Data = new List<JC_LargedataAnalysisConfigInfo>();
            return response;
        }

        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListForCurve(LargeDataAnalysisConfigListForCurveRequest largeDataAnalysisConfigListForCurveRequest)
        {
            if (string.IsNullOrEmpty(largeDataAnalysisConfigListForCurveRequest.QueryDate))
                return new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>() { Data = new List<JC_LargedataAnalysisConfigInfo>() };
            DateTime tryParseDateTime = DateTime.MinValue;
            if (!DateTime.TryParse(largeDataAnalysisConfigListForCurveRequest.QueryDate, out tryParseDateTime))
            {
                return new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>() { Data = new List<JC_LargedataAnalysisConfigInfo>() };
            }
            DataTable dtResult = _Repository.QueryTable("GetLargeDataAnalysisConfigListForCurve", tryParseDateTime.ToString("yyyy-MM-dd"));
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response = new BasicResponse<List<JC_LargedataAnalysisConfigInfo>>();
            if (dtResult != null && dtResult.Rows.Count > 0)
                response.Data = ObjectConverter.Copy<JC_LargedataAnalysisConfigInfo>(dtResult);
            else
                response.Data = new List<JC_LargedataAnalysisConfigInfo>();

            return response;
        }
    }
}


