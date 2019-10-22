using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    /// <summary>
    /// 电源箱（电池箱）信息
    /// </summary>
    public  class BatteryItem
    {
        protected string channel;
        /// <summary>
        /// 表示设备的通道号/MAC地址（=0表示采集设备本身，=1表示此采集设备下的1号设备….）
        /// </summary>
        public string Channel
        {
            get { return channel; }
            set
            {
                channel = value;
            }
        }

        //protected byte batteryState;
        ///// <summary>
        ///// 电池控制状态（0不放电，1放电）
        ///// </summary>
        //public byte BatteryState
        //{
        //    get { return batteryState; }
        //    set
        //    {
        //        batteryState = value;
        //    }
        //}
        protected string batteryAddress;
        /// <summary>
        /// 地址号
        /// </summary>
        public string BatteryAddress
        {
            get { return batteryAddress; }
            set
            {
                batteryAddress = value;
            }
        }
        /// <summary>
        /// 总电压
        /// </summary>
        public float TotalVoltage { get; set; }
        /// <summary>
        /// 温度T1
        /// </summary>
        public float DeviceTemperature1 { get; set; }
        /// <summary>
        /// 温度T2
        /// </summary>
        public float DeviceTemperature2 { get; set; }
        //protected bool batteryTooHot;
        ///// <summary>
        ///// 电源箱过热（true 过热；false正常）
        ///// </summary>
        //public bool BatteryTooHot
        //{
        //    get { return batteryTooHot; }
        //    set
        //    {
        //        batteryTooHot = value;
        //    }
        //}
        //protected bool batteryUndervoltage;
        ///// <summary>
        /////电源箱欠压（true 欠压；false正常）
        ///// </summary>
        //public bool BatteryUndervoltage
        //{
        //    get { return batteryUndervoltage; }
        //    set
        //    {
        //        batteryUndervoltage = value;
        //    }
        //}
        //protected int _BatteryPackStateCd;
        ///// <summary>
        ///// 电源箱充放电状态充电中=1表示条件为真
        ///// </summary>
        //public int BatteryPackStateCd
        //{
        //    get { return _BatteryPackStateCd; }
        //    set
        //    {
        //        _BatteryPackStateCd = value;
        //    }
        //}

        //protected int _BatteryPackStateJh;
        ///// <summary>
        ///// 电源箱充放电状态0 均衡中=1表示条件为真
        ///// </summary>
        //public int BatteryPackStateJh
        //{
        //    get { return _BatteryPackStateJh; }
        //    set
        //    {
        //        _BatteryPackStateJh = value;
        //    }
        //}

        //protected int _BatteryPackStateFd;
        ///// <summary>
        ///// 电源箱充放电状态 放电中 =1表示条件为真
        ///// </summary>
        //public int BatteryPackStateFd
        //{
        //    get { return _BatteryPackStateFd; }
        //    set
        //    {
        //        _BatteryPackStateFd = value;
        //    }
        //}
        //protected bool _BatteryOverCharge;
        ///// <summary>
        ///// 电源箱过充（true 过充；false正常）
        ///// </summary>
        //public bool BatteryOverCharge
        //{
        //    get { return _BatteryOverCharge; }
        //    set
        //    {
        //        _BatteryOverCharge = value;
        //    }
        //}

        protected int _BatteryACDC;
        /// <summary>
        /// 电源箱交直流状态 40 交流（40） 直流（41 和01）
        /// </summary>
        public int BatteryACDC
        {
            get { return _BatteryACDC; }
            set
            {
                _BatteryACDC = value;
            }
        }
        
          //batteryItem.BatteryPackStateCd = batteryDataaItem.BatteryPackStateCd;
          //                  batteryItem.BatteryPackStateJh = batteryDataaItem.BatteryPackStateJh;
          //                  batteryItem.BatteryPackStateFd = batteryDataaItem.BatteryPackStateFd;

        protected byte _PowerPackVOL;
        /// <summary>
        /// 电源箱电量 ????
        /// </summary>
        public byte PowerPackVOL
        {
            get { return _PowerPackVOL; }
            set
            {
                _PowerPackVOL = value;
            }
        }

        //protected float _PowerPackMA;
        ///// <summary>
        ///// 电源箱负载电流
        ///// </summary>
        //public float PowerPackMA
        //{
        //    get { return _PowerPackMA; }
        //    set
        //    {
        //        _PowerPackMA = value;
        //    }
        //}

        protected float[] _BatteryVOL;
        /// <summary>
        /// 电源箱电池电压
        /// </summary>
        public float[] BatteryVOL
        {
            get { return _BatteryVOL; }
            set { _BatteryVOL = value; }
        }


        protected string deviceOnlyCode;
        /// <summary>
        /// 电源箱唯一编码
        /// </summary>
        public string DeviceOnlyCode
        {
            get { return deviceOnlyCode; }
            set { deviceOnlyCode = value; }
        }
    }
}
