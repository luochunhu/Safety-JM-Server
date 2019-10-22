using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Uninstall
{
    public class Uninstaller1 : Uninstaller
    {
        string[] apps = { "Sys.Safety.Client.WindowHost", "Sys.Safety.Server.ConsoleHost", "Sys.DataCollection.ConsoleHost", "Basic.DoubleSwitch", "Basic.DoubleSwitch.Dog", "Sys.Safety.DataAnalysis" };
        public override void BeforeNext()
        {
            base.BeforeNext();
            if (UninstallModel.UninstallItems.Where(q => q.IsSelected).ToList().Count == 0)
                throw new Exception("请选择卸载项目!");
            StringBuilder message = new StringBuilder();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (var process in processes)
            {
                if (apps.Any(q => q.ToLower() == process.ProcessName.ToLower()))
                {
                    message.Append(Environment.NewLine).Append(process.ProcessName);
                }
            }
            if (message.ToString() != string.Empty)
                throw new Exception(string.Format("{0}{1}", "以下应用程序正在运行, 请先关闭再继续.", message.ToString()));
            SelectedItems.AddRange(UninstallModel.UninstallItems.Where(q => q.IsSelected));
        }
    }
}
