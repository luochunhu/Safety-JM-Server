using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.PointDefine;
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

namespace Sys.Safety.Client.Define.Area
{
    public partial class Frm_CheckArea : XtraForm
    {
        IAreaService areaService;
        DataTable dt = new DataTable();
        IPointDefineService pointDefineService;

        public Frm_CheckArea()
        {
            InitializeComponent();
        }
        public void Reload()
        {
            object sender1 = null;
            var e1 = new EventArgs();
            Frm_CheckArea_Load(sender1, e1);
        }
        private void IniTable()
        {
            dt.Columns.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("AreaName");
            dt.Columns.Add("AreaBound");
            dt.Columns.Add("AreaRules");
            dt.Columns.Add("CreateUpdateTime");
            dt.Columns.Add("Areaid");
            gridControl1.DataSource = dt;
        }

        public void LoadArea()
        {
            try
            {
                List<AreaInfo> areaItems = GetArea();
                dt.Rows.Clear();
                for (int i = 0; i < areaItems.Count; i++)
                {
                    dt.Rows.Add(i + 1, areaItems[i].Areaname, areaItems[i].AreaBound, "双击查看", areaItems[i].CreateUpdateTime, areaItems[i].Areaid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadArea Error:" + ex.Message);
            }
        }

        private void Frm_CheckArea_Load(object sender, EventArgs e)
        {
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
            areaService = ServiceFactory.Create<IAreaService>();
            IniTable();
            LoadArea();
        }

        private List<AreaInfo> GetArea()
        {
            List<AreaInfo> areaItems = new List<AreaInfo>();
            try
            {
                AreaGetListRequest areaGetListRequest = new AreaGetListRequest();

                var result = areaService.GetAllAreaList(areaGetListRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    areaItems = result.Data.OrderBy(a => a.Areaname).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadArea Error:" + ex.Message);
            }
            return areaItems;
        }
        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Left && e.Clicks == 2)
                {
                    int rowIndex = gridView1.FocusedRowHandle;
                    DataRow dr = gridView1.GetDataRow(rowIndex);
                    string areaID = dr["AreaID"].ToString();

                    Frm_AreaDefine fa = new Frm_AreaDefine(areaID, 1);
                    fa.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 添加区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_AreaDefine areaDefine = new Frm_AreaDefine("", 0);
            areaDefine.ShowDialog();
            if (areaDefine.DialogResult == DialogResult.OK)
            {
                LoadArea();
            }
        }
        /// <summary>
        /// 编辑区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string areaid = this.gridView1.GetRowCellValue(selectedHandle, "Areaid").ToString();
                Frm_AreaDefine areaDefine = new Frm_AreaDefine(areaid, 1);
                areaDefine.ShowDialog();
                if (areaDefine.DialogResult == DialogResult.OK)
                {
                    LoadArea();
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string areaid = this.gridView1.GetRowCellValue(selectedHandle, "Areaid").ToString();
                //删除前判断当前区域下面是否有活动测点  20170829
                PointDefineGetByAreaIdRequest PointDefineRequest = new PointDefineGetByAreaIdRequest();
                PointDefineRequest.AreaId = areaid;
                var result = pointDefineService.GetPointDefineCacheByAreaId(PointDefineRequest);
                if (result.Data != null && result.Data.Count > 0)
                {
                    XtraMessageBox.Show("当前区域下面存在已定义设备，请先删除设备，再删除当前区域！");
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AreaDeleteRequest arearequest = new AreaDeleteRequest();
                    arearequest.Id = areaid;
                    areaService.DeleteArea(arearequest);
                    LoadArea();
                }
            }
        }
    }
}
