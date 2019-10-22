using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Parameter;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class ParameterService : IParameterService
    {
        private IParameterRepository _Repository;

        public ParameterService(IParameterRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_ParameterInfo> AddJC_Parameter(ParameterAddRequest jC_Parameterrequest)
        {
            var _jC_Parameter = ObjectConverter.Copy<JC_ParameterInfo, JC_ParameterModel>(jC_Parameterrequest.JC_ParameterInfo);
            var resultjC_Parameter = _Repository.AddJC_Parameter(_jC_Parameter);
            var jC_Parameterresponse = new BasicResponse<JC_ParameterInfo>();
            jC_Parameterresponse.Data = ObjectConverter.Copy<JC_ParameterModel, JC_ParameterInfo>(resultjC_Parameter);
            return jC_Parameterresponse;
        }
        public BasicResponse<JC_ParameterInfo> UpdateJC_Parameter(ParameterUpdateRequest jC_Parameterrequest)
        {
            var _jC_Parameter = ObjectConverter.Copy<JC_ParameterInfo, JC_ParameterModel>(jC_Parameterrequest.JC_ParameterInfo);
            _Repository.UpdateJC_Parameter(_jC_Parameter);
            var jC_Parameterresponse = new BasicResponse<JC_ParameterInfo>();
            jC_Parameterresponse.Data = ObjectConverter.Copy<JC_ParameterModel, JC_ParameterInfo>(_jC_Parameter);
            return jC_Parameterresponse;
        }
        public BasicResponse DeleteJC_Parameter(ParameterDeleteRequest jC_Parameterrequest)
        {
            _Repository.DeleteJC_Parameter(jC_Parameterrequest.Id);
            var jC_Parameterresponse = new BasicResponse();
            return jC_Parameterresponse;
        }
        public BasicResponse<List<JC_ParameterInfo>> GetJC_ParameterList(ParameterGetListRequest jC_Parameterrequest)
        {
            var jC_Parameterresponse = new BasicResponse<List<JC_ParameterInfo>>();
         
            var jC_ParameterModelLists = _Repository.GetJC_ParameterList();
            var jC_ParameterInfoLists = new List<JC_ParameterInfo>();
            foreach (var item in jC_ParameterModelLists)
            {
                var JC_ParameterInfo = ObjectConverter.Copy<JC_ParameterModel, JC_ParameterInfo>(item);
                jC_ParameterInfoLists.Add(JC_ParameterInfo);
            }
            jC_Parameterresponse.Data = jC_ParameterInfoLists;
            return jC_Parameterresponse;
        }
        public BasicResponse<JC_ParameterInfo> GetJC_ParameterById(ParameterGetRequest jC_Parameterrequest)
        {
            var result = _Repository.GetJC_ParameterById(jC_Parameterrequest.Id);
            var jC_ParameterInfo = ObjectConverter.Copy<JC_ParameterModel, JC_ParameterInfo>(result);
            var jC_Parameterresponse = new BasicResponse<JC_ParameterInfo>();
            jC_Parameterresponse.Data = jC_ParameterInfo;
            return jC_Parameterresponse;
        }
    }
}


