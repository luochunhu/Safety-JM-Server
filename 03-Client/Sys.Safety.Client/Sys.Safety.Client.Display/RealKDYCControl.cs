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
    public partial class RealKDYCControl : XtraForm
    {
        public RealKDYCControl()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            InitializeComponent();
        }
        #region=====================馈电异常逻辑======================

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;

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
        public List<long> updatelist = new List<long>();

        /// <summary>
        /// 需要修改的id号 措施
        /// </summary>
        public List<long> cslist = new List<long>();

        /// <summary>
        /// 馈电数据源
        /// </summary>
        public Dictionary<long, Jc_BInfo> jc_kd = new Dictionary<long, Jc_BInfo>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","安装位置","设备状态","单位","当前值及时刻",
            "控制口","报警/解警值","断电/复电值",
            "报警及时刻","断电及时刻","断电区域","馈电异常/时刻","措施/时刻","kdid","id"};

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","sbstate","dw",
            "ssz", "kzk","bjz","ddz","bjtime","ddtime","qy","kdtime","cs","kdid","id"};

        public int[] colwith = new int[] { 60, 160, 60, 40, 140, 60, 80, 130, 130, 160, 130, 80, 80, 80, 80 };

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
                if (colname[i] == "kdid" || colname[i] == "id")
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
            DataRow[] row;
            DataRow r;
            int countn = 0;
            string[] kzk = null, kdid = null;
            long temp = 0;
            Jc_BInfo obj = null, tempobj = null;
            int tbcount = 0;
            Dictionary<string, string> kzkd = new Dictionary<string, string>();
            object wz = "", dw = "", bjz = "", ddz = "", fzh = "", sszn = "",
                bjtime = "", ddtime = "", cs = "", idn = "", qy = "", sbstate = "";
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
                                    if (jc_kd.ContainsKey(long.Parse(showdt.Rows[i]["kdid"].ToString())))
                                    {
                                        jc_kd.Remove(long.Parse(showdt.Rows[i]["kdid"].ToString()));
                                    }

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
                OprFuction.SaveErrorLogs("删除馈电记录", ex);
            }
            #endregion

            #region 修改及刷新记录
            if (updatelist.Count > 0)
            {
                try
                {
                    #region 添加记录
                    for (int kj = 0; kj < updatelist.Count; kj++)
                    {
                        key = updatelist[kj];
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
                                    sbstate = OprFuction.StateChange(obj.State.ToString());
                                    sszn = obj.Ssz + "/" + OprFuction.TimeToString(obj.Stime);
                                    if (int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState9 ||
                                        int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState11)
                                    {
                                        bjz = row[0]["sxbj"] + "/" + row[0]["sxyj"];
                                        ddz = row[0]["sxdd"] + "/" + row[0]["sxfd"];
                                        dw = row[0]["dw"];
                                    }
                                    else if (int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState19 ||
                                        int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState21)
                                    {
                                        bjz = row[0]["xxbj"] + "/" + row[0]["xxyj"];
                                        ddz = row[0]["xxdd"] + "/" + row[0]["xxfd"];
                                        dw = row[0]["dw"];
                                    }
                                    else
                                    {
                                        bjz = obj.Ssz;
                                        ddz = obj.Ssz;
                                    }
                                }
                            }
                            fzh = obj.Fzh + "/" + obj.Kh + "/" + obj.Dzh;
                            bjtime = ddtime = OprFuction.TimeToString(obj.Stime);
                            cs = obj.Cs;
                            idn = key;

                            #region 有控制口
                            //if (!string.IsNullOrEmpty(obj.Kdid))
                            //{
                            //    kdid = obj.Kdid.Split('|');
                            //    if (kdid.Length > 0)
                            //    {
                            //        if (!string.IsNullOrEmpty(kdid[0]))
                            //        {
                            //            kdid1 = kdid[0].Split(',');
                            //            #region 断电失败
                            //            lock (StaticClass.bjobj)
                            //            {
                            //                for (int i = 0; i < kdid1.Length; i++)
                            //                {
                            //                    temp = long.Parse(kdid1[i]);
                            //                    if (StaticClass.jcbdata.ContainsKey(temp))
                            //                    {
                            //                        tempobj = StaticClass.jcbdata[temp];
                            //                        if (!jc_kd.ContainsKey(temp))
                            //                        {
                            //                            jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                            //                            #region 添加记录
                            //                            row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
                            //                            if (row.Length > 0)
                            //                            {
                            //                                qy = row[0]["wz"];
                            //                            }
                            //                            r = showdt.NewRow();
                            //                            r["point"] = point;
                            //                            r["wz"] = wz;
                            //                            r["bjz"] = bjz;
                            //                            r["ddz"] = ddz;
                            //                            r["dw"] = dw;
                            //                            //r["fzh"] = fzh;
                            //                            r["ssz"] = sszn;
                            //                            r["bjtime"] = bjtime;
                            //                            r["ddtime"] = ddtime;
                            //                            r["kdid"] = temp;
                            //                            r["id"] = key;
                            //                            r["qy"] = qy;
                            //                            r["kzk"] = tempobj.Point;
                            //                            r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                            //                            showdt.Rows.InsertAt(r, 0);//添加新记录
                            //                            #endregion
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            #endregion
                            //        }
                            //        if (kdid.Length > 1)
                            //        {
                            //            #region 复电失败
                            //            if (!string.IsNullOrEmpty(kdid[1]))
                            //            {
                            //                kdid1 = kdid[1].Split(',');
                            //                lock (StaticClass.bjobj)
                            //                {
                            //                    for (int i = 0; i < kdid1.Length; i++)
                            //                    {
                            //                        temp = long.Parse(kdid1[i]);
                            //                        if (StaticClass.jcbdata.ContainsKey(temp))
                            //                        {
                            //                            tempobj = StaticClass.jcbdata[temp];
                            //                            if (!jc_kd.ContainsKey(temp))
                            //                            {
                            //                                jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                            //                                #region 添加记录
                            //                                row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
                            //                                if (row.Length > 0)
                            //                                {
                            //                                    qy = row[0]["wz"];
                            //                                }
                            //                                r = showdt.NewRow();
                            //                                r["point"] = point;
                            //                                r["wz"] = wz;
                            //                                r["bjz"] = bjz;
                            //                                r["ddz"] = ddz;
                            //                                r["dw"] = dw;
                            //                                //r["fzh"] = fzh;
                            //                                r["ssz"] = sszn;
                            //                                r["bjtime"] = bjtime;
                            //                                r["ddtime"] = ddtime;
                            //                                r["kdid"] = temp;
                            //                                r["id"] = key;
                            //                                r["qy"] = qy;
                            //                                r["kzk"] = tempobj.Point;
                            //                                r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                            //                                showdt.Rows.InsertAt(r, 0);//添加新记录
                            //                                #endregion
                            //                            }
                            //                        }
                            //                    }
                            //                }         
                            //            }
                            //            #endregion
                            //        }
                            //    }
                            //}
                            #endregion

                            #region 有控制口
                            if (!string.IsNullOrEmpty(obj.Kdid))
                            {
                                kdid = obj.Kdid.Split(',');
                                if (kdid.Length > 0)
                                {
                                    #region 馈电异常

                                    lock (StaticClass.bjobj)
                                    {
                                        try
                                        {
                                            for (int i = 0; i < kdid.Length; i++)
                                            {
                                                if (!string.IsNullOrEmpty(kdid[i]))
                                                {
                                                    temp = long.Parse(kdid[i]);
                                                    if (StaticClass.jcbdata.ContainsKey(temp))
                                                    {
                                                        tempobj = StaticClass.jcbdata[temp];
                                                        if (!jc_kd.ContainsKey(temp))//
                                                        {
                                                            jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                                                            #region 添加记录
                                                            lock (StaticClass.allPointDtLockObj)
                                                            {
                                                                row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
                                                                if (row.Length > 0)
                                                                {
                                                                    qy = row[0]["wz"];
                                                                }
                                                            }
                                                            lock (objShowDt)
                                                            {
                                                                r = showdt.NewRow();
                                                                r["point"] = point;
                                                                r["sbstate"] = sbstate;
                                                                r["wz"] = wz;
                                                                r["bjz"] = bjz;
                                                                r["ddz"] = ddz;
                                                                r["dw"] = dw;
                                                                //r["fzh"] = fzh;
                                                                r["ssz"] = sszn;
                                                                r["bjtime"] = bjtime;
                                                                r["ddtime"] = ddtime;
                                                                r["kdid"] = temp;
                                                                r["id"] = key;
                                                                r["qy"] = qy;
                                                                r["kzk"] = tempobj.Point;
                                                                r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                                                                r["cs"] = cs;
                                                                showdt.Rows.InsertAt(r, 0);//添加新记录
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {//措施改变之后，对对应的记录更新   20180206
                                                            for (int k = 0; k < showdt.Rows.Count; k++)
                                                            {
                                                                if (showdt.Rows[k]["id"].ToString() == key.ToString())
                                                                {
                                                                    showdt.Rows[k]["cs"] = cs;                                                                    
                                                                }
                                                            }                                                           
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
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加馈电记录1", ex);
                }
                updatelist.Clear();
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
                            lock (StaticClass.allPointDtLockObj)
                            {
                                row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                if (row.Length > 0)
                                {
                                    wz = row[0]["wz"];
                                    sbstate = OprFuction.StateChange(obj.State.ToString());
                                    sszn = obj.Ssz + "/" + OprFuction.TimeToString(obj.Stime);
                                    if (int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState9 ||
                                       int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState11)
                                    {
                                        bjz = row[0]["sxbj"] + "/" + row[0]["sxyj"];
                                        ddz = row[0]["sxdd"] + "/" + row[0]["sxfd"];
                                        dw = row[0]["dw"];
                                    }
                                    else if (int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState19 ||
                                        int.Parse(obj.Type.ToString()) == StaticClass.itemStateToClient.EqpState21)
                                    {
                                        bjz = row[0]["xxbj"] + "/" + row[0]["xxyj"];
                                        dw = row[0]["dw"];
                                        ddz = row[0]["xxdd"] + "/" + row[0]["xxfd"];
                                    }
                                    else
                                    {
                                        bjz = obj.Ssz;
                                        ddz = obj.Ssz;
                                    }
                                }
                            }
                            fzh = obj.Fzh + "/" + obj.Kh + "/" + obj.Dzh;
                            bjtime = ddtime = OprFuction.TimeToString(obj.Stime);
                            cs = obj.Cs;
                            idn = key;
                            #region 有控制口
                            //if (!string.IsNullOrEmpty(obj.Kdid))
                            //{
                            //    kdid = obj.Kdid.Split('|');
                            //    if (kdid.Length > 0)
                            //    {
                            //        if (!string.IsNullOrEmpty(kdid[0]))
                            //        {
                            //            kdid1 = kdid[0].Split(',');
                            //            #region 断电失败
                            //            lock (StaticClass.bjobj)
                            //            {
                            //                for (int i = 0; i < kdid1.Length; i++)
                            //                {
                            //                    temp = long.Parse(kdid1[i]);
                            //                    if (StaticClass.jcbdata.ContainsKey(temp))
                            //                    {
                            //                        tempobj = StaticClass.jcbdata[temp];
                            //                        if (!jc_kd.ContainsKey(temp))
                            //                        {
                            //                            jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                            //                        }
                            //                        #region 添加记录
                            //                        row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
                            //                        if (row.Length > 0)
                            //                        {
                            //                            qy = row[0]["wz"];
                            //                        }
                            //                        r = showdt.NewRow();
                            //                        r["point"] = point;
                            //                        r["wz"] = wz;
                            //                        r["bjz"] = bjz;
                            //                        r["ddz"] = ddz;
                            //                        r["dw"] = dw;
                            //                        //r["fzh"] = fzh;
                            //                        r["ssz"] = sszn;
                            //                        r["bjtime"] = bjtime;
                            //                        r["ddtime"] = ddtime;
                            //                        r["kdid"] = temp;
                            //                        r["id"] = key;
                            //                        r["qy"] = qy;
                            //                        r["kzk"] = tempobj.Point;
                            //                        r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                            //                        showdt.Rows.InsertAt(r, 0);//添加新记录
                            //                        #endregion
                            //                    }
                            //                }
                            //            }
                            //            #endregion
                            //        }
                            //        if (kdid.Length>1)
                            //        {
                            //            #region 复电失败
                            //            if (!string.IsNullOrEmpty(kdid[1]))
                            //            {
                            //                kdid1 = kdid[1].Split(',');
                            //                lock (StaticClass.bjobj)
                            //                {
                            //                    for (int i = 0; i < kdid1.Length; i++)
                            //                    {
                            //                        temp = long.Parse(kdid1[i]);
                            //                        if (StaticClass.jcbdata.ContainsKey(temp))
                            //                        {
                            //                            tempobj = StaticClass.jcbdata[temp];
                            //                            if (!jc_kd.ContainsKey(temp))
                            //                            {
                            //                                jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                            //                            }
                            //                            #region 添加记录
                            //                            row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
                            //                            if (row.Length > 0)
                            //                            {
                            //                                qy = row[0]["wz"];
                            //                            }
                            //                            r = showdt.NewRow();
                            //                            r["point"] = point;
                            //                            r["wz"] = wz;
                            //                            r["bjz"] = bjz;
                            //                            r["ddz"] = ddz;
                            //                            r["dw"] = dw;
                            //                            //r["fzh"] = fzh;
                            //                            r["ssz"] = sszn;
                            //                            r["bjtime"] = bjtime;
                            //                            r["ddtime"] = ddtime;
                            //                            r["kdid"] = temp;
                            //                            r["id"] = key;
                            //                            r["qy"] = qy;
                            //                            r["kzk"] = tempobj.Point;
                            //                            r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                            //                            showdt.Rows.InsertAt(r, 0);//添加新记录
                            //                        }
                            //                    }
                            //                }
                            //                #endregion
                            //            }
                            //            #endregion
                            //        }
                            //    }
                            //}
                            #endregion


                            #region 有控制口
                            if (!string.IsNullOrEmpty(obj.Kdid))
                            {
                                kdid = obj.Kdid.Split(',');
                                if (kdid.Length > 0)
                                {
                                    #region 馈电异常
                                    lock (StaticClass.bjobj)
                                    {
                                        try
                                        {
                                            for (int i = 0; i < kdid.Length; i++)
                                            {
                                                if (!string.IsNullOrEmpty(kdid[i]))
                                                {
                                                    temp = long.Parse(kdid[i]);
                                                    if (StaticClass.jcbdata.ContainsKey(temp))
                                                    {
                                                        tempobj = StaticClass.jcbdata[temp];
                                                        if (!jc_kd.ContainsKey(temp))
                                                        {
                                                            jc_kd.Add(temp, OprFuction.NewDTO(tempobj));
                                                        }
                                                        #region 添加记录
                                                        lock (StaticClass.allPointDtLockObj)
                                                        {
                                                            row = StaticClass.AllPointDt.Select("point='" + tempobj.Point + "'");
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
                                                            r["sbstate"] = sbstate;
                                                            r["bjz"] = bjz;
                                                            r["ddz"] = ddz;
                                                            r["dw"] = dw;
                                                            //r["fzh"] = fzh;
                                                            r["ssz"] = sszn;
                                                            r["bjtime"] = bjtime;
                                                            r["ddtime"] = ddtime;
                                                            r["kdid"] = temp;
                                                            r["id"] = key;
                                                            r["qy"] = qy;
                                                            r["kzk"] = tempobj.Point;
                                                            r["kdtime"] = OprFuction.StateChange(tempobj.Type.ToString()) + "/" + tempobj.Stime;
                                                            r["cs"] = cs;
                                                            showdt.Rows.InsertAt(r, 0);//添加新记录
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
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    OprFuction.SaveErrorLogs("添加馈电记录", ex);
                }
                //tbcount = showdt.Rows.Count - tbcount;
                //StaticClass.yccount[3] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前报警条数
            tbcount = showdt.Rows.Count;
            StaticClass.yccount[3] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增报警断电信息
        /// </summary>
        public void getdata()
        {
            long id = 0;
            Jc_BInfo jcb = null;
            Jc_BInfo obj = null;
            long key = 0;
            List<long> listkey;
            lock (StaticClass.bjobj)
            {
                try
                {
                    #region 获取馈电异常信息
                    listkey = StaticClass.jcbdata.Keys.ToList();
                    for (int kj = 0; kj < listkey.Count; kj++)
                    {
                        key = listkey[kj];
                        jcb = StaticClass.jcbdata[key];
                        if (!string.IsNullOrEmpty(jcb.Kdid))
                        {
                            id = long.Parse(jcb.ID);
                            if (!jc_b.ContainsKey(id))
                            {
                                #region 新增报警断电馈电
                                obj = OprFuction.NewDTO(jcb);
                                jc_b.Add(id, obj);
                                addlist.Add(id);
                                #endregion
                            }
                            else
                            {
                                #region update 报警 断电
                                if (jc_b[id].Kdid != jcb.Kdid)
                                {
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b[id] = obj;
                                    updatelist.Add(id);
                                }
                                else if (jc_b[id].Cs != jcb.Cs)//措施改变进行页面更新   20180206
                                {
                                    obj = OprFuction.NewDTO(jcb);
                                    jc_b[id] = obj;
                                    updatelist.Add(id);
                                }
                                #endregion
                            }
                        }
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
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
            }
        }

        public void datarun()
        {
            //for (; ; )
            //{
            //    try
            //    {
            //        getdata();
            //        refresh();
            //    }
            //    catch(Exception ex)
            //    {
            //        Basic.Framework.Logging.LogHelper.Error(ex.ToString());
            //    }
            //    Thread.Sleep(2000);
            //}
        }

        #endregion

        private void RealKDYCControl_Load(object sender, EventArgs e)
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
                    var nowtime = Model.RealInterfaceFuction.GetServerNowTime();
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
        private void mainGridView_Click(object sender, EventArgs e)
        {
            //OprFuction.ShowFromText(3);
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
                        var alarmtime = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "ddtime");
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
