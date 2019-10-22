using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("RA_CallPointList")]
    public partial class B_CallpointlistModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string BCallId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CallId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AgentPointId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CalledPointId
        {
            get;
            set;
        }
    }
}

