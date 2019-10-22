using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraBars.Ribbon;

namespace Sys.Safety.Client.Display
{
    public partial class frmRunLog : XtraForm
    {
        public DataTable _dataSource;
        public DataTable alldata;
        private static object _objLock = new object();
        private const int MAX_Record = 600;
        public long freshtime = 0;

        public string ppoint = "";

        private Thread freshthread;
        private bool _isRun = false;
        public frmRunLog(string ll)
        {
            ppoint = ll;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _dataSource = new DataTable();
            _dataSource.Columns.Add("time", typeof(DateTime));
            _dataSource.Columns.Add("point", typeof(string));
            _dataSource.Columns.Add("wz", typeof(string));
            _dataSource.Columns.Add("state", typeof(string));
            _dataSource.Columns.Add("sbstate", typeof(string));
            _dataSource.Columns.Add("ssz", typeof(string));
            _dataSource.Columns.Add("uid", typeof(long));
            alldata = _dataSource.Clone();
        }

        /// <summary>
        /// 当数据总数大于600时 清除 10分钟前的数据 保留500条
        /// </summary>
        private void deletedata(DateTime nowtime)
        {
            try
            {
                //DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                if (alldata != null && alldata.Rows.Count > MAX_Record)
                {
                    lock (_objLock)
                    {
                        for (int i = 0; i <= 200; i++)
                        {
                            if (alldata.Rows.Count > 500)
                            {
                                if ((nowtime - (DateTime.Parse(alldata.Rows[alldata.Rows.Count - 1]["time"].ToString()))).TotalMinutes >= 10)
                                {
                                    alldata.Rows.RemoveAt(alldata.Rows.Count - 1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private void adddate(DataTable dt)
        {
            DataRow row;
            //DataTable dt;
            try
            {
                //dt = Model.RealInterfaceFuction.GetRunLogs(freshtime);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = dt.Rows.Count - 1; i > -1; i--)
                    {
                        row = alldata.NewRow();
                        row.ItemArray = dt.Rows[i].ItemArray;
                        alldata.Rows.InsertAt(row, 0);
                    }
                }
                if (alldata.Rows.Count > 0)
                {
                    freshtime = (long)alldata.Compute("max(uid)", "");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void frmRunLog_Load(object sender, EventArgs e)
        {
            this.txtQueryLog.EditValue = ppoint;
            try
            {
                DataTable dt = Model.RealInterfaceFuction.GetRunLogs(freshtime);
                DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                RefreshClass(nowtime, dt);

                _isRun = true;
                freshthread = new Thread(new ThreadStart(fthread));
                freshthread.IsBackground = true;
                freshthread.Start();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            
        }

        private void btnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string value = "";
            int x = -1, y = -1, count = 0, toprowindex = 0, count2 = 0;
            try
            {
                if (gvLogInfo.FocusedColumn != null)
                {
                    x = gvLogInfo.FocusedColumn.ColumnHandle;
                    y = gvLogInfo.FocusedRowHandle;
                }
                count = gvLogInfo.RowCount;
                toprowindex = gvLogInfo.TopRowIndex;
                if (this.txtQueryLog.EditValue == null)
                {
                    gcLogInfo.DataSource = alldata;
                    count2 = alldata.Rows.Count;
                }
                else
                {
                    value = txtQueryLog.EditValue.ToString();

                    DataRow[] rows = alldata.Select("point like '%" + value + "%' or wz like '%" + value + "%'", "time");
                    if (rows.Length > 0)
                    {
                        DataTable dt = rows.CopyToDataTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _dataSource = dt;
                            count2 = dt.Rows.Count;
                        }
                        else
                        {
                            _dataSource.Rows.Clear();
                        }
                    }
                    else
                    {
                        _dataSource.Rows.Clear();
                    }
                    gcLogInfo.DataSource = _dataSource;
                    count2 = _dataSource.Rows.Count;
                }
                if (count2 == count)
                {
                    gvLogInfo.FocusedColumn.ColumnHandle = x;
                    gvLogInfo.FocusedRowHandle = y;
                    if (x > -1 && y > -1)
                    {
                        gvLogInfo.FocusedColumn.ColumnHandle = x;
                        gvLogInfo.FocusedRowHandle = y;
                    }
                    gvLogInfo.TopRowIndex = toprowindex;
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Enabled = false;
        //    try
        //    {
        //        adddate();
        //        deletedata();
        //        btnQuery_ItemClick(null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Basic.Framework.Logging.LogHelper.Error(ex);
        //    }
        //    timer1.Enabled = true;
        //}
        private void RefreshClass(DateTime nowtime, DataTable dt)
        {
            adddate(dt);
            deletedata(nowtime);
            btnQuery_ItemClick(null, null);
        }
        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    DataTable dt = Model.RealInterfaceFuction.GetRunLogs(freshtime);
                    DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                    MethodInvoker In = new MethodInvoker(() => RefreshClass(nowtime, dt));
                    this.BeginInvoke(In);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(2000);
            }
        }

        private void gvLogInfo_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void frmRunLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }
    }
}
