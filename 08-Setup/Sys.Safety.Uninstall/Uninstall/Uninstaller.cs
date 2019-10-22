using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Uninstall
{
    public class Uninstaller
    {
        public UninstallModel UninstallModel
        {
            get
            {
                return Configuration.Instance.UninstallModel;
            }
        }
        private static List<UninstallItem> selectedItems = new List<UninstallItem>();
        /// <summary>
        /// 选择要安装的项目
        /// </summary>
        public static List<UninstallItem> SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        private void ExecuteCMD(string arguments = "")
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(Environment.SystemDirectory + @"\cmd.exe");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.Verb = "runas";
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(startInfo);
                p.StandardInput.WriteLine(arguments);
                p.WaitForExit(6000);
                p.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExecuteCMD2(string arguments = "")
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(Environment.SystemDirectory + @"\cmd.exe");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.Verb = "runas";
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(startInfo);
                p.StandardInput.WriteLine(arguments);
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UninstallService(string installFile, string serviceName)
        {
            ServiceController service = GetService(serviceName);
            if (null == service)
                return;
            if (service.CanStop && service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
            }
            string arguments = string.Format(@"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe /u {0} & exit", installFile);
            ExecuteCMD2(arguments);
        }

        public ServiceController GetService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.ToLower() == serviceName.ToLower())
                    return service;
            }
            return null;
        }

        /// <summary> 
        /// 开机启动项 
        /// </summary> 
        /// <param name=\"Started\">是否启动</param> 
        /// <param name=\"name\">启动值的名称</param> 
        /// <param name=\"path\">启动程序的路径</param> 
        public static void RunWhenStart(bool Started, string name, string path = "")
        {
            string key = Environment.Is64BitOperatingSystem ? "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run\\" : "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\";
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(key);
            try
            {
                if (Started)
                {
                    Run.SetValue(name, path);
                }
                else
                {
                    try
                    {
                        Run.DeleteValue(name);
                    }catch{}                  
                }
                HKLM.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInstallFolderThenExit()
        {
            //string fileName = System.IO.Path.GetTempPath() + "uninstall.bat";
            //System.IO.StreamWriter bat = new System.IO.StreamWriter(fileName, false, Encoding.Default);
            //bat.WriteLine("ping -n 1 -w 1000 192.186.221.125");
            //bat.WriteLine("ping -n 1 -w 1000 192.186.221.125");
            //bat.WriteLine(string.Format("del \"{0}\" /q", fileName));
            //bat.WriteLine(string.Format("rd \"{0}\" /s/q", folderPath));
            //bat.Close();
            //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(fileName);
            //info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //System.Diagnostics.Process.Start(info);
            if (!UninstallModel.UninstallItems.Exists(q => q.IsUninstalled == false))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ping -n 1 -w 1000 192.186.221.125").Append(Environment.NewLine);
                sb.Append(string.Format("rd \"{0}\" /s/q", UninstallModel.InstallFolder)).Append(Environment.NewLine);
                sb.Append("exit");
                ExecuteCMD2(sb.ToString());
            }
            Environment.Exit(0);
        }

        public virtual void Load() { }
        public virtual void AfterLoad() { }
        public virtual void BeforeNext() { }
        public virtual void Next() { }
        public virtual void Prev() { }
    }
}
