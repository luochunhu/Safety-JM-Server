using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Sys.Safety.ClientFramework.View.Message;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Define.ControlLogic
{
    public partial class CFControlLogic : XtraForm
    {
        ///// <summary>
        ///// 默认构造函数
        ///// </summary>
        public CFControlLogic()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }


        private void CFControlLogic_Load(object sender, EventArgs e)
        {

        }

        #region =========================事件函数===========================
        #region Grid CommMethod
        private void tlbExportExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".xls";
            Export(1, strFileType);
        }

        private void tlbExeclPDF_ItemClick(object sender, ItemClickEventArgs e)
        {


            string strFileType = ".pdf";
            Export(2, strFileType);
        }

        private void txtExportTXT_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".txt";
            Export(3, strFileType);
        }

        private void txtExportCSV_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".csv";
            Export(4, strFileType);
        }

        private void txtExportHTML_ItemClick(object sender, ItemClickEventArgs e)
        {
            string strFileType = ".html";
            Export(5, strFileType);
        }

        /// <summary>
        /// 导出通用方法
        /// </summary>
        /// <param name="LngTypeID">文件类型ID，用于默认选择类型</param>
        /// <param name="strFileType">文件类型名称，用于给文件名赋后缀名</param>
        private void Export(int LngTypeID, string strFileType)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls|PDF文件(*.pdf)|*.pdf|TXT文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|HTML文件(*.html)|*.html";
                saveFileDialog.Title = "保存文件";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = this.Text;
                saveFileDialog.FilterIndex = LngTypeID;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridView.OptionsPrint.PrintHeader = true;//设置后列表的列名会打印出来
                    gridView.OptionsPrint.PrintFooter = true;//设置后列表有footer（如小计）
                    //gridView.OptionsPrint.UsePrintStyles = false;//设置后为保留列表颜色样式
                    //列表方式
                    if (LngTypeID == 1)
                    {
                        gridView.ExportToXls(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 2)
                    {
                        gridView.ExportToPdf(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 3)
                    {
                        gridView.ExportToText(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 4)
                    {
                        gridView.ExportToCsv(saveFileDialog.FileName);
                    }
                    if (LngTypeID == 5)
                    {
                        gridView.ExportToHtml(saveFileDialog.FileName);
                    }
                }
                if (DialogResult.Yes == MessageBox.Show("是否立即打开此文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);

                }
            }
            catch (System.Exception ex)
            {
                DevMessageBox.Show(DevMessageBox.MessageType.Warning, ex.ToString());
            }
        }

        private void tlbPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView.OptionsPrint.PrintHeader = true;
            gridView.OptionsPrint.PrintFooter = true;

            this.gridView.ShowRibbonPrintPreview();
        }

        #endregion
        #endregion

        #region =========================业务函数===========================
        /// <summary> 加载信息
        /// </summary>
        private void LoadBasicInf()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error("加载基础信息【LoadBasicInf】", ex);
            }
        }
        /// <summary> 加载默认的初始信息
        /// </summary>
        private void LoadPretermitInf()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载默认的初始信息【LoadPretermitInf】", ex);
            }
        }
        #endregion
    }
}
