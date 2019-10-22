using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_PointhisInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
        public string PointID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string Areaid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int Sysid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string DataRightID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
           get;
           set;
        }
        /// <summary>
        /// 口号
        ///分站：主队巡检 1-巡 0-不巡

        /// </summary>
        public string Fzh
        {
           get;
           set;
        }
        /// <summary>
        /// 地址号
        ///传感器：
        ///0：单参数 非0：参数地址

        /// </summary>
        public string Kh
        {
           get;
           set;
        }
        /// <summary>
        /// 地址号
        ///传感器：
        ///0：单参数 非0：参数地址

        /// </summary>
        public string Dzh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 设备类型编号
        /// </summary>
        public int Devid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 安装位置编号
        /// </summary>
        public int Wzid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 处理措施编号
        /// </summary>
        public int Csid
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
        /// 分站: mac地址/Ip地址/抽放绑定开停
        ///模拟量：上下限断电交叉控制测点号/断线交叉控制测点号/上溢负漂交叉控制测点号
        ///开关量：0态交叉控制测点号/1态交叉控制测点号/2态交叉控制测点号
        ///导出量：测点1/测点2/测点3
        ///识别器：指定人员编号（当为限制进入时启用）
        ///对应多个测点之间用‘|’分隔如:001C01|005C06

        /// </summary>
        public string Jckz1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string Jckz2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string Jckz3
        {
           get;
           set;
        }
        /// <summary>
        /// 模拟量：上限预警值/上限报警值/上限断电值/上限恢复值/下限预警值/下限报警值/下限断电值/下限恢复值
        ///累计量：偏差值
        ///导出量：参数1/参数2

        /// </summary>
        public double Z1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z5
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z6
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z7
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public double Z8
        {
           get;
           set;
        }
        /// <summary>
        /// 分站：大气压（101）/抽放正负压（0负1正）/串口端口号
        ///模拟量：上限报警控制口/上限断电控制口/下限报警控制口/下限断电控制口/上溢控制口/负漂控制口/断线控制口/备用控制口
        ///开关量:0态控制口/1态控制口/2态控制口/逻辑报警类型（1与 2或）/逻辑报警关联口号
        ///控制量：控制量所关联的馈电传感器的分站号/控制量所关联的馈电传感器的通道号/是否智能开停
        ///累计量：关联的分站号/关联的口号
        ///区域：额定人数
        ///识别器：报警类型/报警时间/离开时间/额定人数

        /// </summary>
        public int K1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K5
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K6
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K7
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public int K8
        {
           get;
           set;
        }
        /// <summary>
        /// 分站：  运行记录标志
        ///模拟量：运行记录标志
        ///开关量：运行记录标志

        /// </summary>
        public string Bz1
        {
           get;
           set;
        }
        /// <summary>
        /// 分站：响铃报警标
        ///模拟量：响铃报警标志
        ///开关量：响铃报警标志

        /// </summary>
        public string Bz2
        {
           get;
           set;
        }
        /// <summary>
        /// 分站：风电闭锁标志 1；逻辑控制 2
        ///模拟量：突出预测标志 

        /// </summary>
        public string Bz3
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
        public string Bz4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 联网上传标志
        /// </summary>
        public string Bz5
        {
           get;
           set;
        }
         	    /// <summary>
        /// 上传标志0-未传1-已传
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
            }
}


