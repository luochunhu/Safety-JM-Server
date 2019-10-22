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
using Sys.Safety.ClientFramework.View.Role;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.User;
using Sys.Safety.Request.Userrole;
using Basic.Framework.Web;
using Basic.Framework.Common;

namespace Sys.Safety.ClientFramework.View.User
{
    public partial class frmAddUser : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================
        /// <summary>
        /// /0:新增 1:修改        
        /// </summary>
        private int type = 0;
        /// <summary>
        /// 用户编号
        /// </summary>
        private long userID = 0;
        /// <summary>
        /// 用户对象
        /// </summary>
        private UserInfo userDTO = null;
        /// <summary>
        /// 复制用户对象
        /// </summary>
        private UserInfo CopyUserDTO = new UserInfo();

        /// <summary>
        /// 用户角色对象
        /// </summary>
        private UserroleInfo userRole = new UserroleInfo();
        /// <summary>
        /// 用户角色对象列表
        /// </summary>
        private IList<UserroleInfo> lstUserRole = new List<UserroleInfo>();
        /// <summary>
        /// 用户数据发生改变事件
        /// </summary>
        public event EventHandler DataChanged;
        /// <summary>
        /// 用户数据是否发生改变的标记
        /// </summary>
        private bool bDataChanged = false;
        public bool BDataChanged
        {
            get
            {
                return bDataChanged;
            }
            set
            {
                bDataChanged = value;
            }
        }

        IUserService _UserService = ServiceFactory.Create<IUserService>();
        IRoleService _RoleService = ServiceFactory.Create<IRoleService>();
        IUserroleService _UserroleService = ServiceFactory.Create<IUserroleService>();
        private void OnDataChanged(EventArgs e)
        {
            if (this.DataChanged != null)
            {
                this.DataChanged(this, e);
            }
        }
        #endregion

        #region ======================窗体函数==========================
        public frmAddUser()
        {
            InitializeComponent();
        }

        public frmAddUser(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.type = Convert.ToInt32(param["Type"]);
                this.userID = Convert.ToInt64(param["UserID"]);
            }
            else
            {
                return;
            }
        }

        public frmAddUser(int userID, int type)
        {
            InitializeComponent();
            this.type = type;
            this.userID = userID;
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            try
            {
                ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                txtCreateName.Text = _ClientItem.UserName;
                txtCreateName.Properties.ReadOnly = true;

                IList<RoleInfo> lstRole = new List<RoleInfo>();
                IList<RoleInfo> lstRoleTmp = new List<RoleInfo>();
                var result = _RoleService.GetRoleList();
                lstRoleTmp = result.Data;
                if (lstRoleTmp != null)
                {
                    for (int i = 0; i < lstRoleTmp.Count; i++)
                    {
                        lstRole.Add(lstRoleTmp[i]);
                    }
                }
                lstRole.Insert(0, null);
                lpRole.Properties.DataSource = lstRole;

                LoadData();
                if (type == 1)
                {
                    this.Text = "修改用户";
                    //layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else
                {
                    this.Text = "添加用户";
                    this.txtCreateName.Text = _ClientItem.UserName;
                }
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message);
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserCode.Text))
                {
                    Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "登录名必须输入");
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "密码必须输入");
                    return;
                }
                if (string.IsNullOrEmpty(lpRole.Text))
                {
                    Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请选择角色");
                    return;
                }
                userDTO.ContactPhone = txtContactPhone.Text;
                userDTO.CreateName = txtCreateName.Text;
                userDTO.CreateTime = DateTime.Now;
                userDTO.DeptCode = "";
                userDTO.Password = Basic.Framework.Common.MD5Helper.MD5Encrypt(txtPassword.Text);
                userDTO.UserCode = txtUserCode.Text;
                userDTO.UserName = txtUserName.Text;
                userDTO.UserType = 0;
                userDTO.UserFlag = (ckUserFlag.Checked ? 1 : 0);

                try
                {
                    UserAddRequest userrequest = new UserAddRequest();
                    userrequest.UserInfo = userDTO;
                    var result = _UserService.AddUserEx(userrequest);
                    if (result.Code == 100)
                    {
                        userDTO = result.Data;
                        userID = long.Parse(userDTO.UserID);
                    }
                    else
                    {
                        StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败," + result.Message;
                        return;
                    }
                    if (!string.IsNullOrEmpty(lpRole.Text))
                    {
                        lstUserRole = new List<UserroleInfo>();
                        userRole.CreateTime = DateTime.Now;
                        userRole.CreateName = userDTO.CreateName;
                        userRole.RoleID = lpRole.EditValue.ToString();
                        userRole.UserID = userID.ToString();
                        lstUserRole.Add(userRole);
                        UserrolesAddRequest userrolerequest = new UserrolesAddRequest();
                        userrolerequest.userId = userID.ToString();
                        userrolerequest.userRoleList = lstUserRole.ToList();
                        _UserroleService.AddUserRoles(userrolerequest);
                    }
                    else
                    {
                        UserroleGetCheckUserIDExistRequest userrolerequest = new UserroleGetCheckUserIDExistRequest();
                        userrolerequest.UserId = userID.ToString();
                        var resultCheckUserIDExist = _UserroleService.CheckUserIDExist(userrolerequest);
                        if (resultCheckUserIDExist.Code == 100)
                        {
                            UserroleDeleteByUserIdRequest deluserrolerequest = new UserroleDeleteByUserIdRequest();
                            deluserrolerequest.UserId = userID.ToString();
                            _UserroleService.DeleteUserroleByUserId(deluserrolerequest);
                        }
                    }

                    type = 1;
                    LoadData();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据成功";
                    // UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "保存数据成功");
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

        private void frmAddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataIsChange())
            {
                bDataChanged = true;
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


        #endregion

        #region ======================自定义函数========================
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (type > 0)
            {
                UserGetRequest userrequest = new UserGetRequest();
                userrequest.Id = userID.ToString();
                var result = _UserService.GetUserById(userrequest);
                userDTO = result.Data;
                userDTO.InfoState = InfoState.Modified;
                UserroleGetByUserIdRequest userrolerequest = new UserroleGetByUserIdRequest();
                userrolerequest.UserId = userID.ToString();
                var resultGetUserRoles = _UserroleService.GetUserRoleByUserId(userrolerequest);
                lstUserRole = resultGetUserRoles.Data;
                if (lstUserRole.Count > 0)
                {
                    userRole = lstUserRole[0];
                    lpRole.EditValue = userRole.RoleID;
                }
            }
            else
            {
                userDTO = new UserInfo();
                userDTO.InfoState = InfoState.AddNew;
            }
            InitData();
            CreateNewVO();
        }
        /// <summary>
        /// 初始化窗体数据
        /// </summary>
        private void InitData()
        {
            txtContactPhone.Text = userDTO.ContactPhone;
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            txtCreateName.Text = _ClientItem.UserName;
            txtPassword.Text = "";
            txtUserCode.Text = userDTO.UserCode;
            txtUserName.Text = userDTO.UserName;
            if (userDTO.UserCode == null)
            {
                ckUserFlag.Checked = ((userDTO.UserFlag == 1) ? false : true);
            }
            else
            {
                ckUserFlag.Checked = ((userDTO.UserFlag == 1) ? true : false);
            }
        }
        /// <summary>
        /// 创建菜单对象
        /// </summary>
        private void CreateNewVO()
        {
            CopyUserDTO.ContactPhone = txtContactPhone.Text;
            CopyUserDTO.CreateName = txtCreateName.Text;
            CopyUserDTO.Password = txtPassword.Text;
            CopyUserDTO.UserCode = txtUserCode.Text;
            CopyUserDTO.UserName = txtUserName.Text;
            CopyUserDTO.UserFlag = ckUserFlag.Checked ? 1 : 0;

        }
        /// <summary>
        /// 检查数据是否变脏
        /// </summary>
        /// <returns></returns>
        private bool DataIsChange()
        {
            try
            {
                if (!string.IsNullOrEmpty(CopyUserDTO.UserCode))
                {
                    if (txtContactPhone.Text != CopyUserDTO.ContactPhone)
                        return true;
                    if (txtCreateName.Text != CopyUserDTO.CreateName)
                        return true;
                    if (txtPassword.Text != CopyUserDTO.Password)
                        return true;
                    if (txtUserCode.Text != CopyUserDTO.UserCode)
                        return true;
                    if (txtUserName.Text != CopyUserDTO.UserName)
                        return true;
                    if ((ckUserFlag.Checked ? 1 : 0) != CopyUserDTO.UserFlag)
                        return true;
                }
                return false;
            }
            catch
            { return false; }

        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="param"></param>
        public void DeleteUser(Dictionary<string, string> param)
        {
            try
            {
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    string UserID = "";
                    if (param != null && param.Count > 0)
                    {
                        UserID = param["UserID"].ToString();
                    }
                    //验证超级管理员，不能删除  20170617
                    UserGetRequest getuserrequest = new UserGetRequest();
                    getuserrequest.Id = UserID;
                    var result = _UserService.GetUserById(getuserrequest);
                    if (result.Data != null)
                    {
                        if (result.Data.UserCode == "admin")
                        {
                            XtraMessageBox.Show("admin为系统默认用户，不能被删除！");
                            return;
                        }
                    }
                    UserDeleteRequest userrequest = new UserDeleteRequest();
                    userrequest.Id = UserID;
                    _UserService.DeleteUser(userrequest);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        #endregion

        private void tlbRoleRight_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRoleRight frm = new frmRoleRight(Convert.ToInt64(lpRole.EditValue));
            frm.ShowDialog();
        }


    }
}