namespace Sys.Safety.Client.Alarm
{
    partial class frmPopupAlert
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
            this.mainGrid = new DevExpress.XtraGrid.GridControl();
            this.mainGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTimer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStateDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSsz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColour = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AlarmColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnClear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainGrid
            // 
            this.mainGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mainGrid.Location = new System.Drawing.Point(0, 0);
            this.mainGrid.MainView = this.mainGridView;
            this.mainGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.Size = new System.Drawing.Size(597, 240);
            this.mainGrid.TabIndex = 3;
            this.mainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainGridView});
            this.mainGrid.DoubleClick += new System.EventHandler(this.mainGrid_DoubleClick);
            // 
            // mainGridView
            // 
            this.mainGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTimer,
            this.colPoint,
            this.colWz,
            this.colTypeDisplay,
            this.colStateDisplay,
            this.colSsz,
            this.colColour,
            this.AlarmColor});
            this.mainGridView.GridControl = this.mainGrid;
            this.mainGridView.IndicatorWidth = 30;
            this.mainGridView.Name = "mainGridView";
            this.mainGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView.OptionsBehavior.Editable = false;
            this.mainGridView.OptionsFind.FindDelay = 100;
            this.mainGridView.OptionsMenu.EnableColumnMenu = false;
            this.mainGridView.OptionsView.ShowGroupPanel = false;
            this.mainGridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            this.mainGridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.mainGridView_CustomDrawRowIndicator);
            this.mainGridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.mainGridView_CustomDrawCell);
            // 
            // colTimer
            // 
            this.colTimer.Caption = "开始时间";
            this.colTimer.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colTimer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTimer.FieldName = "Timer";
            this.colTimer.Name = "colTimer";
            this.colTimer.Visible = true;
            this.colTimer.VisibleIndex = 0;
            this.colTimer.Width = 147;
            // 
            // colPoint
            // 
            this.colPoint.Caption = "编号";
            this.colPoint.FieldName = "Point";
            this.colPoint.Name = "colPoint";
            this.colPoint.Visible = true;
            this.colPoint.VisibleIndex = 1;
            this.colPoint.Width = 61;
            // 
            // colWz
            // 
            this.colWz.Caption = "名称";
            this.colWz.FieldName = "Wz";
            this.colWz.Name = "colWz";
            this.colWz.Visible = true;
            this.colWz.VisibleIndex = 2;
            this.colWz.Width = 111;
            // 
            // colTypeDisplay
            // 
            this.colTypeDisplay.Caption = "状态";
            this.colTypeDisplay.FieldName = "TypeDisplay";
            this.colTypeDisplay.Name = "colTypeDisplay";
            this.colTypeDisplay.Visible = true;
            this.colTypeDisplay.VisibleIndex = 3;
            this.colTypeDisplay.Width = 100;
            // 
            // colStateDisplay
            // 
            this.colStateDisplay.Caption = "设备状态";
            this.colStateDisplay.FieldName = "StateDisplay";
            this.colStateDisplay.Name = "colStateDisplay";
            this.colStateDisplay.Width = 56;
            // 
            // colSsz
            // 
            this.colSsz.Caption = "实时值";
            this.colSsz.FieldName = "Ssz";
            this.colSsz.Name = "colSsz";
            this.colSsz.Visible = true;
            this.colSsz.VisibleIndex = 4;
            this.colSsz.Width = 146;
            // 
            // colColour
            // 
            this.colColour.Caption = "颜色";
            this.colColour.FieldName = "Unit";
            this.colColour.Name = "colColour";
            // 
            // AlarmColor
            // 
            this.AlarmColor.Caption = "AlarmColor";
            this.AlarmColor.FieldName = "AlarmColor";
            this.AlarmColor.Name = "AlarmColor";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(511, 246);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 25);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 246);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "报警处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl1.Location = new System.Drawing.Point(7, 249);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(368, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "提示：传感器未标校，传感器超期服役, 逻辑分析报警双击可查看详细";
            this.labelControl1.Visible = false;
            // 
            // frmPopupAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 275);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.mainGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPopupAlert";
            this.ShowIcon = false;
            this.Text = "弹窗提示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XtraForm1_FormClosing);
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl mainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView mainGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colTimer;
        private DevExpress.XtraGrid.Columns.GridColumn colPoint;
        private DevExpress.XtraGrid.Columns.GridColumn colWz;
        private DevExpress.XtraGrid.Columns.GridColumn colTypeDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colStateDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn colSsz;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraGrid.Columns.GridColumn colColour;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn AlarmColor;
    }
}