namespace Sys.Safety.Client.DataAnalysis
{
    partial class BigDataAnalysisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BigDataAnalysisForm));
            this.xtraTabControlChildFrom = new DevExpress.XtraTab.XtraTabControl();
            this.tabAnalysisTemplateManage = new DevExpress.XtraTab.XtraTabPage();
            this.TabLargedataAnalysisManage = new DevExpress.XtraTab.XtraTabPage();
            this.TabSetAlarmNotificationPersonnelMange = new DevExpress.XtraTab.XtraTabPage();
            this.TabSetRegionOutageManage = new DevExpress.XtraTab.XtraTabPage();
            this.TabSetEmergencyLinkage = new DevExpress.XtraTab.XtraTabPage();
            this.TabCurvilinearQuery = new DevExpress.XtraTab.XtraTabPage();
            this.TabAnalysisResultsInRealtime = new DevExpress.XtraTab.XtraTabPage();
            this.TabViewEmergencyLinkage = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlChildFrom)).BeginInit();
            this.xtraTabControlChildFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControlChildFrom
            // 
            this.xtraTabControlChildFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlChildFrom.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlChildFrom.Name = "xtraTabControlChildFrom";
            this.xtraTabControlChildFrom.SelectedTabPage = this.tabAnalysisTemplateManage;
            this.xtraTabControlChildFrom.Size = new System.Drawing.Size(1401, 789);
            this.xtraTabControlChildFrom.TabIndex = 0;
            this.xtraTabControlChildFrom.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabAnalysisTemplateManage,
            this.TabLargedataAnalysisManage,
            this.TabSetAlarmNotificationPersonnelMange,
            this.TabSetRegionOutageManage,
            this.TabSetEmergencyLinkage,
            this.TabCurvilinearQuery,
            this.TabAnalysisResultsInRealtime,
            this.TabViewEmergencyLinkage});
            this.xtraTabControlChildFrom.Selected += new DevExpress.XtraTab.TabPageEventHandler(this.xtraTabControlChildFrom_Selected);
            // 
            // tabAnalysisTemplateManage
            // 
            this.tabAnalysisTemplateManage.Name = "tabAnalysisTemplateManage";
            this.tabAnalysisTemplateManage.Size = new System.Drawing.Size(1395, 756);
            this.tabAnalysisTemplateManage.Text = "逻辑分析模板管理";
            // 
            // TabLargedataAnalysisManage
            // 
            this.TabLargedataAnalysisManage.Name = "TabLargedataAnalysisManage";
            this.TabLargedataAnalysisManage.Size = new System.Drawing.Size(1395, 756);
            this.TabLargedataAnalysisManage.Text = "大数据分析模型管理";
            // 
            // TabSetAlarmNotificationPersonnelMange
            // 
            this.TabSetAlarmNotificationPersonnelMange.Name = "TabSetAlarmNotificationPersonnelMange";
            this.TabSetAlarmNotificationPersonnelMange.Size = new System.Drawing.Size(1395, 756);
            this.TabSetAlarmNotificationPersonnelMange.Text = "报警推送管理";
            // 
            // TabSetRegionOutageManage
            // 
            this.TabSetRegionOutageManage.Name = "TabSetRegionOutageManage";
            this.TabSetRegionOutageManage.Size = new System.Drawing.Size(1395, 756);
            this.TabSetRegionOutageManage.Text = "区域断电管理";
            // 
            // TabSetEmergencyLinkage
            // 
            this.TabSetEmergencyLinkage.Name = "TabSetEmergencyLinkage";
            this.TabSetEmergencyLinkage.Size = new System.Drawing.Size(1395, 756);
            this.TabSetEmergencyLinkage.Text = "应急联动管理";
            // 
            // TabCurvilinearQuery
            // 
            this.TabCurvilinearQuery.Name = "TabCurvilinearQuery";
            this.TabCurvilinearQuery.Size = new System.Drawing.Size(1395, 756);
            this.TabCurvilinearQuery.Text = "曲线查询";
            // 
            // TabAnalysisResultsInRealtime
            // 
            this.TabAnalysisResultsInRealtime.Name = "TabAnalysisResultsInRealtime";
            this.TabAnalysisResultsInRealtime.Size = new System.Drawing.Size(1395, 756);
            this.TabAnalysisResultsInRealtime.Text = "大数据分析实时列表";
            // 
            // TabViewEmergencyLinkage
            // 
            this.TabViewEmergencyLinkage.Name = "TabViewEmergencyLinkage";
            this.TabViewEmergencyLinkage.Size = new System.Drawing.Size(1395, 756);
            this.TabViewEmergencyLinkage.Text = "查看应急联动";
            // 
            // BigDataAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1401, 789);
            this.Controls.Add(this.xtraTabControlChildFrom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BigDataAnalysisForm";
            this.Text = "大数据分析";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BigDataAnalysisForm_FormClosing);
            this.Load += new System.EventHandler(this.BigDataAnalysisForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlChildFrom)).EndInit();
            this.xtraTabControlChildFrom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControlChildFrom;
        private DevExpress.XtraTab.XtraTabPage tabAnalysisTemplateManage;
        private DevExpress.XtraTab.XtraTabPage TabLargedataAnalysisManage;
        private DevExpress.XtraTab.XtraTabPage TabSetAlarmNotificationPersonnelMange;
        private DevExpress.XtraTab.XtraTabPage TabSetRegionOutageManage;
        private DevExpress.XtraTab.XtraTabPage TabSetEmergencyLinkage;
        private DevExpress.XtraTab.XtraTabPage TabCurvilinearQuery;
        private DevExpress.XtraTab.XtraTabPage TabAnalysisResultsInRealtime;
        private DevExpress.XtraTab.XtraTabPage TabViewEmergencyLinkage;
    }
}