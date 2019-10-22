using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;

namespace Sys.Safety.Reports
{
    public partial class frmListDirName : XtraForm
    {
        private string _dirName = "";

        public frmListDirName()
        {
            ParentDirID = 0;
            ParentDirDt = null;
            InitializeComponent();
        }

        public frmListDirName(string dirName)
        {
            ParentDirID = 0;
            ParentDirDt = null;
            InitializeComponent();
            _dirName = dirName;
        }

        /// <summary>
        ///     父级目录
        /// </summary>
        public DataTable ParentDirDt { get; set; }

        /// <summary>
        ///     父级目录ID
        /// </summary>
        public int ParentDirID { set; get; }

        /// <summary>
        ///     目录名
        /// </summary>
        public string DirName
        {
            set { _dirName = value; }
            get { return _dirName; }
        }

        private void frmListDirName_Load(object sender, EventArgs e)
        {
            txtDirName.Text = _dirName;
            //ParentDirDt.Columns["ListID"].DataType = typeof(int);
            lookUpParentDir.Properties.DataSource = ParentDirDt;
            lookUpParentDir.EditValue = ParentDirID;

            if (DirName != string.Empty)
                layoutControlItem4.Visibility = LayoutVisibility.Never;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtDirName.Text.Trim() == "")
            {
                MessageShowUtil.ShowInfo("目录名不能为空");
                return;
            }

            ParentDirID = lookUpParentDir != null ? Convert.ToInt32(lookUpParentDir.EditValue) : 0;
            _dirName = txtDirName.Text.Trim();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _dirName = "";
            Close();
        }

        private void txtDirName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(null, null);
        }
    }
}