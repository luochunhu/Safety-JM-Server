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
    public partial class Step6 : Form
    {
        Installer installer = new Installer6();
        public Step6()
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

        private string ActiveConnectionString
        {
            get
            {
                return string.Format(" Server={0};Database={1};User ID={2};Password={3};", txtLIP.Text, txtLDatabase.Text, txtLDbUserName.Text, txtLDbPassword.Text);
            }
        }

        private string StandbyConnectionString
        {
            get
            {
                return string.Format(" Server={0};Database={1};User ID={2};Password={3};", txtRIP.Text, txtRDatabase.Text, txtRDbUserName.Text, txtRDbPassword.Text);
            }
        }
        private void btnLTest_Click(object sender, EventArgs e)
        {
            if (PubClass.BlnConnection(ActiveConnectionString))
            {
                MessageBox.Show("连接成功！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("连接失败！请检查是否安装了MySQL以及配置是否正确.", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRTest_Click(object sender, EventArgs e)
        {
            if (PubClass.BlnConnection(StandbyConnectionString))
            {
                MessageBox.Show("连接成功！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("连接失败！ 请检查是否安装了MySQL以及配置是否正确.", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (txtLIP.Text.Trim() == string.Empty)
            {
                MessageBox.Show("主机的数据库IP不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsRightIP(txtLIP.Text))
            {
                MessageBox.Show("主机的数据库IP格式不对。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtLDbUserName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("主机的数据库用户名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtLDbPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("主机的数据库密码不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtLDatabase.Text.Trim() == string.Empty)
            {
                MessageBox.Show("主机的数据库名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtRIP.Text.Trim() == string.Empty)
            {
                MessageBox.Show("备机的数据库IP不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsRightIP(txtRIP.Text))
            {
                MessageBox.Show("备机的数据库IP格式不对。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtRDbUserName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("备机的数据库用户名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtRDbPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("备机的数据库密码不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtRDatabase.Text.Trim() == string.Empty)
            {
                MessageBox.Show("备机的数据库名不能为空", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(txtLDatabase.Text.ToLower().Trim() != txtRDatabase.Text.ToLower().Trim())
            {
                MessageBox.Show("请配置主机数据库名和备机数据库名一致!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            /*主机数据库链接测试*/
            if (!PubClass.BlnConnection(ActiveConnectionString))
            {
                MessageBox.Show("主数据库链接失败! 请检查是否安装了MySQL以及配置是否正确.", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            /*备机数据库链接测试*/
            if (!PubClass.BlnConnection(StandbyConnectionString))
            {
                MessageBox.Show("备数据库链接失败! 请检查是否安装了MySQL以及配置是否正确.", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            ConfigGroup activeConfig = installer.ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "active");
            ConfigGroup standbyConfig = installer.ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "standby");
            if (activeConfig != null)
            {
                activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value = txtLIP.Text;
                activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value = txtLDbUserName.Text;
                activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value = txtLDbPassword.Text;
                activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value = txtLDatabase.Text;
            }
            if (standbyConfig != null)
            {
                standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value = txtRIP.Text;
                standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value = txtRDbUserName.Text;
                standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value = txtRDbPassword.Text;
                standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value = txtRDatabase.Text;
            }

            try
            {
                btnNext.Enabled = false;
                installer.BeforeNext();
                ParentWindow.Next();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                btnNext.Enabled = true;
            }
        }

        private void Step6_Shown(object sender, EventArgs e)
        {
            if (!installer.InstallModel.InstallItems.Exists(q => q.IsSelected && q.ServiceName == "BasicHAService"))
                ParentWindow.Next();
        }

        private void Step6_Load(object sender, EventArgs e)
        {
            ConfigGroup activeConfig = installer.ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "active");
            ConfigGroup standbyConfig = installer.ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "standby");
            if (activeConfig != null)
            {
                txtLIP.Text = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
                txtLDbUserName.Text = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
                txtLDbPassword.Text = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
                txtLDatabase.Text = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;
            }
            if (standbyConfig != null)
            {
                txtRIP.Text = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
                txtRDbUserName.Text = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
                txtRDbPassword.Text = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
                if (txtLDatabase.Text != "")
                    txtRDatabase.Text = txtLDatabase.Text;
                else
                    txtRDatabase.Text = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;
            }
        }
    }
}
