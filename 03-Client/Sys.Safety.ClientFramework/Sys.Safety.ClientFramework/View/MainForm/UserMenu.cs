using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using DevExpress.XtraTab;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using DevExpress.Utils;
using System.Runtime.Serialization;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.ClientFramework.View.MainForm
{
    /// <summary>动态加载菜单类</summary>
    internal class UserMeun
    {


        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);

        /// <summary>X轴上的单个间距为2</summary>
        private readonly Int32 _jWidth = 2;

        /// <summary>应用程序皮肤图片集合对象</summary>
        private WindowSkinPic _skinPic;

        // <summary>当前登录用户对应的模块列表</summary>
        private List<MenuInfo> table;

        /// <summary>快捷按钮的提示信息显示</summary>
        private ToolTip _suBtnTip;

        /// <summary>
        /// 要加载的菜单控件
        /// </summary>
        public MenuStrip SMenuStrip = null;

        /// <summary>
        /// 快捷按钮容器
        /// </summary>
        public PanelControl SPanelControl = null;

        IMenuService _MenuService = ServiceFactory.Create<IMenuService>();
        IRequestService _RequestService = ServiceFactory.Create<IRequestService>();

        static List<RequestInfo> RequestList = new List<RequestInfo>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="skinPic">皮肤集合对象</param>
        /// <param name="dt">当前登录用户对应的模块列表</param>
        public UserMeun(WindowSkinPic skinPic, ToolTip btnTip)
        {
            _skinPic = skinPic;

            _suBtnTip = btnTip;
        }

        /// <summary>
        /// 加载菜单、快捷菜单和快捷按钮
        /// </summary>
        /// <param name="mstrip">要加载的菜单控件</param>
        /// <param name="sbtnControl">快捷按钮容器</param>
        /// <param name="dt">当前登录用户对应的模块列表</param>
        public void ChangeMainMenuItems(MenuStrip mstrip, PanelControl sbtnControl, List<MenuInfo> dt)
        {
            try
            {
                if (null == mstrip) { throw new ArgumentNullException("菜单为空"); }

                table = dt;

                _skinPic.ReadSuBtnImage();

                //sbtnControl.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (sbtnControl.Controls.Count > 0)
                {
                    if (sbtnControl.Controls[0] != null)
                        sbtnControl.Controls[0].Dispose();
                }

                mstrip.Items.Clear();

                List<MenuInfo> rowMenus = new List<MenuInfo>();

                if (null == table || table.Count < 1 || null == (rowMenus = table.Where(p => p.MenuMemo == -1).ToList<MenuInfo>()))
                {
                    return;
                }

                ToolStripMenuItem menuItem = null;

                if (RequestList.Count == 0)
                {
                    RequestList = _RequestService.GetRequestList().Data;
                }
                List<RequestInfo> AllRequest = RequestList;

                foreach (MenuInfo row in rowMenus)
                {
                    if (null == row) { continue; }

                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    menuItem = new ToolStripMenuItem();

                    menuItem.Text = Convert.ToString(row.MenuName);//设置菜单的显示文本

                    menuItem.Name = row.MenuID.ToString();

                    mstrip.Items.Add(menuItem);

                    string bkIcon = Convert.ToString(row.MenuSmallIcon);//小图标,加载到菜单项

                    if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon)) { menuItem.Image = _skinPic.GetSuBtnImageByName(bkIcon); }

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (null != drTempN && drTempN.Count > 0)
                    {
                        LoadMenuNode(menuItem, sbtnControl, drTempN);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row.RequestCode))
                        {
                            //menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);//修改，每次调用服务端影响性能  20170324
                            menuItem.Tag = AllRequest.Find(a => a.RequestCode == row.RequestCode);
                        }
                        menuItem.Click += new EventHandler(MenuClicked);//

                        string bkImg = Convert.ToString(row.MenuLargeIcon);//大图标，加载到快捷按钮

                        if (1 == Convert.ToInt32(row.MenuStatus) && !string.IsNullOrEmpty(bkImg))
                        {
                            AddBtnInSperControl(menuItem, sbtnControl, bkImg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_ChangeMainMenuItems" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 加载dev菜单、快捷菜单和快捷按钮
        /// </summary>
        /// <param name="mstrip">要加载的菜单控件</param>       
        /// <param name="dt">当前登录用户对应的模块列表</param>
        public void ChangeDevMainMenuItems(RibbonControl mstrip, List<MenuInfo> dt)
        {
            try
            {
                if (null == mstrip) { throw new ArgumentNullException("菜单为空"); }

                table = dt;

                _skinPic.ReadSuBtnImage();

                //sbtnControl.Controls.Clear();

                mstrip.Pages.Clear();

                List<MenuInfo> rowMenus = new List<MenuInfo>();

                if (null == table || table.Count < 1 || null == (rowMenus = table.Where(p => p.MenuMemo == -1).ToList<MenuInfo>()))
                {
                    return;
                }

                RibbonPage menuItem = null;


                foreach (MenuInfo row in rowMenus)
                {
                    if (null == row) { continue; }

                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    menuItem = new RibbonPage();

                    menuItem.Text = Convert.ToString(row.MenuName);//设置菜单的显示文本

                    menuItem.Name = row.MenuID.ToString();



                    mstrip.Pages.Add(menuItem);

                    string bkIcon = Convert.ToString(row.MenuSmallIcon);//小图标,加载到菜单项

                    if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon)) { menuItem.Image = _skinPic.GetSuBtnImageByName(bkIcon); }

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (null != drTempN && drTempN.Count > 0)
                    {
                        LoadDevMenu(menuItem, drTempN);
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(row.RequestCode))
                        //{
                        //    menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);
                        //}
                        //menuItem.Click += new EventHandler(MenuClicked);//

                        //string bkImg = Convert.ToString(row.MenuLargeIcon);//大图标，加载到快捷按钮

                        //if (1 == Convert.ToInt32(row.MenuStatus) && !string.IsNullOrEmpty(bkImg))
                        //{
                        //    AddBtnInSperControl(menuItem, sbtnControl, bkImg);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_ChangeDevMainMenuItems" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 加载dev菜单下面的子菜单项
        /// </summary>
        /// <param name="OneItem">父菜单项</param>       
        /// <param name="drTemp">子菜单项数据行</param>
        private void LoadDevMenu(RibbonPage OneItem, List<MenuInfo> drTemp)
        {
            try
            {
                DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup = null;
                if (RequestList.Count == 0)
                {
                    RequestList = _RequestService.GetRequestList().Data;
                }
                List<RequestInfo> AllRequest = RequestList;

                foreach (MenuInfo row in drTemp)
                {
                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (null != drTempN && drTempN.Count > 0)
                    {

                        ribbonPageGroup = new RibbonPageGroup();
                        ribbonPageGroup.Text = Convert.ToString(row.MenuName);
                        OneItem.Groups.Add(ribbonPageGroup);

                        LoadDevMenuNode(ribbonPageGroup, drTempN);
                    }
                    else
                    {
                        ribbonPageGroup = new RibbonPageGroup();
                        ribbonPageGroup.Text = Convert.ToString(row.MenuName);
                        OneItem.Groups.Add(ribbonPageGroup);

                        if (!string.IsNullOrEmpty(row.MenuURL))
                        {
                            BarButtonItem menuItem = null;

                            menuItem = new BarButtonItem();

                            menuItem.Caption = Convert.ToString(row.Remark1);

                            menuItem.Description = Convert.ToString(row.MenuName);

                            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
                            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
                            toolTipTitleItem1.Text = Convert.ToString(row.MenuName);
                            superToolTip2.Items.Add(toolTipTitleItem1);
                            menuItem.SuperTip = superToolTip2;

                            menuItem.Name = row.MenuID.ToString();

                            if (!string.IsNullOrEmpty(row.RequestCode))
                            {
                                //menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);//修改，每次调用服务端影响性能  20170324
                                menuItem.Tag = AllRequest.Find(a => a.RequestCode == row.RequestCode);

                            }
                            menuItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BarButtonItemMenuClicked);

                            ribbonPageGroup.ItemLinks.Add(menuItem);
                            OneItem.Groups.Add(ribbonPageGroup);

                            string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                            if (!string.IsNullOrEmpty(bkIcon))
                            {
                                if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                                {
                                    menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                    menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                }
                                else
                                {
                                    menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName("default.png");
                                    menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                                }

                            }
                            else
                            {
                                bkIcon = Convert.ToString(row.MenuSmallIcon);//图标,加载到菜单项
                                if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                                {
                                    menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                }
                                else
                                {
                                    menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                                }
                            }

                            //判断菜单是事为桌面菜单，如果是在桌面进行加载
                            if (row.IsSystemDesktop == 1)
                            {
                                BarButtonLoadPages(menuItem, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_LoadDevMenuNode" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 加载dev菜单下面的子菜单项
        /// </summary>
        /// <param name="OneItem">父菜单项</param>       
        /// <param name="drTemp">子菜单项数据行</param>
        private void LoadDevMenuNode(RibbonPageGroup OneItem, List<MenuInfo> drTemp)
        {
            try
            {


                foreach (MenuInfo row in drTemp)
                {
                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (RequestList.Count == 0)
                    {
                        RequestList = _RequestService.GetRequestList().Data;
                    }
                    List<RequestInfo> AllRequest = RequestList;

                    if (null != drTempN && drTempN.Count > 0)
                    {
                        BarSubItem menuItem = null;

                        menuItem = new BarSubItem();

                        menuItem.Caption = Convert.ToString(row.Remark1);

                        menuItem.Description = Convert.ToString(row.MenuName);

                        DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
                        DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
                        toolTipTitleItem1.Text = Convert.ToString(row.MenuName);
                        superToolTip2.Items.Add(toolTipTitleItem1);
                        menuItem.SuperTip = superToolTip2;

                        menuItem.Name = row.MenuID.ToString();

                        OneItem.ItemLinks.Add(menuItem);

                        string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                        if (!string.IsNullOrEmpty(bkIcon))
                        {
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName("default.png");
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }

                        }
                        else
                        {
                            bkIcon = Convert.ToString(row.MenuSmallIcon);//图标,加载到菜单项
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }
                        }

                        LoadDevSonMenuNode(menuItem, drTempN);
                    }
                    else
                    {
                        BarButtonItem menuItem = null;

                        menuItem = new BarButtonItem();

                        menuItem.Caption = Convert.ToString(row.Remark1);

                        menuItem.Description = Convert.ToString(row.MenuName);

                        DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
                        DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
                        toolTipTitleItem1.Text = Convert.ToString(row.MenuName);
                        superToolTip2.Items.Add(toolTipTitleItem1);
                        menuItem.SuperTip = superToolTip2;

                        menuItem.Name = row.MenuID.ToString();

                        if (!string.IsNullOrEmpty(row.RequestCode))
                        {
                            //menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);//修改，每次调用服务端影响性能  20170324
                            menuItem.Tag = AllRequest.Find(a => a.RequestCode == row.RequestCode);
                        }
                        menuItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BarButtonItemMenuClicked);

                        OneItem.ItemLinks.Add(menuItem);

                        string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                        if (!string.IsNullOrEmpty(bkIcon))
                        {
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName("default.png");
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }

                        }
                        else
                        {
                            bkIcon = Convert.ToString(row.MenuSmallIcon);//图标,加载到菜单项
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }
                        }
                        //if (!string.IsNullOrEmpty(row.RequestCode))
                        //{
                        //    menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);
                        //}
                        //menuItem.Click += new EventHandler(MenuClicked);

                        //string bkImg = Convert.ToString(row.MenuLargeIcon);//大图标，加载到快捷按钮
                        //if (1 == Convert.ToInt32(row.MenuStatus) && !string.IsNullOrEmpty(bkImg))
                        //{
                        //    AddBtnInSperControl(menuItem,  bkImg);
                        //}

                        //判断菜单是事为桌面菜单，如果是在桌面进行加载
                        if (row.IsSystemDesktop == 1)
                        {
                            BarButtonLoadPages(menuItem, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_LoadDevMenuNode" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 加载dev子菜单下面的子菜单项
        /// </summary>
        /// <param name="OneItem">父菜单项</param>       
        /// <param name="drTemp">子菜单项数据行</param>
        private void LoadDevSonMenuNode(BarSubItem OneItem, List<MenuInfo> drTemp)
        {
            try
            {
                foreach (MenuInfo row in drTemp)
                {
                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (RequestList.Count == 0)
                    {
                        RequestList = _RequestService.GetRequestList().Data;
                    }
                    List<RequestInfo> AllRequest = RequestList;

                    if (null != drTempN && drTempN.Count > 0)
                    {
                        BarSubItem menuItem = null;

                        menuItem = new BarSubItem();

                        menuItem.Caption = Convert.ToString(row.MenuName);

                        menuItem.Name = row.MenuID.ToString();

                        OneItem.ItemLinks.Add(menuItem);

                        string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                        if (!string.IsNullOrEmpty(bkIcon))
                        {
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.LargeGlyph = _skinPic.GetSuBtnImageByName("default.png");
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }

                        }
                        else
                        {
                            bkIcon = Convert.ToString(row.MenuSmallIcon);//图标,加载到菜单项
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                menuItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }
                        }

                        //判断菜单是事为桌面菜单，如果是在桌面进行加载
                        if (row.IsSystemDesktop == 1)
                        {
                            SubItemLoadPages(menuItem, true);
                        }

                        LoadDevSonMenuNode(menuItem, drTempN);
                    }
                    else
                    {
                        BarSubItem barSubItem = null;

                        barSubItem = new BarSubItem();

                        barSubItem.Caption = Convert.ToString(row.MenuName);

                        barSubItem.Name = row.MenuID.ToString();

                        if (!string.IsNullOrEmpty(row.RequestCode))
                        {
                            //barSubItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);//修改，每次调用服务端影响性能  20170324
                            barSubItem.Tag = AllRequest.Find(a => a.RequestCode == row.RequestCode);
                        }
                        barSubItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BarSubItemMenuClicked);

                        OneItem.ItemLinks.Add(barSubItem);

                        string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                        if (!string.IsNullOrEmpty(bkIcon))
                        {
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                barSubItem.LargeGlyph = _skinPic.GetSuBtnImageByName(bkIcon);
                                barSubItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                barSubItem.LargeGlyph = _skinPic.GetSuBtnImageByName("default.png");
                                barSubItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }

                        }
                        else
                        {
                            bkIcon = Convert.ToString(row.MenuSmallIcon);//图标,加载到菜单项
                            if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon))
                            {
                                barSubItem.Glyph = _skinPic.GetSuBtnImageByName(bkIcon);
                            }
                            else
                            {
                                barSubItem.Glyph = _skinPic.GetSuBtnImageByName("default.png");
                            }
                        }

                        //判断菜单是事为桌面菜单，如果是在桌面进行加载
                        if (row.IsSystemDesktop == 1)
                        {
                            SubItemLoadPages(barSubItem, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_LoadDevSonMenuNode" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 加载菜单下面的子菜单项
        /// </summary>
        /// <param name="OneItem">父菜单项</param>
        /// <param name="pControl">快捷按钮容器</param>
        /// <param name="drTemp">子菜单项数据行</param>
        private void LoadMenuNode(ToolStripMenuItem OneItem, PanelControl pControl, List<MenuInfo> drTemp)
        {
            try
            {
                ToolStripMenuItem menuItem = null;

                if (RequestList.Count == 0)
                {
                    RequestList = _RequestService.GetRequestList().Data;
                }
                List<RequestInfo> AllRequest = RequestList;

                foreach (MenuInfo row in drTemp)
                {
                    if (0 == Convert.ToInt32(row.MenuFlag)) { continue; }//如果禁用此项，则不添加

                    menuItem = new ToolStripMenuItem();

                    menuItem.Text = Convert.ToString(row.MenuName);

                    menuItem.Name = row.MenuID.ToString();

                    OneItem.DropDownItems.Add(menuItem);

                    string bkIcon = Convert.ToString(row.MenuLargeIcon);//图标,加载到菜单项
                    if (string.IsNullOrEmpty(bkIcon))
                    {
                        bkIcon = Convert.ToString(row.MenuSmallIcon);
                    }
                    if (!string.IsNullOrEmpty(bkIcon) && File.Exists(_skinPic.SuperDir + bkIcon)) { menuItem.Image = _skinPic.GetSuBtnImageByName(bkIcon); }

                    List<MenuInfo> drTempN = table.Where(p => p.MenuParent == Convert.ToString(row.MenuCode)).ToList<MenuInfo>();

                    if (null != drTempN && drTempN.Count > 0)
                    {
                        LoadMenuNode(menuItem, pControl, drTempN);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row.RequestCode))
                        {
                            //menuItem.Tag = ServiceFactory.CreateService<IRequestService>().GetRequest(row.RequestCode);//修改，每次调用服务端影响性能  20170324
                            menuItem.Tag = AllRequest.Find(a => a.RequestCode == row.RequestCode);
                        }
                        menuItem.Click += new EventHandler(MenuClicked);

                        string bkImg = Convert.ToString(row.MenuLargeIcon);//大图标，加载到快捷按钮
                        if (1 == Convert.ToInt32(row.MenuStatus) && !string.IsNullOrEmpty(bkImg))
                        {
                            AddBtnInSperControl(menuItem, pControl, bkImg);
                        }
                        //判断菜单是事为桌面菜单，如果是在桌面进行加载
                        if (row.IsSystemDesktop == 1)
                        {
                            LoadPages(menuItem, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_LoadMenuNode" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 添加快捷按钮
        /// </summary>
        /// <param name="stripItem">快捷按钮所对应的菜单项</param>
        /// <param name="pControl">快捷按钮容器</param>
        /// <param name="bkimage">背景图片名称</param>
        private void AddBtnInSperControl(ToolStripMenuItem stripItem, PanelControl pControl, string bkimage)
        {
            try
            {
                if (null == pControl) { return; }

                PictureBox pic = new PictureBox();

                Int32 leftPoint = GetSuperBtnMaxPoint(pControl);

                pControl.Controls.Add(pic);

                pic.BackgroundImage = _skinPic.GetSuBtnImageByName(bkimage);

                if (null == pic.BackgroundImage) { pic.BackgroundImage = _skinPic.DefaultBtnImage[Math.Min(pControl.Controls.Count - 1, _skinPic.DefaultBtnImage.Count - 1)]; }

                pic.Width = pic.BackgroundImage.Width;

                pic.Height = pic.BackgroundImage.Height;

                pic.BackgroundImageLayout = ImageLayout.Stretch;

                pic.Left = leftPoint;

                pic.Top = (pControl.Height - pic.Height) / 2;

                pic.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

                _suBtnTip.SetToolTip(pic, stripItem.Text);

                pic.MouseEnter += (object sender, EventArgs e) => { pic.Image = _skinPic.SBtnMouseEnter; };

                pic.MouseLeave += (object sender, EventArgs e) => { pic.Image = null; };

                pic.MouseDown += (object sender, MouseEventArgs e) => { if (e.Button != MouseButtons.Left) { return; } pic.Image = _skinPic.SBtnMouseClick; };

                pic.MouseUp += (object sender, MouseEventArgs e) => { if (e.Button != MouseButtons.Left) { return; } pic.Image = _skinPic.SBtnMouseEnter; };

                pic.MouseClick += (object sender, MouseEventArgs e) => { if (e.Button != MouseButtons.Left) { return; } MenuClicked(stripItem, null); };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_AddBtnInSperControl" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 获取最新加入的btn的X坐标
        /// </summary>
        /// <param name="pControl">快捷按钮容器</param>
        /// <returns>X坐标</returns>
        private Int32 GetSuperBtnMaxPoint(PanelControl pControl)
        {
            PictureBox pic = null;
            try
            {
                if (null == pControl) { throw new ArgumentNullException("快捷按钮容器为空!"); }

                for (int i = 0; i < pControl.Controls.Count; ++i)
                {
                    PictureBox tempPic = pControl.Controls[i] as PictureBox;

                    if (null == tempPic) { continue; }

                    if ((null == pic) || (pic.Handle != tempPic.Handle && tempPic.Left > pic.Left)) { pic = tempPic; }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_GetSuperBtnMaxPoint" + ex.Message + ex.StackTrace);
            }
            return (null == pic) ? (_jWidth * 2) : (pic.Left + pic.Width + _jWidth * 2);
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender">点击的对象</param>
        /// <param name="e"></param>
        private void MenuClicked(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                LoadPages(item, false);
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_MenuClicked" + ex.Message + ex.StackTrace);
            }
        }
        private void BarButtonItemMenuClicked(object sender, ItemClickEventArgs e)
        {
            try
            {
                BarButtonItem item = e.Item as BarButtonItem;
                BarButtonLoadPages(item, false);
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_BarButtonItemMenuClicked" + ex.Message + ex.StackTrace);
            }
        }
        private void BarSubItemMenuClicked(object sender, ItemClickEventArgs e)
        {
            try
            {
                BarSubItem item = e.Item as BarSubItem;
                SubItemLoadPages(item, false);
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_BarSubItemMenuClicked" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="item"></param>
        private void LoadPages(ToolStripMenuItem item, bool isSystemDesktop)
        {
            try
            {
                if (null == item) { return; }
                string ModuleName = item.Text;//获取打开页面的名称
                string ModuleId = item.Name;//获取打开页面的ID              
                if (long.Parse((item.Tag as RequestInfo).RequestID) > 0)
                {
                    #region 加载请求库的参数信息
                    Dictionary<string, string> param = null;
                    if (!string.IsNullOrEmpty((item.Tag as RequestInfo).MenuParams))
                    {
                        if (Convert.ToString((item.Tag as RequestInfo).MenuParams).Contains("&"))
                        {
                            string[] ModuleParams = Convert.ToString((item.Tag as RequestInfo).MenuParams).Split('&');

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
                            if (!string.IsNullOrEmpty(Convert.ToString((item.Tag as RequestInfo).MenuParams)))
                            {
                                string[] ModuleParams = new string[1];//参数传递

                                param = new Dictionary<string, string>();

                                ModuleParams[0] = (item.Tag as RequestInfo).MenuParams;

                                param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                            }
                        }
                    }
                    #endregion

                    RequestUtil.ExcuteCommand((item.Tag as RequestInfo).RequestCode.ToString(), param, isSystemDesktop);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_LoadPages" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// dev菜单单击
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSystemDesktop"></param>
        private void BarButtonLoadPages(BarButtonItem item, bool isSystemDesktop)
        {
            try
            {
                if (null == item) { return; }
                string ModuleName = item.Description;//获取打开页面的名称
                string ModuleId = item.Name;//获取打开页面的ID  
                if (item.Tag != null)
                {
                    if (long.Parse((item.Tag as RequestInfo).RequestID) > 0)
                    {
                        #region 加载请求库的参数信息
                        Dictionary<string, string> param = null;
                        if (!string.IsNullOrEmpty((item.Tag as RequestInfo).MenuParams))
                        {
                            if (Convert.ToString((item.Tag as RequestInfo).MenuParams).Contains("&"))
                            {
                                string[] ModuleParams = Convert.ToString((item.Tag as RequestInfo).MenuParams).Split('&');

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
                                if (!string.IsNullOrEmpty(Convert.ToString((item.Tag as RequestInfo).MenuParams)))
                                {
                                    string[] ModuleParams = new string[1];//参数传递

                                    param = new Dictionary<string, string>();

                                    ModuleParams[0] = (item.Tag as RequestInfo).MenuParams;

                                    param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                                }
                            }
                        }
                        #endregion

                        RequestUtil.ExcuteCommand((item.Tag as RequestInfo).RequestCode.ToString(), param, isSystemDesktop);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_BarButtonLoadPages" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// dev菜单单击
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSystemDesktop"></param>
        private void SubItemLoadPages(BarSubItem item, bool isSystemDesktop)
        {
            try
            {
                if (null == item) { return; }
                string ModuleName = item.Caption;//获取打开页面的名称
                string ModuleId = item.Name;//获取打开页面的ID 
                if (item.Tag != null)
                {
                    if (long.Parse((item.Tag as RequestInfo).RequestID) > 0)
                    {
                        #region 加载请求库的参数信息
                        Dictionary<string, string> param = null;
                        if (!string.IsNullOrEmpty((item.Tag as RequestInfo).MenuParams))
                        {
                            if (Convert.ToString((item.Tag as RequestInfo).MenuParams).Contains("&"))
                            {
                                string[] ModuleParams = Convert.ToString((item.Tag as RequestInfo).MenuParams).Split('&');

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
                                if (!string.IsNullOrEmpty(Convert.ToString((item.Tag as RequestInfo).MenuParams)))
                                {
                                    string[] ModuleParams = new string[1];//参数传递

                                    param = new Dictionary<string, string>();

                                    ModuleParams[0] = (item.Tag as RequestInfo).MenuParams;

                                    param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                                }
                            }
                        }
                        #endregion

                        RequestUtil.ExcuteCommand((item.Tag as RequestInfo).RequestCode.ToString(), param, isSystemDesktop);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserMenu_SubItemLoadPages" + ex.Message + ex.StackTrace);
            }
        }
    }
}
