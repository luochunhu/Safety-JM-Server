using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.Utils;

using System.IO;
using System.Xml;
using System.Globalization;

namespace Sys.Safety.Client.Chart
{
    public partial class Frm_SSZChart : RibbonForm
    {
        const int interval = 20;

        static RegressionLine GetRegressionLine(Series series)
        {
            if (series != null)
            {
                SwiftPlotSeriesView swiftPlotView = series.View as SwiftPlotSeriesView;
                if (swiftPlotView != null)
                    foreach (Indicator indicator in swiftPlotView.Indicators)
                    {
                        RegressionLine regressionLine = indicator as RegressionLine;
                        if (regressionLine != null)
                            return regressionLine;
                    }
            }
            return null;
        }

        Random random = new Random();
        double value1 = 10.0;
        double value2 = -10.0;
        bool? inProcess = null;

        int TimeInterval { get { return Convert.ToInt32(100); } }
        Series Series1 { get { return chart.Series.Count > 0 ? chart.Series[0] : null; } }
        Series Series2 { get { return chart.Series.Count > 1 ? chart.Series[1] : null; } }
        RegressionLine Regression1 { get { return GetRegressionLine(Series1); } }
        RegressionLine Regression2 { get { return GetRegressionLine(Series2); } }
        public  ChartControl ChartControl { get { return chart; } }

        public Frm_SSZChart()
        {
            InitializeComponent();

            Regression1.Visible = false;
            Regression2.Visible = false;
        }
        void SetPauseResumeButtonText()
        {
            btnPauseResume.Text = timer.Enabled ? "Pause" : "Resume";
        }
        double CalculateNextValue(double value)
        {
            return value + (random.NextDouble() * 10.0 - 5.0);
        }
        void UpdateValues()
        {
            value1 = CalculateNextValue(value1);
            value2 = CalculateNextValue(value2);
        }
        void DisableProcess()
        {
            inProcess = timer.Enabled;
            timer.Enabled = false;
        }
        void RestoreProcess()
        {
            if (inProcess != null)
            {
                timer.Enabled = (bool)inProcess;
                inProcess = null;
            }
        }
        protected void InitControls()
        {
            timer.Interval = interval;
            SetPauseResumeButtonText();
        }
        protected void DoShow()
        {
            RestoreProcess();
        }
        protected void DoHide()
        {
            DisableProcess();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (Series1 == null || Series2 == null)
                return;
            DateTime argument = DateTime.Now;
            SeriesPoint[] pointsToUpdate1 = new SeriesPoint[interval];
            SeriesPoint[] pointsToUpdate2 = new SeriesPoint[interval];
            for (int i = 0; i < interval; i++)
            {
                pointsToUpdate1[i] = new SeriesPoint(argument, value1);
                pointsToUpdate2[i] = new SeriesPoint(argument, value2);
                argument = argument.AddMilliseconds(1);
                UpdateValues();
            }
            DateTime minDate = argument.AddSeconds(-TimeInterval);
            int pointsToRemoveCount = 0;
            foreach (SeriesPoint point in Series1.Points)
                if (point.DateTimeArgument < minDate)
                    pointsToRemoveCount++;
            if (pointsToRemoveCount < Series1.Points.Count)
                pointsToRemoveCount--;
            AddPoints(Series1, pointsToUpdate1);
            AddPoints(Series2, pointsToUpdate2);
            if (pointsToRemoveCount > 0)
            {
                Series1.Points.RemoveRange(0, pointsToRemoveCount);
                Series2.Points.RemoveRange(0, pointsToRemoveCount);
            }
            SwiftPlotDiagram diagram = chart.Diagram as SwiftPlotDiagram;
            if (diagram != null && (diagram.AxisX.DateTimeScaleOptions.MeasureUnit == DateTimeMeasureUnit.Millisecond || diagram.AxisX.DateTimeScaleOptions.ScaleMode == ScaleMode.Continuous))
                diagram.AxisX.WholeRange.SetMinMaxValues(minDate, argument);
        }
        void AddPoints(Series series, SeriesPoint[] pointsToUpdate)
        {
            if (series.View is SwiftPlotSeriesViewBase)
                series.Points.AddRange(pointsToUpdate);
        }
        void btnPauseResume_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
            SetPauseResumeButtonText();
        }


    }
}
