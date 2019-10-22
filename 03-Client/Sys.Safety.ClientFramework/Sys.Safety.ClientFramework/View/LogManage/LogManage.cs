using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.Xml;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.View.LogManage
{
    public partial class LogManage : RibbonForm
    {
        public LogManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //保存所有配置信息
                string xmlFileName;
                string xpath;
                string logfilePath;
                string serverIP, serverDB, serverUser, serverPassword;
                xmlFileName = Application.StartupPath + "\\Config\\Log4netConfig.config";
                xpath = "//appender";
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFileName);
                XmlNodeList xmlnodes = xmldoc.SelectNodes(xpath);                
                foreach (XmlNode node in xmlnodes)
                {
                    if (node.OuterXml.Contains("InfoAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "file")
                            {
                                logfilePath = textEdit5.Text + "/info/";
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }                                
                                break;
                            }
                        }
                    }
                    else if (node.OuterXml.Contains("WarnAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "file")
                            {
                                logfilePath = textEdit5.Text + "/warn/";
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }   
                                break;
                            }
                        }
                    }
                    else if (node.OuterXml.Contains("ErrorAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "file")
                            {
                                logfilePath = textEdit5.Text + "/error/";
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }   
                                break;
                            }
                        }
                    }
                    else if (node.OuterXml.Contains("SystemInfoAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "file")
                            {
                                logfilePath = textEdit5.Text + "/system/";
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }   
                                break;
                            }
                        }
                    }
                    else if (node.OuterXml.Contains("DebugAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "file")
                            {
                                logfilePath = textEdit5.Text + "/debug/";
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }   
                                break;
                            }
                        }
                    }
                    else if (node.OuterXml.Contains("ADONetAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "connectionstring")
                            {
                                logfilePath = string.Format("server={0};database={1};Uid={2};Pwd={3};",
                                    textEdit1.Text, textEdit2.Text, textEdit3.Text, textEdit4.Text);
                                //server=127.0.0.1;database=mas;Uid=root;Pwd=root123;                               
                                XmlElement xmlelem = (XmlElement)sonnode;
                                if (xmlelem.HasAttribute("value"))//如果节点有需要更改的属性
                                {
                                    xmlelem.SetAttribute("value", logfilePath);//则把哈希表中相应的值Value赋给此属性Key
                                }   
                                break;
                            }
                        }
                    }
                }
                //保存
                xmldoc.Save(xmlFileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存配置成功,重启生效！");
            }
            catch (System.Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("保存配置失败,详细见错误日志！");
                LogHelper.Error("LogManage-simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogManage_Load(object sender, EventArgs e)
        {
            try
            {
                //加载所有配置信息
                string xmlFileName;
                string xpath;
                string logfilePath;
                string serverIP, serverDB, serverUser, serverPassword;
                xmlFileName = Application.StartupPath + "\\Config\\Log4netConfig.config";
                xpath = "//appender";
                XmlNodeList xmlnodes = Basic.Framework.Common.XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
                foreach (XmlNode node in xmlnodes)
                {
                    if (node.OuterXml.Contains("InfoAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name == "file")
                            {
                                logfilePath = sonnode.OuterXml.Substring(sonnode.OuterXml.IndexOf("\"") + 1,
                                    sonnode.OuterXml.LastIndexOf("\"") - sonnode.OuterXml.IndexOf("\"") - 1);
                                if (logfilePath.Contains("/info"))
                                {
                                    textEdit5.Text = logfilePath.Substring(0, logfilePath.IndexOf("/info"));
                                    break;
                                }
                            }
                        }
                    }
                    if (node.OuterXml.Contains("ADONetAppender"))
                    {
                        XmlNodeList sonnodes = node.ChildNodes;
                        foreach (XmlNode sonnode in sonnodes)
                        {
                            if (sonnode.Name.ToLower() == "connectionstring")
                            {
                                logfilePath = sonnode.OuterXml.Substring(sonnode.OuterXml.IndexOf("\"") + 1,
                                    sonnode.OuterXml.LastIndexOf("\"") - sonnode.OuterXml.IndexOf("\"") - 1);
                                //server=127.0.0.1;database=mas;Uid=root;Pwd=root123;
                                serverIP = logfilePath.Substring(logfilePath.IndexOf("server=") + 7, logfilePath.IndexOf(";database") - logfilePath.IndexOf("server=") - 7);
                                serverDB = logfilePath.Substring(logfilePath.IndexOf("database=") + 9, logfilePath.IndexOf(";Uid") - logfilePath.IndexOf("database=") - 9);
                                serverUser = logfilePath.Substring(logfilePath.IndexOf("Uid=") + 4, logfilePath.IndexOf(";Pwd") - logfilePath.IndexOf("Uid=") - 4);
                                serverPassword = logfilePath.Substring(logfilePath.IndexOf("Pwd=") + 4, logfilePath.LastIndexOf(";") - logfilePath.IndexOf("Pwd=") - 4);
                                textEdit1.Text = serverIP;
                                textEdit2.Text = serverDB;
                                textEdit3.Text = serverUser;
                                textEdit4.Text = serverPassword;
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("加载配置失败,详细见错误日志！");
                LogHelper.Error("LogManage-LogManage_Load" + ex.Message + ex.StackTrace);
            }
        }
    }
}
