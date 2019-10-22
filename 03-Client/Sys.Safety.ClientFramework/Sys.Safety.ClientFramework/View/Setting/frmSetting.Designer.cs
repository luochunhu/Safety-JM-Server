namespace Sys.Safety.ClientFramework.View.Setting
{
    partial class frmSetting
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.tlbSave = new DevExpress.XtraBars.BarButtonItem();
            this.tlbClose = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnstrType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnstrKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnstrKeyCHs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnstrValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnstrDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnCreator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnLastUpdateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repBtnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.tlbSave,
            this.tlbClose,
            this.barStaticItem1});
            this.barManager1.MaxItemId = 3;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbClose, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // tlbSave
            // 
            this.tlbSave.Caption = "保存";
            this.tlbSave.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.save_16x16;
            this.tlbSave.Id = 0;
            this.tlbSave.Name = "tlbSave";
            this.tlbSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSave_ItemClick);
            // 
            // tlbClose
            // 
            this.tlbClose.Caption = "关闭";
            this.tlbClose.Glyph = global::Sys.Safety.ClientFramework.Properties.Resources.close_16x16;
            this.tlbClose.Id = 1;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.barStaticItem1.Caption = "准备就绪...";
            this.barStaticItem1.Id = 2;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItem1.Width = 32;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(646, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 360);
            this.barDockControlBottom.Size = new System.Drawing.Size(646, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 329);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(646, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 329);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 31);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(646, 329);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repBtnDelete});
            this.gridControl1.Size = new System.Drawing.Size(622, 305);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnID,
            this.gridColumnstrType,
            this.gridColumnstrKey,
            this.gridColumnstrKeyCHs,
            this.gridColumnstrValue,
            this.gridColumnstrDesc,
            this.gridColumnCreator,
            this.gridColumnLastUpdateDate,
            this.gridColumnDelete});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 30;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // gridColumnID
            // 
            this.gridColumnID.Caption = "ID";
            this.gridColumnID.FieldName = "ID";
            this.gridColumnID.Name = "gridColumnID";
            this.gridColumnID.OptionsColumn.AllowEdit = false;
            this.gridColumnID.OptionsColumn.AllowFocus = false;
            this.gridColumnID.OptionsColumn.ReadOnly = true;
            // 
            // gridColumnstrType
            // 
            this.gridColumnstrType.Caption = "类型";
            this.gridColumnstrType.FieldName = "StrType";
            this.gridColumnstrType.Name = "gridColumnstrType";
            this.gridColumnstrType.Visible = true;
            this.gridColumnstrType.VisibleIndex = 0;
            // 
            // gridColumnstrKey
            // 
            this.gridColumnstrKey.Caption = "Key值";
            this.gridColumnstrKey.FieldName = "StrKey";
            this.gridColumnstrKey.Name = "gridColumnstrKey";
            this.gridColumnstrKey.Visible = true;
            this.gridColumnstrKey.VisibleIndex = 1;
            // 
            // gridColumnstrKeyCHs
            // 
            this.gridColumnstrKeyCHs.Caption = "key中文名";
            this.gridColumnstrKeyCHs.FieldName = "StrKeyCHs";
            this.gridColumnstrKeyCHs.Name = "gridColumnstrKeyCHs";
            this.gridColumnstrKeyCHs.Visible = true;
            this.gridColumnstrKeyCHs.VisibleIndex = 2;
            // 
            // gridColumnstrValue
            // 
            this.gridColumnstrValue.Caption = "Value";
            this.gridColumnstrValue.FieldName = "StrValue";
            this.gridColumnstrValue.Name = "gridColumnstrValue";
            this.gridColumnstrValue.Visible = true;
            this.gridColumnstrValue.VisibleIndex = 3;
            // 
            // gridColumnstrDesc
            // 
            this.gridColumnstrDesc.Caption = "备注";
            this.gridColumnstrDesc.FieldName = "StrDesc";
            this.gridColumnstrDesc.Name = "gridColumnstrDesc";
            this.gridColumnstrDesc.Visible = true;
            this.gridColumnstrDesc.VisibleIndex = 4;
            // 
            // gridColumnCreator
            // 
            this.gridColumnCreator.Caption = "创建人";
            this.gridColumnCreator.FieldName = "Creator";
            this.gridColumnCreator.Name = "gridColumnCreator";
            this.gridColumnCreator.OptionsColumn.AllowEdit = false;
            this.gridColumnCreator.OptionsColumn.AllowFocus = false;
            this.gridColumnCreator.OptionsColumn.ReadOnly = true;
            this.gridColumnCreator.Visible = true;
            this.gridColumnCreator.VisibleIndex = 5;
            // 
            // gridColumnLastUpdateDate
            // 
            this.gridColumnLastUpdateDate.Caption = "修改时间";
            this.gridColumnLastUpdateDate.FieldName = "LastUpdateDate";
            this.gridColumnLastUpdateDate.Name = "gridColumnLastUpdateDate";
            this.gridColumnLastUpdateDate.OptionsColumn.AllowEdit = false;
            this.gridColumnLastUpdateDate.OptionsColumn.AllowFocus = false;
            this.gridColumnLastUpdateDate.OptionsColumn.ReadOnly = true;
            this.gridColumnLastUpdateDate.Visible = true;
            this.gridColumnLastUpdateDate.VisibleIndex = 6;
            this.gridColumnLastUpdateDate.Width = 91;
            // 
            // gridColumnDelete
            // 
            this.gridColumnDelete.Caption = "删除";
            this.gridColumnDelete.ColumnEdit = this.repBtnDelete;
            this.gridColumnDelete.FieldName = "DetailDelete";
            this.gridColumnDelete.Name = "gridColumnDelete";
            this.gridColumnDelete.Visible = true;
            this.gridColumnDelete.VisibleIndex = 7;
            this.gridColumnDelete.Width = 47;
            // 
            // repBtnDelete
            // 
            this.repBtnDelete.AutoHeight = false;
            this.repBtnDelete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::Sys.Safety.ClientFramework.Properties.Resources.delete_16x16, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repBtnDelete.Name = "repBtnDelete";
            this.repBtnDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repBtnDelete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repBtnDelete_ButtonClick);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(646, 329);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(626, 309);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 387);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmSetting";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem tlbSave;
        private DevExpress.XtraBars.BarButtonItem tlbClose;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnstrType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnstrKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnstrKeyCHs;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnstrValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnstrDesc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnCreator;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLastUpdateDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDelete;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repBtnDelete;
    }
}