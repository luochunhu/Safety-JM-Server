using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.RegionOutageConfig;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Data;
using System.Data;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.ManualCrossControl;

namespace Sys.Safety.Services
{
    public partial class RegionOutageConfigService : IRegionOutageConfigService
    {
        private IRegionOutageConfigRepository _Repository;
        private ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService;
        private IManualCrossControlService manualCrossControlService;
        private IPointDefineCacheService pointDefineCacheService;

        public RegionOutageConfigService(IRegionOutageConfigRepository _Repository,
            ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService,
            IManualCrossControlService manualCrossControlService,
            IPointDefineCacheService pointDefineCacheService)
        {
            this._Repository = _Repository;
            this.largeDataAnalysisConfigCacheService = largeDataAnalysisConfigCacheService;
            this.manualCrossControlService = manualCrossControlService;
            this.pointDefineCacheService = pointDefineCacheService;
        }
        public BasicResponse<JC_RegionOutageConfigInfo> AddJC_Regionoutageconfig(RegionOutageConfigAddRequest jC_Regionoutageconfigrequest)
        {
            var _jC_Regionoutageconfig = ObjectConverter.Copy<JC_RegionOutageConfigInfo, JC_RegionoutageconfigModel>(jC_Regionoutageconfigrequest.JC_RegionOutageConfigInfo);
            var resultjC_Regionoutageconfig = _Repository.AddJC_Regionoutageconfig(_jC_Regionoutageconfig);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey, DateTime.Now);
            }
            var jC_Regionoutageconfigresponse = new BasicResponse<JC_RegionOutageConfigInfo>();
            jC_Regionoutageconfigresponse.Data = ObjectConverter.Copy<JC_RegionoutageconfigModel, JC_RegionOutageConfigInfo>(resultjC_Regionoutageconfig);
            return jC_Regionoutageconfigresponse;
        }

        /// <summary>
        /// 批量新增区域断点设置
        /// </summary>
        /// <param name="jC_RegionOutageConfigListAddRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_RegionOutageConfigInfo>> AddJC_RegionOutageConfigList(RegionOutageConfigListAddRequest jC_RegionOutageConfigListAddRequest)
        {
            string analysisModelId = jC_RegionOutageConfigListAddRequest.AnalysisModelId;
            var _jC_RegionOutageConfig = ObjectConverter.CopyList<JC_RegionOutageConfigInfo, JC_RegionoutageconfigModel>(jC_RegionOutageConfigListAddRequest.JC_RegionOutageConfigInfoList);
            List<JC_RegionoutageconfigModel> updateList = _jC_RegionOutageConfig == null ? new List<JC_RegionoutageconfigModel>() : _jC_RegionOutageConfig.ToList();
            List<JC_RegionoutageconfigModel> regionOutageByModelList = _Repository.GetRegionOutageConfigListByAnalysisModelId(analysisModelId);
            List<JC_RegionoutageconfigModel> removeList = new List<JC_RegionoutageconfigModel>();
            List<Jc_JcsdkzInfo> removeSDKZControlList = new List<Jc_JcsdkzInfo>();


            if (regionOutageByModelList != null && regionOutageByModelList.Count > 0)
            {
                var oriControlList = regionOutageByModelList.Where(q => q.ControlStatus == 1).ToList();
                //var oriRemoveControlList = regionOutageByModelList.Where(q => q.ControlStatus == 0).ToList();
                foreach (var item in oriControlList)
                {
                    if (!updateList.Where(q => q.ControlStatus == 1).ToList().Exists(q => q.PointId == item.PointId))
                    {
                        removeList.Add(item);
                    }
                }
                /*
                foreach (var item in oriRemoveControlList)
                {
                    if (!updateList.Where(q => q.ControlStatus == 0).ToList().Exists(q => q.PointId == item.PointId))
                    {
                        var switchModelId = ObjectConverter.Copy<JC_RegionoutageconfigModel, JC_RegionoutageconfigModel>(item);
                        switchModelId.AnalysisModelId = switchModelId.RemoveModelId;
                        removeList.Add(switchModelId);
                    }
                }
                */
            }
            if (removeList.Count > 0)
            {
                IEnumerable<IGrouping<string, JC_RegionoutageconfigModel>> removeSDKZModelGroup = removeList.GroupBy(p => p.AnalysisModelId);
                foreach (var removeSDKZModelList in removeSDKZModelGroup)
                {
                    string queryAnalysisModelId = removeSDKZModelList.FirstOrDefault().AnalysisModelId;
                    BasicResponse<List<Jc_JcsdkzInfo>> analysisZKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPoint(new ManualCrossControlGetByTypeZkPointRequest() { ZkPoint = queryAnalysisModelId, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
                    if (analysisZKResponse != null && analysisZKResponse.Data != null && analysisZKResponse.Data.Count > 0)
                    {
                        foreach (var removeSDKZModel in removeSDKZModelList)
                        {
                            BasicResponse<Jc_DefInfo> cachedPoint = pointDefineCacheService.PointDefineCacheByPointIdRequeest(new Sys.Safety.Request.Cache.PointDefineCacheByPointIdRequeest() { PointID = removeSDKZModel.PointId });
                            if (null != cachedPoint && cachedPoint.Data != null)
                            {
                                Jc_JcsdkzInfo removeSDKZInfo = analysisZKResponse.Data.FirstOrDefault(q => q.Bkpoint == cachedPoint.Data.Point);
                                if (null != removeSDKZInfo)
                                    removeSDKZControlList.Add(removeSDKZInfo);
                            }
                        }
                    }
                }
            }

            TransactionsManager.BeginTransaction(() =>
                {
                    if (removeSDKZControlList.Count > 0)
                        manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = removeSDKZControlList });
                    _Repository.DeleteUserRoleByAnalysisModelId(analysisModelId);
                    _Repository.AddJC_RegionOutageConfigList(_jC_RegionOutageConfig.ToList());
                });
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey, DateTime.Now);
            }
            var jC_RegionOutageConfig = new BasicResponse<List<JC_RegionOutageConfigInfo>>();
            jC_RegionOutageConfig.Data = jC_RegionOutageConfigListAddRequest.JC_RegionOutageConfigInfoList;
            return jC_RegionOutageConfig;
        }

        public BasicResponse<JC_RegionOutageConfigInfo> UpdateJC_Regionoutageconfig(RegionOutageConfigUpdateRequest jC_Regionoutageconfigrequest)
        {
            var _jC_Regionoutageconfig = ObjectConverter.Copy<JC_RegionOutageConfigInfo, JC_RegionoutageconfigModel>(jC_Regionoutageconfigrequest.JC_RegionOutageConfigInfo);
            _Repository.UpdateJC_Regionoutageconfig(_jC_Regionoutageconfig);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey, DateTime.Now);
            }
            var jC_Regionoutageconfigresponse = new BasicResponse<JC_RegionOutageConfigInfo>();
            jC_Regionoutageconfigresponse.Data = ObjectConverter.Copy<JC_RegionoutageconfigModel, JC_RegionOutageConfigInfo>(_jC_Regionoutageconfig);
            return jC_Regionoutageconfigresponse;
        }
        public BasicResponse DeleteJC_Regionoutageconfig(RegionoutageconfigDeleteRequest jC_Regionoutageconfigrequest)
        {
            _Repository.DeleteJC_Regionoutageconfig(jC_Regionoutageconfigrequest.Id);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey, DateTime.Now);
            }
            var jC_Regionoutageconfigresponse = new BasicResponse();
            return jC_Regionoutageconfigresponse;
        }
        public BasicResponse DeleteJC_RegionoutageconfigByAnalysisModelId(RegionoutageconfigDeleteByAnalysisModelIdRequest jC_Regionoutageconfigrequest)
        {
            string analysisModelId = jC_Regionoutageconfigrequest.AnalysisModelId;
            DataTable dataCheckTable = _Repository.QueryTable("IsRegionOutageControlHasReleaseControl", analysisModelId);
            if (dataCheckTable != null && dataCheckTable.Rows.Count > 0)
            {
                StringBuilder modelNameBuilder = new StringBuilder();
                foreach (DataRow dr in dataCheckTable.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Name"].ToString()))
                        modelNameBuilder.Append(string.Format("{0}{1}", dr["Name"].ToString(), System.Environment.NewLine));
                }
                return new BasicResponse() { Code = -100, Message = "请先删除下列模型的解控配置, 再删除此模型的控制配置." + System.Environment.NewLine + modelNameBuilder.ToString() };
            }

            //删除区域断电配置之前看看存不存在控制，有控制需要解除控制.
            BasicResponse<List<Jc_JcsdkzInfo>> analysisZKResponse = manualCrossControlService.GetManualCrossControlByTypeZkPoint(new ManualCrossControlGetByTypeZkPointRequest() { ZkPoint = analysisModelId, Type = (short)Enums.ControlType.LargeDataAnalyticsAreaPowerOff });
            if (analysisZKResponse != null && analysisZKResponse.Data != null && analysisZKResponse.Data.Count > 0)
            {
                manualCrossControlService.DeleteManualCrossControls(new ManualCrossControlsRequest() { ManualCrossControlInfos = analysisZKResponse.Data });
            }
            _Repository.DeleteUserRoleByAnalysisModelId(jC_Regionoutageconfigrequest.AnalysisModelId);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.RegionOutageChangedKey, DateTime.Now);
            }
            var jC_Regionoutageconfigresponse = new BasicResponse();
            return jC_Regionoutageconfigresponse;
        }
        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetJC_RegionoutageconfigList(RegionOutageConfigGetListRequest jC_Regionoutageconfigrequest)
        {
            var jC_Regionoutageconfigresponse = new BasicResponse<List<JC_RegionOutageConfigInfo>>();
            jC_Regionoutageconfigrequest.PagerInfo.PageIndex = jC_Regionoutageconfigrequest.PagerInfo.PageIndex - 1;
            if (jC_Regionoutageconfigrequest.PagerInfo.PageIndex < 0)
            {
                jC_Regionoutageconfigrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_RegionoutageconfigModelLists = _Repository.GetJC_RegionoutageconfigList(jC_Regionoutageconfigrequest.PagerInfo.PageIndex, jC_Regionoutageconfigrequest.PagerInfo.PageSize, out rowcount);
            var jC_RegionoutageconfigInfoLists = new List<JC_RegionOutageConfigInfo>();
            foreach (var item in jC_RegionoutageconfigModelLists)
            {
                var JC_RegionOutageConfigInfo = ObjectConverter.Copy<JC_RegionoutageconfigModel, JC_RegionOutageConfigInfo>(item);
                jC_RegionoutageconfigInfoLists.Add(JC_RegionOutageConfigInfo);
            }
            jC_Regionoutageconfigresponse.Data = jC_RegionoutageconfigInfoLists;
            return jC_Regionoutageconfigresponse;
        }
        public BasicResponse<JC_RegionOutageConfigInfo> GetJC_RegionoutageconfigById(RegionOutageConfigGetRequest jC_Regionoutageconfigrequest)
        {
            var result = _Repository.GetJC_RegionoutageconfigById(jC_Regionoutageconfigrequest.Id);
            var jC_RegionoutageconfigInfo = ObjectConverter.Copy<JC_RegionoutageconfigModel, JC_RegionOutageConfigInfo>(result);
            var jC_Regionoutageconfigresponse = new BasicResponse<JC_RegionOutageConfigInfo>();
            jC_Regionoutageconfigresponse.Data = jC_RegionoutageconfigInfo;
            return jC_Regionoutageconfigresponse;
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigListByAnalysisModelId(RegionOutageConfigGetListRequest regionOutageConfigGetListRequest)
        {
            if (string.IsNullOrWhiteSpace(regionOutageConfigGetListRequest.AnalysisModelId))
                return new BasicResponse<List<JC_RegionOutageConfigInfo>>() { Code = 1, Message = "模型ID为空" };

            DataTable dataTable = _Repository.QueryTable("global_RegionOutageConfigService_GetAllRegionOutageConfigListByAnalysisModelId", regionOutageConfigGetListRequest.AnalysisModelId);

            List<JC_RegionOutageConfigInfo> listResult = ObjectConverter.Copy<JC_RegionOutageConfigInfo>(dataTable);

            var jC_AnalyticalExpressionresponse = new BasicResponse<List<JC_RegionOutageConfigInfo>>();
            jC_AnalyticalExpressionresponse.Data = listResult;


            return jC_AnalyticalExpressionresponse;
        }

        public BasicResponse NoReleaseControlForAnalysysModelAndPoint(ReleaseControlCheckRequest releaseControlCheckRequest)
        {
            BasicResponse response = new BasicResponse();
            DataTable dataTable = _Repository.QueryTable("IsRegionOutageControlPointHasReleaseControl", releaseControlCheckRequest.AnalysisModelId, releaseControlCheckRequest.PointId);
            StringBuilder modelNameBuilder = new StringBuilder();
            foreach (DataRow dr in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Name"].ToString()))
                    modelNameBuilder.Append(string.Format("{0}{1}", dr["Name"].ToString(), System.Environment.NewLine));
            }
            if (!string.IsNullOrEmpty(modelNameBuilder.ToString()))
            {
                response.Code = -100;
                response.Message = "请先删除下列模型配置的解控测点!" + System.Environment.NewLine + modelNameBuilder.ToString();
            }
            return response;
        }

        public BasicResponse<bool> HasRegionOutageForAnalysisModel(GetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();
            response.Data = _Repository.HasRegionOutageForAnalysisModel(getByAnalysisModelIdRequest.AnalysisModelId);
            return response;
        }

        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigAllList(GetAllRegionOutageConfigRequest getAllRegionOutageConfigRequest)
        {
            BasicResponse<List<JC_RegionOutageConfigInfo>> response = new BasicResponse<List<JC_RegionOutageConfigInfo>>();
            DataTable dtResult = _Repository.QueryTable("dataAnalysis_RegionOutageConfigAllList");
            response.Data = ObjectConverter.Copy<JC_RegionOutageConfigInfo>(dtResult);
            return response;
        }
    }
}


