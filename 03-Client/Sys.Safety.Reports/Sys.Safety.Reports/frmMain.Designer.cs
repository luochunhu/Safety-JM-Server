namespace Sys.Safety.Reports
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.tlbCreatDir = new DevExpress.XtraBars.BarButtonItem();
            this.tlbLastDir = new DevExpress.XtraBars.BarButtonItem();
            this.tlbRefreshDir = new DevExpress.XtraBars.BarButtonItem();
            this.tlbDeleteDir = new DevExpress.XtraBars.BarButtonItem();
            this.tlbRenameDir = new DevExpress.XtraBars.BarButtonItem();
            this.tlbListAdd = new DevExpress.XtraBars.BarButtonItem();
            this.tlbListEdit = new DevExpress.XtraBars.BarButtonItem();
            this.tlbListDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barSubImportExport = new DevExpress.XtraBars.BarSubItem();
            this.tlbExportMetaData = new DevExpress.XtraBars.BarButtonItem();
            this.tlbImportMetaData = new DevExpress.XtraBars.BarButtonItem();
            this.tlbExportReport = new DevExpress.XtraBars.BarButtonItem();
            this.tlbImportReport = new DevExpress.XtraBars.BarButtonItem();
            this.tlbClose = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStatic = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeListManager = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumnName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnListID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPic = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.gridColumnReportName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupName = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.tlbCreatDir,
            this.tlbLastDir,
            this.tlbRefreshDir,
            this.tlbDeleteDir,
            this.tlbRenameDir,
            this.tlbListAdd,
            this.tlbListEdit,
            this.tlbListDelete,
            this.tlbClose,
            this.barStatic,
            this.tlbExportReport,
            this.barSubImportExport,
            this.tlbExportMetaData,
            this.tlbImportMetaData,
            this.tlbImportReport});
            this.barManager1.MaxItemId = 15;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbCreatDir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbLastDir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbRefreshDir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbDeleteDir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbRenameDir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbListAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbListEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbListDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubImportExport, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbClose, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // tlbCreatDir
            // 
            this.tlbCreatDir.Caption = "新增目录";
            this.tlbCreatDir.Glyph = global::Sys.Safety.Reports.Properties.Resources.add_16x16;
            this.tlbCreatDir.Id = 0;
            this.tlbCreatDir.Name = "tlbCreatDir";
            this.tlbCreatDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbCreatDir_ItemClick);
            // 
            // tlbLastDir
            // 
            this.tlbLastDir.Caption = "上级目录";
            this.tlbLastDir.Glyph = global::Sys.Safety.Reports.Properties.Resources.previous_16x16;
            this.tlbLastDir.Id = 1;
            this.tlbLastDir.Name = "tlbLastDir";
            this.tlbLastDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbLastDir_ItemClick);
            // 
            // tlbRefreshDir
            // 
            this.tlbRefreshDir.Caption = "刷新目录";
            this.tlbRefreshDir.Glyph = global::Sys.Safety.Reports.Properties.Resources.recurrence_16x16;
            this.tlbRefreshDir.Id = 2;
            this.tlbRefreshDir.Name = "tlbRefreshDir";
            this.tlbRefreshDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbRefreshDir_ItemClick);
            // 
            // tlbDeleteDir
            // 
            this.tlbDeleteDir.Caption = "删除目录";
            this.tlbDeleteDir.Glyph = global::Sys.Safety.Reports.Properties.Resources.DeleteDir;
            this.tlbDeleteDir.Id = 3;
            this.tlbDeleteDir.Name = "tlbDeleteDir";
            this.tlbDeleteDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbDeleteDir_ItemClick);
            // 
            // tlbRenameDir
            // 
            this.tlbRenameDir.Caption = "重命名目录";
            this.tlbRenameDir.Glyph = global::Sys.Safety.Reports.Properties.Resources.edit_16x16;
            this.tlbRenameDir.Id = 4;
            this.tlbRenameDir.Name = "tlbRenameDir";
            this.tlbRenameDir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbRenameDir_ItemClick);
            // 
            // tlbListAdd
            // 
            this.tlbListAdd.Caption = "新增列表";
            this.tlbListAdd.Glyph = global::Sys.Safety.Reports.Properties.Resources.addgroupheader_16x16;
            this.tlbListAdd.Id = 5;
            this.tlbListAdd.Name = "tlbListAdd";
            this.tlbListAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbListAdd_ItemClick);
            // 
            // tlbListEdit
            // 
            this.tlbListEdit.Caption = "编辑列表";
            this.tlbListEdit.Glyph = global::Sys.Safety.Reports.Properties.Resources.editcontact_16x16;
            this.tlbListEdit.Id = 6;
            this.tlbListEdit.Name = "tlbListEdit";
            this.tlbListEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbListEdit_ItemClick);
            // 
            // tlbListDelete
            // 
            this.tlbListDelete.Caption = "删除列表";
            this.tlbListDelete.Glyph = global::Sys.Safety.Reports.Properties.Resources.deletelist_16x16;
            this.tlbListDelete.Id = 7;
            this.tlbListDelete.Name = "tlbListDelete";
            this.tlbListDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbListDelete_ItemClick);
            // 
            // barSubImportExport
            // 
            this.barSubImportExport.Caption = "升级相关";
            this.barSubImportExport.Glyph = global::Sys.Safety.Reports.Properties.Resources.wizard_16x16;
            this.barSubImportExport.Id = 11;
            this.barSubImportExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbExportMetaData),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbImportMetaData),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbExportReport),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbImportReport)});
            this.barSubImportExport.Name = "barSubImportExport";
            this.barSubImportExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barSubImportExport_ItemClick);
            // 
            // tlbExportMetaData
            // 
            this.tlbExportMetaData.Caption = "导出元数据";
            this.tlbExportMetaData.Glyph = global::Sys.Safety.Reports.Properties.Resources.metadataexport;
            this.tlbExportMetaData.Id = 12;
            this.tlbExportMetaData.Name = "tlbExportMetaData";
            this.tlbExportMetaData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbExportMetaData_ItemClick);
            // 
            // tlbImportMetaData
            // 
            this.tlbImportMetaData.Caption = "导入元数据";
            this.tlbImportMetaData.Glyph = global::Sys.Safety.Reports.Properties.Resources.metadataimport;
            this.tlbImportMetaData.Id = 13;
            this.tlbImportMetaData.Name = "tlbImportMetaData";
            this.tlbImportMetaData.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbImportMetaData_ItemClick);
            // 
            // tlbExportReport
            // 
            this.tlbExportReport.Caption = "导出列表";
            this.tlbExportReport.Glyph = global::Sys.Safety.Reports.Properties.Resources.metadataexport;
            this.tlbExportReport.Id = 10;
            this.tlbExportReport.Name = "tlbExportReport";
            this.tlbExportReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbExportReport_ItemClick);
            // 
            // tlbImportReport
            // 
            this.tlbImportReport.Caption = "导入列表";
            this.tlbImportReport.Glyph = global::Sys.Safety.Reports.Properties.Resources.metadataimport;
            this.tlbImportReport.Id = 14;
            this.tlbImportReport.Name = "tlbImportReport";
            this.tlbImportReport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbImportReport_ItemClick);
            // 
            // tlbClose
            // 
            this.tlbClose.Caption = "关闭";
            this.tlbClose.Glyph = global::Sys.Safety.Reports.Properties.Resources.close;
            this.tlbClose.Id = 8;
            this.tlbClose.Name = "tlbClose";
            this.tlbClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbClose_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStatic)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barStatic
            // 
            this.barStatic.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.barStatic.Caption = "准备就绪...";
            this.barStatic.Id = 9;
            this.barStatic.Name = "barStatic";
            this.barStatic.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStatic.Width = 32;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(854, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 438);
            this.barDockControlBottom.Size = new System.Drawing.Size(854, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 407);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(854, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 407);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("2eed66e4-668c-4dad-bfa6-4246b40f72cf");
            this.dockPanel1.Location = new System.Drawing.Point(0, 31);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.Size = new System.Drawing.Size(200, 407);
            this.dockPanel1.Text = "列(报)表目录";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeListManager);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(192, 380);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeListManager
            // 
            this.treeListManager.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumnName});
            this.treeListManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListManager.KeyFieldName = "ListID";
            this.treeListManager.Location = new System.Drawing.Point(0, 0);
            this.treeListManager.Name = "treeListManager";
            this.treeListManager.OptionsView.ShowHorzLines = false;
            this.treeListManager.OptionsView.ShowIndicator = false;
            this.treeListManager.OptionsView.ShowVertLines = false;
            this.treeListManager.ParentFieldName = "DirID";
            this.treeListManager.PreviewFieldName = "ListID";
            this.treeListManager.SelectImageList = this.imageList1;
            this.treeListManager.Size = new System.Drawing.Size(192, 380);
            this.treeListManager.TabIndex = 0;
            this.treeListManager.GetSelectImage += new DevExpress.XtraTreeList.GetSelectImageEventHandler(this.treeListManager_GetSelectImage);
            this.treeListManager.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListManager_FocusedNodeChanged);
            this.treeListManager.Click += new System.EventHandler(this.treeListManager_Click);
            this.treeListManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeListManager_KeyDown);
            // 
            // treeListColumnName
            // 
            this.treeListColumnName.Caption = "报表名称";
            this.treeListColumnName.FieldName = "strListName";
            this.treeListColumnName.MinWidth = 33;
            this.treeListColumnName.Name = "treeListColumnName";
            this.treeListColumnName.OptionsColumn.AllowEdit = false;
            this.treeListColumnName.OptionsColumn.AllowFocus = false;
            this.treeListColumnName.Visible = true;
            this.treeListColumnName.VisibleIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Shell32 115.ico");
            this.imageList1.Images.SetKeyName(1, "Shell32 004.ico");
            this.imageList1.Images.SetKeyName(2, "Shell32 005.ico");
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomizationMenu = false;
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(200, 31);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(728, 267, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(654, 407);
            this.layoutControl1.TabIndex = 5;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(24, 44);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBox});
            this.gridControl1.Size = new System.Drawing.Size(606, 339);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnListID,
            this.gridColumnPic,
            this.gridColumnReportName,
            this.gridColumnDescription});
            this.gridView1.CustomizationFormBounds = new System.Drawing.Rectangle(774, 439, 210, 179);
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumnListID
            // 
            this.gridColumnListID.Caption = "列表ID";
            this.gridColumnListID.FieldName = "ListID";
            this.gridColumnListID.Name = "gridColumnListID";
            this.gridColumnListID.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            // 
            // gridColumnPic
            // 
            this.gridColumnPic.Caption = " ";
            this.gridColumnPic.ColumnEdit = this.repositoryItemImageComboBox;
            this.gridColumnPic.FieldName = "blnList";
            this.gridColumnPic.Name = "gridColumnPic";
            this.gridColumnPic.OptionsColumn.AllowEdit = false;
            this.gridColumnPic.OptionsColumn.AllowFocus = false;
            this.gridColumnPic.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnPic.OptionsColumn.AllowMove = false;
            this.gridColumnPic.OptionsColumn.AllowSize = false;
            this.gridColumnPic.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumnPic.OptionsColumn.FixedWidth = true;
            this.gridColumnPic.OptionsColumn.ShowInCustomizationForm = false;
            this.gridColumnPic.OptionsFilter.AllowFilter = false;
            this.gridColumnPic.Visible = true;
            this.gridColumnPic.VisibleIndex = 0;
            this.gridColumnPic.Width = 27;
            // 
            // repositoryItemImageComboBox
            // 
            this.repositoryItemImageComboBox.AutoHeight = false;
            this.repositoryItemImageComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", true, 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", false, 1)});
            this.repositoryItemImageComboBox.Name = "repositoryItemImageComboBox";
            this.repositoryItemImageComboBox.SmallImages = this.imageList1;
            // 
            // gridColumnReportName
            // 
            this.gridColumnReportName.Caption = "列表名称";
            this.gridColumnReportName.FieldName = "strListName";
            this.gridColumnReportName.Name = "gridColumnReportName";
            this.gridColumnReportName.OptionsColumn.AllowEdit = false;
            this.gridColumnReportName.OptionsColumn.AllowFocus = false;
            this.gridColumnReportName.Visible = true;
            this.gridColumnReportName.VisibleIndex = 1;
            this.gridColumnReportName.Width = 263;
            // 
            // gridColumnDescription
            // 
            this.gridColumnDescription.Caption = "列表描述";
            this.gridColumnDescription.FieldName = "StrListDescription";
            this.gridColumnDescription.Name = "gridColumnDescription";
            this.gridColumnDescription.OptionsColumn.AllowEdit = false;
            this.gridColumnDescription.OptionsColumn.AllowFocus = false;
            this.gridColumnDescription.Visible = true;
            this.gridColumnDescription.VisibleIndex = 2;
            this.gridColumnDescription.Width = 263;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroupName});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(654, 407);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            // 
            // layoutControlGroupName
            // 
            this.layoutControlGroupName.CustomizationFormText = "列表";
            this.layoutControlGroupName.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroupName.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupName.Name = "layoutControlGroupName";
            this.layoutControlGroupName.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroupName.Size = new System.Drawing.Size(634, 387);
            this.layoutControlGroupName.Text = "列表";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gridControl1;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(610, 343);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 465);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "列(报)表中心";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem tlbCreatDir;
        private DevExpress.XtraBars.BarButtonItem tlbLastDir;
        private DevExpress.XtraBars.BarButtonItem tlbRefreshDir;
        private DevExpress.XtraBars.BarButtonItem tlbDeleteDir;
        private DevExpress.XtraBars.BarButtonItem tlbRenameDir;
        private DevExpress.XtraBars.BarButtonItem tlbListAdd;
        private DevExpress.XtraBars.BarButtonItem tlbListEdit;
        private DevExpress.XtraBars.BarButtonItem tlbListDelete;
        private DevExpress.XtraBars.BarButtonItem tlbClose;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.TreeList treeListManager;
        private DevExpress.XtraBars.BarStaticItem barStatic;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnListID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPic;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnReportName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDescription;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnName;
        private DevExpress.XtraBars.BarButtonItem tlbExportReport;
        private DevExpress.XtraBars.BarSubItem barSubImportExport;
        private DevExpress.XtraBars.BarButtonItem tlbExportMetaData;
        private DevExpress.XtraBars.BarButtonItem tlbImportMetaData;
        private DevExpress.XtraBars.BarButtonItem tlbImportReport;
        
    }
}