namespace Sys.Safety.Reports.Controls
{
    partial class UCDateTimeClass
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
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.gridLookUpClass = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpViewClass = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.datTo = new DevExpress.XtraEditors.DateEdit();
            this.datFrom = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpClass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpViewClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomizationMenu = false;
            this.layoutControl1.AutoScroll = false;
            this.layoutControl1.Controls.Add(this.checkEdit2);
            this.layoutControl1.Controls.Add(this.checkEdit1);
            this.layoutControl1.Controls.Add(this.gridLookUpClass);
            this.layoutControl1.Controls.Add(this.datTo);
            this.layoutControl1.Controls.Add(this.datFrom);
            this.layoutControl1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(400, 27);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // checkEdit2
            // 
            this.checkEdit2.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkEdit2.Location = new System.Drawing.Point(65, 3);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "按日期";
            this.checkEdit2.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit2.Size = new System.Drawing.Size(58, 19);
            this.checkEdit2.StyleController = this.layoutControl1;
            this.checkEdit2.TabIndex = 9;
            this.checkEdit2.CheckedChanged += new System.EventHandler(this.checkEdit2_CheckedChanged);
            // 
            // checkEdit1
            // 
            this.checkEdit1.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkEdit1.Location = new System.Drawing.Point(3, 3);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "按班次";
            this.checkEdit1.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit1.Size = new System.Drawing.Size(58, 19);
            this.checkEdit1.StyleController = this.layoutControl1;
            this.checkEdit1.TabIndex = 8;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // gridLookUpClass
            // 
            this.gridLookUpClass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.gridLookUpClass.EditValue = "";
            this.gridLookUpClass.Location = new System.Drawing.Point(347, 3);
            this.gridLookUpClass.Name = "gridLookUpClass";
            this.gridLookUpClass.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpClass.Properties.NullText = "";
            this.gridLookUpClass.Properties.View = this.gridLookUpViewClass;
            this.gridLookUpClass.Size = new System.Drawing.Size(50, 20);
            this.gridLookUpClass.StyleController = this.layoutControl1;
            this.gridLookUpClass.TabIndex = 7;
            this.gridLookUpClass.EditValueChanged += new System.EventHandler(this.gridLookUpClass_EditValueChanged);
            // 
            // gridLookUpViewClass
            // 
            this.gridLookUpViewClass.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpViewClass.Name = "gridLookUpViewClass";
            this.gridLookUpViewClass.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpViewClass.OptionsView.ShowGroupPanel = false;
            // 
            // datTo
            // 
            this.datTo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.datTo.EditValue = null;
            this.datTo.Location = new System.Drawing.Point(264, 3);
            this.datTo.Name = "datTo";
            this.datTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.datTo.Size = new System.Drawing.Size(50, 20);
            this.datTo.StyleController = this.layoutControl1;
            this.datTo.TabIndex = 4;
            this.datTo.EditValueChanged += new System.EventHandler(this.datTo_EditValueChanged);
            this.datTo.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.datTo_EditValueChanging);
            // 
            // datFrom
            // 
            this.datFrom.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.datFrom.EditValue = null;
            this.datFrom.Location = new System.Drawing.Point(127, 3);
            this.datFrom.Name = "datFrom";
            this.datFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.datFrom.Size = new System.Drawing.Size(113, 20);
            this.datFrom.StyleController = this.layoutControl1;
            this.datFrom.TabIndex = 5;
            this.datFrom.EditValueChanged += new System.EventHandler(this.datFrom_EditValueChanged);
            this.datFrom.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.datFrom_EditValueChanging);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.layoutControlItem5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(400, 27);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.datFrom;
            this.layoutControlItem1.CustomizationFormText = "从";
            this.layoutControlItem1.Location = new System.Drawing.Point(124, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(117, 25);
            this.layoutControlItem1.Text = "日期型";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.datTo;
            this.layoutControlItem2.CustomizationFormText = "到：";
            this.layoutControlItem2.Location = new System.Drawing.Point(241, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(74, 25);
            this.layoutControlItem2.Text = "到";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(15, 20);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.gridLookUpClass;
            this.layoutControlItem4.CustomizationFormText = "班次";
            this.layoutControlItem4.Location = new System.Drawing.Point(315, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(83, 25);
            this.layoutControlItem4.Text = "班次";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(24, 14);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.checkEdit1;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(62, 25);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.checkEdit2;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(62, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(62, 25);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // UCDateTimeClass
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.layoutControl1);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Name = "UCDateTimeClass";
            this.Size = new System.Drawing.Size(400, 27);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpClass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpViewClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit datFrom;
        private DevExpress.XtraEditors.DateEdit datTo;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpClass;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpViewClass;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;

    }
}
