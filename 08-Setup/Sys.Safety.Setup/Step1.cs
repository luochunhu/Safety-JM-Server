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
    public partial class Step1 : Form
    {
        Installer installer = new Installer1();
        public Step1()
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

        private void Step1_Load(object sender, System.EventArgs e)
        {
            var installItems = installer.InstallModel.InstallItems.OrderBy(q => q.DisplayOrder).ToList();
            for (int i = 0; i < installItems.Count; i++)
            {

                checkedInstallItems.Items.Add(installItems[i].Name);
                if (installItems[i].IsSelected)
                    checkedInstallItems.SetItemCheckState(i, CheckState.Checked);
                else
                    checkedInstallItems.SetItemCheckState(i, CheckState.Indeterminate);
            }
        }

        //private void SetTotalStep()
        //{
        //    for (int i = 0; i < checkedInstallItems.Items.Count; i++)
        //    {
        //        if(checkedInstallItems.GetItemText(checkedInstallItems.Items[i]) == "大数据分析" && checkedInstallItems.GetItemCheckState(i) == CheckState.Checked)
        //            Installer.TotalStep = Installer.TotalStep > 4 ? Installer.TotalStep : 4;
        //        if (checkedInstallItems.GetItemText(checkedInstallItems.Items[i]) == "网关程序" && checkedInstallItems.GetItemCheckState(i) == CheckState.Checked)
        //            Installer.TotalStep = Installer.TotalStep > 5 ? Installer.TotalStep : 5;
        //        if (checkedInstallItems.GetItemText(checkedInstallItems.Items[i]) == "服务端程序" && checkedInstallItems.GetItemCheckState(i) == CheckState.Checked)
        //            Installer.TotalStep = Installer.TotalStep > 6 ? Installer.TotalStep : 6;
        //    }
        //}

        private void btnNext_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedInstallItems.Items.Count; i++)
            {
                InstallItem installItem = installer.InstallModel.InstallItems.FirstOrDefault(q => q.Name == checkedInstallItems.GetItemText(checkedInstallItems.Items[i]));
                installItem.IsSelected = checkedInstallItems.GetItemChecked(i) && checkedInstallItems.GetItemCheckState(i) != CheckState.Indeterminate;
            }
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

        private void btnDirectoryBrower_Click(object sender, EventArgs e)
        {
            if(installFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtInstallFolder.Text = installFolderDialog.SelectedPath;
                installer.BaseFolder = txtInstallFolder.Text;
            }
        }

        private void btnLicenceBrower_Click(object sender, EventArgs e)
        {
            if(openLicenceFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtLicencePath.Text = openLicenceFileDialog.FileName;
                installer.LicenceFilePath = txtLicencePath.Text;
            }
        }

        private void btnGetMachineCode_Click(object sender, EventArgs e)
        {
            new MachineCode().Show();
        }

        private void checkedInstallItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Indeterminate)
            {
                e.NewValue = CheckState.Indeterminate;
            }
        }
    }
}
