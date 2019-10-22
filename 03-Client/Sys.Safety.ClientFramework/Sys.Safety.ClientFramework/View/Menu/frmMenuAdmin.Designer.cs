namespace Sys.Safety.ClientFramework.View.Menu
{
    partial class frmMenuAdmin
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnUpdate = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnAll = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.StaticMsg = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.gridCtrlView = new DevExpress.XtraGrid.GridControl();
            this.MenuGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GirdMenuID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuParent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuURL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.GridMenuFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuFile = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuNamespace = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuParams = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridShowType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRightCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuSmallIcon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuLargeIcon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridLoadByIframe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridIsSystemDesktop = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuForSys = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridMenuSort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
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
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.StaticMsg,
            this.btnAdd,
            this.btnUpdate,
            this.btnDelete,
            this.btnClose,
            this.btnAll});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 6;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Tools";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUpdate, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAll, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClose, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.Hidden = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Tools";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "添加菜单(&A)";
            this.btnAdd.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.btnAdd.Id = 1;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Caption = "编辑菜单(&U)";
            this.btnUpdate.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.edit_16x16;
            this.btnUpdate.Id = 2;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdate_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "删除菜单(&D)";
            this.btnDelete.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.delete_16x16;
            this.btnDelete.Id = 3;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnAll
            // 
            this.btnAll.Caption = "加载全部(&A)";
            this.btnAll.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.btnAll.Id = 5;
            this.btnAll.Name = "btnAll";
            this.btnAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAll_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "关闭(&C)";
            this.btnClose.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.close_16x16;
            this.btnClose.Id = 4;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.StaticMsg)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // StaticMsg
            // 
            this.StaticMsg.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.StaticMsg.Caption = "数据准备就绪......";
            this.StaticMsg.Id = 0;
            this.StaticMsg.Name = "StaticMsg";
            this.StaticMsg.TextAlignment = System.Drawing.StringAlignment.Near;
            this.StaticMsg.Width = 32;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(944, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 518);
            this.barDockControlBottom.Size = new System.Drawing.Size(944, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 494);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(944, 24);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 494);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(944, 494);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(944, 494);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // gridCtrlView
            // 
            this.gridCtrlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrlView.Location = new System.Drawing.Point(0, 24);
            this.gridCtrlView.MainView = this.MenuGridView;
            this.gridCtrlView.MenuManager = this.barManager1;
            this.gridCtrlView.Name = "gridCtrlView";
            this.gridCtrlView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemCheckEdit1});
            this.gridCtrlView.Size = new System.Drawing.Size(944, 494);
            this.gridCtrlView.TabIndex = 5;
            this.gridCtrlView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.MenuGridView});
            // 
            // MenuGridView
            // 
            this.MenuGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GirdMenuID,
            this.GridMenuCode,
            this.GridMenuName,
            this.GridMenuParent,
            this.GridMenuURL,
            this.GridMenuMemo,
            this.GridMenuFlag,
            this.GridMenuFile,
            this.GridMenuNamespace,
            this.GridMenuParams,
            this.GridMenuStatus,
            this.GridShowType,
            this.GridRightCode,
            this.GridMenuSmallIcon,
            this.GridMenuLargeIcon,
            this.GridLoadByIframe,
            this.GridIsSystemDesktop,
            this.GridMenuForSys,
            this.GridMenuSort});
            this.MenuGridView.GridControl = this.gridCtrlView;
            this.MenuGridView.Name = "MenuGridView";
            this.MenuGridView.OptionsView.ShowGroupPanel = false;
            this.MenuGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.MenuGridView_FocusedRowChanged);
            // 
            // GirdMenuID
            // 
            this.GirdMenuID.Caption = "编号";
            this.GirdMenuID.FieldName = "MenuID";
            this.GirdMenuID.Name = "GirdMenuID";
            this.GirdMenuID.OptionsColumn.AllowEdit = false;
            this.GirdMenuID.Visible = true;
            this.GirdMenuID.VisibleIndex = 0;
            this.GirdMenuID.Width = 60;
            // 
            // GridMenuCode
            // 
            this.GridMenuCode.Caption = "编码";
            this.GridMenuCode.FieldName = "MenuCode";
            this.GridMenuCode.Name = "GridMenuCode";
            this.GridMenuCode.OptionsColumn.AllowEdit = false;
            this.GridMenuCode.Visible = true;
            this.GridMenuCode.VisibleIndex = 1;
            this.GridMenuCode.Width = 65;
            // 
            // GridMenuName
            // 
            this.GridMenuName.Caption = "菜单名称";
            this.GridMenuName.FieldName = "MenuName";
            this.GridMenuName.Name = "GridMenuName";
            this.GridMenuName.OptionsColumn.AllowEdit = false;
            this.GridMenuName.Visible = true;
            this.GridMenuName.VisibleIndex = 2;
            // 
            // GridMenuParent
            // 
            this.GridMenuParent.Caption = "上级菜单";
            this.GridMenuParent.FieldName = "MenuParent";
            this.GridMenuParent.Name = "GridMenuParent";
            this.GridMenuParent.OptionsColumn.AllowEdit = false;
            this.GridMenuParent.Visible = true;
            this.GridMenuParent.VisibleIndex = 4;
            this.GridMenuParent.Width = 65;
            // 
            // GridMenuURL
            // 
            this.GridMenuURL.Caption = "菜单窗体名";
            this.GridMenuURL.FieldName = "MenuURL";
            this.GridMenuURL.Name = "GridMenuURL";
            this.GridMenuURL.OptionsColumn.AllowEdit = false;
            this.GridMenuURL.Visible = true;
            this.GridMenuURL.VisibleIndex = 3;
            this.GridMenuURL.Width = 120;
            // 
            // GridMenuMemo
            // 
            this.GridMenuMemo.Caption = "顶级菜单标记";
            this.GridMenuMemo.ColumnEdit = this.repositoryItemCheckEdit1;
            this.GridMenuMemo.FieldName = "MenuMemo";
            this.GridMenuMemo.Name = "GridMenuMemo";
            this.GridMenuMemo.OptionsColumn.AllowEdit = false;
            this.GridMenuMemo.Width = 70;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // GridMenuFlag
            // 
            this.GridMenuFlag.Caption = "启用";
            this.GridMenuFlag.FieldName = "MenuFlag";
            this.GridMenuFlag.Name = "GridMenuFlag";
            this.GridMenuFlag.OptionsColumn.AllowEdit = false;
            this.GridMenuFlag.Visible = true;
            this.GridMenuFlag.VisibleIndex = 5;
            this.GridMenuFlag.Width = 80;
            // 
            // GridMenuFile
            // 
            this.GridMenuFile.Caption = "程序文件";
            this.GridMenuFile.FieldName = "MenuFile";
            this.GridMenuFile.Name = "GridMenuFile";
            this.GridMenuFile.OptionsColumn.AllowEdit = false;
            this.GridMenuFile.Visible = true;
            this.GridMenuFile.VisibleIndex = 6;
            // 
            // GridMenuNamespace
            // 
            this.GridMenuNamespace.Caption = "命名空间";
            this.GridMenuNamespace.FieldName = "MenuNamespace";
            this.GridMenuNamespace.Name = "GridMenuNamespace";
            this.GridMenuNamespace.OptionsColumn.AllowEdit = false;
            this.GridMenuNamespace.Visible = true;
            this.GridMenuNamespace.VisibleIndex = 7;
            // 
            // GridMenuParams
            // 
            this.GridMenuParams.Caption = "参数";
            this.GridMenuParams.FieldName = "MenuParams";
            this.GridMenuParams.Name = "GridMenuParams";
            this.GridMenuParams.OptionsColumn.AllowEdit = false;
            this.GridMenuParams.Visible = true;
            this.GridMenuParams.VisibleIndex = 8;
            // 
            // GridMenuStatus
            // 
            this.GridMenuStatus.Caption = "快捷菜单";
            this.GridMenuStatus.FieldName = "MenuStatus";
            this.GridMenuStatus.Name = "GridMenuStatus";
            this.GridMenuStatus.OptionsColumn.AllowEdit = false;
            this.GridMenuStatus.Visible = true;
            this.GridMenuStatus.VisibleIndex = 10;
            // 
            // GridShowType
            // 
            this.GridShowType.Caption = "模态";
            this.GridShowType.FieldName = "ShowType";
            this.GridShowType.Name = "GridShowType";
            this.GridShowType.OptionsColumn.AllowEdit = false;
            this.GridShowType.Visible = true;
            this.GridShowType.VisibleIndex = 11;
            // 
            // GridRightCode
            // 
            this.GridRightCode.Caption = "权限编码";
            this.GridRightCode.FieldName = "RightCode";
            this.GridRightCode.Name = "GridRightCode";
            this.GridRightCode.OptionsColumn.AllowEdit = false;
            this.GridRightCode.Visible = true;
            this.GridRightCode.VisibleIndex = 12;
            // 
            // GridMenuSmallIcon
            // 
            this.GridMenuSmallIcon.Caption = "小图标";
            this.GridMenuSmallIcon.FieldName = "MenuSmallIcon";
            this.GridMenuSmallIcon.Name = "GridMenuSmallIcon";
            this.GridMenuSmallIcon.OptionsColumn.AllowEdit = false;
            this.GridMenuSmallIcon.Visible = true;
            this.GridMenuSmallIcon.VisibleIndex = 13;
            // 
            // GridMenuLargeIcon
            // 
            this.GridMenuLargeIcon.Caption = "大图标";
            this.GridMenuLargeIcon.FieldName = "MenuLargeIcon";
            this.GridMenuLargeIcon.Name = "GridMenuLargeIcon";
            this.GridMenuLargeIcon.OptionsColumn.AllowEdit = false;
            this.GridMenuLargeIcon.Visible = true;
            this.GridMenuLargeIcon.VisibleIndex = 14;
            // 
            // GridLoadByIframe
            // 
            this.GridLoadByIframe.Caption = "显示次数";
            this.GridLoadByIframe.FieldName = "LoadByIframe";
            this.GridLoadByIframe.Name = "GridLoadByIframe";
            this.GridLoadByIframe.OptionsColumn.AllowEdit = false;
            this.GridLoadByIframe.Visible = true;
            this.GridLoadByIframe.VisibleIndex = 15;
            // 
            // GridIsSystemDesktop
            // 
            this.GridIsSystemDesktop.Caption = "桌面/BS";
            this.GridIsSystemDesktop.FieldName = "IsSystemDesktop";
            this.GridIsSystemDesktop.Name = "GridIsSystemDesktop";
            this.GridIsSystemDesktop.OptionsColumn.AllowEdit = false;
            // 
            // GridMenuForSys
            // 
            this.GridMenuForSys.Caption = "系统编号";
            this.GridMenuForSys.FieldName = "MenuForSys";
            this.GridMenuForSys.Name = "GridMenuForSys";
            this.GridMenuForSys.OptionsColumn.AllowEdit = false;
            // 
            // GridMenuSort
            // 
            this.GridMenuSort.Caption = "排序";
            this.GridMenuSort.FieldName = "MenuSort";
            this.GridMenuSort.Name = "GridMenuSort";
            this.GridMenuSort.OptionsColumn.AllowEdit = false;
            this.GridMenuSort.Visible = true;
            this.GridMenuSort.VisibleIndex = 9;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // frmMenuAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 545);
            this.Controls.Add(this.gridCtrlView);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmMenuAdmin";
            this.Text = "菜单管理";
            this.Load += new System.EventHandler(this.frmMenuAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MenuGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnUpdate;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.BarButtonItem btnAll;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarStaticItem StaticMsg;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gridCtrlView;
        private DevExpress.XtraGrid.Views.Grid.GridView MenuGridView;
        private DevExpress.XtraGrid.Columns.GridColumn GirdMenuID;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuCode;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuName;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuURL;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuParent;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuMemo;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuFlag;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuSort;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuFile;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuNamespace;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuParams;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuStatus;
        private DevExpress.XtraGrid.Columns.GridColumn GridShowType;
        private DevExpress.XtraGrid.Columns.GridColumn GridRightCode;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuSmallIcon;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuLargeIcon;
        private DevExpress.XtraGrid.Columns.GridColumn GridLoadByIframe;
        private DevExpress.XtraGrid.Columns.GridColumn GridIsSystemDesktop;
        private DevExpress.XtraGrid.Columns.GridColumn GridMenuForSys;
    }
}