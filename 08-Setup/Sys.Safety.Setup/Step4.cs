using Sys.Safety.Setup.Install;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Setup
{
    public partial class Step4 : Form
    {
        Installer installer = new Installer4();
        public Step4()
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
        bool isComplete = false;
        bool inprogress = false;
        string dbIp = string.Empty;
        string dbUserName = string.Empty;
        string dbPassword = string.Empty;
        string dbName = string.Empty;
        public bool CheckValid()
        {
            ConfigGroup group = installer.ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "server");
            dbIp = group.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
            dbUserName = group.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
            dbPassword = group.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
            dbName = group.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;

            string strConnectString = string.Format(" Server={0};Database={1};User ID={2};Password={3};", dbIp, "information_schema", dbUserName, dbPassword);
            if (!PubClass.BlnConnection(strConnectString))
            {
                MessageBox.Show("数据库连接失败，请确认用户名密码是否输入正确！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string strsql = "SELECT SCHEMA_NAME FROM information_schema.SCHEMATA where SCHEMA_NAME='" + dbName + "'";
            object struser = MySqlHelper.ExecuteScalar(strConnectString, strsql);
            if (struser != null)
            {
                MessageBox.Show("数据库名称已存在,请返回上一步重新输入!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MethodInvoker In = new MethodInvoker(() =>
            {
                if (inprogress)
                {
                    btnPrev.Enabled = false;
                    btnNext.Enabled = false;
                }
                else
                {
                    btnNext.Enabled = isComplete;
                    btnPrev.Enabled = !isComplete;
                }
                progressBar1.Visible = inprogress;
                progressBar1.Enabled = inprogress;
            });
            try
            {
                inprogress = true;
                this.Invoke(In);
                if (this.CheckValid())
                {
                    string strFileName = Environment.CurrentDirectory + "\\Packages\\MySQL\\Data\\safety2019.bak";
                    string command = string.Format("mysql --host={0} --default-character-set=utf8 --port=3306 --user={1} --password={2} {3}<\"{4}\"", new object[]
                    {
                    dbIp,
                    dbUserName,
                    dbPassword,
                    dbName,
                    strFileName
                    });
                    string appDirecroty = Environment.CurrentDirectory + "\\Packages\\MySQL";
                    if (!File.Exists(appDirecroty + "\\mysql.exe"))
                    {
                        MessageBox.Show(string.Format("未找到mysql.exe应用程序,请确认目录:{0}下存在此文件", appDirecroty), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inprogress = false;
                        this.Invoke(In);
                    }
                    else
                    {
                        string strConnectString = string.Format(" Server={0};User ID={1};Password={2};", dbIp, dbUserName, dbPassword);
                        string strCreateDataBase = string.Format("CREATE DATABASE IF not exists {0};", dbName);
                        MySqlHelper.ExecuteNonQuery(strConnectString, strCreateDataBase, null);
                        MySqlHelper.ExecuteNonQuery(strConnectString, string.Format("GRANT ALL ON `database`.* TO '{0}'@'%' IDENTIFIED BY '{1}';flush privileges;", dbUserName, dbPassword), null);
                        string strOpenRight = string.Format("GRANT ALL PRIVILEGES ON *.* TO '{0}'@'%' WITH GRANT OPTION", dbUserName);
                        MySqlHelper.ExecuteNonQuery(strConnectString, strOpenRight, null);
                        Cmd.StartCmd(appDirecroty, command);
                        if (Cmd.bOK)
                        {
                            isComplete = true;
                            //MessageBox.Show("还原数据库" + dbName + "成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("还原数据库" + dbName + "失败,原因为:" + Cmd.Err, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        inprogress = false;
                        this.Invoke(In);
                    }
                }
                else
                {
                    inprogress = false;
                    this.Invoke(In);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("还原数据库失败,原因为:{0}", ex.Message), "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inprogress = false;
                this.Invoke(In);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(isComplete)
            {
                this.label1.Visible = false;
                this.label2.Visible = true;
                pictureBox1.Visible = true;
            }
        }

        private void Step4_Load(object sender, EventArgs e)
        {

        }

        private void Step4_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (!Installer.SelectedItems.Any(q => q.ServiceName == "SafetyCoreService"))
                {
                    ParentWindow.Next();
                    return;
                }
                if (isComplete)
                    ParentWindow.Next();
                else
                    this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            ParentWindow.Prev();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ParentWindow.Next();
        }
    }
}
