using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.Client.Define.Model;
using Sys.Safety.DataContract;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Define.Station
{
    public delegate void ddeee(string msg);
    public partial class CF_Fz : XtraForm
    {
        public CF_Fz(string msg)
        {
            InitializeComponent();
            Str = msg;
        }
        public ddeee del;
        private string Str = "";
        private void CF_Fz_Load(object sender, EventArgs e)
        {
            string str = "";
            cb_pt1.Properties.Items.Add("");
            cb_pt2.Properties.Items.Add("");
            cb_pt3.Properties.Items.Add("");
            cb_pt4.Properties.Items.Add("");
            IList<Jc_DefInfo> list = DEFServiceModel.QueryAllCache();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].DevPropertyID == 2)
                    {
                        str = list[i].Point + "." + list[i].Wz + "[" + list[i].DevName + "]";
                        cb_pt1.Properties.Items.Add(str);
                        cb_pt2.Properties.Items.Add(str);
                        cb_pt3.Properties.Items.Add(str);
                        cb_pt4.Properties.Items.Add(str);
                    }
                }


                if (Str != "")
                {
                    string[] sz = Str.Split('|');
                    if (sz.Length > 0)
                    {
                        for (int i = 0; i < cb_pt1.Properties.Items.Count; i++)
                        {
                            if (cb_pt1.Properties.Items[i].ToString().Contains(sz[0]))
                            {
                                cb_pt1.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    if (sz.Length > 1)
                    {
                        for (int i = 0; i < cb_pt2.Properties.Items.Count; i++)
                        {
                            if (cb_pt2.Properties.Items[i].ToString().Contains(sz[1]))
                            {
                                cb_pt2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    if (sz.Length > 2)
                    {
                        for (int i = 0; i < cb_pt3.Properties.Items.Count; i++)
                        {
                            if (cb_pt3.Properties.Items[i].ToString().Contains(sz[2]))
                            {
                                cb_pt3.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    if (sz.Length > 3)
                    {
                        for (int i = 0; i < cb_pt4.Properties.Items.Count; i++)
                        {
                            if (cb_pt4.Properties.Items[i].ToString().Contains(sz[3]))
                            {
                                cb_pt4.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                if (cb_pt1.SelectedIndex > 0) str += cb_pt1.Text.Substring(0, 7);
                if (cb_pt2.SelectedIndex > 0)
                {
                    if (cb_pt2.Text != cb_pt1.Text)
                    {
                        if (str == "") str += cb_pt2.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt2.Text.Substring(0, 7);
                    }
                }
                if (cb_pt3.SelectedIndex > 0)
                {
                    if (cb_pt3.Text != cb_pt1.Text && cb_pt3.Text != cb_pt2.Text)
                    {
                        if (str == "") str += cb_pt3.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt3.Text.Substring(0, 7);
                    }
                }
                if (cb_pt4.SelectedIndex > 0)
                {
                    if (cb_pt4.Text != cb_pt1.Text && cb_pt4.Text != cb_pt2.Text && cb_pt4.Text != cb_pt3.Text)
                    {
                        if (str == "") str += cb_pt4.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt4.Text.Substring(0, 7);
                    }
                }
                if (del != null)
                {
                    del(str);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = "";
                if (cb_pt1.SelectedIndex > 0) str += cb_pt1.Text.Substring(0, 7);
                if (cb_pt2.SelectedIndex > 0)
                {
                    if (cb_pt2.Text != cb_pt1.Text)
                    {
                        if (str == "") str += cb_pt2.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt2.Text.Substring(0, 7);
                    }
                }
                if (cb_pt3.SelectedIndex > 0)
                {
                    if (cb_pt3.Text != cb_pt1.Text && cb_pt3.Text != cb_pt2.Text)
                    {
                        if (str == "") str += cb_pt3.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt3.Text.Substring(0, 7);
                    }
                }
                if (cb_pt4.SelectedIndex > 0)
                {
                    if (cb_pt4.Text != cb_pt1.Text && cb_pt4.Text != cb_pt2.Text && cb_pt4.Text != cb_pt3.Text)
                    {
                        if (str == "") str += cb_pt4.Text.Substring(0, 7);
                        else str = str + "|" + cb_pt4.Text.Substring(0, 7);
                    }
                }
                if (del != null)
                {
                    del(str);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
