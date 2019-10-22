using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.Client.Define.Model;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.Cache;
using Sys.Safety.Enums;
using System.Text.RegularExpressions;
using System.Threading;
using DevExpress.XtraLayout.Utils;

namespace Sys.Safety.Client.Define.CommCfg
{
    public partial class CFIPModules : XtraForm
    {
        INetworkModuleService _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
        IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>();
        IPersonPointDefineService _PersonPointDefineService = ServiceFactory.Create<IPersonPointDefineService>();
        /// <summary>
        /// 存储获取到的原始数据包
        /// </summary>
        private byte[] srcPack;
        /// <summary>
        /// 存储获取到的原始数据包
        /// </summary>
        private byte[] SocketsrcPack;
        /// <summary>
        /// 存储获取到的原始数据包2
        /// </summary>
        private byte[] SocketsrcPack2;
        /// <summary>
        /// 存储获取到的原始数据包3
        /// </summary>
        private byte[] SocketsrcPack3;
        /// <summary>
        /// 存储获取到的原始数据包4
        /// </summary>
        private byte[] SocketsrcPack4;
        public CFIPModules()
        {
            InitializeComponent();
        }
        string _ArrPoint;
        public CFIPModules(string ArrPoint)
        {
            _ArrPoint = ArrPoint;
            InitializeComponent();
        }
        /// <summary>
        /// 加载IP模块信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CfIPModules_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPretermitInf();
                LoadInf();
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载IP模块信息【CfIPModules_Load】", ex);
            }
        }
        /// <summary>
        /// 单前可操作分站链表
        /// </summary>
        private List<StationTag> tempStationTag = new List<StationTag>();
        /// <summary>
        /// 存储分站标签是否在绑定信息中是否可以显示的结构体
        /// </summary>
        private class StationTag
        {
            /// <summary>
            /// 测点信息 标签+"."+"名称"
            /// </summary>
            public string PointTag;
            /// <summary>
            /// 是否可以显示
            /// </summary>
            public bool bActivity;
        }
        /// <summary>
        /// 是去做值改变判断
        /// </summary>
        private bool bdgComboxValueChange = true;
        /// <summary>
        /// 模块信息验证
        /// </summary>
        private string SelectedSubStation = "";
        /// <summary>
        /// 当前操作的模块IP地址
        /// </summary>
        private string ModuleIPNow = "";


        private string bz2 = "";
        private string bz3 = "";
        /// <summary>
        /// 定义器，用于设置button状态
        /// </summary>
        private System.Windows.Forms.Timer timerSetButtonState = new System.Windows.Forms.Timer();

        /// <summary>
        /// 加载基础信息
        /// </summary>
        private void LoadInf()
        {
            if (string.IsNullOrEmpty(_ArrPoint))
            {
                return;
            }

            Jc_MacInfo temp = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
            if (temp != null)
            {
                //判断模块类型
                if (temp.Upflag == "0")
                {
                    this.xtraTabPage2.PageVisible = false;//分站不支持设置IP、网关、子网掩码等信息

                    for (int i = this.CdgStationQeen.Rows.Count - 1; i > 0; i--)
                    {
                        this.CdgStationQeen.Rows.RemoveAt(i);
                    }
                    this.CdgStationQeen.Enabled = false;
                    layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else
                {
                    this.xtraTabPage2.PageVisible = true;//交换机支持设置IP、网关、子网掩码等信息

                    this.CdgStationQeen.Enabled = true;
                    layoutControlItem21.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                CgleSwitchAdress.EditValue = temp.Wzid; //测点名称 存储交换机位置
                CcmbMAC.Text = temp.MAC; //MAC
                CcmbIP.Text = temp.IP;//IP
                CckTransPort.Checked = (temp.Istmcs == 1) ? true : false;//透传
                bz2 = temp.Bz2;
                bz3 = temp.Bz3;
                if (!string.IsNullOrEmpty(temp.Bz6))
                {
                    SwitchComIP.Text = temp.Bz6;
                }
                else {
                    SwitchComIP.Text = "";
                }
                //加载智能电源箱时，当前交换机下面有一个模块绑定了，则勾选  20170404
                List<Jc_MacInfo> MacList = Model.MACServiceModel.QueryMACBybz2Cache(bz2).ToList().FindAll(a => a.Bz4 == "1");
                if (MacList.Count > 0)
                {
                    ck_dyx.Checked = true;
                }
                else
                {
                    ck_dyx.Checked = false;
                }
                if (!string.IsNullOrEmpty(temp.Bz1))
                {
                    string stationsBZ1 = temp.Bz1;
                    string[] fz1 = stationsBZ1.Split('|');


                    Jc_DefInfo tempStation = null;
                    bdgComboxValueChange = false;
                    //dg1
                    for (int i = 0; i < fz1.Length; i++)
                    {
                        if (fz1[i] == "0")
                        {
                            continue;
                        }
                        if (fz1[i] == "")
                        {
                            continue;
                        }
                        //根据分站号和设备性质去查询分站  20170331
                        List<Jc_DefInfo> tempStationList = Model.DEFServiceModel.QueryPointByInfs(Convert.ToInt32(fz1[i]), 0).ToList();
                        if (tempStationList.Count > 0)
                        {
                            tempStation = tempStationList[0];
                        }
                        if (tempStation != null)
                        {
                            if (i < this.CdgStationQeen.Rows.Count && null != this.CdgStationQeen.Rows[i])
                            {
                                this.CdgStationQeen.Rows[i].Cells[0].Value = tempStation.Point + "." + tempStation.Wz;
                                //((DataGridViewComboBoxCell)(CdgStationQeen.Rows[i].Cells[0])).Items.Add(tempStation.Point + "." + tempStation.Wz);
                                ((DataGridViewComboBoxCell)(CdgStationQeen.Rows[i].Cells[0])).Value = tempStation.Point + "." + tempStation.Wz;
                            }
                        }
                    }
                    bdgComboxValueChange = true;
                }
                //IList<Jc_DefInfo> tempStation = Model.DEFServiceModel.QueryPointByMACCache(temp.MAC);
                //if (tempStation != null)
                //{
                //    tempStation = tempStation.OrderBy(item => item.Fzh).ToList();
                //    //绑定分站
                //    bdgComboxValueChange = false;
                //    int j = 0;
                //    for (int i = 0; i < tempStation.Count; i++)
                //    {
                //        if (null != this.CdgStationQeen.Rows[j])
                //        {
                //            this.CdgStationQeen.Rows[j].Cells[0].Value = tempStation[i].Point + "." + tempStation[i].Wz;
                //            ((DataGridViewComboBoxCell)(CdgStationQeen.Rows[j].Cells[0])).Items.Add(tempStation[i].Point + "." + tempStation[i].Wz);
                //            ((DataGridViewComboBoxCell)(CdgStationQeen.Rows[j].Cells[0])).Value = tempStation[i].Point + "." + tempStation[i].Wz;
                //            j++;
                //        }
                //    }
                //    bdgComboxValueChange = true;
                //}
            }
            else //如果在测点列表中没有找到，就在搜索到的模块内存中寻找
            {
                //if (DefineCacheMrg.SearchIPModules.Count > 0)
                //{
                //    for (int i = 0; i < DefineCacheMrg.SearchIPModules.Count; i++)
                //    {
                //        if (DefineCacheMrg.SearchIPModules[i].ArrPoint == _ArrPoint)
                //        {
                //            tempSwBoardPoint = (DParamSwBoardPoint)DefineCacheMrg.SearchIPModules[i].PointParams;
                //            CcmbMAC.Text = tempSwBoardPoint.Mac_Param15; //MAC
                //            CcmbIP.Text = tempSwBoardPoint.Ip_Param14;//IP
                //            if (CcmbSwitchTye.Properties.Items.Count > 0)
                //            {
                //                CcmbSwitchTye.SelectedIndex = 0;
                //            }
                //            break;
                //        }
                //    }
                //}
            }

        }
        /// <summary>
        /// 加载默认信息
        /// </summary>
        private void LoadPretermitInf()
        {

            CgbQueenInf.Dock = DockStyle.Fill;
            CgbModuleArribute.Dock = DockStyle.Fill;
            CgbQueenInf.BringToFront();
            int count = 6;// 20170104

            try
            {

                SettingInfo cdt = Model.CONFIGServiceModel.GetConfigFKey("CommNum");
                if (cdt != null)
                {
                    count = int.Parse(cdt.StrValue);
                }
                //if (count < 8)//注释  20170610
                //{
                //    count = 8;
                //}

                //根据配置，获取网络模块可挂接的分站数量  20170610
                //Jc_MacInfo NetworkModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                //if (!string.IsNullOrEmpty(NetworkModule.Bz5))
                //{
                //    int ChanelNumber = 0;
                //    int.TryParse(NetworkModule.Bz5, out ChanelNumber);
                //    count = ChanelNumber;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            CdgStationQeen.Rows.Add(count); //连接分站数可配置?


            IList<Jc_WzInfo> temp = Model.WZServiceModel.QueryWZsCache();
            //加入交换机地址
            //this.CgleSwitchAdress.Properties.View.BestFitColumns();
            //this.CgleSwitchAdress.Properties.DisplayMember = "Wz";
            //this.CgleSwitchAdress.Properties.ValueMember = "WzID";

            DataTable dtTemp = Model.DevAdapter.ListToDataTable(temp);
            if (temp != null)
            {
                this.CgleSwitchAdress.Properties.DataSource = dtTemp;
            }
            //加入默认的绑定队列
            Jc_MacInfo tempMac = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
            List<Jc_DefInfo> TempStation = Model.DEFServiceModel.QueryPointByDevpropertIDCache(0);
            List<Jc_DefInfo> TempStation1 = TempStation.FindAll(a => a.Bz12 == _ArrPoint);
            if (tempMac.Upflag == "0")
            {
                layoutControlItem6.Visibility = LayoutVisibility.Never;

                TempStation1 = TempStation.FindAll(a => a.Jckz1 == _ArrPoint);
            }
            else
            {
                layoutControlItem6.Visibility = LayoutVisibility.Always;
            }
            //维护可操作的分站信息
            if (null != TempStation)
            {
                try
                {
                    TempStation = TempStation.OrderBy(item => item.Fzh).ToList();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                StationTag tempStruct;
                for (int i = 0; i < TempStation.Count; i++)
                {
                    if (tempMac.Upflag == "1")
                    {
                        if (string.IsNullOrEmpty(TempStation[i].Bz12))
                        {
                            tempStruct = new StationTag();
                            tempStruct.PointTag = TempStation[i].Point + "." + TempStation[i].Wz;
                            tempStruct.bActivity = true;
                            tempStationTag.Add(tempStruct);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(TempStation[i].Jckz1))
                        {
                            tempStruct = new StationTag();
                            tempStruct.PointTag = TempStation[i].Point + "." + TempStation[i].Wz;
                            tempStruct.bActivity = true;
                            tempStationTag.Add(tempStruct);
                        }
                    }
                }
                for (int i = 0; i < TempStation1.Count; i++)
                {
                    tempStruct = new StationTag();
                    tempStruct.PointTag = TempStation1[i].Point + "." + TempStation1[i].Wz;
                    tempStruct.bActivity = true;
                    tempStationTag.Add(tempStruct);
                }
            }
            //在dg中加入分站信息
            DataGridViewComboBoxCell tempCell;
            DataGridViewComboBoxCell tempCell1;
            DataGridViewComboBoxCell tempCell2;
            DataGridViewComboBoxCell tempCell3;

            for (int i = 0; i < CdgStationQeen.Rows.Count; i++)
            {
                tempCell = (DataGridViewComboBoxCell)(CdgStationQeen.Rows[i].Cells[0]);
                tempCell.Items.Add("");
            }


            if (tempStationTag.Count > 0)
            {
                for (int i = 0; i < tempStationTag.Count; i++)
                {
                    for (int j = 0; j < CdgStationQeen.Rows.Count; j++)
                    {
                        tempCell = (DataGridViewComboBoxCell)(CdgStationQeen.Rows[j].Cells[0]);
                        tempCell.Items.Add(tempStationTag[i].PointTag);
                    }
                }
            }

            //加载模块工作模式 1//0：UDP,1：TCPClient,2：UDPServer, 3：TCP Server, 4：HTTPD Client 
            CcmbWorkMode.Properties.Items.Add("UDP方式");
            CcmbWorkMode.Properties.Items.Add("TCP客户端");
            CcmbWorkMode.Properties.Items.Add("UDP服务器");
            CcmbWorkMode.Properties.Items.Add("TCP服务器");
            CcmbWorkMode.Properties.Items.Add("HTTPD客户端");

            //流量控制 1
            CcmbFlowControl.Properties.Items.Add("无流量控制");
            CcmbFlowControl.Properties.Items.Add("硬件流制(CTS/RTS)");
            CcmbFlowControl.Properties.Items.Add("RS485");


            //模块波特率 1
            CcmbBautrate.Properties.Items.Add("1200");
            CcmbBautrate.Properties.Items.Add("2400");
            CcmbBautrate.Properties.Items.Add("4800");
            CcmbBautrate.Properties.Items.Add("9600");
            CcmbBautrate.Properties.Items.Add("19200");
            CcmbBautrate.Properties.Items.Add("28800");
            CcmbBautrate.Properties.Items.Add("38400");
            CcmbBautrate.Properties.Items.Add("115200");


            //校验位 1
            CcmbCheckBit.Properties.Items.Add("无");
            CcmbCheckBit.Properties.Items.Add("奇效验");
            CcmbCheckBit.Properties.Items.Add("偶效验");
            CcmbCheckBit.Properties.Items.Add("标记");
            CcmbCheckBit.Properties.Items.Add("空格");


            //数据位 1
            CcmbDataBit.Properties.Items.Add("6");
            CcmbDataBit.Properties.Items.Add("7");
            CcmbDataBit.Properties.Items.Add("8");


            //停止位 1
            CcmbStopBit.Properties.Items.Add("1");
            CcmbStopBit.Properties.Items.Add("2");


        }
        /// <summary>
        /// 等待窗体
        /// </summary>
        private Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm WaitDialogFormTemp;
        /// <summary>
        /// 用于更新UI的委托定义
        /// </summary>
        private delegate void UpdateControl();
        List<Jc_MacInfo> allMacList = new List<Jc_MacInfo>();
        /// <summary>
        /// 高级属性按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnModuleAtrribut_Click(object sender, EventArgs e)
        {
            try
            {

                //if (!string.IsNullOrEmpty(CcmbMAC.Text))
                //{
                //    getModuleArributeFromDev(CcmbMAC.Text);
                //}


                CbtnModuleAtrribut.Enabled = false;
                try
                {
                    WaitDialogFormTemp = new Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm("搜索交换机", "正在搜索交换机,请稍后......");
                    UpdateControl task = SearchIP;
                    task.BeginInvoke(null, null);

                    for (int i = 0; i < WaitDialogFormTemp.progressShow.Properties.Maximum; i++)
                    {
                        //处理当前消息队列中的所有windows消息
                        Application.DoEvents();
                        Thread.Sleep(30);
                        WaitDialogFormTemp.progressShow.PerformStep();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    WaitDialogFormTemp.Close();
                    Jc_MacInfo macInfo = allMacList.Find(a => a.MAC == CcmbMAC.Text);
                    if (macInfo != null)
                    {
                        CtxbModuleIP.Text = macInfo.IP;
                        CtxbModuleGetWay.Text = macInfo.GatewayIp;
                        CtxbModuleMark.Text = macInfo.SubMask;

                        CbtnUpdateModuleInf.Enabled = true;
                    }
                    else
                    {
                        CbtnUpdateModuleInf.Enabled = false;
                    }
                }
                CbtnModuleAtrribut.Enabled = true;

            }
            catch (Exception ex)
            {
                LogHelper.Error("高级属性按钮【CbtnModuleAtrribut_Click】", ex);
            }

        }
        /// <summary>
        /// 搜索IP
        /// </summary>
        private void SearchIP()
        {
            try
            {
                //MACServiceModel.SearchALLIPCache();// 20170112
                allMacList = MACServiceModel.SearchALLIPCache8962(0);// 20170112
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ModuleInfoverify())
                {
                    return;
                }
                //如果在当前安装位置信息中没有找到交换机位置 直接创建一个安装位置
                #region 先处理安装位置
                Jc_WzInfo tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CgleSwitchAdress.Text);
                if (null == tempWz)
                {
                    tempWz = new Jc_WzInfo();
                    tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID
                    //tempWz.WZID = Convert.ToInt64(this.CgleSwitchAdress.EditValue); //wzID xuzp20151109
                    tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
                    tempWz.Wz = this.CgleSwitchAdress.Text; //wz
                    tempWz.CreateTime = DateTime.Now;// 20170331
                    tempWz.InfoState = InfoState.AddNew;
                    try
                    {
                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz))//添加安装位置
                        {
                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                Jc_MacInfo temp = new Jc_MacInfo();
                temp.MAC = CcmbMAC.Text;//MAC
                temp.IP = CcmbIP.Text;//IP
                temp.Wz = tempWz.Wz;//所属交换机
                temp.Wzid = tempWz.WzID;
                temp.Istmcs = (Int16)((CckTransPort.Checked) ? 1 : 0);
                temp.Type = 0;
                temp.Bz3 = bz3;
                temp.Bz2 = bz2;
                temp.Bz4 = ck_dyx.Checked ? "1" : "0";
                if (temp.Upflag == "0")
                {
                    temp.Bz1 = UpdateStationInf();//更新绑定分站队列
                }
                else
                {
                    temp.Bz1 = UpdateStationInf1();//更新绑定分站队列
                }
                temp.Bz6 = SwitchComIP.Text;


                Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                if (ExistIPModule != null)
                {
                    //表示更新  
                    if (ExistIPModule != temp)
                    {
                        temp.ID = ExistIPModule.ID;
                        temp.InfoState = InfoState.Modified;
                        temp.Bz5 = ExistIPModule.Bz5;
                        temp.Upflag = ExistIPModule.Upflag;

                        try
                        {
                            Model.MACServiceModel.UpdateMACCache(temp);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        OperateLogHelper.InsertOperateLog(8, CONFIGServiceModel.UpdateMacLogs(ExistIPModule, temp), "");// 20170111                      
                    }
                }
                else
                {
                    //表示新增
                    temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    temp.InfoState = InfoState.AddNew;
                    if (string.IsNullOrEmpty(temp.Upflag))
                    {
                        temp.Upflag = "1";
                    }
                    try
                    {
                        Model.MACServiceModel.AddMACCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OperateLogHelper.InsertOperateLog(8, CONFIGServiceModel.AddMacLogs(temp), "");// 20170111
                }
                this.Close();

            }
            catch (Exception ex)
            {
                LogHelper.Error("提交IP模块信息【CbtnSubmit_Click】", ex);
            }
        }
        /// <summary>
        /// 取消保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// IP模块信息验证
        /// </summary>
        /// <returns></returns>
        private bool ModuleInfoverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(CgleSwitchAdress.Text))
            {
                XtraMessageBox.Show("请填写交换机位置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CgleSwitchAdress.Text))
            {
                XtraMessageBox.Show("交换机位置中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CgleSwitchAdress.Text.Length > 30) //xuzp20151126
            {
                XtraMessageBox.Show("交换机位置长度不能超过30个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbMAC.Text))
            {
                XtraMessageBox.Show("前选择MAC地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (!IsRightMac(CcmbMAC.Text))
            {
                XtraMessageBox.Show("MAC地址不合法！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbIP.Text))
            {
                XtraMessageBox.Show("请选择IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsRightIP(CcmbIP.Text))
            {
                XtraMessageBox.Show("IP地址不合法！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }

            ret = true;
            return ret;
        }
        private bool IsRightMac(string mac)
        {
            bool isRightMac = true;
            string[] macArr = mac.Split('.');
            foreach (string tempmac in macArr)
            {
                try
                {
                    Convert.ToInt32(tempmac, 16);
                }
                catch
                {
                    isRightMac = false;
                    break;
                }
            }
            return isRightMac;
        }
        /// <summary>
        /// 修改分站队列对分站链表的影响
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdgStationQeen_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    DataGridViewComboBoxCell tempCell;
                    DataGridViewComboBoxCell tempCell2;
                    DataGridViewComboBoxCell tempCell3;
                    DataGridViewComboBoxCell tempCell4;

                    bool bAddTag;
                    if (CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null) //如果当前点为空
                    {
                        if (!string.IsNullOrEmpty(SelectedSubStation)) //之前点不为空
                        {
                            bAddTag = true;
                            for (int j = 0; j < tempStationTag.Count; j++)
                            {
                                if (tempStationTag[j].PointTag == SelectedSubStation)
                                {
                                    tempStationTag[j].bActivity = true; //将之前点置为标记置为true
                                    bAddTag = false;
                                    break;
                                }
                            }
                            if (bAddTag)
                            {
                                StationTag temp = new StationTag();
                                temp.bActivity = true;
                                temp.PointTag = SelectedSubStation;//如果没有在之前点找到 则新增
                                tempStationTag.Add(temp);
                            }
                        }
                    }
                    else
                    { //如果当前点不为空
                        if (!string.IsNullOrEmpty(SelectedSubStation))   //之前点不为空
                        {
                            if (SelectedSubStation != CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())  //如果两个点不相等
                            {
                                bAddTag = true;
                                for (int j = 0; j < tempStationTag.Count; j++)
                                {
                                    if (tempStationTag[j].PointTag == SelectedSubStation)
                                    {
                                        tempStationTag[j].bActivity = true;
                                        bAddTag = false;
                                        break;
                                    }
                                }
                                if (bAddTag)
                                {
                                    StationTag temp = new StationTag();
                                    temp.bActivity = true;
                                    temp.PointTag = SelectedSubStation;//如果没有在之前点找到 则新增
                                    tempStationTag.Add(temp);
                                }
                                for (int j = 0; j < tempStationTag.Count; j++)
                                {
                                    if (tempStationTag[j].PointTag == CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                                    {
                                        tempStationTag[j].bActivity = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else //之前点为空
                        {
                            for (int j = 0; j < tempStationTag.Count; j++)
                            {
                                if (tempStationTag[j].PointTag == CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    tempStationTag[j].bActivity = false;
                                    break;
                                }
                            }
                        }
                    }


                    //从新加载分站
                    for (int i = 0; i < tempStationTag.Count; i++)
                    {
                        if (tempStationTag[i].bActivity == true)
                        {
                            for (int j = 0; j < CdgStationQeen.Rows.Count; j++)
                            {
                                tempCell = (DataGridViewComboBoxCell)CdgStationQeen.Rows[j].Cells[0];
                                if (!tempCell.Items.Contains(tempStationTag[i].PointTag))
                                {
                                    tempCell.Items.Add(tempStationTag[i].PointTag);
                                }
                            }
                        }
                        else
                        {
                            //1
                            for (int j = 0; j < CdgStationQeen.Rows.Count; j++)
                            {
                                tempCell = (DataGridViewComboBoxCell)CdgStationQeen.Rows[j].Cells[0];
                                if (tempCell.Items.Contains(tempStationTag[i].PointTag))
                                {
                                    if (null != CdgStationQeen.Rows[j].Cells[0].Value)
                                    {
                                        if (CdgStationQeen.Rows[j].Cells[0].Value.ToString() != tempStationTag[i].PointTag)
                                        {
                                            tempCell.Items.Remove(tempStationTag[i].PointTag);
                                        }
                                    }
                                    else
                                    {
                                        tempCell.Items.Remove(tempStationTag[i].PointTag);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("修改分站队列对分站链表的影响【CdgStationQeen_CellValueChanged】", ex);
            }
        }
        /// <summary>
        /// 得到当前配置的分站队列
        /// </summary>
        /// <returns></returns>
        private IList<Jc_DefInfo> getSubQueen()
        {
            IList<Jc_DefInfo> ret = new List<Jc_DefInfo>();
            Jc_DefInfo temp = new Jc_DefInfo();
            for (int i = 0; i < CdgStationQeen.Rows.Count; i++)
            {
                if (null != CdgStationQeen.Rows[i].Cells[0].Value)
                {
                    if (!string.IsNullOrEmpty(CdgStationQeen.Rows[i].Cells[0].Value.ToString()))
                    {
                        temp = Model.DEFServiceModel.QueryPointByCodeCache(CdgStationQeen.Rows[i].Cells[0].Value.ToString().Substring(0, CdgStationQeen.Rows[i].Cells[0].Value.ToString().IndexOf('.')));
                        if (temp != null)
                        {
                            ret.Add(temp);
                        }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 修改分站信息
        /// </summary>
        /// <param name="SubQueen"></param>
        private string UpdateStationInf()
        {
            Jc_DefInfo temp;
            List<Jc_DefInfo> SubstationList;
            string bz1 = "";//绑定分站队列          

            string ret = "";

            for (int i = 0; i < CdgStationQeen.Rows.Count; i++)
            {
                if (null == CdgStationQeen.Rows[i].Cells[0].Value)
                {
                    bz1 += "0|";
                    continue;
                }
                if (string.IsNullOrEmpty(CdgStationQeen.Rows[i].Cells[0].Value.ToString()))
                {
                    bz1 += "0|";
                    continue;
                }
                temp = Model.DEFServiceModel.QueryPointByCodeCache(CdgStationQeen.Rows[i].Cells[0].Value.ToString().Substring(0, CdgStationQeen.Rows[i].Cells[0].Value.ToString().IndexOf('.')));
                if (null == temp)
                {
                    bz1 += "0|";
                    continue;
                }
                bz1 += temp.Fzh.ToString() + "|";
                if (temp.Jckz1 != CcmbMAC.Text || temp.Jckz2 != CcmbIP.Text)
                {
                    temp.Jckz1 = CcmbMAC.Text;
                    temp.Jckz2 = CcmbIP.Text;
                    temp.K3 = 1;
                    temp.InfoState = InfoState.Modified;
                    try
                    {
                        Model.DEFServiceModel.UpdateDEFCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(bz1))
            {
                if (bz1.Contains('|'))
                {
                    if (bz1.LastIndexOf('|') == bz1.Length - 1)
                    {
                        bz1 = bz1.Substring(0, bz1.Length - 1);
                    }
                }
            }

            ret = bz1;

            return ret;
        }
        private string UpdateStationInf1()
        {
            Jc_DefInfo temp;
            List<Jc_DefInfo> SubstationList;
            string bz1 = "";//绑定分站队列          

            string ret = "";

            for (int i = 0; i < CdgStationQeen.Rows.Count; i++)
            {
                if (null == CdgStationQeen.Rows[i].Cells[0].Value)
                {
                    bz1 += "0|";
                    continue;
                }
                if (string.IsNullOrEmpty(CdgStationQeen.Rows[i].Cells[0].Value.ToString()))
                {
                    bz1 += "0|";
                    continue;
                }
                temp = Model.DEFServiceModel.QueryPointByCodeCache(CdgStationQeen.Rows[i].Cells[0].Value.ToString().Substring(0, CdgStationQeen.Rows[i].Cells[0].Value.ToString().IndexOf('.')));
                if (null == temp)
                {
                    bz1 += "0|";
                    continue;
                }
                bz1 += temp.Fzh.ToString() + "|";
                if (temp.Bz12 != CcmbMAC.Text || temp.Bz13 != CcmbIP.Text)
                {
                    temp.Bz12 = CcmbMAC.Text;
                    temp.Bz13 = CcmbIP.Text;
                    temp.K3 = 1;
                    temp.InfoState = InfoState.Modified;
                    try
                    {
                        Model.DEFServiceModel.UpdateDEFCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                }
            }

            if (!string.IsNullOrEmpty(bz1))
            {
                if (bz1.Contains('|'))
                {
                    if (bz1.LastIndexOf('|') == bz1.Length - 1)
                    {
                        bz1 = bz1.Substring(0, bz1.Length - 1);
                    }
                }
            }

            ret = bz1;

            //移除未绑定的分站
            List<Jc_DefInfo> defList = DEFServiceModel.QueryPointByDevpropertIDCache(0).FindAll(a => a.Upflag != "1" && a.Bz12 == CcmbMAC.Text);
            foreach (Jc_DefInfo def in defList)
            {
                if (!bz1.Split('|').Contains(def.Fzh.ToString()))
                {
                    def.Bz12 = "";
                    def.Bz13 = "";
                    def.InfoState = InfoState.Modified;
                    try
                    {
                        Model.DEFServiceModel.UpdateDEFCache(def);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 从设备中得到模块属性
        /// </summary>
        private void getModuleArributeFromDev(string strmac)
        {


            NetDeviceSettingInfo pConvSetting = new NetDeviceSettingInfo();
            pConvSetting = Model.MACServiceModel.GetConvSetting(strmac, 2000);
            if (pConvSetting == null)
            {
                XtraMessageBox.Show("读取网络设备参数失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (pConvSetting.SockSetting == null || pConvSetting.SockSetting.Length == 0)
            {
                XtraMessageBox.Show("读取网络设备参数失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (string.IsNullOrEmpty(pConvSetting.SockSetting[0].IpServer))
            //{
            //    XtraMessageBox.Show("读取网络设备参数失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}



            CbtnUpdateModuleInf.Enabled = true;
            srcPack = pConvSetting.NetSetting.srcPacket;
            SocketsrcPack = pConvSetting.SockSetting[0].srcPacket;
            #region =========================服务属性=========================
            CtxbSrvSendTime.Text = pConvSetting.ComSetting[0].MinSendTime.ToString();//服务 最小发送时间
            CtxbSrvIP.Text = pConvSetting.SockSetting[0].IpServer;//服务器IP 本机Ip或双机热备虚拟IP
            CtxbSrvPort.Text = pConvSetting.SockSetting[0].ServerPort.ToString();//服务器端口


            #endregion

            //if (pConvSetting[0].sockSetting[0].bUseOcx == 1) ck_IsUseOcx.Checked = true;//模块是否使用虚拟串口
            //else ck_IsUseOcx.Checked = false;

            #region =========================模块属性=========================
            CtxbModuleIP.Text = pConvSetting.NetSetting.IpAddr; //模块IP
            ModuleIPNow = CtxbModuleIP.Text;//赋值当前操作的模块IP
            CtxbModuleMark.Text = pConvSetting.NetSetting.SubMask;//模块子网掩码
            CtxbModuleGetWay.Text = pConvSetting.NetSetting.GatewayIp;//模块网关


            CtxbModulePort.Text = pConvSetting.SockSetting[0].Port.ToString();//模块端口
            CcmbWorkMode.SelectedIndex = pConvSetting.SockSetting[0].Mode;//模块 工作模式

            if (pConvSetting.ComSetting[0].FlowMode == 1) CcmbFlowControl.Text = "无流量控制";
            else if (pConvSetting.ComSetting[0].FlowMode == 3) CcmbFlowControl.Text = "硬件流制(CTS/RTS)";
            else if (pConvSetting.ComSetting[0].FlowMode == 5) CcmbFlowControl.Text = "RS485";
            //CcmbFlowControl.SelectedIndex = (int)pConvSetting.ComSetting[0].FlowMode;//模块流量控制
            CcmbBautrate.Text = pConvSetting.ComSetting[0].Baudrate.ToString();//模块波特率
            CcmbCheckBit.SelectedIndex = (int)(pConvSetting.ComSetting[0].CheckMode - 1);//模块校验位
            CcmbDataBit.Text = pConvSetting.ComSetting[0].Databit.ToString();//模块数据位
            CcmbStopBit.Text = pConvSetting.ComSetting[0].StopBit.ToString();//模块停止位
            CtxbSendBit.Text = pConvSetting.ComSetting[0].MinSendByte.ToString(); //模块最小发送字数



            #endregion

        }
        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnUpdateModuleInf_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Basic.Framework.Common.ValidationHelper.IsRightIP(CtxbModuleIP.Text))
                {
                    XtraMessageBox.Show("模块IP输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //if (!Basic.Framework.Common.ValidationHelper.IsRightIP(CtxbSrvIP.Text))
                //{
                //    XtraMessageBox.Show("服务器IP输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbSrvPort.Text))
                //{
                //    XtraMessageBox.Show("服务端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbModulePort.Text))
                //{
                //    XtraMessageBox.Show("模块端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                CbtnUpdateModuleInf.Enabled = false;
                CbtnModuleAtrribut.Enabled = false;
                NetDeviceSettingInfo pConvSetting = new NetDeviceSettingInfo();
                pConvSetting.SockSetting = new SocketSetting[8];
                pConvSetting.ComSetting = new ComSetting[8];

                //目前主要设置交换机的IP、子网掩码、网关信息
                pConvSetting.NetSetting = new Netsetting();
                pConvSetting.NetSetting.IpAddr = CtxbModuleIP.Text; //模块 IP
                pConvSetting.NetSetting.SubMask = CtxbModuleMark.Text;//模块 子网掩码
                pConvSetting.NetSetting.GatewayIp = CtxbModuleGetWay.Text; //模块 网关

                #region 未使用
                pConvSetting.NetSetting.srcPacket = srcPack;
                pConvSetting.NetSetting.IsUseStaticIP = 1;

                pConvSetting.NetSetting.DnsIp = textDnsIP.Text;//模块 DNS
                pConvSetting.NetSetting.NetWorkName = textModuleName.Text;//模块名称
                pConvSetting.NetSetting.User = textUserName.Text;//用户名
                pConvSetting.NetSetting.PassWord = textPassword.Text;//密码
                pConvSetting.SockSetting[0] = new SocketSetting();
                pConvSetting.SockSetting[0].srcPacket = SocketsrcPack;
                pConvSetting.SockSetting[0].IpServer = CtxbSrvIP.Text;//服务 IP
                pConvSetting.SockSetting[0].Mode = CcmbWorkMode.SelectedIndex;//模块 工作模式
                pConvSetting.SockSetting[0].IsUseOcx = 0;// 模块 是否使用虚拟串口(0:表示使用，1：表示不使用)
                pConvSetting.SockSetting[0].ServerPort = Convert.ToInt32(string.IsNullOrEmpty(CtxbSrvPort.Text) ? "0" : CtxbSrvPort.Text); //服务端口
                pConvSetting.SockSetting[0].Port = Convert.ToInt32(string.IsNullOrEmpty(CtxbModulePort.Text) ? "0" : CtxbModulePort.Text); //模块端口               
                pConvSetting.ComSetting[0] = new ComSetting();
                pConvSetting.ComSetting[0].Baudrate = Convert.ToUInt32(string.IsNullOrEmpty(CcmbBautrate.Text) ? "0" : CcmbBautrate.Text);//模块波特率     
                pConvSetting.ComSetting[0].CheckMode = (uint)(CcmbCheckBit.SelectedIndex + 1);//模块校验位
                pConvSetting.ComSetting[0].Databit = Convert.ToUInt32(string.IsNullOrEmpty(CcmbDataBit.Text) ? "0" : CcmbDataBit.Text); ;//模块数据位
                pConvSetting.ComSetting[0].StopBit = Convert.ToUInt32(string.IsNullOrEmpty(CcmbStopBit.Text) ? "0" : CcmbStopBit.Text);//模块停止位c\

                if (CcmbFlowControl.Text == "无流量控制") pConvSetting.ComSetting[0].FlowMode = 1;
                else if (CcmbFlowControl.Text == "硬件流制(CTS/RTS)") pConvSetting.ComSetting[0].FlowMode = 3;
                else if (CcmbFlowControl.Text == "RS485") pConvSetting.ComSetting[0].FlowMode = 5;
                else pConvSetting.ComSetting[0].FlowMode = 5;

                //pConvSetting.ComSetting[0].FlowMode = (uint)CcmbFlowControl.SelectedIndex;//流量模式
                pConvSetting.ComSetting[0].MinSendTime = Convert.ToUInt32(string.IsNullOrEmpty(CtxbSrvSendTime.Text) ? "0" : CtxbSrvSendTime.Text); //最小发送时间
                pConvSetting.ComSetting[0].MinSendByte = Convert.ToUInt32(string.IsNullOrEmpty(CtxbSendBit.Text) ? "0" : CtxbSendBit.Text); ;//最小发送字数
                #endregion

                if (Model.MACServiceModel.SetConvSetting(CcmbMAC.Text, pConvSetting, 8000, "0"))
                {
                    CcmbIP.Text = CtxbModuleIP.Text;
                    //设置网络模块参数成功的同时，更新缓存及数据库的模块IP地址  20171220
                    if (CtxbModuleIP.Text != ModuleIPNow)
                    {
                        //更新网络模块IP地址
                        Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                        if (ExistIPModule != null)
                        {
                            //表示更新  
                            ExistIPModule.IP = CtxbModuleIP.Text;
                            ExistIPModule.InfoState = InfoState.Modified;
                            try
                            {
                                Model.MACServiceModel.UpdateMACCache(ExistIPModule);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            ModuleIPNow = CtxbModuleIP.Text;
                        }
                    }
                    XtraMessageBox.Show("设置网络设备参数成功！", "提示");
                    timerSetButtonState.Enabled = true;
                    timerSetButtonState.Interval = 10000;
                    timerSetButtonState.Tick += SetButtonEnabled;
                    timerSetButtonState.Start();
                }
                else
                {
                    CbtnUpdateModuleInf.Enabled = true;
                    XtraMessageBox.Show("设置网络设备参数失败！", "警告");
                }
            }
            catch (Exception ex)
            {
                CbtnUpdateModuleInf.Enabled = true;
                LogHelper.Error("更新模块信息【CbtnUpdateModuleInf_Click】", ex);
            }
        }
        private void SetButtonEnabled(object sender, EventArgs e)
        {
            CbtnUpdateModuleInf.Enabled = true;
            CbtnModuleAtrribut.Enabled = true;
            timerSetButtonState.Stop();
        }
        /// <summary>
        /// 点击IP模块banding的datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdgStationQeen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    if (CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    {
                        SelectedSubStation = "";
                    }
                    else
                    {
                        SelectedSubStation = CdgStationQeen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("点击IP模块banding的datagridview【CdgStationQeen_CellClick】", ex);
            }
        }

        private void CpDown_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CgleSwitchAdress_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    string displayName = this.CgleSwitchAdress.Properties.DisplayMember;
                    string valueName = this.CgleSwitchAdress.Properties.ValueMember;
                    string display = e.DisplayValue.ToString();
                    if (string.IsNullOrEmpty(display)) //xuzp20151023
                    {
                        return;
                    }
                    DataTable dtTemp = this.CgleSwitchAdress.Properties.DataSource as DataTable;
                    if (dtTemp != null)
                    {
                        DataRow[] selectedRows = dtTemp.Select(string.Format("{0}='{1}'", displayName, display.Replace("'", "‘")));
                        if (selectedRows == null || selectedRows.Length == 0)
                        {
                            DataRow row = dtTemp.NewRow();
                            row[displayName] = display;
                            row[valueName] = WZServiceModel.GetMaxWzidInCache(dtTemp) + 1; //xuzp20151109
                            dtTemp.Rows.Add(row);
                            dtTemp.AcceptChanges();
                        }
                    }

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 删除网络模块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNetworkModuleDelte_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CcmbMAC.Text))
                {
                    XtraMessageBox.Show("交换机MAC不存在,无法删除!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mac = CcmbMAC.Text;
                    NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                    NetworkModuleRequest.Mac = mac;
                    var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                    Jc_MacInfo tempMac = null;
                    if (result.Data != null)
                    {
                        tempMac = result.Data[0];
                    }
                    if (tempMac != null)
                    {
                        if (!DefineYX(tempMac.Bz1))//如果网络模块未绑定分站，才允许删除　 20170420
                        {
                            NetworkModuleDeleteByMacRequest NetworkModuleDeleteRequest = new NetworkModuleDeleteByMacRequest();
                            NetworkModuleDeleteRequest.Mac = tempMac.MAC;
                            var resultDelete = _NetworkModuleService.DeleteNetworkModule(NetworkModuleDeleteRequest);
                            if (resultDelete.Code != 100)
                            {
                                XtraMessageBox.Show(resultDelete.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("网络模块已绑定分站，请先解除网络模块的分站绑定！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        public bool DefineYX(string s)//txy 20160803
        {
            //20170323 modified by  当s参数为空串时，之前会返回TRUE，导致未绑定的MAC删除不掉
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            bool flg = false;
            string[] com;
            string[] fzh;
            try
            {
                com = s.Split(';');
                for (int i = 0; i < com.Length; i++)
                {
                    fzh = com[i].Split('|');
                    for (int j = 0; j < fzh.Length; j++)
                    {
                        if (fzh[j] != "0")
                        {
                            flg = true;
                            break;
                        }
                    }
                }

            }
            catch
            { }
            return flg;
        }


        private void CbtnUpdateDevioceInf_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Basic.Framework.Common.ValidationHelper.IsRightIP(CtxbModuleIP.Text))
                {
                    XtraMessageBox.Show("模块IP输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Basic.Framework.Common.ValidationHelper.IsRightIP(CtxbSrvIP.Text))
                {
                    XtraMessageBox.Show("服务器IP输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbSrvPort.Text))
                {
                    XtraMessageBox.Show("服务端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!Basic.Framework.Common.ValidationHelper.IsNumber(CtxbModulePort.Text))
                {
                    XtraMessageBox.Show("模块端口输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                CbtnUpdateModuleInf.Enabled = false;
                CbtnModuleAtrribut.Enabled = false;
                NetDeviceSettingInfo pConvSetting = new NetDeviceSettingInfo();
                pConvSetting.SockSetting = new SocketSetting[8];
                pConvSetting.ComSetting = new ComSetting[8];
                pConvSetting.NetSetting = new Netsetting();
                pConvSetting.NetSetting.srcPacket = srcPack;
                pConvSetting.NetSetting.IsUseStaticIP = 1;
                pConvSetting.NetSetting.IpAddr = CtxbModuleIP.Text; //模块 IP
                pConvSetting.NetSetting.SubMask = CtxbModuleMark.Text;//模块 子网掩码
                pConvSetting.NetSetting.GatewayIp = CtxbModuleGetWay.Text; //模块 网关
                pConvSetting.NetSetting.DnsIp = "0.0.0.0";//模块 DNS
                pConvSetting.SockSetting[0] = new SocketSetting();
                pConvSetting.SockSetting[0].IpServer = CtxbSrvIP.Text;//服务 IP
                pConvSetting.SockSetting[0].Mode = CcmbWorkMode.SelectedIndex;//模块 工作模式
                pConvSetting.SockSetting[0].IsUseOcx = 0;// 模块 是否使用虚拟串口(0:表示使用，1：表示不使用)
                pConvSetting.SockSetting[0].ServerPort = Convert.ToInt32(CtxbSrvPort.Text); //服务端口
                pConvSetting.SockSetting[0].Port = Convert.ToInt32(CtxbModulePort.Text); //模块端口
                pConvSetting.ComSetting[0] = new ComSetting();
                pConvSetting.ComSetting[0].Baudrate = Convert.ToUInt32(CcmbBautrate.Text);//模块波特率     
                pConvSetting.ComSetting[0].CheckMode = (uint)(CcmbCheckBit.SelectedIndex + 1);//模块校验位
                pConvSetting.ComSetting[0].Databit = Convert.ToUInt32(CcmbDataBit.Text); ;//模块数据位
                pConvSetting.ComSetting[0].StopBit = Convert.ToUInt32(CcmbStopBit.Text);//模块停止位c\

                if (CcmbFlowControl.Text == "无流量控制") pConvSetting.ComSetting[0].FlowMode = 1;
                else if (CcmbFlowControl.Text == "硬件流制(CTS/RTS)") pConvSetting.ComSetting[0].FlowMode = 3;
                else if (CcmbFlowControl.Text == "RS485") pConvSetting.ComSetting[0].FlowMode = 5;
                else pConvSetting.ComSetting[0].FlowMode = 5;

                //pConvSetting.ComSetting[0].FlowMode = (uint)CcmbFlowControl.SelectedIndex;//流量模式
                pConvSetting.ComSetting[0].MinSendTime = Convert.ToUInt32(CtxbSrvSendTime.Text); //最小发送时间
                pConvSetting.ComSetting[0].MinSendByte = Convert.ToUInt32(CtxbSendBit.Text); ;//最小发送字数

                if (Model.MACServiceModel.SetConvSetting(CcmbMAC.Text, pConvSetting, 8000, "0"))
                {
                    CcmbIP.Text = CtxbModuleIP.Text;
                    //设置网络模块参数成功的同时，更新缓存及数据库的模块IP地址  20171220
                    if (CtxbModuleIP.Text != ModuleIPNow)
                    {
                        //更新网络模块IP地址
                        Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                        if (ExistIPModule != null)
                        {
                            //表示更新  
                            ExistIPModule.IP = CtxbModuleIP.Text;
                            ExistIPModule.InfoState = InfoState.Modified;
                            try
                            {
                                Model.MACServiceModel.UpdateMACCache(ExistIPModule);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            ModuleIPNow = CtxbModuleIP.Text;
                        }
                    }
                    XtraMessageBox.Show("设置网络设备参数成功！", "提示");
                    timerSetButtonState.Enabled = true;
                    timerSetButtonState.Interval = 10000;
                    timerSetButtonState.Tick += SetButtonEnabled;
                    timerSetButtonState.Start();
                }
                else
                {
                    CbtnUpdateModuleInf.Enabled = true;
                    XtraMessageBox.Show("设置网络设备参数失败！", "警告");
                }
            }
            catch (Exception ex)
            {
                CbtnUpdateModuleInf.Enabled = true;
                LogHelper.Error("更新模块信息【CbtnUpdateModuleInf_Click】", ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!ModuleInfoverify())
                {
                    return;
                }
                //如果在当前安装位置信息中没有找到交换机位置 直接创建一个安装位置
                #region 先处理安装位置
                Jc_WzInfo tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CgleSwitchAdress.Text);
                if (null == tempWz)
                {
                    tempWz = new Jc_WzInfo();
                    tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID
                    //tempWz.WZID = Convert.ToInt64(this.CgleSwitchAdress.EditValue); //wzID xuzp20151109
                    tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
                    tempWz.Wz = this.CgleSwitchAdress.Text; //wz
                    tempWz.CreateTime = DateTime.Now;// 20170331
                    tempWz.InfoState = InfoState.AddNew;
                    try
                    {
                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz))//添加安装位置
                        {
                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                Jc_MacInfo temp = new Jc_MacInfo();
                temp.MAC = CcmbMAC.Text;//MAC
                temp.IP = CcmbIP.Text;//IP
                temp.Wz = tempWz.Wz;//所属交换机
                temp.Wzid = tempWz.WzID;
                temp.Istmcs = (Int16)((CckTransPort.Checked) ? 1 : 0);
                temp.Type = 0;
                temp.Bz3 = bz3;
                temp.Bz2 = bz2;
                temp.Bz4 = ck_dyx.Checked ? "1" : "0";
                temp.Bz1 = UpdateStationInf();//更新绑定分站队列


                //增加智能电源箱判断，同一交换机，只能勾选一次智能电源箱  20170404
                if (ck_dyx.Checked)
                {
                    List<Jc_MacInfo> MacList = Model.MACServiceModel.QueryMACBybz2Cache(bz2).ToList().FindAll(a => a.Bz4 == "1");
                    if (MacList.Count > 0)
                    {
                        //ck_dyx.Checked = false;
                        temp.Bz4 = "0";//同一交换机下，各个模块只需要设置一次智能电源箱  20170404
                    }
                }
                else
                {
                    //如果取消勾选智能电源箱，则取消当前交换机下面所有勾选了智能电源箱的模块  20170404
                    List<Jc_MacInfo> MacList = Model.MACServiceModel.QueryMACBybz2Cache(bz2).ToList().FindAll(a => a.Bz4 == "1");
                    if (MacList.Count > 0)
                    {
                        List<Jc_MacInfo> UpdateList = new List<Jc_MacInfo>();
                        for (int i = 0; i < MacList.Count; i++)
                        {
                            if (MacList[i].MAC != temp.MAC)//如果不是当前模块，则取消勾选智能电源箱
                            {
                                Jc_MacInfo tempMAC = MacList[i];
                                tempMAC.Bz4 = "0";
                                tempMAC.InfoState = InfoState.Modified;
                                UpdateList.Add(tempMAC);
                            }
                        }
                        if (UpdateList.Count > 0)
                        {
                            try
                            {
                                Model.MACServiceModel.UpdateMACsCache(UpdateList);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(_ArrPoint);
                if (ExistIPModule != null)
                {
                    //表示更新  
                    if (ExistIPModule != temp)
                    {
                        temp.ID = ExistIPModule.ID;
                        temp.InfoState = InfoState.Modified;
                        temp.Bz5 = ExistIPModule.Bz5;
                        try
                        {
                            Model.MACServiceModel.UpdateMACCache(temp);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        OperateLogHelper.InsertOperateLog(8, CONFIGServiceModel.UpdateMacLogs(ExistIPModule, temp), "");// 20170111

                        #region 修改模块下所有该交换机的安装位置  20170112
                        if (!string.IsNullOrEmpty(bz2))
                        {
                            IList<Jc_MacInfo> macs = Model.MACServiceModel.QueryMACBybz2Cache(bz2);
                            if (macs.Count > 0)
                            {
                                List<Jc_MacInfo> tempMacs = new List<Jc_MacInfo>();
                                for (int i = 0; i < macs.Count; i++)
                                {
                                    if (macs[i].Wzid != temp.Wzid && macs[i].Wz != temp.Wz && macs[i].MAC != temp.MAC)//增加位置名称不等的判断  20170331
                                    {
                                        macs[i].Wzid = temp.Wzid;
                                        macs[i].Wz = temp.Wz;
                                        macs[i].InfoState = InfoState.Modified;
                                        tempMacs.Add(macs[i]);
                                    }
                                }
                                try
                                {
                                    Model.MACServiceModel.UpdateMACsCache(tempMacs);
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    //表示新增
                    temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    temp.InfoState = InfoState.AddNew;
                    try
                    {
                        Model.MACServiceModel.AddMACCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OperateLogHelper.InsertOperateLog(8, CONFIGServiceModel.AddMacLogs(temp), "");// 20170111
                }
                this.Close();

            }
            catch (Exception ex)
            {
                LogHelper.Error("提交IP模块信息【CbtnSubmit_Click】", ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CcmbMAC.Text))
                {
                    XtraMessageBox.Show("交换机MAC不存在,无法删除!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mac = CcmbMAC.Text;
                    NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                    NetworkModuleRequest.Mac = mac;
                    var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                    Jc_MacInfo tempMac = null;
                    if (result.Data != null)
                    {
                        tempMac = result.Data[0];
                    }
                    if (tempMac != null)
                    {
                        if (!DefineYX(tempMac.Bz1))//如果网络模块未绑定分站，才允许删除　 20170420
                        {
                            NetworkModuleDeleteByMacRequest NetworkModuleDeleteRequest = new NetworkModuleDeleteByMacRequest();
                            NetworkModuleDeleteRequest.Mac = tempMac.MAC;
                            var resultDelete = _NetworkModuleService.DeleteNetworkModule(NetworkModuleDeleteRequest);
                            if (resultDelete.Code != 100)
                            {
                                XtraMessageBox.Show(resultDelete.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("网络模块已绑定分站，请先解除网络模块的分站绑定！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
