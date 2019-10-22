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
    public partial class SetupMain : Form
    {
        List<Form> formSteps = new List<Form>();
        int stepIndex = 0;
        public SetupMain()
        {
            InitializeComponent();
        }

        private void ShowWindow(Form form)
        {
            Installer installer = form.Tag as Installer;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            try
            {
                installer.Load();
                form.Show();
                Thread threadAfter = new Thread(new ThreadStart(installer.AfterLoad));
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

        private void SetupMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void SetupMain_Load(object sender, EventArgs e)
        {
            formSteps.Add(new Step1());
            formSteps.Add(new Step2());
            formSteps.Add(new Step3());
            formSteps.Add(new Step4());
            formSteps.Add(new Step5());
            formSteps.Add(new Step6());
            formSteps.Add(new Finished());
            ShowWindow(formSteps[0]);
        }
    }
}
