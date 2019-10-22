namespace Sys.Safety.Client.Display
{
    partial class SetPoints
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPoints));
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.comb_fz = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comb_lb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comb_zl = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.listB = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.btn_selall = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.btn_select = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delall = new DevExpress.XtraEditors.SimpleButton();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_close = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.listA = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.comb_fz.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_lb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_zl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listA)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(24, 104);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(65, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "筛选条件：";
            // 
            // comb_fz
            // 
            this.comb_fz.EditValue = "";
            this.comb_fz.Location = new System.Drawing.Point(90, 101);
            this.comb_fz.Name = "comb_fz";
            this.comb_fz.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_fz.Size = new System.Drawing.Size(280, 20);
            this.comb_fz.TabIndex = 8;
            this.comb_fz.SelectedIndexChanged += new System.EventHandler(this.comb_fz_SelectedIndexChanged);
            // 
            // comb_lb
            // 
            this.comb_lb.EditValue = "";
            this.comb_lb.Location = new System.Drawing.Point(422, 101);
            this.comb_lb.Name = "comb_lb";
            this.comb_lb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_lb.Size = new System.Drawing.Size(100, 20);
            this.comb_lb.TabIndex = 9;
            this.comb_lb.SelectedIndexChanged += new System.EventHandler(this.comb_lb_SelectedIndexChanged);
            // 
            // comb_zl
            // 
            this.comb_zl.EditValue = "";
            this.comb_zl.Location = new System.Drawing.Point(571, 101);
            this.comb_zl.Name = "comb_zl";
            this.comb_zl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comb_zl.Size = new System.Drawing.Size(146, 20);
            this.comb_zl.TabIndex = 10;
            this.comb_zl.SelectedIndexChanged += new System.EventHandler(this.comb_zl_SelectedIndexChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(537, 104);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(17, 14);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "-->";
            // 
            // listB
            // 
            this.listB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listB.Location = new System.Drawing.Point(24, 179);
            this.listB.Name = "listB";
            this.listB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listB.Size = new System.Drawing.Size(288, 309);
            this.listB.TabIndex = 13;
            this.listB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listB_MouseDown);
            this.listB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listB_MouseUp);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Location = new System.Drawing.Point(24, 155);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(65, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "待选择测点";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Location = new System.Drawing.Point(388, 155);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(65, 14);
            this.labelControl9.TabIndex = 15;
            this.labelControl9.Text = "已选择测点";
            // 
            // btn_selall
            // 
            this.btn_selall.Location = new System.Drawing.Point(318, 279);
            this.btn_selall.Name = "btn_selall";
            this.btn_selall.Size = new System.Drawing.Size(64, 23);
            this.btn_selall.TabIndex = 16;
            this.btn_selall.Text = ">>全选";
            this.btn_selall.Click += new System.EventHandler(this.btn_selall_Click);
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl10.Location = new System.Drawing.Point(24, 84);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(96, 14);
            this.labelControl10.TabIndex = 17;
            this.labelControl10.Text = "设置测点筛选条件";
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(318, 320);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(64, 23);
            this.btn_select.TabIndex = 19;
            this.btn_select.Text = ">选择";
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(318, 359);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(64, 23);
            this.btn_delete.TabIndex = 20;
            this.btn_delete.Text = "<删除";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_delall
            // 
            this.btn_delall.Location = new System.Drawing.Point(318, 403);
            this.btn_delall.Name = "btn_delall";
            this.btn_delall.Size = new System.Drawing.Size(64, 23);
            this.btn_delall.TabIndex = 21;
            this.btn_delall.Text = "<<全删";
            this.btn_delall.Click += new System.EventHandler(this.btn_delall_Click);
            // 
            // checkEdit1
            // 
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(96, 153);
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
            this.btn_save.Location = new System.Drawing.Point(521, 494);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(64, 23);
            this.btn_save.TabIndex = 23;
            this.btn_save.Text = "保 存";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.Location = new System.Drawing.Point(612, 494);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(64, 23);
            this.btn_close.TabIndex = 24;
            this.btn_close.Text = "退 出";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(388, 104);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(17, 14);
            this.labelControl5.TabIndex = 28;
            this.labelControl5.Text = "-->";
            // 
            // labelControl13
            // 
            this.labelControl13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelControl13.Location = new System.Drawing.Point(24, 503);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(55, 14);
            this.labelControl13.TabIndex = 30;
            this.labelControl13.Text = "保存中....";
            this.labelControl13.Visible = false;
            // 
            // listA
            // 
            this.listA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listA.Location = new System.Drawing.Point(388, 179);
            this.listA.Name = "listA";
            this.listA.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listA.Size = new System.Drawing.Size(288, 309);
            this.listA.TabIndex = 32;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(24, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(91, 14);
            this.labelControl1.TabIndex = 33;
            this.labelControl1.Text = "预定标校日期：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(121, 47);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 34;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // SetPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 529);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.listA);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.btn_delall);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_select);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.btn_selall);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.listB);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.comb_zl);
            this.Controls.Add(this.comb_lb);
            this.Controls.Add(this.comb_fz);
            this.Controls.Add(this.labelControl4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetPoints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标校测点定义";
            this.Load += new System.EventHandler(this.SetPointsShowForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comb_fz.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_lb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comb_zl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit comb_fz;
        private DevExpress.XtraEditors.ComboBoxEdit comb_lb;
        private DevExpress.XtraEditors.ComboBoxEdit comb_zl;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ListBoxControl listB;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btn_selall;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.SimpleButton btn_select;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraEditors.SimpleButton btn_delall;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_close;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.ListBoxControl listA;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}