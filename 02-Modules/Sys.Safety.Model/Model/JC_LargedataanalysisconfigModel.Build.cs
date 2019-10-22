using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sys.Safety.Enums.Enums;

namespace Sys.Safety.Model
{
    [Table("KJ_LargedataAnalysisConfig")]
    public partial class JC_LargedataanalysisconfigModel
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
        /// 分析模型名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 分析模板ID
        /// </summary>
        public string TempleteId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用(1：启用（默认）；2：停用)
        /// </summary>
        public EnableState IsEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// 满足条件时输出值
        /// </summary>
        public string TrueDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 不满足条件时输出值
        /// </summary>
        public string FalseDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 分析周期, 1-3600秒
        /// </summary>
        public int AnalysisInterval
        {
            get;
            set;
        }
        /// <summary>
        /// 0-未知，1-不成立, 2-成立
        /// </summary>
        public int AnalysisResult
        {
            get;
            set;
        }
        /// <summary>
        /// 分析时间
        /// </summary>
        public DateTime? AnalysisTime
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

