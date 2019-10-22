using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Runtime.InteropServices;
using Sys.Safety.DataContract;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Role;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.View.Role
{
    public partial class frmAddRole : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================
        /// <summary>
        /// /0:新增 1:修改        
        /// </summary>
        private int type = 0;
        /// <summary>
        /// 角色编号
        /// </summary>
        private long roleID = 0;
        /// <summary>
        /// 角色对象
        /// </summary>
        private RoleInfo roleDTO = null;
        /// <summary>
        /// 复制角色对象
        /// </summary>
        private RoleInfo CopyRoleDTO = new RoleInfo();

        IRoleService _RoleService = ServiceFactory.Create<IRoleService>();
        #endregion

        #region ======================窗体函数==========================

        public frmAddRole()
        {
            InitializeComponent();
        }

        public frmAddRole(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.type = Convert.ToInt32(param["Type"]);
                this.roleID = Convert.ToInt64(param["RoleID"]);
            }
            else
            {
                return;
            }

            bool a = ClientPermission.Authorize("OpenReportDesign");
        }

        public frmAddRole(int roleID, int type)
        {
            InitializeComponent();
            this.type = type;
            this.roleID = roleID;
        }

        private void frmAddRole_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                if (type == 1)
                {
                    this.Text = "修改角色";
                }
                else
                {
                    this.Text = "添加角色";
                    ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                    this.txtCreateName.Text = _ClientItem.UserName;
                }
            }
            catch
            { }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmAddRole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataIsChange())
            {
                DialogResult dr = UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Confirm, " 当前数据已被修改，是否保存？");
                if (dr == DialogResult.Yes)
                {
                    btnAdd_ItemClick(null, null);
                    e.Cancel = false;
                }
                if (dr == DialogResult.Cancel)
                    e.Cancel = true;
                else e.Cancel = false;
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRoleCode.Text))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "角色编码必须输入");
                    return;
                }
                if (string.IsNullOrEmpty(txtRoleName.Text))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "角色名称输入");
                    return;
                }
                roleDTO.CreateName = txtCreateName.Text;
                roleDTO.CreateTime = DateTime.Now;
                roleDTO.RoleCode = txtRoleCode.Text;
                roleDTO.RoleDescription = txtRoleDescription.Text;
                roleDTO.RoleFlag = (ckRoleFlag.Checked ? 1 : 0);
                roleDTO.RoleName = txtRoleName.Text;

                try
                {
                    RoleAddRequest rolerequest = new RoleAddRequest();
                    rolerequest.RoleInfo = roleDTO;
                    var result = _RoleService.AddRoleEx(rolerequest);
                    if (result.Code == 100)
                    {
                        roleDTO = result.Data;
                        roleID = long.Parse(roleDTO.RoleID);
                    }
                    else
                    {
                        StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败," + result.Message;
                        return;
                    }
                    type = 1;
                    LoadData();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据成功";
                    //UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "保存数据成功");
                }
                catch (Exception ex)
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败";
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message + " " + StaticMsg.Caption);
                }
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message);
            }
        }
        #endregion

        #region ======================自定义函数========================
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (type > 0)
            {
                RoleGetRequest rolerequest = new RoleGetRequest();
                rolerequest.Id = roleID.ToString();
                var result = _RoleService.GetRoleById(rolerequest);
                roleDTO = result.Data;
                roleDTO.InfoState = InfoState.Modified;
            }
            else
            {
                roleDTO = new RoleInfo();
                roleDTO.InfoState = InfoState.AddNew;
            }
            InitData();
            CreateNewVO();
        }
        /// <summary>
        /// 初始化窗体数据
        /// </summary>
        private void InitData()
        {
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            txtCreateName.Text = _ClientItem.UserName;
            txtRoleCode.Text = roleDTO.RoleCode;
            txtRoleDescription.Text = roleDTO.RoleDescription;
            txtRoleName.Text = roleDTO.RoleName;
            if (roleDTO.RoleCode == null)
            {
                ckRoleFlag.Checked = ((roleDTO.RoleFlag == 1) ? false : true);
            }
            else
            {
                ckRoleFlag.Checked = ((roleDTO.RoleFlag == 1) ? true : false);
            }
        }
        /// <summary>
        /// 创建菜单对象
        /// </summary>
        private void CreateNewVO()
        {
            CopyRoleDTO.CreateName = txtCreateName.Text;
            CopyRoleDTO.RoleCode = txtRoleCode.Text;
            CopyRoleDTO.RoleDescription = txtRoleDescription.Text;
            CopyRoleDTO.RoleName = txtRoleName.Text;
            CopyRoleDTO.RoleFlag = ckRoleFlag.Checked ? 1 : 0;
        }
        /// <summary>
        /// 检查数据是否变脏
        /// </summary>
        /// <returns></returns>
        private bool DataIsChange()
        {
            try
            {
                if (!string.IsNullOrEmpty(CopyRoleDTO.RoleCode))
                {
                    if (txtCreateName.Text != CopyRoleDTO.CreateName)
                        return true;
                    if (txtRoleCode.Text != CopyRoleDTO.RoleCode)
                        return true;
                    if (txtRoleDescription.Text != CopyRoleDTO.RoleDescription)
                        return true;
                    if (txtRoleName.Text != CopyRoleDTO.RoleName)
                        return true;
                    if ((ckRoleFlag.Checked ? 1 : 0) != CopyRoleDTO.RoleFlag)
                        return true;
                }
                return false;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="param"></param>
        public void DeleteRole(Dictionary<string, string> param)
        {
            try
            {
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    string RoleID = "";
                    if (param != null && param.Count > 0)
                    {
                        RoleID = param["RoleID"].ToString();
                    }
                    //验证超级管理员，不能删除  20170617
                    RoleGetRequest getrolerequest = new RoleGetRequest();
                    getrolerequest.Id = RoleID;
                    var result = _RoleService.GetRoleById(getrolerequest);
                    if (result.Data != null)
                    {
                        if (result.Data.RoleName.Contains("超级管理员"))
                        {
                            XtraMessageBox.Show("超级管理员不能被删除！");
                            return;
                        }
                    }
                    RoleDeleteRequest rolerequest = new RoleDeleteRequest();
                    rolerequest.Id = RoleID;
                    _RoleService.DeleteRole(rolerequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion





    }
}