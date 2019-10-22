namespace Sys.Safety.Client.Display
{
    partial class SBSJForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SBSJForm));
            this.txt_sb = new System.Windows.Forms.TextBox();
            this.btn_view = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.sx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.xh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txzt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dqbb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sjzt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_pl = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lb_time = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.cmb_bbxx = new System.Windows.Forms.ComboBox();
            this.cmb_bbsx = new System.Windows.Forms.ComboBox();
            this.cmb_dqyjbb = new System.Windows.Forms.ComboBox();
            this.cmb_dqbb = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_sb
            // 
            this.txt_sb.Enabled = false;
            this.txt_sb.Location = new System.Drawing.Point(24, 30);
            this.txt_sb.Name = "txt_sb";
            this.txt_sb.Size = new System.Drawing.Size(770, 22);
            this.txt_sb.TabIndex = 1;
            this.txt_sb.Text = "请点击浏览选择升级文件";
            // 
            // btn_view
            // 
            this.btn_view.Location = new System.Drawing.Point(833, 29);
            this.btn_view.Name = "btn_view";
            this.btn_view.Size = new System.Drawing.Size(87, 27);
            this.btn_view.TabIndex = 2;
            this.btn_view.Text = "浏览";
            this.btn_view.UseVisualStyleBackColor = true;
            this.btn_view.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "升级的软件版本号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(566, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "选择或输入升级的设备类型：";
            // 
            // cmb_type
            // 
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Items.AddRange(new object[] {
            "[22]KJ306-F(16)本安型分站",
            "[2]KJ306-F(16)H本安型分站"});
            this.cmb_type.Location = new System.Drawing.Point(776, 97);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(256, 22);
            this.cmb_type.TabIndex = 6;
            this.cmb_type.SelectedIndexChanged += new System.EventHandler(this.cmb_type_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sx,
            this.xh,
            this.dz,
            this.wz,
            this.type,
            this.txzt,
            this.dqbb,
            this.zs,
            this.sjzt,
            this.jd});
            this.dataGridView1.Location = new System.Drawing.Point(5, 167);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1009, 286);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // sx
            // 
            this.sx.HeaderText = "筛选";
            this.sx.Name = "sx";
            this.sx.Width = 50;
            // 
            // xh
            // 
            this.xh.HeaderText = "序号";
            this.xh.Name = "xh";
            this.xh.Width = 60;
            // 
            // dz
            // 
            this.dz.HeaderText = "设备地址";
            this.dz.Name = "dz";
            this.dz.Width = 80;
            // 
            // wz
            // 
            this.wz.HeaderText = "安装位置";
            this.wz.Name = "wz";
            // 
            // type
            // 
            this.type.HeaderText = "设备类型";
            this.type.Name = "type";
            // 
            // txzt
            // 
            this.txzt.HeaderText = "通讯状态";
            this.txzt.Name = "txzt";
            // 
            // dqbb
            // 
            this.dqbb.HeaderText = "当前版本";
            this.dqbb.Name = "dqbb";
            // 
            // zs
            // 
            this.zs.HeaderText = "下发帧数";
            this.zs.Name = "zs";
            this.zs.Width = 80;
            // 
            // sjzt
            // 
            this.sjzt.HeaderText = "升级状态";
            this.sjzt.Name = "sjzt";
            // 
            // jd
            // 
            this.jd.HeaderText = "升级进度";
            this.jd.Name = "jd";
            this.jd.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(2, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "设备升级的进度状态浏览表";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(22, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "总帧数【】";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 27);
            this.button1.TabIndex = 10;
            this.button1.Text = "下发文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 27);
            this.button2.TabIndex = 11;
            this.button2.Text = "重启升级";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(547, 70);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 27);
            this.button3.TabIndex = 12;
            this.button3.Text = "取消升级";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(547, 105);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 27);
            this.button4.TabIndex = 13;
            this.button4.Text = "版本还原";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(111, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "将需要升级的文件版本程序下发的设备上";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(653, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(163, 14);
            this.label7.TabIndex = 15;
            this.label7.Text = "告诉设备将接收到的文件丢弃";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(111, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(307, 14);
            this.label8.TabIndex = 16;
            this.label8.Text = "当设备接收完升级文件后，告诉设备可以重启进行升级了";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(653, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(331, 14);
            this.label9.TabIndex = 17;
            this.label9.Text = "当设备升级完成后，需要取消这次升级恢复成之前的版本程序";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(111, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(263, 14);
            this.label10.TabIndex = 19;
            this.label10.Text = "向分站发送请求升级命令,需要在列表中筛选分站";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(7, 36);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 27);
            this.button5.TabIndex = 18;
            this.button5.Text = "请求升级";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(22, 163);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "选择下发频率：";
            // 
            // cmb_pl
            // 
            this.cmb_pl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_pl.FormattingEnabled = true;
            this.cmb_pl.Items.AddRange(new object[] {
            "1",
            "3",
            "5",
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
            "60"});
            this.cmb_pl.Location = new System.Drawing.Point(122, 160);
            this.cmb_pl.Name = "cmb_pl";
            this.cmb_pl.Size = new System.Drawing.Size(84, 22);
            this.cmb_pl.TabIndex = 21;
            this.cmb_pl.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(227, 163);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 14);
            this.label12.TabIndex = 22;
            this.label12.Text = "分";
            // 
            // lb_time
            // 
            this.lb_time.AutoSize = true;
            this.lb_time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lb_time.Location = new System.Drawing.Point(276, 163);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(139, 14);
            this.lb_time.TabIndex = 23;
            this.lb_time.Text = "文件下发，所需时间【】";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmb_bbxx);
            this.groupBox1.Controls.Add(this.cmb_bbsx);
            this.groupBox1.Controls.Add(this.cmb_dqyjbb);
            this.groupBox1.Controls.Add(this.cmb_dqbb);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cmb_type);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_sb);
            this.groupBox1.Controls.Add(this.lb_time);
            this.groupBox1.Controls.Add(this.btn_view);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmb_pl);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(12, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1039, 197);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本基础信息";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(833, 132);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(104, 45);
            this.button8.TabIndex = 22;
            this.button8.Text = "开始升级";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label15.Location = new System.Drawing.Point(169, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 14);
            this.label15.TabIndex = 34;
            this.label15.Text = "版本号放大10倍";
            // 
            // cmb_bbxx
            // 
            this.cmb_bbxx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_bbxx.FormattingEnabled = true;
            this.cmb_bbxx.Location = new System.Drawing.Point(432, 126);
            this.cmb_bbxx.Name = "cmb_bbxx";
            this.cmb_bbxx.Size = new System.Drawing.Size(126, 22);
            this.cmb_bbxx.TabIndex = 33;
            // 
            // cmb_bbsx
            // 
            this.cmb_bbsx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_bbsx.FormattingEnabled = true;
            this.cmb_bbsx.Location = new System.Drawing.Point(147, 126);
            this.cmb_bbsx.Name = "cmb_bbsx";
            this.cmb_bbsx.Size = new System.Drawing.Size(118, 22);
            this.cmb_bbsx.TabIndex = 32;
            // 
            // cmb_dqyjbb
            // 
            this.cmb_dqyjbb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_dqyjbb.FormattingEnabled = true;
            this.cmb_dqyjbb.Location = new System.Drawing.Point(432, 96);
            this.cmb_dqyjbb.Name = "cmb_dqyjbb";
            this.cmb_dqyjbb.Size = new System.Drawing.Size(126, 22);
            this.cmb_dqyjbb.TabIndex = 31;
            // 
            // cmb_dqbb
            // 
            this.cmb_dqbb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_dqbb.FormattingEnabled = true;
            this.cmb_dqbb.Location = new System.Drawing.Point(147, 96);
            this.cmb_dqbb.Name = "cmb_dqbb";
            this.cmb_dqbb.Size = new System.Drawing.Size(118, 22);
            this.cmb_dqbb.TabIndex = 30;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(322, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 12);
            this.label14.TabIndex = 28;
            this.label14.Text = "升级版本下限：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(37, 132);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 12);
            this.label13.TabIndex = 26;
            this.label13.Text = "升级版本上限：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(276, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "需升级的硬件版本号：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(12, 262);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1039, 471);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "升级操作";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(547, 23);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(104, 27);
            this.button7.TabIndex = 21;
            this.button7.Text = "查看接收情况";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(860, 63);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(104, 27);
            this.button6.TabIndex = 20;
            this.button6.Text = "获取版本";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // SBSJForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 685);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SBSJForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备远程升级管理";
            this.Load += new System.EventHandler(this.SBSJForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_sb;
        private System.Windows.Forms.Button btn_view;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_type;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sx;
        private System.Windows.Forms.DataGridViewTextBoxColumn xh;
        private System.Windows.Forms.DataGridViewTextBoxColumn dz;
        private System.Windows.Forms.DataGridViewTextBoxColumn wz;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn txzt;
        private System.Windows.Forms.DataGridViewTextBoxColumn dqbb;
        private System.Windows.Forms.DataGridViewTextBoxColumn zs;
        private System.Windows.Forms.DataGridViewTextBoxColumn sjzt;
        private System.Windows.Forms.DataGridViewTextBoxColumn jd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_pl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lb_time;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmb_bbxx;
        private System.Windows.Forms.ComboBox cmb_bbsx;
        private System.Windows.Forms.ComboBox cmb_dqyjbb;
        private System.Windows.Forms.ComboBox cmb_dqbb;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}