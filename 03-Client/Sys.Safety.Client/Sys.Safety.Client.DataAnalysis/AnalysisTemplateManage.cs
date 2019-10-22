using DevExpress.XtraEditors;
using Basic.Framework.Logging;
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Web;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class AnalysisTemplateManage : XtraForm
    {
        DateTime executeBeginTime = DateTime.Now;
        AnalysisTemplateConfigBusiness analysisTemplateConfigBusiness;//= new AnalysisTemplateConfigBusiness();
        AnalysisTemplateBusinessModel analysisTemplateBusinessModel;//= new AnalysisTemplateBusinessModel();
        /// <summary>
        /// 逻辑分析模板列表
        /// </summary>
        public List<JC_AnalysisTemplateInfo> AnalysisTemplateInfoList = new List<JC_AnalysisTemplateInfo>();
        public AnalysisTemplateManage()
        {
            InitializeComponent();


        }

        public void LoadForm()
        {
            try
            {
                analysisTemplateConfigBusiness = new AnalysisTemplateConfigBusiness();
                analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();
                gridControlModule.DataSource = AnalysisTemplateInfoList;
                //初始化列表数据
                QueryGridData(1, 50);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "查询模板失败");
            }

        }
        int dataPageIndex = 1;
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

        private void AnalysisTemplateManage_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GetIsDesignMode())
                {
                    DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开分析模板列表窗体...", "请等待...");
                    LoadForm();
                    wdf.Close();
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
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddAnalysisTemplate addAnalysisTemplate = new AddAnalysisTemplate();

            addAnalysisTemplate.ShowDialog();
            if (addAnalysisTemplate.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (addAnalysisTemplate.dataType == "add")
                {
                    if (addAnalysisTemplate.returnModel != null)
                    {
                        AnalysisTemplateInfoList.Insert(0, addAnalysisTemplate.returnModel);//获取弹出窗体的属性值
                        gridControlDetail.DataSource = analysisTemplateConfigBusiness.GetAnalyticalExpressionInfoListByTempleteId(addAnalysisTemplate.returnModel.Id).AnalysisExpressionInfoList;
                        for (int i = 0; i < AnalysisTemplateInfoList.Count; i++)
                        {
                            gridViewModele.UnselectRow(i);
                        }
                        gridViewModele.SelectRow(0);
                    }
                }
            }
            gridViewModele.RefreshData();
            //btnQuery_ItemClick(sender, e);
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
                List<JC_AnalysisTemplateInfo> model = gridControlModule.DataSource as List<JC_AnalysisTemplateInfo>;
                if (model == null || model.Count == 0)
                {
                    XtraMessageBox.Show("请选择模板", "消息");
                    return;
                }

                int[] Handle = gridViewModele.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模板", "消息");
                    return;
                }

                int rowHandle = gridViewModele.FocusedRowHandle;


                if (gridViewModele.GetRowCellValue(rowHandle, "Id") == null)
                {
                    XtraMessageBox.Show("请选择模板", "消息");
                    return;
                }

                string daID = gridViewModele.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值

                if (analysisTemplateConfigBusiness.checkTemplateUsed(daID))
                {
                    XtraMessageBox.Show("此模板已被使用,不能对模板进行修改操作。", "消息");
                    return;
                }

                AddAnalysisTemplate addAnalysisTemplate = new AddAnalysisTemplate(daID);
                addAnalysisTemplate.ShowDialog();
                if (addAnalysisTemplate.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (addAnalysisTemplate.dataType == "edit")
                    {
                        if (addAnalysisTemplate.returnModel != null)
                        {
                            if (AnalysisTemplateInfoList.Count > 0)
                            {
                                bool isUpdate = false;
                                foreach (var item in AnalysisTemplateInfoList)
                                {
                                    if (item.Id == addAnalysisTemplate.returnModel.Id)
                                    {
                                        isUpdate = true;
                                        AnalysisTemplateInfoList.Remove(item);
                                        break;
                                    }
                                }
                                if (isUpdate)
                                {
                                    AnalysisTemplateInfoList.Insert(0, addAnalysisTemplate.returnModel);
                                    gridControlDetail.DataSource = analysisTemplateConfigBusiness.GetAnalyticalExpressionInfoListByTempleteId(daID).AnalysisExpressionInfoList;

                                    for (int i = 0; i < AnalysisTemplateInfoList.Count; i++)
                                    {
                                        gridViewModele.UnselectRow(i);
                                    }
                                    gridViewModele.SelectRow(0);
                                    gridViewModele.RefreshData();
                                }
                            }

                        }
                    }
                }

                //btnQuery_ItemClick(sender, e);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show(ex.Message, "修改模板失败");
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
                List<JC_AnalysisTemplateInfo> model = gridControlModule.DataSource as List<JC_AnalysisTemplateInfo>;
                if (model == null || model.Count == 0)
                {
                    XtraMessageBox.Show("无数据", "消息");
                    return;
                }
                int[] Handle = gridViewModele.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }
                int rowHandle = gridViewModele.FocusedRowHandle;
                if (gridViewModele.GetRowCellValue(rowHandle, "Id") == null)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }

                List<delAnalysisTemplateModel> ids = new List<delAnalysisTemplateModel>();
                StringBuilder sbNames = new StringBuilder();
                foreach (var item in Handle)
                {
                    delAnalysisTemplateModel delAnalysisTemplateModel = new delAnalysisTemplateModel();
                    delAnalysisTemplateModel.id = gridViewModele.GetRowCellValue(item, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值
                    delAnalysisTemplateModel.name = gridViewModele.GetRowCellValue(item, "Name").ToString();　//是ookUpEdit.Properties.ValueMember的值
                    ids.Add(delAnalysisTemplateModel);
                    if (string.IsNullOrEmpty(sbNames.ToString()))
                    {
                        sbNames.Append(delAnalysisTemplateModel.name);
                    }
                    else
                    {
                        sbNames.Append(",");
                        sbNames.Append(delAnalysisTemplateModel.name);
                    }
                }


                if (XtraMessageBox.Show("确定删除选择的模板吗?", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string reError = analysisTemplateConfigBusiness.DeleteAnalysisTemplateConfig(ids);

                    if (reError == "100")
                    {
                        XtraMessageBox.Show("删除成功", "消息");
                        OperateLogHelper.InsertOperateLog(16, "大数据分析模版-删除【" + sbNames.ToString() + "】,", "大数据分析模版-删除");
                        foreach (var itemID in ids)
                        {
                            foreach (var item in AnalysisTemplateInfoList)
                            {
                                if (item.Id == itemID.id)
                                {
                                    AnalysisTemplateInfoList.Remove(item);
                                    break;
                                }
                            }
                        }
                        gridViewModele.RefreshData();
                        //btnQuery_ItemClick(sender, e);
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
                XtraMessageBox.Show(ex.Message, "修改模板失败");
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
        /// 显示模板表达式信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewModele_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                int rowHandle = gridViewModele.FocusedRowHandle;
                string daID = gridViewModele.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值

                gridControlDetail.DataSource = analysisTemplateConfigBusiness.GetAnalyticalExpressionInfoListByTempleteId(daID).AnalysisExpressionInfoList;

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);

            }
        }
        /// <summary>
        /// 复制模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
        {

            try
            {
                List<JC_AnalysisTemplateInfo> model = gridControlModule.DataSource as List<JC_AnalysisTemplateInfo>;
                if (model == null || model.Count == 0)
                {
                    XtraMessageBox.Show("无数据", "消息");
                    return;
                }
                int[] Handle = gridViewModele.GetSelectedRows();
                if (Handle == null || Handle.Length == 0)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }
                int rowHandle = gridViewModele.FocusedRowHandle;
                if (gridViewModele.GetRowCellValue(rowHandle, "Id") == null)
                {
                    XtraMessageBox.Show("请选择模型", "消息");
                    return;
                }
                string daID = gridViewModele.GetRowCellValue(rowHandle, "Id").ToString();　//是ookUpEdit.Properties.ValueMember的值

                List<JC_AnalyticalExpressionInfo> modelList = analysisTemplateConfigBusiness.GetAnalyticalExpressionInfoListByTempleteId(daID).AnalysisExpressionInfoList;
                List<GridData> gridDataList = new List<GridData>();

                for (int i = 0; i < modelList.Count; i++)
                {
                    GridData gridData = new GridData();
                    gridData.Id = modelList[i].Id;
                    gridData.ContinueTime = modelList[i].ContinueTime;
                    gridData.ExpresstionOperationRecord = modelList[i].ExpresstionOperationRecord;
                    gridData.ExpresstionText = modelList[i].ExpresstionText;
                    gridDataList.Add(gridData);
                }

                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, gridDataList);
                Clipboard.SetData(DataFormats.Serializable, stream);
                Clipboard.SetAudio(stream);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show("复制模板失败", "消息");
            }

        }
        /// <summary>
        /// 复制表达式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCopyExpress_Click(object sender, EventArgs e)
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

        private void QueryGridData(int pageIndex, int pageSize)
        {
            try
            {
                SetExecuteBeginTime();
                AnalysisTemplateBusinessModel model = analysisTemplateConfigBusiness.GetAnalysisTemplateListByTmplateName(textEditModelName.Text.Trim(), pageIndex, pageSize);

                AnalysisTemplateInfoList.Clear();
                foreach (var item in model.AnalysisTemplateInfoList.OrderBy(t => t.Name).ToList())
                {
                    AnalysisTemplateInfoList.Add(item);
                }
                gridViewModele.RefreshData();
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
        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewModele_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

    }
}
