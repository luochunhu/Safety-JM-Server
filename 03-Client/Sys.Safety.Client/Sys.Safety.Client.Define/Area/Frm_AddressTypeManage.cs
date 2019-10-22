using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
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
using Sys.Safety.Request.KJ_Addresstype;
using Sys.Safety.Request.KJ_Addresstyperule;

namespace Sys.Safety.Client.Define
{
    public partial class Frm_AddressTypeManage : XtraForm
    {
        IKJ_AddresstyperuleService AddresstyperuleService;
        IKJ_AddresstypeService AddressTypeService;
        DataTable dt = new DataTable();
        IPointDefineService pointDefineService;

        public Frm_AddressTypeManage()
        {
            InitializeComponent();
        }
        public void Reload()
        {
            object sender1 = null;
            var e1 = new EventArgs();
            Frm_CheckAddressType_Load(sender1, e1);
        }
        private void IniTable()
        {
            dt.Columns.Clear();
            dt.Columns.Add("ID");
            dt.Columns.Add("Addresstypename");
            dt.Columns.Add("Addresstypedesc");
            dt.Columns.Add("Createupdatetime");    
            gridControl1.DataSource = dt;
        }

        public void LoadAddressType()
        {
            try
            {
                List<KJ_AddresstypeInfo> AddressTypeItems = GetAddressType();
                dt.Rows.Clear();
                for (int i = 0; i < AddressTypeItems.Count; i++)
                {
                    dt.Rows.Add(AddressTypeItems[i].ID, AddressTypeItems[i].Addresstypename, AddressTypeItems[i].Addresstypedesc,AddressTypeItems[i].Createupdatetime);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadAddressType Error:" + ex.Message);
            }
        }

        private void Frm_CheckAddressType_Load(object sender, EventArgs e)
        {
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
            AddressTypeService = ServiceFactory.Create<IKJ_AddresstypeService>();
            AddresstyperuleService = ServiceFactory.Create<IKJ_AddresstyperuleService>();
            IniTable();
            LoadAddressType();
        }

        private List<KJ_AddresstypeInfo> GetAddressType()
        {
            List<KJ_AddresstypeInfo> AddressTypeItems = new List<KJ_AddresstypeInfo>();
            try
            {
                KJ_AddresstypeGetListRequest kJ_AddresstypeRequest = new KJ_AddresstypeGetListRequest();

                var result = AddressTypeService.GetKJ_AddresstypeList(kJ_AddresstypeRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    AddressTypeItems = result.Data.OrderBy(a => a.Createupdatetime).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadAddressType Error:" + ex.Message);
            }
            return AddressTypeItems;
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
                    string AddressTypeID = dr["ID"].ToString();

                    Frm_AddressTypeDefine fa = new Frm_AddressTypeDefine(AddressTypeID, 1);
                    fa.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 添加地点类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_AddressTypeDefine AddressTypeDefine = new Frm_AddressTypeDefine("", 0);
            AddressTypeDefine.ShowDialog();
            if (AddressTypeDefine.DialogResult == DialogResult.OK)
            {
                LoadAddressType();
            }
        }
        /// <summary>
        /// 编辑地点类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string AddressTypeid = this.gridView1.GetRowCellValue(selectedHandle, "ID").ToString();
                Frm_AddressTypeDefine AddressTypeDefine = new Frm_AddressTypeDefine(AddressTypeid, 1);
                AddressTypeDefine.ShowDialog();
                if (AddressTypeDefine.DialogResult == DialogResult.OK)
                {
                    LoadAddressType();
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string AddressTypeid = this.gridView1.GetRowCellValue(selectedHandle, "ID").ToString();
                //删除前判断当前地点类型下面是否有活动测点  20170829
                PointDefineGetByAddressTypeIdRequest PointDefineRequest = new PointDefineGetByAddressTypeIdRequest();
                PointDefineRequest.AddressTypeId = AddressTypeid;
                var result = pointDefineService.GetPointDefineCacheByAddressTypeId(PointDefineRequest);
                if (result.Data != null && result.Data.Count > 0)
                {
                    XtraMessageBox.Show("当前地点类型下面存在已定义设备，请先删除设备，再删除当前地点类型！");
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    KJ_AddresstypeDeleteRequest AddressTyperequest = new KJ_AddresstypeDeleteRequest();
                    AddressTyperequest.Id = AddressTypeid;
                    AddressTypeService.DeleteKJ_Addresstype(AddressTyperequest);
                    //删除当前地点类型下面的定义的规则
                    KJ_AddresstyperuleDeleteByAddressTypeIdRequest kJ_AddresstyperuleRequest=new KJ_AddresstyperuleDeleteByAddressTypeIdRequest();
                    kJ_AddresstyperuleRequest.AddressTypeId=AddressTypeid;
                    AddresstyperuleService.DeleteKJ_AddresstyperuleByAddressTypeId(kJ_AddresstyperuleRequest);
                    LoadAddressType();
                }
            }
        }
    }
}
