using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Uninstall
{
    public partial class Step1 : Form
    {
        Uninstaller uninstaller = new Uninstaller1();
        public Step1()
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedUninstallItems.Items.Count; i++)
            {
                UninstallItem installItem = uninstaller.UninstallModel.UninstallItems.FirstOrDefault(q => q.DisplyName == checkedUninstallItems.GetItemText(checkedUninstallItems.Items[i]));
                installItem.IsSelected = checkedUninstallItems.GetItemChecked(i) && checkedUninstallItems.GetItemCheckState(i) != CheckState.Indeterminate;
            }
            try
            {
                uninstaller.BeforeNext();
                ParentWindow.Next();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Step1_Load(object sender, EventArgs e)
        {
            var uninstallItems = uninstaller.UninstallModel.UninstallItems;
            for (int i = 0; i < uninstallItems.Count; i++)
            {
                checkedUninstallItems.Items.Add(uninstallItems[i].DisplyName);
                if (uninstallItems[i].IsSelected)
                    checkedUninstallItems.SetItemCheckState(i, CheckState.Checked);
            }
        }
    }
}
