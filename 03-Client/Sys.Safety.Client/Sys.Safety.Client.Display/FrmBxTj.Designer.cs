namespace Sys.Safety.Client.Display
{
    partial class FrmBxTj
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBxTj));
            this.syDay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BxTjGri = new DevExpress.XtraGrid.GridControl();
            this.gvLogInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ybxcd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.wbxcd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.QueryTime = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Query = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.BxTjGri)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueryTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // syDay
            // 
            this.syDay.Caption = "剩余天数";
            this.syDay.FieldName = "syDay";
            this.syDay.Name = "syDay";
            this.syDay.Visible = true;
            this.syDay.VisibleIndex = 3;
            this.syDay.Width = 185;
            // 
            // BxTjGri
            // 
            this.BxTjGri.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BxTjGri.Location = new System.Drawing.Point(0, 67);
            this.BxTjGri.MainView = this.gvLogInfo;
            this.BxTjGri.Name = "BxTjGri";
            this.BxTjGri.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemMemoEdit2});
            this.BxTjGri.Size = new System.Drawing.Size(884, 395);
            this.BxTjGri.TabIndex = 3;
            this.BxTjGri.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLogInfo});
            this.BxTjGri.DoubleClick += new System.EventHandler(this.BxTjGri_DoubleClick);
            // 
            // gvLogInfo
            // 
            this.gvLogInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.time,
            this.ybxcd,
            this.wbxcd,
            this.syDay});
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Column = this.syDay;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "已过期";
            this.gvLogInfo.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gvLogInfo.GridControl = this.BxTjGri;
            this.gvLogInfo.IndicatorWidth = 30;
            this.gvLogInfo.Name = "gvLogInfo";
            this.gvLogInfo.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gvLogInfo.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.gvLogInfo.OptionsBehavior.Editable = false;
            this.gvLogInfo.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gvLogInfo.OptionsCustomization.AllowColumnMoving = false;
            this.gvLogInfo.OptionsCustomization.AllowGroup = false;
            this.gvLogInfo.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvLogInfo.OptionsFilter.AllowFilterEditor = false;
            this.gvLogInfo.OptionsMenu.EnableColumnMenu = false;
            this.gvLogInfo.OptionsMenu.EnableFooterMenu = false;
            this.gvLogInfo.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLogInfo.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvLogInfo.OptionsView.RowAutoHeight = true;
            this.gvLogInfo.OptionsView.ShowDetailButtons = false;
            this.gvLogInfo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvLogInfo.OptionsView.ShowGroupPanel = false;
            // 
            // time
            // 
            this.time.Caption = "标校时间";
            this.time.FieldName = "time";
            this.time.Name = "time";
            this.time.OptionsFilter.AllowFilter = false;
            this.time.Visible = true;
            this.time.VisibleIndex = 0;
            this.time.Width = 120;
            // 
            // ybxcd
            // 
            this.ybxcd.AppearanceCell.ForeColor = System.Drawing.Color.Green;
            this.ybxcd.AppearanceCell.Options.UseForeColor = true;
            this.ybxcd.Caption = "已标校测点";
            this.ybxcd.ColumnEdit = this.repositoryItemMemoEdit1;
            this.ybxcd.FieldName = "ybxcd";
            this.ybxcd.Name = "ybxcd";
            this.ybxcd.OptionsFilter.AllowFilter = false;
            this.ybxcd.Visible = true;
            this.ybxcd.VisibleIndex = 1;
            this.ybxcd.Width = 300;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // wbxcd
            // 
            this.wbxcd.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.wbxcd.AppearanceCell.Options.UseForeColor = true;
            this.wbxcd.Caption = "未标校测点";
            this.wbxcd.ColumnEdit = this.repositoryItemMemoEdit2;
            this.wbxcd.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.wbxcd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.wbxcd.FieldName = "wbxcd";
            this.wbxcd.Name = "wbxcd";
            this.wbxcd.Visible = true;
            this.wbxcd.VisibleIndex = 2;
            this.wbxcd.Width = 300;
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            // 
            // QueryTime
            // 
            this.QueryTime.Location = new System.Drawing.Point(42, 12);
            this.QueryTime.Name = "QueryTime";
            this.QueryTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.QueryTime.Size = new System.Drawing.Size(100, 20);
            this.QueryTime.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "年份";
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(148, 11);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(47, 23);
            this.Query.TabIndex = 6;
            this.Query.Text = "查询";
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl5.Location = new System.Drawing.Point(12, 40);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(180, 14);
            this.labelControl5.TabIndex = 102;
            this.labelControl5.Text = "操作提示：双击查询详细标校信息";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(229, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(84, 23);
            this.simpleButton1.TabIndex = 103;
            this.simpleButton1.Text = "编辑标校测点";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmBxTj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 462);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.Query);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.QueryTime);
            this.Controls.Add(this.BxTjGri);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBxTj";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标校记录统计";
            ((System.ComponentModel.ISupportInitialize)(this.BxTjGri)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueryTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl BxTjGri;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLogInfo;
        private DevExpress.XtraGrid.Columns.GridColumn time;
        private DevExpress.XtraGrid.Columns.GridColumn ybxcd;
        private DevExpress.XtraGrid.Columns.GridColumn wbxcd;
        private DevExpress.XtraGrid.Columns.GridColumn syDay;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private DevExpress.XtraEditors.ComboBoxEdit QueryTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton Query;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}