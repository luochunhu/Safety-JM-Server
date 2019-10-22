using Sys.Safety.Setup.Install;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Setup
{
    public partial class Step2 : Form
    {
        Installer installer = new Installer2();
        public Step2()
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

        Thread refreshThread;
        private void Step2_Load(object sender, EventArgs e)
        {
            refreshThread = new Thread(new ThreadStart(BindInstallItems));
            refreshThread.IsBackground = true;
            refreshThread.Start();
        }

        private List<ListViewItem> listView = new List<ListViewItem>();

        private void BindInstallItems()
        {
            while(!this.IsDisposed)
            {
                MethodInvoker In = new MethodInvoker(() =>
                {
                    foreach (var item in Installer.SelectedItems)
                    {
                        if (item.Type == "install")
                        {
                            if (item.RunType == "service")
                                item.IsInstalled = installer.IsWindowsServiceInstalled(item.ServiceName) && installer.GetService(item.ServiceName).Status == System.ServiceProcess.ServiceControllerStatus.Running;
                            else
                                item.IsInstalled = installer.GetUninstallList().Exists(q => q.StartsWith(item.ApplicationName));
                        }
                        int imageIndex = item.IsInstalled ? 1 : item.Installing ? 0 : -1;                  
                        var listViewItem = listView.FirstOrDefault(q => q.SubItems[1].Text == item.Name);
                        if (listViewItem == null)
                        {
                            listViewItem = new ListViewItem("", imageIndex);
                            listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.Name));
                            listView.Add(listViewItem);
                            listView1.Items.Add(listViewItem);
                        }
                        else
                        {
                            if (imageIndex >= 0)
                                listViewItem.ImageIndex = imageIndex;
                        }
                    }
                    if (!Installer.SelectedItems.Any(q => q.IsInstalled == false))
                    { 
                        btnNext.Enabled = true;
                        label1.Text = "安装完成! 请点击下一步.";
                    }
                });
                if (listView1.InvokeRequired)
                    listView1.Invoke(In);
                Thread.Sleep(1000);
            }
        }

        private void Step2_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (null != refreshThread)
            {
                try
                {
                    refreshThread.Abort();
                }
                catch (Exception ex){}
            }   
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                installer.BeforeNext();
                ParentWindow.Next();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
