using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Data;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class AlarmNotificationPersonnelConfigService : IAlarmNotificationPersonnelConfigService
    {
        private IAlarmNotificationPersonnelConfigRepository _Repository;
        private IAlarmNotificationPersonnelRepository _AlarmNotificationPersonnelRepository;
        private IAlarmHandleService _AlarmHandleService;

        public AlarmNotificationPersonnelConfigService(IAlarmNotificationPersonnelConfigRepository _Repository,
            IAlarmNotificationPersonnelRepository _AlarmNotificationPersonnelRepository,
            IAlarmHandleService _AlarmHandleService)
        {
            this._Repository = _Repository;
            this._AlarmNotificationPersonnelRepository = _AlarmNotificationPersonnelRepository;
            this._AlarmHandleService = _AlarmHandleService;
        }
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> AddJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigAddRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var _jC_AlarmNotificationPersonnel = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>
         (jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelInfoList);

            var _jC_Alarmnotificationpersonnelconfig = ObjectConverter.Copy<JC_AlarmNotificationPersonnelConfigInfo, JC_AlarmnotificationpersonnelconfigModel>(
                jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelConfigInfo
                );

            IList<JC_AlarmnotificationpersonnelconfigModel> alarmConfig = _Repository.GetAlarmNotificationPersonnelConfigByAnalysisModelId(_jC_Alarmnotificationpersonnelconfig.AnalysisModelId);
            if (alarmConfig != null && alarmConfig.ToList().Count > 0)
                return new BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>() { Message = "分析模型已配置报警通知!", Code = -100, Data = jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelConfigInfo };

            JC_AlarmnotificationpersonnelconfigModel addedAlarmnotificationpersonnelconfigModel = null;
            TransactionsManager.BeginTransaction(() =>
            {
                addedAlarmnotificationpersonnelconfigModel = _Repository.AddJC_AlarmNotificationPersonnelConfig(_jC_Alarmnotificationpersonnelconfig);
                if (jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelInfoList != null)
                {
                    _AlarmNotificationPersonnelRepository.AddJC_AlarmNotificationPersonnelList(_jC_AlarmNotificationPersonnel.ToList());
                }
            });

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey, DateTime.Now);
            }

            var jC_Alarmnotificationpersonnelconfigresponse = new BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>();
            jC_Alarmnotificationpersonnelconfigresponse.Data = jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelConfigInfo;
            return jC_Alarmnotificationpersonnelconfigresponse;
        }
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> UpdateJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigUpdateRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var _jC_Alarmnotificationpersonnelconfig = ObjectConverter.Copy<JC_AlarmNotificationPersonnelConfigInfo, JC_AlarmnotificationpersonnelconfigModel>(jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelConfigInfo);

            var _jC_AlarmNotificationPersonnel = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>
               (jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelInfoList);
            TransactionsManager.BeginTransaction(() =>
            {
                _Repository.UpdateJC_AlarmNotificationPersonnelConfig(_jC_Alarmnotificationpersonnelconfig);

                _AlarmNotificationPersonnelRepository.DeleteJC_AlarmNotificationPersonnelByAlarmConfigId(_jC_Alarmnotificationpersonnelconfig.Id);

                _AlarmNotificationPersonnelRepository.AddJC_AlarmNotificationPersonnelList(_jC_AlarmNotificationPersonnel.ToList());

            });

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey, DateTime.Now);
            }

            var jC_Alarmnotificationpersonnelconfigresponse = new BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>();
            jC_Alarmnotificationpersonnelconfigresponse.Data = jC_Alarmnotificationpersonnelconfigrequest.JC_AlarmNotificationPersonnelConfigInfo;
            return jC_Alarmnotificationpersonnelconfigresponse;
        }
        public BasicResponse DeleteJC_AlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigDeleteRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            TransactionsManager.BeginTransaction(() =>
            {
                if (!string.IsNullOrWhiteSpace(jC_Alarmnotificationpersonnelconfigrequest.Id))
                {
                    //删除报警推送信息的时候将存在的报警处理记录填上EndTime
                    JC_AlarmnotificationpersonnelconfigModel alarmnotificationpersonnelconfigModel = _Repository.GetJC_AlarmNotificationPersonnelConfigById(jC_Alarmnotificationpersonnelconfigrequest.Id);
                    _AlarmHandleService.CloseUnclosedAlarmHandleByAnalysisModelId(new Sys.Safety.Request.AlarmHandle.AlarmHandleGetByAnalysisModelIdRequest() { AnalysisModelId = alarmnotificationpersonnelconfigModel.AnalysisModelId });

                    _Repository.DeleteJC_AlarmNotificationPersonnelConfig(jC_Alarmnotificationpersonnelconfigrequest.Id);
                    _AlarmNotificationPersonnelRepository.DeleteJC_AlarmNotificationPersonnelByAlarmConfigId(jC_Alarmnotificationpersonnelconfigrequest.Id);
                }
                else
                {
                    foreach (var item in jC_Alarmnotificationpersonnelconfigrequest.ids)
                    {
                        //删除报警推送信息的时候将存在的报警处理记录填上EndTime
                        JC_AlarmnotificationpersonnelconfigModel alarmnotificationpersonnelconfigModel = _Repository.GetJC_AlarmNotificationPersonnelConfigById(item);
                        _AlarmHandleService.CloseUnclosedAlarmHandleByAnalysisModelId(new Sys.Safety.Request.AlarmHandle.AlarmHandleGetByAnalysisModelIdRequest() { AnalysisModelId = alarmnotificationpersonnelconfigModel.AnalysisModelId });

                        _Repository.DeleteJC_AlarmNotificationPersonnelConfig(item);
                        _AlarmNotificationPersonnelRepository.DeleteJC_AlarmNotificationPersonnelByAlarmConfigId(item);
                    }
                }
            });

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey, DateTime.Now);
            }

            var jC_Alarmnotificationpersonnelconfigresponse = new BasicResponse();
            return jC_Alarmnotificationpersonnelconfigresponse;
        }
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetJC_AlarmNotificationPersonnelConfigList(AlarmNotificationPersonnelConfigGetListRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var jC_Alarmnotificationpersonnelconfigresponse = new BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>();
            jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageIndex = jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageIndex - 1;
            if (jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageIndex < 0)
            {
                jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_AlarmnotificationpersonnelconfigModelLists = _Repository.GetJC_AlarmNotificationPersonnelConfigList(jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageIndex, jC_Alarmnotificationpersonnelconfigrequest.PagerInfo.PageSize, out rowcount);
            var jC_AlarmnotificationpersonnelconfigInfoLists = new List<JC_AlarmNotificationPersonnelConfigInfo>();
            foreach (var item in jC_AlarmnotificationpersonnelconfigModelLists)
            {
                var JC_AlarmNotificationPersonnelConfigInfo = ObjectConverter.Copy<JC_AlarmnotificationpersonnelconfigModel, JC_AlarmNotificationPersonnelConfigInfo>(item);
                jC_AlarmnotificationpersonnelconfigInfoLists.Add(JC_AlarmNotificationPersonnelConfigInfo);
            }
            jC_Alarmnotificationpersonnelconfigresponse.Data = jC_AlarmnotificationpersonnelconfigInfoLists;
            return jC_Alarmnotificationpersonnelconfigresponse;
        }
        public BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> GetJC_AlarmNotificationPersonnelConfigById(AlarmNotificationPersonnelConfigGetRequest jC_Alarmnotificationpersonnelconfigrequest)
        {
            var result = _Repository.GetJC_AlarmNotificationPersonnelConfigById(jC_Alarmnotificationpersonnelconfigrequest.Id);
            var jC_AlarmnotificationpersonnelconfigInfo = ObjectConverter.Copy<JC_AlarmnotificationpersonnelconfigModel, JC_AlarmNotificationPersonnelConfigInfo>(result);

            IList<JC_AlarmNotificationPersonnelModel> alarmNotificationPersonnelModelList = _AlarmNotificationPersonnelRepository.GetJC_AlarmNotificationPersonnelListByAlarmConfigId(jC_AlarmnotificationpersonnelconfigInfo.Id);
            if (alarmNotificationPersonnelModelList != null)
                jC_AlarmnotificationpersonnelconfigInfo.JC_AlarmNotificationPersonnelInfoList = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(alarmNotificationPersonnelModelList).ToList();
            var jC_Alarmnotificationpersonnelconfigresponse = new BasicResponse<JC_AlarmNotificationPersonnelConfigInfo>();
            jC_Alarmnotificationpersonnelconfigresponse.Data = jC_AlarmnotificationpersonnelconfigInfo;
            return jC_Alarmnotificationpersonnelconfigresponse;
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigByAnalysisModelId(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            var response = new BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>();
            IList<JC_AlarmnotificationpersonnelconfigModel> alarmNotificationPersonnelConfigModelList = _Repository.GetAlarmNotificationPersonnelConfigByAnalysisModelId(getListByAnalysisModelIdRequest.AnalysisModelId);
            if (alarmNotificationPersonnelConfigModelList != null)
                response.Data = ObjectConverter.CopyList<JC_AlarmnotificationpersonnelconfigModel, JC_AlarmNotificationPersonnelConfigInfo>(alarmNotificationPersonnelConfigModelList).ToList();
            return response;
        }
        /// <summary>
        /// 根据模型名称模糊查询报警推送配置信息
        /// </summary>
        /// <param name="getListByAnalysisModelIdRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelListByAnalysisModeName(AlarmNotificationPersonnelConfigGetListRequest getListByAnalysisModelIdRequest)
        {
            var JC_AlarmNotificationPersonnelConfigInforesponse = new BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>();
            getListByAnalysisModelIdRequest.PagerInfo.PageIndex = getListByAnalysisModelIdRequest.PagerInfo.PageIndex - 1;
            if (getListByAnalysisModelIdRequest.PagerInfo.PageIndex < 0)
            {
                getListByAnalysisModelIdRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            int pageIndex = getListByAnalysisModelIdRequest.PagerInfo.PageIndex;
            int pageSize = getListByAnalysisModelIdRequest.PagerInfo.PageSize;
            rowcount = getListByAnalysisModelIdRequest.PagerInfo.RowCount;
            try
            {
                StringBuilder sqlWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(getListByAnalysisModelIdRequest.AnalysisModeName))
                {
                    sqlWhere.Append(" and b.Name like '%");
                    sqlWhere.Append(getListByAnalysisModelIdRequest.AnalysisModeName.Trim());
                    sqlWhere.Append("%'");
                }

                DataTable dataTable = _Repository.QueryTable("global_AlarmNotificationPersonnelService_GetAlarmNotificationPersonnelListByAnalysisModeName", sqlWhere.ToString());
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    List<JC_AlarmNotificationPersonnelConfigInfo> listResult = ObjectConverter.Copy<JC_AlarmNotificationPersonnelConfigInfo>(dataTable);

                    rowcount = listResult.Count();
                    if (pageSize == 0)
                    {//查询所有数据
                        JC_AlarmNotificationPersonnelConfigInforesponse.Data = listResult.OrderByDescending(t => t.UpdatedTime).ToList();
                    }
                    else
                    {
                        JC_AlarmNotificationPersonnelConfigInforesponse.Data = listResult.OrderByDescending(t => t.UpdatedTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    if (JC_AlarmNotificationPersonnelConfigInforesponse.Data != null && JC_AlarmNotificationPersonnelConfigInforesponse.Data.Count > 0)
                    {

                        JC_AlarmNotificationPersonnelConfigInforesponse.PagerInfo.PageIndex = pageIndex;
                        JC_AlarmNotificationPersonnelConfigInforesponse.PagerInfo.PageSize = pageSize;
                        JC_AlarmNotificationPersonnelConfigInforesponse.PagerInfo.RowCount = rowcount;
                    }
                }
                else
                {
                    JC_AlarmNotificationPersonnelConfigInforesponse.Data = new List<JC_AlarmNotificationPersonnelConfigInfo>();
                }

            }
            catch
            {
                JC_AlarmNotificationPersonnelConfigInforesponse.Data = new List<JC_AlarmNotificationPersonnelConfigInfo>();
            }
            return JC_AlarmNotificationPersonnelConfigInforesponse;
        }

        /// <summary>
        /// 是否有某个分析模型的报警通知配置信息
        /// </summary>
        /// <returns>是否有某个分析模型的报警通知配置信息</returns>
        public BasicResponse<bool> HasAlarmNotificationForAnalysisModel(GetAlarmNotificationByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();
            response.Data = _Repository.HasAlarmNotificationForAnalysisModel(getByAnalysisModelIdRequest.AnalysisModelId);

            return response;
        }

        /// <summary>
        /// 获取所有报警通知人员配置列表
        /// </summary>
        /// <param name="getAllAlarmNotificationRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> GetAlarmNotificationPersonnelConfigAllList(GetAllAlarmNotificationRequest getAllAlarmNotificationRequest)
        {
            BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> response = new BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>();
            DataTable dtResult = _Repository.QueryTable("dataAnalysis_AlarmNotificationPersonnelConfigAllList");
            response.Data = ObjectConverter.Copy<JC_AlarmNotificationPersonnelConfigInfo>(dtResult);
            return response;
        }

        public BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> AddAlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigListAddRequest addRequest)
        {
            var addConfigParameter = addRequest.JC_AlarmNotificationPersonnelConfigListInfo;
            var addPersonnelParameter = addRequest.JC_AlarmNotificationPersonnelInfoList;
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in addConfigParameter)
                {
                    IList<JC_AlarmnotificationpersonnelconfigModel> alarmConfig = _Repository.GetAlarmNotificationPersonnelConfigByAnalysisModelId(item.AnalysisModelId);
                    if (alarmConfig != null && alarmConfig.ToList().Count > 0)
                        continue;
                    //item.Id = IdHelper.CreateGuidId();
                    item.Id = IdHelper.CreateLongId().ToString();
                    var config = ObjectConverter.Copy<JC_AlarmNotificationPersonnelConfigInfo, JC_AlarmnotificationpersonnelconfigModel>(item);
                    var personnel = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>(addPersonnelParameter);
                    foreach (var p in personnel)
                    {
                        p.Id = IdHelper.CreateLongId().ToString();
                        p.AlarmConfigId = item.Id;
                    }

                    _Repository.AddJC_AlarmNotificationPersonnelConfig(config);
                    _AlarmNotificationPersonnelRepository.AddJC_AlarmNotificationPersonnelList(personnel.ToList());
                }
            });
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.AlarmNotificationChangedKey, DateTime.Now);
            }
            var response = new BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>>();
            response.Data = addConfigParameter;
            return response;
        }
    }
}


