using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Lighthistory;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class LighthistoryService:ILighthistoryService
    {
		private ILighthistoryRepository _Repository;

		public LighthistoryService(ILighthistoryRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<LighthistoryInfo> AddLighthistory(LighthistoryAddRequest lighthistoryrequest)
        {
            var _lighthistory = ObjectConverter.Copy<LighthistoryInfo, LighthistoryModel>(lighthistoryrequest.LighthistoryInfo);
            var resultlighthistory = _Repository.AddLighthistory(_lighthistory);
            var lighthistoryresponse = new BasicResponse<LighthistoryInfo>();
            lighthistoryresponse.Data = ObjectConverter.Copy<LighthistoryModel, LighthistoryInfo>(resultlighthistory);
            return lighthistoryresponse;
        }
				public BasicResponse<LighthistoryInfo> UpdateLighthistory(LighthistoryUpdateRequest lighthistoryrequest)
        {
            var _lighthistory = ObjectConverter.Copy<LighthistoryInfo, LighthistoryModel>(lighthistoryrequest.LighthistoryInfo);
            _Repository.UpdateLighthistory(_lighthistory);
            var lighthistoryresponse = new BasicResponse<LighthistoryInfo>();
            lighthistoryresponse.Data = ObjectConverter.Copy<LighthistoryModel, LighthistoryInfo>(_lighthistory);  
            return lighthistoryresponse;
        }
				public BasicResponse DeleteLighthistory(LighthistoryDeleteRequest lighthistoryrequest)
        {
            _Repository.DeleteLighthistory(lighthistoryrequest.Id);
            var lighthistoryresponse = new BasicResponse();            
            return lighthistoryresponse;
        }
				public BasicResponse<List<LighthistoryInfo>> GetLighthistoryList(LighthistoryGetListRequest lighthistoryrequest)
        {
            var lighthistoryresponse = new BasicResponse<List<LighthistoryInfo>>();
            lighthistoryrequest.PagerInfo.PageIndex = lighthistoryrequest.PagerInfo.PageIndex - 1;
            if (lighthistoryrequest.PagerInfo.PageIndex < 0)
            {
                lighthistoryrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var lighthistoryModelLists = _Repository.GetLighthistoryList(lighthistoryrequest.PagerInfo.PageIndex, lighthistoryrequest.PagerInfo.PageSize, out rowcount);
            var lighthistoryInfoLists = new List<LighthistoryInfo>();
            foreach (var item in lighthistoryModelLists)
            {
                var LighthistoryInfo = ObjectConverter.Copy<LighthistoryModel, LighthistoryInfo>(item);
                lighthistoryInfoLists.Add(LighthistoryInfo);
            }
            lighthistoryresponse.Data = lighthistoryInfoLists;
            return lighthistoryresponse;
        }
				public BasicResponse<LighthistoryInfo> GetLighthistoryById(LighthistoryGetRequest lighthistoryrequest)
        {
            var result = _Repository.GetLighthistoryById(lighthistoryrequest.Id);
            var lighthistoryInfo = ObjectConverter.Copy<LighthistoryModel, LighthistoryInfo>(result);
            var lighthistoryresponse = new BasicResponse<LighthistoryInfo>();
            lighthistoryresponse.Data = lighthistoryInfo;
            return lighthistoryresponse;
        }
	}
}


