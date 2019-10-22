namespace Sys.Safety.Client.Graphic
{
    partial class GraphDrawing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphDrawing));
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            DevExpress.XtraBars.Ribbon.GalleryItem galleryItem1 = new DevExpress.XtraBars.Ribbon.GalleryItem();
            this.pcMain = new DevExpress.XtraEditors.PanelControl();
            this.mx = new Axmetamap2dLib.AxMetaMapX2D();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.galleryControl1 = new DevExpress.XtraBars.Ribbon.GalleryControl();
            this.galleryControlClient1 = new DevExpress.XtraBars.Ribbon.GalleryControlClient();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.rpSearch = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbgClose = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgFilterColumns = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgFind = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu();
            ((System.ComponentModel.ISupportInitialize)(this.pcMain)).BeginInit();
            this.pcMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.galleryControl1)).BeginInit();
            this.galleryControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // pcMain
            // 
            this.pcMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.pcMain.Controls.Add(this.mx);
            this.pcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcMain.Location = new System.Drawing.Point(0, 0);
            this.pcMain.Margin = new System.Windows.Forms.Padding(0);
            this.pcMain.Name = "pcMain";
            this.pcMain.Size = new System.Drawing.Size(990, 449);
            this.pcMain.TabIndex = 7;
            // 
            // mx
            // 
            this.mx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mx.Enabled = true;
            this.mx.Location = new System.Drawing.Point(2, 2);
            this.mx.Name = "mx";
            this.mx.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mx.OcxState")));
            this.mx.Size = new System.Drawing.Size(986, 445);
            this.mx.TabIndex = 0;
            this.mx.OnViewCallOutCommand += new Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEventHandler(this.mx_OnViewCallOutCommand);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.LightGray;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(820, -23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.TabIndex = 100;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // galleryControl1
            // 
            this.galleryControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.galleryControl1.Controls.Add(this.galleryControlClient1);
            this.galleryControl1.DesignGalleryGroupIndex = 0;
            this.galleryControl1.DesignGalleryItemIndex = 0;
            this.galleryControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // galleryControlGallery1
            // 
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Hovered.Options.UseTextOptions = true;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Hovered.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Normal.Options.UseTextOptions = true;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Normal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Pressed.Options.UseTextOptions = true;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Pressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.galleryControl1.Gallery.Appearance.ItemCaptionAppearance.Pressed.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.galleryControl1.Gallery.AutoFitColumns = false;
            this.galleryControl1.Gallery.AutoSize = DevExpress.XtraBars.Ribbon.GallerySizeMode.None;
            this.galleryControl1.Gallery.BackColor = System.Drawing.Color.Transparent;
            this.galleryControl1.Gallery.ColumnCount = 1;
            this.galleryControl1.Gallery.FixedImageSize = false;
            galleryItemGroup1.Caption = "Group1";
            galleryItem1.Caption = "Open Calendar";
            galleryItem1.Description = "Open a calendar file (.ics)";
            galleryItem1.Tag = "OpenCalendar";
            galleryItemGroup1.Items.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItem[] {
            galleryItem1});
            this.galleryControl1.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1});
            this.galleryControl1.Gallery.ItemImageLocation = DevExpress.Utils.Locations.Left;
            this.galleryControl1.Gallery.ShowGroupCaption = false;
            this.galleryControl1.Gallery.ShowItemText = true;
            this.galleryControl1.Gallery.ShowScrollBar = DevExpress.XtraBars.Ribbon.Gallery.ShowScrollBar.Hide;
            this.galleryControl1.Location = new System.Drawing.Point(23, 22);
            this.galleryControl1.Name = "galleryControl1";
            this.galleryControl1.Size = new System.Drawing.Size(1005, 300);
            this.galleryControl1.TabIndex = 0;
            this.galleryControl1.Text = "galleryControl1";
            // 
            // galleryControlClient1
            // 
            this.galleryControlClient1.GalleryControl = this.galleryControl1;
            this.galleryControlClient1.Location = new System.Drawing.Point(1, 1);
            this.galleryControlClient1.Size = new System.Drawing.Size(1003, 298);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(990, 449);
            this.panelControl1.TabIndex = 0;
            // 
            // rpSearch
            // 
            this.rpSearch.Name = "rpSearch";
            this.rpSearch.Text = "SAERCH";
            this.rpSearch.Visible = false;
            // 
            // rbgClose
            // 
            this.rbgClose.AllowTextClipping = false;
            this.rbgClose.Name = "rbgClose";
            this.rbgClose.ShowCaptionButton = false;
            this.rbgClose.Text = "Close";
            // 
            // rpgFilterColumns
            // 
            this.rpgFilterColumns.Name = "rpgFilterColumns";
            this.rpgFilterColumns.ShowCaptionButton = false;
            this.rpgFilterColumns.Text = "Filter Columns";
            // 
            // rpgFind
            // 
            this.rpgFind.Name = "rpgFind";
            this.rpgFind.ShowCaptionButton = false;
            this.rpgFind.Text = "Filter Actions";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "图片文件|*.png";
            // 
            // popupMenu2
            // 
            this.popupMenu2.Name = "popupMenu2";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            // 
            // GraphDrawing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 449);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pcMain);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GraphDrawing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GIS图形展示平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GISPlatformCenter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GISPlatformCenter_FormClosed);
            this.Load += new System.EventHandler(this.GISPlatformCenter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcMain)).EndInit();
            this.pcMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.galleryControl1)).EndInit();
            this.galleryControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl pcMain;
        private DevExpress.XtraBars.Ribbon.GalleryControl galleryControl1;
        private DevExpress.XtraBars.Ribbon.GalleryControlClient galleryControlClient1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpSearch;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbgClose;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgFilterColumns;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgFind;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Axmetamap2dLib.AxMetaMapX2D mx;
        private DevExpress.XtraBars.PopupMenu popupMenu2;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
    }
}