using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.Request.Graphicsbaseinf;
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

namespace Sys.Safety.Client.Define.Station
{
    public partial class WindPowerLockPointBind : XtraForm
    {
        private string _EditPoint = "";
        private short _StationNumber = 0;
        private StationWindPowerLock _StationWindPowerLock;
        private IGraphicsbaseinfService _GraphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();
        public WindPowerLockPointBind(short StationNumber, string PointName, StationWindPowerLock StationWindPowerLock)
        {
            _StationNumber = StationNumber;
            _EditPoint = PointName;
            _StationWindPowerLock = StationWindPowerLock;
            InitializeComponent();
        }
        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return new DataTable();
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构  
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中  
            return tmp;
        }
        private void WindPowerLockPointBind_Load(object sender, EventArgs e)
        {
            //加载列表
            DataTable dtRvalue = new DataTable();
            var request = new LoadAllpointDefByTypeRequest() { Type = "4" };
            var resposne = _GraphicsbaseinfService.LoadAllpointDefByType(request);
            dtRvalue = resposne.Data;
            if (dtRvalue.Rows.Count > 0)
            {
                DataRow[] _DataRow = dtRvalue.Select("DevName like '%开停%' and Point like '" + _StationNumber.ToString("000") + "%'");
                dtRvalue = ToDataTable(_DataRow);
            }
            if (dtRvalue.Rows.Count > 0)
            {
                gridControl1.DataSource = dtRvalue;
                //设置选择状态
                for (int i = 0; i < dtRvalue.Rows.Count; i++)
                {
                    if (_EditPoint.Contains(gridView1.GetDataRow(i).ItemArray[0].ToString()))
                    {
                        gridView1.FocusedRowHandle = i;
                    }
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            string Point = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString();
            string Wz = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Wz").ToString();
            string DevName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DevName").ToString();
            _StationWindPowerLock.SetPointInMap(_EditPoint, Point, Wz, DevName);
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Point = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString();
            string Wz = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Wz").ToString();
            string DevName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DevName").ToString();
            _StationWindPowerLock.SetPointInMap(_EditPoint, Point, Wz, DevName);
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
