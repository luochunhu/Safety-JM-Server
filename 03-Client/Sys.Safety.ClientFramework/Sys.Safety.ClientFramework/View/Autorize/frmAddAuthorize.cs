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
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Right;
using Basic.Framework.Web;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.View.Autorize
{
    public partial class frmAddAuthorize : DevExpress.XtraEditors.XtraForm
    {

        /// <summary>
        /// /0:新增 1:修改        
        /// </summary>
        private int type = 0;
        /// <summary>
        /// 权限对象
        /// </summary>
        private RightInfo rightDTO = null;

        public RightInfo RightDTO
        {
            get { return rightDTO; }
            set { rightDTO = value; }
        }
        /// <summary>
        /// 复制权限对象
        /// </summary>
        private RightInfo CopyRightDTO = new RightInfo();
        /// <summary>
        /// 权限编号
        /// </summary>
        private long rightID;

        IRightService _RightService = ServiceFactory.Create<IRightService>();


        public frmAddAuthorize()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="RightID">权限编号</param>
        /// <param name="Type">0:新增 1:修改</param>
        public frmAddAuthorize(int RightID, int Type)
        {
            InitializeComponent();
            type = Type;
            rightID = RightID;
        }

        public frmAddAuthorize(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.type = Convert.ToInt32(param["Type"]);
                this.rightID = Convert.ToInt64(param["RightID"]);
            }
            else
            {
                return;
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 收集数据
        /// </summary>
        private void GetData()
        {
            rightDTO.RightName = txtRightName.Text;
            rightDTO.RightCode = txtRightCode.Text;
            rightDTO.RightDescription = txtRightDescription.Text;
            rightDTO.RightType = txtRightType.Text;
            rightDTO.CreateName = txtTrueUserName.Text;
            rightDTO.CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (type > 0)
            {
                RightGetRequest rightrequest = new RightGetRequest();
                rightrequest.Id = rightID.ToString();
                var result = _RightService.GetRightById(rightrequest);
                rightDTO = result.Data;
                rightDTO.InfoState = InfoState.Modified;
            }
            else
            {
                rightDTO = new RightInfo();
                rightDTO.InfoState = InfoState.AddNew;
            }
            InitData();
            CreateNewVO();
        }
        /// <summary>
        /// 初始化窗体数据
        /// </summary>
        private void InitData()
        {
            txtRightName.Text = rightDTO.RightName;
            txtRightCode.Text = rightDTO.RightCode;
            txtRightDescription.Text = rightDTO.RightDescription;
            txtRightType.Text = rightDTO.RightType;
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            txtTrueUserName.Text = _ClientItem.UserName;
        }
        /// <summary>
        /// 创建权限对象
        /// </summary>
        private void CreateNewVO()
        {
            CopyRightDTO.RightName = txtRightName.Text;
            CopyRightDTO.RightCode = txtRightCode.Text;
            CopyRightDTO.RightDescription = txtRightDescription.Text;
            CopyRightDTO.RightType = txtRightType.Text;
            CopyRightDTO.CreateName = txtTrueUserName.Text;

        }
        /// <summary>
        /// 检查数据是否变脏
        /// </summary>
        /// <returns></returns>
        private bool DataIsChange()
        {
            if (txtRightCode.Text != CopyRightDTO.RightCode)
                return true;
            if (txtRightDescription.Text != CopyRightDTO.RightDescription)
                return true;
            if (txtRightName.Text != CopyRightDTO.RightName)
                return true;
            if (txtRightType.Text != CopyRightDTO.RightType)
                return true;
            if (txtTrueUserName.Text != CopyRightDTO.CreateName)
                return true;
            return false;

        }
        private void frmAddAuthorize_Load(object sender, EventArgs e)
        {
            LoadData();
            if (type == 1)
            {
                this.Text = "修改权限";
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GetData();
                RightAddRequest rightrequest = new RightAddRequest();
                rightrequest.RightInfo = rightDTO;
                var result = _RightService.AddRightEx(rightrequest);
                if (result.Code == 100)
                {
                    rightDTO = result.Data;
                    rightID = long.Parse(rightDTO.RightID);
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
        private void frmAddAuthorize_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="param"></param>
        public void DeleteRight(Dictionary<string, string> param)
        {
            try
            {
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    string RightID = "";
                    if (param != null && param.Count > 0)
                    {
                        RightID = param["RightID"].ToString();
                    }
                    RightDeleteRequest rightrequest = new RightDeleteRequest();
                    rightrequest.Id = RightID;
                    _RightService.DeleteRight(rightrequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}