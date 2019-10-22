using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request.Right;

namespace Sys.Safety.ClientFramework.View.Autorize
{
    public partial class frmAuthorizeAdmin : DevExpress.XtraEditors.XtraForm
    {
        #region 成员变量
      
        /// <summary>
        /// 权限列表
        /// </summary>
        private List<RightInfo> lstRight = new List<RightInfo>();
        /// <summary>
        /// 权限类型编号
        /// </summary>
        private string rightType = "";
        /// <summary>
        /// 当前选择的权限编号
        /// </summary>
        private int rightID = 0;

        IRightService _RightService = ServiceFactory.Create<IRightService>();
        #endregion

        #region 构造函数
        public frmAuthorizeAdmin()
        {
            InitializeComponent();
        }
        #endregion

        #region 窗体事件
        private void frmAuthorizeAdmin_Load(object sender, EventArgs e)
        {
            AuthorizeGridView.IndicatorWidth = 35;
            LoadData();
            AuthorizeGridView.FocusedRowHandle = -1;
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmAddAuthorize frm = new frmAddAuthorize(0, 0);
                frm.ShowDialog();
                rightType = frm.RightDTO.RightType;
                LoadTreeList();
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  添加数据成功";
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message.ToString() + "  " + StaticMsg.Caption.ToString());
            }
            finally { }
        }

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = AuthorizeGridView.FocusedRowHandle;
                rightID = TypeConvert.ToInt(AuthorizeGridView.GetRowCellValue(rowHandle, "RightID"));
                if (rightID > 0)
                {
                    frmAddAuthorize frm = new frmAddAuthorize(rightID, 1);
                    frm.ShowDialog();
                    rightType = frm.RightDTO.RightType;
                    LoadTreeList();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  修改数据成功";
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

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RightInfo dto = null;
                List<RightInfo> rightTypeDTO = new List<RightInfo>();
                if (rightID > 0)
                {
                    DialogResult dr = UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Confirm, "确定要删除此权限?");
                    if (dr == DialogResult.Yes)
                    {
                        dto = GetData();
                        RightDeleteRequest rightrequest = new RightDeleteRequest();
                        rightrequest.Id = dto.RightID;
                        _RightService.DeleteRight(rightrequest);
                        rightType = dto.RightType;
                        LoadTreeList();

                        rightTypeDTO = GetRightTypeLst(rightType);
                        if (rightTypeDTO.Count == 0)
                        {
                            LoadData();
                        }

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

        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                rightType = e.Node.GetValue("RightType").ToString();
                AuthorizeGridView.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(AuthorizeGridView_FocusedRowChanged);
                gridCtrlView.DataSource = GetRightTypeLst(rightType);
                AuthorizeGridView.FocusedRowHandle = -1;
                if (!string.IsNullOrEmpty(rightType))
                {
                    TreeListNode currNode = treeList.FindNodeByFieldValue("RightType", rightType);
                    if (currNode != null)
                    {
                        treeList.SetFocusedNode(currNode);
                    }
                    else
                    {
                        treeList.SetFocusedNode(null);
                    }
                }
                AuthorizeGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(AuthorizeGridView_FocusedRowChanged);

            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeList_GetSelectImage(object sender, DevExpress.XtraTreeList.GetSelectImageEventArgs e)
        {
            if (e.FocusedNode)
                e.NodeImageIndex = 2;
            else
                e.NodeImageIndex = 1;
        }

        private void AuthorizeGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowHandle = AuthorizeGridView.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            rightID = TypeConvert.ToInt(AuthorizeGridView.GetRowCellValue(rowHandle, "RightID"));
        }

        private void btnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }
        #endregion

        #region 自定义函数接口
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            var result = _RightService.GetRightList();
            lstRight = result.Data;
            gridCtrlView.DataSource = lstRight;
            rightType = "";
            LoadTreeList();
        }

        /// <summary>
        /// 收据数据，用于删除权限对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private RightInfo GetData()
        {
            RightInfo type = new RightInfo();
            object obj = AuthorizeGridView.GetFocusedRow();
            type = (RightInfo)obj;
            type.InfoState = InfoState.Delete; 
            return type;
        }

        /// <summary>
        /// 加载TreeList
        /// </summary>
        private void LoadTreeList()
        {
            List<RightInfo> lstRightType = new List<RightInfo>();
            int i = 0, j = 0;
            var result = _RightService.GetRightList();
            lstRight = result.Data;
            if (lstRight.Count > 0)
            {
                for (i = 0; i < lstRight.Count; i++)
                {
                    if (!string.IsNullOrEmpty(lstRight[i].RightType))
                    {
                        if (lstRightType.Count == 0)
                        {
                            lstRightType.Add(lstRight[i]);
                        }
                        else
                        {

                            for (j = 0; j < lstRightType.Count; j++)
                            {
                                if (lstRight[i].RightType != lstRightType[j].RightType)
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (j == lstRightType.Count)
                            {
                                lstRightType.Add(lstRight[i]);
                            }
                        }
                    }
                }
            }
            treeList.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeList_FocusedNodeChanged);
            treeList.DataSource = lstRightType;
            treeList.ExpandAll();
            treeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeList_FocusedNodeChanged);
            if (!string.IsNullOrEmpty(rightType))
            {
                TreeListNode currNode = treeList.FindNodeByFieldValue("RightType", rightType);
                if (currNode != null) treeList.SetFocusedNode(currNode);
                else treeList.SetFocusedNode(null);
            }

        }

        /// <summary>
        /// 根据权限类型获取权限对象列表
        /// </summary>
        /// <param name="rightType"></param>
        /// <returns></returns>
        private List<RightInfo> GetRightTypeLst(string rightType)
        {
            List<RightInfo> lstRightInfo = new List<RightInfo>();
            int i = 0;
            if (lstRight.Count > 0)
            {
                for (i = 0; i < lstRight.Count; i++)
                {
                    if (!string.IsNullOrEmpty(lstRight[i].RightType))
                    {
                        if (lstRight[i].RightType == rightType)
                        {
                            lstRightInfo.Add(lstRight[i]);
                        }
                    }
                }
            }
            return lstRightInfo;
        }
        #endregion
    }
}