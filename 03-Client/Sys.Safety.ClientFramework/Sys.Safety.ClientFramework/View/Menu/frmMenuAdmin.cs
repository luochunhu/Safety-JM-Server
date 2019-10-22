using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Menu;
using Basic.Framework.Web;

namespace Sys.Safety.ClientFramework.View.Menu
{
    public partial class frmMenuAdmin : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================

        /// <summary>
        /// 菜单列表
        /// </summary>
        private IList<MenuInfo> lstMenu = new List<MenuInfo>();
        /// <summary>
        /// 当前选择的菜单编号
        /// </summary>
        private int menuID = 0;

        IMenuService _MenuService = ServiceFactory.Create<IMenuService>();
        #endregion

        #region ======================窗体函数==========================
        public frmMenuAdmin()
        {
            InitializeComponent();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmAddMenu frm = new frmAddMenu();
                frm.ShowDialog();
                LoadData();
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  添加数据成功";
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message.ToString() + "  " + StaticMsg.Caption.ToString());
            }
            finally { }
        }

        private void frmMenuAdmin_Load(object sender, EventArgs e)
        {
            MenuGridView.IndicatorWidth = 35;
            LoadData();
            MenuGridView.FocusedRowHandle = -1;
        }

        private void btnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                MenuInfo dto = null;
                List<MenuInfo> rightTypeDTO = new List<MenuInfo>();
                if (menuID > 0)
                {
                    DialogResult dr = UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Confirm, "确定要删除此菜单?");
                    if (dr == DialogResult.Yes)
                    {
                        dto = GetData();
                        MenuDeleteRequest menurequest = new MenuDeleteRequest();
                        menurequest.Id = dto.MenuID;
                        _MenuService.DeleteMenu(menurequest);
                        LoadData();
                        StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  删除数据成功";
                    }
                }
                else
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  请选择需要删除的数据!";
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void MenuGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowHandle = MenuGridView.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            menuID = TypeConvert.ToInt(MenuGridView.GetRowCellValue(rowHandle, "MenuID"));
        }

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = MenuGridView.FocusedRowHandle;
                menuID = TypeConvert.ToInt(MenuGridView.GetRowCellValue(rowHandle, "MenuID"));
                if (menuID > 0)
                {
                    frmAddMenu frm = new frmAddMenu(menuID, 1);
                    frm.ShowDialog();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  修改数据成功";
                    LoadData();
                }
                else
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请先选择需要修改的权限！");
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region ======================自定义函数========================
        /// <summary>
        /// 收据数据，用于删除菜单对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private MenuInfo GetData()
        {
            MenuInfo menu = new MenuInfo();
            object obj = MenuGridView.GetFocusedRow();
            menu = (MenuInfo)obj;
            menu.InfoState = InfoState.Delete;
            return menu;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            var result = _MenuService.GetMenuList();
            lstMenu = result.Data;
            gridCtrlView.DataSource = lstMenu;
        }
        #endregion
        
    }
}