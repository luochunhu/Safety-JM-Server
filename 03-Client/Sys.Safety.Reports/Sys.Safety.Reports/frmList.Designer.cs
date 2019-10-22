using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace Sys.Safety.Reports
{
    partial class frmList : DevExpress.XtraEditors.XtraForm
    {

        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmList));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ToolBar = new DevExpress.XtraBars.Bar();
            this.tlbRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.BeforeDay = new DevExpress.XtraBars.BarButtonItem();
            this.AfterDay = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItemExport = new DevExpress.XtraBars.BarSubItem();
            this.tlbExportExcel = new DevExpress.XtraBars.BarButtonItem();
            this.tlbExeclPDF = new DevExpress.XtraBars.BarButtonItem();
            this.txtExportTXT = new DevExpress.XtraBars.BarButtonItem();
            this.txtExportCSV = new DevExpress.XtraBars.BarButtonItem();
            this.txtExportHTML = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExportExcelAll = new DevExpress.XtraBars.BarButtonItem();
            this.tlbPrint = new DevExpress.XtraBars.BarButtonItem();
            this.tlbMultiSelection = new DevExpress.XtraBars.BarCheckItem();
            this.tlbGroupList = new DevExpress.XtraBars.BarSubItem();
            this.tlbDesign = new DevExpress.XtraBars.BarSubItem();
            this.tlbSimpleSearch = new DevExpress.XtraBars.BarCheckItem();
            this.tlbAdvancedSearch = new DevExpress.XtraBars.BarButtonItem();
            this.chkItemAllowCopy = new DevExpress.XtraBars.BarCheckItem();
            this.chkItemFreQryCondition = new DevExpress.XtraBars.BarCheckItem();
            this.tlbSaveFreQryCon = new DevExpress.XtraBars.BarButtonItem();
            this.tlbSaveWidth = new DevExpress.XtraBars.BarButtonItem();
            this.menuListStyle = new DevExpress.XtraBars.BarSubItem();
            this.tlbSetDefaultStyle = new DevExpress.XtraBars.BarButtonItem();
            this.tlbFilterAutoSelect = new DevExpress.XtraBars.BarCheckItem();
            this.tlbSet = new DevExpress.XtraBars.BarButtonItem();
            this.tlbDescInput = new DevExpress.XtraBars.BarButtonItem();
            this.tlbColumnSortSet = new DevExpress.XtraBars.BarButtonItem();
            this.tlbSchema = new DevExpress.XtraBars.BarButtonItem();
            this.tlbPivotSetting = new DevExpress.XtraBars.BarButtonItem();
            this.chkblnKeyWord = new DevExpress.XtraBars.BarCheckItem();
            this.ToolBarPage = new DevExpress.XtraBars.Bar();
            this.tlbGoToFirstPage = new DevExpress.XtraBars.BarButtonItem();
            this.tlbGoToPreviousPage = new DevExpress.XtraBars.BarButtonItem();
            this.tlbSetCurrentPage = new DevExpress.XtraBars.BarEditItem();
            this.CurrentPageText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tlbGoToPage = new DevExpress.XtraBars.BarButtonItem();
            this.tlbSetPerPageNumber = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.tlbGoToNextPage = new DevExpress.XtraBars.BarButtonItem();
            this.tlbGoToLastPage = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItemMsg = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemRowCount = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.txtContent = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem3 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem4 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItem5 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tlbChartSetting = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.cboListStyle = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit10 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit11 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit12 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit13 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnContent = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemTextEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lblTile = new System.Windows.Forms.Label();
            this.ps = new DevExpress.XtraPrinting.PrintingSystem(this.components);
            this.buttonContent = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.panelFreQry = new System.Windows.Forms.Panel();
            this.panelControlPointFilter = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditArrangeActivity = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEditRemoveDuplication = new DevExpress.XtraEditors.LookUpEdit();
            this.labelArrangePoint = new System.Windows.Forms.Label();
            this.lookUpEditArrangeTime = new DevExpress.XtraEditors.LookUpEdit();
            this.radioButtonArrangePoint = new System.Windows.Forms.RadioButton();
            this.radioButtonStorePoint = new System.Windows.Forms.RadioButton();
            this.radioButtonActivityPoint = new System.Windows.Forms.RadioButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.tlbRefresh1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.documentViewer1 = new DevExpress.XtraPrinting.Preview.DocumentViewer();
            this.chartControl = new DevExpress.XtraCharts.ChartControl();
            this.pivotGridControl = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutItemGrid = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutItemChart = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutItemPivotGrid = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutItemReport = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupFreQry = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemPointFilter = new DevExpress.XtraLayout.LayoutControlItem();
            this.lookupShowStyle = new DevExpress.XtraEditors.LookUpEdit();
            this.lookupSchema = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPageText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboListStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlPointFilter)).BeginInit();
            this.panelControlPointFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditArrangeActivity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditRemoveDuplication.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditArrangeTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemPivotGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFreQry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPointFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupShowStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupSchema.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ToolBar,
            this.ToolBarPage});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.tlbRefresh,
            this.barEditItem1,
            this.barButtonItem1,
            this.tlbMultiSelection,
            this.barButtonItem2,
            this.barEditItem2,
            this.barEditItem3,
            this.barEditItem4,
            this.tlbGoToFirstPage,
            this.tlbGoToPreviousPage,
            this.barEditItem5,
            this.tlbSetCurrentPage,
            this.tlbGoToPage,
            this.tlbGoToNextPage,
            this.tlbGoToLastPage,
            this.tlbSetPerPageNumber,
            this.tlbDesign,
            this.menuListStyle,
            this.tlbPrint,
            this.tlbAdvancedSearch,
            this.tlbSimpleSearch,
            this.tlbSchema,
            this.barStaticItemMsg,
            this.tlbPivotSetting,
            this.tlbChartSetting,
            this.chkItemAllowCopy,
            this.chkItemFreQryCondition,
            this.tlbSaveFreQryCon,
            this.tlbSetDefaultStyle,
            this.barStaticItemRowCount,
            this.tlbFilterAutoSelect,
            this.barButtonItem3,
            this.tlbGroupList,
            this.barSubItemExport,
            this.tlbExportExcel,
            this.tlbExeclPDF,
            this.txtExportTXT,
            this.txtExportCSV,
            this.tlbSaveWidth,
            this.txtExportHTML,
            this.tlbSet,
            this.tlbDescInput,
            this.chkblnKeyWord,
            this.tlbColumnSortSet,
            this.BeforeDay,
            this.AfterDay,
            this.barButtonItemExportExcelAll});
            this.barManager.MaxItemId = 93;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.cboListStyle,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit4,
            this.repositoryItemTextEdit5,
            this.txtContent,
            this.repositoryItemTextEdit7,
            this.repositoryItemTextEdit8,
            this.repositoryItemTextEdit9,
            this.CurrentPageText,
            this.repositoryItemComboBox2,
            this.repositoryItemTextEdit10,
            this.repositoryItemTextEdit11,
            this.repositoryItemTextEdit12,
            this.repositoryItemTextEdit13,
            this.btnContent,
            this.repositoryItemTextEdit6,
            this.repositoryItemLookUpEdit1});
            // 
            // ToolBar
            // 
            this.ToolBar.BarName = "ToolBar";
            this.ToolBar.DockCol = 0;
            this.ToolBar.DockRow = 0;
            this.ToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.ToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbRefresh, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.BeforeDay, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.AfterDay, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItemExport, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbPrint, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbMultiSelection, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbGroupList, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbDesign, "", true, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.tlbSchema, false),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.tlbPivotSetting, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.chkblnKeyWord)});
            this.ToolBar.OptionsBar.AllowQuickCustomization = false;
            this.ToolBar.OptionsBar.DrawDragBorder = false;
            this.ToolBar.OptionsBar.MultiLine = true;
            this.ToolBar.OptionsBar.UseWholeRow = true;
            this.ToolBar.Text = "工具栏";
            this.ToolBar.Visible = false;
            // 
            // tlbRefresh
            // 
            this.tlbRefresh.Caption = "查询(&R)";
            this.tlbRefresh.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbRefresh.Glyph")));
            this.tlbRefresh.Hint = "刷新当前列表数据";
            this.tlbRefresh.Id = 1;
            this.tlbRefresh.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbRefresh.LargeGlyph")));
            this.tlbRefresh.Name = "tlbRefresh";
            this.tlbRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbRefresh_ItemClick);
            // 
            // BeforeDay
            // 
            this.BeforeDay.Caption = "前一天";
            this.BeforeDay.Glyph = ((System.Drawing.Image)(resources.GetObject("BeforeDay.Glyph")));
            this.BeforeDay.Id = 90;
            this.BeforeDay.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("BeforeDay.LargeGlyph")));
            this.BeforeDay.Name = "BeforeDay";
            this.BeforeDay.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BeforeDay_ItemClick);
            // 
            // AfterDay
            // 
            this.AfterDay.Caption = "后一天";
            this.AfterDay.Glyph = ((System.Drawing.Image)(resources.GetObject("AfterDay.Glyph")));
            this.AfterDay.Id = 91;
            this.AfterDay.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("AfterDay.LargeGlyph")));
            this.AfterDay.Name = "AfterDay";
            this.AfterDay.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AfterDay_ItemClick);
            // 
            // barSubItemExport
            // 
            this.barSubItemExport.Caption = "导出(&E)";
            this.barSubItemExport.Glyph = ((System.Drawing.Image)(resources.GetObject("barSubItemExport.Glyph")));
            this.barSubItemExport.Id = 76;
            this.barSubItemExport.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barSubItemExport.LargeGlyph")));
            this.barSubItemExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbExportExcel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.tlbExeclPDF, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.txtExportTXT),
            new DevExpress.XtraBars.LinkPersistInfo(this.txtExportCSV),
            new DevExpress.XtraBars.LinkPersistInfo(this.txtExportHTML),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemExportExcelAll, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barSubItemExport.Name = "barSubItemExport";
            this.barSubItemExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barSubItemExport_ItemClick);
            // 
            // tlbExportExcel
            // 
            this.tlbExportExcel.Caption = "导出Excel";
            this.tlbExportExcel.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbExportExcel.Glyph")));
            this.tlbExportExcel.Id = 77;
            this.tlbExportExcel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbExportExcel.LargeGlyph")));
            this.tlbExportExcel.Name = "tlbExportExcel";
            this.tlbExportExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbExportExcel_ItemClick);
            // 
            // tlbExeclPDF
            // 
            this.tlbExeclPDF.Caption = "导出PDF";
            this.tlbExeclPDF.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbExeclPDF.Glyph")));
            this.tlbExeclPDF.Id = 78;
            this.tlbExeclPDF.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbExeclPDF.LargeGlyph")));
            this.tlbExeclPDF.Name = "tlbExeclPDF";
            this.tlbExeclPDF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbExeclPDF_ItemClick);
            // 
            // txtExportTXT
            // 
            this.txtExportTXT.Caption = "导出TXT";
            this.txtExportTXT.Glyph = ((System.Drawing.Image)(resources.GetObject("txtExportTXT.Glyph")));
            this.txtExportTXT.Id = 79;
            this.txtExportTXT.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("txtExportTXT.LargeGlyph")));
            this.txtExportTXT.Name = "txtExportTXT";
            this.txtExportTXT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.txtExportTXT_ItemClick);
            // 
            // txtExportCSV
            // 
            this.txtExportCSV.Caption = "导出CSV";
            this.txtExportCSV.Glyph = ((System.Drawing.Image)(resources.GetObject("txtExportCSV.Glyph")));
            this.txtExportCSV.Id = 80;
            this.txtExportCSV.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("txtExportCSV.LargeGlyph")));
            this.txtExportCSV.Name = "txtExportCSV";
            this.txtExportCSV.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.txtExportCSV_ItemClick);
            // 
            // txtExportHTML
            // 
            this.txtExportHTML.Caption = "导出HTML";
            this.txtExportHTML.Glyph = ((System.Drawing.Image)(resources.GetObject("txtExportHTML.Glyph")));
            this.txtExportHTML.Id = 84;
            this.txtExportHTML.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("txtExportHTML.LargeGlyph")));
            this.txtExportHTML.Name = "txtExportHTML";
            this.txtExportHTML.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.txtExportHTML_ItemClick);
            // 
            // barButtonItemExportExcelAll
            // 
            this.barButtonItemExportExcelAll.Caption = "导出Excel（全部）";
            this.barButtonItemExportExcelAll.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemExportExcelAll.Glyph")));
            this.barButtonItemExportExcelAll.Id = 92;
            this.barButtonItemExportExcelAll.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemExportExcelAll.LargeGlyph")));
            this.barButtonItemExportExcelAll.Name = "barButtonItemExportExcelAll";
            this.barButtonItemExportExcelAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemExportExcelAll_ItemClick);
            // 
            // tlbPrint
            // 
            this.tlbPrint.Caption = "打印(&P)";
            this.tlbPrint.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbPrint.Glyph")));
            this.tlbPrint.Id = 50;
            this.tlbPrint.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbPrint.LargeGlyph")));
            this.tlbPrint.Name = "tlbPrint";
            this.tlbPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbPrint_ItemClick);
            // 
            // tlbMultiSelection
            // 
            this.tlbMultiSelection.Caption = "全选(&M)";
            this.tlbMultiSelection.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.tlbMultiSelection.Hint = "选择全部数据或者取消全选";
            this.tlbMultiSelection.Id = 9;
            this.tlbMultiSelection.Name = "tlbMultiSelection";
            this.tlbMultiSelection.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbMultiSelection_CheckedChanged);
            // 
            // tlbGroupList
            // 
            this.tlbGroupList.Caption = "关联列表(&T)";
            this.tlbGroupList.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGroupList.Glyph")));
            this.tlbGroupList.Id = 75;
            this.tlbGroupList.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGroupList.LargeGlyph")));
            this.tlbGroupList.Name = "tlbGroupList";
            // 
            // tlbDesign
            // 
            this.tlbDesign.Caption = "常用功能(&U)";
            this.tlbDesign.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbDesign.Glyph")));
            this.tlbDesign.Id = 35;
            this.tlbDesign.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbDesign.LargeGlyph")));
            this.tlbDesign.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbSimpleSearch),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbAdvancedSearch),
            new DevExpress.XtraBars.LinkPersistInfo(this.chkItemAllowCopy, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.chkItemFreQryCondition),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbSaveFreQryCon, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbSaveWidth),
            new DevExpress.XtraBars.LinkPersistInfo(this.menuListStyle),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbSetDefaultStyle),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbFilterAutoSelect),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbSet),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbDescInput),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbColumnSortSet)});
            this.tlbDesign.Name = "tlbDesign";
            this.tlbDesign.Popup += new System.EventHandler(this.tlbDesign_Popup);
            // 
            // tlbSimpleSearch
            // 
            this.tlbSimpleSearch.Caption = "简单查询";
            this.tlbSimpleSearch.Id = 52;
            this.tlbSimpleSearch.Name = "tlbSimpleSearch";
            this.tlbSimpleSearch.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.tlbSimpleSearch.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSimpleSearch_CheckedChanged);
            // 
            // tlbAdvancedSearch
            // 
            this.tlbAdvancedSearch.Caption = "高级查询";
            this.tlbAdvancedSearch.Id = 51;
            this.tlbAdvancedSearch.Name = "tlbAdvancedSearch";
            this.tlbAdvancedSearch.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.tlbAdvancedSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbAdvancedSearch_ItemClick);
            // 
            // chkItemAllowCopy
            // 
            this.chkItemAllowCopy.Caption = "允许复制数据";
            this.chkItemAllowCopy.Id = 65;
            this.chkItemAllowCopy.Name = "chkItemAllowCopy";
            this.chkItemAllowCopy.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.chkItemAllowCopy_CheckedChanged);
            // 
            // chkItemFreQryCondition
            // 
            this.chkItemFreQryCondition.Caption = "显示常用查询条件";
            this.chkItemFreQryCondition.Id = 67;
            this.chkItemFreQryCondition.Name = "chkItemFreQryCondition";
            this.chkItemFreQryCondition.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.chkItemFreQryCondition_CheckedChanged);
            // 
            // tlbSaveFreQryCon
            // 
            this.tlbSaveFreQryCon.Caption = "保存常用条件";
            this.tlbSaveFreQryCon.Id = 68;
            this.tlbSaveFreQryCon.Name = "tlbSaveFreQryCon";
            this.tlbSaveFreQryCon.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.tlbSaveFreQryCon.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSaveFreQryCon_ItemClick);
            // 
            // tlbSaveWidth
            // 
            this.tlbSaveWidth.Caption = "保存栏目宽度";
            this.tlbSaveWidth.Id = 81;
            this.tlbSaveWidth.Name = "tlbSaveWidth";
            this.tlbSaveWidth.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSaveWidth_ItemClick);
            // 
            // menuListStyle
            // 
            this.menuListStyle.Caption = "列表风格";
            this.menuListStyle.Id = 46;
            this.menuListStyle.Name = "menuListStyle";
            // 
            // tlbSetDefaultStyle
            // 
            this.tlbSetDefaultStyle.Caption = "设置为默认展现方式";
            this.tlbSetDefaultStyle.Id = 69;
            this.tlbSetDefaultStyle.Name = "tlbSetDefaultStyle";
            this.tlbSetDefaultStyle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSetDefaultStyle_ItemClick);
            // 
            // tlbFilterAutoSelect
            // 
            this.tlbFilterAutoSelect.Caption = "过滤自动选择";
            this.tlbFilterAutoSelect.Hint = "过滤自动选择数据行";
            this.tlbFilterAutoSelect.Id = 71;
            this.tlbFilterAutoSelect.Name = "tlbFilterAutoSelect";
            this.tlbFilterAutoSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // tlbSet
            // 
            this.tlbSet.Caption = "参数设置";
            this.tlbSet.Id = 85;
            this.tlbSet.Name = "tlbSet";
            this.tlbSet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSet_ItemClick);
            // 
            // tlbDescInput
            // 
            this.tlbDescInput.Caption = "备注录入";
            this.tlbDescInput.Id = 86;
            this.tlbDescInput.Name = "tlbDescInput";
            this.tlbDescInput.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbDescInput_ItemClick);
            // 
            // tlbColumnSortSet
            // 
            this.tlbColumnSortSet.Caption = "栏目顺序编排";
            this.tlbColumnSortSet.Id = 89;
            this.tlbColumnSortSet.Name = "tlbColumnSortSet";
            this.tlbColumnSortSet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbColumnSortSet_ItemClick);
            // 
            // tlbSchema
            // 
            this.tlbSchema.Caption = "方案(&S)";
            this.tlbSchema.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbSchema.Glyph")));
            this.tlbSchema.Id = 59;
            this.tlbSchema.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbSchema.LargeGlyph")));
            this.tlbSchema.Name = "tlbSchema";
            this.tlbSchema.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.tlbSchema.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbSchema_ItemClick);
            // 
            // tlbPivotSetting
            // 
            this.tlbPivotSetting.Caption = "报表设计器(R)";
            this.tlbPivotSetting.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbPivotSetting.Glyph")));
            this.tlbPivotSetting.Id = 63;
            this.tlbPivotSetting.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbPivotSetting.LargeGlyph")));
            this.tlbPivotSetting.Name = "tlbPivotSetting";
            this.tlbPivotSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.tlbPivotSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbPivotSetting_ItemClick);
            // 
            // chkblnKeyWord
            // 
            this.chkblnKeyWord.Caption = "忽略安装位置";
            this.chkblnKeyWord.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.chkblnKeyWord.Id = 88;
            this.chkblnKeyWord.Name = "chkblnKeyWord";
            this.chkblnKeyWord.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.chkblnKeyWord.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.chkblnKeyWord_CheckedChanged);
            // 
            // ToolBarPage
            // 
            this.ToolBarPage.BarName = "ToolBarPage";
            this.ToolBarPage.DockCol = 0;
            this.ToolBarPage.DockRow = 0;
            this.ToolBarPage.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.ToolBarPage.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbGoToFirstPage),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbGoToPreviousPage),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.tlbSetCurrentPage, "", true, true, true, 65),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbGoToPage),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.tlbSetPerPageNumber, "", true, true, true, 58),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbGoToNextPage, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.tlbGoToLastPage),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemMsg),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemRowCount)});
            this.ToolBarPage.OptionsBar.AllowQuickCustomization = false;
            this.ToolBarPage.OptionsBar.DrawDragBorder = false;
            this.ToolBarPage.OptionsBar.MultiLine = true;
            this.ToolBarPage.OptionsBar.UseWholeRow = true;
            this.ToolBarPage.Text = "页面工具栏";
            this.ToolBarPage.Visible = false;
            // 
            // tlbGoToFirstPage
            // 
            this.tlbGoToFirstPage.Caption = "第一页";
            this.tlbGoToFirstPage.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToFirstPage.Glyph")));
            this.tlbGoToFirstPage.Hint = "显示第一页数据";
            this.tlbGoToFirstPage.Id = 23;
            this.tlbGoToFirstPage.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToFirstPage.LargeGlyph")));
            this.tlbGoToFirstPage.Name = "tlbGoToFirstPage";
            this.tlbGoToFirstPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbGoToFirstPage_ItemClick);
            // 
            // tlbGoToPreviousPage
            // 
            this.tlbGoToPreviousPage.Caption = "上一页";
            this.tlbGoToPreviousPage.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToPreviousPage.Glyph")));
            this.tlbGoToPreviousPage.Hint = "显示上一页数据";
            this.tlbGoToPreviousPage.Id = 24;
            this.tlbGoToPreviousPage.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToPreviousPage.LargeGlyph")));
            this.tlbGoToPreviousPage.Name = "tlbGoToPreviousPage";
            this.tlbGoToPreviousPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbGoToPreviousPage_ItemClick);
            // 
            // tlbSetCurrentPage
            // 
            this.tlbSetCurrentPage.Caption = "当前页:";
            this.tlbSetCurrentPage.Edit = this.CurrentPageText;
            this.tlbSetCurrentPage.Hint = "显示或设置当前显示页";
            this.tlbSetCurrentPage.Id = 26;
            this.tlbSetCurrentPage.Name = "tlbSetCurrentPage";
            this.tlbSetCurrentPage.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.tlbSetCurrentPage.EditValueChanged += new System.EventHandler(this.tlbSetCurrentPage_EditValueChanged);
            // 
            // CurrentPageText
            // 
            this.CurrentPageText.AutoHeight = false;
            this.CurrentPageText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.CurrentPageText.Mask.ShowPlaceHolders = false;
            this.CurrentPageText.Name = "CurrentPageText";
            // 
            // tlbGoToPage
            // 
            this.tlbGoToPage.Caption = "转到";
            this.tlbGoToPage.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToPage.Glyph")));
            this.tlbGoToPage.Hint = "跳转到所选页";
            this.tlbGoToPage.Id = 27;
            this.tlbGoToPage.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToPage.LargeGlyph")));
            this.tlbGoToPage.Name = "tlbGoToPage";
            this.tlbGoToPage.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.tlbGoToPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbGoToPage_ItemClick);
            // 
            // tlbSetPerPageNumber
            // 
            this.tlbSetPerPageNumber.Caption = "行数：";
            this.tlbSetPerPageNumber.Edit = this.repositoryItemComboBox2;
            this.tlbSetPerPageNumber.EditValue = 50;
            this.tlbSetPerPageNumber.Hint = "设置列表每页显示的行数";
            this.tlbSetPerPageNumber.Id = 30;
            this.tlbSetPerPageNumber.Name = "tlbSetPerPageNumber";
            this.tlbSetPerPageNumber.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.tlbSetPerPageNumber.EditValueChanged += new System.EventHandler(this.tlbSetPerPageNumber_EditValueChanged);
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "30",
            "50",
            "100",
            "200",
            "500",
            "1000",
            "5000",
            "10000",
            "50000"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // tlbGoToNextPage
            // 
            this.tlbGoToNextPage.Caption = "下一页";
            this.tlbGoToNextPage.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToNextPage.Glyph")));
            this.tlbGoToNextPage.Id = 28;
            this.tlbGoToNextPage.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToNextPage.LargeGlyph")));
            this.tlbGoToNextPage.Name = "tlbGoToNextPage";
            this.tlbGoToNextPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbGoToNextPage_ItemClick);
            // 
            // tlbGoToLastPage
            // 
            this.tlbGoToLastPage.Caption = "最后一页";
            this.tlbGoToLastPage.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToLastPage.Glyph")));
            this.tlbGoToLastPage.Hint = "显示最后一页数据";
            this.tlbGoToLastPage.Id = 29;
            this.tlbGoToLastPage.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbGoToLastPage.LargeGlyph")));
            this.tlbGoToLastPage.Name = "tlbGoToLastPage";
            this.tlbGoToLastPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbGoToLastPage_ItemClick);
            // 
            // barStaticItemMsg
            // 
            this.barStaticItemMsg.Caption = "执行时间:";
            this.barStaticItemMsg.Id = 60;
            this.barStaticItemMsg.Name = "barStaticItemMsg";
            this.barStaticItemMsg.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItemRowCount
            // 
            this.barStaticItemRowCount.Caption = "共(0)记录";
            this.barStaticItemRowCount.Id = 70;
            this.barStaticItemRowCount.Name = "barStaticItemRowCount";
            this.barStaticItemRowCount.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1078, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 495);
            this.barDockControlBottom.Size = new System.Drawing.Size(1078, 31);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 464);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1078, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 464);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = this.repositoryItemTextEdit1;
            this.barEditItem1.Id = 3;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 4;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 19;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barEditItem2
            // 
            this.barEditItem2.Caption = "barEditItem2";
            this.barEditItem2.Edit = this.txtContent;
            this.barEditItem2.Id = 20;
            this.barEditItem2.Name = "barEditItem2";
            // 
            // txtContent
            // 
            this.txtContent.AutoHeight = false;
            this.txtContent.Name = "txtContent";
            // 
            // barEditItem3
            // 
            this.barEditItem3.Caption = "/";
            this.barEditItem3.Edit = this.repositoryItemTextEdit7;
            this.barEditItem3.Id = 21;
            this.barEditItem3.Name = "barEditItem3";
            // 
            // repositoryItemTextEdit7
            // 
            this.repositoryItemTextEdit7.AutoHeight = false;
            this.repositoryItemTextEdit7.Name = "repositoryItemTextEdit7";
            // 
            // barEditItem4
            // 
            this.barEditItem4.Caption = "barEditItem4";
            this.barEditItem4.Edit = this.repositoryItemTextEdit8;
            this.barEditItem4.Id = 22;
            this.barEditItem4.Name = "barEditItem4";
            // 
            // repositoryItemTextEdit8
            // 
            this.repositoryItemTextEdit8.AutoHeight = false;
            this.repositoryItemTextEdit8.Name = "repositoryItemTextEdit8";
            // 
            // barEditItem5
            // 
            this.barEditItem5.Caption = "barEditItem5";
            this.barEditItem5.Edit = this.repositoryItemTextEdit9;
            this.barEditItem5.Id = 25;
            this.barEditItem5.Name = "barEditItem5";
            // 
            // repositoryItemTextEdit9
            // 
            this.repositoryItemTextEdit9.AutoHeight = false;
            this.repositoryItemTextEdit9.Name = "repositoryItemTextEdit9";
            // 
            // tlbChartSetting
            // 
            this.tlbChartSetting.Caption = "图表设置(C)";
            this.tlbChartSetting.Glyph = ((System.Drawing.Image)(resources.GetObject("tlbChartSetting.Glyph")));
            this.tlbChartSetting.Id = 64;
            this.tlbChartSetting.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("tlbChartSetting.LargeGlyph")));
            this.tlbChartSetting.Name = "tlbChartSetting";
            this.tlbChartSetting.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.tlbChartSetting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.tlbChartSetting_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "barButtonItem3";
            this.barButtonItem3.Id = 72;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // cboListStyle
            // 
            this.cboListStyle.AutoHeight = false;
            this.cboListStyle.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboListStyle.DropDownRows = 20;
            this.cboListStyle.Name = "cboListStyle";
            this.cboListStyle.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "30",
            "50",
            "100",
            "500",
            "1000",
            "0"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // repositoryItemTextEdit5
            // 
            this.repositoryItemTextEdit5.AutoHeight = false;
            this.repositoryItemTextEdit5.Mask.EditMask = "\\d+/\\d";
            this.repositoryItemTextEdit5.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.repositoryItemTextEdit5.Mask.PlaceHolder = '/';
            this.repositoryItemTextEdit5.Name = "repositoryItemTextEdit5";
            // 
            // repositoryItemTextEdit10
            // 
            this.repositoryItemTextEdit10.AutoHeight = false;
            this.repositoryItemTextEdit10.Name = "repositoryItemTextEdit10";
            // 
            // repositoryItemTextEdit11
            // 
            this.repositoryItemTextEdit11.AutoHeight = false;
            this.repositoryItemTextEdit11.Name = "repositoryItemTextEdit11";
            // 
            // repositoryItemTextEdit12
            // 
            this.repositoryItemTextEdit12.AutoHeight = false;
            this.repositoryItemTextEdit12.Name = "repositoryItemTextEdit12";
            // 
            // repositoryItemTextEdit13
            // 
            this.repositoryItemTextEdit13.AutoHeight = false;
            this.repositoryItemTextEdit13.Name = "repositoryItemTextEdit13";
            // 
            // btnContent
            // 
            this.btnContent.AutoHeight = false;
            this.btnContent.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnContent.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "清除", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnContent.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.btnContent.Name = "btnContent";
            // 
            // repositoryItemTextEdit6
            // 
            this.repositoryItemTextEdit6.AutoHeight = false;
            this.repositoryItemTextEdit6.Name = "repositoryItemTextEdit6";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // gridControl
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl.Location = new System.Drawing.Point(12, 117);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl.Size = new System.Drawing.Size(742, 248);
            this.gridControl.TabIndex = 4;
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
            this.gridView.CellMerge += new DevExpress.XtraGrid.Views.Grid.CellMergeEventHandler(this.gridView_CellMerge);
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView_RowCellStyle);
            this.gridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView_FocusedRowChanged);
            this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lblTile);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 31);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1078, 40);
            this.panelControl2.TabIndex = 10;
            this.panelControl2.Visible = false;
            // 
            // lblTile
            // 
            this.lblTile.BackColor = System.Drawing.Color.Transparent;
            this.lblTile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.lblTile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTile.Location = new System.Drawing.Point(2, 2);
            this.lblTile.Name = "lblTile";
            this.lblTile.Size = new System.Drawing.Size(1074, 35);
            this.lblTile.TabIndex = 1;
            this.lblTile.Text = "通用列表";
            this.lblTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTile.Visible = false;
            // 
            // buttonContent
            // 
            this.buttonContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContent.Location = new System.Drawing.Point(903, 3);
            this.buttonContent.Name = "buttonContent";
            this.buttonContent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("buttonContent.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.buttonContent.Size = new System.Drawing.Size(60, 22);
            this.buttonContent.TabIndex = 11;
            this.buttonContent.Visible = false;
            this.buttonContent.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonContent_ButtonClick);
            this.buttonContent.EditValueChanged += new System.EventHandler(this.buttonContent_EditValueChanged);
            this.buttonContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonContent_KeyDown);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.panelControl3);
            this.layoutControl1.Controls.Add(this.panelFreQry);
            this.layoutControl1.Controls.Add(this.panelControlPointFilter);
            this.layoutControl1.Controls.Add(this.panelControl1);
            this.layoutControl1.Controls.Add(this.documentViewer1);
            this.layoutControl1.Controls.Add(this.chartControl);
            this.layoutControl1.Controls.Add(this.gridControl);
            this.layoutControl1.Controls.Add(this.pivotGridControl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 71);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(604, 276, 443, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1078, 424);
            this.layoutControl1.TabIndex = 13;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.comboBoxEdit1);
            this.panelControl3.Controls.Add(this.labelControl2);
            this.panelControl3.Controls.Add(this.textEdit1);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.simpleButton6);
            this.panelControl3.Controls.Add(this.simpleButton8);
            this.panelControl3.Controls.Add(this.simpleButton7);
            this.panelControl3.Controls.Add(this.simpleButton9);
            this.panelControl3.Controls.Add(this.simpleButton5);
            this.panelControl3.Location = new System.Drawing.Point(12, 369);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1054, 43);
            this.panelControl3.TabIndex = 18;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.EditValue = "50";
            this.comboBoxEdit1.Location = new System.Drawing.Point(469, 7);
            this.comboBoxEdit1.MenuManager = this.barManager;
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "30",
            "50",
            "100",
            "200",
            "500",
            "1000",
            "5000",
            "10000",
            "50000"});
            this.comboBoxEdit1.Size = new System.Drawing.Size(71, 20);
            this.comboBoxEdit1.TabIndex = 21;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(300, 10);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 14);
            this.labelControl2.TabIndex = 20;
            this.labelControl2.Text = "共（0）条记录";
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(114, 8);
            this.textEdit1.MenuManager = this.barManager;
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(57, 20);
            this.textEdit1.TabIndex = 19;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(429, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "显示行";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(72, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "当前页";
            // 
            // simpleButton6
            // 
            this.simpleButton6.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton6.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton6.Image")));
            this.simpleButton6.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton6.Location = new System.Drawing.Point(175, 7);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(57, 22);
            this.simpleButton6.TabIndex = 17;
            this.simpleButton6.Text = "跳转";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton8.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton8.Image")));
            this.simpleButton8.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton8.Location = new System.Drawing.Point(266, 7);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(27, 22);
            this.simpleButton8.TabIndex = 17;
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton7.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton7.Image")));
            this.simpleButton7.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton7.Location = new System.Drawing.Point(237, 7);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(27, 22);
            this.simpleButton7.TabIndex = 17;
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // simpleButton9
            // 
            this.simpleButton9.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton9.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton9.Image")));
            this.simpleButton9.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton9.Location = new System.Drawing.Point(8, 7);
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Size = new System.Drawing.Size(27, 22);
            this.simpleButton9.TabIndex = 17;
            this.simpleButton9.Click += new System.EventHandler(this.simpleButton9_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton5.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.Image")));
            this.simpleButton5.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton5.Location = new System.Drawing.Point(38, 7);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(27, 22);
            this.simpleButton5.TabIndex = 17;
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // panelFreQry
            // 
            this.panelFreQry.Location = new System.Drawing.Point(15, 15);
            this.panelFreQry.Name = "panelFreQry";
            this.panelFreQry.Size = new System.Drawing.Size(666, 95);
            this.panelFreQry.TabIndex = 14;
            // 
            // panelControlPointFilter
            // 
            this.panelControlPointFilter.Controls.Add(this.lookUpEditArrangeActivity);
            this.panelControlPointFilter.Controls.Add(this.lookUpEditRemoveDuplication);
            this.panelControlPointFilter.Controls.Add(this.labelArrangePoint);
            this.panelControlPointFilter.Controls.Add(this.lookUpEditArrangeTime);
            this.panelControlPointFilter.Controls.Add(this.radioButtonArrangePoint);
            this.panelControlPointFilter.Controls.Add(this.radioButtonStorePoint);
            this.panelControlPointFilter.Controls.Add(this.radioButtonActivityPoint);
            this.panelControlPointFilter.Location = new System.Drawing.Point(685, 15);
            this.panelControlPointFilter.Name = "panelControlPointFilter";
            this.panelControlPointFilter.Size = new System.Drawing.Size(187, 95);
            this.panelControlPointFilter.TabIndex = 16;
            // 
            // lookUpEditArrangeActivity
            // 
            this.lookUpEditArrangeActivity.Enabled = false;
            this.lookUpEditArrangeActivity.Location = new System.Drawing.Point(87, 70);
            this.lookUpEditArrangeActivity.MenuManager = this.barManager;
            this.lookUpEditArrangeActivity.Name = "lookUpEditArrangeActivity";
            this.lookUpEditArrangeActivity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditArrangeActivity.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Text", "方式")});
            this.lookUpEditArrangeActivity.Properties.DisplayMember = "Text";
            this.lookUpEditArrangeActivity.Properties.ValueMember = "Id";
            this.lookUpEditArrangeActivity.Size = new System.Drawing.Size(134, 20);
            this.lookUpEditArrangeActivity.TabIndex = 8;
            // 
            // lookUpEditRemoveDuplication
            // 
            this.lookUpEditRemoveDuplication.Enabled = false;
            this.lookUpEditRemoveDuplication.Location = new System.Drawing.Point(87, 22);
            this.lookUpEditRemoveDuplication.MenuManager = this.barManager;
            this.lookUpEditRemoveDuplication.Name = "lookUpEditRemoveDuplication";
            this.lookUpEditRemoveDuplication.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditRemoveDuplication.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Num", "Num", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("QcfItem", "QcfItem")});
            this.lookUpEditRemoveDuplication.Properties.DisplayMember = "QcfItem";
            this.lookUpEditRemoveDuplication.Properties.ValueMember = "Num";
            this.lookUpEditRemoveDuplication.Size = new System.Drawing.Size(134, 20);
            this.lookUpEditRemoveDuplication.TabIndex = 6;
            // 
            // labelArrangePoint
            // 
            this.labelArrangePoint.AutoSize = true;
            this.labelArrangePoint.Location = new System.Drawing.Point(1010, 7);
            this.labelArrangePoint.Name = "labelArrangePoint";
            this.labelArrangePoint.Size = new System.Drawing.Size(0, 14);
            this.labelArrangePoint.TabIndex = 5;
            this.labelArrangePoint.Visible = false;
            // 
            // lookUpEditArrangeTime
            // 
            this.lookUpEditArrangeTime.EditValue = "";
            this.lookUpEditArrangeTime.Enabled = false;
            this.lookUpEditArrangeTime.Location = new System.Drawing.Point(87, 48);
            this.lookUpEditArrangeTime.MenuManager = this.barManager;
            this.lookUpEditArrangeTime.Name = "lookUpEditArrangeTime";
            this.lookUpEditArrangeTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditArrangeTime.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ListDataLayoutID", "ListDataLayoutID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("StrDate", "编排时间")});
            this.lookUpEditArrangeTime.Properties.DisplayMember = "StrDate";
            this.lookUpEditArrangeTime.Properties.ValueMember = "ListDataLayoutID";
            this.lookUpEditArrangeTime.Size = new System.Drawing.Size(134, 20);
            this.lookUpEditArrangeTime.TabIndex = 4;
            this.lookUpEditArrangeTime.EditValueChanged += new System.EventHandler(this.lookUpEditArrangeTime_EditValueChanged);
            // 
            // radioButtonArrangePoint
            // 
            this.radioButtonArrangePoint.AutoSize = true;
            this.radioButtonArrangePoint.Location = new System.Drawing.Point(5, 49);
            this.radioButtonArrangePoint.Name = "radioButtonArrangePoint";
            this.radioButtonArrangePoint.Size = new System.Drawing.Size(85, 18);
            this.radioButtonArrangePoint.TabIndex = 2;
            this.radioButtonArrangePoint.Text = "用户自定义";
            this.radioButtonArrangePoint.UseVisualStyleBackColor = true;
            this.radioButtonArrangePoint.CheckedChanged += new System.EventHandler(this.radioButtonArrangePoint_CheckedChanged);
            // 
            // radioButtonStorePoint
            // 
            this.radioButtonStorePoint.AutoSize = true;
            this.radioButtonStorePoint.Location = new System.Drawing.Point(5, 26);
            this.radioButtonStorePoint.Name = "radioButtonStorePoint";
            this.radioButtonStorePoint.Size = new System.Drawing.Size(73, 18);
            this.radioButtonStorePoint.TabIndex = 1;
            this.radioButtonStorePoint.Text = "所有设备";
            this.radioButtonStorePoint.UseVisualStyleBackColor = true;
            this.radioButtonStorePoint.CheckedChanged += new System.EventHandler(this.radioButtonStorePoint_CheckedChanged);
            // 
            // radioButtonActivityPoint
            // 
            this.radioButtonActivityPoint.AutoSize = true;
            this.radioButtonActivityPoint.Checked = true;
            this.radioButtonActivityPoint.Location = new System.Drawing.Point(5, 4);
            this.radioButtonActivityPoint.Name = "radioButtonActivityPoint";
            this.radioButtonActivityPoint.Size = new System.Drawing.Size(97, 18);
            this.radioButtonActivityPoint.TabIndex = 0;
            this.radioButtonActivityPoint.TabStop = true;
            this.radioButtonActivityPoint.Text = "当前定义设备";
            this.radioButtonActivityPoint.UseVisualStyleBackColor = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.tlbRefresh1);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.simpleButton10);
            this.panelControl1.Controls.Add(this.simpleButton4);
            this.panelControl1.Controls.Add(this.simpleButton12);
            this.panelControl1.Controls.Add(this.simpleButton11);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Location = new System.Drawing.Point(876, 15);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(187, 95);
            this.panelControl1.TabIndex = 17;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(164, 5);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(60, 25);
            this.simpleButton2.TabIndex = 17;
            this.simpleButton2.Text = "向后";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // tlbRefresh1
            // 
            this.tlbRefresh1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.tlbRefresh1.Appearance.Options.UseFont = true;
            this.tlbRefresh1.Image = ((System.Drawing.Image)(resources.GetObject("tlbRefresh1.Image")));
            this.tlbRefresh1.Location = new System.Drawing.Point(9, 5);
            this.tlbRefresh1.Name = "tlbRefresh1";
            this.tlbRefresh1.Size = new System.Drawing.Size(60, 25);
            this.tlbRefresh1.TabIndex = 17;
            this.tlbRefresh1.Text = "查询";
            this.tlbRefresh1.Click += new System.EventHandler(this.tlbRefresh1_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(85, 5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(60, 25);
            this.simpleButton1.TabIndex = 17;
            this.simpleButton1.Text = "向前";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton10.Image")));
            this.simpleButton10.Location = new System.Drawing.Point(164, 34);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(60, 25);
            this.simpleButton10.TabIndex = 17;
            this.simpleButton10.Text = "打印";
            this.simpleButton10.Click += new System.EventHandler(this.simpleButton10_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(85, 34);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(60, 25);
            this.simpleButton4.TabIndex = 17;
            this.simpleButton4.Text = "预览";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton12
            // 
            this.simpleButton12.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton12.Image")));
            this.simpleButton12.Location = new System.Drawing.Point(93, 64);
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.Size = new System.Drawing.Size(78, 25);
            this.simpleButton12.TabIndex = 17;
            this.simpleButton12.Text = "测点编排";
            this.simpleButton12.Click += new System.EventHandler(this.simpleButton12_Click);
            // 
            // simpleButton11
            // 
            this.simpleButton11.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton11.Image")));
            this.simpleButton11.Location = new System.Drawing.Point(9, 64);
            this.simpleButton11.Name = "simpleButton11";
            this.simpleButton11.Size = new System.Drawing.Size(78, 25);
            this.simpleButton11.TabIndex = 17;
            this.simpleButton11.Text = "录入备注";
            this.simpleButton11.Click += new System.EventHandler(this.simpleButton11_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(9, 34);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(60, 25);
            this.simpleButton3.TabIndex = 17;
            this.simpleButton3.Text = "导出";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // documentViewer1
            // 
            this.documentViewer1.IsMetric = true;
            this.documentViewer1.Location = new System.Drawing.Point(758, 117);
            this.documentViewer1.Name = "documentViewer1";
            this.documentViewer1.Size = new System.Drawing.Size(100, 248);
            this.documentViewer1.TabIndex = 15;
            // 
            // chartControl
            // 
            this.chartControl.Location = new System.Drawing.Point(966, 117);
            this.chartControl.Name = "chartControl";
            this.chartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl.Size = new System.Drawing.Size(100, 248);
            this.chartControl.TabIndex = 13;
            // 
            // pivotGridControl
            // 
            this.pivotGridControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.pivotGridControl.Location = new System.Drawing.Point(862, 117);
            this.pivotGridControl.Name = "pivotGridControl";
            this.pivotGridControl.Size = new System.Drawing.Size(100, 248);
            this.pivotGridControl.TabIndex = 12;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGrid,
            this.layoutItemChart,
            this.layoutItemPivotGrid,
            this.layoutItemReport,
            this.layoutControlItem4,
            this.layoutControlGroupFreQry});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(1078, 424);
            this.layoutControlGroup1.Text = "Root";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutItemGrid
            // 
            this.layoutItemGrid.Control = this.gridControl;
            this.layoutItemGrid.CustomizationFormText = "GridList";
            this.layoutItemGrid.Location = new System.Drawing.Point(0, 105);
            this.layoutItemGrid.Name = "layoutItemGrid";
            this.layoutItemGrid.Size = new System.Drawing.Size(746, 252);
            this.layoutItemGrid.Text = "GridList";
            this.layoutItemGrid.TextSize = new System.Drawing.Size(0, 0);
            this.layoutItemGrid.TextToControlDistance = 0;
            this.layoutItemGrid.TextVisible = false;
            // 
            // layoutItemChart
            // 
            this.layoutItemChart.Control = this.chartControl;
            this.layoutItemChart.CustomizationFormText = "layoutItemChart";
            this.layoutItemChart.Location = new System.Drawing.Point(954, 105);
            this.layoutItemChart.Name = "layoutItemChart";
            this.layoutItemChart.Size = new System.Drawing.Size(104, 252);
            this.layoutItemChart.Text = "layoutItemChart";
            this.layoutItemChart.TextSize = new System.Drawing.Size(0, 0);
            this.layoutItemChart.TextToControlDistance = 0;
            this.layoutItemChart.TextVisible = false;
            this.layoutItemChart.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutItemPivotGrid
            // 
            this.layoutItemPivotGrid.Control = this.pivotGridControl;
            this.layoutItemPivotGrid.CustomizationFormText = "PivotGridList";
            this.layoutItemPivotGrid.Location = new System.Drawing.Point(850, 105);
            this.layoutItemPivotGrid.Name = "layoutItemPivotGrid";
            this.layoutItemPivotGrid.Size = new System.Drawing.Size(104, 252);
            this.layoutItemPivotGrid.Text = "PivotGridList";
            this.layoutItemPivotGrid.TextSize = new System.Drawing.Size(0, 0);
            this.layoutItemPivotGrid.TextToControlDistance = 0;
            this.layoutItemPivotGrid.TextVisible = false;
            this.layoutItemPivotGrid.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutItemReport
            // 
            this.layoutItemReport.Control = this.documentViewer1;
            this.layoutItemReport.CustomizationFormText = "layoutItemReport";
            this.layoutItemReport.Location = new System.Drawing.Point(746, 105);
            this.layoutItemReport.Name = "layoutItemReport";
            this.layoutItemReport.Size = new System.Drawing.Size(104, 252);
            this.layoutItemReport.Text = "layoutItemReport";
            this.layoutItemReport.TextSize = new System.Drawing.Size(0, 0);
            this.layoutItemReport.TextToControlDistance = 0;
            this.layoutItemReport.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.panelControl3;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 357);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1058, 47);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlGroupFreQry
            // 
            this.layoutControlGroupFreQry.CustomizationFormText = "常用查询条件";
            this.layoutControlGroupFreQry.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItemPointFilter});
            this.layoutControlGroupFreQry.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupFreQry.Name = "layoutControlGroupFreQry";
            this.layoutControlGroupFreQry.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroupFreQry.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupFreQry.Size = new System.Drawing.Size(1058, 105);
            this.layoutControlGroupFreQry.Text = "常用查询条件";
            this.layoutControlGroupFreQry.TextVisible = false;
            this.layoutControlGroupFreQry.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.panelFreQry;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(670, 99);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.panelControl1;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(861, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(191, 99);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItemPointFilter
            // 
            this.layoutControlItemPointFilter.Control = this.panelControlPointFilter;
            this.layoutControlItemPointFilter.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItemPointFilter.Location = new System.Drawing.Point(670, 0);
            this.layoutControlItemPointFilter.Name = "layoutControlItem1";
            this.layoutControlItemPointFilter.Size = new System.Drawing.Size(191, 99);
            this.layoutControlItemPointFilter.Text = "layoutControlItem1";
            this.layoutControlItemPointFilter.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemPointFilter.TextToControlDistance = 0;
            this.layoutControlItemPointFilter.TextVisible = false;
            // 
            // lookupShowStyle
            // 
            this.lookupShowStyle.Location = new System.Drawing.Point(968, 4);
            this.lookupShowStyle.Name = "lookupShowStyle";
            this.lookupShowStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupShowStyle.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "编码", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "名称")});
            this.lookupShowStyle.Properties.DisplayMember = "Name";
            this.lookupShowStyle.Properties.NullText = "";
            this.lookupShowStyle.Properties.ValueMember = "Code";
            this.lookupShowStyle.Size = new System.Drawing.Size(47, 20);
            this.lookupShowStyle.TabIndex = 15;
            this.lookupShowStyle.Visible = false;
            this.lookupShowStyle.EditValueChanged += new System.EventHandler(this.lookupShowStyle_EditValueChanged);
            // 
            // lookupSchema
            // 
            this.lookupSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lookupSchema.Location = new System.Drawing.Point(1022, 4);
            this.lookupSchema.Name = "lookupSchema";
            this.lookupSchema.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupSchema.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ListDataID", "ListDataID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("strListDataName", "名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("blnDefault", "默认")});
            this.lookupSchema.Properties.DisplayMember = "strListDataName";
            this.lookupSchema.Properties.NullText = "";
            this.lookupSchema.Properties.PopupWidth = 40;
            this.lookupSchema.Properties.ValueMember = "ListDataID";
            this.lookupSchema.Size = new System.Drawing.Size(54, 20);
            this.lookupSchema.TabIndex = 14;
            this.lookupSchema.Visible = false;
            this.lookupSchema.EditValueChanged += new System.EventHandler(this.lookupSchema_EditValueChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.panelControlPointFilter;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 297);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(290, 87);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // frmList
            // 
            this.ClientSize = new System.Drawing.Size(1078, 526);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.lookupSchema);
            this.Controls.Add(this.lookupShowStyle);
            this.Controls.Add(this.buttonContent);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "通用列表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmListEx_FormClosing);
            this.Load += new System.EventHandler(this.frmList_Load);
            this.SizeChanged += new System.EventHandler(this.frmListEx_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPageText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboListStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlPointFilter)).EndInit();
            this.panelControlPointFilter.ResumeLayout(false);
            this.panelControlPointFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditArrangeActivity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditRemoveDuplication.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditArrangeTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemPivotGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutItemReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFreQry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPointFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupShowStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupSchema.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar ToolBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraBars.BarButtonItem tlbRefresh;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboListStyle;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarCheckItem tlbMultiSelection;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit5;
        private DevExpress.XtraBars.BarEditItem barEditItem3;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtContent;
        private DevExpress.XtraBars.BarEditItem barEditItem4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit8;
        private DevExpress.XtraBars.Bar ToolBarPage;
        private DevExpress.XtraBars.BarButtonItem tlbGoToFirstPage;
        private DevExpress.XtraBars.BarButtonItem tlbGoToPreviousPage;
        private DevExpress.XtraBars.BarEditItem tlbSetCurrentPage;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CurrentPageText;
        private DevExpress.XtraBars.BarButtonItem tlbGoToPage;
        private DevExpress.XtraBars.BarButtonItem tlbGoToNextPage;
        private DevExpress.XtraBars.BarButtonItem tlbGoToLastPage;
        private DevExpress.XtraBars.BarEditItem tlbSetPerPageNumber;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.BarEditItem barEditItem5;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit9;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private Label lblTile;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit10;
        private DevExpress.XtraBars.BarSubItem tlbDesign;
        private DevExpress.XtraBars.BarSubItem menuListStyle;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit11;
        private DevExpress.XtraBars.BarButtonItem tlbPrint;
        private DevExpress.XtraPrinting.PrintingSystem ps;
        private DevExpress.XtraBars.BarButtonItem tlbAdvancedSearch;
        private DevExpress.XtraBars.BarCheckItem tlbSimpleSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit12;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit13;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnContent;
        private DevExpress.XtraEditors.ButtonEdit buttonContent;
        private DevExpress.XtraBars.BarButtonItem tlbSchema;
        private DevExpress.XtraBars.BarStaticItem barStaticItemMsg;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemGrid;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemPivotGrid;
        private DevExpress.XtraBars.BarButtonItem tlbPivotSetting;
        private DevExpress.XtraBars.BarButtonItem tlbChartSetting;
        private DevExpress.XtraCharts.ChartControl chartControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemChart;
        private DevExpress.XtraEditors.LookUpEdit lookupSchema;
        private DevExpress.XtraBars.BarCheckItem chkItemAllowCopy;
        private DevExpress.XtraEditors.LookUpEdit lookupShowStyle;
        private DevExpress.XtraBars.BarCheckItem chkItemFreQryCondition;
        private Panel panelFreQry;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupFreQry;
        private DevExpress.XtraBars.BarButtonItem tlbSaveFreQryCon;
        private DevExpress.XtraBars.BarButtonItem tlbSetDefaultStyle;
        private DevExpress.XtraBars.BarStaticItem barStaticItemRowCount;
        private DevExpress.XtraBars.BarCheckItem tlbFilterAutoSelect;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarSubItem tlbGroupList;
        private DevExpress.XtraBars.BarSubItem barSubItemExport;
        private DevExpress.XtraBars.BarButtonItem tlbExportExcel;
        private DevExpress.XtraBars.BarButtonItem tlbExeclPDF;
        private DevExpress.XtraBars.BarButtonItem txtExportTXT;
        private DevExpress.XtraBars.BarButtonItem txtExportCSV;
        private DevExpress.XtraBars.BarButtonItem tlbSaveWidth;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit6;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraPrinting.Preview.DocumentViewer documentViewer1;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemReport;
        private DevExpress.XtraBars.BarButtonItem txtExportHTML;
        private DevExpress.XtraBars.BarButtonItem tlbSet;
        private DevExpress.XtraBars.BarButtonItem tlbDescInput;
        private DevExpress.XtraBars.BarCheckItem chkblnKeyWord;
        private DevExpress.XtraBars.BarButtonItem tlbColumnSortSet;
        private DevExpress.XtraEditors.PanelControl panelControlPointFilter;
        private RadioButton radioButtonArrangePoint;
        private RadioButton radioButtonStorePoint;
        private RadioButton radioButtonActivityPoint;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditArrangeTime;
        private Label labelArrangePoint;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditRemoveDuplication;
        private DevExpress.XtraBars.BarButtonItem BeforeDay;
        private DevExpress.XtraBars.BarButtonItem AfterDay;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditArrangeActivity;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExportExcelAll;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPointFilter;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton tlbRefresh1;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton simpleButton11;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        //测试一下



    }
}
