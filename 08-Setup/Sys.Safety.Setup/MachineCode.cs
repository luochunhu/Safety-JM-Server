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

namespace Sys.Safety.Setup
{
    public partial class MachineCode : Form
    {
        public MachineCode()
        {
            InitializeComponent();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMachineCode.Text);
        }

        private void MachineCode_Load(object sender, EventArgs e)
        {
            txtMachineCode.Text = DESHelper.GetFormatMachineCode();
        }
    }
}
