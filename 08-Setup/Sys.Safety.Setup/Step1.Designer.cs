namespace Sys.Safety.Setup
{
    partial class Step1
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
            this.installFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.PanelInstallFolder = new System.Windows.Forms.Panel();
            this.btnDirectoryBrower = new System.Windows.Forms.Button();
            this.txtInstallFolder = new System.Windows.Forms.TextBox();
            this.lblInstallFolder = new System.Windows.Forms.Label();
            this.PanelInstallLicence = new System.Windows.Forms.Panel();
            this.btnGetMachineCode = new System.Windows.Forms.Button();
            this.btnLicenceBrower = new System.Windows.Forms.Button();
            this.txtLicencePath = new System.Windows.Forms.TextBox();
            this.lblChooseLicence = new System.Windows.Forms.Label();
            this.openLicenceFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.lblTotalStep = new System.Windows.Forms.Label();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.checkedInstallItems = new System.Windows.Forms.CheckedListBox();
            this.PanelItems = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.PanelInstallFolder.SuspendLayout();
            this.PanelInstallLicence.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.PanelItems.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelInstallFolder
            // 
            this.PanelInstallFolder.Controls.Add(this.btnDirectoryBrower);
            this.PanelInstallFolder.Controls.Add(this.txtInstallFolder);
            this.PanelInstallFolder.Controls.Add(this.lblInstallFolder);
            this.PanelInstallFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelInstallFolder.Location = new System.Drawing.Point(0, 0);
            this.PanelInstallFolder.Margin = new System.Windows.Forms.Padding(2);
            this.PanelInstallFolder.Name = "PanelInstallFolder";
            this.PanelInstallFolder.Size = new System.Drawing.Size(534, 32);
            this.PanelInstallFolder.TabIndex = 0;
            // 
            // btnDirectoryBrower
            // 
            this.btnDirectoryBrower.Location = new System.Drawing.Point(344, 5);
            this.btnDirectoryBrower.Margin = new System.Windows.Forms.Padding(2);
            this.btnDirectoryBrower.Name = "btnDirectoryBrower";
            this.btnDirectoryBrower.Size = new System.Drawing.Size(71, 22);
            this.btnDirectoryBrower.TabIndex = 2;
            this.btnDirectoryBrower.Text = "浏览";
            this.btnDirectoryBrower.UseVisualStyleBackColor = true;
            this.btnDirectoryBrower.Click += new System.EventHandler(this.btnDirectoryBrower_Click);
            // 
            // txtInstallFolder
            // 
            this.txtInstallFolder.Location = new System.Drawing.Point(88, 6);
            this.txtInstallFolder.Margin = new System.Windows.Forms.Padding(2);
            this.txtInstallFolder.Name = "txtInstallFolder";
            this.txtInstallFolder.ReadOnly = true;
            this.txtInstallFolder.Size = new System.Drawing.Size(251, 21);
            this.txtInstallFolder.TabIndex = 1;
            // 
            // lblInstallFolder
            // 
            this.lblInstallFolder.AutoSize = true;
            this.lblInstallFolder.Location = new System.Drawing.Point(2, 10);
            this.lblInstallFolder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInstallFolder.Name = "lblInstallFolder";
            this.lblInstallFolder.Size = new System.Drawing.Size(83, 12);
            this.lblInstallFolder.TabIndex = 0;
            this.lblInstallFolder.Text = "选择安装目录:";
            // 
            // PanelInstallLicence
            // 
            this.PanelInstallLicence.Controls.Add(this.btnGetMachineCode);
            this.PanelInstallLicence.Controls.Add(this.btnLicenceBrower);
            this.PanelInstallLicence.Controls.Add(this.txtLicencePath);
            this.PanelInstallLicence.Controls.Add(this.lblChooseLicence);
            this.PanelInstallLicence.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelInstallLicence.Location = new System.Drawing.Point(0, 32);
            this.PanelInstallLicence.Margin = new System.Windows.Forms.Padding(2);
            this.PanelInstallLicence.Name = "PanelInstallLicence";
            this.PanelInstallLicence.Size = new System.Drawing.Size(534, 33);
            this.PanelInstallLicence.TabIndex = 1;
            // 
            // btnGetMachineCode
            // 
            this.btnGetMachineCode.Location = new System.Drawing.Point(420, 6);
            this.btnGetMachineCode.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetMachineCode.Name = "btnGetMachineCode";
            this.btnGetMachineCode.Size = new System.Drawing.Size(88, 22);
            this.btnGetMachineCode.TabIndex = 2;
            this.btnGetMachineCode.Text = "获取机器码";
            this.btnGetMachineCode.UseVisualStyleBackColor = true;
            this.btnGetMachineCode.Click += new System.EventHandler(this.btnGetMachineCode_Click);
            // 
            // btnLicenceBrower
            // 
            this.btnLicenceBrower.Location = new System.Drawing.Point(344, 6);
            this.btnLicenceBrower.Margin = new System.Windows.Forms.Padding(2);
            this.btnLicenceBrower.Name = "btnLicenceBrower";
            this.btnLicenceBrower.Size = new System.Drawing.Size(71, 22);
            this.btnLicenceBrower.TabIndex = 2;
            this.btnLicenceBrower.Text = "浏览";
            this.btnLicenceBrower.UseVisualStyleBackColor = true;
            this.btnLicenceBrower.Click += new System.EventHandler(this.btnLicenceBrower_Click);
            // 
            // txtLicencePath
            // 
            this.txtLicencePath.Location = new System.Drawing.Point(88, 6);
            this.txtLicencePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtLicencePath.Name = "txtLicencePath";
            this.txtLicencePath.ReadOnly = true;
            this.txtLicencePath.Size = new System.Drawing.Size(251, 21);
            this.txtLicencePath.TabIndex = 1;
            // 
            // lblChooseLicence
            // 
            this.lblChooseLicence.AutoSize = true;
            this.lblChooseLicence.Location = new System.Drawing.Point(3, 10);
            this.lblChooseLicence.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblChooseLicence.Name = "lblChooseLicence";
            this.lblChooseLicence.Size = new System.Drawing.Size(83, 12);
            this.lblChooseLicence.TabIndex = 0;
            this.lblChooseLicence.Text = "选择安装证书:";
            // 
            // openLicenceFileDialog
            // 
            this.openLicenceFileDialog.FileName = "i.License";
            this.openLicenceFileDialog.Filter = "License文件(*.License)|*.License";
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.lblTotalStep);
            this.PanelBottom.Controls.Add(this.lblCurrentStep);
            this.PanelBottom.Controls.Add(this.btnNext);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelBottom.Location = new System.Drawing.Point(0, 373);
            this.PanelBottom.Margin = new System.Windows.Forms.Padding(2);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(534, 38);
            this.PanelBottom.TabIndex = 3;
            // 
            // lblTotalStep
            // 
            this.lblTotalStep.AutoSize = true;
            this.lblTotalStep.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalStep.Location = new System.Drawing.Point(18, 8);
            this.lblTotalStep.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalStep.Name = "lblTotalStep";
            this.lblTotalStep.Size = new System.Drawing.Size(19, 12);
            this.lblTotalStep.TabIndex = 2;
            this.lblTotalStep.Text = "/6";
            this.lblTotalStep.Visible = false;
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.AutoSize = true;
            this.lblCurrentStep.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentStep.ForeColor = System.Drawing.Color.Orange;
            this.lblCurrentStep.Location = new System.Drawing.Point(10, 8);
            this.lblCurrentStep.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Size = new System.Drawing.Size(12, 12);
            this.lblCurrentStep.TabIndex = 1;
            this.lblCurrentStep.Text = "1";
            this.lblCurrentStep.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(227, 8);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(71, 22);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // checkedInstallItems
            // 
            this.checkedInstallItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedInstallItems.CheckOnClick = true;
            this.checkedInstallItems.ColumnWidth = 2;
            this.checkedInstallItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedInstallItems.FormattingEnabled = true;
            this.checkedInstallItems.Location = new System.Drawing.Point(0, 0);
            this.checkedInstallItems.Margin = new System.Windows.Forms.Padding(2);
            this.checkedInstallItems.Name = "checkedInstallItems";
            this.checkedInstallItems.Size = new System.Drawing.Size(422, 306);
            this.checkedInstallItems.TabIndex = 0;
            this.checkedInstallItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedInstallItems_ItemCheck);
            // 
            // PanelItems
            // 
            this.PanelItems.Controls.Add(this.panel2);
            this.PanelItems.Controls.Add(this.panel1);
            this.PanelItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelItems.Location = new System.Drawing.Point(0, 65);
            this.PanelItems.Margin = new System.Windows.Forms.Padding(2);
            this.PanelItems.Name = "PanelItems";
            this.PanelItems.Size = new System.Drawing.Size(534, 308);
            this.PanelItems.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.checkedInstallItems);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(88, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(424, 308);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.panel1.Size = new System.Drawing.Size(88, 308);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择安装项目:";
            // 
            // Step1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.ControlBox = false;
            this.Controls.Add(this.PanelItems);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.PanelInstallLicence);
            this.Controls.Add(this.PanelInstallFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Step1";
            this.ShowIcon = false;
            this.Text = "选择安装项目、安装路径、证书";
            this.Load += new System.EventHandler(this.Step1_Load);
            this.PanelInstallFolder.ResumeLayout(false);
            this.PanelInstallFolder.PerformLayout();
            this.PanelInstallLicence.ResumeLayout(false);
            this.PanelInstallLicence.PerformLayout();
            this.PanelBottom.ResumeLayout(false);
            this.PanelBottom.PerformLayout();
            this.PanelItems.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog installFolderDialog;
        private System.Windows.Forms.Panel PanelInstallFolder;
        private System.Windows.Forms.Button btnDirectoryBrower;
        private System.Windows.Forms.TextBox txtInstallFolder;
        private System.Windows.Forms.Label lblInstallFolder;
        private System.Windows.Forms.Panel PanelInstallLicence;
        private System.Windows.Forms.Button btnLicenceBrower;
        private System.Windows.Forms.TextBox txtLicencePath;
        private System.Windows.Forms.Label lblChooseLicence;
        private System.Windows.Forms.OpenFileDialog openLicenceFileDialog;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnGetMachineCode;
        private System.Windows.Forms.CheckedListBox checkedInstallItems;
        private System.Windows.Forms.Panel PanelItems;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.Label lblTotalStep;
    }
}