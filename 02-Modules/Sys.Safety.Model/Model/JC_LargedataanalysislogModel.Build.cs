using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sys.Safety.Enums.Enums;

namespace Sys.Safety.Model
{
    [Table("KJ_LargedataAnalysisLog")]
    public partial class JC_LargedataanalysislogModel
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
        /// 分析模型Id
        /// </summary>
        public string AnalysisModelId
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
        /// 分析结果 '0-未知，1-不成立, 2-成立
        /// </summary>
        public int AnalysisResult
        {
            get;
            set;
        }
        /// <summary>
        /// 分析结果状态描述
        /// </summary>
        public string StatusDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 分析时间
        /// </summary>
        public DateTime AnalysisTime
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
        /// 删除标识（1：未删除（默认）；2：已删除）
        /// </summary>
        public DeleteState IsDeleted
        {
            get;
            set;
        }
    }
}

