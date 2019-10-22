using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Display
{
    public partial class frmGetHistory : DevExpress.XtraEditors.XtraForm
    {
        IPointDefineService jc_DefService = ServiceFactory.Create<IPointDefineService>();

        public frmGetHistory()
        {
            InitializeComponent();
        }

        private void tlbGet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (DialogResult.No == MessageBox.Show("获取历史数据将会覆盖该时间内数据,确认要继续吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            StartProgress(tlbGet);
            this.tlbGet.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync();
        }
        private void StartProgress(BarButtonItem menu)
        {
            this.progressBar1.Enabled = true;
            this.progressBar1.Visible = true;
            this.lblTitle.Visible = true;
            menu.Enabled = false;
        }

        private void EndProgress()
        {
            this.progressBar1.Enabled = false;
            this.progressBar1.Visible = false;
            this.lblTitle.Text = "获取历史数据成功,正在进行入库操作，请稍候查询数据。";
            this.tlbGet.Enabled = true;

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //TODO:和其它业务模块相关联
                //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
                //if (TypeConvert.ToDateTime(datEnd.EditValue).Date > DateTime.Now.Date)
                //{
                //    MessageBox.Show("不能同步今天及今天以后的历史数据！", "提示");
                //    return;
                //}
                //DEFService.GetHistoryData(this.datStart.DateTime.Date.ToString(), this.datEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"));
                //while (true)
                //{
                //    bool blnGetHistorySucuss = DEFService.BlnGetHistorySucuss();
                //    if (blnGetHistorySucuss)
                //    {
                //        break;
                //    }
                //    Thread.Sleep(1000 * 60);
                //}

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("操作失败,原因为" + ex.Message, "提示");
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.EndProgress();
        }

        private void frmGetHistory_Load(object sender, EventArgs e)
        {
            this.datStart.EditValue = DateTime.Now.AddDays(-2);
            this.datEnd.EditValue = DateTime.Now.AddDays(-1);
        }

        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmGetHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker1.IsBusy)
            {
                DialogResult result = MessageBox.Show("正在获取网关历史数据，如果关闭窗口，可能会导致获取数据失败", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}