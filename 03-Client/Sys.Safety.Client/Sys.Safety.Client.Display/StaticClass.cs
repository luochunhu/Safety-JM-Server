using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using Sys.Safety.Client.Display.Model;
using Sys.Safety.Client.Alarm;
using Sys.Safety.DataContract;
using System.Threading;
namespace Sys.Safety.Client.Display
{
    public class StaticClass
    {
        static StaticClass()
        {
            //根据配置动态判断显示配置是否自动从服务端获取  20180122
            string realDisplaySync = RealInterfaceFuction.ReadConfig("RealDisplaySync");
            if (realDisplaySync == "1")
            {
                ReadConfigFromDataBase = true;
            }
            else
            {
                ReadConfigFromDataBase = false;
            }
        }
        public static Int32 userid = 1;

        public static string Cs = "";

        /// <summary>
        /// 实时显示基础配置文件路径
        /// </summary>
        public static string RealDataCnfg = Application.StartupPath + "\\Config\\DisPlayCnfg\\RealDataCnfg.xml";

        /// <summary>
        /// 实时显示默认编排配置文件路径
        /// </summary>
        public static string RealDataDefalutCnfg = Application.StartupPath + "\\Config\\DisPlayCnfg\\RealDataDefalutCnfg.xml";

        /// <summary>
        /// 实时显示自定义编排配置文件路径
        /// </summary>
        public static string RealDataCustomCnfg = Application.StartupPath + "\\Config\\DisPlayCnfg\\RealDataCustomCnfg.xml";

        /// <summary>
        /// 实时显示配置信息
        /// </summary>
        public static RealDataDisplayConfig realdataconfig = new RealDataDisplayConfig();
        /// <summary>
        /// 编排显示配置信息
        /// </summary>
        public static PointArrangeConfig arrangeconfig = new PointArrangeConfig();

        /// <summary>
        /// 包含所有测点的总表
        /// </summary>
        public static DataTable AllPointDt = new DataTable();
        /// <summary>
        /// 测点总表锁对象  20170706
        /// </summary>
        public static object allPointDtLockObj = new object();

        /// <summary>
        /// 实时显示配置文件操作对象
        /// </summary>
        public static XmlConfig RealDataDisplayCnfgDoc = new XmlConfig(RealDataCnfg);

        /// <summary>
        /// 实时显示固定编排配置文件操作对象
        /// </summary>
        public static XmlConfig RealDataDisplayDefalutCnfgDoc = new XmlConfig(RealDataDefalutCnfg);

        /// <summary>
        /// 实时显示自定义编排配置文件操作对象
        /// </summary>
        public static XmlConfig RealDataDisplayCustomCnfgDoc = new XmlConfig(RealDataCustomCnfg);

        /// <summary>
        /// 实时显示日志对象
        /// </summary>
        public static Basic.Framework.Logging.Logger RealDataLog = new Basic.Framework.Logging.Logger();


        /// <summary>
        /// 设置的超时重新打开通道的时间间隔
        /// </summary>
        public static int ReConnectTimeout = 60;

        /// <summary>
        /// 超时重新打开通道即使器
        /// </summary>
        public static int TAG_ReConnectTimeout = 0;

        /// <summary>
        /// 是否已加载测点
        /// </summary>
        public static bool LoadLabel = false;

        /// <summary>
        /// 测点定义改变
        /// </summary>
        public static bool PointChange = false;

        /// <summary>
        /// 实时报警删除间隔时间
        /// </summary>
        public static int DeleteInterval = 60;

        /// <summary>
        /// 实时订阅
        /// </summary>
        public static int RealSubId = 0;

        /// <summary>
        /// 数据刷新时间
        /// </summary>
        public static DateTime RefreshTime = Model.RealInterfaceFuction.GetServerNowTime();

        /// <summary>
        /// 获取报警信息线程
        /// </summary>
        public static Thread GetbjThread;

        /// <summary>
        /// 实时显示窗口
        /// </summary>
        public static RealDisplayForm real_s;

        /// <summary>
        /// 导航窗口
        /// </summary>
        public static DisplayNavagation _type_s;

        /// <summary>
        /// 设置窗口
        /// </summary>
        public static RealDatadisplaySetForm _realSetForm;

        /// <summary>
        /// 定义改变时间
        /// </summary>
        public static string DefineTime = "";

        /// <summary>
        /// 定义改变标记
        /// </summary>
        public static bool Definechange = false;

        /// <summary>
        /// 是否启用模糊查找
        /// </summary>
        public static bool fuzzyserch = false;

        /// <summary>
        /// 模糊查找条件
        /// </summary>
        public static string fuzzyserchtext = "";

        /// <summary>
        /// 显示配置改变时间
        /// </summary>
        public static string RealCfgTime = "";

        /// <summary>
        /// 预警页面
        /// </summary>
        public static RealyjControl YJForm;

        /// <summary>
        /// 报警页面
        /// </summary>
        public static RealBJControl MNLBJForm;

        /// <summary>
        /// 开关量报警页面
        /// </summary>
        public static RealKglBjControl KGLBJForm;

        /// <summary>
        /// 初始显示列
        /// </summary>
        public static string[] showcol = new string[] { "标签名", "安装位置", "实时值", "数据状态", "设备状态", "设备性质", "上限预警", "上限报警", "上限断电", "上限复电", "下限预警", "下限报警", "下限断电", "下限复电", "设备种类", "设备类型", "电压", "分级报警" };

        /// <summary>
        /// 馈电异常页面
        /// </summary>
        public static RealKDYCControl KDYCForm;


        /// <summary>
        /// 控制页面
        /// </summary>
        public static RealKZControl KZForm;


        /// <summary>
        /// 开关量状态变动页面
        /// </summary>
        public static RealKGLBDControl KGLBDForm;

        /// <summary>
        ///手动控制页面
        /// </summary>
        public static RealHandControl RealHandForm;

        /// <summary>
        ///运行记录页面
        /// </summary>
        public static RealRunRecordControl realRunRecordControl;
        /// <summary>
        ///自动挂接页面
        /// </summary>
        public static AutomaticArtControl automaticArtControl;

        /// <summary>
        /// 断电页面
        /// </summary>
        public static RealDDControl MNLDDForm;

        public static frmAlarmBgd FrmAlarmBGD;

        #region nls
        public static dynamic RealChart;
        public static dynamic HisChart;

        #endregion

        public delegate void mydel(int type, int n, List<string> list);

        public static mydel real_del;

        public static dele _type_dele;

        public delegate void dele();

        public delegate void mydel1(int n);

        public static mydel1 updatefromtext;

        public static void iniform()
        {
            try
            {
                string state;

                #region 从本地读取配置信息到内存
                string realDisplaySync = RealInterfaceFuction.ReadConfig("RealDisplaySync");
                if (realDisplaySync == "1")
                {
                    OprFuction.ReadRealConfigFromDB();//修改支持多个客户端显示设置同步  20180113
                }
                OprFuction.ReadRealDataDisplayConfig();
                OprFuction.ReadDefalutDataConfig();
                OprFuction.ReadCustomConfig();
                #endregion

                lock (StaticClass.allPointDtLockObj)
                {
                    StaticClass.AllPointDt = RealInterfaceFuction.GetAllPoint();

                    for (int i = 0; i < StaticClass.AllPointDt.Rows.Count; i++)
                    {
                        state = StaticClass.AllPointDt.Rows[i]["zt"].ToString();
                        if (state == StaticClass.itemStateToClient.EqpState24.ToString() || state == StaticClass.itemStateToClient.EqpState43.ToString())
                        {
                            StaticClass.AllPointDt.Rows[i]["statecolor"] =
                                 StaticClass.AllPointDt.Rows[i]["sszcolor"] =
                                OprFuction.GetShowColor(state,
                                StaticClass.AllPointDt.Rows[i]["0tcolor"].ToString(),
                                StaticClass.AllPointDt.Rows[i]["bj"].ToString());
                        }
                        else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
                        {
                            StaticClass.AllPointDt.Rows[i]["statecolor"] =
                                 StaticClass.AllPointDt.Rows[i]["sszcolor"] =
                                OprFuction.GetShowColor(state, StaticClass.AllPointDt.Rows[i]["1tcolor"].ToString(),
                                 StaticClass.AllPointDt.Rows[i]["bj"].ToString());
                        }
                        else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
                        {
                            StaticClass.AllPointDt.Rows[i]["statecolor"] =
                                 StaticClass.AllPointDt.Rows[i]["sszcolor"] =
                                OprFuction.GetShowColor(state, StaticClass.AllPointDt.Rows[i]["2tcolor"].ToString(),
                                 StaticClass.AllPointDt.Rows[i]["bj"].ToString());
                        }
                        else
                        {
                            StaticClass.AllPointDt.Rows[i]["statecolor"] =
                                 StaticClass.AllPointDt.Rows[i]["sszcolor"] =
                               OprFuction.GetShowColor(state, "0", StaticClass.AllPointDt.Rows[i]["bj"].ToString());
                        }
                    }
                }
                real_s = new RealDisplayForm();

                real_s.Dock = DockStyle.Fill;

                real_del = real_s.Gv_init;

                _type_s = new DisplayNavagation();

                _type_s.Dock = DockStyle.Fill;

                _type_dele = _type_s.refresh;

                GetbjThread = new Thread(new ThreadStart(OprFuction.GetbjTh));
                GetbjThread.Start();

                InitRealWarGridControl();
                //Initalarm();

            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }


        public delegate void mydell1(string num);
        public static mydell1 dell;

        public static void formshow(string shownum)
        {
            if (dell != null)
            {
                dell(shownum);
            }
        }
        /// <summary>
        /// 添加委托
        /// </summary>
        public static void adddelegate()
        {
            if (real_s == null)
            {
                real_del = real_s.Gv_init;
            }
            if (_type_s == null)
            {
                _type_dele = _type_s.refresh;
            }
        }


        /// <summary>
        /// 初始化报警
        /// </summary>
        public static void Initalarm()
        {
            try
            {
                FrmAlarmBGD = new frmAlarmBgd();
                FrmAlarmBGD.Visible = false;
                FrmAlarmBGD.Show();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 取消当前所有报警
        /// </summary>
        public static void CancelAlarm()
        {
            try
            {
                FrmAlarmBGD.CancleAlarm();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 取消当前所有报警
        /// </summary>
        public static void OpenAlarm()
        {
            try
            {
                FrmAlarmBGD.OpenAlarm();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        public static void CloseAlarmForm()
        {
            try
            {
                if (FrmAlarmBGD != null)
                {
                    FrmAlarmBGD.Close();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private static void InitRealWarGridControl()
        {
            YJForm = new RealyjControl();
            YJForm.Dock = DockStyle.Fill;
            MNLBJForm = new RealBJControl();
            MNLBJForm.Dock = DockStyle.Fill;
            KGLBJForm = new RealKglBjControl();
            KGLBJForm.Dock = DockStyle.Fill;
            KDYCForm = new RealKDYCControl();
            KDYCForm.Dock = DockStyle.Fill;
            KZForm = new RealKZControl();
            KZForm.Dock = DockStyle.Fill;
            KGLBDForm = new RealKGLBDControl();
            KGLBDForm.Dock = DockStyle.Fill;
            RealHandForm = new RealHandControl();
            RealHandForm.Dock = DockStyle.Fill;
            realRunRecordControl = new RealRunRecordControl();
            realRunRecordControl.Dock = DockStyle.Fill;
            automaticArtControl = new AutomaticArtControl();
            automaticArtControl.Dock = DockStyle.Fill;

            MNLDDForm = new RealDDControl();
            MNLDDForm.Dock = DockStyle.Fill;
        }


        /// <summary>
        /// 存储所有报警数据
        /// </summary>
        public static Dictionary<long, Jc_BInfo> jcbdata = new Dictionary<long, Jc_BInfo>();

        public static bool haveinit = false;

        /// <summary>
        /// 报警访问锁
        /// </summary>
        public static object bjobj = new object();

        /// <summary>
        /// 初始时间
        /// </summary>
        public static string InitTime = "1900-01-01 00:00:00";

        /// <summary>
        /// 服务端是否连接成功
        /// </summary>
        public static bool ServerConet = true;

        /// <summary>
        /// 服务端连接中断时间
        /// </summary>
        public static DateTime ServerConnetInrTime;

        /// <summary>
        /// 显示配置是否读数据库
        /// </summary>
        public static bool ReadConfigFromDataBase = true;//修改支持多个客户端显示设置同步  20180113

        /// <summary>
        /// 下标0-新增预警数 1-模拟量报警断电 2-开关量报警断电 3-馈电异常 4-控制 5-变动 6-手动控制7-自动挂接
        /// </summary>
        public static long[] yccount = new long[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// 系统退出
        /// </summary>
        public static bool SystemOut = false;

        /// <summary>
        /// 设备状态 传输到客户端显示
        /// </summary>
        public static StateToClient itemStateToClient = new StateToClient();


        #region 远程升级相关 20161124
        public static UInt32[] crc32table = new UInt32[] {    
 0x00000000, 0x04c11db7, 0x09823b6e, 0x0d4326d9, 0x130476dc, 0x17c56b6b,    
 0x1a864db2, 0x1e475005, 0x2608edb8, 0x22c9f00f, 0x2f8ad6d6, 0x2b4bcb61,    
 0x350c9b64, 0x31cd86d3, 0x3c8ea00a, 0x384fbdbd, 0x4c11db70, 0x48d0c6c7,    
 0x4593e01e, 0x4152fda9, 0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75,    
 0x6a1936c8, 0x6ed82b7f, 0x639b0da6, 0x675a1011, 0x791d4014, 0x7ddc5da3,    
 0x709f7b7a, 0x745e66cd, 0x9823b6e0, 0x9ce2ab57, 0x91a18d8e, 0x95609039,    
 0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5, 0xbe2b5b58, 0xbaea46ef,    
 0xb7a96036, 0xb3687d81, 0xad2f2d84, 0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d,    
 0xd4326d90, 0xd0f37027, 0xddb056fe, 0xd9714b49, 0xc7361b4c, 0xc3f706fb,    
 0xceb42022, 0xca753d95, 0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1,    
 0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d, 0x34867077, 0x30476dc0,    
 0x3d044b19, 0x39c556ae, 0x278206ab, 0x23431b1c, 0x2e003dc5, 0x2ac12072,    
 0x128e9dcf, 0x164f8078, 0x1b0ca6a1, 0x1fcdbb16, 0x018aeb13, 0x054bf6a4,    
 0x0808d07d, 0x0cc9cdca, 0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde,    
 0x6b93dddb, 0x6f52c06c, 0x6211e6b5, 0x66d0fb02, 0x5e9f46bf, 0x5a5e5b08,    
 0x571d7dd1, 0x53dc6066, 0x4d9b3063, 0x495a2dd4, 0x44190b0d, 0x40d816ba,    
 0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e, 0xbfa1b04b, 0xbb60adfc,    
 0xb6238b25, 0xb2e29692, 0x8aad2b2f, 0x8e6c3698, 0x832f1041, 0x87ee0df6,    
 0x99a95df3, 0x9d684044, 0x902b669d, 0x94ea7b2a, 0xe0b41de7, 0xe4750050,    
 0xe9362689, 0xedf73b3e, 0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2,    
 0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686, 0xd5b88683, 0xd1799b34,    
 0xdc3abded, 0xd8fba05a, 0x690ce0ee, 0x6dcdfd59, 0x608edb80, 0x644fc637,    
 0x7a089632, 0x7ec98b85, 0x738aad5c, 0x774bb0eb, 0x4f040d56, 0x4bc510e1,    
 0x46863638, 0x42472b8f, 0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53,    
 0x251d3b9e, 0x21dc2629, 0x2c9f00f0, 0x285e1d47, 0x36194d42, 0x32d850f5,    
 0x3f9b762c, 0x3b5a6b9b, 0x0315d626, 0x07d4cb91, 0x0a97ed48, 0x0e56f0ff,    
 0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623, 0xf12f560e, 0xf5ee4bb9,    
 0xf8ad6d60, 0xfc6c70d7, 0xe22b20d2, 0xe6ea3d65, 0xeba91bbc, 0xef68060b,    
 0xd727bbb6, 0xd3e6a601, 0xdea580d8, 0xda649d6f, 0xc423cd6a, 0xc0e2d0dd,    
 0xcda1f604, 0xc960ebb3, 0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7,    
 0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b, 0x9b3660c6, 0x9ff77d71,    
 0x92b45ba8, 0x9675461f, 0x8832161a, 0x8cf30bad, 0x81b02d74, 0x857130c3,    
 0x5d8a9099, 0x594b8d2e, 0x5408abf7, 0x50c9b640, 0x4e8ee645, 0x4a4ffbf2,    
 0x470cdd2b, 0x43cdc09c, 0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8,    
 0x68860bfd, 0x6c47164a, 0x61043093, 0x65c52d24, 0x119b4be9, 0x155a565e,    
 0x18197087, 0x1cd86d30, 0x029f3d35, 0x065e2082, 0x0b1d065b, 0x0fdc1bec,    
 0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088, 0x2497d08d, 0x2056cd3a,    
 0x2d15ebe3, 0x29d4f654, 0xc5a92679, 0xc1683bce, 0xcc2b1d17, 0xc8ea00a0,    
 0xd6ad50a5, 0xd26c4d12, 0xdf2f6bcb, 0xdbee767c, 0xe3a1cbc1, 0xe760d676,    
 0xea23f0af, 0xeee2ed18, 0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4,    
 0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0, 0x9abc8bd5, 0x9e7d9662,    
 0x933eb0bb, 0x97ffad0c, 0xafb010b1, 0xab710d06, 0xa6322bdf, 0xa2f33668,    
 0xbcb4666d, 0xb8757bda, 0xb5365d03, 0xb1f740b4
};

        /// <summary>
        /// 是否正在下发文件升级 tanxingyan 20161024
        /// </summary>
        public static bool IsUpdateFile;

        public static bool cgqbm;

        /// <summary>
        /// 是否已开始远程升级
        /// </summary>
        public static bool isupdate;

        /// <summary>
        /// 文件的crc校验码
        /// </summary>
        public static UInt32 UpdateCrc;
        /// <summary>
        /// 远程升级文件buffer tanxingyan 20161017
        /// </summary>
        public static byte[] UpdateBuffer;

        /// <summary>
        /// 远程升级文件下发总帧数 tanxingyan 20161017
        /// </summary>
        public static int UpdateCount;

        /// <summary>
        /// 上次下发时间
        /// </summary>
        public static DateTime UpdateTime;

        /// <summary>
        /// 升级文件路径 tanxingyan 20161017
        /// </summary>
        public static string UpdateFile;

        /// <summary>
        /// 升级文件版本 tanxingyan 20161018
        /// </summary>
        public static byte UpdateBb;

        /// <summary>
        /// 升级的硬件程序版本 tanxingyan 20161018
        /// </summary>
        public static byte UpdateYBb;

        /// <summary>
        /// 升级的设备类型 tanxingyan 20161018
        /// </summary>
        public static byte UpdatetTypeid;

        /// <summary>
        /// 升级版本上限 tanxingyan 20161021
        /// </summary>
        public static byte UpdateBbXS;

        /// <summary>
        /// 升级版本下限tanxingyan 20161021
        /// </summary>
        public static byte UpageBbXX;

        /// <summary>
        /// 发送频率 tanxingyan 20161018
        /// </summary>
        public static int UpdatePL;

        /// <summary>
        /// 当前已下发帧数 tanxingyan 20161021
        /// </summary>
        public static uint CurrentIndex;

        public static List<byte> UpdateFzh = new List<byte>();

        #endregion

    }
}
