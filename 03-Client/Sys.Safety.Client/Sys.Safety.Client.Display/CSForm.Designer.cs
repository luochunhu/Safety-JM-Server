namespace Sys.Safety.Client.Display
{
    partial class CSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSForm));
            this.text_cs = new DevExpress.XtraEditors.MemoEdit();
            this.memo_cs = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.add_cs = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.text_cs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memo_cs.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // text_cs
            // 
            this.text_cs.Location = new System.Drawing.Point(32, 50);
            this.text_cs.Name = "text_cs";
            this.text_cs.Size = new System.Drawing.Size(478, 83);
            this.text_cs.TabIndex = 0;
            this.text_cs.UseOptimizedRendering = true;
            // 
            // memo_cs
            // 
            this.memo_cs.Enabled = false;
            this.memo_cs.Location = new System.Drawing.Point(32, 186);
            this.memo_cs.Name = "memo_cs";
            this.memo_cs.Size = new System.Drawing.Size(478, 117);
            this.memo_cs.TabIndex = 1;
            this.memo_cs.UseOptimizedRendering = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 166);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "已录入措施：";
            // 
            // add_cs
            // 
            this.add_cs.Location = new System.Drawing.Point(421, 139);
            this.add_cs.Name = "add_cs";
            this.add_cs.Size = new System.Drawing.Size(75, 23);
            this.add_cs.TabIndex = 3;
            this.add_cs.Text = "录 入";
            this.add_cs.Click += new System.EventHandler(this.add_cs_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(32, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "输入措施：";
            // 
            // CSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 315);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.add_cs);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.memo_cs);
            this.Controls.Add(this.text_cs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CSForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "处理措施录入";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CSForm_FormClosed);
            this.Load += new System.EventHandler(this.CSForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.text_cs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memo_cs.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit text_cs;
        private DevExpress.XtraEditors.MemoEdit memo_cs;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton add_cs;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}