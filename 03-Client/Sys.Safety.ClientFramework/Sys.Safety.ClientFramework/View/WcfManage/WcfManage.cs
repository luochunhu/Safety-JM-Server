using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using Basic.Framework.Logging;
using System.Configuration;
using DevExpress.XtraEditors;

namespace Sys.Safety.ClientFramework.View.WcfManage
{
    public partial class WcfManage : XtraForm
    {
        public WcfManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        bool IsValidIp(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //获取新设置，设置到客户端全局缓存     
                string ip = this.txtServerUrl.Text.Trim();

                if (!IsValidIp(ip))
                {
                    XtraMessageBox.Show("请输入正确的IP地址。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtServerUrl.Focus();
                    return;
                }

                string port = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
                string url = string.Format("http://{0}:{1}", ip, port);

                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("ServerUrl"))
                {
                    Basic.Framework.Data.PlatRuntime.Items["ServerUrl"] = url;
                }
                else
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add("ServerUrl", url);
                }

                //更新配置文件
                UpdateAppSettings("", "ServerIp", ip);
                try
                {
                    UpdateAppSettings(Application.StartupPath + "\\Sys.Safety.Client.Graphic.exe", "ServerIp", ip);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
                try
                {
                    UpdateAppSettings(Application.StartupPath + "\\Graphic\\Sys.Safety.Client.Graphic.exe", "ServerIp", ip);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
                try
                {
                    UpdateAppSettings(Application.StartupPath + "\\Sys.Safety.Client.Define.exe", "ServerIp", ip);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
                try
                {
                    UpdateAppSettings(Application.StartupPath + "\\Sys.Safety.Client.Chart.exe", "ServerIp", ip);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }              
               

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (System.Exception ex)
            {
                LogHelper.Error("WcfManage-btnOk_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void WcfManage_Load(object sender, EventArgs e)
        {
            try
            {
                string MyFileName = Application.StartupPath + "\\Image\\Icon\\客户端.ico";
                if (File.Exists(MyFileName))
                {
                    this.Icon = new Icon(MyFileName);
                }
                //todo

            }
            catch (System.Exception ex)
            {
                LogHelper.Error("WcfManage-WcfManage_Load" + ex.Message + ex.StackTrace);
            }

            this.txtServerUrl.Text = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
        }

        private bool UpdateAppSettings(string appPath, string key, string value)
        {
            System.Configuration.Configuration _config = null;
            if (string.IsNullOrEmpty(appPath))
            {
                _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                _config = ConfigurationManager.OpenExeConfiguration(appPath);
            }
            if (!_config.HasFile)
            {
                throw new ArgumentException("程序配置文件缺失！");
            }
            KeyValueConfigurationElement _key = _config.AppSettings.Settings[key];
            if (_key == null)
                _config.AppSettings.Settings.Add(key, value);
            else
                _config.AppSettings.Settings[key].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);

            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            return true;
        }
    }
}
