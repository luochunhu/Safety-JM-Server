using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Model
{
    [Table("BFT_Class")]
    public class ClassCommonConfigModel
    {
        /// <summary>
        /// 班次ID
        /// </summary>
        [Key]
        public string ClassId
        {
            get;
            set;
        }
        /// <summary>
        /// 班次编码
        /// </summary>
        public string StrCode
        {
            get;
            set;
        }
        /// <summary>
        /// 班次名称
        /// </summary>
        public string StrName
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string DatStart
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string DatEnd
        {
            get;
            set;
        }
    }
}
