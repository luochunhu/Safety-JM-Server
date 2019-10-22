using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Webmenuauth;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class WebmenuauthService:IWebmenuauthService
    {
		private IWebmenuauthRepository _Repository;

		public WebmenuauthService(IWebmenuauthRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<WebmenuauthInfo> AddWebmenuauth(WebmenuauthAddRequest webmenuauthrequest)
        {
            var _webmenuauth = ObjectConverter.Copy<WebmenuauthInfo, WebmenuauthModel>(webmenuauthrequest.WebmenuauthInfo);
            var resultwebmenuauth = _Repository.AddWebmenuauth(_webmenuauth);
            var webmenuauthresponse = new BasicResponse<WebmenuauthInfo>();
            webmenuauthresponse.Data = ObjectConverter.Copy<WebmenuauthModel, WebmenuauthInfo>(resultwebmenuauth);
            return webmenuauthresponse;
        }
				public BasicResponse<WebmenuauthInfo> UpdateWebmenuauth(WebmenuauthUpdateRequest webmenuauthrequest)
        {
            var _webmenuauth = ObjectConverter.Copy<WebmenuauthInfo, WebmenuauthModel>(webmenuauthrequest.WebmenuauthInfo);
            _Repository.UpdateWebmenuauth(_webmenuauth);
            var webmenuauthresponse = new BasicResponse<WebmenuauthInfo>();
            webmenuauthresponse.Data = ObjectConverter.Copy<WebmenuauthModel, WebmenuauthInfo>(_webmenuauth);  
            return webmenuauthresponse;
        }
				public BasicResponse DeleteWebmenuauth(WebmenuauthDeleteRequest webmenuauthrequest)
        {
            _Repository.DeleteWebmenuauth(webmenuauthrequest.Id);
            var webmenuauthresponse = new BasicResponse();            
            return webmenuauthresponse;
        }
				public BasicResponse<List<WebmenuauthInfo>> GetWebmenuauthList(WebmenuauthGetListRequest webmenuauthrequest)
        {
            var webmenuauthresponse = new BasicResponse<List<WebmenuauthInfo>>();
            webmenuauthrequest.PagerInfo.PageIndex = webmenuauthrequest.PagerInfo.PageIndex - 1;
            if (webmenuauthrequest.PagerInfo.PageIndex < 0)
            {
                webmenuauthrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var webmenuauthModelLists = _Repository.GetWebmenuauthList(webmenuauthrequest.PagerInfo.PageIndex, webmenuauthrequest.PagerInfo.PageSize, out rowcount);
            var webmenuauthInfoLists = new List<WebmenuauthInfo>();
            foreach (var item in webmenuauthModelLists)
            {
                var WebmenuauthInfo = ObjectConverter.Copy<WebmenuauthModel, WebmenuauthInfo>(item);
                webmenuauthInfoLists.Add(WebmenuauthInfo);
            }
            webmenuauthresponse.Data = webmenuauthInfoLists;
            return webmenuauthresponse;
        }
				public BasicResponse<WebmenuauthInfo> GetWebmenuauthById(WebmenuauthGetRequest webmenuauthrequest)
        {
            var result = _Repository.GetWebmenuauthById(webmenuauthrequest.Id);
            var webmenuauthInfo = ObjectConverter.Copy<WebmenuauthModel, WebmenuauthInfo>(result);
            var webmenuauthresponse = new BasicResponse<WebmenuauthInfo>();
            webmenuauthresponse.Data = webmenuauthInfo;
            return webmenuauthresponse;
        }
	}
}


