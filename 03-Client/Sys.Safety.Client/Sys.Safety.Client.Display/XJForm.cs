using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.Threading;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Display
{
    public partial class XJForm : XtraForm
    {
        public XJForm()
        {
            InitializeComponent();
        }
        private Label[] tbarray = new Label[255];

        private DataTable dt = null;//分站
        private void CpanelPowerPac_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void DyxForm_Load(object sender, EventArgs e)
        {
            DataRow[] rows;
            DataTable dt = new DataTable();
            int i = 0;
            try
            {
                if (StaticClass.AllPointDt.Rows.Count > 0)
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("lx='分站'", "fzh");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow r in rows)
                            {
                                tbarray[i] = new Label();
                                tbarray[i].BorderStyle = BorderStyle.Fixed3D;
                                tbarray[i].TextAlign = ContentAlignment.MiddleCenter;
                                tbarray[i].Width = 40;
                                tbarray[i].Height = 28;
                                tbarray[i].Name = "textbox" + Convert.ToString(r["fzh"].ToString());
                                tbarray[i].Text = r["point"].ToString().PadLeft(3, '0');
                                //tbarray[i].ReadOnly = true;
                                tbarray[i].Font = new Font("宋体", 10, FontStyle.Bold);
                                tbarray[i].Tag = "0";
                                if (r["zt"].ToString() == "3" || r["zt"].ToString() == "4")
                                {
                                }
                                tbarray[i].Click += new System.EventHandler(this.XJ_DoubleClick);
                                flowLayoutPanel1.Controls.Add(tbarray[i]);
                                tbarray[i].Tag = "1";
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("分站运行界面，测点获取", ex);
            }
           
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.Start();
        }

        private void XJ_DoubleClick(object sender, EventArgs e)
        {
            Label tv = (Label)sender;
        }

        private void refresh()
        {
            
        }

        private void show()
        {
           
        }

        private Thread freshthread;
        private void fthread()
        {
            int timern = 1;
            while (!StaticClass.SystemOut)
            {
                try
                {
                    if (timern >= 3)
                    {
                        timern = 0;
                        refresh();
                        MethodInvoker In = new MethodInvoker(show);
                        this.BeginInvoke(In);
                    }
                    else
                    {
                        MethodInvoker In = new MethodInvoker(show);
                        this.BeginInvoke(In);
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                timern++;
                Thread.Sleep(1000);
            }
        }

    }
}
