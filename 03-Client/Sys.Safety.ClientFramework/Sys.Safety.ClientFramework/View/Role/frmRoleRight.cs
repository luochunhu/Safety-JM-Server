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
using Sys.Safety.Request.Roleright;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Role;

namespace Sys.Safety.ClientFramework.View.Role
{
    public partial class frmRoleRight : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        private long RoleID;
        /// <summary>
        /// 角色对象
        /// </summary>
        private RoleInfo roleDTO = null;        
        /// <summary>
        /// 权限列表
        /// </summary>
        private List<RightInfo> lstRight = new List<RightInfo>();
        /// <summary>
        /// 已分配权限列表
        /// </summary>
        private List<RightInfo> lstFPRight = new List<RightInfo>();             
        /// <summary>
        /// 角色权限列表
        /// </summary>
        private List<RolerightInfo> rightList;

        IRolerightService _RolerightService = ServiceFactory.Create<IRolerightService>();
        IRoleService _RoleService = ServiceFactory.Create<IRoleService>();
        IRightService _RightService = ServiceFactory.Create<IRightService>();
        public frmRoleRight()
        {
            InitializeComponent();
        }

        public frmRoleRight(Dictionary<string, string> param)
        {
            InitializeComponent();
            if (param != null && param.Count > 0)
            {
                this.RoleID = Convert.ToInt64(param["RoleID"]);
            }
            else
            {
                return;
            }
        }

        public frmRoleRight(long roleID)
        {
            InitializeComponent();
            this.RoleID = roleID;
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<TreeListNode> list = new List<TreeListNode>();
            RolerightInfo tmpDto;
            rightList = new List<RolerightInfo>();
            for (int i = 0; i < treeList1.Nodes.Count; i++)
            {
                GetChildNodes(treeList1.Nodes[i], list);
            }

            for (int i = 0; i < list.Count; i++)
            {
                TreeListNode node = list[i];
                if (node["ParentName"].ToString() != "")
                {
                    if (node.Checked)
                    {
                        tmpDto = new RolerightInfo();
                        tmpDto.RoleID = RoleID.ToString();
                        tmpDto.RightID = node["RightID"].ToString();
                        rightList.Add(tmpDto);
                    }
                }
            }
            try
            {
                RolerightsAddRequest rolerightrequest = new RolerightsAddRequest();
                rolerightrequest.roleId = RoleID.ToString();
                rolerightrequest.RolerightInfo = rightList;
                _RolerightService.AddRoleRights(rolerightrequest);               
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 保存数据成功";
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "保存数据成功");
            }
            catch (System.Exception ex)
            {
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 保存数据失败";
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message + " " + StaticMsg.Caption);
            }
        
        }

        private void GetChildNodes(TreeListNode parentNode, List<TreeListNode> list)
        {
            if (parentNode.Nodes.Count <= 0) return;

            foreach (TreeListNode node in parentNode.Nodes)
            {
                list.Add(node);
                if (node.Nodes.Count > 0)
                {
                    GetChildNodes(node, list);
                }
            }
        }

        private void frmRoleRight_Load(object sender, EventArgs e)
        {
            try
            {
                if (RoleID > 0)
                {
                    RoleGetRequest rolerequest = new RoleGetRequest();
                    rolerequest.Id = RoleID.ToString();
                    var result = _RoleService.GetRoleById(rolerequest);
                    roleDTO = result.Data;
                    txtRoleName.Text = roleDTO.RoleName;
                    CreateTreeListTable();

                    RolerightGetByRoleIdRequest rolerightrequest = new RolerightGetByRoleIdRequest();
                    rolerightrequest.RoleId = RoleID.ToString();
                    var resultGetRolerightByRoleId = _RolerightService.GetRightsByRoleId(rolerightrequest);
                    lstFPRight = resultGetRolerightByRoleId.Data;
                    for (int i = 0; i < treeList1.Nodes.Count; i++)
                    {
                        CheckNode(treeList1.Nodes[i]);
                    }
                    for (int i = 0; i < treeList1.Nodes.Count; i++)
                    {
                        if (treeList1.Nodes[i]["ParentName"].ToString() == "")
                        {
                            if (!ValidIsHasNoCheckChildNode(treeList1.Nodes[i]))
                            {
                                treeList1.Nodes[i].CheckState = CheckState.Checked;
                            }
                        }
                    }
                }
                this.Text = "角色授权";
            }
            catch (System.Exception ex)
            {
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Stop, "保存失败,加载数据失败,原因为" + ex.Message);
            }
        }

        private void CheckNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
            {
                if (cnode != null)
                {
                    if (cnode["ParentName"].ToString() != "")
                    {
                        for (int i = 0; i < lstFPRight.Count; i++)
                        {
                            if (cnode["RightName"].ToString() == lstFPRight[i].RightName)
                            {
                                cnode.CheckState = CheckState.Checked;
                                break;
                            }
                        }
                    }
                }
                if (cnode.HasChildren)
                {
                    CheckNode(cnode);
                }
            }
        }

        /// <summary>
        /// 判断 子节点 是否 有 状态为 “未选中”状态 
        /// true 表示有 false 表示为 没有
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ValidIsHasNoCheckChildNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            bool isCheck = false;
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
            {
                if (cnode != null)
                {
                    if (cnode["ParentName"].ToString() != "")
                    {
                        if (cnode.CheckState == CheckState.Unchecked)
                        {
                            isCheck = true;
                            return isCheck;
                        }
                    }
                }
                if (cnode.HasChildren)
                {
                    isCheck = ValidIsHasNoCheckChildNode(cnode);
                    if (isCheck == true)
                    {
                        return isCheck;
                    }
                }
            }
            return isCheck;
        }

        /// <summary>
        /// 生成树
        /// </summary>
        private void CreateTreeListTable()
        {
            DataTable dt = new DataTable();
            DataColumn dcRightName = new DataColumn("RightName", Type.GetType("System.String"));
            DataColumn dcParentName = new DataColumn("ParentName", Type.GetType("System.String"));
            DataColumn dcRightDesc = new DataColumn("RightDesc", Type.GetType("System.String"));
            DataColumn dcRightCode = new DataColumn("RightCode", Type.GetType("System.String"));
            DataColumn dcRightID = new DataColumn("RightID", Type.GetType("System.Int64"));
            dt.Columns.Add(dcRightName);
            dt.Columns.Add(dcParentName);
            dt.Columns.Add(dcRightID);
            dt.Columns.Add(dcRightDesc);
            dt.Columns.Add(dcRightCode);

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
                if (lstRightType.Count > 0)
                {
                    for (i = 0; i < lstRightType.Count; i++)
                    {
                        DataRow dr5 = dt.NewRow();
                        dr5["RightName"] = lstRightType[i].RightType;
                        dr5["ParentName"] = "";
                        dr5["RightID"] = lstRightType[i].RightID;
                        dr5["RightDesc"] = "";
                        dr5["RightCode"] = "";
                        dt.Rows.Add(dr5);

                        if (lstRight.Count > 0)
                        {
                            for (j = 0; j < lstRight.Count; j++)
                            {
                                if (lstRightType[i].RightType == lstRight[j].RightType)
                                {
                                    DataRow dr6 = dt.NewRow();
                                    dr6["RightName"] = lstRight[j].RightName;
                                    dr6["ParentName"] = lstRight[j].RightType;
                                    dr6["RightID"] = lstRight[j].RightID;
                                    dr6["RightDesc"] = lstRight[j].RightDescription;
                                    dr6["RightCode"] = lstRight[j].RightCode;
                                    dt.Rows.Add(dr6);
                                }
                            }
                        }
                    }
                }
            }
            treeList1.DataSource = dt;
            treeList1.ExpandAll();  // 全部展开                   

        }

        /// <summary>
        /// 设置子节点的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        /// <summary>
        /// 设置父节点的状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        private void treeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }
    }
}