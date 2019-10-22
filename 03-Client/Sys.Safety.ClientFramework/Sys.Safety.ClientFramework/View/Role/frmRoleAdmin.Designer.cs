namespace Sys.Safety.ClientFramework.View.Role
{
    partial class frmRoleAdmin
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
            this.btnRight = new DevExpress.XtraBars.BarButtonItem();
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
            this.RoleGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GirdRoleID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRoleCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRoleName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRoleDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridCreateName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRoleFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RoleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
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
            this.btnAll,
            this.btnRight});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRight, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
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
            this.btnAdd.Caption = "添加角色(&A)";
            this.btnAdd.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.btnAdd.Id = 1;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Caption = "编辑角色(&U)";
            this.btnUpdate.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.edit_16x16;
            this.btnUpdate.Id = 2;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdate_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "删除角色(&D)";
            this.btnDelete.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.delete_16x16;
            this.btnDelete.Id = 3;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnRight
            // 
            this.btnRight.Caption = "权限授权";
            this.btnRight.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.btnRight.Id = 6;
            this.btnRight.Name = "btnRight";
            this.btnRight.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRight_ItemClick);
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
            this.gridCtrlView.MainView = this.RoleGridView;
            this.gridCtrlView.MenuManager = this.barManager1;
            this.gridCtrlView.Name = "gridCtrlView";
            this.gridCtrlView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemCheckEdit1});
            this.gridCtrlView.Size = new System.Drawing.Size(944, 494);
            this.gridCtrlView.TabIndex = 6;
            this.gridCtrlView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.RoleGridView});
            // 
            // RoleGridView
            // 
            this.RoleGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GirdRoleID,
            this.GridRoleCode,
            this.GridRoleName,
            this.GridRoleDescription,
            this.GridCreateName,
            this.GridRoleFlag});
            this.RoleGridView.GridControl = this.gridCtrlView;
            this.RoleGridView.Name = "RoleGridView";
            this.RoleGridView.OptionsView.ShowGroupPanel = false;
            this.RoleGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.RoleGridView_FocusedRowChanged);
            // 
            // GirdRoleID
            // 
            this.GirdRoleID.Caption = "编号";
            this.GirdRoleID.FieldName = "RoleID";
            this.GirdRoleID.Name = "GirdRoleID";
            this.GirdRoleID.OptionsColumn.AllowEdit = false;
            this.GirdRoleID.Visible = true;
            this.GirdRoleID.VisibleIndex = 0;
            this.GirdRoleID.Width = 60;
            // 
            // GridRoleCode
            // 
            this.GridRoleCode.Caption = "角色编码";
            this.GridRoleCode.FieldName = "RoleCode";
            this.GridRoleCode.Name = "GridRoleCode";
            this.GridRoleCode.OptionsColumn.AllowEdit = false;
            this.GridRoleCode.Visible = true;
            this.GridRoleCode.VisibleIndex = 1;
            this.GridRoleCode.Width = 65;
            // 
            // GridRoleName
            // 
            this.GridRoleName.Caption = "角色名称";
            this.GridRoleName.FieldName = "RoleName";
            this.GridRoleName.Name = "GridRoleName";
            this.GridRoleName.OptionsColumn.AllowEdit = false;
            this.GridRoleName.Visible = true;
            this.GridRoleName.VisibleIndex = 2;
            // 
            // GridRoleDescription
            // 
            this.GridRoleDescription.Caption = "角色描述";
            this.GridRoleDescription.FieldName = "RoleDescription";
            this.GridRoleDescription.Name = "GridRoleDescription";
            this.GridRoleDescription.OptionsColumn.AllowEdit = false;
            this.GridRoleDescription.Visible = true;
            this.GridRoleDescription.VisibleIndex = 4;
            this.GridRoleDescription.Width = 65;
            // 
            // GridCreateName
            // 
            this.GridCreateName.Caption = "创建人";
            this.GridCreateName.FieldName = "CreateName";
            this.GridCreateName.Name = "GridCreateName";
            this.GridCreateName.OptionsColumn.AllowEdit = false;
            this.GridCreateName.Visible = true;
            this.GridCreateName.VisibleIndex = 3;
            this.GridCreateName.Width = 120;
            // 
            // GridRoleFlag
            // 
            this.GridRoleFlag.Caption = "启用";
            this.GridRoleFlag.FieldName = "RoleFlag";
            this.GridRoleFlag.Name = "GridRoleFlag";
            this.GridRoleFlag.OptionsColumn.AllowEdit = false;
            this.GridRoleFlag.Visible = true;
            this.GridRoleFlag.VisibleIndex = 5;
            this.GridRoleFlag.Width = 70;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "加载全部(&A)";
            this.barButtonItem1.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // frmRoleAdmin
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
            this.Name = "frmRoleAdmin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "角色管理";
            this.Load += new System.EventHandler(this.frmRoleAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RoleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
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
        private DevExpress.XtraGrid.Views.Grid.GridView RoleGridView;
        private DevExpress.XtraGrid.Columns.GridColumn GirdRoleID;
        private DevExpress.XtraGrid.Columns.GridColumn GridRoleCode;
        private DevExpress.XtraGrid.Columns.GridColumn GridRoleName;
        private DevExpress.XtraGrid.Columns.GridColumn GridRoleDescription;
        private DevExpress.XtraGrid.Columns.GridColumn GridCreateName;
        private DevExpress.XtraGrid.Columns.GridColumn GridRoleFlag;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraBars.BarButtonItem btnRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}