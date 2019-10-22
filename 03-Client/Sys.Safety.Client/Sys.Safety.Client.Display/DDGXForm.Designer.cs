namespace Sys.Safety.Client.Display
{
    partial class DDGXForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DDGXForm));
            this.tab1 = new System.Windows.Forms.TabControl();
            this.tp1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txt_conditon = new System.Windows.Forms.TextBox();
            this.mainGrid1 = new DevExpress.XtraGrid.GridControl();
            this.mainGridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tp2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.txt_c2 = new System.Windows.Forms.TextBox();
            this.mainGrid2 = new DevExpress.XtraGrid.GridControl();
            this.mainGridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tp3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.txt_c3 = new System.Windows.Forms.TextBox();
            this.mainGrid3 = new DevExpress.XtraGrid.GridControl();
            this.mainGridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.tab1.SuspendLayout();
            this.tp1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView1)).BeginInit();
            this.tp2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView2)).BeginInit();
            this.tp3.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab1.Controls.Add(this.tp1);
            this.tab1.Controls.Add(this.tp2);
            this.tab1.Controls.Add(this.tp3);
            this.tab1.Location = new System.Drawing.Point(3, 3);
            this.tab1.Name = "tab1";
            this.tab1.SelectedIndex = 0;
            this.tab1.Size = new System.Drawing.Size(1255, 585);
            this.tab1.TabIndex = 0;
            // 
            // tp1
            // 
            this.tp1.Controls.Add(this.panel1);
            this.tp1.Controls.Add(this.mainGrid1);
            this.tp1.Location = new System.Drawing.Point(4, 23);
            this.tp1.Name = "tp1";
            this.tp1.Padding = new System.Windows.Forms.Padding(3);
            this.tp1.Size = new System.Drawing.Size(1247, 558);
            this.tp1.TabIndex = 0;
            this.tp1.Text = "模拟量本地断电";
            this.tp1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Controls.Add(this.txt_conditon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1241, 33);
            this.panel1.TabIndex = 7;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(302, 5);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "搜索";
            this.simpleButton2.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_conditon
            // 
            this.txt_conditon.Location = new System.Drawing.Point(4, 6);
            this.txt_conditon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_conditon.Name = "txt_conditon";
            this.txt_conditon.Size = new System.Drawing.Size(290, 22);
            this.txt_conditon.TabIndex = 0;
            // 
            // mainGrid1
            // 
            this.mainGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainGrid1.Location = new System.Drawing.Point(3, 36);
            this.mainGrid1.MainView = this.mainGridView1;
            this.mainGrid1.Name = "mainGrid1";
            this.mainGrid1.Size = new System.Drawing.Size(1241, 519);
            this.mainGrid1.TabIndex = 6;
            this.mainGrid1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainGridView1});
            // 
            // mainGridView1
            // 
            this.mainGridView1.GridControl = this.mainGrid1;
            this.mainGridView1.IndicatorWidth = 30;
            this.mainGridView1.Name = "mainGridView1";
            this.mainGridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView1.OptionsBehavior.Editable = false;
            this.mainGridView1.OptionsFind.FindDelay = 100;
            this.mainGridView1.OptionsMenu.EnableColumnMenu = false;
            this.mainGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // tp2
            // 
            this.tp2.Controls.Add(this.panel2);
            this.tp2.Controls.Add(this.mainGrid2);
            this.tp2.Location = new System.Drawing.Point(4, 23);
            this.tp2.Name = "tp2";
            this.tp2.Padding = new System.Windows.Forms.Padding(3);
            this.tp2.Size = new System.Drawing.Size(1247, 558);
            this.tp2.TabIndex = 1;
            this.tp2.Text = "开关量本地断电";
            this.tp2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.simpleButton3);
            this.panel2.Controls.Add(this.txt_c2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1241, 36);
            this.panel2.TabIndex = 8;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(297, 6);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(75, 23);
            this.simpleButton3.TabIndex = 1;
            this.simpleButton3.Text = "搜索";
            this.simpleButton3.Click += new System.EventHandler(this.btn_s2_Click);
            // 
            // txt_c2
            // 
            this.txt_c2.Location = new System.Drawing.Point(4, 7);
            this.txt_c2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_c2.Name = "txt_c2";
            this.txt_c2.Size = new System.Drawing.Size(287, 22);
            this.txt_c2.TabIndex = 0;
            // 
            // mainGrid2
            // 
            this.mainGrid2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainGrid2.Location = new System.Drawing.Point(3, 39);
            this.mainGrid2.MainView = this.mainGridView2;
            this.mainGrid2.Name = "mainGrid2";
            this.mainGrid2.Size = new System.Drawing.Size(1241, 516);
            this.mainGrid2.TabIndex = 6;
            this.mainGrid2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainGridView2});
            // 
            // mainGridView2
            // 
            this.mainGridView2.GridControl = this.mainGrid2;
            this.mainGridView2.IndicatorWidth = 30;
            this.mainGridView2.Name = "mainGridView2";
            this.mainGridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView2.OptionsBehavior.Editable = false;
            this.mainGridView2.OptionsFind.FindDelay = 100;
            this.mainGridView2.OptionsMenu.EnableColumnMenu = false;
            this.mainGridView2.OptionsView.ShowGroupPanel = false;
            // 
            // tp3
            // 
            this.tp3.Controls.Add(this.panel3);
            this.tp3.Controls.Add(this.mainGrid3);
            this.tp3.Location = new System.Drawing.Point(4, 23);
            this.tp3.Name = "tp3";
            this.tp3.Padding = new System.Windows.Forms.Padding(3);
            this.tp3.Size = new System.Drawing.Size(1247, 558);
            this.tp3.TabIndex = 2;
            this.tp3.Text = "交叉（异地）断电";
            this.tp3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.simpleButton4);
            this.panel3.Controls.Add(this.txt_c3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1241, 41);
            this.panel3.TabIndex = 8;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(306, 9);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(75, 23);
            this.simpleButton4.TabIndex = 1;
            this.simpleButton4.Text = "搜索";
            this.simpleButton4.Click += new System.EventHandler(this.btn_s3_Click);
            // 
            // txt_c3
            // 
            this.txt_c3.Location = new System.Drawing.Point(5, 10);
            this.txt_c3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_c3.Name = "txt_c3";
            this.txt_c3.Size = new System.Drawing.Size(292, 22);
            this.txt_c3.TabIndex = 0;
            // 
            // mainGrid3
            // 
            this.mainGrid3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainGrid3.Location = new System.Drawing.Point(3, 44);
            this.mainGrid3.MainView = this.mainGridView3;
            this.mainGrid3.Name = "mainGrid3";
            this.mainGrid3.Size = new System.Drawing.Size(1241, 511);
            this.mainGrid3.TabIndex = 6;
            this.mainGrid3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainGridView3});
            // 
            // mainGridView3
            // 
            this.mainGridView3.GridControl = this.mainGrid3;
            this.mainGridView3.IndicatorWidth = 30;
            this.mainGridView3.Name = "mainGridView3";
            this.mainGridView3.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView3.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.mainGridView3.OptionsBehavior.Editable = false;
            this.mainGridView3.OptionsFind.FindDelay = 100;
            this.mainGridView3.OptionsMenu.EnableColumnMenu = false;
            this.mainGridView3.OptionsView.ShowGroupPanel = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(1162, 590);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(88, 23);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "导出excel";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // DDGXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 617);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.tab1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DDGXForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "断电关系查询";
            this.Load += new System.EventHandler(this.DDGXForm_Load);
            this.tab1.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView1)).EndInit();
            this.tp2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView2)).EndInit();
            this.tp3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.TabPage tp3;
        private DevExpress.XtraGrid.GridControl mainGrid1;
        private DevExpress.XtraGrid.Views.Grid.GridView mainGridView1;
        private DevExpress.XtraGrid.GridControl mainGrid2;
        private DevExpress.XtraGrid.Views.Grid.GridView mainGridView2;
        private DevExpress.XtraGrid.GridControl mainGrid3;
        private DevExpress.XtraGrid.Views.Grid.GridView mainGridView3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_conditon;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txt_c2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txt_c3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;

    }
}