using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.Utils;
using System.Threading;
using System.Diagnostics;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using DevExpress.XtraEditors;
using Sys.Safety.ClientFramework.CBFCommon;

namespace Sys.Safety.Client.Define
{
    public partial class CuNavigation : UserControl
    {
        public CuNavigation()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }
        public CuNavigation(CFPointMrgFrame CFFram)
        {
            _CFFram = CFFram;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }
        private CFPointMrgFrame _CFFram;
        /// <summary>
        /// 窗口传递参数1
        /// </summary>
        public string StrParameter1;
        /// <summary>
        /// 窗口传递参数2
        /// </summary>
        public string StrParameter2;
        /// <summary>
        /// 用于更新UI的委托定义
        /// </summary>
        private delegate void UpdateControl();
        /// <summary>
        /// 后台委托封装
        /// </summary>
        /// <returns></returns>
        private delegate bool dl_DoAsync();
        /// <summary>
        /// 等待窗体
        /// </summary>
        private Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm WaitDialogFormTemp;
        /// <summary>
        /// 用于更新UI委托声明
        /// </summary>
        private UpdateControl dl_updataUI;
        /// <summary>
        ///是否正大搜索交换机标记  20170401
        /// </summary>
        private bool IsSerachIp = false;



        /// <summary>
        /// 处理完成回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackMethod(IAsyncResult ar)
        {
            try
            {
                dl_DoAsync dl_do = (dl_DoAsync)ar.AsyncState;
                dl_do.EndInvoke(ar);
                this.BeginInvoke(dl_updataUI); //更新UI
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 处理后台事件的过程
        /// </summary>
        /// <returns></returns>
        private bool DoAsync()
        {
            #region 此处写后台代码
            try
            {
                //LogHelper.Info("CuNavigation_Load thread id:" + Thread.CurrentThread.ManagedThreadId);
                // return true;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //todo 优化此处代码，InitDevTypeData(); InitDevData(); 加载时间需要5-10秒   20170314
                InitDevTypeData();

                sw.Stop();
                //LogHelper.Info(" DoAsync().InitDevTypeData(): " + sw.ElapsedMilliseconds);

                sw.Restart();

                InitDevData();

                sw.Stop();
                //LogHelper.Info(" DoAsync().InitDevData(): " + sw.ElapsedMilliseconds);

                sw.Restart();

                InitStationData();

                sw.Stop();
                //LogHelper.Info(" DoAsync().InitStationData(): " + sw.ElapsedMilliseconds);


                //InitDevTypeData();
                //InitDevData();
                //InitStationData();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 更新UI
        /// </summary>
        private void updateUI()
        {
            #region 此处写更新UI的代码
            this.CtreeListDevType.KeyFieldName = "ID";
            this.CtreeListDevType.ParentFieldName = "ParentID";
            this.CtreeListDevType.DataSource = TreeListSource.TreeListDevType;
            //this.CtreeListDevType.RefreshDataSource();//注释  20170722
            //this.CtreeListDevType.ExpandAll();

            this.CtreeListDev.KeyFieldName = "ID";
            this.CtreeListDev.ParentFieldName = "ParentID";
            this.CtreeListDev.DataSource = TreeListSource.TreeListDev;
            //this.CtreeListDev.RefreshDataSource();//注释  20170722
            //this.CtreeListDev.ExpandAll();

            this.CtreeLisStation.KeyFieldName = "ID";
            this.CtreeLisStation.ParentFieldName = "ParentID";
            this.CtreeLisStation.DataSource = TreeListSource.TreeListStation;
            //this.CtreeLisStation.RefreshDataSource();//注释  20170722
            this.CtreeLisStation.ExpandAll();

            this.CtreeListWz.KeyFieldName = "ID";
            this.CtreeListWz.ParentFieldName = "ParentID";
            this.CtreeListWz.DataSource = TreeListSource.TreeListWZ;
            //this.CtreeListWz.RefreshDataSource();//注释  20170722
            //this.CtreeListWz.ExpandAll();

            #endregion
        }
        private void CuNavigation_Load(object sender, EventArgs e)
        {
            //LogHelper.Info("CuNavigation_Load thread id:" + Thread.CurrentThread.ManagedThreadId);
            //dl_updataUI = new UpdateControl(updateUI);  //注册更新界面UI的方法
            //dl_DoAsync dl_doThings = new dl_DoAsync(DoAsync);   //注册后台处理函数的方法
            //AsyncCallback callbak = new AsyncCallback(CallBackMethod); //注册异步完成后的函数
            //dl_doThings.BeginInvoke(callbak, dl_doThings); //异步执行
            DoAsync();
            updateUI();
        }
        /// <summary>
        /// 初始化设备类型数据
        /// </summary>
        public void InitDevTypeData()
        {
            TreeListSource.TreeListDevType.Clear();
            TreeListSource.TreeListWZ.Clear();
            int count = 1;
            Dictionary<int, EnumcodeInfo> DevProperties = new Dictionary<int, EnumcodeInfo>();
            Dictionary<int, EnumcodeInfo> DevClassies = new Dictionary<int, EnumcodeInfo>(); ;
            IList<Jc_DevInfo> DevS;
            int ParentID;
            int TempParentID;
            TreeListSource.TreeListDevType.Add(new TreeListItem("所有设备类型", "0", "100", count++, -1));
            TreeListSource.TreeListWZ.Add(new TreeListItem("安装位置管理", "0", "520", count++, -1)); //加入安装位置管理 xuzp20151124

            try
            {
                DevProperties = DEVServiceModel.QueryDevPropertisCache();
                //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                Dictionary<int, EnumcodeInfo> devTypeDic = DEVServiceModel.QueryDevClasiessCache();
                List<EnumcodeInfo> tempdevTypeDic = new List<EnumcodeInfo>();// 20170326
                //IList<Jc_DevInfo> defList = DEVServiceModel.QueryDevsCache();

                if (DevProperties == null)
                {
                    return;
                }
                foreach (var itemDevDevProperties in DevProperties.Values)
                {
                    if (itemDevDevProperties.StrEnumDisplay.Contains("分站"))
                    {
                        TreeListSource.TreeListDevType.Add(new TreeListItem(itemDevDevProperties.StrEnumDisplay, itemDevDevProperties.LngEnumValue.ToString(), "100", count++, 1));
                    }
                    else
                    {
                        TreeListSource.TreeListDevType.Add(new TreeListItem(itemDevDevProperties.StrEnumDisplay, itemDevDevProperties.LngEnumValue.ToString(), "101", count++, 1));
                    }

                    //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                    //这里修改得有问题，每次都加载了所有设备种类型的数据，应该按当前种类过滤   20170326
                    //DevClassies = DEVServiceModel.QueryDevClassByDevpropertID(itemDevDevProperties.LngEnumValue);
                    DevClassies.Clear();
                    tempdevTypeDic = devTypeDic.Values.ToList().FindAll(a => a.LngEnumValue3.ToString() == itemDevDevProperties.LngEnumValue.ToString());
                    if (devTypeDic != null)
                    {   //循环加载得到devTypeDic
                        foreach (EnumcodeInfo item in tempdevTypeDic)// 20170326
                        {
                            if (item.InfoState == InfoState.Delete)
                            {
                                continue;
                            }
                            if (itemDevDevProperties.LngEnumValue.ToString() == item.LngEnumValue3.ToString())
                            {
                                DevClassies.Add(item.LngEnumValue, item);
                            }
                        }
                    }


                    if (null == DevClassies)
                    {
                        continue;
                    }
                    if (DevClassies.Count > 0)
                    {
                        TempParentID = count - 1;
                        foreach (var itemDevClass in DevClassies.Values)
                        {
                            ParentID = TempParentID;
                            TreeListSource.TreeListDevType.Add(new TreeListItem(itemDevClass.StrEnumDisplay, itemDevClass.LngEnumValue.ToString(), "102", count++, ParentID));

                            //由于这段代码下面没有使用，所以暂时屏蔽  20170317 
                            //tempjc_dev = _JC_DEV.Values.ToList().FindAll(a => DevClassID == a.Bz3 && a.DTOState != Framework.Core.Service.DTO.DTOStateEnum.Delete);

                            //DevS = DEVServiceModel.QueryDevByDevClassIDCache(itemDevClass.LngEnumValue);
                            //if (DevS == null)
                            //{
                            //    continue;
                            //}


                            //DevS = DevS.OrderBy(item => item.Devid).ToList();
                            //if (DevS.Count > 0)
                            //{
                            //    ParentID = count - 1;
                            //    for (int i = 0; i < DevS.Count; i++)
                            //    {
                            //        if (itemDevDevProperties.LngEnumValue == 0)
                            //        {
                            //            TreeListSource.TreeListDevType.Add(new TreeListItem(DevS[i].Name, DevS[i].Devid.ToString(), "103", count++, ParentID));
                            //        }
                            //        else
                            //        {
                            //            TreeListSource.TreeListDevType.Add(new TreeListItem(DevS[i].Name, DevS[i].Devid.ToString(), "104", count++, ParentID));
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        //DevS = DEVServiceModel.QueryDevByDevpropertIDCache(itemDevDevProperties.LngEnumValue);
                        //if (DevS == null)
                        //{
                        //    continue;
                        //}
                        //DevS = DevS.OrderBy(item => item.Devid).ToList();
                        //ParentID = count - 1;
                        //for (int i = 0; i < DevS.Count; i++)
                        //{
                        //    if (itemDevDevProperties.LngEnumValue == 0)
                        //    {
                        //        TreeListSource.TreeListDevType.Add(new TreeListItem(DevS[i].Name, DevS[i].Devid.ToString(), "103", count++, ParentID));
                        //    }
                        //    else
                        //    {
                        //        TreeListSource.TreeListDevType.Add(new TreeListItem(DevS[i].Name, DevS[i].Devid.ToString(), "104", count++, ParentID));
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 初始化设备数据
        /// </summary>
        public void InitDevData()
        {
            TreeListSource.TreeListDev.Clear();
            int count = 1;
            List<string> Switchs;
            IList<Jc_MacInfo> Macs = null;
            IList<Jc_DefInfo> Stations = null;
            int ParentID;
            int TempParentID;
            //TreeListSource.TreeListDev.Add(new TreeListItem("[双击新增设备]", "", "-1", count++, -1));
            TreeListSource.TreeListDev.Add(new TreeListItem("所有设备", "", "0", count++, -1));
            TreeListSource.TreeListDev.Add(new TreeListItem("交换机", "", "1", count++, 1));
            TreeListSource.TreeListDev.Add(new TreeListItem("分站模块", "", "2", count++, 1));
            //TreeListSource.TreeListDev.Add(new TreeListItem("未定义交换机", "", "3", count++, 2));
            //TreeListSource.TreeListDev.Add(new TreeListItem("未通讯设备", "", "8", count++, 1));


            try
            {
                //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                List<Jc_MacInfo> allMacList = MACServiceModel.QueryAllCache();
                List<Jc_MacInfo> macList = allMacList.FindAll(a => a.Upflag == "1");
                List<Jc_MacInfo> stationIPList = allMacList.FindAll(a => a.Upflag == "0");
                int tempint=0;
                List<Jc_MacInfo> tempstationIPList = stationIPList.FindAll(a => int.TryParse(a.Bz1, out tempint));
                List<Jc_MacInfo> tempstationIPList1 = stationIPList.FindAll(a => !int.TryParse(a.Bz1, out tempint));
                stationIPList = tempstationIPList.OrderBy(a => int.Parse(a.Bz1)).ToList();
                stationIPList.AddRange(tempstationIPList1);
                List<Jc_DefInfo> defList = DEFServiceModel.QueryPointByDevpropertIDCache(0).FindAll(a => a.Upflag != "1");//修改，不对同步的数据进行定义  20180131


                //加载网口 及 设备
                //Switchs = MACServiceModel.QuerySwitchsCache();
                //Switchs = new List<string>();
                //foreach (Jc_MacInfo item in macList)
                //{
                //    //if (!Switchs.Contains(item.Wz) && long.Parse(item.Wzid) >= 0)
                //    //{
                //    Switchs.Add(item.Wz);
                //    //}
                //}
                //if (Switchs != null)
                //{
                //for (int i = 0; i < macList.Count; i++)
                //{

                //TreeListSource.TreeListDev.Add(new TreeListItem(Switchs[i], Switchs[i], "4", count++, 2));

                //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                //try
                //{
                //    Macs = macList.ToList().FindAll(a => a.MAC == Switchs[i] && a.InfoState != InfoState.Delete);
                //    //Macs = MACServiceModel.QueryMACByWzCache(Switchs[i]);
                //}
                //catch { }


                //if (Macs == null)
                //{
                //    continue;
                //}
                //Macs = Macs.OrderBy(item => item.IP).ToList();//xuzp 由按照MAC排序改成按照IP排序
                //TempParentID = count - 1;
                for (int j = 0; j < macList.Count; j++)
                {
                    //ParentID = TempParentID;
                    TreeListSource.TreeListDev.Add(new TreeListItem("[" + macList[j].Wz + "]" + macList[j].IP + "-" + macList[j].MAC, macList[j].MAC, "5", count++, 2));
                    ParentID = count - 1;
                    try
                    {
                        //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                        Stations = defList.ToList().FindAll(a => a.Bz12 == macList[j].MAC && a.Activity == "1" && a.InfoState != InfoState.Delete);
                        //Stations = DEFServiceModel.QueryPointByMACCache(Macs[j].MAC);
                    }
                    catch { }

                    if (Stations == null)
                    {
                        continue;
                    }
                    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                    for (int k = 0; k < Stations.Count; k++)
                    {
                        TreeListSource.TreeListDev.Add(new TreeListItem(Stations[k].Fzh.ToString().PadLeft(3, '0') + "." + Stations[k].Wz, Stations[k].Point, "7", count++, ParentID));
                    }
                }
                //    }
                //}

                //分站IP模块管理
                for (int j = 0; j < stationIPList.Count; j++)
                {
                    //ParentID = TempParentID;
                    TreeListSource.TreeListDev.Add(new TreeListItem(stationIPList[j].Bz1 + ":" + stationIPList[j].IP + "-" + stationIPList[j].MAC, stationIPList[j].MAC, "9", count++, 3));
                    ParentID = count - 1;
                    try
                    {
                        //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                        Stations = defList.ToList().FindAll(a => a.Jckz1 == stationIPList[j].MAC && a.Activity == "1" && a.InfoState != InfoState.Delete);
                        //Stations = DEFServiceModel.QueryPointByMACCache(Macs[j].MAC);
                    }
                    catch { }

                    if (Stations == null)
                    {
                        continue;
                    }
                    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                    for (int k = 0; k < Stations.Count; k++)
                    {
                        TreeListSource.TreeListDev.Add(new TreeListItem(Stations[k].Fzh.ToString().PadLeft(3, '0') + "." + Stations[k].Wz, Stations[k].Point, "7", count++, ParentID));
                    }
                }

                ////加载未定义交换机               
                ////Macs = MACServiceModel.QueryMACByWzCache(null);
                //Macs = macList.FindAll(a => a.Wz == null && a.Type == 0);
                //if (Macs != null)
                //{
                //    #region 加载未定义交换机  20170116
                //    List<string> mac = new List<string>();
                //    List<Jc_MacInfo> tempmac = new List<Jc_MacInfo>();
                //    for (int i = 0; i < Macs.Count; i++)
                //    {
                //        if (!string.IsNullOrEmpty(Macs[i].Bz2) && !mac.Contains(Macs[i].Bz2))
                //        {
                //            mac.Add(Macs[i].Bz2);
                //        }
                //    }


                //    if (mac.Count > 0)
                //    {
                //        for (int i = 0; i < mac.Count; i++)
                //        {
                //            TreeListSource.TreeListDev.Add(new TreeListItem(mac[i], mac[i], "4", count++, 4));
                //            tempmac.Clear();
                //            for (int j = 0; j < Macs.Count; j++)
                //            {
                //                if (!string.IsNullOrEmpty(Macs[j].Bz2) && Macs[j].Bz2 == mac[i])
                //                {
                //                    tempmac.Add(Macs[j]);
                //                }
                //            }
                //            if (tempmac.Count > 0)
                //            {
                //                tempmac = tempmac.OrderBy(item => item.IP).ToList();
                //                TempParentID = count - 1;
                //                for (int j = 0; j < tempmac.Count; j++)
                //                {
                //                    ParentID = TempParentID;
                //                    TreeListSource.TreeListDev.Add(new TreeListItem("[" + tempmac[j].Bz3 + "]" + tempmac[j].IP + "-" + tempmac[j].MAC, tempmac[j].MAC, "5", count++, ParentID));

                //                    //未定义安装位置的分站，也会绑定测点，这里需要加载  20170610
                //                    ParentID = count - 1;
                //                    try
                //                    {
                //                        //modified by  20170323 优化加载时的性能问题，由原来的多次调用服务器查询，改为一次性从服务器加载再内存做判断筛选
                //                        Stations = defList.ToList().FindAll(a => a.Jckz1 == tempmac[j].MAC && a.Activity == "1" && a.InfoState != InfoState.Delete);
                //                        //Stations = DEFServiceModel.QueryPointByMACCache(Macs[j].MAC);
                //                    }
                //                    catch { }

                //                    if (Stations == null)
                //                    {
                //                        continue;
                //                    }
                //                    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                //                    for (int k = 0; k < Stations.Count; k++)
                //                    {
                //                        TreeListSource.TreeListDev.Add(new TreeListItem(Stations[k].Fzh.ToString().PadLeft(3, '0') + "." + Stations[k].Wz, Stations[k].Point, "7", count++, ParentID));
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    #endregion

                //    //for (int i = 0; i < Macs.Count; i++)
                //    //{
                //    //    TreeListSource.TreeListDev.Add(new TreeListItem("[" + Macs[i].Bz3 + "]" + Macs[i].IP + "-" + Macs[i].MAC, Macs[i].MAC, "5", count++, 4));
                //    //    ParentID = count - 1;
                //    //    Stations = DEFServiceModel.QueryPointByMACCache(Macs[i].MAC);
                //    //    if (Stations == null)
                //    //    {
                //    //        continue;
                //    //    }
                //    //    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                //    //    for (int j = 0; j < Stations.Count; j++)
                //    //    {
                //    //        TreeListSource.TreeListDev.Add(new TreeListItem(Stations[j].Fzh.ToString().PadLeft(3, '0') + "." + Stations[j].Wz, Stations[j].Point, "7", count++, ParentID));
                //    //    }
                //    //}
                //}
                ////加载串口 及 设备
                //Macs = MACServiceModel.SearchALLCOMCache();//串口未实现 
                //if (Macs != null)
                //{
                //    Macs = Macs.OrderBy(item => item.MAC).ToList();
                //    for (int i = 0; i < Macs.Count; i++)
                //    {
                //        TreeListSource.TreeListDev.Add(new TreeListItem(Macs[i].IP + "-" + Macs[i].MAC, Macs[i].MAC, "6", count++, 3));
                //        ParentID = count - 1;

                //        //if (string.IsNullOrEmpty(com))
                //        //{
                //        //    return result;
                //        //}
                //        //if (!com.Contains("COM"))
                //        //{
                //        //    return result;
                //        //}
                //        //short COM = Convert.ToInt16(com.Substring(3));
                //        //tempjc_def = _JC_DEF.Values.ToList().FindAll(a => a.K3 == com && a.Activity == "1" && a.DTOState != Framework.Core.Service.DTO.DTOStateEnum.Delete);

                //        Stations = DEFServiceModel.QueryPointByCOMCache(Macs[i].MAC);
                //        if (Stations == null)
                //        {
                //            continue;
                //        }
                //        Stations = Stations.OrderBy(item => item.Fzh).ToList();
                //        for (int j = 0; j < Stations.Count; j++)
                //        {
                //            TreeListSource.TreeListDev.Add(new TreeListItem(Stations[j].Fzh.ToString().PadLeft(3, '0') + "." + Stations[j].Wz, Stations[j].Point, "7", count++, ParentID));
                //        }
                //    }
                //}
                ////加载未通讯设备
                ////Stations = DEFServiceModel.QueryStationNoComm();
                //Stations = defList.FindAll(a => a.DevPropertyID == 0 && string.IsNullOrEmpty(a.Jckz1) && string.IsNullOrEmpty(a.Jckz2) && a.K3 <= 0 && a.Activity == "1");
                //if (Stations != null)
                //{
                //    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                //    for (int i = 0; i < Stations.Count; i++)
                //    {
                //        TreeListSource.TreeListDev.Add(new TreeListItem(Stations[i].Fzh.ToString().PadLeft(3, '0') + "." + Stations[i].Wz, Stations[i].Point, "7", count++, 5));
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 初始化分站数据
        /// </summary>
        public void InitStationData()
        {
            TreeListSource.TreeListStation.Clear();
            int count = 1;
            IList<Jc_DefInfo> Stations;
            TreeListSource.TreeListStation.Add(new TreeListItem("所有分站", "", "0", count++, -1));
            try
            {
                //加载监控系统所有分站                
                Stations = DEFServiceModel.QueryPointByDevpropertIDCache(0).FindAll(a => a.Upflag != "1");//修改，不对同步的数据进行定义  20180131           

                if (Stations != null)
                {
                    Stations = Stations.OrderBy(item => item.Fzh).ToList();
                    for (int i = 0; i < Stations.Count; i++)
                    {
                        TreeListSource.TreeListStation.Add(new TreeListItem(Stations[i].Fzh.ToString().PadLeft(3, '0') + "." + Stations[i].Wz, Stations[i].Point, "7", count++, 1));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            this.CtreeLisStation.RefreshDataSource();
        }
        /// <summary>
        /// 选择信息 DEV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeListDev_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (CtreeListDev.FocusedNode == null)
                {
                    CtreeListDev.ContextMenu = null;
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeListDev.GetDataRecordByNode(CtreeListDev.FocusedNode);

                if (DataRecord == null)
                {
                    return;
                }
                RefreshGridInDev(DataRecord.Tag, DataRecord.Code);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 选择信息 DevType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeListDevType_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (CtreeListDevType.FocusedNode == null)
                {
                    return;
                }

                TreeListItem DataRecord = (TreeListItem)CtreeListDevType.GetDataRecordByNode(CtreeListDevType.FocusedNode);
                if (DataRecord.Tag == "103" || DataRecord.Tag == "104")
                {
                    DataRecord = (TreeListItem)CtreeListDevType.GetDataRecordByNode(CtreeListDevType.FocusedNode.ParentNode);
                }
                if (DataRecord == null)
                {
                    return;
                }
                RefreshGridInDevType(DataRecord.Tag, DataRecord.Code);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 根据Tag 刷新Dev Grid
        /// </summary>
        /// <param name="Tag"></param>
        public void RefreshGridInDev(string Tag, string Code)
        {
            switch (Tag)
            {
                case "0":
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    #region "所有设备" TAG = 0
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    this.StrParameter1 = "网口";
                    this.StrParameter2 = "";


                    EvaluateStationDataSource(DEFServiceModel.QueryPointByDevpropertIDCache(0).FindAll(a => a.Upflag != "1"));//修改，不对同步的数据进行定义  20180131       

                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "所有分站";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenu = null;
                    break;
                    #endregion
                case "1":
                    #region "网口" TAG = 1
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridIPSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "5";
                    this.StrParameter1 = "";
                    this.StrParameter2 = "";
                    EvaluateIPDataSource(MACServiceModel.QueryAllIPCache().FindAll(a => a.Upflag == "1"));//
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridIPSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = true;
                    _CFFram.CpTip.Text = "所有交换机";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CcontextMenuDev.Items["搜索交换机ToolStripMenuItem"].Visible = true;
                    CtreeListDev.ContextMenuStrip = CcontextMenuDev;
                    break;
                    #endregion
                case "2":
                    #region "串口" TAG = 2
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    this.StrParameter1 = "串口";
                    this.StrParameter2 = "";
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    EvaluateStationDataSource(DEFServiceModel.QueryStationOfCOM()); //xuzp20151117
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "所有串口通讯分站";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CcontextMenuDev.Items["检测串口信息ToolStripMenuItem"].Visible = true;
                    CtreeListDev.ContextMenuStrip = CcontextMenuDev;
                    break;
                    #endregion
                case "3":
                    #region "未定义交换机"  TAG = 3
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridIPSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "5";
                    this.StrParameter1 = "";
                    this.StrParameter2 = "";
                    EvaluateIPDataSource(MACServiceModel.QueryMACByWzCache(null));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridIPSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "未定义交换机";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenuStrip = null;
                    break;
                    #endregion
                case "4":
                    #region "交换机" TAG = 4
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridIPSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    this.StrParameter1 = Code;
                    this.StrParameter2 = "";
                    _CFFram.cuGrid.CGridControl.Tag = "5";
                    EvaluateIPDataSource(MACServiceModel.QueryMACByWzCache(Code));
                    GridSource.GridIPSource = GridSource.GridIPSource.OrderBy(a => int.Parse(a.IPdz)).ToList();//点击交换机，根据交换机下面的IP编号排序  20170716
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridIPSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = Code + "包含的交换机";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenuStrip = null;
                    break;
                    #endregion
                case "5":
                    #region "交换机" TAG = 5
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    this.StrParameter1 = "网口";
                    this.StrParameter2 = Code;
                    EvaluateStationDataSource(DEFServiceModel.QueryPointBySwitchCache(Code).FindAll(a => a.Upflag != "1"));//修改，不对同步的数据进行定义  20180131   
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = Code + "绑定的分站";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenuStrip = null;
                    break;
                    #endregion
                case "6":
                    #region "COM" TAG = 6
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    this.StrParameter1 = "串口";
                    this.StrParameter2 = Code;
                    EvaluateStationDataSource(DEFServiceModel.QueryPointByCOMCache(Code));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = Code + "下绑定的分站";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenuStrip = null;
                    break;
                    #endregion
                case "7":
                    #region "分站"  TAG = 7
                    Jc_DefInfo StationNow = DEFServiceModel.QueryPointByCodeCache(Code);
                    Jc_DevInfo FzDev = new Jc_DevInfo();
                    if (StationNow != null)
                    {
                        FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == StationNow.Devid);
                    }
                    string setting = Sys.Safety.ClientFramework.Configuration.BaseInfo.GraphicDefine;
                    bool graphicDefine = true;
                    if (setting == "1")
                    {
                        graphicDefine = true;
                    }
                    else
                    {
                        graphicDefine = false;
                    }

                    if (FzDev != null && FzDev.LC2 == 13 && graphicDefine)
                    {
                        _CFFram.CpTip.Text = Code + "图形定义";
                        //智能分站，切换到图形化定义界面
                        bool isMapLoadFlag = _CFFram.cuGrid.StationDefineMapLoad(StationNow);
                        if (!isMapLoadFlag)
                        {//如果图形界面加载异常，则自动切换到列表界面进行显示  20171009
                            //切换到列表界面
                            _CFFram.cuGrid.ChangeToList();
                        }
                    }
                    else
                    {//其它分站，沿用列表定义界面

                        //切换到列表界面
                        _CFFram.cuGrid.ChangeToList();

                        //CcontextMenuDev.Items["新增设备ToolStripMenuItem"].Visible = true;
                        //CcontextMenuDev.Items["修改设备ToolStripMenuItem"].Visible = true;
                        //CcontextMenuDev.Items["删除设备ToolStripMenuItem"].Visible = true;
                        //CtreeListDev.ContextMenuStrip = CcontextMenuDev;
                    }

                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridSensorSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "8";
                    this.StrParameter1 = Code;
                    this.StrParameter2 = "";
                    EvaluateSensorDataSource(DEFServiceModel.QueryPointByStationCache(Code));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridSensorSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = Code + "分站下的通道信息";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CcontextMenuDev.Items["删除设备ToolStripMenuItem"].Visible = true;
                    break;
                    #endregion
                case "8":
                    #region "未通讯设备" TAG = 8
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    this.StrParameter1 = "网口";
                    this.StrParameter2 = "";
                    EvaluateStationDataSource(DEFServiceModel.QueryStationNoComm());
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "未通讯设备";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenu = null;
                    break;
                    #endregion
                case "9":
                    #region "分站网口" TAG = 9
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationSource)
                    {
                        RefreshGridColumForDev(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "7";
                    this.StrParameter1 = "网口";
                    this.StrParameter2 = Code;
                    EvaluateStationDataSource(DEFServiceModel.QueryPointByMACCache(Code).FindAll(a => a.Upflag != "1"));//修改，不对同步的数据进行定义  20180131   
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = Code + "绑定的分站";
                    for (int i = 0; i < CcontextMenuDev.Items.Count; i++)
                    {
                        CcontextMenuDev.Items[i].Visible = false;
                    }
                    CtreeListDev.ContextMenuStrip = null;
                    break;
                    #endregion
                default:
                    break;
            }
        }
        /// <summary>
        /// 根据Tag 刷新DevType Grid
        /// </summary>
        /// <param name="Tag"></param>
        private void RefreshGridInDevType(string Tag, string code)
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> temp = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            Dictionary<int, EnumcodeInfo> tempEnumCode = new Dictionary<int, EnumcodeInfo>();
            switch (Tag)
            {
                case "100":
                    #region 分站(性质) TAG = 100
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridStationTypeSource)
                    {
                        RefreshGridColumForDevType(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "103";
                    this.StrParameter1 = "";
                    this.StrParameter2 = "";
                    EvaluateStationTypeDataSource(DEVServiceModel.QueryDevByDevpropertIDCache(Convert.ToInt32(code)));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridStationTypeSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "所有分站类型";
                    break;
                    #endregion
                case "101":
                    #region  传感器（性质） TAG = 101
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridSensorTypeSource)
                    {
                        RefreshGridColumForDevType(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "104";
                    this.StrParameter1 = code;
                    this.StrParameter2 = "";
                    EvaluateSensorTypeDataSource(DEVServiceModel.QueryDevByDevpropertIDCache(Convert.ToInt32(code)));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridSensorTypeSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    tempEnumCode = Model.DEVServiceModel.QueryDevPropertisCache();
                    try
                    {
                        _CFFram.CpTip.Text = ((tempEnumCode == null) ? "" : (tempEnumCode[Convert.ToInt32(code)].StrEnumDisplay)) + "设备类型";
                    }
                    catch (Exception)
                    {
                        _CFFram.CpTip.Text = "设备类型";
                    }
                    break;
                    #endregion
                case "102":
                    #region 传感器类型(种类) TAG = 102
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridSensorTypeSource)
                    {
                        RefreshGridColumForDevType(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "104";
                    this.StrParameter1 = "1";
                    this.StrParameter2 = code;
                    EvaluateSensorTypeDataSource(DEVServiceModel.QueryDevByDevClassIDCache(Convert.ToInt32(code)));
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridSensorTypeSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    tempEnumCode = Model.DEVServiceModel.QueryDevClasiessCache();
                    try
                    {
                        _CFFram.CpTip.Text = ((tempEnumCode == null) ? "" : (tempEnumCode[Convert.ToInt32(code)].StrEnumDisplay)) + "设备类型";
                    }
                    catch (Exception)
                    {
                        _CFFram.CpTip.Text = "设备类型";
                    }
                    break;
                    #endregion
                case "520":
                    #region 安装位置管理  TAG = 520
                    //切换到列表界面
                    _CFFram.cuGrid.ChangeToList();
                    if (_CFFram.cuGrid.CGridControl.DataSource != GridSource.GridWZSource)
                    {
                        RefreshGridColumForDevType(Tag);
                    }
                    _CFFram.cuGrid.CGridControl.Tag = "520";
                    this.StrParameter1 = "0";
                    this.StrParameter2 = "0";
                    EvaluateWZDataSource(WZServiceModel.QueryWZsCache());
                    _CFFram.cuGrid.CGridControl.DataSource = GridSource.GridWZSource;
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    _CFFram.cuGrid.tlbSearch.Enabled = false;
                    _CFFram.CpTip.Text = "安装位置管理";
                    break;
                    #endregion
                default:
                    break;
            }
        }
        /// <summary>
        /// 设备和设备类型之间切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CxtraTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (CxtraTabControl.SelectedTabPage == CxtraTabPageDevType)
                {
                    this.CtreeListDevType.RefreshDataSource();
                    this.CtreeListDevType.ExpandAll();
                }
                else if (CxtraTabControl.SelectedTabPage == CxtraTabPageDev)
                {
                    InitDevData();
                    this.CtreeListDev.RefreshDataSource();
                    this.CtreeListDev.ExpandAll();
                }
                else if (CxtraTabControl.SelectedTabPage == CxtraTabPagewz)//20170314 
                {
                    if (CtreeListWz.Nodes != null && CtreeListWz.Nodes.Count > 0)
                    {
                        CtreeListWz.FocusedNode = CtreeListWz.MoveFirst();
                        CtreeListWz_Click(CtreeListWz.FocusedNode, new EventArgs());
                    }
                }
                else
                {
                    InitStationData();
                    //CxtraTabPageStation
                    this.CtreeLisStation.RefreshDataSource();
                    this.CtreeLisStation.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Dev 双击编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeListDev_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (null == CtreeListDev.FocusedNode)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeListDev.GetDataRecordByNode(CtreeListDev.FocusedNode);
                if (null == DataRecord)
                {
                    return;
                }
                ShowFormForDev(DataRecord.Tag, DataRecord.Code, StrParameter1, StrParameter2);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// DevType 双击编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeListDevType_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (null == CtreeListDevType.FocusedNode)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeListDevType.GetDataRecordByNode(CtreeListDevType.FocusedNode);
                ShowFormForDev(DataRecord.Tag, DataRecord.Code, StrParameter1, StrParameter2);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 赋值 Grid DataSource of Station
        /// </summary>
        /// <param name="DefDTOs"></param>
        private void EvaluateStationDataSource(IList<Jc_DefInfo> DefDTOs)
        {
            GridStationItem StationItem;
            GridSource.GridStationSource.Clear();
            if (null != DefDTOs)
            {
                DefDTOs = DefDTOs.OrderBy(item => item.Fzh).ToList();
                for (int i = 0; i < DefDTOs.Count; i++)
                {
                    StationItem = new GridStationItem();
                    StationItem.ComNum = DefDTOs[i].K3;
                    StationItem.CommType = DefDTOs[i].Jckz1;
                    StationItem.DevName = DefDTOs[i].DevName;
                    StationItem.fzh = DefDTOs[i].Fzh.ToString();
                    StationItem.Point = DefDTOs[i].Point;
                    StationItem.RunState = DefDTOs[i].Bz4.ToString();
                    StationItem.wz = DefDTOs[i].Wz;
                    StationItem.Tag = "7";
                    StationItem.Code = DefDTOs[i].Point;
                    GridSource.GridStationSource.Add(StationItem);
                }
            }
        }
        /// <summary>
        /// 赋值 Grid DataSource of IP
        /// </summary>
        /// <param name="DefDTOs"></param>
        private void EvaluateIPDataSource(IList<Jc_MacInfo> MacDTOs)
        {
            GridIPItem IpItem;
            GridSource.GridIPSource.Clear();
            if (null != MacDTOs)
            {
                MacDTOs = MacDTOs.OrderBy(item => item.MAC).ToList();
                for (int i = 0; i < MacDTOs.Count; i++)
                {
                    IpItem = new GridIPItem();
                    IpItem.BingingStations = MacDTOs[i].Bz1;
                    IpItem.IPdz = MacDTOs[i].Bz3;// 20170112
                    IpItem.jhjname = MacDTOs[i].Bz2;// 20170112
                    IpItem.IP = MacDTOs[i].IP;
                    IpItem.MAC = MacDTOs[i].MAC;
                    IpItem.wz = MacDTOs[i].Wz;
                    IpItem.Tag = "5";
                    IpItem.Code = MacDTOs[i].MAC;
                    IpItem.NetID = MacDTOs[i].NetID.ToString();//新增连接号  20170712
                    IpItem.TransportaCommFlag = MacDTOs[i].Istmcs == 1 ? "是" : "否";
                    IpItem.SwitchesMac = MacDTOs[i].Bz2;//赋值所属交换机MAC地址  20170912
                    GridSource.GridIPSource.Add(IpItem);
                }
            }
        }
        /// <summary>
        /// 赋值 Grid DataSource of Sensor
        /// </summary>
        /// <param name="DefDTOs"></param>
        private void EvaluateSensorDataSource(IList<Jc_DefInfo> DefDTOs)
        {
            GridSensorItem SensorItem;
            GridSource.GridSensorSource.Clear();
            Jc_DevInfo tempDev;
            if (null != DefDTOs)
            {
                //DefDTOs = DefDTOs.OrderBy(item => item.DevPropertyID).ThenBy(item => item.Kh).ThenBy(item => item.Dzh).ToList();
                //修改排序方式，根据基础通道、智能通道、控制通道、累计通道进行排序  20170712
                DefDTOs = SortShowDt(DefDTOs.ToList());
                for (int i = 0; i < DefDTOs.Count; i++)
                {
                    SensorItem = new GridSensorItem();
                    SensorItem.DevName = DefDTOs[i].DevName;
                    SensorItem.kh = DefDTOs[i].Kh.ToString();
                    SensorItem.Point = DefDTOs[i].Point;
                    SensorItem.RunState = DefDTOs[i].Bz4.ToString();
                    SensorItem.wz = DefDTOs[i].Wz;
                    SensorItem.z2 = DefDTOs[i].Z2.ToString();
                    SensorItem.z3 = DefDTOs[i].Z3.ToString();
                    SensorItem.z4 = DefDTOs[i].Z4.ToString();
                    if (DefDTOs[i].DevPropertyID == 2 || DefDTOs[i].DevPropertyID == 3)
                    {
                        //tempDev = DEVServiceModel.QueryDevByDevIDCache(DefDTOs[i].Devid);
                        //if (null != tempDev)
                        //{
                        //    SensorItem.z2 = tempDev.Xs1;
                        //    SensorItem.z3 = tempDev.Xs2;
                        //    SensorItem.z4 = tempDev.Xs3;
                        //}
                        //2017.7.13 by
                        SensorItem.z2 = DefDTOs[i].Bz6;
                        SensorItem.z3 = DefDTOs[i].Bz7;
                        SensorItem.z4 = DefDTOs[i].Bz8;
                    }
                    SensorItem.Tag = "8";
                    SensorItem.Code = DefDTOs[i].Point;
                    GridSource.GridSensorSource.Add(SensorItem);
                }
            }
        }
        /// <summary>
        /// 修改排序方式，根据基础通道、智能通道、控制通道、累计通道进行排序  20170712
        /// </summary>
        /// <returns></returns>
        private List<Jc_DefInfo> SortShowDt(List<Jc_DefInfo> DefDTOs)
        {
            List<Jc_DefInfo> SortDefInfo = new List<Jc_DefInfo>();
            //加载基础通道
            List<Jc_DefInfo> BaseChanelInStation = DefDTOs.FindAll(a => (a.Kh <= 16 || (a.Kh >= 40 && a.Kh <= 43)) && (a.DevPropertyID == 1 ||
                a.DevPropertyID == 2 || (a.DevPropertyID == 3 && a.Dzh > 0))).ToList().OrderBy(a => a.Kh).ThenBy(a => a.Dzh).ToList();//20190616
            SortDefInfo.AddRange(BaseChanelInStation);

            //加载智能通道
            List<Jc_DefInfo> SmartChanelInStation = DefDTOs.FindAll(a => a.Kh >= 17 && a.Kh <= 24).ToList().OrderBy(a => a.Kh).ThenBy(a => a.Dzh).ToList();
            SortDefInfo.AddRange(SmartChanelInStation);
            //加载本地控制通道
            List<Jc_DefInfo> ControlChanelInStation = DefDTOs.FindAll(a => a.DevPropertyID == 3 && a.Dzh == 0).ToList().OrderBy(a => a.Kh).ThenBy(a => a.Dzh).ToList();
            SortDefInfo.AddRange(ControlChanelInStation);
            //加载累计通道
            List<Jc_DefInfo> TiredChanelInStation = DefDTOs.FindAll(a => a.DevPropertyID == 4).ToList().OrderBy(a => a.Kh).ThenBy(a => a.Dzh).ToList();
            SortDefInfo.AddRange(TiredChanelInStation);

            //加载人员通道   20171123
            List<Jc_DefInfo> PersonChanelInStation = DefDTOs.FindAll(a => a.DevPropertyID == 7).ToList().OrderBy(a => a.Kh).ThenBy(a => a.Dzh).ToList();
            SortDefInfo.AddRange(PersonChanelInStation);

            return SortDefInfo;
        }
        /// <summary>
        /// 赋值 Grid DataSource of StationType
        /// </summary>
        private void EvaluateStationTypeDataSource(IList<Jc_DevInfo> DevDTOs)
        {
            GridStationTypeItem StationTypeItem;
            GridSource.GridStationTypeSource.Clear();
            if (null != DevDTOs)
            {
                DevDTOs = DevDTOs.OrderBy(item => long.Parse(item.Devid)).ToList();
                for (int i = 0; i < DevDTOs.Count; i++)
                {
                    StationTypeItem = new GridStationTypeItem();
                    StationTypeItem.DevID = DevDTOs[i].Devid.ToString();
                    StationTypeItem.DevName = DevDTOs[i].Name;
                    StationTypeItem.DriverID = DevDTOs[i].LC2.ToString();
                    StationTypeItem.Tag = "103";
                    StationTypeItem.Code = DevDTOs[i].Devid.ToString();
                    GridSource.GridStationTypeSource.Add(StationTypeItem);
                }
            }
        }
        /// <summary>
        /// 赋值 Grid DataSource of SensorType
        /// </summary>
        private void EvaluateSensorTypeDataSource(IList<Jc_DevInfo> DevDTOs)
        {
            GridSensorTypeItem SensorTypeItem;
            GridSource.GridSensorTypeSource.Clear();
            if (null != DevDTOs)
            {
                DevDTOs = DevDTOs.OrderBy(item => long.Parse(item.Devid)).ToList();
                for (int i = 0; i < DevDTOs.Count; i++)
                {
                    SensorTypeItem = new GridSensorTypeItem();
                    SensorTypeItem.DevID = DevDTOs[i].Devid.ToString();
                    SensorTypeItem.DevName = DevDTOs[i].Name;
                    SensorTypeItem.DevClassName = DevDTOs[i].DevClass;
                    SensorTypeItem.DevPropertyName = DevDTOs[i].DevProperty;
                    SensorTypeItem.Tag = "104";
                    SensorTypeItem.Code = DevDTOs[i].Devid.ToString();
                    GridSource.GridSensorTypeSource.Add(SensorTypeItem);
                }
            }
        }

        /// <summary>
        /// 赋值 Grid DataSource of WZ
        /// </summary>
        private void EvaluateWZDataSource(List<Jc_WzInfo> WZDTOs)
        {
            GridWZItem WZItem;
            var defs = DEFServiceModel.QueryAllCache();
            GridSource.GridWZSource.Clear();
            if (null != WZDTOs)
            {
                WZDTOs = WZDTOs.OrderBy(item => long.Parse(item.WzID)).ToList();
                for (int i = 0; i < WZDTOs.Count; i++)
                {
                    WZItem = new GridWZItem();
                    WZItem.WZID = WZDTOs[i].WzID.ToString();
                    WZItem.WZ = WZDTOs[i].Wz;
                    WZItem.Tag = "520";
                    WZItem.Code = WZDTOs[i].WzID.ToString();
                    WZItem.UseState = "历史";
                    if (defs.FindIndex(a => a.Wzid == WZDTOs[i].WzID) >= 0)
                    {
                        WZItem.UseState = "正在使用";
                    }                   
                    GridSource.GridWZSource.Add(WZItem);
                }
            }
        }
        /// <summary>
        /// 刷新 Grid Column  for Dev
        /// </summary>
        /// <param name="DefDTOs"></param>
        private void RefreshGridColumForDev(string Tag)
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> temp = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            _CFFram.cuGrid.gridView.Columns.Clear();
            switch (Tag)
            {
                case "0":
                case "2":
                case "5":
                case "6":
                case "8":
                case "9":
                    temp = GridClumnsMrg.StationGridColumn();
                    break;
                case "1":
                case "3":
                case "4":
                    temp = GridClumnsMrg.IPGridColumn();
                    break;
                case "7":
                    temp = GridClumnsMrg.SensorGridColumn();
                    break;
                default:
                    break;
            }
            if (null != temp)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    _CFFram.cuGrid.gridView.Columns.Add(temp[i]);
                }
            }
        }
        /// <summary>
        /// 刷新 Grid Column for DevType
        /// </summary>
        /// <param name="DefDTOs"></param>
        private void RefreshGridColumForDevType(string Tag)
        {
            List<DevExpress.XtraGrid.Columns.GridColumn> temp = new List<DevExpress.XtraGrid.Columns.GridColumn>();
            _CFFram.cuGrid.gridView.Columns.Clear();
            switch (Tag)
            {
                case "100":
                    temp = GridClumnsMrg.StationTyepGridColumn();
                    break;
                case "101":
                case "102":
                    temp = GridClumnsMrg.SensorTyepGridColumn();
                    break;
                case "520":
                    temp = GridClumnsMrg.WZGridColumn();
                    break;
                default:
                    break;
            }
            if (null != temp)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    _CFFram.cuGrid.gridView.Columns.Add(temp[i]);
                }
            }
        }
        private void 搜索交换机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsSerachIp)
            {
                IsSerachIp = true;
            }
            else
            {
                return;//如果正大搜索，则不能继续操作  20170401
            }
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
                InitDevData();
                CtreeListDev.RefreshDataSource();
                CtreeListDev.ExpandAll();
            }
            IsSerachIp = false;//搜索完成将标记置成false  20170401
        }
        private void 检测串口信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MACServiceModel.SearchALLCOMCache();
                InitDevData();
                CtreeListDev.RefreshDataSource();
                CtreeListDev.ExpandAll();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Add Station 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新增设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Edit Station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == CtreeListDev.FocusedNode)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeListDev.GetDataRecordByNode(CtreeListDev.FocusedNode);
                if (null == DataRecord)
                {
                    return;
                }
                ShowFormForDev(DataRecord.Tag, DataRecord.Code, StrParameter1, StrParameter2);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Delete Station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == CtreeLisStation.FocusedNode)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeLisStation.GetDataRecordByNode(CtreeLisStation.FocusedNode);
                if (null == DataRecord)
                {
                    return;
                }
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                List<Jc_DefInfo> DefAll = Model.DEFServiceModel.QueryAllCache().ToList();
                List<Jc_DefInfo> fzAll = DefAll.FindAll(a => a.Point == DataRecord.Code);
                string fzh = DataRecord.Code.Substring(0, 3);
                if (fzAll.Count < 1)
                {
                    XtraMessageBox.Show("当前删除的分站不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(DataRecord.Code))
                    {
                        //如果删除控制，则判断是否绑定了交叉控制，如果绑定了交叉控制，先提示用户解除交叉控制，再删除  20170401                       
                        foreach (Jc_DefInfo temp in fzAll)
                        {
                            if (temp.DevPropertyID == 3)
                            {
                                List<Jc_DefInfo> tempJCKZ = DefAll.FindAll(a => (a.Jckz1 != null && a.Jckz2 != null && a.Jckz3 != null) &&
                                     (a.Jckz1.Contains(temp.Point) || a.Jckz2.Contains(temp.Point) || a.Jckz3.Contains(temp.Point)));
                                if (tempJCKZ.Count > 0)
                                {
                                    XtraMessageBox.Show("当前删除控制量设备绑了交叉控制，请先解除交叉控制，再删除当前控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            //如果当前测点已经复制，则清除  20170503
                            if (Model.CopyInf.CopySensor != null)
                            {
                                if (Model.CopyInf.CopySensor.Point == temp.Point)
                                {
                                    Model.CopyInf.CopySensor = null;
                                }
                            }
                        }


                        DeleteSubStatoin(Convert.ToInt32(fzh));
                    }
                    //加延时  20170721
                    Thread.Sleep(1000);

                    InitStationData();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        private void DeleteSubStatoin(int StationNum)
        {
            WaitDialogForm wdf = new WaitDialogForm("正在删除分站下级设备...", "请等待...");
            try
            {
                //删除分站
                List<Jc_DefInfo> Points;
                Points = Model.DEFServiceModel.QueryPointByFzhCache(StationNum).FindAll(a => a.Upflag != "1"); //找出需要删除的测点 包括分站
                string MAC = "";
                string Switches = "";
                int fzh = 0;
                string StrPoints = ""; //测点列表
                string originBZ1 = "";
                if (null == Points)
                {
                    return;
                }
                if (Points.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < Points.Count; i++)
                {
                    StrPoints += Points[i].Point + ",";
                    Points[i].InfoState = InfoState.Modified;
                    Points[i].Activity = "0";
                    Points[i].DeleteTime = DateTime.Now;
                    try
                    {
                        if (Points[i].DevPropertyID == 3)//控制量删除关联更新
                        {
                            Model.RelateUpdate.ControlReUpdate(Points[i]);
                        }
                        if (Points[i].DevPropertyID == 2)//开关量删除关联更新
                        {
                            Model.RelateUpdate.DerailReUpdate(Points[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                    if (Points[i].DevPropertyID == 0 && !string.IsNullOrEmpty(Points[i].Jckz1))//&& !string.IsNullOrEmpty(Points[i].Bz12)
                    {
                        fzh = Points[i].Fzh;
                        MAC = Points[i].Jckz1;
                        Switches = Points[i].Bz12;
                    }
                }
                try
                {
                    Model.DEFServiceModel.UpdateDEFsCache(Points);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!string.IsNullOrEmpty(StrPoints))
                {
                    OperateLogHelper.InsertOperateLog(1, "删除分站【" + StrPoints + "】", "");
                }

                #region//更新MAC表中的分站绑定队列
                if (!string.IsNullOrEmpty(MAC))
                {
                    Jc_MacInfo temp = Model.MACServiceModel.QueryMACByCode(MAC);
                    string[] StationBz1;
                    if (temp != null)
                    {
                        originBZ1 = temp.Bz1;

                        //修改删除绑定列表方法，采用替换  20170415
                        if (temp.Bz1.Contains("|"))
                        {
                            if (temp.Bz1.IndexOf("|" + fzh.ToString() + "|") > 0)//中间替换
                            {
                                temp.Bz1 = temp.Bz1.Replace("|" + fzh.ToString() + "|", "|0|");
                            }
                            else if (temp.Bz1.IndexOf("|" + fzh.ToString()) == temp.Bz1.LastIndexOf("|"))//尾部替换
                            {
                                temp.Bz1 = temp.Bz1.Substring(0, temp.Bz1.LastIndexOf("|")) + "|0";
                            }
                            else if (temp.Bz1.IndexOf(fzh.ToString() + "|") == 0)//头部替换
                            {
                                temp.Bz1 = "0|" + temp.Bz1.Substring(temp.Bz1.IndexOf("|") + 1);
                            }
                        }
                        else
                        {
                            temp.Bz1 = "";
                        }
                    }
                    if (originBZ1 != temp.Bz1)
                    {
                        temp.InfoState = InfoState.Modified;
                        try
                        {
                            Model.MACServiceModel.UpdateMACCache(temp);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                #endregion
                #region//更新MAC表中的分站绑定交换机信息
                if (!string.IsNullOrEmpty(Switches))
                {
                    Jc_MacInfo temp = Model.MACServiceModel.QueryMACByCode(Switches);
                    string[] StationBz1;
                    if (temp != null)
                    {
                        originBZ1 = temp.Bz1;

                        //修改删除绑定列表方法，采用替换  20170415
                        if (temp.Bz1.Contains("|"))
                        {
                            if (temp.Bz1.IndexOf("|" + fzh.ToString() + "|") > 0)//中间替换
                            {
                                temp.Bz1 = temp.Bz1.Replace("|" + fzh.ToString() + "|", "|0|");
                            }
                            else if (temp.Bz1.IndexOf("|" + fzh.ToString()) == temp.Bz1.LastIndexOf("|"))//尾部替换
                            {
                                temp.Bz1 = temp.Bz1.Substring(0, temp.Bz1.LastIndexOf("|")) + "|0";
                            }
                            else if (temp.Bz1.IndexOf(fzh.ToString() + "|") == 0)//头部替换
                            {
                                temp.Bz1 = "0|" + temp.Bz1.Substring(temp.Bz1.IndexOf("|") + 1);
                            }
                        }
                        else
                        {
                            temp.Bz1 = "";
                        }
                    }
                    if (originBZ1 != temp.Bz1)
                    {
                        temp.InfoState = InfoState.Modified;
                        try
                        {
                            Model.MACServiceModel.UpdateMACCache(temp);
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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            if (wdf != null)
                wdf.Close();
        }
        /// <summary>
        ///Add DevType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Edit DevType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// Delete DevType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 右键获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeListDev_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hitInfo.Node;
                if (e.Button == MouseButtons.Right)
                {
                    if (null != node)
                    {
                        node.TreeList.FocusedNode = node;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 搜索IP
        /// </summary>
        private void SearchIP()
        {
            //MACServiceModel.SearchALLIPCache();// 20170112
            MACServiceModel.SearchALLIPCache8962(0);// 20170112
        }
        /// <summary>
        /// 根据设备标签 和 编码 ShowForm
        /// </summary>
        /// <param name="Tag"></param>
        /// <param name="code"></param>
        public void ShowFormForDev(string Tag, string code, string Parameter1, string Parameter2)
        {
            switch (Tag)
            {
                case "5":
                case "9":
                    #region "FF.FF.FF.FF.FF.FF" 交换机
                    CommCfg.CFIPModules CFIPModules = new CommCfg.CFIPModules(code);
                    CFIPModules.ShowDialog();
                    InitDevData();
                    CtreeListDev.RefreshDataSource();
                    CtreeListDev.ExpandAll();
                    #endregion
                    break;
                case "6":
                    #region "COM1" 串口
                    CommCfg.CFCOMCfg CFCOMCfg = new CommCfg.CFCOMCfg(code);
                    CFCOMCfg.ShowDialog();
                    InitDevData();
                    CtreeListDev.RefreshDataSource();
                    CtreeListDev.ExpandAll();
                    #endregion
                    break;
                case "7":
                    #region "副井底1101米大分站" 分站
                    Station.CFStation CFStation = new Station.CFStation(code, Parameter1, Parameter2);
                    CFStation.ShowDialog();
                    if (CxtraTabControl.SelectedTabPage == CxtraTabPageDev)
                    {
                        //InitStationData();
                        //CtreeLisStation.RefreshDataSource();
                        //CtreeLisStation.ExpandAll();

                        InitDevData();
                        CtreeListDev.RefreshDataSource();
                        CtreeListDev.ExpandAll();
                    }
                    else
                    {
                        //InitDevData();
                        //CtreeListDev.RefreshDataSource();
                        //CtreeListDev.ExpandAll();

                        InitStationData();
                        CtreeLisStation.RefreshDataSource();
                        CtreeLisStation.ExpandAll();
                    }
                    #endregion
                    break;
                case "8":
                    #region "副井底1101米高浓瓦斯"
                    Sensor.CFCMSensor CFCMSensor = new Sensor.CFCMSensor(code, Parameter1, Parameter2);
                    CFCMSensor.ShowDialog();
                    if (CxtraTabControl.SelectedTabPage == CxtraTabPageDev)
                    {
                        //设备管理只加载交换机  20170716
                        //InitStationData();
                        //CtreeLisStation.RefreshDataSource();
                        //CtreeLisStation.ExpandAll();

                        InitDevData();
                        CtreeListDev.RefreshDataSource();
                        CtreeListDev.ExpandAll();
                    }
                    else
                    {
                        //分站管理只加载分站  20170716
                        //InitDevData();
                        //CtreeListDev.RefreshDataSource();
                        //CtreeListDev.ExpandAll();

                        InitStationData();
                        CtreeLisStation.RefreshDataSource();
                        CtreeLisStation.ExpandAll();
                    }
                    #endregion
                    break;
                case "103":
                    #region "KJF86N大分站" 分站类型
                    Station.CFStationType CFStationType = new Station.CFStationType(code);
                    CFStationType.ShowDialog();
                    InitDevTypeData();
                    CtreeListDevType.RefreshDataSource();
                    CtreeListDevType.ExpandAll();
                    #endregion
                    break;
                case "104":
                    #region "迎头瓦斯传感器" 传感器类型
                    Sensor.CFCMSensorType CFCMSensorType = new Sensor.CFCMSensorType(code, Parameter1, Parameter2);
                    CFCMSensorType.ShowDialog();
                    InitDevTypeData();
                    CtreeListDevType.RefreshDataSource();
                    CtreeListDevType.ExpandAll();
                    #endregion
                    break;
                case "520":
                    #region 安装位置管理
                    WZMgr.CFWZ CFWZ = new WZMgr.CFWZ(code);
                    CFWZ.ShowDialog();
                    EvaluateWZDataSource(WZServiceModel.QueryWZsCache());
                    _CFFram.cuGrid.CGridControl.RefreshDataSource();
                    #endregion
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 选择信息 Station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeLisStation_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (CtreeLisStation.FocusedNode == null)
                {
                    CtreeLisStation.ContextMenu = null;
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeLisStation.GetDataRecordByNode(CtreeLisStation.FocusedNode);

                if (DataRecord == null)
                {
                    return;
                }
                RefreshGridInDev(DataRecord.Tag, DataRecord.Code);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 右键选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeLisStation_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hitInfo.Node;
                if (e.Button == MouseButtons.Right)
                {
                    if (null != node)
                    {
                        node.TreeList.FocusedNode = node;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// Station 双击编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtreeLisStation_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (null == CtreeLisStation.FocusedNode)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CtreeLisStation.GetDataRecordByNode(CtreeLisStation.FocusedNode);
                if (null == DataRecord)
                {
                    return;
                }
                ShowFormForDev(DataRecord.Tag, DataRecord.Code, StrParameter1, StrParameter2);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }

        private void CtreeListWz_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {

        }

        private void CtreeListWz_DoubleClick(object sender, EventArgs e)
        {
        }

        private void CtreeListWz_Click(object sender, EventArgs e)
        {
            try
            {
                if (CtreeListWz.FocusedNode == null)
                {
                    return;
                }

                TreeListItem DataRecord = (TreeListItem)CtreeListWz.GetDataRecordByNode(CtreeListWz.FocusedNode);
                if (DataRecord == null)
                {
                    return;
                }
                RefreshGridInDevType(DataRecord.Tag, DataRecord.Code);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}