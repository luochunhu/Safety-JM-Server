using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Reports.Controls;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports
{
    public partial class frmDataSortSetEdit : DevExpress.XtraEditors.XtraForm
    {
        private long ListDataID;

        /// <summary>
        /// 是否确认
        /// </summary>
        public bool IsOk = false;

        /// <summary>
        /// 已选择的测点
        /// </summary>
        public string SelPoint = "";

        public DateTime SelTime;

        public frmDataSortSetEdit(long listDataID, bool isAdd, string selPoint)
        {
            InitializeComponent();

            ListDataID = listDataID;
            bool blnNewData = false;

            // 20171225
            //var lookInfo = LookUpUtil.GetlookInfo("AllPointActivity", ref blnNewData);
            var lookInfo = LookUpUtil.GetlookInfo("AllPointMultisystem", ref blnNewData);

            var dt = (lookInfo["dataSource"] as DataTable).Copy();
            DataColumn dc = new DataColumn("BlnSelect", typeof(bool));
            dc.DefaultValue = false;
            dt.Columns.Add(dc);

            if (!isAdd)
            {
                string[] sArySelPoint = selPoint.Split(',');
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    var point = gridView1.GetRowCellValue(i, "Point");
                    if (sArySelPoint.Contains(point))
                    {
                        gridView1.SetRowCellValue(i, "BlnSelect", true);
                    }
                    else
                    {
                        gridView1.SetRowCellValue(i, "BlnSelect", false);
                    }
                }
            }
            else
            {
                bpsj.DateTime = DateTime.Now;
            }

            gridControl1.DataSource = dt;
        }

        private void saveArrange_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IList<ListdatalayountInfo> lists = new ListExModel().GetListDataLayoutDataA(Convert.ToInt32(ListDataID), bpsj.DateTime.ToString("yyyyMMdd"));
            if (lists.Count > 0)
            {
                MessageShowUtil.ShowInfo("该时间已存在编排。");
                return;
            }

            gridView1.CloseEditor();
            StringBuilder sbPoint = new StringBuilder();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                var blnSel = Convert.ToBoolean(gridView1.GetRowCellValue(i, "BlnSelect"));
                if (blnSel)
                {
                    sbPoint.Append("'" + gridView1.GetRowCellValue(i, "point") + "',");
                }
            }

            if (sbPoint.Length == 0)
            {
                MessageShowUtil.ShowInfo("请先选择测点。");
                return;
            }
            else
            {
                SelTime = bpsj.DateTime;
                SelPoint = sbPoint.ToString().Substring(0, sbPoint.Length - 1);
                IsOk = true;
                this.Close();
            }
        }
    }
}