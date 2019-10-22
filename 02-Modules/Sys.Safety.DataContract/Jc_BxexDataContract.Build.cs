using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_BxexInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// ID编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        public string PointID
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
        /// 标校人
        /// </summary>
        public string Name
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
        /// 持续时间（秒）
        /// </summary>
        public int Cx
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
        public DateTime Zdztime
        {
            get;
            set;
        }
        /// <summary>
        /// 最小值时间
        /// </summary>
        public DateTime Zxztime
        {
            get;
            set;
        }
        /// <summary>
        /// 状态 1-标校中 2-已标校完成
        /// </summary>
        public int Bxzt
        {
            get;
            set;
        }
        public string Cs
        {
            get;
            set;
        }
        public string Bz1
        {
            get;
            set;
        }
        public string Bz2
        {
            get;
            set;
        }
        public string Bz3
        {
            get;
            set;
        }
        public string Bz4
        {
            get;
            set;
        }
        public string Bz5
        {
            get;
            set;
        }
    }
}


