namespace Sys.Safety.Client.Alarm
{
    partial class frmGraph
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
            this.timerGraph = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lab_realtimeVal = new System.Windows.Forms.Label();
            this.lab_address = new DevExpress.XtraEditors.LabelControl();
            this.lab_deviceState = new DevExpress.XtraEditors.LabelControl();
            this.lab_dataType = new DevExpress.XtraEditors.LabelControl();
            this.lab_devname = new DevExpress.XtraEditors.LabelControl();
            this.lab_pointCode = new DevExpress.XtraEditors.LabelControl();
            this.lab_alarm = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.lab_Timer = new DevExpress.XtraEditors.LabelControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // timerGraph
            // 
            this.timerGraph.Interval = 4000;
            this.timerGraph.Tick += new System.EventHandler(this.timerGraph_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.lab_realtimeVal);
            this.panel1.Controls.Add(this.lab_address);
            this.panel1.Controls.Add(this.lab_Timer);
            this.panel1.Controls.Add(this.lab_deviceState);
            this.panel1.Controls.Add(this.lab_dataType);
            this.panel1.Controls.Add(this.lab_devname);
            this.panel1.Controls.Add(this.lab_pointCode);
            this.panel1.Controls.Add(this.lab_alarm);
            this.panel1.Controls.Add(this.pictureEdit1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 360);
            this.panel1.TabIndex = 103;
            this.panel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDoubleClick);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl1.Location = new System.Drawing.Point(12, 309);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 110;
            this.labelControl1.Text = "提示：双击可关闭";
            // 
            // lab_realtimeVal
            // 
            this.lab_realtimeVal.BackColor = System.Drawing.Color.Transparent;
            this.lab_realtimeVal.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_realtimeVal.ForeColor = System.Drawing.Color.Maroon;
            this.lab_realtimeVal.Location = new System.Drawing.Point(214, 172);
            this.lab_realtimeVal.Name = "lab_realtimeVal";
            this.lab_realtimeVal.Size = new System.Drawing.Size(385, 68);
            this.lab_realtimeVal.TabIndex = 109;
            this.lab_realtimeVal.Text = "实时值";
            this.lab_realtimeVal.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_realtimeVal_MouseDoubleClick);
            // 
            // lab_address
            // 
            this.lab_address.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_address.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_address.Location = new System.Drawing.Point(217, 116);
            this.lab_address.Name = "lab_address";
            this.lab_address.Size = new System.Drawing.Size(42, 28);
            this.lab_address.TabIndex = 108;
            this.lab_address.Text = "位置";
            this.lab_address.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_address_MouseDoubleClick);
            // 
            // lab_deviceState
            // 
            this.lab_deviceState.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_deviceState.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_deviceState.Location = new System.Drawing.Point(218, 277);
            this.lab_deviceState.Name = "lab_deviceState";
            this.lab_deviceState.Size = new System.Drawing.Size(84, 28);
            this.lab_deviceState.TabIndex = 104;
            this.lab_deviceState.Text = "设备状态";
            this.lab_deviceState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_deviceState_MouseDoubleClick);
            // 
            // lab_dataType
            // 
            this.lab_dataType.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_dataType.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_dataType.Location = new System.Drawing.Point(219, 238);
            this.lab_dataType.Name = "lab_dataType";
            this.lab_dataType.Size = new System.Drawing.Size(84, 28);
            this.lab_dataType.TabIndex = 105;
            this.lab_dataType.Text = "数据状态";
            this.lab_dataType.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_dataType_MouseDoubleClick);
            // 
            // lab_devname
            // 
            this.lab_devname.Appearance.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_devname.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_devname.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lab_devname.Location = new System.Drawing.Point(217, 54);
            this.lab_devname.Name = "lab_devname";
            this.lab_devname.Size = new System.Drawing.Size(380, 53);
            this.lab_devname.TabIndex = 106;
            this.lab_devname.Text = "类型";
            this.lab_devname.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_devname_MouseDoubleClick);
            // 
            // lab_pointCode
            // 
            this.lab_pointCode.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_pointCode.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_pointCode.Location = new System.Drawing.Point(217, 22);
            this.lab_pointCode.Name = "lab_pointCode";
            this.lab_pointCode.Size = new System.Drawing.Size(42, 28);
            this.lab_pointCode.TabIndex = 107;
            this.lab_pointCode.Text = "编号";
            this.lab_pointCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_pointCode_DoubleClick);
            // 
            // lab_alarm
            // 
            this.lab_alarm.Appearance.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_alarm.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lab_alarm.Location = new System.Drawing.Point(5, 5);
            this.lab_alarm.Name = "lab_alarm";
            this.lab_alarm.Size = new System.Drawing.Size(64, 41);
            this.lab_alarm.TabIndex = 103;
            this.lab_alarm.Text = "报警";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::Sys.Safety.Client.Alarm.Properties.Resources.alertGif;
            this.pictureEdit1.Location = new System.Drawing.Point(51, 70);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Size = new System.Drawing.Size(128, 128);
            this.pictureEdit1.TabIndex = 102;
            this.pictureEdit1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureEdit1_MouseDoubleClick);
            // 
            // lab_Timer
            // 
            this.lab_Timer.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_Timer.Appearance.ForeColor = System.Drawing.Color.Maroon;
            this.lab_Timer.Location = new System.Drawing.Point(219, 311);
            this.lab_Timer.Name = "lab_Timer";
            this.lab_Timer.Size = new System.Drawing.Size(48, 28);
            this.lab_Timer.TabIndex = 104;
            this.lab_Timer.Text = "时间 ";
            this.lab_Timer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lab_deviceState_MouseDoubleClick);
            // 
            // frmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGraph";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图文报警";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Load += new System.EventHandler(this.frmGraph_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerGraph;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Label lab_realtimeVal;
        private DevExpress.XtraEditors.LabelControl lab_address;
        private DevExpress.XtraEditors.LabelControl lab_deviceState;
        private DevExpress.XtraEditors.LabelControl lab_dataType;
        private DevExpress.XtraEditors.LabelControl lab_devname;
        private DevExpress.XtraEditors.LabelControl lab_pointCode;
        private DevExpress.XtraEditors.LabelControl lab_alarm;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl lab_Timer;
    }
}