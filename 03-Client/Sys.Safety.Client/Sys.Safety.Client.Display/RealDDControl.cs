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
using Basic.Framework.Common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Sys.Safety.Client.Alarm;

namespace Sys.Safety.Client.Display
{
    public partial class RealDDControl : XtraForm
    {
        public RealDDControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }


        #region======================模拟量报警逻辑======================

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;

        /// <summary>
        /// 报警断电数据源
        /// </summary>
        public Dictionary<long, Jc_BInfo> jc_b = new Dictionary<long, Jc_BInfo>();

        /// <summary>
        /// 存储措施
        /// </summary>
        public Dictionary<long, string> CSlist = new Dictionary<long, string>();
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
        /// 需要修改的id号 馈电
        /// </summary>
        public List<long> kdlist = new List<long>();
        /// <summary>
        /// 已选择测点列表
        /// </summary>
        private List<string> _masterSelectedPoint = new List<string>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","设备类型","当前值","单位",
            "报警类型","设备状态","开始时间","持续时间","报警/解警值","最大值","最小值","平均值","断电/报警时刻",
            "断电/复电值","区域","馈电异常/时刻","措施/时刻","结束时间","总数","总值","kzk","id"};//"分站/通道/地址",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","devname",
            "ssz","dw","state","sbstate", "stime","cxtime","bjz","zdz",
            "zxz", "pjz","ddtime","ddz","qy","kdtime","cs","endtime","count","allvalue","kzk","id" };//"fzh",

        public int[] colwith = new int[] { 60, 160, 100,  50, 50, 60, 80, 130, 80, 110, 50, 50, 50, 130, 110, 140, 130, 130, 80, 80, 80, 80, 80 };//80,

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
                if (colname[i] == "分站/通道/地址" || colname[i] == "结束时间" || colname[i] == "总数" || colname[i] == "总值" || colname[i] == "id" || colname[i] == "kzk")
                {
                    col.Visible = false;
                }
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = showdt;

            setfontsize(9);
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
        public void refresh(DateTime nowtime)
        {
            string point = "", ssz1, ssz2;
            double ssz, zdz, zxz, pjz, allvlaue, count;
            DataRow[] row, row1;
            DataRow r;
            TimeSpan span;
            int countn = 0;
            long id = 0;
            string[] kzk = null, kdid = null, kdid1 = null;
            long temp = 0;
            Jc_BInfo obj = null, tempobj = null;
            int tbcount = 0;
            long key = 0;
            List<long> listkey = new List<long> { };
            Dictionary<string, string> kzkd = new Dictionary<string, string>();
            object wz = "", bjz = "", ddz = "", fzh = "", sszn = "", state = "", sbstate = "", stime = "", etime = "",
                cxtime = "", zdzn = "", zxzn = "", pjzn = "", ddtime = "", cs = "", cut = "", allvalue = "", idn = "", qy = "", devname = "", dw = "";

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

                        if (CSlist.ContainsKey(key))
                        {
                            CSlist.Remove(key);
                        }
                    }
                    deletelist.Clear();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("删除模拟量报警记录", ex);
            }
            #endregion

            #region 修改及刷新记录

            #region old
            //try
            //{
            //    listkey = jc_b.Keys.ToList();
            //    for (int kj = 0; kj < listkey.Count; kj++)
            //    {
            //        key = listkey[kj];
            //        if (updatelist.Contains(key))
            //        {
            //            lock (objShowDt)
            //            {
            //                row = showdt.Select("id='" + key + "'");
            //                if (row.Length > 0)
            //                {
            //                    #region 刷新 平均值 结束时间 持续时间
            //                    if (row[0]["endtime"].ToString() == "")//结束时间更新
            //                    {
            //                        for (int i = 0; i < row.Length; i++)
            //                        {
            //                            #region 刷新 平均值 结束时间 持续时间
            //                            obj = jc_b[key];
            //                            if (!OprFuction.IsInitTime(obj.Etime))
            //                            {
            //                                //row[i]["zdz"] = obj.Zdz;
            //                                //row[i]["pjz"] = obj.Pjz;
            //                                row[i]["endtime"] = OprFuction.TimeToString(obj.Etime);
            //                                span = obj.Etime - Convert.ToDateTime(row[i]["stime"]);
            //                                row[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
            //                            }
            //                            else
            //                            {
            //                                span = nowtime - Convert.ToDateTime(row[i]["stime"]);
            //                                row[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
            //                            }

            //                            #endregion
            //                        }
            //                    }
            //                    #endregion

            //                    #region 刷新cs
            //                    if (row[0]["cs"].ToString() != jc_b[key].Cs && !OprFuction.GetClientType())
            //                    {
            //                        for (int i = 0; i < row.Length; i++)
            //                        {
            //                            #region 刷新 cs
            //                            row[i]["cs"] = jc_b[key];
            //                            #endregion
            //                        }
            //                    }
            //                    #endregion
            //                }

            //                #region 修改馈电
            //                if (kdlist.Contains(key))
            //                {
            //                    obj = jc_b[key];
            //                    kdid = obj.Kdid.Split('|');
            //                    if (kdid.Length > 0)
            //                    {
            //                        if (!string.IsNullOrEmpty(kdid[0]))
            //                        {
            //                            kdid1 = kdid[0].Split(',');
            //                            lock (StaticClass.bjobj)
            //                            {
            //                                try
            //                                {
            //                                    for (int i = 0; i < kdid1.Length; i++)
            //                                    {
            //                                        if (!string.IsNullOrEmpty(kdid1[i]))
            //                                        {
            //                                            temp = long.Parse(kdid1[i]);
            //                                            if (StaticClass.jcbdata.ContainsKey(temp))
            //                                            {
            //                                                tempobj = StaticClass.jcbdata[temp];
            //                                                row = showdt.Select("id ='" + obj.ID + "' and point='" + obj.Point + "' and kzk='" + tempobj.Point + "'");
            //                                                if (row.Length > 0)
            //                                                {
            //                                                    row[0]["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                {
            //                                    Basic.Framework.Logging.LogHelper.Error(ex);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                #endregion
            //            }
            //        }
            //        else
            //        {
            //            lock (objShowDt)
            //            {
            //                row = showdt.Select("id='" + key + "'");
            //                if (row.Length > 0)
            //                {
            //                    if (row[0].IsNull("endtime") || row[0]["endtime"].ToString() == "")
            //                    {
            //                        #region  //刷新持续时间、实时值、最大d值、最小值、平均值
            //                        span = nowtime - Convert.ToDateTime(row[0]["stime"]);
            //                        cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);

            //                        //for (int j = 0; j < row.Length; j++)
            //                        //{
            //                        //    row[j]["cxtime"] = cxtime;
            //                        //}
            //                        row[0]["cxtime"] = cxtime;

            //                        if (!row[0]["state"].ToString().Contains("断线") && !row[0]["state"].ToString().Contains("上溢") && !row[0]["state"].ToString().Contains("负漂"))
            //                        {
            //                            point = row[0]["point"].ToString();
            //                            ssz2 = row[0]["ssz"].ToString();
            //                            lock (StaticClass.allPointDtLockObj)
            //                            {
            //                                row1 = StaticClass.AllPointDt.Select("point='" + point + "'");
            //                                if (row1.Length > 0)
            //                                {
            //                                    ssz1 = row1[0]["ssz"].ToString();
            //                                    if (ssz1 != ssz2)
            //                                    {
            //                                        if (double.TryParse(ssz1, out ssz))
            //                                        {
            //                                            #region
            //                                            zdz = Convert.ToDouble(row[0]["zdz"]);
            //                                            zxz = Convert.ToDouble(row[0]["zxz"]);
            //                                            pjz = Convert.ToDouble(row[0]["pjz"]);
            //                                            count = Convert.ToDouble(row[0]["count"]);
            //                                            allvlaue = Convert.ToDouble(row[0]["allvalue"]);
            //                                            if (ssz > zdz)
            //                                            {
            //                                                zdz = ssz;
            //                                            }
            //                                            else if (ssz < zxz)
            //                                            {
            //                                                zxz = ssz;
            //                                            }
            //                                            count += 1;
            //                                            allvlaue += ssz;
            //                                            pjz = Math.Round(allvlaue / count, 2);

            //                                            for (int j = 0; j < row.Length; j++)
            //                                            {
            //                                                row[j]["zdz"] = zdz;
            //                                                row[j]["zxz"] = zxz;
            //                                                row[j]["pjz"] = pjz;
            //                                                row[j]["ssz"] = ssz;
            //                                                row[j]["count"] = count;
            //                                                row[j]["allvalue"] = allvlaue;
            //                                            }
            //                                            #endregion
            //                                        }
            //                                        //else
            //                                        //{
            //                                        //    for (int j = 0; j < row.Length; j++)
            //                                        //    {
            //                                        //        row[j]["cxtime"] = cxtime;
            //                                        //    }
            //                                        //}
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        #endregion
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    updatelist.Clear();
            //    kdlist.Clear();
            //}
            //catch (Exception ex)
            //{
            //    OprFuction.SaveErrorLogs("修改模拟量报警记录", ex);
            //}
            #endregion

            #region//重写刷新方法，每次刷新会导致客户端卡死  20170719
            try
            {
                listkey = jc_b.Keys.ToList();
                for (int kj = 0; kj < showdt.Rows.Count; kj++)
                {
                    key = long.Parse(showdt.Rows[kj]["id"].ToString());
                    if (updatelist.Contains(key))
                    {
                        lock (objShowDt)
                        {
                            #region 刷新 平均值 结束时间 持续时间
                            if (showdt.Rows[kj]["endtime"].ToString() == "")//结束时间更新
                            {

                                #region 刷新 平均值 结束时间 持续时间
                                obj = jc_b[key];
                                if (!OprFuction.IsInitTime(obj.Etime))
                                {
                                    //row[i]["zdz"] = obj.Zdz;
                                    //row[i]["pjz"] = obj.Pjz;
                                    showdt.Rows[kj]["endtime"] = OprFuction.TimeToString(obj.Etime);
                                    span = obj.Etime - Convert.ToDateTime(showdt.Rows[kj]["stime"]);
                                    showdt.Rows[kj]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                }
                                else
                                {
                                    span = nowtime - Convert.ToDateTime(showdt.Rows[kj]["stime"]);
                                    showdt.Rows[kj]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                                }

                                #endregion

                            }
                            #endregion

                            #region 刷新cs
                            if (!string.IsNullOrEmpty(jc_b[key].Cs))
                            {
                                if (showdt.Rows[kj]["cs"].ToString() != (jc_b[key].Cs + "/" + jc_b[key].Bz2))//&& !OprFuction.GetClientType() 注释  20170817
                                {
                                    #region 刷新 cs
                                    showdt.Rows[kj]["cs"] = (jc_b[key].Cs + "/" + jc_b[key].Bz2);
                                    #endregion
                                }
                            }
                            else
                            {
                                showdt.Rows[kj]["cs"] = "";
                            }
                            #endregion

                            #region 修改馈电
                            if (kdlist.Contains(key))
                            {
                                obj = jc_b[key];
                                kdid = obj.Kdid.Split('|');
                                if (kdid.Length > 0)
                                {
                                    if (!string.IsNullOrEmpty(kdid[0]))
                                    {
                                        kdid1 = kdid[0].Split(',');
                                        lock (StaticClass.bjobj)
                                        {
                                            try
                                            {
                                                for (int i = 0; i < kdid1.Length; i++)
                                                {
                                                    if (!string.IsNullOrEmpty(kdid1[i]))
                                                    {
                                                        temp = long.Parse(kdid1[i]);
                                                        if (StaticClass.jcbdata.ContainsKey(temp))
                                                        {
                                                            tempobj = StaticClass.jcbdata[temp];
                                                            row = showdt.Select("id ='" + obj.ID + "' and point='" + obj.Point + "' and kzk='" + tempobj.Point + "'");
                                                            if (row.Length > 0)
                                                            {
                                                                showdt.Rows[kj]["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
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
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        lock (objShowDt)
                        {
                            if (showdt.Rows[kj].IsNull("endtime") || showdt.Rows[kj]["endtime"].ToString() == "")
                            {
                                #region  //刷新持续时间、实时值、最大d值、最小值、平均值
                                span = nowtime - Convert.ToDateTime(showdt.Rows[kj]["stime"]);
                                cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);

                                //for (int j = 0; j < row.Length; j++)
                                //{
                                //    row[j]["cxtime"] = cxtime;
                                //}
                                showdt.Rows[kj]["cxtime"] = cxtime;

                                if (!showdt.Rows[kj]["state"].ToString().Contains("断线") && !showdt.Rows[kj]["state"].ToString().Contains("上溢") && !showdt.Rows[kj]["state"].ToString().Contains("负漂"))
                                {
                                    point = showdt.Rows[kj]["point"].ToString();
                                    ssz2 = showdt.Rows[kj]["ssz"].ToString();
                                    lock (StaticClass.allPointDtLockObj)
                                    {
                                        row1 = StaticClass.AllPointDt.Select("point='" + point + "'");
                                        if (row1.Length > 0)
                                        {
                                            ssz1 = row1[0]["ssz"].ToString();
                                            if (ssz1 != ssz2)
                                            {
                                                if (double.TryParse(ssz1, out ssz))
                                                {
                                                    #region
                                                    zdz = Convert.ToDouble(showdt.Rows[kj]["zdz"]);
                                                    zxz = Convert.ToDouble(showdt.Rows[kj]["zxz"]);
                                                    pjz = Convert.ToDouble(showdt.Rows[kj]["pjz"]);
                                                    count = Convert.ToDouble(showdt.Rows[kj]["count"]);
                                                    allvlaue = Convert.ToDouble(showdt.Rows[kj]["allvalue"]);
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


                                                    showdt.Rows[kj]["zdz"] = zdz;
                                                    showdt.Rows[kj]["zxz"] = zxz;
                                                    showdt.Rows[kj]["pjz"] = pjz;
                                                    showdt.Rows[kj]["ssz"] = ssz;
                                                    showdt.Rows[kj]["count"] = count;
                                                    showdt.Rows[kj]["allvalue"] = allvlaue;

                                                    #endregion
                                                }
                                                //else
                                                //{
                                                //    for (int j = 0; j < row.Length; j++)
                                                //    {
                                                //        row[j]["cxtime"] = cxtime;
                                                //    }
                                                //}
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
                kdlist.Clear();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("修改模拟量报警记录", ex);
            }
            #endregion

            #endregion

            #region 添加记录

            if (addlist.Count > 0)
            {
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
                            lock (StaticClass.allPointDtLockObj)
                            {
                                row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (row.Length > 0)
                                {
                                    wz = row[0]["wz"];
                                    if (obj.Type == StaticClass.itemStateToClient.EqpState9 ||
                                       obj.Type == StaticClass.itemStateToClient.EqpState11)
                                    {
                                        bjz = row[0]["sxbj"] + "/" + row[0]["sxyj"];
                                        ddz = row[0]["sxdd"] + "/" + row[0]["sxfd"];
                                    }
                                    else if (obj.Type == StaticClass.itemStateToClient.EqpState19)
                                    {
                                        bjz = row[0]["xxbj"] + "/" + row[0]["xxyj"];
                                        ddz = row[0]["xxdd"] + "/" + row[0]["xxfd"];
                                    }
                                    else
                                    {
                                        bjz = "";
                                        ddz = "";
                                    }
                                    devname = row[0]["lb"];
                                    dw = row[0]["dw"];
                                }
                            }
                            fzh = obj.Fzh + "/" + obj.Kh + "/" + obj.Dzh;

                            sszn = obj.Ssz;
                            if (obj.Type == StaticClass.itemStateToClient.EqpState15 ||
                                 obj.Type == StaticClass.itemStateToClient.EqpState16)
                            {
                                obj.Ssz = "";
                            }
                            state = OprFuction.StateChange(obj.Type.ToString());
                            sbstate = OprFuction.StateChange(obj.State.ToString());
                            stime = OprFuction.TimeToString(obj.Stime);
                            span = nowtime - obj.Stime;
                            cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                            if (!OprFuction.IsInitTime(obj.Etime))
                            {
                                etime = OprFuction.TimeToString(obj.Etime);
                            }
                            zdzn = obj.Ssz;
                            zxzn = obj.Ssz;
                            pjzn = obj.Ssz;
                            ddtime = OprFuction.TimeToString(obj.Stime);
                            cs = obj.Cs;
                            cut = 1;
                            allvalue = obj.Ssz;
                            id = key;

                            if (!string.IsNullOrEmpty(obj.Kzk))
                            {
                                #region 有控制口
                                kzk = obj.Kzk.Split('|');
                                if (!string.IsNullOrEmpty(obj.Kdid))
                                {
                                    kdid = obj.Kdid.Split('|');
                                    if (kdid.Length > 0)
                                    {
                                        if (!string.IsNullOrEmpty(kdid[0]))
                                        {
                                            kdid1 = kdid[0].Split(',');
                                            lock (StaticClass.bjobj)
                                            {
                                                try
                                                {
                                                    for (int i = 0; i < kdid1.Length; i++)
                                                    {
                                                        if (!string.IsNullOrEmpty(kdid1[i]))
                                                        {
                                                            temp = long.Parse(kdid1[i]);
                                                            if (StaticClass.jcbdata.ContainsKey(temp))
                                                            {
                                                                tempobj = StaticClass.jcbdata[temp];
                                                                kzkd.Add(tempobj.Point, OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime);
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Basic.Framework.Logging.LogHelper.Error(ex);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (kzk.Length > 0)
                                {
                                    for (int i = 0; i < kzk.Length; i++)
                                    {
                                        #region 添加记录
                                        lock (StaticClass.allPointDtLockObj)
                                        {
                                            row = StaticClass.AllPointDt.Select("point='" + kzk[i] + "'");
                                            if (row.Length > 0)
                                            {
                                                qy = row[0]["wz"];
                                            }
                                        }
                                        lock (objShowDt)
                                        {
                                            r = showdt.NewRow();
                                            r["point"] = point;
                                            r["wz"] = wz;
                                            r["bjz"] = bjz;
                                            r["ddz"] = ddz;
                                            //r["fzh"] = fzh;
                                            r["ssz"] = sszn;
                                            r["state"] = state;
                                            r["sbstate"] = sbstate;
                                            r["stime"] = stime;
                                            r["cxtime"] = cxtime;
                                            r["endtime"] = etime;
                                            r["zdz"] = zdzn;
                                            r["zxz"] = zxzn;
                                            r["pjz"] = pjzn;
                                            r["ddtime"] = ddtime;
                                            if (!string.IsNullOrEmpty(cs.ToString()))
                                            {
                                                r["cs"] = cs + "/" + obj.Bz2;
                                            }
                                            else
                                            {
                                                r["cs"] = "";
                                            }
                                            r["count"] = 1;
                                            r["allvalue"] = obj.Ssz;
                                            r["id"] = key;
                                            r["qy"] = qy;
                                            r["kzk"] = kzk[i];
                                            r["devname"] = devname;
                                            r["dw"] = dw;
                                            if (kzkd.ContainsKey(kzk[i]))
                                            {
                                                r["kdtime"] = kzkd[kzk[i]];
                                            }
                                            row1 = showdt.Select("id='" + key + "' and kzk='" + kzk[i] + "'");
                                            if (row1.Length < 1)
                                            {
                                                showdt.Rows.InsertAt(r, 0);//添加新记录
                                            }
                                        }
                                        //showdt.Rows.InsertAt(r, 0);//添加新记录
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 无控制口
                                lock (objShowDt)
                                {
                                    r = showdt.NewRow();
                                    r["point"] = point;
                                    r["wz"] = wz;
                                    r["bjz"] = bjz;
                                    r["ddz"] = ddz;
                                    //r["fzh"] = fzh;
                                    r["ssz"] = sszn;
                                    r["state"] = state;
                                    r["sbstate"] = sbstate;
                                    r["stime"] = stime;
                                    r["cxtime"] = cxtime;
                                    r["endtime"] = etime;
                                    r["zdz"] = zdzn;
                                    r["zxz"] = zxzn;
                                    r["pjz"] = pjzn;
                                    if (!string.IsNullOrEmpty(cs.ToString()))
                                    {
                                        r["cs"] = cs + "/" + obj.Bz2;
                                    }
                                    else
                                    {
                                        r["cs"] = "";
                                    }
                                    r["count"] = 1;
                                    r["allvalue"] = obj.Ssz;
                                    r["id"] = key;
                                    r["devname"] = devname;
                                    r["dw"] = dw;
                                    row1 = showdt.Select("id='" + key + "'");
                                    if (row1.Length < 1)
                                    {
                                        showdt.Rows.InsertAt(r, 0);//添加新记录
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加模拟量报警记录", ex);
                }
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前断电条数
            tbcount = showdt.Select("endtime='' or endtime='1900-01-01 00:00:00'", "").Length;
            StaticClass.yccount[8] = tbcount;
            #endregion
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
        /// 获取新增报警断电信息
        /// </summary>
        public void getdata()
        {
            short type = 0;
            long id = 0;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            List<long> listkey;
            long key = 0;
            lock (StaticClass.bjobj)
            {
                #region 获取报警信息
                try
                {
                    listkey = StaticClass.jcbdata.Keys.ToList();
                    for (int kj = 0; kj < listkey.Count; kj++)
                    {
                        key = listkey[kj];

                        jcb = StaticClass.jcbdata[key];
                        type = jcb.Type;
                        if ( type == StaticClass.itemStateToClient.EqpState11 ||
                             type == StaticClass.itemStateToClient.EqpState21)
                        {
                            id = long.Parse(jcb.ID);
                            if (!jc_b.ContainsKey(id))
                            {
                                #region 新增报警断电
                                obj = OprFuction.NewDTO(jcb);
                                jc_b.Add(id, obj);
                                addlist.Add(id);
                                #endregion
                            }
                            else
                            {
                                #region 控制口改变
                                if (jc_b[id].Kzk != jcb.Kzk)//报警断电结束
                                {
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b[id] = obj;
                                    addlist.Add(id);
                                }
                                #endregion
                                #region update 报警 断电
                                if (OprFuction.IsInitTime(jc_b[id].Etime) &&
                                        !OprFuction.IsInitTime(jcb.Etime) ||
                                    jc_b[id].Kdid != jcb.Kdid)//报警断电结束
                                {
                                    obj = OprFuction.NewDTO(jcb);
                                    if (jc_b[id].Kdid != jcb.Kdid)//馈电改变
                                    {
                                        kdlist.Add(id);
                                    }
                                    jc_b[id] = obj;
                                    updatelist.Add(id);
                                }
                                else if (jc_b[id].Cs != jcb.Cs)//措施改变 //&& !OprFuction.GetClientType() 注释  20170817
                                {
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b[id] = obj;
                                    updatelist.Add(id);
                                }
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                #region 判断报警断电是否还存在

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
        private void RealBJControl_Load(object sender, EventArgs e)
        {

            inigrid();
            try
            {
                //获取服务器当前时间
                var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
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

        private void mainGrid_Click(object sender, EventArgs e)
        {

        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            object obj;
            try
            {
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
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
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
                        getdata();

                        //获取服务器当前时间
                        var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
                        MethodInvoker In = new MethodInvoker(() => refresh(nowtime));
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                    }
                    else
                    {
                        //获取服务器当前时间
                        var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
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

        private void mainGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        private void barSubItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            long id = 0; string cs = "";
            int i = 0;
            object obj;
            //popupMenu1.HidePopup();
            if (OprFuction.GetClientType())
            {
                if (mainGridView.FocusedRowHandle > -1)
                {
                    i = mainGridView.FocusedRowHandle;
                    obj = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, mainGridView.Columns["id"]);
                    if (obj != null && long.TryParse(obj.ToString(), out id))
                    {
                        if (jc_b.ContainsKey(id))
                        {
                            if (CSlist.ContainsKey(id))
                            {
                                cs = CSlist[id];
                            }
                            CSForm csf = new CSForm(id, cs);
                            csf.ShowDialog();
                            if (StaticClass.Cs != cs)
                            {
                                if (CSlist.ContainsKey(id))
                                {
                                    CSlist[id] = StaticClass.Cs;
                                }
                                else
                                {
                                    CSlist.Add(id, StaticClass.Cs);
                                }
                                lock (objShowDt)
                                {
                                    for (int ij = 0; ij < showdt.Rows.Count; ij++)
                                    {
                                        if (long.Parse(showdt.Rows[ij]["id"].ToString()) == id)
                                        {
                                            showdt.Rows[ij]["cs"] = StaticClass.Cs;
                                            break;
                                        }
                                    }
                                }
                                //mainGridView.SetRowCellValue(i, mainGridView.Columns["cs"], StaticClass.Cs);
                                Model.RealInterfaceFuction.UpdateCs(id.ToString(), jc_b[id].Stime, StaticClass.Cs);
                            }
                            StaticClass.Cs = "";
                        }
                    }
                }
            }
            else
            {
                OprFuction.MessageBoxShow(0, "该客户端不为主操作客户端，无措施录入权限！");
            }
        }

        private void barSubItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void mainGridView_Click(object sender, EventArgs e)
        {
            //OprFuction.ShowFromText(1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            long id = 0; string cs = "";
            int i = 0;
            object obj;
            //popupMenu1.HidePopup();
            if (OprFuction.GetClientType())
            {
                if (mainGridView.FocusedRowHandle > -1)
                {
                    i = mainGridView.FocusedRowHandle;
                    obj = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, mainGridView.Columns["id"]);
                    if (obj != null && long.TryParse(obj.ToString(), out id))
                    {
                        if (jc_b.ContainsKey(id))
                        {
                            if (CSlist.ContainsKey(id))
                            {
                                cs = CSlist[id];
                            }
                            CSForm csf = new CSForm(id, cs);
                            csf.ShowDialog();
                            if (StaticClass.Cs != cs)
                            {
                                if (CSlist.ContainsKey(id))
                                {
                                    CSlist[id] = StaticClass.Cs;
                                }
                                else
                                {
                                    CSlist.Add(id, StaticClass.Cs);
                                }
                                lock (objShowDt)
                                {
                                    for (int ij = 0; ij < showdt.Rows.Count; ij++)
                                    {
                                        if (long.Parse(showdt.Rows[ij]["id"].ToString()) == id)
                                        {
                                            showdt.Rows[ij]["cs"] = StaticClass.Cs;
                                        }
                                    }
                                }
                                //mainGridView.SetRowCellValue(i, mainGridView.Columns["cs"], StaticClass.Cs);
                                Model.RealInterfaceFuction.UpdateCs(id.ToString(), jc_b[id].Stime, StaticClass.Cs);
                            }
                            StaticClass.Cs = "";
                        }
                    }
                }
            }
            else
            {
                OprFuction.MessageBoxShow(0, "该客户端不为主操作客户端，无措施录入权限！");
            }
        }
        /// <summary>
        /// 测点筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //加窗体
            DataTable _dtSelectItem = new DataTable();
            _dtSelectItem.Columns.Add("Check", typeof(bool));
            _dtSelectItem.Columns.Add("Id");
            _dtSelectItem.Columns.Add("Text");
            foreach (DataRow dr in showdt.Rows)
            {
                if (_dtSelectItem.Select("Id='" + dr["point"].ToString() + "'").Length < 1)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = dr["point"];
                    row["Text"] = dr["point"].ToString() + ":" + dr["wz"].ToString() + "[" + dr["devname"].ToString() + "]";
                    _dtSelectItem.Rows.Add(row);
                }
            }
            var copyData = new List<string>();
            var selectForm = new LinkageItemSelect("测点选择", "测点号", _dtSelectItem, copyData);

            var res = selectForm.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }

            _masterSelectedPoint = ObjectConverter.DeepCopy(selectForm.SelectedIds);

            if (_masterSelectedPoint.Count > 0)
            {
                DataTable dtShowCopy = showdt.Clone();
                foreach (DataRow dr in showdt.Rows)
                {
                    if (_masterSelectedPoint.Contains(dr["point"]))
                    {
                        dtShowCopy.Rows.Add(dr.ItemArray);
                    }
                }
                mainGrid.DataSource = dtShowCopy;
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mainGrid.DataSource = showdt;
        }

        private void mainGridView_MouseDown(object sender, MouseEventArgs e)
        {
              GridHitInfo hInfo = mainGridView.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    var alarmId = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "id");
                    if (alarmId != null)
                    {
                        string _alarmId = alarmId.ToString();
                        var alarmtime = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "stime");
                        string alarmtimeStr = (DateTime.Parse(alarmtime.ToString())).ToString("yyyyMM");

                        frmAlarmProcessDetail frmDetai = new frmAlarmProcessDetail(1, _alarmId, alarmtimeStr);

                        if (frmDetai.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                           
                        }
                    }
                }
            }
        }
    }
}
