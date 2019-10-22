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
using DevExpress.XtraEditors.Repository;
using Basic.Framework.Common;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Control.PersonInfo
{
    public partial class ItemSelect : XtraForm
    {
        /// <summary>
        /// 选择项
        /// </summary>
        private DataTable _selectItem = null;

        /// <summary>
        /// 选择的id
        /// </summary>
        public List<string> SelectedIds = new List<string>();

        public ItemSelect(string formText, string colText, DataTable selectItem, List<string> selectedIds)
        {
            InitializeComponent();

            SelectedIds = selectedIds;
            _selectItem = selectItem;
            var rows = _selectItem.Select();
            var res = SetCheckValue(rows);
            GridControlSelectItem.DataSource = res;
            Text = formText;
            ColText.Caption = colText;
            LabQueryName.Text = colText + "：";
            SelectCount.Caption = "选中项数量：" + SelectedIds.Count;
        }

        private void ButQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var text = QueryName.Text;
                var rows = _selectItem.Select("Text like '%" + text + "%'", "Text asc");
                var res = SetCheckValue(rows);
                GridControlSelectItem.DataSource = res;
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        private void repositoryItemCheckEdit3_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var focRowHandle = GridViewSelectItem.FocusedRowHandle;
                var id = GridViewSelectItem.GetRowCellValue(focRowHandle, "Id");
                var checkEdit = sender as CheckEdit;
                var ifCheck = checkEdit.Checked;

                if (ifCheck)        //添加选中项
                {
                    var ifContains = SelectedIds.Contains(id.ToString());
                    if (!ifContains)
                    {
                        SelectedIds.Add(id.ToString());
                    }
                }
                else        //取消选中项
                {
                    var ifContains = SelectedIds.Contains(id.ToString());
                    if (ifContains)
                    {
                        SelectedIds.Remove(id.ToString());
                    }
                }
                SelectCount.Caption = "选中项数量：" + SelectedIds.Count;
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        private DataTable SetCheckValue(DataRow[] rows)
        {
            var queryItem = _selectItem.Clone();
            foreach (var item in rows)
            {
                var ifContains = SelectedIds.Contains(item["Id"].ToString());
                if (ifContains)
                {
                    item["Check"] = true;
                }
                else
                {
                    item["Check"] = false;
                }
                queryItem.Rows.Add(item.ItemArray);
            }
            return queryItem;
        }

        private void Affirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
