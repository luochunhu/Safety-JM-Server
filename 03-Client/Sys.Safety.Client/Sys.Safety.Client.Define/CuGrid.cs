using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Sys.Safety.Client.Define.Model;
using Sys.Safety.ClientFramework.View.Message;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Threading;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.Client.Define.Sensor;
using Sys.Safety.Client.GraphDefine;
using System.Runtime.InteropServices;
using Sys.Safety.Client.Define.Area;


namespace Sys.Safety.Client.Define
{

    public partial class CuGrid : UserControl
    {
        public CuGrid()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }

        public CuGrid(CFPointMrgFrame CFFram)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            _CFFram = CFFram;
            InitializeComponent();

            
        }
        /// <summary>
        /// 主窗体对象
        /// </summary>
        private CFPointMrgFrame _CFFram;
        /// <summary>
        /// 等待窗体
        /// </summary>
        private Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm WaitDialogFormTemp;
        /// <summary>
        /// 用于更新UI的委托定义
        /// </summary>
        private delegate void UpdateControl();
        /// <summary>
        /// 后台委托封装
        /// </summary>
        /// <returns></returns>
        private delegate bool dl_DoAsync();
        /// <summary>
        /// 用于更新UI委托声明
        /// </summary>
        private UpdateControl dl_updataUI;
        /// <summary>
        /// 分站图形定义标记
        /// </summary>
        private bool StationDefineFlag = false;
        /// <summary>
        /// 当前编辑的分站对象
        /// </summary>
        public Jc_DefInfo EditStationNow = null;
        /// <summary>
        /// 图形操作类
        /// </summary>
        public GraphicOperations GraphOpt;

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
        IntPtr hwnd,
        int wMsg,
        int wParam,
        int lParam
        );
        Hashtable htCmd = new Hashtable();//保存命令
        Hashtable htParam = new Hashtable();//保存参数
        int nViewCmdIndex = 0;//索引

        /// <summary>
        /// 处理完成回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackMethod(IAsyncResult ar)
        {
            try
            {
                dl_DoAsync dl_do = (dl_DoAsync)ar.AsyncState;
                dl_do.EndInvoke(ar);
                this.BeginInvoke(dl_updataUI); //更新UI
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 处理后台事件的过程
        /// </summary>
        /// <returns></returns>
        private bool DoAsync()
        {
            #region 此处写后台代码
            try
            {
                InitGridData();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 更新UI
        /// </summary>
        private void updateUI()
        {
            #region 此处写更新UI的代码

            #endregion
        }
        private void CuGrid_Load(object sender, EventArgs e)
        {
            try
            {
                GraphOpt = new GraphicOperations();
                //if (Basic.Framework.Utils.COMREG.COMUtils.IsRegistered("8ACFF379-3E78-4E73-B75E-ECD908072A02"))
                //{
                mx = new Axmetamap2dLib.AxMetaMapX2D();               
                mx.OnViewCallOutCommand += new Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEventHandler(mx_OnViewCallOutCommand);
                CpDocument.Controls.Add(mx);
                mx.Dock = DockStyle.Fill;
                mx.SendToBack();
                tlbTopology.Enabled = true;
                //}

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 初次加载数据
        /// </summary>
        private void InitGridData()
        {
            GridSource.GridStationSource.Clear();
            IList<Jc_DefInfo> Stations;
            GridStationItem StationItem;
            try
            {
                Stations = DEFServiceModel.QueryPointByDevpropertIDCache(0);
                if (Stations == null)
                {
                    return;
                }
                for (int i = 0; i < Stations.Count; i++)
                {
                    StationItem = new GridStationItem();
                    StationItem.fzh = Stations[i].Fzh.ToString();
                    StationItem.Point = Stations[i].Point.ToString();
                    StationItem.wz = Stations[i].Wz.ToString();
                    StationItem.DevName = Stations[i].DevName.ToString();
                    StationItem.RunState = Stations[i].Bz4.ToString();
                    StationItem.ComNum = Stations[i].K3;
                    StationItem.CommType = Stations[i].Jckz1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #region Grid CommMethod
        private void tlbExportExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".xls";
            Export(1, strFileType);
        }

        private void tlbExeclPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".pdf";
            Export(2, strFileType);
        }

        private void txtExportTXT_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".txt";
            Export(3, strFileType);
        }

        private void txtExportCSV_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".csv";
            Export(4, strFileType);
        }

        private void txtExportHTML_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".html";
            Export(5, strFileType);
        }

        /// <summary>
        /// 导出通用方法
        /// </summary>
        /// <param name="LngTypeID">文件类型ID，用于默认选择类型</param>
        /// <param name="strFileType">文件类型名称，用于给文件名赋后缀名</param>
        private void Export(int LngTypeID, string strFileType)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls|PDF文件(*.pdf)|*.pdf|TXT文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|HTML文件(*.html)|*.html";
                saveFileDialog.Title = "保存文件";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = this.Text;
                saveFileDialog.FilterIndex = LngTypeID;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridView.OptionsPrint.PrintHeader = true;//设置后列表的列名会打印出来
                    gridView.OptionsPrint.PrintFooter = true;//设置后列表有footer（如小计）
                    //gridView.OptionsPrint.UsePrintStyles = false;//设置后为保留列表颜色样式
                    //列表方式
                    if (LngTypeID == 1)
                    {
                        gridView.ExportToXls(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 2)
                    {
                        gridView.ExportToPdf(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 3)
                    {
                        gridView.ExportToText(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 4)
                    {
                        gridView.ExportToCsv(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 5)
                    {
                        gridView.ExportToHtml(saveFileDialog.FileName);
                    }
                }
                if (DialogResult.Yes == MessageBox.Show("是否立即打开此文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);

                }
            }
            catch (System.Exception ex)
            {
                DevMessageBox.Show(DevMessageBox.MessageType.Warning, ex.ToString());
            }
        }

        private void tlbPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView.OptionsPrint.PrintHeader = true;
            gridView.OptionsPrint.PrintFooter = true;

            this.gridView.ShowRibbonPrintPreview();
        }

        private void tlbSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            tlbSearch.Enabled = false;
            try
            {
                WaitDialogFormTemp = new Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm("搜索交换机", "正在搜索交换机,请稍后......");
                UpdateControl task = SearchIP;
                task.BeginInvoke(null, null);

                for (int i = 0; i < WaitDialogFormTemp.progressShow.Properties.Maximum; i++)
                {
                    //处理当前消息队列中的所有windows消息
                    Application.DoEvents();
                    Thread.Sleep(30);
                    WaitDialogFormTemp.progressShow.PerformStep();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                WaitDialogFormTemp.Close();
                _CFFram.cuNavigation.InitDevData();
                _CFFram.cuNavigation.CtreeListDev.RefreshDataSource();
            }
            tlbSearch.Enabled = true;
        }

        /// <summary>
        /// 搜索IP
        /// </summary>
        private void SearchIP()
        {
            try
            {
                //MACServiceModel.SearchALLIPCache();// 20170112
                MACServiceModel.SearchALLIPCache8962(0);// 20170112
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
        }
        #endregion

        #region Grid Event
        /// <summary>
        /// 自动增加序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 设备管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hInfo = gridView.CalcHitInfo(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Left && e.Clicks == 2)
                {
                    //新增数据
                    if (this.gridView.IsNewItemRow(this.gridView.FocusedRowHandle))
                    {
                        if (this.CGridControl.Tag != null)
                        {
                            //if (this.CGridControl.Tag.ToString() != "5")//网络模块不能新增  20170615
                            //{
                                _CFFram.cuNavigation.ShowFormForDev(this.CGridControl.Tag.ToString(), "", _CFFram.cuNavigation.StrParameter1, _CFFram.cuNavigation.StrParameter2);
                            //}
                        }
                    }
                    else
                    {
                        //编辑数据
                        string colValueTag = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Tag").ToString();
                        string colValueCode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code").ToString();
                        _CFFram.cuNavigation.ShowFormForDev(colValueTag, colValueCode, _CFFram.cuNavigation.StrParameter1, _CFFram.cuNavigation.StrParameter2);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion


        #region Graph Methord
        /// <summary>
        /// 分站定义图形加载
        /// </summary>
        public bool StationDefineMapLoad(Jc_DefInfo StationNow)
        {
            bool rvalue = true;
            try
            {
                if (EditStationNow == null)
                {
                    EditStationNow = StationNow;
                    StationDefineFlag = true;//置标记

                    this.mx.BringToFront();
                    this.CGridControl.SendToBack();

                    //增加服务授权设置  20171026
                    mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
                    mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
                    string path = Application.StartupPath;
                    mx.SetWebRootPath(path + "\\");//设置根目录
                    mx.SetMapScriptPath("mx/");//设置元图脚本API路径，相对于根目录地址
                    string dwgFileName = "";
                    //dwgFileName = "width:1000;height:800";
                    //mx.SetData("showProgress", "false");
                    //mx.OpenDwg(dwgFileName, false, "");
                    dwgFileName = path + "\\blank.html";
                    mx.SetData("showProgress", "false");
                    
                    mx.OpenUrl(dwgFileName, false, "");
                }
                else
                {
                    StationDefineFlag = true;//置标记

                    this.mx.BringToFront();
                    this.CGridControl.SendToBack();

                    if (EditStationNow != StationNow)
                    {
                        EditStationNow = StationNow;
                    }

                    LoadStationPoint();

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                rvalue = false;
            }
            return rvalue;
        }
        /// <summary>
        /// 系统拓扑图展示加载
        /// </summary>
        public void TopologyDefineMapLoad()
        {
            try
            {
                StationDefineFlag = false;//置标记

                this.mx.BringToFront();
                this.CGridControl.SendToBack();

                //增加服务授权设置  20171026
                mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
                mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
                string path = Application.StartupPath;
                mx.SetWebRootPath(path + "\\");//设置根目录
                mx.SetMapScriptPath("mx/");//设置元图脚本API路径，相对于根目录地址
                string dwgFileName = "";
                dwgFileName = "width:1000;height:800";
               
                mx.OpenDwg(dwgFileName, false, "");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 加载拓扑定义静态图元
        /// </summary>
        public void LoadStationPoint()
        {

            string PointListStr = "";
            PointListStr = PointListStr + @"拓扑定义-分站背景|||拓扑定义-分站背景|1|0|0|-20.0|740|424|504|1|,";
            PointListStr = PointListStr + @"拓扑定义-标注|||拓扑定义-标注|1|0|0|516.0|840|300|48|1|,";

            //PointListStr = PointListStr + EditStationNow.Wz + @"|||拓扑定义-分站位置|1|0|0|-20.0|820|650|34|1|,";//不显示分站位置  20170628

            //查找分站下传感器的定义情况
            string[] PointDefineArray = new string[16];
            string[] ControlDefineArray = new string[6];
            string[] SmartDefineArray = new string[8];
            StaionInitInfo staionInitInfo = new StaionInitInfo();
            staionInitInfo.AddStationTopologyDefineLineList = new List<AddStationTopologyDefineLineObject>();
            List<Jc_DefInfo> StationSonPoint = DEFServiceModel.QueryPointByStationCache(EditStationNow.Point);
            Dictionary<int, EnumcodeInfo> devMoelsInfoList = DEVServiceModel.QueryDevMoelsCache();
            List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
            //分支1
            List<Jc_DefInfo> StationSonPoint1 = StationSonPoint.FindAll(a => a.Kh >= 1 && a.Kh <= 4 && (a.DevPropertyID == 1 || a.DevPropertyID == 2));
            int StartYIndex = 735;
            for (int i = 1; i <= 4; i++)
            {
                Jc_DefInfo TempPoint = StationSonPoint1.Find(a => a.Kh == i);
                if (TempPoint != null)
                {
                    PointDefineArray[i - 1] = TempPoint.Point;
                    PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-传感器蓝|1|0|0|-280.0|" + StartYIndex + "|138|42|1|,";
                }
                else
                {
                    PointDefineArray[i - 1] = "传感器-" + i.ToString();
                    PointListStr = PointListStr + @"传感器-" + i.ToString() + "|||拓扑定义-传感器灰|1|0|0|-280.0|" + StartYIndex + "|138|42|1|,";
                }

                //分支1下面是否存在自动挂接设备               
                AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                    && a.ChanelNumber == i);
                if (tempAutomaticArticulatedDevice != null)
                {
                    string devModelName = "";
                    if (tempAutomaticArticulatedDevice.DeviceModel > 0 && devMoelsInfoList.ContainsKey(tempAutomaticArticulatedDevice.DeviceModel))
                    {
                        devModelName = devMoelsInfoList[tempAutomaticArticulatedDevice.DeviceModel].StrEnumDisplay;
                    }
                    if (tempAutomaticArticulatedDevice.BranchNumber == 1)
                    { //表示是当前分支及口号下挂接的自定义设备                        
                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + "||" + devModelName + "|拓扑定义-传感器黄|1|0|0|-430.0|" + StartYIndex + "|138|42|1|,";
                    }
                    else
                    { //表示当前设备挂接地址号错误（口号与地址号不配置)

                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + ":" + tempAutomaticArticulatedDevice.BranchNumber + "||" + devModelName + "|拓扑定义-传感器红|1|0|0|-430.0|" + StartYIndex + "|138|42|1|,";
                    }
                }

                StartYIndex = StartYIndex - 60;
            }
            //分支2
            List<Jc_DefInfo> StationSonPoint2 = StationSonPoint.FindAll(a => a.Kh >= 5 && a.Kh <= 16 && (a.DevPropertyID == 1 || a.DevPropertyID == 2));
            StartYIndex = 465;
            for (int i = 5; i <= 8; i++)
            {
                Jc_DefInfo TempPoint = StationSonPoint2.Find(a => a.Kh == i);
                if (TempPoint != null)
                {
                    PointDefineArray[i - 1] = TempPoint.Point;
                    PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-传感器蓝|1|0|0|-280.0|" + StartYIndex + "|138|42|1|,";
                }
                else
                {
                    PointDefineArray[i - 1] = "传感器-" + i.ToString();
                    PointListStr = PointListStr + @"传感器-" + i.ToString() + "|||拓扑定义-传感器灰|1|0|0|-280.0|" + StartYIndex + "|138|42|1|,";
                }
                //分支2下面是否存在自动挂接设备               
                AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                    && a.ChanelNumber == i);
                if (tempAutomaticArticulatedDevice != null)
                {
                    string devModelName = "";
                    if (tempAutomaticArticulatedDevice.DeviceModel > 0 && devMoelsInfoList.ContainsKey(tempAutomaticArticulatedDevice.DeviceModel))
                    {
                        devModelName = devMoelsInfoList[tempAutomaticArticulatedDevice.DeviceModel].StrEnumDisplay;
                    }
                    if (tempAutomaticArticulatedDevice.BranchNumber == 2)
                    { //表示是当前分支及口号下挂接的自定义设备                        
                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + "||" + devModelName + "|拓扑定义-传感器黄|1|0|0|-430.0|" + StartYIndex + "|138|42|1|,";
                    }
                    else
                    { //表示当前设备挂接地址号错误（口号与地址号不配置)

                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + ":" + tempAutomaticArticulatedDevice.BranchNumber + "||" + devModelName + "|拓扑定义-传感器红|1|0|0|-430.0|" + StartYIndex + "|138|42|1|,";
                    }
                }

                StartYIndex = StartYIndex - 60;
            }
            //分支3
            StartYIndex = 735;
            for (int i = 9; i <= 12; i++)
            {
                Jc_DefInfo TempPoint = StationSonPoint2.Find(a => a.Kh == i);
                if (TempPoint != null)
                {
                    PointDefineArray[i - 1] = TempPoint.Point;
                    PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-传感器蓝|1|0|0|540.0|" + StartYIndex + "|138|42|1|,";
                }
                else
                {
                    PointDefineArray[i - 1] = "传感器-" + i.ToString();
                    PointListStr = PointListStr + @"传感器-" + i.ToString() + "|||拓扑定义-传感器灰|1|0|0|540.0|" + StartYIndex + "|138|42|1|,";
                }
                //分支3下面是否存在自动挂接设备               
                AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                    && a.ChanelNumber == i);
                if (tempAutomaticArticulatedDevice != null)
                {
                    string devModelName = "";
                    if (tempAutomaticArticulatedDevice.DeviceModel > 0 && devMoelsInfoList.ContainsKey(tempAutomaticArticulatedDevice.DeviceModel))
                    {
                        devModelName = devMoelsInfoList[tempAutomaticArticulatedDevice.DeviceModel].StrEnumDisplay;
                    }
                    if (tempAutomaticArticulatedDevice.BranchNumber == 3)
                    { //表示是当前分支及口号下挂接的自定义设备                        
                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + "||" + devModelName + "|拓扑定义-传感器黄|1|0|0|690.0|" + StartYIndex + "|138|42|1|,";
                    }
                    else
                    { //表示当前设备挂接地址号错误（口号与地址号不配置)

                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + ":" + tempAutomaticArticulatedDevice.BranchNumber + "||" + devModelName + "|拓扑定义-传感器红|1|0|0|690.0|" + StartYIndex + "|138|42|1|,";
                    }
                }
                StartYIndex = StartYIndex - 60;
            }
            //分支4
            StartYIndex = 465;
            for (int i = 13; i <= 16; i++)
            {
                Jc_DefInfo TempPoint = StationSonPoint2.Find(a => a.Kh == i);
                if (TempPoint != null)
                {
                    PointDefineArray[i - 1] = TempPoint.Point;
                    PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-传感器蓝|1|0|0|540.0|" + StartYIndex + "|138|42|1|,";
                }
                else
                {
                    PointDefineArray[i - 1] = "传感器-" + i.ToString();
                    PointListStr = PointListStr + @"传感器-" + i.ToString() + "|||拓扑定义-传感器灰|1|0|0|540.0|" + StartYIndex + "|138|42|1|,";
                }
                //分支4下面是否存在自动挂接设备               
                AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                    && a.ChanelNumber == i);
                if (tempAutomaticArticulatedDevice != null)
                {
                    string devModelName = "";
                    if (tempAutomaticArticulatedDevice.DeviceModel > 0 && devMoelsInfoList.ContainsKey(tempAutomaticArticulatedDevice.DeviceModel))
                    {
                        devModelName = devMoelsInfoList[tempAutomaticArticulatedDevice.DeviceModel].StrEnumDisplay;
                    }
                    if (tempAutomaticArticulatedDevice.BranchNumber == 4)
                    { //表示是当前分支及口号下挂接的自定义设备                        
                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + "||" + devModelName + "|拓扑定义-传感器黄|1|0|0|690.0|" + StartYIndex + "|138|42|1|,";
                    }
                    else
                    { //表示当前设备挂接地址号错误（口号与地址号不配置)

                        PointListStr = PointListStr + @"自动挂接-" + i.ToString() + ":" + tempAutomaticArticulatedDevice.BranchNumber + "||" + devModelName + "|拓扑定义-传感器红|1|0|0|690.0|" + StartYIndex + "|138|42|1|,";
                    }
                }
                StartYIndex = StartYIndex - 60;
            }
            //控制通道
            List<Jc_DefInfo> StationSonPoint3 = StationSonPoint.FindAll(a => a.Kh >= 1 && a.Kh <= 6 && (a.DevPropertyID == 3));
            int StartXIndex = -160;
            //判断是否存在智能通道，如果存在，则增加智能通道定义 
            Jc_DevInfo stationDev = Model.DEVServiceModel.QueryDevByDevIDCache(EditStationNow.Devid);
            if (stationDev.Pl3 > 0)//存在智能口
            {
                StartXIndex = -440;
                for (int i = 1; i <= 6; i++)
                {
                    Jc_DefInfo TempPoint = StationSonPoint3.Find(a => a.Kh == i);
                    if (TempPoint != null)
                    {
                        ControlDefineArray[i - 1] = TempPoint.Point;
                        PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-控制器蓝|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    else
                    {
                        ControlDefineArray[i - 1] = "控制器-" + i.ToString();
                        PointListStr = PointListStr + @"控制器-" + i.ToString() + "|||拓扑定义-控制器灰|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    StartXIndex = StartXIndex + 110;
                }
                //绘制线路控制器
                for (int i = 0; i < ControlDefineArray.Length; i++)
                {
                    int BranchNumBer = 7;
                    AddStationTopologyDefineLineObject tempObject = new AddStationTopologyDefineLineObject();
                    tempObject.Point = ControlDefineArray[i];
                    tempObject.BranchNumBer = BranchNumBer.ToString();
                    staionInitInfo.AddStationTopologyDefineLineList.Add(tempObject);
                }
                StartXIndex = 60;
                List<Jc_DefInfo> StationSonPoint4 = StationSonPoint.FindAll(a => a.Kh >= 17 && a.Kh <= 24);
                //添加智能通道
                for (int i = 17; i <= 24; i++)
                {
                    Jc_DefInfo TempPoint = StationSonPoint4.Find(a => a.Kh == i);
                    if (TempPoint != null)
                    {
                        SmartDefineArray[i - 17] = TempPoint.Point;
                        PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-智能量蓝|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    else
                    {
                        SmartDefineArray[i - 17] = "智能量-" + i.ToString();
                        PointListStr = PointListStr + @"智能量-" + i.ToString() + "|||拓扑定义-智能量灰|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    StartXIndex = StartXIndex + 110;
                }
                //绘制线路智能量
                for (int i = 0; i < SmartDefineArray.Length; i++)
                {
                    int BranchNumBer = 6;
                    AddStationTopologyDefineLineObject tempObject = new AddStationTopologyDefineLineObject();
                    tempObject.Point = SmartDefineArray[i];
                    tempObject.BranchNumBer = BranchNumBer.ToString();
                    staionInitInfo.AddStationTopologyDefineLineList.Add(tempObject);
                }
            }
            else
            {
                for (int i = 1; i <= 6; i++)
                {
                    Jc_DefInfo TempPoint = StationSonPoint3.Find(a => a.Kh == i);
                    if (TempPoint != null)
                    {
                        ControlDefineArray[i - 1] = TempPoint.Point;
                        PointListStr = PointListStr + @"" + TempPoint.Point + "|" + TempPoint.Wz + "|" + TempPoint.DevName + "|拓扑定义-控制器蓝|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    else
                    {
                        ControlDefineArray[i - 1] = "控制器-" + i.ToString();
                        PointListStr = PointListStr + @"控制器-" + i.ToString() + "|||拓扑定义-控制器灰|1|0|0|" + StartXIndex + "|190|94|86|1|,";
                    }
                    StartXIndex = StartXIndex + 120;
                }
                //绘制线路控制器
                for (int i = 0; i < ControlDefineArray.Length; i++)
                {
                    int BranchNumBer = 5;
                    AddStationTopologyDefineLineObject tempObject = new AddStationTopologyDefineLineObject();
                    tempObject.Point = ControlDefineArray[i];
                    tempObject.BranchNumBer = BranchNumBer.ToString();
                    staionInitInfo.AddStationTopologyDefineLineList.Add(tempObject);
                }
            }


            //先删除之前的设备
            staionInitInfo.IsRemoveAllVectorLayerOverlay = true;
            //重新加载
            staionInitInfo.LoadPointString = PointListStr;

            //绘制线路

            for (int i = 0; i < PointDefineArray.Length; i++)
            {
                int BranchNumBer = (i / 4) + 1;
                AddStationTopologyDefineLineObject tempObject = new AddStationTopologyDefineLineObject();
                tempObject.Point = PointDefineArray[i];
                tempObject.BranchNumBer = BranchNumBer.ToString();
                staionInitInfo.AddStationTopologyDefineLineList.Add(tempObject);
            }


            //添加ToolTips
            staionInitInfo.IsLoadToolTip = true;

            string scriptCmd = "SubstationGraphicalLoad('" + Basic.Framework.Common.JSONHelper.ToJSONString(staionInitInfo) + "')";
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            //view.EvaluateJavaScript(scriptCmd);
            mx.CurViewEvaluateJavaScript(scriptCmd);
        }

        #endregion

        #region Graph Event
        /// <summary>
        /// 图形列表切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbTopology_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (tlbTopology.Caption == "拓扑图预览(&G)")
                {
                    tlbTopology.Caption = "数据列表(&L)";
                    this.mx.BringToFront();
                    this.CGridControl.SendToBack();
                    //TODO，不直接把图形写死，先判断图形是否存在，如果不存在则创建
                    GraphOpt.IsGraphicEdit = true;
                    TopologyDefineMapLoad();
                }
                else
                {
                    tlbTopology.Caption = "拓扑图预览(&G)";
                    this.mx.SendToBack();
                    this.CGridControl.BringToFront();
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        /// <summary>
        /// 转到列表界面
        /// </summary>
        public void ChangeToList()
        {
            try
            {
                tlbTopology.Caption = "拓扑图预览(&G)";
                //屏蔽图形  20170720
                //this.mx.SendToBack();
                this.CGridControl.BringToFront();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 图形控件命令响应方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
        {
            String strCmd = e.p_sCmd;
            String strParam = e.p_sParam;
            nViewCmdIndex = nViewCmdIndex + 1;
            if (nViewCmdIndex >= Int32.MaxValue - 1)
            {
                nViewCmdIndex = 1;
            }
            htCmd.Add(nViewCmdIndex, strCmd);
            htParam.Add(nViewCmdIndex, strParam);

            PostMessage(this.Handle, 0x0401, nViewCmdIndex, 0);

        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0401:
                    int nIndex = (int)m.WParam;
                    if (htCmd.Contains(nIndex) && htParam.Contains(nIndex))
                    {
                        ProcessViewCallOutCommand(htCmd[nIndex].ToString(), htParam[nIndex].ToString());
                        htCmd.Remove(nIndex);
                        htParam.Remove(nIndex);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void ProcessViewCallOutCommand(String strCmd, String strParam)
        {
            string Param = "";

            switch (strCmd)
            {
                case "MessagePub"://测试交互代码
                    MessageBox.Show(strParam);
                    break;
                case "PointEdit"://图形测点编辑
                    PointEdit(strParam);
                    break;
                case "PointDel"://图形测点删除
                    try
                    {
                        Param = strParam;
                        GraphOpt.DelPoint(mx, Param);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("图形测点删除失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_PointDel" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "PointsSave"://图形测点保存
                    try
                    {
                        Param = strParam;
                        GraphOpt.PointsSave(Param);
                        MessageBox.Show("图形测点保存成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("图形测点保存失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_PointsSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "RoutesSave"://保存测点连线 
                    try
                    {
                        Param = strParam;
                        GraphOpt.RoutesSave(Param);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存测点连线失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_RoutesSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "LoadPoint"://加载图形绑定的测点信息     
                    try
                    {
                        if (StationDefineFlag)
                        {
                            LoadStationPoint();
                        }
                        else
                        {
                            //GraphOpt.LoadMapPointsInfo(mx, GraphOpt.GraphNameNow);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("加载图形绑定的测点信息失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_LoadPoint" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "SetMapEditState"://设置图形的可编辑状态 
                    try
                    {
                        GraphOpt.setGraphEditState(mx, GraphOpt.IsGraphicEdit);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("设置图形的可编辑状态失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_SetMapEditState" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "setMapEditSave"://设置图形是否保存
                    try
                    {
                        Param = strParam;
                        GraphOpt.setGraphEditSave(bool.Parse(Param));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("设置图形是否保存失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_setMapEditSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "SetMapTopologyInit"://拓扑图初始化所有井上设备
                    try
                    {
                        if (!StationDefineFlag)
                        {
                            EditStationNow = null;
                            //生成井上设备
                            GraphOpt.SetMapTopologyInit(mx);
                            //自动生成拓扑图
                            GraphOpt.AutoDragTopologyTrans(mx);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("拓扑图初始化失败，详细请查看错误日志！");
                        LogHelper.Error("mx_OnViewCallOutCommand_SetMapTopologyInit" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "pageToImg":
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        GraphOpt.mapToImage(mx, saveFileDialog1.FileName);
                    }
                    break;
                case "layerDis":
                    //LayerDisHid layerdis = new LayerDisHid();
                    //layerdis.ShowDialog();
                    break;
                case "pointDis":
                    //PointDisHid pointdishid = new PointDisHid();
                    //pointdishid.ShowDialog();
                    break;
                case "PageChange":
                    break;
                case "AddMapRightMenu":


                    break;
                case "DoRefPointSsz":

                    break;
                case "MapLoadEndEvent":
                    try
                    {
                        //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                        //if (view == null) return;//如果还没有打开图形或网页  
                        if (StationDefineFlag)
                        {
                            string scriptCmd1 = "map.setZoom(5);map.setZoomRange(5,6,5,5);SetGraphicEdit(true);";
                            string scriptCmd2 = "UsersetMapCenter('220', '440');map.removeControl(ovctrl);";
                            //view.EvaluateJavaScript(scriptCmd1);
                            mx.CurViewEvaluateJavaScript(scriptCmd1);
                            //view.EvaluateJavaScript(scriptCmd2);
                            mx.CurViewEvaluateJavaScript(scriptCmd2);
                        }
                        else
                        {
                            string scriptCmd1 = "SetGraphicEdit(false);";
                            //view.EvaluateJavaScript(scriptCmd1);
                            mx.CurViewEvaluateJavaScript(scriptCmd1);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message + ex.StackTrace);
                    }
                    break;
            }
        }
        private void PointEdit(string Param)
        {
            try
            {
                //Param = e.p_sParam;
                string Point = Param.ToString().Split('|')[0];
                string UnitName = Param.ToString().Split('|')[1];
                if (Point.Contains("传感器-") || Point.Contains("控制器-") || Point.Contains("智能量-"))
                {
                    //新增
                    int ChanelNumber = int.Parse(Point.Substring(Point.IndexOf("-") + 1));
                    long DeviceId = 0;
                    int DevPropertyId = 0;
                    List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                    AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
            && a.ChanelNumber == ChanelNumber);
                    if (tempAutomaticArticulatedDevice != null)
                    {
                        Jc_DevInfo FzDev = new Jc_DevInfo();
                        FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Bz4 == tempAutomaticArticulatedDevice.DeviceModel);
                        if (FzDev != null)
                        {
                            DeviceId = long.Parse(FzDev.Devid);
                            DevPropertyId = FzDev.Type;
                        }
                    }
                    if (Point.Contains("传感器-"))
                    {
                        _CFFram.cuNavigation.ShowFormForDev("8", "", EditStationNow.Point, "基础通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                    else if (Point.Contains("控制器-"))
                    {
                        _CFFram.cuNavigation.ShowFormForDev("8", "", EditStationNow.Point, "控制通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                    else if (Point.Contains("智能量-"))
                    {
                        _CFFram.cuNavigation.ShowFormForDev("8", "", EditStationNow.Point, "智能通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                }
                else if (Point.Contains("自动挂接-") && Point.Contains(":"))//自动挂接设备地址号异常
                {
                    int ChanelNumber = int.Parse(Point.Split('-')[1].Split(':')[0]);
                    int BranchNumber = int.Parse(Point.Split(':')[1]);
                    string DeviceOnlyCode = "";
                    List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                    AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
            && a.ChanelNumber == ChanelNumber);
                    DeviceOnlyCode = tempAutomaticArticulatedDevice.DeviceOnlyCode;
                    if (DeviceOnlyCode.Substring(0, 1) != "3")
                    {
                        DeviceChanelNumberEdit deviceChanelNumberEdit = new DeviceChanelNumberEdit(DeviceOnlyCode, ChanelNumber.ToString(), BranchNumber.ToString(), EditStationNow.Fzh.ToString(), tempAutomaticArticulatedDevice.DeviceModel);
                        deviceChanelNumberEdit.ShowDialog();

                        LoadStationPoint();//重新加载分站图形化界面  20170908
                    }
                    else
                    {
                        MessageBox.Show("该设备无唯一编码，无法修改地址号");    //2017.12.15 by
                    }

                }
                else if (Point.Contains("自动挂接-") && !Point.Contains(":"))//自动挂接设备处理
                {
                    int ChanelNumber = int.Parse(Point.Split('-')[1]);
                    //判断当前自动挂接的设备是否定义
                    List<Jc_DefInfo> stationSonPoint = DEFServiceModel.QueryPointByStationCache(EditStationNow.Point).FindAll(a => (a.Kh >= 1 && a.Kh <= 4 && (a.DevPropertyID == 1 || a.DevPropertyID == 2))
                        || (a.Kh >= 5 && a.Kh <= 16));
                    Jc_DefInfo definedPoint = stationSonPoint.Find(a => a.Kh == ChanelNumber);
                    if (definedPoint != null)
                    {//已存在，则检测设备类型是否匹配并进行修改
                        long DeviceId = 0;
                        int DevPropertyId = 0;
                        List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                        AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                && a.ChanelNumber == ChanelNumber);
                        if (tempAutomaticArticulatedDevice != null)
                        {
                            Jc_DevInfo FzDev = new Jc_DevInfo();
                            FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Bz4 == tempAutomaticArticulatedDevice.DeviceModel);
                            if (FzDev != null)
                            {
                                DeviceId = long.Parse(FzDev.Devid);
                                DevPropertyId = FzDev.Type;
                            }
                        }
                        //修改                           
                        _CFFram.cuNavigation.ShowFormForDev("8", Point, EditStationNow.Point, "基础通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                    else
                    {//新增加
                        long DeviceId = 0;
                        int DevPropertyId = 0;
                        List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                        AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
                && a.ChanelNumber == ChanelNumber);
                        if (tempAutomaticArticulatedDevice != null)
                        {
                            Jc_DevInfo FzDev = new Jc_DevInfo();
                            FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Bz4 == tempAutomaticArticulatedDevice.DeviceModel);
                            if (FzDev != null)
                            {
                                DeviceId = long.Parse(FzDev.Devid);
                                DevPropertyId = FzDev.Type;
                            }
                        }
                        _CFFram.cuNavigation.ShowFormForDev("8", "", EditStationNow.Point, "基础通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                }
                else if (!UnitName.Contains("拓扑定义-分站位置") && !UnitName.Contains("拓扑定义-分站背景") && !UnitName.Contains("拓扑定义-标注"))//静态图元不能编辑
                {
                    int ChanelNumber = int.Parse(Point.Substring(4, 2));
                    int addressNumber = int.Parse(Point.Substring(6, 1));
                    long DeviceId = 0;
                    int DevPropertyId = 0;
                    List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                    AutomaticArticulatedDeviceInfo tempAutomaticArticulatedDevice = automaticArticulatedDeviceInfo.FirstOrDefault(a => a.StationNumber == EditStationNow.Fzh
            && a.ChanelNumber == ChanelNumber);
                    if (tempAutomaticArticulatedDevice != null)
                    {
                        Jc_DevInfo FzDev = new Jc_DevInfo();
                        FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Bz4 == tempAutomaticArticulatedDevice.DeviceModel);
                        if (FzDev != null)
                        {
                            DeviceId = long.Parse(FzDev.Devid);
                            DevPropertyId = FzDev.Type;
                        }
                    }
                    //修改时，赋值选择的通道类型  20171023
                    if (ChanelNumber >= 17 && ChanelNumber <= 24)//智能通道
                    {
                        //修改                           
                        _CFFram.cuNavigation.ShowFormForDev("8", Point, EditStationNow.Point, "智能通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                    else if (Point.Contains("C") && addressNumber == 0)//本地控制通道
                    {
                        //修改                           
                        _CFFram.cuNavigation.ShowFormForDev("8", Point, EditStationNow.Point, "控制通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                    else//基础通道
                    {
                        //修改                           
                        _CFFram.cuNavigation.ShowFormForDev("8", Point, EditStationNow.Point, "基础通道," + ChanelNumber.ToString() + "," + DevPropertyId + "," + DeviceId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("测点编辑失败，详细请查看错误日志！");
                LogHelper.Error("mx_OnViewCallOutCommand_PointEdit" + ex.Message + ex.StackTrace);
            }
        }

        #endregion

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frm_CheckArea fca = new Frm_CheckArea();
            fca.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frm_AreaDefine fad = new Frm_AreaDefine("1111,2222,3333,4444", 0);
            fad.ShowDialog();
        }
        /// <summary>
        /// 刷新实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_CFFram.cuNavigation.CtreeLisStation.FocusedNode == null)
            {
                _CFFram.cuNavigation.CtreeLisStation.ContextMenu = null;
                return;
            }
            TreeListItem DataRecord = (TreeListItem)_CFFram.cuNavigation.CtreeLisStation.GetDataRecordByNode(_CFFram.cuNavigation.CtreeLisStation.FocusedNode);

            if (DataRecord == null)
            {
                return;
            }
            _CFFram.cuNavigation.RefreshGridInDev(DataRecord.Tag, DataRecord.Code);
        }
    }
}
