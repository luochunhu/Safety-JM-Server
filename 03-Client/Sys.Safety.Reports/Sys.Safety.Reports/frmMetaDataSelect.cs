using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports
{
    public partial class frmMetaDataSelect : XtraForm
    {
        private DataTable dt;

        public frmMetaDataSelect()
        {
            MetadataId = 0;
            InitializeComponent();
        }


        /// <summary>
        ///     选择主实体ID
        /// </summary>
        public int MetadataId { get; set; }

        private void frmMetaDataSelect_Load(object sender, EventArgs e)
        {
            try
            {
                dt = ClientCacheModel.GetServerMetaData();
                gridControl1.DataSource = dt;

                var strFilter = "";
                dt.DefaultView.RowFilter = strFilter;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SetSelectMetaData();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     查找过滤
        /// </summary>
        private void buttonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
                Search();
        }

        private void buttonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Search();
        }

        private void Search()
        {
            var strFilter = "strBusinessModule<>'GG'";
            strFilter += " and strName like '%" + buttonEdit1.Text.Trim() + "%' or strTableName like '%" +
                         buttonEdit1.Text.Trim() + "%'";
            dt.DefaultView.RowFilter = strFilter;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            SetSelectMetaData();
            Close();
        }

        private void SetSelectMetaData()
        {
            var rowHandle = gridView1.FocusedRowHandle;
            MetadataId = TypeUtil.ToInt(gridView1.GetRowCellValue(rowHandle, "ID"));
        }

        private void buttonEdit1_EditValueChanging(object sender, ChangingEventArgs e)
        {
            Search();
        }


        private void frmMetaDataSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}