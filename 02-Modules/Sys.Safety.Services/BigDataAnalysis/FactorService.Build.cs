using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Factor;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class FactorService : IFactorService
    {
        private IFactorRepository _Repository;

        public FactorService(IFactorRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_FactorInfo> AddJC_Factor(FactorAddRequest jC_Factorrequest)
        {
            var _jC_Factor = ObjectConverter.Copy<JC_FactorInfo, JC_FactorModel>(jC_Factorrequest.JC_FactorInfo);
            var resultjC_Factor = _Repository.AddJC_Factor(_jC_Factor);
            var jC_Factorresponse = new BasicResponse<JC_FactorInfo>();
            jC_Factorresponse.Data = ObjectConverter.Copy<JC_FactorModel, JC_FactorInfo>(resultjC_Factor);
            return jC_Factorresponse;
        }
        public BasicResponse<JC_FactorInfo> UpdateJC_Factor(FactorUpdateRequest jC_Factorrequest)
        {
            var _jC_Factor = ObjectConverter.Copy<JC_FactorInfo, JC_FactorModel>(jC_Factorrequest.JC_FactorInfo);
            _Repository.UpdateJC_Factor(_jC_Factor);
            var jC_Factorresponse = new BasicResponse<JC_FactorInfo>();
            jC_Factorresponse.Data = ObjectConverter.Copy<JC_FactorModel, JC_FactorInfo>(_jC_Factor);
            return jC_Factorresponse;
        }
        public BasicResponse DeleteJC_Factor(FactorDeleteRequest jC_Factorrequest)
        {
            _Repository.DeleteJC_Factor(jC_Factorrequest.Id);
            var jC_Factorresponse = new BasicResponse();
            return jC_Factorresponse;
        }
        public BasicResponse<List<JC_FactorInfo>> GetJC_FactorList(FactorGetListRequest jC_Factorrequest)
        {
            var jC_Factorresponse = new BasicResponse<List<JC_FactorInfo>>();
            
            var jC_FactorModelLists = _Repository.GetJC_FactorList();
            var jC_FactorInfoLists = new List<JC_FactorInfo>();
            foreach (var item in jC_FactorModelLists)
            {
                var JC_FactorInfo = ObjectConverter.Copy<JC_FactorModel, JC_FactorInfo>(item);
                jC_FactorInfoLists.Add(JC_FactorInfo);
            }
            jC_Factorresponse.Data = jC_FactorInfoLists;
            return jC_Factorresponse;
        }
        public BasicResponse<JC_FactorInfo> GetJC_FactorById(FactorGetRequest jC_Factorrequest)
        {
            var result = _Repository.GetJC_FactorById(jC_Factorrequest.Id);
            var jC_FactorInfo = ObjectConverter.Copy<JC_FactorModel, JC_FactorInfo>(result);
            var jC_Factorresponse = new BasicResponse<JC_FactorInfo>();
            jC_Factorresponse.Data = jC_FactorInfo;
            return jC_Factorresponse;
        }
    }
}


