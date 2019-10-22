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
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Display
{
    public partial class RealyjControl : XtraForm
    {
        public RealyjControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }

        #region======================模拟量预警======================
        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;

        public Thread thread;

        /// <summary>
        /// 预警数据源
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
        public List<long> updatelist = new List<long>();


        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","设备类型","当前值","单位",
            "预警类型","设备状态","预警开始时间","预警持续时间","预警值","最大值","最小值","平均值","结束时间","总数","总值","id"};//"分站/通道/地址",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","devname",
            "ssz","dw","state","sbstate", "stime","cxtime","yjz","zdz",
            "zxz", "pjz","endtime","count","allvalue","id" };//"fzh",

        public int[] colwith = new int[] { 60, 160, 100,  100, 50, 50, 80, 80, 130, 80, 50, 50, 50, 80, 80, 80, 80, 80 };

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
                if (colname[i] == "结束时间" || colname[i] == "总数" || colname[i] == "总值" || colname[i] == "id")
                {
                    col.Visible = false;
                }
                mainGridView.Columns.Add(col);
            }
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
            mainGrid.DataSource = showdt;
        }

        /// <summary>
        /// 刷新预警数据
        /// </summary>
        public void refresh(DateTime nowtime)
        {
            string point = "", ssz1, ssz2;
            double ssz, zdz, zxz, pjz, allvlaue, count;
            DataRow[] row;
            DataRow r;
            TimeSpan span;
            int countn = 0;
            long id = 0;
            Jc_BInfo obj = null;
            int tbcount = 0;
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
                OprFuction.SaveErrorLogs("删除预警记录", ex);
            }
            #endregion

            #region 刷新实时值、最大值、最小值、平均值 持续时间 结束时间
            try
            {
                lock (objShowDt)//防止删除的时候，遍历datatable修改对象找不到对象
                {
                    for (int i = 0; i < showdt.Rows.Count; i++)
                    {
                        if (showdt.Rows[i].IsNull("endtime") || showdt.Rows[i]["endtime"].ToString() == "")
                        {

                            id = long.Parse(showdt.Rows[i]["id"].ToString());

                            if (updatelist.Contains(id))
                            {

                                #region 刷新 最大值、平均值 结束时间
                                obj = jc_b[id];
                                //showdt.Rows[i]["zdz"] = obj .Zdz;

                                showdt.Rows[i]["pjz"] = obj.Pjz;
                                showdt.Rows[i]["endtime"] = OprFuction.TimeToString(obj.Etime);

                                #endregion

                                #region 刷新持续时间
                                span = obj.Etime - Convert.ToDateTime(showdt.Rows[i]["stime"]);
                                showdt.Rows[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                #endregion
                            }
                            else
                            {
                                #region 刷新持续时间
                                span = nowtime - Convert.ToDateTime(showdt.Rows[i]["stime"]);
                                showdt.Rows[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                #endregion

                                #region 刷新实时值、最大值、最小值、平均值
                                point = showdt.Rows[i]["point"].ToString();
                                ssz2 = showdt.Rows[i]["ssz"].ToString();
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                    if (row.Length > 0)
                                    {
                                        ssz1 = row[0]["ssz"].ToString();
                                        if (ssz1 != ssz2)
                                        {
                                            if (double.TryParse(ssz1, out ssz))
                                            {
                                                zdz = Convert.ToDouble(showdt.Rows[i]["zdz"]);
                                                zxz = Convert.ToDouble(showdt.Rows[i]["zxz"]);
                                                pjz = Convert.ToDouble(showdt.Rows[i]["pjz"]);
                                                count = Convert.ToDouble(showdt.Rows[i]["count"]);
                                                allvlaue = Convert.ToDouble(showdt.Rows[i]["allvalue"]);
                                                if (ssz > zdz)
                                                {
                                                    zdz = ssz;
                                                }
                                                else if (ssz < zxz)
                                                {
                                                    zxz = ssz;
                                                }
                                                count += 1;
                                                allvlaue += ssz;
                                                pjz = Math.Round(allvlaue / count, 2);
                                                showdt.Rows[i]["zdz"] = zdz;
                                                showdt.Rows[i]["zxz"] = zxz;
                                                showdt.Rows[i]["pjz"] = pjz;
                                                showdt.Rows[i]["ssz"] = ssz;
                                                showdt.Rows[i]["count"] = count;
                                                if (showdt.Rows[i].Table.Columns.Contains("allvlaue"))
                                                {
                                                    showdt.Rows[i]["allvlaue"] = allvlaue;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                updatelist.Clear();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新实时值、最大值、最小值、平均值", ex);
            }
            #endregion

            #region 添加记录
            if (addlist.Count > 0)
            {
                lock (objShowDt)
                {
                    for (int kj = 0; kj < addlist.Count; kj++)
                    {
                        key = addlist[kj];
                        if (jc_b.ContainsKey(key))
                        {
                            obj = jc_b[key];

                            r = showdt.NewRow();
                            r["point"] = obj.Point;
                            lock (StaticClass.allPointDtLockObj)
                            {
                                row = StaticClass.AllPointDt.Select("point='" + obj.Point + "'");
                                if (row.Length > 0)
                                {
                                    r["wz"] = row[0]["wz"];
                                    if (obj.Type == StaticClass.itemStateToClient.EqpState7)
                                    {
                                        r["yjz"] = row[0]["sxyj"];
                                    }
                                    else
                                    {
                                        r["yjz"] = row[0]["xxyj"];
                                    }
                                }
                            }
                            //TODO:和其它业务模块相关联
                            //r["fzh"] = obj.Fzh + "/" + obj.Kh + "/" + obj.Dzh;
                            //r["ssz"] = obj.Ssz;
                            //r["state"] = OprFuction.StateChange(obj.Type.ToString());
                            //r["sbstate"] = OprFuction.StateChange(obj.State.ToString());
                            //r["stime"] = OprFuction.TimeToString(obj.Stime);
                            //if (!OprFuction.IsInitTime(obj.Etime))
                            //{
                            //    r["endtime"] = OprFuction.TimeToString(obj.Etime);
                            //}
                            //span = nowtime - obj.Stime;
                            //r["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);

                            //r["zdz"] = obj.Ssz;
                            //r["zxz"] = obj.Ssz;
                            //r["pjz"] = obj.Ssz;
                            //r["count"] = 1;
                            //r["allvalue"] = obj.Ssz;
                            r["id"] = key;

                            showdt.Rows.InsertAt(r, 0);//添加新记录
                        }
                    }
                }
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前报警条数
            tbcount = showdt.Rows.Count;
            StaticClass.yccount[0] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增预警信息
        /// </summary>
        public void getdata()
        {
            short type = 0;
            long id = 0;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            lock (StaticClass.bjobj)
            {
                try
                {
                    #region 获取预警信息
                    foreach (long key in StaticClass.jcbdata.Keys)
                    {
                        jcb = StaticClass.jcbdata[key];
                        type = jcb.Type;
                        if (type == StaticClass.itemStateToClient.EqpState7 || type == StaticClass.itemStateToClient.EqpState17)
                        {
                            id = long.Parse(jcb.ID);
                            if (!jc_b.ContainsKey(id))
                            {
                                #region 新增预警
                                obj = OprFuction.NewDTO(jcb);
                                jc_b.Add(id, obj);
                                addlist.Add(id);
                                #endregion
                            }
                            else
                            {
                                #region 新增预警结束
                                if (!OprFuction.IsInitTime(jcb.Etime))
                                {
                                    if (OprFuction.IsInitTime(jc_b[id].Etime))
                                    {
                                        obj = OprFuction.NewDTO(jcb);
                                        jc_b[id] = obj;
                                        updatelist.Add(id);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #region 判断是否预警还存在
                    foreach (long key in jc_b.Keys)
                    {
                        if (!StaticClass.jcbdata.ContainsKey(key))
                        {
                            deletelist.Add(key);
                        }
                    }
                    #endregion
                    #endregion
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
            }
        }

        #endregion

        private void RealyjControl_Load(object sender, EventArgs e)
        {
            //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            inigrid();
            inidt();
            try
            {
                var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                getdt(nowtime);
                refresh1(nowtime);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            //timer1.Enabled = true;
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.Start();
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

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            object obj;
            if (e.RowHandle > -1)
            {
                obj = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns["endtime"]);
                if (obj == null || obj.ToString() == "")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Empty;
                }
            }
        }
        private int timern = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //try
            //{
            //    if (timern >= 3)
            //    {
            //        timern = 0;
            //        getdata();
            //        refresh();
            //    }
            //    else
            //    {
            //        refresh();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Basic.Framework.Logging.LogHelper.Error(ex);
            //}
            //timern++;
            //timer1.Enabled = true;
        }
        private Thread freshthread;
        private void fthread()
        {
            int timern = 1;
            while (!StaticClass.SystemOut)
            {
                try
                {
                    //调用服务端接口，看能否正常调用来判断服务端是否开启
                    var response = ServiceFactory.Create<IRemoteStateService>().GetLastReciveTime();
                    if (timern >= 3)
                    {
                        timern = 0;
                        var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                        MethodInvoker In = new MethodInvoker(() => getdt(nowtime));
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                    }
                    else
                    {
                        DateTime nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                        MethodInvoker In = new MethodInvoker(() => refresh(nowtime));
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                timern++;
                Thread.Sleep(1000);
            }
        }

        private void mainGrid_Click(object sender, EventArgs e)
        {

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
            //OprFuction.ShowFromText(0);
        }



        private void getdt(DateTime nowtime)
        {
            DataRow[] rows = null;
            DataRow row = null, r = null;
            TimeSpan span;
            string point = "", ssz1, ssz2;
            double ssz, zdz, zxz, pjz, allvlaue;
            int zt = -1;
            DataTable dt = showdt.Clone();
            List<int> fzh;
            int x = -1, y = -1, count = 0, toprowindex = 0;
            try
            {

                #region  模拟量预警信息
                lock (StaticClass.allPointDtLockObj)
                {                   
                    rows = StaticClass.AllPointDt.Select("lx='模拟量' and ( zt='8' or zt='14')", "fzh,tdh,dzh");

                    //modified by  20170315
                    //在调试代码时，发现此select返回的行，第一行有时有为null的情况（估计是datatable读写并发引起返回为空行记录，临时通过特殊处理空行记录），
                    if (rows != null && rows.Length > 0)
                    {
                        List<DataRow> list = new List<DataRow>();
                        foreach (DataRow dr in rows)
                        {
                            if (dr != null) //移除空行
                            {
                                list.Add(dr);
                            }
                        }
                        rows = list.ToArray();
                    }

                    if (rows.Length > 0)
                    {
                        DataTable dt1 = rows.CopyToDataTable();
                        #region 删除已结束的报警
                        lock (objShowDt)
                        {
                            for (int i = 0; i < showdt.Rows.Count; i++)
                            {
                                rows = dt1.Select("point='" + showdt.Rows[i]["point"].ToString() + "'");
                                if (rows.Length > 0)
                                {

                                    #region 刷新持续时间
                                    span = nowtime - Convert.ToDateTime(showdt.Rows[i]["stime"]);
                                    showdt.Rows[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                    #endregion

                                    #region 刷新实时值、最大值、最小值、平均值
                                    point = showdt.Rows[i]["point"].ToString();
                                    ssz2 = showdt.Rows[i]["ssz"].ToString();
                                    ssz1 = rows[0]["ssz"].ToString();
                                    if (ssz1 != ssz2)
                                    {
                                        if (double.TryParse(ssz1, out ssz))
                                        {
                                            zdz = Convert.ToDouble(showdt.Rows[i]["zdz"]);
                                            zxz = Convert.ToDouble(showdt.Rows[i]["zxz"]);
                                            pjz = Convert.ToDouble(showdt.Rows[i]["pjz"]);
                                            count = Convert.ToInt32(showdt.Rows[i]["count"]);
                                            allvlaue = Convert.ToDouble(showdt.Rows[i]["allvalue"]);
                                            if (ssz > zdz)
                                            {
                                                zdz = ssz;
                                            }
                                            else if (ssz < zxz)
                                            {
                                                zxz = ssz;
                                            }
                                            count += 1;
                                            allvlaue += ssz;
                                            pjz = Math.Round(allvlaue / count, 2);
                                            showdt.Rows[i]["zdz"] = zdz;
                                            showdt.Rows[i]["zxz"] = zxz;
                                            showdt.Rows[i]["pjz"] = pjz;
                                            showdt.Rows[i]["ssz"] = ssz;
                                            showdt.Rows[i]["count"] = count;

                                            showdt.Rows[i]["allvalue"] = allvlaue;


                                        }
                                    }
                                    #endregion

                                }
                                else
                                {
                                    showdt.Rows.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        #endregion

                        #region 添加新的报警
                        lock (objShowDt)
                        {
                            for (int kj = 0; kj < dt1.Rows.Count; kj++)
                            {
                                r = dt1.Rows[kj];
                                rows = showdt.Select("point='" + r["point"].ToString() + "'");
                                {
                                    if (rows.Length > 0)
                                    {
                                        continue;
                                    }
                                }
                                row = showdt.NewRow();
                                row["point"] = r["point"];
                                row["wz"] = r["wz"];
                                if (r["zt"].ToString() == StaticClass.itemStateToClient.EqpState7.ToString())
                                {
                                    row["yjz"] = r["sxyj"];
                                }
                                else
                                {
                                    row["yjz"] = r["xxyj"];
                                }
                                //row["fzh"] = r["fzh"] + "/" + r["tdh"] + "/" + r["dzh"];
                                row["ssz"] = r["ssz"];
                                row["state"] = OprFuction.StateChange(r["zt"].ToString());
                                row["sbstate"] = OprFuction.StateChange(r["sbzt"].ToString());
                                row["stime"] = OprFuction.TimeToString(DateTime.Parse(r["time"].ToString()));
                                span = nowtime - DateTime.Parse(r["time"].ToString());
                                row["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                row["zdz"] = r["ssz"];
                                row["zxz"] = r["ssz"];
                                row["pjz"] = r["ssz"];
                                row["count"] = 1;
                                row["allvalue"] = r["ssz"];
                                row["id"] = 0;
                                row["devname"] = r["lb"];
                                row["dw"] = r["dw"];
                                showdt.Rows.InsertAt(row, 0);//添加新记录
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        lock (objShowDt)
                        {
                            showdt.Rows.Clear();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

        }


        /// <summary>
        /// 刷新故障数据
        /// </summary>
        public void refresh1(DateTime nowtime)
        {
            string point = "", ssz1, ssz2, state;
            double ssz, zdz, zxz, pjz, allvlaue, count, yjz;
            DataRow[] row;
            TimeSpan span;
            int tbcount = 0;
            #region 刷新实时值、最大值、最小值、平均值 持续时间 结束时间
            try
            {
                lock (objShowDt)
                {
                    for (int i = 0; i < showdt.Rows.Count; i++)
                    {

                        #region 刷新持续时间
                        span = nowtime - Convert.ToDateTime(showdt.Rows[i]["stime"]);
                        showdt.Rows[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                        #endregion

                        #region 刷新实时值、最大值、最小值、平均值
                        point = showdt.Rows[i]["point"].ToString();
                        ssz2 = showdt.Rows[i]["ssz"].ToString();
                        lock (StaticClass.allPointDtLockObj)
                        {
                            row = StaticClass.AllPointDt.Select("point='" + point + "'");
                            if (row.Length > 0)
                            {
                                ssz1 = row[0]["ssz"].ToString();
                                if (ssz1 != ssz2)
                                {
                                    if (double.TryParse(ssz1, out ssz))
                                    {
                                        state = showdt.Rows[i]["state"].ToString();
                                        yjz = Convert.ToDouble(showdt.Rows[i]["yjz"]);
                                        zdz = Convert.ToDouble(showdt.Rows[i]["zdz"]);
                                        zxz = Convert.ToDouble(showdt.Rows[i]["zxz"]);
                                        pjz = Convert.ToDouble(showdt.Rows[i]["pjz"]);
                                        count = Convert.ToDouble(showdt.Rows[i]["count"]);
                                        allvlaue = Convert.ToDouble(showdt.Rows[i]["allvalue"]);
                                        if (ssz > zdz)
                                        {
                                            zdz = ssz;
                                        }
                                        else if (ssz < zxz)
                                        {
                                            zxz = ssz;
                                        }
                                        count += 1;
                                        allvlaue += ssz;
                                        pjz = Math.Round(allvlaue / count, 2);
                                        if ((state.Contains("上限") && (ssz >= yjz)) || (state.Contains("下限") && (ssz <= yjz)))
                                        {
                                            showdt.Rows[i]["zdz"] = zdz;
                                            showdt.Rows[i]["zxz"] = zxz;
                                            showdt.Rows[i]["pjz"] = pjz;
                                            showdt.Rows[i]["ssz"] = ssz;
                                            showdt.Rows[i]["count"] = count;
                                            showdt.Rows[i]["allvalue"] = allvlaue;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                #region 当前报警条数
                tbcount = showdt.Rows.Count;
                StaticClass.yccount[0] = tbcount;
                #endregion
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新实时值、最大值、最小值、平均值", ex);
            }
            #endregion
        }
    }
}
