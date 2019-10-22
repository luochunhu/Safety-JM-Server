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

namespace Sys.Safety.Uninstall
{
    public partial class Step2 : Form
    {
        Uninstaller uninstaller = new Uninstaller2();
        public Step2()
        {
            InitializeComponent();
            this.Tag = uninstaller;
        }

        private UninstallMain ParentWindow
        {
            get
            {
                return (UninstallMain)this.MdiParent;
            }
        }
        Thread refreshThread;

        private void Step2_Load(object sender, EventArgs e)
        {
            refreshThread = new Thread(new ThreadStart(BindUninstallItems));
            refreshThread.IsBackground = true;
            refreshThread.Start();
        }
        private List<ListViewItem> listView = new List<ListViewItem>();
        private void BindUninstallItems()
        {
            while (!this.IsDisposed)
            {
                MethodInvoker In = new MethodInvoker(() =>
                {
                    foreach (var item in Uninstaller.SelectedItems)
                    {
                        int imageIndex = item.IsUninstalled ? 1 : item.Uninstalling ? 0 : -1;
                        var listViewItem = listView.FirstOrDefault(q => q.SubItems[1].Text == item.DisplyName);
                        if (listViewItem == null)
                        {
                            listViewItem = new ListViewItem("", imageIndex);
                            listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.DisplyName));
                            listView.Add(listViewItem);
                            listView1.Items.Add(listViewItem);
                        }
                        else
                        {
                            if (imageIndex >= 0)
                                listViewItem.ImageIndex = imageIndex;
                        }
                    }
                    if (!Uninstaller.SelectedItems.Any(q => q.IsUninstalled == false))
                    {
                        btnDone.Enabled = true;
                        label1.Text = "卸载完成! 请点击完成按钮.";
                    }
                });
                if (listView1.InvokeRequired)
                    listView1.Invoke(In);
                Thread.Sleep(1000);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            StopThread();
            if (!string.IsNullOrEmpty(uninstaller.UninstallModel.InstallFolder))
                uninstaller.DeleteInstallFolderThenExit();
        }

        private void StopThread()
        {
            if (null != refreshThread)
            {
                try
                {
                    refreshThread.Abort();
                }
                catch (Exception ex) { }
            }
        }

        private void Step2_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            StopThread();
            if (!string.IsNullOrEmpty(uninstaller.UninstallModel.InstallFolder))
                uninstaller.DeleteInstallFolderThenExit();
        }
    }
}
