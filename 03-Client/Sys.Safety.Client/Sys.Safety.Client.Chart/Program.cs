using DevExpress.LookAndFeel;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sys.Safety.Client.Chart
{
    static class Program
    {
        public static string WindowStypeNow = "Visual Studio 2013 Blue";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            LogHelper.Debug(DateTime.Now);
            //注册全局异常捕获  20170502
            //设置应用程序处理异常方式：ThreadException处理  
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常  
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常  
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ///注册Ioc服务
            ServiceManager.RegisterService();

            //初始化配置文件 added by  20170615
            InitConfig();
            stopwatch.Stop();
            LogHelper.Debug("初始化ioc服务，耗时：" + stopwatch.ElapsedMilliseconds);
            //初始化登录用户  20170714
            bool isLogin = false;
            ClientItem _ClientItem = new ClientItem();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string menuURL = "";
            if (args != null)
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("=") && args[i].Split('=')[0] == "userName")
                        {
                            _ClientItem.UserName = args[i].Split('=')[1];
                            isLogin = true;
                        }
                        if (args[i].Contains("=") && args[i].Split('=')[0] == "userID")
                        {
                            _ClientItem.UserID = args[i].Split('=')[1];
                            isLogin = true;
                        }
                        if (args[i].Contains("=") && args[i].Split('=')[0] == "WindowStyle")//继承主窗体皮肤
                        {
                            WindowStypeNow = TextToSkin(args[i].Split('=')[1]);
                        }
                        if (args[i].Contains("=") && args[i].Split('=')[0] == "menuURL")//接收窗体参数
                        {
                            menuURL = args[i].Split('=')[1];
                        }
                        if (args[i].Contains("=") && args[i].Split('=')[0] == "PointID")
                        {
                            dic.Add(args[i].Split('=')[0], args[i].Split('=')[1]);
                        }
                    }
                }
            }
            if (!isLogin)
            {
                MessageBox.Show("非法操作！");
                Environment.Exit(0);//非菜单进入执行文件，直接退出  20170715
            }
            //保存到运行对象缓存中
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] = _ClientItem;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(KeyConst.ClientItemKey, _ClientItem);
            }

            ////设置所有窗体支持皮肤设置
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            if (!string.IsNullOrEmpty(Program.WindowStypeNow))
            {
                defaultLookAndFeel.LookAndFeel.SetSkinStyle(Program.WindowStypeNow);
            }
            else
            {
                defaultLookAndFeel.LookAndFeel.SetSkinStyle("Visual Studio 2013 Blue");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LogHelper.Debug(DateTime.Now);
            switch (menuURL)
            {
                case "ChartSetting":
                    Application.Run(new ChartSetting());
                    break;
                case "Kgl_StateBar":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Kgl_StateBar(dic));
                    }
                    else
                    {
                        Application.Run(new Kgl_StateBar());
                    }
                    break;
                case "Kgl_StateChg":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Kgl_StateChg(dic));
                    }
                    else
                    {
                        Application.Run(new Kgl_StateChg());
                    }
                    break;
                case "Kgl_StateLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Kgl_StateLine(dic));
                    }
                    else
                    {
                        Application.Run(new Kgl_StateLine());
                    }
                    break;
                case "Kgl_StateLineWidthScreen":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Kgl_StateLineWidthScreen(dic));
                    }
                    else
                    {
                        Application.Run(new Kgl_StateLineWidthScreen());
                    }
                    break;
                case "Mnl_BjLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_BjLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_BjLine());
                    }
                    break;
                case "Mnl_DayZdzLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_DayZdzLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_DayZdzLine());
                    }
                    break;
                case "Mnl_DdLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_DdLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_DdLine());
                    }
                    break;
                case "Mnl_FiveMiniteLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_FiveMiniteLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_FiveMiniteLine());
                    }
                    break;
                case "Mnl_LineWithCoordinate":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_LineWithCoordinate(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_LineWithCoordinate());
                    }
                    break;
                case "Mnl_LineWithScreen":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_LineWithScreen(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_LineWithScreen());
                    }
                    break;
                case "Mnl_McLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_McLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_McLine());
                    }
                    break;
                case "Mnl_MthBarAndLine":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_MthBarAndLine(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_MthBarAndLine());
                    }
                    break;
                case "Mnl_SSZChart":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new Mnl_SSZChart(dic));
                    }
                    else
                    {
                        Application.Run(new Mnl_SSZChart());
                    }
                    break;
                case "MnlAndKgl_LineWithScreen":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new MnlAndKgl_LineWithScreen(dic));
                    }
                    else
                    {
                        Application.Run(new MnlAndKgl_LineWithScreen());
                    }
                    break;
                case "MnlAndKzl_LineWithScreen":
                    //Application.Run(new Mnl_DayZdzLine(dic));
                    if (dic.Count > 0)
                    {
                        Application.Run(new MnlAndKzl_LineWithScreen(dic));
                    }
                    else
                    {
                        Application.Run(new MnlAndKzl_LineWithScreen());
                    }
                    break;
                case "MnlKglKzl_LineWithScreen":
                    Application.Run(new MnlKglKzl_LineWithScreen());
                    break;
                default:
                    Application.Run(new Mnl_FiveMiniteLine());
                    break;
            }
        }
        public static string TextToSkin(string TextName)
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
                    rvalue = "Visual Studio 2013 Blue";
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
                    rvalue = "Visual Studio 2013 Blue";
                    break;
            }
            return rvalue;

        }
        //初始化配置文件
        private static void InitConfig()
        {
            //获取设置，设置到客户端全局缓存     
            string ip = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
            string port = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
            string url = string.Format("http://{0}:{1}", ip, port);

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("ServerUrl"))
            {
                Basic.Framework.Data.PlatRuntime.Items["ServerUrl"] = url;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add("ServerUrl", url);
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            LogHelper.Error(str);
            //LogManager.WriteLog(str);  
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            LogHelper.Error(str);
            //LogManager.WriteLog(str);  
        }
        /// <summary>  
        /// 生成自定义异常消息  
        /// </summary>  
        /// <param name="ex">异常对象</param>  
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>  
        /// <returns>异常字符串文本</returns>  
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}
