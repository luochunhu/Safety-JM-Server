using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("GS_GraphicsPointsInf")]
    public partial class GraphicspointsinfModel
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
        /// 系统编号
        /// </summary>
        public int SysId
        {
            get;
            set;
        }

        /// <summary>
        /// 测点ID编号
        /// </summary>
        public string PointID
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定测点号
        /// </summary>
        public string Point
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定的图元名称
        /// </summary>
        public string GraphBindName
        {
            get;
            set;
        }
        /// <summary>
        /// 图元类型 0-悬浮图元 1-拓扑图元 2-SVG图元 3-静态图元 4-GIS图元
        /// </summary>
        public short GraphBindType
        {
            get;
            set;
        }
        /// <summary>
        /// 缩放级别以上进行显示（指定在某个缩放级别以上进行显示）
        /// </summary>
        public string DisZoomlevel
        {
            get;
            set;
        }
        /// <summary>
        /// X坐标   经度
        /// </summary>
        public string XCoordinate
        {
            get;
            set;
        }
        /// <summary>
        /// Y坐标   纬度
        /// </summary>
        public string YCoordinate
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
        /// 备用2,图元的宽度
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 备用3,图元的高度
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
        /// <summary>
        /// 备用4,双击要跳转的页面（存页面的名称，空表示不跳转）
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

