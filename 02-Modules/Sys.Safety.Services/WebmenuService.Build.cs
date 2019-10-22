using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Webmenu;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class WebmenuService : IWebmenuService
    {
        private IWebmenuRepository _Repository;

        public WebmenuService(IWebmenuRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<WebmenuInfo> AddWebmenu(WebmenuAddRequest webmenurequest)
        {
            var _webmenu = ObjectConverter.Copy<WebmenuInfo, WebmenuModel>(webmenurequest.WebmenuInfo);
            var resultwebmenu = _Repository.AddWebmenu(_webmenu);
            var webmenuresponse = new BasicResponse<WebmenuInfo>();
            webmenuresponse.Data = ObjectConverter.Copy<WebmenuModel, WebmenuInfo>(resultwebmenu);
            return webmenuresponse;
        }
        public BasicResponse<WebmenuInfo> UpdateWebmenu(WebmenuUpdateRequest webmenurequest)
        {
            var _webmenu = ObjectConverter.Copy<WebmenuInfo, WebmenuModel>(webmenurequest.WebmenuInfo);
            _Repository.UpdateWebmenu(_webmenu);
            var webmenuresponse = new BasicResponse<WebmenuInfo>();
            webmenuresponse.Data = ObjectConverter.Copy<WebmenuModel, WebmenuInfo>(_webmenu);
            return webmenuresponse;
        }
        public BasicResponse DeleteWebmenu(WebmenuDeleteRequest webmenurequest)
        {
            _Repository.DeleteWebmenu(webmenurequest.Id);
            var webmenuresponse = new BasicResponse();
            return webmenuresponse;
        }
        public BasicResponse<List<WebmenuInfo>> GetWebmenuList(WebmenuGetListRequest webmenurequest)
        {
            var webmenuresponse = new BasicResponse<List<WebmenuInfo>>();
            webmenurequest.PagerInfo.PageIndex = webmenurequest.PagerInfo.PageIndex - 1;
            if (webmenurequest.PagerInfo.PageIndex < 0)
            {
                webmenurequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var webmenuModelLists = _Repository.GetWebmenuList(webmenurequest.PagerInfo.PageIndex, webmenurequest.PagerInfo.PageSize, out rowcount);
            var webmenuInfoLists = new List<WebmenuInfo>();
            foreach (var item in webmenuModelLists)
            {
                var WebmenuInfo = ObjectConverter.Copy<WebmenuModel, WebmenuInfo>(item);
                webmenuInfoLists.Add(WebmenuInfo);
            }
            webmenuresponse.Data = webmenuInfoLists;
            return webmenuresponse;
        }
        public BasicResponse<WebmenuInfo> GetWebmenuById(WebmenuGetRequest webmenurequest)
        {
            var result = _Repository.GetWebmenuById(webmenurequest.Id);
            var webmenuInfo = ObjectConverter.Copy<WebmenuModel, WebmenuInfo>(result);
            var webmenuresponse = new BasicResponse<WebmenuInfo>();
            webmenuresponse.Data = webmenuInfo;
            return webmenuresponse;
        }

        public BasicResponse<List<WebmenuInfo>> GetWebmenuListByUserCode(WebmunuGetListByUserCodeRequest webmenurequest)
        {
            var webmenuresponse = new BasicResponse<List<WebmenuInfo>>();
            var result = _Repository.QueryTable("global_GetWebmenuListByUserCode", new object[] { webmenurequest.UserCode });

            var webmenuInfoList = _Repository.ToEntityFromTable<WebmenuInfo>(result).ToList();
            webmenuresponse.Data = webmenuInfoList;

            return webmenuresponse;
        }




        public BasicResponse<bool> BatchInsertWebMenus(WebmenuBatchInsertRequest webmenurequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<bool> BatchDeleteWebMenus(WebmenuBatchDeleteRequest webmenurequest)
        {
            throw new NotImplementedException();
        }


        public BasicResponse<List<WebmenuInfo>> GetAllWebMenuInfos(BasicRequest webmenurequest)
        {
            var webmenuresponse = new BasicResponse<List<WebmenuInfo>>();
            var result = _Repository.Datas.ToList();

            var webmenuInfoList = ObjectConverter.CopyList<WebmenuModel, WebmenuInfo>(result).ToList();
            webmenuresponse.Data = webmenuInfoList;

            return webmenuresponse;
        }
    }
}


