using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("GS_GraphicsRouteInf")]
    public partial class GraphicsrouteinfModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 图形Id
        /// </summary>
        public string GraphId
        {
            get;
            set;
        }
        /// <summary>
        /// 线路开始测点ID编号
        /// </summary>
        public string SPointID
        {
            get;
            set;
        }
        /// <summary>
        /// 线路开始测点号
        /// </summary>
        public string SPoint
        {
            get;
            set;
        }
        /// <summary>
        /// 开始测点号X坐标
        /// </summary>
        public double SPointX
        {
            get;
            set;
        }
        /// <summary>
        /// 开始测点号Y坐标
        /// </summary>
        public double SPointY
        {
            get;
            set;
        }
        /// <summary>
        /// 线路结束测点ID编号
        /// </summary>
        public string EPointID
        {
            get;
            set;
        }
        /// <summary>
        /// 线路结束测点号
        /// </summary>
        public string EPoint
        {
            get;
            set;
        }
        /// <summary>
        /// 结束测点号X坐标
        /// </summary>
        public double EPointX
        {
            get;
            set;
        }
        /// <summary>
        /// 结束测点号Y坐标
        /// </summary>
        public double EPointY
        {
            get;
            set;
        }
        /// <summary>
        /// 线路数据信息(格式为：“X,Y#X,Y”)
        /// </summary>
        public string GraphLines
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
        /// 数据标记
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
    }
}

