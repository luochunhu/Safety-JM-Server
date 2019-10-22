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
    public partial class Finished : Form
    {
        Installer installer = new LastInstaller();
        public Finished()
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

        private void btnFinish_Click(object sender, System.EventArgs e)
        {
            ParentWindow.Close();
        }
    }
}
