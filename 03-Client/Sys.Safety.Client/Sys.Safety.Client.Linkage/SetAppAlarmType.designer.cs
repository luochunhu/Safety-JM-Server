namespace Sys.Safety.Client.Linkage
{
    partial class SetAppAlarmType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetAppAlarmType));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.GridControlDataState = new DevExpress.XtraGrid.GridControl();
            this.GridViewDataState = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DataStateCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.StrEnumDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LngEnumValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridControlDataState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDataState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.GridControlDataState);
            this.groupControl1.Location = new System.Drawing.Point(4, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(542, 489);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "触发数据状态";
            // 
            // GridControlDataState
            // 
            this.GridControlDataState.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.GridControlDataState.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.GridControlDataState.Location = new System.Drawing.Point(2, 22);
            this.GridControlDataState.MainView = this.GridViewDataState;
            this.GridControlDataState.Name = "GridControlDataState";
            this.GridControlDataState.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit3});
            this.GridControlDataState.Size = new System.Drawing.Size(538, 465);
            this.GridControlDataState.TabIndex = 5;
            this.GridControlDataState.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewDataState});
            // 
            // GridViewDataState
            // 
            this.GridViewDataState.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewDataState.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GridViewDataState.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.GridViewDataState.AppearancePrint.HeaderPanel.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.GridViewDataState.AppearancePrint.Row.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridViewDataState.AppearancePrint.Row.Options.UseFont = true;
            this.GridViewDataState.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DataStateCheck,
            this.StrEnumDisplay,
            this.LngEnumValue});
            this.GridViewDataState.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.GridViewDataState.GridControl = this.GridControlDataState;
            this.GridViewDataState.GroupFormat = "{1} {2}";
            this.GridViewDataState.IndicatorWidth = 40;
            this.GridViewDataState.Name = "GridViewDataState";
            this.GridViewDataState.OptionsDetail.AllowZoomDetail = false;
            this.GridViewDataState.OptionsMenu.EnableFooterMenu = false;
            this.GridViewDataState.OptionsPrint.AutoWidth = false;
            this.GridViewDataState.OptionsPrint.PrintPreview = true;
            this.GridViewDataState.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GridViewDataState.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridViewDataState.OptionsSelection.InvertSelection = true;
            this.GridViewDataState.OptionsView.ColumnAutoWidth = false;
            this.GridViewDataState.OptionsView.RowAutoHeight = true;
            this.GridViewDataState.OptionsView.ShowGroupPanel = false;
            // 
            // DataStateCheck
            // 
            this.DataStateCheck.Caption = " ";
            this.DataStateCheck.ColumnEdit = this.repositoryItemCheckEdit3;
            this.DataStateCheck.CustomizationCaption = " ";
            this.DataStateCheck.FieldName = "Check";
            this.DataStateCheck.Name = "DataStateCheck";
            this.DataStateCheck.Visible = true;
            this.DataStateCheck.VisibleIndex = 0;
            this.DataStateCheck.Width = 30;
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.Caption = "Check";
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            // 
            // StrEnumDisplay
            // 
            this.StrEnumDisplay.Caption = "数据状态";
            this.StrEnumDisplay.FieldName = "StrEnumDisplay";
            this.StrEnumDisplay.Name = "StrEnumDisplay";
            this.StrEnumDisplay.OptionsColumn.AllowEdit = false;
            this.StrEnumDisplay.Visible = true;
            this.StrEnumDisplay.VisibleIndex = 1;
            this.StrEnumDisplay.Width = 374;
            // 
            // LngEnumValue
            // 
            this.LngEnumValue.Caption = "LngEnumValue";
            this.LngEnumValue.FieldName = "LngEnumValue";
            this.LngEnumValue.Name = "LngEnumValue";
            // 
            // btn_ok
            // 
            this.btn_ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_ok.Image")));
            this.btn_ok.Location = new System.Drawing.Point(433, 498);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(92, 41);
            this.btn_ok.TabIndex = 11;
            this.btn_ok.Text = "保 存";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // SetAppAlarmType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 551);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetAppAlarmType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "APP报警推动设置";
            this.Load += new System.EventHandler(this.SetAppAlarmType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridControlDataState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDataState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl GridControlDataState;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewDataState;
        private DevExpress.XtraGrid.Columns.GridColumn DataStateCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn StrEnumDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn LngEnumValue;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
    }
}