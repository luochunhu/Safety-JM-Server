using Basic.Framework.Common;
using DevExpress.XtraEditors;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetPoint : XtraForm
    {
        private int devicePropertyId = -1;
        private string id = string.Empty;
        private AddLargedataAnalysis invokeForm;
        private bool isSingle = true;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="invokeForm">设置分析模型测点定义的窗体</param>
        /// <param name="Id">表达式测点定义Id</param>
        /// <param name="devicePropertyId">设备性质Id</param>
        /// <param name="isSingle">是否单选</param>
        public SetPoint(AddLargedataAnalysis invokeForm, string id, int devicePropertyId, bool isSingle = true)
        {
            this.invokeForm = invokeForm;
            this.id = id;
            this.devicePropertyId = devicePropertyId;
            this.isSingle = isSingle;
            InitializeComponent();
        }

        private void SetPoint_Load(object sender, System.EventArgs e)
        {
            loadDataSource();
            bindGrid();
            bindDevClass();
        }

        private List<Jc_DefInfo> dataSource;
        private List<KeyValuePair<int, string>> devClassSource = new List<KeyValuePair<int, string>>();
        private void loadDataSource()
        {
            dataSource = PointDefineBusiness.QueryPointByDevpropertIDCache(this.devicePropertyId);
            devClassSource.Add(new KeyValuePair<int, string>(-1, "未选择"));
            foreach (var item in dataSource)
            {
                if (!devClassSource.Any(q => q.Key == item.DevClassID))
                    devClassSource.Add(new KeyValuePair<int, string>(item.DevClassID, item.DevClass));
            }
        }

        private void bindDevClass()
        {
            gridLookUpDevClass.Properties.DataSource = devClassSource;
            gridLookUpDevClass.EditValue = -1;
        }

        private void bindGrid()
        {
            var devClassID = -1;
            if (gridLookUpDevClass.EditValue != null && gridLookUpDevClass.EditValue.ToString() != "")
                devClassID = int.Parse(gridLookUpDevClass.EditValue.ToString());
            List<Jc_DefInfo> gridDataSource = ObjectConverter.DeepCopy<List<Jc_DefInfo>>(dataSource);
            if (!string.IsNullOrEmpty(textEdit1.Text))
            {
                gridDataSource = gridDataSource.Where(q => q.Point.Contains(textEdit1.Text)).ToList();
            }
            if(devClassID > -1)
                gridDataSource = gridDataSource.Where(q => q.DevClassID == devClassID).ToList();
            gridDataSource = gridDataSource.OrderBy(q => q.Point).ToList();
            this.gridControl1.DataSource = gridDataSource;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bindGrid();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            StringBuilder sbPoint = new StringBuilder();
            StringBuilder sbPointId = new StringBuilder();
            int[] selectedRows = this.gridView1.GetSelectedRows();
            if (this.isSingle && selectedRows.Length > 1)
            {
                XtraMessageBox.Show("请选择单个测点.", "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < selectedRows.Length; i++)
            {
                string point = this.gridView1.GetRowCellValue(selectedRows[i], "Point").ToString();
                string pointId = this.gridView1.GetRowCellValue(selectedRows[i], "PointID").ToString();
                if (!string.IsNullOrEmpty(point))
                {
                    sbPoint.Append(point);
                    sbPoint.Append(",");
                }
                if (!string.IsNullOrEmpty(pointId))
                {
                    sbPointId.Append(pointId);
                    sbPointId.Append(",");
                }
            }
            //没有选择表示清空
            //if (sbPoint.ToString() != string.Empty && sbPointId.ToString() != string.Empty)
            //{
            string points = sbPoint.ToString().TrimEnd(new char[] { ',' });
            string pointIds = sbPointId.ToString().TrimEnd(new char[] { ',' });
            invokeForm.ReBindGrid(this.id, points, pointIds);
            //}
            this.Close();
        }
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
    }
}
