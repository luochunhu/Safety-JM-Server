using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Configuration;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon.Gallery;
using DevExpress.XtraRichEdit;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.Utils.About;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Graphic
{
    public partial class AreaGraphDrawing : DevExpress.XtraEditors.XtraForm
    {

        private IGraphicsbaseinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();

        /// <summary>
        /// 当前鼠标所在X坐标
        /// </summary>
        private int x;
        /// <summary>
        /// 当前鼠标所在Y坐标
        /// </summary>
        private int y;
        /// <summary>
        /// 图形库是否展开
        /// </summary>
        private bool IsGraphicsLibOn = true;
        /// <summary>
        /// 图形操作类
        /// </summary>
        public GraphicOperations GraphOpt = new GraphicOperations();
        /// <summary>
        /// 鼠标ToolTip事件
        /// </summary>
        ToolTip MousetoolTip = new ToolTip();
        /// <summary>
        /// 图形库位置下标变量
        /// </summary>
        int GraphLibindex = 0;
        /// <summary>
        /// 等待窗口
        /// </summary>
        private DevExpress.Utils.WaitDialogForm wdf = null;
        /// <summary>
        /// 是否在主界面中显示
        /// </summary>
        private bool IsInIframe = false;

        /// <summary>
        /// 记录鼠标上一次移动的时间
        /// </summary>
        private DateTime lastMouseMoveTime = System.DateTime.Now;
        DefaultLookAndFeel defaultLookAndFeel;

        /// <summary>
        /// 获取图形更新线程
        /// </summary>
        private System.Threading.Thread m_GetMapDataThread;
        /// <summary>
        /// 运行标记
        /// </summary>
        public bool _isRun = false;


        public string Jsonstr { get; set; }

        public static Axmetamap2dLib.AxMetaMapX2D Mapobj = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GraphicEdit">是否处于编辑状态(true:是，false:否)</param>
        public AreaGraphDrawing()
        {
            InitializeComponent();
            //增加服务授权设置  20171026
            mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
            mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
            // 解决窗口闪烁的问题  
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            //Program.main = this;
        }
        public AreaGraphDrawing(string jsonStr)
        {
            InitializeComponent();
            //增加服务授权设置  20171026
            mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
            mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
            // 解决窗口闪烁的问题  
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            //Program.main = this;

            this.Jsonstr = jsonStr;
        }
        public AreaGraphDrawing(Dictionary<string, string> param)
        {
            if (param != null && param.Count > 0)
            {
                if (param["GraphicEdit"].ToString().ToLower() == "true")
                {
                    GraphOpt.IsGraphicEdit = true;
                }
                else
                {
                    GraphOpt.IsGraphicEdit = false;
                }
                if (param["GraphicInIframe"].ToString().ToLower() == "true")
                {
                    IsInIframe = true;
                }
                else
                {
                    IsInIframe = false;
                }
            }
            else
            {
                return;
            }
            InitializeComponent();
            //增加服务授权设置  20171026
            mx.SetPirvateKey("{D2D720B4-85C1-4CDF-AB0C-4C1BC04DEB8A}");
            mx.SetRegisterMode(2, "http://" + System.Configuration.ConfigurationManager.AppSettings["ServerIp"].ToString() + ":6789");
            // 解决窗口闪烁的问题  
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            //Program.main = this;
        }

        /// <summary>
        ///  添加测点到地图
        /// </summary>
        /// <param name="PointName"></param>
        /// <param name="GraphBindType"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void AddPointToMap(string PointName, string GrapUnitName, int GraphBindType, string zoomLevel, string animationState,
            float left, float top, string Width, string Height, string graphType)
        {
            try
            {
                GraphOpt.AddPointToMap(mx, PointName, GrapUnitName, GraphBindType, zoomLevel, animationState, left, top, Width, Height, graphType);
            }
            catch (Exception ex)
            {
                LogHelper.Error("AddPointToMap" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 测点绑定
        /// </summary>
        /// <param name="PointName"></param>
        /// <param name="PointWz"></param>
        /// <param name="PointDevName"></param>
        public void EditPoint(string PointName, string PointWz, string PointDevName, string DisZoomlevel, string animationState, string Width, string Height, string TurnToPage)
        {
            try
            {
                //GraphOpt.EditPoint(mx, PointName, PointWz, PointDevName, DisZoomlevel, animationState, Width, Height, TurnToPage);
            }
            catch (Exception ex)
            {
                LogHelper.Error("EditPoint" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 加载图形
        /// </summary>
        /// <param name="GraphName"></param>
        public void LoadMap(string GraphName)
        {
            try
            {
                if (string.IsNullOrEmpty(GraphName))
                {
                    //barStaticItem2.Caption = "未打开图形";
                }
                else
                {
                    //barStaticItem2.Caption = "当前图形：" + GraphName;
                }

                GraphOpt.LoadMap(mx, GraphName);

                //将当前打开的图形在页面切换栏目中选中
                //for (int i = 0; i < ribbonGalleryBarItem1.Gallery.Groups.Count; i++)
                //{
                //    for (int j = 0; j < ribbonGalleryBarItem1.Gallery.Groups[i].Items.Count; j++)
                //    {
                //        if (ribbonGalleryBarItem1.Gallery.Groups[i].Items[j].Caption == GraphName)
                //        {
                //            ribbonGalleryBarItem1.Gallery.Groups[i].Items[j].Checked = true;
                //        }
                //    }
                //}
                Mapobj = mx;
            }
            catch (Exception ex)
            {
                LogHelper.Error("LoadMap" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 获取所有图层
        /// </summary>
        /// <returns></returns>
        public List<string> LoadLayers()
        {
            List<string> templist = new List<string>();
            try
            {
                templist = GraphOpt.LoadLayers(mx);
            }
            catch (Exception ex)
            {
                LogHelper.Error("LoadLayers" + ex.Message + ex.StackTrace);
            }
            return templist;
        }
        /// <summary>
        /// 图层显示
        /// </summary>
        public void LayerDisplay(string LayerName)
        {
            try
            {
                GraphOpt.LayerDisplay(mx, LayerName);
            }
            catch (Exception ex)
            {
                LogHelper.Error("LayerDisplay" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 测点显示
        /// </summary>
        /// <param name="Point"></param>
        public void PointDisplay(string Point)
        {
            try
            {
                GraphOpt.PointDisplay(mx, Point);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisplay" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 设备定位
        /// </summary>
        /// <param name="Point"></param>
        public void PointSercah(string Point)
        {
            try
            {
                GraphOpt.PointSercah(mx, Point);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointSercah" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 图层隐藏
        /// </summary>
        public void LayerHidden(string LayerName)
        {
            try
            {
                GraphOpt.LayerHidden(mx, LayerName);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisplay" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 测点隐藏
        /// </summary>
        /// <param name="Point"></param>
        public void PointHidden(string Point)
        {
            try
            {
                GraphOpt.PointHidden(mx, Point);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisplay" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 获取所有测点的显示/隐藏状态
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPointDis()
        {
            List<string> templist = new List<string>();
            try
            {
                templist = GraphOpt.getAllGraphPointDis(mx);
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetAllPointDis" + ex.Message + ex.StackTrace);
            }
            return templist;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GISPlatformCenter_Load(object sender, EventArgs e)
        {
            try
            {
                #region wcf检查服务端是否正常运行
                if (ConfigurationManager.AppSettings["ServiceType"].ToString() == "wcf")
                {
                    try
                    {
                        //ServiceFactory.CreateService<IGraphicsbaseinfService>().GetAll();//调用服务端接口，看能否正常调用来判断服务端是否开启

                    }
                    catch
                    {
                        //MessageBox.Show("连接服务端异常，请配置服务器！");

                        WcfManage wcfmag = new WcfManage();
                        wcfmag.ShowDialog();
                        //退出应用程序
                        System.Environment.Exit(0);
                    }
                }
                #endregion

                wdf = new WaitDialogForm("正在加载数据...", "请等待...");

                //InitDevControlSkin();//加载皮肤

                //设置窗体高度和宽度
                //this.Width = Screen.GetWorkingArea(this).Width;
                //this.Height = Screen.GetWorkingArea(this).Height;
                this.Left = 0;
                this.Top = 0;
                if (!GraphOpt.IsGraphicEdit)//如果是非编辑状态
                {
                    //隐藏图形库
                    //navBarControl1.Width = 0;
                    //隐藏图形基本操作、命令操作功能
                    // BasicOptMenu.Visible = false;
                    //CommandPage.Visible = false;
                }
                //判断是否为嵌入方式，如果是嵌入方式，则隐藏ribboncontrol1
                if (IsInIframe)
                {
                    // ribbonControl1.Visible = false;
                    //ribbonStatusBar1.Visible = false;
                }

                //设置不显示gis控件的滚动条
                mx.SetData("showProgress", "false");

                #region 从数据库读取所有图形文件
                GraphOpt.LoadGraphicsInfo();
                #endregion

                #region 加载图元库
                GraphLibindex = 0;
                XtraScrollableControl pal = new XtraScrollableControl();
                pal.Dock = DockStyle.Fill;
                pal.AutoScroll = true;
                pal.Controls.AddRange(GraphLibLoad(0));
                pal.Controls.AddRange(GraphLibLoad(3));
                GraphLibindex = 0;
                pal.Controls.AddRange(GraphLibTextLoad(0));
                pal.Controls.AddRange(GraphLibTextLoad(3));
                //navBarGroupControlContainer4.Controls.Add(pal);

                GraphLibindex = 0;
                pal = new XtraScrollableControl();
                pal.Dock = DockStyle.Fill;
                pal.AutoScroll = true;
                pal.Controls.AddRange(GraphLibLoad(1));
                GraphLibindex = 0;
                pal.Controls.AddRange(GraphLibTextLoad(1));
                //navBarGroupControlContainer1.Controls.Add(pal);

                GraphLibindex = 0;
                pal = new XtraScrollableControl();
                pal.Dock = DockStyle.Fill;
                pal.AutoScroll = true;
                pal.Controls.AddRange(GraphLibLoad(2));
                GraphLibindex = 0;
                pal.Controls.AddRange(GraphLibTextLoad(2));
                //navBarGroupControlContainer3.Controls.Add(pal);

                #endregion

                //隐藏悬浮拖动图元
                pictureBox1.Visible = false;
                //进入时隐藏菜单
                //ribbonControl1.Minimized = true;

                if (wdf != null)
                {
                    wdf.Close();
                }

                //获取通风系统默认图形
                var graphicsbaseinfInfo = GraphOpt.getAllGraphicDto().ToList().Find(a => a.Bz3 == "1");
                if (graphicsbaseinfInfo != null && !string.IsNullOrWhiteSpace(graphicsbaseinfInfo.GraphId))
                {
                    LoadMap(graphicsbaseinfInfo.GraphName);
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("没有设置应急联动默认图形，请去设置", "系统提示", MessageBoxButtons.OK);
                }

                //barStaticItem1.Caption = "启动成功";
            }
            catch (Exception ex)
            {
                if (wdf != null)
                {
                    wdf.Close();
                }
                LogHelper.Error("GISPlatformCenter_Load" + ex.Message + ex.StackTrace);
            }

        }

        /// <summary>
        /// 加载图形库
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private PictureBox[] GraphLibLoad(int type)
        {

            IList<string> GraphImgList = new List<string>();
            string graphPath = "";
            PictureBox[] pb = null;
            try
            {
                GraphImgList = GraphOpt.GraphicsLibLoad(type);
                pb = new PictureBox[GraphImgList.Count];
                switch (type)
                {
                    case 0:
                        graphPath = "Text";
                        break;
                    case 1:
                        graphPath = "Topology";
                        break;
                    case 2:
                        graphPath = "Svg";
                        break;
                    case 3:
                        graphPath = "Gif";
                        break;
                }

                for (int i = 0; i < GraphImgList.Count; i++)
                {
                    pb[i] = new System.Windows.Forms.PictureBox();
                    pb[i].BorderStyle = BorderStyle.FixedSingle;
                    pb[i].Name = GraphImgList[i];
                    pb[i].Tag = type;
                    pb[i].Width = 60;
                    pb[i].Height = 60;
                    if (GraphLibindex % 2 == 0)
                    {
                        pb[i].Location = new Point(20, (int)(GraphLibindex / 2) * (100) + 5);
                        pb[i].Image = Image.FromFile(Application.StartupPath + @"\\mx\\" + graphPath + "\\" + GraphImgList[i] + ".png");
                    }
                    else
                    {
                        pb[i].Location = new Point(110, (int)(GraphLibindex / 2) * (100) + 5);
                        pb[i].Image = Image.FromFile(Application.StartupPath + @"\\mx\\" + graphPath + "\\" + GraphImgList[i] + ".png");
                    }
                    pb[i].SizeMode = PictureBoxSizeMode.Zoom;
                    pb[i].MouseMove += new MouseEventHandler(onMouseMove);

                    GraphLibindex++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphLibLoad" + ex.Message + ex.StackTrace);
            }
            return pb;
        }
        private Label[] GraphLibTextLoad(int type)
        {

            IList<string> GraphImgList = new List<string>();
            string graphPath = "";
            Label[] pb = null;
            try
            {
                GraphImgList = GraphOpt.GraphicsLibLoad(type);
                switch (type)
                {
                    case 0:
                        graphPath = "Text";
                        break;
                    case 1:
                        graphPath = "Topology";
                        break;
                    case 2:
                        graphPath = "Svg";
                        break;
                    case 3:
                        graphPath = "Gif";
                        break;
                }
                pb = new Label[GraphImgList.Count];
                for (int i = 0; i < GraphImgList.Count; i++)
                {
                    pb[i] = new System.Windows.Forms.Label();
                    if (GraphImgList[i].Contains("&"))
                    {
                        pb[i].Text = GraphImgList[i].Substring(0, GraphImgList[i].IndexOf('&'));
                    }
                    else
                    {
                        pb[i].Text = GraphImgList[i];
                    }
                    pb[i].Width = 90;
                    if (GraphLibindex % 2 == 0)
                    {
                        pb[i].Location = new Point(20, (int)(GraphLibindex / 2) * (100) + 70);

                    }
                    else
                    {
                        pb[i].Location = new Point(110, (int)(GraphLibindex / 2) * (100) + 70);

                    }


                    GraphLibindex++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphLibTextLoad" + ex.Message + ex.StackTrace);
            }
            return pb;
        }


        /// <summary>
        /// 图元鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PictureBox pic = (PictureBox)sender;
                pictureBox1.Width = 60;
                pictureBox1.Height = 60;
                pictureBox1.Image = pic.Image;
                pictureBox1.Visible = true;
                x = e.Location.X;
                y = e.Location.Y;
                pictureBox1.Left = this.PointToClient(Control.MousePosition).X - 10;
                pictureBox1.Top = this.PointToClient(Control.MousePosition).Y - 10;
                pictureBox1.Name = pic.Name;
                pictureBox1.Tag = pic.Tag;

                MousetoolTip.AutoPopDelay = 3000;
                MousetoolTip.InitialDelay = 1000;
                MousetoolTip.ReshowDelay = 200;

                if (IsGraphicsLibOn)
                {
                    MousetoolTip.SetToolTip(this.pictureBox1, "拖动到图上添加设备");
                }
                else
                {
                    MousetoolTip.SetToolTip(this.pictureBox1, "拖动到图上并单击添加设备");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("onMouseMove" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 悬浮拖动图元鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 悬浮拖动图元鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 图形库控件SizeChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControl1_SizeChanged(object sender, EventArgs e)
        {
            //if (navBarControl1.Width < 50)//未展开
            //{
            //    IsGraphicsLibOn = false;
            //    //pictureBox1.Visible = false;
            //}
            //else
            //{
            //    IsGraphicsLibOn = true;
            //}
        }
        /// <summary>
        /// 基础图形库点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarGroupControlContainer4_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = false;
        }
        /// <summary>
        /// 图库控件单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControl1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = false;
        }
        /// <summary>
        /// 菜单控制单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ribbonControl1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = false;
        }
        /// <summary>
        /// 图形控件命令响应方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
        {
            string Param = "";

            switch (e.p_sCmd)
            {
                case "MessagePub"://测试交互代码
                    DevExpress.XtraEditors.XtraMessageBox.Show(e.p_sParam);
                    break;
                case "PointEdit"://图形测点编辑
                    //barStaticItem1.Caption = "测点编辑";
                    //try
                    //{
                    //    Param = e.p_sParam;
                    //    string Point = e.p_sParam.ToString().Split('|')[0];
                    //    string UnitName = e.p_sParam.ToString().Split('|')[1];
                    //    string BindType = e.p_sParam.ToString().Split('|')[2];
                    //    string ZoomLevel = e.p_sParam.ToString().Split('|')[3];
                    //    string animationState = e.p_sParam.ToString().Split('|')[4];
                    //    string TurnToPage = "";
                    //    //判断当前图元类型
                    //    if (UnitName.Contains("&"))//静态图元
                    //    {
                    //        if (BindType == "1" || BindType == "2")
                    //        {
                    //            string Width = e.p_sParam.ToString().Split('|')[5];
                    //            string Height = e.p_sParam.ToString().Split('|')[6];
                    //            TurnToPage = e.p_sParam.ToString().Split('|')[7];

                    //            PointEdit pointedit = new PointEdit(Point, Width, Height, TurnToPage);
                    //            pointedit.Show();
                    //        }
                    //        else
                    //        {
                    //            TurnToPage = e.p_sParam.ToString().Split('|')[5];
                    //            PointEdit pointedit = new PointEdit(Point, TurnToPage);
                    //            pointedit.Show();
                    //        }
                    //    }
                    //    else//动态图元
                    //    {
                    //        if (BindType == "1" || BindType == "2")
                    //        {
                    //            string Width = e.p_sParam.ToString().Split('|')[5];
                    //            string Height = e.p_sParam.ToString().Split('|')[6];
                    //            TurnToPage = e.p_sParam.ToString().Split('|')[7];
                    //            //PointDefEdit pointedit = new PointDefEdit(Point, UnitName, BindType, ZoomLevel, animationState, Width, Height, TurnToPage);
                    //            //pointedit.Show();
                    //        }
                    //        else
                    //        {
                    //            TurnToPage = e.p_sParam.ToString().Split('|')[5];
                    //            //PointDefEdit pointedit = new PointDefEdit(Point, UnitName, BindType, ZoomLevel, animationState, TurnToPage);
                    //            //pointedit.Show();
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    //DevExpress.XtraEditors.XtraMessageBox.Show("测点编辑失败，详细请查看错误日志！");
                    //    //barStaticItem1.Caption = "编辑失败，详细请查看错误日志！";
                    //    LogHelper.Error("mx_OnViewCallOutCommand_PointEdit" + ex.Message + ex.StackTrace);
                    //}
                    break;
                case "PointDel"://图形测点删除
                // barStaticItem1.Caption = "测点删除";
                //try
                //{
                //    Param = e.p_sParam;
                //    GraphOpt.DelPoint(mx, Param);
                //}
                //catch (Exception ex)
                //{
                //    //DevExpress.XtraEditors.XtraMessageBox.Show("删除失败，详细请查看错误日志！");
                //    // barStaticItem1.Caption = "删除失败，详细请查看错误日志！";
                //    LogHelper.Error("mx_OnViewCallOutCommand_PointDel" + ex.Message + ex.StackTrace);
                //}
                //break;
                case "PointsSave"://图形测点保存
                    //barStaticItem1.Caption = "图形保存";
                    try
                    {
                        Param = e.p_sParam;
                        GraphOpt.PointsSave(Param);
                        // barStaticItem1.Caption = "保存成功！";
                        //DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！");                       
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("保存失败，详细请查看错误日志！");
                        //barStaticItem1.Caption = "保存失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_PointsSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "RoutesSave"://保存测点连线                    
                    try
                    {
                        Param = e.p_sParam;
                        if (string.IsNullOrEmpty(Param))
                        {
                            return;
                        }
                        GraphOpt.RoutesSave(Param);
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("保存测点连线失败，详细请查看错误日志！");
                        //barStaticItem1.Caption = "保存失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_RoutesSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "LoadPoint"://加载图形绑定的测点信息                     
                    try
                    {
                        //GraphOpt.LoadMapPointsInfo(mx, GraphOpt.GraphNameNow);
                        if (!string.IsNullOrWhiteSpace(this.Jsonstr))
                        {
                            string[] json = this.Jsonstr.Split('|');
                            GraphOpt.DoDrawingGraphicsInEdit(mx, json[0], json[1]);
                        }

                        //移除右键菜单
                        GraphOpt.DoRemoveMapContextMenu(mx);
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("加载图形绑定的测点信息失败，详细请查看错误日志！");
                        // barStaticItem1.Caption = "加载图形的测点信息失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_LoadPoint" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "SetMapEditState"://设置图形的可编辑状态                    
                    try
                    {
                        GraphOpt.setGraphEditState(mx, GraphOpt.IsGraphicEdit);
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("设置图形的可编辑状态失败，详细请查看错误日志！");
                        //barStaticItem1.Caption = "设置图形的编辑状态失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_SetMapEditState" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "setMapEditSave"://设置图形是否保存
                    try
                    {
                        Param = e.p_sParam;
                        GraphOpt.setGraphEditSave(bool.Parse(Param));
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("设置图形是否保存失败，详细请查看错误日志！");
                        //  barStaticItem1.Caption = "设置图形是否保存失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_setMapEditSave" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "SetMapTopologyInit"://拓扑图初始化所有井上设备                    
                    try
                    {
                        if (GraphOpt.IsTopologyInit)
                        {
                            GraphOpt.SetMapTopologyInit(mx);
                        }
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("拓扑图加载井上设备失败，详细请查看错误日志！");
                        // barStaticItem1.Caption = "拓扑图加载井上设备失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_SetMapTopologyInit" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "pageToImg":
                    // barStaticItem1.Caption = "导出图片";
                    saveFileDialog1.FileName = GraphOpt.GraphNameNow;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        GraphOpt.mapToImage(mx, saveFileDialog1.FileName);
                    }
                    break;
                case "layerDis":
                    // barStaticItem1.Caption = "图层显示/隐藏";
                    LayerDisHid layerdis = new LayerDisHid();
                    layerdis.Show();
                    break;
                case "pointDis":
                    //barStaticItem1.Caption = "设备显示/隐藏";
                    PointDisHid pointdishid = new PointDisHid();
                    pointdishid.Show();
                    break;
                case "pointSercah":
                    //barStaticItem1.Caption = "设备查找";
                    PointSerach pointsercah = new PointSerach();
                    pointsercah.Show();
                    break;
                case "PageChange":
                    Param = e.p_sParam;
                    LoadMap(Param);
                    GraphOpt.IsTopologyInit = false;
                    break;
                case "AddMapRightMenu":
                    //添加右键菜单
                    //string pageListstr = "";
                    //IList<GraphicsbaseinfInfo> AllGraphicDto = GraphOpt.getAllGraphicDto();
                    //if (AllGraphicDto.Count > 0)
                    //{
                    //    for (int i = 0; i < AllGraphicDto.Count; i++)
                    //    {
                    //        pageListstr += AllGraphicDto[i].GraphName + "|";
                    //    }
                    //    if (pageListstr.Contains("|"))
                    //    {
                    //        pageListstr = pageListstr.Substring(0, pageListstr.Length - 1);
                    //    }
                    //    GraphOpt.AddMapRightMenu(mx, pageListstr);
                    //}
                    break;
                case "DoRefPointSsz":
                    try
                    {
                        GraphOpt.DoRefPointSsz(mx);
                    }
                    catch (Exception ex)
                    {
                        //DevExpress.XtraEditors.XtraMessageBox.Show("实时刷新数据失败，详细请查看错误日志！");
                        //barStaticItem1.Caption = "实时刷新数据失败，详细请查看错误日志！";
                        LogHelper.Error("mx_OnViewCallOutCommand_DoRefPointSsz" + ex.Message + ex.StackTrace);
                    }
                    break;
                case "PointDblClick":
                    string Point_Now = e.p_sParam.ToString().Split('|')[0];
                    string UnitName_Now = e.p_sParam.ToString().Split('|')[1];

                    GraphicspointsinfInfo PointDto = GraphOpt.getPointDto(Point_Now);
                    if (!string.IsNullOrEmpty(PointDto.Bz4))
                    {
                        GraphOpt.LoadMap(mx, PointDto.Bz4);
                    }

                    if (UnitName_Now.Contains("摄像头"))
                    {
                        //CFVideoInf videoForm = new CFVideoInf(Point_Now);
                        //videoForm.Show();
                    }
                    break;
            }

        }
        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GISPlatformCenter_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!GraphOpt.IsGraphicEditSave)
            //{
            //    if (DevExpress.XtraEditors.XtraMessageBox.Show("当前图形未保存，确定要退出吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //}

            if (GraphOpt.refPointssz._isRun)//如果实时刷新线程正在运行，退出实时刷新线程
            {
                GraphOpt.refPointssz.Stop();
            }
            //wdf = new WaitDialogForm("正在退出...", "请等待...");
            Thread.Sleep(1000);

            //if (wdf != null)
            //{
            //    wdf.Close();
            //}
        }

        /// <summary>
        /// 打开图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiShowPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "打开图形";
            if (!GraphOpt.IsGraphicEdit)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("正在预览，要停止吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            GraphOpt.IsGraphicEdit = true;

            GraphicsOpen GraphicsOpen = new GraphicsOpen();
            GraphicsOpen.ShowDialog();
            GraphOpt.IsTopologyInit = false;
        }
        /// <summary>
        /// 缩小图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "缩小图形";
            GraphOpt.ZoomIn(mx, "1");
        }
        /// <summary>
        /// 放大图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "放大图形";
            GraphOpt.ZoomOut(mx, "1");
        }
        /// <summary>
        /// 图形缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "图形缩放";
            GraphOpt.zoomExtent(mx);
        }
        /// <summary>
        /// 新建图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            // barStaticItem1.Caption = "新建图形";
            if (!GraphOpt.IsGraphicEdit)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("正在预览，要停止吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            GraphOpt.IsGraphicEdit = true;


            GraphicsAdd graphAdd = new GraphicsAdd();
            graphAdd.ShowDialog();
        }
        /// <summary>
        /// 底图更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            // barStaticItem1.Caption = "更新底图";
            if (GraphOpt.getGraphicNowType() == 0)
            {
                GraphicsAdd graphAdd = new GraphicsAdd();
                graphAdd.Text = "底图更新";
                graphAdd.ShowDialog();
            }
            else if (GraphOpt.getGraphicNowType() == -1)
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show("当前未打开图形，请先打开图形！");
                //barStaticItem1.Caption = "当前未打开图形，请先打开图形！";
            }
            else
            {
                //DevExpress.XtraEditors.XtraMessageBox.Show("拓扑图不支持底图更新！");
                // barStaticItem1.Caption = "拓扑图不支持底图更新！";
            }
        }
        /// <summary>
        /// 刷新图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "图形刷新";
            LoadMap(GraphOpt.GraphNameNow);
        }

        /// 保存图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "保存图形";
                if (GraphOpt.getGraphicNowType() != -1)
                {
                    GraphOpt.DoSaveAllPoints(mx);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("当前未打开图形，请先打开图形！");
                    //barStaticItem1.Caption = "当前未打开图形，请先打开图形！";
                }
                GraphOpt.IsTopologyInit = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem1_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 图层显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "图层显示/隐藏";
            LayerDisHid layerdis = new LayerDisHid();
            layerdis.Show();
        }
        /// <summary>
        /// 连S型线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "连S型线";
                //只有拓扑图才支持此命令
                if (GraphOpt.getGraphicNowType() == 1)
                {
                    GraphOpt.DoMapTopologyTransDef(mx, 0);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持拓扑图！");
                    //barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem7_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 连L型线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "连L型线";
                //只有拓扑图才支持此命令
                if (GraphOpt.getGraphicNowType() == 1)
                {
                    GraphOpt.DoMapTopologyTransDef(mx, 1);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持拓扑图！");
                    //barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem25_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 结束连线命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "结束拓扑连线";
                //只有拓扑图才支持此命令
                if (GraphOpt.getGraphicNowType() == 1)
                {
                    GraphOpt.EndMapTopologyTransDef(mx);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持拓扑图！");
                    //barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem26_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 巷道连接命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "巷道连线";
                //只有动态图才支持
                if (GraphOpt.getGraphicNowType() == 0)
                {
                    GraphOpt.DoMapTrajectoryDef(mx);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持动态图！");
                    //barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem27_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 实时刷新设备状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem28_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "图形预览";
                if (!GraphOpt.IsGraphicEditSave)
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("图形尚未保存，请先保存再进行图形预览！");
                    //barStaticItem1.Caption = "图形尚未保存，请先保存再进行图形预览！";
                    return;
                }

                GraphOpt.IsGraphicEdit = false;
                LoadMap(GraphOpt.GraphNameNow);
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem28_ItemClick_1" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 返回编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem29_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "结束预览";
                GraphOpt.IsGraphicEdit = true;
                LoadMap(GraphOpt.GraphNameNow);
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem29_ItemClick" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 设备显示隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "设备显示/隐藏";
            PointDisHid pointdishid = new PointDisHid();
            pointdishid.Show();
        }
        /// <summary>
        /// 根据定义信息自动生成拓扑图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem31_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "自动生成拓扑图";
                if (!GraphOpt.IsGraphicEdit)
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("当前图形正在预览，请先停止预览！");
                    // barStaticItem1.Caption = "当前图形正在预览，请先停止预览！";
                    return;
                }

                //只有拓扑图才支持此命令
                if (GraphOpt.getGraphicNowType() == 1)
                {
                    GraphOpt.AutoDragTopologyTrans(mx);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持拓扑图！");
                    // barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem31_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 结束巷道连线命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem32_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "结束巷道连线";
                //只有动态图才支持
                if (GraphOpt.getGraphicNowType() == 0)
                {
                    GraphOpt.EndMapTrajectoryDef(mx);
                }
                else
                {
                    //DevExpress.XtraEditors.XtraMessageBox.Show("此操作只支持动态图！");
                    //barStaticItem1.Caption = "此操作只支持拓扑图！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem32_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 打印图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "打印图形";
                GraphOpt.mapPrint(mx);
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem11_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //barStaticItem1.Caption = "导出图片";
                saveFileDialog1.FileName = GraphOpt.GraphNameNow;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    GraphOpt.mapToImage(mx, saveFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("barButtonItem20_ItemClick" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 切换图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (!GraphOpt.IsGraphicEdit)
            //{
            //    if (DevExpress.XtraEditors.XtraMessageBox.Show("正在预览，要停止吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        return;
            //    }
            //}

            //GraphOpt.IsGraphicEdit = true;
            //isExit = true;

            GraphicsOpen GraphicsOpen = new GraphicsOpen();
            GraphicsOpen.ShowDialog();
            GraphOpt.IsTopologyInit = false;
        }
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GISPlatformCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                //更新服务端标记
                var request = new SetSaveFlagRequest() { Flag = true };
                var response = graphicsbaseinfService.SetSaveFlag(request);

                _isRun = false;
                //释放控件
                //mx.Dispose();
                //释放控件
                //navBarGroupControlContainer4.Dispose();
                //navBarGroupControlContainer4 = null;
            }
            catch (Exception ex)
            {
                _isRun = false;
                //释放控件
                //mx.Dispose();
                //释放控件
                //navBarGroupControlContainer4.Dispose();
                //navBarGroupControlContainer4 = null;

                LogHelper.Error("GISPlatformCenter_GISPlatformCenter_FormClosed" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 设备定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barStaticItem1.Caption = "设备查找";
            PointSerach pointsercah = new PointSerach();
            pointsercah.ShowDialog();
        }

        public void RefMapData()
        {
            while (_isRun)
            {

                try
                {
                    //如果编辑工具改变了图形，则重新加载图形
                    var response = graphicsbaseinfService.GetSaveFlag();
                    if (response.Data)
                    {
                        #region 从数据库读取所有图形文件
                        GraphOpt.LoadGraphicsInfo1();
                        #endregion

                        LoadMap(GraphOpt.GraphNameNow);

                        var setRequest = new SetSaveFlagRequest() { Flag = false };
                        var setResponse = graphicsbaseinfService.SetSaveFlag(setRequest);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("GISPlatformCenter_RefMapData" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(3000);
            }
        }
        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            string result = GraphOpt.DoOtherDrawingGraphics(mx);
            if (!string.IsNullOrWhiteSpace(result))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result, "提示");
                return;
            }
            GraphOpt.DoDrawingPolygon(mx);
        }
        /// <summary>
        /// 清除区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GraphOpt.DoClearDrawing(mx);
        }
        /// <summary>
        /// 上传默认图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            AreaGraphicsAdd graphicsAdd = new AreaGraphicsAdd(GraphOpt);
            //判断是否存在默认图形
            if (GraphOpt.GraphNameNow.Length > 0) {
                graphicsAdd.Text = "图形编辑";
            }            
            graphicsAdd.Show();
        }
        /// <summary>
        /// 保存区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            string result = GraphOpt.DoSaveDrawing(mx);
            //XtraMessageBox.Show(result);
            if (string.IsNullOrEmpty(result)) {
                XtraMessageBox.Show("请在图形上面绘制一个区域信息！");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Jsonstr = result;
        }

        private void barButtonItem5_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            string AreaString = "[{\"pointX\":25942.022463,\"pointY\":-11007.051129},{\"pointX\":27314.093073,\"pointY\":-13410.25359},{\"pointX\":25559.505809,\"pointY\":-14424.754283},{\"pointX\":24461.849321,\"pointY\":-12470.593112}]";
            string type = "polygon";
            string AreaName = "101305工作面掘进巷道";
            GraphOpt.DoDrawinggGraphicsAndName(mx,AreaString, type, AreaName,"");
        }
    }


    public class Circle
    {
        public string pointX { get; set; }

        public string pointY { get; set; }

        public string radius { get; set; }
    }

    public class polygon
    {
        public string pointX { get; set; }

        public string pointY { get; set; }
    }
}
