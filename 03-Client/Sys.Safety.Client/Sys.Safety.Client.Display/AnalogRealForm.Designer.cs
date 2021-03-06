﻿namespace Sys.Safety.Client.Display
{
    partial class AnalogRealForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalogRealForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmb_adr = new DevExpress.XtraEditors.ComboBoxEdit();
            this.DDValue = new DevExpress.XtraEditors.LabelControl();
            this.AlarmVal = new DevExpress.XtraEditors.LabelControl();
            this.lb_value = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.lb_state = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lb_wz = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lb_type = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.mainGrid = new DevExpress.XtraGrid.GridControl();
            this.mainGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_adr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.cmb_adr);
            this.groupControl1.Controls.Add(this.DDValue);
            this.groupControl1.Controls.Add(this.AlarmVal);
            this.groupControl1.Controls.Add(this.lb_value);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.lb_state);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.lb_wz);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.lb_type);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(10, 10);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(239, 477);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "模拟量信息";
            // 
            // cmb_adr
            // 
            this.cmb_adr.Location = new System.Drawing.Point(75, 37);
            this.cmb_adr.Name = "cmb_adr";
            this.cmb_adr.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_adr.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_adr.Size = new System.Drawing.Size(118, 20);
            this.cmb_adr.TabIndex = 11;
            this.cmb_adr.SelectedIndexChanged += new System.EventHandler(this.cmb_adr_SelectedIndexChanged);
            // 
            // DDValue
            // 
            this.DDValue.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DDValue.Appearance.ForeColor = System.Drawing.Color.Black;
            this.DDValue.Location = new System.Drawing.Point(75, 217);
            this.DDValue.Name = "DDValue";
            this.DDValue.Size = new System.Drawing.Size(52, 14);
            this.DDValue.TabIndex = 10;
            this.DDValue.Text = "交流正常";
            // 
            // AlarmVal
            // 
            this.AlarmVal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlarmVal.Appearance.ForeColor = System.Drawing.Color.Black;
            this.AlarmVal.Location = new System.Drawing.Point(75, 182);
            this.AlarmVal.Name = "AlarmVal";
            this.AlarmVal.Size = new System.Drawing.Size(52, 14);
            this.AlarmVal.TabIndex = 10;
            this.AlarmVal.Text = "交流正常";
            // 
            // lb_value
            // 
            this.lb_value.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_value.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lb_value.Location = new System.Drawing.Point(75, 249);
            this.lb_value.Name = "lb_value";
            this.lb_value.Size = new System.Drawing.Size(52, 14);
            this.lb_value.TabIndex = 10;
            this.lb_value.Text = "交流正常";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(19, 217);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(40, 14);
            this.labelControl8.TabIndex = 5;
            this.labelControl8.Text = "断电值:";
            // 
            // lb_state
            // 
            this.lb_state.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_state.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lb_state.Location = new System.Drawing.Point(75, 146);
            this.lb_state.Name = "lb_state";
            this.lb_state.Size = new System.Drawing.Size(52, 14);
            this.lb_state.TabIndex = 10;
            this.lb_state.Text = "交流正常";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(19, 182);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(40, 14);
            this.labelControl5.TabIndex = 5;
            this.labelControl5.Text = "报警值:";
            // 
            // lb_wz
            // 
            this.lb_wz.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(200)));
            this.lb_wz.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lb_wz.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lb_wz.Location = new System.Drawing.Point(75, 77);
            this.lb_wz.Name = "lb_wz";
            this.lb_wz.Size = new System.Drawing.Size(160, 14);
            this.lb_wz.TabIndex = 7;
            this.lb_wz.Text = "----";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(19, 249);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(40, 14);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "实时值:";
            // 
            // lb_type
            // 
            this.lb_type.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_type.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lb_type.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lb_type.Location = new System.Drawing.Point(75, 111);
            this.lb_type.Name = "lb_type";
            this.lb_type.Size = new System.Drawing.Size(160, 14);
            this.lb_type.TabIndex = 6;
            this.lb_type.Text = "大分站";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(7, 146);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "当前状态:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(7, 77);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "安装位置:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(7, 111);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "设备类型:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(7, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(28, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "点号:";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(255, 11);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(748, 481);
            this.xtraTabControl1.TabIndex = 6;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.mainGrid);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(742, 452);
            this.xtraTabPage1.Text = "控制信息";
            // 
            // mainGrid
            // 
            this.mainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainGrid.Location = new System.Drawing.Point(0, 0);
            this.mainGrid.MainView = this.mainGridView;
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.Size = new System.Drawing.Size(742, 452);
            this.mainGrid.TabIndex = 5;
            this.mainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainGridView});
            // 
            // mainGridView
            // 
            this.mainGridView.GridControl = this.mainGrid;
            this.mainGridView.IndicatorWidth = 30;
            this.mainGridView.Name = "mainGridView";
            this.mainGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView.OptionsBehavior.Editable = false;
            this.mainGridView.OptionsFind.FindDelay = 100;
            this.mainGridView.OptionsMenu.EnableColumnMenu = false;
            this.mainGridView.OptionsView.ShowGroupPanel = false;
            // 
            // AnalogRealForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 503);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnalogRealForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模拟量信息显示";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KzRealForm_FormClosed);
            this.Load += new System.EventHandler(this.KzRealForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_adr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_adr;
        private DevExpress.XtraEditors.LabelControl lb_state;
        private DevExpress.XtraEditors.LabelControl lb_wz;
        private DevExpress.XtraEditors.LabelControl lb_type;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl mainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView mainGridView;
        private DevExpress.XtraEditors.LabelControl lb_value;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl DDValue;
        private DevExpress.XtraEditors.LabelControl AlarmVal;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}