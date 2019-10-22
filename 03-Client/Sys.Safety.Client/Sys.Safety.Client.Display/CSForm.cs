using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;

namespace Sys.Safety.Client.Display
{
    public partial class CSForm : XtraForm
    {
        private long id = 0;

        private string cs = "";
        public CSForm(long id,string cs)
        {
            this.id = id;
            this.cs = cs;
            InitializeComponent();
        }

        private void add_cs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(text_cs.Text))
            {
                OprFuction.MessageBoxShow(0, "措施不能为空");
                return;
            }
             memo_cs.Text += text_cs.Text + "|" + Model.RealInterfaceFuction.GetServerNowTime ().ToString();
        }

        private void CSForm_Load(object sender, EventArgs e)
        {
            memo_cs.Text = cs;
        }

        private void CSForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StaticClass.Cs = memo_cs .Text ;
        }
    }
}
