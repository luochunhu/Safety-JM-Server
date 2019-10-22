using Basic.Framework.Version;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic.Framework.Tools.Register
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

   

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMachineCode.Text);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtMachineCode.Enabled = false;
            txtMachineCode.Text = DESHelper.GetFormatMachineCode();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtRegisterCode.Text = Clipboard.GetText();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "授权文件(*.License)|*.License";
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                txtRegisterCode.Text = System.IO.File.ReadAllText(dialog.FileName);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "程序执行文件(*.exe)|*.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtRegistProgram.Text = dialog.FileName;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegisterCode.Text))
            {
                MessageBox.Show("请填写注册码！", "提示");
                return;
            }

            if (string.IsNullOrEmpty(txtRegistProgram.Text))
            {
                MessageBox.Show("请选择要注册的程序或主程序集文件！", "提示");
                return;
            }

            var dir = new System.IO.FileInfo(txtRegistProgram.Text).DirectoryName+ "\\i.license";

            System.IO.File.WriteAllText(dir, txtRegisterCode.Text, Encoding.UTF8);
            MessageBox.Show("注册成功");
        }
    }
}
