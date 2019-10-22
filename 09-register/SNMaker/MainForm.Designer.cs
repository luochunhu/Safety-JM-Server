namespace Basic.Framework.Tools.SNMaker
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCopyProductCode = new System.Windows.Forms.Button();
            this.terminals = new System.Windows.Forms.TextBox();
            this.txtCustomerInfo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxProducts = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoCommercialModel = new System.Windows.Forms.RadioButton();
            this.rdoTrialMode = new System.Windows.Forms.RadioButton();
            this.rdoDevelopModel = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateRegistCode = new System.Windows.Forms.Button();
            this.btnImportfile = new System.Windows.Forms.Button();
            this.btnCopyRegist = new System.Windows.Forms.Button();
            this.txtRegistCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.txtMachineCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCopyProductCode);
            this.groupBox1.Controls.Add(this.terminals);
            this.groupBox1.Controls.Add(this.txtCustomerInfo);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbxProducts);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.rdoCommercialModel);
            this.groupBox1.Controls.Add(this.rdoTrialMode);
            this.groupBox1.Controls.Add(this.rdoDevelopModel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCreateRegistCode);
            this.groupBox1.Controls.Add(this.btnImportfile);
            this.groupBox1.Controls.Add(this.btnCopyRegist);
            this.groupBox1.Controls.Add(this.txtRegistCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnCopy);
            this.groupBox1.Controls.Add(this.btnPaste);
            this.groupBox1.Controls.Add(this.txtMachineCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(598, 502);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnCopyProductCode
            // 
            this.btnCopyProductCode.Location = new System.Drawing.Point(433, 113);
            this.btnCopyProductCode.Name = "btnCopyProductCode";
            this.btnCopyProductCode.Size = new System.Drawing.Size(134, 23);
            this.btnCopyProductCode.TabIndex = 19;
            this.btnCopyProductCode.Text = "复制授权产品编码";
            this.btnCopyProductCode.UseVisualStyleBackColor = true;
            this.btnCopyProductCode.Click += new System.EventHandler(this.btnCopyProductCode_Click);
            // 
            // terminals
            // 
            this.terminals.Location = new System.Drawing.Point(81, 183);
            this.terminals.Name = "terminals";
            this.terminals.Size = new System.Drawing.Size(332, 21);
            this.terminals.TabIndex = 18;
            // 
            // txtCustomerInfo
            // 
            this.txtCustomerInfo.Location = new System.Drawing.Point(81, 145);
            this.txtCustomerInfo.Name = "txtCustomerInfo";
            this.txtCustomerInfo.Size = new System.Drawing.Size(332, 21);
            this.txtCustomerInfo.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(419, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "0表示不限制";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 186);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "终端数量：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "客户信息：";
            // 
            // cbxProducts
            // 
            this.cbxProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProducts.FormattingEnabled = true;
            this.cbxProducts.Items.AddRange(new object[] {
            "002-监控系统"});
            this.cbxProducts.Location = new System.Drawing.Point(81, 113);
            this.cbxProducts.Margin = new System.Windows.Forms.Padding(2);
            this.cbxProducts.Name = "cbxProducts";
            this.cbxProducts.Size = new System.Drawing.Size(332, 20);
            this.cbxProducts.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "授权产品：";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(435, 466);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(116, 21);
            this.dtpEndTime.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(364, 474);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "试用时间：";
            // 
            // rdoCommercialModel
            // 
            this.rdoCommercialModel.AutoSize = true;
            this.rdoCommercialModel.Location = new System.Drawing.Point(166, 473);
            this.rdoCommercialModel.Name = "rdoCommercialModel";
            this.rdoCommercialModel.Size = new System.Drawing.Size(71, 16);
            this.rdoCommercialModel.TabIndex = 12;
            this.rdoCommercialModel.Text = "商用模式";
            this.rdoCommercialModel.UseVisualStyleBackColor = true;
            this.rdoCommercialModel.CheckedChanged += new System.EventHandler(this.rdoCommercialModel_CheckedChanged);
            // 
            // rdoTrialMode
            // 
            this.rdoTrialMode.AutoSize = true;
            this.rdoTrialMode.Checked = true;
            this.rdoTrialMode.Location = new System.Drawing.Point(81, 473);
            this.rdoTrialMode.Name = "rdoTrialMode";
            this.rdoTrialMode.Size = new System.Drawing.Size(71, 16);
            this.rdoTrialMode.TabIndex = 11;
            this.rdoTrialMode.TabStop = true;
            this.rdoTrialMode.Text = "试用模式";
            this.rdoTrialMode.UseVisualStyleBackColor = true;
            this.rdoTrialMode.CheckedChanged += new System.EventHandler(this.rdoTrialMode_CheckedChanged);
            // 
            // rdoDevelopModel
            // 
            this.rdoDevelopModel.AutoSize = true;
            this.rdoDevelopModel.Location = new System.Drawing.Point(254, 473);
            this.rdoDevelopModel.Name = "rdoDevelopModel";
            this.rdoDevelopModel.Size = new System.Drawing.Size(95, 16);
            this.rdoDevelopModel.TabIndex = 10;
            this.rdoDevelopModel.Text = "开发人员模式";
            this.rdoDevelopModel.UseVisualStyleBackColor = true;
            this.rdoDevelopModel.CheckedChanged += new System.EventHandler(this.rdoDevelopModel_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 474);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "授权模式：";
            // 
            // btnCreateRegistCode
            // 
            this.btnCreateRegistCode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreateRegistCode.Location = new System.Drawing.Point(433, 220);
            this.btnCreateRegistCode.Name = "btnCreateRegistCode";
            this.btnCreateRegistCode.Size = new System.Drawing.Size(134, 38);
            this.btnCreateRegistCode.TabIndex = 8;
            this.btnCreateRegistCode.Text = "生成注册码";
            this.btnCreateRegistCode.UseVisualStyleBackColor = true;
            this.btnCreateRegistCode.Click += new System.EventHandler(this.btnCreateRegistCode_Click);
            // 
            // btnImportfile
            // 
            this.btnImportfile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnImportfile.Location = new System.Drawing.Point(433, 344);
            this.btnImportfile.Name = "btnImportfile";
            this.btnImportfile.Size = new System.Drawing.Size(134, 38);
            this.btnImportfile.TabIndex = 7;
            this.btnImportfile.Text = "导出文件...";
            this.btnImportfile.UseVisualStyleBackColor = true;
            this.btnImportfile.Click += new System.EventHandler(this.btnImportfile_Click);
            // 
            // btnCopyRegist
            // 
            this.btnCopyRegist.Location = new System.Drawing.Point(433, 281);
            this.btnCopyRegist.Name = "btnCopyRegist";
            this.btnCopyRegist.Size = new System.Drawing.Size(134, 38);
            this.btnCopyRegist.TabIndex = 6;
            this.btnCopyRegist.Text = "复制注册码";
            this.btnCopyRegist.UseVisualStyleBackColor = true;
            this.btnCopyRegist.Click += new System.EventHandler(this.btnCopyRegist_Click);
            // 
            // txtRegistCode
            // 
            this.txtRegistCode.Location = new System.Drawing.Point(81, 220);
            this.txtRegistCode.Multiline = true;
            this.txtRegistCode.Name = "txtRegistCode";
            this.txtRegistCode.Size = new System.Drawing.Size(332, 238);
            this.txtRegistCode.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 294);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "注册码：";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(433, 74);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(134, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(433, 32);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(134, 23);
            this.btnPaste.TabIndex = 2;
            this.btnPaste.Text = "粘贴";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.Location = new System.Drawing.Point(81, 28);
            this.txtMachineCode.Multiline = true;
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.Size = new System.Drawing.Size(332, 70);
            this.txtMachineCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机器码：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 502);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件授权文件生成器V3.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCreateRegistCode;
        private System.Windows.Forms.Button btnImportfile;
        private System.Windows.Forms.Button btnCopyRegist;
        private System.Windows.Forms.TextBox txtRegistCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdoCommercialModel;
        private System.Windows.Forms.RadioButton rdoTrialMode;
        private System.Windows.Forms.RadioButton rdoDevelopModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxProducts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCustomerInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCopyProductCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox terminals;
        private System.Windows.Forms.Label label8;
    }
}

