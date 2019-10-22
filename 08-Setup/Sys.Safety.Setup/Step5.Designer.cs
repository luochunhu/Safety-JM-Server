namespace Sys.Safety.Setup
{
    partial class Step5
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
            this.PanelStartItems = new System.Windows.Forms.Panel();
            this.checkedStartItems = new System.Windows.Forms.CheckedListBox();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.PanelStartItems.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelStartItems
            // 
            this.PanelStartItems.Controls.Add(this.checkedStartItems);
            this.PanelStartItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelStartItems.Location = new System.Drawing.Point(0, 0);
            this.PanelStartItems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelStartItems.Name = "PanelStartItems";
            this.PanelStartItems.Size = new System.Drawing.Size(534, 367);
            this.PanelStartItems.TabIndex = 0;
            // 
            // checkedStartItems
            // 
            this.checkedStartItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedStartItems.FormattingEnabled = true;
            this.checkedStartItems.Location = new System.Drawing.Point(0, 0);
            this.checkedStartItems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkedStartItems.Name = "checkedStartItems";
            this.checkedStartItems.Size = new System.Drawing.Size(534, 367);
            this.checkedStartItems.TabIndex = 0;
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.btnNext);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelBottom.Location = new System.Drawing.Point(0, 367);
            this.PanelBottom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(534, 44);
            this.PanelBottom.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(225, 11);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(71, 22);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // Step5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.ControlBox = false;
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.PanelStartItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Step5";
            this.ShowIcon = false;
            this.Text = "启动";
            this.Load += new System.EventHandler(this.Step5_Load);
            this.VisibleChanged += new System.EventHandler(this.Step5_VisibleChanged);
            this.PanelStartItems.ResumeLayout(false);
            this.PanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelStartItems;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.CheckedListBox checkedStartItems;
    }
}