using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Sys.Safety.ClientFramework.Configuration
{
    public class ConfigHelper
    {
        /// <summary>
        /// 登录时读取配置
        /// </summary>
        public static string LogOn = "Config";
        /// <summary>
        /// 登录配置文件名称
        /// </summary>
        public static string FileName
        {
            get
            {
                return LogOn + ".xml";
            }
        }
        /// <summary>
        /// 选择路径
        /// </summary>
        public static string SelectPath = "//appSettings/add";
        /// <summary>
        /// 全路径
        /// </summary>
        public static string ConfigFielName
        {
            get
            {               
                return Application.StartupPath + "\\Config\\" + FileName;
            }
        }

        #region 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string key)
        {
            return GetValue(ConfigFielName, SelectPath, key);
        }
        #endregion

        #region 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string fileName, string key)
        {
            return GetValue(fileName, SelectPath, key);
        }
        #endregion

        #region 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        /// <param name="selectPath">查询条件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string fileName, string selectPath, string key)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            return GetValue(xmlDocument, selectPath, key);
        }
        #endregion

        #region 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="xmlDocument">配置文件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(XmlDocument xmlDocument, string key)
        {
            return GetValue(xmlDocument, SelectPath, key);
        }
        #endregion

        #region 设置配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="xmlDocument">配置文件</param>
        /// <param name="selectPath">查询条件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(XmlDocument xmlDocument, string selectPath, string key)
        {
            string returnValue = string.Empty;
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    returnValue = xmlNode.Attributes["value"].Value;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region 读取配置文件
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static void GetConfig()
        {
            GetConfig(ConfigFielName);
        }
        #endregion

        #region 从指定的文件读取配置项
        /// <summary>
        /// 从指定的文件读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        public static void GetConfig(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
             
            BaseInfo.CustomerCompanyName = GetValue(xmlDocument, BaseConfig.COMPANYNAME);
            BaseInfo.SoftName = GetValue(xmlDocument, BaseConfig.SOFTNAME);
            BaseInfo.SoftFullName = GetValue(xmlDocument, BaseConfig.SOFTFULLNAME);
            BaseInfo.Version = GetValue(xmlDocument, BaseConfig.VERSION);
            BaseInfo.AutoLogoIn = GetValue(xmlDocument, BaseConfig.AUTOLOGOIN);
            BaseInfo.AutoLogoUser = GetValue(xmlDocument, BaseConfig.AUTOLOGOUSER);
            BaseInfo.AutoLogoPass = GetValue(xmlDocument, BaseConfig.AUTOLOGOPASS);
            BaseInfo.MenuType = GetValue(xmlDocument, BaseConfig.MENUTYPE);
            BaseInfo.GraphicDefine = GetValue(xmlDocument, BaseConfig.GRAPHICDEFINE);
        }
        #endregion

        #region 设置配置项
        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool SetValue(XmlDocument xmlDocument, string key, string keyValue)
        {
            return SetValue(xmlDocument, SelectPath, key, keyValue);
        }
        #endregion

        #region 设置配置项
        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="selectPath"></param>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool SetValue(XmlDocument xmlDocument, string selectPath, string key, string keyValue)
        {
            bool returnValue = false;
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    xmlNode.Attributes["value"].Value = keyValue;
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region 保存配置文件
        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void SaveConfig()
        {
            SaveConfig(ConfigFielName);
        }
        #endregion

        #region 保存到指定的文件
        /// <summary>
        /// 保存到指定的文件
        /// </summary>
        /// <param name="fileName">配置文件</param>
        public static void SaveConfig(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
             
            SetValue(xmlDocument, BaseConfig.COMPANYNAME, BaseInfo.CustomerCompanyName);
            SetValue(xmlDocument, BaseConfig.SOFTNAME, BaseInfo.SoftName);
            SetValue(xmlDocument, BaseConfig.SOFTFULLNAME, BaseInfo.SoftFullName);
            SetValue(xmlDocument, BaseConfig.VERSION, BaseInfo.Version);
            SetValue(xmlDocument, BaseConfig.AUTOLOGOIN, BaseInfo.AutoLogoIn);
            SetValue(xmlDocument, BaseConfig.AUTOLOGOUSER, BaseInfo.AutoLogoUser);
            SetValue(xmlDocument, BaseConfig.AUTOLOGOPASS, BaseInfo.AutoLogoPass);
            SetValue(xmlDocument, BaseConfig.MENUTYPE, BaseInfo.MenuType);

            xmlDocument.Save(fileName);
        }
        #endregion
    }
}
