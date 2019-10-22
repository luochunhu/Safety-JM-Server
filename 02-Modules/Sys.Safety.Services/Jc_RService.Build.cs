using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_R;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_RService : IJc_RService
    {
        private IJc_RRepository _Repository;

        public Jc_RService(IJc_RRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<Jc_RInfo> AddJc_R(Jc_RAddRequest jc_Rrequest)
        {
            var _jc_R = ObjectConverter.Copy<Jc_RInfo, Jc_RModel>(jc_Rrequest.Jc_RInfo);
            var resultjc_R = _Repository.AddJc_R(_jc_R);
            var jc_Rresponse = new BasicResponse<Jc_RInfo>();
            jc_Rresponse.Data = ObjectConverter.Copy<Jc_RModel, Jc_RInfo>(resultjc_R);
            return jc_Rresponse;
        }
        public BasicResponse<Jc_RInfo> UpdateJc_R(Jc_RUpdateRequest jc_Rrequest)
        {
            var _jc_R = ObjectConverter.Copy<Jc_RInfo, Jc_RModel>(jc_Rrequest.Jc_RInfo);
            _Repository.UpdateJc_R(_jc_R);
            var jc_Rresponse = new BasicResponse<Jc_RInfo>();
            jc_Rresponse.Data = ObjectConverter.Copy<Jc_RModel, Jc_RInfo>(_jc_R);
            return jc_Rresponse;
        }
        public BasicResponse DeleteJc_R(Jc_RDeleteRequest jc_Rrequest)
        {
            _Repository.DeleteJc_R(jc_Rrequest.Id);
            var jc_Rresponse = new BasicResponse();
            return jc_Rresponse;
        }
        public BasicResponse<List<Jc_RInfo>> GetJc_RList(Jc_RGetListRequest jc_Rrequest)
        {
            var jc_Rresponse = new BasicResponse<List<Jc_RInfo>>();
            jc_Rrequest.PagerInfo.PageIndex = jc_Rrequest.PagerInfo.PageIndex - 1;
            if (jc_Rrequest.PagerInfo.PageIndex < 0)
            {
                jc_Rrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_RModelLists = _Repository.GetJc_RList(jc_Rrequest.PagerInfo.PageIndex, jc_Rrequest.PagerInfo.PageSize, out rowcount);
            var jc_RInfoLists = new List<Jc_RInfo>();
            foreach (var item in jc_RModelLists)
            {
                var Jc_RInfo = ObjectConverter.Copy<Jc_RModel, Jc_RInfo>(item);
                jc_RInfoLists.Add(Jc_RInfo);
            }
            jc_Rresponse.Data = jc_RInfoLists;
            return jc_Rresponse;
        }
        public BasicResponse<Jc_RInfo> GetJc_RById(Jc_RGetRequest jc_Rrequest)
        {
            var result = _Repository.GetJc_RById(jc_Rrequest.Id);
            var jc_RInfo = ObjectConverter.Copy<Jc_RModel, Jc_RInfo>(result);
            var jc_Rresponse = new BasicResponse<Jc_RInfo>();
            jc_Rresponse.Data = jc_RInfo;
            return jc_Rresponse;
        }
        public BasicResponse<Jc_RInfo> GetJc_RByDataAndId(Jc_RGetByDateAndIdRequest jc_Rrequest)
        {
            var alarmTable = _Repository.QueryTable("global_GetDateRunRecordById", new object[] { "JC_R" + jc_Rrequest.Data, jc_Rrequest.Id });

            BasicResponse<Jc_RInfo> response = new BasicResponse<Jc_RInfo>();
            if (alarmTable != null && alarmTable.Rows.Count > 0)
            {
                var alarmList = _Repository.ToEntityFromTable<Jc_RInfo>(alarmTable);
                response.Data = alarmList.FirstOrDefault();
            }

            return response;
        }
    }
}


