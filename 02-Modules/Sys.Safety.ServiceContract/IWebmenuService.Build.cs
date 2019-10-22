using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Webmenu;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IWebmenuService
    {
        BasicResponse<WebmenuInfo> AddWebmenu(WebmenuAddRequest webmenurequest);
        BasicResponse<WebmenuInfo> UpdateWebmenu(WebmenuUpdateRequest webmenurequest);
        BasicResponse DeleteWebmenu(WebmenuDeleteRequest webmenurequest);
        BasicResponse<List<WebmenuInfo>> GetWebmenuList(WebmenuGetListRequest webmenurequest);
        BasicResponse<WebmenuInfo> GetWebmenuById(WebmenuGetRequest webmenurequest);
        /// <summary>
        /// 获取所有web菜单
        /// </summary>
        /// <param name="webmenurequest"></param>
        /// <returns></returns>
        BasicResponse<List<WebmenuInfo>> GetWebmenuListByUserCode(WebmunuGetListByUserCodeRequest webmenurequest);
        /// <summary>
        /// 批量添加web菜单
        /// </summary>
        /// <param name="webmenurequest"></param>
        /// <returns></returns>
        BasicResponse<bool> BatchInsertWebMenus(WebmenuBatchInsertRequest webmenurequest);
        /// <summary>
        /// 批量删除web菜单
        /// </summary>
        /// <param name="webmenurequest"></param>
        /// <returns></returns>
        BasicResponse<bool> BatchDeleteWebMenus(WebmenuBatchDeleteRequest webmenurequest);

        BasicResponse<List<WebmenuInfo>> GetAllWebMenuInfos(BasicRequest webmenurequest);
    }
}

