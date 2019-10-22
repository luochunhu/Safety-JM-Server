using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid.Columns;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Display
{
    public partial class frmFaultInfo : XtraForm
    {
        private DataTable showdt;
        private DataTable alldata;
        private static object _objLock = new object();


        public frmFaultInfo()
        {
            InitializeComponent();
            Init();
        }

        #region======================故障实时数据======================

        /// <summary>
        /// 预警数据源
        /// </summary>
        public Dictionary<long, Jc_BInfo> jc_b = new Dictionary<long, Jc_BInfo>();

        /// <summary>
        /// 需要删除的id号
        /// </summary>
        public List<long> deletelist = new List<long>();

        /// <summary>
        /// 需要添加的id号
        /// </summary>
        public List<long> addlist = new List<long>();

        /// <summary>
        /// 需要修改的id号
        /// </summary>
        public List<long> updatelist = new List<long>();

        private Thread freshthread;
        private bool _isRun = false;
       
        #endregion


        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","故障来源(测点/分站)","分站/通道/地址",
            "故障起始时间","持续时间","故障类型","措施","endtime","id"};

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","ly",
            "td","stime","cxtime","state","cs",
            "endtime","id" };

        public int[] colwith = new int[] { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };

        /// <summary>
        /// 初始显示表
        /// </summary>
        public void inigrid()
        {
            GridColumn col;
            Init();
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = tcolname[i];
                col.Width = colwith[i];
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                if (colname[i] == "id" || colname[i] == "endtime" || colname[i] == "措施")
                {
                    col.Visible = false;
                }
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;
        }

        private void Init()
        {
            showdt = new DataTable();
            showdt.Columns.Add("point");
            showdt.Columns.Add("wz");
            showdt.Columns.Add("ly");
            showdt.Columns.Add("td");
            showdt.Columns.Add("stime");
            showdt.Columns.Add("cxtime");
            showdt.Columns.Add("state");
            showdt.Columns.Add("cs");
            showdt.Columns.Add("endtime");
            showdt.Columns.Add("id", typeof(long));
        }

        private void frmFaultInfo_Load(object sender, EventArgs e)
        {
            try
            {
                inigrid();
                DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                getdt(nowtime);
                refresh1(nowtime);
                //getdata();
                //refresh();
                _isRun = true;
                freshthread = new Thread(new ThreadStart(fthread));
                freshthread.IsBackground = true;
                freshthread.Start();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            
        }

        private int interval = 0;
        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    interval++;
                    DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                    if (interval > 3)
                    {
                        interval = 0;
                        MethodInvoker In = new MethodInvoker(() => getdt(nowtime));
                        this.BeginInvoke(In);
                    }
                    else
                    {
                        MethodInvoker In = new MethodInvoker(() => refresh1(nowtime));
                        this.BeginInvoke(In);
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 获取所有分站号
        /// </summary>
        /// <returns></returns>
        private List<int> getfzh()
        {
            DataRow[] rows = null;
            List<int> fzh = new List<int>();
            if (StaticClass.AllPointDt.Rows.Count > 0)
            {
                lock (StaticClass.allPointDtLockObj)
                {
                    //rows = StaticClass.AllPointDt.Select("lx='分站'", "fzh");
                    rows = StaticClass.AllPointDt.Select("lx='分站' AND fzh<256", "fzh");//只显示监控故障信息 edit by 
                    if (rows.Length > 0)
                    {
                        foreach (DataRow r in rows)
                        {
                            if (r != null)
                            {
                                fzh.Add(int.Parse(r["fzh"].ToString()));
                            }
                        }
                    }
                }
            }
            return fzh;
        }

        private void getdt(DateTime nowtime)
        {
            DataRow[] rows = null;
            DataRow row = null;
            TimeSpan span;
            
            int zt = -1;
            DataTable dt = showdt.Clone();
            List<int> fzh;
            int x = -1, y = -1, count = 0, toprowindex = 0;
            try
            {
                fzh = getfzh();
                if (fzh.Count > 0)
                {
                    foreach (int fz in fzh)
                    {
                        lock (StaticClass.allPointDtLockObj)
                        {
                            rows = StaticClass.AllPointDt.Select("fzh=" + fz + " and lx='分站'");
                            if (rows.Length > 0)
                            {

                                zt = int.Parse(rows[0]["zt"].ToString());
                                if (zt == StaticClass.itemStateToClient.EqpState2 || zt == StaticClass.itemStateToClient.EqpState5)
                                {
                                    #region 分站通讯故障
                                    row = dt.NewRow();
                                    row["point"] = rows[0]["point"];
                                    row["wz"] = rows[0]["wz"];
                                    row["td"] = rows[0]["fzh"] + "/" + rows[0]["tdh"] + "/" + rows[0]["dzh"];
                                    row["ly"] = "分站";
                                    row["state"] = OprFuction.StateChange(zt.ToString());
                                    row["stime"] = rows[0]["time"].ToString();
                                    span = nowtime - DateTime.Parse(rows[0]["time"].ToString());
                                    row["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                    row["cs"] = "无";
                                    row["endtime"] = "";
                                    row["id"] = 0;
                                    dt.Rows.Add(row);
                                    #endregion
                                }
                                else
                                {
                                    #region  模拟量、开关量 测点故障
                                    rows = StaticClass.AllPointDt.Select("fzh='" + fz + @"' and (lx='模拟量' or lx='开关量') and (zt=20 
                                   or zt=22 or zt=23 or zt=25 or zt=33 )", "fzh,tdh,dzh");
                                    if (rows.Length > 0)
                                    {
                                        foreach (DataRow r in rows)
                                        {
                                            if (r == null)
                                            {
                                                continue;
                                            }
                                            zt = int.Parse(r["zt"].ToString());
                                            //if (zt == StaticClass.itemStateToClient.EqpState13 
                                            //    || zt == StaticClass.itemStateToClient.EqpState15
                                            //    ||zt ==StaticClass .itemStateToClient .EqpState16
                                            //    ||zt ==StaticClass .itemStateToClient .EqpState24
                                            //    ||zt ==StaticClass .itemStateToClient .EqpState31
                                            //    )
                                            //{
                                            row = dt.NewRow();
                                            row["point"] = r["point"];
                                            row["wz"] = r["wz"];
                                            row["td"] = r["fzh"] + "/" + r["tdh"] + "/" + r["dzh"];
                                            row["ly"] = "测点";
                                            if (zt == StaticClass.itemStateToClient.EqpState24)
                                            {
                                                row["state"] = "断线";
                                            }
                                            else
                                            {
                                                row["state"] = OprFuction.StateChange(zt.ToString());
                                            }
                                            row["stime"] = r["time"].ToString();
                                            span = nowtime - DateTime.Parse(r["time"].ToString());
                                            row["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                            row["cs"] = "无";
                                            row["endtime"] = "";
                                            row["id"] = 0;
                                            dt.Rows.Add(row);
                                            //}

                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            if (mainGridView.FocusedColumn != null)
            {
                x = mainGridView.FocusedColumn.ColumnHandle;
                y = mainGridView.FocusedRowHandle;
            }
            count = mainGridView.RowCount;
            toprowindex = mainGridView.TopRowIndex;
            showdt = dt;
            mainGrid.DataSource = showdt;
            if (showdt.Rows.Count == count)
            {
                if (x > -1 && y > -1)
                {
                    mainGridView.FocusedColumn.ColumnHandle = x;
                    mainGridView.FocusedRowHandle = y;
                }
                mainGridView.TopRowIndex = toprowindex;
            }

            //ScrollViewer scrolViewer = FindVisualChildByName<ScrollViewer>(this.gridView, ControlName);
            //scrolViewer.ScrollToHorizontalOffset(scrollHorizontalOffset);

        }


        /// <summary>
        /// 刷新故障数据
        /// </summary>
        public void refresh1(DateTime nowtime)
        {
            
            TimeSpan span;
            #region 刷新 持续时间 cs
            try
            {
                for (int i = 0; i < showdt.Rows.Count; i++)
                {
                    #region 刷新持续时间
                    span = nowtime - Convert.ToDateTime(showdt.Rows[i]["stime"]);
                    showdt.Rows[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新故障时间", ex);
            }
            #endregion
        }


        #region 导出功能

        /// <summary>
        /// 点击导出按钮后弹出导出功能菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.DropDownControl.Show(this.barManager1, btnExport, new Point(0, btnExport.Height));
        }


        private void btnImportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridExportHelper.ExportToExcel(this.mainGridView);
        }

        private void btnImportHtml_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridExportHelper.ExportToHtml(this.mainGridView);
        }

        private void btnImportText_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridExportHelper.ExportToText(this.mainGridView);
        }

        private void btnImportXml_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridExportHelper.ExportToXml(this.mainGridView);
        }

        #endregion

        #region 打印
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreview(mainGrid);
        }

        //自定义打印
        private void PrintPreview(DevExpress.XtraPrinting.IPrintable gridControlPrint)
        {
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
            compositeLink.PrintingSystem = ps;
            compositeLink.Landscape = true;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            DevExpress.XtraPrinting.PrintableComponentLink link = new DevExpress.XtraPrinting.PrintableComponentLink(ps);

            ps.PageSettings.Landscape = true;
            link.Component = gridControlPrint;
            compositeLink.Links.Add(link);
            link.CreateDocument();  //建立文档
            ps.PreviewFormEx.Show();//进行预览  
        }

        #endregion

        /// <summary>
        /// 重绘Grid的行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFaultInfo_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 持续时间计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDuration_Tick(object sender, EventArgs e)
        {

        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption.ToString() == "故障类型")
            {
                e.Appearance.ForeColor = Color.Red;
            }
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void frmFaultInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }

    }


    /// <summary>
    /// Dev GridView导出帮助类
    /// </summary>
    public static class GridExportHelper
    {
        #region 打开文件
        /**/
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        private static void OpenFile(string fileName)
        {
            if (MessageBox.Show("是否想要打开这个文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageBox.Show("您的系统不能打开该类型的文件！.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region 把GridControl的数据输出
        /**/
        /// <summary>
        /// 把GridControl的数据输出
        /// </summary>
        /// <param name="provider">输出提供者</param>
        /// <param name="gridView1">DevExpress GridView</param>
        public static void ExportTo(IExportProvider provider, GridView gridView1)
        {
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            BaseExportLink link = gridView1.CreateExportLink(provider);
            (link as GridViewExportLink).ExpandAll = false;
            link.ExportTo(true);
            Cursor.Current = currentCursor;
        }
        /**/
        /// <summary>
        /// 把GridControl的数据输出输出成Html
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="gridView1"></param>
        public static void ExportToHtml(GridView gridView1)
        {
            string fileName = ShowSaveFileDialog("HTML 文档", "HTML 文档|*.html");
            if (fileName != "")
            {
                ExportTo(new ExportHtmlProvider(fileName), gridView1);
                OpenFile(fileName);
            }
        }
        /**/
        /// <summary>
        /// 把GridControl的数据输出输出成Xml
        /// </summary>
        /// <param name="gridView1"></param>
        public static void ExportToXml(GridView gridView1)
        {
            string fileName = ShowSaveFileDialog("Xml 文档", "Xml 文档|*.xml");
            if (fileName != "")
            {
                ExportTo(new ExportXmlProvider(fileName), gridView1);
                OpenFile(fileName);
            }
        }
        /**/
        /// <summary>
        /// 把GridControl的数据输出输出成Excel
        /// </summary>
        /// <param name="gridView1"></param>
        public static void ExportToExcel(GridView gridView1)
        {
            string fileName = ShowSaveFileDialog("Excel 文档", "Excel 文档|*.xls");
            if (fileName != "")
            {
                ExportTo(new ExportXlsProvider(fileName), gridView1);
                OpenFile(fileName);
            }
        }
        /**/
        /// <summary>
        /// 把GridControl的数据输出输出成Text文本
        /// </summary>
        /// <param name="gridView1"></param>
        public static void ExportToText(GridView gridView1)
        {
            string fileName = ShowSaveFileDialog("Text 文档", "Text 文档|*.txt");
            if (fileName != "")
            {
                ExportTo(new ExportTxtProvider(fileName), gridView1);
                OpenFile(fileName);
            }
        }
        #endregion

        #region 提示保存窗口
        /**/
        /// <summary>
        /// 提示保存窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            //string name = Application.ProductName;
            string name = "故障信息" + Model.RealInterfaceFuction.GetServerNowTime().ToString("yyMMddHHmmss");
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }
        #endregion

    }
}
