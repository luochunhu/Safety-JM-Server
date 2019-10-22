using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraCharts;

namespace Sys.Safety.Client.Chart
{
    public class ChartPrint
    {
        /// <summary>
        /// 打印曲线
        /// </summary>
        /// <param name="chart"></param>
        public static void chartPrint(ChartControl chart, float scalefactor)
        {
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
            compositeLink.PrintingSystem = ps;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A4; //设置纸张 


            DevExpress.XtraPrinting.PrintableComponentLink link = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
            ps.Links.Add(link);
            link.Component = chart;

            link.CreateDocument();  //建立文档  
            ps.PageSettings.Landscape = true;  //设置为纵向    

            ps.PageSettings.Landscape = true;  //设置为横向

            //设置微缩比例
            ps.Document.ScaleFactor = 1000 / scalefactor;
            ps.PageSettings.LeftMargin = 5;
            ps.PageSettings.RightMargin = 5;
            ps.PageSettings.TopMargin = 5;
            ps.PageSettings.BottomMargin = 5;

            ps.PreviewFormEx.Show();//进行预览
        }
    }
}
