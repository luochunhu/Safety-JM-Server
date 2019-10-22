using Basic.Framework.Logging;
using Basic.Framework.Web;
using DevExpress.XtraEditors;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.Client.DataAnalysis.Common;
using Sys.Safety.DataContract;
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

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetRegionOutageManage : XtraForm
    {
        DateTime executeBeginTime = DateTime.Now;
        int dataPageIndex = 1;
        List<JC_LargedataAnalysisConfigInfo> JC_LargedataAnalysisConfigInfoList = new List<JC_LargedataAnalysisConfigInfo>();
       
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        RegionOutageBusiness regionOutageBusiness;
        public SetRegionOutageManage()
        {
            InitializeComponent();
            try
            {
                regionOutageBusiness = new RegionOutageBusiness();
                largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
                //初始化列表数据
                QueryGridData(1, 50);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 修改区域断电配置
        /// </summary>
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] Handle = gridViewModel.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择要修改区域断电配置的分析模型", "消息");
                    return;
                }
                int rowHandle = gridViewModel.FocusedRowHandle;
                string daID = gridViewModel.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值
                SetRegionOutage setRegionOutage = new SetRegionOutage(daID);
                setRegionOutage.ShowDialog();
                btnQuery_ItemClick(sender, e);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 新增区域断电配置
        /// </summary>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetRegionOutage setRegionOutage = new SetRegionOutage(string.Empty);
            setRegionOutage.ShowDialog();
            btnQuery_ItemClick(sender, e);
        }
        /// <summary>
        /// 删除区域断电配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] Handle = gridViewModel.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择需要删除的记录", "消息");
                    return;
                }
                if (XtraMessageBox.Show("确定删除所选分析模型的断电配置吗?", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    int rowHandle = gridViewModel.FocusedRowHandle;
                    string daID = gridViewModel.GetRowCellValue(rowHandle, "Id").ToString(); //是ookUpEdit.Properties.ValueMember的值

                    string error = regionOutageBusiness.DeleteJC_RegionoutageconfigByAnalysisModelId(daID);

                    if (error == "100")
                    {
                        XtraMessageBox.Show("删除成功", "消息");
                        OperateLogHelper.InsertOperateLog(16, "区域断电配置-删除【" + gridViewModel.GetRowCellValue(rowHandle, "Name").ToString() + "】," + string.Format("内容:Id:{0},模型名称:{1}", daID, gridViewModel.GetRowCellValue(rowHandle, "Name").ToString()), "区域断电配置-删除");
                        btnQuery_ItemClick(sender, e);
                    }
                    else
                    {
                        XtraMessageBox.Show(error, "提示信息");
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(string.Format("删除区域断电配置失败! 提示信息: {0}", ex.Message), "提示信息");
            }
        }

        /// <summary>
        /// 查询区域断电配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }

            QueryGridData(1, pageSize);
        }

        private void gridViewModel_MasterRowExpanding(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowCanExpandEventArgs e)
        {
            List<JC_LargedataAnalysisConfigInfo> dataList = gridControlModel.DataSource as List<JC_LargedataAnalysisConfigInfo>;
            if (dataList == null || dataList.Count == 0)
            {
                return;
            }

            int rowHandle = gridViewModel.FocusedRowHandle;
            string daID = gridViewModel.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值

            //1.根据模型ID查询模型区域断电配置信息
            List<JC_RegionOutageConfigInfo> regionOutageConfigInfoList = regionOutageBusiness.GetRegionOutage(daID);

            List<Jc_DefInfo> LevelTrueDescription = new List<Jc_DefInfo>();
            List<Jc_DefInfo> LevelFalseDescription = new List<Jc_DefInfo>();
            if (regionOutageConfigInfoList != null && regionOutageConfigInfoList.Count > 0)
            {
                foreach (var item in regionOutageConfigInfoList)
                {
                    //2.根据测点ID查询测点信息
                    Jc_DefInfo jc_DefInfo = new Jc_DefInfo();
                    jc_DefInfo = PointDefineBusiness.QueryPointByPointID(item.PointId);
                    if (item.ControlStatus == 1)
                    {
                        LevelTrueDescription.Add(jc_DefInfo);
                    }
                    else
                    {
                        LevelFalseDescription.Add(jc_DefInfo);
                    }
                }
            }

            //3.初始化测点数据
            foreach (var item in dataList)
            {
                if (item.Id == daID)
                {
                    item.LevelTrueDescription = LevelTrueDescription;
                    item.LevelFalseDescription = LevelFalseDescription;
                }
            }

            gridControlModel.DataSource = dataList;
        }

        private void SetRegionOutageManage_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///  点第一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbGoToFirstPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }

            QueryGridData(1, pageSize);
        }
        /// <summary>
        /// 点上一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    MessageShowUtil.ShowInfo("已到第一页");
                }
            }
            catch
            {

            }
            int pageSize = 50;
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
        /// 点GOTO按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            int pageSize = 50;
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
        /// 选择每页显示的数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSetPerPageNumber_EditValueChanged(object sender, EventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageIndex = TypeUtil.ToInt(tlbTotlePages.Caption.ToString().Replace("/", ""));

            }
            catch
            {

            }
            int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(1, pageSize);
        }
        /// <summary>
        /// 点下一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbGoToNextPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int pageIndex = 1;
            try
            {
                pageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
                if (pageIndex + 1 <= TypeUtil.ToInt(tlbTotlePages.Caption.ToString().Replace("/", "")))
                {
                    pageIndex = pageIndex + 1;
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到最后一页");
                }
            }
            catch
            {

            }
            int pageSize = 50;
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
        /// 点最后一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            int pageSize = 50;
            try
            {
                pageSize = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
            }
            catch
            {

            }
            QueryGridData(pageIndex, pageSize);
        }
        private void QueryGridData(int pageIndex, int pageSize)
        {
            try
            {
                SetExecuteBeginTime();
                LargedataAnalysisConfigBusinessModel model = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigWithRegionOutagePage(textEditModelName.Text.Trim(), pageIndex, pageSize);

                gridControlModel.DataSource = model.LargedataAnalysisConfigInfoList;
                gridViewModel.FocusedRowHandle = -1;
                //设置分页信息
                SetPageInfo(model.pagerInfo);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "查询模板失败");
            }
        }
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="pagerInfo"></param>
        private void SetPageInfo(PagerInfo pagerInfo)
        {
            dataPageIndex = pagerInfo.PageIndex + 1;

            barEditItemPage.EditValue = pagerInfo.PageIndex + 1;
            int TotalPage = pagerInfo.RowCount / pagerInfo.PageSize;
            if (pagerInfo.RowCount % pagerInfo.PageSize > 0)
                TotalPage = TotalPage + 1;
            tlbTotlePages.Caption = "/" + TotalPage.ToString();
            barStaticItemRowCount.Caption = "共计" + pagerInfo.RowCount + "条记录。";
            barStaticItemMsg.Caption = "执行时间:" + GetExecuteTimeString();
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
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewModel_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridViewTrueDescription_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridViewFalseDescription_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
    }
}
