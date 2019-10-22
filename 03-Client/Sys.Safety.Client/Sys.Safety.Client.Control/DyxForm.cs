using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Enums;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Control;
using Sys.Safety.Client.Control.Model;
using DevExpress.Utils;
using Sys.Safety.Request.NetworkModule;

namespace Sys.Safety.Client.Control
{
    public partial class DyxForm : XtraForm
    {
        //private bool _ifLoop = true; //线程循环标记

        private readonly Dictionary<string, DateTime> _substationUpdatetime = new Dictionary<string, DateTime>();
        //    //分站电源箱更新时间

        private readonly Dictionary<string, DateTime> _switchUpdatetime = new Dictionary<string, DateTime>();
        //    //交换机电源箱更新时间

        private Thread _thrSubstationRefresh = null; //分站信息刷新线程
        private Thread _thrSwitchRefresh = null; //交换机信息刷新线程
        private DateTime _lastBasicInfoRefTime = new DateTime(); //分站基础信息最后一次下发刷新命令时间
        private List<Jc_DefInfo> _lisDef;

        private List<Jc_MacInfo> _lisMac;

        private Thread _refStationBattery = null;   //分站电源箱信息刷新线程
        /// <summary>
        /// 当前选择的分站
        /// </summary>
        private string selectFzhNow = "";
        /// <summary>
        /// 当前选择的交换机
        /// </summary>
        private string selectMacNow = "";

        public DyxForm()
        {
            InitializeComponent();
        }

        private void DyxForm_Load(object sender, EventArgs e)
        {
            try
            {
                //加载分站树
                var fzs = ControlInterfaceFuction.GetDyxFz();
                //dt = fzs;
                if ((fzs != null) && (fzs.Rows.Count > 0))
                    foreach (DataRow item in fzs.Rows)
                    {
                        var fzh = item["fzh"].ToString();
                        treeViewSubstation.Nodes.Add(fzh, fzh);
                        _substationUpdatetime.Add(fzh, new DateTime());
                    }

                //加载交换机树
                fzs = ControlInterfaceFuction.GetDyxMac();
                //dt1 = fzs;
                if ((fzs != null) && (fzs.Rows.Count > 0))
                    foreach (DataRow item in fzs.Rows)
                    {
                        var wz = item["wz"].ToString();
                        var mac = item["mac"].ToString();
                        treeViewSwitch.Nodes.Add(mac, wz);
                        _switchUpdatetime.Add(mac, new DateTime());
                    }

                //加载风电闭锁树
                //var allSubstation = ControlInterfaceFuction.GetAllSubstation();
                _lisDef = ControlInterfaceFuction.GetAllSubstation();
                foreach (var item in _lisDef)
                {
                    treeViewInterlockedCircuitBreaker.Nodes.Add(item.Fzh.ToString(), item.Fzh.ToString());
                }
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 分站电源箱label点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubstationLabelClick(object sender, EventArgs e)
        {
            try
            {
                var tv = (Label)sender;
                var bi = (BatteryItem)tv.Tag;
                //label2.Text = bi.BatteryAddress;
                label2.Text = bi.Channel;

                _lisDef = ControlInterfaceFuction.GetAllSubstation();//重新加载
                string selectStationPointId = "";
                Jc_DefInfo def = _lisDef.Find(a => a.Fzh == short.Parse(selectFzhNow));
                if (def != null)
                {
                    selectStationPointId = def.PointID;
                }
                //查找分站对应的电源箱5分钟统计信息  
                BatteryPowerConsumption batteryPowerConsumption = new BatteryPowerConsumption();
                if (def.BatteryPowerConsumptions != null)
                {
                    batteryPowerConsumption = def.BatteryPowerConsumptions.Find(a => a.Channel == bi.Channel);
                }
                var tempCharge = new CuCharge(bi, batteryPowerConsumption);
                //CpanelPowerPac.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (CpanelPowerPac.Controls.Count > 0)
                {
                    if (CpanelPowerPac.Controls[0] != null)
                        CpanelPowerPac.Controls[0].Dispose();
                }
                CpanelPowerPac.Controls.Add(tempCharge);
                tempCharge.Dock = DockStyle.Fill;

                //var node = treeViewSubstation.SelectedNode;
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //dic.Add("GetBatteryItem", ((Label)sender).Text);
                //dic.Add("SelNode", node);
                //_refStationBattery = new Thread(GetBatteryItem);

                //_refStationBattery.Start(dic);

            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void GetBatteryItem(object dic)
        //{
        //    Dictionary<string, object> tempdic = dic as Dictionary<string, object>;
        //    string batteryAddress = tempdic["GetBatteryItem"].ToString();
        //    var node = (TreeNode)tempdic["SelNode"];
        //    if (node == null)
        //    {
        //        return;
        //    }
        //    var gsa = ControlInterfaceFuction.GetSubstationPowerBoxInfo(node.Name);

        //    if (gsa.PowerDateTime > Convert.ToDateTime("1900-01-01"))
        //    {
        //        BatteryItem item = gsa.PowerBoxInfo.FirstOrDefault(a => a.Channel == batteryAddress.ToString());
        //        if (item != null)
        //        {
        //            //Action<object> act = RefStationBatteryInfo;
        //            //BeginInvoke(act, item);
        //            MethodInvoker In = new MethodInvoker(() => RefStationBatteryInfo(item));
        //            if (this.InvokeRequired)
        //            {
        //                this.BeginInvoke(In);
        //            }
        //            //_substationUpdatetime[node.Name] = gsa.PowerDateTime;
        //            //substationUpdatetime.Text = _substationUpdatetime[node.Name].ToString("yyyy-MM-dd HH:mm:ss");
        //        }
        //    }
        //}

        //private void RefStationBatteryInfo(object item)
        //{
        //    var tempCharge = new CuCharge((BatteryItem)item);
        //    CpanelPowerPac.Controls.Clear();
        //    CpanelPowerPac.Controls.Add(tempCharge);
        //    tempCharge.Dock = DockStyle.Fill;

        //    //substationUpdatetime.Text = PowerDateTime.ToString();
        //}

        /// <summary>
        /// 交换机电源箱label点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchLabelClick(object sender, EventArgs e)
        {
            try
            {
                var tv = (Label)sender;
                var bi = (BatteryItem)tv.Tag;
                label3.Text = bi.Channel;

                //加载交换机          
                _lisMac = ControlInterfaceFuction.GetAllSwitch();
               
                Jc_MacInfo mac = _lisMac.Find(a => a.MAC == selectMacNow);
               
                //查找分站对应的电源箱5分钟统计信息  
                 BatteryPowerConsumption batteryPowerConsumption = new BatteryPowerConsumption();
                 if (mac.BatteryPowerConsumptions != null)
                 {
                     batteryPowerConsumption = mac.BatteryPowerConsumptions.Find(a => a.Channel == bi.Channel);
                 }

                var tempCharge = new CuCharge(bi, batteryPowerConsumption);
                //panelControl2.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (panelControl2.Controls.Count > 0)
                {
                    if (panelControl2.Controls[0] != null)
                        panelControl2.Controls[0].Dispose();
                }
                panelControl2.Controls.Add(tempCharge);
                tempCharge.Dock = DockStyle.Fill;

                //var node = treeViewSubstation.SelectedNode;
                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //dic.Add("GetBatteryItem", ((Label)sender).Text);
                //dic.Add("SelNode", node);
                //_refStationBattery = new Thread(GetBatteryItemForSwicth);

                //_refStationBattery.Start(dic);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        //private void GetBatteryItemForSwicth(object dic)
        //{
        //    Dictionary<string, object> tempdic = dic as Dictionary<string, object>;
        //    string batteryAddress = tempdic["GetBatteryItem"].ToString();
        //    var node = (TreeNode)tempdic["SelNode"];
        //    if (node == null)
        //    {
        //        return;
        //    }
        //    var gsp = ControlInterfaceFuction.GetSwitchPowerBoxInfo(node.Name);

        //    if (gsp.PowerDateTime > Convert.ToDateTime("1900-01-01"))
        //    {
        //        BatteryItem item = gsp.PowerBoxInfo.FirstOrDefault(a => a.Channel == batteryAddress.ToString());
        //        if (item != null)
        //        {
        //            //Action<object> act = RefStationBatteryInfo;
        //            //BeginInvoke(act, item);
        //            MethodInvoker In = new MethodInvoker(() => RefStationBatteryInfoForSwicth(item));
        //            if (this.InvokeRequired)
        //            {
        //                this.BeginInvoke(In);
        //            }
        //            //_substationUpdatetime[node.Name] = gsa.PowerDateTime;
        //            //substationUpdatetime.Text = _substationUpdatetime[node.Name].ToString("yyyy-MM-dd HH:mm:ss");
        //        }
        //    }
        //}
        //private void RefStationBatteryInfoForSwicth(object item)
        //{
        //    //var tempCharge = new CuCharge((BatteryItem)item);
        //    //CpanelPowerPac.Controls.Clear();
        //    //CpanelPowerPac.Controls.Add(tempCharge);
        //    //tempCharge.Dock = DockStyle.Fill;
        //    var tempCharge = new CuCharge((BatteryItem)item);
        //    panelControl2.Controls.Clear();
        //    panelControl2.Controls.Add(tempCharge);
        //    tempCharge.Dock = DockStyle.Fill;

        //    //switchUpdatetime.Text = PowerDateTime.ToString();
        //}

        private void DyxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_thrSubstationRefresh != null && _thrSubstationRefresh.IsAlive == true)
            {
                _thrSubstationRefresh.Abort();
            }
            if (_thrSwitchRefresh != null && _thrSwitchRefresh.IsAlive == true)
            {
                _thrSwitchRefresh.Abort();
            }
            //_ifLoop = false;            
            //if (_thrSubstationRefresh != null && _thrSubstationRefresh.IsAlive == true)
            //{
            //    _thrSubstationRefresh.Join();
            //}
            //if (_thrSwitchRefresh != null && _thrSwitchRefresh.IsAlive == true)
            //{
            //    _thrSwitchRefresh.Join();
            //}
        }

        /// <summary>
        /// 分站树选择函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewSubstation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
               
                //结束刷新进程
                if (_thrSubstationRefresh != null && _thrSubstationRefresh.IsAlive == true)
                {
                    _thrSubstationRefresh.Abort();
                }

                var node = treeViewSubstation.SelectedNode;
                if (node == null)
                {
                    //XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //刷新放电按钮状态
                //var point = node.Name.PadLeft(3, '0') + "0000";
                //var tempControls = ChargeMrg.QueryJCSDKZbyInf(10, point);
                //if (tempControls.Count > 0)
                //{
                //    //XtraMessageBox.Show("该设备正处于放电状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    //return;
                //    substationExecuteDischarge.Text = "取消放电";
                //    substationExecuteDischarge.ForeColor = Color.Red;
                //}
                //else
                //{
                //    substationExecuteDischarge.Text = "执行放电";
                //    substationExecuteDischarge.ForeColor = Color.Green;
                //}

                //var gsa = ControlInterfaceFuction.GetSubstationPowerBoxInfo(node.Name);
                //SubstationInfoRefresh(gsa);
                selectFzhNow = node.Name;

                ChargeMrg.sendD(0, node.Name);
                _thrSubstationRefresh = new Thread(SubstationRefreshRefreshThrFun)
                {
                    IsBackground = true
                };
                _thrSubstationRefresh.Start(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 交换机树选择函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewSwitch_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //结束刷新进程
                if (_thrSwitchRefresh != null && _thrSwitchRefresh.IsAlive == true)
                {
                    _thrSwitchRefresh.Abort();
                }

                var node = treeViewSwitch.SelectedNode;
                if (node == null)
                {
                    return;
                }

                //刷新放电按钮状态
                //var point = node.Name;
                //var tempControls = ChargeMrg.QueryJCSDKZbyInf(10, point);
                //if (tempControls.Count > 0)
                //{
                //    switchExecuteDischarge.Text = "取消放电";
                //    switchExecuteDischarge.ForeColor = Color.Red;
                //}
                //else
                //{
                //    switchExecuteDischarge.Text = "执行放电";
                //    switchExecuteDischarge.ForeColor = Color.Green;
                //}

                //var gsp = ControlInterfaceFuction.GetSwitchPowerBoxInfo(node.Name);
                //SwitchInfoRefresh(gsp);
                selectMacNow = node.Name;

                ChargeMrg.sendD(16, node.Name);
                _thrSwitchRefresh = new Thread(SwitchRefreshRefreshThrFun)
                {
                    IsBackground = true
                };
                _thrSwitchRefresh.Start(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 分站电源箱放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substationExecuteDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewSubstation.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var point = node.Name;
                ChargeMrg.SendStationDControl(Convert.ToUInt16(point), 2);

                //写放电记录  20180124
                ChargeMrg.AddPowerboxchargehistory(Convert.ToUInt16(point), "");

                //var tempControls = ChargeMrg.QueryJCSDKZbyInf(10, point);
                //if (tempControls.Count > 0) //处于放电状态
                //{
                //    if (substationExecuteDischarge.Text == "取消放电")
                //    {
                //        for (var i = 0; i < tempControls.Count; i++)
                //        {
                //            tempControls[i].InfoState = InfoState.Delete;
                //            OperateLogHelper.InsertOperateLog(4,
                //                "取消放电：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" +
                //                DateTime.Now + "】", "");
                //        }
                //        ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());
                //        substationExecuteDischarge.Text = "执行放电";
                //        substationExecuteDischarge.ForeColor = Color.Green;
                //    }
                //    else if (substationExecuteDischarge.Text == "执行放电")
                //    {
                //        substationExecuteDischarge.Text = "取消放电";
                //        substationExecuteDischarge.ForeColor = Color.Red;
                //    }
                //}
                //else //处于未放电状态
                //{
                //    if (substationExecuteDischarge.Text == "执行放电")
                //    {
                //        var tempControlAdd = new Jc_JcsdkzInfo
                //        {
                //            ID = IdHelper.CreateLongId().ToString(),
                //            Type = 10,
                //            ZkPoint = "0000000",
                //            Bkpoint = point,
                //            InfoState = InfoState.AddNew
                //        };
                //        ChargeMrg.AddJC_JCSDKZCache(tempControlAdd);
                //        OperateLogHelper.InsertOperateLog(4,
                //            "执行放电：主控【" + tempControlAdd.ZkPoint + "】-【" + tempControlAdd.Bkpoint + "】-【" +
                //            DateTime.Now + "】", "");
                //        substationExecuteDischarge.Text = "取消放电";
                //        substationExecuteDischarge.ForeColor = Color.Red;
                //    }
                //    else if (substationExecuteDischarge.Text == "取消放电")
                //    {
                //        substationExecuteDischarge.Text = "执行放电";
                //        substationExecuteDischarge.ForeColor = Color.Green;
                //    }
                //}
                XtraMessageBox.Show("操作成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 分站电源箱信息刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substationRefresh_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var node = treeViewSwitch.SelectedNode;
            //    if (node == null)
            //    {
            //        XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    var point = node.Name;
            //    var tempControls = ChargeMrg.QueryJCSDKZbyInf(10, point);
            //    if (tempControls.Count == 0)
            //    {
            //        XtraMessageBox.Show("该设备未处于放电状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    for (var i = 0; i < tempControls.Count; i++)
            //    {
            //        tempControls[i].InfoState = InfoState.Delete;
            //        OperateLogHelper.InsertOperateLog(4,
            //            "取消放电：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" +
            //            DateTime.Now + "】", "");
            //    }
            //    //ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());

            //    XtraMessageBox.Show("操作成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //catch (Exception exc)
            //{
            //    XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            try
            {
                var node = treeViewSubstation.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_thrSubstationRefresh != null && _thrSubstationRefresh.IsAlive == true)
                {
                    return;
                }
                selectFzhNow = node.Name;
                ChargeMrg.sendD(0, node.Name);
                _thrSubstationRefresh = new Thread(SubstationRefreshRefreshThrFun)
                {
                    IsBackground = true
                };
                _thrSubstationRefresh.Start(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 交换机执行放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchExecuteDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewSwitch.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var mac = node.Name;
                ChargeMrg.SendSwitchesDControl(mac, 2);

                //写放电记录  20180124
                ChargeMrg.AddPowerboxchargehistory(0, mac);

                XtraMessageBox.Show("操作成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 交换机电源箱信息刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewSwitch.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_thrSwitchRefresh != null && _thrSwitchRefresh.IsAlive == true)
                {
                    return;
                }
                selectMacNow = node.Name;
                ChargeMrg.sendD(16, node.Name);
                _thrSwitchRefresh = new Thread(SwitchRefreshRefreshThrFun)
                {
                    IsBackground = true
                };
                _thrSwitchRefresh.Start(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 刷新分站电源箱信息
        /// </summary>
        /// <param name="fzh"></param>
        private void SubstationInfoRefresh(object oSapbi)
        {
            // 20170914
            //var fzh = (string)oFzh;
            //var gsa = ControlInterfaceFuction.GetSubstationPowerBoxInfo(fzh);
            var gsa = (GetSubstationAllPowerBoxInfoResponse)oSapbi;
            if (gsa.PowerDateTime > Convert.ToDateTime("1900-01-01"))
            {
                //flowLayoutPanel1.Controls.Clear();
                //CpanelPowerPac.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (flowLayoutPanel1.Controls.Count > 0)
                {
                    if (flowLayoutPanel1.Controls[0] != null)
                        flowLayoutPanel1.Controls[0].Dispose();
                }
                while (CpanelPowerPac.Controls.Count > 0)
                {
                    if (CpanelPowerPac.Controls[0] != null)
                        CpanelPowerPac.Controls[0].Dispose();
                }
                if (gsa.PowerBoxInfo != null && gsa.PowerBoxInfo.Count > 0)
                {
                    gsa.PowerBoxInfo = gsa.PowerBoxInfo.OrderByDescending(a => a.Channel).ToList();
                    for (int i = 0; i < gsa.PowerBoxInfo.Count; i++)
                    {
                        var lab = new Label()
                        {
                            BorderStyle = BorderStyle.Fixed3D,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Width = 40,
                            Height = 28,
                            Name = "textboxSubstation" + Convert.ToString(gsa.PowerBoxInfo[i].Channel),
                            Text = gsa.PowerBoxInfo[i].Channel,
                            Font = new Font("宋体", 10, FontStyle.Bold),
                            Tag = gsa.PowerBoxInfo[i]
                        };
                        lab.Click += SubstationLabelClick;
                        flowLayoutPanel1.Controls.Add(lab);
                        if (i == 0)
                        {
                            SubstationLabelClick(lab, null);
                        }
                    }
                }

                substationUpdatetime.Text = gsa.PowerDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                //更新电源箱信息获取时间
                //_substationUpdatetime[fzh] = gsa.PowerDateTime;
                //substationUpdatetime.Text = _substationUpdatetime[fzh].ToString("yyyy-MM-dd HH:mm:ss");
            }

        }

        /// <summary>
        /// 分站电源箱信息刷新线程函数
        /// </summary>
        /// <param name="par"></param>
        private void SubstationRefreshRefreshThrFun(object par)
        {
            WaitDialogForm wdf = new WaitDialogForm("正在获取电源箱数据...", "请等待...");
            int i = 0;
            for (i = 0; i < 5; i++)
            {
                try
                {
                    var gsa = ControlInterfaceFuction.GetSubstationPowerBoxInfo((string)par);
                    if (gsa.PowerBoxInfo != null && gsa.PowerBoxInfo.Count > 0)//&&  gsa.PowerDateTime > _substationUpdatetime[(string) par]
                    {
                        _substationUpdatetime[(string)par] = gsa.PowerDateTime;
                        Action<object> act = SubstationInfoRefresh;
                        // 20170914
                        //BeginInvoke(act, par);
                        BeginInvoke(act, gsa);
                        break;
                    }
                    Thread.Sleep(3000);
                    //if (!_ifLoop)
                    //{
                    //    break;
                    //}
                }
                catch (Exception exc)
                {
                    //LogHelper.Error(exc);
                    //线程Abort时会报异常,忽略
                }
            }
            if (i >= 5)
            {
                flowLayoutPanel1.BeginInvoke(new Action(() =>
                  {
                      //flowLayoutPanel1.Controls.Clear();
                      //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                      while (flowLayoutPanel1.Controls.Count > 0)
                      {
                          if (flowLayoutPanel1.Controls[0] != null)
                              flowLayoutPanel1.Controls[0].Dispose();
                      }
                  }));
                CpanelPowerPac.BeginInvoke(new Action(() =>
                  {
                      //CpanelPowerPac.Controls.Clear();
                      //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                      while (CpanelPowerPac.Controls.Count > 0)
                      {
                          if (CpanelPowerPac.Controls[0] != null)
                              CpanelPowerPac.Controls[0].Dispose();
                      }
                  }));
            }
            if (wdf != null)
            {
                wdf.Close();
            }
        }

        /// <summary>
        /// 刷新交换机电源箱信息
        /// </summary>
        private void SwitchInfoRefresh(object oSapbi)
        {
            // 20170914
            //var mac = (string)oMac;
            //var gsp = ControlInterfaceFuction.GetSwitchPowerBoxInfo(mac);
            var gsp = (GetSwitchAllPowerBoxInfoResponse)oSapbi;
            if (gsp.PowerDateTime > Convert.ToDateTime("1900-01-01"))//&& gsp.PowerDateTime > _switchUpdatetime[mac]
            {
                //flowLayoutPanel2.Controls.Clear();
                //panelControl2.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (flowLayoutPanel2.Controls.Count > 0)
                {
                    if (flowLayoutPanel2.Controls[0] != null)
                        flowLayoutPanel2.Controls[0].Dispose();
                }
                while (panelControl2.Controls.Count > 0)
                {
                    if (panelControl2.Controls[0] != null)
                        panelControl2.Controls[0].Dispose();
                }
                if (gsp.PowerBoxInfo != null && gsp.PowerBoxInfo.Count > 0)
                {
                    gsp.PowerBoxInfo = gsp.PowerBoxInfo.OrderByDescending(a => a.Channel).ToList();
                    for (int i = 0; i < gsp.PowerBoxInfo.Count; i++)
                    {
                        var lab = new Label()
                        {
                            BorderStyle = BorderStyle.Fixed3D,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Width = 40,
                            Height = 28,
                            Name = "textboxSwitch" + Convert.ToString(gsp.PowerBoxInfo[i].Channel),
                            Text = gsp.PowerBoxInfo[i].Channel,
                            Font = new Font("宋体", 10, FontStyle.Bold),
                            Tag = gsp.PowerBoxInfo[i]
                        };
                        lab.Click += SwitchLabelClick;
                        flowLayoutPanel2.Controls.Add(lab);
                        if (i == 0)
                        {
                            SwitchLabelClick(lab, null);
                        }
                    }
                }

                switchUpdatetime.Text = gsp.PowerDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                ////更新电源箱信息获取时间
                //_switchUpdatetime[mac] = gsp.PowerDateTime;
                //switchUpdatetime.Text = _switchUpdatetime[mac].ToString("yyyy-MM-dd HH:mm:ss");
            }

        }

        /// <summary>
        /// 交换机电源箱信息刷新线程函数
        /// </summary>
        private void SwitchRefreshRefreshThrFun(object par)
        {
            WaitDialogForm wdf = new WaitDialogForm("正在获取电源箱数据...", "请等待...");
            int i = 0;
            for (i = 0; i < 5; i++)
            {
                try
                {
                    var gsp = ControlInterfaceFuction.GetSwitchPowerBoxInfo((string)par);
                    if (gsp.PowerBoxInfo != null && gsp.PowerBoxInfo.Count > 0 &&
                        gsp.PowerDateTime > _switchUpdatetime[(string)par])
                    {
                        _switchUpdatetime[(string)par] = gsp.PowerDateTime;
                        Action<object> act = SwitchInfoRefresh;
                        // 20170914
                        //BeginInvoke(act, par);
                        BeginInvoke(act, gsp);
                        break;
                    }
                    Thread.Sleep(3000);
                    //Action<object> act = SwitchInfoRefresh;
                    //BeginInvoke(act, par);
                    //Thread.Sleep(3000);
                    //if (!_ifLoop)
                    //{
                    //    break;
                    //}
                }
                catch (Exception exc)
                {
                    //LogHelper.Error(exc);
                    //线程Abort时会报异常,忽略
                }
            }
            if (i >= 5)
            {
                flowLayoutPanel2.BeginInvoke(new Action(() =>
                {
                    //flowLayoutPanel2.Controls.Clear();
                    //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                    while (flowLayoutPanel2.Controls.Count > 0)
                    {
                        if (flowLayoutPanel2.Controls[0] != null)
                            flowLayoutPanel2.Controls[0].Dispose();
                    }
                }));
                panelControl2.BeginInvoke(new Action(() =>
                {
                    //panelControl2.Controls.Clear();
                    //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                    while (panelControl2.Controls.Count > 0)
                    {
                        if (panelControl2.Controls[0] != null)
                            panelControl2.Controls[0].Dispose();
                    }
                }));
            }
            if (wdf != null)
            {
                wdf.Close();
            }
        }

        /// <summary>
        /// 分站取消放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substationCancelDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewSubstation.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var point = node.Name;
                ChargeMrg.SendStationDControl(Convert.ToUInt16(point), 1);
                XtraMessageBox.Show("操作成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 交换机取消放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchCancelDischarge_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewSwitch.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var mac = node.Name;
                ChargeMrg.SendSwitchesDControl(mac, 1);

                //ChargeMrg.MacPowerboxchargehistoryUpdate(mac);//更新放电结束时间
                XtraMessageBox.Show("操作成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        /// 其他管理分站列表选择函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewInterlockedCircuitBreaker_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var node = treeViewInterlockedCircuitBreaker.SelectedNode;
                if (node == null)
                {
                    return;
                }

                _lastBasicInfoRefTime = new DateTime();

                //刷新解锁按钮状态
                var point = node.Name.PadLeft(3, '0') + "0000";
                var tempControls = ChargeMrg.QueryJCSDKZbyInf((int)ControlType.GasThreeUnlockControl, point);
                if (tempControls.Count > 0)
                {
                    icbUnlock.Text = "取消三分风电闭锁解锁";
                    icbUnlock.ForeColor = Color.Red;
                }
                else
                {
                    icbUnlock.Text = "三分风电闭锁解锁";
                    icbUnlock.ForeColor = Color.Green;
                }
                //刷新解锁按钮状态
                var point1 = node.Name.PadLeft(3, '0') + "0000";
                var tempControls1 = ChargeMrg.QueryJCSDKZbyInf((int)ControlType.StationHisDataClear, point1);
                if (tempControls1.Count > 0)
                {
                    simpleButton1.Text = "取消清除分站历史数据";
                    simpleButton1.ForeColor = Color.Red;
                }
                else
                {
                    simpleButton1.Text = "清除分站历史数据";
                    simpleButton1.ForeColor = Color.Green;
                }

                RefBasicInfo(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 风电闭锁解锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icbUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewInterlockedCircuitBreaker.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (icbUnlock.Text == "三分风电闭锁解锁")
                {
                    if (XtraMessageBox.Show("点击三分强制解锁后，需要手动取消，否则设备一直处于强制解锁状态，是否继续？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                var point = node.Name.PadLeft(3, '0') + "0000";
                var tempControls = ChargeMrg.QueryJCSDKZbyInf((int)ControlType.GasThreeUnlockControl, point);
                if (tempControls.Count > 0)         //处于解锁状态
                {
                    if (icbUnlock.Text == "取消三分风电闭锁解锁")
                    {
                        for (var i = 0; i < tempControls.Count; i++)
                        {
                            tempControls[i].InfoState = InfoState.Delete;
                            OperateLogHelper.InsertOperateLog(4,
                                "取消三分风电闭锁解锁：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" +
                                DateTime.Now + "】", "");
                        }
                        ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());
                        icbUnlock.Text = "三分风电闭锁解锁";
                        icbUnlock.ForeColor = Color.Green;
                    }
                    else if (icbUnlock.Text == "三分风电闭锁解锁")
                    {
                        icbUnlock.Text = "取消三分风电闭锁解锁";
                        icbUnlock.ForeColor = Color.Red;
                    }
                }
                else        //未处于解锁状态
                {
                    if (icbUnlock.Text == "三分风电闭锁解锁")
                    {
                        var tempControlAdd = new Jc_JcsdkzInfo
                        {
                            ID = IdHelper.CreateLongId().ToString(),
                            Type = (int)ControlType.GasThreeUnlockControl,
                            ZkPoint = "0000000",
                            Bkpoint = point,
                            InfoState = InfoState.AddNew
                        };
                        ChargeMrg.AddJC_JCSDKZCache(tempControlAdd);
                        OperateLogHelper.InsertOperateLog(4,
                            "三分风电闭锁解锁：主控【" + tempControlAdd.ZkPoint + "】-【" + tempControlAdd.Bkpoint + "】-【" +
                            DateTime.Now + "】", "");
                        icbUnlock.Text = "取消三分风电闭锁解锁";
                        icbUnlock.ForeColor = Color.Red;
                    }
                    else if (icbUnlock.Text == "取消三分风电闭锁解锁")
                    {
                        icbUnlock.Text = "三分风电闭锁解锁";
                        icbUnlock.ForeColor = Color.Green;
                    }
                }

            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 刷新分站基础信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refBasicInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewInterlockedCircuitBreaker.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var now = DateTime.Now;
                if ((now - _lastBasicInfoRefTime).TotalSeconds > 15)
                {
                    ChargeMrg.QueryDeviceInfoRequest(node.Name, 1, 1, 2);
                    _lastBasicInfoRefTime = now;
                }

                RefBasicInfo(node.Name);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 刷新分站基础信息
        /// </summary>
        /// <param name="sFzh"></param>
        private void RefBasicInfo(string sFzh)
        {
            var info = ChargeMrg.GetSubstationBasicInfo(sFzh);

            //绑定分站信息
            textFzh.Text = sFzh;
            fzType.Text = info.Type;
            wz.Text = info.Location;           
            onlyCode.Text = info.OnlyCoding;
            ProductionTime.Text = info.ProductionTime.ToString("yyyy-MM-dd HH:mm:ss");
            Voltage.Text = info.Voltage;
            RestartNum.Text = info.RestartNum.ToString();
            if (!string.IsNullOrEmpty(info.IP))
            {
                IP.Text = info.IP.ToString();
            }
            if (!string.IsNullOrEmpty(info.MAC))
            {
                MAC.Text = info.MAC.ToString();
            }

            //绑定下级设备信息
            var dt = ObjectConverter.ToDataTable<InferiorBasicInfo>(info.InferiorInfo);
            xjsb.DataSource = dt;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeViewInterlockedCircuitBreaker.SelectedNode;
                if (node == null)
                {
                    XtraMessageBox.Show("请先选择需操作的项。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var point = node.Name.PadLeft(3, '0') + "0000";
                var tempControls = ChargeMrg.QueryJCSDKZbyInf((int)ControlType.StationHisDataClear, point);
                if (tempControls.Count > 0)         //处于解锁状态
                {
                    if (simpleButton1.Text == "取消清除分站历史数据")
                    {
                        for (var i = 0; i < tempControls.Count; i++)
                        {
                            tempControls[i].InfoState = InfoState.Delete;
                            OperateLogHelper.InsertOperateLog(4,
                                "取消清除分站历史数据：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" +
                                DateTime.Now + "】", "");
                        }
                        ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());
                        simpleButton1.Text = "清除分站历史数据";
                        simpleButton1.ForeColor = Color.Green;
                    }
                    else if (simpleButton1.Text == "清除分站历史数据")
                    {
                        simpleButton1.Text = "取消清除分站历史数据";
                        simpleButton1.ForeColor = Color.Red;
                    }
                }
                else        //未处于解锁状态
                {
                    if (simpleButton1.Text == "清除分站历史数据")
                    {
                        var tempControlAdd = new Jc_JcsdkzInfo
                        {
                            ID = IdHelper.CreateLongId().ToString(),
                            Type = (int)ControlType.StationHisDataClear,
                            ZkPoint = "0000000",
                            Bkpoint = point,
                            InfoState = InfoState.AddNew
                        };
                        ChargeMrg.AddJC_JCSDKZCache(tempControlAdd);
                        OperateLogHelper.InsertOperateLog(4,
                            "清除分站历史数据：主控【" + tempControlAdd.ZkPoint + "】-【" + tempControlAdd.Bkpoint + "】-【" +
                            DateTime.Now + "】", "");
                        simpleButton1.Text = "取消清除分站历史数据";
                        simpleButton1.ForeColor = Color.Red;
                    }
                    else if (simpleButton1.Text == "取消清除分站历史数据")
                    {
                        simpleButton1.Text = "清除分站历史数据";
                        simpleButton1.ForeColor = Color.Green;
                    }
                }             
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }
    }
}