using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sys.Safety.ClientFramework;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using DevExpress.Utils;
using System.Text;
using Sys.Safety.ServiceContract;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Basic.Framework.Common;


namespace Sys.Safety.Client.WindowHost
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {           
            //判断互斥
            Process[] process = Process.GetProcessesByName("Sys.Safety.Client.WindowHost");
            if (process.Length > 1)
            {
                MessageBox.Show("安全监控系统己处于运行状态!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Environment.Exit(0);
            }

            //注册全局异常捕获  20170502
            //设置应用程序处理异常方式：ThreadException处理  
            try
            {
                ///注册VG
                System.Diagnostics.Process.Start("regsvr32.exe", " -s " + Application.StartupPath + @"\GisVector\Vector\dat\vg.dll");
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


            //try
            //{
            //    //授权文件检测
            //    if (!Basic.Framework.Version.Version.IsAuthorized())
            //    {
            //        MessageBox.Show(Basic.Framework.Version.Version.AuthorizedDescription, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        Environment.Exit(0);
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show(Basic.Framework.Version.Version.AuthorizedDescription, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Environment.Exit(0);
            //}



            ///注册Ioc服务
            ServiceManager.RegisterService();

            //初始化配置文件 added by  20170615
            InitConfig();

            try
            {

                DevExpress.UserSkins.OfficeSkins.Register();
                //设置所有窗体支持皮肤设置
                DevExpress.Skins.SkinManager.EnableFormSkins();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                #region//根据配置启动服务端程序 
                string AutoStartExePath = ConfigurationManager.AppSettings["AutoStartServerPath"].ToString();
                string AutoStartServerExeName = ConfigurationManager.AppSettings["AutoStartServerExeName"].ToString();

                WaitDialogForm wdf = null;

                if (!string.IsNullOrEmpty(AutoStartExePath))
                {
                    try
                    {
                        wdf = new WaitDialogForm("正在启动中心站程序...", "请等待...");
                        var pros = Process.GetProcessesByName(AutoStartServerExeName);
                        if (pros.Length < 1)
                        {
                            Process myprocess = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo(AutoStartExePath + "\\" + AutoStartServerExeName + ".exe");
                            startInfo.UseShellExecute = false;
                            myprocess.StartInfo = startInfo;
                            myprocess.Start();

                        }
                        Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
                #endregion


                #region 检查服务端是否正常运行

                int ErrCount = 0;
                while (true)
                {
                    try
                    {
                        //调用服务端接口，看能否正常调用来判断服务端是否开启
                        var response = ServiceFactory.Create<IRemoteStateService>().GetLastReciveTime();
                        break;
                    }
                    catch (Exception ex)
                    {
                        ErrCount++;

                        Thread.Sleep(100);
                    }

                    if (ErrCount >= 3)
                    {//30秒如果没有连接成功则提示，并退出程序
                        if (wdf != null)
                        {
                            wdf.Close();
                        }
                        if (!string.IsNullOrEmpty(AutoStartExePath))
                        {
                            MessageBox.Show("系统启动失败，请检查系统配置！");
                            System.Environment.Exit(0);
                        }
                        else
                        {
                            Sys.Safety.ClientFramework.View.WcfManage.WcfManage wcfmag = new Sys.Safety.ClientFramework.View.WcfManage.WcfManage();
                            if (wcfmag.ShowDialog() == DialogResult.OK)
                            {
                                //如果用户重新设置了IP，点确定后，重新连接
                                ErrCount = 0;
                            }
                            else
                            {
                                System.Environment.Exit(0);
                            }
                        }
                    }
                    //Thread.Sleep(5000);
                }

                #endregion

                if (wdf != null)
                {
                    wdf.Close();
                }



                Application.Run(new Sys.Safety.ClientFramework.View.MainForm.MDIMainForm());
                /**
             * 当前用户是管理员的时候，直接启动应用程序
             * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
             */
                ////获得当前登录的Windows用户标示
                //System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                //System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                ////判断当前登录用户是否为管理员
                //if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                //{
                //    //如果是管理员，则直接运行
                //    Application.Run(new Sys.Safety.ClientFramework.View.MainForm.MDIMainForm());
                //}
                //else
                //{
                //    //创建启动对象
                //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //    startInfo.UseShellExecute = true;
                //    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                //    startInfo.FileName = Application.ExecutablePath;
                //    //设置启动动作,确保以管理员身份运行
                //    startInfo.Verb = "runas";
                //    try
                //    {
                //        System.Diagnostics.Process.Start(startInfo);
                //    }
                //    catch
                //    {
                //        return;
                //    }
                //    //退出
                //    Application.Exit();
                //}
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
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
