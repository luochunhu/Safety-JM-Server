using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using Sys.Safety.Client.Define.Model;
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Cache;
using Basic.Framework.Web;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.Client.GraphDefine;
using Sys.Safety.Request.Graphicspointsinf;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;


namespace Sys.Safety.Client.Define.Station
{
    public partial class CFStation : XtraForm
    {
        IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>();
        INetworkModuleService _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
        IDeviceDefineService _DeviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        IGraphicspointsinfService graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        public string jckz3 = "";
        /// <summary> 构造函数 参数为空表示新增加
        /// </summary>
        public CFStation()
        {
            InitializeComponent();
        }
        /// <summary>构造函数 传入分站测点
        /// </summary>
        public CFStation(string Point, string StrParameter1, string StrParameter2)
        {
            _StrParameter1 = StrParameter1;
            _StrParameter2 = StrParameter2;
            _subStaionPoint = Model.DEFServiceModel.QueryPointByCodeCache(Point);
            InitializeComponent();
        }

        #region ==========================变量定义=============================
        /// <summary>
        /// 当前分站测点对象
        /// </summary>
        private Jc_DefInfo _subStaionPoint = new Jc_DefInfo();
        /// <summary>
        /// 等待窗体
        /// </summary>
        private Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm WaitDialogFormTemp;
        /// <summary>
        /// 用于更新UI的委托定义
        /// </summary>
        private delegate void UpdateControl();

        private string _StrParameter1;
        private string _StrParameter2;

        /// <summary>
        /// 当前测点所在区域ID
        /// </summary>
        private string PointAreaId = "";
        /// <summary>
        /// 测点所在的地图ID
        /// </summary>
        private string PointInGraphId = "";
        #endregion

        #region ==========================加载信息=============================
        /// <summary>
        /// 加载分站信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CfStation_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPretermitInf();//控件属性及默认值
                LoadBasicInf();//加定载分站信息


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        #endregion

        #region ==========================测点操作=============================
        /// <summary>
        ///  测点保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Stationverify())
                {
                    return;
                }
                Jc_DefInfo temp = new Jc_DefInfo();
                //重新从原来缓存中，将原来测点的数据加载过来 
                //根据分站号和设备性质去查询分站  20170331                
                PointDefineGetByStationIDRequest PointDefineRequest = new PointDefineGetByStationIDRequest();
                PointDefineRequest.StationID = Convert.ToInt16(CcmbStationSourceNum.Text);
                List<Jc_DefInfo> tempStationList = _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest).Data.FindAll(a => 0 == a.DevPropertyID);
                if (tempStationList.Count > 0)
                {
                    temp = tempStationList[0];
                }
                if (temp == null)
                {
                    temp = new Jc_DefInfo();
                }

                Jc_WzInfo tempWz = null;
                #region 先处理安装位置
                tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CgleStaionAdress.Text);
                if (null == tempWz)
                {
                    tempWz = new Jc_WzInfo();
                    tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID
                    //tempWz.WZID = Convert.ToInt64(this.CgleStaionAdress.EditValue); //WZID xuzp20151109
                    tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
                    tempWz.Wz = this.CgleStaionAdress.Text; //wz
                    tempWz.CreateTime = DateTime.Now;// 20170331
                    tempWz.InfoState = InfoState.AddNew;
                    try
                    {
                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz))
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
                temp.Fzh = Convert.ToInt16(CcmbStationSourceNum.Text);
                temp.Kh = 0;
                if (this.CcmbCommType.Text == "串口")
                {
                    temp.Kh = 1;//分站主队巡检标记
                }
                temp.Dzh = 0;//地址号
                temp.DevPropertyID = 0;
                temp.DevProperty = "分站";
                temp.Devid = CcmbStationType.Properties.Items[CcmbStationType.SelectedIndex].Value.ToString();  //设备类型ID
                temp.DevName = CcmbStationType.Properties.Items[CcmbStationType.SelectedIndex].Description.ToString();//设备类型
                temp.Jckz3 = jckz3;
                temp.DevModelID = 0;//设备型号ID 
                temp.DevModel = "";//设备型号名称 
                Dictionary<int, EnumcodeInfo> tempDevClass = Model.DEVServiceModel.QueryDevClasiessCache();//设备种类
                Dictionary<int, EnumcodeInfo> tempDevModel = Model.DEVServiceModel.QueryDevMoelsCache();//设备型号
                Jc_DevInfo tempDev1 = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
                if (null != tempDev1)
                {
                    if (null != tempDevModel)
                    {
                        if (tempDevModel.ContainsKey(tempDev1.Bz4))
                        {
                            temp.DevModelID = tempDev1.Bz4;//设备型号ID 
                            temp.DevModel = tempDevModel[tempDev1.Bz4].StrEnumDisplay;//设备型号名称 
                        }
                    }
                }

                temp.Wzid = tempWz.WzID;//wzID
                temp.Wz = tempWz.Wz;//wz
                temp.Csid = 0;//分站从队巡检标记
                if (this.CcmbCommType.Text == "串口")
                {
                    if (CckRouting.Checked)
                    {
                        temp.Csid = 1;//分站从队巡检标记
                    }
                }
                temp.Point = CcmbStationSourceNum.Text.PadLeft(3, '0') + "0000";//测点编号
                if (this.CcmbCommType.Text == "网口")
                {
                    if (!string.IsNullOrEmpty(CcmbIpModule.Text))
                    {
                        temp.Jckz1 = CcmbIpModule.Text.Split('-')[1];//MAC 地址
                        temp.Jckz2 = CcmbIpModule.Text.Split('-')[0];//IP 地址
                    }
                }
                temp.K1 = 101;//大气压（101）
                temp.K2 = 1; //抽放正负压（0负1正）
                if (this.CcmbCommType.Text == "串口")
                {
                    if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                    {
                        temp.K3 = Convert.ToInt16(CcmbSerialPortNum.Text.Substring(3));
                    }
                }

                //设置网口 绑定串口信息
                if (this.CcmbCommType.Text == "网口")
                {
                    if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                    {
                        temp.K3 = CcmbSerialPortNum.SelectedIndex + 1;
                    }
                }

                temp.Bz1 = 0x1; //运行记录标志
                temp.Bz2 = 0x1;//响铃报警标志
                if (CcmbStaionDefineState.Text == "运行")
                {
                    temp.Bz4 = 0x0;
                }
                else if (CcmbStaionDefineState.Text == "休眠")
                {
                    temp.Bz4 = 0x2;
                }
                else if (CcmbStaionDefineState.Text == "检修")
                {
                    temp.Bz4 = 0x4;
                }
                temp.Bz3 = 0;//默认值 
                if (CchkWindBreak.Checked)
                {
                    temp.Bz3 |= 0x1;
                }
                if (CchkLogicBreak.Checked)
                {
                    temp.Bz3 |= 0x2;
                }
                if (CchkHitchBreak.Checked)
                {
                    temp.Bz3 |= 0x4;
                }
                if (CchkPowerPack.Checked)
                {
                    temp.Bz3 |= 0x8;
                }

                temp.Bz11 = CtxbControlBytesNew.Text;//风电闭锁字节(新)
                temp.Bz10 = CtxbControlBytes.Text;//风电闭锁字节（旧）
                temp.Bz9 = CtxbControlConditon.Text;//风电闭锁条件测点
                temp.K4 = int.Parse(CtxbBackTime.Text);

                temp.Remark = CtxbRemark.Text;

                #region//增加坐标信息及所属区域信息保存  20170829
                if (!string.IsNullOrEmpty(txt_Coordinate.Text))
                {
                    string coordinateX = txt_Coordinate.Text.Split(',')[0];
                    string coordinateY = txt_Coordinate.Text.Split(',')[1];
                    temp.XCoordinate = coordinateX;
                    temp.YCoordinate = coordinateY;
                    if (string.IsNullOrEmpty(PointAreaId))
                    {
                        temp.Areaid = null;
                    }
                    else
                    {
                        temp.Areaid = PointAreaId;
                    }
                    //将图形信息添加到图形测点表中
                    if (!string.IsNullOrEmpty(PointInGraphId))
                    {
                        GraphicspointsinfInfo graphpointInfo = new GraphicspointsinfInfo();
                        graphpointInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        graphpointInfo.GraphId = PointInGraphId;
                        graphpointInfo.PointID = temp.PointID;
                        graphpointInfo.Point = temp.Point;
                        graphpointInfo.GraphBindName = "实时显示";
                        graphpointInfo.GraphBindType = 0;
                        graphpointInfo.DisZoomlevel = "1$22";
                        graphpointInfo.XCoordinate = coordinateX;
                        graphpointInfo.YCoordinate = coordinateY;
                        graphpointInfo.Bz1 = "-1";
                        graphpointInfo.Bz2 = "0";
                        graphpointInfo.Bz3 = "0";
                        graphpointInfo.Upflag = "0";

                        GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest = new GetGraphicspointsinfByGraphIdAndPointRequest();
                        graphicspointsinfrequest.PointId = temp.Point;
                        graphicspointsinfrequest.GraphId = PointInGraphId;
                        GraphicspointsinfInfo graphicspointsinfInfo = graphicspointsinfService.GetGraphicspointsinfByGraphIdAndPoint(graphicspointsinfrequest).Data;
                        if (graphicspointsinfInfo != null)
                        {//先删除之前定义的坐标信息
                            GraphicspointsinfDeleteRequest deletegraphicspointsinfrequest = new GraphicspointsinfDeleteRequest();
                            deletegraphicspointsinfrequest.Id = graphicspointsinfInfo.ID;
                            graphicspointsinfService.DeleteGraphicspointsinf(deletegraphicspointsinfrequest);
                        }
                        //将测点信息保存到图形测点信息表中
                        GraphicspointsinfAddRequest addgraphicspointsinfrequest = new GraphicspointsinfAddRequest();
                        addgraphicspointsinfrequest.GraphicspointsinfInfo = graphpointInfo;
                        graphicspointsinfService.AddGraphicspointsinf(addgraphicspointsinfrequest);
                    }
                }
                #endregion

                if (temp == _subStaionPoint)
                {
                    XtraMessageBox.Show("分站定义无变化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!AddDefAndMac(temp, _subStaionPoint, tempWz))
                {
                    XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //20170313  增加保存成功提示
                    XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //加延时  20170721
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
            this.Close();

        }

        /// <summary>
        /// 同时保存定义及MAC  
        /// </summary>
        public bool AddDefAndMac(Jc_DefInfo temp, Jc_DefInfo _subStaionPoint, Jc_WzInfo tempWz)
        {
            bool Rvalue = false;

            if (_subStaionPoint == null)
            {
                temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                temp.PointID = temp.ID;
                temp.CreateUpdateTime = DateTime.Now;
                temp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                temp.Activity = "1";
                temp.State = 0;//对于新增分站增加默认状态
                temp.DataState = 0;//对于新增分站增加默认状态
                temp.InfoState = InfoState.AddNew;

                #region 新增加时赋值扩展属性 2017.3.14 by
                temp.ClsCommObj = new CommProperty((uint)temp.Fzh);
                temp.ClsAlarmObj = new AlarmProperty();
                temp.ClsCtrlObj = new List<ControlRemote>();
                //temp.realControl = new List<uint>();
                #endregion

                #region 处理MAC BZ1
                Jc_MacInfo TempMAC = null;
                if (!string.IsNullOrEmpty(temp.Jckz1)) //通讯方式或者模块存在差异
                {
                    NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                    NetworkModuleRequest.Mac = temp.Jckz1;
                    var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                    if (result.Data.Count > 0)
                    {
                        TempMAC = result.Data[0];
                    }
                    if (TempMAC != null)
                    {
                        if (string.IsNullOrEmpty(TempMAC.Bz1))
                        {
                            TempMAC.Bz1 = temp.Fzh.ToString();//自动把整个绑定队列全部生成起  20170615
                        }
                        else
                        {
                            //TempMAC.Bz1 = AddStationInMACBZ1(TempMAC.Bz1, temp.Fzh); //xuzp20151229
                            TempMAC.Bz1 = temp.Fzh.ToString();
                        }
                        TempMAC.InfoState = InfoState.Modified;
                    }
                }
                #endregion
                #region 处理Switch BZ1
                Jc_MacInfo TempSwitch = null;
                if (!string.IsNullOrEmpty(temp.Bz12)) //通讯方式或者模块存在差异
                {
                    NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                    NetworkModuleRequest.Mac = temp.Bz12;
                    var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                    if (result.Data.Count > 0)
                    {
                        TempSwitch = result.Data[0];
                    }
                    if (TempSwitch != null)
                    {
                        if (string.IsNullOrEmpty(TempSwitch.Bz1))
                        {
                            TempSwitch.Bz1 = temp.Fzh.ToString() + "|0|0|0|0|0";//自动把整个绑定队列全部生成起  20170615
                        }
                        else
                        {
                            TempSwitch.Bz1 = AddStationInMACBZ1(TempSwitch.Bz1, temp.Fzh); //xuzp20151229
                        }
                        TempSwitch.InfoState = InfoState.Modified;
                    }
                }
                #endregion

                if (TempMAC != null)
                {
                    //todo 监听并获取网关是否保存成功 
                    WaitDialogForm wdf = null;
                    try
                    {
                        var result = DEFServiceModel.AddUpdatePointDefineAndNetworkModuleCache(TempMAC, null, TempSwitch, null, temp, new List<Jc_DefInfo>());
                        if (result) //表示新增,添加定义及MAC信息
                        {
                            Rvalue = true;
                            //新增加的抽放分站，自动生成抽放累计量测点 gaotl UP 20160328
                            if (temp.Devid == "5")
                            {
                                wdf = new WaitDialogForm("正在添加抽放关联设备...", "请等待...");

                                List<Jc_DevInfo> JcDevList = _DeviceDefineService.GetAllDeviceDefineCache().Data;
                                List<Jc_DefInfo> JcCacheList = new List<Jc_DefInfo>();
                                for (int i = 0; i < 16; i++)
                                {
                                    #region  自动增加累计量
                                    Jc_DefInfo temps = new Jc_DefInfo();
                                    temps.Fzh = (Int16)temp.Fzh; //分站号
                                    temps.Kh = Convert.ToInt16((i + 1).ToString());//通道号
                                    temps.Dzh = Convert.ToInt16("0");//地址号
                                    temps.DevPropertyID = Convert.ToInt32("4");//设备性质ID 4 累计量
                                    temps.DevProperty = "累计量";//设备性质种类
                                    temps.DevClassID = 0;//设备种类ID 
                                    temps.DevClass = "";//设备种类名称 
                                    temps.Devid = (61 + i).ToString();//设备类型ID
                                    Jc_DevInfo tempDev = JcDevList.Find(a => a.Devid == temps.Devid);
                                    if (null != tempDev)
                                    {
                                        temps.DevName = tempDev.Name;//设备类型名称
                                    }
                                    temps.DevModelID = 0;//设备型号ID 
                                    temps.DevModel = "";//设备型号名称 
                                    temps.Wzid = tempWz.WzID;//位置ID
                                    temps.Wz = tempWz.Wz;//位置ID
                                    temps.Csid = 0; //措施ID
                                    temps.Point = temps.Fzh.ToString().PadLeft(3, '0') + "L" + temps.Kh.ToString().PadLeft(2, '0') + "0";//测点名称（别名）
                                    temps.Bz1 = 1;//运行记录标记 默认都写成1
                                    temps.Bz2 = 1;//语音报警标记 默认都写成1
                                    temps.Bz3 = 1;//突出预测标记 默认都写成1
                                    temps.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                    temps.Bz4 |= temp.Bz4;

                                    temps.State = 46;//对于变化传感器增加默认状态
                                    temps.DataState = 46;//对于变化传感器增加默认状态
                                    //新增
                                    temps.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                    temps.PointID = temps.ID;
                                    temps.CreateUpdateTime = DateTime.Now;
                                    temps.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                    temps.Activity = "1";
                                    temps.InfoState = InfoState.AddNew;
                                    //AddDEFCache(temps);
                                    JcCacheList.Add(temps);
                                    OperateLogHelper.InsertOperateLog(1, "新增测点【" + temps.Point + "】", "");
                                    #endregion
                                }
                                #region 增加10~12（GD3（流量、温度、负压)、瓦斯、一氧化碳）自动定义  20170415
                                //20190616
                                #region 新增10-1流量传感器
                                Jc_DefInfo additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 10;//通道号
                                additionalTemp.Dzh = 1;

                                additionalTemp.Devid = "1304";
                                Jc_DevInfo tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 

                                    additionalTemp.Bz12 = tempPointDev.Bz8;
                                }

                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置
                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A101";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                               

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion

                                relateAdd(long.Parse(additionalTemp.Devid), additionalTemp);//关联添加   //多参数通道由用户自定义  20170415

                                //#region 新增10-2号温度
                                //additionalTemp = new Jc_DefInfo();
                                //additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                //additionalTemp.PointID = additionalTemp.ID;
                                //additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                //additionalTemp.Kh = 10;//通道号
                                //additionalTemp.Dzh = 2;

                                //additionalTemp.Devid = "271";
                                //tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                //if (null != tempPointDev)
                                //{
                                //    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                //    additionalTemp.Z1 = tempPointDev.Z1;
                                //    additionalTemp.Z2 = tempPointDev.Z2;
                                //    additionalTemp.Z3 = tempPointDev.Z3;
                                //    additionalTemp.Z4 = tempPointDev.Z4;
                                //    additionalTemp.Z5 = tempPointDev.Z5;
                                //    additionalTemp.Z6 = tempPointDev.Z6;
                                //    additionalTemp.Z7 = tempPointDev.Z7;
                                //    additionalTemp.Z8 = tempPointDev.Z8;
                                //    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                //    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                //    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                //    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                //    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                //    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                //}

                                //additionalTemp.Wzid = tempWz.WzID;//位置ID
                                //additionalTemp.Wz = tempWz.Wz;//位置
                                //additionalTemp.Csid = 0; //措施ID


                                //additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A102";//20190616
                                //additionalTemp.K1 = 0;//
                                //additionalTemp.K2 = 0;//
                                //additionalTemp.K3 = 0;
                                //additionalTemp.K4 = 0;//
                                //additionalTemp.K5 = 0;
                                //additionalTemp.K6 = 0;
                                //additionalTemp.K7 = 0;
                                //additionalTemp.K8 = 0;

                                //additionalTemp.Jckz1 = "";
                                //additionalTemp.Jckz2 = "";
                                //additionalTemp.Jckz3 = "";

                                //additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                //additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                //additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                //additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                //additionalTemp.Bz4 |= temp.Bz4;

                                //additionalTemp.State = 46;//对于变化传感器增加默认状态
                                //additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                ////新增                                
                                //additionalTemp.CreateUpdateTime = DateTime.Now;
                                //additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                //additionalTemp.Activity = "1";
                                //additionalTemp.InfoState = InfoState.AddNew;
                                //JcCacheList.Add(additionalTemp);
                                //#endregion

                                //#region 新增10-3 负压
                                //additionalTemp = new Jc_DefInfo();
                                //additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                //additionalTemp.PointID = additionalTemp.ID;
                                //additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                //additionalTemp.Kh = 10;//通道号
                                //additionalTemp.Dzh = 3;
                                //additionalTemp.Devid = "31";
                                // tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                //if (null != tempPointDev)
                                //{
                                //    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                //    additionalTemp.Z1 = tempPointDev.Z1;
                                //    additionalTemp.Z2 = tempPointDev.Z2;
                                //    additionalTemp.Z3 = tempPointDev.Z3;
                                //    additionalTemp.Z4 = tempPointDev.Z4;
                                //    additionalTemp.Z5 = tempPointDev.Z5;
                                //    additionalTemp.Z6 = tempPointDev.Z6;
                                //    additionalTemp.Z7 = tempPointDev.Z7;
                                //    additionalTemp.Z8 = tempPointDev.Z8;
                                //    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                //    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                //    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                //    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                //    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                //    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                //}
                                //additionalTemp.Wzid = tempWz.WzID;//位置ID
                                //additionalTemp.Wz = tempWz.Wz;//位置ID
                                //additionalTemp.Csid = 0; //措施ID


                                //additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A103";//20190616
                                //additionalTemp.K1 = 0;//
                                //additionalTemp.K2 = 0;//
                                //additionalTemp.K3 = 0;
                                //additionalTemp.K4 = 0;//
                                //additionalTemp.K5 = 0;
                                //additionalTemp.K6 = 0;
                                //additionalTemp.K7 = 0;
                                //additionalTemp.K8 = 0;

                                //additionalTemp.Jckz1 = "";
                                //additionalTemp.Jckz2 = "";
                                //additionalTemp.Jckz3 = "";

                                //additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                //additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                //additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                //additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                //additionalTemp.Bz4 |= temp.Bz4;

                                //additionalTemp.State = 46;//对于变化传感器增加默认状态
                                //additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                ////新增                                
                                //additionalTemp.CreateUpdateTime = DateTime.Now;
                                //additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                //additionalTemp.Activity = "1";
                                //additionalTemp.InfoState = InfoState.AddNew;

                                //JcCacheList.Add(additionalTemp);
                                //#endregion

                                #region 新增11号瓦斯
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 11;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "273";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置
                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A110";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion

                                #region 新增12号一氧化碳
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 12;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "25";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A120";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #endregion
                                #region 增加40~43（流量传感器）自动定义  20190616
                                for (int i = 40; i <= 43; i++)
                                {
                                    #region 新增流量传感器
                                    additionalTemp = new Jc_DefInfo();
                                    additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                    additionalTemp.PointID = additionalTemp.ID;
                                    additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                    additionalTemp.Kh = (short)i;//通道号
                                    additionalTemp.Dzh = 0;

                                    additionalTemp.Devid = "35";
                                    tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                    if (null != tempPointDev)
                                    {
                                        additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                        additionalTemp.Z1 = tempPointDev.Z1;
                                        additionalTemp.Z2 = tempPointDev.Z2;
                                        additionalTemp.Z3 = tempPointDev.Z3;
                                        additionalTemp.Z4 = tempPointDev.Z4;
                                        additionalTemp.Z5 = tempPointDev.Z5;
                                        additionalTemp.Z6 = tempPointDev.Z6;
                                        additionalTemp.Z7 = tempPointDev.Z7;
                                        additionalTemp.Z8 = tempPointDev.Z8;
                                        additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                        additionalTemp.DevProperty = tempPointDev.DevProperty;
                                        additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                        additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                        additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                        additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                    }

                                    additionalTemp.Wzid = tempWz.WzID;//位置ID
                                    additionalTemp.Wz = tempWz.Wz;//位置
                                    additionalTemp.Csid = 0; //措施ID


                                    additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A" + i.ToString("00") + "0";
                                    additionalTemp.K1 = 0;//
                                    additionalTemp.K2 = 0;//
                                    additionalTemp.K3 = 0;
                                    additionalTemp.K4 = 0;//
                                    additionalTemp.K5 = 0;
                                    additionalTemp.K6 = 0;
                                    additionalTemp.K7 = 0;
                                    additionalTemp.K8 = 0;

                                    additionalTemp.Jckz1 = "";
                                    additionalTemp.Jckz2 = "";
                                    additionalTemp.Jckz3 = "";

                                    additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                    additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                    additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                    additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                    additionalTemp.Bz4 |= temp.Bz4;

                                    additionalTemp.State = 46;//对于变化传感器增加默认状态
                                    additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                    //新增                                
                                    additionalTemp.CreateUpdateTime = DateTime.Now;
                                    additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                    additionalTemp.Activity = "1";
                                    additionalTemp.InfoState = InfoState.AddNew;
                                    JcCacheList.Add(additionalTemp);
                                    #endregion
                                }
                                #endregion
                                //批量插入所有抽放的测点
                                try
                                {
                                    DEFServiceModel.AddDEFsCache(JcCacheList);
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                            if (temp.Devid == "316")
                            {
                                wdf = new WaitDialogForm("正在添加分站关联设备...", "请等待...");
                                Jc_DefInfo additionalTemp = new Jc_DefInfo();
                                List<Jc_DevInfo> JcDevList = _DeviceDefineService.GetAllDeviceDefineCache().Data;
                                List<Jc_DefInfo> JcCacheList = new List<Jc_DefInfo>();
                                Jc_DevInfo tempPointDev = new Jc_DevInfo();
                                Jc_WzInfo tempWz1 = new Jc_WzInfo();
                                string ljlWz = "";
                                #region 新增1号掘进工作面的甲烷传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 1;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "21";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "掘进工作面甲烷";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A010";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                //分级报警值
                                additionalTemp.Bz8 = "65535,65535,65535,65535";
                                additionalTemp.Bz9 = "255,255,255,255";

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增2号掘进工作面回风流中的甲烷传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 2;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "21";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "掘进工作面回风流甲烷";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A020";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                //分级报警值
                                additionalTemp.Bz8 = "65535,65535,65535,65535";
                                additionalTemp.Bz9 = "255,255,255,255";

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增3号被串掘进工作面入风流中的甲烷传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 3;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "21";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "被串掘进工作面入风流中甲烷";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "A030";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                //分级报警值
                                additionalTemp.Bz8 = "65535,65535,65535,65535";
                                additionalTemp.Bz9 = "255,255,255,255";

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增4号主局部通风机设备开停传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 4;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "41";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "主局部通风机设备开停";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D040";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增5号备局部通风机设备开停传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 5;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "41";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 

                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "备局部通风机设备开停";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D050";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增6号风筒风量开关传感器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 6;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "136";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "风筒风量";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D060";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion

                                #region 新增9号馈电传感器1
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 9;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "43";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//性质
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "1号控制馈电传感器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D090";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增10号馈电传感器2
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 10;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "43";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//性质
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "2号控制馈电传感器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D100";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增11号馈电传感器3
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 11;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "43";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//性质
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "3号控制馈电传感器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D110";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增12号馈电传感器4
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 12;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "43";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//性质
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "4号控制馈电传感器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D120";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增13号馈电传感器5
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 13;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "43";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//性质
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "5号控制馈电传感器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "D130";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                if (tempPointDev.Pl1 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x01;
                                }
                                if (tempPointDev.Pl2 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                }
                                if (tempPointDev.Pl3 == 1)
                                {
                                    additionalTemp.K8 = additionalTemp.K8 | 0x04;
                                }
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz8 = tempPointDev.Xs3;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();
                                additionalTemp.Bz11 = tempPointDev.Color3.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion

                                #region 新增1号控制器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 1;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "45";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "1号控制器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C010";
                                additionalTemp.K1 = temp.Fzh;//
                                additionalTemp.K2 = 9;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;

                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();


                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增2号控制器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 2;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "45";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "2号控制器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C020";
                                additionalTemp.K1 = temp.Fzh;//
                                additionalTemp.K2 = 10;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增3号控制器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 3;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "45";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "3号控制器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C030";
                                additionalTemp.K1 = temp.Fzh;//
                                additionalTemp.K2 = 11;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增4号控制器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 4;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "45";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "4号控制器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C040";
                                additionalTemp.K1 = temp.Fzh;//
                                additionalTemp.K2 = 12;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增5号控制器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 5;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "45";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "5号控制器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C050";
                                additionalTemp.K1 = temp.Fzh;//
                                additionalTemp.K2 = 13;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion
                                #region 新增8号声光报警器
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Fzh = (Int16)temp.Fzh; //分站号
                                additionalTemp.Kh = 8;//通道号
                                additionalTemp.Dzh = 0;

                                additionalTemp.Devid = "47";
                                tempPointDev = JcDevList.Find(a => a.Devid == additionalTemp.Devid);
                                if (null != tempPointDev)
                                {
                                    additionalTemp.DevName = tempPointDev.Name;//设备类型名称
                                    additionalTemp.Z1 = tempPointDev.Z1;
                                    additionalTemp.Z2 = tempPointDev.Z2;
                                    additionalTemp.Z3 = tempPointDev.Z3;
                                    additionalTemp.Z4 = tempPointDev.Z4;
                                    additionalTemp.Z5 = tempPointDev.Z5;
                                    additionalTemp.Z6 = tempPointDev.Z6;
                                    additionalTemp.Z7 = tempPointDev.Z7;
                                    additionalTemp.Z8 = tempPointDev.Z8;
                                    additionalTemp.DevPropertyID = tempPointDev.Type;//模拟量
                                    additionalTemp.DevProperty = tempPointDev.DevProperty;
                                    additionalTemp.DevClassID = tempPointDev.Bz3;//设备种类ID 
                                    additionalTemp.DevClass = tempPointDev.DevClass;//设备种类名称
                                    additionalTemp.DevModelID = tempPointDev.Bz4;//设备型号ID 
                                    additionalTemp.DevModel = tempPointDev.DevModel;//设备型号名称 
                                }

                                #region 先处理安装位置
                                additionalTemp.Wzid = tempWz.WzID;//位置ID
                                additionalTemp.Wz = tempWz.Wz;//位置ID
                                ljlWz = tempWz.Wz + "声光报警器";
                                tempWz1 = Model.WZServiceModel.QueryWZbyWZCache(ljlWz);
                                if (null == tempWz1)
                                {
                                    tempWz1 = new Jc_WzInfo();
                                    tempWz1.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                                    //tempWz.WzID = (this.CglePointName.EditValue).ToString();//
                                    tempWz1.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//保存位置时每次从服务端获取最大的位置ID,用缓存数据有可能会跟服务端系统融合子系统的ID冲突  20180815                    
                                    tempWz1.Wz = ljlWz; //wz
                                    tempWz1.CreateTime = DateTime.Now;// 20170331
                                    tempWz1.InfoState = InfoState.AddNew;
                                    try
                                    {
                                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz1))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
                                        {
                                            XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return false;
                                        }
                                        additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                        additionalTemp.Wz = tempWz1.Wz;//位置ID                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message + ex.StackTrace, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return false;
                                    }
                                }
                                else
                                {
                                    additionalTemp.Wzid = tempWz1.WzID;//位置ID
                                    additionalTemp.Wz = tempWz1.Wz;//位置ID
                                }
                                #endregion

                                additionalTemp.Csid = 0; //措施ID


                                additionalTemp.Point = temp.Fzh.ToString().PadLeft(3, '0') + "C080";
                                additionalTemp.K1 = 0;//
                                additionalTemp.K2 = 0;//
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;//
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;

                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                additionalTemp.Bz1 = 1;//运行记录标记 默认都写成1
                                additionalTemp.Bz2 = 1;//语音报警标记 默认都写成1
                                additionalTemp.Bz3 = 1;//突出预测标记 默认都写成1
                                additionalTemp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                                additionalTemp.Bz4 |= temp.Bz4;
                                additionalTemp.K8 = additionalTemp.K8 | 0x02;
                                additionalTemp.Bz6 = tempPointDev.Xs1;
                                additionalTemp.Bz7 = tempPointDev.Xs2;
                                additionalTemp.Bz9 = tempPointDev.Color1.ToString();
                                additionalTemp.Bz10 = tempPointDev.Color2.ToString();

                                additionalTemp.State = 46;//对于变化传感器增加默认状态
                                additionalTemp.DataState = 46;//对于变化传感器增加默认状态
                                //新增                                
                                additionalTemp.CreateUpdateTime = DateTime.Now;
                                additionalTemp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                additionalTemp.Activity = "1";
                                additionalTemp.InfoState = InfoState.AddNew;
                                JcCacheList.Add(additionalTemp);
                                #endregion

                                //批量插入所有测点
                                try
                                {
                                    DEFServiceModel.AddDEFsCache(JcCacheList);
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                            OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20170111
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (wdf != null)
                        wdf.Close();
                }
            }
            else
            {
                if (temp != _subStaionPoint)
                {
                    temp.ID = _subStaionPoint.ID;
                    temp.PointID = temp.ID;
                    temp.CreateUpdateTime = _subStaionPoint.CreateUpdateTime;
                    temp.Activity = "1";
                    temp.Ssz = _subStaionPoint.Ssz;
                    temp.State = _subStaionPoint.State;
                    temp.DataState = _subStaionPoint.DataState;
                    temp.Alarm = _subStaionPoint.Alarm;
                    temp.Voltage = _subStaionPoint.Voltage;
                    temp.Zts = _subStaionPoint.Zts;
                    temp.InfoState = InfoState.Modified;

                    #region 处理MAC BZ1
                    Jc_MacInfo TempMAC = null;
                    Jc_MacInfo TempMACExist = null;
                    string[] StationBz1;
                    if (temp.Jckz1 != _subStaionPoint.Jckz1) //通讯方式或者模块存在差异
                    {
                        string originBZ1 = "";
                        if (string.IsNullOrEmpty(_subStaionPoint.Jckz1) && !string.IsNullOrEmpty(temp.Jckz1)) //原来是串口通讯，现在是网络通讯
                        {
                            NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            NetworkModuleRequest.Mac = temp.Jckz1;
                            var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                            if (result.Data.Count > 0)
                            {
                                TempMAC = result.Data[0];
                            }
                            if (TempMAC != null)
                            {
                                if (string.IsNullOrEmpty(TempMAC.Bz1))
                                {
                                    TempMAC.Bz1 = temp.Fzh.ToString();//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    //TempMAC.Bz1 = AddStationInMACBZ1(TempMAC.Bz1, temp.Fzh); //xuzp20151229
                                    TempMAC.Bz1 = temp.Fzh.ToString();
                                }
                                TempMAC.InfoState = InfoState.Modified;
                                //if (!Model.MACServiceModel.AddMACCache(TempMAC))//修改MAC
                                //{
                                //    XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    return;
                                //}
                            }
                        }
                        else if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1) && !string.IsNullOrEmpty(temp.Jckz1)) //原来是网络通讯，现在是网络通讯 MAC地址不同
                        {
                            //新增现在的网络模块
                            NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            NetworkModuleRequest.Mac = temp.Jckz1;
                            var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                            if (result.Data.Count > 0)
                            {
                                TempMAC = result.Data[0];
                            }
                            if (TempMAC != null)
                            {
                                if (string.IsNullOrEmpty(TempMAC.Bz1))
                                {
                                    TempMAC.Bz1 = temp.Fzh.ToString();//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    //TempMAC.Bz1 = AddStationInMACBZ1(TempMAC.Bz1, temp.Fzh); //xuzp20151229
                                    TempMAC.Bz1 = temp.Fzh.ToString();
                                }
                                TempMAC.InfoState = InfoState.Modified;
                                //if (!Model.MACServiceModel.AddMACCache(TempMAC))//修改MAC
                                //{
                                //    XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    return;
                                //}

                            }
                            //删除原有的网络模块BZ1中的fzh 
                            NetworkModuleGetByMacRequest ExistNetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            ExistNetworkModuleRequest.Mac = _subStaionPoint.Jckz1;
                            var ExistResult = _NetworkModuleService.GetNetworkModuleCacheByMac(ExistNetworkModuleRequest);
                            if (ExistResult.Data.Count > 0)
                            {
                                TempMACExist = ExistResult.Data[0];
                            }
                            if (TempMACExist != null)
                            {
                                originBZ1 = TempMACExist.Bz1;
                                if (!string.IsNullOrEmpty(TempMACExist.Bz1))
                                {
                                    StationBz1 = TempMACExist.Bz1.Split('|');
                                    TempMACExist.Bz1 = "";
                                    for (int i = 0; i < StationBz1.Length; i++)
                                    {
                                        if (StationBz1[i].ToString() == temp.Fzh.ToString())
                                        {
                                            StationBz1[i] = "0";//xuzp20151229
                                        }
                                        if (StationBz1[i] != "")
                                        {
                                            TempMACExist.Bz1 += StationBz1[i] + "|";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(TempMACExist.Bz1))
                                    {
                                        if (TempMACExist.Bz1.Contains('|'))
                                        {
                                            if (TempMACExist.Bz1.LastIndexOf('|') == TempMACExist.Bz1.Length - 1)
                                            {
                                                TempMACExist.Bz1 = TempMACExist.Bz1.Substring(0, TempMACExist.Bz1.Length - 1);
                                            }
                                        }
                                    }
                                }
                                if (originBZ1 != TempMACExist.Bz1)
                                {
                                    TempMACExist.InfoState = InfoState.Modified;
                                    //if (!Model.MACServiceModel.AddMACCache(TempMACExist)) //修改MAC
                                    //{
                                    //    XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //    return;
                                    //}

                                }

                            }
                        }
                        else if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1) && string.IsNullOrEmpty(temp.Jckz1)) //原来是网络通讯，现在是串口通讯
                        {
                            //删除原有的网络模块BZ1中的fzh
                            NetworkModuleGetByMacRequest ExistNetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            ExistNetworkModuleRequest.Mac = _subStaionPoint.Jckz1;
                            var ExistResult = _NetworkModuleService.GetNetworkModuleCacheByMac(ExistNetworkModuleRequest);
                            if (ExistResult.Data.Count > 0)
                            {
                                TempMACExist = ExistResult.Data[0];
                            }
                            if (TempMACExist != null)
                            {
                                originBZ1 = TempMACExist.Bz1;
                                if (!string.IsNullOrEmpty(TempMACExist.Bz1))
                                {
                                    StationBz1 = TempMACExist.Bz1.Split('|');
                                    TempMACExist.Bz1 = "";
                                    for (int i = 0; i < StationBz1.Length; i++)
                                    {
                                        if (StationBz1[i].ToString() == temp.Fzh.ToString())
                                        {
                                            StationBz1[i] = "0";//xuzp20151229
                                        }
                                        if (StationBz1[i] != "")
                                        {
                                            TempMACExist.Bz1 += StationBz1[i] + "|";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(TempMACExist.Bz1))
                                    {
                                        if (TempMACExist.Bz1.Contains('|'))
                                        {
                                            if (TempMACExist.Bz1.LastIndexOf('|') == TempMACExist.Bz1.Length - 1)
                                            {
                                                TempMACExist.Bz1 = TempMACExist.Bz1.Substring(0, TempMACExist.Bz1.Length - 1);
                                            }
                                        }
                                    }
                                }
                                if (originBZ1 != TempMACExist.Bz1)
                                {
                                    TempMACExist.InfoState = InfoState.Modified;
                                    //if (!Model.MACServiceModel.AddMACCache(TempMACExist))//修改MAC
                                    //{
                                    //    XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //    return;
                                    //}
                                }
                            }
                        }
                    }
                    #endregion
                    #region 处理Switch BZ1
                    Jc_MacInfo TempSwitch = null;
                    Jc_MacInfo TempSwitchExist = null;
                    if (temp.Bz12 != _subStaionPoint.Bz12 && !string.IsNullOrEmpty(temp.Bz12) && !string.IsNullOrEmpty(_subStaionPoint.Bz12)) //通讯方式或者模块存在差异
                    {
                        string originBZ1 = "";
                        if (string.IsNullOrEmpty(_subStaionPoint.Bz12) && !string.IsNullOrEmpty(temp.Bz12)) //原来是串口通讯，现在是网络通讯
                        {
                            NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            NetworkModuleRequest.Mac = temp.Bz12;
                            var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                            if (result.Data.Count > 0)
                            {
                                TempSwitch = result.Data[0];
                            }
                            if (TempSwitch != null)
                            {
                                if (string.IsNullOrEmpty(TempSwitch.Bz1))
                                {
                                    TempSwitch.Bz1 = temp.Fzh.ToString() + "|0|0|0|0|0";//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    TempSwitch.Bz1 = AddStationInMACBZ1(TempSwitch.Bz1, temp.Fzh); //xuzp20151229
                                }
                                TempSwitch.InfoState = InfoState.Modified;
                            }
                        }
                        else if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1) && !string.IsNullOrEmpty(temp.Jckz1)) //原来是网络通讯，现在是网络通讯 MAC地址不同
                        {
                            //新增现在的网络模块
                            NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            NetworkModuleRequest.Mac = temp.Jckz1;
                            var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
                            if (result.Data.Count > 0)
                            {
                                TempSwitch = result.Data[0];
                            }
                            if (TempSwitch != null)
                            {
                                if (string.IsNullOrEmpty(TempSwitch.Bz1))
                                {
                                    TempSwitch.Bz1 = temp.Fzh.ToString() + "|0|0|0|0|0";//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    TempSwitch.Bz1 = AddStationInMACBZ1(TempSwitch.Bz1, temp.Fzh); //xuzp20151229
                                }
                                TempSwitch.InfoState = InfoState.Modified;
                            }
                            //删除原有的网络模块BZ1中的fzh 
                            NetworkModuleGetByMacRequest ExistNetworkModuleRequest = new NetworkModuleGetByMacRequest();
                            ExistNetworkModuleRequest.Mac = _subStaionPoint.Jckz1;
                            var ExistResult = _NetworkModuleService.GetNetworkModuleCacheByMac(ExistNetworkModuleRequest);
                            if (ExistResult.Data.Count > 0)
                            {
                                TempSwitchExist = ExistResult.Data[0];
                            }
                            if (TempSwitchExist != null)
                            {
                                originBZ1 = TempSwitchExist.Bz1;
                                if (!string.IsNullOrEmpty(TempSwitchExist.Bz1))
                                {
                                    StationBz1 = TempSwitchExist.Bz1.Split('|');
                                    TempSwitchExist.Bz1 = "";
                                    for (int i = 0; i < StationBz1.Length; i++)
                                    {
                                        if (StationBz1[i].ToString() == temp.Fzh.ToString())
                                        {
                                            StationBz1[i] = "0";//xuzp20151229
                                        }
                                        if (StationBz1[i] != "")
                                        {
                                            TempSwitchExist.Bz1 += StationBz1[i] + "|";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(TempSwitchExist.Bz1))
                                    {
                                        if (TempSwitchExist.Bz1.Contains('|'))
                                        {
                                            if (TempSwitchExist.Bz1.LastIndexOf('|') == TempSwitchExist.Bz1.Length - 1)
                                            {
                                                TempSwitchExist.Bz1 = TempSwitchExist.Bz1.Substring(0, TempSwitchExist.Bz1.Length - 1);
                                            }
                                        }
                                    }
                                }
                                if (originBZ1 != TempSwitchExist.Bz1)
                                {
                                    TempSwitchExist.InfoState = InfoState.Modified;
                                }

                            }
                        }
                    }
                    #endregion

                    try
                    {
                        #region//判断分站是否休眠或者检修，如果是则更新分站下传感器的状态  20170621
                        List<Jc_DefInfo> UpdateSonPointList = new List<Jc_DefInfo>();
                        if ((temp.Bz4 & 0x02) == 0x02 || (temp.Bz4 & 0x04) == 0x04)
                        {
                            //查找分站下面的所有传感器，并修改传感器的状态为休眠或者检修 
                            UpdateSonPointList = DEFServiceModel.QueryPointByFzhCache(temp.Fzh).FindAll(a => a.DevPropertyID != 0);
                            foreach (Jc_DefInfo item in UpdateSonPointList)
                            {
                                if ((temp.Bz4 & 0x02) == 0x02)
                                {
                                    item.Bz4 = 0x01 + 0x02;//密采标记+标校标记
                                    item.InfoState = InfoState.Modified;
                                }
                                if ((temp.Bz4 & 0x04) == 0x04)
                                {
                                    item.Bz4 = 0x01 + 0x04;//密采标记+检修标记
                                    item.InfoState = InfoState.Modified;
                                }
                            }
                        }
                        else if (_subStaionPoint.Bz4 != temp.Bz4)  //当修改了休眠状态，才进行提示并修改传感器的状态  20170811
                        {//如果未设置休眠或者检修，则判断分站下面传感器是否设置了休眠或者检修，如果传感器设置了休眠或者检修则提示用户是否取消
                            UpdateSonPointList = DEFServiceModel.QueryPointByFzhCache(temp.Fzh).FindAll(a => a.DevPropertyID != 0 && ((a.Bz4 & 0x02) == 0x02 || (a.Bz4 & 0x04) == 0x04));
                            if (UpdateSonPointList.Count > 0)
                            {
                                if (XtraMessageBox.Show("分站下传感器设置了休眠，是否将传感器休眠状态取消？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    foreach (Jc_DefInfo item in UpdateSonPointList)
                                    {
                                        if ((item.Bz4 & 0x02) == 0x02)
                                        {
                                            item.Bz4 &= 0xfd;//取消休眠状态
                                            item.InfoState = InfoState.Modified;
                                        }
                                        if ((item.Bz4 & 0x04) == 0x04)
                                        {
                                            item.Bz4 &= 0xfb;//取消检修状态
                                            item.InfoState = InfoState.Modified;
                                        }
                                    }
                                }
                                else
                                {//如果用户取消，则清除数据，不保存
                                    UpdateSonPointList.Clear();
                                }
                            }
                        }
                        #endregion

                        var result = DEFServiceModel.AddUpdatePointDefineAndNetworkModuleCache(TempMAC, TempMACExist, TempSwitch, TempSwitchExist, temp, UpdateSonPointList);
                        if (result) //表示修改
                        {
                            Rvalue = true;
                            OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.UpdatePointLog(_subStaionPoint, temp), "");
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return Rvalue;
        }
        /// <summary>
        /// 多参数通道自动添加  //除智能断电器外，其它设备 ，让用户自动定义  20170415
        /// </summary>
        private void relateAdd(long TempDevid, Jc_DefInfo temp)
        {
            Jc_DefInfo additionalTemp = new Jc_DefInfo();
            additionalTemp = temp.Clone();

            List<Jc_DevInfo> DevList = Model.DEVServiceModel.QueryDevsCache().ToList();

            Jc_DevInfo TempDTO = DevList.Find(a => a.Devid == TempDevid.ToString());
            if (TempDTO != null)
            {
                int MultCount = 0;
                int.TryParse(TempDTO.Bz8, out MultCount);
                if (MultCount > 1)
                {
                    //如果是多参数则根据定义规则来生成多参数通道  20170505
                    string[] MultChanel = TempDTO.Bz9.Split('|');
                    if (MultChanel.Length > 0)
                    {
                        short TempDzh = 2;//地址号从2号开始
                        foreach (string Chanel in MultChanel)
                        {
                            if (Chanel.Length > 0)
                            {
                                long ChanelDevid = 0;
                                long.TryParse(Chanel, out ChanelDevid);
                                Jc_DevInfo ChanelDTO = DevList.Find(a => a.Devid == ChanelDevid.ToString());
                                #region 新增多参数
                                additionalTemp = new Jc_DefInfo();
                                additionalTemp = temp.Clone();
                                additionalTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                additionalTemp.PointID = additionalTemp.ID;
                                additionalTemp.Dzh = TempDzh;
                                additionalTemp.DevPropertyID = ChanelDTO.Type;//开关量
                                additionalTemp.DevProperty = ChanelDTO.DevProperty;
                                additionalTemp.DevClassID = ChanelDTO.Bz3;//设备种类ID 
                                additionalTemp.DevClass = ChanelDTO.DevClass;//设备种类名称
                                additionalTemp.DevModelID = ChanelDTO.Bz4;
                                additionalTemp.DevModel = ChanelDTO.DevModel;
                                additionalTemp.Devid = ChanelDTO.Devid;
                                additionalTemp.DevName = ChanelDTO.Name;
                                additionalTemp.Point = CreatArrPointTag((uint)temp.Fzh, (uint)temp.Kh, (uint)(additionalTemp.Dzh), ChanelDTO.Type);
                                additionalTemp.K1 = 0;
                                additionalTemp.K2 = 0;
                                additionalTemp.K3 = 0;
                                additionalTemp.K4 = 0;
                                additionalTemp.K5 = 0;
                                additionalTemp.K6 = 0;
                                additionalTemp.K7 = 0;
                                additionalTemp.K8 = 0;
                                if (temp.Bz10 == "1" && ChanelDTO.Type == 2 && temp.K4 == 0)//加入馈电  20170621
                                {
                                    temp.K1 = (int)temp.Fzh;
                                    temp.K2 = (int)temp.Kh;
                                    temp.K4 = TempDzh;//赋为当前传感器的地址号
                                }
                                if (ChanelDTO.Type == 2)
                                {
                                    additionalTemp.K8 = ChanelDTO.Pl1 + (ChanelDTO.Pl2 << 1) + (ChanelDTO.Pl3 << 2);//馈电开关量将设备类型定义的0，1，2态报警关联过来  20180802
                                }
                                additionalTemp.Z1 = ChanelDTO.Z1;
                                additionalTemp.Z2 = ChanelDTO.Z2;
                                additionalTemp.Z3 = ChanelDTO.Z3;
                                additionalTemp.Z4 = ChanelDTO.Z4;
                                additionalTemp.Z5 = ChanelDTO.Z5;
                                additionalTemp.Z6 = ChanelDTO.Z6;
                                additionalTemp.Z7 = ChanelDTO.Z7;
                                additionalTemp.Z8 = ChanelDTO.Z8;
                                additionalTemp.Jckz1 = "";
                                additionalTemp.Jckz2 = "";
                                additionalTemp.Jckz3 = "";

                                //赋值0，1,2态默认值  20170404
                                additionalTemp.Bz6 = ChanelDTO.Xs1;
                                additionalTemp.Bz7 = ChanelDTO.Xs2;
                                additionalTemp.Bz8 = ChanelDTO.Xs3;
                                additionalTemp.Bz9 = ChanelDTO.Color1.ToString();
                                additionalTemp.Bz10 = ChanelDTO.Color2.ToString();
                                additionalTemp.Bz11 = ChanelDTO.Color3.ToString();

                                if (additionalTemp.DevPropertyID == 1)
                                {
                                    additionalTemp.Bz8 = "65535,65535,65535,65535";
                                    additionalTemp.Bz9 = "255,255,255,255";
                                }

                                additionalTemp.Bz12 = "1";//赋值参数个数  20170503
                                additionalTemp.InfoState = InfoState.AddNew;
                                try
                                {
                                    DEFServiceModel.AddDEFCache(additionalTemp);
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                OperateLogHelper.InsertOperateLog(1, "新增测点【" + additionalTemp.Point + "】", "");
                                #endregion
                                TempDzh++;//多参数地址+1
                            }
                        }
                    }
                }
            }
        }
        /// <summary> 创建测点别名
        /// </summary>
        /// <param name="SourceNum">分站号</param>
        /// <param name="ChannelNum">通道号</param>
        /// <param name="AddressNum">地址号</param>
        /// <param name="DevPropertyID">设备性质ID</param>
        /// <returns></returns>
        private string CreatArrPointTag(uint SourceNum, uint ChannelNum, uint AddressNum, int DevPropertyID)
        {

            string ret = "";
            Dictionary<int, EnumcodeInfo> temp; ;
            try
            {
                temp = DEVServiceModel.QueryDevPropertisCache();
                if (null == temp)
                {
                    return ret;
                }
                if (null == temp[DevPropertyID])
                {
                    return ret;
                }
                ret = SourceNum.ToString().PadLeft(3, '0') + temp[DevPropertyID].LngEnumValue4 + ChannelNum.ToString().PadLeft(2, '0') + AddressNum.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error("创建标签点Tag【CreatArrPointTag】", ex);
            }
            return ret;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 删除测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                List<Jc_DefInfo> DefAll = Model.DEFServiceModel.QueryAllCache().ToList();
                List<Jc_DefInfo> fzAll = DefAll.FindAll(a => a.Fzh == int.Parse(this.CcmbStationSourceNum.Text));
                if (fzAll.Count < 1)
                {
                    XtraMessageBox.Show("当前删除的分站不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(this.CcmbStationSourceNum.Text))
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


                        DeleteSubStatoin(Convert.ToInt32(this.CcmbStationSourceNum.Text));
                    }
                    //加延时  20170721
                    Thread.Sleep(1000);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        #endregion

        #region ==========================控件事件=============================
        private void CcmbStationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 20171113
                string stationDevId = CcmbStationType.Properties.Items[CcmbStationType.SelectedIndex].Value.ToString();
                Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(stationDevId);
                int sysId = tempDev.Sysid;
                if (sysId == 4)
                {
                    CpageStationComm.PageEnabled = false;
                }
                else
                {
                    CpageStationComm.PageEnabled = true;

                    if (null != tempDev)
                    {
                        if (tempDev.LC2 != 13)//非智能分站不能勾选智能电源箱  20170623
                        {
                            CchkPowerPack.Enabled = false;
                        }
                        else
                        {
                            CchkPowerPack.Enabled = true;
                        }
                    }

                    //抽放分站进行开停设置，其它分站不可设置   20170318
                    if (CcmbStationType.Text.Contains("抽放") || CcmbStationType.Text.ToUpper().Contains("GD"))
                    {
                        CtbnSetStationType.Enabled = true;
                        CpageXz.PageVisible = true;
                    }
                    else
                    {
                        CtbnSetStationType.Enabled = false;
                        CpageXz.PageVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void Ccmb_CommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (this.CcmbCommType.SelectedItem.ToString() == "网口")
                {

                }
                else if (this.CcmbCommType.SelectedItem.ToString() == "串口")
                {

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public void getjckz3(string msg)
        {
            jckz3 = msg;
        }

        private void CtbnSetStationType_Click(object sender, EventArgs e)
        {
            CF_Fz fz = new CF_Fz(jckz3);
            fz.del = new ddeee(getjckz3);
            fz.ShowDialog();
            // 20170111
            //try
            //{
            //    Station.CFStationType CFStationType = new Station.CFStationType("");
            //    CFStationType.ShowDialog();

            //    //重新加载分站类型
            //    IList<Jc_DevInfo> subStationType;
            //    subStationType = Model.DEVServiceModel.QueryDevByDevpropertIDCache(0);
            //    if (subStationType != null)
            //    {
            //        if (subStationType.Count > 0)
            //        {
            //            CcmbStationType.Properties.Items.Clear();
            //            for (int i = 0; i < subStationType.Count; i++)
            //            {
            //                CcmbStationType.Properties.Items.Add(subStationType[i].Devid.ToString() + "." + subStationType[i].Name);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //}
        }

        private void CbtnSeatchIP_Click(object sender, EventArgs e)
        {
            try
            {
                WaitDialogFormTemp = new Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm("搜索分站IP", "正在搜索分站IP,请稍后......");
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
                LoadStationIPList();
            }
        }
        private void LoadStationIPList()
        {
            List<Jc_DefInfo> SubStations = Model.DEFServiceModel.QueryPointByDevpropertIDCache(0).FindAll(a => a.Fzh != short.Parse(CcmbStationSourceNum.Text));
            IList<Jc_MacInfo> temp = MACServiceModel.QueryAllIPCache().FindAll(a => a.Upflag == "0");
            if (null != temp)
            {
                bool selectFlag = false;
                if (CcmbIpModule.SelectedItem != null)
                {
                    selectFlag = true;
                }

                CcmbIpModule.Properties.Items.Clear();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (SubStations.FindAll(a => a.Jckz1 == temp[i].MAC).Count < 1)//如果IP未被其它分站使用才能选择
                    {
                        CcmbIpModule.Properties.Items.Add(temp[i].IP + "-" + temp[i].MAC);
                    }
                }

                //20170313  如果原来分站IP为空，重新搜索后新增默认值
                if (temp.Count > 0 && selectFlag == false)
                {
                    CcmbIpModule.SelectedIndex = 0;
                }
            }
        }
        private void CcmbCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (CcmbCommType.Text == "网口")
                //{
                //    CcmbIpModule.Enabled = true;
                //    CbtnSeatchIP.Enabled = true;
                //    //CcmbSerialPortNum.Enabled = false;
                //    CckRouting.Enabled = false;
                //}
                //else if (CcmbCommType.Text == "串口")
                //{
                //    CcmbIpModule.Enabled = false;
                //    CbtnSeatchIP.Enabled = false;
                //    CcmbSerialPortNum.Enabled = true;
                //    CckRouting.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CgleStaionAdress_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (!this.DesignMode)
            {
                string displayName = this.CgleStaionAdress.Properties.DisplayMember;
                string valueName = this.CgleStaionAdress.Properties.ValueMember;
                string display = e.DisplayValue.ToString();
                if (string.IsNullOrEmpty(display)) //xuzp20151023
                {
                    return;
                }
                DataTable dtTemp = this.CgleStaionAdress.Properties.DataSource as DataTable;
                if (dtTemp != null)
                {
                    dtTemp.CaseSensitive = true;//设置区分大小写  20170815
                    DataRow[] selectedRows = dtTemp.Select(string.Format("{0}='{1}'", displayName, display.Replace("'", "‘")));
                    if (selectedRows == null || selectedRows.Length == 0)
                    {
                        DataRow row = dtTemp.NewRow();
                        row[displayName] = display;
                        row[valueName] = WZServiceModel.GetMaxWzidInCache(dtTemp) + 1;//xuzp20151109
                        dtTemp.Rows.Add(row);
                        dtTemp.AcceptChanges();
                    }
                }

                e.Handled = true;
            }
        }

        private void CbtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Stationverify())
                {
                    return;
                }
                IList<Jc_DefInfo> Copytemp = Model.DEFServiceModel.QueryPointByFzhCache(Convert.ToInt16(CcmbStationSourceNum.Text));
                if (null == Copytemp)
                {
                    return;
                }
                if (Copytemp.Count == 0)
                {
                    return;
                }
                Model.CopyInf.CopySensorUStation = new List<Jc_DefInfo>();
                foreach (var item in Copytemp)
                {
                    item.Ssz = "";
                    item.State = 46;
                    item.DataState = 46;
                    if (item.DevPropertyID == 0) //如果是分站
                    {
                        item.State = 0;
                        item.DataState = 0;
                    }
                    item.Alarm = 0;
                    item.Voltage = 0;
                    item.Zts = new DateTime(1900, 1, 1, 0, 0, 0);
                    item.InfoState = InfoState.AddNew;
                    Model.CopyInf.CopySensorUStation.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CbtnPasty_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CcmbStationSourceNum.Text))
                {
                    XtraMessageBox.Show("请选设备地址号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Model.CopyInf.CopySensorUStation == null)
                {
                    return;
                }
                if (Model.CopyInf.CopySensorUStation.Count == 0)
                {
                    return;
                }
                //增加分站IP挂接分站是否达到上限  20170713
                Jc_MacInfo TempMAC1 = Model.MACServiceModel.QueryMACByCode(Model.CopyInf.CopySensorUStation[0].Jckz1);
                IList<Jc_DefInfo> tempStation = Model.DEFServiceModel.QueryPointByMACCache(Model.CopyInf.CopySensorUStation[0].Jckz1);
                tempStation = tempStation.Where(i => i.K3 == Model.CopyInf.CopySensorUStation[0].K3).ToList();
                if (tempStation != null)
                {
                    int tempMacStationCount = 8;
                    //if (!string.IsNullOrEmpty(TempMAC1.Bz5))
                    //{
                    //    int.TryParse(TempMAC1.Bz5, out tempMacStationCount);
                    //}
                    SettingInfo cdt = Model.CONFIGServiceModel.GetConfigFKey("CommNum");
                    if (cdt != null)
                    {
                        tempMacStationCount = int.Parse(cdt.StrValue);
                    }
                    if (tempStation.Count >= tempMacStationCount) //分站数目可配置
                    {
                        XtraMessageBox.Show("所复制的分站所属分站IP挂接分站数达到上限，不能再继续进行复制操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }


                Jc_MacInfo TempMAC;
                if (_subStaionPoint == null)
                {
                    foreach (var item in Model.CopyInf.CopySensorUStation)
                    {
                        item.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        item.PointID = item.ID;
                        item.Fzh = Convert.ToInt16(CcmbStationSourceNum.Text);
                        item.Point = item.Fzh.ToString().PadLeft(3, '0') + item.Point.Substring(3);
                        item.InfoState = InfoState.AddNew;
                        //判断测点重复  20170324
                        Jc_DefInfo tempdef = DEFServiceModel.QueryPointByCodeCache(item.Point);
                        if (tempdef != null)
                        {
                            XtraMessageBox.Show("粘贴时检测到有重复数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (item.DevPropertyID == 0)
                        {
                            #region//风电闭锁口Bz9条件修改  20170711
                            string newBz9 = "";
                            if (item.Bz9.Length > 0)
                            {
                                if (item.Bz11.Length > 0)//新风电闭锁
                                {
                                    string[] windPowerAtresiaBindPoint = item.Bz9.Split('|');
                                    //修改甲烷口的分站号
                                    string[] T1T2Point = windPowerAtresiaBindPoint[0].Split(',');
                                    foreach (string tempPoint in T1T2Point)
                                    {
                                        newBz9 += item.Fzh.ToString("000") + tempPoint.Substring(3) + ",";
                                    }
                                    if (newBz9.Contains(","))
                                    {
                                        newBz9 = newBz9.Substring(0, newBz9.Length - 1);
                                    }
                                    newBz9 = newBz9 + "|";
                                    //修改风机绑定开停分站号
                                    string[] KTPoint = windPowerAtresiaBindPoint[1].Split('&');
                                    foreach (string tempPoint in KTPoint)
                                    {
                                        newBz9 += item.Fzh.ToString("000") + tempPoint.Substring(3) + "&";
                                    }
                                    if (newBz9.Contains("&"))
                                    {
                                        newBz9 = newBz9.Substring(0, newBz9.Length - 1);
                                    }
                                    newBz9 = newBz9 + "|";
                                    //修改控制口分站号
                                    string[] ControlPoint = windPowerAtresiaBindPoint[2].Split('&');
                                    foreach (string tempPoint in ControlPoint)
                                    {
                                        newBz9 += item.Fzh.ToString("000") + tempPoint.Substring(3) + "&";
                                    }
                                    if (newBz9.Contains("&"))
                                    {
                                        newBz9 = newBz9.Substring(0, newBz9.Length - 1);
                                    }
                                }
                                else //老风电闭锁
                                {
                                    string[] windPowerAtresiaBindPoint = item.Bz9.Split('|');
                                    newBz9 += windPowerAtresiaBindPoint[0] + "|";
                                    //修改绑定的设备的分站号
                                    if (windPowerAtresiaBindPoint.Length > 1)
                                    {
                                        string[] bindPointArray = windPowerAtresiaBindPoint[1].Split(',');
                                        foreach (string tempPoint in bindPointArray)
                                        {
                                            newBz9 += item.Fzh.ToString("000") + tempPoint.Substring(3) + ",";
                                        }
                                    }
                                    if (newBz9.Contains(","))
                                    {
                                        newBz9 = newBz9.Substring(0, newBz9.Length - 1);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(newBz9))
                            {
                                item.Bz9 = newBz9;
                            }
                            #endregion

                            if (!string.IsNullOrEmpty(this.CcmbCommType.Text))
                            {
                                if (this.CcmbCommType.Text == "网口")
                                {
                                    //item.K3 = 0;

                                    //if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                                    //{
                                    //    item.K3 = CcmbSerialPortNum.SelectedIndex + 1;
                                    //}

                                    if (!string.IsNullOrEmpty(this.CcmbIpModule.Text))
                                    {
                                        item.Jckz1 = CcmbIpModule.Text.Split('-')[1];//MAC 地址
                                        item.Jckz2 = CcmbIpModule.Text.Split('-')[0];//IP 地址

                                    }
                                }
                                else if (this.CcmbCommType.Text == "串口")
                                {
                                    item.Jckz1 = "";
                                    item.Jckz2 = "";
                                    if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                                    {
                                        item.K3 = Convert.ToInt16(CcmbSerialPortNum.Text.Substring(3));
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        Model.DEFServiceModel.AddDEFsCache(Model.CopyInf.CopySensorUStation);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (var item in Model.CopyInf.CopySensorUStation)
                    {
                        if (item.DevPropertyID == 0)
                        {
                            _subStaionPoint = item.Clone();
                        }
                        OperateLogHelper.InsertOperateLog(1, "复制测点【" + item.Point + "】", "");
                    }

                    #region 处理MAC BZ1

                    if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1))
                    {

                        TempMAC = Model.MACServiceModel.QueryMACByCode(_subStaionPoint.Jckz1);
                        if (TempMAC != null)
                        {
                            if (string.IsNullOrEmpty(TempMAC.Bz1))
                            {
                                TempMAC.Bz1 = _subStaionPoint.Fzh.ToString();//自动把整个绑定队列全部生成起  20170615
                            }
                            else
                            {
                                //TempMAC.Bz1 = AddStationInMACBZ1(TempMAC.Bz1, _subStaionPoint.Fzh, _subStaionPoint.K3); //xuzp20151229
                                TempMAC.Bz1 = _subStaionPoint.Fzh.ToString();
                            }
                            TempMAC.InfoState = InfoState.Modified;
                            try
                            {
                                if (!Model.MACServiceModel.UpdateMACCache(TempMAC))
                                {
                                    XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    #endregion
                    #region 处理Switch BZ1
                    Jc_MacInfo TempSwitch;
                    if (!string.IsNullOrEmpty(_subStaionPoint.Bz12))
                    {

                        TempSwitch = Model.MACServiceModel.QueryMACByCode(_subStaionPoint.Bz12);
                        if (TempSwitch != null)
                        {
                            if (string.IsNullOrEmpty(TempSwitch.Bz1))
                            {
                                TempSwitch.Bz1 = _subStaionPoint.Fzh.ToString() + "|0|0|0|0|0";//自动把整个绑定队列全部生成起  20170615
                            }
                            else
                            {
                                TempSwitch.Bz1 = AddStationInMACBZ1(TempSwitch.Bz1, _subStaionPoint.Fzh, _subStaionPoint.K3); //xuzp20151229
                            }
                            TempSwitch.InfoState = InfoState.Modified;
                            try
                            {
                                if (!Model.MACServiceModel.UpdateMACCache(TempSwitch))
                                {
                                    XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    #endregion
                    //加延时  20170721
                    Thread.Sleep(1000);
                    LoadBasicInf();
                }
                else
                {
                    if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DeleteSubStatoin(_subStaionPoint.Fzh);
                        foreach (var item in Model.CopyInf.CopySensorUStation)
                        {
                            item.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                            item.PointID = item.ID;
                            item.Fzh = Convert.ToInt16(CcmbStationSourceNum.Text);
                            item.Point = item.Fzh.ToString().PadLeft(3, '0') + item.Point.Substring(3);
                            item.InfoState = InfoState.AddNew;
                            if (item.DevPropertyID == 0)
                            {
                                item.Ssz = _subStaionPoint.Ssz;
                                item.State = _subStaionPoint.State;
                                item.DataState = _subStaionPoint.DataState;
                                item.Voltage = _subStaionPoint.Voltage;
                                item.Alarm = _subStaionPoint.Alarm;
                                item.Zts = _subStaionPoint.Zts;
                            }
                        }
                        try
                        {
                            Model.DEFServiceModel.AddDEFsCache(Model.CopyInf.CopySensorUStation);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        foreach (var item in Model.CopyInf.CopySensorUStation)
                        {
                            if (item.DevPropertyID == 0)
                            {
                                _subStaionPoint = item.Clone();
                            }
                            OperateLogHelper.InsertOperateLog(1, "复制测点【" + item.Point + "】", "");
                        }

                        #region 处理MAC BZ1

                        if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1))
                        {

                            TempMAC = Model.MACServiceModel.QueryMACByCode(_subStaionPoint.Jckz1);
                            if (TempMAC != null)
                            {
                                if (string.IsNullOrEmpty(TempMAC.Bz1))
                                {
                                    TempMAC.Bz1 = _subStaionPoint.Fzh.ToString();//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    //TempMAC.Bz1 = AddStationInMACBZ1(TempMAC.Bz1, _subStaionPoint.Fzh, _subStaionPoint.K3); //xuzp20151229
                                    TempMAC.Bz1 = _subStaionPoint.Fzh.ToString();
                                }
                                TempMAC.InfoState = InfoState.Modified;
                                try
                                {
                                    if (!Model.MACServiceModel.UpdateMACCache(TempMAC))
                                    {
                                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        #endregion
                        #region 处理Switch BZ1
                        Jc_MacInfo TempSwitch;
                        if (!string.IsNullOrEmpty(_subStaionPoint.Bz12))
                        {

                            TempSwitch = Model.MACServiceModel.QueryMACByCode(_subStaionPoint.Bz12);
                            if (TempSwitch != null)
                            {
                                if (string.IsNullOrEmpty(TempSwitch.Bz1))
                                {
                                    TempSwitch.Bz1 = _subStaionPoint.Fzh.ToString() + "|0|0|0|0|0";//自动把整个绑定队列全部生成起  20170615
                                }
                                else
                                {
                                    TempSwitch.Bz1 = AddStationInMACBZ1(TempSwitch.Bz1, _subStaionPoint.Fzh, _subStaionPoint.K3); //xuzp20151229
                                }
                                TempSwitch.InfoState = InfoState.Modified;
                                try
                                {
                                    if (!Model.MACServiceModel.UpdateMACCache(TempSwitch))
                                    {
                                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        #endregion
                        //加延时  20170721
                        Thread.Sleep(1000);
                        LoadBasicInf();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CbtnWindBreak_Click(object sender, EventArgs e)
        {
            try
            {
                if (_subStaionPoint == null)
                {
                    XtraMessageBox.Show("请先定义并保存分站信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                CFWindBreakCfg tempWind = new CFWindBreakCfg(_subStaionPoint, this, CtxbControlBytes.Text, this.CtxbControlConditon.Text);
                tempWind.ShowDialog();

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CbtnLogicBreak_Click(object sender, EventArgs e)
        {
            try
            {
                if (_subStaionPoint == null)
                {
                    XtraMessageBox.Show("请先定义并保存分站信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //CFWLogicBreakCfg tempLogic = new CFWLogicBreakCfg();
                //tempLogic.ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CchkWindBreak_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CchkWindBreak.Checked)
                {
                    if (_subStaionPoint != null)
                    {
                        if (CchkLogicBreak.Checked)
                        {
                            XtraMessageBox.Show("请先取消逻辑控制配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CbtnWindBreak.Enabled = false;
                            CbtnWindBreakNew.Enabled = false;
                            return;
                        }
                        CbtnWindBreak.Enabled = true;
                        CbtnWindBreakNew.Enabled = true;
                    }
                    else
                    {
                        XtraMessageBox.Show("请先定义并保存分站信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CbtnWindBreak.Enabled = false;
                        CbtnWindBreakNew.Enabled = false;
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(CtxbControlBytes.Text) || !string.IsNullOrEmpty(CtxbControlBytesNew.Text))
                    {
                        if (!CchkLogicBreak.Checked)
                        {
                            if (XtraMessageBox.Show("存在风电闭锁数据 是否删除?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                CtxbControlBytes.Text = "";
                                CtxbControlBytesNew.Text = "";
                                CtxbControlConditon.Text = "";
                                CbtnWindBreak.Enabled = false;
                                CbtnWindBreakNew.Enabled = false;
                            }
                            else
                            {
                                CbtnWindBreak.Enabled = true;
                                CbtnWindBreakNew.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        CbtnWindBreak.Enabled = false;
                        CbtnWindBreakNew.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CchkLogicBreak_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CchkLogicBreak.Checked)
                {
                    if (_subStaionPoint != null)
                    {
                        if (CchkWindBreak.Checked)
                        {
                            XtraMessageBox.Show("请先取消风电闭锁配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CchkLogicBreak.Checked = false;
                            return;
                        }
                        CbtnLogicBreak.Enabled = true;
                    }
                    else
                    {
                        XtraMessageBox.Show("请先定义并保存分站信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CchkLogicBreak.Checked = false;
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(CtxbControlBytes.Text) || !string.IsNullOrEmpty(CtxbControlBytesNew.Text))
                    {
                        if (!CchkWindBreak.Checked)
                        {
                            if (XtraMessageBox.Show("存在逻辑控制数据 是否删除?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                CtxbControlBytes.Text = "";
                                CtxbControlBytesNew.Text = "";
                                CtxbControlConditon.Text = "";
                                CbtnLogicBreak.Enabled = false;
                            }
                            else
                            {
                                CchkLogicBreak.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CbtnLogicBreak.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region ==========================界面验证=============================
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
        private bool Stationverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(CcmbStationSourceNum.Text))
            {
                XtraMessageBox.Show("请选设备地址号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (string.IsNullOrEmpty(CgleStaionAdress.Text))
            {
                XtraMessageBox.Show("请输入设备安装位置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CgleStaionAdress.Text))
            {
                XtraMessageBox.Show("设备安装位置中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CgleStaionAdress.Text.Length > 30)
            {
                XtraMessageBox.Show("设备安装位置长度不能超过30个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbStationType.Text))
            {
                XtraMessageBox.Show("请选择设备类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbStaionDefineState.Text))
            {
                XtraMessageBox.Show("请选择运行状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbCommType.Text))
            {
                XtraMessageBox.Show("请选择通讯方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (CcmbCommType.Text == "网口")
            {
                if (string.IsNullOrEmpty(CcmbIpModule.Text))
                {
                    XtraMessageBox.Show("请选择分站IP", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return ret;
                }
                if (CcmbIpModule.Text.Contains("-"))
                {
                    string IP = CcmbIpModule.Text.Substring(0, CcmbIpModule.Text.IndexOf('-'));
                    string Mac = CcmbIpModule.Text.Substring(CcmbIpModule.Text.IndexOf('-') + 1);
                    if (!IsRightMac(Mac))
                    {
                        XtraMessageBox.Show("MAC地址不合法,格式ip-mac如：192.168.1.11-08.01.AD.EF.01.DE", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                    if (!Basic.Framework.Common.ValidationHelper.IsRightIP(IP))
                    {
                        XtraMessageBox.Show("IP地址不合法,格式ip-mac如：192.168.1.11-08.01.AD.EF.01.DE", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                else
                {
                    XtraMessageBox.Show("IP及MAC输入不合法,格式ip-mac如：192.168.1.11-08.01.AD.EF.01.DE", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }

                if (_subStaionPoint == null)
                {
                    Jc_MacInfo TempMAC = Model.MACServiceModel.QueryMACByCode(CcmbIpSwitches.Text.Substring(CcmbIpSwitches.Text.IndexOf('-') + 1));
                    //增加分站IP挂接分站是否达到上限  20170713
                    IList<Jc_DefInfo> temp = Model.DEFServiceModel.QueryPointBySwitchCache(CcmbIpSwitches.Text.Substring(CcmbIpSwitches.Text.IndexOf('-') + 1));
                    if (temp != null && TempMAC != null)
                    {
                        int tempMacStationCount = 6;
                        if (!string.IsNullOrEmpty(TempMAC.Bz5))
                        {
                            int.TryParse(TempMAC.Bz5, out tempMacStationCount);
                        }
                        if (temp.Count >= tempMacStationCount) //分站数目可配置
                        {
                            XtraMessageBox.Show("所选交换机挂接分站数达到上限，请选择其他交换机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return ret;
                        }
                    }
                    //IList<Jc_DefInfo> temp = Model.DEFServiceModel.QueryPointByMACCache(CcmbIpModule.Text.Substring(CcmbIpModule.Text.IndexOf('-') + 1));
                    //if (temp != null)
                    //{
                    //    if (temp.Count >= 8) //分站数目可配置
                    //    {
                    //        XtraMessageBox.Show("所选分站IP挂接分站数达到上限，请选择其他分站IP", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        return ret;
                    //    }
                    //}
                }
                else
                {
                    if (_subStaionPoint.Jckz1 != CcmbIpModule.Text.Substring(CcmbIpModule.Text.IndexOf('-') + 1))
                    {
                        IList<Jc_DefInfo> temp = Model.DEFServiceModel.QueryPointByMACCache(CcmbIpSwitches.Text.Substring(CcmbIpSwitches.Text.IndexOf('-') + 1));
                        Jc_MacInfo tempMac = Model.MACServiceModel.QueryMACByCode(CcmbIpSwitches.Text.Substring(CcmbIpSwitches.Text.IndexOf('-') + 1));
                        int MacCount = 6;
                        if (tempMac != null)
                        {
                            if (!string.IsNullOrEmpty(tempMac.Bz5))
                            {
                                MacCount = int.Parse(tempMac.Bz5);
                            }
                        }
                        if (temp != null && tempMac != null)
                        {
                            if (temp.Count >= MacCount) //分站数目可配置
                            {
                                XtraMessageBox.Show("所选交换机挂接分站数达到上限，请选择其他交换机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return ret;
                            }
                        }
                    }
                }

            }
            //else if (CcmbCommType.Text == "串口")
            //{
            //    if (string.IsNullOrEmpty(CcmbSerialPortNum.Text))
            //    {
            //        XtraMessageBox.Show("请选择串口编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return ret;
            //    }
            //}
            if (string.IsNullOrEmpty(CtxbBackTime.Text))
            {
                XtraMessageBox.Show("请设置通讯容错次数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            int backTime = 0;
            int.TryParse(CtxbBackTime.Text, out backTime);
            if (backTime > 20)
            {
                XtraMessageBox.Show("通讯容错次数最多设置20次", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            //if (CchkWindBreak.Checked)
            //{
            //    if (string.IsNullOrEmpty(this.CtxbControlBytes.Text) && string.IsNullOrEmpty(this.CtxbControlBytesNew.Text))
            //    {
            //        XtraMessageBox.Show("请设置风电闭锁逻辑", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return ret;
            //    }
            //}
            //if (CchkLogicBreak.Checked)
            //{
            //    if (string.IsNullOrEmpty(this.CtxbControlBytes.Text))
            //    {
            //        XtraMessageBox.Show("请设置逻辑控制逻辑", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return ret;
            //    }
            //}
            ret = true;
            return ret;
        }

        private void CtxbAllowWorngTime_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CtxbBackTime_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CtxbNetPort_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CtxbNetPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CfStation_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        #endregion

        #region ==========================业务函数=============================
        /// <summary>
        /// 加载控件默认信息
        /// </summary>
        private void LoadPretermitInf()
        {

            List<Jc_DefInfo> SubStations = Model.DEFServiceModel.QueryPointByDevpropertIDCache(0);

            ////加载分站编号，最多只能定义到256号分站  20170616
            for (int i = 0; i < 256; i++)
            {
                if (SubStations != null)
                {
                    if (null != SubStations.Where(item => item.Fzh == i + 1))
                    {
                        if (null != SubStations.Where(item => item.Fzh == i + 1).FirstOrDefault())
                        {
                            continue;
                        }
                    }
                }
                CcmbStationSourceNum.Properties.Items.Add(i + 1);
            }

            //安装位置
            //this.CgleStaionAdress.Properties.View.BestFitColumns();
            //this.CgleStaionAdress.Properties.DisplayMember = "Wz";
            //this.CgleStaionAdress.Properties.ValueMember = "WzID";
            List<Jc_WzInfo> subStationWZ = Model.WZServiceModel.QueryWZsCache();
            DataTable dtTemp = DevAdapter.ListToDataTable(subStationWZ);
            if (null != subStationWZ)
            {
                dtTemp.CaseSensitive = true;//设置区分大小写  20170817
                CgleStaionAdress.Properties.DataSource = dtTemp;
            }
            //加载分站类型
            List<Jc_DevInfo> subStationType;
            subStationType = Model.DEVServiceModel.QueryDevByDevpropertIDCache(0);
            if (subStationType != null)
            {
                for (int i = 0; i < subStationType.Count; i++)
                {
                    //CcmbStationType.Properties.Items.Add(subStationType[i].Devid.ToString() + "." + subStationType[i].Name);
                    ImageComboBoxItem item = new ImageComboBoxItem();
                    item.Description = subStationType[i].Name;
                    item.Value = subStationType[i].Devid;
                    CcmbStationType.Properties.Items.Add(item);
                }
            }

            //加载定义状态
            CcmbStaionDefineState.Properties.Items.Add("运行");
            //根据配置判断，是否可以休眠  20180124
            SettingInfo setting = CONFIGServiceModel.GetConfigFKey("EnableDormancy");
            bool enableDormancy = true;
            if (setting != null)
            {
                if (setting.StrValue == "1")
                {
                    enableDormancy = true;
                }
                else
                {
                    enableDormancy = false;
                }
            }
            if (enableDormancy)
            {
                CcmbStaionDefineState.Properties.Items.Add("休眠");
            }
            //CcmbStaionDefineState.Properties.Items.Add("检修"); 取消检修状态设置  20170627

            //加载通讯方式
            CcmbCommType.Properties.Items.Add("网口");
            //CcmbCommType.Properties.Items.Add("串口"); //串口未实现，暂时不让用户选择串口  20170330



            //加载IP
            IList<Jc_MacInfo> TempMAC = MACServiceModel.QueryAllIPCache().FindAll(a => a.Upflag == "0");
            if (TempMAC != null)
            {
                CcmbIpModule.Properties.Items.Clear();
                for (int i = 0; i < TempMAC.Count; i++)
                {
                    if (SubStations.FindAll(a => a.Jckz1 == TempMAC[i].MAC).Count < 1)//如果IP未被其它分站使用才能选择
                    {
                        CcmbIpModule.Properties.Items.Add(TempMAC[i].IP + "-" + TempMAC[i].MAC);
                    }
                }
            }

            //加载交换机
            IList<Jc_MacInfo> TempMACSwitches = MACServiceModel.QueryAllIPCache().FindAll(a => a.Upflag == "1");
            if (TempMACSwitches != null)
            {
                CcmbIpSwitches.Properties.Items.Clear();
                for (int i = 0; i < TempMACSwitches.Count; i++)
                {
                    CcmbIpSwitches.Properties.Items.Add(TempMACSwitches[i].IP + "-" + TempMACSwitches[i].MAC);
                }
            }

            //加载COM
            //TempMAC = MACServiceModel.QueryAllCOMCache();
            //if (TempMAC != null)
            //{
            //    CcmbSerialPortNum.Properties.Items.Clear();
            //    for (int i = 0; i < TempMAC.Count; i++)
            //    {
            //        CcmbSerialPortNum.Properties.Items.Add(TempMAC[i].MAC);
            //    }
            //}

            CcmbSerialPortNum.Properties.Items.Add("串口1");
            CcmbSerialPortNum.Properties.Items.Add("串口2");
            CcmbSerialPortNum.Properties.Items.Add("串口3");
            CcmbSerialPortNum.Properties.Items.Add("串口4");
        }
        /// <summary>
        /// 加定载分站信息
        /// </summary>
        private void LoadBasicInf()
        {

            IList<Jc_DefInfo> SubStations = Model.DEFServiceModel.QueryPointByDevpropertIDCache(0);

            CcmbSerialPortNum.SelectedIndex = 0;//预定义串口号
            CtxbBackTime.Text = "5";//预定义通讯容错次数
            if (_subStaionPoint == null)//新增分站 
            {
                if (CcmbStationSourceNum.Properties.Items.Count > 0)
                {
                    CcmbStationSourceNum.SelectedIndex = 0;
                }
                if (CcmbStationType.Properties.Items.Count > 0)
                {
                    for (int i = 0; i < CcmbStationType.Properties.Items.Count; i++)
                    {
                        if (CcmbStationType.Properties.Items[i].Description.ToString().Contains("普通分站"))
                        {
                            CcmbStationType.SelectedIndex = i;
                        }
                    }
                }
                for (int i = 0; i < SubStations.Count; i++)
                {
                    if (CcmbStationSourceNum.Properties.Items.Contains(SubStations[i].Fzh))
                    {
                        CcmbStationSourceNum.Properties.Items.Remove(SubStations[i].Fzh);
                    }
                }
                CcmbStaionDefineState.SelectedItem = "运行"; //加载预定义运行状态
                if (!string.IsNullOrEmpty(_StrParameter1))
                {
                    CcmbCommType.Text = _StrParameter1;
                }
                else
                {
                    CcmbCommType.Text = "网口";//预定义通讯方式
                }
                if (!string.IsNullOrEmpty(_StrParameter2))
                {
                    for (int i = 0; i < CcmbIpSwitches.Properties.Items.Count; i++)
                    {
                        if (CcmbIpSwitches.Properties.Items[i].ToString().Contains(_StrParameter2))
                        {
                            this.CcmbIpSwitches.SelectedIndex = i;
                            break;
                        }
                    }
                }

                txt_Coordinate.Text = "";
                PointAreaId = "";
            }
            else
            {
                jckz3 = _subStaionPoint.Jckz3;// 20170111
                CcmbStationSourceNum.Text = _subStaionPoint.Fzh.ToString();//分站号
                CcmbStationSourceNum.Enabled = false;
                CgleStaionAdress.EditValue = _subStaionPoint.Wzid;
                //CcmbStationType.Text = _subStaionPoint.DevName;//设备名称
                CcmbStationType.SelectedIndex = -1;
                for (int i = 0; i < CcmbStationType.Properties.Items.Count; i++)
                {
                    if (CcmbStationType.Properties.Items[i].Value.ToString() == _subStaionPoint.Devid.ToString())
                    {
                        CcmbStationType.SelectedIndex = i;
                        break;
                    }
                }
                CcmbStationType.Enabled = false;
                //修改时不能再修改分站绑定的Mac和IP
                this.CcmbIpModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                if (_subStaionPoint.Bz4 == 0x0)
                {
                    CcmbStaionDefineState.Text = "运行";
                }
                else if (_subStaionPoint.Bz4 == 0x2)
                {
                    CcmbStaionDefineState.Text = "休眠";
                }
                else if (_subStaionPoint.Bz4 == 0x4)
                {
                    CcmbStaionDefineState.Text = "检修";
                }

                if (!string.IsNullOrEmpty(_subStaionPoint.Jckz1))
                {
                    CcmbCommType.SelectedItem = "网口";
                    CcmbIpModule.Text = _subStaionPoint.Jckz2 + "-" + _subStaionPoint.Jckz1;
                    CcmbIpSwitches.Text = _subStaionPoint.Bz13 + "-" + _subStaionPoint.Bz12;
                    //CcmbSerialPortNum.SelectedIndex = _subStaionPoint.K3 - 1;
                    CcmbSerialPortNum.SelectedItem = "串口" + _subStaionPoint.K3.ToString();
                }
                else if (_subStaionPoint.K3 > 0)//串口K3>0  20170331
                {
                    CcmbCommType.SelectedItem = "串口";//通讯方式
                    CcmbSerialPortNum.SelectedItem = "COM" + _subStaionPoint.K3.ToString();//串口号

                    if (_subStaionPoint.Kh == 1) //从对巡检
                    {
                        CckRouting.Checked = true;
                    }
                    else
                    {
                        CckRouting.Checked = false;
                    }
                }
                CtxbBackTime.Text = _subStaionPoint.K4.ToString();//通讯容错次数

                #region 扩展属性
                if (_subStaionPoint.Bz3 > 0)
                {
                    if ((_subStaionPoint.Bz3 & 0x1) == 0x1)
                    {
                        CchkWindBreak.Checked = true;
                    }
                    if ((_subStaionPoint.Bz3 & 0x2) == 0x2)
                    {
                        CchkLogicBreak.Checked = true;
                    }
                    if ((_subStaionPoint.Bz3 & 0x4) == 0x4)
                    {
                        CchkHitchBreak.Checked = true;
                    }
                    if ((_subStaionPoint.Bz3 & 0x8) == 0x8)
                    {
                        CchkPowerPack.Checked = true;
                    }

                    CtxbControlBytes.Text = _subStaionPoint.Bz10;
                    CtxbControlBytesNew.Text = _subStaionPoint.Bz11;
                    CtxbControlConditon.Text = _subStaionPoint.Bz9;
                }
                #endregion

                #region 加载抽放修正值
                string[] str = _subStaionPoint.Bz6.Split('|');//20190616
                if (str.Length == 16)
                {
                    hourBh.Text = str[0];
                    hourBc.Text = str[1];
                    hourGh.Text = str[2];
                    hourGc.Text = str[3];
                    dayBh.Text = str[4];
                    dayBc.Text = str[5];
                    dayGh.Text = str[6];
                    dayGc.Text = str[7];
                    monthBh.Text = str[8];
                    monthBc.Text = str[9];
                    monthGh.Text = str[10];
                    monthGc.Text = str[11];
                    yearBh.Text = str[12];
                    yearBc.Text = str[13];
                    yearGh.Text = str[14];
                    yearGc.Text = str[15];
                }
                #endregion
                CtxbRemark.Text = _subStaionPoint.Remark;

                //加载测点的位置信息  20170829
                if (!string.IsNullOrEmpty(_subStaionPoint.XCoordinate) && !string.IsNullOrEmpty(_subStaionPoint.YCoordinate))
                {
                    txt_Coordinate.Text = _subStaionPoint.XCoordinate + "," + _subStaionPoint.YCoordinate;
                    PointAreaId = _subStaionPoint.Areaid;
                }
                else
                {
                    txt_Coordinate.Text = "";
                    PointAreaId = "";
                }

                //CcmbSerialPortNum.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 搜索分站IP
        /// </summary>
        private void SearchIP()
        {
            MACServiceModel.SearchALLIPCache(1);
        }
        /// <summary>
        /// 搜索交换机IP
        /// </summary>
        private void SearchIP1()
        {
            MACServiceModel.SearchALLIPCache(0);
        }
        /// <summary>
        /// 删除分站信息
        /// </summary>
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
        /// 替换队列中的0
        /// </summary>
        /// <param name="MACBZ1"></param>
        /// <param name="fzh"></param>
        /// <returns></returns>
        private string AddStationInMACBZ1(string MACBZ1, int fzh, int COMNum = 0)
        {
            string ret = "";

            string retBZ1 = "";
            bool bAddStation = false;
            if (!string.IsNullOrEmpty(MACBZ1))
            {


                retBZ1 = MACBZ1;
                string[] Stations = MACBZ1.Split('|');
                if (Stations.Length > 0)
                {
                    for (int i = 0; i < Stations.Length; i++)
                    {
                        if (Stations[i] == fzh.ToString())
                        {
                            Stations[i] = "0";
                        }
                        if (!bAddStation)// 20170415
                        {
                            if (Stations[i] == "0")
                            {
                                Stations[i] = fzh.ToString();
                                bAddStation = true;
                            }
                        }
                    }
                    if (bAddStation)
                    {
                        retBZ1 = "";
                        for (int i = 0; i < Stations.Length; i++)
                        {
                            retBZ1 += Stations[i] + "|";
                        }
                        if (!string.IsNullOrEmpty(retBZ1))
                        {
                            if (retBZ1.Contains('|'))
                            {
                                if (retBZ1.LastIndexOf('|') == retBZ1.Length - 1)
                                {
                                    retBZ1 = retBZ1.Substring(0, retBZ1.Length - 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        retBZ1 += "|" + fzh;
                    }
                }
            }
            ret = retBZ1;
            return ret;
        }
        #endregion
        /// <summary>
        /// 新风电闭锁配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnWindBreakNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (_subStaionPoint == null)
                {
                    XtraMessageBox.Show("请先定义并保存分站信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_subStaionPoint.Devid != "244")
                {
                    XtraMessageBox.Show("只有KJ306-F(16)H本安型分站才支持新风电闭锁！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                StationWindPowerLock tempWind = new StationWindPowerLock(_subStaionPoint, this, CtxbControlBytesNew.Text, this.CtxbControlConditon.Text);
                tempWind.ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 坐标拾取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PointCoordinatePickUp coordinateGraphDrawing = new PointCoordinatePickUp(txt_Coordinate.Text);
            coordinateGraphDrawing.ShowDialog();
            if (coordinateGraphDrawing.DialogResult == DialogResult.OK)
            {
                txt_Coordinate.Text = coordinateGraphDrawing.Jsonstr;
                PointAreaId = coordinateGraphDrawing.AreaIdNow;
                PointInGraphId = coordinateGraphDrawing.GraphIdNow;
            }
        }


        private void barButtonISave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Stationverify())
                {
                    return;
                }
                Jc_DefInfo temp = new Jc_DefInfo();
                //重新从原来缓存中，将原来测点的数据加载过来 
                //根据分站号和设备性质去查询分站  20170331                
                PointDefineGetByStationIDRequest PointDefineRequest = new PointDefineGetByStationIDRequest();
                PointDefineRequest.StationID = Convert.ToInt16(CcmbStationSourceNum.Text);
                List<Jc_DefInfo> tempStationList = _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest).Data.FindAll(a => 0 == a.DevPropertyID);
                if (tempStationList.Count > 0)
                {
                    temp = tempStationList[0];
                }
                if (temp == null)
                {
                    temp = new Jc_DefInfo();
                }

                //判断手动输入的情况
                string tempIP = CcmbIpModule.Text.Substring(0, CcmbIpModule.Text.IndexOf('-'));
                string tempMac = CcmbIpModule.Text.Substring(CcmbIpModule.Text.IndexOf('-') + 1);
                Jc_MacInfo ExistIPModule = Model.MACServiceModel.QueryMACByCode(tempMac);
                if (ExistIPModule == null)
                {
                    Jc_MacInfo tempMacInfo = new Jc_MacInfo();
                    tempMacInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    tempMacInfo.MAC = tempMac;//MAC
                    tempMacInfo.IP = tempIP;//IP
                    tempMacInfo.Wz = null;//所属交换机
                    tempMacInfo.Wzid = "-1";
                    tempMacInfo.Istmcs = 0;
                    tempMacInfo.Type = 0;
                    tempMacInfo.Bz3 = "0";
                    tempMacInfo.Bz2 = "";
                    tempMacInfo.Bz4 = "0";
                    tempMacInfo.Bz5 = "1";
                    tempMacInfo.Bz1 = "";
                    tempMacInfo.Upflag = "0";
                    tempMacInfo.InfoState = InfoState.AddNew;
                    Model.MACServiceModel.AddMACCache(tempMacInfo);
                }
                else//如果找到mac对象，则将IP更新一下
                {
                    ExistIPModule.IP = tempIP;
                    ExistIPModule.InfoState = InfoState.Modified;
                    Model.MACServiceModel.UpdateMACCache(ExistIPModule);
                }

                Jc_WzInfo tempWz = null;
                #region 先处理安装位置
                tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CgleStaionAdress.Text);
                if (null == tempWz)
                {
                    tempWz = new Jc_WzInfo();
                    tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID
                    //tempWz.WZID = Convert.ToInt64(this.CgleStaionAdress.EditValue); //WZID xuzp20151109
                    tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
                    tempWz.Wz = this.CgleStaionAdress.Text; //wz
                    tempWz.CreateTime = DateTime.Now;// 20170331
                    tempWz.InfoState = InfoState.AddNew;
                    try
                    {
                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz))
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
                temp.Fzh = Convert.ToInt16(CcmbStationSourceNum.Text);
                temp.Kh = 0;
                if (this.CcmbCommType.Text == "串口")
                {
                    temp.Kh = 1;//分站主队巡检标记
                }
                temp.Dzh = 0;//地址号
                temp.DevPropertyID = 0;
                temp.DevProperty = "分站";
                temp.Devid = CcmbStationType.Properties.Items[CcmbStationType.SelectedIndex].Value.ToString();  //设备类型ID
                temp.DevName = CcmbStationType.Properties.Items[CcmbStationType.SelectedIndex].Description.ToString();//设备类型
                temp.Jckz3 = jckz3;
                temp.DevModelID = 0;//设备型号ID 
                temp.DevModel = "";//设备型号名称 
                Dictionary<int, EnumcodeInfo> tempDevClass = Model.DEVServiceModel.QueryDevClasiessCache();//设备种类
                Dictionary<int, EnumcodeInfo> tempDevModel = Model.DEVServiceModel.QueryDevMoelsCache();//设备型号
                Jc_DevInfo tempDev1 = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
                if (null != tempDev1)
                {
                    if (null != tempDevModel)
                    {
                        if (tempDevModel.ContainsKey(tempDev1.Bz4))
                        {
                            temp.DevModelID = tempDev1.Bz4;//设备型号ID 
                            temp.DevModel = tempDevModel[tempDev1.Bz4].StrEnumDisplay;//设备型号名称 
                        }
                    }
                }

                temp.Wzid = tempWz.WzID;//wzID
                temp.Wz = tempWz.Wz;//wz
                temp.Csid = 0;//分站从队巡检标记
                if (this.CcmbCommType.Text == "串口")
                {
                    if (CckRouting.Checked)
                    {
                        temp.Csid = 1;//分站从队巡检标记
                    }
                }
                temp.Point = CcmbStationSourceNum.Text.PadLeft(3, '0') + "0000";//测点编号
                if (this.CcmbCommType.Text == "网口")
                {
                    if (!string.IsNullOrEmpty(CcmbIpModule.Text))
                    {
                        temp.Jckz1 = CcmbIpModule.Text.Split('-')[1];//MAC 地址
                        temp.Jckz2 = CcmbIpModule.Text.Split('-')[0];//IP 地址
                    }
                }
                temp.K1 = 101;//大气压（101）
                temp.K2 = 1; //抽放正负压（0负1正）
                if (this.CcmbCommType.Text == "串口")
                {
                    if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                    {
                        temp.K3 = Convert.ToInt16(CcmbSerialPortNum.Text.Substring(3));
                    }
                }

                //设置网口 绑定串口信息
                if (this.CcmbCommType.Text == "网口")
                {
                    if (!string.IsNullOrEmpty(CcmbSerialPortNum.Text))
                    {
                        temp.K3 = CcmbSerialPortNum.SelectedIndex + 1;
                    }
                }

                temp.Bz1 = 0x1; //运行记录标志
                temp.Bz2 = 0x1;//响铃报警标志
                if (CcmbStaionDefineState.Text == "运行")
                {
                    temp.Bz4 = 0x0;
                }
                else if (CcmbStaionDefineState.Text == "休眠")
                {
                    temp.Bz4 = 0x2;
                }
                else if (CcmbStaionDefineState.Text == "检修")
                {
                    temp.Bz4 = 0x4;
                }
                temp.Bz3 = 0;//默认值 
                if (CchkWindBreak.Checked || temp.Devid == "316")//风电闭锁标记赋值  20181126
                {
                    temp.Bz3 |= 0x1;
                }
                if (CchkLogicBreak.Checked)
                {
                    temp.Bz3 |= 0x2;
                }
                if (CchkHitchBreak.Checked)
                {
                    temp.Bz3 |= 0x4;
                }
                if (CchkPowerPack.Checked)
                {
                    temp.Bz3 |= 0x8;
                }

                temp.Bz11 = CtxbControlBytesNew.Text;//风电闭锁字节(新)
                temp.Bz10 = CtxbControlBytes.Text;//风电闭锁字节（旧）
                temp.Bz9 = CtxbControlConditon.Text;//风电闭锁条件测点
                temp.K4 = int.Parse(CtxbBackTime.Text);

                temp.Remark = CtxbRemark.Text;
                //存储交换机MAC及IP
                if (!string.IsNullOrEmpty(CcmbIpSwitches.Text))
                {
                    temp.Bz12 = CcmbIpSwitches.Text.Split('-')[1];//MAC 地址
                    temp.Bz13 = CcmbIpSwitches.Text.Split('-')[0];//IP 地址
                }
                //抽放分站修正系数处理
                if (CcmbStationType.Text.Contains("抽放") || CcmbStationType.Text.ToUpper().Contains("GD"))
                {
                    #region 数据校验
                    float tempFloat = 0;
                    if (!float.TryParse(hourBh.Text, out tempFloat))//20190616
                    {
                        hourBh.Text = "0";
                    }
                    if (!float.TryParse(hourBc.Text, out tempFloat))
                    {
                        hourBc.Text = "0";
                    }
                    if (!float.TryParse(hourGh.Text, out tempFloat))
                    {
                        hourGh.Text = "0";
                    }
                    if (!float.TryParse(hourGc.Text, out tempFloat))
                    {
                        hourGc.Text = "0";
                    }

                    if (!float.TryParse(dayBh.Text, out tempFloat))
                    {
                        dayBh.Text = "0";
                    }
                    if (!float.TryParse(dayBc.Text, out tempFloat))
                    {
                        dayBc.Text = "0";
                    }
                    if (!float.TryParse(dayGh.Text, out tempFloat))
                    {
                        dayGh.Text = "0";
                    }
                    if (!float.TryParse(dayGc.Text, out tempFloat))
                    {
                        dayGc.Text = "0";
                    }

                    if (!float.TryParse(monthBh.Text, out tempFloat))
                    {
                        monthBh.Text = "0";
                    }
                    if (!float.TryParse(monthBc.Text, out tempFloat))
                    {
                        monthBc.Text = "0";
                    }
                    if (!float.TryParse(monthGh.Text, out tempFloat))
                    {
                        monthGh.Text = "0";
                    }
                    if (!float.TryParse(monthGc.Text, out tempFloat))
                    {
                        monthGc.Text = "0";
                    }

                    if (!float.TryParse(yearBh.Text, out tempFloat))
                    {
                        yearBh.Text = "0";
                    }
                    if (!float.TryParse(yearBc.Text, out tempFloat))
                    {
                        yearBc.Text = "0";
                    }
                    if (!float.TryParse(yearGh.Text, out tempFloat))
                    {
                        yearGh.Text = "0";
                    }
                    if (!float.TryParse(yearGc.Text, out tempFloat))
                    {
                        yearGc.Text = "0";
                    }
                    #endregion
                    temp.Bz6 = hourBh.Text + "|" + hourBc.Text + "|" + hourGh.Text + "|" + hourGc.Text + "|" + dayBh.Text + "|" + dayBc.Text + "|" + dayGh.Text + "|" + dayGc.Text + "|" +
                        monthBh.Text + "|" + monthBc.Text + "|" + monthGh.Text + "|" + monthGc.Text + "|" +
                        yearBh.Text + "|" + yearBc.Text + "|" + yearGh.Text + "|" + yearGc.Text;//20190616
                }


                #region//增加坐标信息及所属区域信息保存  20170829
                if (!string.IsNullOrEmpty(txt_Coordinate.Text))
                {
                    string coordinateX = txt_Coordinate.Text.Split(',')[0];
                    string coordinateY = txt_Coordinate.Text.Split(',')[1];
                    temp.XCoordinate = coordinateX;
                    temp.YCoordinate = coordinateY;
                    if (string.IsNullOrEmpty(PointAreaId))
                    {
                        temp.Areaid = null;
                    }
                    else
                    {
                        temp.Areaid = PointAreaId;
                    }
                    //将图形信息添加到图形测点表中
                    if (!string.IsNullOrEmpty(PointInGraphId))
                    {
                        GraphicspointsinfInfo graphpointInfo = new GraphicspointsinfInfo();
                        graphpointInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        graphpointInfo.GraphId = PointInGraphId;
                        graphpointInfo.PointID = temp.PointID;
                        graphpointInfo.Point = temp.Point;
                        graphpointInfo.GraphBindName = "实时显示";
                        graphpointInfo.GraphBindType = 0;
                        graphpointInfo.DisZoomlevel = "1$22";
                        graphpointInfo.XCoordinate = coordinateX;
                        graphpointInfo.YCoordinate = coordinateY;
                        graphpointInfo.Bz1 = "-1";
                        graphpointInfo.Bz2 = "0";
                        graphpointInfo.Bz3 = "0";
                        graphpointInfo.Upflag = "0";

                        GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest = new GetGraphicspointsinfByGraphIdAndPointRequest();
                        graphicspointsinfrequest.PointId = temp.Point;
                        graphicspointsinfrequest.GraphId = PointInGraphId;
                        GraphicspointsinfInfo graphicspointsinfInfo = graphicspointsinfService.GetGraphicspointsinfByGraphIdAndPoint(graphicspointsinfrequest).Data;
                        if (graphicspointsinfInfo != null)
                        {//先删除之前定义的坐标信息
                            GraphicspointsinfDeleteRequest deletegraphicspointsinfrequest = new GraphicspointsinfDeleteRequest();
                            deletegraphicspointsinfrequest.Id = graphicspointsinfInfo.ID;
                            graphicspointsinfService.DeleteGraphicspointsinf(deletegraphicspointsinfrequest);
                        }
                        //将测点信息保存到图形测点信息表中
                        GraphicspointsinfAddRequest addgraphicspointsinfrequest = new GraphicspointsinfAddRequest();
                        addgraphicspointsinfrequest.GraphicspointsinfInfo = graphpointInfo;
                        graphicspointsinfService.AddGraphicspointsinf(addgraphicspointsinfrequest);
                    }
                }
                #endregion

                if (temp == _subStaionPoint)
                {
                    XtraMessageBox.Show("分站定义无变化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!AddDefAndMac(temp, _subStaionPoint, tempWz))
                {
                    XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //20170313  增加保存成功提示
                    XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //加延时  20170721
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
            this.Close();
        }

        private void barButtonDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                List<Jc_DefInfo> DefAll = Model.DEFServiceModel.QueryAllCache().ToList();
                List<Jc_DefInfo> fzAll = DefAll.FindAll(a => a.Fzh == int.Parse(this.CcmbStationSourceNum.Text));
                if (fzAll.Count < 1)
                {
                    XtraMessageBox.Show("当前删除的分站不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(this.CcmbStationSourceNum.Text))
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


                        DeleteSubStatoin(Convert.ToInt32(this.CcmbStationSourceNum.Text));
                    }
                    //加延时  20170721
                    Thread.Sleep(1000);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonCancle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                WaitDialogFormTemp = new Sys.Safety.ClientFramework.View.WaitForm.ShowDialogForm("搜索交换机", "正在搜索交换机,请稍后......");
                UpdateControl task = SearchIP1;
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
                IList<Jc_MacInfo> temp = MACServiceModel.QueryAllIPCache().FindAll(a => a.Upflag == "1");
                if (null != temp)
                {
                    bool selectFlag = false;
                    if (CcmbIpSwitches.SelectedItem != null)
                    {
                        selectFlag = true;
                    }

                    CcmbIpSwitches.Properties.Items.Clear();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        CcmbIpSwitches.Properties.Items.Add(temp[i].IP + "-" + temp[i].MAC);
                    }

                    //20170313  如果原来分站IP为空，重新搜索后新增默认值
                    if (temp.Count > 0 && selectFlag == false)
                    {
                        CcmbIpSwitches.SelectedIndex = 0;
                    }
                }
            }
        }

        private void CcmbIpModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ////如果选择了分站iP，则自动修改分站号
                //string mac = CcmbIpModule.Text.Split('-')[1];
                //Jc_MacInfo macinfo = MACServiceModel.QueryMACByCode(mac);
                //if (macinfo != null)
                //{
                //    if (macinfo.BindStatinNumber > 0)
                //    {
                //        CcmbStationSourceNum.SelectedIndex = macinfo.BindStatinNumber - 1;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string mac = "";
            if (CcmbIpModule.Text.Contains("-"))
            {
                mac = CcmbIpModule.Text.Split('-')[1];
                string fzh = CcmbStationSourceNum.Text;

                StationIPSet setip = new StationIPSet(mac, fzh);
                setip.ShowDialog();

                if (setip.isSerach)
                {
                    CbtnSeatchIP_Click(null, new EventArgs());

                    LoadStationIPList();
                }
            }
            else
            {
                XtraMessageBox.Show("请选择一个分站模块进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}