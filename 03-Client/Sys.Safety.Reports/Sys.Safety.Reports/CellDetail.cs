using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Reports
{
    public partial class CellDetail : XtraForm
    {
        private string _nodeName = "";
        public CellDetail(string nodeName)
        {
            _nodeName = nodeName;
            InitializeComponent();
        }

        private void CellDetail_Load(object sender, EventArgs e)
        {
            memoEdit1.Text = _nodeName;
        }
    }
}
