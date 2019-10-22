using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Define.Station
{
    public partial class StationWindPowerLock : XtraForm
    {
        //<summary>
        //当前的分站窗体
        //</summary>
        private CFStation tempStationForm;
        /// <summary>
        /// 风电闭锁字节
        /// </summary>
        private string WindBreakBytes;

        /// <summary>
        /// 风电闭锁条件
        /// </summary>
        private string WindBreakCondition;
        /// <summary>
        /// 分站对象
        /// </summary>
        private Jc_DefInfo tempStation = null;

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
        IntPtr hwnd,
        int wMsg,
        int wParam,
        int lParam
        );
        Hashtable htCmd = new Hashtable();//保存命令
        Hashtable htParam = new Hashtable();//保存参数
        int nViewCmdIndex = 0;//索引

        public StationWindPowerLock(Jc_DefInfo _tempStation, CFStation _tempStationForm, string _WindBreakBytes, string _WindBreakCondition)
        {
            tempStation = _tempStation;
            tempStationForm = _tempStationForm;
            if (!_WindBreakCondition.Contains(":"))//包含:表示是老风电闭锁的配置
            {
                WindBreakCondition = _WindBreakCondition;
                WindBreakBytes = _WindBreakBytes;
            }
            InitializeComponent();
            //增加服务授权设置  20171026
            mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
            mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
        }

        private void StationWindPowerLock_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                StationWindPowerLockInit();//初始化控件

                MapLoad();//加载画布
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 控件初始化
        /// </summary>
        private void StationWindPowerLockInit()
        {
            try
            {
                //测点选择初始化 
                List<Jc_DefInfo> TempAnalog = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 1);
                TempAnalog = TempAnalog.OrderBy(a => a.Point).ToList();
                if (TempAnalog != null)
                {
                    if (TempAnalog.Count > 0)
                    {
                        foreach (Jc_DefInfo item in TempAnalog)
                        {
                            if (item.Unit == "%")//只加载甲烷传感器 
                            {
                                T1Combox.Properties.Items.Add(item.Point.ToString() + "." + item.Wz + "[" + item.DevName + "]");
                                T2Combox.Properties.Items.Add(item.Point.ToString() + "." + item.Wz + "[" + item.DevName + "]");
                            }
                        }
                    }
                }
                List<Jc_DefInfo> TempControl = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 3);
                TempControl = TempControl.OrderBy(a => a.Point).ToList();
                if (TempControl != null)
                {
                    if (TempControl.Count > 0)
                    {
                        foreach (Jc_DefInfo item in TempControl)
                        {
                            string PointName = item.Point.ToString() + "." + item.Wz + "[" + item.DevName + "]";
                            ControlWindBreakCH4.Properties.Items.Add(PointName);
                            ControlWindBreak.Properties.Items.Add(PointName);

                            if (item.Kh == 1 || item.Kh == 2) //地址号为1-2的智能断电器不能作为控制口
                            {
                                if (item.Dzh > 0)
                                {
                                    ControlWindBreakCH4.Properties.Items[PointName].Enabled = false;
                                    ControlWindBreak.Properties.Items[PointName].Enabled = false;
                                }
                            }
                            if (!Model.RelateUpdate.CheckControlEnable(item))
                            {
                                ControlWindBreakCH4.Properties.Items[PointName].Enabled = false;
                                //ControlWindBreak.Properties.Items[PointName].Enabled = false;//风电闭锁口可与本地控制口相同  20170919
                            }
                            List<Jc_JcsdkzInfo> TempJCJCSDKZDTO = Model.JCSDKZServiceModel.QueryJCSDKZbyInf(item.Point);
                            if (null != TempJCJCSDKZDTO)
                            {
                                if (TempJCJCSDKZDTO.Count > 0)
                                {
                                    ControlWindBreakCH4.Properties.Items[PointName].Enabled = false;
                                    //ControlWindBreak.Properties.Items[PointName].Enabled = false;//风电闭锁口可与本地控制口相同  20170919
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 编辑加载
        /// </summary>
        private void StationWindPowerLockEditLoad()
        {
            try
            {
                string[] WindBreakConditionArr = WindBreakCondition.Split('|');
                string[] T1T2PointArr = WindBreakConditionArr[0].Split(',');//T1 T2
                string[] WindPowerLockPointMain = WindBreakConditionArr[1].Split('&')[0].Split(',');//1#号配电柜所有开关量
                string[] WindPowerLockPointBack = WindBreakConditionArr[1].Split('&')[1].Split(',');//2#号配电柜所有开关量
                string CH4WindPowerLockControlPoint = WindBreakConditionArr[2].Split('&')[0];//瓦斯风电闭锁控制口
                string WindPowerLockControlPoint = WindBreakConditionArr[2].Split('&')[1];//风电闭锁控制口
                string KTStopState = "";
                if (WindBreakConditionArr.Length > 3)
                {
                    KTStopState = WindBreakConditionArr[3];
                    stopStateCmb.Text = KTStopState;
                }
                //加载T1已选择
                for (int i = 0; i < T1Combox.Properties.Items.Count; i++)
                {
                    if (T1Combox.Properties.Items[i].ToString().Contains(T1T2PointArr[0]))
                    {
                        T1Combox.SelectedIndex = i;
                    }
                }
                //加载T2已选择
                for (int i = 0; i < T2Combox.Properties.Items.Count; i++)
                {
                    if (T2Combox.Properties.Items[i].ToString().Contains(T1T2PointArr[1]))
                    {
                        T2Combox.SelectedIndex = i;
                    }
                }
                //加载瓦斯风电闭锁控制口已选择
                for (int i = 0; i < ControlWindBreakCH4.Properties.Items.Count; i++)
                {
                    if (CH4WindPowerLockControlPoint.Contains(ControlWindBreakCH4.Properties.Items[i].ToString().Substring(0, 7)))
                    {
                        ControlWindBreakCH4.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }
                //加载风电闭锁控制口已选择
                for (int i = 0; i < ControlWindBreak.Properties.Items.Count; i++)
                {
                    if (WindPowerLockControlPoint.Contains(ControlWindBreak.Properties.Items[i].ToString().Substring(0, 7)))
                    {
                        ControlWindBreak.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }

                List<Jc_DefInfo> TempSwitch = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 2);
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页
                //加载1#号配电柜开关量            
                foreach (string Point in WindPowerLockPointMain)
                {
                    Jc_DefInfo TempPoint = TempSwitch.Find(a => a.Point == Point);
                    string scriptCmd = "AddWindPowerAtresiaPoint('1#','" + TempPoint.Point + "','" + TempPoint.Wz + "','" + TempPoint.DevName + "')";
                    //view.EvaluateJavaScript(scriptCmd);
                    mx.CurViewEvaluateJavaScript(scriptCmd);
                }
                //加载2#号配电柜开关量
                foreach (string Point in WindPowerLockPointBack)
                {
                    Jc_DefInfo TempPoint = TempSwitch.Find(a => a.Point == Point);
                    string scriptCmd = "AddWindPowerAtresiaPoint('2#','" + TempPoint.Point + "','" + TempPoint.Wz + "','" + TempPoint.DevName + "')";
                    //view.EvaluateJavaScript(scriptCmd);
                    mx.CurViewEvaluateJavaScript(scriptCmd);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 加载画布
        /// </summary>
        public void MapLoad()
        {
            try
            {
                string path = Application.StartupPath;
                mx.SetWebRootPath(path + "\\");//设置根目录
                mx.SetMapScriptPath("mx/");//设置元图脚本API路径，相对于根目录地址
                string dwgFileName = "";
                dwgFileName = "width:1000;height:800";
               
                mx.OpenDwg(dwgFileName, false, "");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// T1选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1Combox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(T1Combox.Text))
                {
                    return;
                }
                T1SelPoint.Text = T1Combox.Text.Substring(0, T1Combox.Text.IndexOf("."));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// T2选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2Combox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(T2Combox.Text))
                {
                    return;
                }
                T2SelPoint.Text = T2Combox.Text.Substring(0, T2Combox.Text.IndexOf("."));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
        {
            String strCmd = e.p_sCmd;
            String strParam = e.p_sParam;
            nViewCmdIndex = nViewCmdIndex + 1;
            if (nViewCmdIndex >= Int32.MaxValue - 1)
            {
                nViewCmdIndex = 1;
            }
            htCmd.Add(nViewCmdIndex, strCmd);
            htParam.Add(nViewCmdIndex, strParam);

            PostMessage(this.Handle, 0x0401, nViewCmdIndex, 0);
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0401:
                    int nIndex = (int)m.WParam;
                    if (htCmd.Contains(nIndex) && htParam.Contains(nIndex))
                    {
                        ProcessViewCallOutCommand(htCmd[nIndex].ToString(), htParam[nIndex].ToString());
                        htCmd.Remove(nIndex);
                        htParam.Remove(nIndex);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void ProcessViewCallOutCommand(String strCmd, String strParam)
        {
            try
            {
                string Param = "";
                switch (strCmd)
                {
                    case "MessagePub"://测试交互代码
                        DevExpress.XtraEditors.XtraMessageBox.Show(strParam);
                        break;
                    case "PointEdit"://图形测点编辑  
                        Param = strParam;
                        string Point = strParam.ToString().Split('|')[0];
                        if (!Point.Contains("#"))//配电箱不能编辑
                        {
                            WindPowerLockPointBind _WindPowerLockPointBind = new WindPowerLockPointBind(tempStation.Fzh, Point, this);
                            //_WindPowerLockPointBind.Show();
                            _WindPowerLockPointBind.ShowDialog();//2017.12.20 by
                        }
                        break;
                    case "PointDel"://图形测点删除                    
                        break;
                    case "PointsSave"://图形测点保存 
                        break;
                    case "SaveWindAtresia"://保存风电闭锁信息
                        Param = strParam;
                        string[] strPointWindAtresia = Param.Split('|');
                        List<string> PointWindAtresia1 = strPointWindAtresia[0].Split(',').ToList();
                        List<string> PointWindAtresia2 = strPointWindAtresia[1].Split(',').ToList();
                        SaveWindAtresia(PointWindAtresia1, PointWindAtresia2);
                        break;
                    case "RoutesSave"://保存测点连线
                        break;
                    case "LoadPoint"://加载图形绑定的测点信息
                        LoadDistributionGraphic();//加载配电柜
                        break;
                    case "SetMapEditState"://设置图形的可编辑状态
                        break;
                    case "setMapEditSave"://设置图形是否保存                   
                        break;
                    case "SetMapTopologyInit"://拓扑图初始化所有井上设备                    
                        break;
                    case "pageToImg":
                        break;
                    case "layerDis":
                        break;
                    case "pointDis":
                        break;
                    case "pointSercah":
                        break;
                    case "PageChange":
                        break;
                    case "AddMapRightMenu":
                        break;
                    case "DoRefPointSsz":
                        break;
                    case "PointDblClick":
                        break;
                    case "MapLoadEndEvent":
                        //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                        //if (view == null) return;//如果还没有打开图形或网页  
                        string scriptCmd1 = "map.setZoom(6);map.setZoomRange(6,6,6,6);SetGraphicEdit(true);";
                        string scriptCmd2 = "UsersetMapCenter('250', '540');map.removeControl(ovctrl);";
                        //view.EvaluateJavaScript(scriptCmd1);
                        mx.CurViewEvaluateJavaScript(scriptCmd1);
                        //view.EvaluateJavaScript(scriptCmd2);
                        mx.CurViewEvaluateJavaScript(scriptCmd2);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 加载配电柜
        /// </summary>
        private void LoadDistributionGraphic()
        {
            try
            {
                string PointListStr = "";
                PointListStr = PointListStr + @"1#|||风电闭锁配电柜|1|0|0|0.0|720|165|60|1|,";
                PointListStr = PointListStr + @"2#|||风电闭锁配电柜|1|0|0|350.0|720|165|60|1|";

                string scriptCmd = "LoadPoint('" + PointListStr + "')";
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页
                //view.EvaluateJavaScript(scriptCmd);
                mx.CurViewEvaluateJavaScript(scriptCmd);

                if (WindBreakCondition != null)
                {
                    if (WindBreakCondition.Length > 0)//加载完配电柜后，再加载子集设备及选择绑定
                    {
                        StationWindPowerLockEditLoad();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 设置图元绑定的测点名称
        /// </summary>
        /// <param name="PointName"></param>
        /// <param name="SetPointName"></param>
        public void SetPointInMap(string PointName, string SetPointName, string SetPointWz, string SetPointDevName)
        {
            try
            {
                string scriptCmd = "SetTBPointName('" + PointName + "', '" + SetPointName + "', '" + SetPointWz + "', '" + SetPointDevName + "')";
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页
                //view.EvaluateJavaScript(scriptCmd);
                mx.CurViewEvaluateJavaScript(scriptCmd);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 保存风电闭锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string scriptCmd = "SaveWindAtresia()";
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页
                //view.EvaluateJavaScript(scriptCmd);
                mx.CurViewEvaluateJavaScript(scriptCmd);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 取消并退出页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MapLoad();
        }
        /// <summary>
        /// 保存风电闭锁信息
        /// </summary>
        /// <param name="PointWindAtresiaMain"></param>
        /// <param name="PointWindAtresiaBack"></param>
        private void SaveWindAtresia(List<string> PointWindAtresiaMain, List<string> PointWindAtresiaBack)
        {
            try
            {
                string WindAtresiaPointString = "";//风电闭锁绑点的测点列表 
                string WindAtresiaByteString = "";//风电闭锁生成的命令集合（新协议）
                string WindAtresiaByteStringOld = "";//风电闭锁生成的命令集合（老协议）

                #region //验证
                if (PointWindAtresiaMain.Count < 1 || PointWindAtresiaBack.Count < 1)
                {
                    XtraMessageBox.Show("1# 2# 配电柜至少要定义一个开停设备!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                //if (string.IsNullOrEmpty(ControlWindBreak.Text))
                //{
                //    XtraMessageBox.Show("风电闭锁区域未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //    return;
                //}
                if (!string.IsNullOrEmpty(ControlWindBreakCH4.Text))
                {
                    if (!string.IsNullOrEmpty(ControlWindBreak.Text))
                    {
                        string[] ControlWindBreakCH4List = ControlWindBreakCH4.Text.Split(',');
                        string[] ControlWindBreakList = ControlWindBreak.Text.Split(',');
                        bool isContent=false;
                        foreach (string tempPoint1 in ControlWindBreakCH4List)
                        {
                            foreach (string tempPoint2 in ControlWindBreakList)
                            {
                                if (tempPoint2 == tempPoint1) {
                                    isContent = true;
                                }
                            }
                        }
                        if (isContent)
                        {
                            XtraMessageBox.Show("甲烷风电闭锁区域 和 风电闭锁区域 不能相同!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                }
                if ( (T1Combox.Text == T2Combox.Text) && !string.IsNullOrEmpty(T1Combox.Text) && !string.IsNullOrEmpty(T2Combox.Text))
                {
                    XtraMessageBox.Show("工作面T1,T2不能选择同一设备!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                //if (string.IsNullOrEmpty(T1Combox.Text) || string.IsNullOrEmpty(T2Combox.Text))
                //{
                //    XtraMessageBox.Show("请选择T1 T2!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //    return;
                //}
                if (string.IsNullOrEmpty(stopStateCmb.Text))
                {
                    XtraMessageBox.Show("请选择风机开停传感器停对应的状态!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                #endregion

                string T1BindPoint = "";
                if (!string.IsNullOrEmpty(T1Combox.Text))
                {
                    T1BindPoint = T1Combox.Text.Substring(0, T1Combox.Text.IndexOf('.'));
                }
                string T2BindPoint = "";
                if (!string.IsNullOrEmpty(T2Combox.Text))
                {
                    T2BindPoint = T2Combox.Text.Substring(0, T2Combox.Text.IndexOf('.'));
                }

                List<string> WindBreakControlPoint = new List<string>();
                string[] WindBreakControlPointArr = ControlWindBreak.Text.Split(',');
                foreach (string Point in WindBreakControlPointArr)
                {
                    if (Point.Contains("."))
                    {
                        WindBreakControlPoint.Add(Point.Substring(0, Point.IndexOf('.')));
                    }
                }
                List<string> CH4WindBreakControlPoint = new List<string>();
                string[] CH4WindBreakControlPointArr = ControlWindBreakCH4.Text.Split(',');
                foreach (string Point in CH4WindBreakControlPointArr)
                {
                    if (Point.Contains("."))
                    {
                        CH4WindBreakControlPoint.Add(Point.Substring(0, Point.IndexOf('.')));
                    }
                }
                #region 赋值所有已选择设备
                WindAtresiaPointString += T1BindPoint + "," + T2BindPoint + "|";//T1 T2
                string WindAtresiaMainStr = "";//1#配电柜开停
                foreach (string Point in PointWindAtresiaMain)
                {
                    WindAtresiaMainStr += Point + ",";
                }
                WindAtresiaPointString += WindAtresiaMainStr.TrimEnd(',') + "&";//1# 2# 配电柜绑定的开停中间用&分隔
                string WindAtresiaBackStr = "";//2#配电柜开停
                foreach (string Point in PointWindAtresiaBack)
                {
                    WindAtresiaBackStr += Point + ",";
                }
                WindAtresiaPointString += WindAtresiaBackStr.TrimEnd(',') + "|";
                string CH4WindBreakControlStr = "";//甲烷风电闭锁控制口
                foreach (string Point in CH4WindBreakControlPoint)
                {
                    CH4WindBreakControlStr += Point + ",";
                }
                WindAtresiaPointString += CH4WindBreakControlStr.TrimEnd(',') + "&";//甲烷风电闭锁控制与风电闭锁控制中间用&分隔
                string WindBreakControlStr = "";//风电闭锁控制口
                foreach (string Point in WindBreakControlPoint)
                {
                    WindBreakControlStr += Point + ",";
                }
                WindAtresiaPointString += WindBreakControlStr.TrimEnd(',') + "";
                //风机停对应状态保存  20170922
                if (!string.IsNullOrEmpty(stopStateCmb.Text))
                {
                    WindAtresiaPointString += "|" + stopStateCmb.Text;
                }
                #endregion

                int KTStopState = 1;//开停的停状态对应的状态值
                if (stopStateCmb.Text == "2态")
                {
                    KTStopState = 2;
                }
                else if (stopStateCmb.Text == "0态")
                {
                    KTStopState = 0;
                }
                else
                {
                    KTStopState = 1;
                }

                #region 老协议(32个字节)
                #region//单双风机标记
                if (PointWindAtresiaMain.Count == 1)
                {
                    WindAtresiaByteStringOld += "1,";
                }
                else if (PointWindAtresiaMain.Count == 2)
                {
                    WindAtresiaByteStringOld += "2,";
                }
                else if (PointWindAtresiaMain.Count > 2)
                {
                    WindAtresiaByteStringOld += "3,";
                }
                #endregion
                #region//甲烷风电闭锁控制口
                byte CH4WindBreakControlChannelHighBit = 0;
                byte CH4WindBreakControlChannelLowBit = 0;
                string[] CH4WindBreakControlChannel = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                foreach (string Point in CH4WindBreakControlPoint)
                {
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    CH4WindBreakControlChannel[16 - ChannelIndex] = "1";
                }
                CH4WindBreakControlChannelLowBit = (byte)Convert.ToInt16(string.Join("", CH4WindBreakControlChannel), 2);
                CH4WindBreakControlChannelHighBit = (byte)(Convert.ToInt16(string.Join("", CH4WindBreakControlChannel), 2) >> 8);
                WindAtresiaByteStringOld += CH4WindBreakControlChannelLowBit + ",";
                #endregion
                #region//风电闭锁解锁条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前全是或关系
                #endregion
                #region//主控通道T1口号及判断条件
                int T1ChannelNumber = 0;
                if (!string.IsNullOrEmpty(T1BindPoint))
                {
                    T1ChannelNumber = (byte)(3 << 5) + byte.Parse(T1BindPoint.Trim().Substring(4, 2));//闭锁条件默认是 011 >=
                }
                WindAtresiaByteStringOld += T1ChannelNumber + ",";
                #endregion
                #region//主控通道T2口号及判断条件
                int T2ChannelNumber = 0;
                if (!string.IsNullOrEmpty(T2BindPoint))
                {
                    T2ChannelNumber = (byte)(3 << 5) + byte.Parse(T2BindPoint.Trim().Substring(4, 2));//闭锁条件默认是 011 >=
                }
                WindAtresiaByteStringOld += T2ChannelNumber + ",";
                #endregion
                #region//风筒风量1及判断条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风筒风量2及判断条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//1#配电柜 主开停地址号及判断条件、取值
                int MainKT1Point = 0;
                if (PointWindAtresiaMain.Count > 0)
                {
                    MainKT1Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaMain[0].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT1Point = 0;
                }
                WindAtresiaByteStringOld += MainKT1Point + ",";
                #endregion
                #region//2#配电柜 主开停地址号及判断条件、取值
                int MainKT3Point = 0;
                if (PointWindAtresiaBack.Count > 0)
                {
                    MainKT3Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaBack[0].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT3Point = 0;
                }
                WindAtresiaByteStringOld += MainKT3Point + ",";
                #endregion
                #region//1#配电柜 副开停地址号及判断条件、取值
                int MainKT2Point = 0;
                if (PointWindAtresiaMain.Count > 1)
                {
                    MainKT2Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaMain[1].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT2Point = 0;
                }
                WindAtresiaByteStringOld += MainKT2Point + ",";
                #endregion
                #region//2#配电柜 副开停地址号及判断条件、取值
                int MainKT4Point = 0;
                if (PointWindAtresiaBack.Count > 1)
                {
                    MainKT4Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaBack[1].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT4Point = 0;
                }
                WindAtresiaByteStringOld += MainKT4Point + ",";
                #endregion
                #region//T1甲烷闭锁值及条件
                int T1CH4ValueHighBit = 0;
                int T1CH4ValueLowBit = 0;
                int T1CH4Value = 0;
                Jc_DevInfo tempDEV = new Jc_DevInfo();
                Jc_DefInfo tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T1SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                Jc_DevInfo tempStationDev = Model.DEVServiceModel.QueryDevByDevIDCache(tempStation.Devid);
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                {
                    T1CH4Value = (int)(Convert.ToDouble(T1LockValue.Text) * 100);
                }
                else
                {
                    if (tempDEV != null)
                    {
                        if (tempDEV.Pl2 != 2000)
                        {
                            T1CH4Value = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(T1LockValue.Text));
                        }
                        else
                        {
                            T1CH4Value = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(T1LockValue.Text));
                        }
                    }
                }
                T1CH4ValueLowBit = (byte)T1CH4Value;
                T1CH4ValueHighBit = (byte)(T1CH4Value >> 8);
                WindAtresiaByteStringOld += T1CH4ValueHighBit + ",";
                WindAtresiaByteStringOld += T1CH4ValueLowBit + ",";
                #endregion
                #region//T2甲烷闭锁值及条件
                int T2CH4ValueHighBit = 0;
                int T2CH4ValueLowBit = 0;
                int T2CH4Value = 0;
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T2SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                {
                    T2CH4Value = (int)(Convert.ToDouble(T2LockValue.Text) * 100);
                }
                else
                {
                    if (tempDEV != null)
                    {
                        if (tempDEV.Pl2 != 2000)
                        {
                            T2CH4Value = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(T2LockValue.Text));
                        }
                        else
                        {
                            T2CH4Value = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(T2LockValue.Text));
                        }
                    }
                }
                T2CH4ValueLowBit = (byte)T2CH4Value;
                T2CH4ValueHighBit = (byte)(T2CH4Value >> 8);
                WindAtresiaByteStringOld += T2CH4ValueHighBit + ",";
                WindAtresiaByteStringOld += T2CH4ValueLowBit + ",";
                #endregion
                #region//风筒风量1闭锁值及条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风筒风量2闭锁值及条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//T1 T2解锁条件
                int T1T2UnLockCondition = (byte)(4 << 4) + (byte)(4);//解锁条件默认为 0100 <
                WindAtresiaByteStringOld += T1T2UnLockCondition + ",";
                #endregion
                #region//风筒风量1 风筒风量2解锁条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//T1甲烷解锁值及条件
                int T1CH4UnlockValueHighBit = 0;
                int T1CH4UnlockValueLowBit = 0;
                int T1CH4UnlockValue = 0;//条件默认为且
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T1SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                {
                    T1CH4UnlockValue = (1 << 15) + (int)(Convert.ToDouble(T1UnlockValue.Text) * 100);
                }
                else
                {
                    if (tempDEV != null)
                    {
                        if (tempDEV.Pl2 != 2000)
                        {
                            T1CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(T1UnlockValue.Text));
                        }
                        else
                        {
                            T1CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(T1UnlockValue.Text));
                        }
                    }
                }
                T1CH4UnlockValueLowBit = (byte)T1CH4UnlockValue;
                T1CH4UnlockValueHighBit = (byte)(T1CH4UnlockValue >> 8);
                WindAtresiaByteStringOld += T1CH4UnlockValueHighBit + ",";
                WindAtresiaByteStringOld += T1CH4UnlockValueLowBit + ",";
                #endregion
                #region//T2甲烷解锁值及条件
                int T2CH4UnlockValueHighBit = 0;
                int T2CH4UnlockValueLowBit = 0;
                int T2CH4UnlockValue = 0;//条件默认为且
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T2SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                {
                    T2CH4UnlockValue = (1 << 15) + (int)(Convert.ToDouble(T2UnlockValue.Text) * 100);
                }
                else
                {
                    if (tempDEV != null)
                    {
                        if (tempDEV.Pl2 != 2000)
                        {
                            T2CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(T2UnlockValue.Text));
                        }
                        else
                        {
                            T2CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(T2UnlockValue.Text));
                        }
                    }
                }
                T2CH4UnlockValueLowBit = (byte)T2CH4UnlockValue;
                T2CH4UnlockValueHighBit = (byte)(T2CH4UnlockValue >> 8);
                WindAtresiaByteStringOld += T2CH4UnlockValueHighBit + ",";
                WindAtresiaByteStringOld += T2CH4UnlockValueLowBit + ",";
                #endregion
                #region//风筒风量1解锁值及条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风筒风量2解锁值及条件
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteStringOld += "0,";//此字节全部填0，目前未使用
                #endregion
                #region //风电闭锁开关量解锁值
                WindAtresiaByteStringOld += Convert.ToByte(string.Join("", "10101010"), 2) + ",";//开关量默认2态解锁
                #endregion
                #region//风电闭锁控制口
                byte WindBreakControlChannelHighBit = 0;
                byte WindBreakControlChannelLowBit = 0;
                string[] WindBreakControlChannel = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                foreach (string Point in WindBreakControlPoint)
                {
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    WindBreakControlChannel[16 - ChannelIndex] = "1";
                }
                WindBreakControlChannelLowBit = (byte)Convert.ToInt16(string.Join("", WindBreakControlChannel), 2);
                WindBreakControlChannelHighBit = (byte)(Convert.ToInt16(string.Join("", WindBreakControlChannel), 2) >> 8);
                WindAtresiaByteStringOld += WindBreakControlChannelLowBit + ",";
                #endregion
                #region//风电闭锁控制口(高)
                //增加故障闭锁标记  20170627
                if ((tempStation.Bz3 & 0x4) == 0x4)
                {
                    WindBreakControlChannelHighBit |= 0x01;//置故障闭锁标记
                }
                WindAtresiaByteStringOld += WindBreakControlChannelHighBit + ",";

                #endregion
                #region//甲烷风电闭锁控制口(高)
                WindAtresiaByteStringOld += CH4WindBreakControlChannelHighBit;
                #endregion
                #endregion

                #region 新协议（36个字节）
                #region//甲烷风电闭锁控制口
                CH4WindBreakControlChannelHighBit = 0;
                CH4WindBreakControlChannelLowBit = 0;
                CH4WindBreakControlChannel = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                foreach (string Point in CH4WindBreakControlPoint)
                {
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    CH4WindBreakControlChannel[16 - ChannelIndex] = "1";
                }
                CH4WindBreakControlChannelLowBit = (byte)Convert.ToInt16(string.Join("", CH4WindBreakControlChannel), 2);
                CH4WindBreakControlChannelHighBit = (byte)(Convert.ToInt16(string.Join("", CH4WindBreakControlChannel), 2) >> 8);
                WindAtresiaByteString += CH4WindBreakControlChannelHighBit + ",";
                WindAtresiaByteString += CH4WindBreakControlChannelLowBit + ",";
                #endregion
                #region//风电闭锁解锁条件
                WindAtresiaByteString += "10,";//开关量2态解锁，或关系（默认：1010【二进制】）
                #endregion
                #region//主控通道T1口号及判断条件
                T1ChannelNumber = 0;
                if (!string.IsNullOrEmpty(T1BindPoint))
                {
                    T1ChannelNumber = (byte)(3 << 5) + byte.Parse(T1BindPoint.Trim().Substring(4, 2));//闭锁条件默认是 011 >=
                }
                WindAtresiaByteString += T1ChannelNumber + ",";
                #endregion
                #region//主控通道T2口号及判断条件
                T2ChannelNumber = 0;
                if (!string.IsNullOrEmpty(T2BindPoint))
                {
                    T2ChannelNumber = (byte)(3 << 5) + byte.Parse(T2BindPoint.Trim().Substring(4, 2));//闭锁条件默认是 011 >=
                }
                WindAtresiaByteString += T2ChannelNumber + ",";
                #endregion
                #region//风筒风量1及判断条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region/风筒风量2及判断条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//1#配电柜 主开停地址号及判断条件、取值
                MainKT1Point = 0;
                if (PointWindAtresiaMain.Count > 0)
                {
                    MainKT1Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaMain[0].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT1Point = 0;
                }
                WindAtresiaByteString += MainKT1Point + ",";
                #endregion
                #region//1#配电柜 副开停地址号
                byte BackKT1PointHighBit = 0;
                byte BackKT1PointMiddleBit = 0;
                byte BackKT1PointLowBit = 0;
                string[] BackKT1Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                int index = 0;
                foreach (string Point in PointWindAtresiaMain)
                {
                    if (index == 0)
                    {
                        index = 1;
                        continue;//第一是主开停，不参数计算
                    }
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    BackKT1Point[24 - ChannelIndex] = "1";
                }
                BackKT1PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 16);
                BackKT1PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 8);
                BackKT1PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2));
                WindAtresiaByteString += BackKT1PointHighBit + ",";
                WindAtresiaByteString += BackKT1PointMiddleBit + ",";
                WindAtresiaByteString += BackKT1PointLowBit + ",";
                #endregion
                #region//2#配电柜 主开停地址号及判断条件、取值
                MainKT2Point = 0;
                if (PointWindAtresiaBack.Count > 0)
                {
                    MainKT2Point = (byte)(1 << 7) + (byte)(KTStopState << 5) + byte.Parse(PointWindAtresiaBack[0].Trim().Substring(4, 2));//闭锁条件默认为与
                }
                else
                {
                    MainKT2Point = 0;
                }
                WindAtresiaByteString += MainKT2Point + ",";
                #endregion
                #region//2#配电柜 副开停地址号
                byte BackKT2PointHighBit = 0;
                byte BackKT2PointMiddleBit = 0;
                byte BackKT2PointLowBit = 0;
                string[] BackKT2Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                index = 0;
                foreach (string Point in PointWindAtresiaBack)
                {
                    if (index == 0)
                    {
                        index = 1;
                        continue;//第一是主开停，不参数计算
                    }
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    BackKT2Point[24 - ChannelIndex] = "1";
                }
                BackKT2PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 16);
                BackKT2PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 8);
                BackKT2PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2));
                WindAtresiaByteString += BackKT2PointHighBit + ",";
                WindAtresiaByteString += BackKT2PointMiddleBit + ",";
                WindAtresiaByteString += BackKT2PointLowBit + ",";
                #endregion
                #region//T1甲烷闭锁值及条件
                T1CH4ValueHighBit = 0;
                T1CH4ValueLowBit = 0;
                T1CH4Value = 0;
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T1SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                tempStationDev = Model.DEVServiceModel.QueryDevByDevIDCache(tempStation.Devid);
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                T1CH4Value = (int)(Convert.ToDouble(T1LockValue.Text) * 100);

                T1CH4ValueLowBit = (byte)T1CH4Value;
                T1CH4ValueHighBit = (byte)(T1CH4Value >> 8);
                WindAtresiaByteString += T1CH4ValueHighBit + ",";
                WindAtresiaByteString += T1CH4ValueLowBit + ",";
                #endregion
                #region//T2甲烷闭锁值及条件
                T2CH4ValueHighBit = 0;
                T2CH4ValueLowBit = 0;
                T2CH4Value = 0;
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T2SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                T2CH4Value = (1 << 15) + (int)(Convert.ToDouble(T2LockValue.Text) * 100);//最高位默认为1  20170831

                T2CH4ValueLowBit = (byte)T2CH4Value;
                T2CH4ValueHighBit = (byte)(T2CH4Value >> 8);
                WindAtresiaByteString += T2CH4ValueHighBit + ",";
                WindAtresiaByteString += T2CH4ValueLowBit + ",";
                #endregion
                #region//风筒风量1闭锁值及条件
                int FTFLvalue = 0;
                FTFLvalue = 1 << 15;
                WindAtresiaByteString += (byte)(FTFLvalue >> 8) + ",";//此字节最高位为1，其它全部填0，目前未使用
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风筒风量2闭锁值及条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//T1 T2解锁条件
                T1T2UnLockCondition = (byte)(4 << 4) + (byte)(4);//解锁条件默认为 0100 <
                WindAtresiaByteString += T1T2UnLockCondition + ",";
                #endregion
                #region//风筒风量1 风筒风量2解锁条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//T1甲烷解锁值及条件
                T1CH4UnlockValueHighBit = 0;
                T1CH4UnlockValueLowBit = 0;
                T1CH4UnlockValue = 0;//条件默认为且
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T1SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                T1CH4UnlockValue = (1 << 15) + (int)(Convert.ToDouble(T1UnlockValue.Text) * 100);

                T1CH4UnlockValueLowBit = (byte)T1CH4UnlockValue;
                T1CH4UnlockValueHighBit = (byte)(T1CH4UnlockValue >> 8);
                WindAtresiaByteString += T1CH4UnlockValueHighBit + ",";
                WindAtresiaByteString += T1CH4UnlockValueLowBit + ",";
                #endregion
                #region//T2甲烷解锁值及条件
                T2CH4UnlockValueHighBit = 0;
                T2CH4UnlockValueLowBit = 0;
                T2CH4UnlockValue = 0;//条件默认为且
                tempDEV = new Jc_DevInfo();
                tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(T2SelPoint.Text);
                if (tempPoint != null)
                {
                    tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                }
                //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                {
                    T2CH4UnlockValue = (1 << 15) + (int)(Convert.ToDouble(T2UnlockValue.Text) * 100);
                }
                else
                {
                    if (tempDEV != null)
                    {
                        if (tempDEV.Pl2 != 2000)
                        {
                            T2CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(T2UnlockValue.Text));
                        }
                        else
                        {
                            T2CH4UnlockValue = (1 << 15) + Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(T2UnlockValue.Text));
                        }
                    }
                }
                T2CH4UnlockValueLowBit = (byte)T2CH4UnlockValue;
                T2CH4UnlockValueHighBit = (byte)(T2CH4UnlockValue >> 8);
                WindAtresiaByteString += T2CH4UnlockValueHighBit + ",";
                WindAtresiaByteString += T2CH4UnlockValueLowBit + ",";
                #endregion
                #region//风筒风量1解锁值及条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风筒风量2解锁值及条件
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                WindAtresiaByteString += "0,";//此字节全部填0，目前未使用
                #endregion
                #region//风电闭锁控制口
                WindBreakControlChannelHighBit = 0;
                WindBreakControlChannelLowBit = 0;
                WindBreakControlChannel = new string[16] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                foreach (string Point in WindBreakControlPoint)
                {
                    int ChannelIndex = 0;
                    string tempChannel = Point.Trim().Substring(4, 2);
                    int.TryParse(tempChannel, out ChannelIndex);
                    WindBreakControlChannel[16 - ChannelIndex] = "1";
                }
                WindBreakControlChannelLowBit = (byte)Convert.ToInt16(string.Join("", WindBreakControlChannel), 2);
                WindBreakControlChannelHighBit = (byte)(Convert.ToInt16(string.Join("", WindBreakControlChannel), 2) >> 8);
                WindAtresiaByteString += WindBreakControlChannelHighBit + ",";
                WindAtresiaByteString += WindBreakControlChannelLowBit + ",";
                #endregion
                #region//T1 T2 风筒风量1 风筒风量2 多参数地址号
                byte T1AddressNumber = 0;
                if (!string.IsNullOrEmpty(T1BindPoint))
                {
                    T1AddressNumber = byte.Parse(T1BindPoint.Trim().Substring(6, 1));
                }
                byte T2AddressNumber = 0;
                if (!string.IsNullOrEmpty(T2BindPoint))
                {
                    T2AddressNumber = byte.Parse(T2BindPoint.Trim().Substring(6, 1));
                }
                int AdderssNumberByte = (T2AddressNumber << 2) + T1AddressNumber;//风筒风量暂时未用
                WindAtresiaByteString += AdderssNumberByte;
                #endregion
                #endregion

                #region 保存到父窗体中
                if (WindAtresiaByteString.Length > 0)
                {
                    try
                    {
                        if (tempStation.Bz10 != WindAtresiaByteStringOld.ToString() || tempStation.Bz9 != WindAtresiaPointString || tempStation.Bz11 != WindAtresiaByteString)
                        {
                            tempStationForm.CtxbControlBytes.Text = WindAtresiaByteStringOld.ToString();
                            tempStationForm.CtxbControlBytesNew.Text = WindAtresiaByteString.ToString();
                            tempStationForm.CtxbControlConditon.Text = WindAtresiaPointString;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
                #endregion

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                XtraMessageBox.Show("保存风电闭锁数据出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string scriptCmd = "SaveWindAtresia()";
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页
                //view.EvaluateJavaScript(scriptCmd);
                mx.CurViewEvaluateJavaScript(scriptCmd);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}
