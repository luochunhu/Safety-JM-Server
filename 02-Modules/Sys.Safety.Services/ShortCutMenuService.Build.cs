using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.ShortCutMenu;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataAccess;

namespace Sys.Safety.Services
{
    public partial class ShortCutMenuService : IShortCutMenuService
    {
        private IShortCutMenuRepository _Repository;
        private IUserRepository _UserRepository;

        public ShortCutMenuService(IShortCutMenuRepository _Repository, IUserRepository UserRepository)
        {
            this._Repository = _Repository;
            this._UserRepository = UserRepository;
        }
        public BasicResponse<ShortCutMenuInfo> AddShortCutMenu(ShortCutMenuAddRequest shortCutMenuRequest)
        {
            var _shortCutMenu = ObjectConverter.Copy<ShortCutMenuInfo, ShortCutMenuModel>(shortCutMenuRequest.ShortCutMenuInfo);
            var resultshortCutMenu = _Repository.AddShortCutMenu(_shortCutMenu);
            var shortCutMenuresponse = new BasicResponse<ShortCutMenuInfo>();
            shortCutMenuresponse.Data = ObjectConverter.Copy<ShortCutMenuModel, ShortCutMenuInfo>(resultshortCutMenu);
            return shortCutMenuresponse;
        }
        public BasicResponse<ShortCutMenuInfo> UpdateShortCutMenu(ShortCutMenuUpdateRequest shortCutMenuRequest)
        {
            var _shortCutMenu = ObjectConverter.Copy<ShortCutMenuInfo, ShortCutMenuModel>(shortCutMenuRequest.ShortCutMenuInfo);
            _Repository.UpdateShortCutMenu(_shortCutMenu);
            var shortCutMenuresponse = new BasicResponse<ShortCutMenuInfo>();
            shortCutMenuresponse.Data = ObjectConverter.Copy<ShortCutMenuModel, ShortCutMenuInfo>(_shortCutMenu);
            return shortCutMenuresponse;
        }
        public BasicResponse DeleteShortCutMenu(ShortCutMenuDeleteRequest shortCutMenuRequest)
        {
            _Repository.DeleteShortCutMenu(shortCutMenuRequest.Id);
            var shortCutMenuresponse = new BasicResponse();
            return shortCutMenuresponse;
        }

        public BasicResponse<List<ShortCutMenuInfo>> GetShortCutMenuList(ShortCutMenuGetListRequest shortCutMenuRequest)
        {
            var shortCutMenuresponse = new BasicResponse<List<ShortCutMenuInfo>>();
            shortCutMenuRequest.PagerInfo.PageIndex = shortCutMenuRequest.PagerInfo.PageIndex - 1;
            if (shortCutMenuRequest.PagerInfo.PageIndex < 0)
            {
                shortCutMenuRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var shortCutMenuModelLists = _Repository.GetShortCutMenuList(shortCutMenuRequest.PagerInfo.PageIndex, shortCutMenuRequest.PagerInfo.PageSize, out rowcount);
            var shortCutMenuInfoLists = new List<ShortCutMenuInfo>();
            foreach (var item in shortCutMenuModelLists)
            {
                var ShortCutMenuInfo = ObjectConverter.Copy<ShortCutMenuModel, ShortCutMenuInfo>(item);
                shortCutMenuInfoLists.Add(ShortCutMenuInfo);
            }
            shortCutMenuresponse.Data = shortCutMenuInfoLists;
            return shortCutMenuresponse;
        }
        public BasicResponse<ShortCutMenuInfo> GetShortCutMenuById(ShortCutMenuGetRequest shortCutMenuRequest)
        {
            var result = _Repository.GetShortCutMenuById(shortCutMenuRequest.Id);
            var shortCutMenuInfo = ObjectConverter.Copy<ShortCutMenuModel, ShortCutMenuInfo>(result);
            var shortCutMenuresponse = new BasicResponse<ShortCutMenuInfo>();
            shortCutMenuresponse.Data = shortCutMenuInfo;
            return shortCutMenuresponse;
        }

        public BasicResponse<List<ShortCutMenuInfo>> GetShortCutMenuByUserId(ShortCutMenuUserRequest shortCutMenuRequest)
        {
            var result = _Repository.Datas.Where(o => o.UserId == shortCutMenuRequest.UserId).ToList();
            var shortCutMenuInfo = ObjectConverter.CopyList<ShortCutMenuModel, ShortCutMenuInfo>(result).ToList();
            var shortCutMenuresponse = new BasicResponse<List<ShortCutMenuInfo>>();
            shortCutMenuresponse.Data = shortCutMenuInfo;
            return shortCutMenuresponse;
        }

        public BasicResponse<bool> BatchInsetShortCutMenu(ShortCutMenuBatchInsertRequest shortCutMenuRequest)
        {
            var shortCutMenuresponse = new BasicResponse<bool>();
            var shortcutmenumodels = ObjectConverter.CopyList<ShortCutMenuInfo, ShortCutMenuModel>(shortCutMenuRequest.ShortCutMenuInfos).ToList();
            _Repository.Insert(shortcutmenumodels);
            shortCutMenuresponse.Data = true;
            return shortCutMenuresponse;
        }


        public BasicResponse<bool> DeleteShortCutMenuByUserId(ShortCutMenuUserRequest shortCutMenuRequest)
        {
            var shortCutMenuresponse = new BasicResponse<bool>();
            var result = _Repository.Datas.Where(o => o.UserId == shortCutMenuRequest.UserId).ToList();
            _Repository.Delete(result);
            shortCutMenuresponse.Data = true;
            return shortCutMenuresponse;
        }
    }
}


