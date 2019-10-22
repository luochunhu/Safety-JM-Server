using DevExpress.XtraEditors;
using Basic.Framework.JobSchedule;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class AnalysisResultsInRealtime : XtraForm
    {
        public AnalysisResultsInRealtime()
        {
            InitializeComponent();
            StaticClass.SystemOut = false;
        }
        List<JC_LargedataAnalysisConfigInfo> gridList = new List<JC_LargedataAnalysisConfigInfo>();

        private void AnalysisResultsInRealtime_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GetIsDesignMode())
                {
                    InitializeControls();
                    gridControlData.DataSource = gridList;

                    gridView1.RefreshData();
                    Thread refreshThread = new Thread(new ThreadStart(RefreshData));
                    refreshThread.IsBackground = true;
                    refreshThread.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService;

        private void InitializeControls()
        {
            largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();
        }

        private void RefreshData()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response = largeDataAnalysisCacheClientService.GetAllLargeDataAnalysisConfigCache(new Sys.Safety.Request.LargeDataAnalysisCacheClientGetAllRequest());
                    if (response.Data != null)
                    {
                        //var dataSource = from q in response.Data.AsQueryable()
                        //                 select new
                        //                 {
                        //                     Name = q.Name,
                        //                     AnalysisTime = q.AnalysisTime.HasValue ? q.AnalysisTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty,
                        //                     AnalysisResultText = q.AnalysisResultText,
                        //                     ExpressionRealTimeResultList = (
                        //                     from rt in q.ExpressionRealTimeResultList 
                        //                     select new { AnalysisModelName = rt.AnalysisModelName,
                        //                         ExpressionText = rt.ExpressionText,
                        //                         FirstSuccessfulTime = rt.FirstSuccessfulTime == DateTime.MinValue ? string.Empty : rt.FirstSuccessfulTime.ToString("yyyy/MM/dd HH:mm:ss"), 
                        //                         LastAnalysisTime = rt.LastAnalysisTime == DateTime.MinValue ? string.Empty : rt.LastAnalysisTime.ToString("yyyy/MM/dd HH:mm:ss"), 
                        //                         AnalysisResultText = rt.AnalysisResultText,
                        //                         ActualContinueTime = string.Format("{0}:{1}:{2}", (rt.ActualContinueTime / 3600).ToString().PadLeft(2, '0'), 
                        //                         (rt.ActualContinueTime % 3600 / 60).ToString().PadLeft(2, '0'), (rt.ActualContinueTime % 3600 % 60).ToString().PadLeft(2, '0')) }).ToList()
                        //                 };

                        //gridList.Clear();
                        foreach (var item in response.Data)
                        {
                            bool isHaveModel = false;
                            foreach (var itemGrid in gridList)
                            {//old列表
                                if (itemGrid.Id == item.Id)
                                {//分析模型已存在

                                    isHaveModel = true;
                                    itemGrid.AnalysisTime = item.AnalysisTime;
                                    itemGrid.AnalysisResultText = item.AnalysisResultText;
                                    itemGrid.Name = item.Name;

                                    #region 分析列表
                                    if (item.ExpressionRealTimeResultList != null)
                                    {//old列表
                                        if (itemGrid.ExpressionRealTimeResultList != null && itemGrid.ExpressionRealTimeResultList.Count > 0)
                                        {//追加
                                            foreach (var itemExpression in item.ExpressionRealTimeResultList)
                                            {//新分析数据
                                                bool isAdd = true;
                                                foreach (var itemResult in itemGrid.ExpressionRealTimeResultList)
                                                {//old列表
                                                    if (itemResult.ExpressionId == itemExpression.ExpressionId)
                                                    {
                                                        isAdd = false;
                                                        itemResult.ShowFirstSuccessfulTime = itemExpression.FirstSuccessfulTime == DateTime.MinValue ? string.Empty : itemExpression.FirstSuccessfulTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                        itemResult.ShowLastAnalysisTime = itemExpression.LastAnalysisTime == DateTime.MinValue ? string.Empty : itemExpression.LastAnalysisTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                        itemResult.AnalysisResultText = itemExpression.AnalysisResultText;

                                                        itemResult.ShowActualContinueTime = string.Format("{0}:{1}:{2}", (itemExpression.ActualContinueTime / 3600).ToString().PadLeft(2, '0'),
                                                               (itemExpression.ActualContinueTime % 3600 / 60).ToString().PadLeft(2, '0'), (itemExpression.ActualContinueTime % 3600 % 60).ToString().PadLeft(2, '0'));

                                                        break;
                                                    }
                                                }

                                                if (isAdd)
                                                {
                                                    ExpressionRealTimeResultInfo modelInfo = new ExpressionRealTimeResultInfo();
                                                    modelInfo.ExpressionText = itemExpression.ExpressionText;
                                                    modelInfo.ExpressionId = itemExpression.ExpressionId;
                                                    modelInfo.ShowFirstSuccessfulTime = itemExpression.FirstSuccessfulTime == DateTime.MinValue ? string.Empty : itemExpression.FirstSuccessfulTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                    modelInfo.ShowLastAnalysisTime = itemExpression.LastAnalysisTime == DateTime.MinValue ? string.Empty : itemExpression.LastAnalysisTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                    modelInfo.AnalysisResultText = itemExpression.AnalysisResultText;

                                                    modelInfo.ShowActualContinueTime = string.Format("{0}:{1}:{2}", (itemExpression.ActualContinueTime / 3600).ToString().PadLeft(2, '0'),
                                                           (itemExpression.ActualContinueTime % 3600 / 60).ToString().PadLeft(2, '0'), (itemExpression.ActualContinueTime % 3600 % 60).ToString().PadLeft(2, '0'));

                                                    itemGrid.ExpressionRealTimeResultList.Add(modelInfo);
                                                }
                                            }
                                        }
                                        else
                                        {//新增
                                            List<ExpressionRealTimeResultInfo> expressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();

                                            foreach (var itemExpression in item.ExpressionRealTimeResultList)
                                            {
                                                ExpressionRealTimeResultInfo modelInfo = new ExpressionRealTimeResultInfo();
                                                modelInfo.ExpressionText = itemExpression.ExpressionText;
                                                modelInfo.ExpressionId = itemExpression.ExpressionId;
                                                modelInfo.ShowFirstSuccessfulTime = itemExpression.FirstSuccessfulTime == DateTime.MinValue ? string.Empty : itemExpression.FirstSuccessfulTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                modelInfo.ShowLastAnalysisTime = itemExpression.LastAnalysisTime == DateTime.MinValue ? string.Empty : itemExpression.LastAnalysisTime.ToString("yyyy/MM/dd HH:mm:ss");
                                                modelInfo.AnalysisResultText = itemExpression.AnalysisResultText;

                                                modelInfo.ShowActualContinueTime = string.Format("{0}:{1}:{2}", (itemExpression.ActualContinueTime / 3600).ToString().PadLeft(2, '0'),
                                                       (itemExpression.ActualContinueTime % 3600 / 60).ToString().PadLeft(2, '0'), (itemExpression.ActualContinueTime % 3600 % 60).ToString().PadLeft(2, '0'));

                                                expressionRealTimeResultList.Add(modelInfo);
                                            }
                                            itemGrid.ExpressionRealTimeResultList = expressionRealTimeResultList;
                                        }
                                    }

                                    #endregion
                                }
                            }

                            if (isHaveModel == false)
                            {
                                JC_LargedataAnalysisConfigInfo model = new JC_LargedataAnalysisConfigInfo();
                                model.Id = item.Id;
                                model.Name = item.Name;
                                model.AnalysisTime = item.AnalysisTime;
                                model.AnalysisResultText = item.AnalysisResultText;

                                List<ExpressionRealTimeResultInfo> expressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();
                                if (item.ExpressionRealTimeResultList != null)
                                {

                                    foreach (var itemExpression in item.ExpressionRealTimeResultList)
                                    {
                                        ExpressionRealTimeResultInfo modelInfo = new ExpressionRealTimeResultInfo();
                                        modelInfo.ExpressionText = itemExpression.ExpressionText;
                                        modelInfo.ExpressionId = itemExpression.ExpressionId;
                                        modelInfo.ShowFirstSuccessfulTime = itemExpression.FirstSuccessfulTime == DateTime.MinValue ? string.Empty : itemExpression.FirstSuccessfulTime.ToString("yyyy/MM/dd HH:mm:ss");
                                        modelInfo.ShowLastAnalysisTime = itemExpression.LastAnalysisTime == DateTime.MinValue ? string.Empty : itemExpression.LastAnalysisTime.ToString("yyyy/MM/dd HH:mm:ss");
                                        modelInfo.AnalysisResultText = itemExpression.AnalysisResultText;

                                        modelInfo.ShowActualContinueTime = string.Format("{0}:{1}:{2}", (itemExpression.ActualContinueTime / 3600).ToString().PadLeft(2, '0'),
                                               (itemExpression.ActualContinueTime % 3600 / 60).ToString().PadLeft(2, '0'), (itemExpression.ActualContinueTime % 3600 % 60).ToString().PadLeft(2, '0'));

                                        expressionRealTimeResultList.Add(modelInfo);
                                    }
                                }
                                model.ExpressionRealTimeResultList = expressionRealTimeResultList;
                                gridList.Add(model);
                            }

                        }
                        MethodInvoker In = new MethodInvoker(() =>
                        {
                            if (gridList != null)
                            {
                                int topRowIndex = gridView1.TopRowIndex;
                                gridView1.RefreshData();
                                for (int i = 0; i < gridView1.RowCount; i++)
                                {
                                    //默认展开所有子项
                                    //gridView1.ExpandMasterRow(i);
                                    DevExpress.XtraGrid.Views.Base.BaseView childView = gridView1.GetVisibleDetailView(i);
                                    if (childView != null)
                                        childView.RefreshData();
                                }
                                gridView1.TopRowIndex = topRowIndex;
                            }
                        });
                        if (gridControlData.InvokeRequired)
                            gridControlData.Invoke(In);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message);
                }
                Thread.Sleep(4000);
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

        private void AnalysisResultsInRealtime_FormClosing(object sender, FormClosingEventArgs e)
        {
            StaticClass.SystemOut = true;
        }
        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }
    }
}
