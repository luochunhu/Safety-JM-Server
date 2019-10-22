using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("kj_devicearearule")]
    public partial class AreaRuleModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public string RuleID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Areaid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Devid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceCount
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public float MaxValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public float MinValue
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
