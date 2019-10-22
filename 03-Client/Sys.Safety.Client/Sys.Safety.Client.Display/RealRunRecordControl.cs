using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using System.Threading;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.Client.Display.Model;

namespace Sys.Safety.Client.Display
{
    public partial class RealRunRecordControl : RibbonForm
    {
        public RealRunRecordControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
            
        }

        #region======================运行记录======================

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;

        /// <summary>
        /// 当前最大id
        /// </summary>
        public long dqid = 0;

        /// <summary>
        /// 获取数据的最后一条的时间
        /// </summary>
        public DateTime timenow = Model.RealInterfaceFuction.GetServerNowTime();

        /// <summary>
        /// 运行记录数据源
        /// </summary>
        public Dictionary<long, Jc_RInfo> jc_r = new Dictionary<long, Jc_RInfo>();
        /// <summary>
        /// 最后获取记录的Counter
        /// </summary>
        public long LastCounter = 0;

        /// <summary>
        /// 需要删除的id号
        /// </summary>
        public List<long> deletelist = new List<long>();

        /// <summary>
        /// 需要添加的id号
        /// </summary>
        public List<long> addlist = new List<long>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","设备类型","分站/通道/地址","当前值","状态","设备状态","变化时刻",
            "id"};//"设备状态","分站/通道/地址",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","devName","fzh","ssz","state","sbstate",
            "time","id"};//"sbstate","fzh",


        public int[] colwith = new int[] { 80, 180, 100, 80, 100, 80, 80, 180, 80 };//100,

        /// <summary>
        /// 人员采集-列表显示名称
        /// </summary>
        public string[] colname1 = new string[] { "时间", "卡号", "姓名", "标志", "id" };

        /// <summary>
        /// 人员采集-表列头名称
        /// </summary>
        public string[] tcolname1 = new string[] { "rtime", "bh", "name", "flag", "id" };

        /// <summary>
        /// 人员采集-表头宽度
        /// </summary>
        public int[] colwith1 = new int[] { 150, 60, 80, 60, 60 };
        /// <summary>
        /// 人员实时采集数据源
        /// </summary>

        public DataTable showdt1;

        /// <summary>
        /// 人员定位实时采集数据源（缓存）
        /// </summary>
        public Dictionary<long, R_PhistoryInfo> R_PhistoryList = new Dictionary<long, R_PhistoryInfo>();
        /// <summary>
        /// 需要添加的id号
        /// </summary>
        public List<long> r_phistoryaddlist = new List<long>();
        /// <summary>
        /// 需要删除的id号
        /// </summary>
        public List<long> r_phistorydeletelist = new List<long>();
        /// <summary>
        /// 人员最后获取数据时间
        /// </summary>
        DateTime R_PhistoryLastGetTime = DateTime.Now;

        /// <summary>
        /// 显示showdt操作对象锁  20170705
        /// </summary>
        protected readonly object objShowDt = new object();
        /// <summary>
        /// 显示showdt操作对象锁  20170705
        /// </summary>
        protected readonly object objShowDt1 = new object();
        /// <summary>
        /// 是否显示风门运行记录
        /// </summary>
        private bool isDispalyDamper = true;

        IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();

        List<Jc_DevInfo> devAllList = new List<Jc_DevInfo>();

        List<Jc_DevInfo> listJcdev = null;

        /// <summary>
        /// 初始显示表
        /// </summary>
        public void inigrid()
        {
            GridColumn col;
            inidt();
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = tcolname[i];
                col.Width = colwith[i];
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                col.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                if (colname[i] == "id" || colname[i] == "分站/通道/地址" || colname[i] == "设备状态")
                {
                    col.Visible = false;
                }
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;

            for (int i = 0; i < colname1.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname1[i];
                col.FieldName = tcolname1[i];
                col.Width = colwith1[i];
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                col.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                if (colname1[i] == "id")
                {
                    col.Visible = false;
                }
                gridView1.Columns.Add(col);
            }
            gridControl1.DataSource = showdt1;
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        public void inidt()
        {
            DataColumn col;
            lock (objShowDt)
            {
                showdt = new DataTable();
                for (int i = 0; i < colname.Length; i++)
                {
                    col = new DataColumn(tcolname[i]);
                    showdt.Columns.Add(col);
                }

            }
            lock (objShowDt1)
            {
                //人员实时采集赋初值  20171214
                showdt1 = new DataTable();
                for (int i = 0; i < colname1.Length; i++)
                {
                    col = new DataColumn(tcolname1[i]);
                    showdt1.Columns.Add(col);
                }
            }
        }
        public void setfontsize(float n)
        {
            try
            {
                Font f = new System.Drawing.Font(mainGridView.Appearance.Row.Font.Name, n);
                mainGridView.Appearance.Row.Font = f;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 刷新运行记录数据
        /// </summary>
        public void refresh(DateTime nowtime)
        {
            string point = "";
            DataRow[] row;
            DataRow r;
            int countn = 0;
            long id = 0;
            Jc_RInfo obj = null;
            int tbcount = 0;
            object wz = "", fzh = "", sszn = "", state = "", stime = "", sbstate = "";
            long key = 0;
            #region 删除已结束的记录
            try
            {
                if (deletelist.Count > 0)
                {
                    #region 删除显示
                    countn = showdt.Rows.Count;
                    lock (objShowDt)
                    {
                        for (int i = 0; i < countn; i++)
                        {
                            try
                            {
                                if (deletelist.Contains(long.Parse(showdt.Rows[i]["id"].ToString())))
                                {
                                    showdt.Rows.Remove(showdt.Rows[i]);
                                    i--; countn--;
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                        }
                    }
                    #endregion

                    #region 删除内存
                    for (int kj = 0; kj < deletelist.Count; kj++)
                    {
                        key = deletelist[kj];
                        if (jc_r.ContainsKey(key))
                        {
                            jc_r.Remove(key);
                        }
                    }
                    deletelist.Clear();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("删除运行记录", ex);
            }
            #endregion

            #region 删除已结束的人员实时采集记录
            try
            {
                if (r_phistorydeletelist.Count > 0)
                {
                    #region 删除显示
                    countn = showdt1.Rows.Count;
                    lock (objShowDt1)
                    {
                        for (int i = 0; i < countn; i++)
                        {
                            try
                            {
                                if (r_phistorydeletelist.Contains(long.Parse(showdt1.Rows[i]["id"].ToString())))
                                {
                                    showdt1.Rows.Remove(showdt1.Rows[i]);
                                    i--; countn--;
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                        }
                    }
                    #endregion

                    #region 删除内存
                    for (int kj = 0; kj < r_phistorydeletelist.Count; kj++)
                    {
                        key = r_phistorydeletelist[kj];
                        if (R_PhistoryList.ContainsKey(key))
                        {
                            R_PhistoryList.Remove(key);
                        }
                    }
                    r_phistorydeletelist.Clear();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("删除人员实时采集记录", ex);
            }
            #endregion

            #region 添加记录

            if (addlist.Count > 0)
            {
                //tbcount = showdt.Rows.Count;
                try
                {
                    #region 添加记录
                    for (int kj = 0; kj < addlist.Count; kj++)
                    {
                        key = addlist[kj];
                        if (jc_r.ContainsKey(key))
                        {
                            obj = jc_r[key];
                            point = obj.Point;
                            lock (StaticClass.allPointDtLockObj)
                            {
                                row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (row.Length > 0)
                                {
                                    wz = row[0]["wz"];
                                }
                            }
                            fzh = obj.Fzh + "/" + obj.Kh + "/" + obj.Dzh;
                            sszn = obj.Val;
                            state = OprFuction.StateChange(obj.Type.ToString());
                            sbstate = OprFuction.StateChange(obj.State.ToString());
                            stime = OprFuction.TimeToString(obj.Timer);
                            id = key;

                            // 20180821
                            //设备类型
                            var devid = obj.Devid;
                            var dev = listJcdev.FirstOrDefault(a => a.Devid == devid);
                            string devName = "";
                            if (dev != null)
                            {
                                devName = dev.Name;
                            }
                            lock (objShowDt)
                            {
                                r = showdt.NewRow();
                                r["point"] = point;
                                r["wz"] = wz;
                                r["fzh"] = fzh;
                                r["ssz"] = sszn;
                                r["state"] = state;
                                //r["sbstate"] = sbstate;
                                r["devName"] = devName;
                                r["time"] = stime;
                                r["id"] = key;
                                showdt.Rows.InsertAt(r, 0);//添加新记录

                                this.mainGridView.FocusedRowHandle = 0;
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加开关量运行记录记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[5] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 添加人员采集记录

            R_PhistoryInfo obj1 = null;
            if (r_phistoryaddlist.Count > 0)
            {
                //tbcount = showdt.Rows.Count;
                try
                {
                    #region 添加记录
                    for (int kj = 0; kj < r_phistoryaddlist.Count; kj++)
                    {
                        key = r_phistoryaddlist[kj];
                        if (R_PhistoryList.ContainsKey(key))
                        {
                            obj1 = R_PhistoryList[key];
                            lock (objShowDt1)
                            {
                                r = showdt1.NewRow();
                                r["Id"] = R_PhistoryList[key].Id;
                                r["bh"] = R_PhistoryList[key].Bh;
                                r["name"] = R_PhistoryList[key].Name;
                                r["flag"] = R_PhistoryList[key].Flag;
                                r["rtime"] = R_PhistoryList[key].Rtime;

                                showdt1.Rows.InsertAt(r, 0);//添加新记录

                                this.gridView1.FocusedRowHandle = 0;
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加人员实时采集记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[5] += tbcount;
                r_phistoryaddlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前运行记录条数
            //tbcount = showdt.Rows.Count;
            //StaticClass.yccount[5] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增运行记录信息
        /// </summary>
        public void getdata()
        {
            Jc_RInfo obj = null;
            long key = 0;
            #region 获取运行记录数据
            try
            {
                List<Jc_RInfo> runLogList = Model.RealInterfaceFuction.GetRunRecordByCounter(LastCounter);
                for (int kj = 0; kj < runLogList.Count; kj++)
                {
                    key = long.Parse(runLogList[kj].ID);
                    if (runLogList[kj].Counter > LastCounter)
                    {//取最后获取的时间，作为下次获取数据的开始时间  20170909
                        LastCounter = runLogList[kj].Counter;
                    }
                    if (!jc_r.ContainsKey(key) && !runLogList[kj].Point.Contains("."))//修改，不显示网络模块运行记录  20180421
                    {

                        var tempDev = devAllList.Find(a => a.Devid == runLogList[kj].Devid);
                        if (tempDev != null)
                        {
                            if (tempDev.Name.Contains("风门") && !isDispalyDamper)
                            {
                                continue;//如果设置了风门不显示，则添加时，不添加风门运行记录到列表中
                            }
                        }

                        #region 新增运行记录
                        obj = new Jc_RInfo();
                        obj.ID = runLogList[kj].ID;
                        obj.PointID = runLogList[kj].PointID;
                        obj.Fzh = runLogList[kj].Fzh;
                        obj.Kh = runLogList[kj].Kh;
                        obj.Dzh = runLogList[kj].Dzh;
                        obj.Devid = runLogList[kj].Devid;
                        obj.Wzid = runLogList[kj].Wzid;
                        obj.Point = runLogList[kj].Point;
                        obj.Type = runLogList[kj].Type;
                        obj.State = runLogList[kj].State;
                        obj.Val = runLogList[kj].Val;
                        obj.Timer = runLogList[kj].Timer;
                        obj.Remark = runLogList[kj].Remark;
                        obj.Bz1 = runLogList[kj].Bz1;
                        obj.Bz2 = runLogList[kj].Bz2;
                        obj.Bz3 = runLogList[kj].Bz3;
                        obj.Bz4 = runLogList[kj].Bz4;
                        obj.Bz5 = runLogList[kj].Bz5;
                        obj.Upflag = runLogList[kj].Upflag;

                        jc_r.Add(long.Parse(obj.ID), obj);
                        addlist.Add(long.Parse(obj.ID));
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

            #region 判断运行记录记录是否可以删除
            DateTime now = Model.RealInterfaceFuction.GetServerNowTime();
            List<long> listkey = jc_r.Keys.ToList();
            for (int kj = 0; kj < listkey.Count; kj++)
            {
                key = listkey[kj];
                if ((now - jc_r[key].Timer).TotalMinutes > 1440)//将一天内的运行记录保留在客户端界面上  20170909
                {
                    deletelist.Add(key);
                }
            }
            #endregion
            #endregion

            #region //获取人员采集的轨迹实时信息 20171214
            try
            {
                //R_PhistoryInfo obj1 = null;
                //List<R_PhistoryInfo> r_PhistoryInfoList = Model.RealInterfaceFuction.GetRealR_PhistoryInfoList(R_PhistoryLastGetTime);
                //for (int kj = 0; kj < r_PhistoryInfoList.Count; kj++)
                //{
                //    key = long.Parse(r_PhistoryInfoList[kj].Id);
                //    if (r_PhistoryInfoList[kj].Timer > R_PhistoryLastGetTime)
                //    {//取最后获取的时间，作为下次获取数据的开始时间  20171214
                //        R_PhistoryLastGetTime = r_PhistoryInfoList[kj].Timer;
                //    }
                //    if (!R_PhistoryList.ContainsKey(key))
                //    {
                //        #region 新增人员实时采集记录
                //        obj1 = new R_PhistoryInfo();
                //        obj1.Id = r_PhistoryInfoList[kj].Id;
                //        obj1.Bh = r_PhistoryInfoList[kj].Bh;
                //        obj1.Name = r_PhistoryInfoList[kj].Name;
                //        obj1.Rtime = r_PhistoryInfoList[kj].Rtime;
                //        obj1.Timer = r_PhistoryInfoList[kj].Timer;
                //        obj1.Flag = r_PhistoryInfoList[kj].Flag;

                //        R_PhistoryList.Add(long.Parse(obj1.Id), obj1);
                //        r_phistoryaddlist.Add(long.Parse(obj1.Id));
                //        #endregion
                //    }
                //}
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            #endregion

            #region 判断人员定位采集记录是否可以删除
            List<long> listkey1 = R_PhistoryList.Keys.ToList();
            for (int kj = 0; kj < listkey1.Count; kj++)
            {
                key = listkey1[kj];
                if ((now - R_PhistoryList[key].Timer).TotalMinutes > 120)//将1小时内的人员采集记录保留在客户端界面上  20170909
                {
                    r_phistorydeletelist.Add(key);
                }
            }
            #endregion
        }

        #endregion

        private void RealRunRecordControl_Load(object sender, EventArgs e)
        {
            inigrid();
            try
            {
                //根据配置，判断并动态加载人员采集信息显示  20180131
                string displayPersonCollect = "1";
                SettingInfo setting = RealInterfaceFuction.GetConfigFKey("DisplayPersonCollect");
                if (setting != null)
                {
                    displayPersonCollect = setting.StrValue;
                }
                if (displayPersonCollect == "0")
                {
                    gridControl1.Visible = false;
                    mainGrid.Dock = DockStyle.Fill;
                }
                else
                {
                    gridControl1.Visible = true;
                    mainGrid.Dock = DockStyle.None;
                }

                devAllList = deviceDefineService.GetAllDeviceDefineCache().Data;

                //获取服务器当前时间
                var nowtime = Model.RealInterfaceFuction.GetServerNowTime();

                LastCounter = nowtime.Ticks;

                getdata();

                refresh(nowtime);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            //timer1.Enabled = true;
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.Start();
        }
        private Thread freshthread;
        private void fthread()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    //获取所有设备类型
                    listJcdev = RealInterfaceFuction.GetAllDeviceDefine();

                    //调用服务端接口，看能否正常调用来判断服务端是否开启
                    var response = ServiceFactory.Create<IRemoteStateService>().GetLastReciveTime();
                    getdata();
                    //获取服务器当前时间
                    DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                    MethodInvoker In = new MethodInvoker(() => refresh(nowtime));
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(In);
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(5000);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //try
            //{
            //    getdata();
            //    refresh();
            //}
            //catch (Exception ex)
            //{
            //    Basic.Framework.Logging.LogHelper.Error(ex);
            //}
            //timer1.Enabled = true;
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void mainGridView_Click(object sender, EventArgs e)
        {
            //OprFuction.ShowFromText(5);
        }
        /// <summary>
        /// 清除运行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barSubItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            jc_r.Clear();
            showdt.Rows.Clear();
        }

        private void mainGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }
        /// <summary>
        /// 风门过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barButtonItem2.Caption == "不显示风门")
            {

                isDispalyDamper = false;
                barButtonItem2.Caption = "显示风门";
            }
            else
            {
                isDispalyDamper = true;
                barButtonItem2.Caption = "不显示风门";
            }

        }
        /// <summary>
        /// 清除人员采集记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barSubItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            R_PhistoryList.Clear();
            showdt1.Rows.Clear();
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu2.ShowPopup(Control.MousePosition);
            }
        }
    }
}
