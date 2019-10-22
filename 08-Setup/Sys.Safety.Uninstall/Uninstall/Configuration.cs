using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Uninstall
{
    public class Configuration : Basic.Framework.DesignPattern.Singletons.Singleton<Configuration>
    {
        public const string ROOT = "SOFTWARE\\Zhzsoft\\Safety";
        private Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
        private UninstallModel uninstallModel;
        public UninstallModel UninstallModel
        {
            get
            {
                if (null == uninstallModel)
                    uninstallModel = LoadUninstallModel();
                return uninstallModel;
            }
        }

        private UninstallModel LoadUninstallModel()
        {
            UninstallModel uninstallModel = new UninstallModel();
            Microsoft.Win32.RegistryKey productKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ROOT);
            if (productKey != null)
            {
                uninstallModel.SystemName = productKey.GetValue("SystemName").ToString();
                uninstallModel.InstallFolder = productKey.GetValue("InstallFolder").ToString();
                UninstallItem uninstallItem;
                string[] subKeyNames = productKey.GetSubKeyNames();
                foreach (var key in subKeyNames)
                {
                    uninstallItem = new UninstallItem();
                    uninstallItem.DisplyName = RegistryHelper.GetRegistryData(localMachine, string.Format("{0}\\{1}", ROOT, key), "DisplyName");
                    uninstallItem.ApplicationName = RegistryHelper.GetRegistryData(localMachine, string.Format("{0}\\{1}", ROOT, key), "ApplicationName");
                    uninstallItem.ServiceName = RegistryHelper.GetRegistryData(localMachine, string.Format("{0}\\{1}", ROOT, key), "ServiceName");
                    uninstallItem.File = RegistryHelper.GetRegistryData(localMachine, string.Format("{0}\\{1}", ROOT, key), "File");
                    uninstallItem.RunType = RegistryHelper.GetRegistryData(localMachine, string.Format("{0}\\{1}", ROOT, key), "RunType");
                    uninstallItem.RegistryKey = key;
                    uninstallItem.IsSelected = true;
                    uninstallModel.UninstallItems.Add(uninstallItem);
                }
            }
            return uninstallModel;
        }
    }
}
