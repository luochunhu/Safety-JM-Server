using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid.Columns;
using Basic.Framework.Logging;
using DevExpress.XtraEditors;
using System.Threading;
using DevExpress.XtraPrinting;

namespace Sys.Safety.Client.Display
{
    public partial class KzDefineMsg : XtraForm
    {
        public KzDefineMsg()
        {
            InitializeComponent();
        }

        private DataTable showdt;

        /// <summary>
        /// 列表显示名称
        /// </summary>
        private string[] colname = new string[] { "主控测点", "类型", "安装位置", "控制测点", "控制区域", "馈电测点", "馈电位置", "控制类型", "上限断电值", "上限复电值", "当前控制", "实时值" };

        //private string[] colname = new string[] { "主控测点", "类型", "安装位置", "控制测点", "控制区域", "控制类型", "上限断电值", "上限复电值", "当前控制", "实时值" };
        /// <summary>
        /// 列表显示名称
        /// </summary>
        private string[] colnamekz = new string[] { "point", "type", "wz", "kzd", "qy", "kdpoint", "kdwz", "kztype", "sd", "sf", "kzlx", "ssz" };
        private int[] colwidth = new int[] { 80, 120, 200, 80, 200, 80, 200, 180, 80, 80, 80, 80 };

        //private string[] colnamekz = new string[] { "point","type","wz","kzd","qy","kztype","sd","sf","kzlx","ssz"};
        private Thread freshthread;
        private bool _isRun = false;

        /// <summary>
        /// 初始显示表
        /// </summary>
        private void inigrid()
        {
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            // 
            // repositoryItemMemoEdit1
            // 
            repositoryItemMemoEdit1.Appearance.Options.UseTextOptions = true;
            repositoryItemMemoEdit1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";

            GridColumn col;
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = colnamekz[i];
                col.Width = colwidth[i];
                col.Tag = i;
                col.Visible = true;
                col.ColumnEdit = repositoryItemMemoEdit1;
                //col.OptionsFilter.AllowFilter = false;
                //col.OptionsFilter.AllowAutoFilter = false;
                //col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;
        }

        private void realshow(DataTable dt)
        {
            int x = -1, y = -1, count = 0, toprowindex = 0;

            toprowindex = mainGridView.TopRowIndex;
            if (mainGridView.FocusedColumn != null)
            {
                x = mainGridView.FocusedColumn.ColumnHandle;
                y = mainGridView.FocusedRowHandle;
            }
            count = mainGridView.RowCount;

            if (dt != null)
            {
                showdt = dt;
            }
            else
            {
                showdt.Rows.Clear();
            }
            mainGrid.DataSource = showdt;

            if (showdt.Rows.Count == count)
            {
                mainGridView.FocusedColumn.ColumnHandle = x;
                mainGridView.FocusedRowHandle = y;
                if (x > -1 && y > -1)
                {
                    mainGridView.FocusedColumn.ColumnHandle = x;
                    mainGridView.FocusedRowHandle = y;
                }
                mainGridView.TopRowIndex = toprowindex;
            }
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Enabled = false;
        //    try
        //    {
        //        realshow();
        //    }
        //    catch (Exception ex)
        //    {
        //        Basic.Framework.Logging.LogHelper.Error(ex);
        //    }
        //    timer1.Enabled = true;
        //}
        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    DataTable dt = Model.RealInterfaceFuction.Getzkpointex();
                    MethodInvoker In = new MethodInvoker(() => realshow(dt));
                    this.BeginInvoke(In);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        private void KzDefineMsg_Load(object sender, EventArgs e)
        {
            try
            {
                inigrid();
                DataTable dt = Model.RealInterfaceFuction.Getzkpointex();
                realshow(dt);

                _isRun = true;
                freshthread = new Thread(new ThreadStart(fthread));
                freshthread.IsBackground = true;
                freshthread.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string point = "";
            DataRow[] rows;
            try
            {
                if (e.Column.Tag.ToString() == "7")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                if (e.Column.Tag.ToString() == "8")
                {
                    point = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns[0]).ToString();
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                        if (rows.Length > 0)
                        {
                            int tempColor = 0;
                            int.TryParse(rows[0]["sszcolor"].ToString(), out tempColor);
                            e.Appearance.ForeColor = Color.FromArgb(tempColor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void KzDefineMsg_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件t(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                mainGrid.ExportToXls(fileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = this.mainGrid;
            link.Landscape = true;
            link.PaperKind = System.Drawing.Printing.PaperKind.A3;
            link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            link.CreateDocument();
            link.ShowPreview();
        }
        private void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string title = string.Format("断电关系查询", DateTime.Now.ToString("yyyy-MM-dd"));
            PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.None, title, Color.Black,
               new RectangleF(0, 0, 100, 35), BorderSide.None);

            brick.LineAlignment = BrickAlignment.Center;
            brick.Alignment = BrickAlignment.Center;
            brick.AutoWidth = true;
            brick.Font = new System.Drawing.Font("宋体", 12f, FontStyle.Regular);
        }
    }
}
