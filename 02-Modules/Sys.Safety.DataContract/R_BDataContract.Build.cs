using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class R_BInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 测点ID编号[历史表关联字段]
        /// </summary>
        public string PointID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 分站号
        /// </summary>
        public short Fzh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 口号
        /// </summary>
        public short Kh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 地址号
        /// </summary>
        public short Dzh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 设备类型索引ID
        /// </summary>
        public string Devid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 安装位置索引ID
        /// </summary>
        public string Wzid
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
        /// 数据状态
        /// </summary>
        public short Type
        {
           get;
           set;
        }
         	    /// <summary>
        /// 设备状态
        /// </summary>
        public short State
        {
           get;
           set;
        }
         	    /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Stime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Etime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 显示值（报警初始值）
        /// </summary>
        public string Ssz
        {
           get;
           set;
        }
         	    /// <summary>
        /// 最大值
        /// </summary>
        public double Zdz
        {
           get;
           set;
        }
         	    /// <summary>
        /// 平均值
        /// </summary>
        public double Pjz
        {
           get;
           set;
        }
         	    /// <summary>
        /// 最大值时间
        /// </summary>
        public DateTime Zdzs
        {
           get;
           set;
        }
         	    /// <summary>
        /// 处理措施
        /// </summary>
        public string Cs
        {
           get;
           set;
        }
        /// <summary>
        /// 模拟量、开关量：控制测点号
        ///控制量：馈电点号
        /// </summary>
        public string Kzk
        {
           get;
           set;
        }
         	    /// <summary>
        /// 馈电异常记录ID EX:201509091200000909,201509091200000910
        /// </summary>
        public string Kdid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 否为报警标志
        /// </summary>
        public short Isalarm
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用1
        /// </summary>
        public string Bz1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用2
        /// </summary>
        public string Bz2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用3
        /// </summary>
        public string Bz3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用4
        /// </summary>
        public string Bz4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用5
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


