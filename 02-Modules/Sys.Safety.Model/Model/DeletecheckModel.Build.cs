using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_DeleteCheck")]
    public partial class DeletecheckModel
    {
        /// <summary>
        /// 基础编码删除检查ID
        /// </summary>
        [Key]
        public string DeleteCheckID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 来源表
        /// </summary>
                public string SourceTable
        {
           get;
           set;
        }
        	    /// <summary>
        /// 来源字段
        /// </summary>
                public string SourceField
        {
           get;
           set;
        }
        	    /// <summary>
        /// 目标表
        /// </summary>
                public string TargetTable
        {
           get;
           set;
        }
        	    /// <summary>
        /// 目标字段
        /// </summary>
                public string TargetField
        {
           get;
           set;
        }
        	    /// <summary>
        /// 说明
        /// </summary>
                public string Description
        {
           get;
           set;
        }
            }
}

