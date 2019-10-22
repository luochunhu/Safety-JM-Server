using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Client.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Common;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class CurvilinearQuery : XtraForm
    {
        LargedataAnalysisLogBusiness largedataAnalysisLogBusiness;
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        public CurvilinearQuery()
        {
            InitializeComponent();


        }
        public void InitializeControls()
        {
            largedataAnalysisLogBusiness = new LargedataAnalysisLogBusiness();
            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
            gridLookUpEditName.Properties.ImmediatePopup = true;
            gridLookUpEditName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;//允许输入
            gridLookUpEditName.Properties.NullText = "请输入模型名称";//清空默认值
            gridLookUpEditName.Properties.DataSource = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigListForCurve(DateTime.Now.ToString("yyyy-MM-dd"));

            dateEditStart.EditValue = DateTime.Now;
        }

        private void gridLookUpEditName_EditValueChanged(object sender, System.EventArgs e)
        {
            this.chartControl1.SeriesSerializable[0].Name = gridLookUpEditName.Text;
        }

        //数据源
        List<JC_LargedataAnalysisLogInfo> analysisLogList;
        void btnQuery_Click(object sender, EventArgs e)
        {

            string analysisModelId = string.Empty, startTime = string.Empty;
            if (gridLookUpEditName.EditValue != null)
                analysisModelId = gridLookUpEditName.EditValue.ToString();
            if (dateEditStart.EditValue != null)
                startTime = dateEditStart.EditValue.ToString();

            if (string.IsNullOrEmpty(startTime))
            {
                XtraMessageBox.Show("请选择分析日期.", "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(analysisModelId))
            {
                XtraMessageBox.Show("请选择分析模型.", "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            analysisLogList = largedataAnalysisLogBusiness.AnalysisLogQueryByModelIdAndTime(analysisModelId, startTime);
            if (analysisLogList == null)
                analysisLogList = new List<JC_LargedataAnalysisLogInfo>();
            if (analysisLogList.Count > 0)
            {
                analysisLogList = analysisLogList.OrderBy(q => q.AnalysisTime).ToList();
                JC_LargedataAnalysisLogInfo analysisLogInfo = ObjectConverter.Copy<JC_LargedataAnalysisLogInfo, JC_LargedataAnalysisLogInfo>(analysisLogList.LastOrDefault());
                if (DateTime.Parse(startTime) > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0))
                    analysisLogInfo.AnalysisTime = DateTime.Now;
                else
                    analysisLogInfo.AnalysisTime = new DateTime(analysisLogInfo.AnalysisTime.Year, analysisLogInfo.AnalysisTime.Month, analysisLogInfo.AnalysisTime.Day, 23, 59, 59);
                analysisLogList.Add(analysisLogInfo);
            }

            if (this.chartControl1.SeriesSerializable[0] != null)
            {
                this.chartControl1.SeriesSerializable[0].Points.BeginUpdate();
                this.chartControl1.SeriesSerializable[0].Points.Clear();
                //重新定义数据             
                for (var i = 0; i < analysisLogList.Count; i++)
                {
                    var rate = double.Parse(analysisLogList[i].AnalysisResult.ToString(), CultureInfo.InvariantCulture);
                    if (rate == -1)
                    {
                        rate = 0.00001;
                    }
                    this.chartControl1.SeriesSerializable[0].Points.Add(new SeriesPoint(analysisLogList[i].AnalysisTime, rate));
                }
                this.chartControl1.SeriesSerializable[0].Points.EndUpdate();
            }
            var analysisDate = DateTime.Parse(startTime);
            var _minX = new DateTime(analysisDate.Year, analysisDate.Month, analysisDate.Day, 0, 0, 0);
            var _maxX = new DateTime(analysisDate.Year, analysisDate.Month, analysisDate.Day, 23, 59, 59);

            (chartControl1.Diagram as XYDiagram).AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
            (chartControl1.Diagram as XYDiagram).AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);

            /*
            int count = 0;
            chartControl1.AnnotationRepository.Clear();
            foreach (SeriesPoint point in this.chartControl1.SeriesSerializable[0].Points)
            {
                SeriesPointAnchorPoint anchorPoint = new SeriesPointAnchorPoint();
                anchorPoint.SeriesPoint = point;

                TextAnnotation txtAnnotation = new TextAnnotation();
                txtAnnotation.RuntimeRotation = true;
                txtAnnotation.RuntimeResizing = true;
                txtAnnotation.RuntimeMoving = true;
                txtAnnotation.RuntimeAnchoring = true;

                txtAnnotation.Text = analysisLogList[count++].StatusDescription;
                txtAnnotation.ShapePosition = new RelativePosition(319.97, -48);
                txtAnnotation.AnchorPoint = anchorPoint;

                chartControl1.AnnotationRepository.Add(txtAnnotation);
            }**/
        }

        private void CurvilinearQuery_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GetIsDesignMode())
                {
                    InitializeControls();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("判断是否处于设计模式出错: Exception:{0}", ex.Message));
            }
        }

        private void chartControl1_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            try
            {
                var ShowText = "";
                var SelTime = new DateTime();
                var SelTimeNow = new DateTime();
                var index = 0;
                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    SelTime = DateTime.Parse(point.ArgumentSerializable);

                    var analysis = analysisLogList.FirstOrDefault(q => q.AnalysisTime.ToString("yyyy-MM-dd HH:mm:ss").Equals(SelTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    foreach (var element1 in e.CrosshairAxisLabelElements)
                        SelTimeNow = DateTime.Parse(element1.AxisValue.ToString());
                    if (index == 0)
                        element.LabelElement.HeaderText = SelTimeNow.ToString();
                    ShowText = string.Format("{0}:{1}", element.Series.Name, analysis == null ? "" : analysis.StatusDescription);
                    element.LabelElement.Text = ShowText;
                    index++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("CurvilinearQuery>>chartControl1_CustomDrawCrosshair。 Message: " + ex.Message + ex.StackTrace);
            }
        }

        private void dateEditStart_EditValueChanged(object sender, System.EventArgs e)
        {
            gridLookUpEditName.Properties.DataSource = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigListForCurve(this.dateEditStart.EditValue.ToString());
            this.chartControl1.SeriesSerializable[0].Points.Clear();
            this.chartControl1.Refresh();

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
    }
}
