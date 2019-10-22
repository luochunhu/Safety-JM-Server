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
    public partial class Step5 : Form
    {
        Installer installer = new Installer5();
        public Step5()
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
        private void Step5_Load(object sender, EventArgs e)
        {
            foreach (var item in installer.StartServices)
            {
                checkedStartItems.Items.Add(item.Name, false);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedStartItems.Items.Count; i++)
            {
                InstallItem installItem = installer.StartServices.FirstOrDefault(q => q.Name == checkedStartItems.GetItemText(checkedStartItems.Items[i]));
                installItem.IsSelected = checkedStartItems.GetItemChecked(i);
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

        private void Step5_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                ParentWindow.Next();//手动启动服务
                //if (installer.StartServices.Count == 0)
                //    ParentWindow.Next();
            }
        }
    }
}
