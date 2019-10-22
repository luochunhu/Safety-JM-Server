using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Graphic
{
    public partial class WcfManage : RibbonForm
    {
        public WcfManage()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string ServiceIp=textEdit1.Text;
                //if (Basic.Framework.Utils.Generic.TypeValidation.IsRightIP(ServiceIp))
                //{
                //    Basic.Framework.Utils.Configurations.AppConfig.UpdateServiceModelConfig(Application.ExecutablePath, ServiceIp);
                //    DevExpress.XtraEditors.XtraMessageBox.Show("配置服务端IP成功，重启生效！");
                //}
                //else
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show("请输入正确的IP地址！");                   
                //}               
            }
            catch (System.Exception ex)
            {
                LogHelper.Error("WcfManage-simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
        }
    }
}
