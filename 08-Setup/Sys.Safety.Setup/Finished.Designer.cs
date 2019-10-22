namespace Sys.Safety.Setup
{
    partial class Finished
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
            this.PanelFinished = new System.Windows.Forms.Panel();
            this.lblInformation = new System.Windows.Forms.Label();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.PanelFinished.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelFinished
            // 
            this.PanelFinished.Controls.Add(this.lblInformation);
            this.PanelFinished.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelFinished.Location = new System.Drawing.Point(0, 0);
            this.PanelFinished.Name = "PanelFinished";
            this.PanelFinished.Size = new System.Drawing.Size(724, 448);
            this.PanelFinished.TabIndex = 0;
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Location = new System.Drawing.Point(294, 50);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(120, 15);
            this.lblInformation.TabIndex = 0;
            this.lblInformation.Text = "恭喜，安装完成!";
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.btnFinish);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBottom.Location = new System.Drawing.Point(0, 448);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(724, 47);
            this.PanelBottom.TabIndex = 1;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(319, 10);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(95, 27);
            this.btnFinish.TabIndex = 12;
            this.btnFinish.Text = "完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // Finished
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(724, 495);
            this.ControlBox = false;
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.PanelFinished);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Finished";
            this.ShowIcon = false;
            this.Text = "完成";
            this.PanelFinished.ResumeLayout(false);
            this.PanelFinished.PerformLayout();
            this.PanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelFinished;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label lblInformation;
    }
}