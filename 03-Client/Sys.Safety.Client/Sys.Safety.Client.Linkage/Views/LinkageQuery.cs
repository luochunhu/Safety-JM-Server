using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.Client.Linkage.Handlers;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.Client.Linkage.Views
{
    public partial class LinkageQuery : XtraForm
    {
        private DataTable _dtDetailInfo = new DataTable();

        private List<GetSysEmergencyLinkageListAndStatisticsResponse> _currentGridData;

        public LinkageQuery()
        {
            InitializeComponent();
        }

        private void LinkageQuery_Load(object sender, EventArgs e)
        {
            var res = LinkageHandler.GetSysEmergencyLinkageListAndStatistics("");
            _currentGridData = res;
            GridControlLinkage.DataSource = res;

            _dtDetailInfo = new DataTable();
            _dtDetailInfo.Columns.Add("Id");
            _dtDetailInfo.Columns.Add("Text");
        }

        private void Query_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var wait = new WaitDialogForm("数据加载中", "请等待....");
            try
            {
                var name = QueryName.Text;
                var res = LinkageHandler.GetSysEmergencyLinkageListAndStatistics(name);
                _currentGridData = res;
                GridControlLinkage.DataSource = res;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                wait.Close();
            }
        }

        private void Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var form = new LinkageDefinition();
                var res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                Query_ItemClick(null, null);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var res2 = XtraMessageBox.Show("确认要执行该操作吗？", "提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (res2 != DialogResult.Yes)
            {
                return;
            }
            var wait = new WaitDialogForm("数据加载中", "请等待....");
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();
                LinkageHandler.SoftDeleteSysEmergencyLinkage(res.Id);
                Query_ItemClick(null, null);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                wait.Close();
            }
        }

        private void MasterInfoDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();

                if (res == null)
                {
                    XtraMessageBox.Show("请先选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (res.MasterPointNum == "0" && res.MasterAreaNum == "0" && res.MasterDevTypeNum == "0")
                {
                    XtraMessageBox.Show("该应急联动项非普通应急联动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (res.MasterPointNum != "0")
                {
                    var points = LinkageHandler.GetMasterPointInfoByAssId(res.MasterPointAssId);
                    _dtDetailInfo.Rows.Clear();
                    foreach (var item in points)
                    {
                        var row = _dtDetailInfo.NewRow();
                        row["Id"] = item.PointID;
                        row["Text"] = item.Point + "（" + item.Wz + "）";
                        _dtDetailInfo.Rows.Add(row);
                    }

                    var form = new LinkageDetail("主控信息", "名称", _dtDetailInfo);
                    form.ShowDialog();
                }

                if (res.MasterAreaNum != "0")
                {
                    var points = LinkageHandler.GetMasterAreaInfoByAssId(res.MasterAreaAssId);
                    _dtDetailInfo.Rows.Clear();
                    foreach (var item in points)
                    {
                        var row = _dtDetailInfo.NewRow();
                        row["Id"] = item.Areaid;
                        row["Text"] = item.Areaname;
                        _dtDetailInfo.Rows.Add(row);
                    }

                    var form = new LinkageDetail("主控区域", "区域名称", _dtDetailInfo);
                    form.ShowDialog();
                }

                if (res.MasterDevTypeNum != "0")
                {
                    var points = LinkageHandler.GetMasterEquTypeInfoByAssId(res.MasterDevTypeAssId);
                    _dtDetailInfo.Rows.Clear();
                    foreach (var item in points)
                    {
                        var row = _dtDetailInfo.NewRow();
                        row["Id"] = item.Devid;
                        row["Text"] = item.Name;
                        _dtDetailInfo.Rows.Add(row);
                    }

                    var form = new LinkageDetail("主控设备类型", "设备类型名称", _dtDetailInfo);
                    form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>获取选择行的详细数据
        /// 
        /// </summary>
        /// <returns></returns>
        private GetSysEmergencyLinkageListAndStatisticsResponse GetSysEmergencyLinkageByGridViewFocused()
        {
            var focRowHandle = GridViewLinkage.FocusedRowHandle;
            if (focRowHandle < 0)
            {
                return null;
            }
            string nodeId = GridViewLinkage.GetRowCellValue(focRowHandle, "Id").ToString();
            var res = _currentGridData.First(a => a.Id == nodeId);
            return res;
        }

        private void MasterTirDataStateDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();

                if (res == null)
                {
                    XtraMessageBox.Show("请先选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var masterTriDataState = LinkageHandler.GetMasterTriDataStateByAssId(res.MasterTriDataStateAssId);
                _dtDetailInfo.Rows.Clear();
                foreach (var item in masterTriDataState)
                {
                    var row = _dtDetailInfo.NewRow();
                    row["Id"] = item.LngEnumValue;
                    row["Text"] = item.StrEnumDisplay;
                    _dtDetailInfo.Rows.Add(row);
                }

                var form = new LinkageDetail("主控触发数据状态", "数据状态", _dtDetailInfo);
                form.ShowDialog();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassivePersonDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();

                if (res == null)
                {
                    XtraMessageBox.Show("请先选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var personNum = res.PassivePersonNum;
                if (personNum == "0")
                {
                    XtraMessageBox.Show("未配置被控人员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var points = LinkageHandler.GetPassivePersonByAssId(res.PassivePersonAssId);
                _dtDetailInfo.Rows.Clear();
                foreach (var item in points)
                {
                    var row = _dtDetailInfo.NewRow();
                    row["Id"] = item.Id;
                    row["Text"] = item.Name;
                    _dtDetailInfo.Rows.Add(row);
                }

                var form = new LinkageDetail("被控人员", "人员名称", _dtDetailInfo);
                form.ShowDialog();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassiveOtherInfoDetaile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();

                if (res == null)
                {
                    XtraMessageBox.Show("请先选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (res.PassiveAreaNum == "0" && res.PassivePointNum == "0")
                {
                    XtraMessageBox.Show("该应急联动项未配置被控测点或区域！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (res.PassivePointNum != "0")
                {
                    var points = LinkageHandler.GetPassivePointInfoByAssId(res.PassivePointAssId);
                    _dtDetailInfo.Rows.Clear();
                    foreach (var item in points)
                    {
                        var row = _dtDetailInfo.NewRow();
                        row["Id"] = item.Id;
                        row["Text"] = item.Text;
                        _dtDetailInfo.Rows.Add(row);
                    }

                    var form = new LinkageDetail("被控其他信息", "名称", _dtDetailInfo);
                    form.ShowDialog();
                }

                if (res.PassiveAreaNum != "0")
                {
                    var points = LinkageHandler.GetPassiveAreaInfoByAssId(res.PassiveAreaAssId);
                    _dtDetailInfo.Rows.Clear();
                    foreach (var item in points)
                    {
                        var row = _dtDetailInfo.NewRow();
                        row["Id"] = item.Areaid;
                        row["Text"] = item.Areaname;
                        _dtDetailInfo.Rows.Add(row);
                    }

                    var form = new LinkageDetail("主控测点", "测点号", _dtDetailInfo);
                    form.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItemDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var res = GetSysEmergencyLinkageByGridViewFocused();

                if (res == null)
                {
                    XtraMessageBox.Show("请先选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var form = new LinkageDetailAll(res);
                form.Show();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var wait = new WaitDialogForm("数据加载中", "请等待....");
            try
            {
                var name = QueryName.Text;
                var res = LinkageHandler.GetSysEmergencyLinkageListAndStatistics(name);
                _currentGridData = res;
                GridControlLinkage.DataSource = res;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                wait.Close();
            }
        }
    }
}
