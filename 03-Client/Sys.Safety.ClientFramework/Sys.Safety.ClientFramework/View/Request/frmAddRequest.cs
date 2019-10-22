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
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.View.Request
{
    public partial class frmAddRequest : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================
        /// <summary>
        /// 0:新增请求库
        /// 1:修改请求库
        /// </summary>
        private int type = 0;
        /// <summary>
        /// 请求库编码
        /// </summary>
        private long requestID = 0;
        /// <summary>
        /// 请求库对象
        /// </summary>
        private RequestInfo requestDTO = null;
        /// <summary>
        /// 请求库对象
        /// </summary>
        public RequestInfo RequestDTO
        {
            get { return requestDTO; }
            set { requestDTO = value; }
        }
        /// <summary>
        /// 复制请求库对象
        /// </summary>
        private RequestInfo CopyRequestDTO = new RequestInfo();

        IRequestService _RequestService = ServiceFactory.Create<IRequestService>();
        #endregion

        #region ======================窗体函数==========================
        public frmAddRequest()
        {
            InitializeComponent();
        }
        public frmAddRequest(int requestID, int type)
        {
            InitializeComponent();
            this.type = type;
            this.requestID = requestID;
        }
        public frmAddRequest(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.type = Convert.ToInt32(param["Type"]);
                this.requestID = Convert.ToInt64(param["RequestID"]);
            }
            else
            {
                return;
            }
        }
        private void btnCancle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void frmAddRequest_Load(object sender, EventArgs e)
        {
            LoadData();
            if (type == 1)
            {
                this.Text = "修改请求库";
            }
        }
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GetData();
                RequestAddRequest requestrequest = new RequestAddRequest();
                requestrequest.RequestInfo = requestDTO;
                var result = _RequestService.AddRequestEx(requestrequest);
                if (result.Code == 100)
                {
                    requestDTO = result.Data;
                    requestID = long.Parse(requestDTO.RequestID);
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
                RequestGetRequest requestrequest = new RequestGetRequest();
                requestrequest.Id = requestID.ToString();
                var result = _RequestService.GetRequestById(requestrequest);
                requestDTO = result.Data;
                requestDTO.InfoState = InfoState.Modified;
            }
            else
            {
                requestDTO = new RequestInfo();
                requestDTO.InfoState = InfoState.AddNew;
            }
            InitData();
            CreateNewVO();
        }
        /// <summary>
        /// 初始化窗体数据
        /// </summary>
        private void InitData()
        {
            txtRequestCode.Text = requestDTO.RequestCode;
            txtRequestName.Text = requestDTO.RequestName;
            txtMenuURL.Text = requestDTO.MenuURL;
            txtMenuParams.Text = requestDTO.MenuParams;
            txtMenuNameSpace.Text = requestDTO.MenuNamespace;
            txtMenuFile.Text = requestDTO.MenuFile;
            cmbMenuForSys.SelectedIndex = requestDTO.MenuForSys;
            cmbMenuForSys_SelectedIndexChanged(null, null);
            cmbMenuLoadFrame.SelectedIndex = requestDTO.LoadByIframe;
            cmbShowType.SelectedIndex = requestDTO.ShowType;
            txtFunctionName.Text = requestDTO.BZ1;
            chkMain.Checked = requestDTO.BZ2 == "1" ? true : false;
        }
        /// <summary>
        /// 创建请求库对象
        /// </summary>
        private void CreateNewVO()
        {
            CopyRequestDTO.LoadByIframe = cmbMenuLoadFrame.SelectedIndex;
            CopyRequestDTO.MenuFile = txtMenuFile.Text;
            CopyRequestDTO.MenuForSys = cmbMenuForSys.SelectedIndex;
            CopyRequestDTO.MenuNamespace = txtMenuNameSpace.Text;
            CopyRequestDTO.MenuParams = txtMenuParams.Text;
            CopyRequestDTO.MenuURL = txtMenuURL.Text;
            CopyRequestDTO.RequestCode = txtRequestCode.Text;
            CopyRequestDTO.RequestName = txtRequestName.Text;
            CopyRequestDTO.ShowType = cmbShowType.SelectedIndex;
            CopyRequestDTO.BZ1 = txtFunctionName.Text;
            CopyRequestDTO.BZ2 = chkMain.Checked ? "1" : "0";
        }
        /// <summary>
        /// 收集数据
        /// </summary>
        private void GetData()
        {
            requestDTO.LoadByIframe = cmbMenuLoadFrame.SelectedIndex;
            requestDTO.MenuFile = txtMenuFile.Text;
            requestDTO.MenuForSys = cmbMenuForSys.SelectedIndex;
            requestDTO.MenuNamespace = txtMenuNameSpace.Text;
            requestDTO.MenuParams = txtMenuParams.Text;
            requestDTO.MenuURL = txtMenuURL.Text;
            requestDTO.RequestCode = txtRequestCode.Text;
            requestDTO.RequestName = txtRequestName.Text;
            requestDTO.ShowType = cmbShowType.SelectedIndex;
            requestDTO.BZ1 = txtFunctionName.Text;
            requestDTO.BZ2 = chkMain.Checked ? "1" : "0";
        }
        /// <summary>
        /// 检查数据是否变脏
        /// </summary>
        /// <returns></returns>
        private bool DataIsChange()
        {
            try
            {
                if (!string.IsNullOrEmpty(CopyRequestDTO.RequestName))
                {
                    if (cmbShowType.SelectedIndex != CopyRequestDTO.ShowType)
                        return true;
                    if (txtMenuFile.Text != CopyRequestDTO.MenuFile)
                        return true;
                    if (txtMenuNameSpace.Text != CopyRequestDTO.MenuNamespace)
                        return true;
                    if (txtMenuParams.Text != CopyRequestDTO.MenuParams)
                        return true;
                    if (txtMenuURL.Text != CopyRequestDTO.MenuURL)
                        return true;
                    if (txtRequestCode.Text != CopyRequestDTO.RequestCode)
                        return true;
                    if (txtRequestName.Text != CopyRequestDTO.RequestName)
                        return true;
                    if (cmbMenuLoadFrame.SelectedIndex != CopyRequestDTO.LoadByIframe)
                        return true;
                    if (cmbMenuForSys.SelectedIndex != CopyRequestDTO.MenuForSys)
                        return true;
                    if (txtFunctionName.Text != CopyRequestDTO.BZ1)
                        return true;
                }
                return false;
            }
            catch
            { return false; }

        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="param"></param>
        public void DeleteRequest(Dictionary<string, string> param)
        {
            try
            {
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    string RequestID = "";
                    if (param != null && param.Count > 0)
                    {
                        RequestID = param["RequestID"].ToString();
                    }
                    RequestDeleteRequest requestrequest = new RequestDeleteRequest();
                    requestrequest.Id = RequestID;
                    _RequestService.DeleteRequest(requestrequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        private void frmAddRequest_FormClosing(object sender, FormClosingEventArgs e)
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

        private void cmbMenuForSys_SelectedIndexChanged(object sender, EventArgs e)
        {//如果选择的打开方式为方法，那么才让方法名可以编辑
            if (cmbMenuForSys.SelectedIndex == 2)
            {
                this.txtFunctionName.Properties.ReadOnly = false;
            }
            else
            {
                this.txtFunctionName.Properties.ReadOnly = true;
            }
        }

    }
}