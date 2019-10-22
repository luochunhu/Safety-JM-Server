using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using System.Threading;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Chart;

namespace Sys.Safety.Client.Display
{
    public partial class FzShowForm : XtraForm
    {
        private Thread freshthread;
        private bool _isRun = false;
        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        IAllSystemPointDefineService allSystemPointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();
        IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        public FzShowForm(string point)
        {
            InitializeComponent();
            try
            {
                objfz.point = point;
                lock (StaticClass.allPointDtLockObj)
                {
                    DataRow[] rows = StaticClass.AllPointDt.Select("point='" + objfz.point + "'");
                    if (rows.Length > 0)
                    {
                        objfz.fzh = int.Parse(rows[0]["fzh"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private ShowFZ objfz = new ShowFZ();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        private string[] colname = new string[] { "测点编号","安装位置","类型","当前值","数据状态","电压/电量",
            "上限预警值","上限报警值","上限断电值","上限复电值",
            "下限预警值","下限报警值","下限断电值","下限复电值"};
        public int[] colwith = new int[] { 80, 200, 120, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };

        /// <summary>
        /// 列表显示名称
        /// </summary>
        private string[] colnamekz = new string[] { "主控点","主控点类型","安装位置","控制类型",
            "状态","被控测点","被控测点位置"};



        /// <summary>
        /// 初始显示表
        /// </summary>
        private void inigrid()
        {
            GridColumn col;
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = objfz.tcolname[i];
                col.Width = colwith[i];
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = objfz.showdt;
        }


        /// <summary>
        /// 初始显示表
        /// </summary>
        private void inigrid1()
        {
            GridColumn col;
            for (int i = 0; i < colnamekz.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colnamekz[i];
                col.FieldName = objfz.tcolnamekz[i];
                col.Width = 80;
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                gridView1.Columns.Add(col);
            }
            mainGrid.DataSource = objfz.showdtkz;
        }

        /// <summary>
        /// 获取点号
        /// </summary>
        private void getpoint()
        {
            DataRow[] rows;
            DataTable dt = new DataTable();
            cmb_fzadr.Properties.Items.Clear();
            try
            {
                if (StaticClass.AllPointDt.Rows.Count > 0)
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("lx='分站'", "fzh");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow r in rows)
                            {
                                cmb_fzadr.Properties.Items.Add(r["fzh"].ToString().PadLeft(3, '0'));
                            }
                            if (objfz.fzh != 0)
                            {
                                for (int i = 0; i < cmb_fzadr.Properties.Items.Count; i++)
                                {
                                    if (objfz.fzh.ToString().PadLeft(3, '0') == cmb_fzadr.Properties.Items[i].ToString())
                                    {
                                        cmb_fzadr.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                cmb_fzadr.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("分站运行界面，测点获取", ex);
            }
        }

        /// <summary>
        /// 获取显示数据
        /// </summary>
        private void getmsg(Jc_DefInfo def, DataTable dt)
        {
            DataRow[] rows;
            DataRow row;
            float ssz = 0;

            if (objfz.fzh != 0)
            {
                PointDefineGetByStationIDRequest PointDefineRequest = new PointDefineGetByStationIDRequest();
                PointDefineRequest.StationID = objfz.fzh;
                //List<Jc_DefInfo> FzList = pointDefineService.GetPointDefineCacheByStationID(PointDefineRequest).Data;
                //多系统融合修改  20171123              
                List<Jc_DefInfo> FzList = allSystemPointDefineService.GetPointDefineCacheByStationID(PointDefineRequest).Data;
                Jc_DefInfo FzInfo = FzList.Find(a => a.DevPropertyID == 0);
                Jc_DevInfo FzDev = new Jc_DevInfo();
                if (FzInfo != null)
                {

                    if (FzInfo != null)
                    {
                        DeviceDefineGetByDevIdRequest DeviceDefineRequest = new DeviceDefineGetByDevIdRequest();
                        DeviceDefineRequest.DevId = FzInfo.Devid;
                        FzDev = deviceDefineService.GetDeviceDefineCacheByDevId(DeviceDefineRequest).Data;
                    }
                }
                if (objfz.showdt != null)
                {
                    objfz.showdtrowscount = objfz.showdt.Rows.Count;
                    objfz.topindex1 = mainGridView.TopRowIndex;
                    if (mainGridView.FocusedColumn != null)
                    {
                        objfz.showdtfocusrowid = mainGridView.FocusedRowHandle;
                        objfz.showdtfocuscolid = mainGridView.FocusedColumn.ColumnHandle;
                    }
                }
                if (objfz.showdtkz != null)
                {
                    objfz.showdtkzrowscount = objfz.showdtkz.Rows.Count;
                    objfz.topindex2 = gridView1.TopRowIndex;
                    if (gridView1.FocusedColumn != null)
                    {
                        objfz.showdtkzfocuscolid = gridView1.FocusedColumn.ColumnHandle;
                        objfz.showdtkzfocusrowid = gridView1.FocusedRowHandle;
                    }
                }
                objfz.clear();
                lock (StaticClass.allPointDtLockObj)
                {
                    rows = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and lx='分站'");
                    if (rows.Length > 0)
                    {
                        objfz.point = rows[0]["point"].ToString();

                        if (def != null)
                        {
                            if (FzDev != null && FzDev.LC2 == 13)//这几种分站都支持电量显示
                            {
                                objfz.power = def.Voltage.ToString();
                                labelControl7.Text = "电量";
                            }
                            else if (FzDev != null && (FzDev.LC2 == 12 || FzDev.LC2 == 15))
                            {
                                objfz.power = def.Voltage.ToString();
                                labelControl7.Text = "电压";
                            }
                            else
                            {
                                objfz.power = "-";
                            }
                            objfz.wz = def.Wz;
                            objfz.type = def.DevName;
                            rows = StaticClass.AllPointDt.Select("point='" + objfz.point + "'");
                            if (rows.Length > 0)
                            {
                                objfz.sszcolor = rows[0]["sszcolor"].ToString();
                            }
                            else
                            {
                                objfz.sszcolor = Color.Blue.ToArgb().ToString();
                            }

                            objfz.mac = def.Jckz1;
                            objfz.ip = def.Jckz2;
                            objfz.ssz = OprFuction.StateChange(def.State.ToString());

                            objfz.stationDyType = rows[0]["StationDyType"].ToString() == "1" ? "通讯故障" : "通讯正常";
                        }

                        //rows = StaticClass.AllPointDt.Select("fzh=" + objfz.fzh, "tdh");
                        //重新排列 按基础通道，智能通道,控制通道，累计通道顺序进行排序  20170715
                        DataTable SortShowDt = StaticClass.AllPointDt.Clone();
                        //加载基础通道
                        DataRow[] BaseChanelInStation = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and (tdh<=16 or (tdh>=40 and tdh<=43)) and (lxtype='1' or lxtype='2' or (lxtype='3' and dzh>0))", "fzh,tdh,dzh ASC");
                        foreach (DataRow temprow in BaseChanelInStation)
                        {
                            SortShowDt.Rows.Add(temprow.ItemArray);
                        }
                        //加载智能通道
                        DataRow[] SmartChanelInStation = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and tdh>=17 and tdh<=24 ", "fzh,tdh,dzh ASC");
                        foreach (DataRow temprow in SmartChanelInStation)
                        {
                            SortShowDt.Rows.Add(temprow.ItemArray);
                        }
                        //加载本地控制通道
                        DataRow[] ControlChanelInStation = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and (lxtype='3' and dzh=0)", "fzh,tdh,dzh ASC");
                        foreach (DataRow temprow in ControlChanelInStation)
                        {
                            SortShowDt.Rows.Add(temprow.ItemArray);
                        }
                        //加载累计通道
                        DataRow[] TiredChanelInStation = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and (lxtype='4')", "fzh,tdh,dzh ASC");
                        foreach (DataRow temprow in TiredChanelInStation)
                        {
                            SortShowDt.Rows.Add(temprow.ItemArray);
                        }
                        //加载人员通道
                        DataRow[] PointChanelInStation = StaticClass.AllPointDt.Select("fzh='" + objfz.fzh + "' and (lxtype='7')", "fzh,tdh,dzh ASC");
                        foreach (DataRow temprow in PointChanelInStation)
                        {
                            SortShowDt.Rows.Add(temprow.ItemArray);
                        }
                        rows = SortShowDt.Select("1=1");
                        if (rows.Length > 0)
                        {
                            for (int i = 0; i < rows.Length; i++)
                            {
                                row = objfz.showdt.NewRow();
                                row["point"] = rows[i]["point"];
                                row["wz"] = rows[i]["wz"];
                                row["type"] = rows[i]["lb"];
                                if (int.Parse(rows[i]["zt"].ToString()) == StaticClass.itemStateToClient.EqpState33)//休眠
                                {
                                    row["ssz"] = OprFuction.StateChange(rows[i]["zt"].ToString());
                                }
                                else
                                {
                                    if (rows[i]["lx"].ToString() == "模拟量" && float.TryParse(rows[i]["ssz"].ToString(), out ssz))
                                    {
                                        try
                                        {
                                            if (rows[i]["zl"].ToString().Contains("甲烷"))
                                            {
                                                if (rows[i]["ssz"].ToString() == "0")
                                                {
                                                    rows[i]["ssz"] = "0.00";
                                                }
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                                        }
                                        row["ssz"] = rows[i]["ssz"].ToString() + " " + rows[i]["dw"].ToString();
                                    }
                                    else
                                    {
                                        row["ssz"] = rows[i]["ssz"];
                                    }
                                }
                                row["state"] = OprFuction.StateChange(rows[i]["zt"].ToString());
                                if (FzDev != null && (FzDev.LC2 == 13 || FzDev.LC2 == 12 || FzDev.LC2 == 15))
                                {
                                    if (rows[i]["lb"].ToString().Contains("无线"))
                                    {
                                        row["voltage"] = rows[i]["voltage"].ToString() + "%";
                                    }
                                    else
                                    {
                                        row["voltage"] = rows[i]["voltage"].ToString() + "V";
                                    }
                                }
                                else
                                {
                                    row["voltage"] = "-";
                                }

                                row["sxyj"] = rows[i]["sxyj"].ToString() == "0" ? "-" : rows[i]["sxyj"].ToString();
                                row["sxbj"] = rows[i]["sxbj"].ToString() == "0" ? "-" : rows[i]["sxbj"].ToString();
                                row["sxdd"] = rows[i]["sxdd"].ToString() == "0" ? "-" : rows[i]["sxdd"].ToString();
                                row["sxfd"] = rows[i]["sxfd"].ToString() == "0" ? "-" : rows[i]["sxfd"].ToString();
                                row["xxyj"] = rows[i]["xxyj"].ToString() == "0" ? "-" : rows[i]["xxyj"].ToString();
                                row["xxbj"] = rows[i]["xxbj"].ToString() == "0" ? "-" : rows[i]["xxbj"].ToString();
                                row["xxdd"] = rows[i]["xxdd"].ToString() == "0" ? "-" : rows[i]["xxdd"].ToString();
                                row["xxfd"] = rows[i]["xxfd"].ToString() == "0" ? "-" : rows[i]["xxfd"].ToString();
                                objfz.showdt.Rows.Add(row);
                            }
                        }

                        if (dt != null)
                        {
                            objfz.showdtkz = dt;
                        }
                    }
                }
            }

            realshow();
        }

        private void FzShowForm_Load(object sender, EventArgs e)
        {
            lb_fzip.Text = "";
            lb_fzmac.Text = "";
            lb_fzstate.Text = "";
            lb_fztype.Text = "";
            lb_fzwz.Text = "";
            lb_fzpower.Text = "";
            inigrid();
            inigrid1();
            getpoint();

            _isRun = true;
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.IsBackground = true;
            freshthread.Start();
        }

        private void realshow()
        {
            lb_fzip.Text = objfz.ip;
            lb_fzmac.Text = objfz.mac;
            lb_fzstate.Text = objfz.ssz;
            int tempSszColor = 0;
            int.TryParse(objfz.sszcolor, out tempSszColor);
            lb_fzstate.ForeColor = Color.FromArgb(tempSszColor);
            lb_fztype.Text = objfz.type;
            lb_fzwz.Text = objfz.wz;
            float tempfloat = 0;
            float.TryParse(objfz.power, out tempfloat);
            if (objfz.power == "-")
            {
                lb_fzpower.Text = objfz.power;
            }
            else
            {
                lb_fzpower.Text = tempfloat.ToString("f0") + "%";
            }
            if (lb_fzstate.Text == "通讯中断" || lb_fzstate.Text == "未知")
            {
                txtPowerBoxState.Text = "-";
            }
            else
            {
                txtPowerBoxState.Text = objfz.stationDyType;
            }
            if (txtPowerBoxState.Text == "通讯正常")
            {
                txtPowerBoxState.ForeColor = Color.Green;
            }
            else
            {
                txtPowerBoxState.ForeColor = Color.Red;
            }
            mainGrid.DataSource = objfz.showdt;
            if (objfz.showdt.Rows.Count == objfz.showdtrowscount)
            {
                if (objfz.showdtfocuscolid > -1 && objfz.showdtfocusrowid > -1 &&
                objfz.showdt.Rows.Count > 0)
                {
                    mainGridView.FocusedRowHandle = objfz.showdtfocusrowid;
                    mainGridView.FocusedColumn.ColumnHandle = objfz.showdtfocuscolid;
                }
                mainGridView.TopRowIndex = objfz.topindex1;
            }
            gridControl1.DataSource = objfz.showdtkz;
            if (objfz.showdtkz.Rows.Count == objfz.showdtkzrowscount)
            {
                if (objfz.showdtkzfocuscolid > -1 && objfz.showdtkzfocusrowid > -1 &&
       objfz.showdtkz.Rows.Count > 0)
                {
                    gridView1.FocusedRowHandle = objfz.showdtkzfocusrowid;
                    gridView1.FocusedColumn.ColumnHandle = objfz.showdtkzfocuscolid;
                }
                gridView1.TopRowIndex = objfz.topindex2;
            }
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Enabled = false;
        //    try
        //    {
        //        getmsg();
        //        realshow();
        //    }
        //    catch (Exception ex)
        //    {
        //        Basic.Framework.Logging.LogHelper.Error(ex);
        //    }
        //    timer1.Enabled = true;
        //}
        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    Jc_DefInfo def = Model.RealInterfaceFuction.Getpoint(objfz.point);
                    DataTable dt = Model.RealInterfaceFuction.Getfzjckz(objfz.fzh);
                    MethodInvoker In = new MethodInvoker(() => getmsg(def, dt));
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(In);
                    }
                    //MethodInvoker In1 = new MethodInvoker(() => realshow());
                    //this.BeginInvoke(In1);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmb_fzadr_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                objfz.fzh = int.Parse(cmb_fzadr.Text);
                objfz.showdtfocusrowid = objfz.showdtfocuscolid = -1;
                objfz.showdtkzfocuscolid = objfz.showdtkzfocusrowid = -1;
                Jc_DefInfo def = Model.RealInterfaceFuction.Getpoint(objfz.point);
                DataTable dt = Model.RealInterfaceFuction.Getfzjckz(objfz.fzh);
                getmsg(def, dt);
                realshow();
                InitChargeMrg();//xuzp20151015

            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string point = "";
            DataRow[] rows;
            try
            {
                if (e.Column.Tag.ToString() == "3")
                {
                    point = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns[0]).ToString();
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                        if (rows.Length > 0)
                        {
                            int tempInt = 0;
                            int.TryParse(rows[0]["sszcolor"].ToString(), out tempInt);
                            if (string.IsNullOrEmpty(rows[0]["sszcolor"].ToString()))
                            {
                                e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                            }
                            else
                            {
                                e.Appearance.ForeColor = Color.FromArgb(tempInt);
                            }
                        }
                    }
                }

                if (e.Column.Tag.ToString() == "4")
                {
                    point = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns[0]).ToString();
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                        if (rows.Length > 0)
                        {
                            int tempInt = 0;
                            int.TryParse(rows[0]["sszcolor"].ToString(), out tempInt);
                            if (string.IsNullOrEmpty(rows[0]["sszcolor"].ToString()))
                            {
                                e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                            }
                            else
                            {
                                e.Appearance.ForeColor = Color.FromArgb(tempInt);
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

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string point = "";
            DataRow[] rows;
            try
            {
                if (e.Column.Tag.ToString() == "3")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                //if (e.Column.Tag.ToString() == "4")
                //{
                //    point = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[5]).ToString();
                //    lock (StaticClass.allPointDtLockObj)
                //    {
                //        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                //        if (rows.Length > 0)
                //        {
                //            e.Appearance.ForeColor = Color.FromArgb(int.Parse(rows[0]["sszcolor"].ToString()));
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void mainGrid_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 初始化电源管理界面 xuzp20151015
        /// </summary>
        private void InitChargeMrg()
        {
            try
            {
                if (objfz.fzh != 0)
                {
                    //TODO:与其它业务模块相关联
                    //JCDEFDTO tempDef = Model.ChargeMrg.QueryPointByCodeCache(objfz.point);
                    //if (null != tempDef)
                    //{
                    //    JCDEVDTO tempDev = Model.ChargeMrg.QueryDevByDevIDCache(tempDef.Devid);
                    //    if (null != tempDev)
                    //    {
                    //        if (tempDev.LC2 == 12 || tempDev.LC2 == 13)//如果是KJ306-F(16)/KJ306-F(16)H智能分站协议
                    //        {
                    //            xtraTabPage4.PageVisible = true;
                    //            CuCharge tempCharge = new CuCharge(tempDef.Point);
                    //            CpanelPowerPac.Controls.Clear();
                    //            CpanelPowerPac.Controls.Add(tempCharge);
                    //            tempCharge.Dock = DockStyle.Fill;
                    //        }
                    //        else
                    //        {
                    //            CpanelPowerPac.Controls.Clear();
                    //            xtraTabPage4.PageVisible = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        CpanelPowerPac.Controls.Clear();
                    //        xtraTabPage4.PageVisible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    CpanelPowerPac.Controls.Clear();
                    //    xtraTabPage4.PageVisible = false;
                    //}


                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("分站运行界面，初始化电源管理界面", ex);
            }
        }

        private void labelControl5_Click(object sender, EventArgs e)
        {
            //SetPoints  f = new SetPoints ();
            //f.ShowDialog();
        }

        private void FzShowForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }

        private void mainGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }
        /// <summary>
        /// 运行记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    point1.Add("SourceIsList", "true");
                    point1.Add("Key_viewsbrunlogreport1_point", "等于&&$" + point);
                    point1.Add("Display_viewsbrunlogreport1_point", "等于&&$" + point);
                }
                point1.Add("ListID", "27");
                //RequestUtil.ExcuteCommand("Requestsbrunlogreport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("历史运行记录", ex);
            }
        }
        private string GetSelectPoint()
        {
            int row = 0;
            string point = "";
            try
            {
                row = mainGridView.FocusedRowHandle;
                if (row > -1 && mainGridView.FocusedColumn != null)
                {

                    point = mainGridView.GetFocusedRowCellValue("point").ToString();

                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
            return point;
        }
        private bool PointLx(string point, string lx)
        {
            bool flg = false;
            try
            {
                DataRow[] rows = null;
                if (!string.IsNullOrEmpty(point))
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                        if (rows.Length > 0)
                        {
                            if (rows[0]["lx"].ToString() == lx)
                            {
                                flg = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
            return flg;
        }
        private string GetPointID(string point)
        {
            string str = point;
            DataRow[] rows;
            lock (StaticClass.allPointDtLockObj)
            {
                rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                if (rows.Length > 0)
                {
                    str = rows[0]["pointid"].ToString();
                }
            }
            return str;
        }
        /// <summary>
        /// 模拟量实时曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_SSZChart", point1, false);
                        Mnl_SSZChart sszChart = new Mnl_SSZChart(point1);
                        sszChart.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_SSZChart", null, false);
                    Mnl_SSZChart sszChart = new Mnl_SSZChart();
                    sszChart.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量实时曲线", ex);
            }
        }
        /// <summary>
        /// 5分钟曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_FiveMiniteLine", point1, false);
                        Mnl_FiveMiniteLine mnl_FiveMiniteLine = new Mnl_FiveMiniteLine(point1);
                        mnl_FiveMiniteLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_FiveMiniteLine", null, false);
                    Mnl_FiveMiniteLine mnl_FiveMiniteLine = new Mnl_FiveMiniteLine();
                    mnl_FiveMiniteLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量历史曲线5分钟", ex);
            }
        }
        /// <summary>
        /// 密采曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_McLine", point1, false);
                        Mnl_McLine mnl_McLine = new Mnl_McLine(point1);
                        mnl_McLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_McLine", null, false);
                    Mnl_McLine mnl_McLine = new Mnl_McLine();
                    mnl_McLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量历史曲线密采", ex);
            }
        }
        /// <summary>
        /// 小时曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_DayZdzLine", point1, false);
                        Mnl_DayZdzLine mnl_DayZdzLine = new Mnl_DayZdzLine(point1);
                        mnl_DayZdzLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_DayZdzLine", null, false);
                    Mnl_DayZdzLine mnl_DayZdzLine = new Mnl_DayZdzLine();
                    mnl_DayZdzLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量小时曲线", ex);
            }
        }
        /// <summary>
        /// 开关量状态图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestKgl_StateLine", point1, false);
                        Kgl_StateLine kgl_StateLine = new Kgl_StateLine(point1);
                        kgl_StateLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestKgl_StateLine", null, false);
                    Kgl_StateLine kgl_StateLine = new Kgl_StateLine();
                    kgl_StateLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量状态图", ex);
            }
        }
        /// <summary>
        /// 开关量柱状图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestKgl_StateBar", point1, false);
                        Kgl_StateBar kgl_StateBar = new Kgl_StateBar(point1);
                        kgl_StateBar.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestKgl_StateBar", null, false);
                    Kgl_StateBar kgl_StateBar = new Kgl_StateBar();
                    kgl_StateBar.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量柱状图", ex);
            }

        }
        /// <summary>
        /// 模拟量密采记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point1.Add("SourceIsList", "true");
                        point1.Add("Key_viewmcrunlogreport1_point", "等于&&$" + point);
                        point1.Add("Display_viewmcrunlogreport1_point", "等于&&$" + point);
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                        return;
                    }
                }
                point1.Add("ListID", "28");
                //RequestUtil.ExcuteCommand("RequestMCRungLogReport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量密采记录", ex);
            }
        }
        /// <summary>
        /// 开关量状态变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();

                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        //point = GetPointID(point);

                        point1.Add("SourceIsList", "true");
                        point1.Add("Key_ViewJC_KGStateMonth1_point", "等于&&$" + point);
                        point1.Add("Display_ViewJC_KGStateMonth1_point", "等于&&$" + point);
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                        return;
                    }
                }
                point1.Add("ListID", "17");
                //RequestUtil.ExcuteCommand("RequestKGLStateRBReport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量状态变动记录", ex);
            }
        }

    }

    public class ShowFZ
    {
        public ShowFZ()
        {
            inidt();
            inidt1();
        }

        /// <summary>
        /// 分站测点号
        /// </summary>
        public string point;

        /// <summary>
        /// 分站号
        /// </summary>
        public int fzh = 0;

        /// <summary>
        /// 安装位置
        /// </summary>
        public string wz = "";

        /// <summary>
        /// 分站类型
        /// </summary>
        public string type = "";

        /// <summary>
        /// mac地址
        /// </summary>
        public string mac = "";

        /// <summary>
        /// ip地址
        /// </summary>
        public string ip = "";

        /// <summary>
        /// 实时值
        /// </summary>
        public string ssz = "";

        /// <summary>
        /// 实时值颜色
        /// </summary>
        public string sszcolor = "";
        /// <summary>
        /// 分站电量
        /// </summary>
        public string power = "";

        /// <summary>
        /// 分站电源箱通讯状态
        /// </summary>
        public string stationDyType = "";

        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable showdt;

        /// <summary>
        /// 数据源2
        /// </summary>
        public DataTable showdtkz;

        public int showdtfocusrowid = -1;

        public int showdtfocuscolid = -1;

        public int showdtrowscount = 0;

        public int showdtkzfocusrowid = -1;

        public int showdtkzfocuscolid = -1;

        public int showdtkzrowscount = 0;

        public int topindex1 = 0;

        public int topindex2 = 0;

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"point","wz","type",
            "ssz","state", "voltage","sxyj","sxbj","sxdd","sxfd","xxyj","xxbj","xxdd","xxfd"};


        public string[] tcolnamekz = new string[] { "zk","zktype","wz","kzlx",
            "state","bk","bkwz"};

        public void clear()
        {
            point = "";
            wz = "";
            type = "";
            ssz = "";
            mac = "";
            ip = "";
            showdt.Rows.Clear();
        }
        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void inidt()
        {
            DataColumn col;
            showdt = new DataTable();
            for (int i = 0; i < tcolname.Length; i++)
            {
                col = new DataColumn(tcolname[i]);
                showdt.Columns.Add(col);
            }
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void inidt1()
        {
            DataColumn col;
            showdtkz = new DataTable();
            for (int i = 0; i < tcolnamekz.Length; i++)
            {
                col = new DataColumn(tcolnamekz[i]);
                showdtkz.Columns.Add(col);
            }
        }


    }
}
