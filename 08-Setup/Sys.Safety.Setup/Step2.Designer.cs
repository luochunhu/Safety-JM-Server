namespace Sys.Safety.Setup
{
    partial class Step2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Step2));
            this.PanelInformation = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.PanelMiddle = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.InstallStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InstallItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.PanelInformation.SuspendLayout();
            this.PanelMiddle.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelInformation
            // 
            this.PanelInformation.Controls.Add(this.label1);
            this.PanelInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelInformation.Location = new System.Drawing.Point(0, 0);
            this.PanelInformation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelInformation.Name = "PanelInformation";
            this.PanelInformation.Size = new System.Drawing.Size(534, 34);
            this.PanelInformation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "正在安装...";
            // 
            // PanelMiddle
            // 
            this.PanelMiddle.Controls.Add(this.listView1);
            this.PanelMiddle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelMiddle.Location = new System.Drawing.Point(0, 34);
            this.PanelMiddle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelMiddle.Name = "PanelMiddle";
            this.PanelMiddle.Size = new System.Drawing.Size(534, 329);
            this.PanelMiddle.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.InstallStatus,
            this.InstallItemName});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(534, 329);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.StateImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // InstallStatus
            // 
            this.InstallStatus.Text = "";
            this.InstallStatus.Width = 75;
            // 
            // InstallItemName
            // 
            this.InstallItemName.Text = "";
            this.InstallItemName.Width = 450;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ing.gif");
            this.imageList1.Images.SetKeyName(1, "success.png");
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.btnNext);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBottom.Location = new System.Drawing.Point(0, 363);
            this.PanelBottom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(534, 48);
            this.PanelBottom.TabIndex = 3;
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(234, 15);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(71, 22);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // Step2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.ControlBox = false;
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.PanelMiddle);
            this.Controls.Add(this.PanelInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Step2";
            this.ShowIcon = false;
            this.Text = "安装";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Step2_FormClosing);
            this.Load += new System.EventHandler(this.Step2_Load);
            this.PanelInformation.ResumeLayout(false);
            this.PanelInformation.PerformLayout();
            this.PanelMiddle.ResumeLayout(false);
            this.PanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel PanelMiddle;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader InstallStatus;
        private System.Windows.Forms.ColumnHeader InstallItemName;
    }
}