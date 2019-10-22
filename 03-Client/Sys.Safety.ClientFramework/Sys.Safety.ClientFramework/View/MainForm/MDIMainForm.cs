using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading.Tasks;
using DevExpress.XtraBars;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using System.Collections;
using Sys.Safety.ClientFramework.View.LogOn;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Utils;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.ClientFramework.Configuration;
using System.Reflection;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Setting;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.Request;


namespace Sys.Safety.ClientFramework.View.MainForm
{
    public partial class MDIMainForm : RibbonForm
    {
        #region ====================窗体属性====================
        /// <summary>
        /// 菜单名称
        /// </summary>
        private string menuname = "";
        /// <summary>
        /// 用户登录
        /// </summary>
        private frmLogOn userLog = null;

        /// <summary>窗体图片属性</summary>
        private WindowSkinPic _skinPic;

        protected static DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;

        private ParallelLoopResult paraResult;

        /// <summary>动态菜单管理器</summary>
        private UserMeun _menuManager;

        /// <summary>用户菜单列表</summary>
        private List<MenuInfo> menuLst;

        /// <summary>菜单</summary>
        private ToolStripColorTable _colorTable;

        /// <summary>鼠标左键是否按下</summary>
        private bool _isLeftBtnDown;
        /// <summary>
        /// 系统退出标记
        /// </summary>
        private bool isSysEixt = false;

        /// <summary>
        /// 等待窗口
        /// </summary>
        private DevExpress.Utils.WaitDialogForm wdf = null;

        /// <summary>
        /// 关闭主控应用程序线程
        /// </summary>
        public System.Threading.Thread m_CloseThread;
        /// <summary>
        /// 刷新时间线程
        /// </summary>
        public System.Threading.Thread m_RefTimeThread;
        /// <summary>
        /// 刷新服务端、网关状态线程
        /// </summary>
        public System.Threading.Thread m_RefStateThread;
        /// <summary>
        /// 测试线程
        /// </summary>
        public Thread testThread;
        bool _isRun = false;

        IUserService _UserService = ServiceFactory.Create<IUserService>();
        IMenuService _MenuService = ServiceFactory.Create<IMenuService>();
        IRequestService requestService = ServiceFactory.Create<IRequestService>();
        IConfigService _configService = ServiceFactory.Create<IConfigService>();
        IRemoteStateService _RemoteStateService = ServiceFactory.Create<IRemoteStateService>();
        ISettingService _settingService = ServiceFactory.Create<ISettingService>();
        //Basic.Framework.Utils.Log.LogDelete logdelete = null;注释客户端日志清理线程功能   20170525

        //调用API置顶窗口
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow(); //获得本窗体的句柄
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
        //定义变量,句柄类型
        public IntPtr Handle1;
        #endregion

        #region ====================构造函数、程序退出处理====================
        public MDIMainForm()
        {
            try
            {
                //写系统启动日志
                OperateLogHelper.InsertOperateLog(12, "启动客户端...", "");

                #region wcf检查服务端是否正常运行
                if (ConfigurationManager.AppSettings["ServiceType"].ToString() == "wcf")
                {
                    try
                    {
                        _UserService.GetUserList();//调用服务端接口，看能否正常调用来判断服务端是否开启
                    }
                    catch
                    {
                        MessageBox.Show("连接服务端异常，请检查服务器IP端口是否配置正确！");

                        //WcfManage.WcfManage wcfmag = new WcfManage.WcfManage();
                        //wcfmag.ShowDialog();
                        //退出应用程序
                        System.Environment.Exit(0);
                    }
                }
                #endregion

                //初始化窗体成员
                InitMainFormVaribleFront();

                InitializeComponent();

                //初始化配置信息
                Sys.Safety.ClientFramework.Configuration.BaseConfig.GetSetting();

                //用户登录并根据登录结果加载菜单
                InitConfigAndSysMenu();

                //初始化窗体成员之后
                InitMainFormVaribleBack();

                //写系统启动日志
                OperateLogHelper.InsertOperateLog(12, "启动客户端成功", "");

                //测试程序
                //testThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.RefFormTest));
                //testThread.IsBackground = true;
                //testThread.Priority = ThreadPriority.Normal;
                //testThread.Start();

                //启动日志清理线程 //注释客户端日志清理线程功能   20170525
                //logdelete = new Framework.Utils.Log.LogDelete();
                //logdelete.Start();

                //开启刷新时间线程 
                _isRun = true;
                m_RefTimeThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.RefTime));
                m_RefTimeThread.IsBackground = true;
                m_RefTimeThread.Priority = ThreadPriority.Normal;
                m_RefTimeThread.Start();

                m_RefStateThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.RefState));
                m_RefStateThread.IsBackground = true;
                m_RefStateThread.Priority = ThreadPriority.Normal;
                m_RefStateThread.Start();

                //20170907 added by  定时检测数据库磁盘处理
                new Thread(new ThreadStart(RefreshDatabaseDiskInfo)) { IsBackground = true }.Start();

            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_MDIMainForm" + ex.Message + ex.StackTrace);
            }
        }
        List<Form> formObjects = new List<Form>();
        /// <summary>
        /// 窗体反射测试
        /// </summary>
        private void RefFormTest()
        {


            IRequestService requestService = ServiceFactory.Create<IRequestService>();
            var result = requestService.GetRequestList().Data;
            while (true)
            {
                try
                {


                    foreach (RequestInfo dto in result)
                    {
                        string fileName = Application.StartupPath + "\\" + dto.MenuFile;//dll文件
                        string UrlName = Convert.ToString(dto.MenuNamespace) + "." + Convert.ToString(dto.MenuURL); //类名称   
                        Form TempForm = null;
                        try
                        {
                            if (dto.MenuParams.Contains("=$"))
                            {
                                continue;
                            }
                            #region 加载请求库的参数信息
                            Dictionary<string, string> param = null;
                            if (!string.IsNullOrEmpty(dto.MenuParams))
                            {
                                if (Convert.ToString(dto.MenuParams).Contains("&"))
                                {
                                    string[] ModuleParams = Convert.ToString(dto.MenuParams).Split('&');

                                    param = new Dictionary<string, string>();

                                    for (int i = 0; i < ModuleParams.Length; i++)
                                    {
                                        if (ModuleParams[i].Split('=').Length > 0)
                                        {
                                            param.Add(ModuleParams[i].Split('=')[0], ModuleParams[i].Split('=')[1]);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dto.MenuParams)))
                                    {
                                        string[] ModuleParams = new string[1];//参数传递

                                        param = new Dictionary<string, string>();

                                        ModuleParams[0] = dto.MenuParams;

                                        param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                                    }
                                }
                            }
                            #endregion
                            object[] obj = null;//参数
                            if (param != null && param.Count > 0)
                            {
                                obj = new object[1];
                                obj[0] = param;
                            }

                            if (dto.MenuFile.Contains("Sys.Safety.Client.Control") || dto.MenuFile.Contains("Sys.Safety.ClientFramework") || dto.MenuFile.Contains("Sys.Safety.Client.Graphic"))
                            {
                                continue;
                            }
                            if (!dto.MenuFile.Contains("Sys.Safety.Reports"))
                            {
                                continue;
                            }
                            object Temp_Obj = null;
                            bool isload = false;
                            Temp_Obj = AssemblyManager.CreateInstance(fileName,
                                 UrlName, obj, ref isload);

                            TempForm = Temp_Obj as Form;

                            TempForm.FormClosed += new FormClosedEventHandler(TempForm_FormClosed);

                            if (!formObjects.Contains(TempForm))
                            {
                                formObjects.Add(TempForm);
                            }

                            TempForm.Show();

                            if (TempForm != null)
                            {
                                TempForm.Close();
                            }
                        }
                        catch
                        { }
                        //Thread.Sleep(5000);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }
        public void TempForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form temp = (Form)sender;
                //注销内存对象
                for (int j = formObjects.Count - 1; j >= 0; j--)
                {
                    if (((Form)(formObjects[j])).Text == temp.Text || string.IsNullOrEmpty(((Form)(formObjects[j])).Text))
                    {
                        //移除对象、释放资源
                        ((Form)(formObjects[j])).Dispose();
                        formObjects[j] = null;
                        formObjects.RemoveAt(j);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void MDIMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //检查服务端是否正常运行
            try
            {
                _UserService.GetUserList();//调用服务端接口，看能否正常调用来判断服务端是否开启
            }
            catch
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("连接服务端异常，即将退出应用程序！");
                System.Environment.Exit(0);
            }

            try
            {
                userLog = null;
                isSysEixt = true;
                if (null == userLog)
                {
                    userLog = new frmLogOn();

                    //userLog.ShowInTaskbar = false;

                    userLog.ShowDialog();
                }
                if (!userLog.isLogOk)//没有在登录窗口进行了登录验证，直接退出
                {
                    e.Cancel = true;
                    isSysEixt = false;
                }
                else
                {
                    if (LoginManager.IsLogin)//是否登录成功
                    {
                        e.Cancel = false;
                        //通知调用的应用程序，主控即将退出
                        RequestUtil.OnMainFormCloseing();
                        //关闭所有打开的子窗口
                        foreach (XtraTabPage xtp in xtTabCtrl.TabPages)
                        {
                            for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                            {
                                if (((Form)(RequestUtil.formObjects[j])).Text == xtp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                                {
                                    ((Form)(RequestUtil.formObjects[j])).Close();
                                }
                            }
                        }
                        //退出操作日志异步入库线程 
                        OperateLogHelper.Stop();
                        //退出日志清理线程//注释清理日志线程  20170525
                        //logdelete.Stop();
                        //关闭刷新时间线程
                        _isRun = false;
                    }
                    else
                    {
                        e.Cancel = true;
                        isSysEixt = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_MDIMainForm_FormClosing" + ex.Message + ex.StackTrace);
            }
        }
        private void MDIMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //应用关闭线程
            m_CloseThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThisClose));
            m_CloseThread.Priority = ThreadPriority.Normal;
            m_CloseThread.Start();
        }
        /// <summary>
        /// 关闭应用程序线程
        /// </summary>
        public void ThisClose()
        {
            wdf = new WaitDialogForm("正在退出系统...", "请等待...");
            Thread.Sleep(1000);//延时等待子窗体线程退出
            if (wdf != null)
            {
                wdf.Close();
            }
            //写系统退出日志
            OperateLogHelper.InsertOperateLog(13, "退出客户端成功", "");

            //退出应用程序
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public void RefTime()
        {
            //IDatetimeService datetimeService = null;
            string DatetimeStr = "";

            while (_isRun)
            {
                try
                {
                    //if (datetimeService == null)
                    //{
                    //    datetimeService = ServiceFactory.CreateService<IDatetimeService>();//重新创建对象
                    //}
                    //DatetimeStr = datetimeService.getServiceTime();
                    DatetimeStr = DateTime.Now.ToString();
                    ClientItem _ClientItem = null;
                    if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                    {
                        _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                    }
                    if (_ClientItem != null)
                    {
                        if (string.IsNullOrEmpty(_ClientItem.UserName))
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                barStaticItem2.Caption = string.Format("  【未登录】  " + DatetimeStr);
                            }));
                        }
                        else
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                barStaticItem2.Caption = string.Format("  用户名：{0}  " + DatetimeStr, _ClientItem.UserName);
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("MDIMainForm_RefTime" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(1000);//延时等待
            }
        }
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string IniReadValue(string Section, string Key, string filepath)
        {
            StringBuilder retVal = new StringBuilder(0x800);
            try
            {
                int num = GetPrivateProfileString(Section, Key, "", retVal, 0x800, filepath);
            }
            catch
            {

            }
            return retVal.ToString();
        }
        /// <summary>
        /// 刷新服务端、网关状态
        /// </summary>
        public void RefState()
        {

            Image Imagenormal = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\apply_16x16.png");
            Image Imageabnormal = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\cancel_16x16.png");
            int LoseCount = 0, LoseCount1 = 0;

            DataTable dt = new DataTable();
            DateTime ServerConnetInrTime = DateTime.Now;
            string isShowFlag="";
            while (_isRun)
            {
                try
                {

                    //网关的状态
                    bool DataCollectorState = _RemoteStateService.GetGatewayState().Data;

                    bool dbState = _configService.GetDbState().Data;

                    RunningInfo runinfo = _configService.GetRunningInfo().Data;

                    if (!string.IsNullOrEmpty(runinfo.CustomerInfo))
                    {
                        if (!barStaticItem10.Caption.Contains("[授权" + runinfo.CustomerInfo + "使用]"))
                        {
                            barStaticItem10.Caption = "[授权" + runinfo.CustomerInfo + "使用]";
                            if (runinfo.AuthorizationExpires)
                            {
                                barStaticItem10.Caption += "[授权到期]";
                            }
                        }
                    }
                    else
                    {
                        if (!barStaticItem10.Caption.Contains("[未授权用户]"))
                        {
                            barStaticItem10.Caption = "[未授权用户]";
                        }
                    }

                    ServerConnetInrTime = DateTime.Now;

                    LoseCount = 0;
                    this.BeginInvoke(new Action(() =>
                 {
                     barStaticItem4.Glyph = Imagenormal;//能够正常调用服务端方法，表示服务端正常  20170323
                 }));

                    //获取当前服务器是在主机运行还是在备机运行  20171109
                    if (runinfo != null)
                    {
                        try { isShowFlag = ConfigurationManager.AppSettings["ShowZbInfo"].ToString(); }
                        catch
                        {

                        }
                        if (isShowFlag == "") isShowFlag = "0";

                        if (isShowFlag == "0")
                        {
                            barStaticItem12.Visibility =  BarItemVisibility.Always;
                            barStaticItem13.Visibility = BarItemVisibility.Always;

                            if (runinfo.IsUseHA)
                            {
                                //增加获取双机热备工作状态   20180123
                                switch (runinfo.BackUpWorkState)
                                {
                                    case -1:
                                        this.BeginInvoke(new Action(() =>
                                        {
                                            barStaticItem12.Glyph = Imageabnormal;
                                            barStaticItem13.Glyph = Imageabnormal;
                                        }));
                                        barStaticItem12.Caption = "主:未知";
                                        barStaticItem13.Caption = "备:未知";
                                        break;
                                    case 0:
                                        this.BeginInvoke(new Action(() =>
                                        {
                                            barStaticItem12.Glyph = Imagenormal;
                                            barStaticItem13.Glyph = Imagenormal;
                                        }));
                                        barStaticItem12.Caption = "主:正常";
                                        barStaticItem13.Caption = "备:正常";
                                        break;
                                    case 1:
                                        if (runinfo.IsMasterOrBackup == 1)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imagenormal;
                                                barStaticItem13.Glyph = Imageabnormal;
                                            }));
                                            barStaticItem12.Caption = "主:正常";
                                            barStaticItem13.Caption = "备:网络中断";
                                        }
                                        else if (runinfo.IsMasterOrBackup == 2)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imageabnormal;
                                                barStaticItem13.Glyph = Imagenormal;
                                            }));
                                            barStaticItem12.Caption = "主:网络中断";
                                            barStaticItem13.Caption = "备:正常";
                                        }
                                        break;
                                    case 2:
                                        // this.BeginInvoke(new Action(() =>
                                        //{
                                        //    barStaticItem12.Glyph = Imageabnormal;
                                        //    barStaticItem13.Glyph = Imageabnormal;
                                        //}));
                                        // barStaticItem12.Caption = "主:连接中断";
                                        // barStaticItem13.Caption = "备:连接中断";                                  
                                        if (runinfo.IsMasterOrBackup == 1)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imagenormal;
                                                barStaticItem13.Glyph = Imageabnormal;
                                            }));
                                            barStaticItem12.Caption = "主:正常";
                                            barStaticItem13.Caption = "备:连接中断";
                                        }
                                        else if (runinfo.IsMasterOrBackup == 2)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imageabnormal;
                                                barStaticItem13.Glyph = Imagenormal;
                                            }));
                                            barStaticItem12.Caption = "主:连接中断";
                                            barStaticItem13.Caption = "备:正常";
                                        }
                                        break;
                                    case 3:
                                        this.BeginInvoke(new Action(() =>
                                       {
                                           barStaticItem12.Glyph = Imagenormal;
                                           barStaticItem13.Glyph = Imagenormal;
                                       }));
                                        barStaticItem12.Caption = "主:正常";
                                        barStaticItem13.Caption = "备:正常";
                                        break;
                                    case 4:
                                        this.BeginInvoke(new Action(() =>
                                       {
                                           barStaticItem12.Glyph = Imagenormal;
                                           barStaticItem13.Glyph = Imagenormal;
                                       }));
                                        barStaticItem12.Caption = "主:正常";
                                        barStaticItem13.Caption = "备:正常";
                                        break;
                                    case 5:
                                        this.BeginInvoke(new Action(() =>
                                       {
                                           barStaticItem12.Glyph = Imagenormal;
                                           barStaticItem13.Glyph = Imagenormal;
                                       }));
                                        barStaticItem12.Caption = "主:正常";
                                        barStaticItem13.Caption = "备:正常";
                                        break;
                                    case 6:
                                        this.BeginInvoke(new Action(() =>
                                       {
                                           barStaticItem12.Glyph = Imagenormal;
                                           barStaticItem13.Glyph = Imagenormal;
                                       }));
                                        barStaticItem12.Caption = "主:正常";
                                        barStaticItem13.Caption = "备:正常";
                                        break;
                                    case 7:
                                        this.BeginInvoke(new Action(() =>
                                      {
                                          barStaticItem12.Glyph = Imageabnormal;
                                          barStaticItem13.Glyph = Imageabnormal;
                                      }));
                                        barStaticItem12.Caption = "主:同时运行";
                                        barStaticItem13.Caption = "备:运行中";
                                        break;
                                    case 8:
                                        //  this.BeginInvoke(new Action(() =>
                                        //{
                                        //    barStaticItem12.Glyph = Imageabnormal;
                                        //    barStaticItem13.Glyph = Imageabnormal;
                                        //}));
                                        //  barStaticItem12.Caption = "主:网卡异常";
                                        //  barStaticItem13.Caption = "备:网络中断";
                                        if (runinfo.IsMasterOrBackup == 1)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imagenormal;
                                                barStaticItem13.Glyph = Imageabnormal;
                                            }));
                                            barStaticItem12.Caption = "主:正常";
                                            barStaticItem13.Caption = "备:网络设备异常";
                                        }
                                        else if (runinfo.IsMasterOrBackup == 2)
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {
                                                barStaticItem12.Glyph = Imageabnormal;
                                                barStaticItem13.Glyph = Imagenormal;
                                            }));
                                            barStaticItem12.Caption = "主:网络设备异常";
                                            barStaticItem13.Caption = "备:正常";
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                this.BeginInvoke(new Action(() =>
                                {
                                    barStaticItem12.Glyph = Imageabnormal;
                                    barStaticItem13.Glyph = Imageabnormal;
                                }));
                                barStaticItem12.Caption = "主:未启用";
                                barStaticItem13.Caption = "备:未启用";
                            }
                        }
                        else
                        {//修改关于主备显示，只显示当前是主机还是备机。

                            barStaticItem12.Visibility = BarItemVisibility.Always;
                            barStaticItem13.Visibility = BarItemVisibility.Never;
                            isShowFlag = IniReadValue("Backupdb", "BackupZbj", Application.StartupPath.Substring(0, Application.StartupPath.Length - 7) + "\\HA\\BackConfig.ini");
                            if (isShowFlag == "1")
                                barStaticItem12.Caption = "主备状态:主机";
                            else
                                barStaticItem12.Caption = "主备状态:备机";
                        }

                        try
                        {
                            if (runinfo.IsMasterOrBackup == 1)
                            {
                                barStaticItem4.Caption = "服务器:主机";
                                barStaticItem5.Caption = "采集端:主机";
                            }
                            else if (runinfo.IsMasterOrBackup == 2)
                            {
                                barStaticItem4.Caption = "服务器:备机";
                                barStaticItem5.Caption = "采集端:备机";
                            }
                            else
                            {
                                barStaticItem4.Caption = "服务器:双机未运行";
                                barStaticItem5.Caption = "采集端:双机未运行";
                            }
                        }
                        catch
                        {
                            barStaticItem4.Caption = "服务器:双机未运行";
                            barStaticItem5.Caption = "采集端:双机未运行";
                        }
                    }
                    else
                    {
                        barStaticItem4.Caption = "服务器:双机未运行";
                        barStaticItem5.Caption = "采集端:双机未运行";
                    }

                    if (!DataCollectorState)//中断
                    {
                        this.BeginInvoke(new Action(() =>
                  {
                      barStaticItem5.Glyph = Imageabnormal;
                  }));
                    }
                    else//正常
                    {
                        this.BeginInvoke(new Action(() =>
                 {
                     barStaticItem5.Glyph = Imagenormal;
                 }));
                    }

                    //数据库状态                   
                    if (!dbState)//中断
                    {
                        this.BeginInvoke(new Action(() =>
                 {
                     barStaticItem6.Glyph = Imageabnormal;
                 }));
                    }
                    else//正常
                    {
                        this.BeginInvoke(new Action(() =>
                 {
                     barStaticItem6.Glyph = Imagenormal;
                 }));
                    }
                    PorcessInfo processInfo = GetProcessInfo("Sys.Safety.Client.WindowHost");
                    if (processInfo != null)
                    {
                        if (processInfo.MemoryUsageSize > 1448)//超过2G则提示用户重启
                        {
                            if (XtraMessageBox.Show("客户端已连续长时间运行，资源即将达到上限，建议您重新启动来提高操作效率,重启不会影响与井下通讯，是否继续？",
                                "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                this.Close();
                            }
                        }
                    }

                    //获取巡检时间
                    var result = _RemoteStateService.GetInspectionTime(new GetInspectionTimeRequest());
                    if (result.IsSuccess)
                    {
                        barStaticItem8.Caption = "巡检 " + result.Data + " ms";
                    }
                    else
                    {
                        barStaticItem8.Caption = "";
                    }
                    //获取安全生产时间               
                    GetSettingByKeyRequest request1 = new GetSettingByKeyRequest();
                    request1.StrKey = "SaveDate";
                    var result1 = _settingService.GetSettingByKey(request1);
                    if (result1.IsSuccess && result1.Data != null)
                    {
                        barStaticItem9.Caption = "安全生产 " + (int)(DateTime.Now - Convert.ToDateTime(result1.Data.StrValue)).TotalDays + " 天";
                    }
                    else
                    {
                        barStaticItem9.Caption = "";
                    }
                }
                catch (Exception ex)
                {
                    //if (LoseCount <= 2)//服务端3次连接不上，认为中断
                    //{
                    //    LoseCount++;
                    //}
                    LoseCount = (int)(DateTime.Now - ServerConnetInrTime).TotalSeconds;
                    //连接服务端异常
                    if (LoseCount > 15)//服务端3次连接不上，认为中断，将所有状态置成中断   20170323
                    {
                        this.BeginInvoke(new Action(() =>
                 {
                     barStaticItem4.Glyph = Imageabnormal;
                     barStaticItem5.Glyph = Imageabnormal;
                     barStaticItem6.Glyph = Imageabnormal;
                 }));
                        barStaticItem8.Caption = "";
                    }
                }
                Thread.Sleep(5000);//延时等待
            }
        }


        public PorcessInfo GetProcessInfo(string processName)
        {
            PorcessInfo result = null;

            var processList = Process.GetProcessesByName(processName);
            if (processList.Length <= 0)
            {
                return result;
            }

            result = new PorcessInfo();
            Process cur = processList[0];

            PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
            PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

            const int KB_DIV = 1024;
            const int MB_DIV = 1024 * 1024;
            const int GB_DIV = 1024 * 1024 * 1024;

            // Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024, curtime.NextValue() / Environment.ProcessorCount);
            result.ProcessName = processName;
            result.MemoryUsageSize = Math.Round((decimal)curpcp.NextValue() / MB_DIV, 2);
            result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);

            //added by  20170318 有时获取CPU为0，这里增加5次循环，尽量让CPU使用率不为0
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                if (result.CpuUsageRate <= 0)
                {
                    result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 定时检测数据库磁盘占用信息
        /// </summary>
        public void RefreshDatabaseDiskInfo()
        {
            IConfigService configService = ServiceFactory.Create<IConfigService>();

            //客户端程序启动延后一分钟启动检测模块
            Thread.Sleep(1000 * 60);

            //如果为10点，则在循环外不检测
            if (DateTime.Now.Hour != 10)
            {
                try
                {
                    //获取数据库磁盘信息
                    var response = configService.GetDatabaseDiskInfo();
                    if (response.IsSuccess)
                    {
                        CheckDatabaseDiskInfo(response.Data);
                        Thread.Sleep(1000 * 60 * 60);//如果启动时间不是10点，检测后，则休眠一小时
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("MDIMainForm_RefreshDatabaseDiskInfo:" + ex.Message + ex.StackTrace);
                }
            }

            while (_isRun)
            {
                try
                {
                    //每天时间检测一次
                    if (DateTime.Now.Hour == 10)
                    {
                        var response = configService.GetDatabaseDiskInfo();
                        if (response.IsSuccess)
                        {
                            CheckDatabaseDiskInfo(response.Data);
                        }
                        else
                        {
                            LogHelper.Error("configService.GetDatabaseDiskInfo:" + response.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("MDIMainForm_RefreshDatabaseDiskInfo:" + ex.Message + ex.StackTrace);
                }
                finally
                {
                    Thread.Sleep(1000 * 60 * 60);//休眠一小时
                }
            }
        }

        private void CheckDatabaseDiskInfo(HardDiskInfo diskinfo)
        {
            if (diskinfo.TotalUsageRate < 90)
            {
                return;
            }

            this.BeginInvoke(new Action(() =>
            {
                new frmDiskAlert().Show();
            }));
        }





        //protected override void WndProc(ref System.Windows.Forms.Message msg)
        //{
        //    //if (msg.Msg == 0x401)
        //    //{
        //    //    DevExpress.XtraEditors.XtraMessageBox.Show("ss");
        //    //    return;
        //    //}
        //    base.WndProc(ref msg);
        //}
        #endregion

        #region ====================私有函数====================
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDIMainForm_Load(object sender, EventArgs e)
        {
            //设置窗体高度和宽度
            this.Width = Screen.GetWorkingArea(this).Width;
            this.Height = Screen.GetWorkingArea(this).Height;
            this.Left = 0;
            this.Top = 0;
        }
        /// <summary>
        /// 关闭菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtTabCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //Point point = new Point(e.X, e.Y);
                if (e.Button == MouseButtons.Right)
                {
                    DevExpress.XtraTab.ViewInfo.BaseTabHitInfo hInfo = ((XtraTabControl)sender).CalcHitInfo(e.Location);
                    //右键点击位置：在Page上且不在关闭按钮内                   
                    menuname = hInfo.Page.Text;
                    if (hInfo.IsValid && hInfo.Page != null)
                    {
                        this.popupMenu1.ShowPopup(Control.MousePosition);//在鼠标位置弹出，而不是e.Location
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_xtTabCtrl_MouseDown" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 全部关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barAllClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //关闭对象
                foreach (XtraTabPage xtp in xtTabCtrl.TabPages)
                {
                    if (xtp.ShowCloseButton == DevExpress.Utils.DefaultBoolean.False)
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                        {
                            if (((Form)(RequestUtil.formObjects[j])).Text == xtp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                            {
                                ((Form)(RequestUtil.formObjects[j])).Close();
                            }
                        }
                    }
                }
                //关闭所有page
                for (int i = 0; i < xtTabCtrl.TabPages.Count; i++)
                {
                    if (xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.True ||
                        xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.Default)
                    {
                        xtTabCtrl.TabPages.RemoveAt(i);
                        i--;
                    }
                }
                //for (int i = 0; i < mainMenuStrip.Items.Count; i++)
                //{
                //    ToolStripMenuItem windowItem = (ToolStripMenuItem)mainMenuStrip.Items[i];
                //    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                //    if (windowItem.Text == "窗口")
                //    {
                //for (int j = 0; j < RequestUtil.formObjects.Count; j++)
                //{
                //    RequestUtil.formObjects.RemoveAt(j);
                //    //windowItem.DropDownItems.RemoveAt(j);
                //    j--;
                //}
                //        break;
                //    }
                //}
                //
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_barAllClose_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barClose_ItemClick(object sender, ItemClickEventArgs e)
        {

            try
            {
                string tabpagename = menuname;
                bool isClose = false;
                //for (int i = 0; i < mainMenuStrip.Items.Count; i++)
                //{
                //    ToolStripMenuItem windowItem = (ToolStripMenuItem)mainMenuStrip.Items[i];
                //    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                //    if (windowItem.Text == "窗口")
                //    {
                //for (int j = 0; j < RequestUtil.formObjects.Count; j++)
                //{
                //    if (((Form)RequestUtil.formObjects[j]).Text == tabpagename)
                //    {
                //        RequestUtil.formObjects.RemoveAt(j);
                //        //windowItem.DropDownItems.RemoveAt(j);
                //        menuname = "";
                //        xtTabCtrl.TabPages.RemoveAt(j + 1);
                //        this.xtTabCtrl.SelectedTabPage = this.xtTabCtrl.TabPages[xtTabCtrl.TabPages.Count - 1];
                //        break;
                //    }
                //}
                //        break;
                //    }
                //}     

                //关闭对象
                foreach (XtraTabPage xtp in xtTabCtrl.TabPages)
                {
                    if (xtp.ShowCloseButton == DevExpress.Utils.DefaultBoolean.False)
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                        {
                            if (((Form)(RequestUtil.formObjects[j])).Text == xtp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                            {
                                if (((Form)RequestUtil.formObjects[j]).Text == tabpagename)
                                {
                                    ((Form)(RequestUtil.formObjects[j])).Close();
                                }
                            }
                        }
                    }
                }
                //关闭当前page
                for (int i = 0; i < xtTabCtrl.TabPages.Count; i++)
                {
                    if (xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.True ||
                        xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.Default)
                    {
                        if (xtTabCtrl.TabPages[i].Text == tabpagename)
                        {
                            xtTabCtrl.TabPages.RemoveAt(i);
                            isClose = true;
                            i--;
                        }
                    }
                }
                if (isClose)
                {
                    this.xtTabCtrl.SelectedTabPage = this.xtTabCtrl.TabPages[xtTabCtrl.TabPages.Count - 1];
                    //
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_barClose_ItemClick" + ex.Message + ex.StackTrace);
            }

        }
        /// <summary>
        /// 当登录发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginManager_LogOnChangeEvent(object sender, EventArgs e)
        {
            try
            {
                if (!isSysEixt && LoginManager.LoginSuccessIsLoadMenu)
                {
                    IDictionary<string, MenuInfo> dic = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, MenuInfo>>(Basic.Framework.Data.PlatRuntime.Items["_Menus"].ToString());

                    menuLst = new List<MenuInfo>();

                    foreach (KeyValuePair<string, MenuInfo> kvp in dic)
                    {
                        menuLst.Add(kvp.Value);
                    }

                    //menuLst = ClientContext.Current.GetContext("_Menus") as List<MenuInfo>;

                    _menuManager.ChangeDevMainMenuItems(this.ribbonControl1, menuLst);



                    //关闭对象
                    foreach (XtraTabPage xtp in xtTabCtrl.TabPages)
                    {
                        if (xtp.ShowCloseButton == DevExpress.Utils.DefaultBoolean.False)
                        {
                            continue;
                        }
                        else
                        {
                            for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                            {
                                if (((Form)(RequestUtil.formObjects[j])).Text == xtp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                                {
                                    ((Form)(RequestUtil.formObjects[j])).Close();
                                }
                            }
                        }
                    }
                    for (int i = 0; i < xtTabCtrl.TabPages.Count; i++)
                    {
                        if (xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.True ||
                             xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.Default)
                        {
                            xtTabCtrl.TabPages.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_LoginManager_LogOnChangeEvent" + ex.Message + ex.StackTrace);
            }
            finally
            {
                //LockWindowUpdate(IntPtr.Zero);
            }
        }
        /// <summary>
        /// 将生成的窗体加入主框架中
        /// </summary>
        /// <param name="frm"></param>
        private void ShowToMainForm(Form frm, RequestInfo dto, bool isSystemDesktop)
        {
            try
            {
                int i, j, k;
                if (frm != null)
                {
                    Form childForm = (Form)frm;

                    //添加对内存缓存对象中
                    if (!RequestUtil.formObjects.Contains(childForm))
                    {
                        RequestUtil.formObjects.Add(childForm);
                    }
                    if (dto.ShowType == 0)
                    {
                        if (xtTabCtrl.TabPages.Count <= 20)
                        {
                            #region 窗体嵌入主控中显示
                            if (dto.LoadByIframe == 0)
                            {

                                XtraTabPage page = new XtraTabPage();


                                childForm.Dock = DockStyle.Fill;
                                childForm.FormBorderStyle = FormBorderStyle.None;
                                childForm.TopLevel = false;
                                childForm.Visible = true;
                                if (isSystemDesktop)
                                {
                                    page.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
                                }
                                else
                                {
                                    page.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
                                }

                                //先判断窗口是否存在，如果存在，则跳转到窗口
                                for (i = 0; i < xtTabCtrl.TabPages.Count; i++)
                                {
                                    if (xtTabCtrl.TabPages[i].Text == frm.Text)
                                    {
                                        xtTabCtrl.TabPages[i].Show();
                                        return;
                                    }
                                }

                                page.Text = childForm.Text.ToString();
                                xtTabCtrl.TabPages.Add(page);
                                xtTabCtrl.SelectedTabPage = page;
                                xtTabCtrl.CloseButtonClick += new EventHandler(XtraTabControl1CloseButtonClick);

                                page.Controls.Add(childForm);
                                //缓冲当前打开的窗体对象 
                                //for (i = 0; i < mainMenuStrip.Items.Count; i++)
                                //{
                                //    ToolStripMenuItem windowItem = (ToolStripMenuItem)mainMenuStrip.Items[i];
                                //    //if (windowItem.Text == "窗口")
                                //    //{
                                //    if (childForm.Text != "登录")
                                //    {
                                //        for (j = 0; j < mainMenuStrip.Items.Count; j++)
                                //        {
                                //            ToolStripMenuItem menuItem = (ToolStripMenuItem)mainMenuStrip.Items[j];
                                //            if (menuItem != null)
                                //            {
                                //                for (k = 0; k < menuItem.DropDownItems.Count; k++)
                                //                {
                                //                    ToolStripMenuItem menuItemNew = (ToolStripMenuItem)menuItem.DropDownItems[k];
                                //                    if (menuItemNew.Tag != null)
                                //                    {
                                //                        if (menuItemNew.Tag as RequestDTO != null)
                                //                        {
                                //                            if ((menuItemNew.Tag as RequestDTO).RequestCode == dto.RequestCode)
                                //                            {
                                //                                if (!formObjects.Contains(childForm))
                                //                                {
                                //                                    formObjects.Add(childForm);
                                //                                }
                                //                                AddWindowMenu(childForm);
                                //                            }
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //    //}
                                //}

                                //AddWindowMenu(childForm);
                            }
                            else
                            {
                                XtraTabPage page = new XtraTabPage();

                                childForm.Dock = DockStyle.Fill;
                                childForm.FormBorderStyle = FormBorderStyle.None;
                                childForm.TopLevel = false;
                                page.Controls.Add(childForm);

                                childForm.Visible = true;
                                if (isSystemDesktop)
                                {
                                    page.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
                                }
                                else
                                {
                                    page.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
                                }
                                page.Text = childForm.Text.ToString();
                                xtTabCtrl.TabPages.Add(page);
                                xtTabCtrl.SelectedTabPage = page;
                                xtTabCtrl.CloseButtonClick += new EventHandler(XtraTabControl1CloseButtonClick);


                                //缓冲当前打开的窗体对象  
                                //for (i = 0; i < mainMenuStrip.Items.Count; i++)
                                //{
                                //    ToolStripMenuItem windowItem = (ToolStripMenuItem)mainMenuStrip.Items[i];
                                //    //if (windowItem.Text == "窗口")
                                //    //{
                                //    if (childForm.Text != "登录")
                                //    {
                                //        for (j = 0; j < mainMenuStrip.Items.Count; j++)
                                //        {
                                //            ToolStripMenuItem menuItem = (ToolStripMenuItem)mainMenuStrip.Items[j];
                                //            if (menuItem != null)
                                //            {
                                //                for (k = 0; k < menuItem.DropDownItems.Count; k++)
                                //                {
                                //                    ToolStripMenuItem menuItemNew = (ToolStripMenuItem)menuItem.DropDownItems[k];
                                //                    if (menuItemNew.Tag != null)
                                //                    {
                                //                        if (menuItemNew.Tag as RequestDTO != null)
                                //                        {
                                //                            if ((menuItemNew.Tag as RequestDTO).RequestCode == dto.RequestCode)
                                //                            {
                                //                                if (!formObjects.Contains(childForm))
                                //                                {
                                //                                    formObjects.Add(childForm);
                                //                                }
                                //                                AddWindowMenu(childForm);
                                //                            }
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //    //}
                                //}

                                //AddWindowMenu(childForm);
                            }
                            #endregion
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("子窗口打开数量最多为20");
                        }
                    }
                    else if (dto.ShowType == 1)
                    {
                        #region 模态窗体


                        childForm.TopLevel = true;
                        childForm.ShowInTaskbar = true;
                        childForm.StartPosition = FormStartPosition.CenterScreen;
                        if (childForm.Visible)
                        {
                            childForm.Visible = false;
                        }
                        childForm.ShowDialog();

                        #endregion
                    }
                    else if (dto.ShowType == 2)
                    {
                        #region 非模态窗体
                        if (dto.LoadByIframe == 0)
                        {
                            foreach (XtraTabPage childrenForm in this.xtTabCtrl.TabPages)
                            {
                                if (childrenForm.Text == frm.Text)
                                {
                                    return;
                                }
                            }
                            foreach (Form childrenForm in this.OwnedForms)
                            {
                                if (childrenForm.Text == frm.Text)
                                {
                                    return;
                                }
                            }
                        }


                        //childForm.Owner = this;
                        childForm.TopLevel = true;
                        childForm.ShowInTaskbar = true;
                        childForm.StartPosition = FormStartPosition.CenterScreen;
                        childForm.Show();

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_ShowToMainForm" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XtraTabControl1CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs a = (ClosePageButtonEventArgs)e;
                string tabpagename = a.Page.Text;
                //关闭对象
                foreach (Control xtp in xtTabCtrl.TabPages)
                {
                    if (a.Page.ShowCloseButton == DevExpress.Utils.DefaultBoolean.False)
                    {
                        continue;
                    }
                    else if (xtp.Text == tabpagename)
                    {
                        for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                        {
                            if (((Form)(RequestUtil.formObjects[j])).Text == xtp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                            {
                                if (((Form)RequestUtil.formObjects[j]).Text == tabpagename)
                                {
                                    ((Form)(RequestUtil.formObjects[j])).Close();
                                }
                            }
                        }
                    }
                }
                //关闭当前page
                for (int i = 0; i < xtTabCtrl.TabPages.Count; i++)
                {
                    if (xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.True ||
                        xtTabCtrl.TabPages[i].ShowCloseButton == DevExpress.Utils.DefaultBoolean.Default)
                    {
                        if (xtTabCtrl.TabPages[i].Text == tabpagename)
                        {
                            xtTabCtrl.TabPages.RemoveAt(i);
                            i--;
                        }
                    }
                }
                this.xtTabCtrl.SelectedTabPage = this.xtTabCtrl.TabPages[xtTabCtrl.TabPages.Count - 1];
                //
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_XtraTabControl1CloseButtonClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>初始化窗体成员-在init之前</summary>
        private void InitMainFormVaribleFront()
        {
            try
            {
                //设置所有窗体支持皮肤设置
                DevExpress.Skins.SkinManager.EnableFormSkins();

                defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
                defaultLookAndFeel.LookAndFeel.SetSkinStyle("Visual Studio 2013 Blue");

                paraResult = Parallel.For(0, 1, i => { _skinPic = new WindowSkinPic(); });

                SetStyle(ControlStyles.UserPaint, true);

                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.

                SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲

            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_InitMainFormVaribleFront" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>用户登录并根据登录结果加载菜单</summary>
        private void InitConfigAndSysMenu()
        {
            try
            {
                while (!paraResult.IsCompleted) ;

                if (Sys.Safety.ClientFramework.Configuration.BaseInfo.AutoLogoIn == "0")
                {
                    if (null == userLog)
                    {
                        userLog = new frmLogOn();

                        //userLog.ShowInTaskbar = false;

                        userLog.ShowDialog();
                    }
                }
                else
                {
                    try
                    {
                        LoginManager.Login(Sys.Safety.ClientFramework.Configuration.BaseInfo.AutoLogoUser,
                            Sys.Safety.ClientFramework.Configuration.BaseInfo.AutoLogoPass, Sys.Safety.ClientFramework.Configuration.BaseInfo.MenuType);
                    }
                    catch
                    {
                        Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "管理员密码错误，请重新登录!");
                        if (null == userLog)
                        {
                            userLog = new frmLogOn();

                            //userLog.ShowInTaskbar = false;

                            userLog.ShowDialog();
                        }
                    }
                }
                if (!LoginManager.IsLogin)//登录不成功
                {
                    System.Environment.Exit(0);
                }
                wdf = new WaitDialogForm("正在加载数据...", "请等待...");
                if (null == _menuManager)
                {
                    _menuManager = new UserMeun(_skinPic, this.suBtnTip);

                    RequestUtil.OnShowToMainForm = new RequestUtil.ShowToMainForm(ShowToMainForm);
                }

                if (LoginManager.IsLogin)
                {
                    ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                    barStaticItem2.Caption = string.Format("  用户名：{0}  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), _ClientItem.UserName);
                    barStaticItem1.Caption = Sys.Safety.ClientFramework.Configuration.BaseInfo.CustomerCompanyName;
                    barStaticItem1.Glyph = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\jm.png");
                    //barStaticItem10.Glyph = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\MA.png");
                    //barStaticItem10.Caption = "授权" + Basic.Framework.Data.PlatRuntime.Items["CustomerInfo"].ToString() + "使用";
                    barStaticItem3.Caption = "软件版本：" + Sys.Safety.ClientFramework.Configuration.BaseInfo.Version;

                    Dictionary<string, MenuInfo> dic = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, MenuInfo>>(Basic.Framework.Data.PlatRuntime.Items["_Menus"].ToString());

                    menuLst = new List<MenuInfo>();

                    foreach (KeyValuePair<string, MenuInfo> kvp in dic)
                    {
                        menuLst.Add(kvp.Value);
                    }

                    _menuManager.ChangeDevMainMenuItems(this.ribbonControl1, menuLst);

                    LoginManager.LogOnChangeEvent += new EventHandler(LoginManager_LogOnChangeEvent);
                    //注册菜单切换事件
                    RequestUtil.OnMainTabChangeEvent += new RequestUtil.OnMainTabChange(MainFormTabChangeEvent);

                    Handle1 = this.Handle;
                    //注册子窗体关闭委托
                    RequestUtil.OnSunFormCloseEvent = new RequestUtil.OnSunFormClose(SunFormCloseEvent);


                }
                else
                {
                    Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "系统菜单加载失败");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_InitConfigAndSysMenu" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 菜单切换事件
        /// </summary>
        /// <param name="menuname">菜单名称</param>
        private void MainFormTabChangeEvent(string menuname)
        {
            //先判断窗口是否存在，如果存在，则跳转到窗口
            for (int i = 0; i < xtTabCtrl.TabPages.Count; i++)
            {
                if (xtTabCtrl.TabPages[i].Text == menuname)
                {
                    xtTabCtrl.TabPages[i].Show();
                    return;
                }
            }
        }
        /// <summary>
        /// 子窗口关闭时，将主窗口置为最前
        /// </summary>
        private void SunFormCloseEvent()
        {
            if (Handle1 != GetForegroundWindow()) //持续使该窗体置为最前
            {
                SetForegroundWindow(Handle1);
            }
        }
        /// <summary>初始化窗体成员-在init之后</summary>
        private void InitMainFormVaribleBack()
        {
            try
            {

                //初始化Dev的皮肤
                InitDevControlSkin();

                SetFormTitleString(Sys.Safety.ClientFramework.Configuration.BaseInfo.SoftFullName);

                CheckSkinColor();

                string MyFileName = Application.StartupPath + "\\Image\\Icon\\客户端.ico";
                if (File.Exists(MyFileName))
                {
                    this.Icon = new Icon(MyFileName);
                }
                if (wdf != null)
                {
                    wdf.Close();
                }
                barSubItem1.Glyph = Image.FromFile(Application.StartupPath + "\\Image\\Icon\\skin.png");

            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_InitMainFormVaribleBack" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>初始化皮肤样式</summary>
        private void InitDevControlSkin()
        {
            try
            {
                //DevExpress.UserSkins.BonusSkins.Register();
                BarButtonItem barBI = null;
                foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
                {
                    barBI = new BarButtonItem();
                    barBI.Tag = barBI.Name = barBI.Caption = SkinToText(skin.SkinName);
                    barBI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(SkinItemClick);
                    barSubItem1.AddItem(barBI);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_InitDevControlSkin" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>设置窗体标题字符串</summary>
        private bool SetFormTitleString(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title)) { return false; }

                if (title.Length > 50) { title = title.Substring(0, 50); }

                //this.Text = title;

                barStaticItem11.Caption = title;



                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_SetFormTitleString" + ex.Message + ex.StackTrace);
                return false;
            }
        }
        /// <summary>皮肤单击</summary>
        private void SkinItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string eTag = "";
                eTag = TextToSkin(e.Item.Tag.ToString());

                BaseInfo.WindowStyleNow = e.Item.Tag.ToString();
                //LockWindowUpdate(this.Handle);

                defaultLookAndFeel.LookAndFeel.SetSkinStyle(eTag);
                e.Item.Hint = eTag;

                CheckSkinColor();
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_SkinItemClick" + ex.Message + ex.StackTrace);
            }
            finally
            {
                //LockWindowUpdate(IntPtr.Zero);

                this.UpdateStyles();
            }
        }

        /// <summary>设置菜单颜色</summary>
        private void CheckSkinColor()
        {
            try
            {
                if (null == _colorTable) { _colorTable = new ToolStripColorTable(); }

                _colorTable.Base = this.BackColor;

                _colorTable.Border = defaultLookAndFeel.LookAndFeel.ActiveLookAndFeel.Painter.Border.DefaultAppearance.BackColor;

                _colorTable.Fore = this.ForeColor;
                //_colorTable.Fore = Color.White;

                _colorTable.DropDownImageBack = Color.Red;

                _colorTable.DropDownImageSeparator = defaultLookAndFeel.LookAndFeel.ActiveLookAndFeel.Painter.Border.DefaultAppearance.BackColor;

                ToolStripManager.Renderer = new ProfessionalToolStripRendererEx(_colorTable);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_CheckSkinColor" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>重绘ToolTip控件</summary>
        private void suBtnTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(e.Bounds.Width, e.Bounds.Height), Color.FromArgb(255, Color.White), Color.FromArgb(255, this.BackColor)))
                {
                    g.FillRectangle(brush, e.Bounds);
                }

                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    SizeF size = g.MeasureString(e.ToolTipText, e.Font);

                    g.DrawString(e.ToolTipText, e.Font, brush, (e.Bounds.X + (e.Bounds.Width - size.Width) / 2), (e.Bounds.Y + (e.Bounds.Height - size.Height) / 2));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MDIMainForm_suBtnTip_Draw" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 皮肤显示中文转换
        /// </summary>
        /// <param name="skinName"></param>
        /// <returns></returns>
        public string SkinToText(string skinName)
        {
            string rvalue = "";
            switch (skinName)
            {
                case "DevExpress Style":
                    rvalue = "灰色风格";
                    break;
                case "DevExpress Dark Style":
                    rvalue = "夜间深色风格";
                    break;
                case "VS2010":
                    rvalue = "深蓝色风格";
                    break;
                case "Seven Classic":
                    rvalue = "win7风格";
                    break;
                case "Office 2010 Blue":
                    rvalue = "蓝色风格(默认)";
                    break;
                case "Office 2010 Black":
                    rvalue = "黑色风格";
                    break;
                case "Office 2010 Silver":
                    rvalue = "银光白风格";
                    break;
                case "Office 2013":
                    rvalue = "蓝白组合风格";
                    break;
                case "Office 2013 Dark Gray":
                    rvalue = "灰黑色风格";
                    break;
                case "Office 2013 Light Gray":
                    rvalue = "白灰色风格";
                    break;
                case "Visual Studio 2013 Blue":
                    rvalue = "深蓝灰组合风格";
                    break;
                case "Visual Studio 2013 Light":
                    rvalue = "深蓝白组合风格";
                    break;
                case "Visual Studio 2013 Dark":
                    rvalue = "黑水晶风格";
                    break;
                default:
                    rvalue = "蓝色风格(默认)";
                    break;
            }
            return rvalue;
        }
        /// <summary>
        /// 中文显示转皮肤
        /// </summary>
        /// <param name="TextName"></param>
        /// <returns></returns>
        public string TextToSkin(string TextName)
        {
            string rvalue = "";
            switch (TextName)
            {
                case "灰色风格":
                    rvalue = "DevExpress Style";
                    break;
                case "夜间深色风格":
                    rvalue = "DevExpress Dark Style";
                    break;
                case "深蓝色风格":
                    rvalue = "VS2010";
                    break;
                case "win7风格":
                    rvalue = "Seven Classic";
                    break;
                case "蓝色风格(默认)":
                    rvalue = "Office 2010 Blue";
                    break;
                case "黑色风格":
                    rvalue = "Office 2010 Black";
                    break;
                case "银光白风格":
                    rvalue = "Office 2010 Silver";
                    break;
                case "蓝白组合风格":
                    rvalue = "Office 2013";
                    break;
                case "灰黑色风格":
                    rvalue = "Office 2013 Dark Gray";
                    break;
                case "白灰色风格":
                    rvalue = "Office 2013 Light Gray";
                    break;
                case "深蓝灰组合风格":
                    rvalue = "Visual Studio 2013 Blue";
                    break;
                case "深蓝白组合风格":
                    rvalue = "Visual Studio 2013 Light";
                    break;
                case "黑水晶风格":
                    rvalue = "Visual Studio 2013 Dark";
                    break;
                default:
                    rvalue = "Office 2010 Blue";
                    break;
            }
            return rvalue;

        }

        #endregion
        #region ====================公共函数====================
        //[DllImport("user32.dll")]
        //public extern static bool LockWindowUpdate(IntPtr hWndLock);
        //[DllImport("user32")]
        //public static extern int ReleaseCapture();
        //[DllImport("user32")]
        //public static extern int SendMessage(IntPtr hwnd, int msg, int wp, int lp);
        #endregion

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            char mychar = 'A';

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {

                List<MenuInfo> menuList = _MenuService.GetMenuList().Data.FindAll(a => a.Remark4 != null && a.Remark4.Contains("ALT+"));
                foreach (MenuInfo menu in menuList)
                {
                    mychar = (menu.Remark4.Split('+')[1])[0];
                    Keys k1 = (Keys)mychar;
                    if (keyData == (Keys.Alt | k1))
                    {
                        MenuItemClick(menu);
                    }
                }
            }
            return true;
        }
        private void MenuItemClick(MenuInfo menuinfo)
        {
            try
            {
                if (menuinfo.RequestCode != null)
                {
                    RequestGetByCodeRequest request = new RequestGetByCodeRequest();
                    request.Code = menuinfo.RequestCode;
                    var requestResponse = requestService.GetRequestByCode(request);
                    if (requestResponse.IsSuccess && requestResponse.Data != null)
                    {

                        RequestInfo requestinfo = requestResponse.Data;


                        #region 加载请求库的参数信息
                        Dictionary<string, string> param = null;
                        if (!string.IsNullOrEmpty(requestinfo.MenuParams))
                        {
                            if (Convert.ToString(requestinfo.MenuParams).Contains("&"))
                            {
                                string[] ModuleParams = Convert.ToString(requestinfo.MenuParams).Split('&');

                                param = new Dictionary<string, string>();

                                for (int i = 0; i < ModuleParams.Length; i++)
                                {
                                    if (ModuleParams[i].Split('=').Length > 0)
                                    {
                                        param.Add(ModuleParams[i].Split('=')[0], ModuleParams[i].Split('=')[1]);
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(requestinfo.MenuParams)))
                                {
                                    string[] ModuleParams = new string[1];//参数传递

                                    param = new Dictionary<string, string>();

                                    ModuleParams[0] = requestinfo.MenuParams;

                                    param.Add(ModuleParams[0].Split('=')[0], ModuleParams[0].Split('=')[1]);
                                }
                            }
                        }
                        #endregion

                        RequestUtil.ExcuteCommand(requestinfo.RequestCode.ToString(), param, menuinfo.IsSystemDesktop == 1);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ShortCutMenu_LoadPage" + ex.Message + ex.StackTrace);
            }
        }

        private void MDIMainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Text = barStaticItem11.Caption;
            }
            else
            {
                this.Text = "";
            }
        }

    }
}