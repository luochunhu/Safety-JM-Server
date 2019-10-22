namespace Sys.Safety.Video
{
    partial class frmVideoPreview
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.videopicture = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnrecord = new System.Windows.Forms.Button();
            this.btndraw = new System.Windows.Forms.Button();
            this.controlptz = new System.Windows.Forms.GroupBox();
            this.btnright = new System.Windows.Forms.Button();
            this.btndown = new System.Windows.Forms.Button();
            this.btnleft = new System.Windows.Forms.Button();
            this.btnup = new System.Windows.Forms.Button();
            this.btnpreview = new System.Windows.Forms.Button();
            this.videotree = new System.Windows.Forms.TreeView();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.StaticMsg = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videopicture)).BeginInit();
            this.panel1.SuspendLayout();
            this.controlptz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.96872F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.03128F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 224F));
            this.tableLayoutPanel1.Controls.Add(this.videopicture, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.videotree, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1062, 538);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // videopicture
            // 
            this.videopicture.BackColor = System.Drawing.SystemColors.WindowText;
            this.videopicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videopicture.Location = new System.Drawing.Point(228, 3);
            this.videopicture.Name = "videopicture";
            this.videopicture.Size = new System.Drawing.Size(606, 532);
            this.videopicture.TabIndex = 1;
            this.videopicture.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnrecord);
            this.panel1.Controls.Add(this.btndraw);
            this.panel1.Controls.Add(this.controlptz);
            this.panel1.Controls.Add(this.btnpreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(840, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 532);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // btnrecord
            // 
            this.btnrecord.Location = new System.Drawing.Point(121, 235);
            this.btnrecord.Name = "btnrecord";
            this.btnrecord.Size = new System.Drawing.Size(74, 33);
            this.btnrecord.TabIndex = 4;
            this.btnrecord.Text = "录像";
            this.btnrecord.UseVisualStyleBackColor = true;
            this.btnrecord.Click += new System.EventHandler(this.btnrecord_Click);
            // 
            // btndraw
            // 
            this.btndraw.Location = new System.Drawing.Point(20, 235);
            this.btndraw.Name = "btndraw";
            this.btndraw.Size = new System.Drawing.Size(66, 33);
            this.btndraw.TabIndex = 3;
            this.btndraw.Text = "抓图";
            this.btndraw.UseVisualStyleBackColor = true;
            this.btndraw.Click += new System.EventHandler(this.btndraw_Click);
            // 
            // controlptz
            // 
            this.controlptz.Controls.Add(this.btnright);
            this.controlptz.Controls.Add(this.btndown);
            this.controlptz.Controls.Add(this.btnleft);
            this.controlptz.Controls.Add(this.btnup);
            this.controlptz.Location = new System.Drawing.Point(20, 71);
            this.controlptz.Name = "controlptz";
            this.controlptz.Size = new System.Drawing.Size(175, 142);
            this.controlptz.TabIndex = 2;
            this.controlptz.TabStop = false;
            this.controlptz.Text = "云台控制";
            // 
            // btnright
            // 
            this.btnright.Location = new System.Drawing.Point(111, 57);
            this.btnright.Name = "btnright";
            this.btnright.Size = new System.Drawing.Size(56, 33);
            this.btnright.TabIndex = 3;
            this.btnright.Text = "右";
            this.btnright.UseVisualStyleBackColor = true;
            this.btnright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnright_MouseDown);
            this.btnright.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnright_MouseUp);
            // 
            // btndown
            // 
            this.btndown.Location = new System.Drawing.Point(60, 100);
            this.btndown.Name = "btndown";
            this.btndown.Size = new System.Drawing.Size(58, 33);
            this.btndown.TabIndex = 2;
            this.btndown.Text = "下";
            this.btndown.UseVisualStyleBackColor = true;
            this.btndown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btndown_MouseDown);
            this.btndown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btndown_MouseUp);
            // 
            // btnleft
            // 
            this.btnleft.Location = new System.Drawing.Point(5, 57);
            this.btnleft.Name = "btnleft";
            this.btnleft.Size = new System.Drawing.Size(60, 33);
            this.btnleft.TabIndex = 1;
            this.btnleft.Text = "左";
            this.btnleft.UseVisualStyleBackColor = true;
            this.btnleft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnleft_MouseDown);
            this.btnleft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnleft_MouseUp);
            // 
            // btnup
            // 
            this.btnup.Location = new System.Drawing.Point(58, 14);
            this.btnup.Name = "btnup";
            this.btnup.Size = new System.Drawing.Size(58, 33);
            this.btnup.TabIndex = 0;
            this.btnup.Text = "上";
            this.btnup.UseVisualStyleBackColor = true;
            this.btnup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnup_MouseDown);
            this.btnup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnup_MouseUp);
            // 
            // btnpreview
            // 
            this.btnpreview.Location = new System.Drawing.Point(20, 24);
            this.btnpreview.Name = "btnpreview";
            this.btnpreview.Size = new System.Drawing.Size(175, 33);
            this.btnpreview.TabIndex = 0;
            this.btnpreview.Text = "预览";
            this.btnpreview.UseVisualStyleBackColor = true;
            this.btnpreview.Click += new System.EventHandler(this.btnpreview_Click);
            // 
            // videotree
            // 
            this.videotree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videotree.Location = new System.Drawing.Point(3, 3);
            this.videotree.Name = "videotree";
            this.videotree.Size = new System.Drawing.Size(219, 532);
            this.videotree.TabIndex = 3;
            this.videotree.Visible = false;
            this.videotree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.videotree_NodeMouseClick);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.StaticMsg});
            this.barManager1.MaxItemId = 1;
            this.barManager1.StatusBar = this.bar1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.StaticMsg)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // StaticMsg
            // 
            this.StaticMsg.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.StaticMsg.Caption = "数据准备就绪...";
            this.StaticMsg.Id = 0;
            this.StaticMsg.Name = "StaticMsg";
            this.StaticMsg.TextAlignment = System.Drawing.StringAlignment.Near;
            this.StaticMsg.Width = 32;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(1062, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 538);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(1062, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 538);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1062, 0);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 538);
            // 
            // frmVideoPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 565);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmVideoPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实时视频";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVideoPreview_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videopicture)).EndInit();
            this.panel1.ResumeLayout(false);
            this.controlptz.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox videopicture;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox controlptz;
        private System.Windows.Forms.Button btnright;
        private System.Windows.Forms.Button btndown;
        private System.Windows.Forms.Button btnleft;
        private System.Windows.Forms.Button btnup;
        private System.Windows.Forms.Button btnpreview;
        private System.Windows.Forms.Button btnrecord;
        private System.Windows.Forms.Button btndraw;
        private System.Windows.Forms.TreeView videotree;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem StaticMsg;

    }
}