namespace Sys.Safety.Setup
{
    partial class Step3
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
            this.lblDBIP = new System.Windows.Forms.Label();
            this.txtDBIP = new System.Windows.Forms.TextBox();
            this.lblDBUser = new System.Windows.Forms.Label();
            this.txtDBUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.lblDBName = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.groupDBConfig = new System.Windows.Forms.GroupBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.groupVirtualIPConfig = new System.Windows.Forms.GroupBox();
            this.lblVirtualIP = new System.Windows.Forms.Label();
            this.txtVirtualIP = new System.Windows.Forms.TextBox();
            this.groupDataAnalysisIPConfig = new System.Windows.Forms.GroupBox();
            this.lblServerSideIP = new System.Windows.Forms.Label();
            this.txtAnalysisServerSideIP = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.groupDBConfig.SuspendLayout();
            this.groupVirtualIPConfig.SuspendLayout();
            this.groupDataAnalysisIPConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDBIP
            // 
            this.lblDBIP.AutoSize = true;
            this.lblDBIP.Location = new System.Drawing.Point(4, 25);
            this.lblDBIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDBIP.Name = "lblDBIP";
            this.lblDBIP.Size = new System.Drawing.Size(59, 12);
            this.lblDBIP.TabIndex = 0;
            this.lblDBIP.Text = "数据库IP:";
            // 
            // txtDBIP
            // 
            this.txtDBIP.Location = new System.Drawing.Point(74, 22);
            this.txtDBIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtDBIP.Name = "txtDBIP";
            this.txtDBIP.Size = new System.Drawing.Size(174, 21);
            this.txtDBIP.TabIndex = 1;
            // 
            // lblDBUser
            // 
            this.lblDBUser.AutoSize = true;
            this.lblDBUser.Location = new System.Drawing.Point(262, 25);
            this.lblDBUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDBUser.Name = "lblDBUser";
            this.lblDBUser.Size = new System.Drawing.Size(83, 12);
            this.lblDBUser.TabIndex = 2;
            this.lblDBUser.Text = "数据库用户名:";
            // 
            // txtDBUser
            // 
            this.txtDBUser.Location = new System.Drawing.Point(346, 22);
            this.txtDBUser.Margin = new System.Windows.Forms.Padding(2);
            this.txtDBUser.Name = "txtDBUser";
            this.txtDBUser.Size = new System.Drawing.Size(177, 21);
            this.txtDBUser.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "数据库密码:";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(74, 60);
            this.txtDBPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(174, 21);
            this.txtDBPassword.TabIndex = 5;
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(262, 62);
            this.lblDBName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(59, 12);
            this.lblDBName.TabIndex = 2;
            this.lblDBName.Text = "数据库名:";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(346, 60);
            this.txtDBName.Margin = new System.Windows.Forms.Padding(2);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(177, 21);
            this.txtDBName.TabIndex = 3;
            // 
            // groupDBConfig
            // 
            this.groupDBConfig.Controls.Add(this.btnTestConnection);
            this.groupDBConfig.Controls.Add(this.lblDBUser);
            this.groupDBConfig.Controls.Add(this.txtDBPassword);
            this.groupDBConfig.Controls.Add(this.lblDBIP);
            this.groupDBConfig.Controls.Add(this.label1);
            this.groupDBConfig.Controls.Add(this.txtDBIP);
            this.groupDBConfig.Controls.Add(this.txtDBName);
            this.groupDBConfig.Controls.Add(this.lblDBName);
            this.groupDBConfig.Controls.Add(this.txtDBUser);
            this.groupDBConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDBConfig.Location = new System.Drawing.Point(0, 0);
            this.groupDBConfig.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupDBConfig.Name = "groupDBConfig";
            this.groupDBConfig.Padding = new System.Windows.Forms.Padding(2);
            this.groupDBConfig.Size = new System.Drawing.Size(534, 133);
            this.groupDBConfig.TabIndex = 6;
            this.groupDBConfig.TabStop = false;
            this.groupDBConfig.Text = "数据库配置";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(74, 93);
            this.btnTestConnection.Margin = new System.Windows.Forms.Padding(2);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(104, 25);
            this.btnTestConnection.TabIndex = 6;
            this.btnTestConnection.Text = "测试连接";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // groupVirtualIPConfig
            // 
            this.groupVirtualIPConfig.Controls.Add(this.lblVirtualIP);
            this.groupVirtualIPConfig.Controls.Add(this.txtVirtualIP);
            this.groupVirtualIPConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupVirtualIPConfig.Location = new System.Drawing.Point(0, 133);
            this.groupVirtualIPConfig.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupVirtualIPConfig.Name = "groupVirtualIPConfig";
            this.groupVirtualIPConfig.Padding = new System.Windows.Forms.Padding(2);
            this.groupVirtualIPConfig.Size = new System.Drawing.Size(534, 57);
            this.groupVirtualIPConfig.TabIndex = 7;
            this.groupVirtualIPConfig.TabStop = false;
            this.groupVirtualIPConfig.Text = "通讯虚拟IP配置(主备机虚拟IP需设置一致)";
            // 
            // lblVirtualIP
            // 
            this.lblVirtualIP.AutoSize = true;
            this.lblVirtualIP.Location = new System.Drawing.Point(4, 27);
            this.lblVirtualIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVirtualIP.Name = "lblVirtualIP";
            this.lblVirtualIP.Size = new System.Drawing.Size(71, 12);
            this.lblVirtualIP.TabIndex = 0;
            this.lblVirtualIP.Text = "通讯虚拟IP:";
            // 
            // txtVirtualIP
            // 
            this.txtVirtualIP.Location = new System.Drawing.Point(74, 23);
            this.txtVirtualIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtVirtualIP.Name = "txtVirtualIP";
            this.txtVirtualIP.Size = new System.Drawing.Size(174, 21);
            this.txtVirtualIP.TabIndex = 5;
            // 
            // groupDataAnalysisIPConfig
            // 
            this.groupDataAnalysisIPConfig.Controls.Add(this.lblServerSideIP);
            this.groupDataAnalysisIPConfig.Controls.Add(this.txtAnalysisServerSideIP);
            this.groupDataAnalysisIPConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupDataAnalysisIPConfig.Location = new System.Drawing.Point(0, 190);
            this.groupDataAnalysisIPConfig.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupDataAnalysisIPConfig.Name = "groupDataAnalysisIPConfig";
            this.groupDataAnalysisIPConfig.Padding = new System.Windows.Forms.Padding(2);
            this.groupDataAnalysisIPConfig.Size = new System.Drawing.Size(534, 62);
            this.groupDataAnalysisIPConfig.TabIndex = 8;
            this.groupDataAnalysisIPConfig.TabStop = false;
            this.groupDataAnalysisIPConfig.Text = "大数据分析服务端IP配置";
            // 
            // lblServerSideIP
            // 
            this.lblServerSideIP.AutoSize = true;
            this.lblServerSideIP.Location = new System.Drawing.Point(4, 29);
            this.lblServerSideIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblServerSideIP.Name = "lblServerSideIP";
            this.lblServerSideIP.Size = new System.Drawing.Size(59, 12);
            this.lblServerSideIP.TabIndex = 0;
            this.lblServerSideIP.Text = "服务端IP:";
            // 
            // txtAnalysisServerSideIP
            // 
            this.txtAnalysisServerSideIP.Location = new System.Drawing.Point(74, 24);
            this.txtAnalysisServerSideIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtAnalysisServerSideIP.Name = "txtAnalysisServerSideIP";
            this.txtAnalysisServerSideIP.Size = new System.Drawing.Size(174, 21);
            this.txtAnalysisServerSideIP.TabIndex = 5;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(236, 362);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(71, 22);
            this.btnNext.TabIndex = 9;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // Step3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.ControlBox = false;
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupDataAnalysisIPConfig);
            this.Controls.Add(this.groupVirtualIPConfig);
            this.Controls.Add(this.groupDBConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Step3";
            this.ShowIcon = false;
            this.Text = "配置";
            this.Load += new System.EventHandler(this.Step3_Load);
            this.VisibleChanged += new System.EventHandler(this.Step3_VisibleChanged);
            this.groupDBConfig.ResumeLayout(false);
            this.groupDBConfig.PerformLayout();
            this.groupVirtualIPConfig.ResumeLayout(false);
            this.groupVirtualIPConfig.PerformLayout();
            this.groupDataAnalysisIPConfig.ResumeLayout(false);
            this.groupDataAnalysisIPConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDBIP;
        private System.Windows.Forms.TextBox txtDBIP;
        private System.Windows.Forms.Label lblDBUser;
        private System.Windows.Forms.TextBox txtDBUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.GroupBox groupDBConfig;
        private System.Windows.Forms.GroupBox groupVirtualIPConfig;
        private System.Windows.Forms.Label lblVirtualIP;
        private System.Windows.Forms.TextBox txtVirtualIP;
        private System.Windows.Forms.GroupBox groupDataAnalysisIPConfig;
        private System.Windows.Forms.Label lblServerSideIP;
        private System.Windows.Forms.TextBox txtAnalysisServerSideIP;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnTestConnection;
    }
}