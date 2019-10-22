using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class JC_AnalysisTemplateInfo 
    {
        /// <summary>
        /// 表达式Id
        /// </summary>
        public string ExpressionId
        {
            get;
            set;
        }
        /// <summary>
        ///表达式内容（ID）
        /// </summary>
        public string Expresstion
        {
            get;
            set;
        }
        /// <summary>
        /// 表达式内容text
        /// </summary>
        public string ExpresstionText
        {
            get;
            set;
        }
        
        /// <summary>
        /// 持续时间
        /// </summary>
        public int ContinueTime
        {
            get;
            set;
        }
        
    }
}
