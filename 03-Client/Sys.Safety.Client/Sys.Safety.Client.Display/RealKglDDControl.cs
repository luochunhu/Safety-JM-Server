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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Sys.Safety.Client.Alarm;

namespace Sys.Safety.Client.Display
{
    public partial class RealKglDDControl : XtraForm
    {
        public RealKglDDControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }

        #region======================开关量报警逻辑======================

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
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","当前值","设备状态",
            "开始时间","持续时间","报警值","断电状态/时刻",
            "断电值","区域","馈电异常/时刻","措施/时刻","kzk","id","endtime"};//"分站/通道/地址",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz",
            "ssz","sbstate", "stime","cxtime","bjz","ddtime","ddz","qy","kdtime","cs","kzk","id" ,"endtime"};//"fzh",

        public int[] colwith = new int[] { 60, 160,  50, 80, 130, 80, 110, 130, 110, 160, 130, 130, 80, 80, 80 };

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
                if (colname[i] == "kzk" || colname[i] == "id" || colname[i] == "endtime")
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

        /// <summary>
        /// 刷新报警数据
        /// </summary>
        public void refresh(DateTime nowtime)
        {
            string point = "";
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
            List<long> listkey;
            Dictionary<string, string> kzkd = new Dictionary<string, string>();
            object wz = "", bjz = "", ddz = "", fzh = "", sszn = "", state = "", stime = "", etime = "",
                cxtime = "", ddtime = "", cs = "", cut = "", allvalue = "", idn = "", qy = "", sbstate = "";

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
                OprFuction.SaveErrorLogs("删除开关量报警断电记录", ex);
            }
            #endregion

            #region 修改及刷新记录
            //foreach (long key in jc_b.Keys)
            //{
            //try
            //{
            //    listkey = jc_b.Keys.ToList();
            //    for (int kj = 0; kj < listkey.Count; kj++)
            //    {
            //        key = listkey[kj];
            //        if (updatelist.Contains(key))
            //        {
            //            row = showdt.Select("id='" + key + "'");
            //            if (row.Length > 0)
            //            {
            //                #region 刷新  结束时间 持续时间
            //                if (row[0]["endtime"].ToString() == "")//结束时间更新
            //                {
            //                    for (int i = 0; i < row.Length; i++)
            //                    {
            //                        #region 刷新 结束时间 持续时间

            //                        obj = jc_b[key];
            //                        if (!OprFuction.IsInitTime(obj.Etime))
            //                        {
            //                            row[i]["endtime"] = OprFuction.TimeToString(obj.Etime);
            //                            span = obj.Etime - Convert.ToDateTime(row[i]["stime"]);
            //                            row[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
            //                        }
            //                        else
            //                        {
            //                            span = nowtime - Convert.ToDateTime(row[i]["stime"]);
            //                            row[i]["cxtime"] = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
            //                        }
            //                        #endregion
            //                    }
            //                }
            //                #endregion

            //                #region 刷新cs
            //                if (row[0]["cs"].ToString() != jc_b[key].Cs && !OprFuction.GetClientType())
            //                {
            //                    for (int i = 0; i < row.Length; i++)
            //                    {
            //                        #region 刷新 cs
            //                        row[i]["cs"] = jc_b[key];
            //                        #endregion
            //                    }
            //                }
            //                #endregion
            //            }

            //            #region 修改馈电
            //            if (kdlist.Contains(key))
            //            {
            //                obj = jc_b[key];
            //                kdid = obj.Kdid.Split('|');
            //                if (kdid.Length > 0)
            //                {
            //                    if (!string.IsNullOrEmpty(kdid[0]))
            //                    {
            //                        kdid1 = kdid[0].Split(',');
            //                        lock (StaticClass.bjobj)
            //                        {
            //                            try
            //                            {
            //                                for (int i = 0; i < kdid1.Length; i++)
            //                                {
            //                                    if (!string.IsNullOrEmpty(kdid1[i]))
            //                                    {
            //                                        temp = long.Parse(kdid1[i]);
            //                                        if (StaticClass.jcbdata.ContainsKey(temp))
            //                                        {
            //                                            tempobj = StaticClass.jcbdata[temp];
            //                                            row = showdt.Select("id ='" + obj.ID + "' and  point='" + obj.Point + "' and kzk='" + tempobj.Point + "'");
            //                                            if (row.Length > 0)
            //                                            {
            //                                                row[0]["ddtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                Basic.Framework.Logging.LogHelper.Error(ex);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion
            //        }
            //        else
            //        {
            //            row = showdt.Select("id='" + key + "'");
            //            if (row.Length > 0)
            //            {
            //                if (row[0]["endtime"].ToString() == "")
            //                {
            //                    #region  //刷新持续时间
            //                    span = nowtime - Convert.ToDateTime(row[0]["stime"]);
            //                    cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
            //                    for (int j = 0; j < row.Length; j++)
            //                    {
            //                        row[j]["cxtime"] = cxtime;
            //                    }
            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    OprFuction.SaveErrorLogs("修改开关量报警记录", ex);
            //}

            #region//重写刷新方法，每次刷新会导致客户端卡死  20170719
            try
            {
                listkey = jc_b.Keys.ToList();
                for (int kj = 0; kj < showdt.Rows.Count; kj++)
                {
                    key = long.Parse(showdt.Rows[kj]["id"].ToString());
                    if (updatelist.Contains(key))
                    {
                        #region 刷新  结束时间 持续时间
                        if (showdt.Rows[kj]["endtime"].ToString() == "")//结束时间更新
                        {
                            #region 刷新 结束时间 持续时间
                            obj = jc_b[key];
                            if (!OprFuction.IsInitTime(obj.Etime))
                            {
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
                                                        row = showdt.Select("id ='" + obj.ID + "' and  point='" + obj.Point + "' and kzk='" + tempobj.Point + "'");
                                                        if (row.Length > 0)
                                                        {
                                                            showdt.Rows[kj]["ddtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
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
                    else
                    {
                        if (showdt.Rows[kj]["endtime"].ToString() == "")
                        {
                            #region  //刷新持续时间
                            span = nowtime - Convert.ToDateTime(showdt.Rows[kj]["stime"]);
                            cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                            showdt.Rows[kj]["cxtime"] = cxtime;
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("修改开关量报警记录", ex);
            }
            #endregion


            //}
            updatelist.Clear();
            kdlist.Clear();
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
                            bjz = sszn;
                            ddz = sszn;
                            state = OprFuction.StateChange(obj.Type.ToString());
                            sbstate = OprFuction.StateChange(obj.State.ToString());
                            stime = OprFuction.TimeToString(obj.Stime);
                            span = nowtime - obj.Stime;
                            cxtime = String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", span.Days, span.Hours, span.Minutes, span.Seconds);
                            etime = ""; //by
                            if (!OprFuction.IsInitTime(obj.Etime))
                            {
                                etime = OprFuction.TimeToString(obj.Etime);
                            }
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
                                            r["sbstate"] = sbstate;
                                            //r["state"] = state;
                                            r["stime"] = stime;
                                            r["cxtime"] = cxtime;
                                            r["endtime"] = etime;
                                            r["ddtime"] = ddtime;
                                            if (!string.IsNullOrEmpty(cs.ToString()))
                                            {
                                                r["cs"] = cs + "/" + obj.Bz2;
                                            }
                                            else {
                                                r["cs"] = "";
                                            }
                                            //r["count"] = 1;
                                            r["id"] = key;
                                            r["qy"] = qy;
                                            r["kzk"] = kzk[i];
                                            if (kzkd.ContainsKey(kzk[i]))
                                            {
                                                r["ddtime"] = kzkd[kzk[i]];
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
                                    //r["fzh"] = fzh;
                                    r["ssz"] = sszn;
                                    r["sbstate"] = sbstate;
                                    //r["state"] = state;
                                    r["stime"] = stime;
                                    r["cxtime"] = cxtime;
                                    r["endtime"] = etime;
                                    if (!string.IsNullOrEmpty(cs.ToString()))
                                    {
                                        r["cs"] = cs + "/" + obj.Bz2;
                                    }
                                    else
                                    {
                                        r["cs"] = "";
                                    }
                                    //r["count"] = 1;
                                    //r["allvalue"] = obj.Ssz;
                                    r["id"] = key;
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
                    OprFuction.SaveErrorLogs("添加开关量报警记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[2] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前报警条数
            tbcount = showdt.Select("endtime='' or endtime='1900-01-01 00:00:00'", "").Length;
            StaticClass.yccount[2] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增报警断电信息
        /// </summary>
        public void getdata()
        {
            short type = 0;
            int count = 0;
            long id = 0;
            List<long> keylist;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            lock (StaticClass.bjobj)
            {
                try
                {
                    #region 获取报警信息
                    keylist = new List<long>(StaticClass.jcbdata.Keys);
                    for (int i = 0; i < keylist.Count; i++)
                    {
                        if (StaticClass.jcbdata.ContainsKey(keylist[i]))
                        {
                            jcb = StaticClass.jcbdata[keylist[i]];
                            type = jcb.Type;
                            if (type == 27)
                            {

                            }
                            if (type == StaticClass.itemStateToClient.EqpState24 || type == StaticClass.itemStateToClient.EqpState25 ||
                                type == StaticClass.itemStateToClient.EqpState26)
                            {
                                id = long.Parse(jcb.ID);
                                if (!jc_b.ContainsKey(id) && ( !string.IsNullOrEmpty(jcb.Kzk)))//勾选了控制的进行显示  20170726
                                {
                                    #region 新增报警断电
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b.Add(id, obj);
                                    addlist.Add(id);
                                    #endregion
                                }
                                else
                                {
                                    #region update 报警 断电
                                    if (jc_b.ContainsKey(id))
                                    {
                                        #region 控制口改变
                                        if (jc_b[id].Kzk != jcb.Kzk)//报警断电结束
                                        {
                                            obj = OprFuction.NewDTO(jcb);
                                            jc_b[id] = obj;
                                            addlist.Add(id);
                                        }
                                        #endregion

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
                                        else if (jc_b[id].Cs != jcb.Cs)//措施改变//&& !OprFuction.GetClientType() 注释  20170817
                                        {
                                            obj = OprFuction.NewDTO(jcb);
                                            jc_b[id] = obj;
                                            updatelist.Add(id);
                                        }
                                        //else if (jc_b[id].Isalarm != jcb.Isalarm)
                                        //{
                                        //    //2017.7.14 by
                                        //    obj = OprFuction.NewDTO(jcb);
                                        //    jc_b[id] = obj;
                                        //    updatelist.Add(id);
                                        //}
                                        else//无变化，直接更新结束时间  20170627
                                        {
                                            updatelist.Add(id);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    #region 判断报警断电是否还存在
                    foreach (long key in jc_b.Keys)
                    {
                        if (!StaticClass.jcbdata.ContainsKey(key) || (StaticClass.jcbdata[key].Isalarm == 0 &&string.IsNullOrEmpty( StaticClass.jcbdata[key].Kzk)))//如果用户取消报警(只有报警，没有控制时，可取消)，此时数据处理会将标记置成正常，客户端显示的时候也取消报警  20170627
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

        private void RealKglBjControl_Load(object sender, EventArgs e)
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

        private void mainGridView_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    popupMenu1.ShowPopup(Control.MousePosition);
            //}
        }

        private void barSubItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            long id = 0; string cs = "";
            object obj;
            if (OprFuction.GetClientType())
            {
                if (mainGridView.FocusedRowHandle > -1)
                {
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

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void mainGridView_Click(object sender, EventArgs e)
        {
            //OprFuction.ShowFromText(2);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            long id = 0; string cs = "";
            object obj;
            if (OprFuction.GetClientType())
            {
                if (mainGridView.FocusedRowHandle > -1)
                {
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
