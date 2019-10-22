namespace Sys.Safety.ClientFramework.View.User
{
    partial class frmUserAdmin
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
            this.UserGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GirdUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridUserCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridPassword = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRole = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridDeptCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridLoginCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridCreateName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridCreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridLastLoginTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridLoginIP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridUserFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridContactPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridUserType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRemark1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRemark2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRemark3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRemark4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridRemark5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.simplePager1 = new Sys.Safety.ClientFramework.View.UserControl.Pager.SimplePager();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).BeginInit();
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
            this.btnAdd.Caption = "添加用户(&A)";
            this.btnAdd.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.add_16x16;
            this.btnAdd.Id = 1;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Caption = "编辑用户(&U)";
            this.btnUpdate.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.edit_16x16;
            this.btnUpdate.Id = 2;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdate_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "删除用户(&D)";
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
            this.gridCtrlView.MainView = this.UserGridView;
            this.gridCtrlView.MenuManager = this.barManager1;
            this.gridCtrlView.Name = "gridCtrlView";
            this.gridCtrlView.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemCheckEdit1});
            this.gridCtrlView.Size = new System.Drawing.Size(944, 494);
            this.gridCtrlView.TabIndex = 6;
            this.gridCtrlView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.UserGridView});
            // 
            // UserGridView
            // 
            this.UserGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GirdUserID,
            this.GridUserCode,
            this.GridUserName,
            this.GridPassword,
            this.GridRole,
            this.GridDeptCode,
            this.GridLoginCount,
            this.GridCreateName,
            this.GridCreateTime,
            this.GridLastLoginTime,
            this.GridLoginIP,
            this.GridUserFlag,
            this.GridContactPhone,
            this.GridUserType,
            this.GridRemark1,
            this.GridRemark2,
            this.GridRemark3,
            this.GridRemark4,
            this.GridRemark5});
            this.UserGridView.GridControl = this.gridCtrlView;
            this.UserGridView.Name = "UserGridView";
            this.UserGridView.OptionsView.ShowGroupPanel = false;
            this.UserGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.UserGridView_FocusedRowChanged);
            this.UserGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.UserGridView_CustomColumnDisplayText);
            // 
            // GirdUserID
            // 
            this.GirdUserID.Caption = "编号";
            this.GirdUserID.FieldName = "UserID";
            this.GirdUserID.Name = "GirdUserID";
            this.GirdUserID.OptionsColumn.AllowEdit = false;
            this.GirdUserID.Visible = true;
            this.GirdUserID.VisibleIndex = 0;
            this.GirdUserID.Width = 60;
            // 
            // GridUserCode
            // 
            this.GridUserCode.Caption = "登录用户名";
            this.GridUserCode.FieldName = "UserCode";
            this.GridUserCode.Name = "GridUserCode";
            this.GridUserCode.OptionsColumn.AllowEdit = false;
            this.GridUserCode.Visible = true;
            this.GridUserCode.VisibleIndex = 1;
            this.GridUserCode.Width = 116;
            // 
            // GridUserName
            // 
            this.GridUserName.Caption = "姓名";
            this.GridUserName.FieldName = "UserName";
            this.GridUserName.Name = "GridUserName";
            this.GridUserName.OptionsColumn.AllowEdit = false;
            this.GridUserName.Visible = true;
            this.GridUserName.VisibleIndex = 2;
            this.GridUserName.Width = 94;
            // 
            // GridPassword
            // 
            this.GridPassword.Caption = "密码";
            this.GridPassword.FieldName = "Password";
            this.GridPassword.Name = "GridPassword";
            this.GridPassword.OptionsColumn.AllowEdit = false;
            this.GridPassword.Width = 65;
            // 
            // GridRole
            // 
            this.GridRole.Caption = "角色名称";
            this.GridRole.FieldName = "RoleName";
            this.GridRole.Name = "GridRole";
            // 
            // GridDeptCode
            // 
            this.GridDeptCode.Caption = "部门编码";
            this.GridDeptCode.FieldName = "DeptCode";
            this.GridDeptCode.Name = "GridDeptCode";
            this.GridDeptCode.OptionsColumn.AllowEdit = false;
            this.GridDeptCode.Width = 120;
            // 
            // GridLoginCount
            // 
            this.GridLoginCount.Caption = "登录次数";
            this.GridLoginCount.FieldName = "LoginCount";
            this.GridLoginCount.Name = "GridLoginCount";
            this.GridLoginCount.OptionsColumn.AllowEdit = false;
            this.GridLoginCount.Width = 70;
            // 
            // GridCreateName
            // 
            this.GridCreateName.Caption = "创建人";
            this.GridCreateName.FieldName = "CreateName";
            this.GridCreateName.Name = "GridCreateName";
            this.GridCreateName.OptionsColumn.AllowEdit = false;
            this.GridCreateName.Visible = true;
            this.GridCreateName.VisibleIndex = 3;
            this.GridCreateName.Width = 94;
            // 
            // GridCreateTime
            // 
            this.GridCreateTime.Caption = "创建时间";
            this.GridCreateTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.GridCreateTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.GridCreateTime.FieldName = "CreateTime";
            this.GridCreateTime.Name = "GridCreateTime";
            this.GridCreateTime.OptionsColumn.AllowEdit = false;
            this.GridCreateTime.Visible = true;
            this.GridCreateTime.VisibleIndex = 4;
            this.GridCreateTime.Width = 145;
            // 
            // GridLastLoginTime
            // 
            this.GridLastLoginTime.Caption = "最后登录时间";
            this.GridLastLoginTime.FieldName = "LastLoginTime";
            this.GridLastLoginTime.Name = "GridLastLoginTime";
            this.GridLastLoginTime.OptionsColumn.AllowEdit = false;
            this.GridLastLoginTime.Width = 145;
            // 
            // GridLoginIP
            // 
            this.GridLoginIP.Caption = "登录IP";
            this.GridLoginIP.FieldName = "LoginIP";
            this.GridLoginIP.Name = "GridLoginIP";
            this.GridLoginIP.OptionsColumn.AllowEdit = false;
            this.GridLoginIP.Visible = true;
            this.GridLoginIP.VisibleIndex = 5;
            // 
            // GridUserFlag
            // 
            this.GridUserFlag.Caption = "启用";
            this.GridUserFlag.FieldName = "UserFlag";
            this.GridUserFlag.Name = "GridUserFlag";
            this.GridUserFlag.OptionsColumn.AllowEdit = false;
            this.GridUserFlag.Visible = true;
            this.GridUserFlag.VisibleIndex = 6;
            this.GridUserFlag.Width = 58;
            // 
            // GridContactPhone
            // 
            this.GridContactPhone.Caption = "联系电话";
            this.GridContactPhone.FieldName = "ContactPhone";
            this.GridContactPhone.Name = "GridContactPhone";
            this.GridContactPhone.OptionsColumn.AllowEdit = false;
            this.GridContactPhone.Visible = true;
            this.GridContactPhone.VisibleIndex = 7;
            this.GridContactPhone.Width = 100;
            // 
            // GridUserType
            // 
            this.GridUserType.Caption = "用户类型";
            this.GridUserType.FieldName = "UserType";
            this.GridUserType.Name = "GridUserType";
            this.GridUserType.OptionsColumn.AllowEdit = false;
            this.GridUserType.Width = 65;
            // 
            // GridRemark1
            // 
            this.GridRemark1.Caption = "备注1";
            this.GridRemark1.FieldName = "Remark1";
            this.GridRemark1.Name = "GridRemark1";
            this.GridRemark1.OptionsColumn.AllowEdit = false;
            this.GridRemark1.Width = 65;
            // 
            // GridRemark2
            // 
            this.GridRemark2.Caption = "备注2";
            this.GridRemark2.FieldName = "Remark2";
            this.GridRemark2.Name = "GridRemark2";
            this.GridRemark2.OptionsColumn.AllowEdit = false;
            this.GridRemark2.Width = 65;
            // 
            // GridRemark3
            // 
            this.GridRemark3.Caption = "备注3";
            this.GridRemark3.FieldName = "Remark3";
            this.GridRemark3.Name = "GridRemark3";
            this.GridRemark3.OptionsColumn.AllowEdit = false;
            this.GridRemark3.Width = 96;
            // 
            // GridRemark4
            // 
            this.GridRemark4.Caption = "备注4";
            this.GridRemark4.FieldName = "Remark4";
            this.GridRemark4.Name = "GridRemark4";
            this.GridRemark4.OptionsColumn.AllowEdit = false;
            // 
            // GridRemark5
            // 
            this.GridRemark5.Caption = "备注5";
            this.GridRemark5.FieldName = "Remark5";
            this.GridRemark5.Name = "GridRemark5";
            this.GridRemark5.OptionsColumn.AllowEdit = false;
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
            // simplePager1
            // 
            this.simplePager1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.simplePager1.Location = new System.Drawing.Point(1, 484);
            this.simplePager1.Name = "simplePager1";
            this.simplePager1.Size = new System.Drawing.Size(943, 33);
            this.simplePager1.TabIndex = 11;
            this.simplePager1.myPagerEvents += new Sys.Safety.ClientFramework.View.UserControl.Pager.SimplePager.MyPagerEvents(this.simplePager1_myPagerEvents);
            // 
            // frmUserAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 545);
            this.Controls.Add(this.simplePager1);
            this.Controls.Add(this.gridCtrlView);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmUserAdmin";
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.frmUserAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).EndInit();
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
        private DevExpress.XtraGrid.Views.Grid.GridView UserGridView;
        private DevExpress.XtraGrid.Columns.GridColumn GirdUserID;
        private DevExpress.XtraGrid.Columns.GridColumn GridUserCode;
        private DevExpress.XtraGrid.Columns.GridColumn GridUserName;
        private DevExpress.XtraGrid.Columns.GridColumn GridPassword;
        private DevExpress.XtraGrid.Columns.GridColumn GridDeptCode;
        private DevExpress.XtraGrid.Columns.GridColumn GridLoginCount;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn GridCreateName;
        private DevExpress.XtraGrid.Columns.GridColumn GridCreateTime;
        private DevExpress.XtraGrid.Columns.GridColumn GridLastLoginTime;
        private DevExpress.XtraGrid.Columns.GridColumn GridLoginIP;
        private DevExpress.XtraGrid.Columns.GridColumn GridUserFlag;
        private DevExpress.XtraGrid.Columns.GridColumn GridContactPhone;
        private DevExpress.XtraGrid.Columns.GridColumn GridUserType;
        private DevExpress.XtraGrid.Columns.GridColumn GridRemark1;
        private DevExpress.XtraGrid.Columns.GridColumn GridRemark2;
        private DevExpress.XtraGrid.Columns.GridColumn GridRemark3;
        private DevExpress.XtraGrid.Columns.GridColumn GridRemark4;
        private DevExpress.XtraGrid.Columns.GridColumn GridRemark5;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn GridRole;
        private UserControl.Pager.SimplePager simplePager1;
    }
}