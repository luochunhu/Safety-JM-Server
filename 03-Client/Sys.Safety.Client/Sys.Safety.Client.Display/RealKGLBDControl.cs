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

namespace Sys.Safety.Client.Display
{
    public partial class RealKGLBDControl : XtraForm
    {
        public RealKGLBDControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }

        #region======================开关量状态变动======================

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
        /// 报警断电数据源
        /// </summary>
        public Dictionary<long, Jc_BInfo> jc_b = new Dictionary<long, Jc_BInfo>();


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
        public string[] colname = new string[] { "测点编号","安装位置","当前状态","设备状态","变化时刻",
            "id"};//"分站/通道/地址",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","state","sbstate",
            "time","id"};//"fzh",

        public int[] colwith = new int[] { 80, 180,  80, 80, 130, 80 };

        /// <summary>
        /// 显示showdt操作对象锁  20170705
        /// </summary>
        protected readonly object objShowDt = new object();

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
                if (colname[i] == "id" || colname[i] == "分站/通道/地址")
                {
                    col.Visible = false;
                }
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;
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
        /// 刷新报警数据
        /// </summary>
        public void refresh(DateTime nowtime)
        {            
            string point = "";
            DataRow[] row;
            DataRow r;
            int countn = 0;
            long id = 0;
            Jc_BInfo obj = null;
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
                        if (jc_b.ContainsKey(key))
                        {
                            jc_b.Remove(key);
                        }
                    }
                    deletelist.Clear();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("删除开关量记录", ex);
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
                        if (jc_b.ContainsKey(key))
                        {
                            obj = jc_b[key];
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
                            sszn = obj.Ssz;
                            state = OprFuction.StateChange(obj.Type.ToString());
                            sbstate = OprFuction.StateChange(obj.State.ToString());
                            stime = OprFuction.TimeToString(obj.Stime);
                            id = key;
                            lock (objShowDt)
                            {
                                r = showdt.NewRow();
                                r["point"] = point;
                                r["wz"] = wz;
                                //r["fzh"] = fzh;
                                //r["ssz"] = sszn;
                                if (state.ToString() == "0态")
                                {
                                    r["state"] = row[0]["0t"].ToString();
                                }
                                else if (state.ToString() == "1态")
                                {
                                    r["state"] = row[0]["1t"].ToString();
                                }
                                else
                                {
                                    r["state"] = row[0]["2t"].ToString();
                                }
                                r["sbstate"] = sbstate;
                                r["time"] = stime;
                                r["id"] = key;
                                showdt.Rows.InsertAt(r, 0);//添加新记录
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加开关量报警记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[5] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion
            #region 当前报警条数
            tbcount = showdt.Rows.Count;
            StaticClass.yccount[5] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增报警断电信息
        /// </summary>
        public void getdata()
        {
            short type = 0;
            long id = 0;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            DateTime maxtime = timenow;
            long key = 0;
            List<long> listkey;
            lock (StaticClass.bjobj)
            {
                #region 获取开关量状态变动数据
                try
                {
                    listkey = StaticClass.jcbdata.Keys.ToList();
                    for (int kj = 0; kj < listkey.Count; kj++)
                    {
                        key = listkey[kj];
                        jcb = StaticClass.jcbdata[key];
                        type = jcb.Type;
                        if (jcb.Stime >= timenow)
                        {

                            if (OprFuction.IsKGL(jcb) && !jc_b.ContainsKey(long.Parse(jcb.ID)))
                            {
                                if (jcb.Stime > maxtime)
                                {
                                    maxtime = jcb.Stime;
                                    id = long.Parse(jcb.ID);
                                }
                                #region 新增开关量状态
                                obj = OprFuction.NewDTO(jcb);
                                jc_b.Add(long.Parse(obj.ID), obj);
                                addlist.Add(long.Parse(obj.ID));
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                if (id > dqid)
                {
                    dqid = id;
                }
                if (maxtime > timenow)
                {
                    timenow = maxtime;
                }
                #region 判断开关量状态记录是否可以删除
                DateTime now = Model.RealInterfaceFuction.GetServerNowTime();
                listkey = jc_b.Keys.ToList();
                for (int kj = 0; kj < listkey.Count; kj++)
                {
                    key = listkey[kj];
                    if ((now - jc_b[key].Stime).TotalMinutes > 1)
                    {
                        deletelist.Add(key);
                    }
                }
                #endregion
                #endregion
            }
        }

        #endregion

        private void RealKGLBDControl_Load(object sender, EventArgs e)
        {
            inigrid();
            try
            {
                getdata();
                //获取服务器当前时间
                var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
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
                    //调用服务端接口，看能否正常调用来判断服务端是否开启
                    var response = ServiceFactory.Create<IRemoteStateService>().GetLastReciveTime();
                    getdata();
                    //获取服务器当前时间
                    DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                    MethodInvoker In = new MethodInvoker(()=>refresh(nowtime));
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(In);
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(1000);
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
    }
}
