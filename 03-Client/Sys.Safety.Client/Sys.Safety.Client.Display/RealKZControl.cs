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
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Client.Display
{
    public partial class RealKZControl : XtraForm
    {
        List<Jc_DefInfo> pointDefineControlList = new List<Jc_DefInfo>();
        private static IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();
        string DefineTime = "";
        public RealKZControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }

        #region=====================实时控制逻辑======================

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
        /// 需要修改的id号
        /// </summary>
        public List<long> uplist = new List<long>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","断电/复电值","当前值","设备状态",
            "控制口","控制口状态","断电区域",
            "馈电监测点","馈电点状态","断馈状态","控制时间","id","endtime","当前状态"};

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","ddz",
            "ssz","sbstate", "kzk","kzzt","qy","kdk","kdkzt","kdzt","stime","id","endtime","alarmstate"};

        public int[] colwith = new int[] { 80, 80, 80, 80, 80, 80, 160, 80, 80, 80, 130, 80, 80, 80 };

        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();

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
                col.Tag = i;
                if (colname[i] == "id" || colname[i] == "endtime")
                {
                    col.Visible = false;
                }
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
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

        /// <summary>
        /// 刷新报警数据
        /// </summary>
        public void refresh(DateTime nowtime, List<Jc_DefInfo> pointDefineControlList)
        {
            string point = "";
            DataRow[] row, row1;
            DataRow r;
            int countn = 0;
            string[] kzk = null;
            Jc_BInfo obj = null;
            Jc_DefInfo tempdef = null;
            int state1 = 0, state2 = 0;
            int tbcount = 0;
            Dictionary<string, string> kzkd = new Dictionary<string, string>();
            string kdk = "";
            object wz = "", dw = "", bjz = "", ddz = "", fzh = "", sszn = "",
                bjtime = "", ddtime = "", cs = "", idn = "", qy = "", sbstate = "";
            long key = 0;
            List<long> listkey;

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
                OprFuction.SaveErrorLogs("删除控制记录", ex);
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
                        kzkd.Clear();
                        if (jc_b.ContainsKey(key))
                        {
                            obj = jc_b[key];
                            point = obj.Point;
                            sbstate = OprFuction.StateChange(obj.State.ToString());
                            if (obj.State == (short)StaticClass.itemStateToClient.EqpState32)
                            {
                                continue;//类型有误不做处理
                            }
                            lock (StaticClass.allPointDtLockObj)
                            {
                                row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (row.Length > 0)
                                {
                                    sszn = obj.Ssz + "/" + OprFuction.TimeToString(obj.Stime);
                                    if (obj.Type == StaticClass.itemStateToClient.EqpState9)
                                    {
                                        ddz = row[0]["sxbj"] + "|" + row[0]["sxyj"];
                                    }
                                    else if (obj.Type == StaticClass.itemStateToClient.EqpState11)
                                    {
                                        ddz = row[0]["sxdd"] + "|" + row[0]["sxfd"];
                                    }
                                    else if (obj.Type == StaticClass.itemStateToClient.EqpState19)
                                    {
                                        ddz = row[0]["xxbj"] + "|" + row[0]["xxyj"];
                                    }
                                    else if (obj.Type == StaticClass.itemStateToClient.EqpState21)
                                    {
                                        ddz = row[0]["xxdd"] + "|" + row[0]["xxfd"];
                                    }
                                    else
                                    {
                                        bjz = obj.Ssz;
                                        ddz = obj.Ssz;
                                    }
                                }
                            }
                            bjtime = OprFuction.TimeToString(obj.Stime);
                            idn = key;
                            #region 有控制口
                            if (!string.IsNullOrEmpty(obj.Kzk))
                            {
                                kzk = obj.Kzk.Split('|');
                                if (kzk.Length > 0)
                                {
                                    for (int i = 0; i < kzk.Length; i++)
                                    {
                                        #region 添加记录
                                        //tempdef = Model.RealInterfaceFuction.Getpoint(kzk[i]);
                                        tempdef = pointDefineControlList.Find(a => a.Point == kzk[i]);
                                        if (tempdef != null)
                                        {
                                            lock (objShowDt)
                                            {
                                                r = showdt.NewRow();
                                                r["point"] = point;
                                                r["ddz"] = ddz;
                                                r["ssz"] = sszn;
                                                r["sbstate"] = sbstate;
                                                r["qy"] = tempdef.Wz;
                                                r["kzk"] = kzk[i];
                                                lock (StaticClass.allPointDtLockObj)
                                                {
                                                    row = StaticClass.AllPointDt.Select("point='" + kzk[i] + "'");
                                                    if (row.Length > 0)
                                                    {
                                                        kdk = OprFuction.StateChange(row[0]["zt"].ToString());
                                                        if (kdk == "0态")
                                                        {
                                                            r["kzzt"] = row[0]["0t"].ToString();
                                                        }
                                                        else if (kdk == "1态")
                                                        {
                                                            r["kzzt"] = row[0]["1t"].ToString();
                                                        }
                                                        else
                                                        {
                                                            r["kzzt"] = kdk;
                                                        }
                                                    }
                                                }
                                                if (tempdef.K2 != 0)
                                                {
                                                    lock (StaticClass.allPointDtLockObj)
                                                    {
                                                        row = StaticClass.AllPointDt.Select("fzh='" + tempdef.Fzh + "' and tdh='" + tempdef.K2 + "' and lx='开关量'");
                                                        if (row.Length > 0)
                                                        {
                                                            r["kdk"] = row[0]["point"].ToString();
                                                            kdk = OprFuction.StateChange(row[0]["zt"].ToString());
                                                            if (kdk == "0态")
                                                            {
                                                                r["kdkzt"] = row[0]["0t"].ToString();
                                                            }
                                                            else if (kdk == "1态")
                                                            {
                                                                r["kdkzt"] = row[0]["1t"].ToString();
                                                            }
                                                            else if (kdk == "2态")
                                                            {
                                                                r["kdkzt"] = row[0]["2t"].ToString();
                                                            }
                                                            else
                                                            {
                                                                r["kdkzt"] = kdk;
                                                            }
                                                        }
                                                    }
                                                    r["kdzt"] = "检测中";
                                                }
                                                else
                                                {
                                                    r["kdk"] = "无";
                                                    r["kdkzt"] = "无";
                                                    r["kdzt"] = "无";
                                                }
                                                r["stime"] = bjtime;
                                                r["id"] = key;
                                                r["endtime"] = "";
                                                r["alarmstate"] = "进行中";
                                                row = showdt.Select("id='" + key + "' and kzk='" + kzk[i] + "'");
                                                if (row.Length < 1)
                                                {
                                                    showdt.Rows.InsertAt(r, 0);//添加新记录                                                
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加控制记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[4] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 刷新记录，控制状态、馈电开关状态、馈电状态
            try
            {
                if (showdt.Rows.Count > 0)
                {
                    lock (objShowDt)
                    {
                        for (int i = 0; i < showdt.Rows.Count; i++)
                        {

                            r = showdt.Rows[i];
                            if (r["endtime"].ToString() == "")//只要在列表中,并且未结束，就实时更新控制口状态  20170712
                            {
                                if (!r.IsNull("kzk"))
                                {
                                    #region 控制口状态
                                    lock (StaticClass.allPointDtLockObj)
                                    {
                                        row = StaticClass.AllPointDt.Select("point='" + r["kzk"].ToString() + "'");

                                        // 20170326
                                        if (row.Length > 0)
                                        {
                                            if (row[0].Table.Columns.Contains("zt"))
                                            {
                                                if (!row[0].IsNull("zt"))
                                                {
                                                    kdk = OprFuction.StateChange(row[0]["zt"].ToString());
                                                    if (kdk == "0态")
                                                    {
                                                        r["kzzt"] = row[0]["0t"].ToString();
                                                    }
                                                    else if (kdk == "1态")
                                                    {
                                                        r["kzzt"] = row[0]["1t"].ToString();
                                                    }
                                                    else
                                                    {
                                                        r["kzzt"] = kdk;
                                                    }
                                                    state1 = int.Parse(row[0]["zt"].ToString());
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 馈电及状态
                                    if (!r.IsNull("kdk") && r["kdk"].ToString() != "无")
                                    {
                                        lock (StaticClass.allPointDtLockObj)
                                        {
                                            row1 = StaticClass.AllPointDt.Select("point='" + r["kdk"].ToString() + "'");
                                            // 20170326
                                            if (row1.Length > 0)
                                            {
                                                if (row1[0].Table.Columns.Contains("zt"))
                                                {
                                                    if (!row1[0].IsNull("zt"))
                                                    {
                                                        kdk = OprFuction.StateChange(row1[0]["zt"].ToString());
                                                        if (kdk == "0态")
                                                        {
                                                            r["kdkzt"] = row1[0]["0t"].ToString();
                                                        }
                                                        else if (kdk == "1态")
                                                        {
                                                            r["kdkzt"] = row1[0]["1t"].ToString();
                                                        }
                                                        else if (kdk == "2态")
                                                        {
                                                            r["kdkzt"] = row1[0]["2t"].ToString();
                                                        }
                                                        else
                                                        {
                                                            r["kdkzt"] = kdk;
                                                        }
                                                        state2 = int.Parse(row1[0]["zt"].ToString());
                                                        //if (((nowtime -DateTime .Parse (row [0]["stime"].ToString ())).TotalSeconds>20))
                                                        //{
                                                        #region 馈电判断
                                                        //if (state1 == StaticClass.itemStateToClient.EqpState44)//控制口断开
                                                        //{
                                                        //    if (state2 == StaticClass.itemStateToClient.EqpState26)//馈电开关量为2态
                                                        //    {
                                                        //        r["kdzt"] = "断电失败";
                                                        //    }
                                                        //    else if (state2 == StaticClass.itemStateToClient.EqpState25)//馈电开关量为1态
                                                        //    {
                                                        //        r["kdzt"] = "断电成功";
                                                        //    }
                                                        //    else//馈电开关量为0态
                                                        //    {
                                                        //        r["kdzt"] = "断电成功";//0态默认是断电成功   20170622
                                                        //    }
                                                        //}
                                                        //else//控制口接通
                                                        //{
                                                        //    r["kdzt"] = "断电失败";
                                                        //}
                                                        //不自己判断，取内存中的控制量对应的馈电状态 
                                                        if (row[0]["NCtrlSate"].ToString() == "0") //复电成功
                                                        {
                                                            r["kdzt"] = "复电成功";
                                                        }
                                                        else if (row[0]["NCtrlSate"].ToString() == "2") //断电成功
                                                        {
                                                            r["kdzt"] = "断电成功";
                                                        }
                                                        else if (row[0]["NCtrlSate"].ToString() == "30") //复电失败
                                                        {
                                                            r["kdzt"] = "复电失败";
                                                        }
                                                        else if (row[0]["NCtrlSate"].ToString() == "32") //断电失败
                                                        {
                                                            r["kdzt"] = "断电失败";
                                                        }
                                                        else if (row[0]["NCtrlSate"].ToString() == "46") //未知
                                                        {
                                                            r["kdzt"] = "未知";
                                                        }

                                                        #endregion
                                                        //}
                                                        //else
                                                        //{
                                                        //    if (state1 == StaticClass.itemStateToClient.EqpState44)
                                                        //    {
                                                        //        if (state2 == StaticClass.itemStateToClient.EqpState25)
                                                        //        {
                                                        //            r["kdzt"] = "断电成功";
                                                        //        }
                                                        //    }
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 结束时间
                                    //if (uplist.Contains(long.Parse(r["id"].ToString())))
                                    //{
                                    //    r["endtime"] = "jieshu";
                                    //}
                                    lock (StaticClass.bjobj)
                                    {
                                        if (!OprFuction.IsInitTime(StaticClass.jcbdata[long.Parse(r["id"].ToString())].Etime))
                                        {
                                            r["endtime"] = StaticClass.jcbdata[long.Parse(r["id"].ToString())].Etime.ToString("yyyy-MM-dd HH:mm:ss");
                                            r["alarmstate"] = "已结束";
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    //uplist.Clear();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新控制记录", ex);
            }
            #endregion

            #region 当前报警条数
            tbcount = showdt.Select("endtime=''").Length;
            StaticClass.yccount[4] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增报警断电信息
        /// </summary>
        public void getdata()
        {
            short type = 0;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            DateTime maxtime = timenow;
            long key = 0;
            List<long> listkey;
            lock (StaticClass.bjobj)
            {
                #region 获取控制数据
                try
                {
                    listkey = StaticClass.jcbdata.Keys.ToList();
                    for (int kj = 0; kj < listkey.Count; kj++)
                    {
                        key = listkey[kj];
                        jcb = StaticClass.jcbdata[key];
                        type = jcb.Type;
                        if (!string.IsNullOrEmpty(jcb.Kzk))
                        {
                            if (OprFuction.IsInitTime(jcb.Etime))
                            {
                                if (!jc_b.ContainsKey(long.Parse(jcb.ID)))
                                {
                                    #region 新增主控点引发控制
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b.Add(long.Parse(jcb.ID), obj);
                                    addlist.Add(long.Parse(jcb.ID));
                                    #endregion
                                }
                                else
                                {
                                    if (jc_b[long.Parse(jcb.ID)].Kzk != jcb.Kzk)
                                    {
                                        obj = OprFuction.NewDTO(jcb);
                                        jc_b[long.Parse(jcb.ID)] = obj;
                                        addlist.Add(long.Parse(jcb.ID));
                                    }
                                }
                            }
                            else//只要列表中未结束的记录都更新，此处不需要每次加到uplist中  20170715
                            {
                                //if (jc_b.ContainsKey(long.Parse(jcb.ID)))//&& !OprFuction.IsInitTime(jc_b[long.Parse(jcb.ID)].Etime)  20170706
                                //{
                                //    #region 新增主控点引发控制
                                //    obj = OprFuction.NewDTO(jcb);
                                //    jc_b[long.Parse(jcb.ID)] = obj;
                                //    uplist.Add(long.Parse(jcb.ID));
                                //    #endregion
                                //}
                            }
                        }
                        else
                        {
                            //解决控制口取消绑定后，无法删除实时控制记录问题 2017.7.11 by
                            if (jc_b.ContainsKey(long.Parse(jcb.ID)))
                            {
                                deletelist.Add(long.Parse(jcb.ID));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                #region 判断记录是否可以删除
                DateTime now = Model.RealInterfaceFuction.GetServerNowTime();
                listkey = jc_b.Keys.ToList();
                for (int kj = 0; kj < listkey.Count; kj++)
                {
                    key = listkey[kj];
                    if (!StaticClass.jcbdata.ContainsKey(key))
                    {
                        deletelist.Add(key);
                    }
                }
                #endregion
                #endregion
            }
        }
        #endregion

        private void RealKZControl_Load(object sender, EventArgs e)
        {
            inigrid();
            try
            {
                getdata();
                var nowtime = Model.RealInterfaceFuction.GetServerNowTime();

                PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
                PointDefineRequest.DevpropertID = 3;
                List<Jc_DefInfo> pointDefineControlList = pointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest).Data;

                refresh(nowtime, pointDefineControlList);
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
                    PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
                    PointDefineRequest.DevpropertID = 3;
                    if (pointDefineControlList.Count == 0 || GetDefineChangeFlg())
                    {
                        pointDefineControlList = pointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest).Data;
                    }

                    getdata();
                    var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                    MethodInvoker In = new MethodInvoker(() => refresh(nowtime, pointDefineControlList));
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(In);
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 获取定义改变时间 判断定义是否改变
        /// </summary>
        /// <returns></returns>
        public bool GetDefineChangeFlg()
        {
            bool flg = false;
            string time = "";
            try
            {
                var respone = realMessageService.GetDefineChangeFlg();
                time = respone.Data;
                if (time != "")
                {
                    if (DefineTime == "")
                    {
                        DefineTime = time;
                        flg = true;
                    }
                    else
                    {
                        if (DefineTime != time)
                        {
                            DefineTime = time;
                            flg = true;
                        }
                    }
                }
            }
            catch
            {

            }
            return flg;
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
            //OprFuction.ShowFromText(4);
        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string value = "";
            if (e.Column.Tag.ToString() == "8")
            {
                if (e.CellValue != null)
                {
                    value = e.CellValue.ToString();
                    if (value == "断电失败" || value == "检测失败")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else if (value == "断电成功")
                    {
                        e.Appearance.ForeColor = Color.Blue;
                    }
                }
            }
        }
    }
}
