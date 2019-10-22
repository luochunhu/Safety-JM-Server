using Sys.Safety.Setup.Install;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Setup
{
    public partial class Step3 : Form
    {
        Installer installer = new Installer3();
        public Step3()
        {
            InitializeComponent();
            this.Tag = installer;
        }
        private SetupMain ParentWindow
        {
            get
            {
                return (SetupMain)this.MdiParent;
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var group in installer.ConfigModel.ConfigGroup)
                {
                    if (group.Key == "server")
                    {
                        if (txtDBIP.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("数据库IP不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (!Basic.Framework.Common.ValidationHelper.IsRightIP(txtDBIP.Text))
                        {
                            MessageBox.Show("数据库IP格式不对。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtDBUser.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("数据库用户名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtDBPassword.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("数据库密码不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (txtDBName.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("数据库名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        group.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value = txtDBIP.Text;
                        group.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value = txtDBUser.Text;
                        group.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value = txtDBPassword.Text;
                        group.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value = txtDBName.Text;
                    }
                    if (group.Key == "gateway")
                    {
                        if (txtVirtualIP.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("虚拟IP不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (!Basic.Framework.Common.ValidationHelper.IsRightIP(txtVirtualIP.Text))
                        {
                            MessageBox.Show("虚拟IP格式不对。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        group.ConfigItems.FirstOrDefault(q => q.Key == "NetServerIp").Value = txtVirtualIP.Text;
                    }
                    if (group.Key == "analysis")
                    {
                        if (txtAnalysisServerSideIP.Text.Trim() == string.Empty)
                        {
                            MessageBox.Show("服务端IP不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (!Basic.Framework.Common.ValidationHelper.IsRightIP(txtAnalysisServerSideIP.Text))
                        {
                            MessageBox.Show("服务端IP格式不对。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        group.ConfigItems.FirstOrDefault(q => q.Key == "ServerIp").Value = txtAnalysisServerSideIP.Text;
                    }
                }

                installer.BeforeNext();
                ParentWindow.Next();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        bool serviceSelected = false;
        bool gatewaySelected = false;
        bool analysisSelected = false;


        private void Step3_Load(object sender, EventArgs e)
        {
            serviceSelected = installer.InstallModel.InstallItems.Any(q => q.IsSelected && q.ServiceName == "SafetyCoreService");
            gatewaySelected = installer.InstallModel.InstallItems.Any(q => q.IsSelected && q.ServiceName == "DataCollectionService");
            analysisSelected = installer.InstallModel.InstallItems.Any(q => q.IsSelected && q.ServiceName == "DataAnalysisService");
            groupDBConfig.Visible = serviceSelected;
            groupVirtualIPConfig.Visible = gatewaySelected;
            groupDataAnalysisIPConfig.Visible = analysisSelected && !serviceSelected;
            foreach (var group in installer.ConfigModel.ConfigGroup)
            {
                if (group.Key == "server")
                {
                    txtDBIP.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
                    txtDBUser.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
                    txtDBPassword.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
                    txtDBName.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;
                }
                if (group.Key == "gateway")
                {
                    txtVirtualIP.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "NetServerIp").Value;
                }
                if (group.Key == "analysis")
                {
                    txtAnalysisServerSideIP.Text = group.ConfigItems.FirstOrDefault(q => q.Key == "ServerIp").Value;
                }
            }
        }

        private void Step3_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (!serviceSelected && !gatewaySelected && !analysisSelected)
                {
                    ParentWindow.Next();
                }
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string strConnectString = string.Format(" Server={0};Database={1};User ID={2};Password={3};", txtDBIP.Text, "information_schema", txtDBUser.Text, txtDBPassword.Text);
            if (PubClass.BlnConnection(strConnectString))
            {
                MessageBox.Show("连接成功！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("连接失败！ 请检查数据库IP，用户名，密码是否正确！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
