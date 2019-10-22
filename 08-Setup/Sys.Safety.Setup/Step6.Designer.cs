namespace Sys.Safety.Setup
{
    partial class Step6
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.groupStandby = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnRTest = new System.Windows.Forms.Button();
            this.txtRIP = new System.Windows.Forms.TextBox();
            this.txtRDatabase = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRDbUserName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRDbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupActive = new System.Windows.Forms.GroupBox();
            this.btnLTest = new System.Windows.Forms.Button();
            this.txtLDatabase = new System.Windows.Forms.TextBox();
            this.txtLDbUserName = new System.Windows.Forms.TextBox();
            this.txtLDbPassword = new System.Windows.Forms.TextBox();
            this.txtLIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupStandby.SuspendLayout();
            this.groupActive.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.groupStandby);
            this.panel1.Controls.Add(this.groupActive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panel1.Size = new System.Drawing.Size(534, 411);
            this.panel1.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(236, 372);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(56, 22);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // groupStandby
            // 
            this.groupStandby.Controls.Add(this.label9);
            this.groupStandby.Controls.Add(this.btnRTest);
            this.groupStandby.Controls.Add(this.txtRIP);
            this.groupStandby.Controls.Add(this.txtRDatabase);
            this.groupStandby.Controls.Add(this.label5);
            this.groupStandby.Controls.Add(this.txtRDbUserName);
            this.groupStandby.Controls.Add(this.label6);
            this.groupStandby.Controls.Add(this.txtRDbPassword);
            this.groupStandby.Controls.Add(this.label7);
            this.groupStandby.Controls.Add(this.label8);
            this.groupStandby.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupStandby.Location = new System.Drawing.Point(0, 159);
            this.groupStandby.Margin = new System.Windows.Forms.Padding(2);
            this.groupStandby.Name = "groupStandby";
            this.groupStandby.Padding = new System.Windows.Forms.Padding(2);
            this.groupStandby.Size = new System.Drawing.Size(534, 197);
            this.groupStandby.TabIndex = 1;
            this.groupStandby.TabStop = false;
            this.groupStandby.Text = "远程机数据库配置";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(265, 107);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(197, 24);
            this.label9.TabIndex = 4;
            this.label9.Text = "请设置相同的本机和备机数据库名\r\n(备机是指相对于本机的另一台电脑)";
            // 
            // btnRTest
            // 
            this.btnRTest.Location = new System.Drawing.Point(81, 102);
            this.btnRTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnRTest.Name = "btnRTest";
            this.btnRTest.Size = new System.Drawing.Size(107, 22);
            this.btnRTest.TabIndex = 3;
            this.btnRTest.Text = "测试连接";
            this.btnRTest.UseVisualStyleBackColor = true;
            this.btnRTest.Click += new System.EventHandler(this.btnRTest_Click);
            // 
            // txtRIP
            // 
            this.txtRIP.Location = new System.Drawing.Point(81, 19);
            this.txtRIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtRIP.Name = "txtRIP";
            this.txtRIP.Size = new System.Drawing.Size(174, 21);
            this.txtRIP.TabIndex = 1;
            // 
            // txtRDatabase
            // 
            this.txtRDatabase.Location = new System.Drawing.Point(347, 54);
            this.txtRDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.txtRDatabase.Name = "txtRDatabase";
            this.txtRDatabase.Size = new System.Drawing.Size(173, 21);
            this.txtRDatabase.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "IP:";
            // 
            // txtRDbUserName
            // 
            this.txtRDbUserName.Location = new System.Drawing.Point(347, 19);
            this.txtRDbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtRDbUserName.Name = "txtRDbUserName";
            this.txtRDbUserName.Size = new System.Drawing.Size(173, 21);
            this.txtRDbUserName.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(265, 24);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "数据库用户名:";
            // 
            // txtRDbPassword
            // 
            this.txtRDbPassword.Location = new System.Drawing.Point(81, 56);
            this.txtRDbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtRDbPassword.Name = "txtRDbPassword";
            this.txtRDbPassword.Size = new System.Drawing.Size(174, 21);
            this.txtRDbPassword.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(265, 58);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "数据库名:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 62);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "数据库密码:";
            // 
            // groupActive
            // 
            this.groupActive.Controls.Add(this.btnLTest);
            this.groupActive.Controls.Add(this.txtLDatabase);
            this.groupActive.Controls.Add(this.txtLDbUserName);
            this.groupActive.Controls.Add(this.txtLDbPassword);
            this.groupActive.Controls.Add(this.txtLIP);
            this.groupActive.Controls.Add(this.label3);
            this.groupActive.Controls.Add(this.label4);
            this.groupActive.Controls.Add(this.label2);
            this.groupActive.Controls.Add(this.label1);
            this.groupActive.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupActive.Location = new System.Drawing.Point(0, 8);
            this.groupActive.Margin = new System.Windows.Forms.Padding(2);
            this.groupActive.Name = "groupActive";
            this.groupActive.Padding = new System.Windows.Forms.Padding(2);
            this.groupActive.Size = new System.Drawing.Size(534, 151);
            this.groupActive.TabIndex = 0;
            this.groupActive.TabStop = false;
            this.groupActive.Text = "本机数据库配置";
            // 
            // btnLTest
            // 
            this.btnLTest.Location = new System.Drawing.Point(81, 99);
            this.btnLTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnLTest.Name = "btnLTest";
            this.btnLTest.Size = new System.Drawing.Size(107, 22);
            this.btnLTest.TabIndex = 3;
            this.btnLTest.Text = "测试连接";
            this.btnLTest.UseVisualStyleBackColor = true;
            this.btnLTest.Click += new System.EventHandler(this.btnLTest_Click);
            // 
            // txtLDatabase
            // 
            this.txtLDatabase.Location = new System.Drawing.Point(347, 51);
            this.txtLDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.txtLDatabase.Name = "txtLDatabase";
            this.txtLDatabase.Size = new System.Drawing.Size(173, 21);
            this.txtLDatabase.TabIndex = 2;
            // 
            // txtLDbUserName
            // 
            this.txtLDbUserName.Location = new System.Drawing.Point(347, 16);
            this.txtLDbUserName.Margin = new System.Windows.Forms.Padding(2);
            this.txtLDbUserName.Name = "txtLDbUserName";
            this.txtLDbUserName.Size = new System.Drawing.Size(173, 21);
            this.txtLDbUserName.TabIndex = 2;
            // 
            // txtLDbPassword
            // 
            this.txtLDbPassword.Location = new System.Drawing.Point(81, 53);
            this.txtLDbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtLDbPassword.Name = "txtLDbPassword";
            this.txtLDbPassword.Size = new System.Drawing.Size(174, 21);
            this.txtLDbPassword.TabIndex = 1;
            // 
            // txtLIP
            // 
            this.txtLIP.Location = new System.Drawing.Point(81, 16);
            this.txtLIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtLIP.Name = "txtLIP";
            this.txtLIP.Size = new System.Drawing.Size(174, 21);
            this.txtLIP.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "数据库密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(265, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "数据库名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据库用户名:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // Step6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Step6";
            this.ShowIcon = false;
            this.Text = "热备配置";
            this.Load += new System.EventHandler(this.Step6_Load);
            this.Shown += new System.EventHandler(this.Step6_Shown);
            this.panel1.ResumeLayout(false);
            this.groupStandby.ResumeLayout(false);
            this.groupStandby.PerformLayout();
            this.groupActive.ResumeLayout(false);
            this.groupActive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupStandby;
        private System.Windows.Forms.GroupBox groupActive;
        private System.Windows.Forms.TextBox txtLIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLDbUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLDatabase;
        private System.Windows.Forms.TextBox txtLDbPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLTest;
        private System.Windows.Forms.Button btnRTest;
        private System.Windows.Forms.TextBox txtRIP;
        private System.Windows.Forms.TextBox txtRDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRDbUserName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRDbPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label9;
    }
}