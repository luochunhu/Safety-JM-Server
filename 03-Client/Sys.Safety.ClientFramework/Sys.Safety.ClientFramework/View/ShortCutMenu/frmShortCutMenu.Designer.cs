namespace Sys.Safety.ClientFramework.View.ShortCutMenu
{
    partial class frmShortCutMenu
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
            this.navMenu = new DevExpress.XtraNavBar.NavBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.navMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // navMenu
            // 
            this.navMenu.ActiveGroup = null;
            this.navMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navMenu.Location = new System.Drawing.Point(0, 0);
            this.navMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.navMenu.Name = "navMenu";
            this.navMenu.OptionsNavPane.ExpandedWidth = 299;
            this.navMenu.Size = new System.Drawing.Size(299, 586);
            this.navMenu.TabIndex = 0;
            this.navMenu.Text = "快捷菜单";
            this.navMenu.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navMenu_LinkClicked);
            this.navMenu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.navMenu_KeyDown);
            // 
            // frmShortCutMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 586);
            this.Controls.Add(this.navMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 800);
            this.MinimumSize = new System.Drawing.Size(358, 792);
            this.Name = "frmShortCutMenu";
            this.Text = "我的菜单";
            this.Load += new System.EventHandler(this.frmShortCutMenu_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmShortCutMenu_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.navMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navMenu;

    }
}