using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Linkage.Views
{
    public partial class LinkageDetail : XtraForm
    {
        public LinkageDetail(string text, string colText, DataTable detailItem)
        {
            InitializeComponent();

            Text = text;
            ColText.Caption = colText;
            GridControlLinkage.DataSource = detailItem;
        }

        private void GoBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
