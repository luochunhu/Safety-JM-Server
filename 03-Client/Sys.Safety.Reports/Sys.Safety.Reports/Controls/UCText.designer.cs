namespace Sys.Safety.Reports.Controls
{
    partial class UCText
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
            this.txtValue2 = new DevExpress.XtraEditors.TextEdit();
            this.txtValue1 = new DevExpress.XtraEditors.TextEdit();
            this.cboCondition = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCondition.Properties)).BeginInit();
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
            this.layoutControl1.Controls.Add(this.txtValue2);
            this.layoutControl1.Controls.Add(this.txtValue1);
            this.layoutControl1.Controls.Add(this.cboCondition);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(400, 27);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtValue2
            // 
            this.txtValue2.Location = new System.Drawing.Point(288, 3);
            this.txtValue2.Name = "txtValue2";
            this.txtValue2.Size = new System.Drawing.Size(109, 20);
            this.txtValue2.StyleController = this.layoutControl1;
            this.txtValue2.TabIndex = 5;
            this.txtValue2.EditValueChanged += new System.EventHandler(this.txtValue2_EditValueChanged);
            // 
            // txtValue1
            // 
            this.txtValue1.Location = new System.Drawing.Point(161, 3);
            this.txtValue1.Name = "txtValue1";
            this.txtValue1.Size = new System.Drawing.Size(102, 20);
            this.txtValue1.StyleController = this.layoutControl1;
            this.txtValue1.TabIndex = 4;
            this.txtValue1.EditValueChanged += new System.EventHandler(this.txtValue1_EditValueChanged);
            // 
            // cboCondition
            // 
            this.cboCondition.EditValue = "";
            this.cboCondition.Location = new System.Drawing.Point(32, 3);
            this.cboCondition.Name = "cboCondition";
            this.cboCondition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCondition.Properties.DropDownRows = 11;
            this.cboCondition.Properties.Items.AddRange(new object[] {
            "",
            "等于",
            "大于",
            "小于",
            "不等于",
            "大于等于",
            "小于等于",
            "介于",
            "开始于",
            "包含",
            "不包含"});
            this.cboCondition.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboCondition.Size = new System.Drawing.Size(104, 20);
            this.cboCondition.StyleController = this.layoutControl1;
            this.cboCondition.TabIndex = 3;
            this.cboCondition.EditValueChanged += new System.EventHandler(this.cboCondition_EditValueChanged);
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
            this.layoutControlItem3.Control = this.txtValue2;
            this.layoutControlItem3.CustomizationFormText = " ";
            this.layoutControlItem3.Location = new System.Drawing.Point(264, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(134, 25);
            this.layoutControlItem3.Text = " 到";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(16, 14);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.cboCondition;
            this.layoutControlItem1.CustomizationFormText = "条件";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(137, 25);
            this.layoutControlItem1.Text = "条件";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(24, 14);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtValue1;
            this.layoutControlItem2.CustomizationFormText = " ";
            this.layoutControlItem2.Location = new System.Drawing.Point(137, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(127, 25);
            this.layoutControlItem2.Text = " 从";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(16, 14);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // UCText
            // 
            this.Controls.Add(this.layoutControl1);
            this.Name = "UCText";
            this.Size = new System.Drawing.Size(400, 27);
            this.Load += new System.EventHandler(this.UCText_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtValue2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCondition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboCondition;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.TextEdit txtValue2;
        private DevExpress.XtraEditors.TextEdit txtValue1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}
