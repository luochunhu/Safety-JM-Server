using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_AnalysistemplateAlarmlevel")]
    public partial class Jc_AnalysistemplatealarmlevelModel
    {
        /// <summary>
        /// 唯一编码
        /// </summary>
        [Key]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 分析模板Id
        /// </summary>
        public string AnalysisModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 传感器报警等级
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
    }
}

