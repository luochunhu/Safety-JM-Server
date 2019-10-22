using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Data;
using System.Windows.Forms;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Config;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Basic.Framework.Logging;

namespace Sys.Safety.ClientFramework.CBFCommon
{
    /// <summary>
    /// 主控管理静态操作类
    /// </summary>
    public class MasterManagement
    {
        private static IConfigService configService = ServiceFactory.Create<IConfigService>();
        /// <summary>
        /// 获取本机MAC地址(活动网卡)
        /// </summary>
        /// <returns></returns>
        public static string getMAC()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                //if (!string.IsNullOrEmpty(mo["MacAddress"].ToString()))
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }
            if (mac == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("获取当前主机MAC地址失败，请检查主机网络配置！");
            }
            moc = null;
            mc = null;
            return mac;
        }
        /// <summary>
        /// 判断mac是否在本机存在
        /// </summary>
        /// <param name="mac"></param>
        private static bool MACinComputer(string mac)
        {
            bool rvalue = false;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    if (mo["MacAddress"].ToString().ToLower() == mac.ToLower())
                    {
                        rvalue = true;
                        break;
                    }
                }
            }
            moc = null;
            mc = null;
            return rvalue;
        }
        /// <summary>
        /// 升为主控
        /// </summary>
        /// <returns></returns>
        public static void UpdateMaster()
        {
            string mac = "";
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("升本机为主控？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    mac = getMAC();
                    if (string.IsNullOrEmpty(mac)) {
                        return;
                    }
                    ConfigGetByNameRequest ConfigRequest = new ConfigGetByNameRequest();
                    ConfigRequest.Name = "MasterMAC";
                    var result1 = configService.GetConfigByName(ConfigRequest);
                    ConfigInfo MyConfigInfo = result1.Data;
                    if (MyConfigInfo != null)
                    {
                        if (mac != "")
                        {
                            ConfigInfo configdto = new ConfigInfo();
                            configdto.ID = MyConfigInfo.ID;
                            configdto.Name = "MasterMAC";
                            configdto.Text = mac;
                            configdto.Upflag = "0";
                            ConfigUpdateRequest configrequest = new ConfigUpdateRequest();
                            configrequest.ConfigInfo = configdto;
                            configService.UpdateConfig(configrequest);
                        }
                    }
                    else
                    {
                        if (mac != "")
                        {
                            ConfigInfo configdto = new ConfigInfo();
                            configdto.ID =  IdHelper.CreateLongId().ToString();
                            configdto.Name = "MasterMAC";
                            configdto.Text = mac;
                            configdto.Upflag = "0";
                            ConfigAddRequest configrequest = new ConfigAddRequest();
                            configrequest.ConfigInfo = configdto;
                            configService.AddConfig(configrequest);
                        }
                    }
                    DevExpress.XtraEditors.XtraMessageBox.Show("成功升为主控！");
                    //写操作日志
                    OperateLogHelper.InsertOperateLog(15, "成功升为主控", "");
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("升为主控失败,错误详细见日志文件！");
                LogHelper.Error("UpdateMasterd" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 降为热备
        /// </summary>
        /// <returns></returns>
        public static void DelMaster()
        {
            string mac = "";
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("降本机为非主控？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ConfigGetByNameRequest ConfigRequest = new ConfigGetByNameRequest();
                    ConfigRequest.Name = "MasterMAC";
                    var result1 = configService.GetConfigByName(ConfigRequest);
                    ConfigInfo MyConfigInfo = result1.Data;
                    if (MyConfigInfo != null)
                    {
                        mac = MyConfigInfo.Text;
                        if (MACinComputer(mac))
                        {
                            ConfigInfo configdto = new ConfigInfo();
                            configdto.ID = MyConfigInfo.ID;
                            configdto.Name = "MasterMAC";
                            configdto.Text = "";
                            configdto.Upflag = "0";
                            ConfigUpdateRequest configrequest = new ConfigUpdateRequest();
                            configrequest.ConfigInfo = configdto;
                            configService.UpdateConfig(configrequest);
                            DevExpress.XtraEditors.XtraMessageBox.Show("成功将当前主机置为非主控状态！");
                            //写操作日志
                            OperateLogHelper.InsertOperateLog(15, "成功降为热备", "");
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("操作失败,当前主机已处于非主控状态！");
                        }
                    }
                    else
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("操作失败,当前主机已处于非主控状态！");
                    }
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("将当前主机置为非主控状态失败,错误详细见日志文件！");
                LogHelper.Error("DelMaster" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 判断当前计算机是否为主控
        /// </summary>
        public static int IsMaster()
        {
            int rvalue = 0;
            string mac = "";
            try
            {
                ConfigGetByNameRequest ConfigRequest = new ConfigGetByNameRequest();
                ConfigRequest.Name = "MasterMAC";
                var result1 = configService.GetConfigByName(ConfigRequest);
                ConfigInfo MyConfigInfo = result1.Data;
                if (MyConfigInfo != null)
                {
                    mac = MyConfigInfo.Text;
                    if (MACinComputer(mac))
                    {
                        rvalue = 0;//正常
                    }
                    else
                    {
                        rvalue = 1;//当前非主控电脑,请确认本机是否为主控并检查本机网络是否正常
                    }
                }
                else
                {
                    rvalue = 2;//连接服务器失败,请检查网络是否正常
                }
            }
            catch (Exception ex)
            {
                rvalue = 3;//获取当前主机是否为主控主机失败，详细见日志
                LogHelper.Error("IsMaster" + ex.Message + ex.StackTrace);
            }
            return rvalue;
        }

        /// <summary>
        /// 退出当前服务端程序  20180123
        /// </summary>
        public static void ExitServer()
        {
            try
            {
                configService.ExitServer();



            }
            catch (Exception ex)
            {
                //LogHelper.Error("ExitServer" + ex.Message + ex.StackTrace);
            }
            DevExpress.XtraEditors.XtraMessageBox.Show("主备机切换成功！");
            //写操作日志
            OperateLogHelper.InsertOperateLog(15, "进行主备机切换", "");
        }
    }
}
