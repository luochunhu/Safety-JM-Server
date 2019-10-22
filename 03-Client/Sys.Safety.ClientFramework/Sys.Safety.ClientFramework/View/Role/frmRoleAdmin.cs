using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.ClientFramework.View.UserControl.Message;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Role;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.ClientFramework.View.Role
{
    public partial class frmRoleAdmin : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================        
        /// <summary>
        /// 角色列表
        /// </summary>
        private IList<RoleInfo> lstRole = new List<RoleInfo>();
        /// <summary>
        /// 当前选择的角色编号
        /// </summary>
        private int roleID = 0;

        IRoleService _RoleService = ServiceFactory.Create<IRoleService>();
        #endregion

        #region ======================窗体函数==========================
        public frmRoleAdmin()
        {
            InitializeComponent();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmAddRole frm = new frmAddRole();
                frm.ShowDialog();
                LoadData();
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  添加数据成功";
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message.ToString());
            }
            finally { }
        }

        private void frmRoleAdmin_Load(object sender, EventArgs e)
        {
            RoleGridView.IndicatorWidth = 35;
            LoadData();
            RoleGridView.FocusedRowHandle = -1;
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RoleInfo dto = null;
                List<RoleInfo> rightTypeDTO = new List<RoleInfo>();
                if (roleID > 0)
                {
                    DialogResult dr = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Confirm, "确定要删除此角色?");
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            dto = GetData();
                            RoleDeleteRequest rolerequest = new RoleDeleteRequest();
                            rolerequest.Id = dto.RoleID;
                            _RoleService.DeleteRole(rolerequest);
                            LoadData();
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  删除数据成功";
                        }
                        catch (Exception ex)
                        {
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + ex.Message.ToString();
                            UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, StaticMsg.Caption.ToString());
                        }
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

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = RoleGridView.FocusedRowHandle;
                roleID = TypeConvert.ToInt(RoleGridView.GetRowCellValue(rowHandle, "RoleID"));
                if (roleID > 0)
                {
                    frmAddRole frm = new frmAddRole(roleID, 1);
                    frm.ShowDialog();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  修改数据成功";
                    LoadData();
                }
                else
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请先选择需要修改的角色！");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void RoleGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowHandle = RoleGridView.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            roleID = TypeConvert.ToInt(RoleGridView.GetRowCellValue(rowHandle, "RoleID"));
        }

        private void btnRight_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRoleRight frm = new frmRoleRight(roleID);
            frm.ShowDialog();
        }
        #endregion

        #region ======================自定义函数========================
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            var result = _RoleService.GetRoleList();
            lstRole = result.Data;
            gridCtrlView.DataSource = lstRole;
        }

        /// <summary>
        /// 收据数据，用于删除角色对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private RoleInfo GetData()
        {
            RoleInfo role = new RoleInfo();
            object obj = RoleGridView.GetFocusedRow();
            role = (RoleInfo)obj;
            role.InfoState = InfoState.Delete;
            return role;
        }
        #endregion
    }
}