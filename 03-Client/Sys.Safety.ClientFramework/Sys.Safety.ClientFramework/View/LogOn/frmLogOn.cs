using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.ClientFramework.View.MainForm;
using System.Diagnostics;
using Sys.Safety.ClientFramework.Properties;
using System.Runtime.InteropServices;
using System.Net;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using Sys.Safety.ClientFramework.Configuration;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.User;
using System.Configuration;

namespace Sys.Safety.ClientFramework.View.LogOn
{
    public partial class frmLogOn : XtraForm
    {
        #region 窗体图标
        /// <summary>登录窗口logo图标</summary>
        private Image _imgHeadLogo;

        /// <summary>窗体关闭按钮背景图片-鼠标移动</summary>
        private Image _imgCloseMouse;

        /// <summary>窗体关闭按钮背景图片</summary>
        private Image _imgClose;

        /// <summary>窗体最小化按钮背景图片-鼠标移动</summary>
        private Image _imgMinMouse;

        /// <summary>窗体最小化按钮背景图片</summary>
        private Image _imgMin;

        /// <summary>窗体设置按钮背景图片-鼠标移动</summary>
        private Image _imgSetMouse;

        /// <summary>窗体设置按钮背景图片</summary>
        private Image _imgSet;

        /// <summary>able背景图片</summary>
        private Image _imgAble;

        /// <summary>disable背景图片</summary>
        private Image _imgDisable;

        /// <summary>用户名背景图片</summary>
        private Image _imgUser;

        /// <summary>密码背景图片</summary>
        private Image _imgPassword;

        /// <summary>登录背景图片</summary>
        private Image _imgLogoOn;

        /// <summary>鼠标左键是否按下</summary>
        private bool _isLeftBtnDown;

        IUserService _UserService = ServiceFactory.Create<IUserService>();
        #endregion

        #region 图标切换
        /// <summary>
        /// 从图片资源中加载图标
        /// </summary>
        private void LoadFrmImage()
        {
            _imgHeadLogo = Resources.logo1;
            _imgClose = Resources.close;
            _imgCloseMouse = Resources.close2;
            _imgMin = Resources.zuixiaohua1;
            _imgMinMouse = Resources.zuixiaohua2;
            _imgSet = Resources.set;
            _imgSetMouse = Resources.set2;
        }

        /// <summary>
        /// 初始化窗体图片属性
        /// </summary>
        private void InitFormSkinPic()
        {
            LoadFrmImage();
        }

        /// <summary>
        /// 窗体顶部区域鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) { return; }
            _isLeftBtnDown = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelHead_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isLeftBtnDown && e.Button == MouseButtons.Left && (this.Width != Screen.PrimaryScreen.Bounds.Width || this.Height != Screen.PrimaryScreen.Bounds.Height))
            {
                ReleaseCapture();
                SendMessage(Handle, 274, 61440 + 9, 0);
            }
        }
        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) { return; }
            PictureEdit pic = sender as PictureEdit;
            DoSysBtnOnClick(pic);
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            PictureEdit pic = sender as PictureEdit;
            SetSysBtnOnMouseImage(pic, true);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            PictureEdit pic = sender as PictureEdit;
            SetSysBtnOnMouseImage(pic, false);
        }
        /// <summary>
        /// 系统按钮点击的事件响应
        /// </summary>
        private void DoSysBtnOnClick(PictureEdit pic)
        {
            if (null == pic) { return; }
            #region
            switch (pic.Name)
            {
                case "btnClose":
                    this.Close();
                    break;
                case "btnSet":
                    break;
                case "btnMin":
                    if (this.WindowState != FormWindowState.Minimized) { this.WindowState = FormWindowState.Minimized; }
                    break;
                default:
                    break;
            }
            #endregion
            Application.DoEvents();
        }
        /// <summary>
        /// 系统按钮鼠标进入事件时更换图片
        /// </summary>
        private void SetSysBtnOnMouseImage(PictureEdit pic, bool enter)
        {
            if (null == pic) { return; }

            switch (pic.Name)
            {
                case "btnClose":
                    pic.BackgroundImage = (enter ? _imgCloseMouse : _imgClose);
                    break;
                case "btnMin":
                    pic.BackgroundImage = (enter ? _imgMinMouse : _imgMin);
                    break;
                case "btnSet":
                    pic.BackgroundImage = (enter ? _imgSetMouse : _imgSet); ;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 窗体顶部区域鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelHead_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) { return; }
            _isLeftBtnDown = false;
        }
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wp, int lp);
        [DllImport("user32")]
        public static extern int ReleaseCapture();
        #endregion

        #region 成员变量
        /// <summary>
        /// 用户对象
        /// </summary>
        private UserInfo userDTO = null;

        /// <summary>
        /// 用户列表
        /// </summary>
        private IList<UserInfo> lstUser;

        /// <summary>
        /// 允许错误登录次数
        /// </summary>
        private int AllowLogOnCount = 3;

        /// <summary>
        /// 已登录次数
        /// </summary>
        private int LogOnCount = 0;

        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool isLogOk = false;

        //调用API置顶窗口        
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
        #endregion

        #region 构造函数
        public frmLogOn()
        {

            try
            {
                LoginManager.LoginSuccessIsLoadMenu = true;

                InitializeComponent();

                InitFormSkinPic();

                this.Text = "用户登录";
                this.labelControl1.Text = Sys.Safety.ClientFramework.Configuration.BaseInfo.SoftFullName;
                //panelFoot.ContentImage = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\bg.png");
                this.labelControl1.Left = this.Left + this.Width / 2 - this.labelControl1.Width / 2 - 10;
                isLogOk = false;

                //this.labelControl4.Text = "软件版本："+Sys.Safety.ClientFramework.Configuration.BaseInfo.Version;
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_frmLogOn" + ex.Message + ex.StackTrace);
            }
        }
        public frmLogOn(bool isLoadMenu)
        {

            try
            {
                LoginManager.LoginSuccessIsLoadMenu = isLoadMenu;
                LoginManager.isLoginSuccess = false;

                //if (LoginManager.IsLogin)//如果已有用户登录，则退出当前用户
                //{
                //    LoginManager.Logout();//退出当前登录
                //}

                InitializeComponent();

                InitFormSkinPic();

                this.Text = "用户登录";
                this.labelControl1.Text = Sys.Safety.ClientFramework.Configuration.BaseInfo.SoftFullName;
                //panelFoot.ContentImage = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\bg.png");
                this.labelControl1.Left = this.Left + this.Width / 2 - this.labelControl1.Width / 2;
                isLogOk = false;

                //this.labelControl4.Text = "软件版本："+Sys.Safety.ClientFramework.Configuration.BaseInfo.Version;
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_frmLogOn" + ex.Message + ex.StackTrace);
            }
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 检查输入的有效性
        /// </summary>
        private bool CheckInput()
        {
            try
            {
                //是否没有输入用户名
                if (this.txtUserName.Text.Length == 0)
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请输入用户名不允许为空，请输入。");
                    this.txtUserName.Focus();
                    return false;
                }

                if (this.txtPass.Text.Length == 0)
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请输入密码不允许为空，请输入。");
                    this.txtPass.Focus();
                    return false;
                }
                this.btnOK.Focus();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_CheckInput" + ex.Message + ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// 允许登录次数已经到了
        /// </summary>
        /// <returns>继续允许输入</returns>
        private bool CheckAllowLogOnCount()
        {
            try
            {
                if (this.LogOnCount > this.AllowLogOnCount)
                {
                    // 控件重新设置状态
                    this.txtPass.Text = "请输入密码";

                    this.txtUserName.Enabled = false;
                    this.txtPass.Enabled = false;
                    this.btnOK.Enabled = false;

                    // 进行提示信息，不能再输入了，已经错误N次了
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, string.Format("已输入{0}次错误密码，不再允许继续登录，请重新启动程序进行登录", this.AllowLogOnCount.ToString()));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_CheckAllowLogOnCount" + ex.Message + ex.StackTrace);
                return false;
            }
        }
        #endregion

        #region 窗体函数
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                #region wcf检查服务端是否正常运行
                if (ConfigurationManager.AppSettings["ServiceType"].ToString() == "wcf")
                {
                    try
                    {
                        _UserService.GetUserList();//调用服务端接口，看能否正常调用来判断服务端是否开启
                    }
                    catch
                    {
                        MessageBox.Show("连接服务端异常，请检查服务器IP端口是否配置正确！");

                        //WcfManage.WcfManage wcfmag = new WcfManage.WcfManage();
                        //wcfmag.ShowDialog();
                        //退出应用程序
                        System.Environment.Exit(0);
                    }
                }
                #endregion

                // 忙状态
                this.Cursor = Cursors.WaitCursor;
                this.LogOnCount++;
                // 验证用户输入
                if (this.CheckInput())
                {
                    //if (this.CheckAllowLogOnCount())//不对输入次数进行限制   20170525
                    //{

                    //提示是否注销用户 
                    if (LoginManager.IsLogin)
                    {
                        if (LoginManager.LoginSuccessIsLoadMenu)//如果是主控登录验证，则不提示  20170711
                        {
                            //if (DevExpress.XtraEditors.XtraMessageBox.Show("退出当前用户？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            //{
                            LoginManager.Logout();//退出当前登录
                            //}
                            //else
                            //{
                            //    return;//返回操作
                            //}
                        }
                    }

                    LoginManager.Login(txtUserName.Text, txtPass.Text, Sys.Safety.ClientFramework.Configuration.BaseInfo.MenuType);



                    if (LoginManager.IsLogin)
                    {
                        if (!LoginManager.LoginSuccessIsLoadMenu)
                        {
                            //如果是主控验证，则直接赋值登录成功与否对象  20170711
                            LoginManager.isLoginSuccess = true;
                        }

                        #region 更新登录时间
                        System.Net.IPAddress addr;
                        addr = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                        UserGetByCodeRequest userrequest = new UserGetByCodeRequest();
                        userrequest.Code = txtUserName.Text;
                        var result = _UserService.GetUserByCode(userrequest);
                        userDTO = result.Data;
                        userDTO.LastLoginTime = DateTime.Now;
                        userDTO.LoginCount++;
                        userDTO.LoginIP = addr.ToString();
                        UserUpdateRequest userrequestUp = new UserUpdateRequest();
                        userrequestUp.UserInfo = userDTO;
                        _UserService.UpdateUser(userrequestUp);
                        #endregion
                        //写操作日志
                        OperateLogHelper.InsertOperateLog(15, "用户登录", "");
                    }

                    if (this.Parent == null)
                    {
                        if (LoginManager.IsLogin)
                        {
                            isLogOk = true;
                            this.Close();
                        }
                    }

                    //}
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("&&"))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message.Substring(2));
                }
                else
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "登录异常，请检查与服务器的连接配置及网络！");
                }
                LogHelper.Error("frmLogOn_btnOK_Click" + ex.Message + ex.StackTrace);
            }
            finally
            {
                // 已经忙完了
                this.Cursor = Cursors.Default;
            }
        }

        private void frmLogOn_Load(object sender, EventArgs e)
        {
            try
            {


                var result = _UserService.GetUserList();
                lstUser = result.Data;
                txtPass.Text = "请输入密码";
                txtUserName.Text = "请输入登录名";
                //加载当前用户列表 
                txtUserName.Properties.Items.Clear();
                lstUser = lstUser.OrderBy(a => a.UserCode).ToList();//按用户编码进行排序
                foreach (UserInfo User in lstUser)
                {
                    txtUserName.Properties.Items.Add(User.UserCode);
                }

                SetForegroundWindow(this.Handle);

                string MyFileName = Application.StartupPath + "\\Image\\Icon\\客户端.ico";
                if (File.Exists(MyFileName))
                {
                    this.Icon = new Icon(MyFileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_frmLogOn_Load" + ex.Message + ex.StackTrace);
            }
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text == "请输入登录名")
                {
                    txtUserName.Text = "";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_txtUserName_Enter" + ex.Message + ex.StackTrace);
            }
        }
        private void txtPass_Enter(object sender, EventArgs e)
        {
            try
            {
                txtPass.Properties.UseSystemPasswordChar = true;
                if (txtPass.Text == "请输入密码")
                {
                    txtPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_txtPass_Enter" + ex.Message + ex.StackTrace);
            }
        }

        private void txtUserName_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text == "")
                {
                    txtUserName.Text = "请输入登录名";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_txtUserName_MouseLeave" + ex.Message + ex.StackTrace);
            }
        }

        private void txtPass_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text == "")
                {
                    txtPass.Properties.UseSystemPasswordChar = false;
                    txtPass.Text = "请输入密码";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("frmLogOn_txtPass_MouseLeave" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 服务器IP设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            WcfManage.WcfManage wcfmag = new WcfManage.WcfManage();
            wcfmag.ShowDialog();
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 用户注销接口
        /// </summary>
        public void UserLoginOut()
        {
            try
            {
                // 用户登录--2015-3-20 输入错误密码时，菜单无法使用了。
                if (LoginManager.IsLogin)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("退出当前用户？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //写操作日志
                        OperateLogHelper.InsertOperateLog(15, "用户注销", "");

                        LoginManager.Logout();//退出当前登录
                    }
                    else
                    {
                        return;//返回操作
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("当前没有用户进行登录操作！");
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error("frmLogOn_UserLoginOut" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 菜单切换接口 
        /// </summary>
        public void MenuTypeChange()
        {
            try
            {
                if (Sys.Safety.ClientFramework.Configuration.BaseInfo.MenuType == "0")
                {//从标准菜单切换到AQ菜单
                    LoginManager.MenuChange("1");
                    Sys.Safety.ClientFramework.Configuration.BaseInfo.MenuType = "1";
                }
                else
                {//从AQ菜单切换到标准菜单
                    LoginManager.MenuChange("0");
                    Sys.Safety.ClientFramework.Configuration.BaseInfo.MenuType = "0";
                }

            }
            catch (System.Exception ex)
            {
                LogHelper.Error("frmLogOn_MenuTypeChange" + ex.Message + ex.StackTrace);
            }
        }
        #endregion

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}