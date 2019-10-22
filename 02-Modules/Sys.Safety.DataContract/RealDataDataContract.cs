using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{

    public partial class RealDataDataInfo
    {

        /// <summary>
        /// 测点ID编号[历史表关联字段]
        /// </summary>
        public string PointID
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        public string Areaid
        {
            get;
            set;
        }
        /// <summary>
        /// 系统标志/编码，关联CBF_SysInf
        /// </summary>
        public int Sysid
        {
            get;
            set;
        }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point
        {
            get;
            set;
        }
        /// <summary>
        /// 位置 
        /// </summary>
        public string Wz
        {
            get;
            set;
        }
        /// <summary>
        /// 实时值
        /// </summary>
        public string Ssz
        {
            get;
            set;
        }
        /// <summary>
        /// 数据状态
        /// </summary>
        public short DataState
        {
            get;
            set;
        }
        /// <summary>
        /// 状态值
        /// </summary>
        public short State
        {
            get;
            set;
        }
        /// <summary>
        /// 报警状态 大于零为报警
        /// </summary>
        public short Alarm
        {
            get;
            set;
        }
        /// <summary>
        /// 电压等级
        /// </summary>
        public float Voltage
        {
            get;
            set;
        }
        /// <summary>
        /// 状态变动时间
        /// </summary>
        public DateTime Zts
        {
            get;
            set;
        }
        /// <summary>
        /// 分站：设备休眠标志
        ///模拟量：密采记录标志
        ///开关量：设备休眠标志
        ///控制量：标记

        /// </summary>
        public int Bz4
        {
            get;
            set;
        }
        /// <summary>
        /// 获取控制量馈电状态
        /// </summary>
        public int NCtrlSate
        {
            get;
            set;
        }
        /// <summary>
        /// 最后采集时间
        /// </summary>
        public DateTime DttStateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DevModel
        {
            get;
            set;
        }
        /// <summary>
        /// 设备种类
        /// </summary>
        public string DevClass
        {
            get;
            set;
        }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 设备性质
        /// </summary>
        public string DevProperty
        {
            get;
            set;
        }
        /// <summary>
        /// 传感器分级报警等级
        /// </summary>
        public int GradingAlarmLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 分站电源状态，=1表示分站电源箱和分站通讯故障。
        /// </summary>
        public byte StationDyType { get; set; }
    }
}


