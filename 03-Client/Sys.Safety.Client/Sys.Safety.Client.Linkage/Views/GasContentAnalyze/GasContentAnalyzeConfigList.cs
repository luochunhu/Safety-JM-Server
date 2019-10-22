using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.Client.Linkage.Handlers;

namespace Sys.Safety.Client.Linkage.Views.GasContentAnalyze
{
    public partial class GasContentAnalyzeConfigList : DevExpress.XtraEditors.XtraForm
    {
        private Action _action;

        /// <summary>运行标记
        /// 
        /// </summary>
        private static bool _isRun;

        /// <summary>最后一次运行时间
        /// 
        /// </summary>
        private static DateTime _lastRunTime;

        private Thread _handleThread;

        public GasContentAnalyzeConfigList()
        {
            InitializeComponent();
        }

        private void GasContentAnalyzeConfigList_Load(object sender, EventArgs e)
        {
            try
            {
                _action = RefreshGrid;

                RefreshGrid();

                _isRun = true;
                if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
                {
                    _handleThread = new Thread(HandleThreadFun);
                    _handleThread.Start();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshGrid()
        {
            var configs = GasContentAnalyzeRequest.GetAllGasContentAnalyzeConfig();
            gridControl.DataSource = configs;
        }

        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var form = new GasContentAnalyzeConfigEdit();
                var res = form.ShowDialog();
                if (res == DialogResult.Yes)
                {
                    RefreshGrid();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var selRow = gridView.GetFocusedRow();
                if (selRow == null)
                {
                    XtraMessageBox.Show("请选择操作项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = XtraMessageBox.Show("确认要删除该项吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    return;
                }

                var selRowData = selRow as GascontentanalyzeconfigInfo;
                var id = selRowData.Id;
                GasContentAnalyzeRequest.DeleteGasContentAnalyzeConfig(id);
                RefreshGrid();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HandleThreadFun()
        {
            while (_isRun)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    if ((dtNow - _lastRunTime).TotalSeconds >= 5)
                    {
                        Invoke(_action);
                        _lastRunTime = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

                Thread.Sleep(1000);
            }
            _isRun = true;
        }

        private void GasContentAnalyzeConfigList_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRun = false;
            while (true)
            {
                if (_isRun) break;
                Thread.Sleep(1000);
            }
        }
    }
}