using DevExpress.XtraGrid.Columns;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Display
{
    public partial class DDGXForm : DevExpress.XtraEditors.XtraForm
    {
        public DDGXForm()
        {
            InitializeComponent();
        }

        string[] mnlkz_colName = { "序号", "测点号", "传感器类型", "安装位置", "报警值", "断电值", "复电值", "上限报警控制", "上限断电控制", "下限报警控制", "下限断电控制", "断线控制" };
        string[] mnlkz_colField = { "index", "point", "devname", "wz", "bgvalue", "ddvalue", "fdvalue", "sxbjkz", "sxddkz", "xxbjkz", "xxddkz", "dxkz", };
        DataTable mnlkzDt = new DataTable();

        string[] kglkz_colName = { "序号", "测点号", "传感器类型", "安装位置", "0态控制", "1态控制", "2态控制" };
        string[] kglkz_colField = { "index", "point", "devname", "wz", "kz0", "kz1", "kz2" };
        DataTable kglkzDt = new DataTable();

        string[] jckz_colName = { "序号", "测点号", "传感器类型", "安装位置", "断线控制/0态控制", "断电控制/1态控制", "故障控制/2态控制" };
        string[] jckz_colField = { "index", "point", "devname", "wz", "kz0", "kz1", "kz2" };
        DataTable jckzDt = new DataTable();
        private void inigrid()
        {
            GridColumn col;
            for (int i = 0; i < mnlkz_colName.Length; i++)
            {
                col = new GridColumn();
                col.Caption = mnlkz_colName[i];
                col.FieldName = mnlkz_colField[i];
                col.Width = 80;
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView1.Columns.Add(col);

                mnlkzDt.Columns.Add(mnlkz_colField[i]);
            }


            mainGrid1.DataSource = mnlkzDt;

            for (int i = 0; i < kglkz_colName.Length; i++)
            {
                col = new GridColumn();
                col.Caption = kglkz_colName[i];
                col.FieldName = kglkz_colField[i];
                col.Width = 80;
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView2.Columns.Add(col);

                kglkzDt.Columns.Add(kglkz_colField[i]);
            }
            mainGrid2.DataSource = kglkzDt;

            for (int i = 0; i < jckz_colName.Length; i++)
            {
                col = new GridColumn();
                col.Caption = jckz_colName[i];
                col.FieldName = jckz_colField[i];
                col.Width = 80;
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView3.Columns.Add(col);

                jckzDt.Columns.Add(jckz_colField[i]);
            }
            mainGrid3.DataSource = jckzDt;
        }

        private void DDGXForm_Load(object sender, EventArgs e)
        {
            inigrid();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                IPointDefineService service = ServiceFactory.Create<IPointDefineService>();
                var result = service.GetAllPointDefineCache();
                if (result.IsSuccess && result.Data != null)
                {

                    List<Jc_DefInfo> allDef = result.Data.OrderBy(a => a.Point).ToList();
                    List<Jc_DefInfo> tempDef;

                    //模拟量本地控制
                    tempDef = allDef.Where(a => a.DevPropertyID == 1 && a.DevClassID != 13).ToList();
                    for (int i = 0; i < tempDef.Count; i++)
                    {
                        mnlkzDt.Rows.Add(i + 1,
                         tempDef[i].Point,
                         tempDef[i].DevName,
                         tempDef[i].Wz,
                         tempDef[i].Z2,
                         tempDef[i].Z3,
                         tempDef[i].Z4,
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K1),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K2),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K3),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K4),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K7)
                         );
                    }
                    //开关量本地控制
                    tempDef = allDef.Where(a => a.DevPropertyID == 2).ToList();
                    for (int i = 0; i < tempDef.Count; i++)
                    {
                        kglkzDt.Rows.Add(i + 1,
                         tempDef[i].Point,
                         tempDef[i].DevName,
                         tempDef[i].Wz,
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K1),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K2),
                         GetLocalControlText(tempDef[i].Fzh, tempDef[i].K3)
                         );
                    }
                    //交叉控制
                    tempDef = allDef.Where(a => (a.DevPropertyID == 1 || a.DevPropertyID == 2) && a.DevClassID != 13).ToList();
                    for (int i = 0; i < tempDef.Count; i++)
                    {
                        jckzDt.Rows.Add(i + 1,
                         tempDef[i].Point,
                         tempDef[i].DevName,
                         tempDef[i].Wz,
                         tempDef[i].Jckz1,
                         tempDef[i].Jckz2,
                         tempDef[i].Jckz3
                         );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private string GetKzk(int kz)
        //{
        //    string kzk = "";

        //    for (int i = 0; i < 16; i++)
        //    {
        //        if (((kz >> i) & 0x01) == 0x01)
        //        {
        //            kzk += (i + 1) + ",";
        //        }
        //    }
        //    if (kzk.Length > 0)
        //    {
        //        kzk = kzk.Substring(0, kzk.Length - 1)+" 号口";
        //    }
        //    return kzk;
        //}
        private string GetLocalControlText(short fzh, int K)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {

                            temp += fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";

                        }
                        else
                        {
                            temp += fzh.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1" + ",";
                        }
                    }
                }
                if (temp.Contains(","))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            return temp;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件t(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                if (tab1.SelectedIndex == 0)
                {
                    mainGrid1.ExportToXls(fileDialog.FileName);
                    DevExpress.XtraEditors.XtraMessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tab1.SelectedIndex == 1)
                {
                    mainGrid2.ExportToXls(fileDialog.FileName);
                    DevExpress.XtraEditors.XtraMessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tab1.SelectedIndex == 2)
                {
                    mainGrid3.ExportToXls(fileDialog.FileName);
                    DevExpress.XtraEditors.XtraMessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_conditon.Text))
            {
                var condition = txt_conditon.Text;
                DataTable newdt = mnlkzDt.Clone();
                DataRow[] rows = mnlkzDt.Select("point like '%" + condition + "%' OR wz like '%" + condition + "%'");
                foreach (DataRow row in rows)
                {
                    newdt.Rows.Add(row.ItemArray);
                }
                mainGrid1.DataSource = newdt;
            }
        }

        private void btn_s2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_c2.Text))
            {
                var condition = txt_c2.Text;
                DataTable newdt = kglkzDt.Clone();
                DataRow[] rows = kglkzDt.Select("point like '%" + condition + "%' OR wz like '%" + condition + "%'");
                foreach (DataRow row in rows)
                {
                    newdt.Rows.Add(row.ItemArray);
                }
                mainGrid2.DataSource = newdt;
            }
        }

        private void btn_s3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_c3.Text))
            {
                var condition = txt_c3.Text;
                DataTable newdt = jckzDt.Clone();
                DataRow[] rows = jckzDt.Select("point like '%" + condition + "%' OR wz like '%" + condition + "%'");
                foreach (DataRow row in rows)
                {
                    newdt.Rows.Add(row.ItemArray);
                }
                mainGrid3.DataSource = newdt;
            }
        }
    }
}
