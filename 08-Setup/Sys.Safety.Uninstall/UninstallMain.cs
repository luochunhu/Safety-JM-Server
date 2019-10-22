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
    public partial class UninstallMain : Form
    {
        List<Form> formSteps = new List<Form>();
        int stepIndex = 0;
        public UninstallMain()
        {
            InitializeComponent();
        }

        private void ShowWindow(Form form)
        {
            Uninstaller uninstaller = form.Tag as Uninstaller;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            try
            {
                uninstaller.Load();
                form.Show();
                Thread threadAfter = new Thread(new ThreadStart(uninstaller.AfterLoad));
                threadAfter.IsBackground = true;
                threadAfter.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Next()
        {
            formSteps[stepIndex].Visible = false;
            stepIndex++;
            ShowWindow(formSteps[stepIndex]);
        }

        public void Prev()
        {
            formSteps[stepIndex].Visible = false;
            stepIndex--;
            ShowWindow(formSteps[stepIndex]);
        }

        private void UninstallMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void UninstallMain_Load(object sender, EventArgs e)
        {
            formSteps.Add(new Step1());
            formSteps.Add(new Step2());
            ShowWindow(formSteps[0]);
        }

    }
}
