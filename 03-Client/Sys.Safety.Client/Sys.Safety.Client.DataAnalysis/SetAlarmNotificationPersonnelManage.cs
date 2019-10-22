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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetAlarmNotificationPersonnelManage : XtraForm
    {
        DateTime executeBeginTime = DateTime.Now;
        int dataPageIndex = 1;
        List<JC_AlarmNotificationPersonnelConfigInfo> JC_LargedataAnalysisConfigInfoList = new List<JC_AlarmNotificationPersonnelConfigInfo>();
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        AlarmNotificationPersonnelBusiness alarmNotificationPersonnelBusiness;
        public SetAlarmNotificationPersonnelManage()
        {
            InitializeComponent();
        }

        private void SetAlarmNotificationPersonnelManage_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void LoadForm()
        {
            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
            alarmNotificationPersonnelBusiness = new AlarmNotificationPersonnelBusiness();
            //初始化列表数据
            QueryGridData(1, 50);
        }
        /// <summary>
        /// 设置单元格报警颜色
        /// </summary>
        public void SetgridAlarmColor(List<JC_AlarmNotificationPersonnelConfigInfo> modelInfo)
        {
            if (modelInfo != null && modelInfo.Count > 0)
            {
                DevExpress.XtraGrid.StyleFormatCondition[] dataStyle = new DevExpress.XtraGrid.StyleFormatCondition[modelInfo.Count];

                for (int i = 0; i < modelInfo.Count; i++)
                {
                    DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition = new DevExpress.XtraGrid.StyleFormatCondition();
                    styleFormatCondition.Appearance.BackColor = Color.FromArgb(int.Parse(modelInfo[i].AlarmColor));
                    styleFormatCondition.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
                    styleFormatCondition.Appearance.ForeColor = Color.FromArgb(int.Parse(modelInfo[i].AlarmColor));
                    styleFormatCondition.Appearance.Options.UseBackColor = true;
                    styleFormatCondition.Appearance.Options.UseFont = true;
                    styleFormatCondition.Appearance.Options.UseForeColor = true;
                    styleFormatCondition.Column = this.gridAlarmColor;
                    styleFormatCondition.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                    styleFormatCondition.Value1 = modelInfo[i].AlarmColor;
                    dataStyle[i] = styleFormatCondition;
                }
                this.gridView.FormatConditions.AddRange(dataStyle);
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
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetAlarmNotificationPersonnel setAlasrmNotificationPersonnel = new SetAlarmNotificationPersonnel();
            setAlasrmNotificationPersonnel.ShowDialog();
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
                int[] Handle = gridView.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择需要修改的报警配置", "消息");
                    return;
                }

                int rowHandle = gridView.FocusedRowHandle;
                string daID = gridView.GetRowCellValue(rowHandle, "AnalysisModelId").ToString();　//是ookUpEdit.Properties.ValueMember的值

                SetAlarmNotificationPersonnel setAlasrmNotificationPersonnel = new SetAlarmNotificationPersonnel(daID);
                setAlasrmNotificationPersonnel.ShowDialog();
                btnQuery_ItemClick(sender, e);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "消息");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int[] Handle = gridView.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择需要删除的记录", "消息");
                    return;
                }

                if (XtraMessageBox.Show("确定删除所选模型的报警推送配置吗?", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    List<string> ids = new List<string>();
                    StringBuilder analysisNames = new StringBuilder();
                    foreach (var item in Handle)
                    {
                        ids.Add(gridView.GetRowCellValue(item, "Id").ToString().Trim());
                        analysisNames.Append(gridView.GetRowCellValue(item, "AnalysisModeName").ToString()).Append(",");
                    }
                    string reError = alarmNotificationPersonnelBusiness.DeleteJC_AlarmNotificationPersonnelConfig(ids);
                    if (reError == "100")
                    {
                        XtraMessageBox.Show("删除成功", "消息");
                        OperateLogHelper.InsertOperateLog(16, "报警推送-删除" + string.Format("内容:{0}", analysisNames.ToString().TrimEnd(',')), "报警推送-删除");
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
                XtraMessageBox.Show(ex.Message, "消息");
            }

        }
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
                AlarmNotificationPersonnelConfigBusinessModel model = alarmNotificationPersonnelBusiness.GetAlarmNotificationPersonnelListByAnalysisModeName(textEditName.Text.Trim(), pageIndex, pageSize);
                List<JC_AlarmNotificationPersonnelConfigInfo> modelList = model.AlarmNotificationPersonnelConfigInfoList;
                SetgridAlarmColor(modelList);
                foreach (var item in modelList)
                {
                    if (!string.IsNullOrWhiteSpace(item.AlarmType))
                    {
                        string[] dataStr = item.AlarmType.Split(',');
                        StringBuilder sbData = new StringBuilder();
                        foreach (var itemStr in dataStr)
                        {
                            if (!string.IsNullOrWhiteSpace(itemStr))
                            {
                                if (!string.IsNullOrWhiteSpace(sbData.ToString()))
                                {
                                    sbData.Append(",");
                                }
                                switch (itemStr.Trim())
                                {
                                    case "1":
                                        sbData.Append("语音播报");
                                        break;
                                    case "2":
                                        sbData.Append("声光报警");
                                        break;
                                    case "3":
                                        sbData.Append("图文弹窗");
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        item.AlarmType = sbData.ToString();
                    }
                }
                gridControlData.DataSource = modelList;
                gridView.FocusedRowHandle = -1;
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
        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
    }
}
