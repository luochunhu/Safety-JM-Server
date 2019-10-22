using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Chart
{
    public partial class Kgl_StateLineWidthScreen : XtraForm
    {
        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid1 = "0";

        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid2 = "0";

        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid3 = "0";

        /// <summary>
        ///     当前测点ID
        /// </summary>
        private string CurrentPointID1 = "";

        /// <summary>
        ///     当前测点ID
        /// </summary>
        private string CurrentPointID2 = "";

        /// <summary>
        ///     当前测点ID
        /// </summary>
        private string CurrentPointID3 = "";

        /// <summary>
        ///     当前位置ID
        /// </summary>
        private string CurrentWzid1 = "0";

        /// <summary>
        ///     当前位置ID
        /// </summary>
        private string CurrentWzid2 = "0";

        /// <summary>
        ///     当前位置ID
        /// </summary>
        private string CurrentWzid3 = "0";

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList = new List<string>();

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line1 = new DataTable();

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line2 = new DataTable();

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line3 = new DataTable();

        private bool IsInIframe = false;

        /// <summary>
        ///     是否计算未知状态
        /// </summary>
        private bool kglztjsfs = true;

        /// <summary>
        ///     测点加载列表
        /// </summary>
        private List<string> LoadPointStr = new List<string>();

        /// <summary>
        ///     当前测点号
        /// </summary>
        private string point1 = "";

        /// <summary>
        ///     当前测点号
        /// </summary>
        private string point2 = "";

        /// <summary>
        ///     当前测点号
        /// </summary>
        private string point3 = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        public Kgl_StateLineWidthScreen()
        {
            InitializeComponent();
        }

        public Kgl_StateLineWidthScreen(Dictionary<string, string> param)
        {
            InitializeComponent();
        }
        public void Reload(Dictionary<string, string> param)
        {
            object sender1 = null;
            var e1 = new EventArgs();
            Kgl_StateLineWidthScreen_Load(sender1, e1);
        }
       

        /// <summary>
        ///     坐标系
        /// </summary>
        private XYDiagram Diagram
        {
            get { return chart.Diagram as XYDiagram; }
        }

        /// <summary>
        ///     X坐标
        /// </summary>
        private AxisBase AxisX
        {
            get { return Diagram != null ? Diagram.AxisX : null; }
        }

        /// <summary>
        ///     Y坐标
        /// </summary>
        private AxisBase AxisY
        {
            get { return Diagram != null ? Diagram.AxisY : null; }
        }

        /// <summary>
        ///     监测值曲线
        /// </summary>
        private Series Series1
        {
            get { return chart.Series[0]; }
        }

        /// <summary>
        ///     监测值曲线
        /// </summary>
        private Series Series2
        {
            get { return chart.Series[1]; }
        }

        /// <summary>
        ///     监测值曲线
        /// </summary>
        private Series Series3
        {
            get { return chart.Series[2]; }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt, string name, Series series)
        {
            LoadSeries(series, name, "C", dt);
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries(Series series, string name, string shortName, DataTable dt)
        {
            var CodeLen = 3;
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
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["sTimer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     产生随机数(测试用)
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public string RandCode(int N)
        {
            char[] arrChar = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            var num = new StringBuilder();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < N; i++)
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            return num.ToString();
        }

        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kgl_StateLineWidthScreen_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width*0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height*0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width*0.1/2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height*0.1/2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;

                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                Series1.Points.Clear();
                Series2.Points.Clear();
                Series3.Points.Clear();
                

                #region//加载曲线颜色                 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_KglColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_KglColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series3, "Chart_KglColor");
                InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");

                #endregion

                LoadPointSelList1(dateEdit1.DateTime, dateEdit1.DateTime);
                LoadPointSelList2(dateEdit1.DateTime, dateEdit1.DateTime);
                LoadPointSelList3(dateEdit1.DateTime, dateEdit1.DateTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_Kgl_StateLineWidthScreen_Load" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     打印曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (chart != null)
                    ChartPrint.chartPrint(chart, (float)(Width * 0.8));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_simpleButton4_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        private void ExportToCore(string filename, string ext)
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
                LogHelper.Error("Kgl_StateLineWidthScreen_ExportToCore" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     查询
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
                    XtraMessageBox.Show("请选择测点1！");
                    return;
                }
                if (comboBoxEdit2.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点2！");
                    return;
                }
                if (comboBoxEdit3.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点3！");
                    return;
                }


                point1 = comboBoxEdit1.SelectedItem.ToString().Substring(0,
                    comboBoxEdit1.SelectedItem.ToString().IndexOf("."));
                CurrentPointID1 = PointIDList[comboBoxEdit1.SelectedIndex];
                CurrentDevid1 = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid1 = WzList[comboBoxEdit1.SelectedIndex];

                point2 = comboBoxEdit2.SelectedItem.ToString().Substring(0,
                    comboBoxEdit2.SelectedItem.ToString().IndexOf("."));
                CurrentPointID2 = PointIDList[comboBoxEdit2.SelectedIndex];
                CurrentDevid2 = DevList[comboBoxEdit2.SelectedIndex];
                CurrentWzid2 = WzList[comboBoxEdit2.SelectedIndex];

                point3 = comboBoxEdit3.SelectedItem.ToString().Substring(0,
                    comboBoxEdit3.SelectedItem.ToString().IndexOf("."));
                CurrentPointID3 = PointIDList[comboBoxEdit3.SelectedIndex];
                CurrentDevid3 = DevList[comboBoxEdit3.SelectedIndex];
                CurrentWzid3 = WzList[comboBoxEdit3.SelectedIndex];

                if (checkEdit1.Checked)
                    kglztjsfs = true;
                else
                    kglztjsfs = false;

                dt_line1 = InterfaceClass.KglStateLineQueryClass_.getStateLineDt(SzNameS, CurrentPointID1, CurrentDevid1,
                    CurrentWzid1, kglztjsfs);
                var pointDev1 = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentPointID1);
                var stateName1 = "2态:" + pointDev1[2] + "," + "1态:" + pointDev1[1] + "," + "0态:" + pointDev1[0];
                var name1 = comboBoxEdit1.SelectedItem + "(" + stateName1 + ")";
                var shortName1 = comboBoxEdit1.SelectedItem.ToString()
                    .Substring(0, comboBoxEdit1.SelectedItem.ToString().IndexOf("."));
                chart.Titles[0].Text = name1;
                InitControls(dt_line1, shortName1, Series1);

                dt_line2 = InterfaceClass.KglStateLineQueryClass_.getStateLineDt(SzNameS, CurrentPointID2, CurrentDevid2,
                    CurrentWzid2, kglztjsfs);
                var pointDev2 = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentPointID2);
                var stateName2 = "2态:" + pointDev2[2] + "," + "1态:" + pointDev2[1] + "," + "0态:" + pointDev2[0];
                var name2 = comboBoxEdit2.SelectedItem + "(" + stateName2 + ")";
                var shortName2 = comboBoxEdit2.SelectedItem.ToString()
                    .Substring(0, comboBoxEdit2.SelectedItem.ToString().IndexOf("."));
                chart.Titles[0].Text += "\n" + name2;
                InitControls(dt_line2, shortName2, Series2);

                dt_line3 = InterfaceClass.KglStateLineQueryClass_.getStateLineDt(SzNameS, CurrentPointID3, CurrentDevid3,
                    CurrentWzid3, kglztjsfs);
                var pointDev3 = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentPointID3);
                var stateName3 = "2态:" + pointDev3[2] + "," + "1态:" + pointDev3[1] + "," + "0态:" + pointDev3[0];
                var name3 = comboBoxEdit3.SelectedItem + "(" + stateName3 + ")";
                var shortName3 = comboBoxEdit3.SelectedItem.ToString()
                    .Substring(0, comboBoxEdit3.SelectedItem.ToString().IndexOf("."));
                chart.Titles[0].Text += "\n" + name3;
                InitControls(dt_line3, shortName3, Series3);

                var _minX = DateTime.Parse(SzNameS.ToShortDateString());
                var _maxX = DateTime.Parse(SzNameS.ToShortDateString() + " 23:59:59");

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     开始日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointSelList1(dateEdit1.DateTime, dateEdit1.DateTime);
                    LoadPointSelList2(dateEdit1.DateTime, dateEdit1.DateTime);
                    LoadPointSelList3(dateEdit1.DateTime, dateEdit1.DateTime);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList1(DateTime stime, DateTime etime)
        {
            try
            {
                LoadPointStr = new List<string>();
                comboBoxEdit1.Properties.Items.Clear();
                if (LoadPointStr.Count < 1)
                {
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
                if (!string.IsNullOrEmpty(CurrentPointID1))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID1))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_LoadPointSelList1" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList2(DateTime stime, DateTime etime)
        {
            try
            {
                comboBoxEdit2.Properties.Items.Clear();
                if (LoadPointStr.Count < 1)
                {
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
                }
                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit2.Properties.Items.Add(PointStr);
                if (comboBoxEdit2.Properties.Items.Count > 0)
                {
                    comboBoxEdit2.SelectedIndex = 0;
                    comboBoxEdit2.Enabled = true;
                }
                else
                {
                    comboBoxEdit2.Enabled = false;
                    comboBoxEdit2.Text = "没有数据";
                }
                if (!string.IsNullOrEmpty(CurrentPointID2))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID2))
                        {
                            comboBoxEdit2.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_LoadPointSelList2" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList3(DateTime stime, DateTime etime)
        {
            try
            {
                comboBoxEdit3.Properties.Items.Clear();
                if (LoadPointStr.Count < 1)
                {
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
                }
                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit3.Properties.Items.Add(PointStr);
                if (comboBoxEdit3.Properties.Items.Count > 0)
                {
                    comboBoxEdit3.SelectedIndex = 0;
                    comboBoxEdit3.Enabled = true;
                }
                else
                {
                    comboBoxEdit3.Enabled = false;
                    comboBoxEdit3.Text = "没有数据";
                }
                if (!string.IsNullOrEmpty(CurrentPointID3))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID3))
                        {
                            comboBoxEdit3.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_LoadPointSelList3" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "开关量同屏曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     曲线鼠标移动获取游标的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            try
            {
                var ShowText = "";
                var value = "";
                DataRow[] drs1 = null;
                DataRow[] drs2 = null;
                DataRow[] drs3 = null;
                string SelTime = "";
                string SelTimeNow ="";
                var index = 0;

                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    foreach (var element1 in e.CrosshairAxisLabelElements)
                        SelTimeNow = Convert.ToDateTime(element1.AxisValue).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    if (index == 0)
                        element.LabelElement.HeaderText = SelTimeNow.ToString(); //显示要显示的文字
                    drs1 = dt_line1.Select("sTimer='" + SelTime + "' ");
                    drs2 = dt_line2.Select("sTimer='" + SelTime + "' ");
                    drs3 = dt_line3.Select("sTimer='" + SelTime + "' ");

                    if (point.Values[0].ToString() == "1E-05")
                    {
                        ShowText = element.Series.Name + ":" + "未记录";
                    }
                    else
                    {
                        if (element.Series.Name == point1)
                        {
                            if (drs1.Length > 0)
                                ShowText = element.Series.Name + ":" + drs1[0]["stateName"];
                        }
                        else if (element.Series.Name == point2)
                        {
                            if (drs2.Length > 0)
                                ShowText = element.Series.Name + ":" + drs2[0]["stateName"];
                        }
                        else if (element.Series.Name == point3)
                        {
                            if (drs3.Length > 0)
                                ShowText = element.Series.Name + ":" + drs3[0]["stateName"];
                        }
                        else
                        {
                            ShowText = element.Series.Name + ":" + "未记录";
                        }
                    }
                    element.LabelElement.Text = ShowText; //显示要显示的文字
                    index++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLineWidthScreen_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 已定义，已存储功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPointSelList1(dateEdit1.DateTime, dateEdit1.DateTime);
            LoadPointSelList2(dateEdit1.DateTime, dateEdit1.DateTime);
            LoadPointSelList3(dateEdit1.DateTime, dateEdit1.DateTime);
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