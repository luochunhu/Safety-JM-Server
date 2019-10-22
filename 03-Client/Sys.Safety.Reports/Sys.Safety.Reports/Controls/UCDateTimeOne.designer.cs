namespace Sys.Safety.Reports.Controls
{
    partial class UCDateTimeOne
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.datFrom = new DevExpress.XtraEditors.DateEdit();
            this.cboDate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.datTo = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomizationMenu = false;
            this.layoutControl1.AutoScroll = false;
            this.layoutControl1.Controls.Add(this.datFrom);
            this.layoutControl1.Controls.Add(this.cboDate);
            this.layoutControl1.Controls.Add(this.datTo);
            this.layoutControl1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(400, 27);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // datFrom
            // 
            this.datFrom.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.datFrom.EditValue = null;
            this.datFrom.Location = new System.Drawing.Point(161, 3);
            this.datFrom.Name = "datFrom";
            this.datFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.datFrom.Size = new System.Drawing.Size(105, 20);
            this.datFrom.StyleController = this.layoutControl1;
            this.datFrom.TabIndex = 5;
            this.datFrom.EditValueChanged += new System.EventHandler(this.datFrom_EditValueChanged);
            // 
            // cboDate
            // 
            this.cboDate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cboDate.EditValue = "";
            this.cboDate.Location = new System.Drawing.Point(56, 3);
            this.cboDate.Name = "cboDate";
            this.cboDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDate.Properties.DropDownRows = 14;
            this.cboDate.Properties.Items.AddRange(new object[] {
            "",
            "等于",
            "大于",
            "小于",
            "不等于",
            "大于等于",
            "小于等于",
            "介于",
            "今天",
            "本周",
            "本周至今日",
            "本月",
            "本月至今日",
            "本季度",
            "本季度至今日",
            "本年",
            "本年至今日",
            "上周",
            "上月",
            "上季度",
            "上年",
            "最近一周",
            "最近一月",
            "最近一季度",
            "最近一年"});
            this.cboDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboDate.Size = new System.Drawing.Size(81, 20);
            this.cboDate.StyleController = this.layoutControl1;
            this.cboDate.TabIndex = 3;
            this.cboDate.EditValueChanged += new System.EventHandler(this.cboDate_EditValueChanged);
            // 
            // datTo
            // 
            this.datTo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.datTo.EditValue = null;
            this.datTo.Location = new System.Drawing.Point(290, 3);
            this.datTo.Name = "datTo";
            this.datTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.datTo.Size = new System.Drawing.Size(107, 20);
            this.datTo.StyleController = this.layoutControl1;
            this.datTo.TabIndex = 4;
            this.datTo.EditValueChanged += new System.EventHandler(this.datTo_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(400, 27);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.cboDate;
            this.layoutControlItem3.CustomizationFormText = "日期：";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(138, 25);
            this.layoutControlItem3.Text = "订单日期";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.datFrom;
            this.layoutControlItem1.CustomizationFormText = "从";
            this.layoutControlItem1.Location = new System.Drawing.Point(138, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(129, 25);
            this.layoutControlItem1.Text = "从";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(15, 20);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.datTo;
            this.layoutControlItem2.CustomizationFormText = "到：";
            this.layoutControlItem2.Location = new System.Drawing.Point(267, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(131, 25);
            this.layoutControlItem2.Text = "到";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(15, 20);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // UCDateTimeOne
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.layoutControl1);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Name = "UCDateTimeOne";
            this.Size = new System.Drawing.Size(400, 27);
            this.Load += new System.EventHandler(this.UCDateTimeOne_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit datFrom;
        private DevExpress.XtraEditors.ComboBoxEdit cboDate;
        private DevExpress.XtraEditors.DateEdit datTo;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;

    }
}
