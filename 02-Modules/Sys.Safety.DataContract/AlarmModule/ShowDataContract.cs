using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public class ShowDataInfo
    {      
        public long ID { get; set; }
        public long Counter { get; set; }
        public int Fzh { get; set; }        
        public int Kh { get; set; }        
        public string Devid { get; set; }        
        public string Wzid { get; set; }
        /// <summary>
        /// 设备性质编号
        /// </summary>        
        public int Property { get; set; }
        /// <summary>
        /// 设备种类编号
        /// </summary>        
        public int Class { get; set; }
        /// <summary>
        /// 数据状态编号
        /// </summary>        
        public int Type { get; set; }
        /// <summary>
        /// 数据状态显示信息
        /// </summary>        
        public string TypeDisplay { get; set; }
        /// <summary>
        /// 测点
        /// </summary>        
        public string Point { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>        
        public string Devname { get; set; }
        /// <summary>
        /// 位置
        /// </summary>        
        public string Wz { get; set; }
        /// <summary>
        /// 实时值
        /// </summary>        
        public string Ssz { get; set; }
        /// <summary>
        /// 单位
        /// </summary>        
        public string Unit { get; set; }        
        public DateTime Timer { get; set; }
        /// <summary>
        /// 设备状态编号
        /// </summary>        
        public int State { get; set; }
        /// <summary>
        /// 设备状态显示信息
        /// </summary>     
        public string StateDisplay { get; set; }    
        /// <summary>
        /// 报警展示方式
        /// </summary>
        public string Alarm { get; set; }
        public byte Flag { get; set; }

        /// <summary>
        /// 大数据报警颜色
        /// </summary>
        public string AlarmColor { get; set; }
    }
    public class StateToClient
    {
        /// <summary>
        /// 分站通讯正常
        /// </summary>
        private const int eqpState1 = 3;

        /// <summary>
        /// 分站通讯正常
        /// </summary>
        public int EqpState1
        {
            get { return eqpState1; }
        }
        /// <summary>
        /// 通讯中断
        /// </summary>
        private const int eqpState2 = 0;

        /// <summary>
        /// 通讯中断
        /// </summary>
        public int EqpState2
        {
            get { return eqpState2; }
        }
        /// <summary>
        /// 交流正常
        /// </summary>
        private const int eqpState3 = 3;

        /// <summary>
        /// 交流正常
        /// </summary>
        public int EqpState3
        {
            get { return eqpState3; }
        }

        /// <summary>
        /// 直流正常
        /// </summary>
        private const int eqpState4 = 4;

        /// <summary>
        /// 直流正常
        /// </summary>
        public int EqpState4
        {
            get { return eqpState4; }
        }

        /// <summary>
        /// 通讯误码
        /// </summary>
        private const int eqpState5 = 1;

        /// <summary>
        /// 通讯误码
        /// </summary>
        public int EqpState5
        {
            get { return eqpState5; }
        }

        /// <summary>
        /// 初始化中
        /// </summary>
        private const int eqpState6 = 2;

        /// <summary>
        /// 初始化中
        /// </summary>
        public int EqpState6
        {
            get { return eqpState6; }
        }

        /// <summary>
        /// 上限预警
        /// </summary>
        private const int eqpState7 = 8;

        /// <summary>
        /// 上限预警
        /// </summary>
        public int EqpState7
        {
            get { return eqpState7; }
        }

        /// <summary>
        /// 预警解除
        /// </summary>
        private const int eqpState8 = 9;

        /// <summary>
        /// 预警解除
        /// </summary>
        public int EqpState8
        {
            get { return eqpState8; }
        }

        /// <summary>
        /// 上限报警
        /// </summary>
        private const int eqpState9 = 10;

        /// <summary>
        /// 上限报警
        /// </summary>
        public int EqpState9
        {
            get { return eqpState9; }
        }

        /// <summary>
        /// 报警解除
        /// </summary>
        private const int eqpState10 = 11;

        /// <summary>
        /// 报警解除
        /// </summary>
        public int EqpState10
        {
            get { return eqpState10; }
        }

        /// <summary>
        /// 上限断电
        /// </summary>
        private const int eqpState11 = 12;

        /// <summary>
        /// 上限断电
        /// </summary>
        public int EqpState11
        {
            get { return eqpState11; }
        }

        /// <summary>
        /// 断电恢复
        /// </summary>
        private const int eqpState12 = 13;

        /// <summary>
        /// 断电恢复
        /// </summary>
        public int EqpState12
        {
            get { return eqpState12; }
        }

        /// <summary>
        /// 断线
        /// </summary>
        private const int eqpState13 = 20;

        /// <summary>
        /// 断线
        /// </summary>
        public int EqpState13
        {
            get { return eqpState13; }
        }

        /// <summary>
        /// 正常
        /// </summary>
        private const int eqpState14 = 21;

        /// <summary>
        /// 正常
        /// </summary>
        public int EqpState14
        {
            get { return eqpState14; }
        }

        /// <summary>
        /// 上溢
        /// </summary>
        private const int eqpState15 = 22;

        /// <summary>
        /// 上溢
        /// </summary>
        public int EqpState15
        {
            get { return eqpState15; }
        }

        /// <summary>
        /// 负漂
        /// </summary>
        private const int eqpState16 = 23;

        /// <summary>
        /// 负漂
        /// </summary>
        public int EqpState16
        {
            get { return eqpState16; }
        }

        /// <summary>
        /// 下限预警
        /// </summary>
        private const int eqpState17 = 14;

        /// <summary>
        /// 下限预警
        /// </summary>
        public int EqpState17
        {
            get { return eqpState17; }
        }

        /// <summary>
        /// 下预解除
        /// </summary>
        private const int eqpState18 = 15;

        /// <summary>
        /// 下预解除
        /// </summary>
        public int EqpState18
        {
            get { return eqpState18; }
        }

        /// <summary>
        /// 下限报警
        /// </summary>
        private const int eqpState19 = 16;

        /// <summary>
        /// 下限报警
        /// </summary>
        public int EqpState19
        {
            get { return eqpState19; }
        }

        /// <summary>
        /// 下报解除
        /// </summary>
        private const int eqpState20 = 17;

        /// <summary>
        /// 下报解除
        /// </summary>
        public int EqpState20
        {
            get { return eqpState20; }
        }

        /// <summary>
        /// 下限断电
        /// </summary>
        private const int eqpState21 = 18;

        /// <summary>
        /// 下限断电
        /// </summary>
        public int EqpState21
        {
            get { return eqpState21; }
        }

        /// <summary>
        /// 下断恢复
        /// </summary>
        private const int eqpState22 = 19;

        /// <summary>
        /// 下断恢复
        /// </summary>
        public int EqpState22
        {
            get { return eqpState22; }
        }

        /// <summary>
        /// 标校
        /// </summary>
        private const int eqpState23 = 24;

        /// <summary>
        /// 标校
        /// </summary>
        public int EqpState23
        {
            get { return eqpState23; }
        }

        /// <summary>
        /// 0态
        /// </summary>
        private const int eqpState24 = 25;

        /// <summary>
        /// 0态
        /// </summary>
        public int EqpState24
        {
            get { return eqpState24; }
        }

        /// <summary>
        /// 1态
        /// </summary>
        private const int eqpState25 = 26;

        /// <summary>
        /// 1态
        /// </summary>
        public int EqpState25
        {
            get { return eqpState25; }
        }

        /// <summary>
        /// 2态
        /// </summary>
        private const int eqpState26 = 27;

        /// <summary>
        /// 2态
        /// </summary>
        public int EqpState26
        {
            get { return eqpState26; }
        }

        /// <summary>
        /// 传感器对应分站中断
        /// </summary>
        private const int eqpState27 = 20;

        /// <summary>
        /// 传感器对应分站中断
        /// </summary>
        public int EqpState27
        {
            get { return eqpState27; }
        }

        /// <summary>
        /// 开机
        /// </summary>
        private const int eqpState28 = 28;

        /// <summary>
        /// 开机
        /// </summary>
        public int EqpState28
        {
            get { return eqpState28; }
        }

        /// <summary>
        /// 红外遥控中
        /// </summary>
        private const int eqpState29 = 5;

        /// <summary>
        /// 红外遥控中
        /// </summary>
        public int EqpState29
        {
            get { return eqpState29; }
        }

        /// <summary>
        /// 线性突变
        /// </summary>
        private const int eqpState30 = 29;

        /// <summary>
        /// 线性突变
        /// </summary>
        public int EqpState30
        {
            get { return eqpState30; }
        }

        /// <summary>
        /// 头子断线
        /// </summary>
        private const int eqpState31 = 33;

        /// <summary>
        /// 头子断线
        /// </summary>
        public int EqpState31
        {
            get { return eqpState31; }
        }

        /// <summary>
        /// 类型不对
        /// </summary>
        private const int eqpState32 = 34;

        /// <summary>
        /// 类型不对
        /// </summary>
        public int EqpState32
        {
            get { return eqpState32; }
        }

        /// <summary>
        /// 设备休眠
        /// </summary>
        private const int eqpState33 = 6;

        /// <summary>
        /// 设备休眠
        /// </summary>
        public int EqpState33
        {
            get { return eqpState33; }
        }

        /// <summary>
        /// 设备检修
        /// </summary>
        private const int eqpState34 = 7;

        /// <summary>
        /// 设备检修
        /// </summary>
        public int EqpState34
        {
            get { return eqpState34; }
        }

        /// <summary>
        /// 控制量0态
        /// </summary>
        private const int eqpState43 = 43;

        /// <summary>
        /// 控制量0态
        /// </summary>
        public int EqpState43
        {
            get { return eqpState43; }
        }

        /// <summary>
        /// 控制量1态
        /// </summary>
        private const int eqpState44 = 44;

        /// <summary>
        /// 控制量1态
        /// </summary>
        public int EqpState44
        {
            get { return eqpState44; }
        }

        /// <summary>
        /// 控制量断线
        /// </summary>
        private const int eqpState45 = 45;

        /// <summary>
        /// 控制量断线
        /// </summary>
        public int EqpState45
        {
            get { return eqpState45; }
        }

        /// <summary>
        /// 未知
        /// </summary>
        private const int eqpState46 = 46;

        /// <summary>
        /// 未知
        /// </summary>
        public int EqpState46
        {
            get { return eqpState46; }
        }
    }

}
