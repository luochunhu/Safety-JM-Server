namespace Sys.Safety.Client.Chart
{
    partial class Frm_KglLineWithScreen
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.XYDiagramPane xyDiagramPane1 = new DevExpress.XtraCharts.XYDiagramPane();
            DevExpress.XtraCharts.XYDiagramPane xyDiagramPane2 = new DevExpress.XtraCharts.XYDiagramPane();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY2 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.StepLineSeriesView stepLineSeriesView1 = new DevExpress.XtraCharts.StepLineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.StepLineSeriesView stepLineSeriesView2 = new DevExpress.XtraCharts.StepLineSeriesView();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.StepLineSeriesView stepLineSeriesView3 = new DevExpress.XtraCharts.StepLineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.chart = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView3)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 1;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(909, 27);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // chart
            // 
            this.chart.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.DeepSkyBlue;
            this.chart.CrosshairOptions.ArgumentLineStyle.Thickness = 2;
            this.chart.CrosshairOptions.GroupHeaderPattern = "{A:yyyy-MM-dd HH:mm:ss}";
            this.chart.CrosshairOptions.ShowOnlyInFocusedPane = false;
            xyDiagram1.AxisX.DateTimeScaleOptions.AggregateFunction = DevExpress.XtraCharts.AggregateFunction.Maximum;
            xyDiagram1.AxisX.DateTimeScaleOptions.AutoGrid = false;
            xyDiagram1.AxisX.DateTimeScaleOptions.GridSpacing = 31D;
            xyDiagram1.AxisX.DateTimeScaleOptions.ScaleMode = DevExpress.XtraCharts.ScaleMode.Automatic;
            xyDiagram1.AxisX.Label.DateTimeOptions.AutoFormat = false;
            xyDiagram1.AxisX.Label.DateTimeOptions.Format = DevExpress.XtraCharts.DateTimeFormat.Custom;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "1";
            xyDiagram1.AxisY.Label.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            xyDiagram1.AxisY.Label.NumericOptions.Precision = 0;
            xyDiagram1.AxisY.NumericScaleOptions.AutoGrid = false;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisualRange.Auto = false;
            xyDiagram1.AxisY.VisualRange.AutoSideMargins = false;
            xyDiagram1.AxisY.VisualRange.MaxValueSerializable = "2";
            xyDiagram1.AxisY.VisualRange.MinValueSerializable = "0";
            xyDiagram1.AxisY.VisualRange.SideMarginsValue = 0D;
            xyDiagram1.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            xyDiagram1.AxisY.WholeRange.Auto = false;
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram1.AxisY.WholeRange.MaxValueSerializable = "2";
            xyDiagram1.AxisY.WholeRange.MinValueSerializable = "0";
            xyDiagram1.AxisY.WholeRange.SideMarginsValue = 0D;
            xyDiagram1.DefaultPane.ScrollBarOptions.XAxisScrollBarVisible = false;
            xyDiagram1.DefaultPane.ScrollBarOptions.YAxisScrollBarVisible = false;
            xyDiagram1.DefaultPane.Weight = 3D;
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.EnableAxisXZooming = true;
            xyDiagram1.PaneDistance = 4;
            xyDiagramPane1.Name = "Pane 1";
            xyDiagramPane1.PaneID = 0;
            xyDiagramPane1.ScrollBarOptions.XAxisScrollBarVisible = false;
            xyDiagramPane1.ScrollBarOptions.YAxisScrollBarVisible = false;
            xyDiagramPane1.Weight = 3D;
            xyDiagramPane2.Name = "Pane 2";
            xyDiagramPane2.PaneID = 1;
            xyDiagramPane2.ScrollBarOptions.YAxisScrollBarVisible = false;
            xyDiagramPane2.Weight = 3D;
            xyDiagram1.Panes.AddRange(new DevExpress.XtraCharts.XYDiagramPane[] {
            xyDiagramPane1,
            xyDiagramPane2});
            secondaryAxisY1.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.GridLines.MinorVisible = true;
            secondaryAxisY1.GridLines.Visible = true;
            secondaryAxisY1.Label.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            secondaryAxisY1.Label.NumericOptions.Precision = 0;
            secondaryAxisY1.Name = "secondaryAxisY1";
            secondaryAxisY1.NumericScaleOptions.AutoGrid = false;
            secondaryAxisY1.Title.Font = new System.Drawing.Font("Tahoma", 10F);
            secondaryAxisY1.Title.Text = "";
            secondaryAxisY1.Title.Visible = true;
            secondaryAxisY1.VisibleInPanesSerializable = "0";
            secondaryAxisY1.VisualRange.Auto = false;
            secondaryAxisY1.VisualRange.AutoSideMargins = false;
            secondaryAxisY1.VisualRange.MaxValueSerializable = "2";
            secondaryAxisY1.VisualRange.MinValueSerializable = "0";
            secondaryAxisY1.VisualRange.SideMarginsValue = 0D;
            secondaryAxisY1.WholeRange.Auto = false;
            secondaryAxisY1.WholeRange.AutoSideMargins = false;
            secondaryAxisY1.WholeRange.MaxValueSerializable = "2";
            secondaryAxisY1.WholeRange.MinValueSerializable = "0";
            secondaryAxisY1.WholeRange.SideMarginsValue = 0D;
            secondaryAxisY2.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisY2.AxisID = 1;
            secondaryAxisY2.GridLines.MinorVisible = true;
            secondaryAxisY2.GridLines.Visible = true;
            secondaryAxisY2.Label.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            secondaryAxisY2.Label.NumericOptions.Precision = 0;
            secondaryAxisY2.Name = "secondaryAxisY2";
            secondaryAxisY2.NumericScaleOptions.AutoGrid = false;
            secondaryAxisY2.Title.Font = new System.Drawing.Font("Tahoma", 10F);
            secondaryAxisY2.Title.Text = "";
            secondaryAxisY2.Title.Visible = true;
            secondaryAxisY2.VisibleInPanesSerializable = "1";
            secondaryAxisY2.VisualRange.Auto = false;
            secondaryAxisY2.VisualRange.AutoSideMargins = false;
            secondaryAxisY2.VisualRange.MaxValueSerializable = "2";
            secondaryAxisY2.VisualRange.MinValueSerializable = "0";
            secondaryAxisY2.VisualRange.SideMarginsValue = 0D;
            secondaryAxisY2.WholeRange.Auto = false;
            secondaryAxisY2.WholeRange.AutoSideMargins = false;
            secondaryAxisY2.WholeRange.MaxValueSerializable = "2";
            secondaryAxisY2.WholeRange.MinValueSerializable = "0";
            secondaryAxisY2.WholeRange.SideMarginsValue = 0D;
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1,
            secondaryAxisY2});
            this.chart.Diagram = xyDiagram1;
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.chart.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.chart.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chart.Location = new System.Drawing.Point(0, 27);
            this.chart.Name = "chart";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            pointSeriesLabel1.Angle = 90;
            series1.Label = pointSeriesLabel1;
            series1.Name = "Temperature (F)";
            series1.View = stepLineSeriesView1;
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            pointSeriesLabel2.Angle = 90;
            series2.Label = pointSeriesLabel2;
            series2.Name = "系列1";
            stepLineSeriesView2.AxisYName = "secondaryAxisY1";
            stepLineSeriesView2.PaneName = "Pane 1";
            series2.View = stepLineSeriesView2;
            series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            pointSeriesLabel3.Angle = 90;
            series3.Label = pointSeriesLabel3;
            series3.Name = "系列2";
            stepLineSeriesView3.AxisYName = "secondaryAxisY2";
            stepLineSeriesView3.PaneName = "Pane 2";
            series3.View = stepLineSeriesView3;
            this.chart.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
            this.chart.Size = new System.Drawing.Size(909, 478);
            this.chart.TabIndex = 4;
            chartTitle1.Text = "开关量多点同屏";
            this.chart.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.chart.ToolTipOptions.ShowForPoints = false;
            // 
            // Frm_KglLineWithScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 505);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "Frm_KglLineWithScreen";
            this.Ribbon = this.ribbonControl1;
            this.Text = "开关量曲线（多点同屏）";
            this.Load += new System.EventHandler(this.Frm_KglLineWithScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraCharts.ChartControl chart;
    }
}