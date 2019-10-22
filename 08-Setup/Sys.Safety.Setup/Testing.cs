using IWshRuntimeLibrary;
using Basic.Framework.Data.DataAccess;
using Sys.Safety.Setup.Install;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Setup
{
    public partial class Testing : Form
    {
        public Testing()
        {
            InitializeComponent();
        }

        private void btnCreateShortcut_Click(object sender, EventArgs e)
        {
            CrtShortCut(@"E:\Project\3.0\trunk\src\Sys.Safety\03-Client\Sys.Safety.Client\Sys.Safety.Client.WindowHost\bin\Debug\Sys.Safety.Client.WindowHost.exe", "安全监控客户端");
        }

        bool CrtShortCut(string FilePath, string description)
        {

            //从COM中引用 Windows Script Host Object Model
            //using IWshRuntimeLibrary;

            WshShell shell = new WshShell();

            //创建桌面快捷方式

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + description + ".lnk");

            shortcut.TargetPath = FilePath;

            shortcut.WorkingDirectory = Environment.CurrentDirectory;

            shortcut.WindowStyle = 1;

            shortcut.Description = description;

            shortcut.Save();

            //创建开始菜单快捷方式
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + "安全监控系统";
            if (!System.IO.Directory.Exists(startMenuPath))
            {
                System.IO.Directory.CreateDirectory(startMenuPath);
            }

            IWshShortcut shortcut1 = (IWshShortcut)shell.CreateShortcut(startMenuPath + "\\" + description + ".lnk");

            shortcut1.TargetPath = FilePath;

            shortcut1.WorkingDirectory = Environment.CurrentDirectory;

            shortcut1.WindowStyle = 1;

            shortcut1.Description = description;

            shortcut1.Save();

            return true;
        }

        private void btnCheckMap_Click(object sender, EventArgs e)
        {
            /*
            RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                          Environment.Is64BitOperatingSystem
                                          ? RegistryView.Registry64
                                          : RegistryView.Registry32);
            string x86Key = @"SOFTWARE\MozillaPlugins\@cmetamap.com/chrome";
            string x64Key = @"SOFTWARE\Wow6432Node\MozillaPlugins\@cmetamap.com/chrome";
            var metamap = localMachineRegistry.OpenSubKey(Environment.Is64BitOperatingSystem?x64Key:x86Key);
            if(null == metamap || !System.IO.File.Exists(metamap.GetValue("PATH").ToString()))
                MessageBox.Show("没有安装元图");
            else
                MessageBox.Show("已安装元图");
                */
            List<string> uninstallList = GetUninstallList();
            if (uninstallList.Any(q => q == "元图地图开放平台"))
                MessageBox.Show("已安装元图");
            else
                MessageBox.Show("没有安装元图");
        }

        private void btnCheckBit_Click(object sender, EventArgs e)
        {
            if (Environment.Is64BitOperatingSystem)
                MessageBox.Show("x64");
            else
                MessageBox.Show("x86");
        }

        public bool IsWindowsServiceInstalled(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                    return true;
            }
            return false;
        }

        private List<string> GetUninstallList()
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

        private void btnGetUninstallList_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey uninstallNode = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("DisplayName");
                if (displayName != null)
                {
                    UninstallList.Items.Add(displayName);
                }
            }
        }

        private void btnExecuteEXE_Click(object sender, EventArgs e)
        {
            const int ERROR_CANCELLED = 1223; //The operation was canceled by the user.

            ProcessStartInfo info = new ProcessStartInfo(@"C:\Windows\Notepad.exe");
            info.UseShellExecute = true;
            info.Verb = "runas";
            try
            {
                Process.Start(info);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_CANCELLED)
                    MessageBox.Show("已取消! 若要继续安装请重试后选择【是】");
                else
                    throw;
            }
        }

        private void btnReadConfiguration_Click(object sender, EventArgs e)
        {
            InstallModel installModel = Configuration.Instance.InstallModel;
            //ConfigModel configModel = Configuration.Instance.ConfigModel;
        }

        private void btn_MYSQLINI_Click(object sender, EventArgs e)
        {
            string iniFilePath = string.Empty;
            string connectionString = string.Format(" Server={0};Database={1};User ID={2};Password={3};", "127.0.0.1", "information_schema", "root", "root123");
            object datadir = MySqlHelper.ExecuteScalar(connectionString, "select @@datadir");
            if (datadir != null && datadir.ToString() != "" && System.IO.Directory.Exists(datadir.ToString()))
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(datadir.ToString());
                iniFilePath = directoryInfo.Parent.FullName + "\\my.ini";
                if (System.IO.File.Exists(iniFilePath))
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(iniFilePath))
                    {
                        sw.WriteLine("log_bin=mysql-bin");
                        sw.WriteLine("server_id=5");
                        sw.WriteLine("replicate_do_db=testsync1");
                        sw.WriteLine("sync_binlog=1");
                        sw.WriteLine("log_slave_updates=1");
                        sw.WriteLine("relay-log=relay-bin");
                        sw.WriteLine("slave_skip_errors=all");
                        sw.WriteLine("read-only=0");
                        sw.WriteLine("slave_net_timeout=10");
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        private void btnRestarMySQL_Click(object sender, EventArgs e)
        {
            ServiceController[] services = ServiceController.GetServices();
            ServiceController service = services.FirstOrDefault(q => q.ServiceName.ToLower() == "mysql56");
            if (service != null && service.CanStop)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                service.Start();
            }
        }

        private void btnSyncAccount_Click(object sender, EventArgs e)
        {
            string connectionString = string.Format(" Server={0};Database={1};User ID={2};Password={3};", "127.0.0.1", "information_schema", "root", "root123");
            int result = MySqlHelper.ExecuteNonQuery(connectionString, "GRANT replication SLAVE, replication client on *.* to 'repl'@'192.168.1.%' IDENTIFIED by '123456';");
        }

        private void btnQueryMasterStatus_Click(object sender, EventArgs e)
        {
            string connectionString = string.Format(" Server={0};User ID={1};Password={2};", "127.0.0.1", "root", "root123");
            DataTable dtResult = MySqlHelper.ExecuteDataTable(connectionString, "show master status;");
            string file = dtResult.Rows.Count > 0 ? dtResult.Rows[0]["File"].ToString() : string.Empty;
            string position = dtResult.Rows.Count > 0 ? dtResult.Rows[0]["Position"].ToString() : string.Empty;
        }

        private void btnSlave_Click(object sender, EventArgs e)
        {
            string connectionString = string.Format(" Server={0};User ID={1};Password={2};", "127.0.0.1", "root", "root123");
            DataTable dtResult = MySqlHelper.ExecuteDataTable(connectionString, "show slave status;");
            string Slave_IO_Running = dtResult.Rows.Count > 0 ? dtResult.Rows[0]["Slave_IO_Running"].ToString() : string.Empty;
            string Slave_SQL_Running = dtResult.Rows.Count > 0 ? dtResult.Rows[0]["Slave_SQL_Running"].ToString() : string.Empty;
        }
    }
}
