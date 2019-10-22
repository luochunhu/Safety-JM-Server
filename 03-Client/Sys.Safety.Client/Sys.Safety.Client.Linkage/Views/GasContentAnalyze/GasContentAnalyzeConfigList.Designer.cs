namespace Sys.Safety.Client.Linkage.Views.GasContentAnalyze
{
    partial class GasContentAnalyzeConfigList
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GasContentAnalyzeConfigList));
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPointid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRealTimeValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnComparevalue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnHeight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnThickness = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSpeed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAcreage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnPercent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnWind = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItemAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl.Location = new System.Drawing.Point(0, 32);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl.Size = new System.Drawing.Size(1180, 434);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.gridView.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.gridView.AppearancePrint.HeaderPanel.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.gridView.AppearancePrint.Row.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView.AppearancePrint.Row.Options.UseFont = true;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnId,
            this.gridColumnPointid,
            this.gridColumnPoint,
            this.gridColumnRealTimeValue,
            this.gridColumnState,
            this.gridColumnComparevalue,
            this.gridColumnLocation,
            this.gridColumnHeight,
            this.gridColumnWidth,
            this.gridColumnThickness,
            this.gridColumnSpeed,
            this.gridColumnLength,
            this.gridColumnAcreage,
            this.gridColumnPercent,
            this.gridColumnWind});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView.GridControl = this.gridControl;
            this.gridView.GroupFormat = "{1} {2}";
            this.gridView.IndicatorWidth = 40;
            this.gridView.Name = "gridView";
            this.gridView.OptionsDetail.AllowZoomDetail = false;
            this.gridView.OptionsMenu.EnableFooterMenu = false;
            this.gridView.OptionsPrint.AutoWidth = false;
            this.gridView.OptionsPrint.PrintPreview = true;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridView.OptionsSelection.InvertSelection = true;
            this.gridView.OptionsSelection.MultiSelect = true;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.RowAutoHeight = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnId
            // 
            this.gridColumnId.Caption = "Id";
            this.gridColumnId.FieldName = "Id";
            this.gridColumnId.Name = "gridColumnId";
            // 
            // gridColumnPointid
            // 
            this.gridColumnPointid.Caption = "Pointid";
            this.gridColumnPointid.FieldName = "Pointid";
            this.gridColumnPointid.Name = "gridColumnPointid";
            // 
            // gridColumnPoint
            // 
            this.gridColumnPoint.Caption = "测点号";
            this.gridColumnPoint.FieldName = "Point";
            this.gridColumnPoint.Name = "gridColumnPoint";
            this.gridColumnPoint.OptionsColumn.AllowEdit = false;
            this.gridColumnPoint.Visible = true;
            this.gridColumnPoint.VisibleIndex = 0;
            // 
            // gridColumnRealTimeValue
            // 
            this.gridColumnRealTimeValue.Caption = "实时值";
            this.gridColumnRealTimeValue.FieldName = "RealTimeValue";
            this.gridColumnRealTimeValue.Name = "gridColumnRealTimeValue";
            this.gridColumnRealTimeValue.OptionsColumn.AllowEdit = false;
            this.gridColumnRealTimeValue.Visible = true;
            this.gridColumnRealTimeValue.VisibleIndex = 1;
            // 
            // gridColumnState
            // 
            this.gridColumnState.Caption = "报警状态";
            this.gridColumnState.FieldName = "State";
            this.gridColumnState.Name = "gridColumnState";
            this.gridColumnState.OptionsColumn.AllowEdit = false;
            this.gridColumnState.Visible = true;
            this.gridColumnState.VisibleIndex = 2;
            // 
            // gridColumnComparevalue
            // 
            this.gridColumnComparevalue.Caption = "瓦斯含量报警值";
            this.gridColumnComparevalue.FieldName = "Comparevalue";
            this.gridColumnComparevalue.Name = "gridColumnComparevalue";
            this.gridColumnComparevalue.OptionsColumn.AllowEdit = false;
            this.gridColumnComparevalue.Visible = true;
            this.gridColumnComparevalue.VisibleIndex = 3;
            this.gridColumnComparevalue.Width = 100;
            // 
            // gridColumnLocation
            // 
            this.gridColumnLocation.Caption = "安装位置";
            this.gridColumnLocation.FieldName = "Location";
            this.gridColumnLocation.Name = "gridColumnLocation";
            this.gridColumnLocation.OptionsColumn.AllowEdit = false;
            this.gridColumnLocation.Visible = true;
            this.gridColumnLocation.VisibleIndex = 4;
            this.gridColumnLocation.Width = 127;
            // 
            // gridColumnHeight
            // 
            this.gridColumnHeight.Caption = "巷道高";
            this.gridColumnHeight.FieldName = "Height";
            this.gridColumnHeight.Name = "gridColumnHeight";
            this.gridColumnHeight.OptionsColumn.AllowEdit = false;
            this.gridColumnHeight.Visible = true;
            this.gridColumnHeight.VisibleIndex = 5;
            this.gridColumnHeight.Width = 60;
            // 
            // gridColumnWidth
            // 
            this.gridColumnWidth.Caption = "巷道宽";
            this.gridColumnWidth.FieldName = "Width";
            this.gridColumnWidth.Name = "gridColumnWidth";
            this.gridColumnWidth.OptionsColumn.AllowEdit = false;
            this.gridColumnWidth.Visible = true;
            this.gridColumnWidth.VisibleIndex = 6;
            this.gridColumnWidth.Width = 60;
            // 
            // gridColumnThickness
            // 
            this.gridColumnThickness.Caption = "煤层厚度";
            this.gridColumnThickness.FieldName = "Thickness";
            this.gridColumnThickness.Name = "gridColumnThickness";
            this.gridColumnThickness.OptionsColumn.AllowEdit = false;
            this.gridColumnThickness.Visible = true;
            this.gridColumnThickness.VisibleIndex = 7;
            // 
            // gridColumnSpeed
            // 
            this.gridColumnSpeed.Caption = "掘进速度(m/Month)";
            this.gridColumnSpeed.FieldName = "Speed";
            this.gridColumnSpeed.Name = "gridColumnSpeed";
            this.gridColumnSpeed.OptionsColumn.AllowEdit = false;
            this.gridColumnSpeed.Visible = true;
            this.gridColumnSpeed.VisibleIndex = 8;
            this.gridColumnSpeed.Width = 120;
            // 
            // gridColumnLength
            // 
            this.gridColumnLength.Caption = "巷道已暴露长度(m)";
            this.gridColumnLength.FieldName = "Length";
            this.gridColumnLength.Name = "gridColumnLength";
            this.gridColumnLength.OptionsColumn.AllowEdit = false;
            this.gridColumnLength.Visible = true;
            this.gridColumnLength.VisibleIndex = 9;
            this.gridColumnLength.Width = 100;
            // 
            // gridColumnAcreage
            // 
            this.gridColumnAcreage.Caption = "断面";
            this.gridColumnAcreage.FieldName = "Acreage";
            this.gridColumnAcreage.Name = "gridColumnAcreage";
            this.gridColumnAcreage.OptionsColumn.AllowEdit = false;
            this.gridColumnAcreage.Visible = true;
            this.gridColumnAcreage.VisibleIndex = 10;
            this.gridColumnAcreage.Width = 60;
            // 
            // gridColumnPercent
            // 
            this.gridColumnPercent.Caption = "煤的挥发分(%)";
            this.gridColumnPercent.FieldName = "Percent";
            this.gridColumnPercent.Name = "gridColumnPercent";
            this.gridColumnPercent.OptionsColumn.AllowEdit = false;
            this.gridColumnPercent.Visible = true;
            this.gridColumnPercent.VisibleIndex = 11;
            this.gridColumnPercent.Width = 100;
            // 
            // gridColumnWind
            // 
            this.gridColumnWind.Caption = "风量";
            this.gridColumnWind.FieldName = "Wind";
            this.gridColumnWind.Name = "gridColumnWind";
            this.gridColumnWind.OptionsColumn.AllowEdit = false;
            this.gridColumnWind.Visible = true;
            this.gridColumnWind.VisibleIndex = 12;
            this.gridColumnWind.Width = 60;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemAdd,
            this.barButtonItemDelete});
            this.barManager1.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // barButtonItemAdd
            // 
            this.barButtonItemAdd.Caption = "新增";
            this.barButtonItemAdd.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemAdd.Glyph")));
            this.barButtonItemAdd.Id = 1;
            this.barButtonItemAdd.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemAdd.LargeGlyph")));
            this.barButtonItemAdd.Name = "barButtonItemAdd";
            this.barButtonItemAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAdd_ItemClick);
            // 
            // barButtonItemDelete
            // 
            this.barButtonItemDelete.Caption = "删除";
            this.barButtonItemDelete.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDelete.Glyph")));
            this.barButtonItemDelete.Id = 2;
            this.barButtonItemDelete.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemDelete.LargeGlyph")));
            this.barButtonItemDelete.Name = "barButtonItemDelete";
            this.barButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelete_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1180, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 467);
            this.barDockControlBottom.Size = new System.Drawing.Size(1180, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 436);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1180, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 436);
            // 
            // GasContentAnalyzeConfigList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 467);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GasContentAnalyzeConfigList";
            this.Text = "瓦斯含量分析配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GasContentAnalyzeConfigList_FormClosing);
            this.Load += new System.EventHandler(this.GasContentAnalyzeConfigList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAdd;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDelete;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnId;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPointid;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPoint;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLocation;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnHeight;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnWidth;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnThickness;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSpeed;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLength;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAcreage;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPercent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnWind;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnComparevalue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRealTimeValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnState;
    }
}