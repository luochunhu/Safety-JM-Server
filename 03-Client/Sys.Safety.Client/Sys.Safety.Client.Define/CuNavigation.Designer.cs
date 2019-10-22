namespace Sys.Safety.Client.Define
{
    partial class CuNavigation
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CxtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.CxtraTabPageStation = new DevExpress.XtraTab.XtraTabPage();
            this.CtreeLisStation = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CcontextMenuDev = new System.Windows.Forms.ContextMenuStrip();
            this.搜索交换机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.检测串口信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.新增设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CxtraTabPageDev = new DevExpress.XtraTab.XtraTabPage();
            this.CtreeListDev = new DevExpress.XtraTreeList.TreeList();
            this.CDevtreeListColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CtreeListColumnTag = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CxtraTabPageDevType = new DevExpress.XtraTab.XtraTabPage();
            this.CtreeListDevType = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CtreeListColumnDevType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CcontextMenuDevType = new System.Windows.Forms.ContextMenuStrip();
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CxtraTabPagewz = new DevExpress.XtraTab.XtraTabPage();
            this.CtreeListWz = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn5 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CImageListDev = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.CxtraTabControl)).BeginInit();
            this.CxtraTabControl.SuspendLayout();
            this.CxtraTabPageStation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CtreeLisStation)).BeginInit();
            this.CcontextMenuDev.SuspendLayout();
            this.CxtraTabPageDev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListDev)).BeginInit();
            this.CxtraTabPageDevType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListDevType)).BeginInit();
            this.CcontextMenuDevType.SuspendLayout();
            this.CxtraTabPagewz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListWz)).BeginInit();
            this.SuspendLayout();
            // 
            // CxtraTabControl
            // 
            this.CxtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CxtraTabControl.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CxtraTabControl.Location = new System.Drawing.Point(0, 0);
            this.CxtraTabControl.Name = "CxtraTabControl";
            this.CxtraTabControl.SelectedTabPage = this.CxtraTabPageStation;
            this.CxtraTabControl.Size = new System.Drawing.Size(394, 478);
            this.CxtraTabControl.TabIndex = 0;
            this.CxtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.CxtraTabPageDev,
            this.CxtraTabPageStation,
            this.CxtraTabPageDevType,
            this.CxtraTabPagewz});
            this.CxtraTabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.CxtraTabControl_SelectedPageChanged);
            // 
            // CxtraTabPageStation
            // 
            this.CxtraTabPageStation.Appearance.Header.Font = new System.Drawing.Font("黑体", 10.5F);
            this.CxtraTabPageStation.Appearance.Header.Options.UseFont = true;
            this.CxtraTabPageStation.Controls.Add(this.CtreeLisStation);
            this.CxtraTabPageStation.Name = "CxtraTabPageStation";
            this.CxtraTabPageStation.Size = new System.Drawing.Size(388, 449);
            this.CxtraTabPageStation.Text = "分站管理";
            // 
            // CtreeLisStation
            // 
            this.CtreeLisStation.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CtreeLisStation.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeLisStation.Appearance.FocusedRow.Options.UseBackColor = true;
            this.CtreeLisStation.Appearance.FocusedRow.Options.UseFont = true;
            this.CtreeLisStation.Appearance.HeaderPanel.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeLisStation.Appearance.HeaderPanel.Options.UseFont = true;
            this.CtreeLisStation.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2,
            this.treeListColumn3});
            this.CtreeLisStation.ContextMenuStrip = this.CcontextMenuDev;
            this.CtreeLisStation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CtreeLisStation.Location = new System.Drawing.Point(0, 0);
            this.CtreeLisStation.Name = "CtreeLisStation";
            this.CtreeLisStation.OptionsBehavior.Editable = false;
            this.CtreeLisStation.OptionsView.ShowColumns = false;
            this.CtreeLisStation.OptionsView.ShowHorzLines = false;
            this.CtreeLisStation.OptionsView.ShowIndicator = false;
            this.CtreeLisStation.OptionsView.ShowVertLines = false;
            this.CtreeLisStation.Size = new System.Drawing.Size(388, 449);
            this.CtreeLisStation.TabIndex = 1;
            this.CtreeLisStation.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.CtreeLisStation_FocusedNodeChanged);
            this.CtreeLisStation.DoubleClick += new System.EventHandler(this.CtreeLisStation_DoubleClick);
            this.CtreeLisStation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CtreeLisStation_MouseDown);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "所有设备";
            this.treeListColumn2.FieldName = "Name";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            this.treeListColumn2.Width = 370;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "nodeTag";
            this.treeListColumn3.FieldName = "Tag";
            this.treeListColumn3.MinWidth = 16;
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.Width = 16;
            // 
            // CcontextMenuDev
            // 
            this.CcontextMenuDev.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.搜索交换机ToolStripMenuItem,
            this.toolStripSeparator1,
            this.检测串口信息ToolStripMenuItem,
            this.toolStripSeparator2,
            this.新增设备ToolStripMenuItem,
            this.修改设备ToolStripMenuItem,
            this.删除设备ToolStripMenuItem});
            this.CcontextMenuDev.Name = "CcontextMenuDev";
            this.CcontextMenuDev.Size = new System.Drawing.Size(149, 126);
            // 
            // 搜索交换机ToolStripMenuItem
            // 
            this.搜索交换机ToolStripMenuItem.Name = "搜索交换机ToolStripMenuItem";
            this.搜索交换机ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.搜索交换机ToolStripMenuItem.Text = "搜索交换机";
            this.搜索交换机ToolStripMenuItem.Click += new System.EventHandler(this.搜索交换机ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // 检测串口信息ToolStripMenuItem
            // 
            this.检测串口信息ToolStripMenuItem.Name = "检测串口信息ToolStripMenuItem";
            this.检测串口信息ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.检测串口信息ToolStripMenuItem.Text = "检测串口信息";
            this.检测串口信息ToolStripMenuItem.Click += new System.EventHandler(this.检测串口信息ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // 新增设备ToolStripMenuItem
            // 
            this.新增设备ToolStripMenuItem.Name = "新增设备ToolStripMenuItem";
            this.新增设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.新增设备ToolStripMenuItem.Text = "新增设备";
            this.新增设备ToolStripMenuItem.Visible = false;
            this.新增设备ToolStripMenuItem.Click += new System.EventHandler(this.新增设备ToolStripMenuItem_Click);
            // 
            // 修改设备ToolStripMenuItem
            // 
            this.修改设备ToolStripMenuItem.Name = "修改设备ToolStripMenuItem";
            this.修改设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.修改设备ToolStripMenuItem.Text = "修改设备";
            this.修改设备ToolStripMenuItem.Visible = false;
            this.修改设备ToolStripMenuItem.Click += new System.EventHandler(this.修改设备ToolStripMenuItem_Click);
            // 
            // 删除设备ToolStripMenuItem
            // 
            this.删除设备ToolStripMenuItem.Name = "删除设备ToolStripMenuItem";
            this.删除设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除设备ToolStripMenuItem.Text = "删除设备";
            this.删除设备ToolStripMenuItem.Visible = false;
            this.删除设备ToolStripMenuItem.Click += new System.EventHandler(this.删除设备ToolStripMenuItem_Click);
            // 
            // CxtraTabPageDev
            // 
            this.CxtraTabPageDev.Appearance.Header.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CxtraTabPageDev.Appearance.Header.Options.UseFont = true;
            this.CxtraTabPageDev.Controls.Add(this.CtreeListDev);
            this.CxtraTabPageDev.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CxtraTabPageDev.Name = "CxtraTabPageDev";
            this.CxtraTabPageDev.Size = new System.Drawing.Size(388, 449);
            this.CxtraTabPageDev.Text = "交换机管理";
            // 
            // CtreeListDev
            // 
            this.CtreeListDev.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CtreeListDev.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListDev.Appearance.FocusedRow.Options.UseBackColor = true;
            this.CtreeListDev.Appearance.FocusedRow.Options.UseFont = true;
            this.CtreeListDev.Appearance.HeaderPanel.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListDev.Appearance.HeaderPanel.Options.UseFont = true;
            this.CtreeListDev.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.CDevtreeListColumn,
            this.CtreeListColumnTag});
            this.CtreeListDev.ContextMenuStrip = this.CcontextMenuDev;
            this.CtreeListDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CtreeListDev.Location = new System.Drawing.Point(0, 0);
            this.CtreeListDev.Name = "CtreeListDev";
            this.CtreeListDev.OptionsBehavior.Editable = false;
            this.CtreeListDev.OptionsView.ShowColumns = false;
            this.CtreeListDev.OptionsView.ShowHorzLines = false;
            this.CtreeListDev.OptionsView.ShowIndicator = false;
            this.CtreeListDev.OptionsView.ShowVertLines = false;
            this.CtreeListDev.Size = new System.Drawing.Size(388, 449);
            this.CtreeListDev.TabIndex = 0;
            this.CtreeListDev.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.CtreeListDev_FocusedNodeChanged);
            this.CtreeListDev.DoubleClick += new System.EventHandler(this.CtreeListDev_DoubleClick);
            this.CtreeListDev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CtreeListDev_MouseDown);
            // 
            // CDevtreeListColumn
            // 
            this.CDevtreeListColumn.Caption = "所有设备";
            this.CDevtreeListColumn.FieldName = "Name";
            this.CDevtreeListColumn.Name = "CDevtreeListColumn";
            this.CDevtreeListColumn.Visible = true;
            this.CDevtreeListColumn.VisibleIndex = 0;
            this.CDevtreeListColumn.Width = 370;
            // 
            // CtreeListColumnTag
            // 
            this.CtreeListColumnTag.Caption = "nodeTag";
            this.CtreeListColumnTag.FieldName = "Tag";
            this.CtreeListColumnTag.MinWidth = 16;
            this.CtreeListColumnTag.Name = "CtreeListColumnTag";
            this.CtreeListColumnTag.Width = 16;
            // 
            // CxtraTabPageDevType
            // 
            this.CxtraTabPageDevType.Appearance.Header.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CxtraTabPageDevType.Appearance.Header.Options.UseFont = true;
            this.CxtraTabPageDevType.Controls.Add(this.CtreeListDevType);
            this.CxtraTabPageDevType.Name = "CxtraTabPageDevType";
            this.CxtraTabPageDevType.Size = new System.Drawing.Size(388, 449);
            this.CxtraTabPageDevType.Text = "设备类型";
            // 
            // CtreeListDevType
            // 
            this.CtreeListDevType.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CtreeListDevType.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListDevType.Appearance.FocusedRow.Options.UseBackColor = true;
            this.CtreeListDevType.Appearance.FocusedRow.Options.UseFont = true;
            this.CtreeListDevType.Appearance.HeaderPanel.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListDevType.Appearance.HeaderPanel.Options.UseFont = true;
            this.CtreeListDevType.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.CtreeListColumnDevType});
            this.CtreeListDevType.ContextMenuStrip = this.CcontextMenuDevType;
            this.CtreeListDevType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CtreeListDevType.Location = new System.Drawing.Point(0, 0);
            this.CtreeListDevType.Name = "CtreeListDevType";
            this.CtreeListDevType.OptionsBehavior.Editable = false;
            this.CtreeListDevType.OptionsView.ShowColumns = false;
            this.CtreeListDevType.OptionsView.ShowHorzLines = false;
            this.CtreeListDevType.OptionsView.ShowIndicator = false;
            this.CtreeListDevType.OptionsView.ShowVertLines = false;
            this.CtreeListDevType.Size = new System.Drawing.Size(388, 449);
            this.CtreeListDevType.TabIndex = 1;
            this.CtreeListDevType.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.CtreeListDevType_FocusedNodeChanged);
            this.CtreeListDevType.DoubleClick += new System.EventHandler(this.CtreeListDevType_DoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "所有设备类型";
            this.treeListColumn1.FieldName = "Name";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 350;
            // 
            // CtreeListColumnDevType
            // 
            this.CtreeListColumnDevType.Caption = "nodeTag";
            this.CtreeListColumnDevType.FieldName = "Tag";
            this.CtreeListColumnDevType.MinWidth = 16;
            this.CtreeListColumnDevType.Name = "CtreeListColumnDevType";
            this.CtreeListColumnDevType.Width = 20;
            // 
            // CcontextMenuDevType
            // 
            this.CcontextMenuDevType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.CcontextMenuDevType.Name = "CcontextMenuDev";
            this.CcontextMenuDevType.Size = new System.Drawing.Size(101, 70);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Visible = false;
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增ToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Visible = false;
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Visible = false;
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // CxtraTabPagewz
            // 
            this.CxtraTabPagewz.Controls.Add(this.CtreeListWz);
            this.CxtraTabPagewz.Name = "CxtraTabPagewz";
            this.CxtraTabPagewz.Size = new System.Drawing.Size(388, 449);
            this.CxtraTabPagewz.Text = "位置管理";
            // 
            // CtreeListWz
            // 
            this.CtreeListWz.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.CtreeListWz.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListWz.Appearance.FocusedRow.Options.UseBackColor = true;
            this.CtreeListWz.Appearance.FocusedRow.Options.UseFont = true;
            this.CtreeListWz.Appearance.HeaderPanel.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtreeListWz.Appearance.HeaderPanel.Options.UseFont = true;
            this.CtreeListWz.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn4,
            this.treeListColumn5});
            this.CtreeListWz.ContextMenuStrip = this.CcontextMenuDevType;
            this.CtreeListWz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CtreeListWz.Location = new System.Drawing.Point(0, 0);
            this.CtreeListWz.Name = "CtreeListWz";
            this.CtreeListWz.OptionsBehavior.Editable = false;
            this.CtreeListWz.OptionsView.ShowColumns = false;
            this.CtreeListWz.OptionsView.ShowHorzLines = false;
            this.CtreeListWz.OptionsView.ShowIndicator = false;
            this.CtreeListWz.OptionsView.ShowVertLines = false;
            this.CtreeListWz.Size = new System.Drawing.Size(388, 449);
            this.CtreeListWz.TabIndex = 2;
            this.CtreeListWz.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.CtreeListWz_FocusedNodeChanged);
            this.CtreeListWz.Click += new System.EventHandler(this.CtreeListWz_Click);
            this.CtreeListWz.DoubleClick += new System.EventHandler(this.CtreeListWz_DoubleClick);
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "所有设备类型";
            this.treeListColumn4.FieldName = "Name";
            this.treeListColumn4.Name = "treeListColumn4";
            this.treeListColumn4.Visible = true;
            this.treeListColumn4.VisibleIndex = 0;
            this.treeListColumn4.Width = 350;
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.Caption = "nodeTag";
            this.treeListColumn5.FieldName = "Tag";
            this.treeListColumn5.MinWidth = 16;
            this.treeListColumn5.Name = "treeListColumn5";
            this.treeListColumn5.Width = 20;
            // 
            // CImageListDev
            // 
            this.CImageListDev.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.CImageListDev.ImageSize = new System.Drawing.Size(16, 16);
            this.CImageListDev.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CuNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CxtraTabControl);
            this.Name = "CuNavigation";
            this.Size = new System.Drawing.Size(394, 478);
            this.Load += new System.EventHandler(this.CuNavigation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CxtraTabControl)).EndInit();
            this.CxtraTabControl.ResumeLayout(false);
            this.CxtraTabPageStation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CtreeLisStation)).EndInit();
            this.CcontextMenuDev.ResumeLayout(false);
            this.CxtraTabPageDev.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListDev)).EndInit();
            this.CxtraTabPageDevType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListDevType)).EndInit();
            this.CcontextMenuDevType.ResumeLayout(false);
            this.CxtraTabPagewz.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CtreeListWz)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl CxtraTabControl;
        private DevExpress.XtraTab.XtraTabPage CxtraTabPageDev;
        private DevExpress.XtraTab.XtraTabPage CxtraTabPageDevType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn CDevtreeListColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private System.Windows.Forms.ImageList CImageListDev;
        private DevExpress.XtraTreeList.Columns.TreeListColumn CtreeListColumnTag;
        private DevExpress.XtraTreeList.Columns.TreeListColumn CtreeListColumnDevType;
        private System.Windows.Forms.ContextMenuStrip CcontextMenuDev;
        private System.Windows.Forms.ContextMenuStrip CcontextMenuDevType;
        private System.Windows.Forms.ToolStripMenuItem 搜索交换机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 检测串口信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 新增设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        public DevExpress.XtraTreeList.TreeList CtreeListDev;
        public DevExpress.XtraTreeList.TreeList CtreeListDevType;
        private DevExpress.XtraTab.XtraTabPage CxtraTabPageStation;
        public DevExpress.XtraTreeList.TreeList CtreeLisStation;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTab.XtraTabPage CxtraTabPagewz;
        public DevExpress.XtraTreeList.TreeList CtreeListWz;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn5;

    }
}
