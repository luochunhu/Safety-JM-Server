using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Alarm
{
    public class ClientAlarmConfig
    {
        static XmlDocument _docoment = new XmlDocument();

        static string _filePath = string.Empty;

        /// <summary>
        /// 服务端是否连接正常
        /// </summary>
        public static bool serverisconnect = true;

        public static void setserverconnectstate(bool flg)
        {
            serverisconnect = flg;
        }

        /// <summary>
        /// 服务端连接状态
        /// </summary>
        /// <returns></returns>
        public static bool getsererconnectstate()
        {
            return serverisconnect;
        }

        public static string FilePath
        {
            get { return _filePath; }
        }

        static ClientAlarmConfig()
        {
            _docoment = new XmlDocument();
            
            _filePath = Application.StartupPath + "\\Config\\AlarmConfig\\AlarmConfig.xml";
        }

        /// <summary>
        /// 保存报警配置到本地文件
        /// </summary>
        /// <param name="flag">性质、种类、测点</param>
        /// <param name="dt"></param>
        public static bool SaveConfig(int flag, List<ClientAlarmItems> list)
        {
            bool b = true;
            try
            {
                _docoment.Load(_filePath);
                System.Xml.XmlNode node = null;
                switch (flag)
                {
                    case 1://性质
                        node = _docoment.SelectSingleNode("//AlarmSettings/PropertySetting");
                        break;
                    case 2://种类
                        node = _docoment.SelectSingleNode("//AlarmSettings/ClassSetting");
                        break;
                    case 3://测点
                        node = _docoment.SelectSingleNode("//AlarmSettings/PointSetting");
                        break;
                    case 4://设备类型
                        node = _docoment.SelectSingleNode("//AlarmSettings/DevSetting");
                        break;
                    default:
                        break;
                }
                //先删除代码相同的报警设置
                if (list.Count > 0)//增加判断  20180113
                {
                    for (int i = 0; i < node.ChildNodes.Count; )
                    {
                        if (node.ChildNodes[i].Attributes["code"].Value == list[0].code)
                        {
                            node.RemoveChild(node.ChildNodes[i]);
                            continue;
                        }
                        i++;
                    }
                }
                bool isContentFlag = false;
                foreach (var r in list)
                {
                    if (!string.IsNullOrEmpty(r.alarmShow) && r.alarmShow != "无设置")//增加报警配置判断，如果未配置报警，则不保存  20171228
                    {
                        isContentFlag = true;
                    }
                }
                if (isContentFlag)
                {
                    //再新建该代码的报警设置
                    foreach (var r in list)
                    {
                        XmlElement el = _docoment.CreateElement("Item");
                        el.SetAttribute("code", r.code);
                        el.SetAttribute("name", r.name);
                        el.SetAttribute("alarmType", r.alarmType);
                        el.SetAttribute("alarmCode", r.alarmCode);
                        el.SetAttribute("alarmShow", r.alarmShow == "无设置" ? "" : r.alarmShow);
                        node.AppendChild(el);
                    }
                }
                _docoment.Save(_filePath);
            }
            catch (Exception ex)
            {
                //写日志、抛异常等
                LogHelper.Error("保存报警配置到本地文件 发生异常", ex);
                b = false;
            }
            return b;
        }
        /// <summary>
        /// 保存声光报警设备的串口设置
        /// </summary>
        /// <returns></returns>
        public static bool SaveSoundLighPortSetting()
        {
            bool b = true;
            try
            {
                _docoment.Load(_filePath);
                System.Xml.XmlNodeList nodes = _docoment.SelectNodes("//AlarmSettings/SoundLightSetting/Item");
                foreach (XmlNode node in nodes)
                {
                    string kye = node.Attributes.GetNamedItem("key").Value;
                    switch (kye)
                    {
                        case "SoundLightPortName":
                            node.Attributes.GetNamedItem("value").Value = ClientAlarmConfigCache.soundLightPortName;
                            break;
                        case "SoundLightBaudRate":
                            node.Attributes.GetNamedItem("value").Value = ClientAlarmConfigCache.soundLightBaudRate.ToString();
                            break;
                    }
                }
                _docoment.Save(_filePath);
            }
            catch (Exception ex)
            {
                //写日志、抛异常等
                LogHelper.Error("保存报警配置到本地文件 发生异常", ex);
                b = false;
            }
            return b;
        }
        /// <summary>
        /// 保存其他报警开关设置
        /// </summary>
        /// <returns></returns>
        public static bool SaveOtherAlarmSwitch()
        {
            bool b = true;
            try
            {
                _docoment.Load(_filePath);
                System.Xml.XmlNodeList nodes = _docoment.SelectNodes("//AlarmSettings/OtherAlarmSwitch/Item");
                foreach (XmlNode node in nodes)
                {
                    string kye = node.Attributes.GetNamedItem("key").Value;
                    switch (kye)
                    {
                        case "IsUseAlarmConfig":
                            node.Attributes.GetNamedItem("value").Value = ClientAlarmConfigCache.IsUseAlarmConfig.ToString();
                            break;
                        case "IsUsePopupAlarm":
                            node.Attributes.GetNamedItem("value").Value = ClientAlarmConfigCache.IsUsePopupAlarm.ToString();
                            break;
                    }
                }
                _docoment.Save(_filePath);
            }
            catch (Exception ex)
            {
                //写日志、抛异常等
                LogHelper.Error("保存报警配置到本地文件 发生异常", ex);
                b = false;
            }
            return b;
        }
        /// <summary>
        /// 保存报警配置文件到数据库
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfigToServer()
        {
            bool b = true;
            string s = string.Empty;
            try
            {
                if (File.Exists(_filePath))
                {
                    Stream stream = File.Open(_filePath, FileMode.Open);
                    using(stream)
                    {
                        byte[] by = new byte[stream.Length];
                        stream.Read(by, 0, (int)stream.Length);
                        s = Convert.ToBase64String(by);
                    }
                     SettingInfo dto = ClientAlarmServer.CheckAlarmConfigIsOnServer();
                    if (!ClientAlarmServer.SaveConfigToDatabase(dto,s))
                    {
                        b = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //写日志、抛异常等
                LogHelper.Error("保存报警配置文件到数据库 发生异常", ex);
                b = false;
            }
            return b;
        }
        /// <summary>
        /// 加载配置文件到客户端缓存
        /// </summary>
        public static void LoadConfigToCache()
        {
            try
            {
                ClientAlarmConfigCache.listProperty = GetOneConfigList("//AlarmSettings/PropertySetting");

                ClientAlarmConfigCache.listClass = GetOneConfigList("//AlarmSettings/ClassSetting");

                ClientAlarmConfigCache.listDev = GetOneConfigList("//AlarmSettings/DevSetting");

                ClientAlarmConfigCache.listPoint = GetOneConfigList("//AlarmSettings/PointSetting");

                GetSoundLightAlarmSetting();

                GetOtherAlarmSwitchSetting();
            }
            catch (Exception ex)
            {
                //写日志、抛异常
                LogHelper.Error("加载配置文件到客户端缓存 发生异常", ex);
            }
        }
        /// <summary>
        /// 从本地配置文件读取声光报警设置到客户端缓存
        /// </summary>
        private static void GetSoundLightAlarmSetting()
        {
            _docoment.Load(_filePath);
            System.Xml.XmlNodeList nodes = _docoment.SelectNodes("//AlarmSettings/SoundLightSetting/Item");
            foreach (XmlNode node in nodes)
            {
                string key = node.Attributes.GetNamedItem("key").Value;
                switch (key)
                {
                    case "SoundLightPortName":
                        ClientAlarmConfigCache.soundLightPortName = node.Attributes.GetNamedItem("value").Value;
                        break;
                    case "SoundLightBaudRate":
                        int iBaudRate = 9600;
                        bool bIsOk = int.TryParse(node.Attributes.GetNamedItem("value").Value, out iBaudRate);
                        ClientAlarmConfigCache.soundLightBaudRate = iBaudRate;
                        break;
                }
            }
        }
        /// <summary>
        /// 从本地配置文件读取其他报警开关设置到客户端缓存
        /// </summary>
        private static void GetOtherAlarmSwitchSetting()
        {
            _docoment.Load(_filePath);
            System.Xml.XmlNodeList nodes = _docoment.SelectNodes("//AlarmSettings/OtherAlarmSwitch/Item");
            foreach (XmlNode node in nodes)
            {
                string key = node.Attributes.GetNamedItem("key").Value;
                switch (key)
                {
                    case "IsUseAlarmConfig":
                        bool bIsUseAlarmConfig = true;
                        bool bIsOk = bool.TryParse(node.Attributes.GetNamedItem("value").Value, out bIsUseAlarmConfig);
                        ClientAlarmConfigCache.IsUseAlarmConfig = bIsUseAlarmConfig;
                        break;
                    case "IsUsePopupAlarm":
                        bool isUsePopupAlarm = true;
                        bool.TryParse(node.Attributes.GetNamedItem("value").Value, out isUsePopupAlarm);
                        ClientAlarmConfigCache.IsUsePopupAlarm = isUsePopupAlarm;
                        break;
                }
            }
        }
        /// <summary>
        /// 从服务端下载报警配置到本地
        /// </summary>
        /// <returns></returns>
        public static bool DownloadConfigFromServer()
        {
            bool b = false;
            try
            {
                SettingInfo dto = ClientAlarmServer.CheckAlarmConfigIsOnServer();
                if (!string.IsNullOrEmpty(dto.StrValue))
                {
                    byte[] by = Convert.FromBase64String(dto.StrValue);
                    Stream stream = File.Open(_filePath, FileMode.Create, FileAccess.ReadWrite);
                    using (stream)
                    {
                        stream.Write(by, 0, by.Length);
                        stream.Close();
                    }
                    b = true;
                }
            }
            catch (Exception ex)
            {
                b = false;
                //写日志、抛异常
                LogHelper.Error("从服务端下载报警配置到本地 发生异常", ex);
            }
            return b;
        }
        /// <summary>
        /// 获取某种配置的所有配置项列表
        /// </summary>
        /// <param name="sConfigNodeTag">某种配置的节点标签</param>
        /// <returns></returns>
        private static List<ClientAlarmItems> GetOneConfigList(string sConfigNodeTag)
        {
            List<ClientAlarmItems> listItem = new List<ClientAlarmItems>();
            try
            {
                _docoment.Load(_filePath);
                System.Xml.XmlNode node = _docoment.SelectSingleNode(sConfigNodeTag);
                foreach (XmlNode n in node.ChildNodes)
                {
                    ClientAlarmItems i = new ClientAlarmItems();
                    i.code = n.Attributes["code"].Value;
                    i.name = n.Attributes["name"].Value;
                    i.alarmType = n.Attributes["alarmType"].Value;
                    i.alarmCode = n.Attributes["alarmCode"].Value;
                    i.alarmShow = n.Attributes["alarmShow"].Value;
                    listItem.Add(i);
                }
                return listItem;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取某种配置的所有配置项列表 发生异常", ex);
                //throw;
                return listItem;
            }
        }
    }
}
