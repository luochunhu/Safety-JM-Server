namespace Sys.Safety.Client.Alarm
{
    partial class SensorCalibrationDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorCalibrationDetail));
            this.txt_cs = new System.Windows.Forms.TextBox();
            this.dtp_stime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_etime = new System.Windows.Forms.DateTimePicker();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // txt_cs
            // 
            this.txt_cs.Location = new System.Drawing.Point(12, 12);
            this.txt_cs.Multiline = true;
            this.txt_cs.Name = "txt_cs";
            this.txt_cs.Size = new System.Drawing.Size(391, 171);
            this.txt_cs.TabIndex = 0;
            // 
            // dtp_stime
            // 
            this.dtp_stime.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtp_stime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_stime.Location = new System.Drawing.Point(83, 196);
            this.dtp_stime.Name = "dtp_stime";
            this.dtp_stime.Size = new System.Drawing.Size(200, 22);
            this.dtp_stime.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "开始时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "结束时间";
            // 
            // dtp_etime
            // 
            this.dtp_etime.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtp_etime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_etime.Location = new System.Drawing.Point(83, 224);
            this.dtp_etime.Name = "dtp_etime";
            this.dtp_etime.Size = new System.Drawing.Size(200, 22);
            this.dtp_etime.TabIndex = 2;
            // 
            // btn_ok
            // 
            this.btn_ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_ok.Image")));
            this.btn_ok.Location = new System.Drawing.Point(302, 200);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(100, 39);
            this.btn_ok.TabIndex = 4;
            this.btn_ok.Text = "确认并退出";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // SensorCalibrationDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 264);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtp_etime);
            this.Controls.Add(this.dtp_stime);
            this.Controls.Add(this.txt_cs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SensorCalibrationDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备未标校处理措施";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_cs;
        private System.Windows.Forms.DateTimePicker dtp_stime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp_etime;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
    }
}