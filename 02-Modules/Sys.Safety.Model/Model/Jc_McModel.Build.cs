using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_DataDetail")]
    public partial class Jc_McModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
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
        /// 测点编号
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
        /// 采集时间
        /// </summary>
        public DateTime Timer
        {
            get;
            set;
        }
        /// <summary>
        /// 实时值
        /// </summary>
        public double Ssz
        {
            get;
            set;
        }
        /// <summary>
        /// 电压等级
        /// </summary>
        public double Voltage
        {
            get;
            set;
        }
        /// <summary>
        /// 备用1
        /// </summary>
        public long Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用2
        /// </summary>
        public long Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 备用3
        /// </summary>
        public long Bz3
        {
            get;
            set;
        }
        /// <summary>
        /// 标记
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
        /// <summary>
        /// 备用4，补录数据（by 20170414）
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
        /// 备用6
        /// </summary>
        public string Bz6
        {
            get;
            set;
        }
    }
}

