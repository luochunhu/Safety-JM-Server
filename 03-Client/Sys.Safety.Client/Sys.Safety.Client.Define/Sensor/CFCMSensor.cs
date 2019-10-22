using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;
using System.Threading;
using Sys.Safety.Client.GraphDefine;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicspointsinf;
using Basic.Framework.Service;
using Sys.Safety.Request.PersonCache;
using DevExpress.XtraEditors.Controls;
using Sys.Safety.Request.KJ_Addresstype;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CFCMSensor : XtraForm
    {
        IGraphicspointsinfService graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        IAreaService areaService = ServiceFactory.Create<IAreaService>();
        IKJ_AddresstypeService addresstypeService = ServiceFactory.Create<IKJ_AddresstypeService>();
        IKJ_AddresstyperuleService addresstyperuleService = ServiceFactory.Create<IKJ_AddresstyperuleService>();
        List<AreaInfo> AreaListAll = new List<AreaInfo>();
        List<KJ_AddresstypeInfo> AddresstypeInfoListAll = new List<KJ_AddresstypeInfo>();
        ///// <summary>
        ///// 默认构造函数
        ///// </summary>
        public CFCMSensor()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }
        /// <summary>
        /// 通过外部传入 测点编号
        /// </summary>
        /// <param name="arrPoint"></param>
        /// <param name="Dvtype"></param>
        public CFCMSensor(string arrPoint, string sourcePoint, string Parameter)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            try
            {
                _SourcePoint = sourcePoint;
                if (!string.IsNullOrEmpty(arrPoint))
                {
                    _Point = DEFServiceModel.QueryPointByCodeCache(arrPoint);
                }
                if (!string.IsNullOrEmpty(_SourcePoint))
                {
                    _SourceNum = Convert.ToUInt16(_SourcePoint.Substring(0, 3));
                }

                if (null != _Point)
                {
                    _ChannelNum = (uint)_Point.Kh;
                    _AddressNum = (uint)_Point.Dzh;
                    //赋值当前选择的通道类型  20171023
                    SelectChanelNow = Parameter.Split(',')[0];
                }
                else
                {
                    GetNewPointInf(); //todo 优化此处代码，此处加载时间需要2秒   20170314
                    //赋值当前选择的通道类型  20171023
                    if (!string.IsNullOrEmpty(Parameter))//参数不为空时，才进行赋值  20171123
                    {
                        SelectChanelNow = Parameter.Split(',')[0];
                    }
                    if (!string.IsNullOrEmpty(Parameter) && Parameter.Contains(","))
                    {//自动加载通道号

                        _ChannelNum = uint.Parse(Parameter.Split(',')[1]);
                        _AddressNum = uint.Parse(Parameter.Split(',')[2]);
                        _PropertyNum = int.Parse(Parameter.Split(',')[3]);
                        //判断，如果是自动挂接设备，检测以前是否定义了此传感器，并赋值已定义_Point变量  20171018
                        if (Parameter.Contains("基础通道"))
                        {
                            _Point = DEFServiceModel.QueryPointByStationCache(_SourcePoint).Find(a => a.Kh == _ChannelNum && a.Dzh == _AddressNum && (a.DevPropertyID == 1 || a.DevPropertyID == 2));
                        }
                        _DevId = long.Parse(Parameter.Split(',')[4]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
            InitializeComponent();
        }

        #region =========================变量定义===========================
        /// <summary>
        /// 分站测点
        /// </summary>
        private string _SourcePoint = "";
        /// <summary>
        /// 设备性质
        /// </summary>
        private EnumcodeInfo _Dvtype;
        /// <summary>
        /// 测点
        /// </summary>
        private Jc_DefInfo _Point;
        /// <summary>
        /// 分站号
        /// </summary>
        private uint _SourceNum = 0;
        /// <summary>
        /// 通道编号
        /// </summary>
        private uint _ChannelNum = 0;
        /// <summary>
        /// 地址编号
        /// </summary>
        private uint _AddressNum = 0;
        /// <summary>
        /// 设备性质_ID
        /// </summary>
        private int _PropertyNum = -1;
        /// <summary>
        /// 首次加载标记
        /// </summary>
        private bool bFirstLoad = true;
        /// <summary>
        /// 集合改变标记
        /// </summary>
        //private bool bChangeDevType = true;
        /// <summary>
        /// 是否需要充布局标记 改变窗口大小
        /// </summary>
        private bool bReLayout = true;
        /// <summary>
        /// 控制原始信息
        /// </summary>
        private string OrigionDevText;
        /// <summary>
        /// 当前选择的通道,用来做通道定义判断   20170318
        /// </summary>
        private string SelectChanelNow = "";
        /// <summary>
        /// 是否是自动设置的设备性质
        /// </summary>
        private bool isAutoSelCcmbDvProPerty = false;
        /// <summary>
        /// 是否是智能分站
        /// </summary>
        private bool IsZLFz = false;
        /// <summary>
        /// 设备类型ID
        /// </summary>
        private long _DevId;
        /// <summary>
        /// 当前测点所在区域ID
        /// </summary>
        private string PointAreaId = "";
        /// <summary>
        /// 测点所在的地图ID
        /// </summary>
        private string PointInGraphId = "";
        #endregion

        #region =========================事件相关===========================
        /// <summary>
        /// 在函数中加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CfCommonSensor_Load(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                //todo 优化此处代码，此处加载时间需要3秒   20170314

                LoadPretermitInf(); //加载默认属性
                InitChannleTreeData();
                LoadBasicInf(); //加载具体信息
                bFirstLoad = false;

                sw.Stop();
                LogHelper.Info(" CfCommonSensor_Load(): " + sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LogHelper.Error("分站通用部分界面Load【CfCommonSensor_Load】", ex);
            }
        }
        /// <summary> 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            ////重新加载原来测点的信息
            ////if (!string.IsNullOrEmpty(_SourceNum.ToString()))
            ////{
            ////    _Point = DEFServiceModel.QueryPointByChannelInfs((Int16)_SourceNum, Convert.ToInt16(CcmbChannelNum.Text), Convert.ToInt16(CcmbAddressNum.Text),
            ////        Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
            ////}

            //long TempDevid = 0;
            //try
            //{
            //    //判断当前电脑是否为主控  2070504
            //    if (!CONFIGServiceModel.GetClinetDefineState())
            //    {
            //        XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    //保存新测点
            //    if (!Sensorverify())
            //    {
            //        return;
            //    }

            //    #region//判断不能定义智能断电器和多参数传感器  20170401
            //    List<Jc_DefInfo> FzList = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).ToList();
            //    Jc_DefInfo FzInfo = FzList.Find(a => a.DevPropertyID == 0);
            //    Jc_DevInfo FzDev = new Jc_DevInfo();
            //    if (FzInfo != null)
            //    {

            //        if (FzInfo != null)
            //        {
            //            FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == FzInfo.Devid);
            //        }
            //        if (FzDev.LC2 == 13 || FzDev.LC2 == 12)//智能分站 
            //        {
            //            if (SelectChanelNow.Contains("基础通道"))
            //            {
            //                if (_ChannelNum > 0 && _ChannelNum <= 8)
            //                {
            //                    if (CcmbDvType.Text.Contains("智能断电器"))
            //                    {
            //                        XtraMessageBox.Show("基础通道1~8号口，不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                        CcmbDvType.SelectedIndex = 0;
            //                        return;
            //                    }
            //                    // 20170504
            //                    if (CcmbDvProPerty.Text.Contains("控制量"))
            //                    {
            //                        XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                        CcmbDvType.SelectedIndex = 0;
            //                        return;
            //                    }
            //                    //判断多参数中存在控制量的情况  20170505
            //                    int SelDevid = Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString());
            //                    Jc_DevInfo KhDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == SelDevid.ToString());
            //                    if (KhDev != null)
            //                    {
            //                        string[] SonDev = KhDev.Bz9.Split('|');
            //                        foreach (string Dev in SonDev)
            //                        {
            //                            if (!string.IsNullOrEmpty(Dev))
            //                            {
            //                                Jc_DevInfo SonDevDTO = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == Dev);
            //                                if (SonDevDTO != null)
            //                                {
            //                                    if (SonDevDTO.Type == 3)
            //                                    { //控制量
            //                                        XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                                        CcmbDvType.SelectedIndex = 0;
            //                                        return;
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                //基础通道只能定义智能断电器的控制量参数  20180309
            //                if (!CcmbDvType.Text.Contains("智能断电器") && !CcmbDvType.Text.Contains("声光报警") && CcmbDvProPerty.Text.Contains("控制量"))
            //                {
            //                    XtraMessageBox.Show("基础通道控制量只能定义智能断电器或者声光报警器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    return;
            //                }

            //            }

            //            if (SelectChanelNow.Contains("智能通道"))//智能通道，只能定义普通开关量，不能定义智能断电器  20170404
            //            {
            //                if (CcmbDvType.Text.Contains("智能断电器"))
            //                {
            //                    XtraMessageBox.Show("智能通道不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    CcmbDvType.SelectedIndex = 0;
            //                    return;
            //                }
            //                if (MultipleCount.SelectedIndex > 0)//智能通道不能定义多参数
            //                {
            //                    XtraMessageBox.Show("智能通道不能定义多参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    MultipleCount.SelectedIndex = 0;
            //                    return;
            //                }
            //            }
            //            //控制通道不能定义智能断电器  20170401
            //            if (SelectChanelNow.Contains("控制通道"))
            //            {
            //                if (CcmbDvType.Text.Contains("智能断电器"))
            //                {
            //                    XtraMessageBox.Show("控制通道不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    CcmbDvType.SelectedIndex = 0;
            //                    return;
            //                }
            //                //不能定义多参数传感器  20170401
            //                int TempInt = 0;
            //                int.TryParse(CcmbAddressNum.Text, out TempInt);
            //                if (TempInt > 0)
            //                {
            //                    XtraMessageBox.Show("控制通道不能定义多参数传感器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    CcmbDvType.SelectedIndex = 0;
            //                    return;
            //                }
            //            }

            //        }
            //        else
            //        {
            //            if (CcmbDvType.Text.Contains("智能断电器"))
            //            {
            //                XtraMessageBox.Show("非智能分站，不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                CcmbDvType.SelectedIndex = 0;
            //                return;
            //            }
            //            //非智能分站，不能定义多参数传感器  20170401
            //            int TempInt = 0;
            //            int.TryParse(CcmbAddressNum.Text, out TempInt);
            //            if (TempInt > 0)
            //            {
            //                XtraMessageBox.Show("非智能分站，不能定义多参数传感器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                CcmbDvType.SelectedIndex = 0;
            //                return;
            //            }
            //        }
            //    }
            //    #endregion

            //    #region 先处理安装位置
            //    Jc_WzInfo tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CglePointName.Text);
            //    if (null == tempWz)
            //    {
            //        tempWz = new Jc_WzInfo();
            //        tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
            //        tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
            //        tempWz.Wz = this.CglePointName.Text; //wz
            //        tempWz.CreateTime = DateTime.Now;// 20170331
            //        tempWz.InfoState = InfoState.AddNew;
            //        try
            //        {
            //            if (!Model.WZServiceModel.AddJC_WZCache(tempWz))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
            //            {
            //                XtraMessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                return;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //    }
            //    #endregion
            //    Jc_DefInfo temp = new Jc_DefInfo();
            //    //重新从原来缓存中，将原来测点的数据加载过来 
            //    temp = DEFServiceModel.QueryPointByChannelInfs((Int16)_SourceNum, Convert.ToInt16(CcmbChannelNum.Text), Convert.ToInt16(CcmbAddressNum.Text),
            //        Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
            //    if (temp == null)
            //    {
            //        temp = new Jc_DefInfo();
            //    }
            //    temp.Fzh = (Int16)_SourceNum; //分站号
            //    temp.Kh = Convert.ToInt16(CcmbChannelNum.Text);//通道号
            //    temp.Dzh = Convert.ToInt16(CcmbAddressNum.Text);//地址号

            //    temp.Bz12 = MultipleCount.Text;//赋值参数个数  20170415
            //    //赋值传感器通信类型  20170612
            //    if (SensorCommunicationType.Text == "智能型")
            //    {
            //        temp.Bz18 = "0";
            //    }
            //    else
            //    {
            //        temp.Bz18 = "1";
            //    }

            //    temp.DevPropertyID = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.')));//设备性质ID
            //    temp.DevProperty = CcmbDvProPerty.Text.Substring(CcmbDvProPerty.Text.IndexOf('.') + 1);//设备性质种类
            //    temp.DevClassID = 0;//设备种类ID 
            //    temp.DevClass = "";//设备种类名称 
            //    temp.Devid = Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()).ToString();//设备类型ID
            //    temp.DevName = CcmbDvType.Text.Substring(CcmbDvType.Text.IndexOf('.') + 1);//设备类型名称
            //    temp.DevModelID = 0;//设备型号ID 
            //    temp.DevModel = "";//设备型号名称 
            //    Dictionary<int, EnumcodeInfo> tempDevClass = Model.DEVServiceModel.QueryDevClasiessCache();//设备种类
            //    Dictionary<int, EnumcodeInfo> tempDevModel = Model.DEVServiceModel.QueryDevMoelsCache();//设备型号
            //    Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
            //    if (null != tempDev)
            //    {
            //        if (null != tempDevClass)
            //        {
            //            if (tempDevClass.ContainsKey(tempDev.Bz3))
            //            {
            //                temp.DevClassID = tempDev.Bz3;//设备种类ID 
            //                temp.DevClass = tempDevClass[tempDev.Bz3].StrEnumDisplay;//设备种类名称 
            //            }
            //        }
            //        if (null != tempDevModel)
            //        {
            //            if (tempDevModel.ContainsKey(tempDev.Bz4))
            //            {
            //                temp.DevModelID = tempDev.Bz4;//设备型号ID 
            //                temp.DevModel = tempDevModel[tempDev.Bz4].StrEnumDisplay;//设备型号名称 
            //            }
            //        }
            //    }
            //    temp.Wzid = tempWz.WzID;//位置ID
            //    temp.Wz = tempWz.Wz;//位置ID
            //    temp.Csid = 0; //措施ID
            //    temp.Point = CtxbArrPoint.Text;//测点名称（别名）
            //    temp.Bz1 = 1;//运行记录标记 默认都写成1
            //    temp.Bz2 = 1;//语音报警标记 默认都写成1
            //    temp.Bz3 = 1;//突出预测标记 默认都写成1
            //    temp.Bz4 = 0x01;//定义状态标记 默认密采勾选
            //    if (CcmbDefineState.Text == "运行")
            //    {
            //        temp.Bz4 |= 0x00;
            //    }
            //    else if (CcmbDefineState.Text == "休眠")
            //    {
            //        temp.Bz4 |= 0x02;
            //    }
            //    else if (CcmbDefineState.Text == "检修")
            //    {
            //        temp.Bz4 |= 0x04;
            //    }
            //    else if (CcmbDefineState.Text == "标校")
            //    {
            //        temp.Bz4 |= 0x08;
            //    }
            //    if (CpDocument.Controls.Count > 0)
            //    {
            //        bool resultCpCocument = ((CuBase)(CpDocument.Controls[0])).ConfirmInf(temp); //传入传感器对象 赋值属性集  20170622
            //        //返回false 表示数据验证不合法  20170622
            //        if (!resultCpCocument)
            //        {
            //            return;
            //        }
            //    }
            //    if (temp.Dzh > 0)
            //    {
            //        CcmbDvProPerty.Properties.ReadOnly = true;
            //        CcmbDvType.Properties.ReadOnly = true;
            //    }
            //    else
            //    {
            //        CcmbDvProPerty.Properties.ReadOnly = false;
            //        CcmbDvType.Properties.ReadOnly = false;
            //    }
            //    temp.State = 46;//对于变化传感器增加默认状态
            //    temp.DataState = 46;//对于变化传感器增加默认状态

            //    #region//增加传感器坐标信息及所属区域信息保存  20170829
            //    if (!string.IsNullOrEmpty(txt_Coordinate.Text))
            //    {
            //        string coordinateX = txt_Coordinate.Text.Split(',')[0];
            //        string coordinateY = txt_Coordinate.Text.Split(',')[1];
            //        temp.XCoordinate = coordinateX;
            //        temp.YCoordinate = coordinateY;
            //        if (string.IsNullOrEmpty(PointAreaId))
            //        {
            //            temp.Areaid = null;
            //        }
            //        else
            //        {
            //            temp.Areaid = PointAreaId;
            //        }
            //        if (!string.IsNullOrEmpty(PointInGraphId))
            //        {
            //            //将图形信息添加到图形测点表中
            //            GraphicspointsinfInfo graphpointInfo = new GraphicspointsinfInfo();
            //            graphpointInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
            //            graphpointInfo.GraphId = PointInGraphId;
            //            graphpointInfo.PointID = temp.PointID;
            //            graphpointInfo.Point = temp.Point;
            //            //判断，如果是识别器，则保存识别器的图元
            //            if (temp.DevPropertyID == 7)
            //            {
            //                graphpointInfo.GraphBindName = "识别器实时显示";
            //            }
            //            else
            //            {
            //                graphpointInfo.GraphBindName = "实时显示";
            //            }
            //            graphpointInfo.GraphBindType = 0;
            //            graphpointInfo.DisZoomlevel = "1$22";
            //            graphpointInfo.XCoordinate = coordinateX;
            //            graphpointInfo.YCoordinate = coordinateY;
            //            graphpointInfo.Bz1 = "-1";
            //            graphpointInfo.Bz2 = "0";
            //            graphpointInfo.Bz3 = "0";
            //            graphpointInfo.Upflag = "0";

            //            GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest = new GetGraphicspointsinfByGraphIdAndPointRequest();
            //            graphicspointsinfrequest.PointId = temp.Point;
            //            graphicspointsinfrequest.GraphId = PointInGraphId;
            //            GraphicspointsinfInfo graphicspointsinfInfo = graphicspointsinfService.GetGraphicspointsinfByGraphIdAndPoint(graphicspointsinfrequest).Data;
            //            if (graphicspointsinfInfo != null)
            //            {//先删除之前定义的坐标信息
            //                GraphicspointsinfDeleteRequest deletegraphicspointsinfrequest = new GraphicspointsinfDeleteRequest();
            //                deletegraphicspointsinfrequest.Id = graphicspointsinfInfo.ID;
            //                graphicspointsinfService.DeleteGraphicspointsinf(deletegraphicspointsinfrequest);
            //            }
            //            //将测点信息保存到图形测点信息表中
            //            GraphicspointsinfAddRequest addgraphicspointsinfrequest = new GraphicspointsinfAddRequest();
            //            addgraphicspointsinfrequest.GraphicspointsinfInfo = graphpointInfo;
            //            graphicspointsinfService.AddGraphicspointsinf(addgraphicspointsinfrequest);
            //        }
            //    }
            //    #endregion

            //    //判断T0~T8门限定义异常  20180309
            //    if ((temp.Wz.ToLower().Contains("t1") || temp.Wz.ToLower().Contains("t0") || temp.Wz.ToLower().Contains("t5")) && (temp.Z2 > 1.0 || temp.Z3 > 1.5))
            //    {
            //        XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    if ((temp.Wz.ToLower().Contains("t2") || temp.Wz.ToLower().Contains("t6") || temp.Wz.ToLower().Contains("t8")) && (temp.Z2 > 1.0 || temp.Z3 > 1.0))
            //    {
            //        XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    if ((temp.Wz.ToLower().Contains("t3") || temp.Wz.ToLower().Contains("t4")) && (temp.Z2 > 0.5 || temp.Z3 > 0.5))
            //    {
            //        XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    if ((temp.Wz.ToLower().Contains("t7")) && (temp.Z2 > 2.5 || temp.Z3 > 2.5))
            //    {
            //        XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    if (null == _Point) //表示新增
            //    {
            //        //除控制量多参数外其它多参数传感器只能定义4个  20170503                  
            //        int TempDzh = Convert.ToInt16(CcmbAddressNum.Text);
            //        if (TempDzh > 0 && (temp.DevPropertyID == 1 || temp.DevPropertyID == 2))
            //        {
            //            int MultCount = 0;
            //            //计算非控制量多参数  20170622
            //            for (int i = 1; i <= 16; i++)
            //            {
            //                List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.Dzh > 0);
            //                if (MultList.Count > 0)
            //                {
            //                    List<Jc_DefInfo> MultListControl = MultList.FindAll(a => a.DevPropertyID == 3);
            //                    if (MultListControl.Count < 1)
            //                    {
            //                        MultCount++;
            //                    }
            //                }
            //            }
            //            if (MultCount >= 4)
            //            {
            //                XtraMessageBox.Show("除控制量多参数外最多只能定义4个其它多参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                CcmbDvType.SelectedIndex = 0;
            //                return;
            //            }
            //        }
            //        #region//判断控制量最多只能定义8个  20170615
            //        if (temp.DevPropertyID == 3)
            //        {
            //            int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
            //            if (controlPointCount >= 8)
            //            {
            //                XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                CcmbDvType.SelectedIndex = 0;
            //                return;
            //            }
            //        }
            //        //多参数中包含控制量的情况判断
            //        Jc_DevInfo tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid.ToString());
            //        TempDzh = Convert.ToInt16(CcmbAddressNum.Text);
            //        if (TempDzh > 0 && temp.DevPropertyID == 3)
            //        {

            //            if (!string.IsNullOrEmpty(tempDevInfo.Bz9))
            //            {
            //                string[] tempDevIdList = tempDevInfo.Bz9.Split(',');
            //                foreach (string devid in tempDevIdList)
            //                {
            //                    tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(devid);
            //                    if (tempDevInfo.Type == 3)
            //                    { //副通道中包含控制量
            //                        int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
            //                        if (controlPointCount >= 8)
            //                        {
            //                            XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                            CcmbDvType.SelectedIndex = 0;
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        #endregion

            //        #region//基础通道1-16只能定义8个参量的多参数（4参数最多2个，2参最多4个）
            //        int defineNowCount = 0;//当前定义参量数
            //        if (tempDevInfo != null)
            //        {
            //            if (!string.IsNullOrEmpty(tempDevInfo.Bz8) && int.Parse(tempDevInfo.Bz8) > 1)
            //            {
            //                defineNowCount = int.Parse(tempDevInfo.Bz8);
            //            }
            //        }
            //        Jc_DevInfo tempMultDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid.ToString());
            //        int definedParamCount = 0;//已定义参量数
            //        for (int i = 1; i <= 16; i++)
            //        {
            //            List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.DevPropertyID == 1);//只判断模拟量多参数
            //            if (MultList.Count > 1)
            //            {
            //                definedParamCount += MultList.Count;
            //            }
            //        }
            //        if (definedParamCount + defineNowCount > 8)
            //        {
            //            XtraMessageBox.Show("多参数传感器最多只能定义8个参量（2参4个，4参2个）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }
            //        #endregion


            //        //判断测点重复  20170318
            //        Jc_DefInfo tempdef = DEFServiceModel.QueryPointByCodeCache(temp.Point);
            //        if (tempdef != null)
            //        {
            //            XtraMessageBox.Show("已定义了该设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }

            //        temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
            //        temp.PointID = temp.ID;
            //        temp.Activity = "1";
            //        temp.InfoState = InfoState.AddNew;
            //        temp.CreateUpdateTime = DateTime.Now;
            //        temp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");

            //        PersonSpecialHand(temp);//人员定位特殊处理  20171128

            //        //加入馈电开停  20170621
            //        AddFeedDrive(temp);
            //        //添加测点
            //        try
            //        {
            //            if (!DEFServiceModel.AddDEFCache(temp))
            //            {
            //                XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                return;
            //            }
            //            else
            //            {
            //                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //                _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //        //写操作日志 
            //        OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(_Point), "");// 20160111
            //        TempDevid = long.Parse(temp.Devid);

            //        relateAdd(TempDevid, temp);//关联添加   //多参数通道由用户自定义  20170415
            //    }
            //    else  //表示更新
            //    {
            //        if (temp != _Point)
            //        {
            //            if (temp.Devid != _Point.Devid || temp.Bz12 != _Point.Bz12)//设备类型不同时为新增 之前点在数据库置非活动点
            //            {
            //                try
            //                {
            //                    //_Point.Activity = "0";
            //                    //_Point.DeleteTime = DateTime.Now;
            //                    //_Point.InfoState = InfoState.Modified;
            //                    //DEFServiceModel.UpdateDEFCache(_Point);
            //                    //如果当前测点已经复制，则清除  20170503
            //                    if (Model.CopyInf.CopySensor != null)
            //                    {
            //                        if (Model.CopyInf.CopySensor.Fzh == _Point.Fzh && Model.CopyInf.CopySensor.Kh == _Point.Kh)
            //                        {
            //                            Model.CopyInf.CopySensor = null;
            //                        }
            //                    }
            //                    DeletePoint(_Point);



            //                    temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
            //                    temp.PointID = temp.ID;
            //                    temp.CreateUpdateTime = DateTime.Now;
            //                    temp.Activity = "1";
            //                    temp.InfoState = InfoState.AddNew;
            //                    temp.CreateUpdateTime = DateTime.Now;
            //                    temp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");

            //                    //加入馈电开停  20170621
            //                    AddFeedDrive(temp);

            //                    DEFServiceModel.AddDEFCache(temp);

            //                    OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20160111

            //                    TempDevid = long.Parse(temp.Devid);
            //                    relateAdd(TempDevid, temp);//关联添加   //多参数通道由用户自定义  20170415

            //                    XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //                    _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627

            //                }
            //                catch (Exception ex)
            //                {
            //                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                    return;
            //                }

            //                //OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(_Point), "");// 20160111
            //                //OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20160111
            //            }
            //            else
            //            {
            //                temp.ID = _Point.ID;
            //                temp.PointID = _Point.PointID;
            //                temp.State = _Point.State;//对于变化传感器赋值成原来的设备状态
            //                temp.DataState = _Point.DataState;//对于变化传感器赋值成原来的数据状态
            //                temp.Ssz = _Point.Ssz; ;//对于变化传感器赋值成原来的实时值
            //                temp.CreateUpdateTime = _Point.CreateUpdateTime;
            //                temp.Activity = "1";



            //                OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.UpdatePointLog(_Point, temp), "");// 20160111

            //                temp.InfoState = InfoState.Modified;
            //                try
            //                {
            //                    if (!DEFServiceModel.UpdateDEFCache(temp))
            //                    {
            //                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                        //保存成功时，才更新内存对象  20170627
            //                        _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            XtraMessageBox.Show("定义无变化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    //加延时  20170721
            //    Thread.Sleep(1000);
            //    InitChannleTreeData();
            //    CTreeListChanne.RefreshDataSource();

            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("确认保存【Cbtn_Confirm_Click】", ex);
            //}
        }
        //增加馈电开停绑定  20170621
        private void AddFeedDrive(Jc_DefInfo temp)
        {
            List<Jc_DevInfo> DevList = Model.DEVServiceModel.QueryDevsCache().ToList();
            Jc_DevInfo tempDev = DevList.Find(a => a.Devid == temp.Devid);
            if (tempDev.Bz10 == "1")//如果当前设备选择了自动绑定馈电  20170621
            {
                //计算副通道中哪个是开关量
                int MultCount = 0;
                int.TryParse(tempDev.Bz8, out MultCount);
                if (MultCount > 1)//如果当前设备是多参数
                {
                    string[] MultChanel = tempDev.Bz9.Split('|');
                    if (MultChanel.Length > 0)
                    {
                        short TempMultDzh = 2;//地址号从2号开始
                        foreach (string Chanel in MultChanel)
                        {
                            if (Chanel.Length > 0)
                            {
                                long ChanelDevid = 0;
                                long.TryParse(Chanel, out ChanelDevid);
                                Jc_DevInfo ChanelDTO = DevList.Find(a => a.Devid == ChanelDevid.ToString());
                                if (ChanelDTO.Type == 2)//加入馈电  20170621
                                {
                                    temp.K1 = (int)_SourceNum;
                                    temp.K2 = (int)_ChannelNum;
                                    temp.K4 = TempMultDzh;//赋为当前传感器的地址号
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary> 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbntDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(this.CtxbArrPoint.Text);
                if (null == temp)
                {
                    return;
                }
                //删除验证  20170621
                if (!DeleteCheck(temp))
                {
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(this.CtxbArrPoint.Text))
                    {

                        //如果当前测点已经复制，则清除  20170503
                        if (Model.CopyInf.CopySensor != null)
                        {
                            if (Model.CopyInf.CopySensor.Fzh == temp.Fzh && Model.CopyInf.CopySensor.Kh == temp.Kh)
                            {
                                Model.CopyInf.CopySensor = null;
                            }
                        }


                        if (DeletePoint(temp))
                        {
                            XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //加延时  20170721
                        Thread.Sleep(1000);
                        LoadBasicInf();
                        InitChannleTreeData();
                        CTreeListChanne.RefreshDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("确认删除【CbntDelete_Click】", ex);
            }
        }
        /// <summary>
        /// 测点删除验证，交叉控制绑定，风电闭锁绑定等
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private bool DeleteCheck(Jc_DefInfo temp)
        {
            bool rvalue = true;
            //如果删除控制，则判断是否绑定了交叉控制，如果绑定了交叉控制，先提示用户解除交叉控制，再删除  20170401
            if (temp.DevPropertyID == 3)
            {
                List<Jc_DefInfo> DefAll = Model.DEFServiceModel.QueryAllCache().ToList();
                if (DefAll != null)
                {
                    List<Jc_DefInfo> tempJCKZ = DefAll.FindAll(a => (a.Jckz1 != null && a.Jckz2 != null && a.Jckz3 != null) &&
                        (a.Jckz1.Contains(temp.Point) || a.Jckz2.Contains(temp.Point) || a.Jckz3.Contains(temp.Point)));
                    if (tempJCKZ != null)
                    {
                        if (tempJCKZ.Count > 0)
                        {
                            XtraMessageBox.Show("当前删除控制量设备绑了交叉控制，请先解除交叉控制，再删除当前控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
                //判断风电闭锁是否绑定
                if (!RelateUpdate.ControlPointLegal(temp))
                {
                    XtraMessageBox.Show("当前删除的设备在甲烷风电闭锁中已使用，请先解除甲烷风电闭锁使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else if (temp.DevPropertyID == 1 || temp.DevPropertyID == 2)
            {
                if (!RelateUpdate.PointLegal(temp))
                {
                    XtraMessageBox.Show("当前删除的设备在风电闭锁中已使用，请先解除风电闭锁使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return rvalue;
        }
        /// <summary>复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnSensorCopy_Click(object sender, EventArgs e)
        {
            //保存新测点
            if (!Sensorverify())
            {
                return;
            }
            if (null == _Point)
            {
                XtraMessageBox.Show("请先保存测点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Model.CopyInf.CopySensor = _Point.Clone();

            Model.CopyInf.CopySensor.Alarm = 0;
            Model.CopyInf.CopySensor.State = 46;
            Model.CopyInf.CopySensor.DataState = 46;
            Model.CopyInf.CopySensor.Voltage = 0;
            Model.CopyInf.CopySensor.Ssz = "";
            Model.CopyInf.CopySensor.Zts = new DateTime(1900, 1, 1, 0, 0, 0);

        }
        /// <summary>粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnSensorPasty_Click(object sender, EventArgs e)
        {
            if (Model.CopyInf.CopySensor == null)
            {
                return;
            }

            //
            List<Jc_DefInfo> JcDefAllCache = Model.DEFServiceModel.QueryAllCache().ToList();
            List<Jc_DefInfo> FzList = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum);
            Jc_DefInfo JcDefFzNow = JcDefAllCache.Find(a => a.Fzh == (int)_SourceNum && a.DevPropertyID == 0);
            Jc_DevInfo FzDev = new Jc_DevInfo();
            if (JcDefFzNow != null)
            {
                FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == JcDefFzNow.Devid);
            }

            Jc_DefInfo _TempDef = Model.CopyInf.CopySensor.Clone();
            if (_TempDef.Kh == 0)
            {
                XtraMessageBox.Show("不能复制分站到通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (FzDev.LC2 == 13 || FzDev.LC2 == 12)//智能分站判断  20170323
            {
                if (_TempDef.Kh < 16 && _ChannelNum > 16)//基础通道不能复制到智能通道上  20170322
                {
                    XtraMessageBox.Show("基础通道不能复制到智能通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.DevPropertyID != 3 && SelectChanelNow.Contains("控制通道"))//非控制通道不能复制到控制通道上  20170322
                {
                    XtraMessageBox.Show("非控制通道不能复制到控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.Kh > 16 && _TempDef.Kh <= 32 && ((_ChannelNum > 0 && _ChannelNum <= 16)))//智能通道不能复制到基础通道或控制通道上  20170322
                {
                    XtraMessageBox.Show("智能通道不能复制到基础通道或控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if ((_TempDef.DevPropertyID == 3 && _TempDef.Dzh == 0 && _TempDef.Devid != "58") && ((_ChannelNum > 0 && _ChannelNum <= 16 && SelectChanelNow.Contains("基础通道"))
                    || (_ChannelNum > 16 && _ChannelNum <= 32 && SelectChanelNow.Contains("智能通道"))))//控制通道不能复制到基础通道或者智能通道上,智能断电器可以复制  20170322
                {//多参数允许复制，单参数不能复制  20170502
                    XtraMessageBox.Show("控制通道不能复制到基础通道或者智能通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (SelectChanelNow.Length == 7 && !SelectChanelNow.Contains("通道"))
                { //不能粘贴到多参数传感器的口上面  20170329
                    XtraMessageBox.Show("不能粘贴到多参数传感器的口上面！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.Dzh > 0 && SelectChanelNow.Contains("控制通道"))
                {//多参数传感器下的通道不能复制到控制通道上
                    XtraMessageBox.Show("多参数传感器下的通道不能复制到控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if ((_TempDef.Devid == "58" || _TempDef.Devid == "60" || _TempDef.Devid == "97" || _TempDef.Devid == "98") && (_ChannelNum > 0 && _ChannelNum <= 8 && SelectChanelNow.Contains("基础通道")))
                {//智能断电器不能复制到基础通道1~4上
                    XtraMessageBox.Show("智能断电器或者声光报警器不能复制到基础通道1~8上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if ((_TempDef.Devid == "58" || _TempDef.Devid == "60" || _TempDef.Devid == "97" || _TempDef.Devid == "98") && SelectChanelNow.Contains("智能通道"))//智能断电器不能复制到智能口上面，智能口只能定义开关量  20170404
                {//智能断电器不能复制到智能通道上
                    XtraMessageBox.Show("智能断电器或者声光报警器不能复制到智能通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //除控制量多参数外其它多参数传感器只能定义4个  20170503
                int TempDzh = Convert.ToInt16(_TempDef.Dzh);
                if (TempDzh > 0 && (_TempDef.DevPropertyID == 1 || _TempDef.DevPropertyID == 2))
                {
                    int MultCount = 0;
                    //计算非控制量多参数  20170622
                    for (int i = 1; i <= 16; i++)
                    {
                        List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.Dzh > 0);
                        if (MultList.Count > 0)
                        {
                            List<Jc_DefInfo> MultListControl = MultList.FindAll(a => a.DevPropertyID == 3);
                            if (MultListControl.Count < 1)
                            {
                                MultCount++;
                            }
                        }
                    }
                    if (MultCount >= 4)
                    {
                        XtraMessageBox.Show("除控制量多参数外最多只能定义4个其它多参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CcmbDvType.SelectedIndex = 0;
                        return;
                    }
                }
                #region//判断控制量最多只能定义8个  20170615
                if (_TempDef.DevPropertyID == 3)
                {
                    int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
                    if (controlPointCount == 8)
                    {
                        XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CcmbDvType.SelectedIndex = 0;
                        return;
                    }
                }
                //多参数中包含控制量的情况判断
                Jc_DevInfo tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(_TempDef.Devid.ToString());
                TempDzh = _TempDef.Dzh;
                if (TempDzh > 0 && _TempDef.DevPropertyID == 3)
                {

                    if (!string.IsNullOrEmpty(tempDevInfo.Bz9))
                    {
                        string[] tempDevIdList = tempDevInfo.Bz9.Split(',');
                        foreach (string devid in tempDevIdList)
                        {
                            tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(devid);
                            if (tempDevInfo.Type == 3)
                            { //副通道中包含控制量
                                int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
                                if (controlPointCount == 8)
                                {
                                    XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CcmbDvType.SelectedIndex = 0;
                                    return;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region//基础通道1-16只能定义8个参量的多参数（4参数最多2个，2参最多4个）
                int defineNowCount = 0;//当前定义参量数
                if (tempDevInfo != null)
                {
                    if (!string.IsNullOrEmpty(tempDevInfo.Bz8) && int.Parse(tempDevInfo.Bz8) > 1)
                    {
                        defineNowCount = int.Parse(tempDevInfo.Bz8);
                    }
                }
                Jc_DevInfo tempMultDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(_TempDef.Devid.ToString());
                int definedParamCount = 0;//已定义参量数
                for (int i = 1; i <= 16; i++)
                {
                    List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.DevPropertyID == 1);
                    if (MultList.Count > 1)
                    {
                        definedParamCount += MultList.Count;
                    }
                }
                if (definedParamCount + defineNowCount > 8)
                {
                    XtraMessageBox.Show("多参数传感器最多只能定义8个参量（2参4个，4参2个）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                //带有控制的多参数传感器不能复制到1~6号通道  20170505
                if (SelectChanelNow.Contains("基础通道"))
                {
                    if (_ChannelNum > 0 && _ChannelNum <= 8)
                    {
                        //判断多参数中存在控制量的情况  20170505                      
                        List<Jc_DefInfo> FzKhPointList = JcDefAllCache.FindAll(a => a.Fzh == _TempDef.Fzh && a.Kh == _TempDef.Kh && a.Dzh != _TempDef.Dzh && a.Dzh > 0);
                        if (FzKhPointList.Count > 0)
                        {
                            for (int i = 0; i < FzKhPointList.Count; i++)
                            {
                                if (FzKhPointList[i].DevPropertyID == 3)
                                { //控制量
                                    XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }
                }

            }
            else
            {//非智能分站判断  20170323
                if (_TempDef.DevPropertyID != 3 && SelectChanelNow.Contains("控制通道"))//非控制通道不能复制到控制通道上  20170322
                {
                    XtraMessageBox.Show("非控制通道不能复制到控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.DevPropertyID == 3 && ((_ChannelNum > 0 && _ChannelNum <= 16 && SelectChanelNow.Contains("基础通道"))))//控制通道不能复制到基础通道上  20170322
                {
                    XtraMessageBox.Show("控制通道不能复制到基础通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.DevPropertyID == 6 && (SelectChanelNow.Contains("基础通道") || SelectChanelNow.Contains("控制通道")))//累计通道不能复制到其它通道上  20170322
                {
                    XtraMessageBox.Show("累计通道不能复制到其它通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.DevPropertyID != 6 && SelectChanelNow.Contains("累计通道"))//非累计通道不能复制到累计通道上  20170322
                {
                    XtraMessageBox.Show("非累计通道不能复制到累计通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (SelectChanelNow.Length == 7 && !SelectChanelNow.Contains("通道"))
                { //不能粘贴到多参数传感器的口上面  20170329
                    XtraMessageBox.Show("不能粘贴到多参数传感器的口上面！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (_TempDef.Dzh > 0 && SelectChanelNow.Contains("控制通道"))
                {//多参数传感器下的通道不能复制到控制通道上
                    XtraMessageBox.Show("多参数传感器下的通道不能复制到控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // 20170401
                if ((_TempDef.Devid == "58" || _TempDef.Devid == "60"))
                {//智能断电器不能复制到基础通道上
                    XtraMessageBox.Show("智能断电器不能复制到非智能分站的通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // 20170401
                if (_TempDef.Dzh > 0)
                {//多参数传感器不能复制到非智能分站通道上
                    XtraMessageBox.Show("多参数传感器不能复制到非智能分站通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (SelectChanelNow.Contains("控制通道") && _TempDef.DevPropertyID != 3)//非控制通道不能复制到控制通道上  20170323
            {
                XtraMessageBox.Show("非控制通道不能复制到控制通道上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (null == _Point)
            {


                _Point = Model.CopyInf.CopySensor.Clone();

                //如果从频率通道复制到智能通道上面，则把Bz18改成智能型  20180416
                if (_Point.Bz18 == "1" && _ChannelNum <= 8)
                {
                    _Point.Bz18 = "0";
                }

                int OldFzh = _Point.Fzh;
                int OldKh = _Point.Kh;
                _Point.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                //20170316 modified by  解决复制粘贴无法保存测点的问题，原因之前的pointId粘贴时未改为新值
                _Point.PointID = _Point.ID;
                _Point.CreateUpdateTime = DateTime.Now;
                _Point.Fzh = (Int16)_SourceNum;
                _Point.Kh = (Int16)_ChannelNum;
                _AddressNum = (uint)_Point.Dzh;
                _Point.Point = CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, _Point.DevPropertyID);
                //加入馈电开停  20170621
                AddFeedDrive(_Point);
                _Point.InfoState = InfoState.AddNew;

                //判断测点重复  20170318
                Jc_DefInfo tempdef = DEFServiceModel.QueryPointByCodeCache(_Point.Point);
                if (tempdef != null)
                {
                    XtraMessageBox.Show("已定义了该设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    //批量添加   20170415
                    List<Jc_DefInfo> AllPointList = new List<Jc_DefInfo>();
                    AllPointList.Add(_Point);
                    //多参数传感器复制时，复制其它测点  20170415
                    if (_Point.Dzh > 0)
                    {
                        List<Jc_DefInfo> FzKhPointList = JcDefAllCache.FindAll(a => a.Fzh == OldFzh && a.Kh == OldKh && a.Dzh != _Point.Dzh && a.Dzh > 0);
                        if (FzKhPointList.Count > 0)
                        {
                            foreach (Jc_DefInfo tempdef1 in FzKhPointList)
                            {
                                Jc_DefInfo NewPoint = tempdef1.Clone();
                                NewPoint.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                //20170316 modified by  解决复制粘贴无法保存测点的问题，原因之前的pointId粘贴时未改为新值
                                NewPoint.PointID = NewPoint.ID;
                                NewPoint.CreateUpdateTime = DateTime.Now;
                                NewPoint.Fzh = (Int16)_SourceNum;
                                NewPoint.Kh = (Int16)_ChannelNum;
                                NewPoint.Point = CreatArrPointTag(_SourceNum, _ChannelNum, (uint)NewPoint.Dzh, NewPoint.DevPropertyID);
                                //加入馈电开停  20170621
                                AddFeedDrive(NewPoint);
                                NewPoint.InfoState = InfoState.AddNew;

                                AllPointList.Add(NewPoint);
                            }
                        }
                    }

                    PersonSpecialHand(_Point);//人员定位特殊处理  20171128

                    if (!Model.DEFServiceModel.AddDEFsCache(AllPointList))
                    {
                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                OperateLogHelper.InsertOperateLog(1, "复制测点【" + _Point.Point + "】", "");

                //relateAdd(_Point.Devid, _Point);//关联添加 //多参数通道由用户自定义  20170415
                //加延时  20170721
                Thread.Sleep(1000);
                LoadBasicInf();
                InitChannleTreeData();
                CTreeListChanne.RefreshDataSource();
            }
            else
            {
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeletePoint(_Point);
                    _Point = Model.CopyInf.CopySensor.Clone();
                    //如果从频率通道复制到智能通道上面，则把Bz18改成智能型  20180416
                    if (_Point.Bz18 == "1" && _ChannelNum <= 8)
                    {
                        _Point.Bz18 = "0";
                    }
                    int OldFzh = _Point.Fzh;
                    int OldKh = _Point.Kh;
                    _Point.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    //20170316 modified by  解决复制粘贴无法保存测点的问题，原因之前的pointId粘贴时未改为新值
                    _Point.PointID = _Point.ID;
                    _Point.Fzh = (Int16)_SourceNum;
                    _Point.Kh = (Int16)_ChannelNum;
                    _AddressNum = (uint)_Point.Dzh;
                    _Point.Point = CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, _Point.DevPropertyID);
                    //加入馈电开停  20170621
                    AddFeedDrive(_Point);
                    _Point.InfoState = InfoState.AddNew;

                    //判断测点重复  20170318
                    Jc_DefInfo tempdef = DEFServiceModel.QueryPointByCodeCache(_Point.Point);
                    if (tempdef != null)
                    {
                        XtraMessageBox.Show("已定义了该设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    try
                    {
                        //批量添加   20170415
                        List<Jc_DefInfo> AllPointList = new List<Jc_DefInfo>();
                        AllPointList.Add(_Point);
                        //多参数传感器复制时，复制其它测点  20170415
                        if (_Point.Dzh > 0)
                        {
                            List<Jc_DefInfo> FzKhPointList = JcDefAllCache.FindAll(a => a.Fzh == OldFzh && a.Kh == OldKh && a.Dzh != _Point.Dzh && a.Dzh > 0);
                            if (FzKhPointList.Count > 0)
                            {
                                foreach (Jc_DefInfo tempdef1 in FzKhPointList)
                                {
                                    Jc_DefInfo NewPoint = tempdef1.Clone();
                                    NewPoint.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                    //20170316 modified by  解决复制粘贴无法保存测点的问题，原因之前的pointId粘贴时未改为新值
                                    NewPoint.PointID = NewPoint.ID;
                                    NewPoint.CreateUpdateTime = DateTime.Now;
                                    NewPoint.Fzh = (Int16)_SourceNum;
                                    NewPoint.Kh = (Int16)_ChannelNum;
                                    NewPoint.Point = CreatArrPointTag(_SourceNum, _ChannelNum, (uint)NewPoint.Dzh, NewPoint.DevPropertyID);
                                    //加入馈电开停  20170621
                                    AddFeedDrive(NewPoint);

                                    NewPoint.InfoState = InfoState.AddNew;

                                    AllPointList.Add(NewPoint);
                                }
                            }
                        }
                        PersonSpecialHand(_Point);//人员定位特殊处理  20171128

                        if (!Model.DEFServiceModel.AddDEFsCache(AllPointList))
                        {
                            XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OperateLogHelper.InsertOperateLog(1, "复制测点【" + _Point.Point + "】", "");

                    //relateAdd(_Point.Devid, _Point);//关联添加 //多参数通道由用户自定义  20170415
                    //加延时  20170721
                    Thread.Sleep(1000);
                    LoadBasicInf();
                    InitChannleTreeData();
                    CTreeListChanne.RefreshDataSource();
                }
            }

        }

        /// <summary>添加设备类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbConfigDevType_Click(object sender, EventArgs e)
        {
            try
            {
                _PropertyNum = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))); /*传到另一个窗体前，设备性质ID 再从下拉列表取当前的选择. 2017-3-01 */
                long devid = Convert.ToInt64(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString());
                Sensor.CFCMSensorType CFCMSensorType = new Sensor.CFCMSensorType(devid.ToString(), _PropertyNum.ToString(), "");
                CFCMSensorType.ShowDialog();
                //重新加载详细信息  20170726
                object sender1 = new object();
                EventArgs e1 = new EventArgs();
                CcmbDvType_SelectedIndexChanged(sender1, e1);
                //IList<Jc_DevInfo> temp = Model.DEVServiceModel.QueryDevByDevpropertIDCache(_PropertyNum);
                //if (null != temp)
                //{
                //    if (temp.Count > 0)
                //    {
                //        temp = temp.OrderBy(item => item.Devid).ToList();
                //        CcmbDvType.Properties.Items.Clear();
                //        for (int i = 0; i < temp.Count; i++)
                //        {
                //            CcmbDvType.Properties.Items.Add(temp[i].Devid + "." + temp[i].Name);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("配置设备类型【CcmbConfigDevType_Click】", ex);
            }
        }
        /// <summary>设备性质改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbDvProPerty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CcmbDvProPerty.Text))
                {
                    return;
                }

                int DevPropertyID = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.')));

                if (DevPropertyID == 5)//判断，如果定义导出量，则提示暂时不支持  20170419
                {
                    XtraMessageBox.Show("系统暂不支持导出量定义！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CcmbDvProPerty.SelectedIndex = 0;
                    return;
                }

                if (DevPropertyID == 3 && SelectChanelNow.Contains("基础通道"))
                { //如果前面的基础通道定义控制量，只能是智能型
                    SensorCommunicationType.Text = "智能型";
                    SensorCommunicationType.Enabled = false;
                }
                else
                {
                    SensorCommunicationType.Enabled = true;
                }

                if (SelectChanelNow.Contains("智能通道"))
                {
                    if (DevPropertyID != 2)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("智能通道只能定义开关量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 1;
                        return;
                    }
                }
                else if (SelectChanelNow.Contains("控制通道"))
                {
                    if (DevPropertyID != 3)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("控制通道只能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 2;
                        return;
                    }
                }
                else if (SelectChanelNow.Contains("基础通道"))
                {

                    List<Jc_DefInfo> JcDefAllCache = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).ToList();

                    Jc_DefInfo JcDefFzNow = JcDefAllCache.Find(a => a.DevPropertyID == 0);
                    Jc_DevInfo FzDev = new Jc_DevInfo();

                    if (JcDefFzNow != null)
                    {
                        FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == JcDefFzNow.Devid);
                    }

                    if (FzDev.LC2 == 13 || FzDev.LC2 == 12)        //智能分站  20170323
                    {
                        if (_ChannelNum > 0 && _ChannelNum <= 8 && DevPropertyID == 3)
                        {
                            if (!isAutoSelCcmbDvProPerty)
                            {
                                XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            CcmbDvProPerty.SelectedIndex = 0;
                            return;
                        }
                    }
                    else
                    { //非智能分站
                        if (_ChannelNum > 0 && _ChannelNum <= 16 && DevPropertyID == 3)
                        {
                            if (!isAutoSelCcmbDvProPerty)
                            {
                                XtraMessageBox.Show("基础通道1~16号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            CcmbDvProPerty.SelectedIndex = 0;
                            return;
                        }
                    }

                    if (DevPropertyID == 4)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("基础通道不能定义累计量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 0;
                        return;
                    }
                    if (DevPropertyID == 7)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("基础通道不能定义智能量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 0;
                        return;
                    }
                }
                else if (SelectChanelNow.Contains("累计通道"))
                {
                    if (DevPropertyID != 4)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("累计通道只能定义累计量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 3;
                        return;
                    }
                }
                // 20171118
                else if (SelectChanelNow.Contains("人员通道"))
                {
                    if (DevPropertyID != 7)
                    {
                        if (!isAutoSelCcmbDvProPerty)
                        {
                            XtraMessageBox.Show("人员通道只能定义识别器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CcmbDvProPerty.SelectedIndex = 6;
                        return;
                    }
                }

                isAutoSelCcmbDvProPerty = false;//置自动选择为false

                //修改设备类型内容
                CcmbDvType.Properties.Items.Clear();
                CcmbDvType.SelectedItem = null;

                List<Jc_DevInfo> temp = Model.DEVServiceModel.QueryDevByDevpropertIDCache(DevPropertyID);
                if (null != temp)
                {
                    if (temp.Count > 0)
                    {
                        temp = temp.OrderBy(a => int.Parse(a.Devid)).ToList();
                        CcmbDvType.Properties.Items.Clear();
                        for (int i = 0; i < temp.Count; i++)
                        {
                            //CcmbDvType.Properties.Items.Add(temp[i].Devid + "." + temp[i].Name);
                            ImageComboBoxItem item = new ImageComboBoxItem();
                            item.Description = temp[i].Name;
                            item.Value = temp[i].Devid;
                            CcmbDvType.Properties.Items.Add(item);
                        }

                    }
                }


                if (_Point != null)
                {
                    if (_Point.DevPropertyID != DevPropertyID)
                    {
                        //风电闭锁设备判断  20170713
                        if (!RelateUpdate.ControlPointLegalAll(_Point))
                        {
                            XtraMessageBox.Show("当设备在风电闭锁中已使用，请先解除风电闭锁使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.Text = _Point.DevName;
                            return;
                        }
                    }
                }

                //改变属性集内容
                //CpDocument.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (CpDocument.Controls.Count > 0)
                {
                    if (CpDocument.Controls[0] != null)
                    {
                        CpDocument.Controls[0].Dispose();
                    }
                }
                CuBase SensroTemp = new CuBase();
                string Arrpoint = "";
                if (_Point != null)
                {
                    Arrpoint = _Point.Point;
                }

                SensroTemp = DevAdapter.DevSensorAdapter(Arrpoint, _SourceNum, Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))), DevPropertyID);
                CpDocument.Controls.Add(SensroTemp);//添加测点属性集控件
                SensroTemp.Dock = DockStyle.Fill;
                this.CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, DevPropertyID);

                //先根据已定义的信息,进行绑定,如果未找到定义的信息则默认加载第一个设备  20170726
                if (_Point != null)
                {
                    for (int i = 0; i < CcmbDvType.Properties.Items.Count; i++)
                    {
                        if (CcmbDvType.Properties.Items[i].Value.ToString() == _Point.Devid.ToString())
                        {
                            CcmbDvType.SelectedIndex = i;
                            break;
                        }
                    }
                }
                if (this.CcmbDvType.SelectedItem == null)
                {
                    //modified by  20170313 增加选择后的默认值
                    CcmbDvType.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("设备性质改变事件【CcmbDvProPerty_SelectedIndexChanged】", ex);
            }
        }
        /// <summary>设备类型改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbDvType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //风电闭锁判断  20170715
                if (_Point != null && !string.IsNullOrEmpty(CcmbDvType.Text))
                {
                    if (_Point.Devid != CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString())//如果设备类型不相等  20170406
                    {
                        if (!RelateUpdate.ControlPointLegalAll(_Point))
                        {
                            XtraMessageBox.Show("当设备在风电闭锁中已使用，请先解除风电闭锁使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.Text = _Point.DevName;
                            return;
                        }
                    }
                }

                if (CpDocument.Controls.Count > 0)
                {
                    if (null != CpDocument.Controls[0])
                    {
                        //if (!bChangeDevType)
                        //{
                        if (!string.IsNullOrEmpty(CcmbDvType.Text))
                        {
                            //如果修改了设备类型，则清除  20170321                                
                            //if (_Point != null)
                            //{
                            //    if (_Point.Dzh == 0)
                            //    {
                            //        if (_Point.Devid != CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString())//如果设备类型不相等  20170406
                            //        {
                            //            //增加提示  20170504
                            //            if (XtraMessageBox.Show("当前修改了设备类型，是否将原有设备删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //            {
                            //                DeletePoint(_Point);
                            //                InitChannleTreeData();
                            //                CTreeListChanne.RefreshDataSource();
                            //            }
                            //            else
                            //            {
                            //                CcmbDvType.Text = _Point.Devid + "." + _Point.DevName;
                            //                return;
                            //            }
                            //        }
                            //    }
                            //}

                            //修改多参数方式，从设备类型定义中读取  20170505
                            if (_Point == null || _Point.Dzh == 0)//如果非编辑状态，才从设备类型定义中读取  20171103//如果从单参数，改成多参数，需要重新根据设备类型置地址号  20171122
                            {
                                Jc_DevInfo Dev = Model.DEVServiceModel.QueryDevByDevIDCache(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString());
                                if (Dev != null)
                                {
                                    if (!string.IsNullOrEmpty(Dev.Bz8))//如果选择了默认的多参数，将设备自动改为2参数 
                                    {
                                        int TempMult = 0;
                                        int.TryParse(Dev.Bz8, out TempMult);
                                        if (TempMult > 1)//表示多参数主通道
                                        {
                                            MultipleCount.SelectedIndex = TempMult - 1;
                                            _AddressNum = 1;
                                        }
                                        else
                                        {//其它通道,地址号不能为1
                                            MultipleCount.SelectedIndex = 0;
                                            if (_AddressNum == 1)
                                            {
                                                _AddressNum = 0;
                                            }
                                        }
                                    }
                                    else
                                    {//其它通道,地址号不能为1
                                        MultipleCount.SelectedIndex = 0;
                                        if (_AddressNum == 1)
                                        {
                                            _AddressNum = 0;
                                        }
                                    }
                                }
                            }

                            CcmbAddressNum.Text = _AddressNum.ToString();
                            if (!string.IsNullOrEmpty(CcmbDvProPerty.Text))
                            {
                                CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
                            }
                            CuBase temp = (CuBase)CpDocument.Controls[0];
                            if (_Point != null)
                            {
                                if (_Point.Devid != CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString())
                                {//如果设备类型改变,则不传原来定义的测点进行加载,重新按新的测点加载数据  20170726
                                    temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), null);
                                }
                                else
                                {
                                    if (temp._arrPoint != _Point.Point)
                                    {
                                        temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), _Point);
                                    }
                                }
                            }
                            else
                            {
                                temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), null);
                            }
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("设备类型改变事件【CcmbDvProPerty_SelectedIndexChanged】", ex);
            }
        }
        /// <summary>窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CfCommonSensor_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        /// <summary>选择通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CTreeListChanne_Click(object sender, EventArgs e)
        {
            try
            {
                if (CTreeListChanne.FocusedNode == null)
                {
                    return;
                }
                TreeListItem DataRecord = (TreeListItem)CTreeListChanne.GetDataRecordByNode(CTreeListChanne.FocusedNode);

                if (DataRecord == null)
                {
                    return;
                }

                //清除传递到窗口的参数  20170924
                _PropertyNum = -1;
                _DevId = -1;

                SelectChanelNow = DataRecord.Name;


                //累计通道不能进行修改，删除，复制，粘贴操作  20170401
                if (SelectChanelNow.Contains("累计通道"))
                {
                    CbtnSensorCopy.Enabled = false;
                    CbtnSensorPasty.Enabled = false;
                    //Cbtn_Confirm.Enabled = false;//修改允许累计量修改安装位置信息  20180309
                    CcmbDvProPerty.Enabled = false;
                    CcmbDvType.Enabled = false;
                    CcmbDefineState.Enabled = false;

                    CbntDelete.Enabled = false;
                }
                else
                {
                    CbtnSensorCopy.Enabled = true;
                    CbtnSensorPasty.Enabled = true;
                    //Cbtn_Confirm.Enabled = true;
                    CcmbDvProPerty.Enabled = true;
                    CcmbDvType.Enabled = true;
                    CcmbDefineState.Enabled = true;

                    CbntDelete.Enabled = true;
                }


                if (DataRecord.Code == "多参数")
                {
                    if (CTreeListChanne.FocusedNode.HasChildren)
                    {
                        DataRecord = (TreeListItem)CTreeListChanne.GetDataRecordByNode(CTreeListChanne.FocusedNode.Nodes[0]);
                    }
                }
                //if (_Point != null)
                //{
                //    if (DataRecord.Code == _Point.Point)
                //    {
                //        return;
                //    }
                //}
                RefreshGridInDev(DataRecord.Tag, DataRecord.Code, DataRecord.Pragram1, DataRecord.Pragram2);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary> 测点名称值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CglePointName_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    string displayName = this.CglePointName.Properties.DisplayMember;
                    string valueName = this.CglePointName.Properties.ValueMember;
                    string display = e.DisplayValue.ToString();
                    if (string.IsNullOrEmpty(display))//xuzp20151023
                    {
                        return;
                    }
                    //特殊字符判断  20170428
                    if (DefinePublicClass.ValidationSpecialSymbols(CglePointName.Text))
                    {
                        XtraMessageBox.Show("测点名称中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DataTable dtTemp = this.CglePointName.Properties.DataSource as DataTable;
                    if (dtTemp != null)
                    {
                        dtTemp.CaseSensitive = true;//设置区分大小写  20170815
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
        /// 地址号改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbAddressNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CcmbAddressNum.Text))
                {
                    return;
                }
                //if (!bChangeDevType)
                //{
                if (!string.IsNullOrEmpty(CcmbDvProPerty.Text))
                {
                    CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, Convert.ToUInt32(CcmbAddressNum.Text), Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
                }
                if (string.IsNullOrEmpty(CtxbArrPoint.Text))
                {
                    return;
                }
                _Point = Model.DEFServiceModel.QueryPointByCodeCache(CtxbArrPoint.Text);
                if (null != _Point)
                {
                    _SourceNum = (uint)_Point.Fzh;
                    _ChannelNum = (uint)_Point.Kh;
                    _AddressNum = (uint)_Point.Dzh;
                }
                else
                {
                    if (!string.IsNullOrEmpty(CcmbChannelNum.Text))
                    {
                        _ChannelNum = Convert.ToUInt16(CcmbChannelNum.Text);
                    }
                    if (!string.IsNullOrEmpty(CcmbAddressNum.Text))
                    {
                        _AddressNum = Convert.ToUInt16(CcmbAddressNum.Text);
                    }
                    else
                    {
                        _AddressNum = 0;
                    }

                    if (string.IsNullOrEmpty(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))))
                    {
                        _PropertyNum = -1;
                    }
                    else
                    {
                        _PropertyNum = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.')));
                    }
                }
                LoadBasicInf();
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region =========================业务函数===========================
        /// <summary>
        /// 人员定位特殊处理
        /// </summary>
        /// <param name="temp"></param>
        private void PersonSpecialHand(Jc_DefInfo _Point)
        {
            //人员定位特殊处理，增加未定义识别器查找，并自动定义 20171127
            R_UndefinedDefInfo undefinedDefInfo = DEFServiceModel.QueryAllR_UndefinedDefCache().Find(a => a.Point == _Point.Point);
            if (undefinedDefInfo != null)
            {
                _Point.PointID = undefinedDefInfo.PointId;//将未定义设备缓存中的PointId赋值给当前定义的设备  20171128               
            }
            //人员定位识别器复制时，更改禁止进入、限制进入对象列表的PointId  20171124
            if (_Point.RestrictedpersonInfoList != null)
            {
                if (_Point.RestrictedpersonInfoList.Count > 0)
                {
                    foreach (R_RestrictedpersonInfo temp in _Point.RestrictedpersonInfoList)
                    {
                        temp.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        temp.PointId = _Point.PointID;
                    }
                }
            }
        }
        /// <summary> 适应布局
        /// </summary>
        private void ArrtibuteLayout()
        {
            if (CpDocument.Controls.Count > 0)
            {
                this.Width -= CpDocument.Controls[0].Width + 5;
                this.Height -= CpDocument.Controls[0].Height;
            }
        }

        /// <summary> 加载信息
        /// </summary>
        private void LoadBasicInf()
        {
            try
            {
                CTreeListChanne.DataSource = TreeListSource.TreeListChannle;
                CTreeListChanne.RefreshDataSource();
                CTreeListChanne.ExpandAll();


                //智能分站判断   20170417               
                Jc_DefInfo FzInfo = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).ToList().Find(a => a.DevPropertyID == 0);

                if (FzInfo != null)
                {
                    Jc_DevInfo FzDev = new Jc_DevInfo();
                    if (FzInfo != null)
                    {
                        FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == FzInfo.Devid);
                    }
                    if (FzDev.LC2 == 13)//智能分站 
                    {
                        IsZLFz = true;
                    }
                }

                //判断智能分站9~16号，需要选择是智能型还是频率型
                //if (IsZLFz)
                //{
                //    if (_ChannelNum >= 9 && _ChannelNum <= 16)//智能分站
                //    {
                SensorCommunicationTypeLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                SensorCommunicationType.Text = "智能型";
                //    }
                //    else
                //    {
                //        SensorCommunicationTypeLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //        SensorCommunicationType.Text = "智能型";
                //    }
                //}
                //else//非智能分站不显示
                //{
                //    SensorCommunicationTypeLayout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //    SensorCommunicationType.Text = "频率型";
                //}


                MultipleCount.Enabled = false;


                if (_Point != null) //已有测点
                {

                    //累计通道不能进行修改，删除，复制，粘贴操作  20170401
                    if (_Point.DevPropertyID == 4)
                    {
                        CbtnSensorCopy.Enabled = false;
                        CbtnSensorPasty.Enabled = false;
                        //Cbtn_Confirm.Enabled = false;//修改允许累计量修改安装位置信息  20180309
                        CcmbDvProPerty.Enabled = false;
                        CcmbDvType.Enabled = false;
                        CcmbDefineState.Enabled = false;

                        CbntDelete.Enabled = false;
                    }
                    else
                    {
                        CbtnSensorCopy.Enabled = true;
                        CbtnSensorPasty.Enabled = true;
                        //Cbtn_Confirm.Enabled = true;
                        CcmbDvProPerty.Enabled = true;
                        CcmbDvType.Enabled = true;
                        CcmbDefineState.Enabled = true;

                        CbntDelete.Enabled = true;
                    }

                    CtxbArrPoint.Text = _Point.Point; //测点编号
                    CglePointName.EditValue = _Point.Wzid;//测点名称

                    #region 赋值是否报警
                    if (CcmbDvType.Text == _Point.DevName) //本次设备类型不发生改变
                    {
                        if (CpDocument.Controls.Count > 0)
                        {
                            if (null != CpDocument.Controls[0])
                            {
                                CuBase temp = (CuBase)CpDocument.Controls[0];
                                temp.DevTypeChanngeEvent(long.Parse(_Point.Devid), _Point);
                            }
                        }
                    }
                    #endregion

                    isAutoSelCcmbDvProPerty = true;
                    CcmbDvProPerty.SelectedIndex = -1;//设置设备性质初值，以便每次重新加载赋初值  20170509
                    CcmbDvProPerty.SelectedItem = _Point.DevPropertyID.ToString() + "." + _Point.DevProperty;//设备性质
                    if (_PropertyNum > 0 && _DevId > 0 && _PropertyNum != _Point.DevPropertyID)//如果传递了自动挂接设备的设备性质及设备类型
                    { //井下挂接设备的设备性质与测点的设备性质不一致时,使用井下挂接设备的设备性质  20170614
                        Dictionary<int, EnumcodeInfo> tempProperty = Model.DEVServiceModel.QueryDevPropertisCache();
                        if (tempProperty != null)
                        {
                            CcmbDvProPerty.SelectedItem = tempProperty[_PropertyNum].LngEnumValue + "." + tempProperty[_PropertyNum].StrEnumDisplay;
                        }
                    }
                    //CcmbDvType.SelectedItem = _Point.Devid.ToString() + "." + _Point.DevName;//设备类型
                    //赋值设备类型修改  20170504
                    //设置设备类型初值，以便每次重新加载赋初值  20170509
                    CcmbDvType.SelectedIndex = -1;
                    for (int i = 0; i < CcmbDvType.Properties.Items.Count; i++)
                    {
                        if (_DevId > 0 && _DevId != long.Parse(_Point.Devid))
                        {//井下挂接设备的设备类型与测点的设备类型不一致时,使用井下挂接设备的设备类型  20170614
                            if (CcmbDvType.Properties.Items[i].Value.ToString() == _DevId.ToString())
                            {
                                CcmbDvType.SelectedIndex = i;
                                break;
                            }
                        }
                        else
                        {
                            if (CcmbDvType.Properties.Items[i].Value.ToString() == _Point.Devid.ToString())
                            {
                                CcmbDvType.SelectedIndex = i;
                                break;
                            }
                        }
                    }


                    if ((_Point.Bz4 & 0x08) == 0x08)
                    {
                        CcmbDefineState.Text = "标校";
                    }
                    else if ((_Point.Bz4 & 0x04) == 0x04)
                    {
                        CcmbDefineState.Text = "检修";
                    }
                    else if ((_Point.Bz4 & 0x02) == 0x02)
                    {
                        CcmbDefineState.Text = "休眠";
                    }
                    else
                    {
                        CcmbDefineState.Text = "运行";
                    }
                    CcmbChannelNum.Text = _Point.Kh.ToString();//通道号/kh
                    //if (_DevId < 1)//如果是自动定义，则不根据已定义的设备动态赋值地址号  20171016
                    //{
                    CcmbAddressNum.Text = _Point.Dzh.ToString();//地址号/dzh
                    //}
                    //bChangeDevType = false; //改变限制标记 *

                    if (!string.IsNullOrEmpty(_Point.Bz12))//如果是自动定义，则不根据已定义的设备动态赋值多参数个数  20171016
                    {
                        MultipleCount.Text = _Point.Bz12;//赋值参数个数  20170415                       
                    }
                    else
                    {
                        MultipleCount.Text = "1";
                    }
                    //赋值传感器通信类型  20170612
                    if (_Point.Bz18 == "0")
                    {
                        SensorCommunicationType.Text = "智能型";
                    }
                    else
                    {
                        SensorCommunicationType.Text = "频率型";
                    }

                    if (_Point.Dzh > 0)
                    {
                        CcmbDvProPerty.Properties.ReadOnly = true;
                        CcmbDvType.Properties.ReadOnly = true;
                    }
                    else
                    {
                        CcmbDvProPerty.Properties.ReadOnly = false;
                        CcmbDvType.Properties.ReadOnly = false;
                    }

                    //加载测点的位置信息  20170829
                    if (!string.IsNullOrEmpty(_Point.XCoordinate) && !string.IsNullOrEmpty(_Point.YCoordinate))
                    {
                        txt_Coordinate.Text = _Point.XCoordinate + "," + _Point.YCoordinate;
                        PointAreaId = _Point.Areaid;
                    }
                    else
                    {
                        txt_Coordinate.Text = "";
                        PointAreaId = "";
                    }
                    //加载测点区域信息
                    areaSelect.SelectedIndex = 0;
                    for (int i = 0; i < areaSelect.Properties.Items.Count; i++)
                    {
                        string areaName = areaSelect.Properties.Items[i].ToString();
                        AreaInfo area = AreaListAll.Find(a => a.Areaname == areaName);
                        if (area != null)
                        {
                            if (area.Areaid == _Point.Areaid)
                            {
                                PointAreaId = area.Areaid;
                                areaSelect.SelectedIndex = i;
                            }
                        }
                    }
                    //加载地点类型信息
                    comboAddresstype.SelectedIndex = 0;
                    for (int i = 0; i < comboAddresstype.Properties.Items.Count; i++)
                    {
                        string addressTypeName = comboAddresstype.Properties.Items[i].ToString();
                        KJ_AddresstypeInfo addressType = AddresstypeInfoListAll.Find(a => a.Addresstypename == addressTypeName);
                        if (addressType != null)
                        {
                            if (addressType.ID == _Point.Addresstypeid)
                            {
                                comboAddresstype.SelectedIndex = i;
                            }
                        }
                    }
                    //加载生产日期
                    dateEdit1.Text = _Point.Bz15;

                }
                else //新增测点
                {
                    txt_Coordinate.Text = "";
                    PointAreaId = "";
                    comboAddresstype.SelectedIndex = 0;
                    areaSelect.SelectedIndex = 0;
                    dateEdit1.Text = "";

                    //bChangeDevType = false; //改变限制标记 *
                    CglePointName.Text = "";
                    OrigionDevText = "";//清空原始值
                    OrigionDevText = CcmbDvType.Text; //记录原始值
                    if (_PropertyNum > 0)
                    {
                        Dictionary<int, EnumcodeInfo> tempProperty = Model.DEVServiceModel.QueryDevPropertisCache();
                        if (tempProperty != null)
                        {
                            isAutoSelCcmbDvProPerty = true;
                            if (tempProperty.ContainsKey(_PropertyNum))//增加判断  20170704
                            {
                                CcmbDvProPerty.SelectedItem = tempProperty[_PropertyNum].LngEnumValue + "." + tempProperty[_PropertyNum].StrEnumDisplay;
                                //重新触发事件  20171124
                                object sender1 = null;
                                EventArgs e1 = new EventArgs();
                                CcmbDvProPerty_SelectedIndexChanged(sender1, e1);
                            }
                        }
                        else
                        {
                            if (CcmbDvProPerty.Properties.Items.Count > 0)
                            {
                                isAutoSelCcmbDvProPerty = true;
                                CcmbDvProPerty.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        if (CcmbDvProPerty.Properties.Items.Count > 0)
                        {
                            isAutoSelCcmbDvProPerty = true;
                            CcmbDvProPerty.SelectedIndex = 0;
                        }
                    }
                    CcmbChannelNum.Text = _ChannelNum.ToString();//通道号
                    CcmbAddressNum.Text = _AddressNum.ToString();//地址号

                    MultipleCount.Text = "1";//赋值参数个数  20170415

                    CcmbDefineState.SelectedItem = "运行";//定义状态                  
                    if (FzInfo.Bz4 > 1)
                    { //如果分站设置了休眠或者检修状态，默认加载分站的休眠检修状态  20170621
                        if ((FzInfo.Bz4 & 0x04) == 0x04)
                        {
                            CcmbDefineState.Text = "检修";
                        }
                        else if ((FzInfo.Bz4 & 0x02) == 0x02)
                        {
                            CcmbDefineState.Text = "休眠";
                        }
                    }

                    //设备类型  
                    CcmbDvType.SelectedIndex = -1;
                    if (_DevId > 0)
                    {
                        for (int i = 0; i < CcmbDvType.Properties.Items.Count; i++)
                        {
                            if (CcmbDvType.Properties.Items[i].Value.ToString() == _DevId.ToString())
                            {
                                CcmbDvType.SelectedIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (CcmbDvType.Properties.Items.Count > 0)
                        {
                            CcmbDvType.SelectedIndex = 0;
                        }
                    }

                    if (OrigionDevText == CcmbDvType.Text)
                    {
                        if (CpDocument.Controls.Count > 0)
                        {
                            if (null != CpDocument.Controls[0])
                            {
                                if (!string.IsNullOrEmpty(CcmbDvType.Text))
                                {
                                    CuBase temp = (CuBase)CpDocument.Controls[0];
                                    temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), _Point);
                                }
                            }
                        }
                    }
                    //测点编码
                    if (!string.IsNullOrEmpty(CcmbDvProPerty.Text))
                    {
                        CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
                    }

                    CcmbDvProPerty.Properties.ReadOnly = false;
                    CcmbDvType.Properties.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载基础信息【LoadBasicInf】", ex);
            }
        }

        /// <summary> 加载默认的初始信息
        /// </summary>
        private void LoadPretermitInf()
        {
            try
            {
                //this.CglePointName.Properties.View.BestFitColumns();
                //this.CglePointName.Properties.DisplayMember = "Wz";
                //this.CglePointName.Properties.ValueMember = "WzID";
                IList<Jc_WzInfo> PointName = Model.WZServiceModel.QueryWZsCache();
                DataTable dtTemp = Model.DevAdapter.ListToDataTable(PointName);
                if (null != PointName)
                {
                    this.CglePointName.Properties.DataSource = dtTemp;
                }
                //设备性质
                Dictionary<int, EnumcodeInfo> tempPropertis = Model.DEVServiceModel.QueryDevPropertisCache();
                if (null != tempPropertis)
                {
                    foreach (var item in tempPropertis.Values)
                    {
                        if (item.LngEnumValue == 0)
                        {
                            continue;
                        }
                        CcmbDvProPerty.Properties.Items.Add(item.LngEnumValue.ToString() + "." + item.StrEnumDisplay);
                    }
                }
                //定义状态
                CcmbDefineState.Properties.Items.Add("运行");
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
                    CcmbDefineState.Properties.Items.Add("休眠");
                }
                //CcmbDefineState.Properties.Items.Add("检修"); 取消检修状态设置  20170627
                //CcmbDefineState.Properties.Items.Add("标校"); 取消标校状态设置  20170627


                //地址号 测试 是否根据设备型号调整
                for (int i = -1; i < 24; i++)
                {
                    CcmbAddressNum.Properties.Items.Add((i + 1).ToString());
                }
                //设备参数个数初始值  20170415
                MultipleCount.Properties.Items.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    MultipleCount.Properties.Items.Add((i).ToString());
                }
                CtxbArrPoint.Enabled = false;
                CcmbAddressNum.Enabled = false;
                AreaCacheGetAllRequest arearequest = new AreaCacheGetAllRequest();
                AreaListAll = areaService.GetAllAreaCache(arearequest).Data;
                for (int i = 0; i < AreaListAll.Count; i++)
                {
                    areaSelect.Properties.Items.Add(AreaListAll[i].Areaname);
                }
                areaSelect.Properties.Items.Insert(0, "");
                areaSelect.SelectedIndex = 0;
                //加载地点类型信息
                KJ_AddresstypeGetListRequest kJ_AddresstypeRequest = new KJ_AddresstypeGetListRequest();
                AddresstypeInfoListAll = addresstypeService.GetKJ_AddresstypeList(kJ_AddresstypeRequest).Data;
                for (int i = 0; i < AddresstypeInfoListAll.Count; i++)
                {
                    comboAddresstype.Properties.Items.Add(AddresstypeInfoListAll[i].Addresstypename);
                }
                comboAddresstype.Properties.Items.Insert(0, "");
                comboAddresstype.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载默认的初始信息【LoadPretermitInf】", ex);
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

        /// <summary> 删除测点
        /// </summary>
        /// <param name="ArrPoint"></param>
        private bool DeletePoint(Jc_DefInfo temp)
        {
            bool rvlaue = false;
            try
            {


                if (null == temp)
                {
                    return rvlaue;
                }

                try
                {
                    if (temp.Dzh > 0)//增加判断,多参数才删除关联  20170504
                    {
                        relatedDelete(temp);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }

                try
                {
                    if (temp.DevPropertyID == 3)//控制量删除关联更新&手动控制
                    {
                        Model.RelateUpdate.ControlReUpdate(temp);
                    }
                    if (temp.DevPropertyID == 2)//开关量删除关联更新
                    {
                        Model.RelateUpdate.DerailReUpdate(temp);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }

                temp.InfoState = InfoState.Modified;
                temp.Activity = "0";
                temp.DeleteTime = DateTime.Now;
                try
                {
                    rvlaue = Model.DEFServiceModel.UpdateDEFCache(temp);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20160111

                _Point = null;

            }
            catch (Exception ex)
            {
                rvlaue = false;
                LogHelper.Error("删除测点【DeletePoint】", ex);
            }
            return rvlaue;
        }

        ///<summary> 验证有效性
        /// </summary>
        /// <returns></returns>
        private bool Sensorverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(CglePointName.Text))
            {
                XtraMessageBox.Show("请填写测点名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CglePointName.Text))
            {
                XtraMessageBox.Show("测点名称中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CglePointName.Text.Length > 30)
            {
                XtraMessageBox.Show("测点名称长度不能超过30个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbDvProPerty.Text))
            {
                XtraMessageBox.Show("请选择设备性质", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbDvProPerty.Text))
            {
                XtraMessageBox.Show("请选择设备类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbDefineState.Text))
            {
                XtraMessageBox.Show("请选择定义状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbArrPoint.Text))
            {
                XtraMessageBox.Show("请填写测点编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbChannelNum.Text))
            {
                XtraMessageBox.Show("请设置通道号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbAddressNum.Text))
            {
                XtraMessageBox.Show("请设置地址号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            //modified by  20170313 增加设备型号的检测判断，不然设备型号为空，后续 程序会报错
            if (string.IsNullOrEmpty(CcmbDvType.Text))
            {
                XtraMessageBox.Show("请设置设备型号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            ret = true;
            return ret;
        }

        /// <summary> 初始化通道数据
        /// </summary>
        private void InitChannleTreeData()
        {
            Model.TreeListSource.TreeListChannle.Clear();//清空数据源
            int count = 1;
            int ParentID;
            Jc_DefInfo Def;
            if (_SourceNum > 0)
            {
                Model.TreeListSource.TreeListChannle.Add(new TreeListItem(_SourceNum.ToString() + "号分站通道列表", "Station", "0", count++, -1));
            }
            else
            {
                Model.TreeListSource.TreeListChannle.Add(new TreeListItem("通道列表", "Station", "0", count++, -1));
            }
            bool bContractCtrl = false;
            if (string.IsNullOrEmpty(_SourcePoint))
            {
                return;
            }
            Def = Model.DEFServiceModel.QueryPointByCodeCache(_SourcePoint);
            if (null == Def)
            {
                return;
            }
            Jc_DevInfo Dev = Model.DEVServiceModel.QueryDevByDevIDCache(Def.Devid);
            if (null == Dev)
            {
                return;
            }
            IList<Jc_DefInfo> tempChannel;

            //先加载所有定义信息，后面循环中，直接从内存中获取 
            List<Jc_DefInfo> JcDefAllCache = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).ToList();
            Jc_DefInfo JcDefFzNow = JcDefAllCache.Find(a => a.DevPropertyID == 0);
            Jc_DevInfo FzDev = new Jc_DevInfo();
            if (JcDefFzNow != null)
            {
                FzDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == JcDefFzNow.Devid);
            }

            #region 加载模开智能断电器通道
            for (int i = 0; i < Dev.Pl1; i++)
            {
                //tempChannel = Model.DEFServiceModel.QueryMulitPramPointByChannel((int)_SourceNum, (i + 1));//智能断电器&KXB18
                //tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh > 0);//智能断电器&KXB18  //注释  20170415
                //if (tempChannel.Count > 0)
                //{
                //    if (tempChannel.Count > 1)
                //    {
                //        tempChannel = tempChannel.OrderBy(temp => temp.Dzh).ToList();
                //        if (tempChannel[0].DevPropertyID == 3)
                //        {
                //            tempChannel = tempChannel.OrderBy(item => item.Dzh).ToList();
                //            ParentID = count;
                //            if (tempChannel[0].Devid == 97)//test
                //            {
                //                TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【声光报警器】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));
                //            }
                //            else
                //            {
                //                TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【智能断电器】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));
                //            }
                //            for (int j = 0; j < tempChannel.Count; j++)
                //            {
                //                TreeListSource.TreeListChannle.Add(new TreeListItem(tempChannel[j].Point, tempChannel[j].Point, tempChannel[j].DevPropertyID.ToString(), tempChannel[j].Kh.ToString(), tempChannel[j].Dzh.ToString(), count++, ParentID));
                //            }
                //            continue;
                //        }
                //    }
                //}

                //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 1); //模拟量
                tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (a.DevPropertyID == 1 || a.Dzh > 0));//模拟量  20170428
                if (tempChannel.Count > 0)
                {
                    tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                    int tempInt = 0;
                    int.TryParse(tempChannel[0].Bz12, out tempInt);
                    if (tempInt > 1 && tempChannel[0].Dzh == 1)//表示多参数传感器
                    {
                        ParentID = count;
                        TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "1", (i + 1).ToString(), "0", count++, 1));

                        for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                        {
                            List<Jc_DefInfo> tempdefC = JcDefAllCache.ToList().FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh == j);
                            if (tempdefC.Count > 0)
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                    tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                            }
                            else
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem("多参数-" + j.ToString(),
                                    "多参数", tempChannel[0].DevPropertyID.ToString(),
                                       tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                            }
                        }
                    }
                    else
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                            tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                    }
                    continue;
                }
                //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 2);//开关量
                tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (a.DevPropertyID == 2 || a.Dzh > 0));//开关量  20170428
                if (tempChannel.Count > 0)
                {
                    tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                    int tempInt = 0;
                    int.TryParse(tempChannel[0].Bz12, out tempInt);
                    if (tempInt > 1 && tempChannel[0].Dzh == 1)//表示多参数传感器
                    {
                        ParentID = count;
                        TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));

                        for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                        {
                            List<Jc_DefInfo> tempdefC = JcDefAllCache.ToList().FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh == j);
                            if (tempdefC.Count > 0)
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                    tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                            }
                            else
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem("多参数-" + j.ToString(),
                                    "多参数", tempChannel[0].DevPropertyID.ToString(),
                                       tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                            }
                        }
                    }
                    else
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                            tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                    }
                    continue;
                }

                if (FzDev.LC2 == 13 || FzDev.LC2 == 12)//智能分站处理  20170323
                {
                    //tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && 3 == a.DevPropertyID && a.Kh >= 5 && a.Kh <= 16);//加载控制量5~16号口
                    tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (a.DevPropertyID == 3 && a.Kh >= 5 && a.Kh <= 16 || a.Dzh > 0));//加载控制量5~16号口  20170428
                    if (tempChannel.Count > 0)
                    {
                        tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                        int tempInt = 0;
                        int.TryParse(tempChannel[0].Bz12, out tempInt);
                        if (tempInt > 1 && tempChannel[0].Dzh == 1)//表示多参数传感器
                        {
                            ParentID = count;
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));

                            for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                            {
                                List<Jc_DefInfo> tempdefC = JcDefAllCache.ToList().FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh == j);
                                if (tempdefC.Count > 0)
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                        tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                                }
                                else
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem("多参数-" + j.ToString(),
                                        "多参数", tempChannel[0].DevPropertyID.ToString(),
                                           tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                                }
                            }
                        }
                        else//注释，只加载多参数，单参数5~16号口不能定义控制量  20180123
                        {
                            //TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                            //    tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "1", (i + 1).ToString(), "0", count++, 1));
                        }
                        continue;
                    }
                }

                TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "1", (i + 1).ToString(), "0", count++, 1));
            }
            #endregion

            #region 如果是抽放方站加载分钟流量通道  20190616
            if (FzDev.LC2 == 14)//抽放分站  20170323
            {
                for (int i = 39; i <= 42; i++)
                {
                    tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (a.DevPropertyID == 1 || a.Dzh > 0));//模拟量  20170428
                    if (tempChannel.Count > 0)
                    {
                        tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                        int tempInt = 0;
                        int.TryParse(tempChannel[0].Bz12, out tempInt);
                        if (tempInt > 1 && tempChannel[0].Dzh == 1)//表示多参数传感器
                        {
                            ParentID = count;
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "1", (i + 1).ToString(), "0", count++, 1));

                            for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                            {
                                List<Jc_DefInfo> tempdefC = JcDefAllCache.ToList().FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh == j);
                                if (tempdefC.Count > 0)
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                        tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                                }
                                else
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem("多参数-" + j.ToString(),
                                        "多参数", tempChannel[0].DevPropertyID.ToString(),
                                           tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                                }
                            }
                        }
                        else
                        {
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                                tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                        }
                        continue;
                    }
                    tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (a.DevPropertyID == 2 || a.Dzh > 0));//开关量  20170428
                    if (tempChannel.Count > 0)
                    {
                        tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                        int tempInt = 0;
                        int.TryParse(tempChannel[0].Bz12, out tempInt);
                        if (tempInt > 1 && tempChannel[0].Dzh == 1)//表示多参数传感器
                        {
                            ParentID = count;
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));

                            for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                            {
                                List<Jc_DefInfo> tempdefC = JcDefAllCache.ToList().FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && a.Dzh == j);
                                if (tempdefC.Count > 0)
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                        tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                                }
                                else
                                {
                                    TreeListSource.TreeListChannle.Add(new TreeListItem("多参数-" + j.ToString(),
                                        "多参数", tempChannel[0].DevPropertyID.ToString(),
                                           tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                                }
                            }
                        }
                        else
                        {
                            TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                                tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                        }
                        continue;
                    }
                    TreeListSource.TreeListChannle.Add(new TreeListItem("基础通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "1", (i + 1).ToString(), "0", count++, 1));
                }
            }
            #endregion

            #region 加载智能通道
            for (int i = 16; i < Dev.Pl3 + 16; i++)
            {
                //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 7);//智能量
                tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && (2 == a.DevPropertyID || 3 == a.DevPropertyID));//智能通道只能定义开关量、控制量,地址号从17~32  20170322
                if (tempChannel.Count > 0)
                {
                    tempChannel = tempChannel.OrderBy(a => a.Dzh).ToList();//按地址号排序
                    int tempInt = 0;
                    int.TryParse(tempChannel[0].Bz12, out tempInt);
                    if (tempInt > 1)//表示多参数传感器
                    {
                        ParentID = count;
                        TreeListSource.TreeListChannle.Add(new TreeListItem("智能通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "2", (i + 1).ToString(), "0", count++, 1));

                        for (int j = 1; j <= tempInt; j++)//遍历添加多参数传感器  20170415
                        {
                            List<Jc_DefInfo> tempdefC = tempChannel.ToList().FindAll(a => a.Dzh == j);
                            if (tempdefC.Count > 0)
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem(tempdefC[0].Point, tempdefC[0].Point, tempdefC[0].DevPropertyID.ToString(),
                                    tempdefC[0].Kh.ToString(), tempdefC[0].Dzh.ToString(), count++, ParentID));
                            }
                            else
                            {
                                TreeListSource.TreeListChannle.Add(new TreeListItem("智能通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数" + j.ToString() + "】",
                                    "多参数", tempChannel[0].DevPropertyID.ToString(),
                                       tempChannel[0].Kh.ToString(), j.ToString(), count++, ParentID));
                            }
                        }
                    }
                    else
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("智能通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】",
                            tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                    }
                    continue;
                }
                //智能通道口号地址号从17~32  20170322
                TreeListSource.TreeListChannle.Add(new TreeListItem("智能通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "2", (i + 1).ToString(), "0", count++, 1));
            }
            #endregion

            #region 加载控制通道
            //if (FzDev.LC2 == 13)//智能分站处理，智能分站控制量从33号地址开始  20170322//取消  20170323
            //{
            //    for (int i = 32; i < Dev.Pl2+32 ; i++)
            //    {
            //        //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 3);//控制量
            //        tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && 3 == a.DevPropertyID);//控制量
            //        if (tempChannel.Count > 0)
            //        {
            //            bContractCtrl = false;
            //            for (int temp = 0; temp < tempChannel.Count; temp++)
            //            {
            //                if (tempChannel[temp].Dzh == 0)
            //                {
            //                    TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】", tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
            //                    bContractCtrl = true;
            //                    break;
            //                }
            //            }
            //            if (!bContractCtrl)
            //            {
            //                TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "3", (i + 1).ToString(), "0", count++, 1));
            //                continue;
            //            }
            //            else
            //            {
            //                continue;
            //            }
            //        }
            //        TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "3", (i + 1).ToString(), "0", count++, 1));
            //    }
            //}
            //else
            //{
            for (int i = 0; i < Dev.Pl2; i++)
            {
                //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 3);//控制量
                tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && 3 == a.DevPropertyID && a.Dzh == 0);//控制量(非智能断电器)  20180123
                if (tempChannel.Count > 0)
                {
                    bContractCtrl = false;
                    for (int temp = 0; temp < tempChannel.Count; temp++)
                    {
                        if (tempChannel[temp].Dzh == 0)
                        {
                            TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】", tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                            bContractCtrl = true;
                            break;
                        }
                    }
                    if (!bContractCtrl)
                    {

                        TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "3", (i + 1).ToString(), "0", count++, 1));

                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                TreeListSource.TreeListChannle.Add(new TreeListItem("控制通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "3", (i + 1).ToString(), "0", count++, 1));
            }
            //}
            #endregion

            #region 加载人员通道
            for (int i = 0; i < Dev.Pl4; i++)
            {
                //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 8);//人员量
                tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && 7 == a.DevPropertyID);//人员量
                if (tempChannel.Count > 0)
                {
                    if (tempChannel.Count > 1)
                    {
                        ParentID = count;
                        TreeListSource.TreeListChannle.Add(new TreeListItem("人员通道" + (i + 1).ToString().PadLeft(2, '0') + "【多参数】", "多参数", "5", (i + 1).ToString(), "0", count++, 1));

                        for (int j = 0; j < tempChannel.Count; j++)
                        {
                            TreeListSource.TreeListChannle.Add(new TreeListItem(tempChannel[j].Point, tempChannel[j].Point, tempChannel[j].DevPropertyID.ToString(), tempChannel[j].Kh.ToString(), tempChannel[j].Dzh.ToString(), count++, ParentID));
                        }
                    }
                    else
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("人员通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】", tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                    }
                    continue;
                }
                TreeListSource.TreeListChannle.Add(new TreeListItem("人员通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "7", (i + 1).ToString(), "0", count++, 1));
            }
            #endregion

            #region 加载累计通道
            if (Dev.Devid == "5")
            {
                for (int i = 0; i < 16; i++)
                {
                    //tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 4); //累计量
                    tempChannel = JcDefAllCache.FindAll(a => a.Fzh == (int)_SourceNum && a.Kh == (i + 1) && 4 == a.DevPropertyID);//累计量
                    if (tempChannel.Count > 0)
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("累计通道" + (i + 1).ToString().PadLeft(2, '0') + "【" + tempChannel[0].Point + "】", tempChannel[0].Point, tempChannel[0].DevPropertyID.ToString(), tempChannel[0].Kh.ToString(), tempChannel[0].Dzh.ToString(), count++, 1));
                    }
                    else
                    {
                        TreeListSource.TreeListChannle.Add(new TreeListItem("累计通道" + (i + 1).ToString().PadLeft(2, '0') + "【未定义】", "未定义", "1", (i + 1).ToString(), "0", count++, 1));
                    }
                    continue;
                }
            }
            #endregion

            #region 加载导出通道

            #endregion
        }
        /// <summary>数据源赋值
        /// </summary>
        private void RefreshGridInDev(string Tag, string Code, string pragram1, string pragram2)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                _Point = DEFServiceModel.QueryPointByCodeCache(Code);
            }
            if (null != _Point)
            {
                _SourceNum = (uint)_Point.Fzh;
                _ChannelNum = (uint)_Point.Kh;
                _AddressNum = (uint)_Point.Dzh;
            }
            else
            {
                if (!string.IsNullOrEmpty(pragram1))
                {
                    _ChannelNum = Convert.ToUInt16(pragram1);
                }
                if (!string.IsNullOrEmpty(pragram2))
                {
                    _AddressNum = Convert.ToUInt16(pragram2);
                }
                else
                {
                    _AddressNum = 0;
                }

                if (string.IsNullOrEmpty(Tag))
                {
                    _PropertyNum = -1;
                }
                else
                {
                    _PropertyNum = Convert.ToInt32(Tag);
                }
            }
            LoadBasicInf();
        }
        /// <summary>获取新增测点基础信息
        /// </summary>
        private void GetNewPointInf()
        {
            Jc_DefInfo Def;
            if (string.IsNullOrEmpty(_SourcePoint))
            {
                return;
            }
            Def = Model.DEFServiceModel.QueryPointByCodeCache(_SourcePoint);
            if (null == Def)
            {
                return;
            }
            Jc_DevInfo Dev = Model.DEVServiceModel.QueryDevByDevIDCache(Def.Devid);
            if (null == Dev)
            {
                return;
            }
            IList<Jc_DefInfo> tempChannel;
            #region 加载基础通道
            for (int i = 0; i < Dev.Pl1; i++)
            {
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 1); //模拟量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 2);//开关量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 3);//控制量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                _PropertyNum = 1;
                _ChannelNum = (uint)(i + 1);
                SelectChanelNow = "基础通道-" + i.ToString("00");
                _AddressNum = 0;
                return;
            }
            #endregion

            #region 加载智能通道
            for (int i = 0; i < Dev.Pl3; i++)
            {
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 7);//智能量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                _PropertyNum = 2;
                _ChannelNum = (uint)(i + 1);
                SelectChanelNow = "智能通道-" + i.ToString("00");
                _AddressNum = 0;
                return;// 20171122
            }
            #endregion

            #region 加载控制通道
            for (int i = 0; i < Dev.Pl2; i++)
            {
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 3);//控制量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                _PropertyNum = 3;
                _ChannelNum = (uint)(i + 1);
                SelectChanelNow = "控制通道-" + i.ToString("00");
                _AddressNum = 0;
                return;
            }
            #endregion

            #region 加载人员通道
            for (int i = 0; i < Dev.Pl4; i++)
            {
                tempChannel = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, (i + 1), 7);//人员量
                if (tempChannel.Count > 0)
                {
                    continue;
                }
                _PropertyNum = 7;//修改，人员通道设备性质为7  20171123
                _ChannelNum = (uint)(i + 1);
                SelectChanelNow = "人员通道-" + i.ToString("00");
                _AddressNum = 0;
                return;// 20171122
            }
            #endregion

            #region 加载累计通道

            #endregion

            #region 加载导出通道

            #endregion
        }
        /// <summary>
        /// 多参数关联删除
        /// </summary>
        private void relatedDelete(Jc_DefInfo temp)
        {
            if (null == temp)
            {
                return;
            }

            IList<Jc_DefInfo> tempPoints = null;
            tempPoints = DEFServiceModel.QueryMulitPramPointByChannel(temp.Fzh, temp.Kh);
            if (null != tempPoints)
            {
                if (tempPoints.Count > 1)
                {
                    for (int i = 0; i < tempPoints.Count; i++)
                    {
                        if (tempPoints[i].Dzh != temp.Dzh)
                        {
                            try
                            {
                                if (tempPoints[i].DevPropertyID == 3)//控制量删除关联更新
                                {
                                    Model.RelateUpdate.ControlReUpdate(tempPoints[i]);
                                }
                                if (tempPoints[i].DevPropertyID == 2)//开关量删除关联更新
                                {
                                    Model.RelateUpdate.DerailReUpdate(tempPoints[i]);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(ex);
                            }
                            tempPoints[i].Activity = "0";
                            tempPoints[i].DeleteTime = DateTime.Now;
                            tempPoints[i].InfoState = InfoState.Modified;
                            try
                            {
                                Model.DEFServiceModel.UpdateDEFCache(tempPoints[i]);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            OperateLogHelper.InsertOperateLog(1, "删除测点【" + tempPoints[i].Point + "】", "");
                        }
                    }
                }
            }
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
                                additionalTemp.Point = CreatArrPointTag(_SourceNum, _ChannelNum, (uint)(additionalTemp.Dzh), ChanelDTO.Type);
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
                                    temp.K1 = (int)_SourceNum;
                                    temp.K2 = (int)_ChannelNum;
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
        #endregion
        /// <summary>
        /// 多参数选择事件   20170415
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultipleCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CtxbArrPoint.Text = CreatArrPointTag(_SourceNum, _ChannelNum, _AddressNum, Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //重新加载原来测点的信息
            //if (!string.IsNullOrEmpty(_SourceNum.ToString()))
            //{
            //    _Point = DEFServiceModel.QueryPointByChannelInfs((Int16)_SourceNum, Convert.ToInt16(CcmbChannelNum.Text), Convert.ToInt16(CcmbAddressNum.Text),
            //        Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
            //}

            long TempDevid = 0;
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<Jc_DevInfo> alldevList = Model.DEVServiceModel.QueryDevsCache();

                //保存新测点
                if (!Sensorverify())
                {
                    return;
                }

                #region//判断不能定义智能断电器和多参数传感器  20170401
                List<Jc_DefInfo> FzList = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).ToList();
                Jc_DefInfo FzInfo = FzList.Find(a => a.DevPropertyID == 0);
                Jc_DevInfo FzDev = new Jc_DevInfo();
                if (FzInfo != null)
                {

                    if (FzInfo != null)
                    {
                        FzDev = alldevList.Find(a => a.Devid == FzInfo.Devid);
                    }
                    //判断脉冲量计数只能定义在9~16号口
                    //if (FzDev.LC2 == 14)//抽放分站 
                    //{
                    if (CcmbDvType.Text.Contains("脉冲"))
                    {
                        if (_ChannelNum < 9 || _ChannelNum > 16)
                        {
                            XtraMessageBox.Show("脉冲量计数传感器只能定义在9~16号口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.SelectedIndex = 0;
                            return;
                        }
                    }
                    //}

                    if (FzDev.LC2 == 14 || FzDev.LC2 == 13 || FzDev.LC2 == 12)//智能分站 //20190616
                    {
                        if (SelectChanelNow.Contains("基础通道"))
                        {
                            if (_ChannelNum > 0 && _ChannelNum <= 8)
                            {
                                if (CcmbDvType.Text.Contains("智能断电器"))
                                {
                                    XtraMessageBox.Show("基础通道1~8号口，不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CcmbDvType.SelectedIndex = 0;
                                    return;
                                }
                                // 20170504
                                if (CcmbDvProPerty.Text.Contains("控制量"))
                                {
                                    XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CcmbDvType.SelectedIndex = 0;
                                    return;
                                }
                                //判断多参数中存在控制量的情况  20170505
                                int SelDevid = Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString());
                                Jc_DevInfo KhDev = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == SelDevid.ToString());
                                if (KhDev != null)
                                {
                                    string[] SonDev = KhDev.Bz9.Split('|');
                                    foreach (string Dev in SonDev)
                                    {
                                        if (!string.IsNullOrEmpty(Dev))
                                        {
                                            Jc_DevInfo SonDevDTO = Model.DEVServiceModel.QueryDevsCache().ToList().Find(a => a.Devid == Dev);
                                            if (SonDevDTO != null)
                                            {
                                                if (SonDevDTO.Type == 3)
                                                { //控制量
                                                    XtraMessageBox.Show("基础通道1~8号口，不能定义控制量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    CcmbDvType.SelectedIndex = 0;
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //基础通道只能定义智能断电器的控制量参数  20180309
                            if (!CcmbDvType.Text.Contains("智能断电器") && !CcmbDvType.Text.Contains("声光报警") && CcmbDvProPerty.Text.Contains("控制量"))
                            {
                                XtraMessageBox.Show("基础通道控制量只能定义智能断电器或者声光报警器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }

                        if (SelectChanelNow.Contains("智能通道"))//智能通道，只能定义普通开关量，不能定义智能断电器  20170404
                        {
                            if (CcmbDvType.Text.Contains("智能断电器"))
                            {
                                XtraMessageBox.Show("智能通道不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CcmbDvType.SelectedIndex = 0;
                                return;
                            }
                            if (MultipleCount.SelectedIndex > 0)//智能通道不能定义多参数
                            {
                                XtraMessageBox.Show("智能通道不能定义多参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                MultipleCount.SelectedIndex = 0;
                                return;
                            }
                        }
                        //控制通道不能定义智能断电器  20170401
                        if (SelectChanelNow.Contains("控制通道"))
                        {
                            if (CcmbDvType.Text.Contains("智能断电器"))
                            {
                                XtraMessageBox.Show("控制通道不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CcmbDvType.SelectedIndex = 0;
                                return;
                            }
                            //不能定义多参数传感器  20170401
                            int TempInt = 0;
                            int.TryParse(CcmbAddressNum.Text, out TempInt);
                            if (TempInt > 0)
                            {
                                XtraMessageBox.Show("控制通道不能定义多参数传感器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CcmbDvType.SelectedIndex = 0;
                                return;
                            }
                        }

                    }
                    else
                    {
                        if (CcmbDvType.Text.Contains("智能断电器"))
                        {
                            XtraMessageBox.Show("非智能分站，不能定义智能断电器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.SelectedIndex = 0;
                            return;
                        }
                        //非智能分站，不能定义多参数传感器  20170401
                        int TempInt = 0;
                        int.TryParse(CcmbAddressNum.Text, out TempInt);
                        if (TempInt > 0)
                        {
                            XtraMessageBox.Show("非智能分站，不能定义多参数传感器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.SelectedIndex = 0;
                            return;
                        }
                    }
                }
                #endregion

                #region 先处理安装位置
                Jc_WzInfo tempWz = Model.WZServiceModel.QueryWZbyWZCache(this.CglePointName.Text);
                if (null == tempWz)
                {
                    tempWz = new Jc_WzInfo();
                    tempWz.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString(); //自动生成ID                   
                    tempWz.WzID = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();//同步时会更新缓存，此处需要重新从缓存中获取 
                    tempWz.Wz = this.CglePointName.Text; //wz
                    tempWz.CreateTime = DateTime.Now;// 20170331
                    tempWz.InfoState = InfoState.AddNew;
                    try
                    {
                        if (!Model.WZServiceModel.AddJC_WZCache(tempWz))//添加安装位置//TODO:需要判断，如果添加失败，则提示并返回  20170410
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
                Jc_DefInfo temp = new Jc_DefInfo();
                //重新从原来缓存中，将原来测点的数据加载过来 
                if (_Point != null)
                {
                    if (Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()).ToString() == _Point.Devid)//未改变设备类型时，才从原来缓存中加载  20180601
                    {
                        temp = DEFServiceModel.QueryPointByChannelInfs((Int16)_SourceNum, Convert.ToInt16(CcmbChannelNum.Text), Convert.ToInt16(CcmbAddressNum.Text),
                            Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
                    }
                }
                if (temp == null)
                {
                    temp = new Jc_DefInfo();
                }
                temp.Fzh = (Int16)_SourceNum; //分站号
                temp.Kh = Convert.ToInt16(CcmbChannelNum.Text);//通道号
                temp.Dzh = Convert.ToInt16(CcmbAddressNum.Text);//地址号

                temp.Bz12 = MultipleCount.Text;//赋值参数个数  20170415
                //赋值传感器通信类型  20170612
                if (SensorCommunicationType.Text == "智能型")
                {
                    temp.Bz18 = "0";
                }
                else
                {
                    temp.Bz18 = "1";
                }
                //赋值脉冲量计数
                if (CcmbDvType.Text.Contains("脉冲"))
                {
                    temp.Bz20 = "1";
                }

                temp.DevPropertyID = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.')));//设备性质ID
                temp.DevProperty = CcmbDvProPerty.Text.Substring(CcmbDvProPerty.Text.IndexOf('.') + 1);//设备性质种类
                temp.DevClassID = 0;//设备种类ID 
                temp.DevClass = "";//设备种类名称 
                temp.Devid = Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()).ToString();//设备类型ID
                temp.DevName = CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Description.ToString();//设备类型名称
                temp.DevModelID = 0;//设备型号ID 
                temp.DevModel = "";//设备型号名称 
                Dictionary<int, EnumcodeInfo> tempDevClass = Model.DEVServiceModel.QueryDevClasiessCache();//设备种类
                Dictionary<int, EnumcodeInfo> tempDevModel = Model.DEVServiceModel.QueryDevMoelsCache();//设备型号
                Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
                if (null != tempDev)
                {
                    if (null != tempDevClass)
                    {
                        if (tempDevClass.ContainsKey(tempDev.Bz3))
                        {
                            temp.DevClassID = tempDev.Bz3;//设备种类ID 
                            temp.DevClass = tempDevClass[tempDev.Bz3].StrEnumDisplay;//设备种类名称 
                        }
                    }
                    if (null != tempDevModel)
                    {
                        if (tempDevModel.ContainsKey(tempDev.Bz4))
                        {
                            temp.DevModelID = tempDev.Bz4;//设备型号ID 
                            temp.DevModel = tempDevModel[tempDev.Bz4].StrEnumDisplay;//设备型号名称 
                        }
                    }
                }
                temp.Wzid = tempWz.WzID;//位置ID
                temp.Wz = tempWz.Wz;//位置ID
                temp.Csid = 0; //措施ID
                temp.Point = CtxbArrPoint.Text;//测点名称（别名）
                temp.Bz1 = 1;//运行记录标记 默认都写成1
                temp.Bz2 = 1;//语音报警标记 默认都写成1
                temp.Bz3 = 1;//突出预测标记 默认都写成1
                temp.Bz4 = 0x01;//定义状态标记 默认密采勾选
                if (CcmbDefineState.Text == "运行")
                {
                    temp.Bz4 |= 0x00;
                }
                else if (CcmbDefineState.Text == "休眠")
                {
                    temp.Bz4 |= 0x02;
                }
                else if (CcmbDefineState.Text == "检修")
                {
                    temp.Bz4 |= 0x04;
                }
                else if (CcmbDefineState.Text == "标校")
                {
                    temp.Bz4 |= 0x08;
                }
                if (CpDocument.Controls.Count > 0)
                {
                    bool resultCpCocument = ((CuBase)(CpDocument.Controls[0])).ConfirmInf(temp); //传入传感器对象 赋值属性集  20170622
                    //返回false 表示数据验证不合法  20170622
                    if (!resultCpCocument)
                    {
                        return;
                    }
                }
                if (temp.Dzh > 0)
                {
                    CcmbDvProPerty.Properties.ReadOnly = true;
                    CcmbDvType.Properties.ReadOnly = true;
                }
                else
                {
                    CcmbDvProPerty.Properties.ReadOnly = false;
                    CcmbDvType.Properties.ReadOnly = false;
                }
                temp.State = 46;//对于变化传感器增加默认状态
                temp.DataState = 46;//对于变化传感器增加默认状态

                #region//增加传感器坐标信息及所属区域信息保存  20170829
                if (string.IsNullOrEmpty(PointAreaId))
                {
                    temp.Areaid = null;
                }
                else
                {
                    temp.Areaid = PointAreaId;
                }
                if (!string.IsNullOrEmpty(txt_Coordinate.Text))
                {
                    string coordinateX = txt_Coordinate.Text.Split(',')[0];
                    string coordinateY = txt_Coordinate.Text.Split(',')[1];
                    temp.XCoordinate = coordinateX;
                    temp.YCoordinate = coordinateY;

                    if (!string.IsNullOrEmpty(PointInGraphId))
                    {
                        //将图形信息添加到图形测点表中
                        GraphicspointsinfInfo graphpointInfo = new GraphicspointsinfInfo();
                        graphpointInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        graphpointInfo.GraphId = PointInGraphId;
                        graphpointInfo.PointID = temp.PointID;
                        graphpointInfo.Point = temp.Point;
                        //判断，如果是识别器，则保存识别器的图元
                        if (temp.DevPropertyID == 7)
                        {
                            graphpointInfo.GraphBindName = "识别器实时显示";
                        }
                        else
                        {
                            graphpointInfo.GraphBindName = "实时显示";
                        }
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

                //传感器所属地点类型保存
                if (!string.IsNullOrEmpty(comboAddresstype.Text))
                {
                    KJ_AddresstypeInfo addresstype = AddresstypeInfoListAll.Find(a => a.Addresstypename == comboAddresstype.Text);
                    if (addresstype != null)
                    {
                        temp.Addresstypeid = addresstype.ID;

                        //判断门限定义异常
                        KJ_AddresstyperuleInfo addresstyperuleInfo = addresstyperuleService.GetKJ_AddresstyperuleListByAddressTypeId(
                            new Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest() { Id = addresstype.ID }).Data.Find(a => a.Devid == temp.Devid);
                        if (addresstyperuleInfo != null)
                        {
                            if (temp.Z2 < addresstyperuleInfo.UpAlarmLowValue || temp.Z2 > addresstyperuleInfo.UpAlarmHighValue)
                            {
                                XtraMessageBox.Show("传感器上限报警门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (temp.Z3 < addresstyperuleInfo.UpPoweroffLowValue || temp.Z3 > addresstyperuleInfo.UpPoweroffHighValue)
                            {
                                XtraMessageBox.Show("传感器上限断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (temp.Z6 < addresstyperuleInfo.LowAlarmLowValue || temp.Z6 > addresstyperuleInfo.LowAlarmHighValue)
                            {
                                XtraMessageBox.Show("传感器下限报警门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (temp.Z7 < addresstyperuleInfo.LowPoweroffLowValue || temp.Z7 > addresstyperuleInfo.LowPoweroffHighValue)
                            {
                                XtraMessageBox.Show("传感器下限断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    else
                    {
                        temp.Addresstypeid = "0";
                    }

                }
                else
                {
                    temp.Addresstypeid = "0";
                }
                //生产日期及唯一编码生成
                if (!string.IsNullOrEmpty(dateEdit1.Text))
                {
                    DateTime time = new DateTime(2018, 01, 01);
                    DateTime.TryParse(dateEdit1.Text, out time);
                    temp.Bz15 = time.ToString("yyyy-MM-dd");
                    List<Jc_DefInfo> alldefInfoList = Model.DEFServiceModel.QueryAllCache().ToList().FindAll(a => a.Bz15 == temp.Bz15);
                    int indexCode = 1;
                    string code = "";
                    while (true)
                    {
                        code = time.ToString("yyyyMMdd") + indexCode.ToString("0000");
                        if (alldefInfoList.FindIndex(a => a.Bz16 == code) < 0)
                        {
                            break;
                        }
                        indexCode++;
                    }
                    temp.Bz16 = code;
                }

                //判断T0~T8门限定义异常  20180309
                //if ((temp.Wz.ToLower().Contains("t1") || temp.Wz.ToLower().Contains("t0") || temp.Wz.ToLower().Contains("t5")) && (temp.Z2 > 1.0 || temp.Z3 > 1.5))
                //{
                //    XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if ((temp.Wz.ToLower().Contains("t2") || temp.Wz.ToLower().Contains("t6") || temp.Wz.ToLower().Contains("t8")) && (temp.Z2 > 1.0 || temp.Z3 > 1.0))
                //{
                //    XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if (temp.Wz.ToLower().Contains("t4") && (temp.Z2 > 0.5 || temp.Z3 > 0.5))
                //{
                //    XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if (temp.Wz.ToLower().Contains("t3") && (temp.Z2 > 1.5 || temp.Z3 > 1.5))
                //{
                //    XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //if ((temp.Wz.ToLower().Contains("t7")) && (temp.Z2 > 2.5 || temp.Z3 > 2.5))
                //{
                //    XtraMessageBox.Show("传感器报警/断电门限设置异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}


                if (null == _Point) //表示新增
                {
                    ////除控制量多参数外其它多参数传感器只能定义4个  20170503                  
                    int TempDzh = Convert.ToInt16(CcmbAddressNum.Text);
                    //if (TempDzh > 0 && (temp.DevPropertyID == 1 || temp.DevPropertyID == 2))
                    //{
                    //    int MultCount = 0;
                    //    //计算非控制量多参数  20170622
                    //    for (int i = 1; i <= 16; i++)
                    //    {
                    //        List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.Dzh > 0);
                    //        if (MultList.Count > 0)
                    //        {
                    //            List<Jc_DefInfo> MultListControl = MultList.FindAll(a => a.DevPropertyID == 3);
                    //            if (MultListControl.Count < 1)
                    //            {
                    //                MultCount++;
                    //            }
                    //        }
                    //    }
                    //    if (MultCount >= 4)
                    //    {
                    //        XtraMessageBox.Show("除控制量多参数外最多只能定义4个其它多参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        CcmbDvType.SelectedIndex = 0;
                    //        return;
                    //    }
                    //}
                    #region//判断控制量最多只能定义8个  20170615
                    if (temp.DevPropertyID == 3)
                    {
                        int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
                        if (controlPointCount >= 8)
                        {
                            XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CcmbDvType.SelectedIndex = 0;
                            return;
                        }
                    }
                    //多参数中包含控制量的情况判断
                    Jc_DevInfo tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid.ToString());
                    TempDzh = Convert.ToInt16(CcmbAddressNum.Text);
                    if (TempDzh > 0 && temp.DevPropertyID == 3)
                    {

                        if (!string.IsNullOrEmpty(tempDevInfo.Bz9))
                        {
                            string[] tempDevIdList = tempDevInfo.Bz9.Split(',');
                            foreach (string devid in tempDevIdList)
                            {
                                tempDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(devid);
                                if (tempDevInfo.Type == 3)
                                { //副通道中包含控制量
                                    int controlPointCount = FzList.FindAll(a => a.DevPropertyID == 3).Count;
                                    if (controlPointCount >= 8)
                                    {
                                        XtraMessageBox.Show("系统最多支持定义8个控制量设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        CcmbDvType.SelectedIndex = 0;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region//基础通道1-16只能定义8个参量的多参数（4参数最多2个，2参最多4个）
                    int defineNowCount = 0;//当前定义参量数
                    if (tempDevInfo != null)
                    {
                        if (!string.IsNullOrEmpty(tempDevInfo.Bz8) && int.Parse(tempDevInfo.Bz8) > 1)
                        {
                            defineNowCount = int.Parse(tempDevInfo.Bz8);
                        }
                    }
                    //Jc_DevInfo tempMultDevInfo = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid.ToString());
                    //int definedParamCount = 0;//已定义参量数
                    //for (int i = 1; i <= 16; i++)
                    //{
                    //    List<Jc_DefInfo> MultList = FzList.FindAll(a => a.Kh == i && a.DevPropertyID == 1);//只判断模拟量多参数
                    //    if (MultList.Count > 1)
                    //    {
                    //        definedParamCount += MultList.Count;
                    //    }
                    //}
                    //if (definedParamCount + defineNowCount > 8)
                    //{
                    //    XtraMessageBox.Show("多参数传感器最多只能定义8个参量（2参4个，4参2个）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}
                    if (defineNowCount > 0)
                    {
                        List<Jc_DefInfo> MultList = FzList.FindAll(a => (a.DevPropertyID == 1 || a.DevPropertyID == 2) && a.Dzh == 1);//只判断模拟量多参数
                        if (MultList.Count >= 8)
                        {
                            XtraMessageBox.Show("多参数传感器最多只能定义8个！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    #endregion


                    //判断测点重复  20170318
                    Jc_DefInfo tempdef = DEFServiceModel.QueryPointByCodeCache(temp.Point);
                    if (tempdef != null)
                    {
                        XtraMessageBox.Show("已定义了该设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    temp.PointID = temp.ID;
                    temp.Activity = "1";
                    temp.InfoState = InfoState.AddNew;
                    temp.CreateUpdateTime = DateTime.Now;
                    temp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");

                    PersonSpecialHand(temp);//人员定位特殊处理  20171128

                    //加入馈电开停  20170621
                    AddFeedDrive(temp);
                    //添加测点
                    try
                    {
                        if (!DEFServiceModel.AddDEFCache(temp))
                        {
                            XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //写操作日志 
                    OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(_Point), "");// 20160111
                    TempDevid = long.Parse(temp.Devid);

                    relateAdd(TempDevid, temp);//关联添加   //多参数通道由用户自定义  20170415
                }
                else  //表示更新
                {
                    if (temp != _Point)
                    {
                        if (temp.Devid != _Point.Devid || temp.Bz12 != _Point.Bz12)//设备类型不同时为新增 之前点在数据库置非活动点
                        {
                            try
                            {
                                //_Point.Activity = "0";
                                //_Point.DeleteTime = DateTime.Now;
                                //_Point.InfoState = InfoState.Modified;
                                //DEFServiceModel.UpdateDEFCache(_Point);
                                //如果当前测点已经复制，则清除  20170503
                                if (Model.CopyInf.CopySensor != null)
                                {
                                    if (Model.CopyInf.CopySensor.Fzh == _Point.Fzh && Model.CopyInf.CopySensor.Kh == _Point.Kh)
                                    {
                                        Model.CopyInf.CopySensor = null;
                                    }
                                }
                                DeletePoint(_Point);



                                temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                                temp.PointID = temp.ID;
                                temp.CreateUpdateTime = DateTime.Now;
                                temp.Activity = "1";
                                temp.InfoState = InfoState.AddNew;
                                temp.CreateUpdateTime = DateTime.Now;
                                temp.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");

                                //加入馈电开停  20170621
                                AddFeedDrive(temp);

                                DEFServiceModel.AddDEFCache(temp);

                                OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20160111

                                TempDevid = long.Parse(temp.Devid);
                                relateAdd(TempDevid, temp);//关联添加   //多参数通道由用户自定义  20170415

                                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627

                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            //OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(_Point), "");// 20160111
                            //OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.AddOrDelPointLog(temp), "");// 20160111
                        }
                        else
                        {
                            temp.ID = _Point.ID;
                            temp.PointID = _Point.PointID;
                            temp.State = _Point.State;//对于变化传感器赋值成原来的设备状态
                            temp.DataState = _Point.DataState;//对于变化传感器赋值成原来的数据状态
                            temp.Ssz = _Point.Ssz; ;//对于变化传感器赋值成原来的实时值
                            temp.CreateUpdateTime = _Point.CreateUpdateTime;
                            temp.Activity = "1";



                            OperateLogHelper.InsertOperateLog(1, CONFIGServiceModel.UpdatePointLog(_Point, temp), "");// 20160111

                            temp.InfoState = InfoState.Modified;
                            try
                            {
                                if (!DEFServiceModel.UpdateDEFCache(temp))
                                {
                                    XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                else
                                {
                                    XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //保存成功时，才更新内存对象  20170627
                                    _Point = Basic.Framework.Common.ObjectConverter.DeepCopy(temp);//保存成功后，赋值内存对象  20170627
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("定义无变化！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //加延时  20170721
                Thread.Sleep(1000);
                InitChannleTreeData();
                CTreeListChanne.RefreshDataSource();

            }
            catch (Exception ex)
            {
                LogHelper.Error("确认保存【Cbtn_Confirm_Click】", ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(this.CtxbArrPoint.Text);
                if (null == temp)
                {
                    return;
                }
                //删除验证  20170621
                if (!DeleteCheck(temp))
                {
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(this.CtxbArrPoint.Text))
                    {

                        //如果当前测点已经复制，则清除  20170503
                        if (Model.CopyInf.CopySensor != null)
                        {
                            if (Model.CopyInf.CopySensor.Fzh == temp.Fzh && Model.CopyInf.CopySensor.Kh == temp.Kh)
                            {
                                Model.CopyInf.CopySensor = null;
                            }
                        }


                        if (DeletePoint(temp))
                        {
                            XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //加延时  20170721
                        Thread.Sleep(1000);
                        LoadBasicInf();
                        InitChannleTreeData();
                        CTreeListChanne.RefreshDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("确认删除【CbntDelete_Click】", ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void areaSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string areaName = areaSelect.Text;
            AreaInfo area = AreaListAll.Find(a => a.Areaname == areaName);
            if (area != null)
            {
                PointAreaId = area.Areaid;
            }
            else
            {
                PointAreaId = "";
            }
        }

        private void comboAddresstype_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CuBase temp = (CuBase)CpDocument.Controls[0];
            //if (_Point != null)
            //{
            //    if (_Point.Devid != CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString())
            //    {//如果设备类型改变,则不传原来定义的测点进行加载,重新按新的测点加载数据  20170726
            //        temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), null);
            //    }
            //    else
            //    {
            //        if (temp._arrPoint != _Point.Point)
            //        {
            //            temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), _Point);
            //        }
            //    }
            //}
            //else
            //{
            //    temp.DevTypeChanngeEvent(Convert.ToInt32(CcmbDvType.Properties.Items[CcmbDvType.SelectedIndex].Value.ToString()), null);
            //}
        }

        private void 删除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == CTreeListChanne.FocusedNode)
            {
                return;
            }
            TreeListItem DataRecord = (TreeListItem)CTreeListChanne.GetDataRecordByNode(CTreeListChanne.FocusedNode);
            if (null == DataRecord)
            {
                return;
            }

            try
            {
                //判断当前电脑是否为主控  2070504
                if (!CONFIGServiceModel.GetClinetDefineState())
                {
                    XtraMessageBox.Show("当前电脑没有操作权限，请与管理员联系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(DataRecord.Code);
                if (null == temp)
                {
                    return;
                }
                //删除验证  20170621
                if (!DeleteCheck(temp))
                {
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复，并且将清除复制，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(DataRecord.Code))
                    {

                        //如果当前测点已经复制，则清除  20170503
                        if (Model.CopyInf.CopySensor != null)
                        {
                            if (Model.CopyInf.CopySensor.Fzh == temp.Fzh && Model.CopyInf.CopySensor.Kh == temp.Kh)
                            {
                                Model.CopyInf.CopySensor = null;
                            }
                        }


                        if (DeletePoint(temp))
                        {
                            XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //加延时  20170721
                        Thread.Sleep(1000);
                        LoadBasicInf();
                        InitChannleTreeData();
                        CTreeListChanne.RefreshDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("确认删除【CbntDelete_Click】", ex);
            }
        }

    }
}
