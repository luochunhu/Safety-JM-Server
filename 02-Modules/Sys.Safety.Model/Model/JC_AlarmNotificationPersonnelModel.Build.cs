using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_AlarmNotificationPersonnel")]
    public partial class JC_AlarmNotificationPersonnelModel
    {
        	    /// <summary>
        /// 主键Id
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警配置Id
        /// </summary>
                public string AlarmConfigId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 报警通知人员Id
        /// </summary>
                public string PersonId
        {
           get;
           set;
        }
            }
}

