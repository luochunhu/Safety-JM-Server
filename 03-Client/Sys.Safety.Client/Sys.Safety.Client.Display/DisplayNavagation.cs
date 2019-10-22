using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;
using Basic.Framework.Logging;
using System.Threading;

namespace Sys.Safety.Client.Display
{
    public partial class DisplayNavagation : XtraUserControl
    {
        public DisplayNavagation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 刷新实时页面委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="n"></param>
        public delegate void mydelegate(int type, int n, List<string> area);

        /// <summary>
        /// 刷新实时页面委托
        /// </summary>
        public mydelegate my;

        /// <summary>
        /// 选择的点
        /// </summary>
        private List<string> checkmsg = new List<string>();

        /// <summary>
        /// 显示编排
        /// </summary>
        private int ShowType = 0;

        /// <summary>
        /// 自定义编排页面
        /// </summary>
        private int bpShowPage = 0;
        private Thread freshthread;

        private void DisplayNavagation_Load(object sender, EventArgs e)
        {
            Initree();
            Click_init();
            freshthread = new Thread(new ThreadStart(getRefreshState));
            freshthread.IsBackground = true;
            freshthread.Start();
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        private void Initree()
        {
            #region 自定义编排树初始化
            tree_custom_init(0, 12, "");
            #endregion

            #region 设备信息树初始化
            tree_devtype_init();
            #endregion

            #region 类别树初始化
            tree_devlb_init();
            #endregion

            tree_zt_init();
        }

        /// <summary>
        /// 状态树初始化
        /// </summary>
        private void tree_zt_init()
        {
            treeList_zt.BeginUpdate();
            treeList_zt.Nodes[0].StateImageIndex = 0;
            try
            {
                for (int j = 0; j < treeList_zt.Nodes[0].Nodes.Count; j++)
                {
                    treeList_zt.Nodes[0].Nodes[j].StateImageIndex = 0;
                    if (treeList_zt.Nodes[0].Nodes[j].Nodes.Count > 0)
                    {
                        for (int i = 0; i < treeList_zt.Nodes[0].Nodes[j].Nodes.Count; i++)
                        {
                            treeList_zt.Nodes[0].Nodes[j].Nodes[i].StateImageIndex = 0;
                        }
                    }
                }
                treeList_zt.EndUpdate();
                treeList_zt.ExpandAll();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }

        /// <summary>
        /// 设备树初始化
        /// </summary>
        private void tree_devtype_init()
        {
            DataTable dt = GetAllDev();

            DevExpress.XtraTreeList.Nodes.TreeListNode node;
            treeList_fz.BeginUpdate();
            treeList_fz.Nodes[0].Nodes.Clear();
            treeList_fz.Nodes[0].StateImageIndex = 0;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        node = treeList_fz.AppendNode(
                        new object[] { dt.Rows[i]["point"].ToString(), i.ToString(), "" }, treeList_fz.Nodes[0]);
                        node.ImageIndex = 0;
                        node.Tag = dt.Rows[i]["fzh"].ToString();
                        node.SelectImageIndex = 1;
                        node.StateImageIndex = 0;
                    }
                }
                treeList_fz.EndUpdate();
                treeList_fz.ExpandAll();

                //根据配置判断，是否可以休眠  20180307
                string enableDormancyStr = Model.RealInterfaceFuction.ReadConfig("EnableDormancy");
                bool enableDormancy = true;

                if (enableDormancyStr == "1")
                {
                    enableDormancy = true;
                }
                else
                {
                    enableDormancy = false;
                }
                if (!enableDormancy)
                {
                    for (int j = 0; j < treeList_zt.Nodes[0].Nodes.Count; j++)
                    {
                        if (treeList_zt.Nodes[0].Nodes[j].GetValue("Column5").ToString() == "休眠")
                        {
                            treeList_zt.Nodes[0].Nodes.RemoveAt(j);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
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
                treeList_zl.BeginUpdate();
                treeList_zl.Nodes[0].StateImageIndex = 0;
                treeList_zl.Nodes[0].Nodes.Clear();
                DataTable dt = OprFuction.GetAlllb("");

                #region 加载类型
                view = new DataView(dt);
                view.Sort = "lxtype";
                dts = view.ToTable(true, "lx", "lxtype");
                if (dts != null && dts.Rows.Count > 0)
                {
                    foreach (DataRow row in dts.Rows)
                    {
                        node = treeList_zl.AppendNode(
                        new object[] { row["lx"].ToString(), i, "" }, treeList_zl.Nodes[0]);
                        i++;
                        node.ImageIndex = 0;
                        node.Tag = row["lx"].ToString();
                        node.SelectImageIndex = 1;
                        node.StateImageIndex = 0;
                    }
                }
                #endregion

                #region 加载种类
                view = new DataView(dt);
                dts = view.ToTable(true, "lx", "zl");
                if (treeList_zl.Nodes[0].Nodes.Count > 0)
                {
                    for (int j = 0; j < treeList_zl.Nodes[0].Nodes.Count; j++)
                    {
                        lx = treeList_zl.Nodes[0].Nodes[j].Tag.ToString();
                        rows = dts.Select("lx='" + lx + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow row in rows)
                            {
                                //if (row["zl"].ToString() != "")
                                //{
                                node = treeList_zl.AppendNode(
                                    new object[] { string.IsNullOrEmpty(row["zl"].ToString()) ? "" : row["zl"].ToString(), i, "" }, treeList_zl.Nodes[0].Nodes[j]);
                                i++;
                                node.ImageIndex = 0;
                                node.Tag = row["zl"].ToString();
                                node.SelectImageIndex = 1;
                                node.StateImageIndex = 0;
                                //}
                            }
                        }
                    }
                }
                #endregion

                #region 加载设备名称
                view = new DataView(dt);
                dts = view.ToTable(true, "lx", "zl", "lb");
                if (treeList_zl.Nodes[0].Nodes.Count > 0)
                {
                    for (int j = 0; j < treeList_zl.Nodes[0].Nodes.Count; j++)
                    {
                        if (treeList_zl.Nodes[0].Nodes[j].Nodes.Count > 0)
                        {
                            for (int k = 0; k < treeList_zl.Nodes[0].Nodes[j].Nodes.Count; k++)
                            {
                                lx = treeList_zl.Nodes[0].Nodes[j].Tag.ToString();
                                zl = treeList_zl.Nodes[0].Nodes[j].Nodes[k].Tag.ToString();
                                rows = dts.Select("lx='" + lx + "' and zl='" + zl + "'");
                                if (rows.Length > 0)
                                {
                                    foreach (DataRow row in rows)
                                    {
                                        node = treeList_zl.AppendNode(
                                        new object[] { row["lb"].ToString(), i, "" }, treeList_zl.Nodes[0].Nodes[j].Nodes[k]);
                                        i++;
                                        node.ImageIndex = 0;
                                        node.Tag = row["lb"].ToString();
                                        node.SelectImageIndex = 1;
                                        node.StateImageIndex = 0;
                                        node.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                treeList_zl.EndUpdate();
                treeList_zl.ExpandAll();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取所有设备
        /// </summary>
        /// <returns></returns>
        private DataTable GetAllDev()
        {
            DataTable dt = new DataTable();
            string point = "", wz = "", lb = "";
            dt.Columns.Add(new DataColumn("fzh", typeof(int)));
            dt.Columns.Add(new DataColumn("point", typeof(string)));
            DataRow[] rows = null;
            try
            {
                lock (StaticClass.allPointDtLockObj)
                {
                    rows = StaticClass.AllPointDt.Select("lx='分站'", "point");
                    if (rows != null && rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            point = "";
                            wz = "";
                            lb = "";
                            if (!row.IsNull("point"))
                            {
                                point = row["point"].ToString();
                            }
                            if (!row.IsNull("wz"))
                            {
                                wz = row["wz"].ToString();
                            }
                            //if (!row.IsNull("lb"))
                            //{
                            //    lb = row["lb"].ToString();
                            //}
                            if (point.Contains("A") || point.Contains("D") || point.Contains("C"))
                            {
                                continue;
                            }
                            dt.Rows.Add(row["fzh"], string.Format("[{0}]{1}", point, wz));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 自定义编排初始化树
        /// </summary>
        public void tree_custom_init(int n, int m, string s)
        {
            if (m == 12)
            {
                #region 给自定义树节点赋名称
                DevExpress.XtraTreeList.Nodes.TreeListNode node;
                treeList_bp.BeginUpdate();
                treeList_bp.Nodes[0].Nodes.Clear();
                try
                {
                    for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
                    {
                        if (StaticClass.arrangeconfig.CustomCofig[i] != null)
                        {
                            node = treeList_bp.AppendNode(
                            new object[] { StaticClass.arrangeconfig.CustomCofig[i].PageName, i.ToString(), "" }, treeList_bp.Nodes[0]);
                            node.ImageIndex = 0;
                            node.Tag = (i + 1).ToString();
                            node.SelectImageIndex = 1;
                            node.StateImageIndex = 0;
                        }
                    }
                    treeList_bp.EndUpdate();
                    treeList_bp.ExpandAll();
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs(ex.Message, ex);
                }
                #endregion
            }
        }

        /// <summary>
        ///  刷新
        /// </summary>
        public void refresh()
        {
            try
            {
                //StaticClass.AllPointDt = Model.RealInterfaceFuction.GetAllPoint();
                //#region 初始化
                //Initree();
                //#endregion

                //#region 赋值
                //SelectTreeList();
                //#endregion

                //#region 刷新
                //if (ShowType != 5)
                //{
                //    if (StaticClass.real_del != null)
                //    {
                //        StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                //    }
                //}
                //#endregion

                //txy 20170401  优化刷新

                DataTable dt = null;
                dt = Model.RealInterfaceFuction.GetAllPoint();               
                lock (StaticClass.allPointDtLockObj)
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        StaticClass.AllPointDt = dt;
                    }
                    else
                    {
                        StaticClass.AllPointDt = Model.RealInterfaceFuction.GetAllPoint();
                    }
                }
                if (dt != null)
                {
                    MethodInvoker In = new MethodInvoker(() => RefreshControl());
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke(In);
                    }
                }

            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }
        private void RefreshControl()
        {
            #region 初始化
            newtreelist();
            Initree();
            #endregion

            #region 赋值
            SelectTreeList();
            #endregion

            #region 刷新
            if (ShowType != 5)
            {
                if (StaticClass.real_del != null)
                {
                    StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                }
            }
            #endregion
        }

        private void newtreelist()
        {
            try
            {
                xtraTabPage2.Controls.Remove(treeList_zl);
                this.treeList_zl = new DevExpress.XtraTreeList.TreeList();
                xtraTabPage2.Controls.Add(treeList_zl);
                this.treeList_zl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                this.treeList_zl.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.Column3});
                this.treeList_zl.Dock = System.Windows.Forms.DockStyle.Fill;
                this.treeList_zl.Location = new System.Drawing.Point(0, 0);
                this.treeList_zl.Name = "treeList_zl";
                this.treeList_zl.BeginUnboundLoad();
                this.treeList_zl.AppendNode(new object[] {
            "所有种类"}, -1, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "分站"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "模拟量"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "开关量"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "控制量"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "累计量"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "导出量"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "识别器"}, 0, 0, 1, 0);
                this.treeList_zl.AppendNode(new object[] {
            "其它"}, 0);
                this.treeList_zl.EndUnboundLoad();
                this.treeList_zl.OptionsBehavior.Editable = false;
                this.treeList_zl.OptionsSelection.MultiSelect = true;
                this.treeList_zl.OptionsView.ShowColumns = false;
                this.treeList_zl.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.ShowAlways;
                this.treeList_zl.OptionsView.ShowHorzLines = false;
                this.treeList_zl.OptionsView.ShowIndicator = false;
                this.treeList_zl.OptionsView.ShowVertLines = false;
                this.treeList_zl.Size = new System.Drawing.Size(339, 476);
                this.treeList_zl.StateImageList = this.imageList;
                this.treeList_zl.TabIndex = 2;
                this.treeList_zl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_zl_MouseClick);

            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 刷 新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tools_refresh_Click(object sender, EventArgs e)
        {
            StaticClass.adddelegate();//重新注册委托
            refresh();
        }

        /// <summary>
        /// 选中树的节点
        /// </summary>
        private void SelectTreeList()
        {
            try
            {
                switch (ShowType)
                {
                    case 1:
                        #region 设备
                        if (checkmsg.Count > 0)
                        {
                            if (checkmsg.Contains("所有设备"))
                            {
                                treeList_fz.Nodes[0].StateImageIndex = 1;
                                if (treeList_fz.Nodes[0].Nodes.Count > 0)
                                {
                                    for (int i = 0; i < treeList_fz.Nodes[0].Nodes.Count; i++)
                                    {
                                        treeList_fz.Nodes[0].Nodes[i].StateImageIndex = 1;
                                    }
                                }
                            }
                            else
                            {
                                if (treeList_fz.Nodes[0].Nodes.Count > 0)
                                {
                                    for (int i = 0; i < treeList_fz.Nodes[0].Nodes.Count; i++)
                                    {
                                        if (treeList_fz.Nodes[0].Nodes[i].Tag != null
                                            && checkmsg.Contains(treeList_fz.Nodes[0].Nodes[i].Tag.ToString()))
                                        {
                                            treeList_fz.Nodes[0].Nodes[i].StateImageIndex = 1;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case 2:
                        #region 种类
                        if (checkmsg.Count > 0)
                        {
                            if (checkmsg.Contains("所有种类"))
                            {
                                if (treeList_zl.Nodes.Count > 0)
                                {
                                    treeList_zl.Nodes[0].StateImageIndex = 1;
                                    if (treeList_zl.Nodes[0].Nodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeList_zl.Nodes[0].Nodes.Count; j++)
                                        {
                                            treeList_zl.Nodes[0].Nodes[j].StateImageIndex = 1;
                                            if (treeList_zl.Nodes[0].Nodes[j].Nodes.Count > 0)
                                            {
                                                for (int i = 0; i < treeList_zl.Nodes[0].Nodes[j].Nodes.Count; i++)
                                                {
                                                    treeList_zl.Nodes[0].Nodes[j].Nodes[i].StateImageIndex = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (treeList_zl.Nodes.Count > 0)
                                {
                                    if (treeList_zl.Nodes[0].Nodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeList_zl.Nodes[0].Nodes.Count; j++)
                                        {
                                            if (checkmsg.Contains(treeList_zl.Nodes[0].Nodes[j].Tag.ToString()))
                                            {
                                                treeList_zl.Nodes[0].Nodes[j].StateImageIndex = 1;
                                            }
                                            if (treeList_zl.Nodes[0].Nodes[j].Nodes.Count > 0)
                                            {
                                                for (int i = 0; i < treeList_zl.Nodes[0].Nodes[j].Nodes.Count; i++)
                                                {
                                                    if (checkmsg.Contains(treeList_zl.Nodes[0].Nodes[j].Nodes[i].Tag.ToString()))
                                                    {
                                                        treeList_zl.Nodes[0].Nodes[j].Nodes[i].StateImageIndex = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case 4:
                        #region 状态
                        if (checkmsg.Count > 0)
                        {
                            if (checkmsg.Contains("所有状态"))
                            {
                                if (treeList_zt.Nodes.Count > 0)
                                {
                                    treeList_zt.Nodes[0].StateImageIndex = 1;
                                    if (treeList_zt.Nodes[0].Nodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeList_zt.Nodes[0].Nodes.Count; j++)
                                        {
                                            treeList_zt.Nodes[0].Nodes[j].StateImageIndex = 1;
                                            if (treeList_zt.Nodes[0].Nodes[j].Nodes.Count > 0)
                                            {
                                                for (int i = 0; i < treeList_zt.Nodes[0].Nodes[j].Nodes.Count; i++)
                                                {
                                                    treeList_zt.Nodes[0].Nodes[j].Nodes[i].StateImageIndex = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (treeList_zt.Nodes.Count > 0)
                                {
                                    if (treeList_zt.Nodes[0].Nodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeList_zt.Nodes[0].Nodes.Count; j++)
                                        {
                                            if (checkmsg.Contains(treeList_zt.Nodes[0].Nodes[j].GetValue("Column5").ToString()))
                                            {
                                                treeList_zt.Nodes[0].Nodes[j].StateImageIndex = 1;
                                            }
                                            if (treeList_zt.Nodes[0].Nodes[j].Nodes.Count > 0)
                                            {
                                                for (int i = 0; i < treeList_zt.Nodes[0].Nodes[j].Nodes.Count; i++)
                                                {
                                                    if (checkmsg.Contains(treeList_zt.Nodes[0].Nodes[j].Nodes[i].GetValue("Column5").ToString()))
                                                    {
                                                        treeList_zt.Nodes[0].Nodes[j].Nodes[i].StateImageIndex = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case 5:
                        #region 自定义编排
                        if (bpShowPage > 0)
                        {
                            treeList_bp.BeginUpdate();
                            if (treeList_bp.Nodes.Count > 0)
                            {
                                for (int i = 0; i < treeList_bp.Nodes[0].Nodes.Count; i++)
                                {
                                    if (treeList_bp.Nodes[0].Nodes[i].Tag != null)
                                    {
                                        if (treeList_bp.Nodes[0].Nodes[i].Tag.ToString() == bpShowPage.ToString())
                                        {
                                            treeList_bp.FocusedNode = treeList_bp.Nodes[0].Nodes[i];
                                            break;
                                        }
                                    }
                                }
                            }
                            treeList_bp.EndUpdate();
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 清空选中状态 三级树
        /// </summary>
        private void ClearTreeState(DevExpress.XtraTreeList.TreeList item)
        {
            if (item.Nodes.Count > 0)
            {
                item.Nodes[0].StateImageIndex = 0;
                if (item.Nodes[0].Nodes.Count > 0)
                {
                    for (int i = 0; i < item.Nodes[0].Nodes.Count; i++)
                    {
                        item.Nodes[0].Nodes[i].StateImageIndex = 0;
                        if (item.Nodes[0].Nodes[i].Nodes.Count > 0)
                        {
                            for (int j = 0; j < item.Nodes[0].Nodes[i].Nodes.Count; j++)
                            {
                                item.Nodes[0].Nodes[i].Nodes[j].StateImageIndex = 0;
                                if (item.Nodes[0].Nodes[i].Nodes[j].Nodes.Count > 0)
                                {
                                    for (int k = 0; k < item.Nodes[0].Nodes[i].Nodes[j].Nodes.Count; k++)//增加4级菜单取消选择
                                    {
                                        item.Nodes[0].Nodes[i].Nodes[j].Nodes[k].StateImageIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清空状态
        /// </summary>
        private void ClearState()
        {
            switch (ShowType)
            {
                case 1:
                    ClearTreeState(treeList_fz);
                    break;
                case 2:
                    ClearTreeState(treeList_zl);
                    break;
                case 4:
                    ClearTreeState(treeList_zt);
                    break;
                case 5:
                    treeList_bp.FocusedNode = null;
                    for (int i = 0; i < treeList_bp.Nodes[0].Nodes.Count; i++)
                    {
                        treeList_bp.Nodes[0].Nodes[i].StateImageIndex = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// 获取选中的测点
        /// </summary>
        /// <param name="tree">树对象</param>
        /// <param name="type" >1-分站树 2-种类树 3-区域树 4-状态树</param>
        /// <returns></returns>
        private void GetCheckNode(DevExpress.XtraTreeList.TreeList item, int type)
        {
            checkmsg.Clear();
            string selectnode = "";
            switch (type)
            {
                case 1:
                    #region 选中的分站获取
                    if (item.Nodes.Count > 0)
                    {
                        if (item.Nodes[0].StateImageIndex == 1)
                        {
                            checkmsg.Add("所有设备");
                        }
                        else
                        {
                            for (int i = 0; i < item.Nodes[0].Nodes.Count; i++)
                            {
                                if (item.Nodes[0].Nodes[i].StateImageIndex == 1)
                                {
                                    checkmsg.Add(item.Nodes[0].Nodes[i].Tag.ToString());
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 2:
                    #region 选中的种类获取
                    if (item.Nodes.Count > 0)
                    {
                        if (item.Nodes[0].StateImageIndex == 1)
                        {
                            checkmsg.Add("所有种类");
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
                                        if (!checkmsg.Contains(selectnode))
                                        {
                                            checkmsg.Add(selectnode);
                                        }
                                    }
                                    if (item.Nodes[0].Nodes[j].Nodes.Count > 0)
                                    {
                                        for (int i = 0; i < item.Nodes[0].Nodes[j].Nodes.Count; i++)
                                        {
                                            if (item.Nodes[0].Nodes[j].Nodes[i].StateImageIndex == 1)
                                            {
                                                selectnode = item.Nodes[0].Nodes[j].Nodes[i].Tag.ToString();
                                                if (!checkmsg.Contains(selectnode))
                                                {
                                                    checkmsg.Add(selectnode);
                                                }
                                            }
                                            if (item.Nodes[0].Nodes[j].Nodes[i].Nodes.Count > 0)
                                            {
                                                for (int m = 0; m < item.Nodes[0].Nodes[j].Nodes[i].Nodes.Count; m++)
                                                {
                                                    if (item.Nodes[0].Nodes[j].Nodes[i].Nodes[m].StateImageIndex == 1)
                                                    {
                                                        selectnode = item.Nodes[0].Nodes[j].Nodes[i].Nodes[m].Tag.ToString();
                                                        if (!checkmsg.Contains(selectnode))
                                                        {
                                                            checkmsg.Add(selectnode);
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
                    break;
                case 4:
                    #region 选中的状态
                    if (item.Nodes.Count > 0)
                    {
                        if (item.Nodes[0].StateImageIndex == 1)
                        {
                            checkmsg.Add("所有状态");
                        }
                        else
                        {
                            if (item.Nodes[0].Nodes.Count > 0)
                            {
                                for (int j = 0; j < item.Nodes[0].Nodes.Count; j++)
                                {
                                    if (item.Nodes[0].Nodes[j].StateImageIndex == 1)
                                    {
                                        checkmsg.Add(item.Nodes[0].Nodes[j].GetValue("Column5").ToString());
                                    }
                                    if (item.Nodes[0].Nodes[j].Nodes.Count > 0)
                                    {
                                        for (int i = 0; i < item.Nodes[0].Nodes[j].Nodes.Count; i++)
                                        {
                                            if (item.Nodes[0].Nodes[j].Nodes[i].StateImageIndex == 1)
                                            {
                                                checkmsg.Add(item.Nodes[0].Nodes[j].Nodes[i].GetValue("Column5").ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case 5:
                    #region 自定义编排
                    if (treeList_bp.Nodes.Count > 0)
                    {
                        for (int i = 0; i < treeList_bp.Nodes[0].Nodes.Count; i++)
                        {
                            if (treeList_bp.Nodes[0].Nodes[i].StateImageIndex == 1)
                            {
                                if (treeList_bp.Nodes[0].Nodes[i].Tag != null)
                                {
                                    bpShowPage = int.Parse(treeList_bp.Nodes[0].Nodes[i].Tag.ToString());
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
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
        /// 设备树鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_fz_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode node = null;
            DevExpress.XtraTreeList.TreeListHitInfo hitInfo;
            if (e.Button == MouseButtons.Left)
            {
                hitInfo = treeList_fz.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell ||
                    hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.StateImage)
                {
                    node = treeList_fz.FocusedNode;
                    if (node != null)
                    {
                        if (node.StateImageIndex == 0)
                        {
                            SetState(node, 1);
                        }
                        else
                        {
                            SetState(node, 0);
                            SetStateS(node.ParentNode, 0);
                        }
                        GetCheckNode(treeList_fz, 1);
                        if (ShowType != 1)
                        {
                            ClearState();
                            ShowType = 1;
                        }
                        if (StaticClass.real_del != null)
                        {
                            StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 初始状态
        /// </summary>
        private void Click_init()
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode node = null;
            try
            {
                treeList_fz.FocusedNode = treeList_fz.Nodes[0];
                node = treeList_fz.FocusedNode;
                if (node != null)
                {
                    if (node.StateImageIndex == 0)
                    {
                        SetState(node, 1);
                    }
                    else
                    {
                        SetState(node, 0);
                        SetStateS(node.ParentNode, 0);
                    }
                    GetCheckNode(treeList_fz, 1);
                    if (ShowType != 1)
                    {
                        ClearState();
                        ShowType = 1;
                    }
                    if (StaticClass.real_del != null)
                    {
                        StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 种类树鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_zl_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode node = null;
            DevExpress.XtraTreeList.TreeListHitInfo hitInfo;
            if (e.Button == MouseButtons.Left)
            {
                hitInfo = treeList_zl.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell ||
                    hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.StateImage)
                {
                    node = treeList_zl.FocusedNode;
                    if (node != null)
                    {
                        if (node.StateImageIndex == 0)
                        {
                            SetState(node, 1);
                        }
                        else
                        {
                            SetState(node, 0);
                            SetStateS(node.ParentNode, 0);
                        }
                        GetCheckNode(treeList_zl, 2);
                        if (ShowType != 2)
                        {
                            ClearState();
                            ShowType = 2;
                        }
                        if (StaticClass.real_del != null)
                        {
                            StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 状态树单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_zt_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode node = null;
            DevExpress.XtraTreeList.TreeListHitInfo hitInfo;
            if (e.Button == MouseButtons.Left)
            {
                hitInfo = treeList_zt.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell ||
                    hitInfo.HitInfoType == DevExpress.XtraTreeList.HitInfoType.StateImage)
                {
                    node = treeList_zt.FocusedNode;
                    if (node != null)
                    {
                        if (node.StateImageIndex == 0)
                        {
                            SetState(node, 1);
                        }
                        else
                        {
                            SetState(node, 0);
                            SetStateS(node.ParentNode, 0);
                        }
                        GetCheckNode(treeList_zt, 4);
                        if (ShowType != 4)
                        {
                            ClearState();
                            ShowType = 4;
                        }
                        if (StaticClass.real_del != null)
                        {
                            StaticClass.real_del(ShowType, bpShowPage, checkmsg);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自定义编排树焦点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_bp_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (treeList_bp.FocusedNode != null && treeList_bp.FocusedNode != treeList_bp.Nodes[0])
            {
                for (int i = 0; i < treeList_bp.Nodes[0].Nodes.Count; i++)
                {
                    treeList_bp.Nodes[0].Nodes[i].StateImageIndex = 0;
                }
                treeList_bp.FocusedNode.StateImageIndex = 1;
                if (treeList_bp.FocusedNode.Tag != null)
                {
                    bpShowPage = int.Parse(treeList_bp.FocusedNode.Tag.ToString());
                }
                if (ShowType != 5)
                {
                    ClearState();
                    ShowType = 5;
                }
                if (StaticClass.real_del != null)
                {
                    StaticClass.real_del(ShowType, bpShowPage, null);
                }
            }
        }

        private void getRefreshState()
        {
            while (true)
            {
                try
                {
                    if (StaticClass.Definechange)
                    {
                        DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                        DateTime time = DateTime.Parse(StaticClass.DefineTime);
                        if ((nowtime - time).TotalSeconds > 10)
                        {
                            StaticClass.Definechange = false;
                            refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(3000);
            }
        }

    }
}
