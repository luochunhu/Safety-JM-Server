using Basic.Framework.Logging;
using DevExpress.XtraEditors;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Client.Define.Model;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Define.Station
{
    public partial class StationIPSet : XtraForm
    {
        string Mac { get; set; }
        string Fzh { get; set; }

        public bool isSerach = false;
        public StationIPSet()
        {
            InitializeComponent();
        }

        public StationIPSet(string mac, string fzh)
        {
            Mac = mac;
            Fzh = fzh;
            InitializeComponent();
        }

        private void barButtonISave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!Basic.Framework.Common.ValidationHelper.IsRightIP(ipTxt.Text))
                {
                    XtraMessageBox.Show("分站IP输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(ymTxt.Text))
                {
                    XtraMessageBox.Show("子网掩码输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Basic.Framework.Common.ValidationHelper.IsRightIP(gatewayTxt.Text))
                {
                    XtraMessageBox.Show("网关地址输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbSrvPort.Text))
                //{
                //    XtraMessageBox.Show("服务端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbModulePort.Text))
                //{
                //    XtraMessageBox.Show("模块端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                NetDeviceSettingInfo pConvSetting = new NetDeviceSettingInfo();
                pConvSetting.SockSetting = new SocketSetting[8];
                pConvSetting.ComSetting = new ComSetting[8];

                //目前主要设置交换机的IP、子网掩码、网关信息
                pConvSetting.NetSetting = new Netsetting();
                pConvSetting.NetSetting.IpAddr = ipTxt.Text; //模块 IP
                pConvSetting.NetSetting.SubMask = ymTxt.Text;//模块 子网掩码
                pConvSetting.NetSetting.GatewayIp = gatewayTxt.Text; //模块 网关
                pConvSetting.NetSetting.SetFzh = byte.Parse(fzhTxt.Text);//分站号

             
                pConvSetting.NetSetting.srcPacket = null;
                pConvSetting.NetSetting.IsUseStaticIP = 1;

                pConvSetting.NetSetting.DnsIp = "";//模块 DNS
                pConvSetting.NetSetting.NetWorkName = "";//模块名称
                pConvSetting.NetSetting.User = "";//用户名
                pConvSetting.NetSetting.PassWord = "";//密码  

                if (Model.MACServiceModel.SetConvSetting(macTxt.Text, pConvSetting, 8000, "1"))
                {
                    //CcmbIP.Text = CtxbModuleIP.Text;
                    //设置网络模块参数成功的同时，更新缓存及数据库的模块IP地址  20171220
                    //if (CtxbModuleIP.Text != ModuleIPNow)
                    //{
                    //    //更新网络模块IP地址
                    //    Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                    //    if (ExistIPModule != null)
                    //    {
                    //        //表示更新  
                    //        ExistIPModule.IP = CtxbModuleIP.Text;
                    //        ExistIPModule.InfoState = InfoState.Modified;
                    //        try
                    //        {
                    //            Model.MACServiceModel.UpdateMACCache(ExistIPModule);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //            return;
                    //        }
                    //        ModuleIPNow = CtxbModuleIP.Text;
                    //    }
                    //}
                    isSerach = true;
                    XtraMessageBox.Show("设置网络设备参数成功！", "提示");                  
                }
                else
                {                   
                    XtraMessageBox.Show("设置网络设备参数失败！", "警告");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新模块信息【barButtonISave_ItemClick】", ex);
            }
        }

        private void barButtonCancle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void StationIPSet_Load(object sender, EventArgs e)
        {
            macTxt.Text = Mac;
            fzhTxt.Text = Fzh;
            Jc_MacInfo mac = MACServiceModel.QueryMACByCode(Mac);
            if (mac != null)
            {
                ipTxt.Text = mac.IP;
                ymTxt.Text = mac.SubMask;
                gatewayTxt.Text = mac.GatewayIp;
            }
        }


    }
}
