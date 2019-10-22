using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.ServiceContract;
using Sys.Safety.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmAlarmProcess : XtraForm
    {
        DateTime executeBeginTime = DateTime.Now;
        int dataPageIndex = 1;
        int pageSize = 5000;

        private readonly IAlarmRecordService alarmRecordService;
        private readonly IAlarmHandleService alarmHandleService;

        public List<AlarmProcessInfo> PointAlarmInfos;
        public List<JC_AlarmHandleInfo> AlarmHandleInfos;

        public frmAlarmProcess()
        {
            InitializeComponent();
            dateEdit1.Text = DateTime.Parse(DateTime.Today.ToShortDateString()).ToString("yyyy-MM-dd HH:mm:ss");
            dateEdit2.Text = DateTime.Parse(DateTime.Today.ToShortDateString() + " 23:59:59").ToString("yyyy-MM-dd HH:mm:ss");
            dateEdit3.Text = DateTime.Parse(DateTime.Today.ToShortDateString()).ToString("yyyy-MM-dd HH:mm:ss");
            dateEdit4.Text = DateTime.Parse(DateTime.Today.ToShortDateString() + " 23:59:59").ToString("yyyy-MM-dd HH:mm:ss");
            gridView1.IndicatorWidth = 50;
            gridView2.IndicatorWidth = 50;

            alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();
            alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();

            PointAlarmInfos = new List<AlarmProcessInfo>();
            AlarmHandleInfos = new List<JC_AlarmHandleInfo>();
        }

        private void frmAlarmProcess_Load(object sender, EventArgs e)
        {
            RefreshAlarmRecordInfo(1, pageSize);
        }

        private void RefreshAlarmRecordInfo(int pageindex, int pagesize)
        {
            DateTime stime = Convert.ToDateTime(dateEdit1.Text);
            DateTime etime = Convert.ToDateTime(dateEdit2.Text);

            if (stime > etime)
            {
                XtraMessageBox.Show("开始时间不应大于结束时间");
                return;
            }
            TimeSpan span = etime.Subtract(stime);
            if (span.TotalDays > 2)
            {
                XtraMessageBox.Show("查询时间不能大于3天");
                return;
            }

            SetExecuteBeginTime();
            AlarmRecordGetByStimeRequest request = new AlarmRecordGetByStimeRequest()
            {
                Stime = stime.ToString(),
                ETime = etime.ToString(),
                PagerInfo = new PagerInfo
                {
                    PageSize = pagesize,
                    PageIndex = pageindex
                }
            };
            var alarmresponse = alarmRecordService.GetAlarmRecordListByStime(request);
            if (alarmresponse != null && alarmresponse.IsSuccess && alarmresponse.Data != null)
            {
                PointAlarmInfos = alarmresponse.Data;
                this.pointAlarmGrid.DataSource = PointAlarmInfos;

                //设置分页信息
                SetPageInfo(alarmresponse.PagerInfo);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RefreshAlarmRecordInfo(1, pageSize);
        }

        private void logicAlarmTable_TabIndexChanged(object sender, EventArgs e)
        {
            if (AlarmTable.SelectedTabPageIndex == 1)
            {
                RefreshAlarmRecordInfo(1, pageSize);
            }
            else if (AlarmTable.SelectedTabPageIndex == 2)
            {
                RefreshAlarmHandleInfo(1, pageSize);
            }
        }

        private void RefreshAlarmHandleInfo(int pageindex, int pagesize)
        {
            DateTime stime = Convert.ToDateTime(dateEdit3.Text).Date;
            DateTime etime = Convert.ToDateTime(dateEdit4.Text).Date;

            if (stime > etime)
            {
                XtraMessageBox.Show("开始时间不应大于结束时间");
                return;
            }
            TimeSpan span = etime.Subtract(stime);
            if (span.TotalDays > 2)
            {
                XtraMessageBox.Show("查询时间不能大于3天");
                return;
            }

            SetExecuteBeginTime();
            AlarmHandelGetByStimeAndETime request = new AlarmHandelGetByStimeAndETime()
            {
                Stime = stime,
                Etime = etime,
                PagerInfo = new PagerInfo
                {
                    PageIndex = pageindex,
                    PageSize = pagesize
                }
            };
            var alarmHandleresponse = alarmHandleService.GetAlarmHandleByStimeAndEtime(request);
            if (alarmHandleresponse != null && alarmHandleresponse.IsSuccess)
            {
                AlarmHandleInfos = alarmHandleresponse.Data;
                this.logicAlarmGrid.DataSource = AlarmHandleInfos;

                SetPageInfo(alarmHandleresponse.PagerInfo);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            RefreshAlarmHandleInfo(1, pageSize);
        }

        private void AlarmTable_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (AlarmTable.SelectedTabPageIndex == 0)
            {
                RefreshAlarmRecordInfo(1, pageSize);
            }
            else if (AlarmTable.SelectedTabPageIndex == 1)
            {
                RefreshAlarmHandleInfo(1, pageSize);
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    var alarmId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
                    if (alarmId != null)
                    {
                        string _alarmId = alarmId.ToString();
                        var alarmtime = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Stime");
                        string alarmtimeStr = (DateTime.Parse(alarmtime.ToString())).ToString("yyyyMM");

                        frmAlarmProcessDetail frmDetai = new frmAlarmProcessDetail(1, _alarmId, alarmtimeStr);

                        if (frmDetai.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            RefreshAlarmRecordInfo(dataPageIndex, pageSize);
                        }
                    }
                }
            }
        }

        private void gridView2_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = gridView2.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    var alarmId = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Id");
                    if (alarmId != null)
                    {
                        string _alarmId = alarmId.ToString();
                        frmAlarmProcessDetail frmDetai = new frmAlarmProcessDetail(2, _alarmId, null);

                        if (frmDetai.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            RefreshAlarmHandleInfo(dataPageIndex, pageSize);
                        }
                    }
                }
            }
        }

        #region 分页

        private void tlbGoToFirstPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }

            QueryGridData(1, pageSize);
        }

        private void tlbGoToPreviousPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
                if (pageIndex - 1 > 0)
                {
                    pageIndex = pageIndex - 1;
                }
                else
                {
                    XtraMessageBox.Show("已到第一页");
                }
            }
            catch
            {

            }
            //int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }

        private void tlbGoToPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int totlePage = 1;
            int pageIndex = 1;
            try
            {

                totlePage = TypeUtil.ToInt(tlbTotlePages.Caption.ToString().Replace("/", ""));
                pageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
                if (pageIndex >= totlePage)
                {
                    pageIndex = totlePage;
                }
                else if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
            }
            catch
            {

            }
            //int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }

        private void tlbGoToNextPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
                if (pageIndex < TypeUtil.ToInt(tlbTotlePages.Caption.ToString().Replace("/", "")))
                {
                    pageIndex = pageIndex + 1;
                }
                else
                {
                    XtraMessageBox.Show("已到最后一页");
                    return;
                }
            }
            catch
            {

            }
            //int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }

        private void tlbGoToLastPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageIndex = TypeUtil.ToInt(tlbTotlePages.Caption.ToString().Replace("/", ""));

            }
            catch
            {

            }
            //int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }

        private void tlbSetPerPageNumber_EditValueChanged(object sender, EventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }

        /// <summary>
        ///     设置执行开始时间
        /// </summary>
        protected void SetExecuteBeginTime()
        {
            executeBeginTime = DateTime.Now;
        }

        /// <summary>
        ///     获取执行时间字符串
        /// </summary>
        /// <returns></returns>
        protected string GetExecuteTimeString()
        {
            var strTime = string.Empty;
            var ts = DateTime.Now - executeBeginTime;
            if (ts.Minutes > 0)
                strTime += ts.Minutes + "分";
            if (ts.Seconds > 0)
                strTime += ts.Seconds + "秒";
            if (ts.Milliseconds > 0)
                strTime += ts.Milliseconds + "毫秒";

            strTime += "。";

            return strTime;
        }
        public bool RegexInteger(string IInteger)
        {
            Regex g = new Regex(@"^[0-9]\d*$");
            return g.IsMatch(IInteger);
        }

        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="pagerInfo"></param>
        private void SetPageInfo(Basic.Framework.Web.PagerInfo pagerInfo)
        {
            dataPageIndex = pagerInfo.PageIndex;
            pageSize = pagerInfo.PageSize;

            //barEditItemPage.EditValue = pagerInfo.PageIndex + 1;
            barEditItemPage.EditValue = pagerInfo.PageIndex;

            int TotalPage = pagerInfo.RowCount / pagerInfo.PageSize;
            if (pagerInfo.RowCount % pagerInfo.PageSize > 0)
                TotalPage = TotalPage + 1;
            tlbTotlePages.Caption = "/" + TotalPage.ToString();
            barStaticItemRowCount.Caption = "共计" + pagerInfo.RowCount + "条记录。";
            barStaticItemMsg.Caption = "执行时间:" + GetExecuteTimeString();
        }

        private void QueryGridData(int pageIndex, int pageSize)
        {
            if (AlarmTable.SelectedTabPageIndex == 0)
            {
                RefreshAlarmRecordInfo(pageIndex, pageSize);
            }
            else
            {
                RefreshAlarmHandleInfo(pageIndex, pageSize);
            }
        }
        #endregion

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                //行号=(当前页数-1)*每页大小+当前行+1
                var rowindex = (dataPageIndex - 1) * pageSize + e.RowHandle + 1;
                e.Info.DisplayText = rowindex.ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                //行号=(当前页数-1)*每页大小+当前行+1
                var rowindex = (dataPageIndex - 1) * pageSize + e.RowHandle + 1;
                e.Info.DisplayText = rowindex.ToString();
            }
        }

    }
}
