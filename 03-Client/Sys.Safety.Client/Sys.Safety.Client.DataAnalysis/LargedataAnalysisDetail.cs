using Basic.Framework.Common;
using DevExpress.XtraEditors;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class LargedataAnalysisDetail : XtraForm
    {
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
        private string largedataAnalysisId = string.Empty;
        public LargedataAnalysisDetail(string largedataAnalysisId)
        {
            InitializeComponent();
            this.largedataAnalysisId = largedataAnalysisId;
        }

        private void LargedataAnalysisDetail_Load(object sender, EventArgs e)
        {
            editLoadForm();
        }
        JC_LargedataAnalysisConfigInfo editModel;
        private void editLoadForm()
        {
            editModel = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigById(largedataAnalysisId);

            StringBuilder sbText = new StringBuilder();
            //分析模型名称：选择分析模板：分析周期(秒)：满足条件输出: 不满足条件输出：
            sbText.Append("分析模型名称：");
            sbText.Append(editModel.Name);
            sbText.Append(" ");
            sbText.Append("分析周期(秒)：");
            sbText.Append(editModel.AnalysisInterval.ToString());
            sbText.Append(" ");
            sbText.Append("满足条件输出：");
            sbText.Append(editModel.TrueDescription);
            sbText.Append(" ");
            sbText.Append("不满足条件输出：");
            sbText.Append(editModel.FalseDescription);

            lblContent.Text = sbText.ToString();
            if (null != editModel.AnalysisModelPointRecordInfoList && editModel.AnalysisModelPointRecordInfoList.Count > 0)
            { 
                editModel.AnalysisModelPointRecordInfoList = editModel.AnalysisModelPointRecordInfoList.OrderBy(q => q.ExpressionId).ThenBy(q => q.ParameterName).ToList();
                setViewList(editModel.AnalysisModelPointRecordInfoList);
            }
            //gridControlData.DataSource = editModel.AnalysisModelPointRecordInfoList;
            gridControlData.DataSource = viewList;

            gridView1.Columns["ExpressionId"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gridView1.BestFitColumns();
        }


        /// <summary>
        /// 页面视图数据
        /// </summary>
        private List<JC_SetAnalysisModelPointRecordInfo> viewList = new List<JC_SetAnalysisModelPointRecordInfo>();
        private void setViewList(List<JC_SetAnalysisModelPointRecordInfo> source)
        {
            viewList.Clear();
            IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> expressionGroup = source.GroupBy(p => p.ExpressionId);
            foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expressionParameterList in expressionGroup)
            {
                IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> parameterGroup = expressionParameterList.GroupBy(p => p.ParameterId);
                foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> parameterList in parameterGroup)
                {
                    JC_SetAnalysisModelPointRecordInfo vi = parameterList.FirstOrDefault();
                    if (null != vi)
                        viewList.Add(ObjectConverter.DeepCopy(vi));
                }
            }
        }

        private void gridView1_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if (e.Column.FieldName == "ParameterName" || e.Column.FieldName == "Point")
            {
                e.Merge = false;
                e.Handled = true;
            }
            else
            {
                if (e.Column.FieldName == "ExpresstionText")
                {
                    //ExpressionId
                    DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                    string expressionId1 = view.GetRowCellValue(e.RowHandle1, view.Columns["ExpressionId"]).ToString();
                    string expressionId2 = view.GetRowCellValue(e.RowHandle2, view.Columns["ExpressionId"]).ToString();
                    e.Merge = expressionId1 == expressionId2;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = true;
                    e.Handled = false;
                }
            }
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

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            var currentView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            Rectangle r = e.Bounds;
            if (currentView != null)
            {
                if (e.Column.FieldName == "Point")
                {
                    object pointActivity = currentView.GetRowCellValue(e.RowHandle, "PointActivity");
                    if (null != pointActivity && pointActivity.ToString() == "0")
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.DisplayText = string.Format("{0}(已删除)", e.DisplayText);
                        e.Appearance.DrawString(e.Cache, e.DisplayText, r);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
