using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Display;

namespace Sys.Safety.Client.Graphic
{
    public partial class DevTypeNvigation : XtraUserControl
    {

        /// <summary>
        /// 选择的点
        /// </summary>
        private List<string> CheckPoint = new List<string>();

        private bool _isEdit;

        public DevTypeNvigation(bool isEdit)
        {
            _isEdit = isEdit;
            InitializeComponent();
        }


        private void DevTpeNvigation_Load_1(object sender, EventArgs e)
        {
            //初始化设备类型树
            tree_devlb_init();

            this.treeList1.Dock = System.Windows.Forms.DockStyle.None;
            if (_isEdit)
            {
                this.treeList1.Height = Screen.GetWorkingArea(this).Height - 310;
            }
            else
            {
                this.treeList1.Height = Screen.GetWorkingArea(this).Height - 200;
            }
            //this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        /// <summary>
        /// 设备类别树初始化
        /// </summary>
        private void tree_devlb_init()
        {
            DataRow[] rows = null;
            int i = 0;
            DataView view;
            DataTable dts;
            string lx = "", zl = "";
            DevExpress.XtraTreeList.Nodes.TreeListNode node;
            try
            {
                treeList1.BeginUpdate();
                treeList1.Nodes[0].StateImageIndex = 1;
                treeList1.Nodes[0].Nodes.Clear();
                DataTable dt = OprFuction.GetAlllb("");

                #region 加载类型
                view = new DataView(dt);
                view.Sort = "lxtype";
                dts = view.ToTable(true, "lx", "lxtype");
                if (dts != null && dts.Rows.Count > 0)
                {
                    foreach (DataRow row in dts.Rows)
                    {
                        node = treeList1.AppendNode(
                        new object[] { row["lx"].ToString(), i, "" }, treeList1.Nodes[0]);
                        i++;
                        node.ImageIndex = 0;
                        node.Tag = row["lx"].ToString();
                        node.SelectImageIndex = 1;
                        node.StateImageIndex = 1;
                    }
                }
                #endregion

                #region 加载种类
                view = new DataView(dt);
                dts = view.ToTable(true, "lx", "zl");
                if (treeList1.Nodes[0].Nodes.Count > 0)
                {
                    for (int j = 0; j < treeList1.Nodes[0].Nodes.Count; j++)
                    {
                        lx = treeList1.Nodes[0].Nodes[j].Tag.ToString();
                        rows = dts.Select("lx='" + lx + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
                                //if (row["zl"].ToString() != "")
                                //{
                                node = treeList1.AppendNode(
                               new object[] { string.IsNullOrEmpty(row["zl"].ToString()) ? "" : row["zl"].ToString(), i, "" }, treeList1.Nodes[0].Nodes[j]);
                                i++;
                                node.ImageIndex = 0;
                                node.Tag = row["zl"].ToString();
                                node.SelectImageIndex = 1;
                                node.StateImageIndex = 1;
                                //}
                            }
                        }
                    }
                }
                #endregion

                #region 加载设备名称
                view = new DataView(dt);
                dts = view.ToTable(true, "lx", "zl", "lb");
                if (treeList1.Nodes[0].Nodes.Count > 0)
                {
                    for (int j = 0; j < treeList1.Nodes[0].Nodes.Count; j++)
                    {
                        if (treeList1.Nodes[0].Nodes[j].Nodes.Count > 0)
                        {
                            for (int k = 0; k < treeList1.Nodes[0].Nodes[j].Nodes.Count; k++)
                            {
                                lx = treeList1.Nodes[0].Nodes[j].Tag.ToString();
                                zl = treeList1.Nodes[0].Nodes[j].Nodes[k].Tag.ToString();
                                rows = dts.Select("lx='" + lx + "' and zl='" + zl + "'");
                                if (rows.Length > 0)
                                {
                                    foreach (DataRow row in rows)
                                    {
                                        node = treeList1.AppendNode(
                                        new object[] { row["lb"].ToString(), i, "" }, treeList1.Nodes[0].Nodes[j].Nodes[k]);
                                        i++;
                                        node.ImageIndex = 0;
                                        node.Tag = row["lb"].ToString();
                                        node.SelectImageIndex = 1;
                                        node.StateImageIndex = 1;
                                        node.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                treeList1.EndUpdate();
                treeList1.ExpandAll();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode node = null;
            DevExpress.XtraTreeList.TreeListHitInfo hitInfo;
            if (e.Button == MouseButtons.Left)
            {
                hitInfo = treeList1.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell ||
                    hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.StateImage)
                {
                    node = treeList1.FocusedNode;
                    if (node != null)
                    {
                        var graph = new GraphicOperations();
                        var jsonList = new List<PointNameAndDevName>();
                        if (GISPlatformCenter.Mapobj == null)//找不到对象,直接退出
                        {
                            return;
                        }
                        string json = graph.DoGetMapPointNameAndDevName(GISPlatformCenter.Mapobj);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            jsonList = Basic.Framework.Common.JSONHelper.ParseJSONString<List<PointNameAndDevName>>(json);
                        }
                        if (node.StateImageIndex == 0)
                        {
                            SetState(node, 1);

                            GetCheckNode(treeList1);

                            if (jsonList != null && jsonList.Count > 0)
                            {
                                if (this.treeList1.Nodes[0].StateImageIndex == 1)
                                {
                                    foreach (var item in jsonList)
                                    {
                                        graph.PointDisplay(GISPlatformCenter.Mapobj, item.PointName);
                                    }
                                }
                                else
                                {
                                    foreach (var item in jsonList)
                                    {
                                        string devName = this.CheckPoint.Find((x) => { return x == item.DevName; });
                                        if (!string.IsNullOrWhiteSpace(devName) && !string.IsNullOrWhiteSpace(item.DevName))
                                        {
                                            graph.PointDisplay(GISPlatformCenter.Mapobj, item.PointName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            SetState(node, 0);

                            SetStateS(node.ParentNode, 0);

                            GetCheckNode(treeList1);
                            if (jsonList != null && jsonList.Count > 0)
                            {
                                if (this.treeList1.Nodes[0].StateImageIndex == 1)
                                {
                                    foreach (var item in jsonList)
                                    {
                                        graph.PointDisplay(GISPlatformCenter.Mapobj, item.PointName);
                                    }
                                }
                                else
                                {
                                    foreach (var item in jsonList)
                                    {
                                        string devName = this.CheckPoint.Find((x) => { return x == item.DevName; });
                                        if (string.IsNullOrWhiteSpace(devName) && !string.IsNullOrWhiteSpace(item.DevName))
                                        {
                                            graph.PointHidden(GISPlatformCenter.Mapobj, item.PointName);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 向上遍历
        /// </summary>
        /// <param name="node"></param>
        private void SetStateS(DevExpress.XtraTreeList.Nodes.TreeListNode node, int n)
        {
            if (node != null)
            {
                node.StateImageIndex = n;
                SetStateS(node.ParentNode, n);
            }
        }

        /// <summary>
        ///  向下遍历
        /// </summary>
        /// <param name="node"></param>
        private void SetState(DevExpress.XtraTreeList.Nodes.TreeListNode node, int n)
        {
            if (node != null)
            {
                if (node.StateImageIndex != n)
                {
                    node.StateImageIndex = n;
                }
                if (node.Nodes.Count > 0)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        SetState(node.Nodes[i], n);
                    }
                }
            }
        }


        /// <summary>
        /// 获取选中的测点
        /// </summary>
        /// <param name="tree">树对象</param>      
        /// <returns></returns>
        private void GetCheckNode(DevExpress.XtraTreeList.TreeList item)
        {
            this.CheckPoint.Clear();
            string selectnode = "";
            #region 选中的种类获取
            if (item.Nodes.Count > 0)
            {
                if (item.Nodes[0].StateImageIndex == 1)
                {
                    this.CheckPoint.Add("所有种类");
                }
                else
                {
                    if (item.Nodes[0].Nodes.Count > 0)
                    {
                        for (int j = 0; j < item.Nodes[0].Nodes.Count; j++)
                        {
                            if (item.Nodes[0].Nodes[j].StateImageIndex == 1)
                            {
                                selectnode = item.Nodes[0].Nodes[j].Tag.ToString();
                                if (!this.CheckPoint.Contains(selectnode))
                                {
                                    this.CheckPoint.Add(selectnode);
                                }
                            }
                            if (item.Nodes[0].Nodes[j].Nodes.Count > 0)
                            {
                                for (int i = 0; i < item.Nodes[0].Nodes[j].Nodes.Count; i++)
                                {
                                    if (item.Nodes[0].Nodes[j].Nodes[i].StateImageIndex == 1)
                                    {
                                        selectnode = item.Nodes[0].Nodes[j].Nodes[i].Tag.ToString();
                                        if (!this.CheckPoint.Contains(selectnode))
                                        {
                                            this.CheckPoint.Add(selectnode);
                                        }
                                    }
                                    if (item.Nodes[0].Nodes[j].Nodes[i].Nodes.Count > 0)
                                    {
                                        for (int m = 0; m < item.Nodes[0].Nodes[j].Nodes[i].Nodes.Count; m++)
                                        {
                                            if (item.Nodes[0].Nodes[j].Nodes[i].Nodes[m].StateImageIndex == 1)
                                            {
                                                selectnode = item.Nodes[0].Nodes[j].Nodes[i].Nodes[m].Tag.ToString();
                                                if (!this.CheckPoint.Contains(selectnode))
                                                {
                                                    this.CheckPoint.Add(selectnode);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

    }
    public class PointNameAndDevName
    {
        public string PointName { get; set; }
        public string DevName { get; set; }
    }
}
