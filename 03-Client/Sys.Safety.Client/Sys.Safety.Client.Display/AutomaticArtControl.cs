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
using Sys.Safety.Client.Define.Model;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.View.LogOn;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using Sys.Safety.Client.Define;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.ClientFramework.CBFCommon;

namespace Sys.Safety.Client.Display
{
    public partial class AutomaticArtControl : RibbonForm
    {
        public AutomaticArtControl()
        {
            
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
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
        public Dictionary<string, Jc_RInfo> jc_r = new Dictionary<string, Jc_RInfo>();
        /// <summary>
        /// 最后获取记录的Counter
        /// </summary>
        public long LastCounter = 0;

        /// <summary>
        /// 需要删除的id号
        /// </summary>
        public List<string> deletelist = new List<string>();

        /// <summary>
        /// 需要添加的id号
        /// </summary>
        public List<string> addlist = new List<string>();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号","挂接状态","安装位置","设备类型","当前值","挂接时刻",
            "id"};  //"状态","设备状态",

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] {"fzh","point","wz","devName","ssz","time",
            "id"}; //"ssz","state","sbstate",


        public int[] colwith = new int[] { 80, 80, 180, 100, 100, 180, 150 };  //100, 80, 80,

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

        IJc_RService runlogService = ServiceFactory.Create<IJc_RService>();

        List<Jc_DevInfo> devAllList = new List<Jc_DevInfo>();

        List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = new List<AutomaticArticulatedDeviceInfo>();


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
                if (colname[i] == "id" || colname[i] == "设备状态")
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
            string id = "0";
            Jc_RInfo obj = null;
            int tbcount = 0;
            object wz = "", fzh = "", sszn = "", state = "", stime = "", sbstate = "";
            string key = "0";
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
                                if (deletelist.Contains(showdt.Rows[i]["id"].ToString()))
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
                            var dev = devAllList.FirstOrDefault(a => a.Devid == devid);
                            string devName = "";
                            if (dev != null)
                            {
                                devName = dev.Name;
                                if (dev.Type == 1)
                                {
                                    fzh = obj.Fzh.ToString("000") + "A" + obj.Kh.ToString("00") + obj.Dzh.ToString("0");
                                }
                                else if (dev.Type == 2)
                                {
                                    fzh = obj.Fzh.ToString("000") + "D" + obj.Kh.ToString("00") + obj.Dzh.ToString("0");
                                }
                                else if (dev.Type == 3)
                                {
                                    fzh = obj.Fzh.ToString("000") + "C" + obj.Kh.ToString("00") + obj.Dzh.ToString("0");
                                }
                            }

                            //判断自动挂接记录
                            if (point != "未定义")
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    row = StaticClass.AllPointDt.Select("point='" + point + "'");
                                    if (row.Length > 0)
                                    {
                                        wz = row[0]["wz"];
                                    }
                                }
                                state = OprFuction.StateChange(obj.Type.ToString());
                                sbstate = OprFuction.StateChange(obj.State.ToString());
                            }
                            else
                            {
                                wz = "-";
                                state = "自动挂接";
                                sbstate = "自动挂接";
                            }

                            lock (objShowDt)
                            {
                                r = showdt.NewRow();
                                r["point"] = point;
                                r["wz"] = wz;
                                r["fzh"] = fzh;
                                r["ssz"] = sszn;
                                //r["state"] = state;
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
                tbcount = showdt.Rows.Count;
                StaticClass.yccount[7] += tbcount;
                addlist.Clear();
                //mainGridView.FocusedRowHandle = 0;
            }
            #endregion

            #region 当前自动挂接记录条数
            tbcount = showdt.Rows.Count;
            StaticClass.yccount[7] = tbcount;
            #endregion
        }

        /// <summary>
        /// 获取新增运行记录信息
        /// </summary>
        public void getdata()
        {
            Jc_RInfo obj = null;
            long key = 0;


            #region 获取自动挂接数据

            try
            {
                automaticArticulatedDeviceInfo = DEFServiceModel.GetAllAutomaticArticulatedDeviceCache();
                //测试数据
                //AutomaticArticulatedDeviceInfo temp = new AutomaticArticulatedDeviceInfo();
                //temp.ID = "1111";
                //temp.DeviceOnlyCode = "2018000000";
                //temp.AddressNumber = 0;
                //temp.BranchNumber = 2;
                //temp.ChanelNumber = 8;
                //temp.DeviceModel = 34;
                //temp.ReciveTime = DateTime.Now;
                //temp.StationNumber = 1;
                //temp.Value = "10.0";
                //automaticArticulatedDeviceInfo.Add(temp);

                //List<AutomaticArticulatedDeviceInfo> automaticArticulatedDeviceInfo = new List<AutomaticArticulatedDeviceInfo>();

                //判断实时运行记录自动挂接记录是否还存在，如果不存在，则删除此自动挂接记录
                DataRow[] acrowinfos = showdt.Select("point='未定义'");
                if (acrowinfos != null && acrowinfos.Length > 0)
                {
                    for (int i = 0; i < acrowinfos.Length; i++)
                    {
                        var arid = acrowinfos[i]["id"].ToString();

                        if (automaticArticulatedDeviceInfo.FirstOrDefault(o => o.DeviceOnlyCode == arid) == null)
                        {
                            //deletelist.Add(long.Parse(arid));
                            deletelist.Add(arid);
                        }
                    }
                }
                if (automaticArticulatedDeviceInfo.Count > 0)
                {
                    automaticArticulatedDeviceInfo.ForEach(arinfo =>
                    {
                        if (arinfo.ChanelNumber <= 16)
                        {
                            //判断实时运行记录是否存在此自动挂接记录，如不存在，则添加一条记录
                            var rowinfo = showdt.Select("id='" + arinfo.DeviceOnlyCode + "'");
                            if (rowinfo == null || rowinfo.Length == 0)
                            {
                                #region 新增运行记录
                                obj = new Jc_RInfo();
                                obj.ID = arinfo.DeviceOnlyCode;
                                obj.Fzh = arinfo.StationNumber;
                                obj.Kh = arinfo.ChanelNumber;
                                Jc_DevInfo tempdev = devAllList.Find(a => a.Bz4 == arinfo.DeviceModel);
                                if (tempdev != null)
                                {
                                    obj.Devid = tempdev.Devid;
                                }
                                else
                                {
                                    obj.Devid = "";
                                }
                                obj.Dzh = arinfo.AddressNumber;
                                obj.Wzid = "-";
                                obj.Point = "未定义";
                                obj.Type = -1;
                                obj.State = -1;
                                obj.Val = arinfo.Value;
                                obj.Timer = arinfo.ReciveTime;
                                jc_r.Add(obj.ID, obj);
                                //addlist.Add(long.Parse(obj.ID));
                                addlist.Add(obj.ID);
                                #endregion
                            }
                            else 
                            {
                                rowinfo[0]["ssz"] = arinfo.Value;//刷新实时值
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
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
            int Count = 0;
            while (!StaticClass.SystemOut)
            {
                try
                {
                    if (Count > 12)
                    {
                        Count = 0;
                        devAllList = deviceDefineService.GetAllDeviceDefineCache().Data;
                    }
                    else
                    {
                        Count++;
                    }
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

        private void mainGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 2)
                {
                    string id = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "id").ToString();
                    string time = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "time").ToString();

                    if (!string.IsNullOrEmpty(id))
                    {
                        string selectpoint = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "point").ToString();
                        //如果是自动挂接记录跳转到分站定义界面
                        if (selectpoint == "未定义")
                        {
                            AutomaticArticulatedDeviceInfo tempautoInfo = automaticArticulatedDeviceInfo.Find(a => a.DeviceOnlyCode == id);
                            //var fzh = mainGridView.GetRowCellValue(mainGridView.FocusedRowHandle, "fzh").ToString();
                            //var fzharr = fzh.Split('/');                            
                            if (tempautoInfo != null)
                            {
                                Jc_DevInfo tempdev = devAllList.Find(a => a.Bz4 == tempautoInfo.DeviceModel);
                                int devPropertyId = 0;
                                int deviceId = 0;
                                if (tempdev != null)
                                {
                                    devPropertyId = tempdev.Type;
                                    deviceId = int.Parse(tempdev.Devid);
                                }
                                
                                int resultIsMaster = MasterManagement.IsMaster();//等于0，表示正常
                                if (resultIsMaster == 1)
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("当前非主控电脑,请确认本机是否为主控并检查本机网络是否正常！");
                                    return;
                                }
                                if (resultIsMaster == 2)
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("连接服务器失败,请检查网络是否正常！");
                                    return;
                                }
                                if (resultIsMaster == 3)
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("获取当前主机是否为主控主机失败，详细见日志！");
                                    return;
                                }

                                frmLogOn loginForm = new frmLogOn(false);
                                loginForm.ShowDialog();
                                if (LoginManager.isLoginSuccess)//登录成功
                                {
                                    CFPointMrgFrame defineform = new CFPointMrgFrame(tempautoInfo.StationNumber, tempautoInfo.ChanelNumber, tempautoInfo.AddressNumber, devPropertyId, deviceId);
                                    defineform.Show();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + "\r\n" + ex.TargetSite);
            }
        }
    }
}
