using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class JC_AnalysisTemplateConfigInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 主键Id
        /// </summary>
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


