namespace Sys.Safety.Client.Display
{
    partial class SetPointsShowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPointsShowForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridC = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.comb_showcount = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comb_showrows = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.comb_fz = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comb_lb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comb_zl = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.listB = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btn_selall = new DevExpress.XtraEditors.SimpleButton();
            this.btn_select = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delall = new DevExpress.XtraEditors.SimpleButton();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_close = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_showcount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_showrows.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_fz.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_lb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_zl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(8, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "每行显示数：";
            // 
            // gridC
            // 
            this.gridC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridC.Location = new System.Drawing.Point(414, 24);
            this.gridC.MainView = this.gv;
            this.gridC.Name = "gridC";
            this.gridC.Size = new System.Drawing.Size(728, 504);
            this.gridC.TabIndex = 1;
            this.gridC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            this.gridC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridC_MouseClick);
            this.gridC.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridC_MouseDoubleClick);
            // 
            // gv
            // 
            this.gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gv.GridControl = this.gridC;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsCustomization.AllowColumnMoving = false;
            this.gv.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gv.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gv_CustomDrawCell);
            this.gv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gv_MouseDown);
            this.gv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gv_MouseUp);
            this.gv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gv_MouseMove);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // comb_showcount
            // 
            this.comb_showcount.EditValue = "2";
            this.comb_showcount.Location = new System.Drawing.Point(82, 12);
            this.comb_showcount.Name = "comb_showcount";
            this.comb_showcount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_showcount.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comb_showcount.Size = new System.Drawing.Size(66, 20);
            this.comb_showcount.TabIndex = 2;
            this.comb_showcount.SelectedIndexChanged += new System.EventHandler(this.comb_showcount_SelectedIndexChanged);
            // 
            // comb_showrows
            // 
            this.comb_showrows.EditValue = "30";
            this.comb_showrows.Location = new System.Drawing.Point(238, 12);
            this.comb_showrows.Name = "comb_showrows";
            this.comb_showrows.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_showrows.Properties.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "65",
            "70",
            "75",
            "80",
            "85",
            "90",
            "95"});
            this.comb_showrows.Size = new System.Drawing.Size(59, 20);
            this.comb_showrows.TabIndex = 4;
            this.comb_showrows.SelectedIndexChanged += new System.EventHandler(this.comb_showrows_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(153, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "每页显示行数：";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(40, 42);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "分站：";
            // 
            // comb_fz
            // 
            this.comb_fz.EditValue = "";
            this.comb_fz.Location = new System.Drawing.Point(82, 39);
            this.comb_fz.Name = "comb_fz";
            this.comb_fz.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_fz.Size = new System.Drawing.Size(222, 20);
            this.comb_fz.TabIndex = 8;
            this.comb_fz.SelectedIndexChanged += new System.EventHandler(this.comb_fz_SelectedIndexChanged);
            // 
            // comb_lb
            // 
            this.comb_lb.EditValue = "";
            this.comb_lb.Location = new System.Drawing.Point(82, 65);
            this.comb_lb.Name = "comb_lb";
            this.comb_lb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_lb.Size = new System.Drawing.Size(222, 20);
            this.comb_lb.TabIndex = 9;
            this.comb_lb.SelectedIndexChanged += new System.EventHandler(this.comb_lb_SelectedIndexChanged);
            // 
            // comb_zl
            // 
            this.comb_zl.EditValue = "";
            this.comb_zl.Location = new System.Drawing.Point(82, 91);
            this.comb_zl.Name = "comb_zl";
            this.comb_zl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_zl.Size = new System.Drawing.Size(222, 20);
            this.comb_zl.TabIndex = 10;
            this.comb_zl.SelectedIndexChanged += new System.EventHandler(this.comb_zl_SelectedIndexChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(40, 93);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "小类：";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl7.Location = new System.Drawing.Point(507, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(132, 14);
            this.labelControl7.TabIndex = 12;
            this.labelControl7.Text = "鼠标拖动，调整显示位置\r\n";
            // 
            // listB
            // 
            this.listB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listB.Location = new System.Drawing.Point(21, 146);
            this.listB.Name = "listB";
            this.listB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listB.Size = new System.Drawing.Size(283, 393);
            this.listB.TabIndex = 13;
            this.listB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listB_MouseDoubleClick);
            this.listB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listB_MouseDown);
            this.listB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listB_MouseMove);
            this.listB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listB_MouseUp);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Location = new System.Drawing.Point(21, 124);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(60, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "待选择测点";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Location = new System.Drawing.Point(419, 4);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(72, 14);
            this.labelControl9.TabIndex = 15;
            this.labelControl9.Text = "显示界面预览";
            // 
            // btn_selall
            // 
            this.btn_selall.Image = ((System.Drawing.Image)(resources.GetObject("btn_selall.Image")));
            this.btn_selall.Location = new System.Drawing.Point(312, 195);
            this.btn_selall.Name = "btn_selall";
            this.btn_selall.Size = new System.Drawing.Size(93, 23);
            this.btn_selall.TabIndex = 16;
            this.btn_selall.Text = "全选";
            this.btn_selall.Click += new System.EventHandler(this.btn_selall_Click);
            // 
            // btn_select
            // 
            this.btn_select.Image = ((System.Drawing.Image)(resources.GetObject("btn_select.Image")));
            this.btn_select.Location = new System.Drawing.Point(312, 237);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(93, 23);
            this.btn_select.TabIndex = 19;
            this.btn_select.Text = "选择";
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.Image")));
            this.btn_delete.Location = new System.Drawing.Point(312, 277);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(93, 23);
            this.btn_delete.TabIndex = 20;
            this.btn_delete.Text = "删除";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_delall
            // 
            this.btn_delall.Image = ((System.Drawing.Image)(resources.GetObject("btn_delall.Image")));
            this.btn_delall.Location = new System.Drawing.Point(312, 316);
            this.btn_delall.Name = "btn_delall";
            this.btn_delall.Size = new System.Drawing.Size(93, 23);
            this.btn_delall.TabIndex = 21;
            this.btn_delall.Text = "全删";
            this.btn_delall.Click += new System.EventHandler(this.btn_delall_Click);
            // 
            // checkEdit1
            // 
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(93, 122);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.checkEdit1.Properties.Appearance.Options.UseForeColor = true;
            this.checkEdit1.Properties.Caption = "已选择的不显示到下面的列表中";
            this.checkEdit1.Size = new System.Drawing.Size(216, 19);
            this.checkEdit1.TabIndex = 22;
            this.checkEdit1.Visible = false;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.Location = new System.Drawing.Point(959, 547);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(64, 23);
            this.btn_save.TabIndex = 23;
            this.btn_save.Text = "保 存";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.Image = ((System.Drawing.Image)(resources.GetObject("btn_close.Image")));
            this.btn_close.Location = new System.Drawing.Point(1059, 547);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(64, 23);
            this.btn_close.TabIndex = 24;
            this.btn_close.Text = "退 出";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(40, 68);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 28;
            this.labelControl5.Text = "大类：";
            // 
            // labelControl13
            // 
            this.labelControl13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelControl13.Location = new System.Drawing.Point(24, 631);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(55, 14);
            this.labelControl13.TabIndex = 30;
            this.labelControl13.Text = "保存中....";
            this.labelControl13.Visible = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(312, 420);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 24);
            this.simpleButton1.TabIndex = 20;
            this.simpleButton1.Text = "删除数据行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(312, 383);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(93, 23);
            this.simpleButton2.TabIndex = 19;
            this.simpleButton2.Text = "插入数据";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // SetPointsShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 576);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.btn_delall);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.btn_selall);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.listB);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.comb_zl);
            this.Controls.Add(this.comb_lb);
            this.Controls.Add(this.comb_fz);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.comb_showrows);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.comb_showcount);
            this.Controls.Add(this.gridC);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetPointsShowForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义显示设置";
            this.Load += new System.EventHandler(this.SetPointsShowForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_showcount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_showrows.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_fz.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_lb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_zl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gridC;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private DevExpress.XtraEditors.ComboBoxEdit comb_showcount;
        private DevExpress.XtraEditors.ComboBoxEdit comb_showrows;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit comb_fz;
        private DevExpress.XtraEditors.ComboBoxEdit comb_lb;
        private DevExpress.XtraEditors.ComboBoxEdit comb_zl;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ListBoxControl listB;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btn_selall;
        private DevExpress.XtraEditors.SimpleButton btn_select;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraEditors.SimpleButton btn_delall;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_close;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}