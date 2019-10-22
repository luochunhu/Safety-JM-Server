namespace Sys.Safety.Client.Define.Station
{
    partial class StationIPSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StationIPSet));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.macTxt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.ipTxt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.ymTxt = new DevExpress.XtraEditors.TextEdit();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonISave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonCancle = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonDelete = new DevExpress.XtraBars.BarButtonItem();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.fzhTxt = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.gatewayTxt = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.macTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fzhTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gatewayTxt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(80, 107);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "MAC：";
            // 
            // macTxt
            // 
            this.macTxt.Enabled = false;
            this.macTxt.Location = new System.Drawing.Point(140, 102);
            this.macTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.macTxt.Name = "macTxt";
            this.macTxt.Size = new System.Drawing.Size(386, 28);
            this.macTxt.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(99, 148);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(35, 22);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "IP：";
            // 
            // ipTxt
            // 
            this.ipTxt.Location = new System.Drawing.Point(140, 143);
            this.ipTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Size = new System.Drawing.Size(386, 28);
            this.ipTxt.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(46, 189);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 22);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "子网掩码：";
            // 
            // ymTxt
            // 
            this.ymTxt.Enabled = false;
            this.ymTxt.Location = new System.Drawing.Point(140, 184);
            this.ymTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ymTxt.Name = "ymTxt";
            this.ymTxt.Size = new System.Drawing.Size(386, 28);
            this.ymTxt.TabIndex = 1;
            // 
            // barManager
            // 
            this.barManager.AllowCustomization = false;
            this.barManager.AllowQuickCustomization = false;
            this.barManager.AllowShowToolbarsPopup = false;
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonISave,
            this.barButtonDelete,
            this.barButtonCancle});
            this.barManager.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonISave, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonCancle, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "Tools";
            // 
            // barButtonISave
            // 
            this.barButtonISave.Caption = "确定";
            this.barButtonISave.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonISave.Glyph")));
            this.barButtonISave.Id = 1;
            this.barButtonISave.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonISave.LargeGlyph")));
            this.barButtonISave.Name = "barButtonISave";
            this.barButtonISave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonISave_ItemClick);
            // 
            // barButtonCancle
            // 
            this.barButtonCancle.Caption = "取消";
            this.barButtonCancle.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonCancle.Glyph")));
            this.barButtonCancle.Id = 3;
            this.barButtonCancle.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonCancle.LargeGlyph")));
            this.barButtonCancle.Name = "barButtonCancle";
            this.barButtonCancle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonCancle_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlTop.Size = new System.Drawing.Size(640, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 284);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlBottom.Size = new System.Drawing.Size(640, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 245);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(640, 39);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 245);
            // 
            // barButtonDelete
            // 
            this.barButtonDelete.Caption = "删除";
            this.barButtonDelete.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonDelete.Glyph")));
            this.barButtonDelete.Id = 2;
            this.barButtonDelete.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonDelete.LargeGlyph")));
            this.barButtonDelete.Name = "barButtonDelete";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(63, 66);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 22);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "分站号：";
            // 
            // fzhTxt
            // 
            this.fzhTxt.Enabled = false;
            this.fzhTxt.Location = new System.Drawing.Point(140, 61);
            this.fzhTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fzhTxt.Name = "fzhTxt";
            this.fzhTxt.Size = new System.Drawing.Size(386, 28);
            this.fzhTxt.TabIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(80, 229);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(54, 22);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "网关：";
            // 
            // gatewayTxt
            // 
            this.gatewayTxt.Enabled = false;
            this.gatewayTxt.Location = new System.Drawing.Point(140, 225);
            this.gatewayTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gatewayTxt.Name = "gatewayTxt";
            this.gatewayTxt.Size = new System.Drawing.Size(386, 28);
            this.gatewayTxt.TabIndex = 1;
            // 
            // StationIPSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 284);
            this.Controls.Add(this.gatewayTxt);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.ymTxt);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.ipTxt);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.fzhTxt);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.macTxt);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "StationIPSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分站IP设置";
            this.Load += new System.EventHandler(this.StationIPSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.macTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ipTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ymTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fzhTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gatewayTxt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit macTxt;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit ipTxt;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit ymTxt;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonISave;
        private DevExpress.XtraBars.BarButtonItem barButtonDelete;
        private DevExpress.XtraBars.BarButtonItem barButtonCancle;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.TextEdit fzhTxt;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit gatewayTxt;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}