using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml ;
using System.IO ;

namespace Sys.Safety.Client.Display
{
    public  class XmlConfig
    {
        private XmlDocument xmlConfigDoc = new XmlDocument();
        private string xmlDocName = "";
        
        public XmlConfig(string _xmlDocName)
        {
            try
            {
                string str2;
                this.xmlDocName = _xmlDocName;
                string path = "";
                str2 = _xmlDocName;
                path = str2.Substring(0, str2.LastIndexOf('\\') + 1);
                if (!File.Exists(str2))
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    this.createXml(str2);
                    this.xmlConfigDoc.Load(str2);
                }
                else
                {
                    try
                    {
                        this.xmlConfigDoc.Load(str2);
                    }
                    catch (Exception)
                    {
                        this.createXml(str2);
                        this.xmlConfigDoc.Load(str2);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        
        private void createXml(string xmlDocPath)
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter(xmlDocPath, Encoding.UTF8) {
                    Formatting = Formatting.Indented
                };
                writer.WriteStartDocument();
                writer.WriteStartElement("DataAccess");
                writer.WriteStartElement("appSettings");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        
        public string GetValue(string key)
        {
            string str;
            try
            {
                string xpath = "//DataAccess/appSettings/add[@key='" + key + "']";
                XmlNodeList list = this.xmlConfigDoc.SelectNodes(xpath);
                if (list.Count == 1)
                {
                    XmlElement element = (XmlElement) list[0];
                    return element.GetAttribute("value");
                }
                if (list.Count == 0)
                {
                    throw new Exception("xmlConfigDoc配置信息设置错误：没有键值为" + key + "的元素");
                }
                throw new Exception("xmlConfigDoc配置信息设置错误：键值为" + key + "的元素有多个");
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }
        
        public void SavaConfig(string strKey, string strValue)
        {
            try
            {
                XmlNodeList elementsByTagName = this.xmlConfigDoc.GetElementsByTagName("add");
                bool flag = false;
                for (int i = 0; i < elementsByTagName.Count; i++)
                {
                    if (elementsByTagName[i].Attributes[0].Value == strKey)
                    {
                        elementsByTagName[i].Attributes[1].Value = strValue;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    XmlNode node = this.xmlConfigDoc.SelectSingleNode("//DataAccess/appSettings");
                    XmlElement newChild = this.xmlConfigDoc.CreateElement("add");
                    newChild.SetAttribute("key", strKey);
                    newChild.SetAttribute("value", strValue);
                    if (null != node)
                    {
                        node.AppendChild(newChild);
                    }
                }
                this.xmlConfigDoc.Save(this.xmlDocName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
