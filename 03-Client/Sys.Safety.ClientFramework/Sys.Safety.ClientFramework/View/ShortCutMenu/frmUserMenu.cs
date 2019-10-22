using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.Request.ShortCutMenu;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.ClientFramework.View.ShortCutMenu
{
    public partial class frmUserMenu : XtraForm
    {
        private ILoginService loginService;
        private IShortCutMenuService shortcutMenuService;

        public frmUserMenu()
        {
            InitializeComponent();

            loginService = ServiceFactory.Create<ILoginService>();
            shortcutMenuService = ServiceFactory.Create<IShortCutMenuService>();

            GetUserMenus();
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        private void GetUserMenus()
        {
            //获取当前登录用户
            string UserNameNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginUserKey].ToString();
            string PasswordNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginPasswordKey].ToString();
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            string userId = _ClientItem == null ? string.Empty : _ClientItem.UserID;

            Dictionary<string, object> loginContext = new Dictionary<string, object>();

            loginContext.Add(KeyConst.LoginUserKey, UserNameNow);
            loginContext.Add(KeyConst.LoginPasswordKey, PasswordNow);
            loginContext.Add(KeyConst.UserMenuTypeKey, "1");

            LoginRequest loginrequest = new LoginRequest();
            loginrequest.loginContext = loginContext;
            var Result = loginService.Login(loginrequest);
            if (Result.Code == 1)
            {
                throw new Exception("&&" + Result.Message);
            }
            Dictionary<string, object> Rvalue = Result.Data;

            var usermenudic = JSONHelper.ParseJSONString<Dictionary<string, MenuInfo>>(Rvalue["_Menus"].ToString());
            var shortcutmenuRequest = new ShortCutMenuUserRequest();
            shortcutmenuRequest.UserId = userId;
            var shortcutmenuResponse = shortcutMenuService.GetShortCutMenuByUserId(shortcutmenuRequest);
            List<ShortCutMenuInfo> shortcutmenuinfos = new List<ShortCutMenuInfo>();
            if (shortcutmenuResponse.IsSuccess)
                shortcutmenuinfos = shortcutmenuResponse.Data;

            List<MenuInfo> usermenus = new List<MenuInfo>();
            foreach (var item in usermenudic)
            {
                var shortcutmenu = shortcutmenuinfos.FirstOrDefault(menu => menu.MenuId == item.Value.MenuID);
                item.Value.IsShortCutMenu = shortcutmenu != null;
                usermenus.Add(item.Value);
            }

            dgvUserMenu.DataSource = usermenus;

            for (int i = 0; i < UserMenuGridView.RowCount; i++)
            {
                var user = UserMenuGridView.GetRow(i) as MenuInfo;
                if (user.IsShortCutMenu)
                {
                    UserMenuGridView.SelectRow(i);
                }
            }

        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //获取当前登录用户信息
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            string userId = _ClientItem == null ? string.Empty : _ClientItem.UserID;

            //获取选中用户菜单
            int[] selectrows = UserMenuGridView.GetSelectedRows();
            List<ShortCutMenuInfo> selectMenus = new List<ShortCutMenuInfo>();

            for (int i = 0; i < selectrows.Length; i++)
            {
                var rowindex = selectrows[i];

                var menuid = UserMenuGridView.GetRowCellValue(rowindex, "MenuID");

                if (menuid != null)
                {
                    ShortCutMenuInfo shortcutmenuinfo = new ShortCutMenuInfo();
                    shortcutmenuinfo.Id = IdHelper.CreateGuidId();
                    shortcutmenuinfo.MenuId = menuid.ToString();
                    shortcutmenuinfo.UserId = userId;
                    shortcutmenuinfo.IsEnable = 1;
                    selectMenus.Add(shortcutmenuinfo);
                }
            }

            //先删除此用户的快捷菜单记录，再重新加入
            ShortCutMenuUserRequest userRequest = new ShortCutMenuUserRequest();
            userRequest.UserId = userId;
            var deleteresponse = shortcutMenuService.DeleteShortCutMenuByUserId(userRequest);
            if (deleteresponse.IsSuccess && deleteresponse.Data)
            {
                if (selectMenus.Count > 0)
                {
                    ShortCutMenuBatchInsertRequest batchinsertRequest = new ShortCutMenuBatchInsertRequest();
                    batchinsertRequest.ShortCutMenuInfos = selectMenus;
                    var batchinsertResponse = shortcutMenuService.BatchInsetShortCutMenu(batchinsertRequest);
                    if (batchinsertResponse.IsSuccess && batchinsertResponse.Data)
                    {
                        this.StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据成功";
                    }
                    else
                    {
                        this.StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败";
                    }
                }
            }
            else
            {
                this.StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败";
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void UserMenuGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
