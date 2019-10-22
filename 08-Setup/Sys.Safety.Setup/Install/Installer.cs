using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sys.Safety.Setup.Install
{
    public class Installer
    {
        /// <summary>
        /// 安装路径
        /// </summary>
        public string BaseFolder
        {
            get
            {
                return Configuration.Instance.InstallModel.BaseFolder;
            }
            set
            {
                Configuration.Instance.InstallModel.BaseFolder = value;
            }
        }
        /// <summary>
        /// Licence 文件路径
        /// </summary>
        public string LicenceFilePath
        {
            get
            {
                return Configuration.Instance.InstallModel.LicenceFilePath;
            }
            set
            {
                Configuration.Instance.InstallModel.LicenceFilePath = value;
            }
        }

        public InstallModel InstallModel
        {
            get
            {
                return Configuration.Instance.InstallModel;
            }
        }

        public ConfigModel ConfigModel
        {
            get
            {
                return Configuration.Instance.ConfigModel;
            }
        }

        private List<InstallItem> startServices;
        /// <summary>
        /// 注册为服务的项目
        /// </summary>
        public List<InstallItem> StartServices
        {
            get
            {
                if (startServices == null)
                    startServices = new List<InstallItem>();
                return startServices;
            }
        }

        private static List<InstallItem> selectedItems = new List<InstallItem>();
        /// <summary>
        /// 选择要安装的项目
        /// </summary>
        public static List<InstallItem> SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        public List<string> GetUninstallList()
        {
            List<string> result = new List<string>();
            Microsoft.Win32.RegistryKey uninstallNode = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("DisplayName");
                if (displayName != null)
                {
                    result.Add(displayName.ToString());
                }
            }
            return result;
        }

        public void ExecuteCMD(string arguments = "")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Environment.SystemDirectory + @"\cmd.exe");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
                p.StandardInput.WriteLine(arguments);
                p.StandardInput.WriteLine("exit");
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteInstallPackage(string filePath)
        {
            const int ERROR_CANCELLED = 1223; //The operation was canceled by the user.

            FileInfo fileInfo = new FileInfo(filePath);

            ProcessStartInfo startInfo;
            //if (fileInfo.Extension == ".msi")
            //    startInfo = new ProcessStartInfo("msiexec.exe", "/i " + filePath);
            //else
                startInfo = new ProcessStartInfo(filePath);
            //startInfo.UseShellExecute = true;
            //startInfo.Verb = "runas";
            try
            {
                Process.Start(startInfo).WaitForExit();
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_CANCELLED)
                    throw new Exception("已取消! 若要继续安装请重试后选择【是】");
                else
                    throw ex;
            }
        }

        public void CopyDirectory(string sourcePath, string destinationPath, bool overwrite)
        {
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    foreach (string fileName in Directory.GetFiles(sourcePath))
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        fileInfo.CopyTo(destinationPath + fileInfo.Name, overwrite);
                    }
                    foreach (string directory in Directory.GetDirectories(sourcePath))
                    {
                        DirectoryInfo directoryInformation = new DirectoryInfo(directory);
                        CopyDirectory(directory, destinationPath + directoryInformation.Name, overwrite);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateShortCut(string filePath, string description, string systemName)
        {

            //从COM中引用 Windows Script Host Object Model
            //using IWshRuntimeLibrary;

            WshShell shell = new WshShell();

            //创建桌面快捷方式

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + description + ".lnk");

            shortcut.TargetPath = filePath;

            shortcut.WorkingDirectory = Environment.CurrentDirectory;

            shortcut.WindowStyle = 1;

            shortcut.Description = description;

            shortcut.Save();

            //创建开始菜单快捷方式
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + systemName;
            if (!System.IO.Directory.Exists(startMenuPath))
            {
                System.IO.Directory.CreateDirectory(startMenuPath);
            }

            IWshShortcut shortcut1 = (IWshShortcut)shell.CreateShortcut(startMenuPath + "\\" + description + ".lnk");

            shortcut1.TargetPath = filePath;

            shortcut1.WorkingDirectory = Environment.CurrentDirectory;

            shortcut1.WindowStyle = 1;

            shortcut1.Description = description;

            shortcut1.Save();
        }

        public bool IsWindowsServiceInstalled(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.ToLower() == serviceName.ToLower())
                    return true;
            }
            return false;
        }

        public ServiceController GetService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.ToLower() == serviceName.ToLower())
                    return service;
            }
            throw new ArgumentException(string.Format("The service {0} not exists.", serviceName));
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="installFile">文件名称</param>
        /// <param name="parameter">默认为空表示安装服务，/u 表示卸载服务</param>
        /// <returns></returns>
        public bool InstallService(string installFile, string serviceName)
        {
            string arguments = string.Format(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe {0} && sc config {1} start=demand && exit", string.Format("\"{0}\"", installFile), serviceName);
            ExecuteCMD(arguments);
            return true;
        }

        public void StartService(string serviceName)
        {
            try
            {
                ServiceController service = GetService(serviceName);
                //StringBuilder command = new StringBuilder();
                //command.Append("Net Start ").Append(serviceName).Append(Environment.NewLine).Append("pause").Append(Environment.NewLine).Append("exit");
                string command = string.Format("Net Start {0} && exit", serviceName);
                ExecuteCMD(command);
                service = GetService(serviceName);
                if (service.Status != ServiceControllerStatus.Running)
                {
                    service.Start();
                    service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, string.Format("\"{0}\"", path));
                    HKLM.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public virtual void Load() { }
        public virtual void AfterLoad() { }
        public virtual void BeforeNext() { }
        public virtual void Next() { }
        public virtual void Prev() { }
    }
}
