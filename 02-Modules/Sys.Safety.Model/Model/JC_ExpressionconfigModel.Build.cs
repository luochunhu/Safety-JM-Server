using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_ExpressionConfig")]
    public partial class JC_ExpressionconfigModel
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
        /// 表达式Id
        /// </summary>
        public string ExpressionId
        {
            get;
            set;
        }
        /// <summary>
        /// 参数Id
        /// </summary>
        public string ParameterId
        {
            get;
            set;
        }
        /// <summary>
        /// 因子Id
        /// </summary>
        public string FactorId
        {
            get;
            set;
        }
    }
}

