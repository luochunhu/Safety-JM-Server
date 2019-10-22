namespace Sys.Safety.Client.Graphic
{
    partial class AnalysisResultsInRealtime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisResultsInRealtime));
            this.childGridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AnalysisModelName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ExpressionText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FirstSuccessfulTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LastAnalysisTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AnalysisResultText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ActualContinueTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControlData = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.childGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // childGridView1
            // 
            this.childGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AnalysisModelName,
            this.ExpressionText,
            this.FirstSuccessfulTime,
            this.LastAnalysisTime,
            this.AnalysisResultText,
            this.ActualContinueTime,
            this.gridColumn3});
            this.childGridView1.GridControl = this.gridControlData;
            this.childGridView1.Name = "childGridView1";
            this.childGridView1.OptionsView.ShowGroupPanel = false;
            this.childGridView1.ViewCaption = "表达式实时分析列表";
            // 
            // AnalysisModelName
            // 
            this.AnalysisModelName.Caption = "分析模型名称";
            this.AnalysisModelName.FieldName = "AnalysisModelName";
            this.AnalysisModelName.Name = "AnalysisModelName";
            this.AnalysisModelName.OptionsColumn.ReadOnly = true;
            // 
            // ExpressionText
            // 
            this.ExpressionText.Caption = "表达式";
            this.ExpressionText.FieldName = "ExpressionText";
            this.ExpressionText.Name = "ExpressionText";
            this.ExpressionText.OptionsColumn.ReadOnly = true;
            this.ExpressionText.Visible = true;
            this.ExpressionText.VisibleIndex = 0;
            // 
            // FirstSuccessfulTime
            // 
            this.FirstSuccessfulTime.Caption = "分析成立开始时间";
            this.FirstSuccessfulTime.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.FirstSuccessfulTime.FieldName = "ShowFirstSuccessfulTime";
            this.FirstSuccessfulTime.Name = "FirstSuccessfulTime";
            this.FirstSuccessfulTime.OptionsColumn.AllowEdit = false;
            this.FirstSuccessfulTime.OptionsColumn.ReadOnly = true;
            this.FirstSuccessfulTime.Visible = true;
            this.FirstSuccessfulTime.VisibleIndex = 1;
            // 
            // LastAnalysisTime
            // 
            this.LastAnalysisTime.Caption = "最后分析时间";
            this.LastAnalysisTime.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.LastAnalysisTime.FieldName = "ShowLastAnalysisTime";
            this.LastAnalysisTime.Name = "LastAnalysisTime";
            this.LastAnalysisTime.OptionsColumn.AllowEdit = false;
            this.LastAnalysisTime.OptionsColumn.ReadOnly = true;
            this.LastAnalysisTime.Visible = true;
            this.LastAnalysisTime.VisibleIndex = 2;
            // 
            // AnalysisResultText
            // 
            this.AnalysisResultText.Caption = "分析结果";
            this.AnalysisResultText.FieldName = "AnalysisResultText";
            this.AnalysisResultText.Name = "AnalysisResultText";
            this.AnalysisResultText.OptionsColumn.ReadOnly = true;
            this.AnalysisResultText.Visible = true;
            this.AnalysisResultText.VisibleIndex = 3;
            // 
            // ActualContinueTime
            // 
            this.ActualContinueTime.Caption = "分析成立实际持续时间 ";
            this.ActualContinueTime.FieldName = "ShowActualContinueTime";
            this.ActualContinueTime.Name = "ActualContinueTime";
            this.ActualContinueTime.OptionsColumn.ReadOnly = true;
            this.ActualContinueTime.Visible = true;
            this.ActualContinueTime.VisibleIndex = 4;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "gridColumn3";
            this.gridColumn3.FieldName = "ExpressionId";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridControlData
            // 
            this.gridControlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            gridLevelNode1.LevelTemplate = this.childGridView1;
            gridLevelNode1.RelationName = "ExpressionRealTimeResultList";
            this.gridControlData.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControlData.Location = new System.Drawing.Point(0, 0);
            this.gridControlData.MainView = this.gridView1;
            this.gridControlData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlData.Name = "gridControlData";
            this.gridControlData.Size = new System.Drawing.Size(896, 529);
            this.gridControlData.TabIndex = 0;
            this.gridControlData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.childGridView1});
            // 
            // gridView1
            // 
            this.gridView1.ChildGridLevelName = "childGridView1";
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn4,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControlData;
            this.gridView1.GroupPanelText = "数据分析实时列表";
            this.gridView1.IndicatorWidth = 35;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "大数据模型名称";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "最后一次分析时间";
            this.gridColumn5.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.gridColumn5.FieldName = "AnalysisTime";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "分析结果";
            this.gridColumn4.FieldName = "AnalysisResultText";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn2";
            this.gridColumn2.FieldName = "ID";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // AnalysisResultsInRealtime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 529);
            this.Controls.Add(this.gridControlData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnalysisResultsInRealtime";
            this.Text = "分析模型实时列表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnalysisResultsInRealtime_FormClosing);
            this.Load += new System.EventHandler(this.AnalysisResultsInRealtime_Load);
            ((System.ComponentModel.ISupportInitialize)(this.childGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView childGridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn AnalysisModelName;
        private DevExpress.XtraGrid.Columns.GridColumn ExpressionText;
        private DevExpress.XtraGrid.Columns.GridColumn FirstSuccessfulTime;
        private DevExpress.XtraGrid.Columns.GridColumn LastAnalysisTime;
        private DevExpress.XtraGrid.Columns.GridColumn AnalysisResultText;
        private DevExpress.XtraGrid.Columns.GridColumn ActualContinueTime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}