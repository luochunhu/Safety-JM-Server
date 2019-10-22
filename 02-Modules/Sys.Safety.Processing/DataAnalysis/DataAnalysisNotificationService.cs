using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Analysistemplatealarmlevel;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.R_Personinf;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Request.SysEmergencyLinkage;

namespace Sys.Safety.Processing.DataAnalysis
{

    public class AnalysisChangedEventArgs : EventArgs
    {
        public JC_LargedataAnalysisConfigInfo AnalysisConfig
        {
            get;
            set;
        }
        public string Action { get; set; }
    }

    public delegate void DataAnalysisNotificationDelegate(AnalysisChangedEventArgs args);

    /// <summary>
    /// 作者:
    /// 创建时间:2017-06-02
    /// 功能描述:大数据分析通知服务
    /// 修改记录:
    /// </summary>
    public class DataAnalysisNotificationService
    {
        #region 单例
        private volatile static DataAnalysisNotificationService _instance = null;
        private static readonly object lockHelper = new object();
        private DataAnalysisNotificationService() { }

        public static DataAnalysisNotificationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new DataAnalysisNotificationService();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// 报警处理服务
        /// </summary>
        private IAlarmHandleService alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();
        /// <summary>
        /// 手动控制服务
        /// </summary>
        private IManualCrossControlService manualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        /// <summary>
        /// 测点定义服务
        /// </summary>
        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        /// <summary>
        /// 大数据分析客户端缓存服务
        /// </summary>
        ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        /// <summary>
        /// 多系统融合应急联动配置服务
        /// </summary>
        IEmergencyLinkHistoryService emergencyLinkHistoryService = ServiceFactory.Create<IEmergencyLinkHistoryService>();

        /// <summary>
        /// 广播呼叫缓存服务
        /// </summary>
        IBroadCastPointDefineService b_defCacheService = ServiceFactory.Create<IBroadCastPointDefineService>();
        IB_CallService bCallService = ServiceFactory.Create<IB_CallService>();

        /// <summary>
        /// 人员呼叫服务
        /// </summary>
        IPersonPointDefineService rdefCacheService = ServiceFactory.Create<IPersonPointDefineService>();
        IR_CallService rCallService = ServiceFactory.Create<IR_CallService>();
        IR_PersoninfService personService = ServiceFactory.Create<IR_PersoninfService>();

        /// <summary>
        /// 应急联动配置服务
        /// </summary
        ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();

        /// <summary>
        /// 传感器分级报警服务
        /// </summary>
        IJc_AnalysistemplatealarmlevelService analysistemplatealarmlevelService = ServiceFactory.Create<IJc_AnalysistemplatealarmlevelService>();

        #endregion

        #region Methods

        public void RegisterAnalysisReulstHandlerEvent(DataAnalysisService dataAnalysisService)
        {
            dataAnalysisService.AnalysisReulstHandlerEvent += DataAnalysisService_AnalysisReulstHandlerEvent;
        }

        #endregion

        #region Event Handler
        /// <summary>
        /// 报警，应急联动
        /// </summary>
        /// <param name="args"></param>
        private void DataAnalysisService_AnalysisReulstHandlerEvent(AnalysisChangedEventArgs args)
        {
            try
            {
                JC_LargedataAnalysisConfigInfo analysisConfigInfo = args.AnalysisConfig;
                string analysisModelId = analysisConfigInfo.Id;

                List<JC_AlarmNotificationPersonnelConfigInfo> responseAlarmConfigList;
                JC_EmergencyLinkageConfigInfo responseEmergencyLinkageConfig;
                List<JC_RegionOutageConfigInfo> responseRegionOutageConfigList;

                //多系统融合应急联动
                SysEmergencyLinkageInfo responsesysEmergencyLinkInfo;

                //分析模板传感器分级报警配置
                Jc_AnalysistemplatealarmlevelInfo analysistemplatealarmlevelInfo;

                try
                {
                    responseAlarmConfigList = AlarmConfigCache.Instance.Query(q => q.AnalysisModelId == analysisModelId);
                    responseEmergencyLinkageConfig = EmergencyLinkageConfigCache.Instance.Query(q => q.AnalysisModelId == analysisModelId).FirstOrDefault();
                    responseRegionOutageConfigList = RegionOutageConfigCache.Instance.Query(q => q.AnalysisModelId == analysisModelId);
                    //
                    //responsesysEmergencyLinkInfos = SysEmergencyLinkageCache.Instance.Query(o => o.MasterModelId == analysisModelId && o.Type == 2).FirstOrDefault();
                    responsesysEmergencyLinkInfo = sysEmergencyLinkageService.GetAllSysEmergencyLinkageList().Data.FirstOrDefault(o => o.MasterModelId == analysisModelId && o.Type == 2);

                    AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest cacherequest = new AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest();
                    cacherequest.AnalysistemplateId = analysisModelId;
                    analysistemplatealarmlevelInfo = analysistemplatealarmlevelService.GetAnalysistemplatealarmlevelByAnalysistemplateId(cacherequest).Data;

                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(string.Format("获取输出配置信息出错:{0}", ex.StackTrace));
                    return;
                }

                #region 报警配置
                //存在报警配置
                if (responseAlarmConfigList != null && responseAlarmConfigList.Count > 0)
                {
                    try
                    {
                        //报警
                        BasicResponse<JC_AlarmHandleInfo> alarmHandleResponse = alarmHandleService.GetUnclosedAlarmByAnalysisModelId(new AlarmHandleGetByAnalysisModelIdRequest() { AnalysisModelId = analysisModelId });
                        if (analysisConfigInfo.AnalysisResult == 2)
                        {
                            if (alarmHandleResponse.Data == null)
                            {
                                //报警消息格式:  测点号+安装位置+分析模型名称+输出结果 
                                StringBuilder alarmMessage = new StringBuilder();
                                var analysisSuccessfulPointList = ObjectConverter.CopyList<AnalysisSuccessfulPointInfo, AnalysisSuccessfulPointInfo>(analysisConfigInfo.AnalysisSuccessfulPointList);
                                foreach (var item in analysisSuccessfulPointList)
                                {
                                    alarmMessage.Append(string.Format("{0} {1} {2} {3}", item.Point, item.Wz, analysisConfigInfo.Name, analysisConfigInfo.TrueDescription)).Append(Environment.NewLine);
                                }
                                alarmHandleService.AddJC_AlarmHandle(new AlarmHandleAddRequest()
                                {
                                    JC_AlarmHandleInfo = new JC_AlarmHandleInfo()
                                    {
                                        Id = IdHelper.CreateLongId().ToString(),
                                        AnalysisModelId = analysisModelId,
                                        AnalysisResult = alarmMessage.ToString(),
                                        StartTime = DateTime.Now,
                                        AlarmType = responseAlarmConfigList[0].AlarmType,
                                        AlarmColor = responseAlarmConfigList[0].AlarmColor,
                                        EndTime = new DateTime(1900, 1, 1, 0, 0, 0)
                                    }
                                });
                            }
                        }
                        else
                        {
                            //不成立或未知更新报警结束时间
                            if (alarmHandleResponse.Data != null)
                            {
                                alarmHandleResponse.Data.EndTime = DateTime.Now;
                                alarmHandleService.UpdateJC_AlarmHandle(new AlarmHandleUpdateRequest() { JC_AlarmHandleInfo = alarmHandleResponse.Data });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(string.Format("添加或更新报警信息出错:{0}", ex.StackTrace));
                    }
                }
                #endregion

                #region 应急联动
                //应急联动
                if (analysisConfigInfo.AnalysisResult == 2 && responseEmergencyLinkageConfig != null)
                {
                    analysisConfigInfo.IsEmergencyLinkage = true;
                    analysisConfigInfo.EmergencyLinkageConfig = responseEmergencyLinkageConfig.Coordinate;
                }
                else
                {
                    //不成立或未知解除应急联动
                    analysisConfigInfo.IsEmergencyLinkage = false;
                    analysisConfigInfo.EmergencyLinkageConfig = string.Empty;
                }
                try
                {
                    largeDataAnalysisCacheClientService.UpdateLargeDataAnalysisConfigCahce(new LargeDataAnalysisConfigCacheUpdateRequest()
                    {
                        //LargeDataAnalysisConfigInfo = ObjectConverter.DeepCopy<JC_LargedataAnalysisConfigInfo>(analysisConfigInfo)
                        LargeDataAnalysisConfigInfo = analysisConfigInfo
                    });
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(string.Format("更新应急联动配置信息到缓存出错:{0}", ex.StackTrace));
                }
                #endregion

                #region 区域断电
                //存在区域断电配置
                if (responseRegionOutageConfigList != null && responseRegionOutageConfigList.Count > 0)
                {
                    //分析成立时
                    if (analysisConfigInfo.AnalysisResult == 2)
                    {
                        //要控制的列表
                        List<Jc_JcsdkzInfo> controlList = new List<Jc_JcsdkzInfo>();
                        //要解控的列表
                        List<Jc_JcsdkzInfo> removeControlList = new List<Jc_JcsdkzInfo>();
                        foreach (var item in responseRegionOutageConfigList)
                        {
                            BasicResponse<List<Jc_JcsdkzInfo>> analysisBKResponse;
                            try
                            {
                                BasicResponse<bool> controlPointLegalResponse = pointDefineService.ControlPointLegal(new Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest() { PointID = item.PointId });
                                if (!controlPointLegalResponse.Data)
                                {
                                    //风电闭锁控制口或者甲烷风电闭锁控制口,数据分析这边不处理.如果风电闭锁和甲烷风电闭锁后定义并且当前控制口已被控制则解除控制。
                                    analysisBKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPointBkPoint(new ManualCrossControlGetByTypeZkPointBkPointRequest() { ZkPoint = analysisModelId, BkPoint = item.Point, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                                    if (analysisBKResponse.Data != null && analysisBKResponse.Data.Count > 0)
                                    {
                                        try
                                        {
                                            manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisBKResponse.Data });
                                        }
                                        catch (Exception ex)
                                        {
                                            Basic.Framework.Logging.LogHelper.Error(string.Format("解除控制出错:{0}", ex.StackTrace));
                                        }
                                    }
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(string.Format("风电闭锁控制口或者甲烷风电闭锁控制口,数据分析这边不处理:{0}", ex.StackTrace));
                            }


                            try
                            {
                                //查询测点为item.Point的被控列表
                                analysisBKResponse = manualCrossControlService.GetManualCrossControlByBkPoint(new ManualCrossControlGetByBkPointRequest() { BkPoint = item.Point });
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(string.Format("查询测点为item.Point的被控列表出错:{0}", ex.StackTrace));
                                continue;
                            }
                            //控制
                            if (item.ControlStatus == 1)
                            {
                                //删除此处判断，添加了模型成立后，再添加区域断电，不会控制  20180919
                                ////如果上一次也是分析成立，则不再添加控制，为了避免一边添加控制一边解除控制这种情况。
                                //if (analysisConfigInfo.PrevAnalysisResult == analysisConfigInfo.AnalysisResult)
                                //    continue;

                                if (analysisBKResponse.Data != null && analysisBKResponse.Data.Count > 0)
                                {
                                    //被控测点 item.Point 已经存在控制,不能再控.
                                    continue;
                                }
                                else
                                {
                                    //向 jc_jcsdkz 插入数据
                                    if (!controlList.Exists(p => p.ZkPoint == item.AnalysisModelId && p.Bkpoint == item.Point))
                                        controlList.Add(new Jc_JcsdkzInfo()
                                        {
                                            ID = IdHelper.CreateLongId().ToString(),
                                            ZkPoint = item.AnalysisModelId,
                                            Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff,
                                            Upflag = "0",
                                            Bkpoint = item.Point
                                        });
                                }
                            }
                            //解除控制
                            if (item.ControlStatus == 0)
                            {
                                //表 jc_jcsdkz 存在被控测点为当前解控测点的记录.
                                if (analysisBKResponse.Data != null && analysisBKResponse.Data.Count > 0)
                                {
                                    //如果表 jc_jcsdkz 存在和当前解控模型和当前解控测点有关的控制则解除.
                                    Jc_JcsdkzInfo JkInfo = analysisBKResponse.Data.FirstOrDefault(q => q.ZkPoint == item.RemoveModelId && q.Bkpoint == item.Point && q.Type == (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff);
                                    if (JkInfo != null)
                                    {
                                        removeControlList.Add(new Jc_JcsdkzInfo()
                                        {
                                            ID = JkInfo.ID,
                                            ZkPoint = JkInfo.ZkPoint,
                                            Bkpoint = JkInfo.Bkpoint,
                                            Type = JkInfo.Type,
                                            Upflag = JkInfo.Upflag
                                        });
                                    }
                                }
                            }
                        }
                        if (controlList.Count > 0)
                        {
                            try
                            {
                                manualCrossControlService.AddManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = controlList });
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(string.Format("添加控制出错:{0}", ex.StackTrace));
                            }
                        }
                        if (removeControlList.Count > 0)
                        {
                            try
                            {
                                manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = removeControlList });
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(string.Format("解除控制出错:{0}", ex.StackTrace));
                            }
                        }
                    }
                    else
                    {
                        BasicResponse<List<Jc_JcsdkzInfo>> analysisZKResponse = null;
                        try
                        {
                            //查询主控为analysisModelId的列表
                            analysisZKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPoint(new ManualCrossControlGetByTypeZkPointRequest() { ZkPoint = analysisModelId, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                        }
                        catch (Exception ex)
                        {
                            Basic.Framework.Logging.LogHelper.Error(string.Format("查询主控为{0}的列表出错:{1}", analysisModelId, ex.StackTrace));
                        }
                        if (analysisZKResponse != null && analysisZKResponse.Data != null && analysisZKResponse.Data.Count > 0)
                        {
                            //不成立或未知时，首先看表达式测点是否还存在。 如果不存在, 分析模型控制的测点全部解除控制.
                            foreach (var modelPoint in analysisConfigInfo.AnalysisModelPointRecordInfoList)
                            {
                                Jc_DefInfo definedPoint = null;
                                try
                                {
                                    definedPoint = PointCache.Instance.Query(q => q.PointID == modelPoint.PointId, false).FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    Basic.Framework.Logging.LogHelper.Error(string.Format("获取测点缓存信息出错, 错误消息:{0}", ex.StackTrace));
                                }
                                if (PointCache.Instance.Count > 0 && definedPoint == null/*测点不存在*/)
                                {
                                    try
                                    {
                                        manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisZKResponse.Data });
                                    }
                                    catch (Exception ex)
                                    {
                                        Basic.Framework.Logging.LogHelper.Error(string.Format("解除控制出错:{0}", ex.StackTrace));
                                    }
                                    return;
                                }
                            }
                            //不成立或未知时解除分析成立时所加的控制
                            List<Jc_JcsdkzInfo> removeControlList = new List<Jc_JcsdkzInfo>();
                            foreach (var item in responseRegionOutageConfigList)
                            {
                                try
                                {
                                    BasicResponse<bool> controlPointLegalResponse = pointDefineService.ControlPointLegal(new Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest() { PointID = item.PointId });
                                    if (!controlPointLegalResponse.Data)
                                    {
                                        //风电闭锁控制口或者甲烷风电闭锁控制口,数据分析这边不处理.如果风电闭锁和甲烷风电闭锁后定义并且当前控制口已被控制则解除控制。
                                        BasicResponse<List<Jc_JcsdkzInfo>> analysisBKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPointBkPoint(new ManualCrossControlGetByTypeZkPointBkPointRequest() { ZkPoint = analysisModelId, BkPoint = item.Point, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                                        if (analysisBKResponse.Data != null && analysisBKResponse.Data.Count > 0)
                                        {
                                            try
                                            {
                                                manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisBKResponse.Data });
                                            }
                                            catch (Exception ex)
                                            {
                                                Basic.Framework.Logging.LogHelper.Error(string.Format("解除控制出错:{0}", ex.StackTrace));
                                            }
                                        }
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Basic.Framework.Logging.LogHelper.Error(string.Format("风电闭锁控制口或者甲烷风电闭锁控制口,数据分析这边不处理:{0}", ex.StackTrace));
                                }
                                //是否配置当分析不成立时解除分析成立时所加的控制. 1-解除控制
                                if (item.ControlStatus == 1 && item.IsRemoveControl == 1)
                                {
                                    Jc_JcsdkzInfo JkInfo = analysisZKResponse.Data.FirstOrDefault(q => q.ZkPoint == item.AnalysisModelId && q.Bkpoint == item.Point && q.Type == (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff);
                                    if (JkInfo != null)
                                    {
                                        removeControlList.Add(new Jc_JcsdkzInfo()
                                        {
                                            ID = JkInfo.ID,
                                            ZkPoint = JkInfo.ZkPoint,
                                            Bkpoint = JkInfo.Bkpoint,
                                            Type = JkInfo.Type,
                                            Upflag = JkInfo.Upflag
                                        });
                                    }
                                }
                            }
                            if (removeControlList.Count > 0)
                            {
                                try
                                {
                                    manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = removeControlList });
                                }
                                catch (Exception ex)
                                {
                                    Basic.Framework.Logging.LogHelper.Error(string.Format("解除控制出错:{0}", ex.StackTrace));
                                }
                            }

                        }
                    }
                }
                else
                {
                    try
                    {
                        //没有区域断电配置.
                        BasicResponse<List<Jc_JcsdkzInfo>> analysisZKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPoint(new ManualCrossControlGetByTypeZkPointRequest() { ZkPoint = analysisModelId, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                        if (analysisZKResponse != null && analysisZKResponse.Data != null && analysisZKResponse.Data.Count > 0)
                        {
                            manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisZKResponse.Data });
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(string.Format("没有区域断电配置时删除控制出错:{0}", ex.StackTrace));
                    }
                }
                #endregion

                #region 多系统融合应急联动

                if (responsesysEmergencyLinkInfo != null && !string.IsNullOrEmpty(responsesysEmergencyLinkInfo.Id))
                {
                    SysEmergencyLinkHandle(analysisConfigInfo, responsesysEmergencyLinkInfo);
                }
                #endregion

                #region 传感器分级报警

                if (analysistemplatealarmlevelInfo != null)
                {
                    AnalysistemplateAlarmLevelHandle(analysisConfigInfo, analysistemplatealarmlevelInfo);
                }
                //如果报警配置等级不存在，则更新模型关联传感器报警等级为0
                else
                {
                    ManualCrossControlGetByTypeZkPointRequest mcrequest = new ManualCrossControlGetByTypeZkPointRequest();
                    mcrequest.Type = 12;
                    mcrequest.ZkPoint = analysisConfigInfo.Id;
                    var kzinfo = manualCrossControlService.GetManualCrossControlByTypeZkPoint(mcrequest).Data;

                    if (kzinfo.Count > 0)
                    {
                        kzinfo.ForEach(kz => kz.Upflag = "0");

                        ManualCrossControlsRequest updaterequest = new ManualCrossControlsRequest();
                        updaterequest.ManualCrossControlInfos = kzinfo;
                        manualCrossControlService.UpdateManualCrossControls(updaterequest);
                    }
                }

                #endregion

                //更新上一次的分析结果
                Dictionary<string, object> upddateitems = new Dictionary<string, object>();
                upddateitems.Add("PrevAnalysisResult", analysisConfigInfo.AnalysisResult);
                if (analysisConfigInfo.AnalysisTime != null)
                {
                    upddateitems.Add("PrevAnalysisTime", analysisConfigInfo.AnalysisTime.Value);
                }
                AnalysisModelCache.Instance.UpdateProperties(analysisConfigInfo.Id, upddateitems);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 多系统融合应急联动输出
        /// </summary>
        /// <param name="responsesysEmergencyLinkInfos"></param>
        private void SysEmergencyLinkHandle(JC_LargedataAnalysisConfigInfo analysisConfigInfo, SysEmergencyLinkageInfo responsesysEmergencyLinkInfo)
        {
            try
            {
                //如果模型成立则1.添加控制 2.更新配置缓存 3.写运行记录
                if (analysisConfigInfo.AnalysisResult == 2)
                {
                    //如果不是强制结束则立即执行，如果是强制结束则在大于延迟时间时执行
                    var isforceend = responsesysEmergencyLinkInfo.IsForceEnd;
                    TimeSpan span = DateTime.Now - responsesysEmergencyLinkInfo.EndTime;

                    if (!isforceend || (isforceend && span.TotalSeconds > responsesysEmergencyLinkInfo.DelayTime))
                    {
                        #region 1.更新控制缓存

                        string bpointlist = string.Empty;
                        string rpointlist = string.Empty;
                        string rpersonlist = string.Empty;

                        //如果应急联动配置存在被控区域，则获取此区域的人员设备和广播设备
                        if (responsesysEmergencyLinkInfo.PassiveAreas != null && responsesysEmergencyLinkInfo.PassiveAreas.Count > 0)
                        {
                            responsesysEmergencyLinkInfo.PassiveAreas.ForEach(area =>
                            {
                                //处理区域广播呼叫
                                var bdefinfos = b_defCacheService.GetPointDefineCacheByAreaID(new PointDefineGetByAreaIDRequest { AreaId = area.AreaId }).Data;
                                bdefinfos.ForEach(b => { bpointlist += b.PointID + ","; });

                                //处理区域人员呼叫
                                var rdefinfos = rdefCacheService.GetPointDefineCacheByAreaID(new PointDefineGetByAreaIDRequest { AreaId = area.AreaId }).Data;
                                rdefinfos.ForEach(r => { rpointlist += r.Point + ","; });

                            });
                        }

                        //如果应急联动配置存在被控人员，则获取被控人员列表
                        if (responsesysEmergencyLinkInfo.PassivePersons != null && responsesysEmergencyLinkInfo.PassivePersons.Count > 0)
                        {
                            responsesysEmergencyLinkInfo.PassivePersons.ForEach(p =>
                            {
                                RPersoninfCacheGetByKeyRequest persongetrequest = new RPersoninfCacheGetByKeyRequest();
                                var person = personService.GetPersoninfCache(new R_PersoninfGetRequest() { Id = p.PersonId }).Data;
                                if (person != null)
                                    rpersonlist += person.Bh + ",";
                            });
                        }

                        //如果应急联动配置存在被控设备，则根据类型获取被控测点
                        if (responsesysEmergencyLinkInfo.PassivePoints != null && responsesysEmergencyLinkInfo.PassivePoints.Count > 0)
                        {
                            responsesysEmergencyLinkInfo.PassivePoints.ForEach(p =>
                            {
                                //人员定位
                                if (p.Sysid == (int)SystemEnum.Personnel)
                                {
                                    var rdef = rdefCacheService.GetPointDefineCacheByPointID(new PointDefineGetByPointIDRequest { PointID = p.PointId }).Data;
                                    if (rdef != null && !string.IsNullOrEmpty(rdef.PointID))
                                        rpointlist += rdef.Point + "|";
                                }
                                //广播
                                else if (p.Sysid == (int)SystemEnum.Broadcast)
                                {
                                    var bdef = b_defCacheService.GetPointDefineCacheByPointID(new PointDefineGetByPointIDRequest { PointID = p.PointId }).Data;
                                    if (bdef != null && !string.IsNullOrEmpty(bdef.PointID))
                                        bpointlist += bdef.PointID + "|";
                                }
                            });
                        }

                        //添加广播呼叫
                        if (bpointlist.Length > 0)
                        {
                            BCallInfoGetByMasterIDRequest b_defExistsRequest = new BCallInfoGetByMasterIDRequest();
                            b_defExistsRequest.MasterId = analysisConfigInfo.Id;
                            b_defExistsRequest.CallType = 1;
                            var bcallinfo = bCallService.GetBCallInfoByMasterID(b_defExistsRequest).Data.FirstOrDefault();
                            if (bcallinfo == null)
                            {
                                bpointlist = bpointlist.Substring(0, bpointlist.Length - 1);

                                var bpointarr = bpointlist.Split('|');

                                B_CallInfo bcalladdinfo = new B_CallInfo();
                                string callid = IdHelper.CreateLongId().ToString();
                                bcalladdinfo.Id = callid;
                                bcalladdinfo.CallType = 1;
                                bcalladdinfo.CallTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                bcalladdinfo.MasterId = analysisConfigInfo.Id;
                                bcalladdinfo.Message = analysisConfigInfo.TrueDescription;
                                //bcalladdinfo.PointList = bpointlist;

                                List<B_CallpointlistInfo> callpointdetails = new List<B_CallpointlistInfo>();
                                Array.ForEach(bpointarr, point =>
                                {
                                    B_CallpointlistInfo callpointdetail = new B_CallpointlistInfo();
                                    callpointdetail.Id = IdHelper.CreateLongId().ToString();
                                    callpointdetail.BCallId = callid;
                                    callpointdetail.CalledPointId = point;

                                    callpointdetails.Add(callpointdetail);
                                });
                                bcalladdinfo.CallPointList = callpointdetails;

                                bCallService.AddCall(new B_CallAddRequest { CallInfo = bcalladdinfo });
                            }
                        }

                        //添加人员定位设备呼叫
                        if (rpointlist.Length > 0)
                        {
                            RCallInfoGetByMasterIDRequest rcallgetRequest = new RCallInfoGetByMasterIDRequest();
                            rcallgetRequest.MasterId = analysisConfigInfo.Id;
                            rcallgetRequest.CallType = 1;
                            rcallgetRequest.IsQueryByType = true;
                            rcallgetRequest.Type = 1;
                            var rcallinfo = rCallService.GetRCallInfoByMasterID(rcallgetRequest).Data.FirstOrDefault();
                            if (rcallinfo == null)
                            {
                                rpointlist = rpointlist.Substring(0, rpointlist.Length - 1);

                                R_CallInfo rcalladdinfo = new R_CallInfo();
                                rcalladdinfo.Id = IdHelper.CreateLongId().ToString();
                                rcalladdinfo.Type = 1;
                                rcalladdinfo.CallType = 1;
                                rcalladdinfo.CallTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                rcalladdinfo.MasterId = analysisConfigInfo.Id;
                                rcalladdinfo.CallPersonDefType = 4;
                                rcalladdinfo.PointList = rpointlist;

                                rCallService.AddCall(new R_CallAddRequest { CallInfo = rcalladdinfo });
                            }
                        }

                        //添加人员卡号呼叫
                        if (rpersonlist.Length > 0)
                        {
                            RCallInfoGetByMasterIDRequest rcallgetRequest = new RCallInfoGetByMasterIDRequest();
                            rcallgetRequest.MasterId = analysisConfigInfo.Id;
                            rcallgetRequest.CallType = 1;
                            rcallgetRequest.IsQueryByType = true;
                            rcallgetRequest.Type = 0;
                            var rcallinfo = rCallService.GetRCallInfoByMasterID(rcallgetRequest).Data.FirstOrDefault();
                            if (rcallinfo == null)
                            {
                                rpersonlist = rpersonlist.Substring(0, rpersonlist.Length - 1);

                                R_CallInfo rcalladdinfo = new R_CallInfo();
                                rcalladdinfo.Id = IdHelper.CreateLongId().ToString();
                                rcalladdinfo.Type = 0;
                                rcalladdinfo.CallType = 1;
                                rcalladdinfo.CallTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                rcalladdinfo.MasterId = analysisConfigInfo.Id;
                                rcalladdinfo.CallPersonDefType = 2;
                                rcalladdinfo.BhContent = rpersonlist;

                                rCallService.AddCall(new R_CallAddRequest { CallInfo = rcalladdinfo });
                            }
                        }
                        #endregion
                    }

                    if (responsesysEmergencyLinkInfo.EmergencyLinkageState != 1)
                    {
                        //2.更新配置缓存
                        responsesysEmergencyLinkInfo.EmergencyLinkageState = 1;
                        sysEmergencyLinkageService.UpdateSysEmergencyLinkage(new SysEmergencyLinkageUpdateRequest { SysEmergencyLinkageInfo = responsesysEmergencyLinkInfo });
                        //3.写运行记录
                        EmergencyLinkHistoryAddRequest ehistoryaddrequest = new EmergencyLinkHistoryAddRequest();
                        EmergencyLinkHistoryInfo ehistory = new EmergencyLinkHistoryInfo();
                        ehistory.Id = IdHelper.CreateLongId().ToString();
                        ehistory.IsForceEnd = 0;
                        ehistory.SysEmergencyLinkageId = responsesysEmergencyLinkInfo.Id;
                        ehistory.StartTime = DateTime.Now;
                        ehistory.EndTime = new DateTime(1900, 1, 1, 0, 0, 0);
                        ehistoryaddrequest.EmergencyLinkHistoryInfo = ehistory;
                        emergencyLinkHistoryService.AddEmergencyLinkHistory(ehistoryaddrequest);
                    }

                }
                //如果模型不成立则1.解除控制 2.更新配置缓存 3.更新运行记录
                else
                {
                    //解除广播控制
                    BCallInfoGetByMasterIDRequest b_defExistsRequest = new BCallInfoGetByMasterIDRequest();
                    b_defExistsRequest.MasterId = analysisConfigInfo.Id;
                    b_defExistsRequest.CallType = 1;
                    var bcallinfo = bCallService.GetBCallInfoByMasterID(b_defExistsRequest).Data.FirstOrDefault();
                    if (bcallinfo != null && bcallinfo.CallType != 2)
                    {
                        bcallinfo.CallType = 2;
                        bCallService.UpdateCall(new B_CallUpdateRequest { CallInfo = bcallinfo });
                    }

                    //解除人员定位控制
                    RCallInfoGetByMasterIDRequest rcallgetRequest = new RCallInfoGetByMasterIDRequest();
                    rcallgetRequest.MasterId = analysisConfigInfo.Id;
                    rcallgetRequest.CallType = 1;
                    rcallgetRequest.IsQueryByType = false;
                    rcallgetRequest.Type = 0;
                    var rcallinfo = rCallService.GetRCallInfoByMasterID(rcallgetRequest).Data;
                    if (rcallinfo.Count > 0)
                    {
                        rcallinfo.ForEach(o =>
                        {
                            if (o.CallType != 2)
                            {
                                o.CallType = 2;
                                rCallService.UpdateCall(new R_CallUpdateRequest { CallInfo = o });
                            }
                        });
                    }

                    if (responsesysEmergencyLinkInfo.EmergencyLinkageState != 0)
                    {
                        //2.更新配置缓存
                        responsesysEmergencyLinkInfo.EmergencyLinkageState = 0;
                        sysEmergencyLinkageService.UpdateSysEmergencyLinkage(new SysEmergencyLinkageUpdateRequest { SysEmergencyLinkageInfo = responsesysEmergencyLinkInfo });
                    }

                    //3.更新运行记录
                    var emergencyLinkHistory = emergencyLinkHistoryService.GetEmergencyLinkHistoryByEmergency(new EmergencyLinkHistoryGetByEmergencyRequest { EmergencyId = responsesysEmergencyLinkInfo.Id }).Data;
                    if (emergencyLinkHistory != null && !string.IsNullOrEmpty(emergencyLinkHistory.Id) && emergencyLinkHistory.EndTime.Year == 1900)
                    {
                        emergencyLinkHistory.EndTime = DateTime.Now;
                        emergencyLinkHistoryService.UpdateEmergencyLinkHistory(new EmergencyLinkHistoryUpdateRequest { EmergencyLinkHistoryInfo = emergencyLinkHistory });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(" 多系统融合应急联动输出出错！" + ex.Message);
            }
        }

        /// <summary>
        /// 传感器分级报警输出
        /// </summary>
        /// <param name="analysisConfigInfo"></param>
        /// <param name="analysistemplatealarmlevelInfo"></param>
        private void AnalysistemplateAlarmLevelHandle(JC_LargedataAnalysisConfigInfo analysisConfigInfo, Jc_AnalysistemplatealarmlevelInfo analysistemplatealarmlevelInfo)
        {
            try
            {
                //如果分析配置成立,则添加手动控制
                if (analysisConfigInfo.AnalysisResult == 2)
                {
                    string level = analysistemplatealarmlevelInfo.Level.ToString();

                    if (analysisConfigInfo.AnalysisSuccessfulPointList.Count > 0)
                    {
                        List<Jc_JcsdkzInfo> jckzinfos = new List<Jc_JcsdkzInfo>();
                        List<Jc_JcsdkzInfo> updatejckzinfos = new List<Jc_JcsdkzInfo>();

                        analysisConfigInfo.AnalysisSuccessfulPointList.ForEach(point =>
                        {
                            //判断是否存在同一配置被控测点的控制缓存，如果不存在则新增一条控制
                            ManualCrossControlGetByTypeZkPointBkPointRequest mcrequest = new ManualCrossControlGetByTypeZkPointBkPointRequest();
                            mcrequest.Type = 12;
                            mcrequest.ZkPoint = analysisConfigInfo.Id;
                            mcrequest.BkPoint = point.Point;
                            var kzinfo = manualCrossControlService.GetManualCrossControlByTypeZkPointBkPoint(mcrequest).Data.FirstOrDefault();

                            if (kzinfo == null)
                            {
                                Jc_JcsdkzInfo JkInfo = new Jc_JcsdkzInfo();
                                JkInfo.ID = IdHelper.CreateLongId().ToString();
                                JkInfo.Type = 12;
                                JkInfo.ZkPoint = analysisConfigInfo.Id;
                                JkInfo.Bkpoint = point.Point;
                                JkInfo.Upflag = level;

                                jckzinfos.Add(JkInfo);
                            }
                            else if (kzinfo.Upflag != level)
                            {
                                kzinfo.Upflag = level;
                                updatejckzinfos.Add(kzinfo);
                            }
                        });

                        //删除已经结束的分级报警传感器  20180919
                        List<Jc_JcsdkzInfo> manualCrossControlList = manualCrossControlService.GetAllManualCrossControl().Data.FindAll(a => a.Upflag != "0");
                        foreach (Jc_JcsdkzInfo manualCrossControl in manualCrossControlList)
                        {
                            bool isInanalysisConfigInfo = false;
                            foreach (AnalysisSuccessfulPointInfo point in analysisConfigInfo.AnalysisSuccessfulPointList)
                            {
                                if (point.Point == manualCrossControl.Bkpoint)
                                {
                                    isInanalysisConfigInfo = true;
                                    break;
                                }
                            }
                            if (!isInanalysisConfigInfo)//如果在当前成立的表达式中没有找到对应交叉控制测点的表达式，则清除传感器分级报警 
                            {
                                manualCrossControl.Upflag = "0";
                                updatejckzinfos.Add(manualCrossControl);
                            }
                        }

                        if (jckzinfos.Count > 0)
                        {
                            ManualCrossControlsRequest batchinsertrequest = new ManualCrossControlsRequest();
                            batchinsertrequest.ManualCrossControlInfos = jckzinfos;
                            manualCrossControlService.AddManualCrossControls(batchinsertrequest);
                        }

                        if (updatejckzinfos.Count > 0)
                        {
                            ManualCrossControlsRequest updaterequest = new ManualCrossControlsRequest();
                            updaterequest.ManualCrossControlInfos = updatejckzinfos;
                            manualCrossControlService.UpdateManualCrossControls(updaterequest);
                        }
                    }
                }
                //如果分析不成立，则修改报警等级为0
                else
                {
                    ManualCrossControlGetByTypeZkPointRequest mcrequest = new ManualCrossControlGetByTypeZkPointRequest();
                    mcrequest.Type = 12;
                    mcrequest.ZkPoint = analysisConfigInfo.Id;
                    var kzinfo = manualCrossControlService.GetManualCrossControlByTypeZkPoint(mcrequest).Data;

                    if (kzinfo.Count > 0)
                    {
                        kzinfo.ForEach(kz => kz.Upflag = "0");

                        ManualCrossControlsRequest updaterequest = new ManualCrossControlsRequest();
                        updaterequest.ManualCrossControlInfos = kzinfo;
                        manualCrossControlService.UpdateManualCrossControls(updaterequest);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("传感器分级报警输出出错！" + ex.Message);
            }
        }

        #endregion
    }
}
