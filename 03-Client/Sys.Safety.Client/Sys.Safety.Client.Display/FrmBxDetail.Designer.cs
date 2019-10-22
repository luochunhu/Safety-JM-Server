namespace Sys.Safety.Client.Display
{
    partial class FrmBxDetail
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBxDetail));
            this.bxztStr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BxDetailGri = new DevExpress.XtraGrid.GridControl();
            this.gvLogInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.point = new DevExpress.XtraGrid.Columns.GridColumn();
            this.name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.stime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.etime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cx = new DevExpress.XtraGrid.Columns.GridColumn();
            this.zdz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.zxz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pjz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.zdztime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.zxztime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pointid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.QueryTime = new DevExpress.XtraEditors.LabelControl();
            this.Query = new DevExpress.XtraEditors.SimpleButton();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.BxDetailGri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // bxztStr
            // 
            this.bxztStr.Caption = "标校状态";
            this.bxztStr.FieldName = "bxztStr";
            this.bxztStr.Name = "bxztStr";
            this.bxztStr.Visible = true;
            this.bxztStr.VisibleIndex = 10;
            this.bxztStr.Width = 60;
            // 
            // BxDetailGri
            // 
            this.BxDetailGri.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BxDetailGri.Location = new System.Drawing.Point(0, 61);
            this.BxDetailGri.MainView = this.gvLogInfo;
            this.BxDetailGri.Name = "BxDetailGri";
            this.BxDetailGri.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemMemoEdit2});
            this.BxDetailGri.Size = new System.Drawing.Size(1084, 401);
            this.BxDetailGri.TabIndex = 8;
            this.BxDetailGri.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLogInfo});
            this.BxDetailGri.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BxDetailGri_MouseUp);
            // 
            // gvLogInfo
            // 
            this.gvLogInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.point,
            this.name,
            this.stime,
            this.etime,
            this.cx,
            this.zdz,
            this.zxz,
            this.pjz,
            this.zdztime,
            this.zxztime,
            this.bxztStr,
            this.pointid});
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.Green;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Column = this.bxztStr;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "标校完成";
            styleFormatCondition2.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition2.Appearance.Options.UseForeColor = true;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = "标校中";
            this.gvLogInfo.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2});
            this.gvLogInfo.GridControl = this.BxDetailGri;
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
            this.gvLogInfo.OptionsView.RowAutoHeight = true;
            this.gvLogInfo.OptionsView.ShowDetailButtons = false;
            this.gvLogInfo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvLogInfo.OptionsView.ShowGroupPanel = false;
            // 
            // point
            // 
            this.point.Caption = "测点号";
            this.point.FieldName = "point";
            this.point.Name = "point";
            this.point.OptionsFilter.AllowFilter = false;
            this.point.Visible = true;
            this.point.VisibleIndex = 0;
            this.point.Width = 80;
            // 
            // name
            // 
            this.name.Caption = "标校人";
            this.name.FieldName = "name";
            this.name.Name = "name";
            this.name.Visible = true;
            this.name.VisibleIndex = 1;
            this.name.Width = 70;
            // 
            // stime
            // 
            this.stime.Caption = "开始时间";
            this.stime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.stime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.stime.FieldName = "stime";
            this.stime.Name = "stime";
            this.stime.Visible = true;
            this.stime.VisibleIndex = 2;
            this.stime.Width = 140;
            // 
            // etime
            // 
            this.etime.Caption = "结束时间";
            this.etime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.etime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.etime.FieldName = "etime";
            this.etime.Name = "etime";
            this.etime.Visible = true;
            this.etime.VisibleIndex = 3;
            this.etime.Width = 140;
            // 
            // cx
            // 
            this.cx.Caption = "持续时间";
            this.cx.FieldName = "cxText";
            this.cx.Name = "cx";
            this.cx.Visible = true;
            this.cx.VisibleIndex = 4;
            this.cx.Width = 72;
            // 
            // zdz
            // 
            this.zdz.Caption = "最大值";
            this.zdz.FieldName = "zdz";
            this.zdz.Name = "zdz";
            this.zdz.Visible = true;
            this.zdz.VisibleIndex = 5;
            this.zdz.Width = 72;
            // 
            // zxz
            // 
            this.zxz.Caption = "最小值";
            this.zxz.FieldName = "zxz";
            this.zxz.Name = "zxz";
            this.zxz.Visible = true;
            this.zxz.VisibleIndex = 6;
            this.zxz.Width = 72;
            // 
            // pjz
            // 
            this.pjz.Caption = "平均值";
            this.pjz.FieldName = "pjz";
            this.pjz.Name = "pjz";
            this.pjz.Visible = true;
            this.pjz.VisibleIndex = 7;
            this.pjz.Width = 72;
            // 
            // zdztime
            // 
            this.zdztime.Caption = "最大值时间";
            this.zdztime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.zdztime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.zdztime.FieldName = "zdztime";
            this.zdztime.Name = "zdztime";
            this.zdztime.Visible = true;
            this.zdztime.VisibleIndex = 8;
            this.zdztime.Width = 140;
            // 
            // zxztime
            // 
            this.zxztime.Caption = "最小值时间";
            this.zxztime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.zxztime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.zxztime.FieldName = "zxztime";
            this.zxztime.Name = "zxztime";
            this.zxztime.Visible = true;
            this.zxztime.VisibleIndex = 9;
            this.zxztime.Width = 140;
            // 
            // pointid
            // 
            this.pointid.Caption = "gridColumn1";
            this.pointid.FieldName = "pointid";
            this.pointid.Name = "pointid";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "日期：";
            // 
            // QueryTime
            // 
            this.QueryTime.Location = new System.Drawing.Point(54, 16);
            this.QueryTime.Name = "QueryTime";
            this.QueryTime.Size = new System.Drawing.Size(92, 14);
            this.QueryTime.TabIndex = 10;
            this.QueryTime.Text = "2017年01月22日";
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(152, 12);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(47, 23);
            this.Query.TabIndex = 11;
            this.Query.Text = "刷新";
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "密采曲线";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "五分钟曲线";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 2;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1084, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 462);
            this.barDockControlBottom.Size = new System.Drawing.Size(1084, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 462);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1084, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 462);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl5.Location = new System.Drawing.Point(12, 41);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(156, 14);
            this.labelControl5.TabIndex = 101;
            this.labelControl5.Text = "操作提示：右键查看相关选项";
            // 
            // FrmBxDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 462);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.Query);
            this.Controls.Add(this.QueryTime);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BxDetailGri);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBxDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标校详细信息";
            ((System.ComponentModel.ISupportInitialize)(this.BxDetailGri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl BxDetailGri;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLogInfo;
        private DevExpress.XtraGrid.Columns.GridColumn point;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn name;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl QueryTime;
        private DevExpress.XtraEditors.SimpleButton Query;
        private DevExpress.XtraGrid.Columns.GridColumn stime;
        private DevExpress.XtraGrid.Columns.GridColumn etime;
        private DevExpress.XtraGrid.Columns.GridColumn cx;
        private DevExpress.XtraGrid.Columns.GridColumn zdz;
        private DevExpress.XtraGrid.Columns.GridColumn zxz;
        private DevExpress.XtraGrid.Columns.GridColumn pjz;
        private DevExpress.XtraGrid.Columns.GridColumn zdztime;
        private DevExpress.XtraGrid.Columns.GridColumn zxztime;
        private DevExpress.XtraGrid.Columns.GridColumn bxztStr;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.Columns.GridColumn pointid;
        private DevExpress.XtraEditors.LabelControl labelControl5;


    }
}