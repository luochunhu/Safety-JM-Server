using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_DataRunRecord")]
    public partial class Jc_RModel
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
        /// 值
        /// </summary>
        public string Val
        {
            get;
            set;
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Timer
        {
            get;
            set;
        }
        /// <summary>
        /// remark
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

