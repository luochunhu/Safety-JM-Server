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
    public partial class Kgl_StateBar : XtraForm
    {
        ///// <summary>
        /////     传入测点ID（参数）
        ///// </summary>
        //private string _PointID = "";

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

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line = new DataTable();

        private bool IsInIframe = false;

        /// <summary>
        ///     是否计算未知状态
        /// </summary>
        private bool kglztjsfs = true;

        /// <summary>
        ///     记录鼠标上一次移动的时间
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        /// <summary>
        ///     测点单位
        /// </summary>
        private string PointDw = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();


        public Kgl_StateBar()
        {
            InitializeComponent();
        }

        public Kgl_StateBar(Dictionary<string, string> param)
        {
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID"))
                    {
                        if (!string.IsNullOrEmpty(param["PointID"]))
                            //_PointID = param["PointID"];
                        CurrentPointID = param["PointID"];
                    }
                    else
                    {
                        //_PointID = "";
                        CurrentPointID = "";
                    }
                }
                catch
                {
                    //_PointID = "";
                    CurrentPointID = "";
                }
            else
                return;
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
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt, string name)
        {
            LoadSeries(Series1, name, "A", dt);
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
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["timer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     产生随机数(测试用)
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public string RandCode(int N)
        {
            char[] arrChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var num = new StringBuilder();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < N; i++)
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            return num.ToString();
        }

        public void Reload(Dictionary<string, string> param)
        {
            //_PointID = "";
            CurrentPointID = "";
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID"))
                    {
                        if (!string.IsNullOrEmpty(param["PointID"]))
                            //_PointID = param["PointID"];
                            CurrentPointID = param["PointID"];
                    }
                    else
                    {
                        //_PointID = "";
                        CurrentPointID = "";
                    }
                }
                catch
                {
                    //_PointID = "";
                    CurrentPointID = "";
                }
            object sender1 = null;
            var e1 = new EventArgs();
            Kgl_StateBar_Load(sender1, e1);
        }


        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kgl_StateBar_Load(object sender, EventArgs e)
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

                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                dateEdit2.DateTime = DateTime.Now;
                Series1.Points.Clear();
                gridControl1.DataSource = null;

                InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");

                LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime,true);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_Kgl_StateBar_Load" + ex.Message + ex.StackTrace);
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
                DataRow[] drs = null;
                var SelTime = "";
                var SelTimeNow = new DateTime();

                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    foreach (var element1 in e.CrosshairAxisLabelElements)
                        SelTimeNow = DateTime.Parse(element1.AxisValue.ToString());
                    SelTime = SelTimeNow.ToString("yyyy-MM-dd") + " " + SelTimeNow.Hour.ToString("00") + ":00:00";
                    drs = dt_line.Select("timer='" + SelTime + "' ");
                    if (drs.Length > 0)
                    {
                        ShowText += "测点号：" + element.Series.Name + "\n";
                        ShowText += "开机率：" + double.Parse(drs[0]["A"].ToString()).ToString("0.00") + "%" + "\n";
                        ShowText += "开机时间：" + drs[0]["B"] + "\n";
                        ShowText += "开停次数：" + drs[0]["C"];
                    }
                    element.LabelElement.Text = ShowText; //显示要显示的文字
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_chart_CustomDrawCrosshair" + ex.Message +
                                                        ex.StackTrace);
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
                    ChartPrint.chartPrint(chart, (float)(Width*0.8));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Kgl_StateBar_ExportToCore" + ex.Message + ex.StackTrace);
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
                var SzNameE = dateEdit2.DateTime;
                //数据校验
                //var ts = SzNameE - SzNameS;
                //bug修正: 2017-03-16
                if (SzNameS.Date > SzNameE.Date)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("查询开始时间不能大于结束时间,请重新选择日期！");
                    return;
                }
                if ((SzNameE.Date - SzNameS.Date).TotalDays > 6)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("查询的最大天数为7天,请重新选择日期！");
                    return;
                }
                if (comboBoxEdit1.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点！");
                    return;
                }


                CurrentPointID = PointIDList[comboBoxEdit1.SelectedIndex];
                //_PointID = CurrentPointID;
                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];

                if (checkEdit1.Checked)
                    kglztjsfs = true;
                else
                    kglztjsfs = false;

                dt_line.Rows.Clear();

                //循环查找所有天的数据
                for (var NTime = SzNameS; NTime <= SzNameE; NTime = NTime.AddDays(1))
                {
                    var tempDt = InterfaceClass.KglStateLineQueryClass_.InitQxZhuZhuang(NTime, CurrentPointID, CurrentDevid,
                    CurrentWzid, kglztjsfs);

                    //var tempDt = InterfaceClass.KglStateLineQueryClass_.getStateLineDt(NTime, CurrentPointID,
                    //    CurrentDevid, CurrentWzid, kglztjsfs);
                    if (dt_line.Columns.Count < 1)
                        dt_line = tempDt.Clone();
                    foreach (DataRow dr in tempDt.Rows)
                        dt_line.Rows.Add(dr.ItemArray);
                }


                //dt_line = InterfaceClass.KglStateLineQueryClass_.InitQxZhuZhuang(SzNameS, CurrentPointID, CurrentDevid,
                //    CurrentWzid, kglztjsfs);

                var pointDev = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentPointID);
                var stateName = "2态:" + pointDev[2] + "," + "1态:" + pointDev[1] + "," + "0态:" + pointDev[0];

                var name = comboBoxEdit1.SelectedItem + "(" + stateName + ")";
                var shortName = comboBoxEdit1.SelectedItem.ToString()
                    .Substring(0, comboBoxEdit1.SelectedItem.ToString().IndexOf("."));

                chart.Titles[0].Text = name;
                InitControls(dt_line, shortName);


                var _minX = SzNameS.Date;
                //var _maxX = DateTime.Parse(SzNameS.ToShortDateString() + " 23:59:59");
                var _maxX = SzNameE.Date.AddDays(1).AddHours(-1);

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);
                //AxisX.ar
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_simpleButton1_Click" + ex.Message + ex.StackTrace);
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
                    LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime,false);
                }

                ////加载曲线
                //if (!string.IsNullOrEmpty(_PointID))
                //{
                //    LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime);

                //    //给combox赋初始值
                //    for (var i = 0; i < PointIDList.Count; i++)
                //        if (PointIDList[i].Contains(_PointID))
                //        {
                //            comboBoxEdit1.SelectedIndex = i;
                //            break;
                //        }
                //    //调用查询
                //    object sender1 = null;
                //    var e1 = new EventArgs();
                //    simpleButton1_Click(sender1, e1);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_dateEdit1_EditValueChanged" + ex.Message +
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
                {
                    comboBoxEdit1.Properties.Items.Add(PointStr);
                }
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
        ///     曲线移动显示列表数据
        /// </summary>
        private void ChartGridDis(DateTime SelTime)
        {
            try
            {
                /* 0:名称及类型
                 * 1:报警及断电状态
                 * 2:读值时区
                 * 3:读值时刻
                 * 4:开机率
                 * 5:开机时间
                 * 6:开停次数
                 * 7:报警/解除
                 * 8:断电/复电
                 * 9:馈电状态
                 * 10:措施及时刻
                 */
                var QueryStr = new string[11];
                var tempPointInf = new string[11];
                var DtRefresh = new DataTable();
                DtRefresh.Columns.Add("名称及类型", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警及断电状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("读值时区", Type.GetType("System.String"));
                DtRefresh.Columns.Add("读值时刻", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开机效率", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开机时间", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开停次数", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警/解除", Type.GetType("System.String"));
                DtRefresh.Columns.Add("断电/复电", Type.GetType("System.String"));
                DtRefresh.Columns.Add("馈电状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("措施及时刻", Type.GetType("System.String"));

                QueryStr[0] =
                    comboBoxEdit1.SelectedItem.ToString()
                        .Substring(comboBoxEdit1.SelectedItem.ToString().IndexOf(".") + 1);
                QueryStr[2] = SelTime.Hour + ":00" + "~" + (SelTime.Hour + 1) + ":00";
                QueryStr[3] = SelTime.ToLongTimeString();
                var Dt1 = new DateTime();
                var dt2 = new DateTime();
                Dt1 = DateTime.Parse(SelTime.ToLongDateString());
                dt2 = DateTime.Parse(SelTime.ToLongDateString());
                Dt1 = Dt1.AddHours(SelTime.Hour);
                dt2 = dt2.AddHours(SelTime.Hour + 1);

                tempPointInf = InterfaceClass.KglStateLineQueryClass_.GetDgView(SelTime, CurrentPointID, CurrentDevid,
                    CurrentWzid, kglztjsfs);
                QueryStr[1] = tempPointInf[1];
                QueryStr[7] = tempPointInf[7];
                QueryStr[8] = tempPointInf[8];
                QueryStr[9] = tempPointInf[9];
                QueryStr[10] = tempPointInf[10];

                tempPointInf = InterfaceClass.KglStateLineQueryClass_.GetKjThings(Dt1, dt2, CurrentPointID, CurrentDevid,
                    CurrentWzid, kglztjsfs);
                QueryStr[4] = tempPointInf[4];
                QueryStr[5] = tempPointInf[5];
                QueryStr[6] = tempPointInf[6];


                DtRefresh.Clear();
                var Dr = DtRefresh.NewRow();
                for (var i = 0; i < 11; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);
                gridControl1.DataSource = DtRefresh;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_ChartGridDis" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "开关量柱状图.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //_isScale = false;
                var diagram = Diagram;
                var _p0 = e.Location;
                var coord = diagram.PointToDiagram(_p0);
                var SelTime = coord.DateTimeArgument;

                ChartGridDis(SelTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateBar_chart_MouseClick" + ex.Message + ex.StackTrace);
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

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
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
                LogHelper.Error("Kgl_StateBar_dateEdit1_EditValueChanged" + ex.Message +
                                                        ex.StackTrace);
            }
        }

    }
}