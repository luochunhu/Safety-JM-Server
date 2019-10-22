using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_AlarmHandle")]
    public partial class JC_AlarmHandleModel
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
        /// 分析模型ID
        /// </summary>
        public string AnalysisModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 分析结果
        /// </summary>
        public string AnalysisResult
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 异常原因
        /// </summary>
        public string ExceptionReason
        {
            get;
            set;
        }
        /// <summary>
        /// 报警方式
        /// </summary>
        public string AlarmType { get; set; }
        /// <summary>
        /// 报警颜色
        /// </summary>
        public string AlarmColor { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandlingTime
        {
            get;
            set;
        }
        /// <summary>
        /// 处理措施
        /// </summary>
        public string Handling
        {
            get;
            set;
        }
        /// <summary>
        /// 处理人
        /// </summary>
        public string HandlingPerson
        {
            get;
            set;
        }
    }

    public partial class JC_AlarmHandleNoEndModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        public string AnalysisModelId { get; set; }
        public DateTime StartTime { get; set; }
        public string AnalysisResult { get; set; }
        public string AlarmType { get; set; }
        public string AlarmColor { get; set; }
    }
}

