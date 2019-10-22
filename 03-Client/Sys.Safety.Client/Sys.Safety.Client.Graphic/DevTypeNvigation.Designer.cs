namespace Sys.Safety.Client.Graphic
{
    partial class DevTypeNvigation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevTypeNvigation));
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ok0.png");
            this.imageList1.Images.SetKeyName(1, "ok.png");
            // 
            // treeList1
            // 
            this.treeList1.AllowDrop = true;
            this.treeList1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Never;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeList1.Name = "treeList1";
            this.treeList1.BeginUnboundLoad();
            this.treeList1.AppendNode(new object[] {
            "设备类型"}, -1, 1, -1, 1);
            this.treeList1.AppendNode(new object[] {
            "分站"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "模拟量"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "开关量"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "控制量"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "累计量"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "导出量"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "识别器"}, 0, 0, 0, 1);
            this.treeList1.AppendNode(new object[] {
            "其它"}, 0);
            this.treeList1.EndUnboundLoad();
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsSelection.MultiSelect = true;
            this.treeList1.OptionsView.ShowColumns = false;
            this.treeList1.OptionsView.ShowHorzLines = false;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.OptionsView.ShowVertLines = false;
            this.treeList1.Padding = new System.Windows.Forms.Padding(4, 3, 3, 3);
            this.treeList1.Size = new System.Drawing.Size(268, 778);
            this.treeList1.StateImageList = this.imageList1;
            this.treeList1.TabIndex = 2;
            this.treeList1.VertScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;
            this.treeList1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.FieldName = "treeListColumn1";
            this.treeListColumn1.MinWidth = 77;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 100;
            // 
            // DevTypeNvigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.treeList1);
            this.Name = "DevTypeNvigation";
            this.Size = new System.Drawing.Size(268, 778);
            this.Load += new System.EventHandler(this.DevTpeNvigation_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }
}
