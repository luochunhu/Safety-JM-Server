namespace Sys.Safety.Client.Display
{
    partial class WorkMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkMainForm));
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.switchStateDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.switchState_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dataNagDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dataNag_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.runRecordPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.automaticArtPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.anaEWarDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.anaEWar_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.anaWCPowDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.anaWCPow_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.anaDDPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.anaDD_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.switchWCPowDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.switchWCPow_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.fpowExeDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.fpowExe_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.realControlDkPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.realControl_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.handControlDockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.mworkPControl = new DevExpress.XtraEditors.PanelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.switchStateDkPanel.SuspendLayout();
            this.dataNagDkPanel.SuspendLayout();
            this.panelContainer1.SuspendLayout();
            this.runRecordPanel.SuspendLayout();
            this.automaticArtPanel.SuspendLayout();
            this.anaEWarDkPanel.SuspendLayout();
            this.anaWCPowDkPanel.SuspendLayout();
            this.anaDDPanel.SuspendLayout();
            this.switchWCPowDkPanel.SuspendLayout();
            this.fpowExeDkPanel.SuspendLayout();
            this.realControlDkPanel.SuspendLayout();
            this.handControlDockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mworkPControl)).BeginInit();
            this.SuspendLayout();
            // 
            // dockManager
            // 
            this.dockManager.DockingOptions.ShowCloseButton = false;
            this.dockManager.Form = this;
            this.dockManager.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.switchStateDkPanel});
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dataNagDkPanel,
            this.panelContainer1});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // switchStateDkPanel
            // 
            this.switchStateDkPanel.Appearance.BackColor2 = System.Drawing.Color.Red;
            this.switchStateDkPanel.Appearance.BorderColor = System.Drawing.Color.Red;
            this.switchStateDkPanel.Appearance.ForeColor = System.Drawing.Color.Red;
            this.switchStateDkPanel.Appearance.Options.UseBorderColor = true;
            this.switchStateDkPanel.Appearance.Options.UseForeColor = true;
            this.switchStateDkPanel.Controls.Add(this.switchState_Container);
            this.switchStateDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Float;
            this.switchStateDkPanel.FloatLocation = new System.Drawing.Point(592, 395);
            this.switchStateDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.switchStateDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.switchStateDkPanel.ID = new System.Guid("8404f4dc-c47b-4674-a537-0a8e80d9a955");
            this.switchStateDkPanel.Location = new System.Drawing.Point(-32768, -32768);
            this.switchStateDkPanel.Name = "switchStateDkPanel";
            this.switchStateDkPanel.Options.AllowDockTop = false;
            this.switchStateDkPanel.Options.ShowCloseButton = false;
            this.switchStateDkPanel.OriginalSize = new System.Drawing.Size(809, 146);
            this.switchStateDkPanel.SavedIndex = 1;
            this.switchStateDkPanel.Size = new System.Drawing.Size(900, 199);
            this.switchStateDkPanel.Text = "开关量状态变动";
            this.switchStateDkPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            this.switchStateDkPanel.Click += new System.EventHandler(this.switchStateDkPanel_Click);
            // 
            // switchState_Container
            // 
            this.switchState_Container.BackColor = System.Drawing.Color.Transparent;
            this.switchState_Container.Location = new System.Drawing.Point(3, 22);
            this.switchState_Container.Name = "switchState_Container";
            this.switchState_Container.Size = new System.Drawing.Size(894, 174);
            this.switchState_Container.TabIndex = 0;
            // 
            // dataNagDkPanel
            // 
            this.dataNagDkPanel.Controls.Add(this.dataNag_Container);
            this.dataNagDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dataNagDkPanel.FloatSize = new System.Drawing.Size(230, 500);
            this.dataNagDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.dataNagDkPanel.ID = new System.Guid("f65a5299-cfe8-4114-81d4-c2a411500da9");
            this.dataNagDkPanel.Location = new System.Drawing.Point(0, 0);
            this.dataNagDkPanel.Name = "dataNagDkPanel";
            this.dataNagDkPanel.Options.AllowDockFill = false;
            this.dataNagDkPanel.Options.AllowDockTop = false;
            this.dataNagDkPanel.Options.ShowCloseButton = false;
            this.dataNagDkPanel.OriginalSize = new System.Drawing.Size(260, 500);
            this.dataNagDkPanel.Size = new System.Drawing.Size(260, 544);
            this.dataNagDkPanel.Text = "数据显示导航";
            // 
            // dataNag_Container
            // 
            this.dataNag_Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataNag_Container.Location = new System.Drawing.Point(4, 23);
            this.dataNag_Container.Name = "dataNag_Container";
            this.dataNag_Container.Size = new System.Drawing.Size(252, 517);
            this.dataNag_Container.TabIndex = 0;
            // 
            // panelContainer1
            // 
            this.panelContainer1.ActiveChild = this.runRecordPanel;
            this.panelContainer1.Controls.Add(this.runRecordPanel);
            this.panelContainer1.Controls.Add(this.automaticArtPanel);
            this.panelContainer1.Controls.Add(this.anaEWarDkPanel);
            this.panelContainer1.Controls.Add(this.anaWCPowDkPanel);
            this.panelContainer1.Controls.Add(this.anaDDPanel);
            this.panelContainer1.Controls.Add(this.switchWCPowDkPanel);
            this.panelContainer1.Controls.Add(this.fpowExeDkPanel);
            this.panelContainer1.Controls.Add(this.realControlDkPanel);
            this.panelContainer1.Controls.Add(this.handControlDockPanel);
            this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer1.FloatSize = new System.Drawing.Size(900, 199);
            this.panelContainer1.FloatVertical = true;
            this.panelContainer1.ID = new System.Guid("fb6d3943-b48b-417e-8adf-e504d1fb6087");
            this.panelContainer1.Location = new System.Drawing.Point(260, 343);
            this.panelContainer1.Name = "panelContainer1";
            this.panelContainer1.Options.AllowDockTop = false;
            this.panelContainer1.OriginalSize = new System.Drawing.Size(200, 201);
            this.panelContainer1.Size = new System.Drawing.Size(813, 201);
            this.panelContainer1.Tabbed = true;
            this.panelContainer1.Text = "panelContainer1";
            this.panelContainer1.Click += new System.EventHandler(this.panelContainer1_Click);
            // 
            // runRecordPanel
            // 
            this.runRecordPanel.Controls.Add(this.controlContainer1);
            this.runRecordPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.runRecordPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.runRecordPanel.ID = new System.Guid("cc5d5d2b-a4db-4c1e-aef0-026f0837c159");
            this.runRecordPanel.Location = new System.Drawing.Point(4, 23);
            this.runRecordPanel.Name = "runRecordPanel";
            this.runRecordPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.runRecordPanel.Size = new System.Drawing.Size(805, 146);
            this.runRecordPanel.Text = "运行日志";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Location = new System.Drawing.Point(0, 0);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(805, 146);
            this.controlContainer1.TabIndex = 0;
            // 
            // automaticArtPanel
            // 
            this.automaticArtPanel.Controls.Add(this.controlContainer2);
            this.automaticArtPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.automaticArtPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.automaticArtPanel.ID = new System.Guid("29a2ae67-b5ae-45f7-a2b4-0622f123f725");
            this.automaticArtPanel.Location = new System.Drawing.Point(4, 23);
            this.automaticArtPanel.Name = "automaticArtPanel";
            this.automaticArtPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.automaticArtPanel.Size = new System.Drawing.Size(805, 146);
            this.automaticArtPanel.Text = "自动挂接";
            // 
            // controlContainer2
            // 
            this.controlContainer2.Location = new System.Drawing.Point(0, 0);
            this.controlContainer2.Name = "controlContainer2";
            this.controlContainer2.Size = new System.Drawing.Size(805, 146);
            this.controlContainer2.TabIndex = 0;
            // 
            // anaEWarDkPanel
            // 
            this.anaEWarDkPanel.Appearance.ForeColor = System.Drawing.Color.Red;
            this.anaEWarDkPanel.Appearance.Options.UseBackColor = true;
            this.anaEWarDkPanel.Appearance.Options.UseBorderColor = true;
            this.anaEWarDkPanel.Appearance.Options.UseForeColor = true;
            this.anaEWarDkPanel.Controls.Add(this.anaEWar_Container);
            this.anaEWarDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.anaEWarDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.anaEWarDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.anaEWarDkPanel.ForeColor = System.Drawing.Color.Red;
            this.anaEWarDkPanel.ID = new System.Guid("5e42e794-e1ed-4c09-83b0-9bf298bfba28");
            this.anaEWarDkPanel.Location = new System.Drawing.Point(4, 23);
            this.anaEWarDkPanel.Name = "anaEWarDkPanel";
            this.anaEWarDkPanel.Options.AllowDockTop = false;
            this.anaEWarDkPanel.Options.ShowCloseButton = false;
            this.anaEWarDkPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.anaEWarDkPanel.Size = new System.Drawing.Size(805, 146);
            this.anaEWarDkPanel.Text = "模拟量预警";
            this.anaEWarDkPanel.Click += new System.EventHandler(this.anaEWarDkPanel_Click);
            // 
            // anaEWar_Container
            // 
            this.anaEWar_Container.Location = new System.Drawing.Point(0, 0);
            this.anaEWar_Container.Name = "anaEWar_Container";
            this.anaEWar_Container.Size = new System.Drawing.Size(805, 146);
            this.anaEWar_Container.TabIndex = 0;
            // 
            // anaWCPowDkPanel
            // 
            this.anaWCPowDkPanel.Appearance.Options.UseForeColor = true;
            this.anaWCPowDkPanel.Appearance.Options.UseTextOptions = true;
            this.anaWCPowDkPanel.Controls.Add(this.anaWCPow_Container);
            this.anaWCPowDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.anaWCPowDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.anaWCPowDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.anaWCPowDkPanel.ForeColor = System.Drawing.Color.Red;
            this.anaWCPowDkPanel.ID = new System.Guid("52b87e95-c789-482f-bf2f-5e42355bc51d");
            this.anaWCPowDkPanel.Location = new System.Drawing.Point(4, 23);
            this.anaWCPowDkPanel.Name = "anaWCPowDkPanel";
            this.anaWCPowDkPanel.Options.AllowDockTop = false;
            this.anaWCPowDkPanel.Options.ShowCloseButton = false;
            this.anaWCPowDkPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.anaWCPowDkPanel.Size = new System.Drawing.Size(805, 146);
            this.anaWCPowDkPanel.Text = "模拟量报警";
            this.anaWCPowDkPanel.Click += new System.EventHandler(this.anaWCPowDkPanel_Click);
            // 
            // anaWCPow_Container
            // 
            this.anaWCPow_Container.Location = new System.Drawing.Point(0, 0);
            this.anaWCPow_Container.Name = "anaWCPow_Container";
            this.anaWCPow_Container.Size = new System.Drawing.Size(805, 146);
            this.anaWCPow_Container.TabIndex = 0;
            // 
            // anaDDPanel
            // 
            this.anaDDPanel.Controls.Add(this.anaDD_Container);
            this.anaDDPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.anaDDPanel.FloatVertical = true;
            this.anaDDPanel.ID = new System.Guid("5f0a58ea-b1d6-406e-91e1-e1f671049680");
            this.anaDDPanel.Location = new System.Drawing.Point(4, 23);
            this.anaDDPanel.Name = "anaDDPanel";
            this.anaDDPanel.OriginalSize = new System.Drawing.Size(200, 200);
            this.anaDDPanel.Size = new System.Drawing.Size(805, 146);
            this.anaDDPanel.Text = "模拟量断电";
            // 
            // anaDD_Container
            // 
            this.anaDD_Container.Location = new System.Drawing.Point(0, 0);
            this.anaDD_Container.Name = "anaDD_Container";
            this.anaDD_Container.Size = new System.Drawing.Size(805, 146);
            this.anaDD_Container.TabIndex = 0;
            // 
            // switchWCPowDkPanel
            // 
            this.switchWCPowDkPanel.Controls.Add(this.switchWCPow_Container);
            this.switchWCPowDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.switchWCPowDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.switchWCPowDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.switchWCPowDkPanel.ForeColor = System.Drawing.Color.Red;
            this.switchWCPowDkPanel.ID = new System.Guid("05f9b493-34c3-48ae-9d08-f336ad6508e3");
            this.switchWCPowDkPanel.Location = new System.Drawing.Point(4, 23);
            this.switchWCPowDkPanel.Name = "switchWCPowDkPanel";
            this.switchWCPowDkPanel.Options.AllowDockTop = false;
            this.switchWCPowDkPanel.Options.ShowCloseButton = false;
            this.switchWCPowDkPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.switchWCPowDkPanel.Size = new System.Drawing.Size(805, 146);
            this.switchWCPowDkPanel.Text = "开关量报警";
            this.switchWCPowDkPanel.Click += new System.EventHandler(this.switchWCPowDkPanel_Click);
            // 
            // switchWCPow_Container
            // 
            this.switchWCPow_Container.Location = new System.Drawing.Point(0, 0);
            this.switchWCPow_Container.Name = "switchWCPow_Container";
            this.switchWCPow_Container.Size = new System.Drawing.Size(805, 146);
            this.switchWCPow_Container.TabIndex = 0;
            // 
            // fpowExeDkPanel
            // 
            this.fpowExeDkPanel.Controls.Add(this.fpowExe_Container);
            this.fpowExeDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.fpowExeDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.fpowExeDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.fpowExeDkPanel.ID = new System.Guid("8f4c210f-f0bc-4d80-ae7c-62c3bceb1832");
            this.fpowExeDkPanel.Location = new System.Drawing.Point(4, 23);
            this.fpowExeDkPanel.Name = "fpowExeDkPanel";
            this.fpowExeDkPanel.Options.AllowDockTop = false;
            this.fpowExeDkPanel.Options.ShowCloseButton = false;
            this.fpowExeDkPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.fpowExeDkPanel.Size = new System.Drawing.Size(805, 146);
            this.fpowExeDkPanel.Text = "馈电异常";
            this.fpowExeDkPanel.Click += new System.EventHandler(this.fpowExeDkPanel_Click);
            // 
            // fpowExe_Container
            // 
            this.fpowExe_Container.Location = new System.Drawing.Point(0, 0);
            this.fpowExe_Container.Name = "fpowExe_Container";
            this.fpowExe_Container.Size = new System.Drawing.Size(805, 146);
            this.fpowExe_Container.TabIndex = 0;
            // 
            // realControlDkPanel
            // 
            this.realControlDkPanel.Controls.Add(this.realControl_Container);
            this.realControlDkPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.realControlDkPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.realControlDkPanel.FloatVertical = true;
            this.realControlDkPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.realControlDkPanel.ID = new System.Guid("00cd9cc6-6ccf-40f0-a7bd-27b2db36ee97");
            this.realControlDkPanel.Location = new System.Drawing.Point(4, 23);
            this.realControlDkPanel.Name = "realControlDkPanel";
            this.realControlDkPanel.Options.AllowDockTop = false;
            this.realControlDkPanel.Options.ShowCloseButton = false;
            this.realControlDkPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.realControlDkPanel.Size = new System.Drawing.Size(805, 146);
            this.realControlDkPanel.Text = "实时控制";
            this.realControlDkPanel.Click += new System.EventHandler(this.realControlDkPanel_Click);
            // 
            // realControl_Container
            // 
            this.realControl_Container.Location = new System.Drawing.Point(0, 0);
            this.realControl_Container.Name = "realControl_Container";
            this.realControl_Container.Size = new System.Drawing.Size(805, 146);
            this.realControl_Container.TabIndex = 0;
            // 
            // handControlDockPanel
            // 
            this.handControlDockPanel.Appearance.BackColor2 = System.Drawing.Color.Red;
            this.handControlDockPanel.Appearance.BorderColor = System.Drawing.Color.Red;
            this.handControlDockPanel.Appearance.ForeColor = System.Drawing.Color.Red;
            this.handControlDockPanel.Appearance.Options.UseBorderColor = true;
            this.handControlDockPanel.Appearance.Options.UseForeColor = true;
            this.handControlDockPanel.Controls.Add(this.dockPanel1_Container);
            this.handControlDockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.handControlDockPanel.FloatSize = new System.Drawing.Size(900, 199);
            this.handControlDockPanel.FloatVertical = true;
            this.handControlDockPanel.Font = new System.Drawing.Font("宋体", 9.75F);
            this.handControlDockPanel.ID = new System.Guid("0834f6a3-7322-43e0-8fbe-9242fe96a203");
            this.handControlDockPanel.Location = new System.Drawing.Point(4, 23);
            this.handControlDockPanel.Name = "handControlDockPanel";
            this.handControlDockPanel.Options.AllowDockTop = false;
            this.handControlDockPanel.Options.ShowCloseButton = false;
            this.handControlDockPanel.OriginalSize = new System.Drawing.Size(722, 146);
            this.handControlDockPanel.Size = new System.Drawing.Size(805, 146);
            this.handControlDockPanel.Text = "中心站控制";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.BackColor = System.Drawing.Color.Transparent;
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(805, 146);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // mworkPControl
            // 
            this.mworkPControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mworkPControl.Location = new System.Drawing.Point(260, 0);
            this.mworkPControl.Name = "mworkPControl";
            this.mworkPControl.Size = new System.Drawing.Size(813, 343);
            this.mworkPControl.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // WorkMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 544);
            this.Controls.Add(this.mworkPControl);
            this.Controls.Add(this.panelContainer1);
            this.Controls.Add(this.dataNagDkPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkMainForm";
            this.Text = "实时显示主界面";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorkMainForm_FormClosed);
            this.Load += new System.EventHandler(this.WorkMainForm_Load);
            this.Resize += new System.EventHandler(this.WorkMainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.switchStateDkPanel.ResumeLayout(false);
            this.dataNagDkPanel.ResumeLayout(false);
            this.panelContainer1.ResumeLayout(false);
            this.runRecordPanel.ResumeLayout(false);
            this.automaticArtPanel.ResumeLayout(false);
            this.anaEWarDkPanel.ResumeLayout(false);
            this.anaWCPowDkPanel.ResumeLayout(false);
            this.anaDDPanel.ResumeLayout(false);
            this.switchWCPowDkPanel.ResumeLayout(false);
            this.fpowExeDkPanel.ResumeLayout(false);
            this.realControlDkPanel.ResumeLayout(false);
            this.handControlDockPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mworkPControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraEditors.PanelControl mworkPControl;
        private DevExpress.XtraBars.Docking.DockPanel anaEWarDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer anaEWar_Container;
        private DevExpress.XtraBars.Docking.DockPanel dataNagDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dataNag_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
        private DevExpress.XtraBars.Docking.DockPanel anaWCPowDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer anaWCPow_Container;
        private DevExpress.XtraBars.Docking.DockPanel switchWCPowDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer switchWCPow_Container;
        private DevExpress.XtraBars.Docking.DockPanel fpowExeDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer fpowExe_Container;
        private DevExpress.XtraBars.Docking.DockPanel realControlDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer realControl_Container;
        private DevExpress.XtraBars.Docking.DockPanel switchStateDkPanel;
        private DevExpress.XtraBars.Docking.ControlContainer switchState_Container;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.Docking.DockPanel handControlDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockPanel runRecordPanel;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.Docking.DockPanel automaticArtPanel;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
        private DevExpress.XtraBars.Docking.DockPanel anaDDPanel;
        private DevExpress.XtraBars.Docking.ControlContainer anaDD_Container;
    }
}