namespace Sys.Safety.Client.Display
{
    partial class frmRunLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRunLog));
            this.gcLogInfo = new DevExpress.XtraGrid.GridControl();
            this.gvLogInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.point = new DevExpress.XtraGrid.Columns.GridColumn();
            this.wz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.state = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sbstate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ssz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.txtQueryLog = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnQuery = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            ((System.ComponentModel.ISupportInitialize)(this.gcLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcLogInfo
            // 
            this.gcLogInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLogInfo.Location = new System.Drawing.Point(0, 0);
            this.gcLogInfo.MainView = this.gvLogInfo;
            this.gcLogInfo.Name = "gcLogInfo";
            this.gcLogInfo.Size = new System.Drawing.Size(870, 353);
            this.gcLogInfo.TabIndex = 2;
            this.gcLogInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLogInfo});
            // 
            // gvLogInfo
            // 
            this.gvLogInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.point,
            this.wz,
            this.time,
            this.state,
            this.sbstate,
            this.ssz,
            this.gridColumn1});
            this.gvLogInfo.GridControl = this.gcLogInfo;
            this.gvLogInfo.IndicatorWidth = 30;
            this.gvLogInfo.Name = "gvLogInfo";
            this.gvLogInfo.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gvLogInfo.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.gvLogInfo.OptionsBehavior.Editable = false;
            this.gvLogInfo.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gvLogInfo.OptionsCustomization.AllowColumnMoving = false;
            this.gvLogInfo.OptionsCustomization.AllowGroup = false;
            this.gvLogInfo.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvLogInfo.OptionsFilter.AllowFilterEditor = false;
            this.gvLogInfo.OptionsMenu.EnableColumnMenu = false;
            this.gvLogInfo.OptionsMenu.EnableFooterMenu = false;
            this.gvLogInfo.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLogInfo.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvLogInfo.OptionsView.ShowDetailButtons = false;
            this.gvLogInfo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvLogInfo.OptionsView.ShowGroupPanel = false;
            this.gvLogInfo.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.time, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gvLogInfo.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvLogInfo_CustomDrawRowIndicator);
            // 
            // point
            // 
            this.point.Caption = "测点编号";
            this.point.FieldName = "point";
            this.point.Name = "point";
            this.point.OptionsFilter.AllowFilter = false;
            this.point.Visible = true;
            this.point.VisibleIndex = 1;
            this.point.Width = 123;
            // 
            // wz
            // 
            this.wz.Caption = "位置";
            this.wz.FieldName = "wz";
            this.wz.Name = "wz";
            this.wz.OptionsFilter.AllowFilter = false;
            this.wz.Visible = true;
            this.wz.VisibleIndex = 2;
            this.wz.Width = 123;
            // 
            // time
            // 
            this.time.Caption = "时间";
            this.time.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.time.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.time.FieldName = "time";
            this.time.Name = "time";
            this.time.Visible = true;
            this.time.VisibleIndex = 0;
            this.time.Width = 163;
            // 
            // state
            // 
            this.state.Caption = "状态";
            this.state.FieldName = "state";
            this.state.Name = "state";
            this.state.Visible = true;
            this.state.VisibleIndex = 3;
            this.state.Width = 109;
            // 
            // sbstate
            // 
            this.sbstate.Caption = "设备状态";
            this.sbstate.FieldName = "sbstate";
            this.sbstate.Name = "sbstate";
            this.sbstate.Visible = true;
            this.sbstate.VisibleIndex = 4;
            // 
            // ssz
            // 
            this.ssz.Caption = "实时值";
            this.ssz.FieldName = "ssz";
            this.ssz.Name = "ssz";
            this.ssz.Visible = true;
            this.ssz.VisibleIndex = 5;
            this.ssz.Width = 113;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "uid";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnQuery,
            this.txtQueryLog,
            this.barStaticItem1,
            this.barHeaderItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 4;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar2.FloatLocation = new System.Drawing.Point(81, 0);
            this.bar2.FloatSize = new System.Drawing.Size(209, 100);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.txtQueryLog),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnQuery)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "工具栏";
            // 
            // txtQueryLog
            // 
            this.txtQueryLog.Edit = this.repositoryItemTextEdit1;
            this.txtQueryLog.Id = 1;
            this.txtQueryLog.Name = "txtQueryLog";
            this.txtQueryLog.Width = 189;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // btnQuery
            // 
            this.btnQuery.Caption = "查询";
            this.btnQuery.Id = 0;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnQuery_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(870, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 353);
            this.barDockControlBottom.Size = new System.Drawing.Size(870, 24);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 353);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(870, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 353);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "barStaticItem1";
            this.barStaticItem1.Id = 2;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "输入测点编号或位置（模糊查找）";
            this.barHeaderItem1.Id = 3;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // frmRunLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 377);
            this.Controls.Add(this.gcLogInfo);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRunLog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备实时运行记录";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRunLog_FormClosed);
            this.Load += new System.EventHandler(this.frmRunLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcLogInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLogInfo;
        private DevExpress.XtraGrid.Columns.GridColumn point;
        private DevExpress.XtraGrid.Columns.GridColumn wz;
        private DevExpress.XtraGrid.Columns.GridColumn time;
        private DevExpress.XtraGrid.Columns.GridColumn state;
        private DevExpress.XtraGrid.Columns.GridColumn ssz;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarEditItem txtQueryLog;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem btnQuery;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraGrid.Columns.GridColumn sbstate;
    }
}