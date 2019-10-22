using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.Client.DataAnalysis.Common;
using Sys.Safety.DataContract;
using Sys.Safety.Request.SysEmergencyLinkage;
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
using Basic.Framework.Web;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class LargedataAnalysisManage : XtraForm
    {
        DateTime executeBeginTime = DateTime.Now;
        int dataPageIndex = 1;
        List<JC_LargedataAnalysisConfigInfo> JC_LargedataAnalysisConfigInfoList = new List<JC_LargedataAnalysisConfigInfo>();

        private ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        public LargedataAnalysisManage()
        {
            InitializeComponent();

        }

        /// <summary>
        ///  初始化窗体
        /// </summary>
        public void LoadForm()
        {
            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();

            gridControlData.DataSource = JC_LargedataAnalysisConfigInfoList;
            //初始化列表数据
            QueryGridData(1, 50);
        }



        private void LargedataAnalysisManage_Load(object sender, EventArgs e)
        {

            try
            {
                if (!GetIsDesignMode())
                {
                    LoadForm();
                }
            }
            catch
            {

            }
        }
        /// <summary>  
        /// 获取当前是否处于设计器模式  
        /// </summary>  
        /// <remarks>  
        /// 在程序初始化时获取一次比较准确，若需要时获取可能由于布局嵌套导致获取不正确，如GridControl-GridView组合。  
        /// </remarks>  
        /// <returns>是否为设计器模式</returns>  
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) != null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddLargedataAnalysis addLargedataAnalysis = new AddLargedataAnalysis();
            addLargedataAnalysis.ShowDialog();
            btnQuery_ItemClick(sender, e);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] Handle = gridViewData.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }
                int rowHandle = gridViewData.FocusedRowHandle;
                string daID = gridViewData.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值
                AddLargedataAnalysis addLargedataAnalysis = new AddLargedataAnalysis(daID);
                addLargedataAnalysis.ShowDialog();
                btnQuery_ItemClick(sender, e);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "修改模型失败");
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string daID = null;
            string daName = null;
            try
            {
                int[] Handle = gridViewData.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }

                //如果此模型绑定了应急联动，则不能删除！
                int rowHandle = gridViewData.FocusedRowHandle;
                daID = gridViewData.GetRowCellValue(rowHandle, "Id").ToString();
                var sysEmergencyInfo = sysEmergencyLinkageService.GetAllSysEmergencyLinkageList().Data;
                if (sysEmergencyInfo.FirstOrDefault(o => o.MasterModelId == daID) != null)
                {
                    XtraMessageBox.Show("此模型已绑定应急联动配置，不能删除！", "消息");
                    return;
                }


                if (XtraMessageBox.Show("确定删除选择的分析模型吗?", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    daName = gridViewData.GetRowCellValue(rowHandle, "Name").ToString();

                    string reError = largedataAnalysisConfigBusiness.DeleteLargedataAnalysisConfigById(daID);
                    if (reError == "100")
                    {
                        XtraMessageBox.Show("删除成功", "消息");
                        OperateLogHelper.InsertOperateLog(16, "大数据分析模型-删除【" + daName + "】,", "大数据分析模型-删除");

                        btnQuery_ItemClick(sender, e);
                    }
                    else
                    {
                        XtraMessageBox.Show(reError, "消息");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "删除模型失败");
            }
        }
        /// <summary>
        /// 查询
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

        private void gridViewData_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.ColumnEditName == "repositoryItemButtonEdit1")
            {
                try
                {
                    //查看
                    int rowHandle = e.RowHandle;
                    string daID = gridViewData.GetRowCellValue(rowHandle, "Id").ToString(); //是ookUpEdit.Properties.ValueMember的值
                    LargedataAnalysisDetail largedataAnalysisDetail = new LargedataAnalysisDetail(daID);
                    largedataAnalysisDetail.Show();
                }
                catch (Exception ex)
                {
                    LogHelper.Info(string.Format("查看模型表达式出错, 错误消息:{0}", ex.Message));
                    XtraMessageBox.Show("查看模型表达式出错, 错误消息:\n" + ex.Message, "查看模型表达式出错", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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

        private void barEditItemPage_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!RegexInteger(this.barEditItemPage.EditValue.ToString()))
                {
                    XtraMessageBox.Show("请输入正整数");
                    this.barEditItemPage.EditValue = dataPageIndex;
                }
                else
                {
                    dataPageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
                }
            }
            catch
            {
                dataPageIndex = TypeUtil.ToInt(barEditItemPage.EditValue);
            }
        }
        private void QueryGridData(int pageIndex, int pageSize)
        {
            try
            {
                SetExecuteBeginTime();
                LargedataAnalysisConfigBusinessModel model = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigListByName(textEditModelName.Text.Trim(), pageIndex, pageSize);
                JC_LargedataAnalysisConfigInfoList.Clear();
                foreach (var item in model.LargedataAnalysisConfigInfoList.OrderBy(t => t.Name).ToList())
                {
                    JC_LargedataAnalysisConfigInfoList.Add(item);
                }
                gridViewData.RefreshData();
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
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        /// <summary>
        /// 分析模型传感器等级配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] Handle = gridViewData.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }

                int rowHandle = gridViewData.FocusedRowHandle;
                string daID = gridViewData.GetRowCellValue(rowHandle, "Id").ToString();
                string daName = gridViewData.GetRowCellValue(rowHandle, "Name").ToString();
                DeviceAlarmLevelSetting alarmlevelform = new DeviceAlarmLevelSetting(daID, daName);
                alarmlevelform.ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.Info("配置传感器报警等级失败！" + ex.Message);
            }
        }
    }
}
