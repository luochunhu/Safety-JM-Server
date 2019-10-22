using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Chart
{
    public partial class Kgl_StateChg : XtraForm
    {
        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid = "0";

        /// <summary>
        ///     当前测点ID
        /// </summary>
        private string CurrentPointID = "";

        /// <summary>
        ///     当前位置ID
        /// </summary>
        private string CurrentWzid = "0";

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList = new List<string>();

        private bool IsInIframe = false;

        /// <summary>
        ///     是否计算分站中断，系统退出
        /// </summary>
        private bool kglztjsfs = true;

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        public Kgl_StateChg()
        {
            InitializeComponent();
        }

        public Kgl_StateChg(Dictionary<string, string> param)
        {
            InitializeComponent();
        }
        public void Reload(Dictionary<string, string> param)
        {
            object sender1 = null;
            var e1 = new EventArgs();
            Kgl_StateChg_Load(sender1, e1);
        }

        /// <summary>
        ///     柱状图XY坐标
        /// </summary>
        private XYDiagram Diagram
        {
            get { return chartControl1.Diagram as XYDiagram; }
        }

        /// <summary>
        ///     柱状图X坐标
        /// </summary>
        private AxisBase AxisX
        {
            get { return Diagram != null ? Diagram.AxisX : null; }
        }

        /// <summary>
        ///     柱状图开机率
        /// </summary>
        private Series Series1
        {
            get { return chartControl1.Series[0]; }
        }

        /// <summary>
        ///     柱状图停机率
        /// </summary>
        private Series Series2
        {
            get { return chartControl1.Series[1]; }
        }

        /// <summary>
        ///     加载明细列表
        /// </summary>
        private void LoadDetailList(DataTable dt)
        {
            try
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                    if ((dt.Rows[i]["type"].ToString() == "27") || (dt.Rows[i]["type"].ToString() == "26") ||
                        (dt.Rows[i]["type"].ToString() == "25"))
                    {
                        dt.Rows[i]["val"] = int.Parse(dt.Rows[i]["type"].ToString()) - 25 + "态(" + dt.Rows[i]["val"] +
                                            ")";
                    }
                    else if ((dt.Rows[i]["type"].ToString() == "46"))
                    {
                        dt.Rows[i]["val"] = "未知";
                    }
                gridControl2.DataSource = dt;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_LoadDetailList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载状态趋势柱形图
        /// </summary>
        private void LoadStateBar(DataTable dt)
        {
            LoadBar(chart.Series[0], "2态", dt, "27");
            LoadBar(chart.Series[1], "1态", dt, "26");
            LoadBar(chart.Series[2], "0态", dt, "25");
            LoadBar(chart.Series[3], "其它", dt, "10");
        }

        private void LoadBar(Series series, string Argument, DataTable dt, string state)
        {
            try
            {
                series.Points.BeginUpdate();
                series.Points.Clear();
                var stateName = "";
                var drs = dt.Select(" state='" + state + "'");
                var pointDev = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentPointID);
                if (state == "27")
                    stateName = pointDev[2];
                else if (state == "26")
                    stateName = pointDev[1];
                else if (state == "25")
                    stateName = pointDev[0];
                else
                    stateName = "未知";
                series.Name = Argument + "(" + stateName + ")";
                if (drs.Length > 0)
                    foreach (var dr in drs)
                    {
                        var timenow = new DateTime[2];
                        timenow[0] = DateTime.Parse(dr["stime"].ToString());
                        timenow[1] = DateTime.Parse(dr["etime"].ToString());
                        series.Points.Add(new SeriesPoint(Argument, timenow));
                    }
                series.Points.EndUpdate();
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_LoadBar" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载状态统计信息表格
        /// </summary>
        private void LoadDataTable(DataTable dt)
        {
            gridControl1.DataSource = dt;
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries(Series series, string name, string shortName, DataTable dt)
        {
            LoadPoints(series, shortName, dt);
            //series.CrosshairLabelPattern = name + " : {V:F2}" + "%";
            series.Name = name;
        }

        /// <summary>
        ///     加载曲线测点的值
        /// </summary>
        /// <param name="series"></param>
        /// <param name="CodeLen"></param>
        private void LoadPoints(Series series, string shortName, DataTable dt)
        {
            try
            {
                if (series != null)
                {
                    series.Points.BeginUpdate();
                    series.Points.Clear();

                    //重新定义数据             

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rate = double.Parse(dt.Rows[i][shortName].ToString(), CultureInfo.InvariantCulture);
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["timer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kgl_StateChg_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;
                layoutControlItem3.Width = 220;

                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                chart.Series[0].Points.Clear();
                chart.Series[1].Points.Clear();
                chart.Series[2].Points.Clear();
                chart.Series[3].Points.Clear();
                Series1.Points.Clear();
                Series2.Points.Clear();
                gridControl1.DataSource = null;
                gridControl2.DataSource = null;

                LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime,true);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_Kgl_StateChg_Load" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     状态统计列表样式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    var category = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Columns2"]);
                    if (category.Contains("点"))
                    {
                        e.Appearance.BackColor = Color.Silver;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_gridView1_RowStyle" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     状态明细列表样式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (e.Column.FieldName == "val")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["val"]);
                    if (aa.Contains("2"))
                        e.Appearance.ForeColor = Color.Green;
                    else if (aa.Contains("1"))
                        e.Appearance.ForeColor = Color.Red;
                    else if (aa.Contains("0"))
                        e.Appearance.ForeColor = Color.Black;
                    else
                        e.Appearance.ForeColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_gridView2_RowCellStyle" + ex.Message +
                                                        ex.StackTrace);
            }
        }

        /// <summary>
        ///     选择日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime,false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_dateEdit1_EditValueChanged" + ex.Message +
                                                        ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList(DateTime stime, DateTime etime,bool loadData)
        {
            try
            {
                var LoadPointStr = new List<string>();
                comboBoxEdit1.Properties.Items.Clear();
                if (radioGroup1.SelectedIndex == 0)//按已定义测点查询
                {
                    LoadPointStr = InterfaceClass.queryConditions_.GetActivePointList(2, ref PointIDList,
                      ref DevList, ref WzList);
                }
                else//按已存储测点查询
                {
                    LoadPointStr = InterfaceClass.queryConditions_.GetPointList(stime, etime, 2, ref PointIDList,
                        ref DevList, ref WzList);
                }
                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit1.Properties.Items.Add(PointStr);
                if (comboBoxEdit1.Properties.Items.Count > 0)
                {
                    comboBoxEdit1.SelectedIndex = 0;
                    comboBoxEdit1.Enabled = true;
                }
                else
                {
                    comboBoxEdit1.Enabled = false;
                    comboBoxEdit1.Text = "没有数据";
                }

                //加载曲线
                if (!string.IsNullOrEmpty(CurrentPointID))
                {
                    var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                    {
                        if (PointIDList[i].Contains(CurrentPointID))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            isinlist = true;
                            break;
                        }
                    }
                    if (loadData)
                    {
                        if (isinlist)
                        {
                            //调用查询
                            object sender1 = null;
                            var e1 = new EventArgs();
                            simpleButton1_Click(sender1, e1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     查询开关量状态变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                var SzNameS = dateEdit1.DateTime;

                if (comboBoxEdit1.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点！");
                    return;
                }

                double MaxLC2 = 0;
                var TjTxt = "";
                CurrentPointID = PointIDList[comboBoxEdit1.SelectedIndex];
                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];

                if (checkEdit1.Checked)
                    kglztjsfs = true;
                else
                    kglztjsfs = false;
                var dt_line = new List<DataTable>();
                InterfaceClass.KglStateChgQueryClass_.getStateBarTable(SzNameS, CurrentPointID,
                    CurrentDevid, CurrentWzid, kglztjsfs, ref TjTxt, ref dt_line);
                if (dt_line.Count < 1)
                    return;
                labelControl1.Text = TjTxt;
                var dtBarStateChg = dt_line[0];
                var dtTotal = dt_line[1];
                var dtBarHourTj = dt_line[2];
                //修改，没有数据时，进行处理并清空曲线  20180320
                //if (dtBarStateChg.Rows.Count > 0)
                LoadStateBar(dtBarStateChg);
                //if (dtTotal.Rows.Count > 0)
                LoadDataTable(dtTotal);
                if (dtBarHourTj.Rows.Count > 0)
                {
                    LoadSeries(Series1, dtBarHourTj.Rows[0]["percentage1Name"] + "百分比", "percentage1", dtBarHourTj);
                    LoadSeries(Series2, dtBarHourTj.Rows[0]["percentage2Name"] + "百分比", "percentage2", dtBarHourTj);
                }
                else
                {
                    Series1.Points.Clear();
                    Series2.Points.Clear();
                }
                var StateChgdt = InterfaceClass.KglStateChgQueryClass_.getStateChgdt(SzNameS, CurrentPointID,
                    CurrentDevid, CurrentWzid, kglztjsfs);
                LoadDetailList(StateChgdt);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     打印状态变化趋势图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (chart != null)
                    ChartPrint.chartPrint(chart, (float)(Width * 0.56));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_simpleButton4_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出状态变化趋势图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "开关量状态变化趋势图.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(chart, saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     打印开停机率柱状图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (chartControl1 != null)
                    ChartPrint.chartPrint(chartControl1, (float)(Width * 0.75));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_simpleButton3_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出开停机率柱状图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "开关量开停机率柱状图.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(chartControl1, saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_simpleButton5_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        private void ExportToCore(ChartControl chart, string filename, string ext)
        {
            try
            {
                if (chart != null)
                {
                    var currentCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    if (ext == "rtf")
                        chart.ExportToRtf(filename);
                    else if (ext == "pdf")
                        chart.ExportToPdf(filename);
                    else if (ext == "mht")
                        chart.ExportToMht(filename);
                    else if (ext == "html")
                        chart.ExportToHtml(filename);
                    else if (ext == "xls")
                        chart.ExportToXls(filename);
                    else if (ext == "xlsx")
                        chart.ExportToXlsx(filename);
                    else
                    {
                        ImageFormat currentImageFormat = null;
                        if (ext.ToLower().Contains("bmp"))
                            currentImageFormat = ImageFormat.Bmp;
                        else if (ext.ToLower().Contains("jpg"))
                            currentImageFormat = ImageFormat.Jpeg;
                        else if (ext.ToLower().Contains("png"))
                            currentImageFormat = ImageFormat.Png;
                        else if (ext.ToLower().Contains("gif"))
                            currentImageFormat = ImageFormat.Gif;
                        chart.ExportToImage(filename, currentImageFormat);
                    }
                    Cursor.Current = currentCursor;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_ExportToCore" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 已定义，已存储功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime,false);
        }

        #region ----查询前一天、后一天记录 2017.7.30 by----

        private void btn_pre_Click(object sender, EventArgs e)
        {
            checkOhterTime(-1);
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            checkOhterTime(1);
        }

        private void checkOhterTime(int days)
        {
            dateEdit1.DateTime = dateEdit1.DateTime.AddDays(days);
            simpleButton1_Click(new object(), new EventArgs());
        }

        #endregion
    }
}