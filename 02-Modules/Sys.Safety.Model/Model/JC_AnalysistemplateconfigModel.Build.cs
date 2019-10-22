using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_AnalysisTemplateConfig")]
    public partial class JC_AnalysistemplateconfigModel
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
        /// 模板Id
        /// </summary>
                public string TempleteId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 表达式Id
        /// </summary>
                public string ExpressionId
        {
           get;
           set;
        }
            }
}

