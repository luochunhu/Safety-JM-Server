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
    public partial class SetPoints : XtraForm
    {

        /// <summary>
        /// 数据表
        /// </summary>
        private DataTable points;

        /// <summary>
        /// 填充顺序
        /// </summary>
        private bool isup = false;

        /// <summary>
        /// 显示行数
        /// </summary>
        private int rowcount = 0;

        /// <summary>
        /// 显示列数
        /// </summary>
        private int columncount = 0;

        /// <summary>
        /// 设备类型表
        /// </summary>
        private DataTable dtlx;

        public SetPoints( )
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载设备类型
        /// </summary>
        private void addlx()
        {
            DataView view;
            DataTable dts;
            DataRow[] rows;
            #region 加载分站
            dts = GetAllDev();
            comb_fz.Properties.Items.Clear();
            comb_fz.Properties.Items.Add("");
            if (dts.Rows.Count > 0)
            {
                rows = dts.Select("","fzh asc");
                for (int i = 0; i < rows.Length ; i++)
                {
                    comb_fz.Properties.Items.Add(rows[i]["point"].ToString());
                }
            }
            #endregion

            #region 加载类型
            comb_lb.Properties.Items.Clear();
            dtlx  = OprFuction.GetAlllb("");
            view = new DataView(dtlx);
            view.Sort = "lxtype asc";
            dts = view.ToTable(true, "lx", "lxtype");
            if (dts != null && dts.Rows.Count > 0)
            {
                foreach (DataRow row in dts.Rows)
                {
                    comb_lb.Properties.Items.Add(row["lx"].ToString());
                }
            }
            #endregion
        }

        private void SetPointsShowForm_Load(object sender, EventArgs e)
        {
            try
            {
                addlx();
                GetPoint();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// 获取所有设备
        /// </summary>
        /// <returns></returns>
        private DataTable GetAllDev()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("fzh", typeof(int)));
            dt.Columns.Add(new DataColumn("point", typeof(string)));
            DataRow[] rows = null;
            lock (StaticClass.allPointDtLockObj)
            {
                rows = StaticClass.AllPointDt.Select("lx='分站' or lx='0'");
                if (rows != null && rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        dt.Rows.Add(row["fzh"], string.Format("[{0}]{1}[{2}]", row["point"].ToString(), row["wz"].ToString(), row["lb"].ToString()));
                    }
                }
            }
            return dt;
        }


 


        private void comb_showcount_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comb_showrows_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void gv_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "")
            {
                e.Appearance.BackColor = Color.LightGray;
            }
        }

        private void gv_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
            }
        }

        private void listB_MouseDown(object sender, MouseEventArgs e)
        {
            if (listB.SelectedItem != null)
            {
            }
        }

        private void listB_MouseUp(object sender, MouseEventArgs e)
        {
            listB.Cursor = Cursors.Default;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (listA.ItemCount == 0)
            {
                // 20170628
                //MessageBox.Show("请先选择测点。");
                XtraMessageBox.Show("请先选择测点。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime time = dateTimePicker1.Value;
            if (Convert.ToDateTime(time.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                XtraMessageBox.Show("标校日期不能小于当日。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_save.Enabled = false;
            labelControl13.Text = "存储中...";
            labelControl13.Visible = true;
            
            string spoint = "";

            if (listA.Items.Count > 0)
            {
                for (int i = 0; i < listA.Items.Count; i++)
                {
                    spoint += listA.Items[i].ToString().Substring(1, 7) + "|";
                }
                spoint = spoint.Substring(0, spoint.Length - 1);
            }

            #region 存储测点信息
            try
            {
                if (Model.RealInterfaceFuction.SavePoint(time , spoint ))
                {
                    labelControl13.Text = "保存成功";
                }
                else
                {
                    labelControl13.Text = "保存失败";
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("测点编排保存测点到数据库", ex);
            }
            #endregion
            
            btn_save.Enabled = true;
        }
        private string fzh = "";
        private void comb_fz_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = "";
            DataRow[] rows = null;

            try
            {
                #region 加载分站下测点
                fzh = "";
                listB.Items.Clear();
                comb_lb.Text = "";
                comb_zl.Text = "";
                comb_lb.Properties.Items.Clear();
                comb_zl.Properties.Items.Clear();
                if (!string.IsNullOrEmpty(comb_fz.Text))
                {
                    temp = comb_fz.Text.Trim();
                    temp = temp.Substring(temp.IndexOf('[') + 1, temp.IndexOf(']') - temp.IndexOf('[') - 1);
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + temp + "'", "tdh asc");
                        if (rows.Length > 0)
                        {
                            fzh = rows[0]["fzh"].ToString();
                            rows = StaticClass.AllPointDt.Select("fzh='" + rows[0]["fzh"].ToString() + "'", "tdh asc");
                            if (rows.Length > 0)
                            {
                                foreach (DataRow row in rows)
                                {
                                    listB.Items.Add(string.Format("[{0}]{1}[{2}]", row["point"].ToString(), row["wz"].ToString(), row["lb"].ToString()));
                                }
                            }
                        }
                    }
                }
                dtlx = OprFuction.GetAlllb(fzh);
                DataView    view = new DataView(dtlx);
                view.Sort = "lxtype asc";
               DataTable   dts = view.ToTable(true, "lx", "lxtype");
                if (dts != null && dts.Rows.Count > 0)
                {
                    foreach (DataRow row in dts.Rows)
                    {
                        comb_lb.Properties.Items.Add(row["lx"].ToString());
                    }
                }
                #endregion
            }
            catch(Exception ex)
            {
                OprFuction.SaveErrorLogs("加载分站下测点", ex);
            }
        }

        private void comb_zl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow[] rows = null;
            string temp = "", str1 = "", str2 = "", str3 = "";
            try
            {
                listB.Items.Clear();
                temp = comb_zl.Text.Trim();
                lock (StaticClass.allPointDtLockObj)
                {
                    if (fzh != "")
                    {
                        rows = StaticClass.AllPointDt.Select("fzh='" + fzh + "' and ( lb='" + temp + "' or zl='" + temp + "')");
                    }
                    else
                    {
                        rows = StaticClass.AllPointDt.Select("lb='" + temp + "' or zl='" + temp + "'");
                    }
                    if (rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (!rows[i].IsNull("point"))
                            {
                                #region 加载测点
                                str1 = rows[i]["point"].ToString();
                                if (!rows[i].IsNull("wz"))
                                {
                                    str2 = rows[i]["wz"].ToString();
                                }
                                else
                                {
                                    str2 = "";
                                }
                                if (!rows[i].IsNull("lb"))
                                {
                                    str3 = rows[i]["lb"].ToString();
                                }
                                else
                                {
                                    str3 = "";
                                }
                                listB.Items.Add(string.Format("[{0}]{1}[{2}]", str1, str2, str3));
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("根据种类加载测点", ex);
            }
        }

        private void comb_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView view;
            DataTable dts;
            DataRow []rows;
            string lx = "";
            try
            {
                lx = comb_lb.Text;
                comb_zl.Properties.Items.Clear();
                comb_zl.Text = "";
                if (!string.IsNullOrEmpty(lx))
                {
                    dtlx = OprFuction.GetAlllb(fzh);
                    view = new DataView(dtlx);
                    dts = view.ToTable(true, "lx", "zl");

                    rows = dts.Select("lx='" + lx + "'");
                    if (rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            comb_zl.Properties.Items.Add(row["zl"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("加载种类", ex);
            }
            string  str1 = "", str2 = "", str3 = "";
            try
            {
                listB.Items.Clear();
                if (!string.IsNullOrEmpty(lx))
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        if (fzh != "")
                        {
                            rows = StaticClass.AllPointDt.Select("fzh='" + fzh + "' and  lx='" + lx + "'");
                        }
                        else
                        {
                            rows = StaticClass.AllPointDt.Select("lx='" + lx + "'");
                        }
                        if (rows.Length > 0)
                        {
                            for (int i = 0; i < rows.Length; i++)
                            {
                                if (!rows[i].IsNull("point"))
                                {
                                    #region 加载测点
                                    str1 = rows[i]["point"].ToString();
                                    if (!rows[i].IsNull("wz"))
                                    {
                                        str2 = rows[i]["wz"].ToString();
                                    }
                                    else
                                    {
                                        str2 = "";
                                    }
                                    if (!rows[i].IsNull("lb"))
                                    {
                                        str3 = rows[i]["lb"].ToString();
                                    }
                                    else
                                    {
                                        str3 = "";
                                    }
                                    listB.Items.Add(string.Format("[{0}]{1}[{2}]", str1, str2, str3));
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("根据种类加载测点", ex);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_delall_Click(object sender, EventArgs e)
        {
            listA.Items.Clear();
        }

        private void gridC_MouseEnter(object sender, EventArgs e)
        {
            gridC_MouseDown(null, null);
        }

        private void gridC_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btn_selall_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listB.Items.Count; i++)
            {
                listA.Items.Add(listB.Items[i]);
            }
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listB.SelectedItems.Count; i++)
            {
                listA.Items.Add(listB.SelectedItems[i]);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listA.SelectedItems.Count; i++)
                {
                    listA.Items.Remove(listA.SelectedItems[i]);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void GetPoint()
        {
            DataRow[] rows = null;
            DateTime time = dateTimePicker1.Value;
            string str2 = "", str3 = "", point = "";
            DataTable dt;
            try
            {
                dt = Model.RealInterfaceFuction.Getbxpoint(time);
                listA.Items.Clear();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        point = dt.Rows[i]["point"].ToString();
                        if (!string.IsNullOrEmpty(point))
                        {
                            lock (StaticClass.allPointDtLockObj)
                            {
                                rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (rows.Length > 0)
                                {
                                    #region 加载测点
                                    if (!rows[0].IsNull("wz"))
                                    {
                                        str2 = rows[0]["wz"].ToString();
                                    }
                                    else
                                    {
                                        str2 = "";
                                    }
                                    if (!rows[0].IsNull("lb"))
                                    {
                                        str3 = rows[0]["lb"].ToString();
                                    }
                                    else
                                    {
                                        str3 = "";
                                    }
                                    listA.Items.Add(string.Format("[{0}]{1}[{2}]", point, str2, str3));
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("加载标校测点", ex);
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GetPoint();

        }
    }
}
