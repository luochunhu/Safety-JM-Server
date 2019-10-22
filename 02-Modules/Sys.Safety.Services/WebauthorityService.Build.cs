using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Webauthority;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class WebauthorityService:IWebauthorityService
    {
		private IWebauthorityRepository _Repository;

		public WebauthorityService(IWebauthorityRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<WebauthorityInfo> AddWebauthority(WebauthorityAddRequest webauthorityrequest)
        {
            var _webauthority = ObjectConverter.Copy<WebauthorityInfo, WebauthorityModel>(webauthorityrequest.WebauthorityInfo);
            var resultwebauthority = _Repository.AddWebauthority(_webauthority);
            var webauthorityresponse = new BasicResponse<WebauthorityInfo>();
            webauthorityresponse.Data = ObjectConverter.Copy<WebauthorityModel, WebauthorityInfo>(resultwebauthority);
            return webauthorityresponse;
        }
				public BasicResponse<WebauthorityInfo> UpdateWebauthority(WebauthorityUpdateRequest webauthorityrequest)
        {
            var _webauthority = ObjectConverter.Copy<WebauthorityInfo, WebauthorityModel>(webauthorityrequest.WebauthorityInfo);
            _Repository.UpdateWebauthority(_webauthority);
            var webauthorityresponse = new BasicResponse<WebauthorityInfo>();
            webauthorityresponse.Data = ObjectConverter.Copy<WebauthorityModel, WebauthorityInfo>(_webauthority);  
            return webauthorityresponse;
        }
				public BasicResponse DeleteWebauthority(WebauthorityDeleteRequest webauthorityrequest)
        {
            _Repository.DeleteWebauthority(webauthorityrequest.Id);
            var webauthorityresponse = new BasicResponse();            
            return webauthorityresponse;
        }
				public BasicResponse<List<WebauthorityInfo>> GetWebauthorityList(WebauthorityGetListRequest webauthorityrequest)
        {
            var webauthorityresponse = new BasicResponse<List<WebauthorityInfo>>();
            webauthorityrequest.PagerInfo.PageIndex = webauthorityrequest.PagerInfo.PageIndex - 1;
            if (webauthorityrequest.PagerInfo.PageIndex < 0)
            {
                webauthorityrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var webauthorityModelLists = _Repository.GetWebauthorityList(webauthorityrequest.PagerInfo.PageIndex, webauthorityrequest.PagerInfo.PageSize, out rowcount);
            var webauthorityInfoLists = new List<WebauthorityInfo>();
            foreach (var item in webauthorityModelLists)
            {
                var WebauthorityInfo = ObjectConverter.Copy<WebauthorityModel, WebauthorityInfo>(item);
                webauthorityInfoLists.Add(WebauthorityInfo);
            }
            webauthorityresponse.Data = webauthorityInfoLists;
            return webauthorityresponse;
        }
				public BasicResponse<WebauthorityInfo> GetWebauthorityById(WebauthorityGetRequest webauthorityrequest)
        {
            var result = _Repository.GetWebauthorityById(webauthorityrequest.Id);
            var webauthorityInfo = ObjectConverter.Copy<WebauthorityModel, WebauthorityInfo>(result);
            var webauthorityresponse = new BasicResponse<WebauthorityInfo>();
            webauthorityresponse.Data = webauthorityInfo;
            return webauthorityresponse;
        }
	}
}


