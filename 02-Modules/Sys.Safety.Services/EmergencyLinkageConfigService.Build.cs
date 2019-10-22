using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class EmergencyLinkageConfigService : IEmergencyLinkageConfigService
    {
        private IEmergencyLinkageConfigRepository _Repository;

        public EmergencyLinkageConfigService(IEmergencyLinkageConfigRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_EmergencyLinkageConfigInfo> AddJC_Emergencylinkageconfig(EmergencyLinkageConfigAddRequest jC_Emergencylinkageconfigrequest)
        {
            var _jC_Emergencylinkageconfig = ObjectConverter.Copy<JC_EmergencyLinkageConfigInfo, JC_EmergencylinkageconfigModel>(jC_Emergencylinkageconfigrequest.JC_EmergencyLinkageConfigInfo);
            _jC_Emergencylinkageconfig.Id = IdHelper.CreateGuidId();
            var resultjC_Emergencylinkageconfig = _Repository.AddJC_Emergencylinkageconfig(_jC_Emergencylinkageconfig);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey, DateTime.Now);
            }
            var jC_Emergencylinkageconfigresponse = new BasicResponse<JC_EmergencyLinkageConfigInfo>();
            jC_Emergencylinkageconfigresponse.Data = ObjectConverter.Copy<JC_EmergencylinkageconfigModel, JC_EmergencyLinkageConfigInfo>(resultjC_Emergencylinkageconfig);
            return jC_Emergencylinkageconfigresponse;
        }
        public BasicResponse<JC_EmergencyLinkageConfigInfo> UpdateJC_Emergencylinkageconfig(EmergencyLinkageConfigUpdateRequest jC_Emergencylinkageconfigrequest)
        {
            var _jC_Emergencylinkageconfig = ObjectConverter.Copy<JC_EmergencyLinkageConfigInfo, JC_EmergencylinkageconfigModel>(jC_Emergencylinkageconfigrequest.JC_EmergencyLinkageConfigInfo);
            _Repository.UpdateJC_Emergencylinkageconfig(_jC_Emergencylinkageconfig);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey, DateTime.Now);
            }
            var jC_Emergencylinkageconfigresponse = new BasicResponse<JC_EmergencyLinkageConfigInfo>();
            jC_Emergencylinkageconfigresponse.Data = ObjectConverter.Copy<JC_EmergencylinkageconfigModel, JC_EmergencyLinkageConfigInfo>(_jC_Emergencylinkageconfig);
            return jC_Emergencylinkageconfigresponse;
        }
        public BasicResponse DeleteJC_Emergencylinkageconfig(EmergencylinkageconfigDeleteRequest jC_Emergencylinkageconfigrequest)
        {
            _Repository.DeleteJC_Emergencylinkageconfig(jC_Emergencylinkageconfigrequest.Id);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey, DateTime.Now);
            }
            var jC_Emergencylinkageconfigresponse = new BasicResponse();
            return jC_Emergencylinkageconfigresponse;
        }
        public BasicResponse DeleteJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            _Repository.DeleteJC_EmergencylinkageconfigByAnalysisModelId(emergencyLinkageConfigGetByAnalysisModelIdRequest.AnalysisModelId);
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey] = DateTime.Now;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(DataContract.UserRoleAuthorize.KeyConst.EmergencyLinkageChangedKey, DateTime.Now);
            }
            var jC_Emergencylinkageconfigresponse = new BasicResponse();
            return jC_Emergencylinkageconfigresponse;
        }
        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetJC_EmergencylinkageconfigList(EmergencyLinkageConfigGetListRequest jC_Emergencylinkageconfigrequest)
        {
            var jC_Emergencylinkageconfigresponse = new BasicResponse<List<JC_EmergencyLinkageConfigInfo>>();
            jC_Emergencylinkageconfigrequest.PagerInfo.PageIndex = jC_Emergencylinkageconfigrequest.PagerInfo.PageIndex - 1;
            if (jC_Emergencylinkageconfigrequest.PagerInfo.PageIndex < 0)
            {
                jC_Emergencylinkageconfigrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_EmergencylinkageconfigModelLists = _Repository.GetJC_EmergencylinkageconfigList(jC_Emergencylinkageconfigrequest.PagerInfo.PageIndex, jC_Emergencylinkageconfigrequest.PagerInfo.PageSize, out rowcount);
            var jC_EmergencylinkageconfigInfoLists = new List<JC_EmergencyLinkageConfigInfo>();
            foreach (var item in jC_EmergencylinkageconfigModelLists)
            {
                var JC_EmergencyLinkageConfigInfo = ObjectConverter.Copy<JC_EmergencylinkageconfigModel, JC_EmergencyLinkageConfigInfo>(item);
                jC_EmergencylinkageconfigInfoLists.Add(JC_EmergencyLinkageConfigInfo);
            }
            jC_Emergencylinkageconfigresponse.Data = jC_EmergencylinkageconfigInfoLists;
            return jC_Emergencylinkageconfigresponse;
        }

        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigByAnalysisModelId(EmergencyLinkageConfigGetByAnalysisModelIdRequest emergencyLinkageConfigGetByAnalysisModelIdRequest)
        {
            var jC_Emergencylinkageconfigresponse = new BasicResponse<JC_EmergencyLinkageConfigInfo>();
            var jC_EmergencylinkageconfigModel = _Repository.GetJC_EmergencylinkageconfigByAnalysisModelId(emergencyLinkageConfigGetByAnalysisModelIdRequest.AnalysisModelId);
            if(jC_EmergencylinkageconfigModel != null)
                jC_Emergencylinkageconfigresponse.Data = ObjectConverter.Copy<JC_EmergencylinkageconfigModel, JC_EmergencyLinkageConfigInfo>(jC_EmergencylinkageconfigModel);
            return jC_Emergencylinkageconfigresponse;
        }

      
        public BasicResponse<JC_EmergencyLinkageConfigInfo> GetJC_EmergencylinkageconfigById(EmergencyLinkageConfigGetRequest jC_Emergencylinkageconfigrequest)
        {
            var result = _Repository.GetJC_EmergencylinkageconfigById(jC_Emergencylinkageconfigrequest.Id);
            var jC_EmergencylinkageconfigInfo = ObjectConverter.Copy<JC_EmergencylinkageconfigModel, JC_EmergencyLinkageConfigInfo>(result);
            var jC_Emergencylinkageconfigresponse = new BasicResponse<JC_EmergencyLinkageConfigInfo>();
            jC_Emergencylinkageconfigresponse.Data = jC_EmergencylinkageconfigInfo;
            return jC_Emergencylinkageconfigresponse;
        }

        public BasicResponse<List<JC_EmergencyLinkageConfigInfo>> GetEmergencyLinkageConfigAllList(GetAllEmergencyLinkageConfigRequest getAllEmergencyLinkageConfigRequest)
        {
            BasicResponse<List<JC_EmergencyLinkageConfigInfo>> response = new BasicResponse<List<JC_EmergencyLinkageConfigInfo>>();
            DataTable dtResult = _Repository.QueryTable("dataAnalysis_EmergencyLinkageConfigAllList");
            response.Data = ObjectConverter.Copy<JC_EmergencyLinkageConfigInfo>(dtResult);
            return response;
        }
    }
}


