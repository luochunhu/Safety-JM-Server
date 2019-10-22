using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_Ll_YInfo : Basic.Framework.Web.BasicInfo
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
        /// 测点唯一id
        /// </summary>
        public string PointID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 负压平均值
        /// </summary>
        public decimal FY
        {
           get;
           set;
        }
         	    /// <summary>
        /// 温度平均值
        /// </summary>
        public decimal WD
        {
           get;
           set;
        }
         	    /// <summary>
        /// 瓦斯平均值
        /// </summary>
        public decimal WS
        {
           get;
           set;
        }
         	    /// <summary>
        /// 一氧化碳平均值
        /// </summary>
        public decimal CO
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标况混合流量平均值
        /// </summary>
        public decimal BH
        {
           get;
           set;
        }
         	    /// <summary>
        /// 工况混合流量平均值
        /// </summary>
        public decimal GH
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标况纯量平均值
        /// </summary>
        public decimal BC
        {
           get;
           set;
        }
         	    /// <summary>
        /// 工况纯量平均值
        /// </summary>
        public decimal GC
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标况混合累计值
        /// </summary>
        public decimal BHL
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标况纯量累计值
        /// </summary>
        public decimal BCL
        {
           get;
           set;
        }
         	    /// <summary>
        /// 工况混合累计值
        /// </summary>
        public decimal GHL
        {
           get;
           set;
        }
         	    /// <summary>
        /// 工况纯量累计值
        /// </summary>
        public decimal GCL
        {
           get;
           set;
        }
         	    /// <summary>
        /// 存储的时间
        /// </summary>
        public DateTime Timer
        {
           get;
           set;
        }
         	    /// <summary>
        /// 该小时绑定开停的累计运行时间
        /// </summary>
        public string Yxsc
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


