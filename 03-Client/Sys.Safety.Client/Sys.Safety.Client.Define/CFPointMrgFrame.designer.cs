namespace Sys.Safety.Client.Define
{
    partial class CFPointMrgFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFPointMrgFrame));
            this.CpDocument = new DevExpress.XtraEditors.PanelControl();
            this.CpTip = new DevExpress.XtraEditors.GroupControl();
            this.CpButtom = new DevExpress.XtraEditors.PanelControl();
            this.cbExist = new DevExpress.XtraEditors.SimpleButton();
            this.cbSave = new DevExpress.XtraEditors.SimpleButton();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.Cpleft = new DevExpress.XtraEditors.PanelControl();
            this.CpleftDocument = new DevExpress.XtraEditors.PanelControl();
            this.CpleftButtom = new DevExpress.XtraEditors.PanelControl();
            this.ClbTip = new DevExpress.XtraEditors.LabelControl();
            this.barManager = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.CpDocument)).BeginInit();
            this.CpDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CpTip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpButtom)).BeginInit();
            this.CpButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cpleft)).BeginInit();
            this.Cpleft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CpleftDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpleftButtom)).BeginInit();
            this.CpleftButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // CpDocument
            // 
            this.CpDocument.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.CpDocument.Controls.Add(this.CpTip);
            this.CpDocument.Controls.Add(this.CpButtom);
            this.CpDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CpDocument.Location = new System.Drawing.Point(334, 47);
            this.CpDocument.Name = "CpDocument";
            this.CpDocument.Size = new System.Drawing.Size(607, 525);
            this.CpDocument.TabIndex = 1;
            // 
            // CpTip
            // 
            this.CpTip.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.CpTip.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CpTip.Appearance.Options.UseBackColor = true;
            this.CpTip.Appearance.Options.UseFont = true;
            this.CpTip.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.CpTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CpTip.Location = new System.Drawing.Point(0, 0);
            this.CpTip.Name = "CpTip";
            this.CpTip.Size = new System.Drawing.Size(607, 524);
            this.CpTip.TabIndex = 0;
            // 
            // CpButtom
            // 
            this.CpButtom.Controls.Add(this.cbExist);
            this.CpButtom.Controls.Add(this.cbSave);
            this.CpButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CpButtom.Location = new System.Drawing.Point(0, 524);
            this.CpButtom.Name = "CpButtom";
            this.CpButtom.Size = new System.Drawing.Size(607, 1);
            this.CpButtom.TabIndex = 2;
            // 
            // cbExist
            // 
            this.cbExist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbExist.Appearance.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbExist.Appearance.Options.UseFont = true;
            this.cbExist.Image = ((System.Drawing.Image)(resources.GetObject("cbExist.Image")));
            this.cbExist.Location = new System.Drawing.Point(462, -38);
            this.cbExist.Name = "cbExist";
            this.cbExist.Size = new System.Drawing.Size(108, 30);
            this.cbExist.TabIndex = 1;
            this.cbExist.Text = "退出";
            this.cbExist.Click += new System.EventHandler(this.cbExist_Click);
            // 
            // cbSave
            // 
            this.cbSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSave.Appearance.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbSave.Appearance.Options.UseFont = true;
            this.cbSave.Image = ((System.Drawing.Image)(resources.GetObject("cbSave.Image")));
            this.cbSave.Location = new System.Drawing.Point(305, -38);
            this.cbSave.Name = "cbSave";
            this.cbSave.Size = new System.Drawing.Size(108, 30);
            this.cbSave.TabIndex = 0;
            this.cbSave.Text = "保存巡检";
            this.cbSave.Click += new System.EventHandler(this.cbSave_Click);
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(329, 47);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 525);
            this.splitterControl1.TabIndex = 8;
            this.splitterControl1.TabStop = false;
            // 
            // Cpleft
            // 
            this.Cpleft.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Cpleft.Controls.Add(this.CpleftDocument);
            this.Cpleft.Controls.Add(this.CpleftButtom);
            this.Cpleft.Dock = System.Windows.Forms.DockStyle.Left;
            this.Cpleft.Location = new System.Drawing.Point(0, 47);
            this.Cpleft.Name = "Cpleft";
            this.Cpleft.Size = new System.Drawing.Size(329, 525);
            this.Cpleft.TabIndex = 7;
            // 
            // CpleftDocument
            // 
            this.CpleftDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CpleftDocument.Location = new System.Drawing.Point(0, 0);
            this.CpleftDocument.Name = "CpleftDocument";
            this.CpleftDocument.Size = new System.Drawing.Size(329, 496);
            this.CpleftDocument.TabIndex = 3;
            // 
            // CpleftButtom
            // 
            this.CpleftButtom.Controls.Add(this.ClbTip);
            this.CpleftButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CpleftButtom.Location = new System.Drawing.Point(0, 496);
            this.CpleftButtom.Name = "CpleftButtom";
            this.CpleftButtom.Size = new System.Drawing.Size(329, 29);
            this.CpleftButtom.TabIndex = 2;
            this.CpleftButtom.Visible = false;
            // 
            // ClbTip
            // 
            this.ClbTip.Appearance.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClbTip.LineVisible = true;
            this.ClbTip.Location = new System.Drawing.Point(38, 7);
            this.ClbTip.Name = "ClbTip";
            this.ClbTip.Size = new System.Drawing.Size(28, 14);
            this.ClbTip.TabIndex = 0;
            this.ClbTip.Text = "就绪";
            // 
            // barManager
            // 
            this.barManager.AllowQuickCustomization = false;
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonSave,
            this.barButtonItem2,
            this.barButtonExit});
            this.barManager.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "Tools";
            // 
            // barButtonSave
            // 
            this.barButtonSave.Caption = "保存巡检";
            this.barButtonSave.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonSave.Glyph")));
            this.barButtonSave.Id = 0;
            this.barButtonSave.Name = "barButtonSave";
            this.barButtonSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSave_ItemClick);
            // 
            // barButtonExit
            // 
            this.barButtonExit.Caption = "退出";
            this.barButtonExit.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonExit.Glyph")));
            this.barButtonExit.Id = 2;
            this.barButtonExit.Name = "barButtonExit";
            this.barButtonExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(941, 47);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 572);
            this.barDockControlBottom.Size = new System.Drawing.Size(941, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 47);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 525);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(941, 47);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 525);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "退出";
            this.barButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Glyph")));
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // CFPointMrgFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 572);
            this.Controls.Add(this.CpDocument);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.Cpleft);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CFPointMrgFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefineFrame_FormClosing);
            this.Load += new System.EventHandler(this.DefineFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CpDocument)).EndInit();
            this.CpDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CpTip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpButtom)).EndInit();
            this.CpButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Cpleft)).EndInit();
            this.Cpleft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CpleftDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpleftButtom)).EndInit();
            this.CpleftButtom.ResumeLayout(false);
            this.CpleftButtom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl CpDocument;
        public DevExpress.XtraEditors.PanelControl CpButtom;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.PanelControl Cpleft;
        private DevExpress.XtraEditors.PanelControl CpleftButtom;
        private DevExpress.XtraEditors.PanelControl CpleftDocument;
        private DevExpress.XtraEditors.SimpleButton cbExist;
        private DevExpress.XtraEditors.SimpleButton cbSave;
        public DevExpress.XtraEditors.GroupControl CpTip;
        public DevExpress.XtraEditors.LabelControl ClbTip;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonSave;
        private DevExpress.XtraBars.BarButtonItem barButtonExit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;

    }
}