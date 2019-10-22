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
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Display
{
    public partial class SetPointsShowForm : XtraForm
    {
        /// <summary>
        /// 自定义编排配置
        /// </summary>
        private PageSetConfig pc;

        /// <summary>
        /// 自定义表数据源
        /// </summary>
        private DataTable pointsdt;

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

        public SetPointsShowForm(PageSetConfig c)
        {
            InitializeComponent();
            pc = c;
            if (c != null && c.ShowColumnCount > 0)
            {
                isup = c.IsDataFillType;
                columncount = c.ShowColumnCount;
                rowcount = c.ShowRowCount;
            }
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
                rows = dts.Select("", "fzh asc");
                for (int i = 0; i < rows.Length; i++)
                {
                    comb_fz.Properties.Items.Add(rows[i]["point"].ToString());
                }
            }
            #endregion

            #region 加载类型
            comb_lb.Properties.Items.Clear();
            dtlx = OprFuction.GetAlllb("");
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
                //listB.Items.Add("拖我试试");
                if (pc != null)
                {
                    for (int i = 0; i < comb_showcount.Properties.Items.Count; i++)
                    {
                        if (comb_showcount.Properties.Items[i].ToString() == pc.ShowColumnCount.ToString())
                        {
                            comb_showcount.SelectedIndex = i;
                            break;
                        }
                    }
                    for (int i = 0; i < comb_showrows.Properties.Items.Count; i++)
                    {
                        if (comb_showrows.Properties.Items[i].ToString() == pc.ShowRowCount.ToString())
                        {
                            comb_showrows.SelectedIndex = i;
                            break;
                        }
                    }
                }
                Getpoints();
                AddColAndRows();
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

        /// <summary>
        /// 根据配置获取已编码测点
        /// </summary>
        private void Getpoints()
        {
            DataRow row;
            pointsinit();
            if (pc != null)
            {
                if (pc.Page > 0)
                {
                    points.Rows.Clear();
                    DataTable dt = Model.RealInterfaceFuction.GetCustomPagePoint(pc.Page);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            row = points.NewRow();
                            row["points"] = dt.Rows[i]["point"];
                            row["col"] = dt.Rows[i]["px"];
                            row["row"] = dt.Rows[i]["rowid"];
                            points.Rows.Add(row);
                        }
                    }

                }
            }
        }

        private void pointsinit()
        {
            points = new DataTable();
            points.Columns.Add("row", typeof(int));
            points.Columns.Add("col", typeof(int));
            points.Columns.Add("points", typeof(string));
        }

        private void AddColAndRows()
        {
            int rows = 0, count = 0;
            int rowindex = 0, colindex = 0;
            string point = "";
            DataRow[] rows1;
            DevExpress.XtraGrid.Columns.GridColumn col;
            try
            {
                columncount = int.Parse(comb_showcount.Text);
                rowcount = int.Parse(comb_showrows.Text);
                #region  创建gridview
                gv.Columns.Clear();
                if (!string.IsNullOrEmpty(comb_showcount.Text))
                {
                    count = columncount;
                    for (int i = 0; i < count; i++)
                    {
                        col = new DevExpress.XtraGrid.Columns.GridColumn();
                        col.ColumnEditName = "count" + i;
                        col.Caption = "测点";
                        col.Tag = i;
                        col.FieldName = "points" + i;
                        col.Visible = true;
                        gv.Columns.Add(col);
                        if (i < count - 1)
                        {
                            col = new DevExpress.XtraGrid.Columns.GridColumn();
                            col.ColumnEditName = "s" + i;
                            col.Caption = "";
                            col.FieldName = "s" + i;
                            col.Width = 10;
                            col.MaxWidth = 10;
                            col.MinWidth = 10;
                            col.Visible = true;
                            gv.Columns.Add(col);

                        }

                    }
                    for (int i = 0; i < gv.Columns.Count; i++)
                    {
                        gv.Columns[i].Tag = i;
                    }
                }
                #endregion

                #region 创建数据源
                DataColumn coll;
                if (!string.IsNullOrEmpty(comb_showrows.Text))
                {
                    pointsdt = new DataTable();
                    for (int i = 0; i < count; i++)
                    {
                        coll = new DataColumn("points" + i, typeof(string));
                        pointsdt.Columns.Add(coll);
                        if (i < count - 1)
                        {
                            coll = new DataColumn("s" + i, typeof(string));
                            pointsdt.Columns.Add(coll);
                        }

                    }
                    rows = rowcount;
                    for (int i = 0; i < rows; i++)
                    {
                        pointsdt.Rows.Add("");
                    }
                    //else
                    //{
                    //    for (int i = 0; i < rows; i++)
                    //    {
                    //        pointsdt.Rows.Add("34WF3");
                    //    }
                    //}

                }
                #endregion

                #region 填充数据
                if (points != null && points.Rows.Count > 0)
                {
                    for (int i = 0; i < points.Rows.Count; i++)
                    {
                        rowindex = int.Parse(points.Rows[i]["row"].ToString());
                        colindex = int.Parse(points.Rows[i]["col"].ToString());
                        if (rowindex < rows && colindex < count)
                        {
                            point = points.Rows[i]["points"].ToString();
                            lock (StaticClass.allPointDtLockObj)
                            {
                                rows1 = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (rows1.Length > 0)
                                {
                                    pointsdt.Rows[rowindex]["points" + colindex] = "[" + point + "]" + rows1[0]["wz"] + "[" + rows1[0]["lb"] + "]";
                                }
                                else
                                {
                                    pointsdt.Rows[rowindex]["points" + colindex] = points.Rows[i]["points"].ToString();
                                }
                            }
                        }
                    }
                }
                gridC.DataSource = pointsdt;
                #endregion
                grgvidinit(gv);//初始化gridview样式
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void movecellvalue(int colindex, int rowindex)
        {
            int nowi, nowj;
            string msg = "";
            if (pointsdt != null && pointsdt.Rows.Count > 0)
            {
                for (int i = pointsdt.Columns.Count - 1; i >= 0; i--)
                {
                    if (!pointsdt.Columns[i].ColumnName.Contains("point"))
                    {
                        continue;
                    }
                    for (int j = pointsdt.Rows.Count - 1; j >= 0; j--)
                    {
                        if (i < colindex || (i == colindex && j == rowindex) || ((i == 0) && (j == 0)))
                        {
                            return;
                        }
                        else
                        {
                            nowi = i;
                            nowj = j;
                            if (j == 0)
                            {
                                nowi--;
                                if (!pointsdt.Columns[nowi].ColumnName.Contains("point"))
                                {
                                    nowi--;
                                    nowj = pointsdt.Rows.Count - 1;
                                }
                            }
                            else
                            {
                                nowj--;
                            }
                            pointsdt.Rows[j][i] = pointsdt.Rows[nowj][nowi];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化gridview样式
        /// </summary>
        /// <param name="gview"></param>
        private void grgvidinit(DevExpress.XtraGrid.Views.Grid.GridView gview)
        {
            gview.OptionsBehavior.Editable = false;//单元格编辑
            gview.OptionsCustomization.AllowColumnMoving = false;//拖动列头
            gview.OptionsCustomization.AllowColumnResizing = false;//修改列宽
            gview.OptionsCustomization.AllowRowSizing = false;//修改行高
            gview.OptionsCustomization.AllowSort = false;//排序
            gview.OptionsFilter.AllowFilterEditor = false;//使用过滤编辑器
            gview.OptionsMenu.EnableColumnMenu = false;//列头菜单
            gview.OptionsMenu.EnableFooterMenu = false;//页脚菜单
            gview.OptionsMenu.EnableGroupPanelMenu = true;//分组面板上的菜单
            gview.OptionsSelection.MultiSelect = false;//多选行
            gview.OptionsView.ShowHorzLines = true;//水平网格线
            gview.OptionsView.ShowVertLines = true;//垂直网格线
            //gview.Appearance.HorzLine.BackColor  = Color.Red;
            gview.Appearance.VertLine.BackColor = Color.Red;
            //gview .Appearance .Row.TextOptions.HAlignment=DevExpress .Utils .HorzAlignment .Center ;
            for (int i = 0; i < gview.Columns.Count; i++)
            {
                gview.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gview.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gview.Columns[i].ColumnHandle = i;

            }
        }

        private void comb_showcount_SelectedIndexChanged(object sender, EventArgs e)
        {
            SavePoints();
            AddColAndRows();
        }

        private void comb_showrows_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddColAndRows();
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
                getcell(e.X, e.Y);
            }
        }

        #region  拖动操作
        /// <summary>
        /// 当前值
        /// </summary>
        private string cmsg = "";
        /// <summary>
        /// 当前单元格 = -2 表示来自测点表 
        /// </summary>
        private int rowi = -10;
        /// <summary>
        /// 当前列 =-2 表示来自测点表
        /// </summary>
        private int coli = -10;

        /// <summary>
        /// 获取拖动的值
        /// </summary>
        private void getcell(int x, int y)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo ghi = this.gv.CalcHitInfo(new Point(x, y));
            if (ghi != null && ghi.Column != null && ghi.Column.FieldName.Contains("point") && ghi.RowHandle >= 0)
            {
                cmsg = gv.GetRowCellValue(ghi.RowHandle, ghi.Column).ToString();
                rowi = ghi.RowHandle;
                coli = ghi.Column.ColumnHandle;
            }
        }

        /// <summary>
        /// 清空选定的值
        /// </summary>
        private void clearcell()
        {
            cmsg = "";
            coli = -10;
            rowi = -10;
        }

        /// <summary>
        /// 交换值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void changecell(int x, int y)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo ghi = this.gv.CalcHitInfo(new Point(x, y));
            if (ghi != null && ghi.Column != null && ghi.Column.FieldName.Contains("point") && ghi.RowHandle >= 0)
            {
                if (ghi.Column.ColumnHandle != coli || ghi.RowHandle != rowi)
                {
                    if (cmsg != "")
                    {
                        string oldmsg = gv.GetRowCellValue(ghi.RowHandle, gv.Columns[ghi.Column.FieldName]).ToString();
                        if (coli != -10 && rowi != -10)
                        {
                            gv.SetRowCellValue(rowi, gv.Columns[coli], oldmsg);
                        }
                        //if (gv.GetRowCellValue(ghi.RowHandle, ghi.Column).ToString() != "")
                        //{
                        //    movecellvalue(ghi.Column.ColumnHandle, ghi.RowHandle);
                        //}

                        gv.SetRowCellValue(ghi.RowHandle, gv.Columns[ghi.Column.FieldName], cmsg);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 将编排存储到表中
        /// </summary>
        private void SavePoints()
        {
            string col = "0";
            string point = "";
            if (pointsdt != null && pointsdt.Rows.Count > 0)
            {
                points.Rows.Clear();
                for (int i = 0; i < pointsdt.Columns.Count; i++)
                {
                    if (pointsdt.Columns[i].ColumnName.Contains("point"))
                    {
                        col = pointsdt.Columns[i].ColumnName.Replace("points", "");
                        for (int j = 0; j < pointsdt.Rows.Count; j++)
                        {
                            point = pointsdt.Rows[j][i].ToString();
                            if (point != "")
                            {
                                if (point.Contains('['))
                                {
                                    points.Rows.Add(j, col, point.Substring(1, point.IndexOf(']') - 1));
                                }
                                else
                                {
                                    points.Rows.Add(j, col, point);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Cursor sr;
        private void gv_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                changecell(e.X, e.Y);
            }
            clearcell();
            gridC.Cursor = Cursors.Default;
        }

        private void gv_MouseMove(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmsg))
            {
                sr = gridC.Cursor;
                gridC.Cursor = Cursors.AppStarting;
                //new Cursor("ico_bbb.ico");
            }
        }

        private void listB_MouseDown(object sender, MouseEventArgs e)
        {
            if (listB.SelectedItem != null)
            {
                cmsg = listB.SelectedItem.ToString();
                coli = -2;
                rowi = -2;
            }
        }

        private void listB_MouseMove(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmsg))
            {
                sr = gridC.Cursor;
                listB.Cursor = Cursors.AppStarting;
                //new Cursor("ico_bbb.ico");
            }
        }

        private void listB_MouseUp(object sender, MouseEventArgs e)
        {
            listB.Cursor = Cursors.Default;
            clearcell();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            btn_save.Enabled = false;
            labelControl13.Text = "存储中...";
            labelControl13.Visible = true;
            #region 存储自定义信息
            try
            {

                StaticClass.arrangeconfig.CustomCofig[pc.Page - 1] = pc;

                pc.ShowColumnCount = columncount;
                pc.ShowRowCount = rowcount;

                OprFuction.SaveCustomConfig();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("测点编排保存编排信息", ex);
            }
            #endregion

            #region 存储测点信息
            try
            {
                SavePoints();
                if (Model.RealInterfaceFuction.SaveCustomPagePoints(pc.Page, points))
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
                DataView view = new DataView(dtlx);
                view.Sort = "lxtype asc";
                DataTable dts = view.ToTable(true, "lx", "lxtype");
                if (dts != null && dts.Rows.Count > 0)
                {
                    foreach (DataRow row in dts.Rows)
                    {
                        comb_lb.Properties.Items.Add(row["lx"].ToString());
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("加载分站下测点", ex);
            }
            GetListBDataSource();
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

            GetListBDataSource();
        }

        private void comb_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView view;
            DataTable dts;
            DataRow[] rows;
            string lx = "";
            try
            {
                lx = comb_lb.Text;
                comb_zl.Properties.Items.Clear();
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
            string str1 = "", str2 = "", str3 = "";
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

            GetListBDataSource();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_selall_Click(object sender, EventArgs e)
        {
            int row, col;
            int k = 0;
            bool flg = false;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null && gv.FocusedColumn.FieldName.Contains("point"))
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    if (listB.Items.Count > 0)
                    {
                        for (int c = col; c < pointsdt.Columns.Count; c++)
                        {
                            for (int i = row; i < pointsdt.Rows.Count; i++)
                            {
                                //if (pointsdt.Rows[i][c].ToString() != "")
                                //{
                                //    movecellvalue(c, i);
                                //}

                                pointsdt.Rows[i][c] = listB.Items[k];
                                k++;
                                if (k >= listB.Items.Count)
                                {
                                    flg = true;
                                    break;
                                }
                            }
                            if (flg)
                            {
                                break;
                            }
                            else
                            {
                                row = 0; c++;
                            }
                        }

                        //listB.Items.Clear();
                        //listB.Refresh();
                        if (checkEdit1.Checked)
                        {
                            GetListBDataSourceBySelectType();
                        }
                    }
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "请先在【显示界面预览】中选择一个位置！");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void btn_delall_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < pointsdt.Rows.Count; i++)
                {
                    for (int j = 0; j < pointsdt.Columns.Count; j++)
                    {
                        pointsdt.Rows[i][j] = "";
                    }
                }
                if (checkEdit1.Checked)
                {
                    GetListBDataSourceBySelectType();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            int row, col;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null)
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    pointsdt.Rows[row][col] = "";

                    if (checkEdit1.Checked)
                    {
                        GetListBDataSourceBySelectType();
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            int row, col;
            int k = 0;
            bool flg = false;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null && gv.FocusedColumn.FieldName.Contains("point"))
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    if (listB.SelectedItems.Count > 0)
                    {
                        //for (int c = col; c < pointsdt.Columns.Count; c++)
                        //{
                        //    for (int i = row; i < pointsdt.Rows.Count; i++)
                        //    {
                        //        if (pointsdt.Rows[i][c].ToString() != "")
                        //        {
                        //            movecellvalue(c, i);
                        //        }
                        //        pointsdt.Rows[i][c] = listB.SelectedItems[k];
                        //        k++;
                        //        if (k >= listB.SelectedItems.Count)
                        //        {
                        //            flg = true;
                        //            break;
                        //        }
                        //    }
                        //    if (flg)
                        //    {
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        row = 0; c++;
                        //    }
                        //}

                        //直接覆盖
                        pointsdt.Rows[row][col] = listB.SelectedItem.ToString();//移除已添加记录，避免重复添加


                        //for (int i = 0; i < listB.SelectedItems.Count; i++)
                        //{
                        //    //var item=listB.Items[]
                        //    listB.Items.Remove(listB.SelectedItems[i]);
                        //}
                        //listB.Refresh();
                        if (checkEdit1.Checked)
                        {
                            GetListBDataSourceBySelectType();
                        }
                    }
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "请先在【显示界面预览】中选择一个位置！");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        //private void gridC_MouseEnter(object sender, EventArgs e)
        //{
        //    gridC_MouseDown(null, null);
        //}

      
        /// <summary>
        /// 只显示未选择测点
        /// </summary>
        private void GetListBDataSource()
        {
            try
            {
                if (checkEdit1.Checked)
                {
                    //显示未选择测点
                    for (int i = 0; i < pointsdt.Rows.Count; i++)
                    {
                        for (int j = 0; j < pointsdt.Columns.Count; j++)
                        {
                            string point = pointsdt.Rows[i][j].ToString();

                            if (listB.Items.Contains(point))
                            {
                                listB.Items.Remove(point);
                            }
                        }
                    }
                }

                listB.Refresh();
            }
            catch (Exception ex)
            {
                LogHelper.Info("刷选未选择测点出错！" + ex.Message);
            }
        }


        private void GetListBDataSourceBySelectType()
        {
            listB.Items.Clear();
            //重新加载数据源
            if (comb_zl.SelectedItem != null && !string.IsNullOrEmpty(comb_zl.SelectedItem.ToString()))
            {
                DataRow[] rows = null;
                string temp = "", str1 = "", str2 = "", str3 = "";
                try
                {
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
            else if (comb_lb.SelectedItem != null && !string.IsNullOrEmpty(comb_lb.SelectedItem.ToString()))
            {
                DataView view;
                DataTable dts;
                DataRow[] rows;
                string lx = "";
                try
                {
                    lx = comb_lb.Text;
                    comb_zl.Properties.Items.Clear();
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
                string str1 = "", str2 = "", str3 = "";
                try
                {
                    
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
            else if (comb_fz.SelectedItem != null && !string.IsNullOrEmpty(comb_fz.SelectedItem.ToString()))
            {
                string temp = "";
                DataRow[] rows = null;
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

            GetListBDataSource();
        }

        /// 删除单元格，并控制后面的单元格前移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int row, col;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null)
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    //pointsdt.Rows[row][col] = "";
                    for (int i = row; i < pointsdt.Rows.Count - 1; i++)
                    {
                        pointsdt.Rows[i][col] = pointsdt.Rows[i + 1][col];
                        if (i == pointsdt.Rows.Count - 1)
                        {
                            pointsdt.Rows[i + 1][col] = "";
                        }
                    }

                    if (checkEdit1.Checked)
                    {
                        GetListBDataSourceBySelectType();
                    }
                }                
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int row, col;
            int k = 0;
            bool flg = false;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null && gv.FocusedColumn.FieldName.Contains("point"))
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    if (listB.SelectedItems.Count > 0)
                    {
                        //先判断并控制单元格向后移动  20180110
                        if (!string.IsNullOrEmpty(pointsdt.Rows[pointsdt.Rows.Count - 1][col].ToString()))
                        {
                            OprFuction.MessageBoxShow(0, "当前列数据已添加满，不能进行插入操作！");
                            return;
                        }
                        for (int i = pointsdt.Rows.Count - 1; i > row; i--)
                        {
                            pointsdt.Rows[i][col] = pointsdt.Rows[i - 1][col];//依次向后移动                           
                        }
                        //pointsdt.Rows[row][col] = "";

                        //向当前空白单元格添加选择的数据                        
                        pointsdt.Rows[row][col] = listB.SelectedItem.ToString();
                    }

                    if (checkEdit1.Checked)
                    {
                        GetListBDataSourceBySelectType();
                    }
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "请先在【显示界面预览】中选择一个位置！");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void listB_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int row, col;
            int k = 0;
            bool flg = false;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null && gv.FocusedColumn.FieldName.Contains("point"))
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    if (listB.SelectedItems.Count > 0)
                    {
                        //for (int c = col; c < pointsdt.Columns.Count; c++)
                        //{
                        //    for (int i = row; i < pointsdt.Rows.Count; i++)
                        //    {
                        //        if (pointsdt.Rows[i][c].ToString() != "")
                        //        {
                        //            movecellvalue(c, i);
                        //        }
                        //        pointsdt.Rows[i][c] = listB.SelectedItems[k];
                        //        k++;
                        //        if (k >= listB.SelectedItems.Count)
                        //        {
                        //            flg = true;
                        //            break;
                        //        }
                        //    }
                        //    if (flg)
                        //    {
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        row = 0; c++;
                        //    }
                        //}

                        //直接覆盖
                        pointsdt.Rows[row][col] = listB.SelectedItem.ToString();//移除已添加记录，避免重复添加


                        //for (int i = 0; i < listB.SelectedItems.Count; i++)
                        //{
                        //    //var item=listB.Items[]
                        //    listB.Items.Remove(listB.SelectedItems[i]);
                        //}
                        //listB.Refresh();
                        if (checkEdit1.Checked)
                        {
                            GetListBDataSourceBySelectType();
                        }
                    }
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "请先在【显示界面预览】中选择一个位置！");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void gridC_MouseClick(object sender, MouseEventArgs e)
        {
            int row, col;
            int k = 0;
            bool flg = false;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null && gv.FocusedColumn.FieldName.Contains("point"))
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    if (listB.SelectedItems.Count > 0)
                    {
                        //for (int c = col; c < pointsdt.Columns.Count; c++)
                        //{
                        //    for (int i = row; i < pointsdt.Rows.Count; i++)
                        //    {
                        //        if (pointsdt.Rows[i][c].ToString() != "")
                        //        {
                        //            movecellvalue(c, i);
                        //        }
                        //        pointsdt.Rows[i][c] = listB.SelectedItems[k];
                        //        k++;
                        //        if (k >= listB.SelectedItems.Count)
                        //        {
                        //            flg = true;
                        //            break;
                        //        }
                        //    }
                        //    if (flg)
                        //    {
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        row = 0; c++;
                        //    }
                        //}

                        //直接覆盖
                        pointsdt.Rows[row][col] = listB.SelectedItem.ToString();//移除已添加记录，避免重复添加


                        //for (int i = 0; i < listB.SelectedItems.Count; i++)
                        //{
                        //    //var item=listB.Items[]
                        //    listB.Items.Remove(listB.SelectedItems[i]);
                        //}
                        //listB.Refresh();
                        if (checkEdit1.Checked)
                        {
                            GetListBDataSourceBySelectType();
                        }
                    }
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "请先在【显示界面预览】中选择一个位置！");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void gridC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int row, col;
            try
            {
                if (gv.FocusedRowHandle > -1 && gv.FocusedColumn != null)
                {
                    row = gv.FocusedRowHandle;
                    col = int.Parse(gv.FocusedColumn.Tag.ToString());
                    pointsdt.Rows[row][col] = "";

                    if (checkEdit1.Checked)
                    {
                        GetListBDataSourceBySelectType();
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
    }
}
