namespace Sys.Safety.Client.Control.PersonInfo
{
    partial class PersonSetForm
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonSetForm));
            this.GridControlAllPeople = new DevExpress.XtraGrid.GridControl();
            this.GridViewAllPeople = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.QueName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.GridControlSelectPeople = new DevExpress.XtraGrid.GridControl();
            this.GridViewSelectPeople = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SelName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ButAdd = new DevExpress.XtraEditors.SimpleButton();
            this.ButAddAll = new DevExpress.XtraEditors.SimpleButton();
            this.ButDelete = new DevExpress.XtraEditors.SimpleButton();
            this.ButDeleteAll = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Query = new DevExpress.XtraEditors.SimpleButton();
            this.TextEditName = new DevExpress.XtraEditors.TextEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GridControlAllPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAllPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControlSelectPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewSelectPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridControlAllPeople
            // 
            gridLevelNode1.RelationName = "Level1";
            this.GridControlAllPeople.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.GridControlAllPeople.Location = new System.Drawing.Point(0, 39);
            this.GridControlAllPeople.MainView = this.GridViewAllPeople;
            this.GridControlAllPeople.Name = "GridControlAllPeople";
            this.GridControlAllPeople.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.GridControlAllPeople.Size = new System.Drawing.Size(294, 410);
            this.GridControlAllPeople.TabIndex = 5;
            this.GridControlAllPeople.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewAllPeople});
            // 
            // GridViewAllPeople
            // 
            this.GridViewAllPeople.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewAllPeople.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GridViewAllPeople.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.GridViewAllPeople.AppearancePrint.HeaderPanel.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.GridViewAllPeople.AppearancePrint.Row.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewAllPeople.AppearancePrint.Row.Options.UseFont = true;
            this.GridViewAllPeople.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.QueName});
            this.GridViewAllPeople.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.GridViewAllPeople.GridControl = this.GridControlAllPeople;
            this.GridViewAllPeople.GroupFormat = "{1} {2}";
            this.GridViewAllPeople.IndicatorWidth = 40;
            this.GridViewAllPeople.Name = "GridViewAllPeople";
            this.GridViewAllPeople.OptionsDetail.AllowZoomDetail = false;
            this.GridViewAllPeople.OptionsMenu.EnableFooterMenu = false;
            this.GridViewAllPeople.OptionsPrint.AutoWidth = false;
            this.GridViewAllPeople.OptionsPrint.PrintPreview = true;
            this.GridViewAllPeople.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GridViewAllPeople.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridViewAllPeople.OptionsSelection.InvertSelection = true;
            this.GridViewAllPeople.OptionsSelection.MultiSelect = true;
            this.GridViewAllPeople.OptionsView.ColumnAutoWidth = false;
            this.GridViewAllPeople.OptionsView.RowAutoHeight = true;
            this.GridViewAllPeople.OptionsView.ShowGroupPanel = false;
            // 
            // QueName
            // 
            this.QueName.Caption = "姓名";
            this.QueName.FieldName = "Name";
            this.QueName.Name = "QueName";
            this.QueName.Visible = true;
            this.QueName.VisibleIndex = 0;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // GridControlSelectPeople
            // 
            this.GridControlSelectPeople.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode2.RelationName = "Level1";
            this.GridControlSelectPeople.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.GridControlSelectPeople.Location = new System.Drawing.Point(0, 0);
            this.GridControlSelectPeople.MainView = this.GridViewSelectPeople;
            this.GridControlSelectPeople.Name = "GridControlSelectPeople";
            this.GridControlSelectPeople.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2});
            this.GridControlSelectPeople.Size = new System.Drawing.Size(294, 449);
            this.GridControlSelectPeople.TabIndex = 6;
            this.GridControlSelectPeople.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewSelectPeople});
            // 
            // GridViewSelectPeople
            // 
            this.GridViewSelectPeople.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewSelectPeople.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GridViewSelectPeople.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.GridViewSelectPeople.AppearancePrint.HeaderPanel.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.GridViewSelectPeople.AppearancePrint.Row.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewSelectPeople.AppearancePrint.Row.Options.UseFont = true;
            this.GridViewSelectPeople.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SelName});
            this.GridViewSelectPeople.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.GridViewSelectPeople.GridControl = this.GridControlSelectPeople;
            this.GridViewSelectPeople.GroupFormat = "{1} {2}";
            this.GridViewSelectPeople.IndicatorWidth = 40;
            this.GridViewSelectPeople.Name = "GridViewSelectPeople";
            this.GridViewSelectPeople.OptionsDetail.AllowZoomDetail = false;
            this.GridViewSelectPeople.OptionsMenu.EnableFooterMenu = false;
            this.GridViewSelectPeople.OptionsPrint.AutoWidth = false;
            this.GridViewSelectPeople.OptionsPrint.PrintPreview = true;
            this.GridViewSelectPeople.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GridViewSelectPeople.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridViewSelectPeople.OptionsSelection.InvertSelection = true;
            this.GridViewSelectPeople.OptionsSelection.MultiSelect = true;
            this.GridViewSelectPeople.OptionsView.ColumnAutoWidth = false;
            this.GridViewSelectPeople.OptionsView.RowAutoHeight = true;
            this.GridViewSelectPeople.OptionsView.ShowGroupPanel = false;
            // 
            // SelName
            // 
            this.SelName.Caption = "姓名";
            this.SelName.FieldName = "Name";
            this.SelName.Name = "SelName";
            this.SelName.Visible = true;
            this.SelName.VisibleIndex = 0;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Caption = "Check";
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // ButAdd
            // 
            this.ButAdd.Location = new System.Drawing.Point(336, 128);
            this.ButAdd.Name = "ButAdd";
            this.ButAdd.Size = new System.Drawing.Size(75, 23);
            this.ButAdd.TabIndex = 7;
            this.ButAdd.Text = ">添加";
            this.ButAdd.Click += new System.EventHandler(this.ButAdd_Click);
            // 
            // ButAddAll
            // 
            this.ButAddAll.Location = new System.Drawing.Point(336, 157);
            this.ButAddAll.Name = "ButAddAll";
            this.ButAddAll.Size = new System.Drawing.Size(75, 23);
            this.ButAddAll.TabIndex = 8;
            this.ButAddAll.Text = ">>添加所有";
            this.ButAddAll.Click += new System.EventHandler(this.ButAddAll_Click);
            // 
            // ButDelete
            // 
            this.ButDelete.Location = new System.Drawing.Point(336, 186);
            this.ButDelete.Name = "ButDelete";
            this.ButDelete.Size = new System.Drawing.Size(75, 23);
            this.ButDelete.TabIndex = 9;
            this.ButDelete.Text = "<删除";
            this.ButDelete.Click += new System.EventHandler(this.ButDelete_Click);
            // 
            // ButDeleteAll
            // 
            this.ButDeleteAll.Location = new System.Drawing.Point(336, 215);
            this.ButDeleteAll.Name = "ButDeleteAll";
            this.ButDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.ButDeleteAll.TabIndex = 10;
            this.ButDeleteAll.Text = "<<删除所有";
            this.ButDeleteAll.Click += new System.EventHandler(this.ButDeleteAll_Click);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.Query);
            this.xtraTabPage1.Controls.Add(this.TextEditName);
            this.xtraTabPage1.Controls.Add(this.GridControlAllPeople);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(294, 449);
            this.xtraTabPage1.Text = "所有人员";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "人员名称";
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(217, 13);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(47, 20);
            this.Query.TabIndex = 7;
            this.Query.Text = "查询";
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // TextEditName
            // 
            this.TextEditName.Location = new System.Drawing.Point(79, 13);
            this.TextEditName.Name = "TextEditName";
            this.TextEditName.Size = new System.Drawing.Size(132, 20);
            this.TextEditName.TabIndex = 6;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 12);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(300, 478);
            this.xtraTabControl1.TabIndex = 11;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Location = new System.Drawing.Point(438, 12);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl2.Size = new System.Drawing.Size(300, 478);
            this.xtraTabControl2.TabIndex = 12;
            this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.GridControlSelectPeople);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(294, 449);
            this.xtraTabPage2.Text = "已选择人员";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(336, 440);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(75, 23);
            this.simpleButton5.TabIndex = 13;
            this.simpleButton5.Text = "取消";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(336, 411);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(75, 23);
            this.simpleButton6.TabIndex = 14;
            this.simpleButton6.Text = "确认";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // PersonSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 502);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.xtraTabControl2);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.ButDeleteAll);
            this.Controls.Add(this.ButDelete);
            this.Controls.Add(this.ButAddAll);
            this.Controls.Add(this.ButAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人员设置";
            this.Load += new System.EventHandler(this.PersonSetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridControlAllPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAllPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControlSelectPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewSelectPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GridControlAllPeople;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewAllPeople;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.GridControl GridControlSelectPeople;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewSelectPeople;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.SimpleButton ButAdd;
        private DevExpress.XtraEditors.SimpleButton ButAddAll;
        private DevExpress.XtraEditors.SimpleButton ButDelete;
        private DevExpress.XtraEditors.SimpleButton ButDeleteAll;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton Query;
        private DevExpress.XtraEditors.TextEdit TextEditName;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraGrid.Columns.GridColumn QueName;
        private DevExpress.XtraGrid.Columns.GridColumn SelName;

    }
}