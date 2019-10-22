using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sys.Safety.Enums.Enums;

namespace Sys.Safety.Model
{
    [Table("KJ_AnalysisTemplate")]
    public partial class JC_AnalysistemplateModel
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name      
        {
            get;
            set;
        }
        /// <summary>
        /// 删除标识（1：未删除（默认）；2：已删除）
        /// </summary>
        public DeleteState IsDeleted
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedTime
        {
            get;
            set;
        }
    }
}

