using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.B_Callpointlist;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class B_CallpointlistService : IB_CallpointlistService
    {
        private IB_CallpointlistRepository _Repository;

        public B_CallpointlistService(IB_CallpointlistRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<B_CallpointlistInfo> AddB_Callpointlist(B_CallpointlistAddRequest b_CallpointlistRequest)
        {
            var _b_Callpointlist = ObjectConverter.Copy<B_CallpointlistInfo, B_CallpointlistModel>(b_CallpointlistRequest.B_CallpointlistInfo);
            var resultb_Callpointlist = _Repository.AddB_Callpointlist(_b_Callpointlist);
            var b_Callpointlistresponse = new BasicResponse<B_CallpointlistInfo>();
            b_Callpointlistresponse.Data = ObjectConverter.Copy<B_CallpointlistModel, B_CallpointlistInfo>(resultb_Callpointlist);
            return b_Callpointlistresponse;
        }
        public BasicResponse<B_CallpointlistInfo> UpdateB_Callpointlist(B_CallpointlistUpdateRequest b_CallpointlistRequest)
        {
            var _b_Callpointlist = ObjectConverter.Copy<B_CallpointlistInfo, B_CallpointlistModel>(b_CallpointlistRequest.B_CallpointlistInfo);
            _Repository.UpdateB_Callpointlist(_b_Callpointlist);
            var b_Callpointlistresponse = new BasicResponse<B_CallpointlistInfo>();
            b_Callpointlistresponse.Data = ObjectConverter.Copy<B_CallpointlistModel, B_CallpointlistInfo>(_b_Callpointlist);
            return b_Callpointlistresponse;
        }
        public BasicResponse DeleteB_Callpointlist(B_CallpointlistDeleteRequest b_CallpointlistRequest)
        {
            _Repository.DeleteB_Callpointlist(b_CallpointlistRequest.Id);
            var b_Callpointlistresponse = new BasicResponse();
            return b_Callpointlistresponse;
        }
        public BasicResponse<List<B_CallpointlistInfo>> GetB_CallpointlistList(B_CallpointlistGetListRequest b_CallpointlistRequest)
        {
            var b_Callpointlistresponse = new BasicResponse<List<B_CallpointlistInfo>>();
            b_CallpointlistRequest.PagerInfo.PageIndex = b_CallpointlistRequest.PagerInfo.PageIndex - 1;
            if (b_CallpointlistRequest.PagerInfo.PageIndex < 0)
            {
                b_CallpointlistRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var b_CallpointlistModelLists = _Repository.GetB_CallpointlistList(b_CallpointlistRequest.PagerInfo.PageIndex, b_CallpointlistRequest.PagerInfo.PageSize, out rowcount);
            var b_CallpointlistInfoLists = new List<B_CallpointlistInfo>();
            foreach (var item in b_CallpointlistModelLists)
            {
                var B_CallpointlistInfo = ObjectConverter.Copy<B_CallpointlistModel, B_CallpointlistInfo>(item);
                b_CallpointlistInfoLists.Add(B_CallpointlistInfo);
            }
            b_Callpointlistresponse.Data = b_CallpointlistInfoLists;
            return b_Callpointlistresponse;
        }
        public BasicResponse<B_CallpointlistInfo> GetB_CallpointlistById(B_CallpointlistGetRequest b_CallpointlistRequest)
        {
            var result = _Repository.GetB_CallpointlistById(b_CallpointlistRequest.Id);
            var b_CallpointlistInfo = ObjectConverter.Copy<B_CallpointlistModel, B_CallpointlistInfo>(result);
            var b_Callpointlistresponse = new BasicResponse<B_CallpointlistInfo>();
            b_Callpointlistresponse.Data = b_CallpointlistInfo;
            return b_Callpointlistresponse;
        }

        public BasicResponse<List<B_CallpointlistInfo>> GetB_CallByBCallId(B_CallpointlistGetRequest b_CallpointlistRequest)
        {
            var result = _Repository.Datas.Where(c => c.BCallId == b_CallpointlistRequest.Id).ToList();
            var callpointinfos = ObjectConverter.CopyList<B_CallpointlistModel, B_CallpointlistInfo>(result).ToList();
            var response = new BasicResponse<List<B_CallpointlistInfo>>();
            response.Data = callpointinfos;
            return response;
        }
    }
}


