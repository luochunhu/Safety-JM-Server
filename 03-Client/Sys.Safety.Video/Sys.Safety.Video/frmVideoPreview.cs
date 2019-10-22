using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;
using Sys.Safety.Video.SDK;
using NetSDKCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Video
{
    public partial class frmVideoPreview : XtraForm
    {
        private readonly IV_DefService _vdefService = ServiceFactory.Create<IV_DefService>();

        /// <summary>
        /// 视频测点集合
        /// </summary>
        private List<V_DefInfo> deflist = new List<V_DefInfo>();
        /// <summary>
        /// 当前选中的视频测点
        /// </summary>
        private V_DefInfo currentDefInfo = new V_DefInfo();

        #region 海康定义

        /// <summary>
        /// 登陆用户ID
        /// </summary>
        private Int32 m_lUserID = -1;
        /// <summary>
        /// 实时预览句柄
        /// </summary>
        private Int32 m_lRealHandle = -1;
        /// <summary>
        /// SDK是否初始化
        /// </summary>
        private bool m_bInitSDK = false;
        /// <summary>
        /// 是否录像
        /// </summary>
        private bool m_bRecord = false;

        #endregion

        #region 大华定义

        /// <summary>
        /// 
        /// </summary>
        fDisConnectCallBack disConnect;
        /// <summary>
        /// call back for receive real data .
        /// </summary>
        private fRealDataCallBackEx m_RealDataCallBack;
        /// <summary>
        /// call back for realplay disconnect.
        /// </summary>
        private fRealPlayDisConnectCallBack m_RealPlayDisConnectCallBack;
        /// <summary>
        /// call back for capture image and save image.
        /// </summary>
        private fSnapRevCallBack m_SnapRevCallBack;
        /// <summary>
        /// wait time.
        /// </summary>
        private const uint TimeOut = 3000;
        /// <summary>
        /// 大华登陆实时句柄
        /// </summary>
        private IntPtr realHandle;
        /// <summary>
        /// 大华登陆ID
        /// </summary>
        private IntPtr loginID;
        /// <summary>
        /// 是否设置抓图回调
        /// </summary>
        private bool m_IsSetCaptureCallBack = false;
        /// <summary>
        /// 大话是否初始化
        /// </summary>
        private bool blnInit = false;
        /// <summary>
        /// 超时
        /// </summary>
        private uint m_SnapSerialNum = 1000;

        #endregion

        /// <summary>
        /// 操作信息
        /// </summary>
        private string staticMsg = string.Empty;

        public frmVideoPreview()
        {
            InitializeComponent();
            //初始化海康SDK
            m_bInitSDK = HK32ChCNetSDK.NET_DVR_Init();

            //初始化大华SDK
            disConnect = new fDisConnectCallBack(DisConnectEvent);
            blnInit = NETClient.Init(disConnect, IntPtr.Zero, null);

            m_RealDataCallBack = new fRealDataCallBackEx(RealDataCallBack); //instance realdata callback.
            m_RealPlayDisConnectCallBack = new fRealPlayDisConnectCallBack(RealPlayDisConnectCallBack); //instance realplay disconnect 
            m_SnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack);

            //加载视频测点
            GetVideoTreeDataSource();
        }

        private void GetVideoTreeDataSource()
        {
            try
            {
                var vdefresponse = _vdefService.GetAllDef(new DefGetAllRequest());
                if (vdefresponse.IsSuccess)
                {
                    deflist = vdefresponse.Data;

                    List<string> defids = new List<string>();

                    deflist.ForEach(def =>
                    {
                        if (!defids.Contains(def.Id))
                        {
                            //如果areaid为空，直接加入视频树；如果不为空则把区域加入视频树，并把视频测点加入该树的子节点
                            if (string.IsNullOrEmpty(def.AreaId))
                            {
                                TreeNode node = new TreeNode();
                                node.Tag = 1;
                                node.Text = def.Devname;
                                node.Name = def.Id;
                                videotree.Nodes.Add(node);

                                defids.Add(def.Id);
                            }
                            else
                            {

                                var areadeflist = deflist.Where(d => d.AreaId == def.AreaId).ToList();

                                TreeNode node = new TreeNode();
                                node.Tag = 0;
                                node.Text = def.By1;
                                node.Name = def.Id;

                                areadeflist.ForEach(area =>
                                {
                                    TreeNode defnode = new TreeNode();
                                    defnode.Tag = 1;
                                    defnode.Text = area.Devname;
                                    defnode.Name = area.Id;
                                    node.Nodes.Add(defnode);

                                    defids.Add(area.Id);
                                });

                                videotree.Nodes.Add(node);
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("视频测点信息加载失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 选中视频测点登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videotree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //如果选择视频测点节点，则登陆此视频设备
                if ((int)e.Node.Tag == 1)
                {
                    VideoDispose();
                    m_lUserID = -1;
                    loginID = IntPtr.Zero;

                    currentDefInfo = deflist.FirstOrDefault(o => o.Id == (string)e.Node.Name);
                    if (currentDefInfo != null)
                    {
                        string ipAddress = currentDefInfo.IPAddress;
                        int port = Convert.ToInt32(currentDefInfo.Port);
                        string username = currentDefInfo.Username;
                        string password = currentDefInfo.Password;

                        if (currentDefInfo.Vendor == 0)
                        {

                            HK32ChCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new HK32ChCNetSDK.NET_DVR_DEVICEINFO_V30();
                            m_lUserID = HK32ChCNetSDK.NET_DVR_Login_V30(ipAddress, port, username, password, ref DeviceInfo);
                            if (m_lUserID < 0)
                            {
                                var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();

                                staticMsg = string.Format("视频测点 {0} 登陆失败： NET_DVR_Login_V30_{1}", currentDefInfo.Devname, iLastErr);
                                this.StaticMsg.Caption = staticMsg;
                            }
                            else
                            {
                                staticMsg = string.Format("视频测点 {0} 登陆成功...", currentDefInfo.Devname);
                                this.StaticMsg.Caption = staticMsg;
                                btnpreview_Click(new object(), new EventArgs());
                            }
                        }
                        else if (currentDefInfo.Vendor == 1)//大华
                        {
                            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
                            loginID = NETClient.Login(ipAddress, (ushort)port, username, password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);

                            if (loginID != IntPtr.Zero)
                            {
                                staticMsg = string.Format("视频测点 {0} 登陆成功...", currentDefInfo.Devname);
                                this.StaticMsg.Caption = staticMsg;
                                btnpreview_Click(new object(), new EventArgs());
                            }
                            else
                            {
                                staticMsg = string.Format("视频测点 {0} 登陆失败...", currentDefInfo.Devname);
                                this.StaticMsg.Caption = staticMsg;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("视频测点登陆失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentDefInfo.Vendor == 0)
                {
                    if (m_lRealHandle < 0)
                    {
                        HK32ChCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HK32ChCNetSDK.NET_DVR_PREVIEWINFO();
                        lpPreviewInfo.hPlayWnd = videopicture.Handle;//预览窗口
                        lpPreviewInfo.lChannel = currentDefInfo.Channel;//预te览的设备通道
                        lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                        lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                        lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                        lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数

                        HK32ChCNetSDK.REALDATACALLBACK RealData = new HK32ChCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                        IntPtr pUser = new IntPtr();//用户数据

                        //打开预览
                        m_lRealHandle = HK32ChCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                        if (m_lRealHandle < 0)
                        {
                            var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();
                            staticMsg = string.Format("视频测点 {0} 预览失败： NET_DVR_RealPlay_V40_{1}", currentDefInfo.Devname, iLastErr);
                            this.StaticMsg.Caption = staticMsg;
                        }
                        else
                        {
                            this.btnpreview.Text = "停止预览";
                            staticMsg = string.Format("视频测点 {0} 预览成功...", currentDefInfo.Devname);
                            this.StaticMsg.Caption = staticMsg;
                        }
                    }
                    //停止预览
                    else
                    {
                        if (!HK32ChCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                        {
                            var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();
                            staticMsg = string.Format("视频测点 {0} 停止预览失败：NET_DVR_StopRealPlay_{1}", currentDefInfo.Devname, iLastErr);
                            this.StaticMsg.Caption = staticMsg;
                        }
                        else
                        {
                            m_lRealHandle = -1;
                            videopicture.Refresh();
                            staticMsg = string.Format("视频测点 {0} 停止预览...", currentDefInfo.Devname);
                            this.btnpreview.Text = "预览";
                        }
                    }
                }
                else if (currentDefInfo.Vendor == 1)
                {
                    if (realHandle == IntPtr.Zero)
                    {
                        realHandle = NETClient.StartRealPlay(loginID, currentDefInfo.Channel, videopicture.Handle, EM_RealPlayType.Realplay, m_RealDataCallBack, m_RealPlayDisConnectCallBack, IntPtr.Zero, TimeOut);
                        if (realHandle != IntPtr.Zero)
                        {
                            videopicture.Refresh();
                            staticMsg = string.Format("视频测点 {0} 预览成功...", currentDefInfo.Devname);
                            this.btnpreview.Text = "停止预览";
                        }
                    }
                    else
                    {
                        NETClient.StopRealPlay(realHandle);
                        videopicture.Refresh();
                        realHandle = IntPtr.Zero;
                        staticMsg = string.Format("视频测点 {0} 停止预览...", currentDefInfo.Devname);
                        this.btnpreview.Text = "预览";
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("视频测点预览失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndraw_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentDefInfo.Vendor == 0)
                {
                    //图片保存路径和文件名
                    string picFileName = "C:\\maspicture\\" + currentDefInfo.Devname + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bmp";

                    //BMP抓图 Capture a BMP picture
                    if (!HK32ChCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, picFileName))
                    {
                        var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();
                        staticMsg = string.Format("视频测点 {0} 抓图失败：NET_DVR_CapturePicture_{1}", currentDefInfo.Devname, iLastErr);
                        this.StaticMsg.Caption = staticMsg;
                    }
                    else
                    {
                        staticMsg = string.Format("视频测点 {0} 抓图成功：{1}", currentDefInfo.Devname, picFileName);
                        this.StaticMsg.Caption = staticMsg;
                    }
                }
                else if (currentDefInfo.Vendor == 1)
                {
                    if (m_IsSetCaptureCallBack == false)
                    {
                        NETClient.SetSnapRevCallBack(m_SnapRevCallBack, IntPtr.Zero);
                        m_IsSetCaptureCallBack = true;
                    }
                    NET_SNAP_PARAMS snap = new NET_SNAP_PARAMS();
                    snap.Channel = (uint)0;
                    snap.Quality = 6;
                    snap.ImageSize = 2;
                    snap.mode = 0;
                    snap.InterSnap = 0;
                    snap.CmdSerial = m_SnapSerialNum;

                    bool ret = NETClient.SnapPictureEx(loginID, snap, IntPtr.Zero); //call capture function.
                    if (ret)
                    {
                        m_SnapSerialNum++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("视频测点抓图失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 录像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnrecord_Click(object sender, EventArgs e)
        {
            try
            {
                //录像保存路径和文件名 the path and file name to save
                string sVideoFileName;
                sVideoFileName = "C:\\masvideo\\" + currentDefInfo.Devname + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";

                if (m_bRecord == false)
                {
                    //强制I帧 Make a I frame
                    int lChannel = 1; //通道号 Channel number
                    HK32ChCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, lChannel);

                    //开始录像 Start recording
                    if (!HK32ChCNetSDK.NET_DVR_SaveRealData(m_lRealHandle, sVideoFileName))
                    {
                        var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();
                        staticMsg = string.Format("视频测点 {0} 录像失败：{1}", currentDefInfo.Devname, sVideoFileName);
                        this.StaticMsg.Caption = staticMsg;
                    }
                    else
                    {
                        staticMsg = string.Format("视频测点 {0} 开始录像...", currentDefInfo.Devname);
                        this.StaticMsg.Caption = staticMsg;
                        this.btnrecord.Text = "停止录像";
                        m_bRecord = true;
                    }
                }
                else
                {
                    //停止录像
                    if (!HK32ChCNetSDK.NET_DVR_StopSaveRealData(m_lRealHandle))
                    {
                        var iLastErr = HK32ChCNetSDK.NET_DVR_GetLastError();
                        staticMsg = string.Format("视频测点 {0} 停止录像失败：NET_DVR_StopSaveRealData_{1}", currentDefInfo.Devname, iLastErr);
                        this.StaticMsg.Caption = staticMsg;
                    }
                    else
                    {
                        m_lRealHandle = -1;
                        staticMsg = string.Format("视频测点 {0} 录像成功：{1}", currentDefInfo.Devname, sVideoFileName);
                        this.btnrecord.Text = "录像";
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("视频测点录像失败！" + ex.Message);
            }
        }

        #region 云台控制

        private void btnup_MouseDown(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.TILT_UP, 0, 5);
        }

        private void btnup_MouseUp(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.TILT_UP, 1, 5);
        }

        private void btnleft_MouseDown(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.PAN_LEFT, 0, 5);
        }

        private void btnleft_MouseUp(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.PAN_LEFT, 1, 5);
        }

        private void btnright_MouseDown(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.PAN_RIGHT, 0, 5);
        }

        private void btnright_MouseUp(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.PAN_RIGHT, 1, 5);
        }

        private void btndown_MouseDown(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.TILT_DOWN, 0, 5);
        }

        private void btndown_MouseUp(object sender, MouseEventArgs e)
        {
            HK32ChCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, HK32ChCNetSDK.TILT_DOWN, 1, 5);
        }

        #endregion

        /// <summary>
        /// 海康视频实时流回调
        /// </summary>
        /// <param name="lRealHandle"></param>
        /// <param name="dwDataType"></param>
        /// <param name="pBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="pUser"></param>
        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {

        }

        /// <summary>
        /// 释放摄像头资源
        /// </summary>
        private void VideoDispose()
        {
            if (m_lRealHandle >= 0)
            {
                HK32ChCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                this.btnpreview.Text = "预览";
            }
            if (m_lUserID >= 0)
            {
                HK32ChCNetSDK.NET_DVR_Logout(m_lUserID);
            }

            if (realHandle != IntPtr.Zero)
            {
                NETClient.StopRealPlay(realHandle);
            }
            if (loginID != IntPtr.Zero)
            {
                NETClient.Logout(loginID);
            }

        }

        /// <summary>
        /// 关闭窗体时释放资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVideoPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            VideoDispose();
            if (m_bInitSDK == true)
            {
                HK32ChCNetSDK.NET_DVR_Cleanup();
            }

            if (blnInit)
            {
                NETClient.Cleanup();
            }
            this.Dispose();
        }

        /// <summary>
        /// 大华断开连接回调
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pchDVRIP"></param>
        /// <param name="nDVRPort"></param>
        /// <param name="dwUser"></param>
        private void DisConnectEvent(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {

        }

        /// <summary>
        /// 实时监测回调
        /// </summary>
        /// <param name="lRealHandle"></param>
        /// <param name="dwDataType"></param>
        /// <param name="pBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="param"></param>
        /// <param name="dwUser"></param>
        private void RealDataCallBack(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, int param, IntPtr dwUser)
        {
            //MessageBox.Show("RealDataCallBack");
        }

        /// <summary>
        /// 实时断开回调
        /// </summary>
        /// <param name="lOperateHandle"></param>
        /// <param name="dwEventType"></param>
        /// <param name="param"></param>
        /// <param name="dwUser"></param>
        private void RealPlayDisConnectCallBack(IntPtr lOperateHandle, EM_REALPLAY_DISCONNECT_EVENT_TYPE dwEventType, IntPtr param, IntPtr dwUser)
        {

        }

        /// <summary>
        /// 大华抓图回调
        /// </summary>
        /// <param name="lLoginID"></param>
        /// <param name="pBuf"></param>
        /// <param name="RevLen"></param>
        /// <param name="EncodeType"></param>
        /// <param name="CmdSerial"></param>
        /// <param name="dwUser"></param>
        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)
        {
            try
            {
                string path = "C:\\maspicture";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (EncodeType == 10) //.jpg
                {
                    string fileName = currentDefInfo.Devname + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    string filePath = path + "\\" + fileName;
                    byte[] data = new byte[RevLen];
                    Marshal.Copy(pBuf, data, 0, (int)RevLen);
                    using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        stream.Write(data, 0, (int)RevLen);
                        stream.Flush();
                        stream.Dispose();
                    }
                    staticMsg = string.Format("视频测点 {0} 抓图成功：{1}", currentDefInfo.Devname, filePath);
                    this.StaticMsg.Caption = staticMsg;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("视频测点抓图失败！" + ex.Message);
            }
        }
    }
}
