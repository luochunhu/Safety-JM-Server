using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class Installer3 : Installer
    {
        public override void BeforeNext()
        {
            base.BeforeNext();

            foreach (var group in ConfigModel.ConfigGroup)
            {
                foreach (var config in group.ConfigItems)
                {
                    if (!string.IsNullOrEmpty(group.ServiceConfigFile))
                        Basic.Framework.Common.AppConfigHelper.SaveAppSetting(config.Key, config.Value, group.ServiceConfigFile);
                    if (!string.IsNullOrEmpty(group.ConsoleConfigFile))
                        Basic.Framework.Common.AppConfigHelper.SaveAppSetting(config.Key, config.Value, group.ConsoleConfigFile);
                }
            }
        }

        public override void Load()
        {
            base.Load();

            //服务端
            InstallItem server = SelectedItems.FirstOrDefault(q => q.ServiceName == "SafetyCoreService");
            //网关
            InstallItem gateway = SelectedItems.FirstOrDefault(q => q.ServiceName == "DataCollectionService");
            //数据分析
            InstallItem dataAnalysis = SelectedItems.FirstOrDefault(q => q.ServiceName == "DataAnalysisService");
            if (server != null)
            {
                ConfigGroup cg = new ConfigGroup();
                cg.Key = "server";
                //cg.ServiceConfigFile = string.Format("{0}\\{1}\\Server\\Sys.Safety.Server.ServiceHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.ConsoleConfigFile = string.Format("{0}\\{1}\\Server\\Sys.Safety.Server.ConsoleHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.IsDatabaseSetting = true;
                cg.Port = 3306;
                cg.ConfigItems.Add(new Config()
                {
                    Description = "数据库IP",
                    Key = "dbIp",
                    Metadata = "Server",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("dbIp", cg.ConsoleConfigFile)
                });
                cg.ConfigItems.Add(new Config()
                {
                    Description = "数据库用户名",
                    Key = "dbUserName",
                    Metadata = "User Id",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("dbUserName", cg.ConsoleConfigFile)
                });
                cg.ConfigItems.Add(new Config()
                {
                    Description = "数据库密码",
                    Key = "dbPassword",
                    Metadata = "Password",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("dbPassword", cg.ConsoleConfigFile)
                });
                cg.ConfigItems.Add(new Config()
                {
                    Description = "数据库名",
                    Key = "dbName",
                    Metadata = "Database",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("dbName", cg.ConsoleConfigFile)
                });
                ConfigModel.ConfigGroup.Add(cg);
            }
            if (gateway != null)
            {
                ConfigGroup cg = new ConfigGroup();
                cg.Key = "gateway";
                //cg.ServiceConfigFile = string.Format("{0}\\{1}\\Collection\\Sys.DataCollection.ServiceHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.ConsoleConfigFile = string.Format("{0}\\{1}\\Collection\\Sys.DataCollection.ConsoleHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.ConfigItems.Add(new Config()
                {
                    Description = "虚拟IP",
                    Key = "NetServerIp",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("NetServerIp", cg.ConsoleConfigFile)
                });
                ConfigModel.ConfigGroup.Add(cg);
            }
            if (dataAnalysis != null)
            {
                ConfigGroup cg = new ConfigGroup();
                cg.Key = "analysis";
                //cg.ServiceConfigFile = string.Format("{0}\\{1}\\DataAnalysis\\Sys.Safety.DataAnalysis.ServiceHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.ConsoleConfigFile = string.Format("{0}\\{1}\\DataAnalysis\\Sys.Safety.DataAnalysis.ConsoleHost.exe", BaseFolder, InstallModel.SystemFolder);
                cg.ConfigItems.Add(new Config()
                {
                    Description = "服务端IP",
                    Key = "ServerIp",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("ServerIp", cg.ConsoleConfigFile)
                });
                ConfigModel.ConfigGroup.Add(cg);
            }
        }
    }
}
