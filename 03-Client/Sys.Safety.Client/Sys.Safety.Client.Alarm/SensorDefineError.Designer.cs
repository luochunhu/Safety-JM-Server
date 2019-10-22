namespace Sys.Safety.Client.Alarm
{
    partial class SensorDefineError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorDefineError));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pointid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Point = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Position = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DevName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UpAarmValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DownAarmValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UpDdValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DownDdValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UpHfValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DownHfValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LC2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SeniorGradeAlarmValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SeniorGradeTimeValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.panelControl1);
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(673, 322, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1008, 515);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Location = new System.Drawing.Point(4, 480);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1000, 31);
            this.panelControl1.TabIndex = 5;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(1036, 3);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "导出数据";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(4, 4);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemButtonEdit2});
            this.gridControl1.Size = new System.Drawing.Size(1000, 472);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.pointid,
            this.Point,
            this.Position,
            this.DevName,
            this.UpAarmValue,
            this.DownAarmValue,
            this.UpDdValue,
            this.DownDdValue,
            this.UpHfValue,
            this.DownHfValue,
            this.LC2,
            this.SeniorGradeAlarmValue,
            this.SeniorGradeTimeValue,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // pointid
            // 
            this.pointid.Caption = "pointid";
            this.pointid.FieldName = "PointId";
            this.pointid.Name = "pointid";
            // 
            // Point
            // 
            this.Point.Caption = "测点号";
            this.Point.FieldName = "Point";
            this.Point.Name = "Point";
            this.Point.Visible = true;
            this.Point.VisibleIndex = 0;
            // 
            // Position
            // 
            this.Position.Caption = "安装位置";
            this.Position.FieldName = "Position";
            this.Position.Name = "Position";
            this.Position.Visible = true;
            this.Position.VisibleIndex = 1;
            // 
            // DevName
            // 
            this.DevName.Caption = "设备类型";
            this.DevName.FieldName = "DevName";
            this.DevName.Name = "DevName";
            this.DevName.Visible = true;
            this.DevName.VisibleIndex = 2;
            // 
            // UpAarmValue
            // 
            this.UpAarmValue.Caption = "上报值";
            this.UpAarmValue.FieldName = "UpAarmValue";
            this.UpAarmValue.Name = "UpAarmValue";
            this.UpAarmValue.Visible = true;
            this.UpAarmValue.VisibleIndex = 3;
            // 
            // DownAarmValue
            // 
            this.DownAarmValue.Caption = "下报值";
            this.DownAarmValue.FieldName = "DownAarmValue";
            this.DownAarmValue.Name = "DownAarmValue";
            this.DownAarmValue.Visible = true;
            this.DownAarmValue.VisibleIndex = 4;
            // 
            // UpDdValue
            // 
            this.UpDdValue.Caption = "上断值";
            this.UpDdValue.FieldName = "UpDdValue";
            this.UpDdValue.Name = "UpDdValue";
            this.UpDdValue.Visible = true;
            this.UpDdValue.VisibleIndex = 6;
            // 
            // DownDdValue
            // 
            this.DownDdValue.Caption = "下断值";
            this.DownDdValue.FieldName = "DownDdValue";
            this.DownDdValue.Name = "DownDdValue";
            this.DownDdValue.Visible = true;
            this.DownDdValue.VisibleIndex = 5;
            // 
            // UpHfValue
            // 
            this.UpHfValue.Caption = "上恢复值";
            this.UpHfValue.FieldName = "UpHfValue";
            this.UpHfValue.Name = "UpHfValue";
            this.UpHfValue.Visible = true;
            this.UpHfValue.VisibleIndex = 7;
            // 
            // DownHfValue
            // 
            this.DownHfValue.Caption = "下恢复值";
            this.DownHfValue.FieldName = "DownHfValue";
            this.DownHfValue.Name = "DownHfValue";
            this.DownHfValue.Visible = true;
            this.DownHfValue.VisibleIndex = 8;
            // 
            // LC2
            // 
            this.LC2.Caption = "量程";
            this.LC2.FieldName = "LC2";
            this.LC2.Name = "LC2";
            // 
            // SeniorGradeAlarmValue
            // 
            this.SeniorGradeAlarmValue.AppearanceCell.Options.UseTextOptions = true;
            this.SeniorGradeAlarmValue.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.SeniorGradeAlarmValue.Caption = "分级报警值";
            this.SeniorGradeAlarmValue.FieldName = "SeniorGradeAlarmValue";
            this.SeniorGradeAlarmValue.Name = "SeniorGradeAlarmValue";
            this.SeniorGradeAlarmValue.Visible = true;
            this.SeniorGradeAlarmValue.VisibleIndex = 9;
            // 
            // SeniorGradeTimeValue
            // 
            this.SeniorGradeTimeValue.AppearanceCell.Options.UseTextOptions = true;
            this.SeniorGradeTimeValue.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.SeniorGradeTimeValue.Caption = "分级报警时间";
            this.SeniorGradeTimeValue.FieldName = "SeniorGradeTimeValue";
            this.SeniorGradeTimeValue.Name = "SeniorGradeTimeValue";
            this.SeniorGradeTimeValue.Visible = true;
            this.SeniorGradeTimeValue.VisibleIndex = 10;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "以中心站为准";
            this.gridColumn1.ColumnEdit = this.repositoryItemButtonEdit1;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 11;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "中心站为准", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit1.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            this.repositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit1_ButtonClick);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "以传感器为准";
            this.gridColumn2.ColumnEdit = this.repositoryItemButtonEdit2;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 12;
            // 
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit2.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            this.repositoryItemButtonEdit2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit2_ButtonClick);
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
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1008, 515);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1004, 476);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.panelControl1;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 476);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1004, 35);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // SensorDefineError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 515);
            this.Controls.Add(this.layoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SensorDefineError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "传感器上下定义不匹配查看";
            this.Load += new System.EventHandler(this.SensorCalibration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn Point;
        private DevExpress.XtraGrid.Columns.GridColumn Position;
        private DevExpress.XtraGrid.Columns.GridColumn DevName;
        private DevExpress.XtraGrid.Columns.GridColumn UpAarmValue;
        private DevExpress.XtraGrid.Columns.GridColumn DownAarmValue;
        private DevExpress.XtraGrid.Columns.GridColumn UpDdValue;
        private DevExpress.XtraGrid.Columns.GridColumn pointid;
        private DevExpress.XtraGrid.Columns.GridColumn DownDdValue;
        private DevExpress.XtraGrid.Columns.GridColumn UpHfValue;
        private DevExpress.XtraGrid.Columns.GridColumn DownHfValue;
        private DevExpress.XtraGrid.Columns.GridColumn LC2;
        private DevExpress.XtraGrid.Columns.GridColumn SeniorGradeAlarmValue;
        private DevExpress.XtraGrid.Columns.GridColumn SeniorGradeTimeValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
    }
}