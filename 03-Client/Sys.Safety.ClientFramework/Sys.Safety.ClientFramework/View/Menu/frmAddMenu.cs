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
using Sys.Safety.ClientFramework.View.Request;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Menu;
using Basic.Framework.Web;

namespace Sys.Safety.ClientFramework.View.Menu
{
    public partial class frmAddMenu : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================
        /// <summary>
        /// /0:新增 1:修改        
        /// </summary>
        private int type = 0;

        private long menuID = 0;

        /// <summary>
        /// 菜单对象
        /// </summary>
        private MenuInfo menuDTO = null;

        public MenuInfo MenuDTO
        {
            get { return menuDTO; }
            set { menuDTO = value; }
        }

        /// <summary>
        /// 复制菜单对象
        /// </summary>
        private MenuInfo CopyMenuDTO = new MenuInfo();

        private RequestInfo requestDTO = new RequestInfo();

        private IList<RequestInfo> lstRequest = new List<RequestInfo>();

        private string strRequestCode = "";

        IMenuService _MenuService = ServiceFactory.Create<IMenuService>();
        IRequestService _RequestService = ServiceFactory.Create<IRequestService>();
        IRightService _RightService = ServiceFactory.Create<IRightService>();
        #endregion

        #region ======================窗体函数==========================
        public frmAddMenu()
        {
            InitializeComponent();
        }

        public frmAddMenu(string requestCode)
        {
            InitializeComponent();
            this.strRequestCode = requestCode;
        }

        public frmAddMenu(int menuID, int type)
        {
            InitializeComponent();
            this.type = type;
            this.menuID = menuID;
        }

        private void btnAddRequest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddRequest request = new frmAddRequest();
            request.ShowDialog();
            IList<RequestInfo> lstRequestTmp = new List<RequestInfo>();
            var result = _RequestService.GetRequestList();
            lstRequestTmp = result.Data;
            if (lstRequestTmp != null)
            {
                for (int i = 0; i < lstRequestTmp.Count; i++)
                {
                    lstRequest.Add(lstRequestTmp[i]);
                }
            }
            lstRequest.Insert(0, null);
            lpRequestID.Properties.DataSource = lstRequest;


        }

        public frmAddMenu(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.type = Convert.ToInt32(param["Type"]);
                this.menuID = Convert.ToInt64(param["MenuID"]);
            }
            else
            {
                return;
            }
        }

        private void frmAddMenu_Load(object sender, EventArgs e)
        {
            try
            {
                // LookUpUtil.CreateGridLookUp("AllRight", repositoryItemGridLookUpRight, false, true);

                int i = 0;
                List<MenuInfo> lstMenu = new List<MenuInfo>();
                var result = _MenuService.GetMenuList();
                string menuType = "";
                menuType = MenuType.SelectedIndex.ToString();
                lstMenu = result.Data.FindAll(a => a.Remark2 == menuType).OrderBy(a => a.MenuCode).ToList();

                //for (i = 0; i < lstMenu.Count; i++)
                //{
                //    lstMenu[i].MenuName = SpaceLen(lstMenu[i].MenuCode.ToString().Length / 3) + lstMenu[i].MenuName;
                //}
                for (i = 0; i < lstMenu.Count; i++)
                {
                    cmbMenuParent.Properties.Items.Add(lstMenu[i].MenuCode.ToString() + "-" + lstMenu[i].MenuName.ToString());
                }
                cmbMenuParent.Properties.Items.Insert(0, "一级模块");

                List<RightInfo> lstRight = new List<RightInfo>();
                List<RightInfo> lstRightTmp = new List<RightInfo>();
                List<RequestInfo> lstRequestTmp = new List<RequestInfo>();
                var resultRight = _RightService.GetRightList();
                lstRightTmp = resultRight.Data;
                if (lstRightTmp != null)
                {
                    for (i = 0; i < lstRightTmp.Count; i++)
                    {
                        lstRight.Add(lstRightTmp[i]);
                    }
                }
                lstRight.Insert(0, null);
                lpRightCode.Properties.DataSource = lstRight;
                var resultRequest = _RequestService.GetRequestList();
                lstRequestTmp = resultRequest.Data;
                if (lstRequestTmp != null)
                {
                    for (i = 0; i < lstRequestTmp.Count; i++)
                    {
                        lstRequest.Add(lstRequestTmp[i]);
                    }
                }
                lstRequest.Insert(0, null);
                lpRequestID.Properties.DataSource = lstRequest;


                if (!string.IsNullOrEmpty(strRequestCode))
                {
                    type = 0;
                }

                LoadData();
                if (type == 1)
                {
                    this.Text = "修改菜单";
                }
                else
                {

                    this.Text = "添加菜单";
                }
            }
            catch
            { }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            short MenuFlagText = 0;//菜单是否可用
            short IsSystemDesktopText = 0;//是否为桌面或BS
            int LoadByIframe = 0; //显示次数  
            try
            {
                if (!string.IsNullOrEmpty(textEdit1.Text.Trim()))
                {
                    if (!textEdit1.Text.ToUpper().Contains("ALT+"))
                    {
                        UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "快捷键需要设置成ALT+S格式，必须是ALT+其它键的组合！！");
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtMenuName.Text))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "菜单名称必须输入");
                    return;
                }
                if (string.IsNullOrEmpty(txtRemark1.Text))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "菜单简称必须输入");
                    return;
                }
                if (txtMenuName.Text.Length > 128)
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "菜单名称不能超过128个字符");
                    return;
                }
                if (string.IsNullOrEmpty(cmbMenuParent.Text))
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请选择上级菜单");
                    return;
                }


                if (!string.IsNullOrEmpty(txtMenuProgram.Text))
                {
                    if (!txtMenuProgram.Text.Contains(".dll") && !txtMenuProgram.Text.Contains(".exe"))
                    {
                        UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "菜单引用文件必须是dll文件或者是exe文件");
                        return;
                    }
                }
                if (ckMenuFlag.Checked)//菜单是否启用
                {
                    MenuFlagText = 1;
                }


                #region  操作
                menuDTO.MenuFlag = MenuFlagText;
                menuDTO.MenuForSys = 0;
                menuDTO.MenuLargeIcon = txtLargeIcon.Text;
                menuDTO.MenuName = txtMenuName.Text;
                menuDTO.Remark1 = txtRemark1.Text;
                menuDTO.Remark2 = MenuType.SelectedIndex.ToString();//
                if (cmbMenuParent.Text == "一级模块")
                {
                    menuDTO.MenuParent = "";
                }
                else
                {
                    menuDTO.MenuParent = cmbMenuParent.Text.Trim().Substring(0, cmbMenuParent.Text.Trim().IndexOf('-'));
                }
                menuDTO.MenuSmallIcon = txtSmallIcon.Text;
                menuDTO.MenuStatus = (ckMenuStatus.Checked ? 1 : 0);//是否是快捷菜单
                menuDTO.RightCode = lpRightCode.Text;
                menuDTO.LoadByIframe = 0;
                menuDTO.IsSystemDesktop = 0;
                menuDTO.ShowType = 0;
                menuDTO.RequestCode = lpRequestID.Text;

                //IsSystemDesktopText = Convert.ToInt16(cmbMenuType.SelectedIndex);
                LoadByIframe = cmbMenuLoadFrame.SelectedIndex;
                menuDTO.IsSystemDesktop = 0;
                menuDTO.LoadByIframe = LoadByIframe;
                menuDTO.MenuFile = txtMenuProgram.Text;
                menuDTO.ShowType = ckShowType.SelectedIndex;
                menuDTO.MenuURL = txtMenuFrmName.Text;
                menuDTO.MenuNamespace = txtMenuNameSpace.Text;
                menuDTO.MenuParams = txtMenuFrmPara.Text;
                //快捷键
                menuDTO.Remark4 = textEdit1.Text;
                try
                {
                    MenuAddRequest menurequest = new MenuAddRequest();
                    menurequest.MenuInfo = menuDTO;

                    var resultAdd = _MenuService.AddMenuEx(menurequest);
                    if (resultAdd.Code == 100)
                    {
                        menuDTO = resultAdd.Data;
                        menuID = long.Parse(menuDTO.MenuID);
                    }
                    else
                    {
                        StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据失败," + resultAdd.Message;
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
                #endregion
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message);
            }
        }

        private void frmAddMenu_FormClosing(object sender, FormClosingEventArgs e)
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

        private void lpRequestID_EditValueChanged(object sender, EventArgs e)
        {

            if (lstRequest.Count > 0)
            {
                for (int i = 0; i < lstRequest.Count; i++)
                {
                    if (string.IsNullOrEmpty(lpRequestID.Text))
                    {
                        requestDTO = lstRequest[i];
                        break;
                    }
                    if (lstRequest[i] == null)
                    {
                        continue;
                    }
                    if (lstRequest[i].RequestCode == lpRequestID.Text)
                    {
                        requestDTO = lstRequest[i];
                        break;
                    }
                }
            }
            if (requestDTO != null)
            {
                ckShowType.SelectedIndex = requestDTO.ShowType;
                txtMenuFrmName.Text = requestDTO.MenuURL;
                txtMenuFrmPara.Text = requestDTO.MenuParams;
                txtMenuNameSpace.Text = requestDTO.MenuNamespace;
                cmbMenuType.Text = cmbMenuType.Properties.Items[requestDTO.MenuForSys].ToString();
                txtMenuProgram.Text = requestDTO.MenuFile;
                cmbMenuLoadFrame.Text = cmbMenuLoadFrame.Properties.Items[requestDTO.LoadByIframe].ToString();
            }
            else
            {
                ckShowType.SelectedIndex = 0;
                txtMenuFrmName.Text = "";
                txtMenuFrmPara.Text = "";
                txtMenuNameSpace.Text = "";
                txtRemark1.Text = "";
                cmbMenuType.SelectedIndex = 0;
                txtMenuProgram.Text = "";
                cmbMenuLoadFrame.SelectedIndex = 0;

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
                MenuGetRequest menurequest = new MenuGetRequest();
                menurequest.Id = menuID.ToString();
                var result = _MenuService.GetMenuById(menurequest);
                menuDTO = result.Data;
                if (lstRequest.Count > 0)
                {
                    for (int i = 0; i < lstRequest.Count; i++)
                    {
                        if (lstRequest[i] != null)
                        {
                            if (lstRequest[i].RequestCode == menuDTO.RequestCode)
                            {
                                requestDTO = lstRequest[i];
                                break;
                            }
                        }
                    }
                }
                menuDTO.InfoState = InfoState.Modified;
            }
            else
            {
                menuDTO = new MenuInfo();
                if (lstRequest.Count > 0)
                {
                    if (!string.IsNullOrEmpty(strRequestCode))
                    {
                        for (int i = 0; i < lstRequest.Count; i++)
                        {
                            if (lstRequest[i] != null)
                            {
                                if (lstRequest[i].RequestCode == strRequestCode)
                                {
                                    requestDTO = lstRequest[i];
                                    break;
                                }
                            }
                        }
                    }
                }

                menuDTO.InfoState = InfoState.AddNew;
            }
            InitData();
            CreateNewVO();
        }
        /// <summary>
        /// 初始化窗体数据
        /// </summary>
        private void InitData()
        {
            txtMenuName.Text = menuDTO.MenuName;
            if (!string.IsNullOrEmpty(strRequestCode))
            {
                txtMenuName.Text = requestDTO.RequestName;
            }
            txtRemark1.Text = menuDTO.Remark1;
            MenuGetByCOdeRequest menurequest = new MenuGetByCOdeRequest();
            menurequest.MenuCode = menuDTO.MenuParent;
            var result = _MenuService.GetMenuNameByMenuCode(menurequest);
            cmbMenuParent.Text = menuDTO.MenuParent + "-" + result.Data;
            lpRightCode.EditValue = menuDTO.RightCode;
            ckMenuFlag.Checked = ((menuDTO.MenuFlag == 1) ? true : false);
            ckMenuStatus.Checked = ((menuDTO.MenuStatus == 1) ? true : false);
            txtLargeIcon.Text = menuDTO.MenuLargeIcon;
            txtSmallIcon.Text = menuDTO.MenuSmallIcon;
            if (!string.IsNullOrEmpty(menuDTO.Remark2))//
            {
                MenuType.Text = MenuType.Properties.Items[int.Parse(menuDTO.Remark2)].ToString();
            }
            textEdit1.Text = menuDTO.Remark4;//快捷键
            if (requestDTO != null)
            {
                ckShowType.SelectedIndex = requestDTO.ShowType;
                lpRequestID.EditValue = (requestDTO.RequestCode == null ? "" : requestDTO.RequestCode);
                txtMenuFrmName.Text = requestDTO.MenuURL;
                txtMenuFrmPara.Text = requestDTO.MenuParams;
                txtMenuNameSpace.Text = requestDTO.MenuNamespace;
                cmbMenuType.Text = cmbMenuType.Properties.Items[requestDTO.MenuForSys].ToString();
                txtMenuProgram.Text = requestDTO.MenuFile;
                cmbMenuLoadFrame.Text = cmbMenuLoadFrame.Properties.Items[requestDTO.LoadByIframe].ToString();


            }
            else
            {
                ckShowType.SelectedIndex = 0;
                lpRequestID.EditValue = "";
                txtMenuFrmName.Text = "";
                txtMenuFrmPara.Text = "";
                txtMenuNameSpace.Text = "";
                cmbMenuType.SelectedIndex = 0;
                txtMenuProgram.Text = "";
                cmbMenuLoadFrame.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 创建菜单对象
        /// </summary>
        private void CreateNewVO()
        {
            //CopyMenuDTO.IsSystemDesktop = cmbMenuType.SelectedIndex;
            CopyMenuDTO.MenuName = txtMenuName.Text;
            CopyMenuDTO.Remark1 = txtRemark1.Text;
            CopyMenuDTO.Remark2 = MenuType.SelectedIndex.ToString();//
            CopyMenuDTO.MenuURL = txtMenuFrmName.Text;
            CopyMenuDTO.MenuParent = cmbMenuParent.Text;
            CopyMenuDTO.RightCode = lpRightCode.Text;
            CopyMenuDTO.MenuFile = txtMenuProgram.Text;
            CopyMenuDTO.MenuNamespace = txtMenuNameSpace.Text;
            CopyMenuDTO.MenuParams = txtMenuFrmPara.Text;
            CopyMenuDTO.ShowType = ckShowType.SelectedIndex;
            CopyMenuDTO.MenuFlag = ckMenuFlag.Checked ? 1 : 0;
            CopyMenuDTO.MenuStatus = ckMenuStatus.Checked ? 1 : 0;
            CopyMenuDTO.MenuLargeIcon = txtLargeIcon.Text;
            CopyMenuDTO.MenuSmallIcon = txtSmallIcon.Text;
            CopyMenuDTO.LoadByIframe = cmbMenuLoadFrame.SelectedIndex;

        }
        /// <summary>
        /// 检查数据是否变脏
        /// </summary>
        /// <returns></returns>
        private bool DataIsChange()
        {
            try
            {
                if (!string.IsNullOrEmpty(CopyMenuDTO.MenuName))
                {
                    if (cmbMenuType.SelectedIndex != CopyMenuDTO.IsSystemDesktop)
                        return true;
                    if (txtMenuName.Text != CopyMenuDTO.MenuName)
                        return true;
                    if (txtRemark1.Text != CopyMenuDTO.Remark1)
                        return true;
                    if (txtMenuFrmName.Text != CopyMenuDTO.MenuURL)
                        return true;
                    if (cmbMenuParent.Text != CopyMenuDTO.MenuParent)
                        return true;
                    if (lpRightCode.Text != CopyMenuDTO.RightCode)
                        return true;
                    if (txtMenuProgram.Text != CopyMenuDTO.MenuFile)
                        return true;
                    if (txtMenuNameSpace.Text != CopyMenuDTO.MenuNamespace)
                        return true;
                    if (cmbMenuLoadFrame.SelectedIndex != CopyMenuDTO.LoadByIframe)
                        return true;
                    if (txtSmallIcon.Text != CopyMenuDTO.MenuSmallIcon)
                        return true;
                    if (txtLargeIcon.Text != CopyMenuDTO.MenuLargeIcon)
                        return true;
                    if ((ckMenuStatus.Checked ? 1 : 0) != CopyMenuDTO.MenuStatus)
                        return true;
                    if ((ckMenuFlag.Checked ? 1 : 0) != CopyMenuDTO.MenuFlag)
                        return true;
                    if (ckShowType.SelectedIndex != CopyMenuDTO.ShowType)
                        return true;
                    if (txtMenuFrmPara.Text != CopyMenuDTO.MenuParams)
                        return true;
                }
                return false;
            }
            catch
            { return false; }

        }

        /// <summary>
        /// 根据模块的级数自动在前面加空格，此函数为空格生成函数
        /// </summary>
        /// <param name="length">空格长度</param>
        /// <returns>返回空格的字符串</returns>
        public static string SpaceLen(int length)
        {
            string space = string.Empty;
            for (int j = 0; j < length; j++)
            {
                space += "　";//注意这里的空白是智能abc输入法状态下的v11字符；
            }
            return space;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="param"></param>
        public void DeleteMenu(Dictionary<string, string> param)
        {
            try
            {
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    string MenuID = "";
                    if (param != null && param.Count > 0)
                    {
                        MenuID = param["MenuID"].ToString();
                    }
                    MenuDeleteRequest menurequest = new MenuDeleteRequest();
                    menurequest.Id = MenuID;
                    _MenuService.DeleteMenu(menurequest);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        #endregion
        /// <summary>
        /// 根据菜单类型加载上级菜单选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuType_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<MenuInfo> lstMenu = new List<MenuInfo>();
            var result = _MenuService.GetMenuList();
            string menuType = "";
            menuType = MenuType.SelectedIndex.ToString();
            lstMenu = result.Data.FindAll(a => a.Remark2 == menuType).OrderBy(a => a.MenuCode).ToList();
            cmbMenuParent.Properties.Items.Clear();
            for (int i = 0; i < lstMenu.Count; i++)
            {
                cmbMenuParent.Properties.Items.Add(lstMenu[i].MenuCode.ToString() + "-" + lstMenu[i].MenuName.ToString());
            }
            cmbMenuParent.Properties.Items.Insert(0, "一级模块");
        }

    }
}