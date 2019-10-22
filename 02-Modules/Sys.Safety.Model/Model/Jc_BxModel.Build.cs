using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_Effect")]
    public partial class Jc_BxModel
    {
        /// <summary>
        /// ID编号
        /// </summary>
        [Key]
        public string ID
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
        /// 日期
        /// </summary>
        public DateTime Timer
        {
            get;
            set;
        }
        /// <summary>
        /// 备用
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
    }
}

