using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraGrid.Views.Grid;
using System.Diagnostics;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.NetworkModule;
using System.Threading;
using Sys.DataCollection.Common.Protocols;

namespace Sys.Safety.Client.Display
{
    public partial class NetForm : XtraForm
    {
        INetworkModuleService _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
        public NetForm()
        {
            InitializeComponent();
        }

        #region=====================网络模块======================

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;
        /// <summary>
        /// 通过WEB来获取交换机状态 
        /// </summary>
        Dictionary<string, WebBrowser> webBrowserList = new Dictionary<string, WebBrowser>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "IP","MAC","安装位置",
            "绑定队列","千兆光口状态","百兆光口状态","百兆电口状态"};//,"连接号","状态","电源箱电池控制状态","电源箱状态","电源箱电池容量","串口服务器-供电电源","串口服务器-运行状态","交换机-供电电源","交换机-运行状态"

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"ip","mac",
            "wz", "dl","Switch1000State","Switch100State","Switch100RJ45State"};//,"ljh","zt","BatteryControlState","BatteryState","BatteryCapacity","SerialPortBatteryState","SerialPortRunState","SwitchBatteryState","SwitchRunState"

        public int[] colwith = new int[] { 80, 80, 80, 100, 100, 100, 100 };//80, 80,100, 100,100, 100, 100, 100, 100,

        private Thread freshthread;
        private bool _isRun = false;
        DataTable dt = new DataTable();

        /// <summary>
        /// 初始显示表
        /// </summary>
        public void inigrid()
        {
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            // 
            // repositoryItemMemoEdit1
            // 
            repositoryItemMemoEdit1.Appearance.Options.UseTextOptions = true;
            repositoryItemMemoEdit1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";

            GridColumn col;
            inidt();
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = tcolname[i];
                col.Width = colwith[i];
                col.Visible = true;
                col.ColumnEdit = repositoryItemMemoEdit1;
                col.Tag = i;
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        public void inidt()
        {
            DataColumn col;
            showdt = new DataTable();
            for (int i = 0; i < colname.Length; i++)
            {
                col = new DataColumn(tcolname[i]);
                showdt.Columns.Add(col);
            }
        }
        #endregion

        private void NetForm_Load(object sender, EventArgs e)
        {
            //设置窗体高度和宽度
            Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
            Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
            Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
            Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

            inigrid();
            //dt = Model.RealInterfaceFuction.GetRealMac();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    WebBrowser temp = new WebBrowser();
            //    temp.Navigate("http://" + dt.Rows[i]["ip"].ToString() + "/view.html?next_file=view.html");
            //    webBrowserList.Add(dt.Rows[i]["mac"].ToString(), temp);
            //}
            //show(dt);


            _isRun = true;
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.IsBackground = true;
            freshthread.Start();
        }
        private void show()
        {
            int x = -1, y = -1, count = 0, topindex = 0;
            try
            {
                topindex = mainGridView.TopRowIndex;
                count = mainGridView.RowCount;
                if (mainGridView.FocusedColumn != null)
                {
                    x = mainGridView.FocusedColumn.ColumnHandle;
                    y = mainGridView.FocusedRowHandle;
                }

                if (dt != null)
                {
                    ////获取交换机状态 
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    //List<string> switchState = GetSwitchState(dt.Rows[i]["mac"].ToString());
                    //    dt.Rows[i]["Switch1000State"] = switchState[2];
                    //    dt.Rows[i]["Switch100State"] = switchState[1];
                    //    dt.Rows[i]["Switch100RJ45State"] = switchState[0];
                    //}

                    mainGrid.DataSource = dt;

                    if (dt.Rows.Count == count)
                    {
                        if (x > -1 && y > -1)
                        {
                            mainGridView.FocusedColumn.ColumnHandle = x;
                            mainGridView.FocusedRowHandle = y;
                        }
                        mainGridView.TopRowIndex = topindex;
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Enabled = false;
        //    show();
        //    timer1.Enabled = true;
        //}
        private void fthread()
        {
            dt = Model.RealInterfaceFuction.GetRealMac();
            MethodInvoker In1 = new MethodInvoker(() => show());
            this.BeginInvoke(In1);

            while (_isRun)
            {
                try
                {
                    //DataTable dt = Model.RealInterfaceFuction.GetRealMac();
                    dt = Model.RealInterfaceFuction.GetRealMac();

                    //MethodInvoker In1 = new MethodInvoker(() => show());
                    //this.BeginInvoke(In1);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        NetDeviceSettingInfo tempNetInfo = _NetworkModuleService.GetNetworkModuletParameters(new NetworkModuletParametersGetRequest()
                          {
                              Mac = dt.Rows[i]["mac"].ToString()
                          }).Data;
                        if (tempNetInfo == null)
                        {
                            dt.Rows[i]["Switch1000State"] = "-";
                            dt.Rows[i]["Switch100State"] = "-";
                            dt.Rows[i]["Switch100RJ45State"] = "-";
                            continue;
                        }

                        string switch1000StateString = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch1000JkState != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch1000JkState.Length; j++)
                            {
                                switch1000StateString += "千兆光口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch1000JkState[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        dt.Rows[i]["Switch1000State"] = switch1000StateString == "" ? "-" : switch1000StateString;
                        string switch100StateString = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch100JkState != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch100JkState.Length; j++)
                            {
                                switch100StateString += "百兆光口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch100JkState[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        dt.Rows[i]["Switch100State"] = switch100StateString == "" ? "-" : switch100StateString;
                        string switch100RJ45State = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch100RJ45State != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch100RJ45State.Length; j++)
                            {
                                switch100RJ45State += "百兆电口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch100RJ45State[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        dt.Rows[i]["Switch100RJ45State"] = switch100RJ45State == "" ? "-" : switch100RJ45State;
                    }

                    In1 = new MethodInvoker(() => show());
                    this.BeginInvoke(In1);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(15000);
            }
        }
        private List<string> GetSwitchState(string mac)
        {
            List<string> revalue = new List<string>();
            try
            {
                WebBrowser webBrowser1 = new WebBrowser();
                webBrowser1 = webBrowserList[mac];

                string str = "";
                string Switch100RJ45StateString = "";
                string[] textArray1 = new string[] { str, "电口1：", (webBrowser1.Document.GetElementById("link_0").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_0").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_0").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("media_0").InnerText, "\r\n" };
                str = string.Concat(textArray1);
                string[] textArray2 = new string[] { str, "电口2：", (webBrowser1.Document.GetElementById("link_1").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_1").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_1").InnerText,
                //"  ", webBrowser1.Document.GetElementById("media_1").InnerText, "\r\n" };
                str = string.Concat(textArray2);
                string[] textArray3 = new string[] { str, "电口3：", (webBrowser1.Document.GetElementById("link_2").InnerText == "LINK" ? "正常" : "断开") };
                //"  ", webBrowser1.Document.GetElementById("port_state_2").InnerText,
                //"  ", webBrowser1.Document.GetElementById("speed_2").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("media_2").InnerText, "\r\n" };
                str = string.Concat(textArray3);
                Switch100RJ45StateString = str;
                revalue.Add(Switch100RJ45StateString);

                string Switch100StateString = "";
                str = "";
                string[] textArray4 = new string[] { str, "光口1：", (webBrowser1.Document.GetElementById("link_3").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_3").InnerText,
                //"  ", webBrowser1.Document.GetElementById("speed_3").InnerText,
                //"  ", webBrowser1.Document.GetElementById("media_3").InnerText, "\r\n" };
                str = string.Concat(textArray4);
                string[] textArray5 = new string[] { str, "光口2：", (webBrowser1.Document.GetElementById("link_4").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_4").InnerText,
                //"  ", webBrowser1.Document.GetElementById("speed_4").InnerText,
                //"  ", webBrowser1.Document.GetElementById("media_4").InnerText, "\r\n" };
                str = string.Concat(textArray5);
                string[] textArray6 = new string[] { str, "光口3：", (webBrowser1.Document.GetElementById("link_5").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_5").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_5").InnerText,
                //"  ", webBrowser1.Document.GetElementById("media_5").InnerText, "\r\n" };
                str = string.Concat(textArray6);
                string[] textArray7 = new string[] { str, "光口4：", (webBrowser1.Document.GetElementById("link_6").InnerText == "LINK" ? "正常" : "断开") };
                //"  ", webBrowser1.Document.GetElementById("port_state_6").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_6").InnerText,
                //"  ", webBrowser1.Document.GetElementById("media_6").InnerText, "\r\n" };
                str = string.Concat(textArray7);
                Switch100StateString = str;
                revalue.Add(Switch100StateString);

                string Switch1000StateString = "";
                str = "";
                string[] textArray8 = new string[] { str, "光口1：", (webBrowser1.Document.GetElementById("link_7").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_7").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_7").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("media_7").InnerText, "\r\n" };
                str = string.Concat(textArray8);
                string[] textArray9 = new string[] { str, "光口2：", (webBrowser1.Document.GetElementById("link_8").InnerText == "LINK" ? "正常" : "断开"), "\r\n" };
                //"  ", webBrowser1.Document.GetElementById("port_state_8").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_8").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("media_8").InnerText, "\r\n" };
                str = string.Concat(textArray9);
                string[] textArray10 = new string[] { str, "光口3：", (webBrowser1.Document.GetElementById("link_9").InnerText == "LINK" ? "正常" : "断开") };
                //"  ", webBrowser1.Document.GetElementById("port_state_9").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("speed_9").InnerText, 
                //"  ", webBrowser1.Document.GetElementById("media_9").InnerText, "\r\n" };
                str = string.Concat(textArray10);
                Switch1000StateString = str;
                revalue.Add(Switch1000StateString);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                revalue.Add("-");
                revalue.Add("-");
                revalue.Add("-");
            }
            return revalue;

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
            string value = "";
            //if (e.Column.Tag.ToString() == "4")
            //{
            //    if (e.CellValue != null)
            //    {
            //        value = e.CellValue.ToString();
            //        if (value == "通讯中断" || value == "通讯误码")
            //        {
            //            e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
            //        }
            //        else if (value == "直流正常")
            //        {
            //            e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.DcColor;
            //        }
            //        else
            //        {
            //            e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
            //        }
            //    }
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ExportDataGridViewToExcel(mainGridView);
        }

        /// <summary>
        /// 将数据导出Excel
        /// </summary>
        private void ExportDataGridViewToExcel(GridView dataGridView)
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
                StartProcess(fileDialog.FileName);
            }
        }
        public static void StartProcess(string path)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = path;
                process.Start();
                process.WaitForInputIdle();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void NetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }


    }
}
