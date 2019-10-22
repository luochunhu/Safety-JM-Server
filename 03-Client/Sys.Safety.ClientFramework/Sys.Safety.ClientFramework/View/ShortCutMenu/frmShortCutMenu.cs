using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.Request;
using Sys.Safety.Request.ShortCutMenu;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.ClientFramework.CBFCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.ClientFramework.View.ShortCutMenu
{
    public partial class frmShortCutMenu : XtraForm
    {

        [DllImport("USER32.DLL")]
        public static extern int GetSystemMenu(int hwnd, int bRevert);

        [DllImport("USER32.DLL")]
        public static extern int RemoveMenu(int hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

        const int MF_REMOVE = 0x1000;

        const int SC_RESTORE = 0xF120; //还原
        const int SC_MOVE = 0xF010; //移动
        const int SC_SIZE = 0xF000; //大小
        const int SC_MINIMIZE = 0xF020; //最小化
        const int SC_MAXIMIZE = 0xF030; //最大化
        const int SC_CLOSE = 0xF060; //关闭 

        private IShortCutMenuService shortcutmunuService;
        private ILoginService loginService;
        private IRequestService requestService;
        private IMenuService menuService;

        public frmShortCutMenu()
        {
            InitializeComponent();

            shortcutmunuService = ServiceFactory.Create<IShortCutMenuService>();
            loginService = ServiceFactory.Create<ILoginService>();
            requestService = ServiceFactory.Create<IRequestService>();
            menuService = ServiceFactory.Create<IMenuService>();
        }

        private void frmShortCutMenu_Load(object sender, EventArgs e)
        {
            int hMenu;
            hMenu = GetSystemMenu(this.Handle.ToInt32(), 0);
            RemoveMenu(hMenu, SC_MAXIMIZE, MF_REMOVE);

            //设置快捷菜单显示到左下角
            int x = Screen.PrimaryScreen.WorkingArea.Left + 8;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height - 38;
            this.Location = new Point(x, y);

            InitMenuTree();
        }

        /// <summary>
        /// 加载快捷菜单列表
        /// </summary>
        private void InitMenuTree()
        {
            try
            {
                navMenu.Groups.Clear();

                //获取当前登录用户信息
                ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                string userId = _ClientItem == null ? string.Empty : _ClientItem.UserID;

                //获取用户缓存菜单
                string UserNameNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginUserKey].ToString();
                string PasswordNow = Basic.Framework.Data.PlatRuntime.Items[KeyConst.LoginPasswordKey].ToString();
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

                //获取用户快捷菜单
                ShortCutMenuUserRequest userRequest = new ShortCutMenuUserRequest();
                userRequest.UserId = userId;
                var shortcutmenuResponse = shortcutmunuService.GetShortCutMenuByUserId(userRequest);
                if (shortcutmenuResponse.IsSuccess && shortcutmenuResponse.Data != null)
                {
                    var shortcutmenus = shortcutmenuResponse.Data;
                    List<ShortCutMenuInfo> tempshortcutmenus = new List<ShortCutMenuInfo>();

                    shortcutmenus.ForEach(menu =>
                   {
                       if (usermenudic.ContainsKey(menu.MenuId))
                       {
                           var usermenu = usermenudic[menu.MenuId];
                           if (usermenu != null)
                           {
                               menu.MenuCode = usermenu.MenuCode;
                           }
                       }
                   });

                    shortcutmenus = shortcutmenus.OrderBy(a => a.MenuCode).ToList();

                    shortcutmenus.ForEach(menu =>
                    {
                        if (usermenudic.ContainsKey(menu.MenuId))
                        {
                            var usermenu = usermenudic[menu.MenuId];
                            if (usermenu != null)
                            {
                                menu.MenuName = usermenu.MenuName;
                                menu.MenuCode = usermenu.MenuCode;
                                menu.MenuParentCode = usermenu.MenuParent;
                                menu.RequestCode = usermenu.RequestCode;
                                if (!string.IsNullOrEmpty(usermenu.MenuSmallIcon))
                                    menu.MenuImage = usermenu.MenuSmallIcon;
                                else if (!string.IsNullOrEmpty(usermenu.MenuLargeIcon))
                                    menu.MenuImage = usermenu.MenuLargeIcon;

                                tempshortcutmenus.Add(menu);
                            }
                        }
                    });

                    var parentmenus = tempshortcutmenus.Where(o => (!string.IsNullOrEmpty(o.MenuCode)) && o.MenuCode.Length == 3).ToList();

                    var imagepath = Application.StartupPath + "\\Image\\WorkImage";

                    parentmenus.ForEach(pm =>
                    {
                        //创建一级菜单
                        NavBarGroup menugroup = new NavBarGroup();
                        menugroup.Name = pm.MenuCode;
                        menugroup.Tag = usermenudic[pm.MenuId];
                        menugroup.Caption = pm.MenuName;
                        //menugroup.SmallImage = Image.FromFile(imagepath+"\\"+pm.MenuImage);
                        menugroup.SmallImageSize = new System.Drawing.Size(16, 16);
                        navMenu.Groups.Add(menugroup);

                        //创建子项菜单 对应菜单表三级菜单
                        var menuitems = tempshortcutmenus.Where(menu =>
                        {
                            var pcode = menu.MenuCode.Substring(0, 3);
                            if (pcode == pm.MenuCode && menu.MenuCode.Length > 6)
                                return true;
                            return false;
                        }).ToList();

                        menuitems.ForEach(cp =>
                        {
                            NavBarItem menuitem = new NavBarItem();
                            menuitem.Name = cp.MenuCode;
                            menuitem.Tag = usermenudic[cp.MenuId];
                            if (string.IsNullOrEmpty(usermenudic[cp.MenuId].Remark4))
                            {
                                menuitem.Caption = cp.MenuName;
                            }
                            else
                            {
                                menuitem.Caption = cp.MenuName + "(" + usermenudic[cp.MenuId].Remark4 + ")";
                            }
                            if (!string.IsNullOrEmpty(cp.MenuImage))
                                menuitem.SmallImage = Image.FromFile(imagepath + "\\" + cp.MenuImage);
                            menuitem.SmallImageSize = new System.Drawing.Size(16, 16);
                            navMenu.Items.Add(menuitem);
                            menugroup.ItemLinks.Add(menuitem);
                        });

                    });

                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("快捷菜单加载失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navMenu_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var menuinfo = e.Link.Item.Tag as MenuInfo;
            MenuItemClick(menuinfo);
        }

        private void MenuItemClick(MenuInfo menuinfo)
        {
            try
            {
                if (menuinfo.RequestCode != null)
                {
                    RequestGetByCodeRequest request = new RequestGetByCodeRequest();
                    request.Code = menuinfo.RequestCode;
                    var requestResponse = requestService.GetRequestByCode(request);
                    if (requestResponse.IsSuccess && requestResponse.Data != null)
                    {

                        RequestInfo requestinfo = requestResponse.Data;


                        #region 加载请求库的参数信息
                        Dictionary<string, string> param = null;
                        if (!string.IsNullOrEmpty(requestinfo.MenuParams))
                        {
                            if (Convert.ToString(requestinfo.MenuParams).Contains("&"))
                            {
                                string[] ModuleParams = Convert.ToString(requestinfo.MenuParams).Split('&');

                                param = new Dictionary<string, string>();

                                for (int i = 0; i < ModuleParams.Length; i++)
                                {
                                    if (ModuleParams[i].Split('=').Length > 0)
                                    {
                                        param.Add(ModuleParams[i].Split('=')[0], ModuleParams[i].Split('=')[1]);
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(requestinfo.MenuParams)))
                                {
                                    string[] ModuleParams = new string[1];//参数传递

                                    param = new Dictionary<string, string>();

                                    ModuleParams[0] = requestinfo.MenuParams;

                                    param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                                }
                            }
                        }
                        #endregion

                        RequestUtil.ExcuteCommand(requestinfo.RequestCode.ToString(), param, menuinfo.IsSystemDesktop == 1);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ShortCutMenu_LoadPage" + ex.Message + ex.StackTrace);
            }
        }

        private void frmShortCutMenu_KeyDown(object sender, KeyEventArgs e)
        {
            char mychar = 'A';
            //Keys k1 = (Keys)mychar;
            if (e.Alt)//e.KeyCode == k1 &&
            {
                List<MenuInfo> menuList = menuService.GetMenuList().Data.FindAll(a => a.Remark4 != null && a.Remark4.Contains("ALT+"));
                foreach (MenuInfo menu in menuList)
                {
                    mychar = (menu.Remark4.Split('+')[1])[0];
                    Keys k1 = (Keys)mychar;
                    if (e.KeyCode == k1)
                    {
                        MenuItemClick(menu);
                    }
                }
                //MenuItemClick(menuinfo);
            }
        }

        private void navMenu_KeyDown(object sender, KeyEventArgs e)
        {
            char mychar = 'A';
            //Keys k1 = (Keys)mychar;
            if (e.Alt)//e.KeyCode == k1 &&
            {
                List<MenuInfo> menuList = menuService.GetMenuList().Data.FindAll(a => a.Remark4 != null && a.Remark4.Contains("ALT+"));
                foreach (MenuInfo menu in menuList)
                {
                    mychar = (menu.Remark4.Split('+')[1])[0];
                    Keys k1 = (Keys)mychar;
                    if (e.KeyCode == k1)
                    {
                        MenuItemClick(menu);
                    }
                }
                //MenuItemClick(menuinfo);
            }
        }
    }
}
