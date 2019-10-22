using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class InstallModel
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 系统文件夹
        /// </summary>
        public string SystemFolder { get; set; }
        /// <summary>
        /// 安装文件夹
        /// </summary>
        public string BaseFolder { get; set; }
        /// <summary>
        /// Licence文件路径
        /// </summary>
        public string LicenceFilePath { get; set; }


        private List<InstallItem> installItems;
        /// <summary>
        /// 可安装的项目
        /// </summary>
        public List<InstallItem> InstallItems {
            get {
                if (null == installItems)
                    installItems = new List<InstallItem>();
                return installItems;
            }
        }
    }

    public class InstallItem
    {
        /// <summary>
        /// 安装项名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 安装类型(Install,Copy)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 运行类型(Service,EXE)
        /// </summary>
        public string RunType { get; set; }
        /// <summary>
        /// 安装文件
        /// </summary>
        public string InstallFile { get; set; }
        /// <summary>
        /// 安装文件夹
        /// </summary>
        public string InstallFolder { get; set; }
        /// <summary>
        /// 安装类型为Copy时要复制的文件夹(相对于安装目录的相对路径)
        /// </summary>
        public string CopyFrom { get; set; }
        /// <summary>
        /// 是否安装Licence文件
        /// </summary>
        public bool InstallLicence { get; set; }
        /// <summary>
        /// 安装项的应用程序名称(创建快捷方式的名称也是使用这个名称)
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 运行类型为Service时注册的服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 是否已安装
        /// </summary>
        public bool IsInstalled { get; set; }
        /// <summary>
        /// 是否正在安装
        /// </summary>
        public bool Installing { get; set; }
        /// <summary>
        /// 是否选择安装
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// 在安装界面上的显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        private List<InstallSubItem> subItems;
        /// <summary>
        /// 安装时要一起安装的程序
        /// </summary>
        public List<InstallSubItem> SubItems {
            get {
                if (null == subItems)
                    subItems = new List<InstallSubItem>();
                return subItems;
            }
        }

    }

    public class InstallSubItem
    {
        /// <summary>
        /// 安装文件
        /// </summary>
        public string InstallFile { get; set; }
        /// <summary>
        /// 安装文件夹
        /// </summary>
        public string InstallFolder { get; set; }
        /// <summary>
        /// 应用程序名称(卸载列表里的应用程序名称)
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 是否已安装
        /// </summary>
        public bool IsInstalled { get; set; }
    }

    
    public class ConfigModel
    {
        private List<ConfigGroup> configGroup;
        /// <summary>
        /// 配置组
        /// </summary>
        public List<ConfigGroup> ConfigGroup
        {
            get
            {
                if (null == configGroup)
                    configGroup = new List<ConfigGroup>();
                return configGroup;
            }
        }
    }

    public class ConfigGroup
    {
        /// <summary>
        /// 配置项Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 服务配置文件
        /// </summary>
        public string ServiceConfigFile { get; set; }
        /// <summary>
        /// 控制台配置文件
        /// </summary>
        public string ConsoleConfigFile { get; set; }
        /// <summary>
        /// 是否是数据库配置
        /// </summary>
        public bool IsDatabaseSetting { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        private List<Config> configItems;
        /// <summary>
        /// 配置项
        /// </summary>
        public List<Config> ConfigItems {
            get
            {
                if (null == configItems)
                    configItems = new List<Config>();
                return configItems;
            }
        }

    }

    public class Config
    {
        /// <summary>
        /// 元数据
        /// </summary>
        public string Metadata { get; set; }
        /// <summary>
        /// 配置项描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 修改配置文件的Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 修改配置文件的Value
        /// </summary>
        public string Value { get; set; }
    }
}
