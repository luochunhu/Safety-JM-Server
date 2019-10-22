namespace Sys.Safety.Setup
{
    partial class Testing
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
            this.btnCreateShortcut = new System.Windows.Forms.Button();
            this.btnCheckMap = new System.Windows.Forms.Button();
            this.btnCheckBit = new System.Windows.Forms.Button();
            this.btnGetUninstallList = new System.Windows.Forms.Button();
            this.UninstallList = new System.Windows.Forms.ListBox();
            this.btnExecuteEXE = new System.Windows.Forms.Button();
            this.btnReadConfiguration = new System.Windows.Forms.Button();
            this.btn_MYSQLINI = new System.Windows.Forms.Button();
            this.btnRestarMySQL = new System.Windows.Forms.Button();
            this.btnSyncAccount = new System.Windows.Forms.Button();
            this.btnQueryMasterStatus = new System.Windows.Forms.Button();
            this.btnSlave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateShortcut
            // 
            this.btnCreateShortcut.Location = new System.Drawing.Point(24, 23);
            this.btnCreateShortcut.Name = "btnCreateShortcut";
            this.btnCreateShortcut.Size = new System.Drawing.Size(118, 31);
            this.btnCreateShortcut.TabIndex = 0;
            this.btnCreateShortcut.Text = "创建快捷菜单";
            this.btnCreateShortcut.UseVisualStyleBackColor = true;
            this.btnCreateShortcut.Click += new System.EventHandler(this.btnCreateShortcut_Click);
            // 
            // btnCheckMap
            // 
            this.btnCheckMap.Location = new System.Drawing.Point(170, 23);
            this.btnCheckMap.Name = "btnCheckMap";
            this.btnCheckMap.Size = new System.Drawing.Size(190, 31);
            this.btnCheckMap.TabIndex = 1;
            this.btnCheckMap.Text = "检查元图控件有没有安装";
            this.btnCheckMap.UseVisualStyleBackColor = true;
            this.btnCheckMap.Click += new System.EventHandler(this.btnCheckMap_Click);
            // 
            // btnCheckBit
            // 
            this.btnCheckBit.Location = new System.Drawing.Point(389, 23);
            this.btnCheckBit.Name = "btnCheckBit";
            this.btnCheckBit.Size = new System.Drawing.Size(154, 31);
            this.btnCheckBit.TabIndex = 2;
            this.btnCheckBit.Text = "判断是x86还是x64";
            this.btnCheckBit.UseVisualStyleBackColor = true;
            this.btnCheckBit.Click += new System.EventHandler(this.btnCheckBit_Click);
            // 
            // btnGetUninstallList
            // 
            this.btnGetUninstallList.Location = new System.Drawing.Point(24, 81);
            this.btnGetUninstallList.Name = "btnGetUninstallList";
            this.btnGetUninstallList.Size = new System.Drawing.Size(118, 29);
            this.btnGetUninstallList.TabIndex = 3;
            this.btnGetUninstallList.Text = "获取卸载列表";
            this.btnGetUninstallList.UseVisualStyleBackColor = true;
            this.btnGetUninstallList.Click += new System.EventHandler(this.btnGetUninstallList_Click);
            // 
            // UninstallList
            // 
            this.UninstallList.FormattingEnabled = true;
            this.UninstallList.ItemHeight = 15;
            this.UninstallList.Location = new System.Drawing.Point(24, 250);
            this.UninstallList.Name = "UninstallList";
            this.UninstallList.Size = new System.Drawing.Size(1096, 304);
            this.UninstallList.TabIndex = 4;
            // 
            // btnExecuteEXE
            // 
            this.btnExecuteEXE.Location = new System.Drawing.Point(170, 81);
            this.btnExecuteEXE.Name = "btnExecuteEXE";
            this.btnExecuteEXE.Size = new System.Drawing.Size(190, 29);
            this.btnExecuteEXE.TabIndex = 5;
            this.btnExecuteEXE.Text = "执行程序";
            this.btnExecuteEXE.UseVisualStyleBackColor = true;
            this.btnExecuteEXE.Click += new System.EventHandler(this.btnExecuteEXE_Click);
            // 
            // btnReadConfiguration
            // 
            this.btnReadConfiguration.Enabled = false;
            this.btnReadConfiguration.Location = new System.Drawing.Point(389, 81);
            this.btnReadConfiguration.Name = "btnReadConfiguration";
            this.btnReadConfiguration.Size = new System.Drawing.Size(154, 29);
            this.btnReadConfiguration.TabIndex = 6;
            this.btnReadConfiguration.Text = "读取配置";
            this.btnReadConfiguration.UseVisualStyleBackColor = true;
            this.btnReadConfiguration.Click += new System.EventHandler(this.btnReadConfiguration_Click);
            // 
            // btn_MYSQLINI
            // 
            this.btn_MYSQLINI.Enabled = false;
            this.btn_MYSQLINI.Location = new System.Drawing.Point(596, 23);
            this.btn_MYSQLINI.Name = "btn_MYSQLINI";
            this.btn_MYSQLINI.Size = new System.Drawing.Size(180, 31);
            this.btn_MYSQLINI.TabIndex = 7;
            this.btn_MYSQLINI.Text = "修改MYSQL-my.ini文件";
            this.btn_MYSQLINI.UseVisualStyleBackColor = true;
            this.btn_MYSQLINI.Click += new System.EventHandler(this.btn_MYSQLINI_Click);
            // 
            // btnRestarMySQL
            // 
            this.btnRestarMySQL.Enabled = false;
            this.btnRestarMySQL.Location = new System.Drawing.Point(596, 81);
            this.btnRestarMySQL.Name = "btnRestarMySQL";
            this.btnRestarMySQL.Size = new System.Drawing.Size(180, 29);
            this.btnRestarMySQL.TabIndex = 8;
            this.btnRestarMySQL.Text = "重启MYSQL服务";
            this.btnRestarMySQL.UseVisualStyleBackColor = true;
            this.btnRestarMySQL.Click += new System.EventHandler(this.btnRestarMySQL_Click);
            // 
            // btnSyncAccount
            // 
            this.btnSyncAccount.Enabled = false;
            this.btnSyncAccount.Location = new System.Drawing.Point(24, 138);
            this.btnSyncAccount.Name = "btnSyncAccount";
            this.btnSyncAccount.Size = new System.Drawing.Size(118, 29);
            this.btnSyncAccount.TabIndex = 9;
            this.btnSyncAccount.Text = "创建同步账号";
            this.btnSyncAccount.UseVisualStyleBackColor = true;
            this.btnSyncAccount.Click += new System.EventHandler(this.btnSyncAccount_Click);
            // 
            // btnQueryMasterStatus
            // 
            this.btnQueryMasterStatus.Enabled = false;
            this.btnQueryMasterStatus.Location = new System.Drawing.Point(170, 138);
            this.btnQueryMasterStatus.Name = "btnQueryMasterStatus";
            this.btnQueryMasterStatus.Size = new System.Drawing.Size(190, 29);
            this.btnQueryMasterStatus.TabIndex = 10;
            this.btnQueryMasterStatus.Text = "查询master的状态";
            this.btnQueryMasterStatus.UseVisualStyleBackColor = true;
            this.btnQueryMasterStatus.Click += new System.EventHandler(this.btnQueryMasterStatus_Click);
            // 
            // btnSlave
            // 
            this.btnSlave.Enabled = false;
            this.btnSlave.Location = new System.Drawing.Point(389, 138);
            this.btnSlave.Name = "btnSlave";
            this.btnSlave.Size = new System.Drawing.Size(154, 29);
            this.btnSlave.TabIndex = 11;
            this.btnSlave.Text = "查询slave状态";
            this.btnSlave.UseVisualStyleBackColor = true;
            this.btnSlave.Click += new System.EventHandler(this.btnSlave_Click);
            // 
            // Testing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 566);
            this.Controls.Add(this.btnSlave);
            this.Controls.Add(this.btnQueryMasterStatus);
            this.Controls.Add(this.btnSyncAccount);
            this.Controls.Add(this.btnRestarMySQL);
            this.Controls.Add(this.btn_MYSQLINI);
            this.Controls.Add(this.btnReadConfiguration);
            this.Controls.Add(this.btnExecuteEXE);
            this.Controls.Add(this.UninstallList);
            this.Controls.Add(this.btnGetUninstallList);
            this.Controls.Add(this.btnCheckBit);
            this.Controls.Add(this.btnCheckMap);
            this.Controls.Add(this.btnCreateShortcut);
            this.Name = "Testing";
            this.Text = "Testing";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateShortcut;
        private System.Windows.Forms.Button btnCheckMap;
        private System.Windows.Forms.Button btnCheckBit;
        private System.Windows.Forms.Button btnGetUninstallList;
        private System.Windows.Forms.ListBox UninstallList;
        private System.Windows.Forms.Button btnExecuteEXE;
        private System.Windows.Forms.Button btnReadConfiguration;
        private System.Windows.Forms.Button btn_MYSQLINI;
        private System.Windows.Forms.Button btnRestarMySQL;
        private System.Windows.Forms.Button btnSyncAccount;
        private System.Windows.Forms.Button btnQueryMasterStatus;
        private System.Windows.Forms.Button btnSlave;
    }
}