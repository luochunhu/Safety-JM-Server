namespace Sys.Safety.Client.Display
{
    partial class DisplayNavagation
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayNavagation));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.tools_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.treeList_fz = new DevExpress.XtraTreeList.TreeList();
            this.Column2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageList = new System.Windows.Forms.ImageList();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.treeList_zl = new DevExpress.XtraTreeList.TreeList();
            this.Column3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.treeList_zt = new DevExpress.XtraTreeList.TreeList();
            this.Column5 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.treeList_bp = new DevExpress.XtraTreeList.TreeList();
            this.Column1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_fz)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_zl)).BeginInit();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_zt)).BeginInit();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList_bp)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.xtraTabControl1.Appearance.Options.UseBackColor = true;
            this.xtraTabControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(345, 778);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage4,
            this.xtraTabPage5});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tools_refresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 26);
            // 
            // tools_refresh
            // 
            this.tools_refresh.Name = "tools_refresh";
            this.tools_refresh.Size = new System.Drawing.Size(108, 22);
            this.tools_refresh.Text = "刷  新";
            this.tools_refresh.Click += new System.EventHandler(this.tools_refresh_Click);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.treeList_fz);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(339, 749);
            this.xtraTabPage1.Text = "分站";
            // 
            // treeList_fz
            // 
            this.treeList_fz.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList_fz.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.Column2});
            this.treeList_fz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_fz.Location = new System.Drawing.Point(0, 0);
            this.treeList_fz.Name = "treeList_fz";
            this.treeList_fz.BeginUnboundLoad();
            this.treeList_fz.AppendNode(new object[] {
            "所有设备"}, -1, 0, 1, 0);
            this.treeList_fz.EndUnboundLoad();
            this.treeList_fz.OptionsBehavior.Editable = false;
            this.treeList_fz.OptionsMenu.EnableFooterMenu = false;
            this.treeList_fz.OptionsSelection.MultiSelect = true;
            this.treeList_fz.OptionsView.ShowColumns = false;
            this.treeList_fz.OptionsView.ShowHorzLines = false;
            this.treeList_fz.OptionsView.ShowIndicator = false;
            this.treeList_fz.OptionsView.ShowVertLines = false;
            this.treeList_fz.Size = new System.Drawing.Size(339, 749);
            this.treeList_fz.StateImageList = this.imageList;
            this.treeList_fz.TabIndex = 1;
            this.treeList_fz.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_fz_MouseClick);
            // 
            // Column2
            // 
            this.Column2.Caption = "treeListColumn1";
            this.Column2.FieldName = "treeListColumn1";
            this.Column2.MinWidth = 119;
            this.Column2.Name = "Column2";
            this.Column2.Visible = true;
            this.Column2.VisibleIndex = 0;
            this.Column2.Width = 119;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "but_w0.png");
            this.imageList.Images.SetKeyName(1, "but_z0.png");
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.treeList_zl);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(339, 749);
            this.xtraTabPage2.Text = "类型";
            // 
            // treeList_zl
            // 
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
            "定位器"}, 0, 0, 1, 0);
            this.treeList_zl.AppendNode(new object[] {
            "其它"}, 0);
            this.treeList_zl.AppendNode(new object[] {
            null}, -1);
            this.treeList_zl.EndUnboundLoad();
            this.treeList_zl.OptionsBehavior.Editable = false;
            this.treeList_zl.OptionsMenu.EnableFooterMenu = false;
            this.treeList_zl.OptionsSelection.MultiSelect = true;
            this.treeList_zl.OptionsView.ShowColumns = false;
            this.treeList_zl.OptionsView.ShowHorzLines = false;
            this.treeList_zl.OptionsView.ShowIndicator = false;
            this.treeList_zl.OptionsView.ShowVertLines = false;
            this.treeList_zl.Size = new System.Drawing.Size(339, 749);
            this.treeList_zl.StateImageList = this.imageList;
            this.treeList_zl.TabIndex = 2;
            this.treeList_zl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_zl_MouseClick);
            // 
            // Column3
            // 
            this.Column3.Caption = "treeListColumn1";
            this.Column3.FieldName = "Column1";
            this.Column3.MinWidth = 119;
            this.Column3.Name = "Column3";
            this.Column3.Visible = true;
            this.Column3.VisibleIndex = 0;
            this.Column3.Width = 86;
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.treeList_zt);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(339, 749);
            this.xtraTabPage4.Text = "状态";
            // 
            // treeList_zt
            // 
            this.treeList_zt.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList_zt.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.Column5});
            this.treeList_zt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_zt.Location = new System.Drawing.Point(0, 0);
            this.treeList_zt.Name = "treeList_zt";
            this.treeList_zt.BeginUnboundLoad();
            this.treeList_zt.AppendNode(new object[] {
            "所有状态"}, -1, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "正常"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "报警"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "模拟量"}, 2, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "开关量"}, 2, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "断电"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "模拟量"}, 5, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "开关量"}, 5, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "异常"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "故障"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "标校"}, 0, 0, 0, 0);
            this.treeList_zt.AppendNode(new object[] {
            "预警"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "开关量断线"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "开关量异常"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "开关量正常"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "控制量接通"}, 0, 0, 1, 0);
            this.treeList_zt.AppendNode(new object[] {
            "控制量断开"}, 0, 0, 1, 0);
            this.treeList_zt.EndUnboundLoad();
            this.treeList_zt.OptionsBehavior.Editable = false;
            this.treeList_zt.OptionsMenu.EnableFooterMenu = false;
            this.treeList_zt.OptionsSelection.MultiSelect = true;
            this.treeList_zt.OptionsView.ShowColumns = false;
            this.treeList_zt.OptionsView.ShowHorzLines = false;
            this.treeList_zt.OptionsView.ShowIndicator = false;
            this.treeList_zt.OptionsView.ShowVertLines = false;
            this.treeList_zt.Size = new System.Drawing.Size(339, 749);
            this.treeList_zt.StateImageList = this.imageList;
            this.treeList_zt.TabIndex = 3;
            this.treeList_zt.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_zt_MouseClick);
            // 
            // Column5
            // 
            this.Column5.Caption = "treeListColumn1";
            this.Column5.FieldName = "Column5";
            this.Column5.MinWidth = 119;
            this.Column5.Name = "Column5";
            this.Column5.Visible = true;
            this.Column5.VisibleIndex = 0;
            this.Column5.Width = 86;
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.treeList_bp);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(339, 749);
            this.xtraTabPage5.Text = "自定义";
            // 
            // treeList_bp
            // 
            this.treeList_bp.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList_bp.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.Column1});
            this.treeList_bp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList_bp.Location = new System.Drawing.Point(0, 0);
            this.treeList_bp.Name = "treeList_bp";
            this.treeList_bp.BeginUnboundLoad();
            this.treeList_bp.AppendNode(new object[] {
            "自定义编排"}, -1, -1, -1, -1);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面1"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面2"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面3"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面4"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面5"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面6"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面7"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面8"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面9"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面10"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面11"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面12"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面13"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面14"}, 0, 0, 1, 0);
            this.treeList_bp.AppendNode(new object[] {
            "自定义页面15"}, 0, 0, 1, 0);
            this.treeList_bp.EndUnboundLoad();
            this.treeList_bp.OptionsBehavior.Editable = false;
            this.treeList_bp.OptionsMenu.EnableFooterMenu = false;
            this.treeList_bp.OptionsSelection.MultiSelect = true;
            this.treeList_bp.OptionsView.ShowColumns = false;
            this.treeList_bp.OptionsView.ShowHorzLines = false;
            this.treeList_bp.OptionsView.ShowIndicator = false;
            this.treeList_bp.OptionsView.ShowVertLines = false;
            this.treeList_bp.Size = new System.Drawing.Size(339, 749);
            this.treeList_bp.StateImageList = this.imageList;
            this.treeList_bp.TabIndex = 3;
            this.treeList_bp.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_bp_FocusedNodeChanged);
            // 
            // Column1
            // 
            this.Column1.Caption = "treeListColumn1";
            this.Column1.FieldName = "treeListColumn1";
            this.Column1.MinWidth = 119;
            this.Column1.Name = "Column1";
            this.Column1.Visible = true;
            this.Column1.VisibleIndex = 0;
            this.Column1.Width = 119;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "5.jpg");
            // 
            // DisplayNavagation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "DisplayNavagation";
            this.Size = new System.Drawing.Size(345, 778);
            this.Load += new System.EventHandler(this.DisplayNavagation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_fz)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_zl)).EndInit();
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_zt)).EndInit();
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList_bp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tools_refresh;
        public DevExpress.XtraTreeList.TreeList treeList_fz;
        private DevExpress.XtraTreeList.Columns.TreeListColumn Column2;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraTreeList.TreeList treeList_zl;
        private DevExpress.XtraTreeList.Columns.TreeListColumn Column3;
        private DevExpress.XtraTreeList.TreeList treeList_zt;
        private DevExpress.XtraTreeList.Columns.TreeListColumn Column5;
        private DevExpress.XtraTreeList.TreeList treeList_bp;
        private DevExpress.XtraTreeList.Columns.TreeListColumn Column1;

    }
}
