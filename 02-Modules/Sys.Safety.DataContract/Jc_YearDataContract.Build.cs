using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_YearInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID编号
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
        public string Fzh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 口号
        /// </summary>
        public string Kh
        {
           get;
           set;
        }
         	    /// <summary>
        /// 地址号
        /// </summary>
        public string Dzh
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
        /// 最大值
        /// </summary>
        public double Zdz
        {
           get;
           set;
        }
         	    /// <summary>
        /// 最小值
        /// </summary>
        public double Zxz
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
        /// 最小值时间
        /// </summary>
        public DateTime Zxzs
        {
           get;
           set;
        }
         	    /// <summary>
        /// 存储时间（如:2016，以年为单位）
        /// </summary>
        public int Timer
        {
           get;
           set;
        }
         	    /// <summary>
        /// 质量等级ID
        /// </summary>
        public string EnvirLevelID
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


