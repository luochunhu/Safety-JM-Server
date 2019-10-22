using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Display.Model;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;


namespace Sys.Safety.Client.Display.VideoMgr
{
    public partial class CFVideoInf : XtraForm
    {
        /// <summary>
        /// IP地址
        /// </summary>
        private string _IP;
        /// <summary>
        /// 端口号
        /// </summary>
        private string _Port;
        /// <summary>
        /// 用户名
        /// </summary>
        private string _UserName;
        /// <summary>
        /// 密码
        /// </summary>
        private string _PassWord;
        /// <summary>
        /// 设备信息对象
        /// </summary>
        private Jc_DefInfo _JCDEFDTO;
        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool m_bInitSDK = false;
        /// <summary>
        /// 当前用户数量
        /// </summary>
        private Int32 m_lUserID = -1;
        /// <summary>
        /// 错误码
        /// </summary>
        private uint iLastErr = 0;
        /// <summary>
        /// 临时Str
        /// </summary>
        private string str;
        /// <summary>
        /// 窗口句柄
        /// </summary>
        private Int32 m_lRealHandle = -1;
        /// <summary>
        /// 云台轮询标记
        /// </summary>
        private bool bAuto = false;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CFVideoInf()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 视频信息构造窗口
        /// </summary>
        /// <param name="IP">摄像头ip地址</param>
        /// <param name="Port">摄像头端口</param>
        public CFVideoInf(string point)
        {
            InitializeComponent();

            //TODO:与其它模块相关联
            //_JCDEFDTO = ChargeMrg.QueryPointByCodeCache(point);
            if (null == _JCDEFDTO)
            {
                return;
            }
            if (string.IsNullOrEmpty(_JCDEFDTO.Remark))
            {
                return;
            }
            if (_JCDEFDTO.Remark.Contains('|'))
            {
                string[] VideoInf = _JCDEFDTO.Remark.Split('|');
                if (VideoInf.Length == 4)
                {
                    _IP = VideoInf[0];
                    _Port = VideoInf[1];
                    _UserName = VideoInf[2];
                    _PassWord = VideoInf[3];
                }
            }

            //_IP = "192.168.1.228";
            //_Port = "8000";
            //_UserName = "admin";
            //_PassWord = "hik12345";
            if (string.IsNullOrEmpty(_IP) || string.IsNullOrEmpty(_Port))
            {
                return;
            }
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                //MessageBox.Show("NET_DVR_Init error!");
                LogHelper.Error("NET_DVR_Init error!");
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }
        }
        /// <summary>
        ///加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFVideoInf_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width, 0);//屏幕工作区域减去窗体宽度
            if (string.IsNullOrEmpty(_IP))
            {
                return;
            }
            if (string.IsNullOrEmpty(_Port))
            {
                return;
            }
            if (string.IsNullOrEmpty(_UserName))
            {
                return;
            }
            if (string.IsNullOrEmpty(_PassWord))
            {
                return;
            }
            if (null != _JCDEFDTO)
            {
                try
                {
                    ClbPoint.Text = _JCDEFDTO.Point;
                    ClbWZ.Text = _JCDEFDTO.Wz;
                    ClbSSZ.Text = _JCDEFDTO.Ssz;
                    ClbZT.Text = _JCDEFDTO.DataState.ToString();
                    ClbZTS.Text = _JCDEFDTO.Zts.ToString();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            try
            {
                if (m_lUserID < 0)
                {
                    string DVRIPAddress = _IP; //设备IP地址或者域名
                    Int16 DVRPortNumber = Int16.Parse(_Port);//设备服务端口号
                    string DVRUserName = _UserName;//设备登录用户名
                    string DVRPassword = _PassWord;//设备登录密码

                    CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                    //登录设备 Login the device
                    m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                    if (m_lUserID < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号
                        //MessageBox.Show(str);
                        LogHelper.Error(str);
                        return;
                    }
                    else
                    {
                        //登录成功
                        //MessageBox.Show("Login Success!");
                    }
                }
                else
                {
                    //注销登录 Logout the device
                    if (m_lRealHandle >= 0)
                    {
                        MessageBox.Show("Please stop live view firstly");
                        return;
                    }
                    if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_Logout failed, error code= " + iLastErr;
                        //MessageBox.Show(str);
                        LogHelper.Error(str);
                        return;
                    }
                    m_lUserID = -1;
                }

                if (m_lUserID < 0)
                {
                    //MessageBox.Show("Please login the device firstly");
                    LogHelper.Error("Please login the device firstly");
                    return;
                }

                if (m_lRealHandle < 0)
                {
                    CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                    lpPreviewInfo.hPlayWnd = CpicVideo.Handle;//预览窗口
                    lpPreviewInfo.lChannel = 1;//预te览的设备通道
                    lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数

                    CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                    IntPtr pUser = new IntPtr();//用户数据

                    //打开预览 Start live view 
                    m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                    if (m_lRealHandle < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                        //MessageBox.Show(str);
                        LogHelper.Error(str);
                        return;
                    }
                    else
                    {
                        //预览成功
                    }
                }
                else
                {
                    //停止预览 Stop live view 
                    if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                        //MessageBox.Show(str);
                        LogHelper.Error(str);
                        return;
                    }
                    m_lRealHandle = -1;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void CFVideoInf_FormClosing(object sender, FormClosingEventArgs e)
        {
            //停止预览 Stop live view 
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //注销登录 Logout the device
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();
        }

        #region 云台向上
        private void CbtnUP_MouseDown(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, 0, 3);
        }

        private void CbtnUP_MouseUp(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, 1, 3);
        }
        #endregion

        #region 云台向左
        private void CbtnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_LEFT, 0, 3);
        }

        private void CbtnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_LEFT, 1, 3);
        }
        #endregion

        #region 云台向右
        private void CbtnRight_MouseUp(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_RIGHT, 1, 3);
        }

        private void CbtnRight_MouseDown(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_RIGHT, 0, 3);
        }
        #endregion

        #region 云台向下
        private void CbtnDown_MouseDown(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, 0, 3);
        }

        private void CbtnDown_MouseUp(object sender, MouseEventArgs e)
        {
            CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, 1, 3);
        }
        #endregion

        private void CbtnRout_Click(object sender, EventArgs e)
        {
            if (!bAuto)
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_AUTO, 0, 3);
                CbtnRout.ToolTip = "Stop";
                bAuto = true;
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_AUTO, 1, 3);
                CbtnRout.ToolTip = "Run";
                bAuto = false;
            }
        }

        #region 云台巡检

        #endregion
    }
}
