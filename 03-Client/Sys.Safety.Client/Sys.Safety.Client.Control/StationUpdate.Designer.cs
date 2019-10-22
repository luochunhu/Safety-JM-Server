namespace Sys.Safety.Client.Control
{
    partial class StationUpdate
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_refrush = new System.Windows.Forms.CheckBox();
            this.btn_showRecive = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
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
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_cancleUpdate = new System.Windows.Forms.Button();
            this.btn_revert = new System.Windows.Forms.Button();
            this.btn_requestUpdate = new System.Windows.Forms.Button();
            this.btn_reStart = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_send = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_minVersion = new System.Windows.Forms.TextBox();
            this.txt_maxVersion = new System.Windows.Forms.TextBox();
            this.txt_hardVersion = new System.Windows.Forms.TextBox();
            this.txt_softVersion = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_sb = new System.Windows.Forms.TextBox();
            this.btn_view = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cb_refrush);
            this.groupBox2.Controls.Add(this.btn_showRecive);
            this.groupBox2.Controls.Add(this.dgv);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btn_cancleUpdate);
            this.groupBox2.Controls.Add(this.btn_revert);
            this.groupBox2.Controls.Add(this.btn_requestUpdate);
            this.groupBox2.Controls.Add(this.btn_reStart);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btn_send);
            this.groupBox2.Location = new System.Drawing.Point(8, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(898, 485);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "升级操作";
            // 
            // cb_refrush
            // 
            this.cb_refrush.AutoSize = true;
            this.cb_refrush.Location = new System.Drawing.Point(158, 122);
            this.cb_refrush.Name = "cb_refrush";
            this.cb_refrush.Size = new System.Drawing.Size(72, 16);
            this.cb_refrush.TabIndex = 22;
            this.cb_refrush.Text = "自动刷新";
            this.cb_refrush.UseVisualStyleBackColor = true;
            this.cb_refrush.CheckedChanged += new System.EventHandler(this.cb_refrush_CheckedChanged);
            // 
            // btn_showRecive
            // 
            this.btn_showRecive.Location = new System.Drawing.Point(796, 25);
            this.btn_showRecive.Name = "btn_showRecive";
            this.btn_showRecive.Size = new System.Drawing.Size(89, 23);
            this.btn_showRecive.TabIndex = 21;
            this.btn_showRecive.Text = "查看版本信息";
            this.btn_showRecive.UseVisualStyleBackColor = true;
            this.btn_showRecive.Click += new System.EventHandler(this.btn_showRecive_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dgv.Location = new System.Drawing.Point(3, 144);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(889, 334);
            this.dgv.TabIndex = 7;
            // 
            // sx
            // 
            this.sx.FalseValue = "0";
            this.sx.HeaderText = "筛选";
            this.sx.Name = "sx";
            this.sx.TrueValue = "1";
            this.sx.Width = 50;
            // 
            // xh
            // 
            this.xh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.xh.HeaderText = "序号";
            this.xh.Name = "xh";
            this.xh.Width = 60;
            // 
            // dz
            // 
            this.dz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dz.HeaderText = "设备地址";
            this.dz.Name = "dz";
            this.dz.Width = 80;
            // 
            // wz
            // 
            this.wz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.wz.HeaderText = "安装位置";
            this.wz.Name = "wz";
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.type.HeaderText = "设备类型";
            this.type.Name = "type";
            // 
            // txzt
            // 
            this.txzt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.txzt.HeaderText = "通讯状态";
            this.txzt.Name = "txzt";
            this.txzt.Width = 80;
            // 
            // dqbb
            // 
            this.dqbb.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dqbb.HeaderText = "当前版本（软|硬）";
            this.dqbb.Name = "dqbb";
            this.dqbb.Width = 130;
            // 
            // zs
            // 
            this.zs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.zs.HeaderText = "下发帧数";
            this.zs.Name = "zs";
            this.zs.Width = 80;
            // 
            // sjzt
            // 
            this.sjzt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sjzt.HeaderText = "升级状态";
            this.sjzt.Name = "sjzt";
            // 
            // jd
            // 
            this.jd.HeaderText = "消息";
            this.jd.Name = "jd";
            this.jd.Width = 110;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(560, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "告诉设备将接收到的文件丢弃";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(560, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(329, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "当设备升级完成后，需要取消这次升级恢复成之前的版本程序";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(95, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(263, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "向分站发送请求升级命令,需要在列表中勾选分站";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(2, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "设备升级的进度状态浏览表";
            // 
            // btn_cancleUpdate
            // 
            this.btn_cancleUpdate.Location = new System.Drawing.Point(469, 60);
            this.btn_cancleUpdate.Name = "btn_cancleUpdate";
            this.btn_cancleUpdate.Size = new System.Drawing.Size(75, 23);
            this.btn_cancleUpdate.TabIndex = 12;
            this.btn_cancleUpdate.Text = "取消升级";
            this.btn_cancleUpdate.UseVisualStyleBackColor = true;
            this.btn_cancleUpdate.Click += new System.EventHandler(this.btn_cancleUpdate_Click);
            // 
            // btn_revert
            // 
            this.btn_revert.Location = new System.Drawing.Point(469, 90);
            this.btn_revert.Name = "btn_revert";
            this.btn_revert.Size = new System.Drawing.Size(75, 23);
            this.btn_revert.TabIndex = 13;
            this.btn_revert.Text = "版本还原";
            this.btn_revert.UseVisualStyleBackColor = true;
            this.btn_revert.Click += new System.EventHandler(this.btn_revert_Click);
            // 
            // btn_requestUpdate
            // 
            this.btn_requestUpdate.Location = new System.Drawing.Point(6, 31);
            this.btn_requestUpdate.Name = "btn_requestUpdate";
            this.btn_requestUpdate.Size = new System.Drawing.Size(75, 23);
            this.btn_requestUpdate.TabIndex = 18;
            this.btn_requestUpdate.Text = "请求升级";
            this.btn_requestUpdate.UseVisualStyleBackColor = true;
            this.btn_requestUpdate.Click += new System.EventHandler(this.btn_requestUpdate_Click);
            // 
            // btn_reStart
            // 
            this.btn_reStart.Location = new System.Drawing.Point(6, 90);
            this.btn_reStart.Name = "btn_reStart";
            this.btn_reStart.Size = new System.Drawing.Size(75, 23);
            this.btn_reStart.TabIndex = 11;
            this.btn_reStart.Text = "重启升级";
            this.btn_reStart.UseVisualStyleBackColor = true;
            this.btn_reStart.Click += new System.EventHandler(this.btn_reStart_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(95, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(293, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "当设备接收完升级文件后，告诉设备可以重启进行升级";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(95, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(221, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "将需要升级的文件版本程序下发的设备上";
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(6, 60);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 10;
            this.btn_send.Text = "下发文件";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txt_minVersion);
            this.groupBox1.Controls.Add(this.txt_maxVersion);
            this.groupBox1.Controls.Add(this.txt_hardVersion);
            this.groupBox1.Controls.Add(this.txt_softVersion);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cmb_type);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_sb);
            this.groupBox1.Controls.Add(this.btn_view);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(894, 150);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本基础信息";
            // 
            // txt_minVersion
            // 
            this.txt_minVersion.Location = new System.Drawing.Point(368, 110);
            this.txt_minVersion.Name = "txt_minVersion";
            this.txt_minVersion.Size = new System.Drawing.Size(100, 21);
            this.txt_minVersion.TabIndex = 35;
            this.txt_minVersion.Text = "2.2";
            // 
            // txt_maxVersion
            // 
            this.txt_maxVersion.Location = new System.Drawing.Point(122, 110);
            this.txt_maxVersion.Name = "txt_maxVersion";
            this.txt_maxVersion.Size = new System.Drawing.Size(100, 21);
            this.txt_maxVersion.TabIndex = 35;
            this.txt_maxVersion.Text = "2.6";
            // 
            // txt_hardVersion
            // 
            this.txt_hardVersion.Location = new System.Drawing.Point(369, 82);
            this.txt_hardVersion.Name = "txt_hardVersion";
            this.txt_hardVersion.Size = new System.Drawing.Size(100, 21);
            this.txt_hardVersion.TabIndex = 35;
            // 
            // txt_softVersion
            // 
            this.txt_softVersion.Location = new System.Drawing.Point(122, 82);
            this.txt_softVersion.Name = "txt_softVersion";
            this.txt_softVersion.Size = new System.Drawing.Size(100, 21);
            this.txt_softVersion.TabIndex = 35;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(32, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 12);
            this.label13.TabIndex = 26;
            this.label13.Text = "升级版本上限：";
            // 
            // cmb_type
            // 
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Items.AddRange(new object[] {
            "2.KJ306-F(16)H本安型分站",
            "22.KJ306-F(16)本安型分站",
            "38.KJ306-F(16)H本安型分站",
            "82.KJ306-F(16)H本安型读卡分站",
            "194.KJ306-F(16)H本安型顶板分站"});
            this.cmb_type.Location = new System.Drawing.Point(665, 83);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(220, 20);
            this.cmb_type.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(237, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "需升级的硬件版本号：";
            // 
            // txt_sb
            // 
            this.txt_sb.Enabled = false;
            this.txt_sb.Location = new System.Drawing.Point(21, 26);
            this.txt_sb.Name = "txt_sb";
            this.txt_sb.Size = new System.Drawing.Size(661, 21);
            this.txt_sb.TabIndex = 1;
            this.txt_sb.Text = "请点击浏览选择升级文件";
            // 
            // btn_view
            // 
            this.btn_view.Location = new System.Drawing.Point(714, 25);
            this.btn_view.Name = "btn_view";
            this.btn_view.Size = new System.Drawing.Size(75, 23);
            this.btn_view.TabIndex = 2;
            this.btn_view.Text = "浏览";
            this.btn_view.UseVisualStyleBackColor = true;
            this.btn_view.Click += new System.EventHandler(this.btn_view_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(6, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "升级的软件版本号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(485, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "选择或输入升级的设备类型：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(19, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "总帧数【】";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(276, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 12);
            this.label11.TabIndex = 28;
            this.label11.Text = "升级版本下限：";
            // 
            // StationUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 659);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StationUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分站远程升级";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StationUpdate_FormClosed);
            this.Load += new System.EventHandler(this.StationUpdate_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_showRecive;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_cancleUpdate;
        private System.Windows.Forms.Button btn_revert;
        private System.Windows.Forms.Button btn_requestUpdate;
        private System.Windows.Forms.Button btn_reStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_minVersion;
        private System.Windows.Forms.TextBox txt_maxVersion;
        private System.Windows.Forms.TextBox txt_hardVersion;
        private System.Windows.Forms.TextBox txt_softVersion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmb_type;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_sb;
        private System.Windows.Forms.Button btn_view;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_refrush;
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
        private System.Windows.Forms.Label label11;
    }
}