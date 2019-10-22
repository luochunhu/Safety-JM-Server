using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace Sys.Safety.Reports
{
    public partial class frmBluerCondition : XtraForm
    {
        private string _strCondition = "";
        private readonly DataTable dt = new DataTable();

        public frmBluerCondition()
        {
            BlnOk = false;
            SelectedDt = null;
            InitializeComponent();
        }

        /// <summary>
        ///     选择栏目数据源
        /// </summary>
        public DataTable SelectedDt { get; set; }


        public bool BlnOk { get; set; }

        /// <summary>
        ///     条件串
        /// </summary>
        public string StrCondition
        {
            get { return _strCondition; }
            set { _strCondition = value; }
        }

        /// <summary>
        ///     Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmConTextCondition_Load(object sender, EventArgs e)
        {
            try
            {
                listBoxControl1.ValueMember = "MetaDataFieldName";
                listBoxControl1.DisplayMember = "strListDisplayFieldNameCHS";
                //selectedDt.DefaultView.RowFilter = "isnull(blnSysProcess,0)=0 and isnull(isCalcField,0)=0";
                SelectedDt.DefaultView.RowFilter = "isnull(blnSysProcess,0)=0 ";
                listBoxControl1.DataSource = SelectedDt;
                GetDt();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     为listBoxControl2赋值
        /// </summary>
        private void GetDt()
        {
            DataRow row = null;
            dt.Columns.Add("strText", Type.GetType("System.String"));
            dt.Columns.Add("strWhere", Type.GetType("System.String"));
            if (_strCondition != string.Empty)
            {
                var strConditions = _strCondition.Split(new[] {"&&$$"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in strConditions)
                {
                    var strs = s.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                    row = dt.NewRow();
                    row["strText"] = strs[0];
                    row["strWhere"] = strs[1];
                    dt.Rows.Add(row);
                }
            }
            gridControl1.DataSource = dt;
        }

        /// <summary>
        ///     双击列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var str = TypeUtil.ToString(listBoxControl1.SelectedValue);
                memoEditFormula.Text += str;
                var rowhandle = gridView1.FocusedRowHandle;
                if (gridView1.FocusedRowHandle >= 0)
                {
                    var strwhere = TypeUtil.ToString(gridView1.GetRowCellValue(rowhandle, "strWhere"));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "strWhere", strwhere + " " + str);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void tlbAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            //convert (TestColumn, 'System.Decimal') > 150 强制转换语法
            var dr = dt.NewRow();
            dr["strText"] = "";
            dr["strWhere"] = "请录入DataTable.Select()的语法,如Name=''";
            dt.Rows.Add(dr);
            gridView1.SelectAll();
        }

        private void tlbDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rowHandle = gridView1.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MessageShowUtil.ShowInfo("请选择要删除的行！");
                    return;
                }
                if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前行吗？"))
                    return;

                gridView1.DeleteRow(rowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbOK_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                gridView1.CloseEditor();
                gridView1.UpdateCurrentRow();
                var strReturn = "";
                for (var i = 0; i < gridView1.RowCount; i++)
                {
                    var strText = TypeUtil.ToString(gridView1.GetRowCellValue(i, "strText"));
                    var strwhere = TypeUtil.ToString(gridView1.GetRowCellValue(i, "strWhere"));
                    if (strwhere.Trim().Length == 0)
                    {
                        MessageShowUtil.ShowInfo("第" + (i + 1) + "行未录入条件，不能保存！");
                        return;
                    }

                    strReturn += strText + ";" + strwhere + "&&$$";
                }
                if (strReturn.Length > 4)
                    strReturn = strReturn.Remove(strReturn.Length - 4);
                _strCondition = strReturn;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }


            BlnOk = true;
            Close();
        }

        private void tlbClear_ItemClick(object sender, ItemClickEventArgs e)
        {
            dt.Rows.Clear();
        }

        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            BlnOk = false;
            Close();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
    }
}