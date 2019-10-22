using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sys.Safety.Setup.Install
{
    public class Configuration : Basic.Framework.DesignPattern.Singletons.Singleton<Configuration>
    {
        private string installFilePath = Environment.CurrentDirectory + "\\Install.xml";
        Installer installer = new Installer();

        private InstallModel installModel;
        public InstallModel InstallModel
        {
            get
            {
                if (null == installModel)
                    installModel = LoadInstallModel();
                return installModel;
            }
        }

        private ConfigModel configModel;
        public ConfigModel ConfigModel
        {
            get
            {
                if (null == configModel)
                    configModel = new ConfigModel();
                return configModel;
            }
        }

        private InstallModel LoadInstallModel()
        {
            InstallModel installModel = new InstallModel();
            if (File.Exists(installFilePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(installFilePath);
                    XmlNode infoNode = xmlDoc.SelectSingleNode("Configuration/Install/Information");
                    installModel.SystemName = infoNode.Attributes["SystemName"].Value;
                    installModel.SystemFolder = infoNode.Attributes["SystemFolder"].Value;

                    XmlNodeList items = xmlDoc.SelectSingleNode("Configuration/Install/Items").ChildNodes;
                    System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcesses();
                    foreach (XmlNode element in items)
                    {
                        if (element.GetType().FullName == "System.Xml.XmlComment")
                            continue;
                        InstallItem item = new InstallItem();
                        XmlElement xe = (XmlElement)element;
                        item.Name = xe.GetAttribute("Name");
                        item.Type = xe.GetAttribute("Type");
                        item.RunType = xe.GetAttribute("RunType");
                        item.InstallFolder = xe.GetAttribute("InstallFolder");
                        item.InstallFile = xe.GetAttribute("InstallFile");
                        item.CopyFrom = string.Format("{0}\\{1}", Environment.CurrentDirectory, xe.GetAttribute("CopyFrom"));
                        item.InstallLicence = bool.Parse(xe.GetAttribute("InstallLicence"));
                        item.ApplicationName = xe.GetAttribute("ApplicationName");
                        item.ServiceName = xe.GetAttribute("ServiceName");
                        item.IsSelected = true;
                        if (item.RunType == "service")
                        {
                            item.IsSelected = !installer.IsWindowsServiceInstalled(item.ServiceName);
                        }else if(item.RunType == "exe")
                        {
                            item.IsSelected = !process.Any(q => q.ProcessName.ToLower() == item.InstallFile.Replace(".exe", string.Empty).ToLower());
                            if (item.IsSelected)
                            {
                                item.IsSelected = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\Zhzsoft\\Safety\\{0}", item.InstallFolder)) == null;
                            }          
                        }                 
                        item.DisplayOrder = string.IsNullOrEmpty(xe.GetAttribute("DisplayOrder")) ? 1 : int.Parse(xe.GetAttribute("DisplayOrder"));

                        XmlNodeList subNodes = element.ChildNodes;
                        if (subNodes != null)
                        {
                            foreach (XmlNode subElement in subNodes)
                            {
                                InstallSubItem subItem = new InstallSubItem();
                                subItem.InstallFolder = subElement.Attributes["InstallFolder"].Value;
                                subItem.InstallFile = subElement.Attributes["InstallFile"].Value;
                                subItem.ApplicationName = subElement.Attributes["ApplicationName"].Value;
                                item.SubItems.Add(subItem);
                            }
                        }
                        installModel.InstallItems.Add(item);
                    }
                    return installModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new FileNotFoundException("安装配置文件不存在!", installFilePath);
            }
        }

    }
}
