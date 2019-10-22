using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_EmergencyLinkagePassivePointAss")]
    public partial class EmergencyLinkagePassivePointAssModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 被控测点关联ID
        /// </summary>
        public string PassivePointAssId
        {
            get;
            set;
        }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统编号
        /// </summary>
        public int Sysid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
    }
}

