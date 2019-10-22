using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Enums;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class GlobalConfigInfo
    {
        //TODO:原有系统的配置
        ///// <summary>
        ///// 系统配置信息，需要保存到配置文件
        ///// </summary>
        //public static GlobalCnfg GlobalInstance = new GlobalCnfg();
        ///// <summary>
        ///// 源码显示全局变量-内含发送和接收的缓冲
        ///// </summary>
        //public static GlobalCommCode GlobalCommCodeInstance = new GlobalCommCode();
        ///// <summary>
        ///// 设备状态用于存储数据库
        ///// </summary>
        public static StateToDB itemStateInstance = new StateToDB();
        ///// <summary>
        ///// 保存报警数据
        ///// </summary>
        //public static ArrayList GlobalAlarmData = new ArrayList();
        ///// <summary>
        ///// 保存修改的记录
        ///// </summary>
        //public static ArrayList GlobalUpdateAlarmData = new ArrayList();
        ///// <summary>
        ///// 保存五分钟数据
        ///// </summary>
        //public static ArrayList GlobalFiveData = new ArrayList();
        ///// <summary>
        ///// 保存运行数据
        ///// </summary>
        //public static ArrayList GlobalRunData = new ArrayList();
        ///// <summary>
        ///// 保存密采数据
        ///// </summary>
        //public static ArrayList GlobalMCData = new ArrayList();
        ///// <summary>
        ///// 保存实时数据
        ///// </summary>
        //public static ArrayList GlobalRealData = new ArrayList();
        ///// <summary>
        ///// 抽放数据
        ///// </summary>
        //public static ArrayList GlobalLLData = new ArrayList();//txy 20160818

        ///// <summary>
        ///// 馈电表数据
        ///// </summary>
        //public static ArrayList GlobalKDData = new ArrayList();//txy 20160818

        ///// <summary>
        ///// 网络模块实时数据
        ///// </summary>
        //public static ArrayList GolbalMacData = new ArrayList();

        ///// <summary>
        ///// 日志表
        ///// </summary>
        //public static ArrayList GlobalLog = new ArrayList();

        ///// <summary>
        ///// 人员历史轨迹表
        ///// </summary>
        //public static ArrayList GlobalRPData = new ArrayList();

        /// <summary>
        /// 运行日志队列
        /// </summary>
        public static Queue<string> Runlog = new Queue<string>();

        /// <summary>
        /// 存储所有交叉控制信息
        /// </summary>
        public static bool Jckzrk = false;
        /// <summary>
        /// 用于五分钟加锁对象
        /// </summary>
        public static object lockflag = new object();

        /// <summary>
        /// 是否需要进行通讯线程切换
        /// </summary>
        public static bool ChangeCommunicationThreadFlag = false;
        /// <summary>
        /// 将日志添加的链表
        /// </summary>
        /// <param name="log"></param>
        public static void AddLogs(string log)
        {
            try
            {
                string logs = string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log);
                if (Runlog.Count < 500)
                {
                    Runlog.Enqueue(logs);
                }
                else
                {
                    Runlog.Clear();
                    Runlog.Enqueue(logs);
                }
            }
            catch
            { }
        }

        #region 远程升级相关 唯一编码相关参数 tanxingyan 20161124
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
        /// 是否正在下发文件升级 
        /// </summary>
        public static bool IsUpdateFile;

        public static bool cgqbm;

        /// <summary>
        /// 是否已进行远程升级
        /// </summary>
        public static bool isupdate;

        /// <summary>
        /// 文件的crc校验码
        /// </summary>
        public static UInt32 UpdateCrc;
        /// <summary>
        /// 远程升级文件buffer 
        /// </summary>
        public static byte[] UpdateBuffer;

        /// <summary>
        /// 远程升级文件下发总帧数 
        /// </summary>
        public static int UpdateCount;

        /// <summary>
        /// 上次下发时间
        /// </summary>
        public static DateTime UpdateTime;

        /// <summary>
        /// 升级文件路径 
        /// </summary>
        public static string UpdateFile;

        /// <summary>
        /// 升级文件版本 
        /// </summary>
        public static byte UpdateBb;

        /// <summary>
        /// 升级的硬件程序版本
        /// </summary>
        public static byte UpdateYBb;

        /// <summary>
        /// 升级的设备类型 
        /// </summary>
        public static byte UpdatetTypeid;

        /// <summary>
        /// 升级版本上限 
        /// </summary>
        public static byte UpdateBbXS;

        /// <summary>
        /// 升级版本下限
        /// </summary>
        public static byte UpageBbXX;

        /// <summary>
        /// 发送频率 
        /// </summary>
        public static int UpdatePL;

        /// <summary>
        /// 当前已下发帧数 
        /// </summary>
        public static uint CurrentIndex;

        public static List<byte> UpdateFzh = new List<byte>();

        /// <summary>
        /// 根据帧数填充当前升级文件片段tanxingyan 20161124
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static void GetUpdateByte(byte[] buf, int sindex, uint indexid)
        {
            uint ist = (indexid - 1) * 256;
            for (int i = 0; i < 256; i++)
            {
                buf[sindex++] = UpdateBuffer[ist + i];
            }
        }

        public static byte[] GETU(uint indexid)//tanxingyan 20161124
        {
            byte[] sbuf = new byte[269];
            int index = 0;
            int cd = 265;
            ushort ljh = 0;
            sbuf[index++] = 0x7e;
            sbuf[index++] = 0xfc;
            sbuf[index++] = (byte)(cd >> 8);//长度高
            sbuf[index++] = (byte)cd;//长度低
            sbuf[index++] = 0x55;
            sbuf[index++] = 0x00;
            //设备类型编码1
            sbuf[index++] = UpdatetTypeid;
            //硬件编码1
            sbuf[index++] = UpdateYBb;
            //升级文件版本号1
            sbuf[index++] = UpdateBb;
            sbuf[index++] = (byte)(indexid >> 8);
            sbuf[index++] = (byte)indexid;
            GetUpdateByte(sbuf, index, indexid);
            for (int i = 0; i < (sbuf.Length - 3); i++) ljh += (Byte)sbuf[1 + i];//从分站号开始计算
            sbuf[sbuf.Length - 1] = (Byte)(ljh & 0x00ff);
            ljh >>= 8;
            sbuf[sbuf.Length - 2] = (Byte)(ljh & 0x00ff);
            return sbuf;
        }


        #endregion


    }

    /// <summary>
    /// 状态编码
    /// </summary>
    public class StateToDB
    {

        /// <summary>
        /// 通讯中断
        /// </summary>
        private const byte txzd_State0 = 0;
        /// <summary>
        /// 通讯中断
        /// </summary>
        public byte Txzd_State0
        {
            get { return txzd_State0; }
        }
        /// <summary>
        /// 通讯误码
        /// </summary>
        private const byte txwm_State1 = 1;
        /// <summary>
        /// 通讯误码
        /// </summary>
        public byte Txwm_State1
        {
            get { return txwm_State1; }
        }
        /// <summary>
        /// 初始化中
        /// </summary>
        private const byte cshz_State2 = 2;
        /// <summary>
        /// 初始化中
        /// </summary>
        public byte Cshz_State2
        {
            get { return cshz_State2; }
        }
        /// <summary>
        /// 交流正常
        /// </summary>
        private const byte jlzc_State3 = 3;
        /// <summary>
        /// 交流正常
        /// </summary>
        public byte Jlzc_State3
        {
            get { return jlzc_State3; }
        }
        /// <summary>
        /// 直流正常
        /// </summary>
        private const byte zlzc_State4 = 4;
        /// <summary>
        /// 直流正常
        /// </summary>
        public byte Zlzc_State4
        {
            get { return zlzc_State4; }
        }
        /// <summary>
        /// 红外遥控
        /// </summary>
        private const byte hwyk_State5 = 5;
        /// <summary>
        /// 红外遥控
        /// </summary>
        public byte Hwyk_State5
        {
            get { return hwyk_State5; }
        }
        /// <summary>
        /// 设备休眠
        /// </summary>
        private const byte sbxm_State6 = 6;
        /// <summary>
        /// 设备休眠
        /// </summary>
        public byte Sbxm_State6
        {
            get { return sbxm_State6; }
        }
        /// <summary>
        /// 设备检修
        /// </summary>
        private const byte sbjx_State7 = 7;
        /// <summary>
        /// 设备检修
        /// </summary>
        public byte Sbjx_State7
        {
            get { return sbjx_State7; }
        }
        /// <summary>
        /// 上限预警
        /// </summary>
        private const byte sxyj_State8 = 8;
        /// <summary>
        /// 上限预警
        /// </summary>
        public byte Sxyj_State8
        {
            get { return sxyj_State8; }
        }
        /// <summary>
        /// 预警解除
        /// </summary>
        private const byte yjjc_State9 = 9;
        /// <summary>
        /// 预警解除
        /// </summary>
        public byte Yjjc_State9
        {
            get { return yjjc_State9; }
        }
        /// <summary>
        /// 上限报警
        /// </summary>
        private const byte sxbj_State10 = 10;
        /// <summary>
        /// 上限报警
        /// </summary>
        public byte Sxbj_State10
        {
            get { return sxbj_State10; }
        }
        /// <summary>
        /// 报警解除
        /// </summary>
        private const byte bjjc_State11 = 11;
        /// <summary>
        /// 报警解除
        /// </summary>
        public byte Bjjc_State11
        {
            get { return bjjc_State11; }
        }
        /// <summary>
        /// 上限断电
        /// </summary>
        private const byte sxdd_State12 = 12;
        /// <summary>
        /// 上限断电
        /// </summary>
        public byte Sxdd_State12
        {
            get { return sxdd_State12; }
        }
        /// <summary>
        /// 断电解除
        /// </summary>
        private const byte ddjc_State13 = 13;
        /// <summary>
        /// 断电解除
        /// </summary>
        public byte Ddjc_State13
        {
            get { return ddjc_State13; }
        }
        /// <summary>
        /// 下限预警
        /// </summary>
        private const byte xxyj_State14 = 14;
        /// <summary>
        /// 下限预警
        /// </summary>
        public byte Xxyj_State14
        {
            get { return xxyj_State14; }
        }
        /// <summary>
        /// 下预解除
        /// </summary>
        private const byte xyjc_State15 = 15;
        /// <summary>
        /// 下预解除
        /// </summary>
        public byte Xyjc_State15
        {
            get { return xyjc_State15; }
        }
        /// <summary>
        /// 下限报警
        /// </summary>
        private const byte xxbj_State16 = 16;
        /// <summary>
        /// 下限报警
        /// </summary>
        public byte Xxbj_State16
        {
            get { return xxbj_State16; }
        }
        /// <summary>
        /// 下报解除
        /// </summary>
        private const byte xbjc_State17 = 17;
        /// <summary>
        /// 下报解除
        /// </summary>
        public byte Xbjc_State17
        {
            get { return xbjc_State17; }
        }
        /// <summary>
        /// 下限断电
        /// </summary>
        private const byte xxdd_State18 = 18;
        /// <summary>
        /// 下限断电
        /// </summary>
        public byte Xxdd_State18
        {
            get { return xxdd_State18; }
        }
        /// <summary>
        /// 下断解除
        /// </summary>
        private const byte xdjc_State19 = 19;
        /// <summary>
        /// 下断解除
        /// </summary>
        public byte Xdjc_State19
        {
            get { return xdjc_State19; }
        }
        /// <summary>
        /// 断线
        /// </summary>
        private const byte dx_State20 = 20;
        /// <summary>
        /// 断线
        /// </summary>
        public byte Dx_State20
        {
            get { return dx_State20; }
        }
        /// <summary>
        /// 正常
        /// </summary>
        private const byte zc_State21 = 21;
        /// <summary>
        /// 正常
        /// </summary>
        public byte Zc_State21
        {
            get { return zc_State21; }
        }
        /// <summary>
        /// 上溢
        /// </summary>
        private const byte sy_State22 = 22;
        /// <summary>
        /// 上溢
        /// </summary>
        public byte Sy_State22
        {
            get { return sy_State22; }
        }
        /// <summary>
        /// 负漂
        /// </summary>
        private const byte fp_State23 = 23;
        /// <summary>
        /// 负漂
        /// </summary>
        public byte Fp_State23
        {
            get { return fp_State23; }
        }
        /// <summary>
        /// 标校
        /// </summary>
        private const byte bx_State24 = 24;
        /// <summary>
        /// 标校
        /// </summary>
        public byte Bx_State24
        {
            get { return bx_State24; }
        }
        /// <summary>
        /// 0态
        /// </summary>
        private const byte lt_State25 = 25;
        /// <summary>
        /// 0态
        /// </summary>
        public byte Lt_State25
        {
            get { return lt_State25; }
        }
        /// <summary>
        /// 1态
        /// </summary>
        private const byte yt_State26 = 26;
        /// <summary>
        /// 1态
        /// </summary>
        public byte Yt_State26
        {
            get { return yt_State26; }
        }
        /// <summary>
        /// 2态
        /// </summary>
        private const byte et_State27 = 27;
        /// <summary>
        /// 2态
        /// </summary>
        public byte Et_State27
        {
            get { return et_State27; }
        }
        /// <summary>
        /// 开机中
        /// </summary>
        private const byte kjz_State28 = 28;
        /// <summary>
        /// 开机中
        /// </summary>
        public byte Kjz_State28
        {
            get { return kjz_State28; }
        }
        /// <summary>
        /// 复电成功
        /// </summary>
        private const byte fdSuccess_State29 = 29;
        /// <summary>
        /// 复电成功
        /// </summary>
        public byte FdSuccess_State29
        {
            get { return fdSuccess_State29; }
        }
        /// <summary>
        /// 复电失败
        /// </summary>
        private const byte fdFailed_State30 = 30;
        /// <summary>
        /// 复电失败
        /// </summary>
        public byte FdFailed_State30
        {
            get { return fdFailed_State30; }
        }
        /// <summary>
        /// 断电成功
        /// </summary>
        private const byte ddSuccess_State31 = 31;
        /// <summary>
        /// 断电成功
        /// </summary>
        public byte DdSuccess_State31
        {
            get { return ddSuccess_State31; }
        }
        /// <summary>
        /// 断电失败
        /// </summary>
        private const byte ddFailed_State32 = 32;
        /// <summary>
        /// 断电失败
        /// </summary>
        public byte DdFailed_State32
        {
            get { return ddFailed_State32; }
        }
        /// <summary>
        /// 头子断线
        /// </summary>
        private const byte tzdx_State33 = 33;
        /// <summary>
        /// 头子断线
        /// </summary>
        public byte Tzdx_State33
        {
            get { return tzdx_State33; }
        }
        /// <summary>
        /// 类型有误
        /// </summary>
        private const byte lxyw_State34 = 34;
        /// <summary>
        /// 类型有误
        /// </summary>
        public byte Lxyw_State34
        {
            get { return lxyw_State34; }
        }
        /// <summary>
        /// 系统退出
        /// </summary>
        private const byte xttc_State35 = 35;
        /// <summary>
        /// 系统退出
        /// </summary>
        public byte Xttc_State35
        {
            get { return xttc_State35; }
        }
        /// <summary>
        /// 系统启动
        /// </summary>
        private const byte xtqd_State36 = 36;
        /// <summary>
        /// 系统启动
        /// </summary>
        public byte Xtqd_State36
        {
            get { return xtqd_State36; }
        }
        /// <summary>
        /// 非法退出
        /// </summary>
        private const byte fftc_State37 = 37;
        /// <summary>
        /// 非法退出
        /// </summary>
        public byte Fftc_State37
        {
            get { return fftc_State37; }
        }
        /// <summary>
        /// 过滤数据
        /// </summary>
        private const byte glsj_State38 = 38;
        /// <summary>
        /// 过滤数据
        /// </summary>
        public byte Glsj_State38
        {
            get { return glsj_State38; }
        }
        /// <summary>
        /// 热备日志
        /// </summary>
        private const byte rbrz_State39 = 39;
        /// <summary>
        /// 热备日志
        /// </summary>
        public byte Rbrz_State39
        {
            get { return rbrz_State39; }
        }
        /// <summary>
        /// 满足条件
        /// </summary>
        private const byte mztj_State40 = 40;
        /// <summary>
        /// 满足条件
        /// </summary>
        public byte Mztj_State40
        {
            get { return mztj_State40; }
        }
        /// <summary>
        /// 不满足条件
        /// </summary>
        private const byte bmztj_State41 = 41;
        /// <summary>
        /// 不满足条件
        /// </summary>
        public byte Bmztj_State41
        {
            get { return bmztj_State41; }
        }
        /// <summary>
        /// 传感器线性突变
        /// </summary>
        private const byte xxtb_State42 = 42;
        /// <summary>
        /// 传感器线性突变
        /// </summary>
        public byte Xxtb_State42
        {
            get { return xxtb_State42; }
        }

        /// <summary>
        /// 控制量0态
        /// </summary>
        private const byte kzllt_State43 = 43;
        /// <summary>
        /// 控制量0态
        /// </summary>
        public byte Kzllt_State43
        {
            get { return kzllt_State43; }
        }
        /// <summary>
        /// 控制量1态
        /// </summary>
        private const byte kzlyt_State44 = 44;
        /// <summary>
        /// 控制量1态
        /// </summary>
        public byte Kzlyt_State44
        {
            get { return kzlyt_State44; }
        }
        /// <summary>
        /// 控制量断线
        /// </summary>
        private const byte kzldx_State45 = 45;
        /// <summary>
        /// 控制量断线
        /// </summary>
        public byte Kzldx_State45
        {
            get { return kzldx_State45; }
        }
        /// <summary>
        /// 设备状态未知(历史表中不会出现该状态，用于实时显示部分表示对应分站通讯中断后传感器的状态)
        /// </summary>
        private const byte wz_State46 = 46;
        /// <summary>
        /// 设备状态未知(历史表中不会出现该状态，用于实时显示部分表示对应分站通讯中断后传感器的状态)
        /// </summary>
        public byte Wz_State46
        {
            get { return wz_State46; }
        }

        public string GetValueFromState(byte StateToDB)
        {
            string strValue = "";
            if (StateToDB < 0)
            {
                return "";
            }
            if (StateToDB == txzd_State0)
            {
                strValue = "通讯中断";
            }
            else if (StateToDB == txwm_State1)
            {
                strValue = "通讯误码";
            }
            else if (StateToDB == cshz_State2)
            {
                strValue = "初始化中";
            }
            else if (StateToDB == jlzc_State3)
            {
                strValue = "交流正常";
            }
            else if (StateToDB == zlzc_State4)
            {
                strValue = "直流正常";
            }
            else if (StateToDB == hwyk_State5)
            {
                strValue = "红外遥控";
            }
            else if (StateToDB == sbxm_State6)
            {
                strValue = "设备休眠";
            }
            else if (StateToDB == sbjx_State7)
            {
                strValue = "设备检修";
            }
            else if (StateToDB == sy_State22)
            {
                strValue = "上溢";
            }
            else if (StateToDB == fp_State23)
            {
                strValue = "负漂";
            }
            else if (StateToDB == dx_State20)
            {
                strValue = "断线";
            }
            else if (StateToDB == fdFailed_State30)
            {
                strValue = "复电失败";
            }
            else if (StateToDB == ddFailed_State32)
            {
                strValue = "断电失败";
            }
            else if (StateToDB == fdSuccess_State29)
            {
                strValue = "复电成功";
            }
            else if (StateToDB == ddSuccess_State31)
            {
                strValue = "断电成功";
            }
            return strValue;
        }
    }
}
