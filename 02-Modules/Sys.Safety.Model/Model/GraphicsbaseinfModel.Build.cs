using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("GS_GraphicsBaseInf")]
    public partial class GraphicsbaseinfModel
    {
        /// <summary>
        /// 图形Id
        /// </summary>
        [Key]
        public string GraphId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统标志/编码，关联BFT_SysInf
        /// </summary>
        public string Sysid
        {
            get;
            set;
        }
        /// <summary>
        /// 图形名称
        /// </summary>
        public string GraphName
        {
            get;
            set;
        }
        /// <summary>
        /// 文件类型 0-动态图 1-拓扑图 2-GIS地图 3-SVG组态图
        /// </summary>
        public short Type
        {
            get;
            set;
        }
        /// <summary>
        /// 文件数据
        /// </summary>
        public string GraphData
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Timer
        {
            get;
            set;
        }
        /// <summary>
        /// 备用1，存储SVG组态图对应的Html文件名称（目前html文件固定为：400米管廊.html）
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用2，存储SVG组态图对应的SVG文件名称
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

