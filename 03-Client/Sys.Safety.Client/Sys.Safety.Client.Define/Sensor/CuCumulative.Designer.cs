namespace Sys.Safety.Client.Define.Sensor
{
    partial class CuCumulative
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.Cp2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.IsEnableRevised = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.CpDocunment = new DevExpress.XtraEditors.PanelControl();
            this.RevisedValue = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Cp2)).BeginInit();
            this.Cp2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IsEnableRevised.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpDocunment)).BeginInit();
            this.CpDocunment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RevisedValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.Size = new System.Drawing.Size(496, 368);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.Cp2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(490, 339);
            this.xtraTabPage2.Text = "详细配置";
            // 
            // Cp2
            // 
            this.Cp2.Controls.Add(this.groupControl1);
            this.Cp2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cp2.Location = new System.Drawing.Point(0, 0);
            this.Cp2.Name = "Cp2";
            this.Cp2.Size = new System.Drawing.Size(490, 339);
            this.Cp2.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(486, 72);
            this.groupControl1.TabIndex = 63;
            this.groupControl1.Text = "详细配置";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.RevisedValue);
            this.layoutControl1.Controls.Add(this.IsEnableRevised);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 22);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(482, 48);
            this.layoutControl1.TabIndex = 62;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // IsEnableRevised
            // 
            this.IsEnableRevised.Location = new System.Drawing.Point(12, 12);
            this.IsEnableRevised.Name = "IsEnableRevised";
            this.IsEnableRevised.Properties.Caption = "是否启用";
            this.IsEnableRevised.Size = new System.Drawing.Size(97, 19);
            this.IsEnableRevised.StyleController = this.layoutControl1;
            this.IsEnableRevised.TabIndex = 51;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(482, 48);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.IsEnableRevised;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(101, 28);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // CpDocunment
            // 
            this.CpDocunment.Controls.Add(this.xtraTabControl1);
            this.CpDocunment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CpDocunment.Location = new System.Drawing.Point(0, 0);
            this.CpDocunment.Name = "CpDocunment";
            this.CpDocunment.Size = new System.Drawing.Size(500, 372);
            this.CpDocunment.TabIndex = 0;
            // 
            // RevisedValue
            // 
            this.RevisedValue.Location = new System.Drawing.Point(164, 12);
            this.RevisedValue.Name = "RevisedValue";
            this.RevisedValue.Size = new System.Drawing.Size(306, 20);
            this.RevisedValue.StyleController = this.layoutControl1;
            this.RevisedValue.TabIndex = 52;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.RevisedValue;
            this.layoutControlItem2.CustomizationFormText = "修正值：";
            this.layoutControlItem2.Location = new System.Drawing.Point(101, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(361, 28);
            this.layoutControlItem2.Text = "修正值：";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // CuCumulative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CpDocunment);
            this.Name = "CuCumulative";
            this.Size = new System.Drawing.Size(500, 372);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Cp2)).EndInit();
            this.Cp2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IsEnableRevised.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CpDocunment)).EndInit();
            this.CpDocunment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RevisedValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.PanelControl Cp2;
        private DevExpress.XtraEditors.CheckEdit IsEnableRevised;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.PanelControl CpDocunment;
        private DevExpress.XtraEditors.TextEdit RevisedValue;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
