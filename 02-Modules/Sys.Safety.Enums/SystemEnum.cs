using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public enum SystemEnum
    {
        /// <summary>
        /// 燃气系统
        /// </summary>
        [Description("燃气系统")]
        Gas = 1,
        /// <summary>
        /// 下水道系统
        /// </summary>
        [Description("下水道系统")]
        Sewer = 2,
        /// <summary>
        /// 井盖系统
        /// </summary>
        [Description("井盖系统")]
        ManholeCover = 3,
        /// <summary>
        /// 公安视频箱系统
        /// </summary>
        [Description("公安视频箱系统")]
        PoliceVideo = 4,
        /// <summary>
        /// 水质公共卫生系统
        /// </summary>
        [Description("水质公共卫生系统")]
        WaterQuality = 5,
        /// <summary>
        /// 路灯监控系统
        /// </summary>
        [Description("路灯监控系统")]
        RoadLamp = 6,
        /// <summary>
        /// 电力管网系统
        /// </summary>
        [Description("电力管网系统")]
        ElectricPower = 7,
        /// <summary>
        /// 电梯系统
        /// </summary>
        [Description("电梯系统")]
        Elevator = 8,
        /// <summary>
        /// 安全监控系统
        /// </summary>
        [Description("安全监控系统")]
        Security = 9,
        /// <summary>
        /// 瓦斯抽采系统
        /// </summary>
        [Description("瓦斯抽采系统")]
        GasExtraction = 10,
        /// <summary>
        /// 人员定位系统
        /// </summary>
        [Description("人员定位系统")]
        Personnel = 11,
        /// <summary>
        /// 语音扩播系统
        /// </summary>
        [Description("语音扩播系统")]
        Broadcast = 12,
        /// <summary>
        /// 降尘系统
        /// </summary>
        [Description("降尘系统")]
        Dust = 13,
        /// <summary>
        /// 空气质量监测
        /// </summary>
        [Description("空气质量监测")]
        AirQuality = 22,
        /// <summary>
        /// 区域环境噪声监测
        /// </summary>
        [Description("区域环境噪声监测")]
        Noise = 23,
        /// <summary>
        /// 大气环境污染源
        /// </summary>
        [Description("大气环境污染源")]
        Atmospheric = 31,
        /// <summary>
        /// 地表水体环境污染源
        /// </summary>
        [Description("地表水体环境污染源")]
        SurfaceWater = 32,
        /// <summary>
        /// 地下水体环境污染源
        /// </summary>
        [Description("地下水体环境污染源")]
        UndergroundWater = 33,
        /// <summary>
        /// 海洋环境污染源
        /// </summary>
        [Description("海洋环境污染源")]
        MarineEnvironment = 34,
        /// <summary>
        /// 土壤环境污染源
        /// </summary>
        [Description("土壤环境污染源")]
        SoilEnvironment = 35,
        /// <summary>
        /// 声环境污染源
        /// </summary>
        [Description("声环境污染源")]
        SoundEnvironment = 36,
        /// <summary>
        /// 振动环境污染源
        /// </summary>
        [Description("振动环境污染源")]
        VibrationEnvironment = 37,
        /// <summary>
        /// 放射性环境污染源
        /// </summary>
        [Description("放射性环境污染源")]
        RadioactiveEnvironment = 38,
        /// <summary>
        /// 电磁环境污染源
        /// </summary>
        [Description("电磁环境污染源")]
        ElectromagneticEnvironment = 41,
        /// <summary>
        /// 结构监测系统
        /// </summary>
        [Description("结构监测系统")]
        Structure = 71,
        /// <summary>
        /// 环境监测系统
        /// </summary>
        [Description("环境监测系统")]
        Environmental = 72,
        /// <summary>
        /// 附属设施监测系统
        /// </summary>
        [Description("附属设施监测系统")]
        AffiliatedFacilities = 73,
        /// <summary>
        /// 视频监控系统
        /// </summary>
        [Description("视频监控系统")]
        Video = 74,
        /// <summary>
        /// 入侵报警系统
        /// </summary>
        [Description("入侵报警系统")]
        IntrusionAlarm = 75,
        /// <summary>
        /// 通信系统
        /// </summary>
        [Description("通信系统")]
        Communication = 76,
        /// <summary>
        /// 巡更系统
        /// </summary>
        [Description("巡更系统")]
        Patrolling = 77,
        /// <summary>
        /// 火灾报警系统
        /// </summary>
        [Description("火灾报警系统")]
        FireAlarm = 78,
        /// <summary>
        /// 可燃气体报警系统
        /// </summary>
        [Description("可燃气体报警系统")]
        GasAlarm = 79,
        /// <summary>
        /// 综合管廊监管平台
        /// </summary>
        [Description("综合管廊监管平台")]
        ComprehensiveUtility = 80,
        /// <summary>
        /// 水务运营管理平台
        /// </summary>
        [Description("水务运营管理平台")]
        WaterOperationManagement = 81,
    }
}
