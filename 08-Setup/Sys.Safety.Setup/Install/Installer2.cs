using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class Installer2 : Installer
    {
        private const string ROOT = "SOFTWARE\\Zhzsoft\\Safety";
        public override void AfterLoad()
        {
            WriteRegistryHeader(InstallModel);
            foreach (var item in SelectedItems)
            {
                try
                {
                    item.Installing = true;
                    InstallSoftware(item);
                    if (item.Type == "copy")
                        WriteRegistry(item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    item.Installing = false;
                }
            }
        }

        private void WriteRegistryHeader(InstallModel installModel)
        {
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, "SOFTWARE\\Zhzsoft", "DisplayName", "");
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, ROOT, "SystemName", installModel.SystemName);
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, ROOT, "InstallFolder", string.Format("{0}\\{1}", BaseFolder, installModel.SystemFolder));
        }

        private void WriteRegistry(InstallItem installItem)
        {
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, string.Format("{0}\\{1}", ROOT, installItem.InstallFolder), "DisplyName", installItem.Name);
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, string.Format("{0}\\{1}", ROOT, installItem.InstallFolder), "ApplicationName", installItem.ApplicationName);
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, string.Format("{0}\\{1}", ROOT, installItem.InstallFolder), "ServiceName", installItem.ServiceName);
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, string.Format("{0}\\{1}", ROOT, installItem.InstallFolder), "File", string.Format("{0}\\{1}\\{2}\\{3}", BaseFolder, InstallModel.SystemFolder, installItem.InstallFolder, installItem.InstallFile));
            RegistryHelper.SetRegistryData(Microsoft.Win32.Registry.LocalMachine, string.Format("{0}\\{1}", ROOT, installItem.InstallFolder), "RunType", installItem.RunType);
        }

        private void InstallSoftware(InstallItem installItem)
        {
            if (installItem.Type == "install")
            {
                string installFile = string.Format("{0}\\{1}\\{2}", Environment.CurrentDirectory, installItem.InstallFolder, installItem.InstallFile);
                InstallPackage(installFile, installItem.ApplicationName);
                installItem.IsInstalled = GetUninstallList().Exists(q => q.StartsWith(installItem.ApplicationName));
            }
            else if (installItem.Type == "copy")
            {
                List<InstallSubItem> subItems = installItem.SubItems;
                foreach (var item in subItems)
                {
                    if (item.ApplicationName.StartsWith("MySQL Connector Net") && SelectedItems.Exists(q => q.ServiceName.ToLower() == "mysql56"))
                    {
                        item.IsInstalled = true;
                        continue;
                    }
                    string subInstallFile = string.Format("{0}\\{1}\\{2}", Environment.CurrentDirectory, item.InstallFolder, item.InstallFile);
                    InstallPackage(subInstallFile, item.ApplicationName);
                    item.IsInstalled = GetUninstallList().Exists(q => q.StartsWith(item.ApplicationName));
                }
                if (subItems.Count(q => q.IsInstalled) == subItems.Count)
                {
                    Copy(installItem);
                    string installFilePath = string.Format("{0}\\{1}\\{2}\\{3}", BaseFolder, InstallModel.SystemFolder, installItem.InstallFolder, installItem.InstallFile);
                    if (installItem.RunType == "service")
                    {
                        InstallService(installFilePath, installItem.ServiceName);
                    }
                    else if (installItem.RunType == "exe")
                    {
                        CreateShortCut(installFilePath, installItem.ApplicationName, InstallModel.SystemName);
                        if (installItem.ServiceName == "BasicHAService")//热备热切开机自动启动
                        {
                            RunWhenStart(true, installItem.ServiceName, installFilePath);
                        }
                    }
                    installItem.IsInstalled = true;
                }
                ////客户端安装调用批处理文件
                //if (installItem.InstallFolder.ToLower() == "client")
                //{
                //    string installFilePath = string.Format("{0}\\{1}\\{2}\\GisVector\\Vector\\", BaseFolder, InstallModel.SystemFolder, installItem.InstallFolder);
                //    ExecuteBat(installFilePath);
                //}
            }
        }

        private void ExecuteBat(string filePath)
        {
            Process proc = null;
            try
            {
                string targetDir = string.Format(filePath);//this is where testChange.bat lies
                proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = "RegisterActiveX.bat";
                proc.StartInfo.Arguments = string.Format("10");//this is argument
                proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }

        private void InstallPackage(string installFile, string applicationName)
        {
            List<string> uninstallList = GetUninstallList();
            if (!uninstallList.Exists(q => q.StartsWith(applicationName)))
            {
                if (!File.Exists(installFile))
                {
                    throw new FileNotFoundException(string.Format("安装文件:{0}.不存在!", installFile));
                }
                ExecuteInstallPackage(installFile);
            }
        }

        private void Copy(InstallItem installItem)
        {
            string copyFromFolder = installItem.CopyFrom;
            string copyToFolder = string.Format("{0}\\{1}\\{2}", BaseFolder, InstallModel.SystemFolder, installItem.InstallFolder);
            if (!Directory.Exists(copyFromFolder))
                throw new DirectoryNotFoundException(string.Format("安装包所在文件夹:{0}.不存在!", copyFromFolder));
            CopyDirectory(copyFromFolder, copyToFolder, true);
            if (installItem.InstallLicence)
            {
                FileInfo fi = new FileInfo(LicenceFilePath);
                File.Copy(LicenceFilePath, string.Format("{0}\\{1}", copyToFolder, fi.Name), true);
            }
        }
    }
}
