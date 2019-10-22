using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Sys.Safety.Reports
{
    public partial class frmInputDesc : XtraForm
    {
        private string _strDesc = "";

        public frmInputDesc()
        {
            BlnOK = false;
            InitializeComponent();
        }

        public frmInputDesc(string strDesc)
        {
            BlnOK = false;
            InitializeComponent();
            _strDesc = strDesc;
        }


        public string StrDesc
        {
            set { _strDesc = value; }
            get { return _strDesc; }
        }

        public bool BlnOK { get; set; }

        private void frmListDirName_Load(object sender, EventArgs e)
        {
            txtDirName.Text = StrDesc;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _strDesc = txtDirName.Text.Trim();
            BlnOK = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _strDesc = "";
            Close();
        }

        private void txtDirName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(null, null);
        }
    }
}